Imports System.ComponentModel

Public Class DeliveryJob
    Implements INotifyPropertyChanged

    Public Sub New()
        DeliveryNumber = "defaultDeliveryNumber"
        CashierTransacID = "defaultCashierTransacID"
        ORNum = "defaultORNum"
        CustAddress = "defaultCustAddress"
        CustSignature = "defaultCustSignature"
        DeliveryNumber = "defaultDelivery"
        DeliverymanNumber = "defaultDeliverymanNumber"

    End Sub

    Private DeliveryNumber As String
    Public Property DeliveryID() As String
        Get
            Return DeliveryNumber
        End Get
        Set(ByVal value As String)
            DeliveryNumber = value
        End Set
    End Property

    Private CashierTransacID As String
    Public Property CashierTransactionID() As String
        Get
            Return CashierTransacID
        End Get
        Set(ByVal value As String)
            CashierTransacID = value
        End Set
    End Property

    Private ORNum As String
    Public Property ORNumber() As String
        Get
            Return ORNum
        End Get
        Set(ByVal value As String)
            ORNum = value
        End Set
    End Property

    Private CustAddress As String
    Public Property CustomerAddress() As String
        Get
            Return CustAddress
        End Get
        Set(ByVal value As String)
            CustAddress = value
        End Set
    End Property

    Private CustSignature As String
    Public Property CustomerSignature() As String
        Get
            Return CustSignature
        End Get
        Set(ByVal value As String)
            CustSignature = value
        End Set
    End Property

    Private DeliverymanNumber As String
    Public Property DeliverymnaID() As String
        Get
            Return DeliverymanNumber
        End Get
        Set(ByVal value As String)
            DeliverymanNumber = value
        End Set
    End Property

    Public Event PropertyChanged As PropertyChangedEventHandler _
        Implements INotifyPropertyChanged.PropertyChanged

    Private Sub NotifyPropertyChanged(ByVal propertyName As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
    End Sub



End Class
