Imports System.Data
Imports System.Data.OleDb
Class MainWindow

    Dim log_values As Char
    Dim DB As New ADODB.Connection
    Dim RS As New ADODB.Recordset
    Dim status As String = "TRUE"
    Dim LG As New ADODB.Connection
    Dim CL As New ADODB.Recordset





    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        FLDUsername.Clear()
        FLDPassword.Clear()
        FLDUsername.Focus()

        Dim table As String = "dataTable"
        Dim cons As String = connectionString
        Dim ds As New DataSet
        Dim cnn As OleDbConnection = New OleDbConnection(cons)
        Dim query As String = "Select * from Status"
        cnn.Open()
        Dim cmd As New OleDbCommand(query, cnn)
        Dim da As New OleDbDataAdapter(cmd)
        da.Fill(ds, table)
        cnn.Close()
        LG.Open(connectionString)
        CL.Open("SELECT * FROM Status WHERE LoggedIn='" & status & Chr(39), LG)
        Dim t1 As DataTable = ds.Tables(table)
        If (t1.Rows.Count <= 0) Then
            Me.Show()

        ElseIf (CL.Fields("LoggedIn").Value = status) Then
            UserType = CL.Fields("LevelofAccess").Value
            AccountId = CL.Fields("AccountID").Value
            Usrnm = CL.Fields("Username").Value

            RecordLog(accountID:=AccountId, loa:=UserType, actionTaken:="LOGGED IN")

            Try
                Using act2 As New OleDbConnection(connectionString)
                    act2.Open()
                    Dim command As New OleDbCommand("insert into STATUS ([AccountID],  [LevelofAccess], [Loggedin], [Username])  values ( @AccountId,  @access, @action, @user)", act2)
                    Dim dt As String
                    dt = Now.ToLongDateString & " / " & Now.ToLongTimeString
                    With command.Parameters
                        .AddWithValue("@AccountId", AccountId.ToUpper)
                        .AddWithValue("@access", UserType.ToString)
                        .AddWithValue("@action", "TRUE")
                        .AddWithValue("@user", Usrnm.ToString)
                    End With
                    command.ExecuteNonQuery()
                    command.Dispose()
                    act2.Close()
                End Using
            Catch ex As Exception
            End Try
            RecordLog(accountID:=AccountId, loa:=UserType, actionTaken:="LOGGED IN")
            Me.Hide()
            Dim mainmenu As New MainMenu
            mainmenu.Show()


        Else
            LG.Close()
        End If


    End Sub

    Private Sub BTNLogin_Click(sender As Object, e As RoutedEventArgs) Handles BTNLogin.Click
        DB.Open(connectionString)
        RS.Open("SELECT * FROM tblUsers WHERE Username='" & FLDUsername.Text & Chr(39), DB)
        Try
            If (RS.Fields("Username").Value = FLDUsername.Text) And (RS.Fields("Password").Value = FLDPassword.Password) Then
                UserType = RS.Fields("LevelofAccess").Value
                AccountId = RS.Fields("AccountID").Value
                Usrnm = RS.Fields("Username").Value
                Me.Hide()
                Dim mainmenu As New MainMenu
                mainmenu.Show()
            Else
                MessageBox.Show("Invalid Credentials")

            End If
        Catch ex As Exception
            MessageBox.Show("Account not Found")
        End Try

        RS.Close()
        DB.Close()

        Try
            Using act As New OleDbConnection(connectionString)
                act.Open()
                Dim command As New OleDbCommand("insert into Logs ([AccountID], [ADate], [LevelofAccess], [Action])  values ( @AccountId, @dates, @access, @action)", act)
                Dim dt As String
                dt = Now.ToLongDateString & " / " & Now.ToLongTimeString
                With command.Parameters
                    .AddWithValue("@AccountId", AccountId.ToUpper)
                    .AddWithValue("@dates", dt.ToString)
                    .AddWithValue("@access", UserType.ToString)
                    .AddWithValue("@action", "LOGGED IN")
                End With
                command.ExecuteNonQuery()
                command.Dispose()
                act.Close()
            End Using
        Catch ex As Exception
        End Try
        Try
            Using act2 As New OleDbConnection(connectionString)
                act2.Open()
                Dim command As New OleDbCommand("insert into STATUS ([AccountID],  [LevelofAccess], [Loggedin], [Username])  values ( @AccountId,  @access, @action, @user)", act2)
                Dim dt As String
                dt = Now.ToLongDateString & " / " & Now.ToLongTimeString
                With command.Parameters
                    .AddWithValue("@AccountId", AccountId.ToUpper)
                    .AddWithValue("@access", UserType.ToString)
                    .AddWithValue("@action", "TRUE")
                    .AddWithValue("@user", Usrnm.ToString)
                End With
                command.ExecuteNonQuery()
                command.Dispose()
                act2.Close()
            End Using
        Catch ex As Exception
        End Try
    End Sub

    Private Sub BTNClear_Click(sender As Object, e As RoutedEventArgs) Handles BTNClear.Click
        FLDUsername.Clear()
        FLDPassword.Clear()
        FLDUsername.Focus()
    End Sub




    Private Sub BTNClose_Click(sender As Object, e As RoutedEventArgs) Handles BTNClose.Click
        CloseProgram(Me)
    End Sub

    Private Sub FLDPassword_KeyDown(sender As Object, e As KeyEventArgs) Handles FLDPassword.KeyDown
        If (e.Key = Key.Enter) Then
            BTNLogin.IsDefault = True
        End If
    End Sub



    Private Sub MainWindow_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If (e.Key = Key.Escape) Then
            If (MessageBox.Show("Close Application?", "Exit", MessageBoxButton.YesNo, MessageBoxImage.Warning) = MessageBoxResult.No) Then
                Me.BringIntoView()
                If FLDUsername.Text = "" Then
                    FLDUsername.Focus()
                Else
                    FLDPassword.Focus()
                End If
            Else
                Me.Close()
            End If
        End If
    End Sub
End Class
