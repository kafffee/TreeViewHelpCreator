Namespace Services
    Public Interface IWindowService

        Function OpenWindow(windowname As String, datacontext As Object, owner As Object, Optional topMost As Boolean = False, Optional showInTaskbar As Boolean = True) As Boolean
        Sub CloseWindow()
        Sub CloseWindow(vm As Object)

    End Interface
End Namespace