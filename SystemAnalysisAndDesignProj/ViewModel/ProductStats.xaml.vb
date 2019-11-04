Imports System.Data
Imports System.Data.OleDb
Imports LiveCharts
Imports LiveCharts.Defaults
Imports LiveCharts.Wpf

Public Class ProductStats
    Public Property MySeriesCollection As LiveCharts.SeriesCollection
    Public Property MyLabels As New List(Of String)
    Public Property XFormatter As Func(Of Double, String)
    Public Property YFormatter As Func(Of Double, String)
    Public Property Dateparser As String

    Public productData As New List(Of DateTimePoint)
    Public productDataTable As New DataTable

    Public productID As String


    Public Sub ReloadStats(ByVal productID As String)
        productData.Clear()
        productData = New List(Of DateTimePoint)
        Me.productID = productID
        InitializeDataGrid()
        InitializeProductData()

        productChart.Series.Add(
                New LineSeries With {
                    .Title = "Product Name",
                    .Values = New ChartValues(Of DateTimePoint)(productData)
                    }
        )

        '---Add a second columnseries(index 1) with nothing in it yet--- 
        '---Define formatter to change double values on y-axis to string labels---
        XFormatter = Function(val) New DateTime(CLng(val)).ToString("yyyy")
        YFormatter = Function(val) val.ToString("N")
        DataContext = Me
    End Sub

    Private Sub InitializeDataGrid()
        Dim oleDatabaseConnection As New OleDb.OleDbConnection(connectionString)
        oleDatabaseConnection.Open()
        Dim databasez As New OleDbCommand
        databasez.CommandText = "select * from ProductsPurchased where ProductID='" & productID & "'"
        databasez.Connection = oleDatabaseConnection
        productDataTable.Load(databasez.ExecuteReader())
        productSalesTable.ItemsSource = databasez.ExecuteReader()

    End Sub

    Private Sub InitializeProductData()
        For Each dr As DataRow In productDataTable.Rows
            Dim productPurchaseDate = dr("DatePurchased").ToString
            Dim productPurchasedUnits = dr("Quantity")

            productData.Add(New DateTimePoint(DateTime.Parse(productPurchaseDate), productPurchasedUnits))
        Next

    End Sub

    Private Sub ProductStats_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded

    End Sub
End Class
