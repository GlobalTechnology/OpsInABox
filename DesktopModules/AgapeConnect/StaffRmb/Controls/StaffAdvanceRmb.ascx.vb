Imports System
Imports System.Collections
Imports System.Configuration
Imports System.Data
Imports System.Linq
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.IO
Imports System.Net
Imports DotNetNuke
Imports DotNetNuke.Security

'Imports AgapeStaff
Imports StaffBroker
Imports StaffBrokerFunctions

Namespace DotNetNuke.Modules.StaffAdvanceRmb
    Partial Class StaffAdvanceRmb
        Inherits Entities.Modules.PortalModuleBase


        Public Property Amount() As Double
            Get
                Return Double.Parse(tbAmount.Text, New CultureInfo("en-US").NumberFormat)
            End Get
            Set(ByVal value As Double)
                tbAmount.Text = value.ToString("n2", New CultureInfo("en-US"))
            End Set
        End Property


        Public Property ReqMessage() As String
            Get
                Return tbRequestText.Text
            End Get
            Set(ByVal value As String)
                tbRequestText.Text = value
            End Set
        End Property






        Dim dBroke As New StaffBrokerDataContext


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


        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load

            If Not Page.IsPostBack Then
                Dim mc As New DotNetNuke.Entities.Modules.ModuleController

                Dim x = mc.GetModuleByDefinition(PortalId, "acStaffRmb")
                If Not x Is Nothing Then
                    Dim RmbSettings = x.TabModuleSettings

                    hfOverAuth.Value = CType(RmbSettings("TeamLeaderLimit"), String)


                    lblNotes2.Text = Translate("lblNotes2")
                    Dim FC = StaffBrokerFunctions.GetFormattedCurrency(PortalId, RmbSettings("TeamLeaderLimit"))
                    lblNotes2.Text = lblNotes2.Text.Replace("[TEAMLEADERLIMIT]", FC)

                    Dim AuthUser As UserInfo
                    If RmbSettings("AuthUser") <> "" Then

                        AuthUser = UserController.GetUserById(PortalId, RmbSettings("AuthUser"))
                    End If
                    Dim AuthAuthUser As UserInfo
                    If RmbSettings("AuthAuthUser") <> "" Then
                        AuthUser = UserController.GetUserById(PortalId, RmbSettings("AuthAuthUser"))
                    End If
                    If Not AuthUser Is Nothing Then
                        If AuthUser.UserID <> UserId Then
                            lblNotes2.Text = lblNotes2.Text.Replace("[AUTHUSER]", AuthUser.DisplayName)
                        End If
                    End If
                    If Not AuthAuthUser Is Nothing Then
                        If AuthAuthUser.UserID <> UserId Then
                            lblNotes2.Text = lblNotes2.Text.Replace("[AUTHUSER]", AuthAuthUser.DisplayName)
                        End If
                    End If
                End If



              

                '    pnlNotes.Height = 0
                '    hfCurrentRequest.Value = 0

                lblCur.Text = StaffBrokerFunctions.GetSetting("Currency", PortalId)

            End If

        End Sub

        
       
        
       
       
        
        Public Function Translate(ByVal ResourceString As String) As String
            Return DotNetNuke.Services.Localization.Localization.GetString(ResourceString & ".Text", LocalResourceFile)

        End Function

       

        
        
    End Class
End Namespace

