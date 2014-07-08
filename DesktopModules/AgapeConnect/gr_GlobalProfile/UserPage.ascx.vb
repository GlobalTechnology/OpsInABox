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

        Public gr_server As String '= "http://192.168.1.40:3000/"

        Public GraphScript As String = ""
       
        Dim gr As GR
        Public jsonGMA As String = ""
       
        Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
            gr_server = StaffBrokerFunctions.GetSetting("gr_api_url", PortalId)
            If Not Page.IsPostBack Then

                LoadForm()
                LoadGraphs()
            End If
        End Sub

        Private Sub LoadGraphs()


            Dim m = gr.GetMeasurements(Request.QueryString("id"), Today.AddMonths("-13").ToString("yyyy-MM"), Today.ToString("yyyy-MM"))
            phGraphs.Controls.Clear()
            For Each row In m
                'add graph
                phGraphs.Controls.Add(New LiteralControl("<div id='measurement" & row.ID & "'></div>"))

                GraphScript &= "var data" & row.ID & " = google.visualization.arrayToDataTable(["
                GraphScript &= "['Period', '" & row.Name & "'],"

                For Each item In row.measurements
                    GraphScript &= "['" & item.Period & "', " & item.Value & "],"
                Next
                GraphScript &= "]);" & vbNewLine
                GraphScript &= "var chart" & row.ID & " = new google.visualization.LineChart(document.getElementById('measurement" & row.ID & "'));" & vbNewLine
                GraphScript &= " var options" & row.ID & " = {""title"": '" & row.Name & "', series: [{ color: '#ff9900', lineWidth: 2 }]}" & vbNewLine
                GraphScript &= " chart" & row.ID & ".draw(data" & row.ID & ", options" & row.ID & ");" & vbNewLine

            Next
           

        End Sub

        Private Sub LoadForm()
            gr = New GR(StaffBrokerFunctions.GetSetting("gr_api_key", PortalId), gr_server)

            Dim thisUser = gr.GetEntity(Request.QueryString("id"), True)
            Session("gr_user") = thisUser
            If Not Page.IsPostBack Then
                PopulateDropdowns()
            End If
            Dim scopeFilter As String = ""
            If Not thisUser Is Nothing Then

                tbFirstName.Text = thisUser.GetPropertyValue("first_name.value")
                tbLastName.Text = thisUser.GetPropertyValue("last_name.value")
                tbPreferredName.Text = thisUser.GetPropertyValue("preferred_name.value")

                lblTitle.Text = tbLastName.Text & ", " & tbFirstName.Text
                tbEmail.Text = thisUser.GetPropertyValue("email_address.email.value")
                tbAddress1.Text = thisUser.GetPropertyValue("address.line1.value")
                tbAddress2.Text = thisUser.GetPropertyValue("address.line2.value")
                tbCity.Text = thisUser.GetPropertyValue("address.city.value")
                tbState.Text = thisUser.GetPropertyValue("address.state.value")
                tbPostalCode.Text = thisUser.GetPropertyValue("address.postal_code.value")
                tbCountry.Text = thisUser.GetPropertyValue("address.country.value")
                ddlGender.SelectedValue = thisUser.GetPropertyValue("gender.value")
                tbBirthday.Text = thisUser.GetPropertyValue("birth_date.value")

                ddlMaritalStatus.SelectedValue = thisUser.GetPropertyValue("marital_status.value")

                hlSpouse.Visible = False
                Dim spouseid = thisUser.GetPropertyValue("wife:relationship.person") & thisUser.GetPropertyValue("husband:relationship.person")
                If spouseId <> "" Then
                    Dim spouse = gr.GetEntity(spouseId, True)
                    hlSpouse.Text = spouse.GetPropertyValue("first_name.value")
                    hlSpouse.NavigateUrl = EditUrl("UserPage") & "?id=" & spouseId
                    hlSpouse.Visible = True
                End If


                For Each item As ListItem In ddlRoleType.Items
                    item.Selected = (item.Text = thisUser.GetPropertyValue("ministry:relationship.role"))
                    If (item.Selected) Then
                        Exit For
                    End If

                Next


                Dim min_id = thisUser.GetPropertyValue("ministry:relationship.ministry")
                'lblText.Text &= min_id & ":"
                If min_id <> "" Then


                    Dim get_min = gr.GetEntity(min_id, True)

                    For Each item As ListItem In ddlMinistryLevel.Items

                        item.Selected = (item.Text = get_min.GetPropertyValue("ministry_scope.value"))
                        If item.Selected Then
                            Exit For
                        End If

                    Next


                    If (ddlMinistryLevel.SelectedValue <> "") Then
                        ddlMinistry.Items.Clear()
                        ddlMinistry.Items.Add("")
                        scopeFilter = "&created_by=all&filters[ministry_scope]=" & ddlMinistryLevel.SelectedItem.Text
                        Dim ministries = From c In gr.GetEntities("ministry", scopeFilter, 1, 1000, 1) Select name = c.GetPropertyValue("name.value"), c.ID Order By name
                        ddlMinistry.DataSource = ministries
                        ddlMinistry.DataTextField = "name"
                        ddlMinistry.DataValueField = "id"
                        ddlMinistry.DataBind()



                        ddlMinistry.SelectedValue = min_id



                    End If
                End If
            End If

            ' lblText.Text = thisUser.ToJson()
        End Sub

        Protected Sub PopulateDropdowns()
            ddlRoleType.Items.Clear()
            ddlMinistryLevel.Items.Clear()
            ddlRoleType.Items.Add("")
            ddlMinistryLevel.Items.Add("")
            Dim role = gr.GetEntities("role", "&created_by=all")

            ddlRoleType.DataSource = From c In role Select name = c.GetPropertyValue("value"), id = c.GetPropertyValue("id")

            ddlRoleType.DataTextField = "name"
            ddlRoleType.DataValueField = "ID"
            ddlRoleType.DataBind()


            Dim scope = gr.GetEntities("ministry_scope", "&created_by=all")

            ddlMinistryLevel.DataSource = From c In scope Select name = c.GetPropertyValue("value"), c.ID

            ddlMinistryLevel.DataTextField = "name"
            ddlMinistryLevel.DataValueField = "ID"
            ddlMinistryLevel.DataBind()



        End Sub

        Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
            Dim thisUser As Entity = Session("gr_user")
            Dim GR = New GR(StaffBrokerFunctions.GetSetting("gr_api_key", PortalId), gr_server)


            If Not thisUser Is Nothing Then
                Dim person As New Entity()
                person.ID = thisUser.ID
                If tbFirstName.Text <> thisUser.GetPropertyValue("first_name.value") Then
                    person.AddPropertyValue("first_name", tbFirstName.Text)
                End If
                If tbPreferredName.Text <> thisUser.GetPropertyValue("preferred_name.value") Then
                    person.AddPropertyValue("preferred_name", tbPreferredName.Text)
                End If
                If tbLastName.Text <> thisUser.GetPropertyValue("last_name.value") Then
                    person.AddPropertyValue("last_name", tbLastName.Text)
                End If
                If tbEmail.Text <> thisUser.GetPropertyValue("email_address.email.value") Then
                    person.AddPropertyValue("email_address.email", tbEmail.Text)
                End If

                If tbAddress1.Text <> thisUser.GetPropertyValue("address.line1.value") Then
                    person.AddPropertyValue("address.line1", tbAddress1.Text)
                End If
                If tbAddress2.Text <> thisUser.GetPropertyValue("address.line2.value") Then
                    person.AddPropertyValue("address.line2", tbAddress2.Text)
                End If
                If tbCity.Text <> thisUser.GetPropertyValue("address.city.value") Then
                    person.AddPropertyValue("address.city", tbCity.Text)
                End If
                If tbState.Text <> thisUser.GetPropertyValue("address.state.value") Then
                    person.AddPropertyValue("address.state", tbState.Text)
                End If
                If tbPostalCode.Text <> thisUser.GetPropertyValue("address.portal_code.value") Then
                    person.AddPropertyValue("address.postal_code", tbPostalCode.Text)
                End If
                If tbCountry.Text <> thisUser.GetPropertyValue("address.country.value") Then
                    person.AddPropertyValue("address.country", tbCountry.Text)
                End If
                If ddlGender.SelectedValue <> thisUser.GetPropertyValue("gender.value") Then
                    person.AddPropertyValue("gender", ddlGender.SelectedValue)
                End If
                If ddlMaritalStatus.SelectedValue <> thisUser.GetPropertyValue("marital_status.value") Then
                    person.AddPropertyValue("marital_status", ddlMaritalStatus.SelectedValue)
                End If
                If tbBirthday.Text <> thisUser.GetPropertyValue("birth_date.value") Then
                    person.AddPropertyValue("birth_date", tbBirthday.Text)
                End If
                If ddlMinistry.SelectedValue <> thisUser.GetPropertyValue("ministry:relationship.ministry.id") Then
                    person.AddPropertyValue("ministry:relationship.ministry", ddlMinistry.SelectedValue)
                    person.AddPropertyValue("ministry:relationship.role", ddlRoleType.SelectedItem.Text)
                End If



                Dim resp = GR.UpdateEntity(person, "person")
                lblText.Text = resp

                LoadForm()
            End If
        End Sub
        Public Function GetDateFormat() As String
            Dim sdp As String = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToLower
            If sdp.IndexOf("d") < sdp.IndexOf("m") Then
                Return "dd/mm/yy"
            Else
                Return "mm/dd/yy"
            End If
        End Function

        Protected Sub UpdatePanel1_Load(sender As Object, e As EventArgs) Handles UpdatePanel1.Load
          
        End Sub

        Protected Sub ddlMinistryLevel_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlMinistryLevel.SelectedIndexChanged
            gr = New GR(StaffBrokerFunctions.GetSetting("gr_api_key", PortalId), gr_server)
            If (ddlMinistryLevel.SelectedValue <> "") Then
                ddlMinistry.Items.Clear()
                ddlMinistry.Items.Add("")
                Dim scopeFilter = "&created_by=all&filters[ministry_scope]=" & ddlMinistryLevel.SelectedItem.Text
                Dim ministries = From c In gr.GetEntities("ministry", scopeFilter, 1, 1000, 1) Select name = c.GetPropertyValue("name.value"), c.ID Order By name
                ddlMinistry.DataSource = ministries
                ddlMinistry.DataTextField = "name"
                ddlMinistry.DataValueField = "id"
                ddlMinistry.DataBind()



                ' ddlMinistry.SelectedValue = min_id



            End If
        End Sub
    End Class
End Namespace
