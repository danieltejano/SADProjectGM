Imports System.Data
Imports System.Data.OleDb
Public Class SupplierPage
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

    Private Sub SupplierPage_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        PullDataFromDatabase(d:=GRDDMan, tableName:="Supplier ORDER BY SupplierID ASC")
        Restrictions()
        STKBtn.IsEnabled = True
        CMBCategory.Items.Clear()
        CMBCategory.Items.Add("SupplierName")
        CMBCategory.Items.Add("Owner")
        CMBCategory.SelectedIndex = 0
    End Sub

    Private Sub buttonAdd_Click(sender As Object, e As RoutedEventArgs) Handles buttonAdd.Click
        FLDSi.IsEnabled = False
        AddEdit()
        GRDDMan.UnselectAllCells()
        FLDSi.Clear()
        FLDSn.Clear()
        FLDSa.Clear()
        FLDOwn.Clear()
        FLDCTn.Clear()
        GRDDMan.IsHitTestVisible = False
        buttonSave.Content = "SAVE"
        FLDSn.Focus()
    End Sub

    Public Sub GRDDMan_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles GRDDMan.SelectionChanged
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
            Dim sn As TextBlock = TryCast(GRDDMan.Columns(1).GetCellContent(GRDDMan.Items(selectedRowIndex)), TextBlock)
            Dim sa As TextBlock = TryCast(GRDDMan.Columns(2).GetCellContent(GRDDMan.Items(selectedRowIndex)), TextBlock)
            Dim own As TextBlock = TryCast(GRDDMan.Columns(3).GetCellContent(GRDDMan.Items(selectedRowIndex)), TextBlock)
            Dim ctn As TextBlock = TryCast(GRDDMan.Columns(4).GetCellContent(GRDDMan.Items(selectedRowIndex)), TextBlock)
            FLDSi.Text = act.Text
            FLDSn.Text = sn.Text
            FLDSa.Text = sa.Text
            FLDOwn.Text = own.Text
            FLDCTn.Text = ctn.Text
        End If
    End Sub
    Private Sub buttonDelete_Click(sender As Object, e As RoutedEventArgs) Handles buttonDelete.Click
        If MsgBox("Are you sure you want to Delete?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Message") = MsgBoxResult.Yes Then
            Try
                DB.Open(connectionString)
                DB.Execute("Delete * from Supplier where SupplierID=" & FLDSi.Text & "")
                MessageBox.Show("Record was Deleted", "SYSTEM")
                DB.Close()
                RecordLog(accountID:=AccountId, loa:=UserType, actionTaken:="DELETED SUPPLIER INFO")
            Catch EX As Exception
            End Try
            PullDataFromDatabase(d:=GRDDMan, tableName:="Supplier")
            FLDSi.Visibility = Visibility.Visible
            Restrictions()
        End If
    End Sub

    Private Sub buttonEdit_Click(sender As Object, e As RoutedEventArgs) Handles buttonEdit.Click
        FLDSi.IsEnabled = False
        AddEdit()
        GRDDMan.IsHitTestVisible = False
        buttonSave.Content = "UPDATE"
        FLDSn.Focus()
    End Sub

    Private Sub buttonSave_Click(sender As Object, e As RoutedEventArgs) Handles buttonSave.Click
        If buttonSave.Content = "SAVE" Then
            GRDDMan.IsHitTestVisible = True
            Dim A As New ADODB.Connection
            Dim B As New ADODB.Recordset
            A.Open(connectionString)
            B.Open("Select * from Supplier where SupplierName='" & FLDSn.Text & "'", A)
            Try
                If FLDSn.Text = "" Or FLDSa.Text = "" Or FLDOwn.Text = "" Or FLDCTn.Text = "" Then
                    MessageBox.Show("All Fields under User Account is needed to be filled out")
                ElseIf FLDSa.Text.Length <= 10 Then
                    MsgBox("Address too short")
                ElseIf FLDSn.Text = B.Fields("SupplierName").Value Then
                    MessageBox.Show("Supplier already exists")
                End If
            Catch ex As Exception
                With B
                    .Close()
                    .Open("Supplier", A, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)
                    .AddNew()
                    .Fields("SupplierName").Value = FLDSn.Text
                    .Fields("SupplierAddress").Value = FLDSa.Text
                    .Fields("Owner").Value = FLDOwn.Text
                    .Fields("ContactNumber").Value = FLDCTn.Text
                    .Update()
                    .Close()
                End With
                A.Close()
                Restrictions()
                FLDSi.Visibility = Visibility.Visible
                RecordLog(accountID:=AccountId, loa:=UserType, actionTaken:="ADDED SUPPLIER")
            End Try
        End If

        If buttonSave.Content = "UPDATE" Then
            GRDDMan.IsHitTestVisible = True
            Dim A As New ADODB.Connection
            Dim B As New ADODB.Recordset
            A.Open(connectionString)
            B.Open("Select * from Supplier where SupplierID=" & FLDSi.Text & "", A)
            If FLDSn.Text = "" Or FLDSa.Text = "" Or FLDOwn.Text = "" Or FLDCTn.Text = "" Then
                MessageBox.Show("All Fields under User Account is needed to be filled out")
            ElseIf B.Fields("SupplierName").Value = FLDSn.Text And B.Fields("SupplierAddress").Value = FLDSa.Text And B.Fields("Owner").Value = FLDOwn.Text And B.Fields("ContactNumber").Value = FLDCTn.Text Then
                MessageBox.Show("No changes made.", "SYSTEM")
            ElseIf FLDSa.Text.Length <= 10 Then
                MsgBox("Address too short")
            ElseIf FLDSn.Text <> B.Fields("SupplierName").Value Then
                B.Close()
                B.Open("Select * from Supplier where SupplierName='" & FLDSn.Text & "'", A)
                Try
                    If FLDSn.Text = B.Fields("SupplierName").Value Then
                        MessageBox.Show("Supplier already exists", "SYSTEM")
                        B.Close()
                    End If
                Catch ex As Exception
                    With B
                        .Close()
                        .Open("Supplier", A, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)
                        .MoveFirst()
                        .Find("SupplierID='" & FLDSi.Text & "'")
                        .Fields("SupplierName").Value = FLDSn.Text                                                                                     'Creates new record with the First Name = to the text of the textbox                                                                                         
                        .Fields("SupplierAddress").Value = FLDSa.Text                                                                                      'Creates new record with the Last Name = to the text of the textbox
                        .Fields("Owner").Value = FLDOwn.Text                                                                        'Creates new record with the SelectedDate = to the text of the textbox
                        .Fields("ContactNumber").Value = FLDCTn.Text                                                                       'Creates new record with the Level Of Access = to the text of the textbox
                        .Update()
                        MessageBox.Show("Account has been successfully updated", "SYSTEM")
                    End With
                End Try
            Else
                With B
                    .Close()
                    .Open("Supplier", A, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)
                    .MoveFirst()
                    .Find("SupplierID='" & FLDSi.Text & "'")
                    .Fields("SupplierName").Value = FLDSn.Text                                                                                     'Creates new record with the First Name = to the text of the textbox                                                                                         
                    .Fields("SupplierAddress").Value = FLDSa.Text                                                                                      'Creates new record with the Last Name = to the text of the textbox
                    .Fields("Owner").Value = FLDOwn.Text                                                                        'Creates new record with the SelectedDate = to the text of the textbox
                    .Fields("ContactNumber").Value = FLDCTn.Text                                                                       'Creates new record with the Level Of Access = to the text of the textbox
                    .Update()
                    MessageBox.Show("Account has been successfully updated", "SYSTEM")
                End With

            End If
            A.Close()
            Restrictions()
            FLDSi.Visibility = Visibility.Visible
            RecordLog(accountID:=AccountId, loa:=UserType, actionTaken:="UPDATED SUPPLIER INFO")
        End If
        PullDataFromDatabase(d:=GRDDMan, tableName:="Supplier")
    End Sub

    Private Sub buttonCancel_Click(sender As Object, e As RoutedEventArgs) Handles buttonCancel.Click
        Dim can As String
        can = MessageBox.Show("Do you want to cancel ?", "SYSTEM", MessageBoxButton.YesNo, MessageBoxImage.Exclamation)
        If can = vbYes Then
            GRDDMan.IsHitTestVisible = True
            GRDDMan.UnselectAllCells()
            FLDSi.Visibility = Visibility.Visible
            Restrictions()
        End If
    End Sub

    Private Sub SearchTextBox_TextChanged(sender As Object, e As TextChangedEventArgs) Handles SearchTextBox.TextChanged
        Dim oleDatabaseConnection As New OleDb.OleDbConnection(connectionString)
        oleDatabaseConnection.Open()
        Dim databasez As New OleDbCommand
        If SearchTextBox.Text = "" Then
            databasez.CommandText = "Select *  From Supplier"
            databasez.Connection = oleDatabaseConnection
            Dim databaseActualTable As OleDbDataReader = databasez.ExecuteReader()
            GRDDMan.ItemsSource = databaseActualTable
            GRDDMan.Items.Refresh()

        ElseIf CMBCategory.SelectedIndex = 0 Then
            databasez.CommandText = "Select *  From Supplier Where SupplierName Like '%" & SearchTextBox.Text & "%'"
            databasez.Connection = oleDatabaseConnection
            Dim databaseActualTable As OleDbDataReader = databasez.ExecuteReader()
            GRDDMan.ItemsSource = databaseActualTable
            GRDDMan.Items.Refresh()
        ElseIf CMBCategory.SelectedIndex = 1 Then
            databasez.CommandText = "Select *  From Supplier Where Owner Like '" & SearchTextBox.Text & "%'"
            databasez.Connection = oleDatabaseConnection
            Dim databaseActualTable As OleDbDataReader = databasez.ExecuteReader()
            GRDDMan.ItemsSource = databaseActualTable
            GRDDMan.Items.Refresh()
        End If


    End Sub

    Private Sub CMBCategory_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles CMBCategory.SelectionChanged
        SearchTextBox.Clear()
    End Sub
#Region "keypress"
    Private Sub FLDSn_PreviewKeyDown(sender As Object, e As KeyEventArgs) Handles FLDSn.PreviewKeyDown
        Dim x As String
        x = e.Key
        If x = Key.Space Or x = Key.Back Or x = Key.Left Or x = Key.Right Then
            e.Handled = False
        ElseIf (x >= Key.A And x <= Key.Z) Or (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x = Key.OemMinus) Or (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x = Key.OemPeriod) Then
            e.Handled = False
        ElseIf x = Key.Tab Then
            FLDSn.Focus()
        ElseIf x = Key.Enter Or x = Key.Down Then
            FLDSa.Focus()
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub FLDSa_PreviewKeyDown(sender As Object, e As KeyEventArgs) Handles FLDSa.PreviewKeyDown
        Dim x As String
        x = e.Key
        If x = Key.Space Or x = Key.Back Or x = Key.Left Or x = Key.Right Then
            e.Handled = False
        ElseIf (Keyboard.IsKeyDown(Key.RightShift) And x = Key.D3) Or (Keyboard.IsKeyDown(Key.LeftShift) And x = Key.D3) Then
            e.Handled = False
        ElseIf (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x >= Key.NumPad0 And x <= Key.NumPad9) Or (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x >= Key.D0 And x <= Key.D9) Or (x >= Key.A And x <= Key.Z) Or (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x = Key.OemMinus) Or (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x = Key.OemPeriod) Or (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x = Key.OemComma) Then
            e.Handled = False
        ElseIf x = Key.Tab Then
            FLDSa.Focus()
        ElseIf x = Key.Enter Or x = Key.Down Then
            FLDOwn.Focus()
        ElseIf x = Key.Up Then
            FLDSn.Focus()
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub FLDOwn_PreviewKeyDown(sender As Object, e As KeyEventArgs) Handles FLDOwn.PreviewKeyDown
        Dim x As String
        x = e.Key
        If x = Key.Space Or x = Key.Back Or x = Key.Left Or x = Key.Right Then
            e.Handled = False
        ElseIf (x >= Key.A And x <= Key.Z) Or (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x = Key.OemMinus) Or (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x = Key.OemPeriod) Then
            e.Handled = False
        ElseIf x = Key.Tab Then
            FLDOwn.Focus()
        ElseIf x = Key.Enter Or x = Key.Down Then
            FLDCTn.Focus()
        ElseIf x = Key.Up Then
            FLDSa.Focus()
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
            FLDOwn.Focus()
        Else
            e.Handled = True
        End If
    End Sub


#End Region
End Class
