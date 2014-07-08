
Partial Class DesktopModules_AgapeConnect_mpdCalc_controls_BudgetTile
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


    Private _status As Integer
    Public Property Status() As Integer
        Get
            Return _status
        End Get
        Set(ByVal value As Integer)
            _status = value
            Select Case Status
                Case StaffRmb.RmbStatus.Processed
                    pnlTile.Attributes("class") &= " alert-success"

                    lblStatus.Text = Translate("Active")
                    lblNote.Text = Translate("ActiveNote")
                Case StaffRmb.RmbStatus.Submitted
                    pnlTile.Attributes("class") &= " alert-info"
                    lblStatus.Text = Translate(StaffRmb.RmbStatus.StatusName(value))

                    lblNote.Text = Translate("SubmittedNote")
                Case StaffRmb.RmbStatus.Approved
                    pnlTile.Attributes("class") &= " alert-success"
                    'lblStatus.Text = Translate("Active")
                    lblStatus.Text = Translate(StaffRmb.RmbStatus.StatusName(value))

                Case StaffRmb.RmbStatus.Cancelled
                    pnlTile.Attributes("class") &= " alert-danger"
                    lblStatus.Text = Translate("Cancelled")
                Case StaffRmb.RmbStatus.Draft
                    pnlTile.Attributes("class") &= " alert-warning"
                    lblStatus.Text = Translate("Draft")
                    lblNote.Text = Translate("DraftNote")
                Case Else
                    pnlTile.Attributes("class") &= " alert-warning"
                    lblStatus.Text = StaffRmb.RmbStatus.StatusName(value)
            End Select
        End Set
    End Property
    Private _navigateURL As String
    Public Property NavigateURL() As String
        Get
            Return _navigateURL
        End Get
        Set(ByVal value As String)
            _navigateURL = value
            hlTile.NavigateUrl = value
        End Set
    End Property
    Private _expired As String
    Public Property Expired() As String
        Get
            Return _expired
        End Get
        Set(ByVal value As String)
            _expired = value
            If (Status = StaffRmb.RmbStatus.Processed Or Status = StaffRmb.RmbStatus.Approved) And Not value = "current" Then
                If value <> "" Then
                    pnlTile.Attributes("class") = pnlTile.Attributes("class").Replace("success", "expired")
                    lblStatus.Text = Translate("Expired")
                    lblNote.Text = Translate("ExpiredNote").Replace("[DATE]", value)
                Else
                    pnlTile.Attributes("class") = pnlTile.Attributes("class").Replace("success", "info")
                    lblStatus.Text = Translate("Approved")
                    lblNote.Text = Translate("ApprovedNode").Replace("[DATE]", lblStart.Text)

                End If




            End If
        End Set
    End Property

    Private _mpdGoal As Double
    Public Property MpdGoal() As Double
        Get
            Return _mpdGoal
        End Get
        Set(ByVal value As Double)
            _mpdGoal = value
            Dim PS = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)
            lblGoal.Text = StaffBrokerFunctions.GetFormattedCurrency(PS.PortalId, value)
        End Set
    End Property


    Private _from As String
    Public Property From() As String
        Get
            Return _from
        End Get
        Set(ByVal value As String)
            _from = value
            If value.Length = 6 Then
                Dim d = New Date(CInt(Left(value, 4)), CInt(Right(value, 2)), 1)

                lblStart.Text = d.ToString("MMM yyyy")
            End If
            
        End Set
    End Property


    Private _staffId As Integer
    Public Property StaffId() As String
        Get
            Return _staffId
        End Get
        Set(ByVal value As String)
            _staffId = value
            Dim staff = StaffBrokerFunctions.GetStaffbyStaffId(value)
            '  lblStaffName.Text = staff.DisplayName

        End Set
    End Property
    Public Function Translate(ByVal ResourceString As String) As String
        Dim rtn As String
        Try
            rtn = DotNetNuke.Services.Localization.Localization.GetString(ResourceString & ".Text", LocalResourceFile)
            If String.IsNullOrEmpty(rtn) Then
                rtn = DotNetNuke.Services.Localization.Localization.GetString(ResourceString & ".Text", "/DesktopModules/AgapeConnect/mpdCalc/controls/App_LocalResources/BudgetTile.ascx.resx")
            End If
        Catch ex As Exception

            rtn = DotNetNuke.Services.Localization.Localization.GetString(ResourceString & ".Text", "/DesktopModules/AgapeConnect/mpdCalc/controls/App_LocalResources/BudgetTile.ascx.resx")

        End Try

        Return rtn

    End Function

End Class
