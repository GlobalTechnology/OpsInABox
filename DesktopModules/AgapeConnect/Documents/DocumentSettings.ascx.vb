Imports DotNetNuke
Imports System.Web.UI
Imports System.Linq

Imports Documents



Namespace DotNetNuke.Modules.AgapeConnect.Documents

    Partial Class DocumentSettings
        Inherits Entities.Modules.ModuleSettingsBase

        Dim d As New DocumentsDataContext

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
            Try

                If Not IsPostBack Then
                    Session("Check_Page_Refresh") = DateTime.Now.ToString()

                    'Get all folders in the portal
                    Dim folders As List(Of AP_Documents_Folder) = DocumentsController.GetFolders()

                    'Build the list of paths
                    Dim pathName As String = ""

                    For Each folder In folders
                        pathName = DocumentsController.GetPathName(folder, pathName)
                        ddlRoot.Items.Add(New ListItem(pathName, folder.FolderId))
                        pathName = ""
                    Next

                    'Populate the dropdown list with correct path 
                    If CType(TabModuleSettings(RootFolderSettingKey), String) <> "" Then

                        If ddlRoot.Items.FindByValue(CType(TabModuleSettings(RootFolderSettingKey), Integer)) Is Nothing Then
                            ddlRoot.SelectedIndex = 0
                        Else
                            ddlRoot.SelectedValue = CType(TabModuleSettings(RootFolderSettingKey), Integer)
                        End If
                    End If
                End If                       'IsPostBack

            Catch exc As Exception           'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Protected Overrides Sub onPreRender(ByVal e As System.EventArgs)
            ViewState("Check_Page_Refresh") = Session("Check_Page_Refresh")
        End Sub










        Protected Sub SaveBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SaveBtn.Click
            Dim objModules As New Entities.Modules.ModuleController

            objModules.UpdateTabModuleSetting(TabModuleId, RootFolderSettingKey, ddlRoot.SelectedValue)

            ' refresh cache
            DotNetNuke.Entities.Modules.ModuleController.SynchronizeModule(ModuleId)
            Response.Redirect(NavigateURL())

        End Sub


        Protected Sub CancelBtn_Click(sender As Object, e As System.EventArgs) Handles CancelBtn.Click
            Response.Redirect(NavigateURL())
        End Sub


    End Class

End Namespace

