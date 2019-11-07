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
        Dim oleDatabaseConnection As New OleDb.OleDbConnection(connectionString)
        oleDatabaseConnection.Open()
        Dim databasez As New OleDbCommand
        databasez.CommandText = "select * from ProductsPurchased"
        databasez.Connection = oleDatabaseConnection
        Dim databaseActualTable As OleDbDataReader = databasez.ExecuteReader()
        GRDSales.ItemsSource = databaseActualTable
    End Sub

    Private Sub GRDSales_AutoGeneratingColumn(sender As Object, e As DataGridAutoGeneratingColumnEventArgs) Handles GRDSales.AutoGeneratingColumn
        If (e.PropertyName = "Date") Then
            Dim column As DataGridTextColumn = e.Column
            Dim binding As Binding = column.Binding
            binding.StringFormat = "MMMM-dd-yyyy"
        End If
    End Sub

    Private Sub BTN_Click(sender As Object, e As RoutedEventArgs) Handles BTN.Click
        If BTN.Content = "New Sales" Then
            Dim oleDatabaseConnection As New OleDb.OleDbConnection(connectionString)
            oleDatabaseConnection.Open()
            Dim databasez As New OleDbCommand
            databasez.CommandText = "select * from ProductsPurchased"
            databasez.Connection = oleDatabaseConnection
            Dim databaseActualTable As OleDbDataReader = databasez.ExecuteReader()
            GRDSales.ItemsSource = databaseActualTable
            BTN.Content = "Old Sales"
        ElseIf BTN.Content = "Old Sales" Then
            Dim oleDatabaseConnection As New OleDb.OleDbConnection(connectionString)
            oleDatabaseConnection.Open()
            Dim databasez As New OleDbCommand
            databasez.CommandText = "select * from Sales"
            databasez.Connection = oleDatabaseConnection
            Dim databaseActualTable As OleDbDataReader = databasez.ExecuteReader()
            GRDSales.ItemsSource = databaseActualTable
            BTN.Content = "New Sales"
        End If
    End Sub
End Class
