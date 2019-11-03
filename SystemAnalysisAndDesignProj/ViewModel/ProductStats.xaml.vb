Imports System.Data
Imports LiveCharts.Defaults

Public Class ProductStats
    Public Property MySeriesCollection As LiveCharts.SeriesCollection
    Public Property MyLabels As New List(Of String)
    Public Property XFormatter As Func(Of Double, String)
    Public Property YFormatter As Func(Of Double, String)
    Public Property Dateparser As String

    Public productData As New List(Of DateTimePoint)

    Public Sub New()
        InitializeComponent()
        InitializeDataGrid()
        InitializeProductData()


        MySeriesCollection = New LiveCharts.SeriesCollection From {
                New LiveCharts.Wpf.LineSeries With {
                    .Title = "Product Name",
                    .Values = productData
                    }
        }

        '---Add a second columnseries(index 1) with nothing in it yet--- 
        '---Define formatter to change double values on y-axis to string labels---
        XFormatter = Function(val) New DateTime(CLng(val)).ToString("yyyy")
        YFormatter = Function(val) val.ToString("N")
        DataContext = Me


    End Sub

    Private Sub InitializeDataGrid()
        PullDataFromDatabase(d:=productSalesTable, tableName:="Sales Where ProductID = FSC-000-0C")
    End Sub

    Private Sub InitializeProductData()
        Dim internalTable As New DataTable
        internalTable = productSalesTable.ItemsSource


        For Each dr As DataRow In internalTable.Rows
            Dim productPurchaseDate = dr.Item("DatePurchased")
            Dim productPurchasedUnits = dr.Item("Quantity")

            productData.Add(New DateTimePoint(DateTime.Parse(productPurchaseDate), productPurchasedUnits))
        Next
    End Sub
End Class
