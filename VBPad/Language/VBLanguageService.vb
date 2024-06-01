Imports System.IO
Imports System.Reflection
Imports Microsoft.CodeAnalysis
Imports Microsoft.CodeAnalysis.Emit
Imports Microsoft.CodeAnalysis.FindSymbols
Imports Microsoft.CodeAnalysis.Host.Mef
Imports Microsoft.CodeAnalysis.Text
Imports Microsoft.CodeAnalysis.VisualBasic
Imports Microsoft.CodeAnalysis.VisualBasic.Syntax

Public Class VBLanguageService
    Dim _host As MefHostServices
    Dim _workspace As AdhocWorkspace
    Dim _project As Project
    Dim _document As Document

    Public Sub Init(referencePaths As String())
        Dim lReferences As MetadataReference() = ReferenceProvider.GetReferences.Concat({MetadataReference.CreateFromFile(GetType(Information).Assembly.Location)}).Concat(referencePaths.Select(Function(x) MetadataReference.CreateFromFile(x))).ToArray
        _host = MefHostServices.Create(MefHostServices.DefaultAssemblies)
        _workspace = New AdhocWorkspace(_host)
        _project = _workspace.AddProject(ProjectInfo.Create(ProjectId.CreateNewId, VersionStamp.Create, "report", "report", LanguageNames.VisualBasic, compilationOptions:=New VisualBasicCompilationOptions(OutputKind.DynamicallyLinkedLibrary)).WithMetadataReferences(lReferences))
        _document = _workspace.AddDocument(_project.Id, "report.vb", SourceText.From(""))
    End Sub

    Public Sub UpdateScript(script As String)
        _document = _document.WithText(SourceText.From(script))
        _project = _document.Project
    End Sub

    Public Function GetDiagnostics() As List(Of Diagnostic)
        Dim results As New List(Of Diagnostic)
        For Each diag As Microsoft.CodeAnalysis.Diagnostic In _project.GetCompilationAsync.Result.GetDiagnostics
            Dim diagnostic As New Diagnostic
            With diagnostic
                .ID = diag.Id
                .Message = diag.GetMessage
                .Severity = If(diag.Severity = DiagnosticSeverity.Error, Severity.Error, Severity.Warning)
                .Location = New Location With {.InSource = diag.Location.IsInSource, .Start = diag.Location.SourceSpan.Start, .Length = diag.Location.SourceSpan.Length, .Line = diag.Location.GetLineSpan.StartLinePosition.Line, .LineStart = diag.Location.GetLineSpan.StartLinePosition.Character, .LineLength = diag.Location.GetLineSpan.EndLinePosition.Character - .LineStart}
            End With
            results.Add(diagnostic)
        Next
        Return results
    End Function

    Public Function Format(code As String) As String
        Return SyntaxFactory.ParseCompilationUnit(code).NormalizeWhitespace.ToFullString
    End Function

    Public Function Compile() As Assembly
        Dim compilation As Compilation = _project.GetCompilationAsync.Result
        Dim asmStream As New MemoryStream
        Dim emitResult As EmitResult = compilation.Emit(asmStream)
        If emitResult.Success Then
            Return Assembly.Load(asmStream.ToArray)
        End If
        Throw New Exception("Compilation failed.")
    End Function

    Public Function IsKeyword(word As String) As Boolean
        Dim token As SyntaxToken = SyntaxFactory.ParseToken(word)
        Return token.IsReservedKeyword() OrElse token.IsContextualKeyword()
    End Function

    Public Function GetSymbol(pos As Integer) As ISymbol
        Return SymbolFinder.FindSymbolAtPositionAsync(_document, pos).Result
    End Function

    Public Async Function GetBlocks() As Task(Of TextSpan())
        Dim tree = Await _document.GetSyntaxRootAsync
        Dim blockTypes As Type() = {GetType(ClassBlockSyntax), GetType(StructureBlockSyntax), GetType(MethodBlockSyntax), GetType(EnumBlockSyntax), GetType(MultiLineIfBlockSyntax), GetType(WithBlockSyntax), GetType(SelectBlockSyntax), GetType(ModuleBlockSyntax)}
        Return tree.DescendantNodes.Where(Function(x) blockTypes.Any(Function(y) y.IsAssignableFrom(x.GetType))).Select(Function(x) x.Span).ToArray
    End Function
End Class