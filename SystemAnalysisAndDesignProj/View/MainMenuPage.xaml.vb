Imports System.Data
Imports System.Data.OleDb
Imports System.Windows.Media
Class MainMenuPage
    Dim log_values As Char
    Dim DB As New ADODB.Connection
    Dim RS As New ADODB.Recordset
    Dim oleDatabaseConnection As New OleDb.OleDbConnection("Provider=Microsoft.jet.oledb.4.0;Data Source=SAD_DB.mdb")
    Public dy As String = Now.ToString






    Private Sub BTNOpenCashier_Click(sender As Object, e As RoutedEventArgs) Handles BTNOpenCashier.Click
        csp = bcsp
        frameMain.Content = csp
        PreviousPage = mmp
    End Sub

    Private Sub BTNManageAccounts_Click(sender As Object, e As RoutedEventArgs) Handles BTNManageAccounts.Click
        accp = baccp
        frameMain.Content = accp
        PreviousPage = mmp
    End Sub

    Private Sub BTNViewInventory_Click(sender As Object, e As RoutedEventArgs) Handles BTNViewInventory.Click
        invp = binvp
        frameMain.Content = invp
        PreviousPage = mmp
    End Sub

    Private Sub BTNLogs_Click(sender As Object, e As RoutedEventArgs) Handles BTNLogs.Click
        lp = blp
        frameMain.Content = lp
        PreviousPage = mmp
    End Sub

    Private Sub MainMenuPage_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        If UserType = "Cashier" Then
            BTNManageAccounts.IsEnabled = False
            BTNLogs.IsEnabled = False
        Else
            BTNManageAccounts.IsEnabled = True
            BTNLogs.IsEnabled = True
        End If
        Dim table As String = "dataTable"
        Dim cons As String = connectionString
        Dim ds As New DataSet
        Dim cnn As OleDbConnection = New OleDbConnection(cons)
        Dim query As String = "SELECT CustomerName,DeliveryAddress,DeliveryDate FROM Delivery_Job WHERE DeliveryDate>=NOW()-1"
        cnn.Open()
        Dim cmd As New OleDbCommand(query, cnn)
        Dim da As New OleDbDataAdapter(cmd)
        da.Fill(ds, table)
        cnn.Close()

        Dim t1 As DataTable = ds.Tables(table)
        If (t1.Rows.Count <= 0) Then
            DGridQue.Visibility = Visibility.Hidden
            LBLPending.Visibility = Visibility.Visible
            LBLPending.Content = "NO PENDING DELIVERIES"
        Else
            oleDatabaseConnection.Close()
            LBLPending.Visibility = Visibility.Hidden
            oleDatabaseConnection.Open()                            'Opens the database using the connection String oleDatabaseConnection declared above
            Dim databasez As New OleDbCommand                       'Creates an Instance of the OleDbCommand class to be the source of SQL Command for the openned Connection
            databasez.CommandText = "SELECT CustomerName,DeliveryAddress,DeliveryDate FROM Delivery_Job WHERE DeliveryDate>=NOW()-1"
            databasez.Parameters.AddWithValue("@dy", dy)
            databasez.Connection = oleDatabaseConnection            'Sets the Connection Variable/Attribute of the OleDbCommandClass to use in executing the SQL Command
            Dim databaseActualTable As OleDbDataReader = databasez.ExecuteReader()          'Creates a OleDbDataReader to execute the SQL Command and store it temporarily in an imaginary table
            DGridQue.ItemsSource = databaseActualTable

        End If

        LBLFormSubtitle.Text = "Welcome " & Usrnm
        CheckInventory()

    End Sub

    Private Sub CheckInventory()
        RemindersContainer.Children.Clear()
        Dim oleDatabaseConnection As New OleDb.OleDbConnection(connectionString)
        oleDatabaseConnection.Open()
        Dim databasez As New OleDbCommand
        databasez.CommandText = "SELECT * FROM Product WHERE (((Product.[UnitsAvailable])<=10)) order by UnitsAvailable ASC"
        databasez.Connection = oleDatabaseConnection
        Using databaseActualTable As OleDbDataReader = databasez.ExecuteReader()
            While databaseActualTable.Read()
                Dim a As New ReminderItem
                If databaseActualTable.GetValue(5) = 0 Then
                    a.NotificationHeader.Text = "Product Depleted"
                    a.NotificationSubheader.Text = databaseActualTable.GetValue(1) & " has been depleted " & " please consider to restock within the week"
                    RemindersContainer.Children.Add(a)
                Else
                    a.NotificationHeader.Text = "Product nearly Depleted"
                    a.NotificationIcon.Foreground = New SolidColorBrush(ColorConverter.ConvertFromString("#FFEED42D"))
                    a.NotificationSubheader.Text = databaseActualTable.GetValue(1) & " is nearly depleted " & " with " & databaseActualTable.GetValue(5) & " stocks available please consider to restock within the week"
                    RemindersContainer.Children.Add(a)
                End If

            End While
        End Using


    End Sub

    Private Sub BTNPrintDailySalesReport_Click(sender As Object, e As RoutedEventArgs) Handles BTNPrintDailySalesReport.Click
        generatedreports = "Daily"
        Dim report As New Reports
        report.ShowDialog()
    End Sub

    Private Sub BTNPrintMonthlySalesReport_Click(sender As Object, e As RoutedEventArgs) Handles BTNPrintMonthlySalesReport.Click
        generatedreports = "Monthly"
        Dim report As New Reports
        report.ShowDialog()
    End Sub

    Private Sub BTNPrintProductPricelist_Click(sender As Object, e As RoutedEventArgs) Handles BTNPrintProductPricelist.Click
        generatedreports = "Pricelist"
        Dim report As New Reports
        report.ShowDialog()
    End Sub

    Private Sub BTNPrintYearlySalesReport_Click(sender As Object, e As RoutedEventArgs) Handles BTNPrintYearlySalesReport.Click
        generatedreports = "Yearly"
        Dim report As New Reports
        report.ShowDialog()
    End Sub

    Private Sub DGridQue_AutoGeneratingColumn(sender As Object, e As DataGridAutoGeneratingColumnEventArgs) Handles DGridQue.AutoGeneratingColumn
        If (e.PropertyName = "DeliveryDate") Then
            Dim column As DataGridTextColumn = e.Column
            Dim binding As Binding = column.Binding
            binding.StringFormat = "MMMMM-dd-yyyy"
        End If
    End Sub
End Class
