Imports System.ComponentModel

Public Class Stock
    Implements INotifyPropertyChanged
    Public Sub New()

        StockNumber = "defaultStockNumber"
        ProductNumber = "defaultProductNumber"
        Quant = "defaultQuant"
        sDat = "defaultDat"
        SupplierNumber = "defaultSupplierNumber"
        StockPrice = "defaultStockPrice"
        TaxStock = "defaultTaxStock"

    End Sub

    Private StockNumber As String
    Public Property StockID() As String
        Get
            Return StockNumber
        End Get
        Set(ByVal value As String)
            StockNumber = value
        End Set
    End Property
    Private ProductNumber As String
    Public Property ProductID() As String
        Get
            Return ProductNumber
        End Get
        Set(ByVal value As String)
            ProductNumber = value
        End Set
    End Property

    Private Quant As String
    Public Property Quantity() As String
        Get
            Return Quant
        End Get
        Set(ByVal value As String)
            Quant = value
        End Set
    End Property

    Private sDat As String
    Public Property StockDate() As String
        Get
            Return sDat
        End Get
        Set(ByVal value As String)
            sDat = value
        End Set
    End Property

    Private SupplierNumber As String
    Public Property SupplierID() As String
        Get
            Return SupplierNumber
        End Get
        Set(ByVal value As String)
            SupplierNumber = value
        End Set
    End Property

    Private StockPrice As String
    Public Property StockPayment() As String
        Get
            Return StockPrice
        End Get
        Set(ByVal value As String)
            StockPrice = value
        End Set
    End Property

    Private TaxStock As String
    Public Property StockTax() As String
        Get
            Return TaxStock
        End Get
        Set(ByVal value As String)
            TaxStock = value
        End Set
    End Property

    Public Event PropertyChanged As PropertyChangedEventHandler _
        Implements INotifyPropertyChanged.PropertyChanged

    Private Sub NotifyPropertyChanged(ByVal propertyName As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
    End Sub

End Class
