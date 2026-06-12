Imports System.IO
Imports DWSIM.GlobalSettings
Imports Python.Runtime

Public Class ReaktoroLoader

    Public Shared Function Initialize() As String

        Dim assemblyPath = Path.GetDirectoryName(Reflection.Assembly.GetExecutingAssembly().Location)

        If Settings.RunningPlatform() = Settings.Platform.Windows Then

            Dim pyver = PythonEngine.Version

            Dim libpath = "", dllpath, shareddllpath As String

            If pyver.Contains("3.7.") Then
                libpath = Path.Combine(assemblyPath, "python_packages", "reaktoro_py37")
            ElseIf pyver.Contains("3.8.") Then
                libpath = Path.Combine(assemblyPath, "python_packages", "reaktoro_py38")
            ElseIf pyver.Contains("3.9.") Then
                libpath = Path.Combine(assemblyPath, "python_packages", "reaktoro_py39")
            Else
                Throw New Exception("Reaktoro requires a Python distribution version between 3.7 and 3.9 (inclusive). Found version " & pyver)
            End If

            dllpath = Path.Combine(libpath, "reaktoro")
            shareddllpath = Path.Combine(assemblyPath, "python_packages", "reaktoro_shared")

            Dim append As String = Settings.PythonPath + ";" + Path.Combine(Settings.PythonPath, "Library", "bin") +
                    ";" + dllpath + ";" + shareddllpath + ";"
            Dim p1 As String = append + Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Machine)
            Environment.SetEnvironmentVariable("PATH", p1, EnvironmentVariableTarget.Process)

            LoadReaktoroLib(Path.Combine(dllpath, "Reaktoro.dll"))

            Return libpath

        Else

            Dim pyver = PythonEngine.Version
            Dim candidates As New List(Of String)

            If pyver.Contains("3.7.") Then candidates.Add(Path.Combine(assemblyPath, "python_packages", "reaktoro_py37"))
            If pyver.Contains("3.8.") Then candidates.Add(Path.Combine(assemblyPath, "python_packages", "reaktoro_py38"))
            If pyver.Contains("3.9.") Then candidates.Add(Path.Combine(assemblyPath, "python_packages", "reaktoro_py39"))

            For Each libpath In candidates
                If Directory.Exists(Path.Combine(libpath, "reaktoro")) Then Return libpath
            Next

            Return ""

        End If

    End Function

    Private Declare Function LoadLibrary Lib "kernel32.dll" Alias "LoadLibraryA" (ByVal lpFileName As String) As IntPtr

    Private Shared Sub LoadReaktoroLib(dllpath As String)

        ' Load the library
        Dim res = LoadLibrary(dllpath)

        If (res = IntPtr.Zero) Then
            Throw New Exception("Failed to load Reaktoro DLL")
        End If

    End Sub

End Class
