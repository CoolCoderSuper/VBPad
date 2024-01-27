Imports Avalonia.Media
Imports AvaloniaEdit.Document
Imports AvaloniaEdit.Editing
Imports AvaloniaEdit.Rendering
Imports AvaloniaEdit.TextMate

Namespace Editor
    Public Class CommentHighlightTransformation
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
            Dim commentStart As Integer = lineText.IndexOf("'", StringComparison.Ordinal)
            If commentStart >= 0 Then
                If commentStart + 1 < lineText.Length AndAlso commentStart + 2 < lineText.Length AndAlso lineText(commentStart + 1) = "'" AndAlso lineText(commentStart + 2) = "'" Then
                    SetTextStyle(line, commentStart, lineText.Length - commentStart, Brushes.Gray, Nothing, FontStyle.Normal, FontWeight.Normal, False)
                Else
                    SetTextStyle(line, commentStart, lineText.Length - commentStart, Brushes.Green, Nothing, FontStyle.Normal, FontWeight.Normal, False)
                End If
            End If
        End Sub
    End Class
End NameSpace