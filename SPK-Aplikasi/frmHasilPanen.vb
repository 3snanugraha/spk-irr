Imports System.Data.OleDb
Public Class frmHasilPanen
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
    Private Conn As OleDbConnection = Nothing
    Private cmd As OleDbCommand = Nothing
    Private sql As String = Nothing
    Private reader As OleDbDataReader = Nothing
    Private da As OleDbDataAdapter = Nothing
    Function Connect()
        If Not Conn Is Nothing Then
            Conn.Close()
        End If
        Conn.Open()
        Return Conn
    End Function
    Function Closedd()
        Conn.Close()
        Return Conn
    End Function
    Sub Tampil()
        Connect()
        Try
            Dim dt As New DataTable
            da = New OleDbDataAdapter("select * from tbl_hasilpanen", Conn)
            da.Fill(dt)
            DataGridView1.DataSource = dt
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
        Closedd()
    End Sub
    Sub LoadData()
        Connect()
        sql = "select id_panen from tbl_hasilpanen"
        cmd = New OleDbCommand(sql, Conn)
        reader = cmd.ExecuteReader()
        Try
            cboID.Items.Clear()
            While reader.Read
                cboID.Items.Add(reader.GetString(0))
            End While
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
        Closedd()
    End Sub
    Sub LoadData_kapiing()
        Connect()
        sql = "select id_kapiing from tbl_modalawal"
        cmd = New OleDbCommand(sql, Conn)
        reader = cmd.ExecuteReader()
        Try
            cboIDKapiing.Items.Clear()
            While reader.Read
                cboIDKapiing.Items.Add(reader.GetString(0))
            End While
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
        Closedd()
    End Sub
    Sub koneksikan()
        Dim ConnString As String
        ConnString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Application.StartupPath & "\db_fauzan.accdb"
        Try
            Conn = New OleDbConnection(ConnString)
            Conn.Open()
            LoadData()
            LoadData_kapiing()
            Tampil()
            Conn.Close()
        Catch ex As Exception
            MessageBox.Show("Koneksi Error : " + ex.Message)
        End Try
    End Sub
    Sub nomoran()
        koneksikan()
        Connect()
        cmd = New OleDbCommand("select * from tbl_hasilpanen order by id_panen desc", Conn)
        reader = cmd.ExecuteReader
        reader.Read()
        If Not reader.HasRows Then
            lblID.Text = "PN" + "0001"
        Else
            lblID.Text = Val(Microsoft.VisualBasic.Mid(reader.Item("id_panen").ToString, 4, 3)) + 1
            If Len(lblID.Text) = 1 Then
                lblID.Text = "PN000" & lblID.Text & ""
            ElseIf Len(lblID.Text) = 2 Then
                lblID.Text = "PN00" & lblID.Text & ""
            ElseIf Len(lblID.Text) = 3 Then
                lblID.Text = "PN0" & lblID.Text & ""
            End If
        End If
        Me.Focus()
    End Sub
    Sub pembersih()
        cboID.Text = "Pilih ID Panen :"
        cboIDKapiing.Text = "Pilih ID Kapiing :"
        txtHarga.Clear()
        txtHasilPenjualan.Text = "0"
        txtJumlah.Value = "1"
    End Sub
    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click
        Me.Close()
    End Sub

    Private Sub frmHasilPanen_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        nomoran()
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles txtHarga.TextChanged
        txtHasilPenjualan.Text = Val(txtHarga.Text) * Val(txtJumlah.Value)
    End Sub

    Private Sub DateTimePicker1_ValueChanged(sender As Object, e As EventArgs) Handles dtpTanggalPanen.ValueChanged

    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Connect()
        Dim insertquery As String
        Dim Hasil As Integer
        Dim cmd As OleDbCommand
        insertquery = ("insert into tbl_hasilpanen(id_panen,id_kapiing,tanggal_panen,jumlah,harga,hasil_penjualan)Values('" & lblID.Text & "','" & cboIDKapiing.Text & "','" & dtpTanggalPanen.Value & "','" & txtJumlah.Value & "','" & txtHarga.Text & "','" & txtHasilPenjualan.Text & "')")
        Try
            cmd = New OleDbCommand(insertquery, Conn)
            Hasil = cmd.ExecuteNonQuery
            If Hasil > 0 Then
                MessageBox.Show("Data berhasil dimasukan.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information)
                nomoran()
                pembersih()
            End If
        Catch ex As OleDbException
            MessageBox.Show("Failed : " & ex.Message, "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Closedd()
    End Sub
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Connect()
        Dim index As Integer = cboID.SelectedIndex
        Dim hasil As Integer
        Dim pesan As DialogResult
        sql = "delete from tbl_hasilpanen where id_panen='" & cboID.Text & "'"
        pesan = MessageBox.Show("Hapus data " & Chr(10) & "dengan ID : " & cboID.Text & " ...?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        cmd = New OleDbCommand(sql, Conn)
        Try
            If pesan = DialogResult.Yes = True Then
                hasil = cmd.ExecuteNonQuery
                nomoran()
                pembersih()
            End If
        Catch ex As OleDbException
            MsgBox("Failed : " & ex.Message)
        End Try
        Closedd()
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Connect()
        Dim hasil As Integer
        sql = "Update tbl_hasilpanen set id_kapiing='" & cboIDKapiing.Text & "',tanggal_panen='" & dtpTanggalPanen.Value & "', jumlah='" & txtJumlah.Value & "', harga='" & txtHarga.Text & "' where id_panen='" & cboID.Text & "'"
        cmd = New OleDbCommand(sql, Conn)
        Try
            hasil = cmd.ExecuteNonQuery
            If (hasil > 0) Then
                MessageBox.Show("Data berhasil diedit.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information)
                nomoran()
                pembersih()
            End If
        Catch ex As OleDbException
            MessageBox.Show("Failed : " & ex.Message)
        End Try
        Closedd()
    End Sub
    Private Sub txtJumlah_ValueChanged(sender As Object, e As EventArgs) Handles txtJumlah.ValueChanged
        txtHasilPenjualan.Text = Val(txtHarga.Text) * Val(txtJumlah.Value)
    End Sub
    Private Sub txtJumlah_TextChanged(sender As Object, e As EventArgs) Handles txtJumlah.TextChanged
        txtHasilPenjualan.Text = Val(txtHarga.Text) * Val(txtJumlah.Value)
    End Sub
    Private Sub cboID_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboID.SelectedIndexChanged
        Connect()
        Dim Kunci As String = cboID.Text
        sql = "select * from tbl_hasilpanen where id_panen='" & Kunci & "'"
        cmd = New OleDbCommand(sql, Conn)
        reader = cmd.ExecuteReader
        Try
            reader.Read()
            cboIDKapiing.Text = reader.Item("id_kapiing")
            dtpTanggalPanen.Value = reader.Item("tanggal_panen")
            txtJumlah.Text = reader.Item("jumlah")
            txtHarga.Text = reader.Item("harga")
            txtHasilPenjualan.Text = reader.Item("hasil_penjualan")
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
        Closedd()
    End Sub
End Class