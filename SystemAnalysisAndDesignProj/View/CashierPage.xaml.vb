Imports System.Data
Imports System.Data.OleDb
Imports System.Windows.Media.Animation

Class CashierPage
    Inherits Page


    Private databaseConnection As New OleDbConnection
    Private dt As New DataTable
    Private dtCategory As New DataTable
    Private category As List(Of String) = New List(Of String)
    Private sqlCategoryCondition As String = ""
    Private selectedProductQty As Integer = 0
    Private list As New List(Of String)
    Private qtyList As New List(Of Integer)
    Private shoppingCart As New List(Of ShoppingCartItem)
    Private isColumnsImplemented As Boolean = False
    Private productList As New List(Of Product)
    Public Property shoppingCartTotal() As Double
        Get
            Return runningTotal
        End Get
        Set(value As Double)
            runningTotal = value
        End Set
    End Property


    Private Sub GetCategories()
        Dim tempCategory As New List(Of String)
        tempCategory.Add("All")
        For Each element As ProductTemplate In InventoryContainer.Children
            Dim tempProduct As New ProductTemplate
            tempProduct = Nothing
            If TypeOf element Is ProductTemplate Then
                tempProduct = element

                tempCategory.Add(tempProduct.Product.Category.ToString)
            Else
                Return
            End If

        Next

        category = tempCategory
        category = category.Distinct.ToList
        CMBCategory.ItemsSource = category
        CMBCategory.SelectedIndex = 0
    End Sub

    Public Sub prepareDatabaseConnection()
        Try
            databaseConnection.ConnectionString = connectionString

            If databaseConnection.State <> ConnectionState.Open Then
                databaseConnection.Open()
            End If
            attachDatabaseToGrid()
        Catch ex As Exception
            'MessageBox.Show("Unable to load database to datagrid")
        End Try
    End Sub

    Private Sub attachDatabaseToGrid()
        Dim cmd As New OleDbCommand
        cmd.Connection = databaseConnection
        cmd.CommandText = "Select * from Product"
        Dim da As New OleDbDataAdapter(cmd)
        da.Fill(dt)
        InventoryTable.ItemsSource = dt.DefaultView
        InventoryTable.Items.Refresh()

    End Sub

    Private Function SearchProductID(ByVal productID As String, ByVal table As DataTable) As Product
        Dim product As New Product
        Dim sprice As String = ""
        table.Clear()
        Dim cmd As New OleDbCommand
        cmd.Connection = databaseConnection
        cmd.CommandText = "Select * from Product Where ProductID='" & productID & "'"
        Dim da As New OleDbDataAdapter(cmd)
        da.Fill(table)

        With product

            .ProductID = table.Rows(0)(0)
            .ProductName = table.Rows(0)(1)
            .ProductPrice = table.Rows(0)(2)
            sprice = table.Rows(0)(2)
            'MessageBox.Show(.ProductPrice)
            .ProductPrice = table.Rows(0)(3)
            .Category = table.Rows(0)(4)
            .UnitsAvailable = table.Rows(0)(5)
            .SupplierID = table.Rows(0)(6)

        End With

        product.ProductPrice = sprice
        'MessageBox.Show(product.ProductPrice)

        Return product
        'SearchItem("", dt)
    End Function



    Private Sub SearchItem(ByVal searchString As String, ByRef table As DataTable)
        table.Clear()
        Dim cmd As New OleDbCommand
        cmd.Connection = databaseConnection
        cmd.CommandText = "Select * from Product where ProductName Like '%" & searchString & "%'" & sqlCategoryCondition
        'MessageBox.Show(cmd.CommandText.ToString)
        Dim da As New OleDbDataAdapter(cmd)
        da.Fill(table)
        InventoryTable.ItemsSource = table.DefaultView
        InventoryTable.Items.Refresh()
    End Sub




    Private Sub LoadInventoryToWrapPanel(ByRef wp As WrapPanel, ByRef dataTable As DataTable)
        'InventoryTable.Visibility = Visibility.Hidden
        'BTNCheckout.Visibility = Visibility.Hidden
        Dim totalDataRows = dataTable.Rows.Count
        Dim price = 0



        For i = 0 To totalDataRows - 1
            Dim InventoryProduct As New ProductTemplate
            With InventoryProduct.Product
                .ProductID = dataTable.Rows(i)(0)
                .ProductName = dataTable.Rows(i)(1)
                .ProductPrice = dataTable.Rows(i)(2)
                .UnitSold = dataTable.Rows(i)(3)
                .Category = dataTable.Rows(i)(4)
                .UnitsAvailable = dataTable.Rows(i)(5)
                .SupplierID = dataTable.Rows(i)(6)
                price = dataTable.Rows(i)(2)
                .Quantity = 0
            End With

            InventoryProduct.qtyManager = qtySelector
            InventoryProduct.totalManager = Total
            InventoryProduct.totalCount = ItemCount
            InventoryProduct.Price.Text = "₱" + Format(price, "##,##0.00")
            InventoryProduct.ProductName.Text = InventoryProduct.Product.ProductName
            InventoryProduct.productDataTableGrid = ShoppingCartTable
            wp.Children.Add(InventoryProduct)


        Next

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

    Private Sub CashierPage_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        LoadDatagridColumns(ShoppingCartTable)
        prepareDatabaseConnection()
        LoadInventoryToWrapPanel(InventoryContainer, dt)
        GetCategories()
        CurrentPage = Me
        PreviousPage = csp
    End Sub

    Private Sub ShoppingCartTable_SelectedCellsChanged(sender As Object, e As SelectedCellsChangedEventArgs) Handles ShoppingCartTable.SelectedCellsChanged
        If ShoppingCartTable.Items.Count = 0 Then
            ShoppingCartTable.Items.Clear()
        End If
        EditItem.IsEnabled = True
    End Sub

    Private Sub SearchTextBox_TextChanged(sender As Object, e As TextChangedEventArgs) Handles SearchTextBox.TextChanged
        SearchItem(SearchTextBox.Text, dt)
        InventoryTable.Items.Refresh()
        InventoryContainer.Children.Clear()
        LoadInventoryToWrapPanel(InventoryContainer, dt)
    End Sub

    Private Function CurrencyToDouble(ByVal CurrencyString As String) As Double

        Dim TempStr As New System.Text.StringBuilder

        For i As Integer = 0 To CurrencyString.Length - 1

            If CurrencyString(i) = "," Or CurrencyString(i) = "." Or (CurrencyString(i) >= "0" And CurrencyString(i) <= "9") Then
                TempStr.Append(CurrencyString(i))
            End If

        Next

        Dim MyDouble As Double

        If Double.TryParse(TempStr.ToString, MyDouble) = False Then
            MyDouble = -1
        End If

        Return MyDouble

    End Function

    Private Sub DeleteItem_Click(sender As Object, e As RoutedEventArgs) Handles DeleteItem.Click
        ShoppingCartTable.Items.Remove(ShoppingCartTable.SelectedItem)
        qtySelector.UpdateStats()
    End Sub

    Private Sub CMBCategory_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles CMBCategory.SelectionChanged
        If CMBCategory.SelectedItem.ToString = "All" Then
            sqlCategoryCondition = ""
        Else
            sqlCategoryCondition = " AND Category = '" & CMBCategory.SelectedItem.ToString & "'"
        End If

        SearchItem(SearchTextBox.Text, dt)
        InventoryTable.Items.Refresh()
        InventoryContainer.Children.Clear()
        LoadInventoryToWrapPanel(InventoryContainer, dt)
    End Sub

    Private Sub Quantity_Selector_Loaded(sender As Object, e As RoutedEventArgs)
        qtySelector.Visibility = Visibility.Hidden
    End Sub

    Private Sub BTNCheckout_Click(sender As Object, e As RoutedEventArgs) Handles BTNCheckout.Click
        Dim review As New ShoppingCartReview
        If ShoppingCartTable.Items.Count = 0 Then

        Else

            list.Clear()
            qtyList.Clear()
            shoppingCart.Clear()
            For i = 0 To ShoppingCartTable.Items.Count - 1

                ShoppingCartTable.SelectedIndex = i
                list.Add(TryCast(ShoppingCartTable.Columns(0).GetCellContent(ShoppingCartTable.Items(i)), TextBlock).Text)
                qtyList.Add(CurrencyToDouble(TryCast(ShoppingCartTable.Columns(2).GetCellContent(ShoppingCartTable.Items(i)), TextBlock).Text))

            Next

            For Each cartitem As Integer In qtyList

            Next
            For Each productId As String In list
                Dim cartItem As New ShoppingCartItem
                cartItem.product = SearchProductID(productId, dt)
                shoppingCart.Add(cartItem)
            Next

            For i = 0 To qtyList.Count - 1
                shoppingCart(i).product.Quantity = qtyList(i)

            Next

            For Each a As ShoppingCartItem In shoppingCart
                review.ProductDataGrid.Items.Add(a.product)
            Next
            review.listOfProducts = shoppingCart
            frameMain.Content = review
            PreviousPage = Me
            review.list = list
        End If


    End Sub

    Private Sub ShoppingCartTable_MouseDoubleClick(sender As Object, e As MouseButtonEventArgs) Handles ShoppingCartTable.MouseDoubleClick
        Dim dg As DataGrid = TryCast(sender, DataGrid)
        Dim row As Product = CType(dg.SelectedItems(0), Product)
        qtySelector.previousQTY = row.Quantity
        qtySelector.productToAdd = row

        qtySelector.editingDataGrid = dg
        qtySelector.isEditing = True
        qtySelector.Visibility = Visibility.Visible
        qtySelector.FLDqty.Focus()
    End Sub

    Private Sub EditItem_Click(sender As Object, e As RoutedEventArgs) Handles EditItem.Click
        qtySelector.isEditing = True
        If ShoppingCartTable.SelectedIndex = -1 Or ShoppingCartTable.Items.Count = 0 Then
            EditItem.IsEnabled = False
        Else
            Dim row As Product = CType(ShoppingCartTable.SelectedItems(0), Product)
            qtySelector.previousQTY = row.Quantity
            qtySelector.productToAdd = row

            qtySelector.editingDataGrid = ShoppingCartTable

            qtySelector.isFromEditButton = True
            qtySelector.Visibility = Visibility.Visible
            qtySelector.FLDqty.Focus()
        End If
    End Sub
End Class
