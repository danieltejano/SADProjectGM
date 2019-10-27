Imports System.ComponentModel


Public Class CashierTransaction
    Implements INotifyPropertyChanged

    Public Sub New()
        CashierTransacNumber = "defaultCashierTransacNumber"
        AccountNumber = "defaultAccountNumber"
        ProductNumber = "defaultProductNumber"
        TransactionDate = "defaultTransactDate"
        IsPartialPayment = "defaultIsPartialPayment"
        IsFullPayment = "defaultIsFullPaymennt"
        TotalPrice = "defaultTotalPrice"
        TaxTotal = "defaultTaxTotal"
        TenderAmount = "defaultTenderAmount"
        Change = "defaultChange"
        ORNumber = "defaultORNumber"
        ToDeliver = "defaultToDeliver"
        Address = "defaultAddress"
        DeliveryFee = "defaultDeliveryFee"

    End Sub

    Private CashierTransacNumber As String
    Public Property CashierTransactionID() As String
        Get
            Return CashierTransacNumber
        End Get
        Set(ByVal value As String)
            CashierTransacNumber = value
        End Set
    End Property

    Private ProductQuantity As Integer
    Public Property Quantity As Integer
        Get
            Return ProductQuantity
        End Get
        Set(value As Integer)
            ProductQuantity = value
        End Set
    End Property

    Private AccountNumber As String
    Public Property AccountId() As String
        Get
            Return AccountNumber
        End Get
        Set(ByVal value As String)
            AccountNumber = value
        End Set
    End Property

    Private ProductNumber As String
    Public Property ProductId() As String
        Get
            Return ProductNumber
        End Get
        Set(ByVal value As String)
            ProductNumber = value
        End Set
    End Property

    Private TransactionDate As String
    Public Property TransDate() As String
        Get
            Return TransactionDate
        End Get
        Set(ByVal value As String)
            TransactionDate = value
        End Set
    End Property

    Private IsPartialPayment As String
    Public Property IsPartialPay() As String
        Get
            Return IsPartialPayment
        End Get
        Set(ByVal value As String)
            IsPartialPayment = value
        End Set
    End Property

    Private IsFullPayment As String
    Public Property IsFullPay() As String
        Get
            Return IsFullPayment
        End Get
        Set(ByVal value As String)
            IsFullPayment = value
        End Set
    End Property

    Private TotalPrice As String
    Public Property TotPay() As String
        Get
            Return TotalPrice
        End Get
        Set(ByVal value As String)
            TotalPrice = value
        End Set
    End Property

    Private TaxTotal As String
    Public Property TaxTot() As String
        Get
            Return TaxTotal
        End Get
        Set(ByVal value As String)
            TaxTotal = value
        End Set
    End Property

    Private TenderAmount As String
    Public Property TendPay() As String
        Get
            Return TenderAmount
        End Get
        Set(ByVal value As String)
            TenderAmount = value
        End Set
    End Property

    Private Change As String
    Public Property Sukli() As String
        Get
            Return Change
        End Get
        Set(ByVal value As String)
            Change = value
        End Set
    End Property

    Private ORNumber As String
    Public Property ORID() As String
        Get
            Return ORNumber
        End Get
        Set(ByVal value As String)
            ORNumber = value
        End Set
    End Property

    Private ToDeliver As String
    Public Property ToDeliv() As String
        Get
            Return ToDeliver
        End Get
        Set(ByVal value As String)
            ToDeliver = value
        End Set
    End Property

    Private Address As String
    Public Property Add() As String
        Get
            Return Address
        End Get
        Set(ByVal value As String)
            Address = value
        End Set
    End Property

    Private DeliveryFee As String
    Public Property DelivFee() As String
        Get
            Return DeliveryFee
        End Get
        Set(ByVal value As String)
            DeliveryFee = value
        End Set
    End Property

    Public Event PropertyChanged As PropertyChangedEventHandler _
        Implements INotifyPropertyChanged.PropertyChanged

    Private Sub NotifyPropertyChanged(ByVal propertyName As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
    End Sub

End Class
