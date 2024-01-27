Imports Avalonia
Imports Avalonia.Controls.ApplicationLifetimes
Imports Avalonia.Markup.Xaml

Public Partial Class App
    Inherits Application
    Public Overrides Sub Initialize()
        AvaloniaXamlLoader.Load(Me)
    End Sub

    Public Overrides Sub OnFrameworkInitializationCompleted()
        Dim desktop As IClassicDesktopStyleApplicationLifetime = TryCast(ApplicationLifetime, IClassicDesktopStyleApplicationLifetime)
        If desktop IsNot Nothing Then desktop.MainWindow = New MainWindow()
        MyBase.OnFrameworkInitializationCompleted()
    End Sub
    
End Class
