Option Strict On
Imports System.Collections.ObjectModel
Imports System.Windows
Imports FileAndFolderBrowser.ViewModel.Instrastructure
Imports System.Windows.Input
Imports Newtonsoft.Json
Imports Microsoft.Win32
Imports System.Runtime.CompilerServices

Public Class JSONCreator
    Inherits Instrastructure.ViewModelBase

    Public Property MainModule As Services.IZentraleKlasse = Services.ServiceContainer.GetService(Of Services.IZentraleKlasse)

    Public Sub New()

    End Sub

    Private KapitelListe() As Model.KapitelModel = {}

    Private _Icon As String
    Public Property Icon As String
        Get
            Return _Icon
        End Get
        Set(value As String)
            _Icon = value
            RaisePropertyChanged()
        End Set
    End Property

    Private _Prefix As String
    Public Property Prefix As String
        Get
            Return _Prefix
        End Get
        Set(value As String)
            _Prefix = value
            RaisePropertyChanged()
        End Set
    End Property

    Private _Ueberschrift As String
    Public Property Ueberschrift As String
        Get
            Return _Ueberschrift
        End Get
        Set(value As String)
            _Ueberschrift = value
            RaisePropertyChanged()
        End Set
    End Property

    Private _Inhalt As String
    Public Property Inhalt As String
        Get
            Return _Inhalt
        End Get
        Set(value As String)
            _Inhalt = value
            RaisePropertyChanged()
        End Set
    End Property

    Private _BildPfad As String
    Public Property BildPfad As String
        Get
            Return _BildPfad
        End Get
        Set(value As String)
            _BildPfad = value
            RaisePropertyChanged()
        End Set
    End Property


    Private Sub Speichern()

        Dim strPrefixes() As String
        Dim Separator() As String = {"."}

        strPrefixes = Prefix.Split(Separator, StringSplitOptions.None)

        Dim Prefixes As New List(Of Integer)

        For Each Eintrag In strPrefixes
            Prefixes.Add(CInt(Eintrag))

        Next

        If KapitelListe.Length <= Prefixes(0) Then
            ReDim Preserve KapitelListe(Prefixes(0))
        End If

        If Prefixes.Count = 2 Then
            If KapitelListe(Prefixes(0)).UnterKapitel.Length <= Prefixes(1) Then
                ReDim Preserve KapitelListe(Prefixes(0)).UnterKapitel(Prefixes(1))
            End If
        End If


        If Prefixes.Count = 1 Then
            WerteSetzen(KapitelListe(Prefixes(0)))
        ElseIf Prefixes.Count = 2 Then
            WerteSetzen(KapitelListe(Prefixes(0)).UnterKapitel(Prefixes(1)))
        End If

        For i = 0 To KapitelListe.Length - 1
            If KapitelListe(i) Is Nothing Then
                KapitelListe(i) = New Model.KapitelModel(CStr(i), "<nicht definiert>", "", "", "")
            End If
        Next

        'If KapitelListe.Count <= Prefixes(0) Then
        '    For i = 0 To Prefixes(0) - 1
        '        If KapitelListe.Count <= Prefixes(0) Then
        '            If i = (Prefixes(0) - 1) Then
        '                KapitelListe.Add(New Model.KapitelModel(Prefix, Ueberschrift, Inhalt, BildPfad, Icon))

        '            End If
        '        Else
        '            'If KapitelListe(i) Is Nothing Then '
        '            KapitelListe(i) = New Model.KapitelModel(Prefix, "<nicht definiert>", "", "", "")
        '            'End If
        '        End If
        '    Next
        'End If

        MainModule.Root = New ObservableCollection(Of Model.KapitelModel)(KapitelListe)
        MainModule.Root.RemoveAt(0)

        MainModule.HelpDisplay.AktuelleDetails = Nothing

    End Sub

    Private Sub WerteSetzen(ByRef argObjekt As Model.KapitelModel)
        argObjekt = New Model.KapitelModel(Prefix, Ueberschrift, Inhalt, BildPfad, Icon)
    End Sub

    Private _AddeKapitel As ICommand
    Public ReadOnly Property AddeKapitel() As ICommand
        Get
            If _AddeKapitel Is Nothing Then _AddeKapitel = New RelayCommand(AddressOf AddeKapitel_Execute, Function(o) True)
            Return _AddeKapitel
        End Get
    End Property
    Private Sub AddeKapitel_Execute(obj As Object)
        Speichern()
        MainModule.ChangesWereMade = True
    End Sub

    Private _OeffneBildDatei As ICommand
    Public ReadOnly Property OeffneBildDatei() As ICommand
        Get
            If _OeffneBildDatei Is Nothing Then _OeffneBildDatei = New RelayCommand(AddressOf OeffneBildDatei_Execute, Function(o) True)
            Return _OeffneBildDatei
        End Get
    End Property
    Private Sub OeffneBildDatei_Execute(obj As Object)
        Dim OFD As New OpenFileDialog
        OFD.InitialDirectory = System.IO.Directory.GetCurrentDirectory
        If OFD.ShowDialog Then
            BildPfad = OFD.FileName
        End If
    End Sub
End Class
