Imports System.ComponentModel

Public Class DeliveryMan
    Implements INotifyPropertyChanged

    Public Sub New()
        DeliverymanNumber = "defaultDeliverymanNumber"
        FName = "defaultFname"
        LName = "defaultLName"
        OnDeliver = "defaultOnDelivery"
        ContactNum = "defaultContactNum"


    End Sub
    Private DeliverymanNumber As String
    Public Property DeliverymanID() As String
        Get
            Return DeliverymanNumber
        End Get
        Set(ByVal value As String)
            DeliverymanNumber = value
        End Set
    End Property

    Private FName As String
    Public Property FirstName() As String
        Get
            Return FName
        End Get
        Set(ByVal value As String)
            FName = value
        End Set
    End Property

    Private LName As String
    Public Property LastName() As String
        Get
            Return LName
        End Get
        Set(ByVal value As String)
            LName = value
        End Set
    End Property

    Private OnDeliver As String
    Public Property OnDelivery() As String
        Get
            Return OnDeliver
        End Get
        Set(ByVal value As String)
            OnDeliver = value
        End Set
    End Property

    Private ContactNum As String
    Public Property ContactNumber() As String
        Get
            Return ContactNum
        End Get
        Set(ByVal value As String)
            ContactNum = value
        End Set
    End Property

    Public Event PropertyChanged As PropertyChangedEventHandler _
        Implements INotifyPropertyChanged.PropertyChanged

    Private Sub NotifyPropertyChanged(ByVal propertyName As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
    End Sub

End Class
