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




Namespace DotNetNuke.Modules.SearchDirectory
    Partial Class SearchDirectory
        Inherits Entities.Modules.PortalModuleBase

       
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
            


        End Sub


       
        Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Button1.Click
            'Dim d As New FullStory.FullStoryDataContext
            'If d.Agape_Main_GlobalDatas.Count > 0 Then
            '    Response.Redirect(NavigateURL(CInt(d.Agape_Main_GlobalDatas.First.StaffDirectoryTabId)) & "?search=" & SearchBox.Text)

            'End If


        End Sub
    End Class
End Namespace
