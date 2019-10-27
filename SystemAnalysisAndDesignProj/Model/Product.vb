Imports System.ComponentModel
Imports System.Data
Imports System.Data.OleDb
Imports ADODB




Public Class Product


    Public Sub New()

        ProductNumber = "defaultProductNumber"
        ProdName = "defaultProdName"
        ProdPrice = "defaultProdPrice"
        SoldUnits = "defaultSold"
        Ctg = "defaultCtg"
        AvailableUnits = "defaultAvailableUnits"

        SupplierNumber = "defaultSupplierNumber"

    End Sub


    Private ProdQty As Integer
    Public Property Quantity As Integer
        Get
            Return ProdQty
        End Get
        Set(value As Integer)
            ProdQty = value
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

    Private ProdName As String
    Public Property ProductName() As String
        Get
            Return ProdName
        End Get
        Set(ByVal value As String)
            ProdName = value
        End Set
    End Property

    Private ProdPriceWithCurrency As String
    Public Property ProductCurrencyPrice() As String
        Get
            Dim intPrice As Integer = 0
            Integer.TryParse(ProdPrice, intPrice)

            Return "₱" + Format(intPrice, "##,##0.00")
        End Get
        Set(value As String)
            Return
        End Set
    End Property

    Private ProdPrice As String
    Public Property ProductPrice() As String
        Get
            Return ProdPrice
        End Get
        Set(ByVal value As String)
            ProdPrice = value
        End Set
    End Property

    Private SoldUnits As String
    Public Property UnitSold() As String
        Get
            Return SoldUnits
        End Get
        Set(ByVal value As String)
            SoldUnits = value
        End Set
    End Property

    Private Ctg As String
    Public Property Category() As String
        Get
            Return Ctg
        End Get
        Set(ByVal value As String)
            Ctg = value
        End Set
    End Property

    Private AvailableUnits As String
    Public Property UnitsAvailable() As String
        Get
            Return AvailableUnits
        End Get
        Set(ByVal value As String)
            AvailableUnits = value
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

    Private stckID As String
    Public Property StockID() As String
        Get
            Return stckID
        End Get
        Set(value As String)
            stckID = value
        End Set
    End Property

    'Public Event PropertyChanged As PropertyChangedEventHandler _
    '        Implements INotifyPropertyChanged.PropertyChanged

    'Private Sub NotifyPropertyChanged(ByVal propertyName As String)
    '    RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
    'End Sub

End Class
