Imports Avalonia.Media
Imports AvaloniaEdit.Document
Imports AvaloniaEdit.Editing
Imports AvaloniaEdit.Rendering
Imports AvaloniaEdit.TextMate
Imports Microsoft.CodeAnalysis
'TODO: Better word distinction
Namespace Editor
    Public Class SemanticHighlightTransformation
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
                inWord = Not Char.IsWhiteSpace(lineText(i)) AndAlso Not ".,()'=+-*/&{}:".Contains(lineText(i))
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
                Dim s As ISymbol = _service.GetSymbol(GetDocumentIndex(pair, line))
                If TypeOf s Is ITypeSymbol Then
                    SetTextStyle(line, pair.Key, pair.Value.Length, Brushes.Purple, Nothing, FontStyle.Normal, FontWeight.Normal, False)
                ElseIf TypeOf s Is IMethodSymbol Then
                    SetTextStyle(line, pair.Key, pair.Value.Length, Brushes.LawnGreen, Nothing, FontStyle.Normal, FontWeight.Normal, False)
                ElseIf TypeOf s Is IPropertySymbol Then
                    SetTextStyle(line, pair.Key, pair.Value.Length, Brushes.DodgerBlue, Nothing, FontStyle.Normal, FontWeight.Normal, False)
                ElseIf TypeOf s Is IFieldSymbol Then
                    SetTextStyle(line, pair.Key, pair.Value.Length, Brushes.DodgerBlue, Nothing, FontStyle.Normal, FontWeight.Normal, False)
                ElseIf TypeOf s Is IEventSymbol Then
                    SetTextStyle(line, pair.Key, pair.Value.Length, Brushes.DodgerBlue, Nothing, FontStyle.Normal, FontWeight.Normal, False)
                End If
            Next
        End Sub

        Private Function GetDocumentIndex(pair As KeyValuePair(Of Integer,String), line As DocumentLine) As Integer
            Dim val As Integer
            For i As Integer = 0 To line.LineNumber - 2
                val += _textArea.Document.Lines(i).Length + 2'line ending
            Next
            Return pair.Key + val
        End Function
    End Class
End NameSpace