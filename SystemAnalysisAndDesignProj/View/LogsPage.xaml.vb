Class LogsPage
    Private Sub LogsPage_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        PullDataFromDatabase(LogsTable, "Logs ORDER BY ADate DESC")
    End Sub

    Private Sub LogsTable_AutoGeneratingColumn(sender As Object, e As DataGridAutoGeneratingColumnEventArgs) Handles LogsTable.AutoGeneratingColumn
        If (e.PropertyName = "ADate") Then
            Dim column As DataGridTextColumn = e.Column
            Dim binding As Binding = column.Binding
            binding.StringFormat = "MMMMM-dd-yyyy-t"
        End If
    End Sub
End Class
