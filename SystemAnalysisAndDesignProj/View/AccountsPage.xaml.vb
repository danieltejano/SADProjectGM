Imports System.Data
Imports System.Data.OleDb
Imports SystemAnalysisAndDesignProj.MainMenu
Class AccountsPage
    Inherits Page
    Dim log_values As Char
    Dim DB As New ADODB.Connection
    Dim RS As New ADODB.Recordset

    Public Sub Restrictions()
        stklbl.Visibility = Visibility.Hidden
        stktxt.Visibility = Visibility.Hidden
        buttonSave.Visibility = Visibility.Hidden
        buttonSave.IsEnabled = False
        buttonCancel.Visibility = Visibility.Hidden
        buttonCancel.IsEnabled = False
        buttonDelete.IsEnabled = False
        buttonEdit.IsEnabled = False
        buttonAdd.IsEnabled = True
    End Sub

    Public Sub AddEdit()
        stklbl.Visibility = Visibility.Visible
        stktxt.Visibility = Visibility.Visible
        buttonSave.Visibility = Visibility.Visible
        buttonSave.IsEnabled = True
        buttonCancel.Visibility = Visibility.Visible
        buttonCancel.IsEnabled = True
        buttonDelete.IsEnabled = False
        buttonEdit.IsEnabled = False
        buttonAdd.IsEnabled = False
        stklbl.IsEnabled = True
        stktxt.IsEnabled = True
    End Sub

    Private Sub AccountsPage_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        PullDataFromDatabase(d:=GRDAccounts, tableName:="tblUsers")
        CBLoa.Items.Add("Administrator")                        'Adds "Administrator" a Combobox Item in the Level of Access (Loa) Combobox
        CBLoa.Items.Add("Cashier")                             'Adds "Cashiers" a ComboBox Item in the Level of Access (Loa) ComboBox
        Restrictions()
        STKBtn.IsEnabled = True
        CMBCategory.Items.Clear()
        CMBCategory.Items.Add("")
        CMBCategory.Items.Add("Administrator")
        CMBCategory.Items.Add("Cashier")
        mm.CheckStocks()
    End Sub

    Private Sub buttonAdd_Click(sender As Object, e As RoutedEventArgs) Handles buttonAdd.Click
        FLDAct.IsEnabled = False
        AddEdit()
        GRDAccounts.UnselectAllCells()
        FLDAct.Clear()
        FLDAdr.Clear()
        FLDFn.Clear()
        FLDLn.Clear()
        FLDPsw.Clear()
        FLDUsr.Clear()
        DPBd.Text = DateTime.Today.AddYears(-18)
        CBLoa.Items.Clear()
        CBLoa.Items.Add("Administrator")
        CBLoa.Items.Add("Cashier")
        GRDAccounts.IsHitTestVisible = False
        buttonSave.Content = "SAVE"
        FLDUsr.Focus()
    End Sub

    Private Sub buttonDelete_Click(sender As Object, e As RoutedEventArgs) Handles buttonDelete.Click
        If CBLoa.Text = "Administrator" Then
            MessageBox.Show("Administrator account cannot be deleted.")
        ElseIf MsgBox("Are you sure you want to Delete?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Message") = MsgBoxResult.Yes Then
            Try
                DB.Open(connectionString)
                DB.Execute("Delete * From tblUsers Where Username= '" & FLDUsr.Text & "'")
                MessageBox.Show("Record was Deleted", "SYSTEM")
                DB.Close()
                RecordLog(accountID:=AccountId, loa:=UserType, actionTaken:="ACCOUNT DELETED")
            Catch EX As Exception
            End Try
        End If
        PullDataFromDatabase(d:=GRDAccounts, tableName:="tblUsers")
        FLDAct.Visibility = Visibility.Visible
        Restrictions()
    End Sub

    Private Sub buttonEdit_Click(sender As Object, e As RoutedEventArgs) Handles buttonEdit.Click
        FLDAct.IsEnabled = False
        AddEdit()
        GRDAccounts.IsHitTestVisible = False
        buttonSave.Content = "UPDATE"
        FLDUsr.Focus()
    End Sub

    Private Sub buttonSave_Click(sender As Object, e As RoutedEventArgs) Handles buttonSave.Click
        If buttonSave.Content = "SAVE" Then
            GRDAccounts.IsHitTestVisible = True
            Dim A As New ADODB.Connection
            Dim B As New ADODB.Recordset
            A.Open(connectionString)
            B.Open("Select * from tblUsers where Username='" & FLDUsr.Text & "'", A)
            Try
                If FLDUsr.Text = "" Or FLDPsw.Text = "" Or FLDFn.Text = "" Or FLDLn.Text = "" Or DPBd.Text = "" Or CBLoa.Text = "" Then
                    MessageBox.Show("All Fields under User Account is needed to be filled out")
                ElseIf FLDAdr.Text.Length <= 10 Then
                    MsgBox("Address too short")
                ElseIf FLDUsr.Text = B.Fields("Username").Value Then
                    MessageBox.Show("Unable to have the same Username")
                End If
            Catch ex As Exception
                With B
                    .Close()
                    .Open("tblUsers", A, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)
                    .MoveLast()
                    .AddNew()
                    .Fields("Username").Value = FLDUsr.Text
                    .Fields("Password").Value = FLDPsw.Text
                    .Fields("FirstName").Value = FLDFn.Text
                    .Fields("LastName").Value = FLDLn.Text
                    .Fields("Birthdate").Value = DPBd.SelectedDate
                    .Fields("Address").Value = FLDAdr.Text
                    .Fields("LevelofAccess").Value = CBLoa.SelectedItem
                    .Update()
                    .Close()
                End With
                A.Close()
                Restrictions()
                FLDAct.Visibility = Visibility.Visible
                RecordLog(accountID:=AccountId, loa:=UserType, actionTaken:="AccountAdded")
            End Try
        End If

        If buttonSave.Content = "UPDATE" Then
            GRDAccounts.IsHitTestVisible = True
            Dim A As New ADODB.Connection
            Dim B As New ADODB.Recordset
            A.Open(connectionString)
            B.Open("Select * from tblUsers where AccountID=" & FLDAct.Text & "", A)
            If FLDUsr.Text = "" Or FLDPsw.Text = "" Or FLDFn.Text = "" Or FLDLn.Text = "" Or DPBd.Text = "" Or FLDAdr.Text = "" Or CBLoa.Text = "" Then
                MessageBox.Show("All Fields under User Account is needed to be filled out")
            ElseIf B.Fields("Username").Value = FLDUsr.Text And B.Fields("Password").Value = FLDPsw.Text And B.Fields("Firstname").Value = FLDFn.Text And B.Fields("Lastname").Value = FLDLn.Text And B.Fields("Birthdate").Value = DPBd.Text And B.Fields("Address").Value = FLDAdr.Text And B.Fields("LevelofAccess").Value = CBLoa.Text Then
                MessageBox.Show("No changes made.", "SYSTEM")
            ElseIf FLDAdr.Text.Length <= 10 Then
                MsgBox("Address too short")
            ElseIf FLDUsr.Text <> B.Fields("Username").Value Then
                B.Close()
                B.Open("Select * from tblUsers where Username='" & FLDUsr.Text & "'", A)
                Try
                    If FLDUsr.Text = B.Fields("Username").Value Then
                        MessageBox.Show("User name already taken. Please type another one.", "SYSTEM")
                        B.Close()
                    End If
                Catch ex As Exception
                    With B
                        .Close()
                        .Open("tblUsers", A, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)
                        .MoveFirst()
                        .Find("AccountID='" & FLDAct.Text & "'")
                        .Fields("Username").Value = FLDUsr.Text                                                                                     'Creates new record with the Username = to the text of the textbox
                        .Fields("Password").Value = FLDPsw.Text                                                                                     'Creates new record with the Password = to the text of the textbox
                        .Fields("FirstName").Value = FLDFn.Text                                                                                     'Creates new record with the First Name = to the text of the textbox                                                                                         
                        .Fields("LastName").Value = FLDLn.Text                                                                                      'Creates new record with the Last Name = to the text of the textbox
                        .Fields("Birthdate").Value = DPBd.SelectedDate                                                                           'Creates new record with the SelectedDate = to the text of the textbox
                        .Fields("Address").Value = FLDAdr.Text                                                                                      'Creates new record with the Address = to the text of the textbox
                        .Fields("LevelofAccess").Value = CBLoa.SelectedItem                                                                         'Creates new record with the Level Of Access = to the text of the textbox
                        .Update()
                        MessageBox.Show("Account has been successfully updated", "SYSTEM")
                        RecordLog(accountID:=AccountId, loa:=UserType, actionTaken:="Account Updated")
                    End With
                End Try
            Else
                With B
                    .Close()
                    .Open("tblUsers", A, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)
                    .MoveFirst()
                    .Find("AccountID='" & FLDAct.Text & "'")
                    .Fields("Username").Value = FLDUsr.Text
                    .Fields("Password").Value = FLDPsw.Text
                    .Fields("FirstName").Value = FLDFn.Text
                    .Fields("LastName").Value = FLDLn.Text
                    .Fields("Birthdate").Value = DPBd.SelectedDate
                    .Fields("Address").Value = FLDAdr.Text
                    .Fields("LevelofAccess").Value = CBLoa.SelectedItem
                    .Update()
                    MessageBox.Show("Account has been successfully updated", "SYSTEM")
                    RecordLog(accountID:=AccountId, loa:=UserType, actionTaken:="Account Updated")
                End With
            End If
            A.Close()
            Restrictions()
            FLDAct.Visibility = Visibility.Visible

        End If
        PullDataFromDatabase(d:=GRDAccounts, tableName:="tblUsers")
    End Sub

    Private Sub GRDAccounts_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles GRDAccounts.SelectionChanged
        GRDAccounts.CanUserSortColumns = False
        If GRDAccounts.SelectedIndex >= 0 Then
            stklbl.Visibility = Visibility.Visible
            stktxt.Visibility = Visibility.Visible
            stklbl.IsEnabled = False
            stktxt.IsEnabled = False
            buttonEdit.IsEnabled = True
            buttonDelete.IsEnabled = True
            Dim selectedRowIndex = GRDAccounts.SelectedIndex                                                                                                   'gets the index of the currentRow Selected                                                                                                           'gets the rowCount of the whole datagrid    
            Dim act As TextBlock = TryCast(GRDAccounts.Columns(0).GetCellContent(GRDAccounts.Items(selectedRowIndex)), TextBlock)                                   'creates a temporary textblock that will hold the value of the cell
            Dim usr As TextBlock = TryCast(GRDAccounts.Columns(1).GetCellContent(GRDAccounts.Items(selectedRowIndex)), TextBlock)
            Dim psw As TextBlock = TryCast(GRDAccounts.Columns(2).GetCellContent(GRDAccounts.Items(selectedRowIndex)), TextBlock)
            Dim fn As TextBlock = TryCast(GRDAccounts.Columns(3).GetCellContent(GRDAccounts.Items(selectedRowIndex)), TextBlock)
            Dim ln As TextBlock = TryCast(GRDAccounts.Columns(4).GetCellContent(GRDAccounts.Items(selectedRowIndex)), TextBlock)
            Dim bd As TextBlock = TryCast(GRDAccounts.Columns(5).GetCellContent(GRDAccounts.Items(selectedRowIndex)), TextBlock)
            Dim adr As TextBlock = TryCast(GRDAccounts.Columns(6).GetCellContent(GRDAccounts.Items(selectedRowIndex)), TextBlock)
            Dim loa As TextBlock = TryCast(GRDAccounts.Columns(7).GetCellContent(GRDAccounts.Items(selectedRowIndex)), TextBlock)
            FLDAct.Text = act.Text
            FLDUsr.Text = usr.Text
            FLDPsw.Text = psw.Text
            FLDFn.Text = fn.Text
            FLDLn.Text = ln.Text
            DPBd.Text = bd.Text
            FLDAdr.Text = adr.Text
            CBLoa.Text = loa.Text
        End If
    End Sub

    Private Sub buttonCancel_Click(sender As Object, e As RoutedEventArgs) Handles buttonCancel.Click
        Dim can As String
        can = MessageBox.Show("Do you want to cancel ?", "SYSTEM", MessageBoxButton.YesNo, MessageBoxImage.Exclamation)
        If can = vbYes Then
            GRDAccounts.IsHitTestVisible = True
            Restrictions()
            GRDAccounts.UnselectAllCells()
            FLDAct.Visibility = Visibility.Visible
        End If
    End Sub

    Private Sub SearchTextBox_TextChanged(sender As Object, e As TextChangedEventArgs) Handles SearchTextBox.TextChanged
        CMBCategory.SelectedIndex = 0
        Dim oleDatabaseConnection As New OleDb.OleDbConnection(connectionString)
        oleDatabaseConnection.Open()
        Dim databasez As New OleDbCommand
        databasez.CommandText = "Select *  From tblUsers Where UserName Like '%" & SearchTextBox.Text & "%'"
        databasez.Connection = oleDatabaseConnection
        Dim databaseActualTable As OleDbDataReader = databasez.ExecuteReader()
        GRDAccounts.ItemsSource = databaseActualTable
        GRDAccounts.Items.Refresh()
    End Sub



    Private Sub CMBCategory_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles CMBCategory.SelectionChanged
        Dim oleDatabaseConnection As New OleDb.OleDbConnection(connectionString)
        oleDatabaseConnection.Open()
        Dim databasez As New OleDbCommand
        If CMBCategory.SelectedIndex = 0 Then
            databasez.CommandText = "Select *  From tblUsers"
            databasez.Connection = oleDatabaseConnection
            Dim databaseActualTable As OleDbDataReader = databasez.ExecuteReader()
            GRDAccounts.ItemsSource = databaseActualTable
            GRDAccounts.Items.Refresh()
        ElseIf CMBCategory.SelectedIndex = 1 Then
            databasez.CommandText = "Select *  From tblUsers Where LevelofAccess='Administrator'"
            databasez.Connection = oleDatabaseConnection
            Dim databaseActualTable As OleDbDataReader = databasez.ExecuteReader()
            GRDAccounts.ItemsSource = databaseActualTable
            GRDAccounts.Items.Refresh()
        ElseIf CMBCategory.SelectedIndex = 2 Then
            databasez.CommandText = "Select *  From tblUsers Where LevelofAccess='Cashier'"
            databasez.Connection = oleDatabaseConnection
            Dim databaseActualTable As OleDbDataReader = databasez.ExecuteReader()
            GRDAccounts.ItemsSource = databaseActualTable
            GRDAccounts.Items.Refresh()
        End If

    End Sub

    Private Sub GRDAccounts_AutoGeneratingColumn(sender As Object, e As DataGridAutoGeneratingColumnEventArgs) Handles GRDAccounts.AutoGeneratingColumn
        If (e.PropertyName = "Birthdate") Then
            Dim column As DataGridTextColumn = e.Column
            Dim binding As Binding = column.Binding
            binding.StringFormat = "MMMMM-dd-yyyy"
        End If
    End Sub


#Region "keypress"
    Private Sub FLDUsr_PreviewKeyDown(sender As Object, e As KeyEventArgs) Handles FLDUsr.PreviewKeyDown
        Dim x As String
        x = e.Key
        If x = Key.Back Or x = Key.Left Or x = Key.Right Then
            e.Handled = False
        ElseIf (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x >= Key.NumPad0 And x <= Key.NumPad9) Or (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x >= Key.D0 And x <= Key.D9) Or (x >= Key.A And x <= Key.Z) Or (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x = Key.OemMinus) Then
            e.Handled = False
        ElseIf x = Key.Tab Then
            FLDUsr.Focus()
        ElseIf x = Key.Enter Or x = Key.Down Then
            FLDPsw.Focus()
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub FLDPsw_PreviewKeyDown(sender As Object, e As KeyEventArgs) Handles FLDPsw.PreviewKeyDown
        Dim x As String
        x = e.Key
        If x = Key.Back Or x = Key.Left Or x = Key.Right Then
            e.Handled = False
        ElseIf (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x >= Key.NumPad0 And x <= Key.NumPad9) Or (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x >= Key.D0 And x <= Key.D9) Or (x >= Key.A And x <= Key.Z) Then
            e.Handled = False
        ElseIf x = Key.Tab Then
            FLDPsw.Focus()
        ElseIf x = Key.Enter Or x = Key.Down Then
            FLDFn.Focus()
        ElseIf x = Key.Up Then
            FLDUsr.Focus()
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub FLDFn_PreviewKeyDown(sender As Object, e As KeyEventArgs) Handles FLDFn.PreviewKeyDown
        Dim x As String
        x = e.Key
        If x = Key.Space Or x = Key.Back Or x = Key.Left Or x = Key.Right Then
            e.Handled = False
        ElseIf (x >= Key.A And x <= Key.Z) Or (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x = Key.OemMinus) Or (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x = Key.OemPeriod) Then
            e.Handled = False
        ElseIf x = Key.Tab Then
            FLDFn.Focus()
        ElseIf x = Key.Enter Or x = Key.Down Then
            FLDLn.Focus()
        ElseIf x = Key.Up Then
            FLDPsw.Focus()
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub FLDLn_PreviewKeyDown(sender As Object, e As KeyEventArgs) Handles FLDLn.PreviewKeyDown
        Dim x As String
        x = e.Key
        If x = Key.Space Or x = Key.Back Or x = Key.Left Or x = Key.Right Then
            e.Handled = False
        ElseIf (x >= Key.A And x <= Key.Z) Or (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x = Key.OemMinus) Or (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x = Key.OemPeriod) Then
            e.Handled = False
        ElseIf x = Key.Tab Then
            FLDLn.Focus()
        ElseIf x = Key.Enter Or x = Key.Down Then
            DPBd.Focus()
        ElseIf x = Key.Up Then
            FLDFn.Focus()
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub DPBd_PreviewKeyDown(sender As Object, e As KeyEventArgs) Handles DPBd.PreviewKeyDown
        Dim x As String
        x = e.Key
        If x = Key.Back Or x = Key.Left Or x = Key.Right Then
            e.Handled = False
        ElseIf (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x >= Key.NumPad0 And x <= Key.NumPad9) Or (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x >= Key.D0 And x <= Key.D9) Or (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x = Key.OemQuestion) Then
            e.Handled = False
        ElseIf x = Key.Tab Then
            DPBd.Focus()
        ElseIf x = Key.Enter Or x = Key.Down Then
            FLDAdr.Focus()
        ElseIf x = Key.Up Then
            FLDLn.Focus()
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub FLDAdr_PreviewKeyDown(sender As Object, e As KeyEventArgs) Handles FLDAdr.PreviewKeyDown
        Dim x As String
        x = e.Key
        If x = Key.Space Or x = Key.Back Or x = Key.Left Or x = Key.Right Then
            e.Handled = False
        ElseIf (Keyboard.IsKeyDown(Key.RightShift) And x = Key.D3) Or (Keyboard.IsKeyDown(Key.LeftShift) And x = Key.D3) Then
            e.Handled = False
        ElseIf (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x >= Key.NumPad0 And x <= Key.NumPad9) Or (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x >= Key.D0 And x <= Key.D9) Or (x >= Key.A And x <= Key.Z) Or (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x = Key.OemMinus) Or (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x = Key.OemPeriod) Or (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x = Key.OemComma) Then
            e.Handled = False
        ElseIf x = Key.Tab Then
            FLDAdr.Focus()
        ElseIf x = Key.Enter Or x = Key.Down Then
            CBLoa.Focus()
        ElseIf x = Key.Up Then
            DPBd.Focus()
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub CBLoa_PreviewKeyDown(sender As Object, e As KeyEventArgs) Handles CBLoa.PreviewKeyDown
        Dim x As String
        x = e.Key
        If x = Key.Tab Then
            CBLoa.Focus()
        ElseIf x = Key.Enter Then
            buttonSave.Focus()
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub DPBd_CalendarOpened(sender As Object, e As RoutedEventArgs) Handles DPBd.CalendarOpened
        DPBd.DisplayDate = DateTime.Today.AddYears(-18)
        Dim startdate As New DateTime
        startdate = DateTime.Today.AddDays(+1).AddYears(-18)
        Dim enddate As New DateTime
        enddate = DateTime.Today.AddYears(-1500)
        DPBd.BlackoutDates.Add(New CalendarDateRange(start:=DateTime.Today.AddDays(+1).AddYears(-18), [end]:=startdate.AddYears(2000)))
        DPBd.BlackoutDates.Add(New CalendarDateRange(start:=enddate, [end]:=enddate.AddYears(1400)))
    End Sub


#End Region
End Class
