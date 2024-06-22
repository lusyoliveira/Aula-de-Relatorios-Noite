Public Class Form1
    Dim tbaux As ADODB.Recordset
    Dim sql As String
    Dim wcpagina As Integer = 1
    Dim imagem As Image

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        sql = "Select * from tbclientes where 1=1 "
        If txtNome.Text <> "" Then
            sql = sql & " and nome like '" & txtNome.Text & "%'"
        End If
        If optAtivos.Checked = True Then
            sql = sql & " and liberado = '1'"
        End If
        If optInativos.Checked = True Then
            sql = sql & " and liberado = '0'"
        End If
        sql = sql & " order by nome"
        tbaux = OpenRecordset(sql)
        If tbaux.RecordCount = 0 Then
            MsgBox("Nenhum cliente no cadastro !", MsgBoxStyle.Information)
            Exit Sub
        End If
        PrintPreviewDialog1.ShowDialog()
    End Sub

    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
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
        e.Graphics.DrawString("Relatório de Clientes", MYFONT3, Brushes.Red, X1 + 100, Y1)
        Y1 += Line3
        e.Graphics.DrawString("Matrícula", MYFONT3, Brushes.Black, X1, Y1)
        e.Graphics.DrawString("Nome", MYFONT3, Brushes.Black, X1 + 80, Y1)
        e.Graphics.DrawString("Endereço", MYFONT3, Brushes.Black, X1 + 400, Y1)
        Y1 += Line3
        e.Graphics.DrawLine(Pens.Black, X1, Y1, 700, Y1)
        While tbaux.EOF = False
            e.Graphics.DrawString(tbaux.Fields("matricula").Value.ToString, MYFONT, Brushes.Black, X1, Y1)
            e.Graphics.DrawString(tbaux.Fields("nome").Value.ToString, MYFONT, Brushes.Black, X1 + 80, Y1)
            e.Graphics.DrawString(tbaux.Fields("logradouro").Value.ToString, MYFONT, Brushes.Black, X1 + 400, Y1)
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
