Imports System.ComponentModel

Public Class Customer
    Implements INotifyPropertyChanged


    Public Sub New()
        CustomerNumber = "defaultCustomerNumber"
        FName = "defaultFName"
        LName = "defaultLName"
        Add = "defaultAdd"
    End Sub

    Public Property FullName() As String
        Get
            Return LName & ", " & FName
        End Get
        Set(value As String)

        End Set
    End Property

    Private CustomerNumber As String
    Public Property CustomerID() As String
        Get
            Return CustomerNumber
        End Get
        Set(ByVal value As String)
            CustomerNumber = value
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

    Private Add As String
    Public Property Address() As String
        Get
            Return Add
        End Get
        Set(ByVal value As String)
            Add = value
        End Set
    End Property

    Public Event PropertyChanged As PropertyChangedEventHandler _
        Implements INotifyPropertyChanged.PropertyChanged

    Private Sub NotifyPropertyChanged(ByVal propertyName As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
    End Sub

End Class
