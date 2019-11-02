Imports System.Data
Imports System.Windows.Media.Animation
Imports System.Windows.Media

Public Class ProductTemplate
    Public Product As New Product
    Public productDataTable As DataTable
    Public productDataTableGrid As DataGrid
    Public quantity As Integer
    Public startUpQuantity As Integer
    Public totalManager As TextBlock
    Public totalCount As TextBlock
    Public qtyManager As Quantity_Selector
    Public checkoutBTN As Button
    Public checkoutFadeIn As Storyboard
    Public checoutFadeOut As Storyboard

    Public Sub PlayPopUp()
        Dim sb As Storyboard = TryCast(Me.FindResource("PopUp"), Storyboard)
        sb.Begin()
    End Sub

    Private Sub ProductButton_MouseEnter(sender As Object, e As MouseEventArgs) Handles ProductButton.MouseEnter
        PlayPopUp()
    End Sub

    Private Sub ProductButton_Click(sender As Object, e As RoutedEventArgs) Handles ProductButton.Click


        Dim data = New String() {Product.ProductID, Product.ProductName}


        qtyManager.productToAdd = Product
        qtyManager.totalManager = totalManager
        qtyManager.totalCount = totalCount
        qtyManager.productDataTableGrid = productDataTableGrid
        qtyManager.Visibility = Visibility.Visible
        qtyManager.DataContext = qtyManager.productToAdd
        qtyManager.FLDqty.Focus()
        qtyManager.productTemplate = Me
    End Sub

    Private Sub ProductTemplate_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        If Product.UnitsAvailable = 0 Then
            Me.IsEnabled = False
            Price.Foreground = New SolidColorBrush(ColorConverter.ConvertFromString("#FFB95D5D"))
            ProductName.Foreground = New SolidColorBrush(ColorConverter.ConvertFromString("#FFB95D5D"))
            ProductName.Text = "No stock Available, " + ProductName.Text
        ElseIf Product.UnitsAvailable < 10 Then
            Price.Foreground = New SolidColorBrush(ColorConverter.ConvertFromString("#FFB9B445"))
            ProductName.Foreground = New SolidColorBrush(ColorConverter.ConvertFromString("#FFB9B445"))
        Else
            PlayPopUp()
        End If
        startUpQuantity = Product.UnitsAvailable
    End Sub
End Class
