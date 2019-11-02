Imports System.Data
Imports System.Data.OleDb
Public Class SalesPage
    Dim log_values As Char
    Dim DB As New ADODB.Connection
    Dim RS As New ADODB.Recordset
    Dim oleDatabaseConnection As New OleDb.OleDbConnection("Provider=Microsoft.jet.oledb.4.0;Data Source=Inventory.mdb")

    Public dy As String
    Public dm As String

    Private Sub Sales_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        PullDataFromDatabase(d:=GRDSales, tableName:="Sales ORDER BY Date DESC")
    End Sub

    Private Sub GRDSales_AutoGeneratingColumn(sender As Object, e As DataGridAutoGeneratingColumnEventArgs) Handles GRDSales.AutoGeneratingColumn
        If (e.PropertyName = "Date") Then
            Dim column As DataGridTextColumn = e.Column
            Dim binding As Binding = column.Binding
            binding.StringFormat = "MMMM-dd-yyyy"
        End If
    End Sub
End Class
