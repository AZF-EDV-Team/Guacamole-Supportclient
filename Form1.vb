Imports MySql
Imports MySql.Data
Imports MySql.Data.MySqlClient
Imports System.Diagnostics.Stopwatch
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Security.Cryptography


Public Class Form1

    Dim conn As New MySqlConnection
    Dim myConnString As String
    Dim cmd As New MySqlCommand
    Dim dr As MySqlDataReader
    Dim id As Integer
    Dim uid As String
    Dim dbIP As String
    Dim dbUser As String
    Dim dbPass As String
    Dim dbName As String
    Dim sAppPath As String = Application.StartupPath


    

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


    Private Function insert() As Boolean
        Dim conn As New MySqlConnection
        Dim myConnString As String
        Dim cmd As New MySqlCommand
        Dim dr As MySqlDataReader
        Dim id As Integer
        Dim uid As String
        rtbLog.AppendText(Now.Date & " " & Now.ToShortTimeString & ": Connection String setzen..." & vbCrLf)

        myConnString = "server=" & dbIP & ";uid=" & dbUser & ";pwd=" & dbPass & ";database=" & dbName & "; "
        conn.ConnectionString = myConnString
        rtbLog.AppendText(Now.Date & " " & Now.ToShortTimeString & ": Connection String gesetzt!" & vbCrLf)
        Try
            rtbLog.AppendText(Now.Date & " " & Now.ToShortTimeString & ": Datenbank Verbindung herstellen..." & vbCrLf)
            conn.Open()          
            rtbLog.AppendText(Now.Date & " " & Now.ToShortTimeString & ": Verbindung hergestellt!" & vbCrLf)
            rtbLog.AppendText(Now.Date & " " & Now.ToShortTimeString & ": Erstelle Connection..." & vbCrLf)
            cmd.CommandText = "INSERT INTO `guacamole`.`guacamole_connection` (`connection_name`, `protocol`) VALUES ('" & Environment.MachineName & "_" & Environment.UserName & "', 'vnc');"
            cmd = New MySqlCommand(cmd.CommandText, conn)
            cmd.ExecuteNonQuery()
            rtbLog.AppendText(Now.Date & " " & Now.ToShortTimeString & ": Connection erstellt!" & vbCrLf)
            rtbLog.AppendText(Now.Date & " " & Now.ToShortTimeString & ": Lese Connection ID..." & vbCrLf)

            cmd.CommandText = "SELECT LAST_INSERT_ID()"
            dr = cmd.ExecuteReader
            dr.Read()
            id = dr(0)
            dr.Close()
            rtbLog.AppendText(Now.Date & " " & Now.ToShortTimeString & ": Connection ID gelesen!" & vbCrLf & "Connection ID:" & id & vbCrLf)
            rtbLog.AppendText(Now.Date & " " & Now.ToShortTimeString & ": Setze Connection Parameter..." & vbCrLf)
            cmd.CommandText = "INSERT INTO `guacamole`.`guacamole_connection_parameter` (`connection_id`, `parameter_name`, `parameter_value`) VALUES ('" & id & "', 'hostname', '" & GetIPAddress() & "');"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "INSERT INTO `guacamole`.`guacamole_connection_parameter` (`connection_id`, `parameter_name`, `parameter_value`) VALUES ('" & id & "', 'password', 'schnuffi');"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "INSERT INTO `guacamole`.`guacamole_connection_parameter` (`connection_id`, `parameter_name`, `parameter_value`) VALUES ('" & id & "', 'port', '5900');"
            cmd.ExecuteNonQuery()
            rtbLog.AppendText(Now.Date & " " & Now.ToShortTimeString & ": Parameter gesetzt!" & vbCrLf)
            If Not chkbox.Checked = True Then
                rtbLog.AppendText(Now.Date & " " & Now.ToShortTimeString & ": Fernsteuerung verweigern..." & vbCrLf)
                cmd.CommandText = "INSERT INTO `guacamole`.`guacamole_connection_parameter` (`connection_id`, `parameter_name`, `parameter_value`) VALUES ('" & id & "', 'read-only', 'true');"
                rtbLog.AppendText(Now.Date & " " & Now.ToShortTimeString & ": Fernsteuerung nicht möglich!" & vbCrLf)
            Else
                rtbLog.AppendText(Now.Date & " " & Now.ToShortTimeString & ": Fernsteuerung zugelassen!" & vbCrLf)
            End If
            rtbLog.AppendText(Now.Date & " " & Now.ToShortTimeString & ": Lese UserID des Support Mitarbeiters..." & vbCrLf)
            cmd.CommandText = "SELECT user_id from `guacamole`.`guacamole_user` where username='" & ComboBox1.SelectedItem.ToString & "'"
            dr = cmd.ExecuteReader
            dr.Read()
            uid = dr(0)
            dr.Close()
            rtbLog.AppendText(Now.Date & " " & Now.ToShortTimeString & ": UserID gelesen!" & vbCrLf)
            rtbLog.AppendText(Now.Date & " " & Now.ToShortTimeString & ": Veröffentliche Connection für gewählten Support Mitarbeiter..." & vbCrLf)
            cmd.CommandText = "INSERT INTO `guacamole`.`guacamole_connection_permission` (`user_id`, `connection_id`, `permission`) VALUES ('" & uid & "', '" & id & "', 'READ');"
            cmd.ExecuteNonQuery()
            rtbLog.AppendText(Now.Date & " " & Now.ToShortTimeString & ": Connection Veröffentlicht!" & vbCrLf)

            rtbLog.AppendText(vbCrLf)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            rtbLog.AppendText(Now.Date & " " & Now.ToShortTimeString & vbCrLf & ": Es Ist ein Fehler aufgetreten:" & vbCrLf & ex.Message & vbCrLf)

            conn.Dispose()
            rtbLog.AppendText(Now.Date & " " & Now.ToShortTimeString & ": Datenbank Verbindung geschlossen!" & vbCrLf)
            Return False
        End Try
        conn.Close()
        rtbLog.AppendText(Now.Date & " " & Now.ToShortTimeString & ": Datenbank Verbindung geschlossen!" & vbCrLf)
        Return True
    End Function

    Private Sub btnOpen_Click(sender As Object, e As EventArgs) Handles btnOpen.Click
        Dim insWatch As New Stopwatch
        Dim insTime As Double
        rtbLog.Clear()
        insWatch.Reset()
        insWatch.Start()
        If ComboBox1.SelectedIndex = -1 Then
            MessageBox.Show("Bitte wählen sie den Support Mitarbeiter...")
            Exit Sub
        End If

        rtbLog.AppendText(Now.Date & " " & Now.ToShortTimeString & ": VNC wird gestartet..." & vbCrLf)
        Try

            Process.Start(Application.StartupPath & "\lib\winvnc.exe")

        Catch ex As Exception
            rtbLog.AppendText(Now.Date & " " & Now.ToShortTimeString & ": VNC konnte nicht gestartet werden: " & vbCrLf & ex.Message)
        End Try

        For Each proc In System.Diagnostics.Process.GetProcessesByName("winvnc")
            rtbLog.AppendText(Now.Date & " " & Now.ToShortTimeString & ": VNC Gestartet!" & vbCrLf)
            Exit For
        Next
        myConnString = "server=" & dbIP & ";uid=" & dbUser & ";pwd=" & dbPass & ";database=" & dbName & "; "
        conn.ConnectionString = myConnString
        insert()
        btnClose.Enabled = True
        btnOpen.Enabled = False
        insWatch.Stop()
        insTime = insWatch.Elapsed.TotalMilliseconds
        rtbLog.AppendText("Verstrichene Zeit: " & insTime & " ms" & vbCrLf)
    End Sub

    Private Function GetIPAddress()
        Dim strHostName As String

        Dim strIPAddress As String



        strHostName = System.Net.Dns.GetHostName()

        strIPAddress = System.Net.Dns.GetHostByName(strHostName).AddressList(0).ToString()
        Return strIPAddress

    End Function

    Private Function onClose() As Boolean
        rtbLog.AppendText(Now.Date & " " & Now.ToShortTimeString & ": VNC Wird beendet..." & vbCrLf)
        For Each proc In System.Diagnostics.Process.GetProcessesByName("winvnc")
            Try
                proc.Kill()
            Catch ex As Exception
                MessageBox.Show(ex.Message)
                rtbLog.AppendText(Now.Date & " " & Now.ToShortTimeString & vbCrLf & ": Es Ist ein Fehler aufgetreten:" & vbCrLf & ex.Message & vbCrLf)
            End Try
        Next
        rtbLog.AppendText(Now.Date & " " & Now.ToShortTimeString & ": VNC wurde beendet!" & vbCrLf)

        rtbLog.AppendText(Now.Date & " " & Now.ToShortTimeString & ": Connection String setzen..." & vbCrLf)
        conn.ConnectionString = myConnString
        rtbLog.AppendText(Now.Date & " " & Now.ToShortTimeString & ": Connection String gesetzt!" & vbCrLf)

        Try
            rtbLog.AppendText(Now.Date & " " & Now.ToShortTimeString & ": Datenbank Verbindung herstellen..." & vbCrLf)
            conn.Open()
            rtbLog.AppendText(Now.Date & " " & Now.ToShortTimeString & ": Datenbank Verbindung aufgebaut!" & vbCrLf)
            rtbLog.AppendText(Now.Date & " " & Now.ToShortTimeString & ": Lösche Connection..." & vbCrLf)
            cmd.CommandText = "DELETE from `guacamole`.`guacamole_connection` where connection_name='" & Environment.MachineName & "_" & Environment.UserName & "'; DELETE from `guacamole`.`guacamole_connection_parameter` where connection_id='" & id & "'; DELETE from `guacamole`.`guacamole_connection_permission` where connection_id='" & id & "'"
            cmd = New MySqlCommand(cmd.CommandText, conn)
            cmd.ExecuteNonQuery()
            rtbLog.AppendText(Now.Date & " " & Now.ToShortTimeString & ": Connection gelöscht!" & vbCrLf)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            rtbLog.AppendText(Now.Date & " " & Now.ToShortTimeString & vbCrLf & ": Es Ist ein Fehler aufgetreten:" & vbCrLf & ex.Message & vbCrLf)
            conn.Dispose()
            Return False
        End Try

        conn.Close()
        rtbLog.AppendText(Now.Date & " " & Now.ToShortTimeString & ": Datenbank Verbindung geschlossen" & vbCrLf)
        Return True
    End Function

    Private Sub Form1_close(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If Not MessageBox.Show("Wollen sie das Programm wirklich beenden?", "Beenden", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then

            e.Cancel = True
        Else
            If btnClose.Enabled = True Then
                onClose()
            End If

        End If

    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Dim closeWatch As New Stopwatch
        Dim closeTime As Double
        closeWatch.Reset()
        closeWatch.Start()
        onClose()
        closeWatch.Stop()
        closeTime = closeWatch.Elapsed.TotalMilliseconds
        rtbLog.AppendText("Verstrichene Zeit: " & closeTime & " ms" & vbCrLf)
        btnOpen.Enabled = True
        btnClose.Enabled = False
    End Sub

    Public Function Lesen() As Boolean
        Dim sb As StringBuilder

        sb = New StringBuilder(1024)
        sAppPath = Application.StartupPath

        INI.Lesen("DB", "Host", "", sb, sb.Capacity, sAppPath & "\lib\Settings.ini")
        dbIP = Decrypt(sb.ToString)
        INI.Lesen("DB", "User", "", sb, sb.Capacity, sAppPath & "\lib\Settings.ini")
        dbUser = Decrypt(sb.ToString)
        INI.Lesen("DB", "Pass", "", sb, sb.Capacity, sAppPath & "\lib\Settings.ini")
        dbPass = Decrypt(sb.ToString)
        INI.Lesen("DB", "DB_Name", "", sb, sb.Capacity, sAppPath & "\lib\Settings.ini")
        dbName = Decrypt(sb.ToString)
        If dbIP.Length > 0 And dbUser.Length > 0 And dbPass.Length > 0 And dbName.Length > 0 Then
            Return True
        Else
            Return False
        End If

    End Function
    Public Function dbLesen() As Boolean
        myConnString = "server=" & dbIP & ";uid=" & dbUser & ";pwd=" & dbPass & ";database=" & dbName & "; "
        conn.ConnectionString = myConnString
        Try
            conn.Open()
            cmd.CommandText = "SELECT username from `guacamole`.`guacamole_user` "
            cmd = New MySqlCommand(cmd.CommandText, conn)
            dr = cmd.ExecuteReader()
            ComboBox1.Items.Clear()
            While dr.Read
                ComboBox1.Items.Add(dr("username"))
            End While
            dr.Close()
            ComboBox1.Text = "Supporter Wählen..."
            conn.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message)

            conn.Close()
        End Try
        Return True
    End Function
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        If Lesen() = False Then
            btnOpen.Enabled = False
            ComboBox1.Text = "Nicht Konfiguriert..."

        Else
            btnOpen.Enabled = True
            myConnString = "server=" & dbIP & ";uid=" & dbUser & ";pwd=" & dbPass & ";database=" & dbName & "; "
            conn.ConnectionString = myConnString
            If Form2.checkDB() = True Then
                dbLesen()
            Else
                ComboBox1.Enabled = False
                ComboBox1.Text = "Nicht Konfiguriert..."
            End If

        End If


    End Sub

    

    Private Sub rtbLog_TextChanged(sender As Object, e As EventArgs) Handles rtbLog.TextChanged
        rtbLog.SelectionStart = rtbLog.Text.Length
        rtbLog.ScrollToCaret()
    End Sub

    Private Sub btnSettings_Click(sender As Object, e As EventArgs) Handles btnSettings.Click
        Me.Hide()
        form2.show()
    End Sub
End Class
