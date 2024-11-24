Option Strict On
Imports System.Collections.ObjectModel

Namespace Services

    Public Interface IZentraleKlasse
        Property Root As ObservableCollection(Of Model.KapitelModel)
        Property ChangesWereMade As Boolean
        Property JSONCreator As JSONCreator
        Property HelpDisplay As HelpDisplayVM
    End Interface
End Namespace