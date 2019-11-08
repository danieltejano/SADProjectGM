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

    Public Sub New()
        InitializeComponent()
        '---Create a seriescollection and add first series as a columnseries (index 0) and some static values to show---
        '---The first series will show just 4 columns---
        MySeriesCollection = New LiveCharts.SeriesCollection From {
                New LiveCharts.Wpf.ColumnSeries With {.Values = New LiveCharts.ChartValues(Of Double) From {
                        110,
                        350,
                        239,
                        550
                    }
                }
            }


        DataContext = Me
    End Sub


    Public Sub ReloadStats(ByVal productID As String)
        productData.Clear()
        MySeriesCollection.Clear()
        MyLabels.Clear()
        productDataTable.Clear()
        Me.productID = productID
        InitializeDataGrid()
        If productDataTable.Rows.Count = 0 Then
            productData.Clear()
            MySeriesCollection.Clear()
            MyLabels.Clear()
        Else
            InitializeProductData()

            MySeriesCollection.Add(New LiveCharts.Wpf.LineSeries With {
                    .Title = "Marble",
                    .Values = New LiveCharts.ChartValues(Of DateTimePoint)(productData),
                    .LineSmoothness = 0
                   })


            '---Add a second columnseries(index 1) with nothing in it yet--- 
            '---Define formatter to change double values on y-axis to string labels---
            XFormatter = Function(value) New DateTime(CLng(value)).ToString("yyyy")
            YFormatter = Function(value) value.ToString("N")
            DataContext = Me
        End If

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

    Private Sub Close_Click(sender As Object, e As RoutedEventArgs) Handles Close.Click
        Me.Visibility = Visibility.Hidden
    End Sub
End Class
