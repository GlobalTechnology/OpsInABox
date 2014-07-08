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


Imports Resources


Namespace DotNetNuke.Modules.StaffRmb
    Partial Class viewRmbTemplates
        Inherits Entities.Modules.PortalModuleBase

        'Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Init

        '    Dim d As New ResourcesDataContext
        '    Dim q = From c In d.Agape_Main_EmailTemplates Where c.PortalId = PortalId And c.TemplateName = "RmbSplash"
        '    If q.Count = 0 Then
        '        Dim insert As New Resources.Agape_Main_EmailTemplate
        '        insert.PortalId = PortalId
        '        insert.TemplateName = "RmbSplash"
        '        insert.Template = ""
        '        d.Agape_Main_EmailTemplates.InsertOnSubmit(insert)
        '        d.SubmitChanges()
        '        FormView1.DataBind()

        '    End If

        '    Dim r = From c In d.Agape_Main_EmailTemplates Where c.PortalId = PortalId And c.TemplateName = "RmbConf"
        '    If r.Count = 0 Then
        '        Dim insert As New Resources.Agape_Main_EmailTemplate
        '        insert.PortalId = PortalId
        '        insert.TemplateName = "RmbConf"
        '        insert.Template = ""
        '        d.Agape_Main_EmailTemplates.InsertOnSubmit(insert)
        '        d.SubmitChanges()
        '        FormView2.DataBind()

        '    End If
        '    Dim s = From c In d.Agape_Main_EmailTemplates Where c.PortalId = PortalId And c.TemplateName = "ApproverEmail"
        '    If s.Count = 0 Then
        '        Dim insert As New Resources.Agape_Main_EmailTemplate
        '        insert.PortalId = PortalId
        '        insert.TemplateName = "ApproverEmail"
        '        insert.Template = ""
        '        d.Agape_Main_EmailTemplates.InsertOnSubmit(insert)
        '        d.SubmitChanges()
        '        FormView3.DataBind()

        '    End If
        '    Dim t = From c In d.Agape_Main_EmailTemplates Where c.PortalId = PortalId And c.TemplateName = "ApprovedEmail"
        '    If t.Count = 0 Then
        '        Dim insert As New Resources.Agape_Main_EmailTemplate
        '        insert.PortalId = PortalId
        '        insert.TemplateName = "ApprovedEmail"
        '        insert.Template = ""
        '        d.Agape_Main_EmailTemplates.InsertOnSubmit(insert)
        '        d.SubmitChanges()
        '        FormView4.DataBind()

        '    End If
        '    Dim u = From c In d.Agape_Main_EmailTemplates Where c.PortalId = PortalId And c.TemplateName = "ApprovedApprEmail"
        '    If u.Count = 0 Then
        '        Dim insert As New Resources.Agape_Main_EmailTemplate
        '        insert.PortalId = PortalId
        '        insert.TemplateName = "ApprovedApprEmail"
        '        insert.Template = ""
        '        d.Agape_Main_EmailTemplates.InsertOnSubmit(insert)
        '        d.SubmitChanges()
        '        FormView4.DataBind()

        '    End If
        'End Sub


        'Protected Sub btnReutrn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReutrn.Click
        '    Response.Redirect(NavigateURL())
        'End Sub
    End Class
End Namespace
