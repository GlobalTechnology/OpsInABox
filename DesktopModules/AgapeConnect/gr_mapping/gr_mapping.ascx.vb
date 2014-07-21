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
Imports DotNetNuke
Imports DotNetNuke.Security

Imports GR_NET
Imports gr_mapping
Namespace DotNetNuke.Modules.AgapeConnect
    Partial Class gr_mapping_mod
        Inherits Entities.Modules.PortalModuleBase

      


        Dim gr As GR

        Private Sub ResetLocalDDL()
            Dim ds As New StaffBroker.StaffBrokerDataContext
            ddlProfileMap.Items.Clear()
            ddlProfileMap.Items.Add(New ListItem("User: Email", "U-Email"))
            ddlProfileMap.Items.Add(New ListItem("User: DisplayName", "U-DisplayName"))
            ddlProfileMap.Items.Add(New ListItem("User: FirstName", "U-FirstName"))
            ddlProfileMap.Items.Add(New ListItem("User: LastName", "U-LastName"))
            ddlProfileMap.Items.Add(New ListItem("User: UserId", "U-UserId"))

           

            Dim ups = From c In ds.ProfilePropertyDefinitions Where c.PortalID = PortalId Order By c.PropertyName Select c.PropertyName, c.PropertyDefinitionID

            For Each row In ups
                ddlProfileMap.Items.Add(New ListItem("User Profile: " & row.PropertyName, "UP-" & row.PropertyDefinitionID))

            Next

            ddlProfileMap.Items.Add(New ListItem("Staff: R/C", "S-CostCenter"))
            ddlProfileMap.Items.Add(New ListItem("Staff: StaffId", "S-StaffId"))
            ddlProfileMap.Items.Add(New ListItem("Staff: DisplayName", "S-DisplayName"))

            Dim sps = From c In ds.AP_StaffBroker_StaffPropertyDefinitions Order By c.PropertyName Where c.PortalId = PortalId
            For Each row In sps
                ddlProfileMap.Items.Add(New ListItem("Staff Profile: " & row.PropertyName, "SP-" & row.StaffPropertyDefinitionId))

            Next

        End Sub


        Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load



            If Not Page.IsPostBack Then
                pnlAdmin.Visible = UserInfo.IsSuperUser


                tbApiKey.Text = StaffBrokerFunctions.GetSetting("gr_api_key", PortalId)


                Dim d As New gr_mappingDataContext
                Dim mappings = From c In d.gr_mappings Where c.PortalId = PortalId
                gvMappings.DataSource = mappings
                gvMappings.DataBind()

                
                ResetLocalDDL()
               


                If Not tbApiKey.Text = "" Then
                    Try
                        Dim gr_server = StaffBrokerFunctions.GetSetting("gr_api_url", PortalId)
                        If String.IsNullOrEmpty(gr_server) Then
                            gr_server = "https://api.global-registry.org/"
                            StaffBrokerFunctions.SetSetting("gr_api_url", gr_server, PortalId)

                        End If

                        gr = New GR(tbApiKey.Text, gr_server, True)


                        Dim ministries = gr.GetEntities("ministry", "&ruleset=global_ministries", 0, 300, 0)
                        ddlMinistries.DataSource = From c In ministries Select Name = c.GetPropertyValue("name"), Value = c.GetPropertyValue("id") Order By Name

                        ddlMinistries.DataValueField = "Value"
                        ddlMinistries.DataTextField = "Name"
                        ddlMinistries.DataBind()
                        ddlMinistries.SelectedValue = StaffBrokerFunctions.GetSetting("gr_ministry_id", PortalId)
                        Dim leaves = (From c In gr.GetFlatEntityLeafList("person") Select Name = c.GetDotNotation, c.ID).ToList()
                        Dim staffLeaves = (From c In gr.GetFlatEntityLeafList("ministry_membership") Select Name = c.GetDotNotation.Replace("ministry_membership.", "person:{in_this_min}."), c.ID).ToList()
                        leaves.AddRange(staffLeaves)

                        gr_entity_types.DataSource = leaves.OrderBy(Function(c) c.Name)
                        gr_entity_types.DataTextField = "Name"
                        gr_entity_types.DataValueField = "ID"
                        gr_entity_types.DataBind()



                       

                   

                        
                    Catch ex As Exception
                        pnlMain.Visible = False
                        lblTest.Text = ex.ToString
                    End Try
                End If

            End If
        End Sub
       


        Protected Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
            Dim d As New gr_mappingDataContext
            Dim q = From c In d.gr_mappings Where c.PortalId = PortalId And c.LocalName = ddlProfileMap.SelectedItem.Text _
              And c.LocalSource = "UP" And c.gr_dot_notated_name = gr_entity_types.SelectedItem.Text

            If q.Count = 0 Then
                Dim insert As New gr_mapping.gr_mapping()
                insert.PortalId = PortalId
                insert.LocalName = ddlProfileMap.SelectedItem.Text.Substring(ddlProfileMap.SelectedItem.Text.IndexOf(":") + 2)
                insert.LocalSource = ddlProfileMap.SelectedValue.Substring(0, ddlProfileMap.SelectedValue.IndexOf("-"))
                insert.gr_dot_notated_name = gr_entity_types.SelectedItem.Text
                insert.FieldType = ddlDataType.SelectedValue
                insert.replace = tbReplaceText.Text
                insert.can_be_updated = can_be_updated.Checked
                d.gr_mappings.InsertOnSubmit(insert)
                d.SubmitChanges()

                Dim mappings = From c In d.gr_mappings Where c.PortalId = PortalId

                gvMappings.DataSource = mappings
                gvMappings.DataBind()

            End If



        End Sub

        Protected Sub gvMappings_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvMappings.RowCommand
            If e.CommandName = "myDelete" Then
                Dim d As New gr_mappingDataContext
                Dim q = From c In d.gr_mappings Where c.Id = CInt(e.CommandArgument)

                d.gr_mappings.DeleteAllOnSubmit(q)
                d.SubmitChanges()
                Dim mappings = From c In d.gr_mappings Where c.PortalId = PortalId

                gvMappings.DataSource = mappings
                gvMappings.DataBind()

            End If
        End Sub
        Public Function getLocalTypeName(ByVal LocalType As String) As String
            Select Case LocalType
                Case "UP"
                    Return "User Profile:"
                Case "U"
                    Return "User:"
                Case "SP"
                    Return "Staff Profile:"
                Case "S"
                    Return "Staff"

                Case Else
                    Return ""
            End Select
        End Function

       
        Protected Sub btnSaveMinistry_Click(sender As Object, e As EventArgs) Handles btnSaveMinistry.Click
            StaffBrokerFunctions.SetSetting("gr_ministry_id", ddlMinistries.SelectedValue, PortalId)
        End Sub

        Protected Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
            For Each staff In StaffBrokerFunctions.GetStaff()
                Dim the_user = UserController.GetUserById(PortalId, staff.UserID)
                the_user.Profile.SetProfileProperty("gr_person_id", "")
                the_user.Profile.SetProfileProperty("gr_ministry_membership_id", "")
                UserController.UpdateUser(PortalId, the_user)

            Next


        End Sub

        Protected Sub btnSaveKey_Click(sender As Object, e As EventArgs) Handles btnSaveKey.Click
            StaffBrokerFunctions.SetSetting("gr_api_key", tbApiKey.Text, PortalId)
        End Sub

        Protected Sub btnCreateKey_Click(sender As Object, e As EventArgs) Handles btnCreateKey.Click
            If tbRootKey.Text <> "" And tbName.Text <> "" Then
                Dim gr_server = StaffBrokerFunctions.GetSetting("gr_api_url", PortalId)
                If String.IsNullOrEmpty(gr_server) Then
                    gr_server = "https://api.global-registry.org/"
                    StaffBrokerFunctions.SetSetting("gr_api_url", gr_server, PortalId)

                End If
                gr = New GR(tbRootKey.Text, gr_server, False)
                gr.CreateSystem("oib_" & tbName.Text.ToLower)
                Dim systems = GR_NET.GR.GetSystems(tbRootKey.Text, gr_server)
                Dim thisSystem = From c In systems Where c.Name = "oib_" & tbName.Text.ToLower

                If thisSystem.Count > 0 Then
                    StaffBrokerFunctions.SetSetting("gr_api_key", thisSystem.First.AccessToken, PortalId)
                    Response.Redirect(NavigateURL())
                End If
            End If
           

        End Sub
    End Class
End Namespace
