Imports System.Data
Imports System.Windows.Media.Animation
Imports Microsoft.VisualBasic.CompilerServices

Public Class Quantity_Selector

    Public productToAdd As Product
    Public productDataTableGrid As DataGrid
    Public quantity As Integer
    Public totalManager As TextBlock
    Public totalCount As TextBlock
    Public qtyManager As Quantity_Selector
    Public isEditing As Boolean = False
    Public editingDataGrid As DataGrid
    Public previousQTY As Integer
    Public isFromEditButton As Boolean = False
    Public productTemplate As ProductTemplate

    Private Sub Quantity_Selector_IsVisibleChanged(sender As Object, e As DependencyPropertyChangedEventArgs) Handles Me.IsVisibleChanged
        If e.NewValue = Visibility.Visible Then
            isSelectingQuantity = True
            FLDqty.Focus()
        Else
            isSelectingQuantity = False
        End If
    End Sub




    Private Sub BTNConfirm_Click(sender As Object, e As RoutedEventArgs) Handles BTNConfirm.Click
        If FLDqty.Text = Nothing Then
            FLDqty.Text = 0
        ElseIf Int(FLDqty.Text) = 0 Then
            FLDqty.Text = 0
        ElseIf Int(FLDqty.Text) > productToAdd.UnitsAvailable Then
            MessageBox.Show("Insufficient product")
        ElseIf isEditing Then

            Integer.TryParse(FLDqty.Text, qtySetter)
            productToAdd.Quantity = qtySetter
            evaluateEdit()
            isEditing = False
            FLDqty.Text = ""
            qtySetter = 0
            Me.Visibility = Visibility.Hidden
            cp.ShoppingCartTable.Items.Refresh()
            UpdateStats()
        ElseIf Not isEditing Then
            If productDataTableGrid.Items.Contains(productToAdd) Then

                For i = 0 To productDataTableGrid.Items.Count - 1
                    productDataTableGrid.SelectedIndex = i
                    Dim row As New Product
                    row = CType(productDataTableGrid.SelectedItem, Product)

                    If productToAdd.Equals(row) Then
                        If row.Quantity > row.UnitsAvailable Then
                            MessageBox.Show("You can not purchase greater than what is available")
                        Else
                            previousQTY = row.Quantity
                            addQuantityToItem(row)
                        End If
                    End If
                    Me.Visibility = Visibility.Hidden
                    cp.ShoppingCartTable.Items.Refresh()
                Next
            ElseIf isFromEditButton Then
                Integer.TryParse(FLDqty.Text, qtySetter)
                productToAdd.Quantity = qtySetter
                evaluateEdit()
                isEditing = False
                FLDqty.Text = ""
                qtySetter = 0
                Me.Visibility = Visibility.Hidden
                cp.ShoppingCartTable.Items.Refresh()
                UpdateStats()
                productToAdd.UnitsAvailable -= productToAdd.Quantity
            Else
                Integer.TryParse(FLDqty.Text, qtySetter)
                productToAdd.Quantity = qtySetter
                productDataTableGrid.Items.Add(productToAdd)
                FLDqty.Text = ""
                qtySetter = 0
                Me.Visibility = Visibility.Hidden
                cp.ShoppingCartTable.Items.Refresh()
                UpdateStats()
                productToAdd.UnitsAvailable -= productToAdd.Quantity
            End If
        End If
        FLDqty.Text = ""

    End Sub

    Private Sub addQuantityToItem(ByRef targetProduct As Product)
        Integer.TryParse(FLDqty.Text, qtySetter)
        Dim itemCountDelta As Integer = qtySetter + previousQTY
        itemCountDelta = itemCountDelta - previousQTY
        qtySetter += previousQTY
        productToAdd.Quantity = qtySetter
        Dim doublePrice As Double = 0.00
        Double.TryParse(targetProduct.ProductPrice, doublePrice)
        doublePrice = itemCountDelta * doublePrice
        runningTotal += doublePrice
        totalManager.Text = "₱" + Format(runningTotal, "##,##0.00")
        runningCount = runningCount + (1 * itemCountDelta)
        totalCount.Text = runningCount
    End Sub

    Private Sub evaluateEdit()
        If qtySetter < previousQTY Then
            previousQTY -= qtySetter

        ElseIf qtySetter > previousQTY Then

            qtySetter -= previousQTY

            If productDataTableGrid.Items.Contains(productToAdd) Then
                productDataTableGrid.Items.Refresh()
            Else
                productDataTableGrid.Items.Add(productToAdd)
            End If

        Else
                MessageBox.Show("No Changes Made")
        End If
    End Sub




    Public Sub UpdateStats()
        Dim total = 0.0
        Dim qty = 0
        For Each prodItem As Product In productDataTableGrid.Items
            total += (prodItem.ProductPrice * prodItem.Quantity)
            qty += prodItem.Quantity
        Next
        MessageBox.Show(total)
        MessageBox.Show(qty)
        runningTotal = total
        runningCount = qty
        totalManager.Text = "₱" + Format(runningTotal, "##,##0.00")
        totalCount.Text = runningCount
    End Sub



    Private Sub FLDqty_PreviewKeyDown(sender As Object, e As KeyEventArgs) Handles FLDqty.PreviewKeyDown
        Dim x As String
        x = e.Key

        If x = Key.Back Or x = Key.Left Or x = Key.Right Then
            e.Handled = False
        ElseIf (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x >= Key.NumPad0 And x <= Key.NumPad9) Or (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x >= Key.D0 And x <= Key.D9) Then
            e.Handled = False
        ElseIf e.Key = Key.Escape Then
            Me.Visibility = Visibility.Hidden
            FLDqty.Text = ""
            qtySetter = 0
            isSelectingQuantity = False
        ElseIf (e.Key = Key.Return) Then
            If FLDqty.Text = Nothing Then
                FLDqty.Text = 0
            ElseIf Int(FLDqty.Text) = 0 Then
                FLDqty.Text = 0
            ElseIf Int(FLDqty.Text) > productToAdd.UnitsAvailable Then
                MessageBox.Show("You only have: " & productToAdd.UnitsAvailable & " " & productToAdd.ProductName & " \n You cannot add more than the available stock")
            ElseIf isEditing Then

                Integer.TryParse(FLDqty.Text, qtySetter)
                productToAdd.Quantity = qtySetter
                evaluateEdit()
                isEditing = False
                FLDqty.Text = ""
                qtySetter = 0
                Me.Visibility = Visibility.Hidden
                cp.ShoppingCartTable.Items.Refresh()
                UpdateStats()
                productToAdd.UnitsAvailable = productTemplate.startUpQuantity
                productToAdd.UnitsAvailable -= productToAdd.Quantity
            ElseIf Not isEditing Then
                If productDataTableGrid.Items.Contains(productToAdd) Then

                    For i = 0 To productDataTableGrid.Items.Count - 1
                        productDataTableGrid.SelectedIndex = i
                        Dim row As New Product
                        row = CType(productDataTableGrid.SelectedItem, Product)

                        If productToAdd.Equals(row) Then
                            previousQTY = row.Quantity
                            addQuantityToItem(row)
                        End If
                        Me.Visibility = Visibility.Hidden
                        UpdateStats()
                        cp.ShoppingCartTable.Items.Refresh()
                        productToAdd.UnitsAvailable -= productToAdd.Quantity
                    Next
                ElseIf isFromEditButton Then
                    Integer.TryParse(FLDqty.Text, qtySetter)
                    productToAdd.Quantity = qtySetter
                    evaluateEdit()
                    isEditing = False
                    FLDqty.Text = ""
                    qtySetter = 0
                    Me.Visibility = Visibility.Hidden
                    cp.ShoppingCartTable.Items.Refresh()
                    UpdateStats()
                    productToAdd.UnitsAvailable -= productToAdd.Quantity
                Else
                    Integer.TryParse(FLDqty.Text, qtySetter)
                    productToAdd.Quantity = qtySetter
                    productDataTableGrid.Items.Add(productToAdd)
                    FLDqty.Text = ""
                    qtySetter = 0
                    Me.Visibility = Visibility.Hidden
                    cp.ShoppingCartTable.Items.Refresh()
                    UpdateStats()
                    productToAdd.UnitsAvailable -= productToAdd.Quantity
                End If
            End If
            FLDqty.Text = ""
        Else
            e.Handled = True
        End If

    End Sub

    Private Sub QTYClose_Click(sender As Object, e As RoutedEventArgs) Handles QTYClose.Click
        Me.Visibility = Visibility.Hidden
        FLDqty.Text = ""
    End Sub
End Class
