Imports System.Data.OleDb
Public Class frmModalAwal
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
            da = New OleDbDataAdapter("select * from tbl_modalawal", Conn)
            da.Fill(dt)
            DataGridView1.DataSource = dt
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
        Closedd()
    End Sub
    Sub LoadData()
        Connect()
        sql = "select id_kapiing from tbl_modalawal"
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
    Sub koneksikan()
        Dim ConnString As String
        ConnString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Application.StartupPath & "\db_fauzan.accdb"
        Try
            Conn = New OleDbConnection(ConnString)
            Conn.Open()
            LoadData()
            Tampil()
            Conn.Close()
        Catch ex As Exception
            MessageBox.Show("Koneksi Error : " + ex.Message)
        End Try
    End Sub
    Sub nomoran()
        koneksikan()
        Connect()
        cmd = New OleDbCommand("select * from tbl_modalawal order by id_kapiing desc", Conn)
        reader = cmd.ExecuteReader
        reader.Read()
        If Not reader.HasRows Then
            lblID.Text = "MA" + "0001"
        Else
            lblID.Text = Val(Microsoft.VisualBasic.Mid(reader.Item("id_kapiing").ToString, 4, 3)) + 1
            If Len(lblID.Text) = 1 Then
                lblID.Text = "MA000" & lblID.Text & ""
            ElseIf Len(lblID.Text) = 2 Then
                lblID.Text = "MA00" & lblID.Text & ""
            ElseIf Len(lblID.Text) = 3 Then
                lblID.Text = "MA0" & lblID.Text & ""
            End If
        End If
        Me.Focus()
    End Sub
    Sub pembersih()
        cboID.Text = "Pilih ID Kapiing :"
        txtJumlahBibit.Value = "1"
        txtBiayaTanam.Clear()
        txtHargaBibit.Clear()
        txtHargaBibit.Clear()
        txtTotalBiayaTanam.Text = "0"
        txtTotalSemua.Text = "0"
    End Sub
    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click
        Me.Close()
    End Sub
    Private Sub frmModalAwal_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        nomoran()
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Connect()
        Dim insertquery As String
        Dim Hasil As Integer
        Dim cmd As OleDbCommand
        insertquery = ("insert into tbl_modalawal(id_kapiing,tanggal_penanaman,jumlah_bibit,harga_bibit,total_harga_bibit,biaya_tanam,total_biaya_tanam,total_semua)Values('" & lblID.Text & "','" & dtpPenanaman.Value & "','" & txtJumlahBibit.Text & "','" & txtHargaBibit.Text & "','" & txtTotalHargaBibit.Text & "','" & txtBiayaTanam.Text & "','" & txtTotalBiayaTanam.Text & "','" & txtTotalSemua.Text & "')")
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
    Private Sub cboID_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboID.SelectedIndexChanged
        Connect()
        Dim Kunci As String = cboID.Text
        sql = "select * from tbl_modalawal where id_kapiing='" & Kunci & "'"
        cmd = New OleDbCommand(sql, Conn)
        reader = cmd.ExecuteReader
        Try
            reader.Read()
            dtpPenanaman.Value = reader.Item("tanggal_penanaman")
            txtJumlahBibit.Value = reader.Item("jumlah_bibit")
            txtHargaBibit.Text = reader.Item("harga_bibit")
            txtTotalHargaBibit.Text = reader.Item("total_harga_bibit")
            txtBiayaTanam.Text = reader.Item("biaya_tanam")
            txtTotalBiayaTanam.Text = reader.Item("total_biaya_tanam")
            txtTotalSemua.Text = reader.Item("total_semua")
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
        Closedd()
    End Sub

    Private Sub txtHargaBibit_TextChanged(sender As Object, e As EventArgs) Handles txtHargaBibit.TextChanged
        txtTotalHargaBibit.Text = Val(txtJumlahBibit.Value) * Val(txtHargaBibit.Text)
        txtTotalSemua.Text = Val(txtTotalHargaBibit.Text) + Val(txtTotalBiayaTanam.Text)
    End Sub

    Private Sub txtJumlahBibit_ValueChanged(sender As Object, e As EventArgs) Handles txtJumlahBibit.ValueChanged
        txtTotalHargaBibit.Text = Val(txtJumlahBibit.Value) * Val(txtHargaBibit.Text)
        txtTotalSemua.Text = Val(txtTotalHargaBibit.Text) + Val(txtTotalBiayaTanam.Text)
    End Sub

    Private Sub txtJumlahBibit_TextChanged(sender As Object, e As EventArgs) Handles txtJumlahBibit.TextChanged
        txtTotalHargaBibit.Text = Val(txtJumlahBibit.Value) * Val(txtHargaBibit.Text)
    End Sub

    Private Sub txtBiayaTanam_TextChanged(sender As Object, e As EventArgs) Handles txtBiayaTanam.TextChanged
        txtTotalBiayaTanam.Text = txtBiayaTanam.Text
        txtTotalSemua.Text = Val(txtTotalHargaBibit.Text) + Val(txtTotalBiayaTanam.Text)
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Connect()
        Dim index As Integer = cboID.SelectedIndex
        Dim hasil As Integer
        Dim pesan As DialogResult
        sql = "delete from tbl_modalawal where id_kapiing='" & cboID.Text & "'"
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

    End Sub
End Class