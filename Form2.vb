Public Class Form2
    Dim tbaux As ADODB.Recordset
    Dim sql As String
    Dim imagem As Image
    Dim wcpagina As Integer

    Private Sub Form2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub ComboBox1_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboClientes.GotFocus
        cboClientes.Items.Clear()
        sql = "Select distinct nomecliente from vwExemploAula order by nomecliente"
        tbaux = OpenRecordset(sql)
        If tbaux.RecordCount <> 0 Then
            tbaux.MoveFirst()
            While tbaux.EOF = False
                cboClientes.Items.Add(tbaux.Fields("nomecliente").Value.ToString)
                tbaux.MoveNext()
            End While
        End If
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboClientes.SelectedIndexChanged

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        If cboClientes.Text = "" Then
            MsgBox("Escolha um paciente !")
            cboClientes.Focus()
            Exit Sub
        End If
        sql = "Select * from vwExemploAula where nomecliente = '" & cboClientes.Text & "'"
        tbaux = OpenRecordset(sql)
        If tbaux.RecordCount = 0 Then
            MsgBox("Nenhuma consulta marcada para o cliente !")
            Exit Sub
        End If
        PrintPreviewDialog1.ShowDialog()
    End Sub

    Private Sub PrintPreviewDialog1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PrintPreviewDialog1.Load
        wcpagina = 1
    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        Dim MYFONT As New Font("ARIAL", 8)
        Dim MYFONT3 As New Font("ARIAL", 10, FontStyle.Bold)
        Dim MYFONT2 As New Font("ARIAL", 12, FontStyle.Bold)
        Dim pulou As Boolean = False

        Dim X1 As Single = e.MarginBounds.Left ' Variavel que controla a coluna
        Dim Y1 As Single = e.MarginBounds.Top  ' Variavel que controla a linha
        Dim Line As Single = MYFONT.GetHeight(e.Graphics) ' Pega o tamanho da linha a ser adicionada quando usar a myfont
        Dim Line2 As Single = MYFONT2.GetHeight(e.Graphics) ' Pega o tamanho da
        Dim Line3 As Single = MYFONT3.GetHeight(e.Graphics) ' Pega o tamanho da
        imagem = Image.FromFile("c:\temp\autobots.jpg")
        e.Graphics.DrawImage(imagem, X1, Y1 - 50, 50, 50)
        e.Graphics.DrawString("Empresa Legal", MYFONT2, Brushes.Blue, X1 + 100, Y1)
        Y1 += Line2
        e.Graphics.DrawString("Consultas marcadas para o cliente", MYFONT3, Brushes.Red, X1 + 100, Y1)
        Y1 += Line3
        e.Graphics.DrawString("Nome", MYFONT3, Brushes.Black, X1, Y1)
        e.Graphics.DrawString("Dia", MYFONT3, Brushes.Black, X1 + 300, Y1)
        e.Graphics.DrawString("Especialidade", MYFONT3, Brushes.Black, X1 + 500, Y1)
        Y1 += Line3
        e.Graphics.DrawLine(Pens.Black, X1, Y1, 700, Y1)
        e.Graphics.DrawString(tbaux.Fields("nomecliente").Value.ToString, MYFONT, Brushes.Black, X1, Y1)
        While tbaux.EOF = False

            e.Graphics.DrawString(tbaux.Fields("data").Value & " - " & tbaux.Fields("hora").Value.ToString, MYFONT, Brushes.Black, X1 + 300, Y1)
            e.Graphics.DrawString(tbaux.Fields("especialidade").Value.ToString, MYFONT, Brushes.Black, X1 + 500, Y1)
            Y1 += Line

            tbaux.MoveNext()
            If Y1 >= e.MarginBounds.Bottom - 50 Then
                pulou = True
                Exit While
            End If
        End While

        If pulou = True Then
            Y1 = e.MarginBounds.Bottom
            e.Graphics.DrawLine(Pens.Black, X1, Y1, X1 + 700, Y1)
            e.Graphics.DrawString("Página:" & wcpagina, MYFONT3, Brushes.Black, X1, Y1)
            wcpagina += 1
            e.HasMorePages = True
        End If
    End Sub
End Class