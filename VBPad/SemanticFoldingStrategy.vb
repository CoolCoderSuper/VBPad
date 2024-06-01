Imports System.Runtime.InteropServices
Imports AvaloniaEdit.Document
Imports AvaloniaEdit.Folding
Imports Microsoft.CodeAnalysis.Text

''' </summary>
Public Class SemanticFoldingStrategy
    Private ReadOnly _service As VBLanguageService

    Public Sub New(service As VBLanguageService)
        _service = service
    End Sub

    Public Sub UpdateFoldings(ByVal manager As FoldingManager, ByVal document As TextDocument)
        Dim firstErrorOffset As Integer
        Dim newFoldings = Me.CreateNewFoldings(document, firstErrorOffset)
        manager.UpdateFoldings(newFoldings, firstErrorOffset)
    End Sub

    Public Function CreateNewFoldings(ByVal document As TextDocument, <Out> ByRef firstErrorOffset As Integer) As IEnumerable(Of NewFolding)
        firstErrorOffset = -1
        Return CreateNewFoldings(document)
    End Function

    Public Function CreateNewFoldings(ByVal document As ITextSource) As IEnumerable(Of NewFolding)
        Dim newFoldings As New List(Of NewFolding)()
        Dim blocks As TextSpan() = _service.GetBlocks.Result
        For Each block In blocks
            newFoldings.Add(New NewFolding(block.Start, block.End))
        Next
        newFoldings.Sort(Function(a, b) a.StartOffset.CompareTo(b.StartOffset))
        Return newFoldings
    End Function
End Class