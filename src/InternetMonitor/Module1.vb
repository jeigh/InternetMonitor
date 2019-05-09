Imports InternetMonitor.Implementation.Services
Imports Microsoft.Practices.Unity

Module Module1

    Sub Main()
        Dim container As New UnityContainer()

        container.RegisterInstance(Of Core.Services.IConfiguration)(New Implementation.Services.Configuration(container), New Microsoft.Practices.Unity.ContainerControlledLifetimeManager())
        container.RegisterInstance(Of Core.Services.ISpeedLogger)(New Implementation.Services.SpeedLogger(container), New Microsoft.Practices.Unity.ContainerControlledLifetimeManager())
        container.RegisterInstance(Of Core.Services.ISpeedChecker)(New Implementation.Services.SpeedChecker(container), New Microsoft.Practices.Unity.ContainerControlledLifetimeManager())
        container.RegisterInstance(Of Core.Services.ISpeedHandler)(New Implementation.Services.SpeedHandler(container), New Microsoft.Practices.Unity.ContainerControlledLifetimeManager())
        container.RegisterInstance(Of Core.Services.IGatewayRebootLoggingService)(New Implementation.Services.GatewayRebootLoggingService(container), New Microsoft.Practices.Unity.ContainerControlledLifetimeManager())


        Dim checker As SpeedChecker = container.Resolve(Of Core.Services.ISpeedChecker)()
        Dim config As Core.Services.IConfiguration = container.Resolve(Of Core.Services.IConfiguration)()

        While True
            checker.CheckCurrentSpeed()

            System.Threading.Thread.Sleep(config.SleepWait)
        End While
    End Sub






End Module
