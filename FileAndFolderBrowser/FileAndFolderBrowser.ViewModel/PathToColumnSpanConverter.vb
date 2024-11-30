Option Strict On
Imports System.Globalization
Imports System.Windows
Imports System.Windows.Data

Namespace Converter
    Public Class PathToColumnSpanConverter
        Implements IValueConverter

        Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
            Dim Pfad As String = DirectCast(value, String)

            If String.IsNullOrEmpty(Pfad) Then
                Return 2
            Else
                If System.IO.File.Exists(Pfad) Then
                    Return 1
                Else
                    Return 2
                End If
            End If
        End Function

        Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
            Throw New NotImplementedException()
        End Function
    End Class
End Namespace