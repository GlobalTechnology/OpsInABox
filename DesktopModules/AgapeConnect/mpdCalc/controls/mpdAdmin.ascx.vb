Imports MPD
Partial Class DesktopModules_AgapeConnect_mpdCalc_controls_mpdAdmin
    Inherits Entities.Modules.PortalModuleBase
    
        Private _mpdDefId As Integer
    Public Property MpdDefId() As Integer
        Get
            Return _mpdDefId
        End Get
        Set(ByVal v As Integer)

            _mpdDefId = v
            hfMpdDefId.Value = v
            If v > 0 Then

                Dim PS = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)
                Dim d As New MPDDataContext
                Dim thisForm = From c In d.AP_mpdCalc_Definitions Where c.PortalId = PS.PortalId And c.mpdDefId = v

                If thisForm.Count > 0 Then


                    Dim ds As New StaffBroker.StaffBrokerDataContext
                    Dim staffTypes = From c In ds.AP_StaffBroker_StaffTypes Where c.PortalId = PS.PortalId Select c.Name, Value = c.StaffTypeId

                    cblStaffTypes.Items.Clear()
                    cblStaffTypes.DataSource = staffTypes
                    cblStaffTypes.DataTextField = "Name"
                    cblStaffTypes.DataValueField = "Value"

                    cblStaffTypes.DataBind()
                    ddlAccount.DataSource = From c In ds.AP_StaffBroker_AccountCodes Where c.PortalId = PS.PortalId Order By c.AccountCode Select c.AccountCode, Name = c.AccountCode & "-" & c.AccountCodeName

                    ddlAccount.DataTextField = "Name"
                    ddlAccount.DataValueField = "AccountCode"
                    ddlAccount.DataBind()
                    pnlAccountCode.Visible = Not StaffBrokerFunctions.GetSetting("NonDynamics", PS.PortalId) = "True"

                    Dim staff = From c In ds.Users Where c.AP_StaffBroker_Staffs.PortalId = PS.PortalId Select c.UserID, Name = c.LastName & ", " & c.FirstName Order By Name

                    ddlAuthUser.DataSource = staff
                    ddlAuthUser.DataTextField = "Name"
                    ddlAuthUser.DataValueField = "UserID"
                    ddlAuthUser.DataBind()
                    If Not thisForm.First.AuthUser Is Nothing Then
                        ddlAuthUser.SelectedValue = thisForm.First.AuthUser
                    End If



                    ddlAuthAuthUser.DataSource = staff
                    ddlAuthAuthUser.DataTextField = "Name"
                    ddlAuthAuthUser.DataValueField = "UserID"
                    ddlAuthAuthUser.DataBind()
                    If Not thisForm.First.AuthAuthUser Is Nothing Then
                        ddlAuthAuthUser.SelectedValue = thisForm.First.AuthAuthUser
                    End If

                    If Not String.IsNullOrEmpty(thisForm.First.StaffTypes) Then


                        Dim selectedTypes = thisForm.First.StaffTypes.Split(";")

                        For Each row As ListItem In cblStaffTypes.Items
                            If selectedTypes.Contains(row.Value) Then
                                row.Selected = True

                            End If
                        Next
                    End If

                    tbComplience.Text = thisForm.First.Complience
                    If Not String.IsNullOrEmpty(thisForm.First.Compensation) Then
                        If thisForm.First.Compensation.StartsWith("%") Then
                            tbCompensation.Text = thisForm.First.Compensation.Trim("%")
                            ddlCompensationType.SelectedValue = "Percentage"
                        Else
                            tbCompensation.Text = thisForm.First.Compensation
                            ddlCompensationType.SelectedValue = "Formula"
                        End If
                    End If
                    If Not String.IsNullOrEmpty(thisForm.First.Assessment) Then
                        If thisForm.First.Assessment.StartsWith("%") Then
                            tbAssessment.Text = thisForm.First.Assessment.Trim("%")
                            ddlAssessmentType.SelectedValue = "Percentage"

                        Else
                            tbAssessment.Text = thisForm.First.Assessment
                            ddlAssessmentType.SelectedValue = "Formula"
                        End If
                    End If
                    tbDataserverURL.Text = StaffBrokerFunctions.GetSetting("DataserverURL", PS.PortalId)
                    'Dim ds As New StaffBroker.StaffBrokerDataContext

                    If Not ddlAccount.Items.FindByValue(thisForm.First.DefaultAccount) Is Nothing Then
                        ddlAccount.SelectedValue = thisForm.First.DefaultAccount
                    End If


                End If

            End If
        End Set
    End Property

    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
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


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load


    End Sub

    Protected Sub btnUpdateConfig_Click(sender As Object, e As EventArgs) Handles btnUpdateConfig.Click

        Dim PS = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)
        Dim d As New MPDDataContext()
        Dim theForm = From c In d.AP_mpdCalc_Definitions Where c.PortalId = PS.PortalId And c.mpdDefId = hfMpdDefId.Value


        If theForm.Count > 0 Then
            Dim st = ""
            For Each row As ListItem In cblStaffTypes.Items
                If row.Selected Then
                    st &= row.Value & ";"
                End If


            Next
            theForm.First.StaffTypes = st

            If ddlAssessmentType.SelectedValue = "Percentage" Then
                theForm.First.Assessment = "%" & tbAssessment.Text.Trim("%")
            Else
                theForm.First.Assessment = tbAssessment.Text
            End If
            If ddlCompensationType.SelectedValue = "Percentage" Then
                theForm.First.Compensation = "%" & tbCompensation.Text.Trim("%")
            Else
                theForm.First.Compensation = tbCompensation.Text
            End If

            theForm.First.Complience = tbComplience.Text
            theForm.First.ShowComplience = tbComplience.Text.Trim(" ").Length > 0
            If Not StaffBrokerFunctions.GetSetting("NonDynamics", PS.PortalId) = "True" Then
                theForm.First.DefaultAccount = ddlAccount.SelectedValue
            End If


            theForm.First.AuthUser = ddlAuthUser.SelectedValue
            theForm.First.AuthAuthUser = ddlAuthAuthUser.SelectedValue

            d.SubmitChanges()
            TrimDataserverString()
            StaffBrokerFunctions.SetSetting("DataserverURL", tbDataserverURL.Text, PS.PortalId)

            Response.Redirect(Request.Url.ToString())
        End If
    End Sub

    Private Sub TrimDataserverString()
        tbDataserverURL.Text = tbDataserverURL.Text.ToLower()

        If Not tbDataserverURL.Text.StartsWith("http") Then
            tbDataserverURL.Text = "https://" & tbDataserverURL.Text
        End If

        Dim r = tbDataserverURL.Text.IndexOf("dataserver/")
        If r > 0 Then
            r += 11
            Dim o = tbDataserverURL.Text.Substring(r).IndexOf("/")
            If o > 0 Then
                tbDataserverURL.Text = tbDataserverURL.Text.Substring(0, r + o + 1)

            End If
        End If

    End Sub

    Protected Sub btnTestDataserver_Click(sender As Object, e As EventArgs) Handles btnTestDataserver.Click
        TrimDataserverString()
        Dim resp = tntWebUsers.TestDataserverConnection(tbDataserverURL.Text)
        imgOK.Visible = False
        imgWarning.Visible = False
        pnlWarning.Visible = False

        If resp.connectionSuccess And resp.hasTrustedUser Then
            imgOK.Visible = True
        Else
            imgWarning.Visible = True
            pnlWarning.Visible = True
            If resp.connectionSuccess Then
                lblWarning.Text = "The URL appears to be correct. However the trusted user,  allowing this site to access your dataserver, has not been setup. You will need to setup ""trusteduser@agapeconnect.me"" in tntDataserver. For help, please contact ThadHoskins@agapeeurope.org. "
            Else
                lblWarning.Text = resp.ErrorMessage
            End If
        End If
    End Sub
End Class
