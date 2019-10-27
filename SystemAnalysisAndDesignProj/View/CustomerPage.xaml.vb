Imports System.Data
Imports System.Data.OleDb

Public Class CustomerPage
    Dim log_values As Char
    Dim DB As New ADODB.Connection
    Dim RS As New ADODB.Recordset
    Dim oleDatabaseConnection As New OleDb.OleDbConnection("Provider=Microsoft.jet.oledb.4.0;Data Source=Inventory.mdb")

    Public dy As String
    Public dm As String

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

    Private Sub DeliveryManPage_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        PullDataFromDatabase(d:=GRDDMan, tableName:="Customer")
        Restrictions()
        STKBtn.IsEnabled = True
        CMBCategory.Items.Clear()
        CMBCategory.Items.Add("First Name")
        CMBCategory.Items.Add("Last Name")
        CMBCategory.SelectedIndex = 0
    End Sub

    Private Sub buttonAdd_Click(sender As Object, e As RoutedEventArgs) Handles buttonAdd.Click
        FLDAct.IsEnabled = False
        AddEdit()
        GRDDMan.UnselectAllCells()
        FLDAct.Clear()
        FLDAdr.Clear()
        FLDFn.Clear()
        FLDLn.Clear()
        FLDCTn.Clear()
        DPBd.Text = DateTime.Today.AddYears(-18)
        GRDDMan.IsHitTestVisible = False
        buttonSave.Content = "SAVE"
        FLDFn.Focus()
    End Sub

    Public Sub GRDDMan_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles GRDDMan.SelectionChanged
        dy = FLDFn.Text
        dm = FLDLn.Text
        GRDDMan.CanUserSortColumns = False
        If GRDDMan.SelectedIndex >= 0 Then
            stklbl.Visibility = Visibility.Visible
            stktxt.Visibility = Visibility.Visible
            stklbl.IsEnabled = False
            stktxt.IsEnabled = False
            buttonEdit.IsEnabled = True
            buttonDelete.IsEnabled = True
            Dim selectedRowIndex = GRDDMan.SelectedIndex                                                                                                   'gets the index of the currentRow Selected                                                                                                           'gets the rowCount of the whole datagrid    
            Dim act As TextBlock = TryCast(GRDDMan.Columns(0).GetCellContent(GRDDMan.Items(selectedRowIndex)), TextBlock)                                   'creates a temporary textblock that will hold the value of the cell
            Dim fn As TextBlock = TryCast(GRDDMan.Columns(1).GetCellContent(GRDDMan.Items(selectedRowIndex)), TextBlock)
            Dim ln As TextBlock = TryCast(GRDDMan.Columns(2).GetCellContent(GRDDMan.Items(selectedRowIndex)), TextBlock)
            Dim bd As TextBlock = TryCast(GRDDMan.Columns(3).GetCellContent(GRDDMan.Items(selectedRowIndex)), TextBlock)
            Dim adr As TextBlock = TryCast(GRDDMan.Columns(4).GetCellContent(GRDDMan.Items(selectedRowIndex)), TextBlock)
            Dim ctn As TextBlock = TryCast(GRDDMan.Columns(5).GetCellContent(GRDDMan.Items(selectedRowIndex)), TextBlock)
            FLDAct.Text = act.Text
            FLDFn.Text = fn.Text
            FLDLn.Text = ln.Text
            DPBd.Text = bd.Text
            FLDAdr.Text = adr.Text
            FLDCTn.Text = ctn.Text
        End If
    End Sub
    Private Sub buttonDelete_Click(sender As Object, e As RoutedEventArgs) Handles buttonDelete.Click
        If MsgBox("Are you sure you want to Delete?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Message") = MsgBoxResult.Yes Then
            Try
                DB.Open(connectionString)
                DB.Execute("Delete * From Customer Where LastName= '" & FLDLn.Text & "'" & "and FirstName='" & FLDFn.Text & "'")
                MessageBox.Show("Record was Deleted", "SYSTEM")
                DB.Close()
                RecordLog(accountID:=AccountId, loa:=UserType, actionTaken:="DELETED CUSTOMER INFO")
            Catch EX As Exception
            End Try
            PullDataFromDatabase(d:=GRDDMan, tableName:="Customer")
            FLDAct.Visibility = Visibility.Visible
            Restrictions()
        End If
    End Sub

    Private Sub buttonEdit_Click(sender As Object, e As RoutedEventArgs) Handles buttonEdit.Click
        FLDAct.IsEnabled = False
        AddEdit()
        GRDDMan.IsHitTestVisible = False
        buttonSave.Content = "UPDATE"
        FLDFn.Focus()
    End Sub

    Private Sub buttonSave_Click(sender As Object, e As RoutedEventArgs) Handles buttonSave.Click
        If buttonSave.Content = "SAVE" Then
            GRDDMan.IsHitTestVisible = True
            Dim A As New ADODB.Connection
            Dim B As New ADODB.Recordset
            A.Open(connectionString)
            B.Open("Select * from Customer where FirstName='" & FLDFn.Text & "'" & "and LastName='" & FLDLn.Text & "'", A)
            Try
                If FLDFn.Text = "" Or FLDLn.Text = "" Or DPBd.Text = "" Or FLDAdr.Text = "" Or FLDCTn.Text = "" Then
                    MessageBox.Show("All Fields under User Account is needed to be filled out")
                ElseIf FLDAdr.Text.Length <= 10 Then
                    MsgBox("Address too short")
                ElseIf FLDFn.Text = B.Fields("FirstName").Value And FLDLn.Text = B.Fields("LastName").Value Then
                    MessageBox.Show("Customer already exists")
                End If
            Catch ex As Exception
                With B
                    .Close()
                    .Open("Customer", A, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)
                    .AddNew()
                    .Fields("FirstName").Value = FLDFn.Text
                    .Fields("LastName").Value = FLDLn.Text
                    .Fields("Birthdate").Value = DPBd.SelectedDate
                    .Fields("Address").Value = FLDAdr.Text
                    .Fields("Contact Number").Value = FLDCTn.Text
                    .Update()
                    .Close()
                End With
                A.Close()
                Restrictions()
                FLDAct.Visibility = Visibility.Visible
                RecordLog(accountID:=AccountId, loa:=UserType, actionTaken:="ADDED CUSTOMER")
            End Try
        End If

        If buttonSave.Content = "UPDATE" Then
            GRDDMan.IsHitTestVisible = True
            Dim A As New ADODB.Connection
            Dim B As New ADODB.Recordset
            A.Open(connectionString)
            B.Open("Select * from Customer where CustomerID=" & FLDAct.Text & "", A)
            If FLDFn.Text = "" Or FLDLn.Text = "" Or DPBd.Text = "" Or FLDAdr.Text = "" Or FLDCTn.Text = "" Then
                MessageBox.Show("All Fields under User Account is needed to be filled out")
            ElseIf B.Fields("Firstname").Value = FLDFn.Text And B.Fields("Lastname").Value = FLDLn.Text And B.Fields("Birthdate").Value = DPBd.Text And B.Fields("Address").Value = FLDAdr.Text And B.Fields("Contact Number").Value = FLDCTn.Text Then
                MessageBox.Show("No changes made.", "SYSTEM")
            ElseIf FLDAdr.Text.Length <= 10 Then
                MsgBox("Address too short")
            ElseIf FLDFn.Text <> B.Fields("FirstName").Value Or FLDLn.Text <> B.Fields("LastName").Value Then
                B.Close()
                B.Open("Select * from Customer where FirstName='" & FLDFn.Text & "'" & "and LastName='" & FLDLn.Text & "'", A)
                Try
                    If FLDFn.Text = B.Fields("FirstName").Value And FLDLn.Text = B.Fields("LastName").Value Then
                        MessageBox.Show("Customer already exists", "SYSTEM")
                        B.Close()
                    End If
                Catch ex As Exception
                    With B
                        .Close()
                        .Open("Customer", A, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)
                        .MoveFirst()
                        .Find("CustomerID='" & FLDAct.Text & "'")
                        .Fields("FirstName").Value = FLDFn.Text                                                                                     'Creates new record with the First Name = to the text of the textbox                                                                                         
                        .Fields("LastName").Value = FLDLn.Text                                                                                      'Creates new record with the Last Name = to the text of the textbox
                        .Fields("Birthdate").Value = DPBd.SelectedDate                                                                           'Creates new record with the SelectedDate = to the text of the textbox
                        .Fields("Address").Value = FLDAdr.Text                                                                                      'Creates new record with the Address = to the text of the textbox
                        .Fields("Contact Number").Value = FLDCTn.Text                                                                       'Creates new record with the Level Of Access = to the text of the textbox
                        .Update()
                        MessageBox.Show("Account has been successfully updated", "SYSTEM")
                    End With
                End Try
            Else
                With B
                    .Close()
                    .Open("Customer", A, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)
                    .MoveFirst()
                    .Find("CustomerID='" & FLDAct.Text & "'")
                    .Fields("FirstName").Value = FLDFn.Text                                                                                     'Creates new record with the First Name = to the text of the textbox                                                                                         
                    .Fields("LastName").Value = FLDLn.Text                                                                                      'Creates new record with the Last Name = to the text of the textbox
                    .Fields("Birthdate").Value = DPBd.SelectedDate                                                                           'Creates new record with the SelectedDate = to the text of the textbox
                    .Fields("Address").Value = FLDAdr.Text                                                                                      'Creates new record with the Address = to the text of the textbox
                    .Fields("Contact Number").Value = FLDCTn.Text                                                                       'Creates new record with the Level Of Access = to the text of the textbox
                    .Update()
                    MessageBox.Show("Account has been successfully updated", "SYSTEM")
                End With
            End If
            A.Close()
            Restrictions()
            FLDAct.Visibility = Visibility.Visible
            RecordLog(accountID:=AccountId, loa:=UserType, actionTaken:="CUSTOMER INFO UPDATED")
        End If
        PullDataFromDatabase(d:=GRDDMan, tableName:="Customer")
    End Sub

    Private Sub buttonCancel_Click(sender As Object, e As RoutedEventArgs) Handles buttonCancel.Click
        Dim can As String
        can = MessageBox.Show("Do you want to cancel ?", "SYSTEM", MessageBoxButton.YesNo, MessageBoxImage.Exclamation)
        If can = vbYes Then
            GRDDMan.IsHitTestVisible = True
            Restrictions()
            GRDDMan.UnselectAllCells()
            FLDAct.Visibility = Visibility.Visible
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
            FLDCTn.Focus()
        ElseIf x = Key.Up Then
            DPBd.Focus()
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub FLDCTn_PreviewKeyDown(sender As Object, e As KeyEventArgs) Handles FLDCTn.PreviewKeyDown
        Dim x As String
        x = e.Key
        If x = Key.Back Or x = Key.Left Or x = Key.Right Then
            e.Handled = False
        ElseIf (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x >= Key.NumPad0 And x <= Key.NumPad9) Or (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x >= Key.D0 And x <= Key.D9) Then
            e.Handled = False
        ElseIf x = Key.Tab Then
            FLDCTn.Focus()
        ElseIf x = Key.Enter Then
            buttonSave.Focus()
        ElseIf x = Key.Up Then
            FLDAdr.Focus()
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub SearchTextBox_TextChanged(sender As Object, e As TextChangedEventArgs) Handles SearchTextBox.TextChanged
        Dim oleDatabaseConnection As New OleDb.OleDbConnection(connectionString)
        oleDatabaseConnection.Open()
        Dim databasez As New OleDbCommand

        If CMBCategory.SelectedIndex = 0 Then
            databasez.CommandText = "Select *  From Customer Where FirstName Like '%" & SearchTextBox.Text & "%'"
            databasez.Connection = oleDatabaseConnection
            Dim databaseActualTable As OleDbDataReader = databasez.ExecuteReader()
            GRDDMan.ItemsSource = databaseActualTable
            GRDDMan.Items.Refresh()
        ElseIf CMBCategory.SelectedIndex = 1 Then
            databasez.CommandText = "Select *  From Customer Where LastName Like '" & SearchTextBox.Text & "%'"
            databasez.Connection = oleDatabaseConnection
            Dim databaseActualTable As OleDbDataReader = databasez.ExecuteReader()
            GRDDMan.ItemsSource = databaseActualTable
            GRDDMan.Items.Refresh()
        End If


    End Sub

    Private Sub CMBCategory_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles CMBCategory.SelectionChanged
        SearchTextBox.Clear()
    End Sub

    Private Sub GRDDMan_AutoGeneratingColumn(sender As Object, e As DataGridAutoGeneratingColumnEventArgs) Handles GRDDMan.AutoGeneratingColumn
        If (e.PropertyName = "Birthdate") Then
            Dim column As DataGridTextColumn = e.Column
            Dim binding As Binding = column.Binding
            binding.StringFormat = "MMMMM-dd-yyyy"
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
End Class
