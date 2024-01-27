Imports Avalonia.Media
Imports AvaloniaEdit.Document
Imports AvaloniaEdit.Editing
Imports AvaloniaEdit.Rendering
Imports AvaloniaEdit.TextMate
'TODO: Multiline strings
Namespace Editor
    Public Class StringHighlightTransformation
        Inherits GenericLineTransformer

        Private ReadOnly _textArea As TextArea
        Private ReadOnly _service As VBLanguageService

        Public Sub New(textArea As TextArea, service As VBLanguageService)
            MyBase.New(Sub(e)

            End Sub)
            _textArea = textArea
            _service = service
        End Sub

        Protected Overrides Sub TransformLine(line As DocumentLine, context As ITextRunConstructionContext)
            Dim lineText As String = context.GetText(line.Offset, line.Length).Text
            Dim parts As New List(Of (Offset As Integer, Length As Integer))
            Dim stringStart As Integer
            Dim inString As Boolean
            For i As Integer = 0 To lineText.Length - 1
                If lineText(i) = """" OrElse (inString AndAlso lineText(i) = "{" AndAlso (i + 1 < lineText.Length AndAlso lineText(i + 1) <> "{") AndAlso (i - 1 > -1 AndAlso lineText(i - 1) <> "{")) OrElse (Not inString AndAlso lineText(i) = "}" AndAlso (i + 1 < lineText.Length AndAlso lineText(i + 1) <> "}") AndAlso (i - 1 > -1 AndAlso lineText(i - 1) <> "}")) Then
                    inString = Not inString
                    If Not inString Then
                        parts.Add((stringStart, i - stringStart + 1))
                    Else
                        If i - 1 > -1 AndAlso lineText(i - 1) = "$" Then
                            stringStart = i - 1
                        Else
                            stringStart = i
                        End If
                    End If
                End If
            Next
            For Each part As (Offset as Integer, Length as Integer) In parts
                SetTextStyle(line, part.Offset, part.Length, Brushes.IndianRed, Nothing, FontStyle.Normal, FontWeight.Normal, False)
            Next
        End Sub
    End Class
End NameSpace