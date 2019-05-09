Imports InternetMonitor.Implementation.Services
Imports Microsoft.Practices.Unity

Public Class InternetMonitorService

    Public Sub New()
        MyBase.New()
        InitializeComponent()
        Me.EventLog1 = New System.Diagnostics.EventLog
        If Not System.Diagnostics.EventLog.SourceExists("MySource") Then
            System.Diagnostics.EventLog.CreateEventSource("MySource",
            "MyNewLog")
        End If
        EventLog1.Source = "MySource"
        EventLog1.Log = "MyNewLog"






    End Sub


    Private _container As IUnityContainer
    Private ReadOnly Property container As IUnityContainer
        Get
            If _container Is Nothing Then
                _container = New UnityContainer

                _container.RegisterInstance(Of Core.Services.IConfiguration)(New Implementation.Services.Configuration(container), New Microsoft.Practices.Unity.ContainerControlledLifetimeManager())
                _container.RegisterInstance(Of Core.Services.ISpeedLogger)(New Implementation.Services.SpeedLogger(container), New Microsoft.Practices.Unity.ContainerControlledLifetimeManager())
                _container.RegisterInstance(Of Core.Services.ISpeedChecker)(New Implementation.Services.SpeedChecker(container), New Microsoft.Practices.Unity.ContainerControlledLifetimeManager())
                _container.RegisterInstance(Of Core.Services.ISpeedHandler)(New Implementation.Services.SpeedHandler(container), New Microsoft.Practices.Unity.ContainerControlledLifetimeManager())
            End If
            Return _container
        End Get
    End Property


    Private _paused As Boolean = False

    Protected Overrides Sub OnStart(ByVal args() As String)

        Dim checker As SpeedChecker = container.Resolve(Of Core.Services.ISpeedChecker)()

        While Not _paused
            checker.CheckCurrentSpeed()
            System.Threading.Thread.Sleep(60000)
        End While

    End Sub




End Class
