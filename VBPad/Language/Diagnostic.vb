Public Class Diagnostic
    Public Property Severity As Severity
    Public Property ID As String
    Public Property Message As String
    Public Property Location As Location
End Class

Public Enum Severity
    [Error]
    Warning
End Enum
