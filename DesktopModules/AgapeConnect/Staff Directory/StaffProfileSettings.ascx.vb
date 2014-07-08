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

                hfPortalId.Value = PortalId
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

                    If CType(TabModuleSettings("StaffProps"), String) <> "" Then
                        Dim StaffProps = CType(TabModuleSettings("StaffProps"), String).Split(";")
                        For Each row As ListItem In cblStaffProps.Items
                            row.Selected = (From c In StaffProps Where c.TrimStart(";") = row.Value).Count > 0
                        Next
                    End If

                    If CType(TabModuleSettings("StaffTypes"), String) <> "" Then
                        lbStaffTypes.DataBind()
                        Dim StaffTypes = CType(TabModuleSettings("StaffTypes"), String).Split(";")
                        For Each row As ListItem In lbStaffTypes.Items
                            row.Selected = StaffTypes.Contains(row.Value)
                        Next
                    End If
                    If CType(TabModuleSettings("ReportsTo"), String) <> "" Then
                        cbReportsTo.Checked = CType(TabModuleSettings("ReportsTo"), Boolean)

                    End If


                End If



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

            objModules.UpdateTabModuleSetting(TabModuleId, "ProfProps", ProfProps.TrimEnd(";"))



            Dim StaffProps As String = ""
            For Each row As ListItem In cblStaffProps.Items
                If row.Selected Then
                    StaffProps &= row.Value & ";"
                End If
            Next

            objModules.UpdateTabModuleSetting(TabModuleId, "StaffProps", StaffProps.TrimEnd(";"))



            Dim StaffTypes As String = ""
            For Each row As ListItem In lbStaffTypes.Items
                If row.Selected Then
                    StaffTypes &= row.Value & ";"
                End If
            Next

            objModules.UpdateTabModuleSetting(TabModuleId, "StaffTypes", StaffTypes.TrimEnd(";"))
            objModules.UpdateTabModuleSetting(TabModuleId, "ReportsTo", cbReportsTo.Checked)

            ' refresh cache
            SynchronizeModule()
            Response.Redirect(NavigateURL())
        End Sub

        
        Protected Sub CancelBtn_Click(sender As Object, e As System.EventArgs) Handles CancelBtn.Click
            Response.Redirect(NavigateURL())
        End Sub
    End Class

End Namespace

