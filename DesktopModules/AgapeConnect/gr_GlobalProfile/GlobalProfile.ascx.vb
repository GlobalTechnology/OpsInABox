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
Imports DotNetNuke.Services.FileSystem
Namespace DotNetNuke.Modules.AgapeConnect
    Partial Class GlobalProfile
        Inherits Entities.Modules.PortalModuleBase

        Public pg As Integer = 1

        Public filters As String = ""
        Public min_strings As String = ""
        Dim gr As GR
        Public gr_server As String = "http://192.168.2.244:3000/"
        Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load


            Dim URL = UrlUtils.DecryptParameter(Context.Request.QueryString("fileticket"))

            Dim File = FileManager.Instance.GetFile(URL)
            Dim folder = FolderManager.Instance.GetFolder(File.FolderId)
            Dim dic = String.Format(DataCache.FolderPermissionCacheKey, PortalId)
            lblTest.Text = dic

            '  lblTest.Text = PortalSecurity.IsInRoles(FileSystemUtils.GetRoles(File.Folder, PortalId, "READ"))


            gr_server = StaffBrokerFunctions.GetSetting("gr_api_url", PortalId)
            gr = New GR(StaffBrokerFunctions.GetSetting("gr_api_key", PortalId), gr_server)
            If Not Page.IsPostBack Then
                PopulateDropdowns()
            End If

            If Not String.IsNullOrEmpty(Request.QueryString("page")) Then
                pg = Request.QueryString("page")
                Search()

            End If


        End Sub


        Protected Sub PopulateDropdowns()
            ddlRoleType.Items.Clear()
            ddlMinistryLevel.Items.Clear()
            ddlRoleType.Items.Add("")
            ddlMinistryLevel.Items.Add("")
            Dim role = gr.GetEntities("role", "")

            ddlRoleType.DataSource = From c In role Select name = c.GetPropertyValue("value"), c.ID

            ddlRoleType.DataTextField = "name"
            ddlRoleType.DataValueField = "ID"
            ddlRoleType.DataBind()


          

            Dim scope = gr.GetEntities("ministry_scope", "")
            ddlMinistryLevel.DataSource = From c In scope Select name = c.GetPropertyValue("value"), c.ID

            ddlMinistryLevel.DataTextField = "name"
            ddlMinistryLevel.DataValueField = "ID"
            ddlMinistryLevel.DataBind()
           
          
        End Sub


        Private Sub Search()


            filters = "&created_by=all"
            If Not String.IsNullOrEmpty(tbFirstNameSearch.Text) Then
                filters &= "&filters[first_name]=" & tbFirstNameSearch.Text
            End If
            If Not String.IsNullOrEmpty(tbLastNameSearch.Text) Then
                filters &= "&filters[last_name]=" & tbLastNameSearch.Text
            End If

            If Not String.IsNullOrEmpty(tbEmailSearch.Text) Then
                filters &= "&filters[email_address][email]=" & tbEmailSearch.Text
            End If

            If (ddlRoleType.SelectedValue <> "") Then
                filters &= "&filters[ministry][role]=" & ddlRoleType.SelectedItem.Text
            End If

            If (ddlMinistryLevel.SelectedValue <> "") Then
                filters &= "&filters[ministry][ministry_scope]=" & ddlMinistryLevel.SelectedItem.Text
            End If

            If Not String.IsNullOrEmpty(tbAdvancedSearch.Text) Then
                filters &= "&" & tbAdvancedSearch.Text
            End If

            'Dim page As Integer
            'If Not String.IsNullOrEmpty(Request.QueryString("page")) Then
            '    page = Request.QueryString("page")
            'End If

            Dim totalPage As Integer

            Dim people = gr.GetEntities("person", filters, pg, 25, totalPage)
            rpResults.DataSource = From c In people Select FirstName = c.GetPropertyValue("first_name.value"), LastName = c.GetPropertyValue("last_name.value"), c.ID Order By LastName

            lblTest.Text = totalPage & ":" & people.Count


            rpResults.DataBind()

            If totalPage > 1 Then
                pnlPagination.Visible = True
                ltPages.Text = ""
                For i As Integer = 1 To totalPage
                    ltPages.Text &= "<li class='" & IIf(pg = i, "active", "") & "'><a href=""" & NavigateURL() & "?page=" & i & """>" & i & "</a></li>"
                Next
                prev.Attributes("class") = IIf(pg > 1, "", "disabled")
                [next].Attributes("enabled") = IIf(pg < totalPage, "", "disabled")
            Else
                pnlPagination.Visible = False
            End If

            ' lblTest.Text = people.First.ToJson()
            'Session("gr_user_results") = people
            pnlResults.Visible = True
        End Sub

        Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
            pg = 1
            Search()

        End Sub
    End Class
End Namespace
