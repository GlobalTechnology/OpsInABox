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
Imports StaffBroker
Imports Documents


Namespace DotNetNuke.Modules.Menu

    Partial Class EditMenu
        Inherits Entities.Modules.ModuleSettingsBase
        Dim dFiles As Dictionary(Of String, String)
        Dim dFolders As Dictionary(Of String, String)
        Dim dtabs As Dictionary(Of String, String)

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            If Not Page.IsPostBack Then
                PortalIdHF.Value = PortalId
                ModuleHF.Value = TabModuleId
                If CType(TabModuleSettings("Prefix"), String) <> "" Then
                    tbPrefix.Text = CType(TabModuleSettings("Prefix"), String)
                End If
                If CType(TabModuleSettings("Suffix"), String) <> "" Then
                    tbSuffix.Text = CType(TabModuleSettings("Suffix"), String)
                End If
                If CType(TabModuleSettings("MenuType"), String) <> "" Then
                    ddlType.SelectedValue = CType(TabModuleSettings("MenuType"), String)
                End If
                'If CType(TabModuleSettings("MenuWidth"), String) <> "" Then
                '    tbMenuWidth.Text = CType(TabModuleSettings("MenuWidth"), String)
                'End If
            End If
        End Sub

        Protected Sub CancelButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CancelButton.Click

            Response.Redirect(NavigateURL())
         

        End Sub

        Protected Sub SaveMenuButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SaveMenuButton.Click
            Dim objModules As New Entities.Modules.ModuleController


            objModules.UpdateTabModuleSetting(TabModuleId, "Prefix", tbPrefix.Text)
            objModules.UpdateTabModuleSetting(TabModuleId, "Suffix", tbSuffix.Text)
            objModules.UpdateTabModuleSetting(TabModuleId, "MenuType", ddlType.SelectedValue)
            ' objModules.UpdateTabModuleSetting(TabModuleId, "MenuWidth", tbMenuWidth.Text)

            SynchronizeModule()


           


            
                Response.Redirect(NavigateURL())
           

        End Sub

        

        Protected Sub gvLinks_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvLinks.RowCommand
            If e.CommandName = "Promote" Then
                Dim d As New StaffBrokerDataContext
                Dim thisLink = (From c In d.AP_Menu_Links Where c.LinkId = CInt(e.CommandArgument)).First

                If thisLink.ViewOrder <> 0 Then
                    thisLink.ViewOrder = thisLink.ViewOrder - 1
                    Dim nextLink = (From c In d.AP_Menu_Links Where c.PortalId = PortalId And c.ViewOrder = thisLink.ViewOrder).First
                    nextLink.ViewOrder = nextLink.ViewOrder + 1

                End If

                d.SubmitChanges()
                gvLinks.DataBind()

            ElseIf e.CommandName = "Demote" Then
                Dim d As New StaffBrokerDataContext
                Dim thisLink = (From c In d.AP_Menu_Links Where c.LinkId = CInt(e.CommandArgument)).First


                thisLink.ViewOrder = thisLink.ViewOrder + 1
                Dim nextLink = (From c In d.AP_Menu_Links Where c.PortalId = PortalId And c.ViewOrder = thisLink.ViewOrder)

                If nextLink.Count > 0 Then
                    nextLink.First.ViewOrder = nextLink.First.ViewOrder - 1
                    d.SubmitChanges()
                    gvLinks.DataBind()

                End If
            ElseIf e.CommandName = "BInsert" Then
                Dim d As New StaffBrokerDataContext
                Dim insert As New AP_Menu_Link
                Dim newViewOrder = 0
                Dim q = (From c In d.AP_Menu_Links Where c.PortalId = PortalId And c.TabModuleId = TabModuleId Select c.ViewOrder)
                If q.Count > 0 Then
                    newViewOrder = q.Max + 1
                End If
                insert.ViewOrder = newViewOrder
                insert.PortalId = PortalId
                insert.TabModuleId = TabModuleId

                insert.LinkType = CType(gvLinks.Controls(0).Controls(0).FindControl("biType"), DropDownList).SelectedValue
                Select Case insert.LinkType
                    Case 0
                        insert.Ref = CType(gvLinks.Controls(0).Controls(0).FindControl("biRef"), TextBox).Text
                    Case 1
                        insert.Ref = CType(gvLinks.Controls(0).Controls(0).FindControl("biTabs"), DropDownList).SelectedValue
                    Case 2
                        insert.Ref = CType(gvLinks.Controls(0).Controls(0).FindControl("biFiles"), DropDownList).SelectedValue
                    Case 3
                        insert.Ref = CType(gvLinks.Controls(0).Controls(0).FindControl("biFolders"), DropDownList).SelectedValue
                    Case 20
                        insert.Ref = ""

                End Select
                If insert.LinkType <> 20 Then
                    insert.DisplayName = CType(gvLinks.Controls(0).Controls(0).FindControl("biDisplayName"), TextBox).Text
                    insert.Target = IIf(CType(gvLinks.Controls(0).Controls(0).FindControl("biNewTab"), CheckBox).Checked, "_blank", "_self")

                Else
                    insert.DisplayName = ""
                    insert.Target = "_self"

                End If


                d.AP_Menu_Links.InsertOnSubmit(insert)
                d.SubmitChanges()
                gvLinks.DataBind()

            ElseIf e.CommandName = "myInsert" Then
                Dim d As New StaffBrokerDataContext
                Dim insert As New AP_Menu_Link
                Dim newViewOrder = 0
                Dim q = (From c In d.AP_Menu_Links Where c.PortalId = PortalId And c.TabModuleId = TabModuleId Select c.ViewOrder)
                If q.Count > 0 Then
                    newViewOrder = q.Max + 1
                End If
                insert.ViewOrder = newViewOrder
                insert.PortalId = PortalId
                insert.TabModuleId = TabModuleId
                insert.LinkType = CType(gvLinks.FooterRow.Controls(0).FindControl("ddlfType"), DropDownList).SelectedValue
                Select Case insert.LinkType
                    Case 0
                        insert.Ref = CType(gvLinks.FooterRow.Controls(0).FindControl("tbfRef"), TextBox).Text
                    Case 1
                        insert.Ref = CType(gvLinks.FooterRow.Controls(0).FindControl("ddlTabs"), DropDownList).SelectedValue
                    Case 2
                        insert.Ref = CType(gvLinks.FooterRow.Controls(0).FindControl("ddlFiles"), DropDownList).SelectedValue
                    Case 3
                        insert.Ref = CType(gvLinks.FooterRow.Controls(0).FindControl("ddlFolders"), DropDownList).SelectedValue
                    Case 20
                        insert.Ref = ""

                End Select

                If insert.LinkType <> 20 Then
                    insert.DisplayName = CType(gvLinks.FooterRow.Controls(0).FindControl("tbfDisplayName"), TextBox).Text()
                    insert.Target = IIf(CType(gvLinks.FooterRow.Controls(0).FindControl("cbfNewTab"), CheckBox).Checked, "_blank", "_self")

                Else
                    insert.DisplayName = ""
                    insert.Target = "_self"

                End If
                d.AP_Menu_Links.InsertOnSubmit(insert)
                d.SubmitChanges()
                gvLinks.DataBind()
            ElseIf e.CommandName = "myUpdate" Then
                Dim d As New StaffBrokerDataContext
                Dim q = From c In d.AP_Menu_Links Where c.LinkId = CInt(e.CommandArgument)
                q.First.LinkType = CType(gvLinks.Rows(gvLinks.EditIndex).FindControl("ddlType"), DropDownList).SelectedValue
                Select Case q.First.LinkType
                    Case 0
                        q.First.Ref = CType(gvLinks.Rows(gvLinks.EditIndex).FindControl("tbRef"), TextBox).Text
                    Case 1
                        q.First.Ref = CType(gvLinks.Rows(gvLinks.EditIndex).FindControl("ddlETabs"), DropDownList).SelectedValue
                    Case 2
                        q.First.Ref = CType(gvLinks.Rows(gvLinks.EditIndex).FindControl("ddlEFiles"), DropDownList).SelectedValue
                    Case 3
                        q.First.Ref = CType(gvLinks.Rows(gvLinks.EditIndex).FindControl("ddlEFolders"), DropDownList).SelectedValue
                    Case 20
                        q.First.Ref = ""

                End Select

                If q.First.LinkType <> 20 Then
                    q.First.DisplayName = CType(gvLinks.Rows(gvLinks.EditIndex).FindControl("tbDisplayName"), TextBox).Text
                    q.First.Target = IIf(CType(gvLinks.Rows(gvLinks.EditIndex).FindControl("cbNewTab"), CheckBox).Checked, "_blank", "_self")


                Else
                    q.First.DisplayName = ""
                    q.First.Target = "_self"

                End If



                d.SubmitChanges()
                gvLinks.EditIndex = -1
                gvLinks.DataBind()

            ElseIf e.CommandName = "myDelete" Then
                Dim d As New StaffBrokerDataContext
                Dim q = From c In d.AP_Menu_Links Where c.LinkId = CInt(e.CommandArgument) And c.PortalId = PortalId And c.TabModuleId = TabModuleId

                d.AP_Menu_Links.DeleteAllOnSubmit(q)
                d.SubmitChanges()



                Dim allFiles = From c In d.AP_Menu_Links Where c.PortalId = PortalId And c.TabModuleId = TabModuleId Order By c.ViewOrder

                Dim i As Integer = 0
                For Each row In allFiles

                    row.ViewOrder = i
                    i += 1
                Next
                d.SubmitChanges()

                gvLinks.DataBind()

            End If
        End Sub

        Public Function GetFiles(Optional ByVal Ref As String = Nothing) As Dictionary(Of String, String)
            Dim rtn As New Dictionary(Of String, String)
            If Not dFiles Is Nothing Then
                rtn = dFiles
                If Not Ref Is Nothing And rtn.Where(Function(x) x.Key = Ref).Count = 0 Then
                    rtn.Add(Ref, "")
                End If
                Return rtn
            End If

            Dim d As New DocumentsDataContext
            Dim files = (From c In d.AP_Documents_Docs Where c.AP_Documents_Folder.PortalId = PortalId Order By c.DisplayName Select c.DisplayName, c.DocId)
            For Each row In files
                rtn.Add(row.DocId, row.DisplayName)

            Next
            dFiles = rtn
            If Not Ref Is Nothing And rtn.Where(Function(x) x.Key = Ref).Count = 0 Then
                rtn.Add(Ref, "")
            End If

            Return rtn
        End Function
        Public Function GetFolders(Optional ByVal Ref As String = Nothing) As Dictionary(Of String, String)
            Dim rtn As New Dictionary(Of String, String)
            If Not dFolders Is Nothing Then
                rtn = dFolders
                If Not Ref Is Nothing And rtn.Where(Function(x) x.Key = Ref).Count = 0 Then
                    rtn.Add(Ref, "")
                End If
                Return rtn
            End If
            Dim d As New DocumentsDataContext
            Dim folders = (From c In d.AP_Documents_Folders Where c.PortalId = PortalId Order By c.Name Select c.Name, c.FolderId)
            For Each row In folders
                rtn.Add(row.FolderId, row.Name)

            Next
            dFolders = rtn
            If Not Ref Is Nothing And rtn.Where(Function(x) x.Key = Ref).Count = 0 Then
                rtn.Add(Ref, "")
            End If

            Return rtn
        End Function

        Public Function GetTabs(Optional ByVal Ref As String = Nothing) As Dictionary(Of String, String)
            Dim rtn As New Dictionary(Of String, String)
            If Not dtabs Is Nothing Then
                rtn = dtabs
                If Not Ref Is Nothing And rtn.Where(Function(x) x.Key = Ref).Count = 0 Then
                    rtn.Add(Ref, "")
                End If
                Return rtn
            End If
            Dim tabs = DotNetNuke.Entities.Tabs.TabController.GetPortalTabs(PortalId, TabId, False, False)

            For Each row In tabs.OrderBy(Function(x) x.TabName)
                rtn.Add(row.TabID, row.TabName)
            Next
            dtabs = rtn
            If Not Ref Is Nothing And rtn.Where(Function(x) x.Key = Ref).Count = 0 Then
                rtn.Add(Ref, "")
            End If

            Return rtn
        End Function
        Public Function IsLastRow(ByVal thisVO As Integer) As Boolean
            Dim d As New StaffBrokerDataContext
            Dim q = From c In d.AP_Menu_Links Where c.PortalId = PortalId And c.TabModuleId = TabModuleId Select c.ViewOrder

            If q.Count = 0 Then
                Return True
            Else
                Return thisVO < q.Max

            End If
        End Function
    End Class
End Namespace
