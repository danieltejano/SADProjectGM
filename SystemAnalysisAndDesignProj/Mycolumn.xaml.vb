Public Class Mycolumn
    '---Need to declare binding properties here so that XAML can find them---
    '---Remember XAML is case sensitive---
    Public Property MySeriesCollection As LiveCharts.SeriesCollection
    Public Property MyLabels As New List(Of String)
    Public Property MyFormatter As Func(Of Double, String)

    Public Sub New()
        InitializeComponent()
        '---Create a seriescollection and add first series as a columnseries (index 0) and some static values to show---
        '---The first series will show just 4 columns---
        MySeriesCollection = New LiveCharts.SeriesCollection From {
                New LiveCharts.Wpf.ColumnSeries With {
                    .Title = "Granite",
                    .Values = New LiveCharts.ChartValues(Of Double) From {
                        110,
                        350,
                        239,
                        550
                    }
                }
            }
        '---Add a second columnseries(index 1) with nothing in it yet---  
        MySeriesCollection.Add(New LiveCharts.Wpf.ColumnSeries With {
                .Title = "Marble",
                .Values = New LiveCharts.ChartValues(Of Double)})
        '---Now add some dynamic values to columnseries (1) - will show 10 columns of results ---
        '---These values can come from a list or array of double calculated elsewhere in the program---
        For i = 1 To 10
            MySeriesCollection(1).Values.Add(CDbl(i + (2 * i) ^ 2))
        Next
        '---Add 10 labels to show on the x-axis---
        For i = 1 To 10
            MyLabels.Add(CStr(i))
        Next
        '---Define formatter to change double values on y-axis to string labels---
        MyFormatter = Function(value) value.ToString("N")
        DataContext = Me
    End Sub

    Private Sub ClearData_Click(sender As Object, e As RoutedEventArgs) Handles ClearData.Click

        MySeriesCollection.Clear
    End Sub
    Private Sub ReloadData_Click(sender As Object, e As RoutedEventArgs) Handles ReloadData.Click

        '---Add a second columnseries(index 1) with nothing in it yet---  
        MySeriesCollection.Add(New LiveCharts.Wpf.ColumnSeries With {
                .Title = "Marble",
                .Values = New LiveCharts.ChartValues(Of Double)})
        '---Now add some dynamic values to columnseries (1) - will show 10 columns of results ---
        '---These values can come from a list or array of double calculated elsewhere in the program---
        For i = 1 To 10
            MySeriesCollection(0).Values.Add(CDbl(i + (2 * i) ^ 2))
        Next
        '---Add 10 labels to show on the x-axis---
        For i = 1 To 10
            MyLabels.Add(CStr(i))
        Next
        '---Define formatter to change double values on y-axis to string labels---
        MyFormatter = Function(value) value.ToString("N")
    End Sub
End Class