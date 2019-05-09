Imports InternetMonitor.Core.Services
Imports Microsoft.Practices.Unity

Namespace Services

    Public Class GatewayRebootLoggingService
        Implements IGatewayRebootLoggingService

        Private Sub New()
        End Sub

        Public Sub New(uc As Microsoft.Practices.Unity.IUnityContainer)
            _resolver = uc
            _id = Guid.NewGuid
            Debug.WriteLine("constructed new GatewayRebootLoggingService with id" & _id.ToString)
        End Sub

        Private _id As Guid
        Private ReadOnly _resolver As IUnityContainer

        Public Sub LogGatewayReboot(ci As Core.TransferObjects.ConnectionInfo) Implements IGatewayRebootLoggingService.LogGatewayReboot
            Using cxt As New Entities.InternetMonitorContext
                cxt.GatewayRestarts.Add(New Entities.GatewayRestart() With {
                    .SpeedDown = ci.DownSpeed,
                    .SpeedUp = ci.UpSpeed,
                    .CreatedOn = Now(),
                    .GatewayRestartId = Guid.NewGuid(),
                    .Ipv4Address = ci.IpAddress})
                cxt.SaveChanges()
            End Using
        End Sub

    End Class

End Namespace