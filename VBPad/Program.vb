Imports Avalonia

Public Module Program
    Public Sub Main(args As String())
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args)
    End Sub

    Public Function BuildAvaloniaApp() As AppBuilder
        Return AppBuilder.Configure(Of App)().UsePlatformDetect().LogToTrace()
    End Function
End Module

'Imports System
'Public Class Test
'    Public Property Test As String = "Hello"
'    Private _testF As Integer
'    
'    '''<summary>
'    ''' test
'    ''' </summary>
'    ''' <param name="p"></param>
'    Public Sub testsub(p As String)
'        Test = "Hello"
'        _testf = 23
'        Console.WriteLine($"Hello{_testF}")
'        Dim array = {
'                        "Hello",
'                        "Bye"
'                    }
'    End Sub
'End Class
''nice stuff
'Public Module Other
'
'End Module