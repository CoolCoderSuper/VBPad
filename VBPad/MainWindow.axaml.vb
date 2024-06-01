Imports Avalonia.Controls
Imports Avalonia.Input
Imports Avalonia.Interactivity
Imports Avalonia.Markup.Xaml
Imports AvaloniaEdit
Imports AvaloniaEdit.Folding
Imports VBPad.Editor
'TODO: Use binding instead of control names
Partial Public Class MainWindow
    Inherits Window

#Region "Components"
    Private tvScripts As TreeView
    Private txtCode As TextEditor
    Private dgDiagnostics As DataGrid
    Private _generator As New ElementGenerator()
    Private _foldingManager As FoldingManager
    Private _foldingStrategy As SemanticFoldingStrategy
    Private _service As VBLanguageService

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub InitializeComponent()
        AvaloniaXamlLoader.Load(Me)
        tvScripts = FindControl(Of TreeView)("tvScripts")
        dgDiagnostics = FindControl(Of DataGrid)("dgDiagnostics")

        _service = New VBLanguageService
        _service.Init(Array.Empty(Of String)())
        txtCode = FindControl(Of TextEditor)("txtCode")
        txtCode.HorizontalScrollBarVisibility = Primitives.ScrollBarVisibility.Visible
        txtCode.Background = Avalonia.Media.Brushes.Transparent
        txtCode.ShowLineNumbers = True
        txtCode.ContextMenu = New ContextMenu With {
        .ItemsSource = New List(Of MenuItem) From {
                            New MenuItem With {
                                .Header = "Copy",
                                .InputGesture = New KeyGesture(Key.C, KeyModifiers.Control)
                            },
                            New MenuItem With {
                                .Header = "Paste",
                                .InputGesture = New KeyGesture(Key.V, KeyModifiers.Control)
                            },
                            New MenuItem With {
                                .Header = "Cut",
                                .InputGesture = New KeyGesture(Key.X, KeyModifiers.Control)
                            }
                }
        }
        txtCode.TextArea.Background = Background
        txtCode.Options.ShowBoxForControlCharacters = True
        txtCode.Options.ColumnRulerPositions = {80, 100}.ToList
        txtCode.TextArea.RightClickMovesCaret = True
        txtCode.TextArea.TextView.ElementGenerators.Add(_generator)
        [AddHandler](PointerWheelChangedEvent, Sub(o, i)
                                                   If i.KeyModifiers <> KeyModifiers.Control Then Return
                                                   If i.Delta.Y > 0 Then
                                                       txtCode.FontSize += 1
                                                   Else
                                                       txtCode.FontSize = If(txtCode.FontSize > 1, txtCode.FontSize - 1, 1)
                                                   End If
                                               End Sub, RoutingStrategies.Bubble, True)
        txtCode.TextArea.TextView.LineTransformers.Add(New SyntaxHighlightTransformation(txtCode.TextArea, _service))
        txtCode.TextArea.TextView.LineTransformers.Add(New SemanticHighlightTransformation(txtCode.TextArea, _service))
        txtCode.TextArea.TextView.LineTransformers.Add(New CommentHighlightTransformation(txtCode.TextArea, _service))
        txtCode.TextArea.TextView.LineTransformers.Add(New StringHighlightTransformation(txtCode.TextArea, _service))
        txtCode.TextArea.TextView.LineTransformers.Add(New DiagnosticTransformation(txtCode.TextArea, _service))
        _foldingStrategy = New SemanticFoldingStrategy(_service)
        _foldingManager = FoldingManager.Install(txtCode.TextArea)
        dgDiagnostics.ItemsSource = New List(Of Diagnostic)
    End Sub

#End Region

    Private Sub btnExit_Click(sender As Object, e As RoutedEventArgs)
        Close()
    End Sub

    Private Sub txtCode_TextChanged(sender As Object, e As EventArgs)
        _service.UpdateScript(txtCode.Text)
        dgDiagnostics.ItemsSource = _service.GetDiagnostics
        UpdateFolding()
    End Sub

    Private Sub dgDiagnostics_DoubleTapped(sender As Object, e As TappedEventArgs)
        Dim diagnostic As Diagnostic = dgDiagnostics.SelectedItem
        If diagnostic IsNot Nothing Then
            txtCode.TextArea.Caret.Line = diagnostic.Location.Line
            txtCode.TextArea.Caret.Column = diagnostic.Location.LineStart
            txtCode.Focus()
        End If
    End Sub

    Private Sub btnFormat_Click(sender As Object, e As RoutedEventArgs)
        txtCode.Text = _service.Format(txtCode.Text)
    End Sub
    
    Private Sub UpdateFolding()
        _foldingStrategy.UpdateFoldings(_foldingManager, txtCode.Document)
    End Sub
End Class