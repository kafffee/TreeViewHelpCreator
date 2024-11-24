Class MainWindow

    Public Property MainModule As ViewModel.Services.IZentraleKlasse = ViewModel.Services.ServiceContainer.GetService(Of ViewModel.Services.IZentraleKlasse)
    Private Sub Window_Closing(sender As Object, e As ComponentModel.CancelEventArgs)
        Dim vm = CType(Me.DataContext, ViewModel.MainViewModel)
        If ((MainModule.Root IsNot Nothing) Or (MainModule.Root.Count <> 0)) And MainModule.ChangesWereMade Then
            Dim Ergebnis As MessageBoxResult = MessageBox.Show("Du hast Änderungen vorgenommen. Möchtest du diese vor dem Beenden speichern?", "", MessageBoxButton.YesNo)
            If Ergebnis = MessageBoxResult.Yes Then
                vm.HelpDisplay.Speichern_Execute(Nothing)
            End If
        End If
    End Sub
End Class
