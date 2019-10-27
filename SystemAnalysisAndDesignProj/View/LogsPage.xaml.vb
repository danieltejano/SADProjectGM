Class LogsPage
    Private Sub LogsPage_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        PullDataFromDatabase(LogsTable, "Logs")
    End Sub
End Class
