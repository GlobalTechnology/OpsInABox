Imports DotNetNuke
Imports System.Web.UI
Imports System.Collections.Generic
Imports System.Reflection
Imports System.Math
Imports System.Net
Imports System.IO
Imports System.Text
Imports System.Net.Mail
Imports System.Collections.Specialized

Imports System.Linq
Imports UK.AgapeDocuments




Namespace DotNetNuke.Modules.AgapeDocuments

    Partial Class AddDocument
        Inherits Entities.Modules.ModuleSettingsBase
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load

        End Sub

       
        Protected Sub AddBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AddBtn.Click
            Dim d As New AgapeDocumentsDataContext

            Dim q = From c In d.Agape_Main_AgapeDocuments Select c.SortOrder
            Dim NewSortOrder = 0
            If q.Count > 0 Then
                NewSortOrder = q.Max + 1

            End If





            Dim insert As New Agape_Main_AgapeDocument
            insert.DocTitle = Title.Text
            insert.Subtitle = Subtitle.Text
            'insert.DocDescription = Description.RichText.Text
            insert.LinkType = Left(theFile.UrlType, 1)

            If (theFile.UrlType = "F") Then
                insert.FileId = CInt(theFile.Url.Substring(7))
            Else
                insert.URL = theFile.Url
            End If



            insert.SortOrder = NewSortOrder
            insert.ModuleId = ModuleId
            insert.PortalId = PortalId

            d.Agape_Main_AgapeDocuments.InsertOnSubmit(insert)
            d.SubmitChanges()

          





            Response.Redirect(NavigateURL)


        End Sub

        Protected Sub CancelBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CancelBtn.Click
            Response.Redirect(NavigateURL())
        End Sub
    End Class
End Namespace
