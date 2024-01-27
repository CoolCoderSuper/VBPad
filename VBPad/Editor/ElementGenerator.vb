Imports Avalonia.Controls
Imports AvaloniaEdit.Rendering

Namespace Editor
    Public Class ElementGenerator
        Inherits VisualLineElementGenerator
        Implements IComparer(Of KeyValuePair(Of Integer,Control))
        Private controls As List(Of KeyValuePair(Of Integer,Control)) = New List(Of KeyValuePair(Of Integer,Control))()

        ''' <summary>
        ''' Gets the first interested offset using binary search
        ''' </summary>
        ''' <returns>The first interested offset.</returns>
        ''' <param name="startOffset">Start offset.</param>
        Public Overrides Function GetFirstInterestedOffset(startOffset As Integer) As Integer
            Dim pos As Integer = controls.BinarySearch(New KeyValuePair(Of Integer,Control)(startOffset, Nothing), Me)
            If pos < 0 Then pos = Not pos
            If pos < controls.Count Then
                Return controls(pos).Key
            Else
                Return -1
            End If
        End Function

        Public Overrides Function ConstructElement(offset As Integer) As VisualLineElement
            Dim pos As Integer = controls.BinarySearch(New KeyValuePair(Of Integer,Control)(offset, Nothing), Me)
            If pos >= 0 Then
                Return New InlineObjectElement(0, controls(pos).Value)
            Else
                Return Nothing
            End If
        End Function

        Private Function Compare(x As KeyValuePair(Of Integer,Control), y As KeyValuePair(Of Integer,Control)) As Integer Implements IComparer(Of KeyValuePair(Of Integer,Control)).Compare
            Return x.Key.CompareTo(y.Key)
        End Function
    End Class
End NameSpace