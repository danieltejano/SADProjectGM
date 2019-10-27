Imports System.ComponentModel

Public Class Supplier
    Implements INotifyPropertyChanged
    Public Sub New()

        SupplierNumber = "defaultSupplierNumber"
        SuppName = "defaultSuppName"
        SuppAdd = "defaultSuppAdd"


    End Sub

    Private SupplierNumber As String
    Public Property SupplierID() As String
        Get
            Return SupplierNumber
        End Get
        Set(ByVal value As String)
            SupplierNumber = value
        End Set
    End Property

    Private SuppName As String
    Public Property SupplierName() As String
        Get
            Return SuppName
        End Get
        Set(ByVal value As String)
            SuppName = value
        End Set
    End Property

    Private SuppAdd As String
    Public Property SupplierAddress() As String
        Get
            Return SuppAdd
        End Get
        Set(ByVal value As String)
            SuppAdd = value
        End Set
    End Property

    Public Event PropertyChanged As PropertyChangedEventHandler _
        Implements INotifyPropertyChanged.PropertyChanged

    Private Sub NotifyPropertyChanged(ByVal propertyName As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
    End Sub

End Class
