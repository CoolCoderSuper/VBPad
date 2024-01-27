Imports Avalonia.Media
Imports AvaloniaEdit.Document
Imports AvaloniaEdit.Editing
Imports AvaloniaEdit.Rendering
Imports AvaloniaEdit.TextMate

Namespace Editor
    Public Class SyntaxHighlightTransformation
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
            Dim words As New Dictionary(Of Integer, String)
            Dim inWord As Boolean
            Dim word As String
            Dim wordStart As Integer
            For i As Integer = 0 To lineText.Length - 1
                inWord = Not Char.IsWhiteSpace(lineText(i))
                If inWord Then
                    If word = "" Then
                        wordStart = i
                    End If
                    word &= lineText(i)
                Else
                    If word <> "" Then
                        words.Add(wordStart, word)
                        word = ""
                    End If
                End If
            Next
            If word <> "" Then
                words.Add(wordStart, word)
            End If
            For Each pair As KeyValuePair(Of Integer,String) In words.Where(Function(x) x.Value <> Nothing)
                If _service.IsKeyword(pair.Value) Then
                    SetTextStyle(line, pair.Key, pair.Value.Length, Brushes.CornflowerBlue, Nothing, FontStyle.Normal, FontWeight.Normal, False)
                End If
            Next
        End Sub
    End Class
End NameSpace