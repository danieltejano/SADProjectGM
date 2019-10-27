Imports System.ComponentModel

Public Class Logs
    Implements INotifyPropertyChanged

    Public Sub New()
        LogNumber = "defaultLogNumber"
        AccountNumber = "defaultAccountNumber"
        FName = "defaultFName"
        LName = "defaultLName"
        lDate = "defaultDat"
        LOA = "defaultLOA"
        ActTaken = "defaultActTaken"


    End Sub

    Private LogNumber As String
    Public Property LogID() As String
        Get
            Return LogNumber
        End Get
        Set(ByVal value As String)
            LogNumber = value
        End Set
    End Property

    Private AccountNumber As String
    Public Property AccountID() As String
        Get
            Return AccountNumber
        End Get
        Set(ByVal value As String)
            AccountNumber = value
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

    Private lDate As String
    Public Property LogDate() As String
        Get
            Return lDate
        End Get
        Set(ByVal value As String)
            lDate = value
        End Set
    End Property

    Private LOA As String
    Public Property LevelofAccess() As String
        Get
            Return LOA
        End Get
        Set(ByVal value As String)
            LOA = value
        End Set
    End Property

    Private ActTaken As String
    Public Property ActionTaken() As String
        Get
            Return ActTaken
        End Get
        Set(ByVal value As String)
            ActTaken = value
        End Set
    End Property

    Public Event PropertyChanged As PropertyChangedEventHandler _
        Implements INotifyPropertyChanged.PropertyChanged

    Private Sub NotifyPropertyChanged(ByVal propertyName As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
    End Sub
End Class
