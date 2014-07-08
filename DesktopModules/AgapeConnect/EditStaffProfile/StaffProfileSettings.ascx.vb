Imports DotNetNuke
Imports System.Web.UI
Imports System.Linq

Imports StaffRmb



Namespace DotNetNuke.Modules.StaffProfile

    Partial Class StaffProfileSettings
        Inherits Entities.Modules.ModuleSettingsBase

#Region "Base Method Implementations"
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
            Try


                If (Page.IsPostBack = False) Then
                   


                    'If CType(TabModuleSettings("NoReceipt"), String) <> "" Then
                    '    tbNoReceipt.Text = CType(TabModuleSettings("NoReceipt"), String)
                    'End If
                    Dim d As New StaffBroker.StaffBrokerDataContext
                    Dim props = From c In d.ProfilePropertyDefinitions Where c.PortalID = PortalId And c.Deleted = False And c.PropertyCategory <> "Name" Order By c.PropertyCategory, c.ViewOrder Select PropertyName = c.PropertyCategory & " - " & c.PropertyName, c.PropertyDefinitionID
                    cblProfileProps.DataSource = props
                    cblProfileProps.DataTextField = "PropertyName"
                    cblProfileProps.DataValueField = "PropertyDefinitionID"
                    cblProfileProps.DataBind()

                    If CType(TabModuleSettings("ProfProps"), String) <> "" Then
                        Dim ProfProps = CType(TabModuleSettings("ProfProps"), String).Split(";")
                        For Each row As ListItem In cblProfileProps.Items
                            row.Selected = (From c In ProfProps Where c.TrimStart(";") = row.Value).Count > 0
                        Next
                    End If


                    Dim sProps = From c In d.AP_StaffBroker_StaffPropertyDefinitions Where c.PortalId = PortalId Order By c.ViewOrder

                    cblStaffProps.DataSource = sProps
                    cblStaffProps.DataTextField = "PropertyName"
                    cblStaffProps.DataValueField = "StaffPropertyDefinitionId"

                    cblStaffProps.DataBind()



                    cblGivingProps.DataSource = sProps
                    cblGivingProps.DataValueField = "StaffPropertyDefinitionId"
                    cblGivingProps.DataTextField = "PropertyName"
                    cblGivingProps.DataBind()


                    If CType(TabModuleSettings("StaffProps"), String) <> "" Then
                        Dim StaffProps = CType(TabModuleSettings("StaffProps"), String).Split(";")
                        For Each row As ListItem In cblStaffProps.Items
                            row.Selected = (From c In StaffProps Where c.TrimStart(";") = row.Value).Count > 0
                        Next
                    End If

                    If CType(TabModuleSettings("GivingProps"), String) <> "" Then
                        Dim GivingProps = CType(TabModuleSettings("GivingProps"), String).Split(";")
                        For Each row As ListItem In cblGivingProps.Items
                            row.Selected = (From c In GivingProps Where c.TrimStart(";") = row.Value).Count > 0
                        Next
                    End If
                    If CType(TabModuleSettings("DisplayGivingTab"), String) <> "" Then
                        cbGiving.Checked = CType(TabModuleSettings("DisplayGivingTab"), Boolean)
                    End If

                End If

                rowGiving.Visible = cbGiving.Checked


            Catch exc As Exception           'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try



        End Sub


#End Region


        Protected Sub SaveBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SaveBtn.Click
            Dim objModules As New Entities.Modules.ModuleController
            Dim ProfProps As String = ""
            For Each row As ListItem In cblProfileProps.Items
                If row.Selected Then
                    ProfProps &= row.Value & ";"
                End If
            Next
            If ProfProps <> "" Then
                objModules.UpdateTabModuleSetting(TabModuleId, "ProfProps", ProfProps.Substring(0, ProfProps.Length - 1))
            Else
                objModules.UpdateTabModuleSetting(TabModuleId, "ProfProps", "")

            End If


            Dim StaffProps As String = ""
            For Each row As ListItem In cblStaffProps.Items
                If row.Selected Then
                    StaffProps &= row.Value & ";"
                End If
            Next
            If StaffProps <> "" Then
                objModules.UpdateTabModuleSetting(TabModuleId, "StaffProps", StaffProps.Substring(0, StaffProps.Length - 1))
            Else
                objModules.UpdateTabModuleSetting(TabModuleId, "StaffProps", "")
            End If

            Dim GivingProps As String = ""
            For Each row As ListItem In cblGivingProps.Items
                If row.Selected Then
                    GivingProps &= row.Value & ";"
                End If
            Next
            objModules.UpdateTabModuleSetting(TabModuleId, "DisplayGivingTab", cbGiving.Checked)
            If GivingProps <> "" Then
                objModules.UpdateTabModuleSetting(TabModuleId, "GivingProps", GivingProps.Substring(0, GivingProps.Length - 1))
            Else
                objModules.UpdateTabModuleSetting(TabModuleId, "GivingProps", "")
            End If

            ' refresh cache
            SynchronizeModule()
            Response.Redirect(NavigateURL())
        End Sub

        
    End Class

End Namespace

