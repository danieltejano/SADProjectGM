Imports System.Data
Imports System.Data.OleDb
Imports System.Windows.Media.Animation
Imports MaterialDesignThemes.Wpf

Public Class MainMenu
    Private databaseConnection As New OleDbConnection
    Private csptrigger_WORKAROUND As Boolean = False
    Dim DB As New ADODB.Connection
    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        CloseProgram(Me)
    End Sub

    Private Sub GenerateCUSTID()

    End Sub

    Public Sub CheckStocks()
        Dim A As New ADODB.Connection
        Dim B As New ADODB.Recordset

        A.Open(connectionString)
        B.Open("SELECT * FROM Product WHERE (((Product.[UnitsAvailable])<=10))", A, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)
        Dim numberofLowStocks = B.RecordCount
        If numberofLowStocks > 0 Then
            Notify("Low Product Stocks", "Currently you have " & numberofLowStocks & " Products that are out of stock, please reconsider to restock products.")
        End If
    End Sub

    Private Sub MainMenu_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        mm = Me
        frameMain = MainFrame
        frameMain.Content = mmp

        If UserType = "Cashier" Then
            NVGAccounts.IsEnabled = False
            NVGLogs.IsEnabled = False
        End If

        CheckStocks()

        BackButton.IsEnabled = False
    End Sub

    Private Sub BackButton_Click(sender As Object, e As RoutedEventArgs) Handles BackButton.Click
        frameMain.Content = PreviousPage
        Dim newcashierpage As New CashierPage
        runningTotal = 0
        runningCount = 0
        cp = newcashierpage


    End Sub

    Private Sub NVGAccounts_Click(sender As Object, e As RoutedEventArgs) Handles NVGAccounts.Click
        BackButton.IsEnabled = True
        frameMain.Content = accp
        NVGClose.Command.Execute("{x:Static materialDesign:DrawerHost.CloseDrawerCommand}")
        PreviousPage = mmp

    End Sub

    Private Sub NVGCashier_Click(sender As Object, e As RoutedEventArgs) Handles NVGCashier.Click
        BackButton.IsEnabled = True
        If csptrigger_WORKAROUND Then
            frameMain.Content = csp
            NVGClose.Command.Execute("{x:Static materialDesign:DrawerHost.CloseDrawerCommand}")
            PreviousPage = mmp
            csptrigger_WORKAROUND = True
        Else
            csp = bcsp
            frameMain.Content = csp
            NVGClose.Command.Execute("{x:Static materialDesign:DrawerHost.CloseDrawerCommand}")
            PreviousPage = mmp
            csptrigger_WORKAROUND = False
        End If
    End Sub

    Private Sub NVGInventory_Click(sender As Object, e As RoutedEventArgs) Handles NVGInventory.Click
        BackButton.IsEnabled = True
        frameMain.Content = invp
        NVGClose.Command.Execute("{x:Static materialDesign:DrawerHost.CloseDrawerCommand}")
        PreviousPage = mmp
    End Sub

    Private Sub NVGCustomers_Click(sender As Object, e As RoutedEventArgs) Handles NVGCustomers.Click
        BackButton.IsEnabled = True
        frameMain.Content = custp
        NVGClose.Command.Execute("{x:Static materialDesign:DrawerHost.CloseDrawerCommand}")
        PreviousPage = mmp
    End Sub

    Private Sub NVGLogs_Click(sender As Object, e As RoutedEventArgs) Handles NVGLogs.Click
        BackButton.IsEnabled = True
        frameMain.Content = lp
        NVGClose.Command.Execute("{x:Static materialDesign:DrawerHost.CloseDrawerCommand}")
        PreviousPage = mmp
    End Sub

    Private Sub Sales_Click(sender As Object, e As RoutedEventArgs) Handles Sales.Click
        BackButton.IsEnabled = True
        frameMain.Content = sp
        NVGClose.Command.Execute("{x:Static materialDesign:DrawerHost.CloseDrawerCommand}")
        PreviousPage = mmp
    End Sub

    Private Sub Supplier_Click(sender As Object, e As RoutedEventArgs) Handles Supplier.Click
        BackButton.IsEnabled = True
        frameMain.Content = sup
        NVGClose.Command.Execute("{x:Static materialDesign:DrawerHost.CloseDrawerCommand}")
        PreviousPage = mmp
    End Sub


    Private Sub BTNLogout_Click(sender As Object, e As RoutedEventArgs) Handles BTNLogout.Click
        Me.Hide()
        DB.Open(connectionString)
        DB.Execute("Delete * From Status")
        DB.Close()
        Dim mainmenu As New MainWindow
        mainmenu.Show()

        RecordLog(accountID:=AccountId, loa:=UserType, actionTaken:="LOGGED OUT")
    End Sub

    Private Sub NVGDelivery_Click(sender As Object, e As RoutedEventArgs) Handles NVGDelivery.Click
        BackButton.IsEnabled = True
        frameMain.Content = dqp
        NVGClose.Command.Execute("{x:Static materialDesign:DrawerHost.CloseDrawerCommand}")
        PreviousPage = mmp
    End Sub

    Public Sub Notify(ByVal header As String, ByVal body As String)
        notificationBanner.Notify(header, body)
        Dim present As Storyboard = Me.FindResource("NotificationPopOut")
        present.Begin()

    End Sub


End Class
