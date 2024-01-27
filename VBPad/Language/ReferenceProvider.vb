Imports System.IO
Imports Microsoft.CodeAnalysis

Public Class ReferenceProvider
    Public Shared Function GetReferences() As MetadataReference()
        Dim lRefs As New List(Of MetadataReference)
        Dim strPath As String = Path.GetDirectoryName(GetType(Object).Assembly.Location) & Path.DirectorySeparatorChar
        lRefs.Add(MetadataReference.CreateFromFile($"{strPath}System.Private.CoreLib.dll"))
        lRefs.Add(MetadataReference.CreateFromFile($"{strPath}System.Runtime.dll"))
        lRefs.Add(MetadataReference.CreateFromFile($"{strPath}System.Console.dll"))
        lRefs.Add(MetadataReference.CreateFromFile($"{strPath}netstandard.dll"))
        lRefs.Add(MetadataReference.CreateFromFile($"{strPath}System.Text.RegularExpressions.dll"))
        lRefs.Add(MetadataReference.CreateFromFile($"{strPath}System.Linq.dll"))
        lRefs.Add(MetadataReference.CreateFromFile($"{strPath}System.Linq.Expressions.dll"))
        lRefs.Add(MetadataReference.CreateFromFile($"{strPath}System.IO.dll"))
        lRefs.Add(MetadataReference.CreateFromFile($"{strPath}System.Net.Primitives.dll"))
        lRefs.Add(MetadataReference.CreateFromFile($"{strPath}System.Net.Http.dll"))
        lRefs.Add(MetadataReference.CreateFromFile($"{strPath}System.Private.Uri.dll"))
        lRefs.Add(MetadataReference.CreateFromFile($"{strPath}System.Reflection.dll"))
        lRefs.Add(MetadataReference.CreateFromFile($"{strPath}System.ComponentModel.Primitives.dll"))
        lRefs.Add(MetadataReference.CreateFromFile($"{strPath}System.Globalization.dll"))
        lRefs.Add(MetadataReference.CreateFromFile($"{strPath}System.Collections.Concurrent.dll"))
        lRefs.Add(MetadataReference.CreateFromFile($"{strPath}System.Collections.NonGeneric.dll"))
        lRefs.Add(MetadataReference.CreateFromFile($"{strPath}System.Collections.dll"))
        lRefs.Add(MetadataReference.CreateFromFile($"{strPath}System.Xml.XDocument.dll"))
        Return lRefs.ToArray
    End Function
End Class