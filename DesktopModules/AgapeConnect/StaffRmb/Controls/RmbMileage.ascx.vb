Imports System.Linq
Partial Class controls_Mileage
    Inherits Entities.Modules.PortalModuleBase
    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        Dim FileName As String = System.IO.Path.GetFileNameWithoutExtension(Me.AppRelativeVirtualPath)
        If Not (Me.ID Is Nothing) Then
            'this will fix it when its placed as a ChildUserControl 
            Me.LocalResourceFile = Me.LocalResourceFile.Replace(Me.ID, FileName)
        Else
            ' this will fix it when its dynamically loaded using LoadControl method 
            Me.LocalResourceFile = Me.LocalResourceFile & FileName & ".ascx.resx"
            Dim Locale = System.Threading.Thread.CurrentThread.CurrentCulture.Name
            Dim AppLocRes As New System.IO.DirectoryInfo(Me.LocalResourceFile.Replace(FileName & ".ascx.resx", ""))
            If Locale = PortalSettings.CultureCode Then
                'look for portal varient
                If AppLocRes.GetFiles(FileName & ".ascx.Portal-" & PortalId & ".resx").Count > 0 Then
                    Me.LocalResourceFile = Me.LocalResourceFile.Replace("resx", "Portal-" & PortalId & ".resx")
                End If
            Else

                If AppLocRes.GetFiles(FileName & ".ascx." & Locale & ".Portal-" & PortalId & ".resx").Count > 0 Then
                    'lookFor a CulturePortalVarient
                    Me.LocalResourceFile = Me.LocalResourceFile.Replace("resx", Locale & ".Portal-" & PortalId & ".resx")
                ElseIf AppLocRes.GetFiles(FileName & ".ascx." & Locale & ".resx").Count > 0 Then
                    'look for a CultureVarient
                    Me.LocalResourceFile = Me.LocalResourceFile.Replace("resx", Locale & ".resx")
                ElseIf AppLocRes.GetFiles(FileName & ".ascx.Portal-" & PortalId & ".resx").Count > 0 Then
                    'lookFor a PortalVarient
                    Me.LocalResourceFile = Me.LocalResourceFile.Replace("resx", "Portal-" & PortalId & ".resx")
                End If
            End If
        End If
    End Sub

   

    Public Property Comment() As String
        Get
            Return tbDesc.Text
        End Get
        Set(ByVal value As String)
            tbDesc.Text = value

           

        End Set
    End Property
    Public Property theDate() As Date
        Get
            Return CDate(dtDate.Text)
        End Get
        Set(ByVal value As Date)
            If value = Nothing Then
                dtDate.Text = Today.ToShortDateString
            Else
                dtDate.Text = value
            End If
        End Set
    End Property
    Public Property VAT() As Boolean
        Get
            Return False

        End Get
        Set(ByVal value As Boolean)
        

        End Set
    End Property

    Public Property Amount() As Double
        Get
            If tbMiles.Text <> "" Then
                Return (CInt(tbMiles.Text) * CDbl(ddlVehicleType.SelectedValue))
            Else
                Return 0
            End If

        End Get
        Set(ByVal value As Double)
            'tbMiles.Text = CInt(value / ((ddlVehicleType.SelectedValue + (5 * CInt(ddlStaff.SelectedValue))) / 100))
        End Set
    End Property
    Public Property Spare1() As String
        Get
            Return ddlStaff.SelectedValue
        End Get
        Set(ByVal value As String)
            If value Is Nothing Or value = "" Then
                ddlStaff.SelectedValue = "0"
            Else
                ddlStaff.SelectedValue = CInt(value)

            End If
        End Set
    End Property
    Public Property Spare2() As String
        Get
           
            Return tbMiles.Text
        End Get
        Set(ByVal value As String)
            Try
                tbMiles.Text = CInt(value)
            Catch ex As Exception
                tbMiles.Text = 0
            End Try


        End Set
    End Property
    Public Property Spare3() As String
        Get
            Return CInt(ddlVehicleType.SelectedValue)
        End Get
        Set(ByVal value As String)
            Try
                ddlVehicleType.SelectedValue = value
            Catch ex As Exception
                ddlVehicleType.SelectedIndex = 0

            End Try
        End Set
    End Property
    Public Property Spare4() As String
        Get
            Return ""
        End Get
        Set(ByVal value As String)

        End Set
    End Property
    Public Property Spare5() As String
        Get
            Return hfAddStaffRate.Value
        End Get
        Set(ByVal value As String)

        End Set
    End Property
    Public Property Receipt() As Boolean
        Get
            Return False ' ddlVATReceipt.SelectedValue = "Yes"
        End Get
        Set(ByVal value As Boolean)
            'If value = False Then
            '    ddlVATReceipt.SelectedValue = "No"
            'Else
            '    ddlVATReceipt.SelectedValue = "Yes"
            'End If
        End Set
    End Property

    Public Property Taxable() As Boolean
        Get
            Return False
        End Get
        Set(ByVal value As Boolean)

        End Set
    End Property
    Public Function ValidateForm(ByVal userId As Integer) As Boolean
        If tbDesc.Text = "" Then
            ErrorLbl.Text = DotNetNuke.Services.Localization.Localization.GetString("Description.Error", LocalResourceFile)
            Return False
        End If
        Try
            Dim theDate As Date = dtDate.Text
            If theDate > Today Then
                ErrorLbl.Text = DotNetNuke.Services.Localization.Localization.GetString("OldDate.Error", LocalResourceFile)
                Return False
            End If
        Catch ex As Exception
            ErrorLbl.Text = DotNetNuke.Services.Localization.Localization.GetString("Date.Error", LocalResourceFile)
            Return False
        End Try

        Try
            Dim theMiles As Double = tbMiles.Text
            If theMiles <= 0 Then
                ErrorLbl.Text = DotNetNuke.Services.Localization.Localization.GetString("Reverse.Error", LocalResourceFile)
                Return False
            End If
        Catch ex As Exception
            ErrorLbl.Text = DotNetNuke.Services.Localization.Localization.GetString("Miles.Error", LocalResourceFile)
            Return False
        End Try

        Dim staff As New ArrayList
        Dim staff2 As New ArrayList
        Dim StaffCount = ddlStaff.SelectedValue
        If StaffCount > 0 Then staff.Add(DDL1.SelectedValue)
        If StaffCount > 1 Then staff.Add(DDL2.SelectedValue)
        If StaffCount > 2 Then staff.Add(DDL3.SelectedValue)
        If StaffCount > 3 Then staff.Add(DDL4.SelectedValue)
        If StaffCount > 4 Then staff.Add(DDL5.SelectedValue)
        If StaffCount > 5 Then staff.Add(DDL6.SelectedValue)
        If StaffCount > 6 Then staff.Add(DDL7.SelectedValue)
        If StaffCount > 7 Then staff.Add(DDL8.SelectedValue)

        'If ( staff.Contains(CStr(userId)) Then
        '    ErrorLbl.Text = "Error: You cannot list yourself as a passenger."
        '    Return False
        'End If

        For i As Integer = 0 To StaffCount - 1
            If staff(i) = userId Then
                ErrorLbl.Text = "Error: You cannot list yourself as a passenger."

                Return False
            ElseIf staff2.Contains(staff(i)) Then
                ErrorLbl.Text = "Error: You have the same passenger listed twice."

                Return False
            Else
                staff2.Add(staff(i))
            End If

        Next

        ErrorLbl.Text = ""




        Return True
    End Function


    Public Sub AddStaff(ByVal RmbLineNo As System.Int64)

        'Dim d As New AgapeStaff.AgapeStaffDataContext
        'Dim q = From c In d.Agape_Staff_RmbLineAddStaffs Where c.RmbLineId = RmbLineNo
        'd.Agape_Staff_RmbLineAddStaffs.DeleteAllOnSubmit(q)
        'd.SubmitChanges()

        'Dim StaffCount = ddlStaff.SelectedValue
        'If StaffCount > 0 Then
        '    Dim insert As New AgapeStaff.Agape_Staff_RmbLineAddStaff
        '    insert.UserId = DDL1.SelectedValue
        '    insert.Name = DDL1.SelectedItem.Text
        '    insert.RmbLineId = RmbLineNo
        '    d.Agape_Staff_RmbLineAddStaffs.InsertOnSubmit(insert)
        'End If
        'If StaffCount > 1 Then
        '    Dim insert As New AgapeStaff.Agape_Staff_RmbLineAddStaff
        '    insert.UserId = DDL2.SelectedValue
        '    insert.Name = DDL2.SelectedItem.Text
        '    insert.RmbLineId = RmbLineNo
        '    d.Agape_Staff_RmbLineAddStaffs.InsertOnSubmit(insert)
        'End If
        'If StaffCount > 2 Then
        '    Dim insert As New AgapeStaff.Agape_Staff_RmbLineAddStaff
        '    insert.UserId = DDL3.SelectedValue
        '    insert.Name = DDL3.SelectedItem.Text
        '    insert.RmbLineId = RmbLineNo
        '    d.Agape_Staff_RmbLineAddStaffs.InsertOnSubmit(insert)
        'End If
        'If StaffCount > 3 Then
        '    Dim insert As New AgapeStaff.Agape_Staff_RmbLineAddStaff
        '    insert.UserId = DDL4.SelectedValue
        '    insert.Name = DDL4.SelectedItem.Text
        '    insert.RmbLineId = RmbLineNo
        '    d.Agape_Staff_RmbLineAddStaffs.InsertOnSubmit(insert)
        'End If
        'If StaffCount > 4 Then
        '    Dim insert As New AgapeStaff.Agape_Staff_RmbLineAddStaff
        '    insert.UserId = DDL5.SelectedValue
        '    insert.Name = DDL5.SelectedItem.Text
        '    insert.RmbLineId = RmbLineNo
        '    d.Agape_Staff_RmbLineAddStaffs.InsertOnSubmit(insert)
        'End If
        'If StaffCount > 5 Then
        '    Dim insert As New AgapeStaff.Agape_Staff_RmbLineAddStaff
        '    insert.UserId = DDL6.SelectedValue
        '    insert.Name = DDL6.SelectedItem.Text
        '    insert.RmbLineId = RmbLineNo
        '    d.Agape_Staff_RmbLineAddStaffs.InsertOnSubmit(insert)
        'End If
        'If StaffCount > 6 Then
        '    Dim insert As New AgapeStaff.Agape_Staff_RmbLineAddStaff
        '    insert.UserId = DDL7.SelectedValue
        '    insert.Name = DDL7.SelectedItem.Text
        '    insert.RmbLineId = RmbLineNo
        '    d.Agape_Staff_RmbLineAddStaffs.InsertOnSubmit(insert)
        'End If
        'If StaffCount > 7 Then
        '    Dim insert As New AgapeStaff.Agape_Staff_RmbLineAddStaff
        '    insert.UserId = DDL8.SelectedValue
        '    insert.Name = DDL8.SelectedItem.Text
        '    insert.RmbLineId = RmbLineNo
        '    d.Agape_Staff_RmbLineAddStaffs.InsertOnSubmit(insert)
        'End If
        'd.SubmitChanges()


    End Sub
    Public Sub LoadStaff(ByVal RmbLineNo As System.Int64, ByVal Settings As System.Collections.Hashtable, ByVal CanAddPass As Boolean)

        'Dim d As New AgapeStaff.AgapeStaffDataContext
        'Dim staff = From c In d.Agape_Staff_RmbLineAddStaffs Where c.RmbLineId = RmbLineNo Select c.UserId

        'Dim i As Integer = 0
        'For Each row In staff
        '    If i = 0 Then DDL1.SelectedValue = row.Value
        '    If i = 1 Then DDL2.SelectedValue = row.Value
        '    If i = 2 Then DDL3.SelectedValue = row.Value
        '    If i = 3 Then DDL4.SelectedValue = row.Value
        '    If i = 4 Then DDL5.SelectedValue = row.Value
        '    If i = 5 Then DDL6.SelectedValue = row.Value
        '    If i = 6 Then DDL7.SelectedValue = row.Value
        '    If i = 7 Then DDL8.SelectedValue = row.Value
        '    i += 1
        'Next
        'SetMileageRates(Settings, CanAddPass)


    End Sub
    Public Property ErrorText() As String
        Get
            Return ""
        End Get
        Set(ByVal value As String)
            ErrorLbl.Text = value
        End Set
    End Property
    Public Sub Initialize(ByVal Settings As System.Collections.Hashtable)
        'If Settings("Motorcycle") > 0 Or Settings("Bicycle") > 0 Then
        '    lblTitle.Text = DotNetNuke.Services.Localization.Localization.GetString("lblVehicle.Text", LocalResourceFile)
        '    lblTitle.HelpText = DotNetNuke.Services.Localization.Localization.GetString("lblVehicle.Help", LocalResourceFile).Replace("[MRate1]", Settings("MRate1")).Replace("[Motorcycle]", Settings("Motorcycle")).Replace("[Bicycle]", Settings("Bicycle"))


        '    ddlVehicleType.Items(0).Value = Settings("MRate1")
        '    ddlVehicleType.Items(1).Value = Settings("Motercycle")
        '    ddlVehicleType.Items(2).Value = Settings("Bicycle")


        '    pnlVehicle.Visible = True
        'Else
        '    ddlVehicleType.Items(0).Value = Settings("MRate1")
        '    ddlVehicleType.SelectedIndex = 0
        '    pnlVehicle.Visible = False
        'End If
        ddlVehicleType.Items.Clear()
        Dim PS = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)
        For I As Integer = 1 To 4
            Dim value As String = Settings("MRate" & I)
            If value <> "" Then
                ddlVehicleType.Items.Add(New ListItem(Settings("MRate" & I & "Name") & " (" & StaffBrokerFunctions.GetSetting("Currency", PS.PortalId) & CDbl(value).ToString("0.00") & ")", CDbl(value)))
            End If
        Next I
        If ddlVehicleType.Items.Count < 2 Then
            pnlVehicle.Visible = False
        End If


        ' VatRow.Visible = Settings("VatAttrib")
        lblDistance.Text = DotNetNuke.Services.Localization.Localization.GetString("lblAmount.Text", LocalResourceFile).Replace("[UNIT]", Settings("DistanceUnit"))
        lblDistance.HelpText = DotNetNuke.Services.Localization.Localization.GetString("lblAmount.Help", LocalResourceFile)

        'hfAddStaffRate.Value = Settings("AddPass")
        hfCanAddPass.Value = False
        Try

       
        If (Not String.IsNullOrEmpty(Settings("DescriptionLength"))) And CInt(Settings("DescriptionLength")) > 0 Then
            tbDesc.Attributes("maxLength") = CInt(Settings("DescriptionLength"))
            If CInt(Settings("DescriptionLength")) < 50 Then
                tbDesc.Columns = CInt(Settings("DescriptionLength")) + 3
                tbDesc.Width = Nothing
            End If
        End If
        Catch ex As Exception

        End Try
        Session("RmbSettings") = Settings
    End Sub
    Protected Sub UpdatePanel1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles UpdatePanel1.PreRender
        If Not Session("RmbSettings") Is Nothing Then
            Initialize(Session("RmbSettings"))
        End If

        If (ddlStaff.SelectedValue = 0) Then
            pnlStaff.Visible = False

        Else
            pnlStaff.Visible = True

            'Dim d As New AgapeStaff.AgapeStaffDataContext
            'Dim staff = From c In d.Agape_Staff_Finances Join b In d.Users On c.UserId1 Equals b.UserID Where c.StaffType = "UK Staff" Or c.StaffType = "Foreign Staff in UK" Or c.StaffType = "UK Staff Overseas" Or c.StaffType = "Centrally Funded" Select DisplayName = b.LastName & ", " & b.FirstName, b.UserID
            'staff = staff.Union(From c In d.Agape_Staff_Finances Join b In d.Users On c.USerId2 Equals b.UserID Where (c.StaffType = "UK Staff" Or c.StaffType = "Foreign Staff in UK" Or c.StaffType = "UK Staff Overseas" Or c.StaffType = "Centrally Funded") And c.USerId2 > 0 Select DisplayName = b.LastName & ", " & b.FirstName, b.UserID)

            'Dim StaffCount = ddlStaff.SelectedValue
            'If StaffCount > 0 Then
            '    DDL1.DataSource = From c In staff Order By c.DisplayName
            '    DDL1.DataTextField = "DisplayName"
            '    DDL1.DataValueField = "UserID"
            '    DDL1.DataBind()
            '    pnlDDL1.Visible = True
            'Else
            '    pnlDDL1.Visible = False
            'End If
            'If StaffCount > 1 Then
            '    DDL2.DataSource = From c In staff Order By c.DisplayName
            '    DDL2.DataTextField = "DisplayName"
            '    DDL2.DataValueField = "UserID"
            '    DDL2.DataBind()
            '    pnlDDL2.Visible = True
            'Else
            '    pnlDDL2.Visible = False
            'End If
            'If StaffCount > 2 Then
            '    DDL3.DataSource = From c In staff Order By c.DisplayName
            '    DDL3.DataTextField = "DisplayName"
            '    DDL3.DataValueField = "UserID"
            '    DDL3.DataBind()
            '    pnlDDL3.Visible = True
            'Else
            '    pnlDDL3.Visible = False
            'End If
            'If StaffCount > 3 Then
            '    DDL4.DataSource = From c In staff Order By c.DisplayName
            '    DDL4.DataTextField = "DisplayName"
            '    DDL4.DataValueField = "UserID"
            '    DDL4.DataBind()
            '    pnlDDL4.Visible = True
            'Else
            '    pnlDDL4.Visible = False
            'End If
            'If StaffCount > 4 Then
            '    DDL5.DataSource = From c In staff Order By c.DisplayName
            '    DDL5.DataTextField = "DisplayName"
            '    DDL5.DataValueField = "UserID"
            '    DDL5.DataBind()
            '    pnlDDL5.Visible = True
            'Else
            '    pnlDDL5.Visible = False
            'End If
            'If StaffCount > 5 Then
            '    DDL6.DataSource = From c In staff Order By c.DisplayName
            '    DDL6.DataTextField = "DisplayName"
            '    DDL6.DataValueField = "UserID"
            '    DDL6.DataBind()
            '    pnlDDL6.Visible = True
            'Else
            '    pnlDDL6.Visible = False
            'End If
            'If StaffCount > 6 Then
            '    DDL7.DataSource = From c In staff Order By c.DisplayName
            '    DDL7.DataTextField = "DisplayName"
            '    DDL7.DataValueField = "UserID"
            '    DDL7.DataBind()
            '    pnlDDL7.Visible = True
            'Else
            '    pnlDDL7.Visible = False
            'End If
            'If StaffCount > 7 Then
            '    DDL8.DataSource = From c In staff Order By c.DisplayName
            '    DDL8.DataTextField = "DisplayName"
            '    DDL8.DataValueField = "UserID"
            '    DDL8.DataBind()
            '    pnlDDL8.Visible = True
            'Else
            '    pnlDDL8.Visible = False
            'End If

        End If

        'If ddlVATReceipt.SelectedIndex = 0 And ddlVehicleType.SelectedIndex = 0 Then
        '    ddlCarType.Visible = True
        'Else
        '    ddlCarType.Visible = False
        'End If

        If ddlVehicleType.SelectedIndex = 0 And hfCanAddPass.Value Then
            PassengersRow.Visible = True

        Else
            PassengersRow.Visible = False
            pnlStaff.Visible = False
        End If


        ' VatRow.Visible = (ddlVehicleType.SelectedIndex <> 2)


    End Sub


   
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        hfCanAddPass.Value = False
    End Sub
End Class


