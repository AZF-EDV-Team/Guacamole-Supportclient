Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Net.IPAddress
Imports MySql
Imports MySql.Data
Imports MySql.Data.MySqlClient
Imports System.Security.Cryptography

Public Class Form2

    Dim sAppPath As String
    Dim WithEvents BGW As New System.ComponentModel.BackgroundWorker With {.WorkerReportsProgress = True, .WorkerSupportsCancellation = True}
    Dim bgwResult As Boolean

    Private Declare Ansi Function WritePrivateProfileString Lib "kernel32" Alias "WritePrivateProfileStringA" ( _
 ByVal lpApplicationName As String, _
 ByVal lpKeyName As String, _
 ByVal lpString As String, _
 ByVal lpFileName As String) _
As Integer

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim dbHost = txtbIP.Text
        Dim dbUser = txtbUser.Text
        Dim dbPass = txtbPass.Text
        Dim dbName = txtbDBName.Text
        Dim sb As StringBuilder

        sb = New StringBuilder(1024)
        sAppPath = Application.StartupPath

        INI.Lesen("DB", "Host", "", sb, sb.Capacity, sAppPath & "\lib\Settings.ini")
        txtbIP.Text = Decrypt(sb.ToString)
        INI.Lesen("DB", "User", "", sb, sb.Capacity, sAppPath & "\lib\Settings.ini")
        txtbUser.Text = Decrypt(sb.ToString)
        INI.Lesen("DB", "Pass", "", sb, sb.Capacity, sAppPath & "\lib\Settings.ini")
        txtbPass.Text = Decrypt(sb.ToString)
        INI.Lesen("DB", "DB_Name", "", sb, sb.Capacity, sAppPath & "\lib\Settings.ini")
        txtbDBName.Text = Decrypt(sb.ToString)

    End Sub

    Private Function Decrypt(cipherText As String) As String
        Dim EncryptionKey As String = "*****"
        Dim cipherBytes As Byte() = Convert.FromBase64String(cipherText)
        Using encryptor As Aes = Aes.Create()
            Dim pdb As New Rfc2898DeriveBytes(EncryptionKey, New Byte() {&H49, &H76, &H61, &H6E, &H20, &H4D, _
             &H65, &H64, &H76, &H65, &H64, &H65, _
             &H76})
            encryptor.Key = pdb.GetBytes(32)
            encryptor.IV = pdb.GetBytes(16)
            Using ms As New MemoryStream()
                Using cs As New CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write)
                    cs.Write(cipherBytes, 0, cipherBytes.Length)
                    cs.Close()
                End Using
                cipherText = Encoding.Unicode.GetString(ms.ToArray())
            End Using
        End Using
        Return cipherText
    End Function

    Private Function Encrypt(clearText As String) As String
        Dim EncryptionKey As String = "*****"
        Dim clearBytes As Byte() = Encoding.Unicode.GetBytes(clearText)
        Using encryptor As Aes = Aes.Create()
            Dim pdb As New Rfc2898DeriveBytes(EncryptionKey, New Byte() {&H49, &H76, &H61, &H6E, &H20, &H4D, _
             &H65, &H64, &H76, &H65, &H64, &H65, _
             &H76})
            encryptor.Key = pdb.GetBytes(32)
            encryptor.IV = pdb.GetBytes(16)
            Using ms As New MemoryStream()
                Using cs As New CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write)
                    cs.Write(clearBytes, 0, clearBytes.Length)
                    cs.Close()
                End Using
                clearText = Convert.ToBase64String(ms.ToArray())
            End Using
        End Using
        Return clearText
    End Function

    Private Function write() As Boolean
        Dim dbHost = Encrypt(txtbIP.Text)
        Dim dbUser = Encrypt(txtbUser.Text)
        Dim dbPass = Encrypt(txtbPass.Text)
        Dim dbName = Encrypt(txtbDBName.Text)

        WritePrivateProfileString("DB", "Host", dbHost, sAppPath & "\lib\Settings.ini")
        WritePrivateProfileString("DB", "User", dbUser, sAppPath & "\lib\Settings.ini")
        WritePrivateProfileString("DB", "Pass", dbPass, sAppPath & "\lib\Settings.ini")
        WritePrivateProfileString("DB", "DB_Name", dbName, sAppPath & "\lib\Settings.ini")

    End Function

    Public Function checkDB() As Boolean
        If Not BGW.IsBusy Then
            lblBGW.Text = "Bitte warten"
            timLbl.Start()
            btnSave.Enabled = False
            btnCancel.Enabled = False
            BGW.RunWorkerAsync()
            While BGW.IsBusy
                Windows.Forms.Application.DoEvents()
            End While

            If bgwResult = True Then
                btnSave.Enabled = True
                'btnCancel.Enabled = True
                timLbl.Stop()
                lblBGW.Text = ""
                Return True
            Else
                Return False
            End If

        End If
    End Function

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        If System.Net.IPAddress.TryParse(txtbIP.Text, Nothing) Then
            If txtbIP.TextLength > 0 And txtbUser.TextLength > 0 And txtbPass.TextLength > 0 And txtbDBName.TextLength > 0 Then
                If checkDB() = True Then
                    write()
                    MessageBox.Show("Verbindung Hergestellt!")
                    Me.Close()
                    Form1.Show()

                End If
            End If
        Else
            MessageBox.Show("Keine gültige IP eingetragen..." & vbCrLf)
            Form1.btnOpen.Enabled = False
            Me.Close()
            Form1.Show()
        End If

    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
        Form1.Show()
    End Sub

    Private Sub Form2_close(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If Form1.Lesen() = True Then

            Form1.dbLesen()
            Form1.btnOpen.Enabled = True
        Else
            Form1.ComboBox1.Items.Clear()
            Form1.ComboBox1.Text = "Nicht Konfiguriert"
            Form1.btnOpen.Enabled = False
        End If

        Form1.Show()
    End Sub



    Private Sub txtbIP_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtbIP.KeyPress

        e.Handled = Not (Char.IsDigit(e.KeyChar) Or e.KeyChar = "." Or e.KeyChar = Convert.ToChar(Keys.Back))

    End Sub

    Private Sub txtbIP_LostFocus(sender As Object, e As EventArgs) Handles txtbIP.LostFocus
        If System.Net.IPAddress.TryParse(txtbIP.Text, Nothing) Then
            txtbIP.BackColor = Color.LightGreen
        Else

            txtbIP.BackColor = Color.Red
        End If
    End Sub

    Private Sub BGW_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BGW.DoWork
        Dim conn As New MySqlConnection
        Dim myConnString As String
        Dim dbHost = txtbIP.Text
        Dim dbUser = txtbUser.Text
        Dim dbPass = txtbPass.Text
        Dim dbName = txtbDBName.Text
        Dim result As Boolean


        myConnString = "server=" & dbHost & ";uid=" & dbUser & ";pwd=" & dbPass & ";database=" & dbName & "; "
        conn.ConnectionString = myConnString
        Try
            conn.Open()
            conn.Close()
            result = True
        Catch ex As Exception
            MessageBox.Show("Es ist ein Fehler aufgetreten:" & vbCrLf & ex.Message)
            conn.Dispose()
            result = False
        End Try
        e.Result = result
    End Sub

    Private Sub timLbl_Tick(sender As Object, e As EventArgs) Handles timLbl.Tick
        Select Case lblBGW.Text
            Case "Bitte warten."
                lblBGW.Text = "Bitte warten.."
            Case "Bitte warten.."
                lblBGW.Text = "Bitte warten..."
            Case "Bitte warten..."
                lblBGW.Text = "Bitte warten"
            Case "Bitte warten"
                lblBGW.Text = "Bitte warten."

        End Select
    End Sub

    Private Sub BGW_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BGW.RunWorkerCompleted
        timLbl.Stop()

        If e.Result = True Then
            bgwResult = True
        Else
            bgwResult = False
        End If

    End Sub
End Class

Public Class INI
    <DllImport("kernel32", EntryPoint:="GetPrivateProfileString")> _
    Shared Function Lesen( _
    ByVal Sektion As String, ByVal Key As String, ByVal StandartVal As String, _
    ByVal Result As StringBuilder, ByVal Size As Int32, ByVal Dateiname As String) As Int32

    End Function
End Class