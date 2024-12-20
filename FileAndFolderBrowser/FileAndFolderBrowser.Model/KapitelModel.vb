﻿Option Strict On
Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.Runtime.CompilerServices
Imports Newtonsoft.Json

Public Class KapitelModel
    Implements INotifyPropertyChanged

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

    <JsonIgnore>
    Public ReadOnly Property BildPfad As String
        Get
            Return System.IO.Directory.GetCurrentDirectory & "\help\img\" & Prefix & ".png"
        End Get
    End Property

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

    Private _UnterKapitel As New ObservableCollection(Of Model.KapitelModel)
    Public Property UnterKapitel As ObservableCollection(Of KapitelModel)
        Get
            Return _UnterKapitel
        End Get
        Set(value As ObservableCollection(Of KapitelModel))
            _UnterKapitel = value
            RaisePropertyChanged()
        End Set
    End Property

    <JsonIgnore>
    Public ReadOnly Property Prefixes As List(Of Integer)
        Get
            Dim strPrefixes() As String
            Dim Separator() As String = {"."}

            strPrefixes = Prefix.Split(Separator, StringSplitOptions.None)

            Dim retPrefixes As New List(Of Integer)

            For Each Eintrag In strPrefixes
                retPrefixes.Add(CInt(Eintrag))
            Next

            Return retPrefixes
        End Get
    End Property

    Public Sub New(argPrefix As String, argUeberschrift As String, argInhalt As String, argBildPfad As String, argIcon As String)
        Prefix = argPrefix
        Ueberschrift = argUeberschrift
        Inhalt = argInhalt
        'BildPfad = argBildPfad
        Icon = argIcon
    End Sub

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged


    ''' <summary>
    ''' Prozedur wirft den INotifyPropertyChanged Event welcher in der WPF benötigt wird um die UI zu verständingen
    ''' das eine Änderung an einem Property stattgefunden hat.
    ''' </summary>
    ''' <param name="prop">Das Propertie welche sich geändert hat. Ist Optional das der Parameter "CallerMemberName" verwendet</param>
    Protected Overridable Sub RaisePropertyChanged(<CallerMemberName> Optional ByVal prop As String = "")
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(prop))
    End Sub
End Class
