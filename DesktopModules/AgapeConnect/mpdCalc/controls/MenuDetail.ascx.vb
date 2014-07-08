Imports MPD
Partial Class DesktopModules_AgapeConnect_mpdCalc_controls_MenuDetail
    Inherits Entities.Modules.PortalModuleBase
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


    Private _displayName As String
    Public Property DisplayName() As String
        Get
            Return _displayName
        End Get
        Set(ByVal value As String)
            _displayName = value
            lblDisplayName.Text = value
        End Set
    End Property

    Private _portalId As Integer
    Public Property PortalId() As Integer
        Get
            Return _portalId
        End Get
        Set(ByVal value As Integer)
            _portalId = value
            hfPortalId.Value = value
        End Set
    End Property
    Private _mpdDefId As Integer
    Public Property MpdDefId() As Integer
        Get
            Return _mpdDefId
        End Get
        Set(ByVal value As Integer)
            _mpdDefId = value
            hfMpdDefId.Value = value
        End Set
    End Property
 



    Private _staffId As Integer
    Public Property StaffId() As Integer
        Get
            Return _staffId
        End Get
        Set(ByVal value As Integer)
            _staffId = value
            Dim d As New MPDDataContext
            myBudgets = From c In d.AP_mpdCalc_StaffBudgets
                                  Where c.AP_mpdCalc_Definition.PortalId = PortalId And c.StaffId = StaffId And c.Status <> StaffRmb.RmbStatus.Cancelled
                                  Order By c.BudgetPeriodStart Descending




            rpMyBudgets.DataSource = myBudgets
            rpMyBudgets.DataBind()

            pnlNoBudget.Visible = myBudgets.Count = 0

            hfStaffId.Value = value

            Dim ds As New StaffBroker.StaffBrokerDataContext
            Dim gr_id = (From c In ds.UserProfiles Where c.User.AP_StaffBroker_Staffs.StaffId = StaffId And c.ProfilePropertyDefinition.PropertyName = "gr_person_id" Select c.PropertyValue)
            If gr_id.Count > 0 Then
                hfmpduId.Value = (From c In d.Ap_mpd_Users Where c.gr_person_id = gr_id.First And PortalId = PortalId Select c.AP_mpd_UserId).FirstOrDefault
            End If

            '    btnViewReport.Visible = hfmpduId.Value > 0


            Dim acInfo = From c In d.AP_mpd_UserAccountInfos Where c.staffId = value Order By c.period Descending

            If acInfo.Count > 0 Then
                lblAccountBalance.Text = StaffBrokerFunctions.GetFormattedCurrency(PortalId, CInt(acInfo.First.balance).ToString("N0"))

            End If



            Dim currentBudget = From c In myBudgets Where (c.Status = StaffRmb.RmbStatus.Processed Or c.Status = StaffRmb.RmbStatus.Approved) And c.BudgetPeriodStart < Today.ToString("yyyyMM")
            If currentBudget.Count > 0 Then
                hfCurrentBudgetId.Value = currentBudget.First.StaffBudgetId
                btnViewCurrentBudget.Visible = True
                lblMPDGoal.Text = StaffBrokerFunctions.GetFormattedCurrency(PortalId, CInt(currentBudget.First.ToRaise).ToString("N0"))
                Dim AveIncome = mpdFunctions.getAverageMonthlyIncomeOver12Periods(value)



                lblSupportLevel.Text = (AveIncome / currentBudget.First.ToRaise).ToString("0%")
                lblSupportLevel.Attributes("data-value") = "sl" & value
           
            End If




        End Set
    End Property

    Private _showCreate As Boolean
    Public Property ShowCreate() As Boolean
        Get
            Return _showCreate
        End Get
        Set(ByVal value As Boolean)
            _showCreate = value
            btnCreateNewBudget.Visible = value
        End Set
    End Property



    Private _editURL As String
    Public Property EditURL() As String
        Get
            Return _editURL
        End Get
        Set(ByVal value As String)
            _editURL = value
            hfEditUrl.Value = value
        End Set
    End Property

    Dim myBudgets As IQueryable(Of AP_mpdCalc_StaffBudget)

   
    Public Function getExpired(ByVal Status As Integer, ByVal staffBudgetId As Integer) As String
        If (Status <> StaffRmb.RmbStatus.Processed And Status <> StaffRmb.RmbStatus.Approved) Then
            Return ""
        Else
            Dim mycompleted = From c In myBudgets Where (c.Status = StaffRmb.RmbStatus.Processed Or c.Status = StaffRmb.RmbStatus.Approved) And c.BudgetPeriodStart < Today.ToString("yyyyMM") Order By c.BudgetYearStart Descending

           

            Dim getNext As Boolean = False

            For Each row In mycompleted
                If getNext = True Then
                    Dim dt = New Date(CInt(Left(row.BudgetPeriodStart, 4)), CInt(Right(row.BudgetPeriodStart, 2)), 1).AddMonths(-1)

                    Return dt.ToString("MMM yyyy")
                End If
                If row.StaffBudgetId = staffBudgetId Then
                    getNext = True

                End If

            Next


            Return "current"

        End If
    End Function


    Protected Sub btnCreateNewBudget_Click(sender As Object, e As EventArgs) Handles btnCreateNewBudget.Click
        Dim d As New MPDDataContext

        Dim def = (From c In d.AP_mpdCalc_Definitions Where c.mpdDefId = hfMpdDefId.Value).First

        Dim insert As New MPD.AP_mpdCalc_StaffBudget
        insert.StaffId = hfStaffId.Value
        insert.DefinitionId = hfMpdDefId.Value
        insert.BudgetYearStart = Today.Year 'ddlNewYear.SelectedValue
        insert.Status = StaffRmb.RmbStatus.Draft
        Dim FirstBudgetMonth As Integer? = def.FirstBudgetPeriod
        If FirstBudgetMonth Is Nothing Then
            FirstBudgetMonth = 7
        End If
        Dim fpStartDate As Date
        If FirstBudgetMonth <= Today.Month Then
            fpStartDate = New Date(Today.Year, FirstBudgetMonth, 1)
        Else
            fpStartDate = New Date(Today.Year - 1, FirstBudgetMonth, 1)
        End If


        insert.BudgetPeriodStart = fpStartDate.ToString("yyyyMM")




        d.AP_mpdCalc_StaffBudgets.InsertOnSubmit(insert)
        d.SubmitChanges()
        Response.Redirect(hfEditUrl.Value & "?sb=" & insert.StaffBudgetId)


    End Sub



    Protected Sub btnViewReport_Click(sender As Object, e As EventArgs) Handles btnViewReport.Click
        Dim mc As New DotNetNuke.Entities.Modules.ModuleController

        Dim x = mc.GetModuleByDefinition(hfPortalId.Value, "ac_mpdDashboard")
        Response.Redirect(NavigateURL(x.TabID) & "?mpd_user_id=" & hfmpduId.Value)

    End Sub

    Protected Sub btnViewCurrentBudget_Click(sender As Object, e As EventArgs) Handles btnViewCurrentBudget.Click
        Response.Redirect(hfEditUrl.Value & "?sb=" & hfCurrentBudgetId.Value)
    End Sub

    Protected Sub btnFirstBudget_Click(sender As Object, e As EventArgs) Handles btnFirstBudget.Click
        btnCreateNewBudget_Click(sender, Nothing)
    End Sub
End Class
