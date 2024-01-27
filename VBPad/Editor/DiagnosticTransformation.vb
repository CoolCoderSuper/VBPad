Imports Avalonia.Media
Imports AvaloniaEdit.Document
Imports AvaloniaEdit.Editing
Imports AvaloniaEdit.Rendering
Imports AvaloniaEdit.TextMate

Namespace Editor
    Public Class DiagnosticTransformation
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
            Dim diagnostics As List(Of Diagnostic) = _service.GetDiagnostics().Where(Function(x) x.Location.Line = line.LineNumber - 1).ToList
            For Each diagnostic As Diagnostic In diagnostics
                SetTextStyle(line, diagnostic.Location.LineStart, diagnostic.Location.LineLength, If(diagnostic.Severity = Severity.Error, Brushes.Red, Brushes.YellowGreen), Nothing, FontStyle.Normal, FontWeight.Normal, True)
            Next
        End Sub
    End Class
End NameSpace