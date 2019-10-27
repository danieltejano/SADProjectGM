Imports System.Data.OleDb
Public Class CashRegister
    Public customer As Customer
    Public ItemsBought As List(Of String)
    Public listOfProductsBrought As List(Of Product)
    Public total As Double
    Public change As Double
    Public tenderAmount As Double
    Public parentPage As ShoppingCartReview
    Public toDeliver As Boolean
    Private TransactionID As String
    Dim DB As New ADODB.Connection
    Dim RS As New ADODB.Recordset
    Public Category As String

    Private Sub QTYClose_Click(sender As Object, e As RoutedEventArgs) Handles QTYClose.Click
        parentPage.GRDDelivery.IsEnabled = True
        parentPage.ProductTable.IsEnabled = True
        parentPage.GRDCustomer.IsEnabled = True
        parentPage.BTNDelivery.IsEnabled = True
        parentPage.BTNDeliveryClose.IsEnabled = True
        Me.Visibility = Visibility.Hidden

    End Sub

    Private Sub BTNConfirm_Click(sender As Object, e As RoutedEventArgs) Handles BTNConfirm.Click
        Double.TryParse(FLDAmountRecieved.Text, tenderAmount)
        change = tenderAmount - total

        If change < 0 Then
            MessageBox.Show("Unable to tender amount due to lack of funds")
        Else
            LBLChange.Text = "₱" + Format(change, "##,##0.00")
            BTNConfirm.Visibility = Visibility.Hidden
            BTNFinish.Visibility = Visibility.Visible
        End If

    End Sub

    Public Function generateNextTransactionID() As String
        Dim dConnection As New ADODB.Connection
        Dim rset As New ADODB.Recordset
        Dim TransactionID As String
        Dim TransactionIDCode As Integer

        dConnection.Open(connectionString)
        rset.Open("Select * from Cashier_Transaction", dConnection)
        With rset
            .Close()
            .Open("Cashier_Transaction", dConnection, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)
            .MoveLast()
            TransactionID = .Fields("CashierTransactionID").Value
        End With

        TransactionID = TransactionID.Remove(0, 2)
        Integer.TryParse(TransactionID, TransactionIDCode)

        TransactionIDCode += 1
        TransactionID = "CN" & TransactionIDCode
        Return TransactionID
    End Function
    'Update
    Private Sub BTNFinish_Click(sender As Object, e As RoutedEventArgs) Handles BTNFinish.Click
        TransactionID = generateNextTransactionID()
        If toDeliver = True Then
            Dim A As New ADODB.Connection
            Dim B As New ADODB.Recordset
            A.Open(connectionString)
            B.Open("Select * from Delivery_Job", A)

            With B
                .Close()
                .Open("Delivery_Job", A, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)
                .AddNew()

                .Fields("CashierTransactionID").Value = TransactionID
                .Fields("CustomerName").Value = currentCustomer.FirstName + " " + currentCustomer.LastName
                .Fields("DeliveryAddress").Value = currentCustomer.Address
                .Fields("DeliveryDate").Value =
                .Fields("ContactNumber").Value = 0
                .Fields("DeliveryStatus").Value = "PENDING"
                .Update()
                .Close()
            End With

            RecordLog(accountID:=AccountId, loa:=UserType, actionTaken:="ADDED DELIVERY INFO")
        End If
        Using act As New OleDbConnection(connectionString)
            act.Open()
            Dim command As New OleDbCommand("insert into Cashier_Transaction ([CashierTransactionID], [AccountID],[DateTran],[TotalPrice], [TaxTotal], [TenderAmount], [Change], [toDeliver?], [QuantityPurchased])  
                                                values ( @CashierTransactionID,  @AccountID, @DateTran, @TotalPrice, @TaxTotal, @TenderAmount, @Change, @toDeliver, @Qty)", act)
            Dim dt As String
            dt = Now.ToLongDateString & " / " & Now.ToLongTimeString
            With command.Parameters
                .AddWithValue("@CashierTransactionID", TransactionID)
                .AddWithValue("@AccountID", AccountId)
                .AddWithValue("@DateTran", dt)
                .AddWithValue("@TotalPrice", total)
                .AddWithValue("@TaxTotal", total * 0.12)
                .AddWithValue("@TenderAmount", tenderAmount)
                .AddWithValue("@Change", change)
                .AddWithValue("@toDeliver?", toDeliver)
            End With
            command.ExecuteNonQuery()
            command.Dispose()
            act.Close()
        End Using

        For Each product As Product In listOfProductsBrought
            Using act As New OleDbConnection(connectionString)
                act.Open()
                Dim command As New OleDbCommand("insert into ProductsPurchased ([Cashier_TransactionID], [CustomerID], [ProductID], [ProductName], [ProductPrice], [DatePurchased], [Quantity])  
                                                values ( @Cashier_TransactionID,  @CustomerID, @ProductID, @ProductName, @ProductPrice, @DatePurchased, @Qty)", act)
                Dim dt As String
                dt = Now.ToLongDateString
                With command.Parameters
                    .AddWithValue("@Cashier_TransactionID", TransactionID)
                    .AddWithValue("@CustomerID", currentCustomer.CustomerID)
                    .AddWithValue("@ProductID", product.ProductID)
                    .AddWithValue("@ProductName", product.ProductName)
                    .AddWithValue("@ProductPrice", product.ProductPrice)
                    .AddWithValue("@DatePurchased", dt)
                    .AddWithValue("@Qty", product.Quantity)
                End With
                command.ExecuteNonQuery()
                command.Dispose()
                act.Close()
            End Using

            Dim i As Integer = -1 + 1
            Dim qty As TextBlock = TryCast(cp.ShoppingCartTable.Columns(2).GetCellContent(cp.ShoppingCartTable.Items(i)), TextBlock)
            Dim qty2 As Integer = qty.Text

            DB.Open(connectionString)
            RS.Open("SELECT * FROM Product WHERE ProductID='" & product.ProductID & Chr(39), DB)

            If (RS.Fields("ProductID").Value = product.ProductID) Then
                Category = RS.Fields("Category").Value
                DB.Close()
            End If
            Using act As New OleDbConnection(connectionString)
                act.Open()
                Dim command As New OleDbCommand("insert into Sales ([ProductID], [ProductName], [Category], [Date], [UnitSold], [Sales])  
                                                values (@ProductID, @ProductName, @Category, @Date, @UnitSold, @Sales)", act)
                Dim dt As Date
                dt = Now.ToShortDateString
                With command.Parameters
                    .AddWithValue("@ProductID", product.ProductID)
                    .AddWithValue("@ProductName", product.ProductName)
                    .AddWithValue("@Category", Category)
                    .AddWithValue("@Date", dt)
                    .AddWithValue("@UnitSold", qty2)
                    .AddWithValue("@Sales", product.ProductPrice * qty2)
                End With
                command.ExecuteNonQuery()
                command.Dispose()
                act.Close()
            End Using
        Next

        RecordLog(accountID:=AccountId, loa:=UserType, actionTaken:="Generated Transaction")
        runningCount = 0
        runningTotal = 0
        GRDCompleteTransaction.Visibility = Visibility.Visible
    End Sub

    Private Sub BTNReturnToCashier_Click(sender As Object, e As RoutedEventArgs) Handles BTNReturnToCashier.Click
        csp = bcsp
        cp = bcp
        frameMain.Content = csp
    End Sub

    Private Sub FLDAmountRecieved_PreviewKeyDown(sender As Object, e As KeyEventArgs) Handles FLDAmountRecieved.PreviewKeyDown
        Dim x As String
        x = e.Key
        If x = Key.Back Or x = Key.Left Or x = Key.Right Then
            e.Handled = False
        ElseIf (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x >= Key.NumPad0 And x <= Key.NumPad9) Or (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x >= Key.D0 And x <= Key.D9) Or (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x = Key.OemMinus) Or (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x = Key.OemPeriod) Then
            e.Handled = False
        ElseIf e.Key = Key.Escape Then
            Me.Visibility = Visibility.Hidden
        ElseIf x = Key.Enter Then
            BTNConfirm.Focus()
        Else
            e.Handled = True
        End If
    End Sub
End Class
