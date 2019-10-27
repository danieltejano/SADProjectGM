Imports System.ComponentModel
Imports System.Data
Imports System.Globalization

Class ShoppingCartReview
    Inherits Page
    Public listOfItemsBought As New List(Of Product)
    Public listOfProducts As List(Of ShoppingCartItem)
    Public list As List(Of String)
    Public dataG As New DataTable
    Private isColumnsImplemented As Boolean = False
    Private gTotal As Double
    Private total As Double
    Private units As Integer
    Private vat As Double
    Public toDeliver As Boolean


    Private dFee As Double
    Public Property DeliveryFee() As Double
        Get
            Return dFee
        End Get
        Set(ByVal value As Double)
            dFee = value
        End Set
    End Property

    Public Property NetAmount As Double
        Get
            Return gTotal - vat
        End Get
        Set(value As Double)
            total = value
        End Set
    End Property

    Public Property GrandTotal As Double
        Get
            Return gTotal
        End Get
        Set(value As Double)
            gTotal = value
        End Set
    End Property

    Public Property TotalUnits As Integer
        Get
            Return units
        End Get
        Set(value As Integer)
            units = value
        End Set
    End Property

    Public Property ValueAddedTax As Double
        Get
            Return vat
        End Get
        Set(value As Double)
            vat = value
        End Set
    End Property

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
    Public Sub LoadDatagridColumns(ByRef d As DataGrid)
        If Not isColumnsImplemented Then
            Dim c1 As DataGridTextColumn = New DataGridTextColumn()
            c1.Header = "ProductID"
            c1.Binding = New Binding("ProductID")
            c1.Width = 110
            c1.IsReadOnly = True
            d.Columns.Add(c1)
            Dim c2 As DataGridTextColumn = New DataGridTextColumn()
            c2.Header = "Product Name"
            c2.Width = 170

            c2.Binding = New Binding("ProductName")
            c2.IsReadOnly = True
            d.Columns.Add(c2)
            Dim c3 As DataGridTextColumn = New DataGridTextColumn()
            c3.Header = "Qty"
            c3.Width = 50
            c3.Binding = New Binding("Quantity")
            c3.IsReadOnly = False
            d.Columns.Add(c3)
            Dim c4 As DataGridTextColumn = New DataGridTextColumn()
            c4.Header = "Price"
            c4.Width = 110
            c4.Binding = New Binding("ProductCurrencyPrice")
            c4.IsReadOnly = True
            d.Columns.Add(c4)
            isColumnsImplemented = True
        Else
            Return
        End If
    End Sub

    Private Sub ShoppingCartReview_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        PaymentGrid.DataContext = Me
        GrandTotal = runningTotal
        TotalUnits = runningCount
        ValueAddedTax = GrandTotal * 0.12
        LoadDatagridColumns(ProductDataGrid)
        For Each cartItem As ShoppingCartItem In listOfProducts
            cartItem.Width = 420
            ShoppingCartReviewer.Children.Add(cartItem)
        Next
        BTNDelivery.Visibility = Visibility.Visible
        GRDDelivery.Height = 0
        FLDCustomerID.Text = currentCustomer.CustomerID
        FLDCustomerLastName.Text = currentCustomer.LastName
        FLDCutstomerFirstName.Text = currentCustomer.FirstName
        LFLDCustomerAddress.Document.Blocks.Clear()
        LFLDCustomerAddress.Document.Blocks.Add(New Paragraph(New Run(currentCustomer.Address)))
    End Sub

    Private Sub BTNDelivery_Click(sender As Object, e As RoutedEventArgs) Handles BTNDelivery.Click

        MessageBox.Show(gTotal)
        MessageBox.Show(NetAmount)
        dFee = 234.0
        LBLVat_Copy.Text = "₱" + Format(gTotal + dFee, "##,##0.00")
        GRDDelivery.Height = 260
        BTNDelivery.Visibility = Visibility.Hidden
        DPDeliveryDate.Text = Now
        LFLDDeliveryAddress.Document.Blocks.Clear()
        LFLDDeliveryAddress.Document.Blocks.Add(New Paragraph(New Run(currentCustomer.Address)))
        LBLDeliveryFee.Text = "₱" + Format(DeliveryFee, "##,##0.00")
        toDeliver = True
    End Sub

    Private Sub BTNDeliveryCLose_Click(sender As Object, e As RoutedEventArgs) Handles BTNDeliveryClose.Click
        GRDDelivery.Height = 0
        BTNDelivery.Visibility = Visibility.Visible
        dFee = 0.0
        LBLDeliveryFee.Text = dFee
        LBLVat_Copy.Text = "₱" + Format(gTotal, "##,##0.00")
        toDeliver = False
    End Sub

    Private Sub BTNRecievePayment_Click(sender As Object, e As RoutedEventArgs) Handles BTNRecievePayment.Click

        If toDeliver = True Then
            If DPDeliveryDate.Text = "" Then
                MessageBox.Show("Cannot have an empty delivery date.")
            Else
                PaymentControl.Visibility = Visibility.Visible
                GRDDelivery.IsEnabled = False
                ProductTable.IsEnabled = False
                GRDCustomer.IsEnabled = False
                BTNDelivery.IsEnabled = False
                BTNDeliveryClose.IsEnabled = False
                DeliveryDate = DPDeliveryDate.Text
            End If
        Else
            PaymentControl.Visibility = Visibility.Visible
            GRDDelivery.IsEnabled = False
            ProductTable.IsEnabled = False
            GRDCustomer.IsEnabled = False
            BTNDelivery.IsEnabled = False
            BTNDeliveryClose.IsEnabled = False
        End If



        PaymentControl.total = gTotal + DeliveryFee
        PaymentControl.LBLAmountDue.Text = "₱" + Format(PaymentControl.total, "##,##0.00")
        PaymentControl.LBLChange.Text = ""
        PaymentControl.FLDAmountRecieved.Focus()
        PaymentControl.ItemsBought = list
        PaymentControl.parentPage = Me
        PaymentControl.toDeliver = toDeliver

        For Each product As Product In ProductDataGrid.Items
            listOfItemsBought.Add(product)
        Next

        PaymentControl.listOfProductsBrought = listOfItemsBought
    End Sub

    Private Sub DPDeliveryDate_CalendarOpened(sender As Object, e As RoutedEventArgs) Handles DPDeliveryDate.CalendarOpened
        DPDeliveryDate.DisplayDate = Now
        Dim startdate As New DateTime
        startdate = DateTime.Today.AddYears(100)
        DPDeliveryDate.BlackoutDates.Add(New CalendarDateRange(start:=startdate, [end]:=startdate.AddYears(2000)))
        DPDeliveryDate.BlackoutDates.AddDatesInPast()
    End Sub
End Class
