Imports System.Data
Imports System.Data.OleDb

Module General

    Dim DB As New ADODB.Connection
    Dim RS As New ADODB.Recordset
    Dim switchToPage As Page
    Public connectionString As String = "Provider=Microsoft.jet.oledb.4.0;Data Source=SAD_DB.mdb"
    Public runningTotal As Double = 0
    Public runningCount As Integer = 0
    Public qtySetter As Integer = 0
    Public isSelectingQuantity As Boolean
    Public isQuantitySelected As Boolean
    Public frameMain As Frame
    Public Usrnm As String
    Public UserType As String
    Public AccountId As String
    Public CurrentPage As Page
    Public PreviousPage As Page
    Public Stockstatus As String
    Public pagestatus As String
    Public currentCustomer As Customer
    Public isNewCustomer As Boolean = False
    Public isExistingCustomer As Boolean = False
    Public deliveryFee As Double = 217
    Public generatedreports As String
    Public DeliveryDate As String

    'navigatiion Initializers
    Public mmp As New MainMenuPage
    Public cp As New CashierPage
    Public invp As New InventoryPage
    Public accp As New AccountsPage
    Public csp As New CustomerSelectionPage
    Public custp As New CustomerPage
    Public lp As New LogsPage
    Public sp As New SalesPage
    Public sup As New SupplierPage
    Public dqp As New DeliveryScheduler

    'blank navigation Initializers
    Public bmmp As New MainMenu
    Public bcp As New CashierPage
    Public binvp As New InventoryPage
    Public baccp As New AccountsPage
    Public bcsp As New CustomerSelectionPage
    Public bcustp As New CustomerPage
    Public blp As New LogsPage
    Public bsp As New SalesPage
    Public bsup As New SupplierPage
    Public bdqp As New DeliveryScheduler

    Public mm As MainMenu

    Public Sub CloseProgram(ByRef w As Window)
        w.Close()
    End Sub
    Public Sub MinimizeProgram(ByRef w As Window)
        w.WindowState = WindowState.Minimized
    End Sub
    Public Sub ReturntoMain(ByRef w As Window)
        Dim mainPage As New MainMenuPage
        frameMain.Content = mainPage
    End Sub

    Public Sub MovetoCashier()
        Dim mainWindow As New MainMenu
        Dim cashierPage As New CashierPage
        mainWindow.MainFrame.Content = cashierPage
    End Sub

    Public Sub Notify()


    End Sub

    Public Sub LogOut(ByRef w As Window)
        'code here
    End Sub

    Public Sub PullDataFromDatabase(ByRef d As DataGrid, ByRef tableName As String)
        Dim oleDatabaseConnection As New OleDb.OleDbConnection(connectionString)
        oleDatabaseConnection.Open()
        Dim databasez As New OleDbCommand
        databasez.CommandText = "select * from " & tableName
        databasez.Connection = oleDatabaseConnection
        Dim databaseActualTable As OleDbDataReader = databasez.ExecuteReader()
        d.ItemsSource = databaseActualTable
    End Sub
    Public Sub RefreshTable(ByRef d As DataGrid, ByVal tableName As String)
        Dim oleDatabaseConnection As New OleDb.OleDbConnection(connectionString)
        oleDatabaseConnection.Open()
        Dim databasez As New OleDbCommand
        databasez.CommandText = "select * from " & tableName
        databasez.Connection = oleDatabaseConnection
        Dim databaseActualTable As OleDbDataReader = databasez.ExecuteReader()
        d.ItemsSource = databaseActualTable
        oleDatabaseConnection.Close()
    End Sub



    Public Sub RecordLog(ByVal accountID As String, ByVal loa As String, ByVal actionTaken As String)
        Using act As New OleDbConnection(connectionString)
            act.Open()
            Dim command As New OleDbCommand("insert into Logs ([AccountID],  [LevelofAccess], [ActionTaken], [ADate])  values ( @AccountId,  @access, @action, @adate)", act)
            Dim dt As String
            dt = Now.ToShortDateString & " / " & Now.ToShortTimeString
            With command.Parameters
                .AddWithValue("@AccountId", accountID.ToString)
                .AddWithValue("@access", loa.ToString)
                .AddWithValue("@action", actionTaken.ToString)
                .AddWithValue("@adate", dt.ToString)
            End With
            command.ExecuteNonQuery()
            command.Dispose()
            act.Close()
        End Using

    End Sub
#Region "Inventory"
    Public Sub InventorySave(ByVal buttonSave As Button, ByVal buttonAdd As Button, ByVal buttonEdit As Button, ByVal buttonDelete As Button, ByRef d As DataGrid, ByVal fldPID As TextBox, ByVal fldStID As TextBox, ByVal fldPN As TextBox, ByVal fldPP As TextBox, ByVal fldUS As TextBox, ByVal fldCat As TextBox, ByVal fldUA As TextBox, ByVal fldTV As TextBox, ByVal fldSID As TextBox, ByVal fldAva As TextBox, ByVal fldTax As TextBox, ByRef stkLBL As StackPanel, ByRef stkTXT As StackPanel)
        If buttonSave.Content = "SAVE" Then
            d.IsHitTestVisible = True
            Dim A As New ADODB.Connection
            Dim B As New ADODB.Recordset
            A.Open(connectionString)
            B.Open("Select * from Product where ProductID='" & fldPID.Text & "'", A)
            Try
                If fldPID.Text = "" Or fldStID.Text = "" Or fldPN.Text = "" Or fldPP.Text = "" Or fldUS.Text = "" Or fldCat.Text = "" Or fldUA.Text = "" Or fldTV.Text = "" Or fldSID.Text = "" Then
                    MessageBox.Show("All Fields under Product Account is needed to be filled out")
                ElseIf fldPID.Text = B.Fields("ProductID").Value Then
                    MessageBox.Show("Unable to have the same Product ID")
                End If
            Catch ex As Exception
                With B
                    .Close()
                    .Open("Product", A, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)
                    .MoveLast()
                    .AddNew()
                    .Fields("ProductID").Value = fldPID.Text
                    .Fields("StockID").Value = fldStID.Text
                    .Fields("ProductName").Value = fldPN.Text
                    .Fields("ProductPrice").Value = fldPP.Text
                    .Fields("UnitSolid").Value = fldUS.Text
                    .Fields("Category").Value = fldCat.Text
                    .Fields("UnitsAvailable").Value = fldUA.Text
                    .Fields("TaxValue").Value = fldTV.Text
                    .Fields("SupplierID").Value = fldSID.Text
                    .Fields("Available?").Value = fldAva.Text
                    .Fields("Taxable?").Value = fldTax.Text
                    .Update()
                    .Close()
                    MessageBox.Show("Product has been successfully added", "SYSTEM")
                End With
                A.Close()

                'Restrictions

                'RecordLog

            End Try
        End If

        If buttonSave.Content = "UPDATE" Then
            d.IsHitTestVisible = True
            Dim A As New ADODB.Connection
            Dim B As New ADODB.Recordset
            A.Open(connectionString)
            B.Open("Select * from Product where ProductID='" & fldPID.Text & "'", A)
            If fldPID.Text = "" Or fldStID.Text = "" Or fldPN.Text = "" Or fldPP.Text = "" Or fldUS.Text = "" Or fldCat.Text = "" Or fldUA.Text = "" Or fldTV.Text = "" Or fldSID.Text = "" Then
                MessageBox.Show("All Fields under Product Account is needed to be filled out")
            ElseIf B.Fields("StockID").Value = fldStID.Text And B.Fields("ProductName").Value = fldPN.Text And B.Fields("ProductPrice").Value = fldPP.Text And B.Fields("UnitSolid").Value = fldUS.Text And B.Fields("Category").Value = fldCat.Text And B.Fields("UnitsAvailable").Value = fldUA.Text And B.Fields("TaxValue").Value = fldTV.Text And B.Fields("SupplierID").Value = fldSID.Text And B.Fields("Available?").Value = fldAva.Text And B.Fields("Taxable?").Value = fldTax.Text Then
                MessageBox.Show("No changes made.", "SYSTEM")
            ElseIf fldPID.Text <> B.Fields("ProductID").Value Then
                B.Close()
                B.Open("Select * from Product where ProductID='" & fldPID.Text & "'", A)
                Try
                    If fldPN.Text = B.Fields("ProductName").Value Or fldPID.Text = B.Fields("ProductID").Value Then
                        MessageBox.Show("Product ID already taken. Please type another one.", "SYSTEM")
                        B.Close()
                    End If
                Catch ex As Exception
                    With B
                        .Close()
                        .Open("Product", A, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)
                        .MoveFirst()
                        .Find("ProductID='" & fldPID.Text & "'")
                        .Fields("ProductID").Value = fldPID.Text
                        .Fields("StockID").Value = fldStID.Text
                        .Fields("ProductName").Value = fldPN.Text
                        .Fields("ProductPrice").Value = fldPP.Text
                        .Fields("UnitSolid").Value = fldUS.Text
                        .Fields("Category").Value = fldCat.Text
                        .Fields("UnitsAvailable").Value = fldUA.Text
                        .Fields("TaxValue").Value = fldTV.Text
                        .Fields("SupplierID").Value = fldSID.Text
                        .Fields("Available?").Value = fldAva.Text
                        .Fields("Taxable?").Value = fldTax.Text
                        .Update()
                        MessageBox.Show("Product has been successfully updated", "SYSTEM")
                    End With
                End Try
            Else
                With B
                    .Close()
                    .Open("Product", A, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)
                    .MoveFirst()
                    .Find("ProductID='" & fldPID.Text & "'")
                    .Fields("ProductID").Value = fldPID.Text
                    .Fields("StockID").Value = fldStID.Text
                    .Fields("ProductName").Value = fldPN.Text
                    .Fields("ProductPrice").Value = fldPP.Text
                    .Fields("UnitSolid").Value = fldUS.Text
                    .Fields("Category").Value = fldCat.Text
                    .Fields("UnitsAvailable").Value = fldUA.Text
                    .Fields("TaxValue").Value = fldTV.Text
                    .Fields("SupplierID").Value = fldSID.Text
                    .Fields("Available?").Value = fldAva.Text
                    .Fields("Taxable?").Value = fldTax.Text
                    .Update()
                    MessageBox.Show("Product has been successfully updated", "SYSTEM")
                End With
            End If
            A.Close()

            'Restrictions

            'RecordLog

        End If

        'RefreshTable

    End Sub
    Public Sub InventoryAdd(ByVal buttonSave As Button, ByVal buttonAdd As Button, ByVal buttonEdit As Button, ByVal buttonDelete As Button, ByRef d As DataGrid, ByVal fldPID As TextBox, ByVal fldStID As TextBox, ByVal fldPN As TextBox, ByVal fldPP As TextBox, ByVal fldUS As TextBox, ByVal fldCat As TextBox, ByVal fldUA As TextBox, ByVal fldTV As TextBox, ByVal fldSID As TextBox, ByVal fldAva As TextBox, ByVal fldTax As TextBox, ByRef stkLBL As StackPanel, ByRef stkTXT As StackPanel, ByRef chkAva As CheckBox, ByRef chkTax As CheckBox)
        'AddEdit
        d.UnselectAllCells()
        fldPID.Clear()
        fldStID.Clear()
        fldPN.Clear()
        fldPP.Clear()
        fldUS.Clear()
        fldCat.Clear()
        fldUA.Clear()
        fldTV.Clear()
        fldSID.Clear()
        fldTax.Text = "No"
        fldAva.Text = "No"
        chkAva.IsChecked = False
        chkTax.IsChecked = False
        buttonSave.Content = "SAVE"
        d.IsHitTestVisible = False
        fldPID.IsEnabled = True
    End Sub
    Public Sub InventoryEdit(ByVal buttonSave As Button, ByVal buttonAdd As Button, ByVal buttonEdit As Button, ByVal buttonDelete As Button, ByRef d As DataGrid, ByVal fldPID As TextBox, ByVal fldStID As TextBox, ByVal fldPN As TextBox, ByVal fldPP As TextBox, ByVal fldUS As TextBox, ByVal fldCat As TextBox, ByVal fldUA As TextBox, ByVal fldTV As TextBox, ByVal fldSID As TextBox, ByVal fldAva As TextBox, ByVal fldTax As TextBox, ByRef stkLBL As StackPanel, ByRef stkTXT As StackPanel, ByRef chkAva As CheckBox, ByRef chkTax As CheckBox)
        'AddEdit
        d.IsHitTestVisible = False
        buttonSave.Content = "UPDATE"
        fldPID.IsEnabled = False
    End Sub
    Public Sub InventoryDelete(ByVal buttonSave As Button, ByVal buttonAdd As Button, ByVal buttonEdit As Button, ByVal buttonDelete As Button, ByRef d As DataGrid, ByVal fldPID As TextBox, ByVal fldStID As TextBox, ByVal fldPN As TextBox, ByVal fldPP As TextBox, ByVal fldUS As TextBox, ByVal fldCat As TextBox, ByVal fldUA As TextBox, ByVal fldTV As TextBox, ByVal fldSID As TextBox, ByVal fldAva As TextBox, ByVal fldTax As TextBox, ByRef stkLBL As StackPanel, ByRef stkTXT As StackPanel, ByRef chkAva As CheckBox, ByRef chkTax As CheckBox)
        If MsgBox("Are you sure you want to Delete?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Message") = MsgBoxResult.Yes Then
            Try
                DB.Open(connectionString)
                DB.Execute("Delete * From Product Where ProductID= '" & fldPID.Text & "'")
                MessageBox.Show("Record was Deleted", "SYSTEM")
                DB.Close()

                'RecordLog

            Catch EX As Exception
            End Try
        End If

        'RefreshTable

        'Restriction
    End Sub
#End Region

#Region "Accounts"
    Public Sub AccountsSave(ByVal buttonSave As Button, buttonAdd As Button, ByVal buttonEdit As Button, ByVal buttonDelete As Button, ByRef d As DataGrid, ByVal fldUsr As TextBox, ByVal fldPsw As TextBox, ByVal fldFN As TextBox, ByVal fldLN As TextBox, ByVal DPBD As DatePicker, ByVal fldAdr As TextBox, ByVal CBLoA As ComboBox, ByVal fldAct As TextBox, ByRef stkLBL As StackPanel, ByRef stkTXT As StackPanel)
        If buttonSave.Content = "SAVE" Then
            d.IsHitTestVisible = True
            Dim A As New ADODB.Connection
            Dim B As New ADODB.Recordset
            A.Open(connectionString)
            B.Open("Select * from tblUsers where Username='" & fldUsr.Text & "'", A)
            Try
                If fldUsr.Text = "" Or fldPsw.Text = "" Or fldFN.Text = "" Or fldLN.Text = "" Or DPBD.Text = "" Or CBLoA.Text = "" Then
                    MessageBox.Show("All Fields under User Account is needed to be filled out")
                ElseIf fldUsr.Text = B.Fields("Username").Value Then
                    MessageBox.Show("Unable to have the same Username")
                End If
            Catch ex As Exception
                With B
                    .Close()
                    .Open("tblUsers", A, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)
                    .MoveLast()
                    .AddNew()
                    .Fields("Username").Value = fldUsr.Text
                    .Fields("Password").Value = fldPsw.Text
                    .Fields("FirstName").Value = fldFN.Text
                    .Fields("LastName").Value = fldLN.Text
                    .Fields("Birthdate").Value = DPBD.SelectedDate
                    .Fields("Address").Value = fldAdr.Text
                    .Fields("LevelofAccess").Value = CBLoA.SelectedItem
                    .Update()
                    .Close()
                End With
                A.Close()

                'Restrictions
                fldAct.Visibility = Visibility.Visible

                'RecordLog

            End Try
        End If

        If buttonSave.Content = "UPDATE" Then
            d.IsHitTestVisible = True
            Dim A As New ADODB.Connection
            Dim B As New ADODB.Recordset
            A.Open(connectionString)
            B.Open("Select * from tblUsers where AccountID=" & fldAct.Text & "", A)
            If fldUsr.Text = "" Or fldPsw.Text = "" Or fldFN.Text = "" Or fldLN.Text = "" Or DPBD.Text = "" Or CBLoA.Text = "" Then
                MessageBox.Show("All Fields under User Account is needed to be filled out")
            ElseIf B.Fields("Username").Value = fldUsr.Text And B.Fields("Password").Value = fldPsw.Text And B.Fields("Firstname").Value = fldFN.Text And B.Fields("Lastname").Value = fldLN.Text And B.Fields("Birthdate").Value = DPBD.Text And B.Fields("Address").Value = fldAdr.Text And B.Fields("LevelofAccess").Value = CBLoA.Text Then
                MessageBox.Show("No changes made.", "SYSTEM")
            ElseIf fldUsr.Text <> B.Fields("Username").Value Then
                B.Close()
                B.Open("Select * from tblUsers where Username='" & fldUsr.Text & "'", A)
                Try
                    If fldUsr.Text = B.Fields("Username").Value Then
                        MessageBox.Show("User name already taken. Please type another one.", "SYSTEM")
                        B.Close()
                    End If
                Catch ex As Exception
                    With B
                        .Close()
                        .Open("tblUsers", A, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)
                        .MoveFirst()
                        .Find("AccountID='" & fldAct.Text & "'")
                        .Fields("Username").Value = fldUsr.Text                                                                                     'Creates new record with the Username = to the text of the textbox
                        .Fields("Password").Value = fldPsw.Text                                                                                     'Creates new record with the Password = to the text of the textbox
                        .Fields("FirstName").Value = fldFN.Text                                                                                     'Creates new record with the First Name = to the text of the textbox                                                                                         
                        .Fields("LastName").Value = fldLN.Text                                                                                      'Creates new record with the Last Name = to the text of the textbox
                        .Fields("Birthdate").Value = DPBD.SelectedDate                                                                           'Creates new record with the SelectedDate = to the text of the textbox
                        .Fields("Address").Value = fldAdr.Text                                                                                      'Creates new record with the Address = to the text of the textbox
                        .Fields("LevelofAccess").Value = CBLoA.SelectedItem                                                                         'Creates new record with the Level Of Access = to the text of the textbox
                        .Update()
                        MessageBox.Show("Account has been successfully updated", "SYSTEM")
                    End With
                End Try
            Else
                With B
                    .Close()
                    .Open("tblUsers", A, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)
                    .MoveFirst()
                    .Find("AccountID='" & fldAct.Text & "'")
                    .Fields("Username").Value = fldUsr.Text
                    .Fields("Password").Value = fldPsw.Text
                    .Fields("FirstName").Value = fldFN.Text
                    .Fields("LastName").Value = fldLN.Text
                    .Fields("Birthdate").Value = DPBD.SelectedDate
                    .Fields("Address").Value = fldAdr.Text
                    .Fields("LevelofAccess").Value = CBLoA.SelectedItem
                    .Update()
                    MessageBox.Show("Account has been successfully updated", "SYSTEM")
                End With
            End If
            A.Close()

            'Restrictions
            fldAct.Visibility = Visibility.Visible

            'RecordLog

        End If

        'RefreshTable

    End Sub
    Public Sub AccountsAdd(ByVal buttonSave As Button, buttonAdd As Button, ByVal buttonEdit As Button, ByVal buttonDelete As Button, ByRef d As DataGrid, ByVal fldUsr As TextBox, ByVal fldPsw As TextBox, ByVal fldFN As TextBox, ByVal fldLN As TextBox, ByVal DPBD As DatePicker, ByVal fldAdr As TextBox, ByVal CBLoA As ComboBox, ByVal fldAct As TextBox, ByRef stkLBL As StackPanel, ByRef stkTXT As StackPanel)
        fldAct.Visibility = Visibility.Hidden
        'AddEdit
        d.UnselectAllCells()
        fldAct.Clear()
        fldAdr.Clear()
        fldFN.Clear()
        fldLN.Clear()
        fldPsw.Clear()
        fldUsr.Clear()
        CBLoA.Items.Clear()
        CBLoA.Items.Add("Administrator")
        CBLoA.Items.Add("Cashier")
        d.IsHitTestVisible = False
        buttonSave.Content = "SAVE"
    End Sub
    Public Sub AccountsEdit(ByVal buttonSave As Button, buttonAdd As Button, ByVal buttonEdit As Button, ByVal buttonDelete As Button, ByRef d As DataGrid, ByVal fldUsr As TextBox, ByVal fldPsw As TextBox, ByVal fldFN As TextBox, ByVal fldLN As TextBox, ByVal DPBD As DatePicker, ByVal fldAdr As TextBox, ByVal CBLoA As ComboBox, ByVal fldAct As TextBox, ByRef stkLBL As StackPanel, ByRef stkTXT As StackPanel)
        fldAct.Visibility = Visibility.Hidden
        'AddEdit
        d.IsHitTestVisible = False
        buttonSave.Content = "UPDATE"
    End Sub
    Public Sub AccountsDelete(ByVal buttonSave As Button, buttonAdd As Button, ByVal buttonEdit As Button, ByVal buttonDelete As Button, ByRef d As DataGrid, ByVal fldUsr As TextBox, ByVal fldPsw As TextBox, ByVal fldFN As TextBox, ByVal fldLN As TextBox, ByVal DPBD As DatePicker, ByVal fldAdr As TextBox, ByVal CBLoA As ComboBox, ByVal fldAct As TextBox, ByRef stkLBL As StackPanel, ByRef stkTXT As StackPanel)
        If CBLoA.Text = "Administrator" Then
            MessageBox.Show("Administrator account cannot be deleted.")
        ElseIf MsgBox("Are you sure you want to Delete?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Message") = MsgBoxResult.Yes Then
            Try
                DB.Open(connectionString)
                DB.Execute("Delete * From tblUsers Where Username= '" & fldUsr.Text & "'")
                MessageBox.Show("Record was Deleted", "SYSTEM")
                DB.Close()

                'RecordLog

            Catch EX As Exception
            End Try
        End If

        'RefreshTable

        fldAct.Visibility = Visibility.Visible
        'Restriction
    End Sub
#End Region

#Region "Cashier (code here)"
    Public Sub CashierSave()

    End Sub
    Public Sub CashierAdd()

    End Sub
    Public Sub CashierEdit()

    End Sub
    Public Sub CashierDelete()

    End Sub
#End Region

#Region "Delivery (code here)"
    Public Sub DeliverySave()

    End Sub
    Public Sub DeliveryAdd()

    End Sub
    Public Sub DeliveryEdit()

    End Sub
    Public Sub DeliveryDelete()

    End Sub
#End Region

#Region "DeliveryMan"
    Public Sub DeliveryManSave(ByVal buttonSave As Button, buttonAdd As Button, ByVal buttonEdit As Button, ByVal buttonDelete As Button, ByRef d As DataGrid, ByRef stkLBL As StackPanel, ByRef stkTXT As StackPanel, ByVal fldFN As TextBox, ByVal fldLN As TextBox, ByVal fldOD As TextBox, ByVal fldCN As TextBox, ByVal fldID As TextBox)
        If buttonSave.Content = "SAVE" Then
            d.IsHitTestVisible = True
            Dim A As New ADODB.Connection
            Dim B As New ADODB.Recordset

            A.Open(connectionString)
            B.Open("Select * from DeliveryMan where FirstName='" & fldFN.Text & "'", A)

            Try
                If fldFN.Text = "" Or fldLN.Text = "" Or fldOD.Text = "" Or fldCN.Text = "" Then
                    MessageBox.Show("All Fields under Delivery Men is needed to be filled out")
                ElseIf fldFN.Text = B.Fields("FirstName").Value Then
                    MessageBox.Show("Unable to have the same Name")
                End If
            Catch ex As Exception
                With B
                    .Close()
                    .Open("DeliveryMan", A, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)
                    .MoveLast()
                    .AddNew()
                    .Fields("FirstName").Value = fldFN.Text
                    .Fields("LastName").Value = fldLN.Text
                    .Fields("OnDelivey").Value = fldOD.Text
                    .Fields("ContactNumber").Value = fldCN.Text
                    .Update()
                    .Close()
                    MessageBox.Show("Delivery Man has been successfully added", "SYSTEM")
                End With
                A.Close()

                'Restrictions

                'RecordLog

            End Try
        End If

        If buttonSave.Content = "UPDATE" Then
            d.IsHitTestVisible = True
            Dim A As New ADODB.Connection
            Dim B As New ADODB.Recordset
            A.Open(connectionString)
            B.Open("Select * from DeliveryMan where FirstName='" & fldFN.Text & "'", A)
            If fldFN.Text = "" Or fldLN.Text = "" Or fldOD.Text = "" Or fldCN.Text = "" Then
                MessageBox.Show("All Fields under Delivery Men is needed to be filled out")
            ElseIf B.Fields("DeliveymanID").Value = fldID.Text And B.Fields("FirstName").Value = fldFN.Text And B.Fields("LastName").Value = fldLN.Text And B.Fields("OnDelivey").Value = fldOD.Text And B.Fields("ContactNumer").Value = fldCN.Text Then
                MessageBox.Show("No changes made.", "SYSTEM")
            ElseIf fldFN.Text <> B.Fields("FirstName").Value Then
                B.Close()
                B.Open("Select * from DeliveryMan where FirstName='" & fldFN.Text & "'", A)
                Try
                    If fldFN.Text = B.Fields("FirstName").Value Then
                        MessageBox.Show("Name already taken. Please type another one.", "SYSTEM")
                        B.Close()
                    End If
                Catch ex As Exception
                    With B
                        .Close()
                        .Open("DeliveryMan", A, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)
                        .MoveFirst()
                        .Find("DeliveymanID='" & fldID.Text & "'")
                        .Fields("FirstName").Value = fldFN.Text
                        .Fields("LastName").Value = fldLN.Text
                        .Fields("OnDelivey").Value = fldOD.Text
                        .Fields("ContactNumer").Value = fldCN.Text
                        .Update()
                        MessageBox.Show("Delivery Man has been successfully updated", "SYSTEM")
                    End With
                End Try
            Else
                With B
                    .Close()
                    .Open("DeliveryMan", A, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)
                    .MoveFirst()
                    .Find("DeliveymanID='" & fldID.Text & "'")
                    .Fields("FirstName").Value = fldFN.Text
                    .Fields("LastName").Value = fldLN.Text
                    .Fields("OnDelivey").Value = fldOD.Text
                    .Fields("ContactNumer").Value = fldCN.Text
                    .Update()
                    MessageBox.Show("Delivery Man has been successfully updated", "SYSTEM")
                End With
            End If
            A.Close()
            'Restrictions

            'RecordLog

        End If

        'RefreshTable

    End Sub
    Public Sub DeliveryManAdd(ByVal buttonSave As Button, buttonAdd As Button, ByVal buttonEdit As Button, ByVal buttonDelete As Button, ByRef d As DataGrid, ByRef stkLBL As StackPanel, ByRef stkTXT As StackPanel, ByVal fldFN As TextBox, ByVal fldLN As TextBox, ByVal fldOD As TextBox, ByVal fldCN As TextBox, ByVal fldID As TextBox)
        'AddEdit
        d.UnselectAllCells()
        fldID.Clear()
        fldFN.Clear()
        fldLN.Clear()
        fldOD.Clear()
        fldCN.Clear()
        buttonSave.Content = "SAVE"
        d.IsHitTestVisible = False
        fldFN.IsEnabled = True
    End Sub
    Public Sub DeliveryManEdit(ByVal buttonSave As Button, buttonAdd As Button, ByVal buttonEdit As Button, ByVal buttonDelete As Button, ByRef d As DataGrid, ByRef stkLBL As StackPanel, ByRef stkTXT As StackPanel, ByVal fldFN As TextBox, ByVal fldLN As TextBox, ByVal fldOD As TextBox, ByVal fldCN As TextBox, ByVal fldID As TextBox)
        'AddEdit
        d.IsHitTestVisible = False
        buttonSave.Content = "UPDATE"
        fldFN.IsEnabled = False
    End Sub
    Public Sub DeliveryManDelete(ByVal buttonSave As Button, buttonAdd As Button, ByVal buttonEdit As Button, ByVal buttonDelete As Button, ByRef d As DataGrid, ByRef stkLBL As StackPanel, ByRef stkTXT As StackPanel, ByVal fldFN As TextBox, ByVal fldLN As TextBox, ByVal fldOD As TextBox, ByVal fldCN As TextBox, ByVal fldID As TextBox)
        If MsgBox("Are you sure you want to Delete?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Message") = MsgBoxResult.Yes Then
            Try
                DB.Open(connectionString)
                DB.Execute("Delete * From DeliveryMan Where FirstName= '" & fldFN.Text & "'")
                MessageBox.Show("Record was Deleted", "SYSTEM")
                DB.Close()

                'RecordLog

            Catch EX As Exception
            End Try
        End If

        'RefreshTable

        'Restriction
    End Sub
#End Region

#Region "Logs (code here)"
    Public Sub LogsSave()

    End Sub
    Public Sub LogsAdd()

    End Sub
    Public Sub LogsEdit()

    End Sub
    Public Sub LogsDelete()

    End Sub
#End Region

#Region "Trends (code here)"
    Public Sub TrendsSave()

    End Sub
    Public Sub TrendsAdd()

    End Sub
    Public Sub TrendsEdit()

    End Sub
    Public Sub TrendsDelete()

    End Sub
#End Region

#Region "User (code here)"
    Public Sub UserSave()

    End Sub
    Public Sub UserAdd()

    End Sub
    Public Sub UserEdit()

    End Sub
    Public Sub UserDelete()

    End Sub
#End Region
End Module
