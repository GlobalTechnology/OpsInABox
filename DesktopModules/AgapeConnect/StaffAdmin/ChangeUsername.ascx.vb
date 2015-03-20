Imports System
Imports System.Collections
Imports System.Configuration
Imports System.Data
Imports System.Linq

Imports DotNetNuke
Imports DotNetNuke.Security
Imports StaffBroker
Imports System.IO
Imports System.Data.OleDb




Namespace DotNetNuke.Modules.StaffAdmin
    Partial Class ChangeUsername
        Inherits Entities.Modules.PortalModuleBase



        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                ddlOrigUser.DataSource = UserController.GetUsers(PortalId)
                ddlOrigUser.DataTextField = "Username"
                ddlOrigUser.DataValueField = "Username"
                ddlOrigUser.DataBind()
            End If

        End Sub


        Protected Sub btnReturn_Click(sender As Object, e As EventArgs) Handles btnReturn.Click
            Response.Redirect(NavigateURL)
        End Sub





        Protected Sub btnChange_Click(sender As Object, e As EventArgs) Handles btnChange.Click
            Try


                tbNewUsername.Text = tbNewUsername.Text.Trim()
                lblLog.Text &= "Change Username Starting...:<br />"
                If ddlOrigUser.SelectedValue.EndsWith(PortalId) And Not tbNewUsername.Text.EndsWith(PortalId) Then
                    lblLog.Text &= "Converting GCX Name: " & tbNewUsername.Text & " to: "


                    tbNewUsername.Text &= PortalId
                    lblLog.Text &= tbNewUsername.Text & "<br />"
                End If

                'Check if the new user exists
                Dim thenewUser = UserController.GetUserByName(PortalId, tbNewUsername.Text)
                If Not thenewUser Is Nothing Then
                    lblLog.Text &= "New username already exists (possibly because they have already attempted logged in). <br />"
                    Dim ouName = "old_"
                    Dim theOldUser = UserController.GetUserByName(PortalId, ouName & tbNewUsername.Text)
                    Dim i As Integer = 0
                    While Not theOldUser Is Nothing
                        i += 1
                        ouName = "old" & i & "_"
                        theOldUser = UserController.GetUserByName(PortalId, ouName & tbNewUsername.Text)

                    End While

                    lblLog.Text &= "Changing " & tbNewUsername.Text & " to: " & ouName & tbNewUsername.Text & "<br />"
                    StaffBrokerFunctions.ChangeUsername(tbNewUsername.Text, ouName & tbNewUsername.Text)
                    Try


                        Dim theOldUserAfter = UserController.GetUserByName(PortalId, ouName & tbNewUsername.Text)
                        If Not theOldUserAfter Is Nothing Then
                            ' UserController.RemoveUser(theOldUserAfter)
                            Dim rc As New DotNetNuke.Security.Roles.RoleController
                            Dim staffRole = rc.GetRoleByName(PortalId, "Staff")
                            DotNetNuke.Security.Roles.RoleController.DeleteUserRole(theOldUserAfter, staffRole, PortalSettings, False)
                            'UserController.DeleteUser(theOldUserAfter, False, False)
                        End If
                    Catch ex As Exception

                    End Try
                End If

                lblLog.Text &= "Changing " & ddlOrigUser.SelectedValue & " to: " & tbNewUsername.Text & "<br />"

                StaffBrokerFunctions.ChangeUsername(ddlOrigUser.SelectedValue, tbNewUsername.Text)
                lblLog.Text &= "Change Username Completed <br />"
            Catch ex As Exception
                lblLog.Text &= "ERROR: Change Username FAILED - " & ex.Message & " <br />"
            End Try
        End Sub
    End Class
End Namespace
