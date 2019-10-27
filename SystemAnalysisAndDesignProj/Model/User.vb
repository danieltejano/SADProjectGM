

Public Class User


    Public Sub New()

        AccountNumber = "defaultAccountNumber"
        UName = "defaultUName"
        Pass = "defaultPass"
        FName = "defaultFName"
        LName = "defaultLName"
        BDate = "defaultBDate"
        Add = "defaultAdd"
        LOA = "defaultLOA"
        SessionNumber = "SessionNumber"

    End Sub

    Private AccountNumber As String
    Public Property AccountID() As String
        Get
            Return AccountNumber
        End Get
        Set(ByVal value As String)
            AccountNumber = value
        End Set
    End Property

    Private UName As String
    Public Property UserName() As String
        Get
            Return UName
        End Get
        Set(ByVal value As String)
            UName = value
        End Set
    End Property

    Private Pass As String
    Public Property Password() As String
        Get
            Return Pass
        End Get
        Set(ByVal value As String)
            Pass = value
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

    Private BDate As String
    Public Property BirthDate() As String
        Get
            Return BDate
        End Get
        Set(ByVal value As String)
            BDate = value
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

    Private LOA As String
    Public Property LevelofAccess() As String
        Get
            Return LOA
        End Get
        Set(ByVal value As String)
            LOA = value
        End Set
    End Property

    Private SessionNumber As String
    Public Property SessionID() As String
        Get
            Return SessionNumber
        End Get
        Set(ByVal value As String)
            SessionNumber = value
        End Set
    End Property


End Class
