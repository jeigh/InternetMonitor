Imports System.IO
Imports System.Net
Imports InternetMonitor.Core.Services
Imports Microsoft.Practices.Unity

Namespace Services

    Public Class SpeedHandler
    Implements ISpeedHandler

        Public Sub New(uc As Microsoft.Practices.Unity.IUnityContainer)
            _resolver = uc
            _id = Guid.NewGuid
            Debug.WriteLine("constructed new SpeedHandler with id" & _id.ToString)
        End Sub

        Private _id As Guid
        Private ReadOnly _resolver As IUnityContainer

        Public ReadOnly Property _config As IConfiguration
            Get
                Return _resolver.Resolve(Of IConfiguration)()
            End Get
        End Property

        Public ReadOnly Property _rebootLogger As IGatewayRebootLoggingService
            Get
                Return _resolver.Resolve(Of IGatewayRebootLoggingService)()
            End Get
        End Property


        Public Sub HandleSpeed(ci As Core.TransferObjects.ConnectionInfo) Implements ISpeedHandler.HandleSpeed
            Console.WriteLine(String.Format("UpSpeed:{0},DownSpeed:{1},IpAddress:{2}", ci.UpSpeed, ci.DownSpeed, ci.IpAddress))

            If ci.DownSpeed <> 0 AndAlso ci.DownSpeed < _config.LowestSpeedThreshold Then
                RebootGateway()
                'todo: only if logging enabled:   _rebootLogger.LogGatewayReboot(ci)
            End If


        End Sub

        Private Sub RebootGateway()
            Console.WriteLine("REBOOT!")
            Dim request As HttpWebRequest = HttpWebRequest.Create("http://192.168.0.1/goform/EventForm")

            request.Method = "POST"

            request.Headers.Add("Authorization", _config.AuthorizationHeaderContents)
            Dim dataStream As Stream = request.GetRequestStream()
            Dim byteArray As Byte() = System.Text.Encoding.ASCII.GetBytes("next_page=%2FhtmlV%2Freset.asp%3Frestart%3DTRUE&RebootEvent=noreset")

            dataStream.Write(byteArray, 0, byteArray.Length)
            dataStream.Close()

            Try
                Dim response As WebResponse = request.GetResponse()

                dataStream = response.GetResponseStream()

                Dim reader As New StreamReader(dataStream)

                Dim responseFromServer As String = reader.ReadToEnd()

                Console.WriteLine(responseFromServer)

                reader.Close()

                dataStream.Close()

                response.Close()

                Debug.WriteLine("sent")
            Catch ex As System.Net.WebException
                Console.WriteLine("*")
            Catch ex As Exception
                Console.WriteLine(ex.ToString)


            End Try

        End Sub

    End Class

End Namespace