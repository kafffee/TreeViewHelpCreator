Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.Runtime.CompilerServices
Imports FileAndFolderBrowser.Model
Imports FileAndFolderBrowser.ViewModel.Services

Namespace Services
    Public Class ZentraleKlasse
        Implements ViewModel.Services.IZentraleKlasse
        Implements INotifyPropertyChanged

        Private _Root As New ObservableCollection(Of KapitelModel)
        Public Property Root As ObservableCollection(Of KapitelModel) Implements IZentraleKlasse.Root
            Get
                Return _Root
            End Get
            Set(value As ObservableCollection(Of KapitelModel))
                _Root = value
                RaisePropertyChanged()
            End Set
        End Property

        Public Property ChangesWereMade As Boolean Implements IZentraleKlasse.ChangesWereMade

        Public Property JSONCreator As ViewModel.JSONCreator Implements IZentraleKlasse.JSONCreator

        Public Property HelpDisplay As ViewModel.HelpDisplayVM Implements IZentraleKlasse.HelpDisplay

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        Protected Overridable Sub RaisePropertyChanged(<CallerMemberName> Optional ByVal prop As String = "")
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(prop))
        End Sub
    End Class
End Namespace
