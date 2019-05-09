Imports Microsoft.Practices.Unity
Imports InternetMonitor.Core.Services

Namespace Services

    Public Class SpeedLogger
        Implements ISpeedLogger

        Private Sub New()
        End Sub

        Public Sub New(uc As Microsoft.Practices.Unity.IUnityContainer)
            _resolver = uc
            _id = Guid.NewGuid
            Debug.WriteLine("constructed new SpeedLoggger with id " & _id.ToString)
        End Sub

        Private _id As Guid
        Private ReadOnly _resolver As IUnityContainer

        Public Sub LogSpeed(ci As Core.TransferObjects.ConnectionInfo) Implements ISpeedLogger.LogSpeed
            Dim logSpeed As Integer = 0 'todo: retrieve from config
            If logSpeed <> 0 Then
                Using dbContext As New InternetMonitor.Entities.InternetMonitorContext
                    Dim newWanSpeed As New Entities.WanSpeed()

                    newWanSpeed.SpeedTime = Now()
                    newWanSpeed.SpeedDown = ci.DownSpeed
                    newWanSpeed.SpeedUp = ci.UpSpeed
                    newWanSpeed.WanSpeedId = Guid.NewGuid
                    newWanSpeed.Ipv4Address = ci.IpAddress

                    dbContext.WanSpeeds.Add(newWanSpeed)
                    dbContext.SaveChanges()
                End Using

            End If
        End Sub
    End Class

End Namespace