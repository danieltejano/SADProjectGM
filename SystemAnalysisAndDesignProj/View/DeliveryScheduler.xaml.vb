Imports System.Data
Imports System.Data.OleDb
Class DeliveryScheduler
    Dim log_values As Char
    Dim DB As New ADODB.Connection
    Dim RS As New ADODB.Recordset
    Dim oleDatabaseConnection As New OleDb.OleDbConnection(connectionString)

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
        stklbl.IsEnabled = True
        stktxt.IsEnabled = True
    End Sub

    Private Sub DeliveryScheduler_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        PullDataFromDatabase(d:=GRDDel, tableName:="Delivery_Job")
        Restrictions()
        CBDs.Items.Add("CANCELLED")                        'Adds "Administrator" a Combobox Item in the Level of Access (Loa) Combobox
        CBDs.Items.Add("PENDING")
        CBDs.Items.Add("DELIVERED")
        STKBtn.IsEnabled = True
        CMBCategory.Items.Clear()
        CMBCategory.Items.Add("")
        CMBCategory.Items.Add("CANCELLED")
        CMBCategory.Items.Add("DELIVERED")
        CMBCategory.Items.Add("PENDING")
    End Sub

    Private Sub GRDDel_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles GRDDel.SelectionChanged
        GRDDel.CanUserSortColumns = False
        If GRDDel.SelectedIndex >= 0 Then
            stklbl.Visibility = Visibility.Visible
            stktxt.Visibility = Visibility.Visible
            stklbl.IsEnabled = False
            stktxt.IsEnabled = False
            buttonEdit.IsEnabled = True
            buttonDelete.IsEnabled = True
            Dim selectedRowIndex = GRDDel.SelectedIndex                                                                                                   'gets the index of the currentRow Selected                                                                                                           'gets the rowCount of the whole datagrid    
            Dim di As TextBlock = TryCast(GRDDel.Columns(0).GetCellContent(GRDDel.Items(selectedRowIndex)), TextBlock)                                   'creates a temporary textblock that will hold the value of the cell
            Dim ti As TextBlock = TryCast(GRDDel.Columns(1).GetCellContent(GRDDel.Items(selectedRowIndex)), TextBlock)
            Dim cn As TextBlock = TryCast(GRDDel.Columns(2).GetCellContent(GRDDel.Items(selectedRowIndex)), TextBlock)
            Dim ca As TextBlock = TryCast(GRDDel.Columns(3).GetCellContent(GRDDel.Items(selectedRowIndex)), TextBlock)
            Dim dd As TextBlock = TryCast(GRDDel.Columns(4).GetCellContent(GRDDel.Items(selectedRowIndex)), TextBlock)
            Dim ctn As TextBlock = TryCast(GRDDel.Columns(5).GetCellContent(GRDDel.Items(selectedRowIndex)), TextBlock)
            Dim ds As TextBlock = TryCast(GRDDel.Columns(6).GetCellContent(GRDDel.Items(selectedRowIndex)), TextBlock)
            FLDDi.Text = di.Text
            FLDPTi.Text = ti.Text
            FLDCn.Text = cn.Text
            FLDCa.Text = ca.Text
            DPDd.Text = dd.Text
            FLDCtn.Text = ctn.Text
            CBDs.Text = ds.Text.ToUpper
            If CBDs.Text = "DELIVERED" Then
                buttonEdit.IsEnabled = False
                buttonDelete.IsEnabled = False
            End If
        End If
    End Sub

    Private Sub buttonDelete_Click(sender As Object, e As RoutedEventArgs) Handles buttonDelete.Click
        If MsgBox("Are you sure you want to Delete?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Message") = MsgBoxResult.Yes Then
            Try
                DB.Open(connectionString)
                DB.Execute("Delete * from Delivery_Job where DeliveryID=" & FLDDi.Text & "")
                MessageBox.Show("Record was Deleted", "SYSTEM")
                DB.Close()
                RecordLog(accountID:=AccountId, loa:=UserType, actionTaken:="DELETED DELIVERY SCHEDULE")
            Catch EX As Exception
            End Try
            PullDataFromDatabase(d:=GRDDel, tableName:="Delivery_Job")
            Restrictions()
        End If


    End Sub

    Private Sub buttonEdit_Click(sender As Object, e As RoutedEventArgs) Handles buttonEdit.Click
        AddEdit()
        GRDDel.IsHitTestVisible = False
        buttonSave.Content = "UPDATE"
        FLDDi.IsEnabled = False
        FLDPTi.IsEnabled = False
        DPDd.Text = Now
        FLDCn.IsEnabled = False
        FLDCtn.IsEnabled = False
        FLDCa.Focus()
    End Sub

    Private Sub buttonSave_Click(sender As Object, e As RoutedEventArgs) Handles buttonSave.Click
        If buttonSave.Content = "UPDATE" Then
            GRDDel.IsHitTestVisible = True
            Dim A As New ADODB.Connection
            Dim B As New ADODB.Recordset
            A.Open(connectionString)
            B.Open("Select * from Delivery_Job where DeliveryID=" & FLDDi.Text & "", A)
            If FLDCa.Text = "" Or DPDd.Text = "" Then
                MessageBox.Show("All Fields under Delivery is needed to be filled out")
            ElseIf B.Fields("DeliveryAddress").Value = FLDCa.Text And B.Fields("DeliveryDate").Value = DPDd.Text And B.Fields("DeliveryStatus").Value = CBDs.Text Then
                MessageBox.Show("No changes made.", "SYSTEM")
            Else

                With B
                    .Close()
                    .Open("Delivery_Job", A, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)
                    .MoveFirst()
                    .Find("DeliveryID='" & FLDDi.Text & "'")
                    .Fields("DeliveryAddress").Value = FLDCa.Text                                                                                     'Creates new record with the First Name = to the text of the textbox                                                                                         
                    .Fields("DeliveryDate").Value = DPDd.Text                                                                                      'Creates new record with the Last Name = to the text of the textbox
                    .Fields("DeliveryStatus").Value = CBDs.Text                                                                        'Creates new record with the SelectedDate = to the text of the textbox
                    .Update()
                    MessageBox.Show("Account has been successfully updated", "SYSTEM")
                End With
            End If
            A.Close()
            Restrictions()
            RecordLog(accountID:=AccountId, loa:=UserType, actionTaken:="UPDATED DELIVERY SCHEDULE")
        End If
        PullDataFromDatabase(d:=GRDDel, tableName:="Delivery_Job")
    End Sub

    Private Sub buttonCancel_Click(sender As Object, e As RoutedEventArgs) Handles buttonCancel.Click
        Dim can As String
        can = MessageBox.Show("Do you want to cancel ?", "SYSTEM", MessageBoxButton.YesNo, MessageBoxImage.Exclamation)
        If can = vbYes Then
            GRDDel.IsHitTestVisible = True
            GRDDel.UnselectAllCells()
            Restrictions()
        End If
    End Sub

    Private Sub SearchTextBox_TextChanged(sender As Object, e As TextChangedEventArgs) Handles SearchTextBox.TextChanged
        CMBCategory.SelectedIndex = 0
        Dim oleDatabaseConnection As New OleDb.OleDbConnection(connectionString)
        oleDatabaseConnection.Open()
        Dim databasez As New OleDbCommand
        databasez.CommandText = "Select *  From Delivery_Job Where CustomerName Like '%" & SearchTextBox.Text & "%'"
        databasez.Connection = oleDatabaseConnection
        Dim databaseActualTable As OleDbDataReader = databasez.ExecuteReader()
        GRDDel.ItemsSource = databaseActualTable
        GRDDel.Items.Refresh()
    End Sub



    Private Sub CMBCategory_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles CMBCategory.SelectionChanged
        Dim oleDatabaseConnection As New OleDb.OleDbConnection(connectionString)
        oleDatabaseConnection.Open()
        Dim databasez As New OleDbCommand
        If CMBCategory.SelectedIndex = 0 Then
            databasez.CommandText = "Select *  From Delivery_Job"
            databasez.Connection = oleDatabaseConnection
            Dim databaseActualTable As OleDbDataReader = databasez.ExecuteReader()
            GRDDel.ItemsSource = databaseActualTable
            GRDDel.Items.Refresh()
        ElseIf CMBCategory.SelectedIndex = 1 Then
            databasez.CommandText = "Select *  From Delivery_Job Where DeliveryStatus='CANCELLED'"
            databasez.Connection = oleDatabaseConnection
            Dim databaseActualTable As OleDbDataReader = databasez.ExecuteReader()
            GRDDel.ItemsSource = databaseActualTable
            GRDDel.Items.Refresh()
        ElseIf CMBCategory.SelectedIndex = 2 Then
            databasez.CommandText = "Select *  From Delivery_Job Where DeliveryStatus='DELIVERED'"
            databasez.Connection = oleDatabaseConnection
            Dim databaseActualTable As OleDbDataReader = databasez.ExecuteReader()
            GRDDel.ItemsSource = databaseActualTable
            GRDDel.Items.Refresh()
        ElseIf CMBCategory.SelectedIndex = 3 Then
            databasez.CommandText = "Select *  From Delivery_Job Where DeliveryStatus='PENDING'"
            databasez.Connection = oleDatabaseConnection
            Dim databaseActualTable As OleDbDataReader = databasez.ExecuteReader()
            GRDDel.ItemsSource = databaseActualTable
            GRDDel.Items.Refresh()
        End If
    End Sub

    Private Sub GRDDel_AutoGeneratingColumn(sender As Object, e As DataGridAutoGeneratingColumnEventArgs) Handles GRDDel.AutoGeneratingColumn
        If (e.PropertyName = "DeliveryDate") Then
            Dim column As DataGridTextColumn = e.Column
            Dim binding As Binding = column.Binding
            binding.StringFormat = "MMMMM-dd-yyyy"
        End If
    End Sub


#Region "keypress"
    Private Sub FLDCa_PreviewKeyDown(sender As Object, e As KeyEventArgs) Handles FLDCa.PreviewKeyDown
        Dim x As String
        x = e.Key
        If x = Key.Space Or x = Key.Back Or x = Key.Left Or x = Key.Right Then
            e.Handled = False
        ElseIf (Keyboard.IsKeyDown(Key.RightShift) And x = Key.D3) Or (Keyboard.IsKeyDown(Key.LeftShift) And x = Key.D3) Then
            e.Handled = False
        ElseIf (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x >= Key.NumPad0 And x <= Key.NumPad9) Or (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x >= Key.D0 And x <= Key.D9) Or (x >= Key.A And x <= Key.Z) Or (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x = Key.OemMinus) Or (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x = Key.OemPeriod) Or (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x = Key.OemComma) Then
            e.Handled = False
        ElseIf x = Key.Tab Then
            FLDCa.Focus()
        ElseIf x = Key.Enter Or x = Key.Down Then
            DPDd.Focus()
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub DPDd_PreviewKeyDown(sender As Object, e As KeyEventArgs) Handles DPDd.PreviewKeyDown
        Dim x As String
        x = e.Key
        If x = Key.Back Or x = Key.Left Or x = Key.Right Then
            e.Handled = False
        ElseIf (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x >= Key.NumPad0 And x <= Key.NumPad9) Or (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x >= Key.D0 And x <= Key.D9) Or (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x = Key.OemQuestion) Then
            e.Handled = False
        ElseIf x = Key.Tab Then
            DPDd.Focus()
        ElseIf x = Key.Enter Or x = Key.Down Then
            CBDs.Focus()
        ElseIf x = Key.Up Then
            FLDCa.Focus()
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub CBDs_PreviewKeyDown(sender As Object, e As KeyEventArgs) Handles CBDs.PreviewKeyDown
        Dim x As String
        x = e.Key
        If x = Key.Tab Then
            CBDs.Focus()
        ElseIf x = Key.Enter Then
            buttonSave.Focus()
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub DPDd_CalendarOpened(sender As Object, e As RoutedEventArgs) Handles DPDd.CalendarOpened
        DPDd.DisplayDate = Now
        DPDd.BlackoutDates.AddDatesInPast()
        Dim startdate As New DateTime
        startdate = DateTime.Today.AddYears(100)

        DPDd.BlackoutDates.Add(New CalendarDateRange(start:=startdate, [end]:=startdate.AddYears(2000)))
    End Sub




#End Region
End Class
