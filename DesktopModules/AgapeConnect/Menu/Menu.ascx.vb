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
Imports System.Xml.Linq
Imports System.Linq
Imports Documents

Namespace DotNetNuke.Modules.Menu



    Partial Class Menu
        Inherits Entities.Modules.PortalModuleBase

        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init


        End Sub

        Dim mode As String = ""
        Dim DocTabId As String = ""
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            PrefixLabel.Text = Settings("Prefix")
            SuffixLabel.Text = Settings("Suffix")
            'If Settings("MenuWidth") <> "" Then
            '    mainPanel.Width = New Unit(Settings("MenuWidth"), UnitType.Pixel)
            'End If

            PortalIdHF.Value = PortalId

            'For now this is a documents module
            Select Case Settings("MenuType")
                Case "Tags"

                    mode = "tags"
                    Dim d As New DocumentsDataContext
                    Dim q = From c In d.AP_Documents_Tags Where c.PortalId = PortalId

                    MenuItemGrid.DataSource = q

                    MenuItemGrid.DataBind()


                Case "Normal"
                    mode = "normal"
                    Dim ds As New StaffBroker.StaffBrokerDataContext
                    Dim q = From c In ds.AP_Menu_Links Where c.PortalId = PortalId And c.TabModuleId = TabModuleId

                    ManualMenu.DataSource = q
                    ManualMenu.DataBind()

            End Select



            If IsEditable Then
                EditButton.Visible = True
            End If


            Dim mc As New DotNetNuke.Entities.Modules.ModuleController
            Dim modulesOnPage = mc.GetTabModules(TabId)
            If modulesOnPage.Where(Function(x) x.Value.ModuleDefinition.FriendlyName = "acDocuments").Count > 0 Then
                DocTabId = TabId
            Else
                Dim x = mc.GetModuleByDefinition(PortalId, "acDocuments")
                If Not x Is Nothing Then
                    DocTabId = x.TabID
                End If

            End If



           


        End Sub

        Public Function GetDocumnetLinkURL(ByVal value As String) As String
          
            If Not DocTabId Is Nothing Then

                Return NavigateURL(DocTabId) & "?search=" & Server.HtmlEncode(value) & "&mode=" & mode
            End If
            Return ""
        End Function


        Public Function GetLinkURL(ByVal LinkId As Integer) As String
            Dim d As New StaffBroker.StaffBrokerDataContext
            Dim q = From c In d.AP_Menu_Links Where c.LinkId = LinkId
            
            If q.Count > 0 Then
                Select Case q.First.LinkType
                    Case 0 'URL
                        Return q.First.Ref
                    Case 1 'Tab
                        Return NavigateURL(CInt(q.First.Ref))
                    Case 2 ' File
                        Return NavigateURL(DocTabId) & "?DocId=" & q.First.Ref
                    Case 3 'Folder
                        Return NavigateURL(DocTabId) & "?FolderId=" & q.First.Ref


                End Select


            End If
            Return ""

        End Function

        Protected Sub EditButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles EditButton.Click

            Response.Redirect(EditUrl("EditMenu"))


        End Sub
        Public Function GetColor() As Color
            If ModuleConfiguration.ContainerSrc.IndexOf("Green") > 0 Then
                Return Color.FromArgb(255, 70, 102, 41)
            ElseIf ModuleConfiguration.ContainerSrc.IndexOf("BlueStripe") > 0 Then
                Return Color.FromArgb(255, 0, 81, 121)

            Else
                Return Color.FromArgb(255, 102, 0, 0)

            End If

        End Function
    End Class
End Namespace
