Imports System.ComponentModel

Public Class AccountsViewModel
    Implements INotifyPropertyChanged

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Public Property Accounts As List(Of User)
        Get
            Return Accounts
        End Get
        Set(value As List(Of User))
            Accounts = value
            NotifyPropertyChanged("Accounts")
        End Set
    End Property

    Private Sub NotifyPropertyChanged(ByVal info As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(info))
    End Sub


    Dim dummyAccount As New User With {.AccountID = "00014"}


End Class
