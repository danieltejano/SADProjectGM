Imports System.Data
Imports System.Data.OleDb

Class InventoryPage
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
        BTNAddStocks.IsEnabled = False
        FLDadd.IsEnabled = False
        btnminus.IsEnabled = False
        btnplus.IsEnabled = False
        btnminus_10.IsEnabled = False
        btnplus_10.IsEnabled = False
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
        BTNAddStocks.IsEnabled = False
        FLDadd.IsEnabled = False
        btnminus.IsEnabled = False
        btnplus.IsEnabled = False
        btnminus_10.IsEnabled = False
        buttonAdd.IsEnabled = False
        stklbl.IsEnabled = True
        stktxt.IsEnabled = True
    End Sub



    Private Sub InventoryPage_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        PullDataFromDatabase(d:=GRDInv, tableName:="formula")
        Restrictions()
        STKBtn.IsEnabled = True
        CMBCategory.Items.Clear()
        CMBCategory.Items.Add("")
        CMBCategory.Items.Add("Bed")
        CMBCategory.Items.Add("Cabinet")
        CMBCategory.Items.Add("Dining Chair")
        CMBCategory.Items.Add("Dining Set")
        CMBCategory.Items.Add("Dining Table")
        CMBCategory.Items.Add("Sala Set")
    End Sub

    Private Sub GRDInv_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles GRDInv.SelectionChanged
        GRDInv.CanUserSortColumns = False
        If GRDInv.SelectedIndex >= 0 Then
            stklbl.Visibility = Visibility.Visible
            stktxt.Visibility = Visibility.Visible
            stklbl.IsEnabled = False
            stktxt.IsEnabled = False
            buttonEdit.IsEnabled = True
            BTNAddStocks.IsEnabled = True
            buttonDelete.IsEnabled = True
            Dim selectedRowIndex = GRDInv.SelectedIndex                                                                                                   'gets the index of the currentRow Selected                                                                                                           'gets the rowCount of the whole datagrid    
            Dim pid As TextBlock = TryCast(GRDInv.Columns(0).GetCellContent(GRDInv.Items(selectedRowIndex)), TextBlock)                                   'creates a temporary textblock that will hold the value of the cell
            Dim pnm As TextBlock = TryCast(GRDInv.Columns(1).GetCellContent(GRDInv.Items(selectedRowIndex)), TextBlock)
            Dim prc As TextBlock = TryCast(GRDInv.Columns(2).GetCellContent(GRDInv.Items(selectedRowIndex)), TextBlock)
            Dim tv As TextBlock = TryCast(GRDInv.Columns(3).GetCellContent(GRDInv.Items(selectedRowIndex)), TextBlock)
            Dim us As TextBlock = TryCast(GRDInv.Columns(4).GetCellContent(GRDInv.Items(selectedRowIndex)), TextBlock)
            Dim cat As TextBlock = TryCast(GRDInv.Columns(5).GetCellContent(GRDInv.Items(selectedRowIndex)), TextBlock)
            Dim ua As TextBlock = TryCast(GRDInv.Columns(6).GetCellContent(GRDInv.Items(selectedRowIndex)), TextBlock)
            Dim si As TextBlock = TryCast(GRDInv.Columns(7).GetCellContent(GRDInv.Items(selectedRowIndex)), TextBlock)
            FLDPid.Text = pid.Text
            FLDPName.Text = pnm.Text
            FLDPrc.Text = prc.Text
            FLDTv.Text = tv.Text
            FLDUs.Text = us.Text
            FLDCat.Text = cat.Text
            FLDUa.Text = ua.Text
            FLDSi.Text = si.Text
        End If
    End Sub

    Private Sub buttonAdd_Click(sender As Object, e As RoutedEventArgs) Handles buttonAdd.Click
        FLDTv.IsEnabled = False
        FLDUs.IsEnabled = False
        AddEdit()
        GRDInv.UnselectAllCells()
        FLDPid.Clear()
        FLDPName.Clear()
        FLDPrc.Clear()
        FLDTv.Clear()
        FLDUs.Clear()
        FLDCat.Clear()
        FLDUa.Clear()
        FLDSi.Clear()
        GRDInv.IsHitTestVisible = False
        buttonSave.Content = "SAVE"
        FLDPid.Focus()
    End Sub

    Private Sub buttonDelete_Click(sender As Object, e As RoutedEventArgs) Handles buttonDelete.Click
        If MsgBox("Are you sure you want to Delete?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Message") = MsgBoxResult.Yes Then
            Try
                DB.Open(connectionString)
                DB.Execute("Delete * From Product Where ProductID= '" & FLDPid.Text & "'")
                MessageBox.Show("Record was Deleted", "SYSTEM")
                DB.Close()
                RecordLog(accountID:=AccountId, loa:=UserType, actionTaken:="Product DELETED")
            Catch EX As Exception
            End Try
        End If
        PullDataFromDatabase(d:=GRDInv, tableName:="formula")
        FLDPid.Visibility = Visibility.Visible
        Restrictions()
    End Sub

    Private Sub buttonEdit_Click(sender As Object, e As RoutedEventArgs) Handles buttonEdit.Click
        FLDTv.IsEnabled = False
        FLDUs.IsEnabled = False
        FLDUa.IsEnabled = False
        AddEdit()
        FLDPid.IsEnabled = False
        GRDInv.IsHitTestVisible = False
        buttonSave.Content = "UPDATE"
        FLDPName.Focus()
    End Sub

    Private Sub BTNAddStocks_Click(sender As Object, e As RoutedEventArgs) Handles BTNAddStocks.Click
        If BTNAddStocks.Content = "AddStocks" Then
            GRDInv.IsHitTestVisible = False

            FLDadd.Text = 0
            btnplus.IsEnabled = True
            BTNAddStocks.Content = "SAVE"
            buttonCancel.Visibility = Visibility.Visible
            buttonCancel.IsEnabled = True
            buttonDelete.IsEnabled = False
            buttonEdit.IsEnabled = False
            buttonAdd.IsEnabled = False
        ElseIf BTNAddStocks.Content = "SAVE" Then
            GRDInv.IsHitTestVisible = True
            Dim A As New ADODB.Connection
            Dim B As New ADODB.Recordset
            A.Open(connectionString)
            B.Open("Select * from Product where ProductID='" & FLDPid.Text & "'" & "or ProductName='" & FLDPName.Text & "'", A)
            With B
                .Close()
                .Open("Product", A, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)
                .MoveFirst()
                .Find("ProductID='" & FLDPid.Text & "'")
                .Fields("UnitsAvailable").Value = Int(FLDUa.Text) + Int(FLDadd.Text)
                .Update()
                MessageBox.Show("Stocks Added", "SYSTEM")
            End With
            FLDadd.Text = 0
            BTNAddStocks.Content = "AddStocks"
            A.Close()
            Restrictions()
            RecordLog(accountID:=AccountId, loa:=UserType, actionTaken:="AddedStocks")
        End If
        PullDataFromDatabase(d:=GRDInv, tableName:="formula")
    End Sub

    Private Sub btnplus_10_Click(sender As Object, e As RoutedEventArgs) Handles btnplus_10.Click
        FLDadd.Text = FLDadd.Text + 10
        If FLDadd.Text <= 0 Then
            FLDadd.Text = 0
            btnminus.IsEnabled = False
            btnminus_10.IsEnabled = False
        Else
            btnminus.IsEnabled = True
            btnminus_10.IsEnabled = True
        End If
        If FLDadd.Text >= 999 Then
            FLDadd.Text = 999
            btnplus.IsEnabled = False
            btnplus_10.IsEnabled = False
        Else
            btnplus.IsEnabled = True
            btnplus_10.IsEnabled = True
        End If
    End Sub

    Private Sub btnminus_10_Click(sender As Object, e As RoutedEventArgs) Handles btnminus_10.Click
        FLDadd.Text = FLDadd.Text - 10
        If FLDadd.Text <= 0 Then
            FLDadd.Text = 0
            btnminus.IsEnabled = False
            btnminus_10.IsEnabled = False
        Else
            btnminus.IsEnabled = True
            btnminus_10.IsEnabled = True
        End If
        If FLDadd.Text >= 999 Then
            FLDadd.Text = 999
            btnplus.IsEnabled = False
            btnplus_10.IsEnabled = False
        Else
            btnplus.IsEnabled = True
            btnplus_10.IsEnabled = True
        End If
    End Sub
    Private Sub btnplus_Click(sender As Object, e As RoutedEventArgs) Handles btnplus.Click
        FLDadd.Text = FLDadd.Text + 1
        If FLDadd.Text <= 0 Then
            FLDadd.Text = 0
            btnminus.IsEnabled = False
            btnminus_10.IsEnabled = False
        Else
            btnminus.IsEnabled = True
            btnminus_10.IsEnabled = True
        End If
        If FLDadd.Text >= 999 Then
            FLDadd.Text = 999
            btnplus.IsEnabled = False
            btnplus_10.IsEnabled = False
        Else
            btnplus.IsEnabled = True
            btnplus_10.IsEnabled = True
        End If
    End Sub
    Private Sub btnminus_Click(sender As Object, e As RoutedEventArgs) Handles btnminus.Click
        FLDadd.Text = FLDadd.Text - 1
        If FLDadd.Text <= 0 Then
            FLDadd.Text = 0
            btnminus.IsEnabled = False
            btnminus_10.IsEnabled = False
        Else
            btnminus.IsEnabled = True
            btnminus_10.IsEnabled = True
        End If
        If FLDadd.Text >= 999 Then
            FLDadd.Text = 999
            btnplus.IsEnabled = False
            btnplus_10.IsEnabled = False
        Else
            btnplus.IsEnabled = True
            btnplus_10.IsEnabled = True
        End If
    End Sub
    Private Sub buttonSave_Click(sender As Object, e As RoutedEventArgs) Handles buttonSave.Click
        If buttonSave.Content = "SAVE" Then
            GRDInv.IsHitTestVisible = True
            Dim A As New ADODB.Connection
            Dim B As New ADODB.Recordset
            A.Open(connectionString)
            B.Open("Select * from Product where ProductID='" & FLDPid.Text & "'" & "or ProductName='" & FLDPName.Text & "'", A)
            Try
                If FLDPid.Text = "" Or FLDPName.Text = "" Or FLDPrc.Text = "" Or FLDCat.Text = "" Or FLDSi.Text = "" Then
                    MessageBox.Show("All Fields under User Account is needed to be filled out")
                ElseIf FLDPid.Text = B.Fields("ProductID").Value Or FLDPName.Text = B.Fields("ProductName").Value Then
                    MessageBox.Show("Product Already Exists")
                End If
            Catch ex As Exception
                With B
                    .Close()
                    .Open("Product", A, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)
                    .MoveLast()
                    .AddNew()
                    .Fields("ProductID").Value = FLDPid.Text
                    .Fields("ProductName").Value = FLDPName.Text
                    .Fields("ProductPrice").Value = FLDPrc.Text
                    .Fields("Category").Value = FLDCat.Text
                    .Fields("SupplierID").Value = FLDSi.Text
                    .Fields("UnitSold").Value = 0
                    .Update()
                    .Close()
                End With
                A.Close()
                Restrictions()
                FLDTv.Visibility = Visibility.Visible
                FLDUs.Visibility = Visibility.Visible
                FLDUa.Visibility = Visibility.Visible
                FLDPid.IsEnabled = True
                RecordLog(accountID:=AccountId, loa:=UserType, actionTaken:="AccountAdded")
            End Try
        End If

        If buttonSave.Content = "UPDATE" Then
            GRDInv.IsHitTestVisible = True
            Dim A As New ADODB.Connection
            Dim B As New ADODB.Recordset
            A.Open(connectionString)
            B.Open("Select * from Product where ProductID='" & FLDPid.Text & "'" & "or ProductName='" & FLDPName.Text & "'", A)
            If FLDPid.Text = "" Or FLDPName.Text = "" Or FLDPrc.Text = "" Or FLDCat.Text = "" Or FLDSi.Text = "" Then
                MessageBox.Show("All Fields under User Account is needed to be filled out")
            ElseIf B.Fields("ProductID").Value = FLDPid.Text And B.Fields("ProductName").Value = FLDPName.Text And B.Fields("ProductPrice").Value = FLDPrc.Text And B.Fields("Category").Value = FLDCat.Text And B.Fields("UnitsAvailable").Value = FLDUa.Text And B.Fields("SupplierID").Value = FLDSi.Text Then
                MessageBox.Show("No changes made.", "SYSTEM")
            ElseIf FLDPName.Text <> B.Fields("ProductName").Value Then
                B.Close()
                B.Open("Select * from Product where ProductName='" & FLDPName.Text & "'", A)
                Try
                    If FLDPName.Text = B.Fields("ProductName").Value Then
                        MessageBox.Show("Unable to change Product Name.", "SYSTEM")
                        B.Close()
                    End If

                Catch ex As Exception
                    With B
                        .Close()
                        .Open("Product", A, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)
                        .MoveFirst()
                        .Find("ProductID='" & FLDPid.Text & "'")
                        .Fields("ProductID").Value = FLDPid.Text
                        .Fields("ProductName").Value = FLDPName.Text
                        .Fields("ProductPrice").Value = FLDPrc.Text
                        .Fields("Category").Value = FLDCat.Text
                        .Fields("SupplierID").Value = FLDSi.Text                                                                        'Creates new record with the Level Of Access = to the text of the textbox
                        .Update()
                        MessageBox.Show("Product has been successfully updated", "SYSTEM")
                    End With
                End Try
            Else
                With B
                    .Close()
                    .Open("Product", A, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)
                    .MoveFirst()
                    .Find("ProductID='" & FLDPid.Text & "'")
                    .Fields("ProductID").Value = FLDPid.Text
                    .Fields("ProductName").Value = FLDPName.Text
                    .Fields("ProductPrice").Value = FLDPrc.Text
                    .Fields("Category").Value = FLDCat.Text
                    .Fields("SupplierID").Value = FLDSi.Text
                    .Update()
                    MessageBox.Show("Account has been successfully updated", "SYSTEM")
                    PullDataFromDatabase(d:=GRDInv, tableName:="formula")
                End With
            End If
            A.Close()
            Restrictions()
            FLDTv.Visibility = Visibility.Visible
            FLDUs.Visibility = Visibility.Visible
            FLDUa.Visibility = Visibility.Visible
            FLDPid.IsEnabled = True
            RecordLog(accountID:=AccountId, loa:=UserType, actionTaken:="AccountUpdated")
        End If
        PullDataFromDatabase(d:=GRDInv, tableName:="formula")
    End Sub

    Private Sub buttonCancel_Click(sender As Object, e As RoutedEventArgs) Handles buttonCancel.Click
        Dim can As String
        can = MessageBox.Show("Do you want to cancel ?", "SYSTEM", MessageBoxButton.YesNo, MessageBoxImage.Exclamation)
        If can = vbYes Then
            GRDInv.IsHitTestVisible = True
            GRDInv.UnselectAllCells()
            Restrictions()
            FLDPid.IsEnabled = True
            FLDTv.Visibility = Visibility.Visible
            FLDUs.Visibility = Visibility.Visible
            FLDUa.Visibility = Visibility.Visible
            BTNAddStocks.Content = "AddStocks"
            FLDadd.Text = 0
        End If
    End Sub

#Region "keypress"

    Private Sub FLDPid_PreviewKeyDown(sender As Object, e As KeyEventArgs) Handles FLDPid.PreviewKeyDown
        Dim x As String
        x = e.Key
        If x = Key.Back Or x = Key.Left Or x = Key.Right Then
            e.Handled = False
        ElseIf (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x >= Key.NumPad0 And x <= Key.NumPad9) Or (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x >= Key.D0 And x <= Key.D9) Or (x >= Key.A And x <= Key.Z) Or (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x = Key.OemMinus) Then
            e.Handled = False
        ElseIf x = Key.Tab Then
            FLDPid.Focus()
        ElseIf x = Key.Enter Or x = Key.Down Then
            FLDPName.Focus()
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub FLDPName_PreviewKeyDown(sender As Object, e As KeyEventArgs) Handles FLDPName.PreviewKeyDown
        Dim x As String
        x = e.Key
        If x = Key.Space Or x = Key.Back Or x = Key.Left Or x = Key.Right Then
            e.Handled = False
        ElseIf (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x >= Key.NumPad0 And x <= Key.NumPad9) Or (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x >= Key.D0 And x <= Key.D9) Or (x >= Key.A And x <= Key.Z) Or (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x = Key.OemMinus) Then
            e.Handled = False
        ElseIf x = Key.Tab Then
            FLDPName.Focus()
        ElseIf x = Key.Enter Or x = Key.Down Then
            FLDPrc.Focus()
        ElseIf x = Key.Up Then
            FLDPid.Focus()
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub FLDPrc_PreviewKeyDown(sender As Object, e As KeyEventArgs) Handles FLDPrc.PreviewKeyDown
        Dim x As String
        x = e.Key
        If x = Key.Back Or x = Key.Left Or x = Key.Right Then
            e.Handled = False
        ElseIf (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x >= Key.NumPad0 And x <= Key.NumPad9) Or (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x >= Key.D0 And x <= Key.D9) Or (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x = Key.OemPeriod) Then
            e.Handled = False
        ElseIf x = Key.Tab Then
            FLDPrc.Focus()
        ElseIf x = Key.Enter Or x = Key.Down Then
            FLDCat.Focus()
        ElseIf x = Key.Up Then
            FLDPName.Focus()
        Else
            e.Handled = True
        End If

    End Sub

    Private Sub FLDCat_PreviewKeyDown(sender As Object, e As KeyEventArgs) Handles FLDCat.PreviewKeyDown
        Dim x As String
        x = e.Key
        If x = Key.Space Or x = Key.Back Or x = Key.Left Or x = Key.Right Then
            e.Handled = False
        ElseIf (x >= Key.A And x <= Key.Z) Then
            e.Handled = False
        ElseIf x = Key.Tab Then
            FLDCat.Focus()
        ElseIf x = Key.Enter Or x = Key.Down Then
            FLDSi.Focus()
        ElseIf x = Key.Up Then
            FLDPrc.Focus()
        Else
            e.Handled = True
        End If
    End Sub


    Private Sub FLDSi_PreviewKeyDown(sender As Object, e As KeyEventArgs) Handles FLDSi.PreviewKeyDown
        Dim x As String
        x = e.Key
        If x = Key.Back Or x = Key.Left Or x = Key.Right Then
            e.Handled = False
        ElseIf (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x >= Key.NumPad0 And x <= Key.NumPad9) Or (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x >= Key.D0 And x <= Key.D9) Then
            e.Handled = False
        ElseIf x = Key.Tab Then
            FLDSi.Focus()
        ElseIf x = Key.Enter Or x = Key.Down Then
            buttonSave.Focus()
        ElseIf x = Key.Up Then
            FLDCat.Focus()
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub SearchTextBox_TextChanged(sender As Object, e As TextChangedEventArgs) Handles SearchTextBox.TextChanged
        CMBCategory.SelectedIndex = 0
        Dim oleDatabaseConnection As New OleDb.OleDbConnection(connectionString)
        oleDatabaseConnection.Open()
        Dim databasez As New OleDbCommand
        databasez.CommandText = "Select *  From Product Where ProductName Like '" & SearchTextBox.Text & "%'"
        databasez.Connection = oleDatabaseConnection
        Dim databaseActualTable As OleDbDataReader = databasez.ExecuteReader()
        GRDInv.ItemsSource = databaseActualTable
        GRDInv.Items.Refresh()
    End Sub



    Private Sub CMBCategory_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles CMBCategory.SelectionChanged
        Dim oleDatabaseConnection As New OleDb.OleDbConnection(connectionString)
        oleDatabaseConnection.Open()
        Dim databasez As New OleDbCommand
        If CMBCategory.SelectedIndex = 0 Then
            databasez.CommandText = "Select *  From Product"
            databasez.Connection = oleDatabaseConnection
            Dim databaseActualTable As OleDbDataReader = databasez.ExecuteReader()
            GRDInv.ItemsSource = databaseActualTable
            GRDInv.Items.Refresh()
        ElseIf CMBCategory.SelectedIndex = 1 Then
            databasez.CommandText = "Select *  From Product Where Category='Bed'"
            databasez.Connection = oleDatabaseConnection
            Dim databaseActualTable As OleDbDataReader = databasez.ExecuteReader()
            GRDInv.ItemsSource = databaseActualTable
            GRDInv.Items.Refresh()
        ElseIf CMBCategory.SelectedIndex = 2 Then
            databasez.CommandText = "Select *  From Product Where Category='Cabinet'"
            databasez.Connection = oleDatabaseConnection
            Dim databaseActualTable As OleDbDataReader = databasez.ExecuteReader()
            GRDInv.ItemsSource = databaseActualTable
            GRDInv.Items.Refresh()
        ElseIf CMBCategory.SelectedIndex = 3 Then
            databasez.CommandText = "Select *  From Product Where Category='Dining Chair'"
            databasez.Connection = oleDatabaseConnection
            Dim databaseActualTable As OleDbDataReader = databasez.ExecuteReader()
            GRDInv.ItemsSource = databaseActualTable
            GRDInv.Items.Refresh()
        ElseIf CMBCategory.SelectedIndex = 4 Then
            databasez.CommandText = "Select *  From Product Where Category='Dining Set'"
            databasez.Connection = oleDatabaseConnection
            Dim databaseActualTable As OleDbDataReader = databasez.ExecuteReader()
            GRDInv.ItemsSource = databaseActualTable
            GRDInv.Items.Refresh()
        ElseIf CMBCategory.SelectedIndex = 5 Then
            databasez.CommandText = "Select *  From Product Where Category='Dining Table'"
            databasez.Connection = oleDatabaseConnection
            Dim databaseActualTable As OleDbDataReader = databasez.ExecuteReader()
            GRDInv.ItemsSource = databaseActualTable
            GRDInv.Items.Refresh()
        ElseIf CMBCategory.SelectedIndex = 6 Then
            databasez.CommandText = "Select *  From Product Where Category='Sala Set'"
            databasez.Connection = oleDatabaseConnection
            Dim databaseActualTable As OleDbDataReader = databasez.ExecuteReader()
            GRDInv.ItemsSource = databaseActualTable
            GRDInv.Items.Refresh()
        End If

    End Sub



#End Region
End Class
