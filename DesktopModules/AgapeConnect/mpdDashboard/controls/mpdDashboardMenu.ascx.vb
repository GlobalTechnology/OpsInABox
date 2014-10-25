Imports MPD
Partial Class DesktopModules_AgapeConnect_mpdCalc_controls_mpdAdmin
    Inherits Entities.Modules.PortalModuleBase

    Enum mpdMenuMode
        Countries
        Team
        Staff
    End Enum

    Private _mode As mpdMenuMode
    Public Property Mode() As mpdMenuMode
        Get
            Return _mode
        End Get
        Set(ByVal value As mpdMenuMode)
            _mode = value
            Select Case value
                Case mpdMenuMode.Countries
                    menuCountries.Attributes.Add("class", "active")
                Case mpdMenuMode.Team
                    menuTeam.Attributes.Add("class", "active")
                Case mpdMenuMode.Staff
                    menuStaff.Attributes.Add("class", "active")

            End Select
        End Set
    End Property

    Private _title As String
    Public Property Title() As String
        Get
            Return _title
        End Get
        Set(ByVal value As String)
            _title = value
            hlTitle.Text = value
        End Set
    End Property

    Private _countrUrl As String
    Public Property CountryURL() As String
        Get
            Return _countrUrl
        End Get
        Set(ByVal value As String)
            _countrUrl = value
        End Set
    End Property

    Private _staffUrl As String
    Public Property StaffUrl() As String
        Get
            Return _staffUrl
        End Get
        Set(ByVal value As String)
            _staffUrl = value
        End Set
    End Property

    Private _ssoGuid As String
    Public Property ssoGuid() As String
        Get
            Return _ssoGuid
        End Get
        Set(ByVal value As String)
            _ssoGuid = value
            Dim d As New MPDDataContext
            Dim staff = From c In d.Ap_mpd_Users Where c.Key_GUID = value Select c.AP_mpd_Country.name, c.AP_mpd_UserId
            If staff.Count = 0 Then
                'menuStaff.Visible = False

            Else

                rpUserAccts.DataSource = staff
                rpUserAccts.DataBind()
            End If
            

            menuTeam.Visible = (From c In d.ap_mpd_user_reportings Where c.leader_sso_guid = value).Count > 0

            menuCountries.Visible = ((From c In d.AP_MPD_CountryAdmins Where c.sso_guid = value).Count > 0) Or ((From c In d.AP_mpd_AreaAdmins Where c.sso_guid = value).Count > 0)



        End Set
    End Property

  

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load


    End Sub

End Class
