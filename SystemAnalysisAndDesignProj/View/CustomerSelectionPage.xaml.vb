Imports System.Data
Imports System.Data.OleDb

Class CustomerSelectionPage
    Public hasSelected As Boolean = False
    Public hasSelectedECustomer As Boolean = False
    Public hasSelectedNewCustomer As Boolean = False
    Public customerList As New List(Of Customer)
    Private databaseConnection As New OleDbConnection
    Private dt As New DataTable
    Public RBXText As String = ""
    Public isLoadedOnce As Boolean = False

    Private Sub BTNECustomer_Click(sender As Object, e As RoutedEventArgs) Handles BTNECustomer.Click
        FLDFirstName.Clear()
        hasSelected = False
        BTNProceedNewCustomer.Visibility = Visibility.Hidden
        BTNCloseNewCustomer.Visibility = Visibility.Hidden
        LBLNewCustomer.Visibility = Visibility.Visible
        ICONewCustomer.Visibility = Visibility.Visible
        LBLCustomerID.Visibility = Visibility.Hidden
        FLDContactNumber.Visibility = Visibility.Hidden
        LBLFirstName.Visibility = Visibility.Hidden
        FLDFirstName.Visibility = Visibility.Hidden
        LBLLastName.Visibility = Visibility.Hidden
        FLDLastname.Visibility = Visibility.Hidden
        LBLAddress.Visibility = Visibility.Hidden
        LFLDAddress.Visibility = Visibility.Hidden
        BTNProceedECustomer.Visibility = Visibility.Hidden
        CMBSearch.Focus()
        CMBSearch.IsDropDownOpen = True
        hasSelectedECustomer = True
        hasSelectedNewCustomer = False
        If Not hasSelected And Not hasSelectedNewCustomer Then
            hasSelected = True

            CMBSearch.Visibility = Visibility.Visible
            CMBSearch.IsHitTestVisible = False
            CMBSearch.IsDropDownOpen = True
            BTNCloseECustomer.Visibility = Visibility.Visible

            CMBSearch.Focus()
        End If
    End Sub

    Public Sub prepareDatabaseConnection()
        Try
            databaseConnection.ConnectionString = connectionString

            If databaseConnection.State <> ConnectionState.Open Then
                databaseConnection.Open()
            End If
            attachDatabaseToGrid()
        Catch ex As Exception
            'MessageBox.Show("Unable to load database to datagrid")
        End Try
    End Sub

    Private Sub attachDatabaseToGrid()
        Dim cmd As New OleDbCommand
        cmd.Connection = databaseConnection
        cmd.CommandText = "Select * from Customer"
        Dim da As New OleDbDataAdapter(cmd)
        da.Fill(dt)
    End Sub

    Private Sub LoadCustomersToList()
        If isLoadedOnce <> True Then
            For Each customerData As DataRow In dt.Rows
                Dim newInstanceOfCustomer As New Customer
                With newInstanceOfCustomer
                    .CustomerID = customerData(0)
                    .FirstName = customerData(1)
                    .LastName = customerData(2)
                    .Address = customerData(3)
                End With
                customerList.Add(newInstanceOfCustomer)
            Next
            customerList = customerList.OrderBy(Function(x) x.FullName).ToList
            CMBSearch.ItemsSource = customerList
            CMBSearch.DisplayMemberPath = "FullName"
        End If
    End Sub
    Private Sub SetToDefaults()
        CMBSearch.Visibility = Visibility.Hidden
        BTNCloseNewCustomer.Visibility = Visibility.Hidden
        BTNCloseECustomer.Visibility = Visibility.Hidden
        LBLCustomerID.Visibility = Visibility.Hidden
        FLDContactNumber.Visibility = Visibility.Hidden
        LBLFirstName.Visibility = Visibility.Hidden
        FLDFirstName.Visibility = Visibility.Hidden
        LBLLastName.Visibility = Visibility.Hidden
        FLDLastname.Visibility = Visibility.Hidden
        LBLAddress.Visibility = Visibility.Hidden
        LFLDAddress.Visibility = Visibility.Hidden
        BTNProceedECustomer.Visibility = Visibility.Hidden
        BTNProceedNewCustomer.Visibility = Visibility.Hidden
    End Sub

    Private Sub CheckSelectedCustomer()
        If hasSelectedECustomer Then
            CMBSearch.Visibility = Visibility.Visible
            BTNCloseECustomer.Visibility = Visibility.Visible
            CMBSearch.IsDropDownOpen = True
        ElseIf hasSelectedNewCustomer Then
            BTNCloseNewCustomer.Visibility = Visibility.Visible
            LBLNewCustomer.Visibility = Visibility.Hidden
            ICONewCustomer.Visibility = Visibility.Hidden
            LBLCustomerID.Visibility = Visibility.Visible
            FLDContactNumber.Visibility = Visibility.Visible
            LBLFirstName.Visibility = Visibility.Visible
            FLDFirstName.Visibility = Visibility.Visible
            LBLLastName.Visibility = Visibility.Visible
            FLDLastname.Visibility = Visibility.Visible
            LBLAddress.Visibility = Visibility.Visible
            LFLDAddress.Visibility = Visibility.Visible
        End If
    End Sub

    Private Sub CustomerSelectionPage_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        hasSelectedECustomer = False
        hasSelectedNewCustomer = False
        FLDContactNumber.Clear()
        FLDFirstName.Clear()
        FLDLastName.Clear()
        RBXText = ""
        ICONewCustomer.Visibility = Visibility.Visible
        LBLNewCustomer.Visibility = Visibility.Visible
        SetToDefaults()
        prepareDatabaseConnection()
        LoadCustomersToList()
        CheckSelectedCustomer()
        CurrentPage = Me
        PreviousPage = mmp
        isLoadedOnce = True


        If (FLDContactNumber.Text <> Nothing) And (FLDFirstName.Text <> Nothing) And (FLDLastName.Text <> Nothing) And (RBXText <> Nothing) Then
            BTNProceedNewCustomer.Visibility = Visibility.Visible
        Else
            BTNProceedNewCustomer.Visibility = Visibility.Hidden
        End If
    End Sub

    Private Sub BTNNewCustomer_Click(sender As Object, e As RoutedEventArgs) Handles BTNNewCustomer.Click
        hasSelected = False
        BTNCloseECustomer.Visibility = Visibility.Hidden
        CMBSearch.Text = ""
        CMBSearch.Visibility = Visibility.Hidden
        BTNProceedECustomer.Visibility = Visibility.Hidden
        hasSelectedECustomer = False
        hasSelectedNewCustomer = True
        BTNProceedNewCustomer.Visibility = Visibility.Hidden
        If Not hasSelected And Not hasSelectedECustomer Then
            hasSelected = True
            BTNCloseNewCustomer.Visibility = Visibility.Visible
            LBLNewCustomer.Visibility = Visibility.Hidden
            ICONewCustomer.Visibility = Visibility.Hidden
            LBLCustomerID.Visibility = Visibility.Visible
            FLDContactNumber.Visibility = Visibility.Visible
            LBLFirstName.Visibility = Visibility.Visible
            FLDFirstName.Visibility = Visibility.Visible
            LBLLastName.Visibility = Visibility.Visible
            FLDLastname.Visibility = Visibility.Visible
            LBLAddress.Visibility = Visibility.Visible
            LFLDAddress.Visibility = Visibility.Visible
            FLDFirstName.Focus()
        End If


    End Sub

    Private Sub BTNCloseECustomer_Click(sender As Object, e As RoutedEventArgs) Handles BTNCloseECustomer.Click
        hasSelected = False
        BTNCloseECustomer.Visibility = Visibility.Hidden
        CMBSearch.Text = ""
        CMBSearch.Visibility = Visibility.Hidden
        BTNProceedECustomer.Visibility = Visibility.Hidden
    End Sub

    Private Sub BTNCloseNewCustomer_Click(sender As Object, e As RoutedEventArgs) Handles BTNCloseNewCustomer.Click
        hasSelected = False
        BTNCloseNewCustomer.Visibility = Visibility.Hidden
        LBLNewCustomer.Visibility = Visibility.Visible
        ICONewCustomer.Visibility = Visibility.Visible
        LBLCustomerID.Visibility = Visibility.Hidden
        FLDContactNumber.Visibility = Visibility.Hidden
        LBLFirstName.Visibility = Visibility.Hidden
        FLDFirstName.Visibility = Visibility.Hidden
        LBLLastName.Visibility = Visibility.Hidden
        FLDLastname.Visibility = Visibility.Hidden
        LBLAddress.Visibility = Visibility.Hidden
        LFLDAddress.Visibility = Visibility.Hidden
        FLDFirstName.Clear()
    End Sub

    Private Sub CustomerSelectionPage_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If (FLDContactNumber.Text <> Nothing) And (FLDFirstName.Text <> Nothing) And (FLDLastname.Text <> Nothing) And (RBXText <> Nothing) Then
            BTNProceedNewCustomer.Visibility = Visibility.Visible
        Else
            BTNProceedNewCustomer.Visibility = Visibility.Hidden
        End If
    End Sub

    Private Sub LFLDAddress_KeyDown(sender As Object, e As KeyEventArgs) Handles LFLDAddress.KeyDown
        RBXText = New TextRange(LFLDAddress.Document.ContentStart, LFLDAddress.Document.ContentEnd).Text
    End Sub

    Private Sub BTNProceedNewCustomer_Click(sender As Object, e As RoutedEventArgs) Handles BTNProceedNewCustomer.Click


        Dim A As New ADODB.Connection
        Dim B As New ADODB.Recordset
        Dim LG As New ADODB.Connection
        Dim CL As New ADODB.Recordset
        A.Open(connectionString)
        B.Open("Select * from Customer where FirstName='" & FLDFirstName.Text & "'" & "and LastName='" & FLDLastname.Text & "'", A)

        Try
            If FLDFirstName.Text = B.Fields("FirstName").Value And FLDLastname.Text = B.Fields("LastName").Value Then
                MessageBox.Show("Customer already exists")
                FLDFirstName.Clear()
                FLDLastname.Clear()
                FLDFirstName.Focus()
            End If
        Catch ex As Exception


            With B
                .Close()
                .Open("Customer", A, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)
                .AddNew()
                .Fields("FirstName").Value = FLDFirstName.Text
                .Fields("LastName").Value = FLDLastname.Text
                .Fields("Birthdate").Value = Now
                .Fields("Address").Value = RBXText
                .Fields("Contact Number").Value = FLDContactNumber.Text
                .Update()
                .Close()
            End With
            A.Close()
            LG.Open(connectionString)
            CL.Open("Select * from Customer where FirstName='" & FLDFirstName.Text & "'" & "and LastName='" & FLDLastname.Text & "'", LG)
            Dim CID As String = ""
            CID = CL.Fields("CustomerID").Value

            RecordLog(accountID:=AccountId, loa:=UserType, actionTaken:="ADDED CUSTOMER")
            Dim newCurrentCustomer As New Customer
            With newCurrentCustomer
                .CustomerID = CID
                .FirstName = FLDFirstName.Text
                .LastName = FLDLastname.Text
                .Address = RBXText
            End With
            currentCustomer = newCurrentCustomer
            PreviousPage = csp
            frameMain.Content = cp

        End Try


    End Sub

    Private Sub BTNProceedECustomer_Click(sender As Object, e As RoutedEventArgs) Handles BTNProceedECustomer.Click
        currentCustomer = CMBSearch.SelectedItem
        MessageBox.Show(currentCustomer.CustomerID)
        MessageBox.Show(currentCustomer.FullName)
        frameMain.Content = cp
        PreviousPage = csp
        hasSelected = False
    End Sub

    Private Sub CMBSearch_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles CMBSearch.SelectionChanged
        If hasSelectedECustomer And CMBSearch.SelectedIndex > -1 Then
            BTNProceedECustomer.Visibility = Visibility.Visible
        End If

    End Sub

    Private Sub FLDFirstName_PreviewKeyDown(sender As Object, e As KeyEventArgs) Handles FLDFirstName.PreviewKeyDown
        Dim x As String
        x = e.Key
        If x = Key.Space Or x = Key.Back Or x = Key.Left Or x = Key.Right Then
            e.Handled = False
        ElseIf (x >= Key.A And x <= Key.Z) Or (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x = Key.OemMinus) Or (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x = Key.OemPeriod) Then
            e.Handled = False
        ElseIf x = Key.Tab Then
            FLDFirstName.Focus()
        ElseIf x = Key.Enter Or x = Key.Down Then
            FLDLastname.Focus()
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub FLDLastName_PreviewKeyDown(sender As Object, e As KeyEventArgs) Handles FLDLastname.PreviewKeyDown
        Dim x As String
        x = e.Key
        If x = Key.Space Or x = Key.Back Or x = Key.Left Or x = Key.Right Then
            e.Handled = False
        ElseIf (x >= Key.A And x <= Key.Z) Or (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x = Key.OemMinus) Or (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x = Key.OemPeriod) Then
            e.Handled = False
        ElseIf x = Key.Tab Then
            FLDLastname.Focus()
        ElseIf x = Key.Enter Or x = Key.Down Then
            FLDContactNumber.Focus()
        ElseIf x = Key.Up Then
            FLDFirstName.Focus()
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub FLDContactNumber_PreviewKeyDown(sender As Object, e As KeyEventArgs) Handles FLDContactNumber.PreviewKeyDown
        Dim x As String
        x = e.Key
        If x = Key.Back Or x = Key.Left Or x = Key.Right Then
            e.Handled = False
        ElseIf (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x >= Key.NumPad0 And x <= Key.NumPad9) Or (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x >= Key.D0 And x <= Key.D9) Then
            e.Handled = False
        ElseIf x = Key.Tab Then
            FLDContactNumber.Focus()
        ElseIf x = Key.Enter Or x = Key.Down Then
            LFLDAddress.Focus()
        ElseIf x = Key.Up Then
            FLDLastname.Focus()
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub LFLDAddress_PreviewKeyDown(sender As Object, e As KeyEventArgs) Handles LFLDAddress.PreviewKeyDown
        Dim x As String
        x = e.Key
        If x = Key.Space Or x = Key.Back Or x = Key.Left Or x = Key.Right Then
            e.Handled = False
        ElseIf (Keyboard.IsKeyDown(Key.RightShift) And x = Key.D3) Or (Keyboard.IsKeyDown(Key.LeftShift) And x = Key.D3) Then
            e.Handled = False
        ElseIf (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x >= Key.NumPad0 And x <= Key.NumPad9) Or (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x >= Key.D0 And x <= Key.D9) Or (x >= Key.A And x <= Key.Z) Or (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x = Key.OemMinus) Or (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x = Key.OemPeriod) Or (Keyboard.IsKeyUp(Key.RightShift) And Keyboard.IsKeyUp(Key.LeftShift) And x = Key.OemComma) Then
            e.Handled = False
        ElseIf x = Key.Tab Then
            LFLDAddress.Focus()
        ElseIf x = Key.Enter Or x = Key.Down Then
            BTNProceedNewCustomer.Focus()
        ElseIf x = Key.Up Then
            FLDContactNumber.Focus()
        Else
            e.Handled = True
        End If
    End Sub
End Class
