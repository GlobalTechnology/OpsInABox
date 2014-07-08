'
' DotNetNuke® - http://www.dotnetnuke.com
' Copyright (c) 2002-2009
' by DotNetNuke Corporation
'
' Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
' documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
' the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
' to permit persons to whom the Software is furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in all copies or substantial portions 
' of the Software.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
' DEALINGS IN THE SOFTWARE.
'
Imports DotNetNuke.Services.Vendors
Imports System.Linq


Namespace DotNetNuke.UI.Skins.Controls
    ''' -----------------------------------------------------------------------------
    ''' <summary></summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' 	[cniknet]	10/15/2004	Replaced public members with properties and removed
    '''                             brackets from property names
    ''' </history>
    ''' -----------------------------------------------------------------------------

    Public MustInherit Class AgapeSkinIcons

        Inherits UI.Skins.SkinObjectBase

        ' private members

        Private _IconHeight As String

        Private _IconSpacing As String


        Const MyFileName As String = "Banner.ascx"

#Region "Public Members"
    


#End Region

#Region "Event Handlers"

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            
            Dim d As New AgapeIconAdmin.AgapeIconsDataContext
            Dim q = From c In d.Agape_Skin_AgapeIcons Where c.PortalId = PortalSettings.PortalId Order By c.ViewOrder Descending
            Dim r = From c In d.Agape_Skin_IconSettings Where c.PortalId = PortalSettings.PortalId
            For Each row In q
                Dim pd As Integer = 5
                If r.Count > 0 Then
                    pd = r.First.Padding
                End If

                PlaceHolder1.Controls.Add(New LiteralControl("<div style=""float: right; text-align: center; padding: 0 " & pd & "px 0 " & pd & "px; white-space: nowrap; background: url(" & Request.Url.Scheme & "://" & Request.Url.Authority & Request.ApplicationPath & GetUrlFromFileId(row.HovrIconFile).Substring(1) & ") no-repeat -9999px -9999px; "">"))

                Dim h As New HyperLink()
                h.NavigateUrl = GetUrl(row.LinkType, row.LinkLoc)
                h.Attributes.Add("Style", " text-align: center;")


                Dim i As New System.Web.UI.WebControls.Image()

                i.ImageUrl = GetUrlFromFileId(row.IconFile)
                i.Attributes("onmouseout") = "this.src='" & GetUrlFromFileId(row.IconFile) & "';"
                i.Attributes("onmouseover") = "this.src='" & GetUrlFromFileId(row.HovrIconFile) & "';"

                If r.Count > 0 Then
                    i.Height = r.First.IconHeight

                    h.Controls.Add(i)
                    If r.First.ShowTitles Then
                        i.Attributes.Add("Style", " ")
                        Dim lbl As New Label
                        lbl.Text = "<br />" & row.Title
                        h.Controls.Add(lbl)
                    End If



                Else
                    i.Height = 110
                    h.Controls.Add(i)
                End If

                PlaceHolder1.Controls.Add(h)
                PlaceHolder1.Controls.Add(New LiteralControl("</div>"))






                'Dim icon As New ImageButton
                'icon.ImageUrl = GetUrlFromFileId(row.IconFile)
                'icon.Attributes("onmouseout") = "this.src='" & GetUrlFromFileId(row.IconFile) & "';"
                'icon.Attributes("onmouseover") = "this.src='" & GetUrlFromFileId(row.HovrIconFile) & "';"
                'icon.OnClientClick = "window.location='" & GetUrl(row.LinkType, row.LinkLoc) & "';"

                'If r.Count > 0 Then
                '    icon.Height = r.First.IconHeight
                'Else
                '    icon.Height = 110
                'End If



            Next
            PlaceHolder1.Controls.Add(New LiteralControl("<div style=""clear: both"" />"))
            ' Dim test = "this.src= '" & GetUrlFromFileId(Eval("IconFile")) & "';"


        End Sub
#End Region

#Region "Public Methods"
        Public Function GetUrl(ByVal FileType As String, ByVal Url As String) As String
            If FileType = "T" Then
                Return NavigateURL(CInt(Url))
            Else
                Return Url
            End If
        End Function

        Public Function GetUrlFromFileId(ByVal FileId As Integer) As String
            Dim d As New AgapeIconAdmin.AgapeIconsDataContext

            Dim q = From c In d.Files Where c.FileId = FileId

            If q.Count > 0 Then
                Return PortalSettings.HomeDirectory & q.First.Folder & q.First.FileName
            End If
            Return ""
           

        End Function

     
#End Region

    End Class
End Namespace
