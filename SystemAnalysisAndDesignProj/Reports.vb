Public Class Reports
    Private Sub CrystalReportViewer1_Load(sender As Object, e As EventArgs) Handles CrystalReportViewer1.Load
        CrystalReportViewer1.Show()
        If generatedreports = "Daily" Then
            Dim reportview As New Daily
            CrystalReportViewer1.ReportSource = reportview
            CrystalReportViewer1.RefreshReport()
        ElseIf generatedreports = "Monthly" Then
            Dim reportview As New Monthly
            CrystalReportViewer1.ReportSource = reportview
            CrystalReportViewer1.RefreshReport()
        ElseIf generatedreports = "Yearly" Then
            Dim reportview As New Yearly
            CrystalReportViewer1.ReportSource = reportview
            CrystalReportViewer1.RefreshReport()
        ElseIf generatedreports = "Pricelist" Then
            Dim reportview As New Pricelist
            CrystalReportViewer1.ReportSource = reportview
            CrystalReportViewer1.RefreshReport()
        End If
    End Sub
End Class