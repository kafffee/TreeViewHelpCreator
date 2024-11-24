Option Strict On
Imports System.Collections.ObjectModel
Imports System.Runtime.InteropServices.WindowsRuntime
Imports FileAndFolderBrowser.ViewModel.Instrastructure
Imports System.Windows.Input
Imports Microsoft.Win32
Imports Newtonsoft.Json

Public Class HelpDisplayVM
    Inherits Instrastructure.ViewModelBase

    Public Property MainModule As Services.IZentraleKlasse = Services.ServiceContainer.GetService(Of Services.IZentraleKlasse)

    Public Sub New()

    End Sub

    Private _AktuelleDetails As Model.KapitelModel
    Public Property AktuelleDetails As Model.KapitelModel
        Get
            Return _AktuelleDetails
        End Get
        Set(value As Model.KapitelModel)
            _AktuelleDetails = value
            RaisePropertyChanged()
        End Set
    End Property

    Private _OeffneDetails As ICommand
    Public ReadOnly Property OeffneDetails() As ICommand
        Get
            If _OeffneDetails Is Nothing Then _OeffneDetails = New RelayCommand(AddressOf OeffneDetails_Execute, Function(o) True)
            Return _OeffneDetails
        End Get
    End Property
    Private Sub OeffneDetails_Execute(obj As Object)
        AktuelleDetails = DirectCast(obj, Model.KapitelModel)

    End Sub

    Private _Laden As ICommand
    Public ReadOnly Property Laden() As ICommand
        Get
            If _Laden Is Nothing Then _Laden = New RelayCommand(AddressOf Laden_Execute, Function(o) True)
            Return _Laden
        End Get
    End Property
    Private Sub Laden_Execute(obj As Object)
        Dim OFD As New OpenFileDialog
        OFD.InitialDirectory = System.IO.Directory.GetCurrentDirectory
        If OFD.ShowDialog Then
            MainModule.Root = JsonConvert.DeserializeObject(Of ObservableCollection(Of Model.KapitelModel))(System.IO.File.ReadAllText(OFD.FileName))
        End If
    End Sub

    Private _Speichern As ICommand
    Public ReadOnly Property Speichern() As ICommand
        Get
            If _Speichern Is Nothing Then _Speichern = New RelayCommand(AddressOf Speichern_Execute, Function(o) True)
            Return _Speichern
        End Get
    End Property
    Public Sub Speichern_Execute(obj As Object)
        Dim ZuSpeicherndeDTB As String = JsonConvert.SerializeObject(MainModule.Root)
        Dim SFD As New SaveFileDialog
        If SFD.ShowDialog Then
            If Not System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(SFD.FileName)) Then
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(SFD.FileName))
            End If
            System.IO.File.WriteAllText(SFD.FileName, ZuSpeicherndeDTB)
        End If
        MainModule.ChangesWereMade = False
    End Sub

    Private _Bearbeiten As ICommand
    Public ReadOnly Property Bearbeiten() As ICommand
        Get
            If _Bearbeiten Is Nothing Then _Bearbeiten = New RelayCommand(AddressOf Bearbeiten_Execute, Function(o) True)
            Return _Bearbeiten
        End Get
    End Property
    Public Sub Bearbeiten_Execute(obj As Object)
        Dim Objekt As Model.KapitelModel = DirectCast(obj, Model.KapitelModel)
        MainModule.JSONCreator.Icon = Objekt.Icon
        MainModule.JSONCreator.Prefix = Objekt.Prefix
        MainModule.JSONCreator.Ueberschrift = Objekt.Ueberschrift
        MainModule.JSONCreator.Inhalt = Objekt.Inhalt
        MainModule.JSONCreator.BildPfad = Objekt.BildPfad
    End Sub

End Class
