' Copyright (c) 2009 Daniel Grunwald
' 
' Permission is hereby granted, free of charge, to any person obtaining a copy of this
' software and associated documentation files (the "Software"), to deal in the Software
' without restriction, including without limitation the rights to use, copy, modify, merge,
' publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
' to whom the Software is furnished to do so, subject to the following conditions:
' 
' The above copyright notice and this permission notice shall be included in all copies or
' substantial portions of the Software.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
' INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
' PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
' FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
' OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.

Imports System.Collections.Generic
Imports System.Runtime.InteropServices
Imports AvaloniaEdit.Document
Imports AvaloniaEdit.Folding

''' <summary>
''' Allows producing foldings from a document based on braces.
''' </summary>
Public Class BraceFoldingStrategy
    ''' <summary>
    ''' Gets/Sets the opening brace. The default value is '{'.
    ''' </summary>
    Public Property OpeningBrace As Char

    ''' <summary>
    ''' Gets/Sets the closing brace. The default value is '}'.
    ''' </summary>
    Public Property ClosingBrace As Char

    ''' <summary>
    ''' Creates a new BraceFoldingStrategy.
    ''' </summary>
    Public Sub New()
        OpeningBrace = "{"c
        ClosingBrace = "}"c
    End Sub

    Public Sub UpdateFoldings(ByVal manager As FoldingManager, ByVal document As TextDocument)
        Dim firstErrorOffset As Integer
        Dim newFoldings = Me.CreateNewFoldings(document, firstErrorOffset)
        manager.UpdateFoldings(newFoldings, firstErrorOffset)
    End Sub

    ''' <summary>
    ''' Create <seecref="NewFolding"/>s for the specified document.
    ''' </summary>
    Public Function CreateNewFoldings(ByVal document As TextDocument, <Out> ByRef firstErrorOffset As Integer) As IEnumerable(Of NewFolding)
        firstErrorOffset = -1
        Return CreateNewFoldings(document)
    End Function

    ''' <summary>
    ''' Create <seecref="NewFolding"/>s for the specified document.
    ''' </summary>
    Public Function CreateNewFoldings(ByVal document As ITextSource) As IEnumerable(Of NewFolding)
        Dim newFoldings As List(Of NewFolding) = New List(Of NewFolding)()

        Dim startOffsets As Stack(Of Integer) = New Stack(Of Integer)()
        Dim lastNewLineOffset = 0
        Dim openingBrace = Me.OpeningBrace
        Dim closingBrace = Me.ClosingBrace
        For i As Integer = 0 To document.TextLength - 1
            Dim c As Char = document.GetCharAt(i)
            If c = openingBrace Then
                startOffsets.Push(i)
            ElseIf c = closingBrace AndAlso startOffsets.Count > 0 Then
                Dim startOffset As Integer = startOffsets.Pop()
                ' don't fold if opening and closing brace are on the same line
                If startOffset < lastNewLineOffset Then
                    newFoldings.Add(New NewFolding(startOffset, i + 1))
                End If
            ElseIf c = ChrW(10) OrElse c = ChrW(13) Then
                lastNewLineOffset = i + 1
            End If
        Next
        newFoldings.Sort(Function(a, b) a.StartOffset.CompareTo(b.StartOffset))
        Return newFoldings
    End Function
End Class