Imports System.Net
Imports System.IO
Imports Microsoft.Practices.Unity
Imports InternetMonitor.Core.Services

Namespace Services

    Public Class SpeedChecker

        Implements ISpeedChecker


        Private Sub New()
        End Sub

        Public Sub New(uc As Microsoft.Practices.Unity.IUnityContainer)
            _resolver = uc
            _id = Guid.NewGuid
            Debug.WriteLine("constructed new SpeedChecker with id" & _id.ToString)
        End Sub

        Private _id As Guid
        Private ReadOnly _resolver As IUnityContainer

        Public ReadOnly Property _speedLogger As ISpeedLogger
            Get
                Return _resolver.Resolve(Of ISpeedLogger)()
            End Get
        End Property
        Public ReadOnly Property _config As IConfiguration
            Get
                Return _resolver.Resolve(Of IConfiguration)()
            End Get
        End Property

        Public ReadOnly Property _speedHandler As ISpeedHandler
            Get
                Return _resolver.Resolve(Of ISpeedHandler)()
            End Get
        End Property


        Public Sub CheckCurrentSpeed() Implements ISpeedChecker.CheckCurrentSpeed
            Try

                Dim req As HttpWebRequest = System.Net.WebRequest.Create(_config.SpeedTestUrl)
                Dim response As HttpWebResponse = CType(req, HttpWebRequest).GetResponse()
                Dim dataStream As Stream = response.GetResponseStream()
                Dim reader As New StreamReader(dataStream)
                Dim responseFromServer As String = reader.ReadToEnd()

                Dim blah As String() = responseFromServer.Split(";")

                Dim upSpeed As Decimal
                Dim downSpeed As Decimal
                Dim ipv4Address As String = String.Empty

                For Each row As String In blah
                    If row.Contains("var Xcvr_SpdDwnStrm") Then
                        Dim words As String() = row.Split("""")

                        If Not Decimal.TryParse(words(1), downSpeed) Then
                            Throw New Exception("unable to get downSpeed")
                        End If

                    End If

                    If row.Contains("var Xcvr_SpdUpStrm") Then
                        Dim words As String() = row.Split("""")

                        If Not Decimal.TryParse(words(1), upSpeed) Then
                            Throw New Exception("unable to get upSpeed")
                        End If
                    End If

                    If row.Contains("var Xcvr_State") Then
                        Dim words As String() = row.Split("=")

                        If Not words(1).Contains("UP") Then
                            upSpeed = 0
                            downSpeed = 0
                        End If
                    End If

                    If row.Contains("var WanIPRoutingState_WanIPAddress") Then
                        Dim words As String() = row.Split("=")

                        If words.Length = 2 Then
                            ipv4Address = words(1)
                        End If


                    End If
                Next




                Dim theConnectionInfo As New Core.TransferObjects.ConnectionInfo
                theConnectionInfo.DownSpeed = downSpeed
                theConnectionInfo.UpSpeed = upSpeed
                theConnectionInfo.IpAddress = ipv4Address.Replace(" ", "").Replace("""", "")





                _speedLogger.LogSpeed(theConnectionInfo)
                _speedHandler.HandleSpeed(theConnectionInfo)
            Catch ex As Entity.Core.EntityException
                Console.WriteLine(ex.ToString)
            Catch ex As System.Net.WebException
                Console.WriteLine(ex.ToString)
            Catch ex As Exception
                Console.WriteLine(ex.ToString)
            End Try

        End Sub
    End Class

End Namespace
