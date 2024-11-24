Class Application
    Private Sub Application_Startup(sender As Object, e As StartupEventArgs) Handles Me.Startup
        ViewModel.Services.ServiceContainer.Instance.AddService(Of ViewModel.Services.IWindowService)(New Services.WindowService())
        ViewModel.Services.ServiceContainer.Instance.AddService(Of ViewModel.Services.IZentraleKlasse)(New Services.ZentraleKlasse())
    End Sub

    ' Ereignisse auf Anwendungsebene wie Startup, Exit und DispatcherUnhandledException
    ' können in dieser Datei verarbeitet werden.

End Class
