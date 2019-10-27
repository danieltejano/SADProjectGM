Public Class ShoppingCartItem
    Public product As New Product

    Private Sub ShoppingCartItem_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Me.DataContext = product
    End Sub
End Class
