Imports System.Data.OleDb
Public Class frmLogin
    Const WM_NCHITTEST As Integer = &H84
    Const HTCLIENT As Integer = &H1
    Const HTCAPTION As Integer = &H2
    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        Select Case m.Msg
            Case WM_NCHITTEST
                MyBase.WndProc(m)
                If m.Result = IntPtr.op_Explicit(HTCLIENT) Then m.Result = IntPtr.op_Explicit(HTCAPTION)
            Case Else
                MyBase.WndProc(m)
        End Select
    End Sub
    Private Sub frmLogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtUsername.Focus()
    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles txtPassword.TextChanged

    End Sub

    Private Sub TextBox2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtPassword.KeyPress
        If e.KeyChar = Chr(13) Then Call Button1_Click(sender, e)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If txtUsername.Text = "" And txtPassword.Text = "" Then
            MsgBox("Username/password tidak boleh kosong.", vbExclamation, "Info")
        Else
            Call Koneksi()
            cmd = New OleDbCommand("select * from tbl_administrator where username='" & txtUsername.Text & "' and password='" & txtPassword.Text & "'", Conn)
            rd = cmd.ExecuteReader
            rd.Read()
            If rd.HasRows Then
                Me.Hide()
                frmMain.Show()
                frmMain.lblUser.Text = rd.Item("nama_admin")
            Else
                MsgBox("Username/password salah.", vbExclamation, "Info")
            End If
        End If
    End Sub
End Class