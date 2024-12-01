Option Strict On
Imports FileAndFolderBrowser.ViewModel.Instrastructure

Public Class MainViewModel
    Inherits ViewModelBase

    Private MainModule As Services.IZentraleKlasse = Services.ServiceContainer.GetService(Of Services.IZentraleKlasse)
    Public Sub New()
        'Debug.WriteLine("")
        MainModule.JSONCreator = JSONCreator
        MainModule.HelpDisplay = HelpDisplay
    End Sub
    Public Property JSONCreator As New JSONCreator
    Public Property HelpDisplay As New HelpDisplayVM
End Class
