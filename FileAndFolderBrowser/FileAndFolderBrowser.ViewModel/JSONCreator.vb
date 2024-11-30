Option Strict On
Imports System.Collections.ObjectModel
Imports System.Windows
Imports FileAndFolderBrowser.ViewModel.Instrastructure
Imports System.Windows.Input
Imports Newtonsoft.Json
Imports Microsoft.Win32
Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices
Imports Microsoft.VisualBasic
Imports FileAndFolderBrowser.Model

Public Class JSONCreator
    Inherits Instrastructure.ViewModelBase

    Public Property MainModule As Services.IZentraleKlasse = Services.ServiceContainer.GetService(Of Services.IZentraleKlasse)

    Public Sub New()

    End Sub

    Private KapitelListe As New ObservableCollection(Of Model.KapitelModel)

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

            Dim strPrefixes() As String
            Dim Separator() As String = {"."}
            strPrefixes = value.Split(Separator, StringSplitOptions.RemoveEmptyEntries)

            For Each Eintrag In strPrefixes
                If Eintrag.Contains(" ") Then Eintrag.Replace(" ", "")
                If Eintrag = "0" Then
                    MessageBox.Show("Du versuchst, einen ungültigen Wert für das Prefix einzugeben. Es sind nur Zahlen und Punkte erlaubt und maximal drei Punkte, also vier Hierarchieebenen, also z.B. 1, 2.3 oder 3.4.1 usw.. Ausserdem sind keine alleinstehenden Nullen erlaubt, also z.B. 1.0, 0.1 usw.. So etwas ist aber möglich: 10.1")
                    Return
                End If
            Next

            If (IsNumeric(value) And strPrefixes.Length <= 4) OrElse (String.IsNullOrEmpty(value)) Then
                _Prefix = value
                RaisePropertyChanged()
            Else
                MessageBox.Show("Du versuchst, einen ungültigen Wert für das Prefix einzugeben. Es sind nur Zahlen und Punkte erlaubt und maximal drei Punkte, also vier Hierarchieebenen, also z.B. 1, 2.3 oder 3.4.1 usw.. Ausserdem sind keine alleinstehenden Nullen erlaubt, also z.B. 1.0, 0.1 usw.. So etwas ist aber möglich: 10.1")
            End If
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
        Try

            If String.IsNullOrEmpty(Prefix) Then
                MessageBox.Show("Bitte gebe ein gültiges Prefix ein.")
                Return
            End If

            If String.IsNullOrEmpty(Ueberschrift) Then
                MessageBox.Show("Bitte gebe eine Überschrift ein.")
                Return
            End If


            Dim strPrefixes() As String
            Dim Separator() As String = {"."}

            strPrefixes = Prefix.Split(Separator, StringSplitOptions.None)

            Dim Prefixes As New List(Of Integer)

            For Each Eintrag In strPrefixes
                Prefixes.Add(CInt(Eintrag))
            Next

            Select Case Prefixes.Count
                Case 1
                    ErzeugeEintraege(MainModule.Root, Prefixes)
                    MainModule.HelpDisplay.AktuelleDetails = MainModule.Root(Prefixes(0) - 1)
                Case 2
                    ErzeugeEbene1(Prefixes)
                    ErzeugeEintraege(MainModule.Root(Prefixes(0) - 1).UnterKapitel, Prefixes)
                    MainModule.HelpDisplay.AktuelleDetails = MainModule.Root(Prefixes(0) - 1).UnterKapitel(Prefixes(1) - 1)
                Case 3
                    ErzeugeEbene1(Prefixes)
                    ErzeugeEbene2(Prefixes)
                    ErzeugeEintraege(MainModule.Root(Prefixes(0) - 1).UnterKapitel(Prefixes(1) - 1).UnterKapitel, Prefixes)
                    MainModule.HelpDisplay.AktuelleDetails = MainModule.Root(Prefixes(0) - 1).UnterKapitel(Prefixes(1) - 1).UnterKapitel(Prefixes(2) - 1)
                Case 4
                    ErzeugeEbene1(Prefixes)
                    ErzeugeEbene2(Prefixes)
                    ErzeugeEbene3(Prefixes)
                    ErzeugeEintraege(MainModule.Root(Prefixes(0) - 1).UnterKapitel(Prefixes(1) - 1).UnterKapitel(Prefixes(2) - 1).UnterKapitel, Prefixes)
                    MainModule.HelpDisplay.AktuelleDetails = MainModule.Root(Prefixes(0) - 1).UnterKapitel(Prefixes(1) - 1).UnterKapitel(Prefixes(2) - 1).UnterKapitel(Prefixes(3) - 1)
            End Select

            Prefix = ""
            Icon = ""
            Ueberschrift = ""
            Inhalt = ""
        Catch ex As Exception
            MessageBox.Show("Es ist ein Fehler aufgetreten." & Environment.NewLine & "Fehlermeldung: " & ex.Message)
        End Try
    End Sub

    Private Sub ErzeugeEbene1(argPrefixes As List(Of Integer))
        Dim index As Integer = MainModule.Root.ToList.FindIndex(Function(x) x.Prefixes(0) = argPrefixes(0))
        If index = -1 Then
            Dim prfx As New List(Of Integer)
            prfx.Add(argPrefixes(0))
            AddeEintrag(MainModule.Root, prfx, True)
        End If
    End Sub

    Private Sub ErzeugeEbene2(argPrefixes As List(Of Integer))
        Dim index As Integer = MainModule.Root(argPrefixes(0) - 1).UnterKapitel.ToList.FindIndex(Function(x) x.Prefixes(1) = argPrefixes(1))
        If index = -1 Then
            Dim prfx As New List(Of Integer)
            prfx.Add(argPrefixes(0))
            prfx.Add(argPrefixes(1))
            AddeEintrag(MainModule.Root(argPrefixes(0) - 1).UnterKapitel, prfx, True)
        End If
    End Sub

    Private Sub ErzeugeEbene3(argPrefixes As List(Of Integer))
        Dim index As Integer = MainModule.Root(argPrefixes(0) - 1).UnterKapitel(argPrefixes(1) - 1).UnterKapitel.ToList.FindIndex(Function(x) x.Prefixes(1) = argPrefixes(1))
        If index = -1 Then
            Dim prfx As New List(Of Integer)
            prfx.Add(argPrefixes(0))
            prfx.Add(argPrefixes(1))
            prfx.Add(argPrefixes(2))
            AddeEintrag(MainModule.Root(argPrefixes(0) - 1).UnterKapitel(argPrefixes(1) - 1).UnterKapitel, prfx, True)
        End If
    End Sub

    Private Sub ErzeugeEintraege(ByRef Ebene As ObservableCollection(Of Model.KapitelModel), argPrefixes As List(Of Integer))
        Dim EintragErsetzt As Boolean = False
        If Ebene.Count = 0 Then
            AddeEintrag(Ebene, argPrefixes, False)

        Else
            For i = 0 To Ebene.Count - 1
                If Ebene(i).Prefix = Prefix Then
                    Dim Ergebnis As MessageBoxResult = MessageBox.Show("Ein Kapitel mit diesem Prefix ist bereits vorhanden." & Environment.NewLine & Environment.NewLine & "Wenn dieser überschrieben werden soll, klicke auf Ja." & Environment.NewLine & Environment.NewLine & "Wenn das neue Kapitel (" & Prefix & ") an dieser Stelle eingefügt werden soll, klicke auf Nein. Dabei werden alle Prefixes dieser Hierarchieebene um eins erhöht.", "", MessageBoxButton.YesNoCancel)
                    If Ergebnis = MessageBoxResult.Yes Then
                        Ebene(i) = New Model.KapitelModel(Prefix, Ueberschrift, Inhalt, BildPfad, Icon)
                        EintragErsetzt = True
                        Exit For
                    ElseIf ergebnis = MessageBoxResult.No Then
                        InsertKapitel_Execute(Nothing)
                        EintragErsetzt = True
                        Exit For
                    ElseIf Ergebnis = MessageBoxResult.Cancel Then
                        Return
                    End If
                End If
            Next


            If Not EintragErsetzt Then AddeEintrag(Ebene, argPrefixes, False)
        End If
    End Sub
    Private Sub AddeEintrag(ByRef Ebene As ObservableCollection(Of Model.KapitelModel), argPrefixes As List(Of Integer), AufrufVonSpeichern As Boolean)

        Dim LaengeLetztesPrefix As Integer = CStr(argPrefixes(argPrefixes.Count - 1)).Length

        Dim strPrefix As String = String.Join(".", argPrefixes)

        Dim VorhandenePrefixes As New List(Of Integer)
        For i = 0 To Ebene.Count - 1
            VorhandenePrefixes.Add(i + 1)
        Next
        For j = 1 To argPrefixes(argPrefixes.Count - 1) - 1
            If Not VorhandenePrefixes.Contains(j) Then
                Ebene.Add(New Model.KapitelModel(strPrefix.Substring(0, strPrefix.Length - LaengeLetztesPrefix) & CStr(j), "<nicht festgelegt>", "", "", ""))
            End If
        Next


        If AufrufVonSpeichern Then
            Ebene.Add(New Model.KapitelModel(String.Join(".", argPrefixes), "<nicht festgelegt>", "", "", ""))
        Else
            Ebene.Add(New Model.KapitelModel(String.Join(".", argPrefixes), Ueberschrift, Inhalt, BildPfad, Icon))
        End If
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

    Private _InsertKapitel As ICommand
    Public ReadOnly Property InsertKapitel() As ICommand
        Get
            If _InsertKapitel Is Nothing Then _InsertKapitel = New RelayCommand(AddressOf InsertKapitel_Execute, Function(o) True)
            Return _InsertKapitel
        End Get
    End Property
    Private Sub InsertKapitel_Execute(obj As Object)

        Dim Separator() As String = {"."}
        Dim Prefixes() As String = Prefix.Split(Separator, StringSplitOptions.None)
        Dim intPrefixes As New List(Of Integer)

        For Each item In Prefixes
            intPrefixes.Add(CInt(item))
        Next

        Select Case intPrefixes.Count
            Case 1
                If intPrefixes(0) - 1 > MainModule.Root.Count Then
                    MessageBox.Show("Folgendes Kapitel kann nicht eingefügt werden, da es sich ausserhalb der bereits definierten Grenzen befindet:" & Environment.NewLine & Environment.NewLine & "Prefix: " & Prefix & Environment.NewLine & "Überschrift: " & Ueberschrift & Environment.NewLine & Environment.NewLine & "Um dieses Kapitel zuzufügen, klicke bitte auf Hinzufügen/Ändern.")
                    Return
                End If
                MainModule.Root.Insert(intPrefixes(0) - 1, New Model.KapitelModel(Prefix, Ueberschrift, Inhalt, BildPfad, Icon))
                For i = intPrefixes(0) - 1 To MainModule.Root.Count - 1
                    MainModule.Root(i).Prefix = CStr(i + 1)
                Next
            Case 2
                If intPrefixes(1) - 1 > MainModule.Root(intPrefixes(0) - 1).UnterKapitel.Count Then
                    MessageBox.Show("Folgendes Kapitel kann nicht eingefügt werden, da es sich ausserhalb der bereits definierten Grenzen befindet:" & Environment.NewLine & Environment.NewLine & "Prefix: " & Prefix & Environment.NewLine & "Überschrift: " & Ueberschrift & Environment.NewLine & Environment.NewLine & "Um dieses Kapitel zuzufügen, klicke bitte auf Hinzufügen/Ändern.")
                    Return
                End If
                MainModule.Root(intPrefixes(0) - 1).UnterKapitel.Insert(intPrefixes(1) - 1, New Model.KapitelModel(Prefix, Ueberschrift, Inhalt, BildPfad, Icon))
                For i = intPrefixes(1) - 1 To MainModule.Root(intPrefixes(0) - 1).UnterKapitel.Count - 1
                    MainModule.Root(intPrefixes(0) - 1).UnterKapitel(i).Prefix = MainModule.HelpDisplay.ErstelleNeuesPrefix(intPrefixes, i)
                Next
            Case 3
                If intPrefixes(2) - 1 > MainModule.Root(intPrefixes(0) - 1).UnterKapitel(intPrefixes(1) - 1).UnterKapitel.Count Then
                    MessageBox.Show("Folgendes Kapitel kann nicht eingefügt werden, da es sich ausserhalb der bereits definierten Grenzen befindet:" & Environment.NewLine & Environment.NewLine & "Prefix: " & Prefix & Environment.NewLine & "Überschrift: " & Ueberschrift & Environment.NewLine & Environment.NewLine & "Um dieses Kapitel zuzufügen, klicke bitte auf Hinzufügen/Ändern.")
                    Return
                End If
                MainModule.Root(intPrefixes(0) - 1).UnterKapitel(intPrefixes(1) - 1).UnterKapitel.Insert(intPrefixes(2) - 1, New Model.KapitelModel(Prefix, Ueberschrift, Inhalt, BildPfad, Icon))
                For i = intPrefixes(2) - 1 To MainModule.Root(intPrefixes(0) - 1).UnterKapitel(intPrefixes(1) - 1).UnterKapitel.Count - 1
                    MainModule.Root(intPrefixes(0) - 1).UnterKapitel(intPrefixes(1) - 1).UnterKapitel(i).Prefix = MainModule.HelpDisplay.ErstelleNeuesPrefix(intPrefixes, i)
                Next
            Case 4
                If intPrefixes(3) - 1 > MainModule.Root(intPrefixes(0) - 1).UnterKapitel(intPrefixes(1) - 1).UnterKapitel(intPrefixes(2) - 1).UnterKapitel.Count Then
                    MessageBox.Show("Folgendes Kapitel kann nicht eingefügt werden, da es sich ausserhalb der bereits definierten Grenzen befindet:" & Environment.NewLine & Environment.NewLine & "Prefix: " & Prefix & Environment.NewLine & "Überschrift: " & Ueberschrift & Environment.NewLine & Environment.NewLine & "Um dieses Kapitel zuzufügen, klicke bitte auf Hinzufügen/Ändern.")
                    Return
                End If
                MainModule.Root(intPrefixes(0) - 1).UnterKapitel(intPrefixes(1) - 1).UnterKapitel(intPrefixes(2) - 1).UnterKapitel(intPrefixes(3) - 1).UnterKapitel.Insert(intPrefixes(3) - 1, New Model.KapitelModel(Prefix, Ueberschrift, Inhalt, BildPfad, Icon))
                For i = intPrefixes(3) - 1 To MainModule.Root(intPrefixes(0) - 1).UnterKapitel(intPrefixes(1) - 1).UnterKapitel(intPrefixes(2) - 1).UnterKapitel.Count - 1
                    MainModule.Root(intPrefixes(0) - 1).UnterKapitel(intPrefixes(1) - 1).UnterKapitel(intPrefixes(2) - 1).UnterKapitel(i).Prefix = MainModule.HelpDisplay.ErstelleNeuesPrefix(intPrefixes, i)
                Next
        End Select
    End Sub
End Class
