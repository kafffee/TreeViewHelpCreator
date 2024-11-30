Option Strict On
Imports System.Globalization
Imports System.Windows
Imports System.Windows.Data

Namespace Converter
    Public Class CreatorModeToVisibilityConverter
        Implements IValueConverter

        Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert

            Dim Wert As Boolean = DirectCast(value, Boolean)

            If Wert Then
                Return Visibility.Visible
            Else
                Return Visibility.Hidden
            End If

        End Function

        Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
            Throw New NotImplementedException()
        End Function
    End Class
End Namespace