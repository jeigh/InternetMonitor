Imports Microsoft.Practices.Unity
Imports InternetMonitor.Core.Services
Imports System.Configuration
Imports System.IO


Namespace Services

    Public Class Configuration
        Implements IConfiguration

        Private Sub New()
        End Sub

        Public Sub New(uc As Microsoft.Practices.Unity.IUnityContainer)
            _resolver = uc
            _id = Guid.NewGuid
            Debug.WriteLine("constructed new SpeedChecker with id" & _id.ToString)

            SleepWait = 60000D

            AuthorizationHeaderContents = File.ReadAllText("AuthorizationHeaderContents.txt")

        End Sub

        Private _id As Guid
        Public ReadOnly _resolver As IUnityContainer

        ' you might need to set this based on your ip address.   
        Public Property SpeedTestUrl As String = "http://192.168.0.1/htmlV/index.asp" Implements IConfiguration.SpeedTestUrl

        ' what are you willing to tolerate before resetting your connection?
        Public Property LowestSpeedThreshold As Decimal = 1000000D Implements IConfiguration.LowestSpeedThreshold

        Public Property SleepWait As Decimal Implements IConfiguration.SleepWait

        ' this will probably get customized per machine;  You'll have to find it out on your own using fiddler.
        Public Property AuthorizationHeaderContents As String Implements IConfiguration.AuthorizationHeaderContents

    End Class

End Namespace