Imports System.IO
Imports System.Xml
Imports System.Net
Imports DotNetNuke.Entities.Tabs
Imports DotNetNuke.Security.Permissions
Imports DotNetNuke.UI.Skins
Imports DotNetNuke.UI.Utilities
Imports DotNetNuke.Security.Membership
Imports DotNetNuke.Services.Authentication
Imports System.Linq


'Imports Resources


Namespace DotNetNuke.Modules.AgapeConnect

  

    Partial Class Translate
        Inherits Entities.Modules.PortalModuleBase

        Private exlList As String() = {".svn"}
        Private root
        Private Sub CreateNewDataNode(ByRef tdoc As XmlDocument, ByVal KeyName As String)
            Dim rootNode = tdoc.SelectSingleNode("root")
            Dim newNode As XmlElement = tdoc.CreateElement("data")
            Dim nameAtt As XmlAttribute = tdoc.CreateAttribute("name")
            nameAtt.Value = KeyName
            newNode.Attributes.Append(nameAtt)
            rootNode.AppendChild(newNode)

        End Sub
        
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try
                'Dim admAuth As New MicrosoftTranslatorSdk.HttpSamples.AdmAuthentication("AgapeConnect", "rsNAKe3a3wS5VPqiH2mtKAiFDahWvLFbNCLYzzkgOAM=")
                'Dim token = admAuth.GetAccessToken()
                'hfBingToken.Value = token.access_token

                If Not String.IsNullOrEmpty(Request.Form("UpdateItems")) Then
                    Dim updatestring = Request.Form("UpdateItems").Split(New String() {";;"}, StringSplitOptions.RemoveEmptyEntries)

                    For Each item In updatestring
                        Dim splitItem = item.Trim(";").Split(New String() {"::"}, StringSplitOptions.None)
                        Dim FileName = splitItem(0).Trim(":")
                        Dim KeyName = splitItem(1).Trim(":")
                        Dim Value = splitItem(2)

                        SaveTranslation(FileName, KeyName, Value)
                       
                        



                    Next
                    Return
                End If
            Catch ex As Exception
                Response.TrySkipIisCustomErrors = True
                Response.StatusCode = 500
                Return
            End Try
            'For Each item In Request.Form.AllKeys
            '    If Not item Is Nothing Then


            '        If item.Contains(".resx") Then
            '            Dim tDoc As New XmlDocument()
            '            Dim tDataNodes As XmlNodeList

            '            If File.Exists(item) Then

            '                tDoc.Load(item)

            '                tDataNodes = tDoc.SelectNodes("root/data")
            '            End If

            '        End If
            '    End If
            'Next



            If Not Page.IsPostBack Then
                Dim rootPath As String = ddlRoot.SelectedValue
                Dim locales = DotNetNuke.Services.Localization.LocaleController.Instance.GetLocales(PortalId)
                ddlLanguages.DataSource = From c In locales Select c.Value.EnglishName, c.Value.NativeName, c.Value.Code

                ddlLanguages.DataTextField = "EnglishName"
                ddlLanguages.DataValueField = "Code"
                ddlLanguages.DataBind()
                For Each row As ListItem In ddlLanguages.Items
                    If row.Text.Contains("Macedonian") Then
                        row.Text = "Macedonian (Macedonia)"
                    End If
                Next


                Dim Modules As New DirectoryInfo(Server.MapPath(rootPath))

                Dim theModules = (From c In Modules.GetDirectories Where Not exlList.Contains(c.Name) Select c.Name).ToList
                theModules.Add("StaffRmb (Expense Types)")


                lbModules.DataSource = theModules.Where(Function(x) (New DirectoryInfo(Server.MapPath(rootPath & IIf(x.Contains("Rmb"), "StaffRmb", x)))).GetFiles("*.ascx.resx", SearchOption.AllDirectories).Count).OrderBy(Function(x) x)
                lbModules.DataBind()
               


            End If

            lbModules_SelectedIndexChanged(Me, Nothing)

        End Sub

        Private Sub SaveTranslation(ByVal FileName As String, ByVal KeyName As String, ByVal Value As String)

            Dim tDoc As New XmlDocument()
            Dim tDataNodes As XmlNodeList



            If Not File.Exists(FileName) Then
                If String.IsNullOrEmpty(Value) Then
                    Return
                End If
                'create a new Resx
                System.IO.File.Copy(Server.MapPath("/DesktopModules/AgapeConnect/Translate/Template.resx"), FileName)

                tDoc.Load(FileName)
                Dim rootNode = tDoc.SelectSingleNode("root")
                Dim dummyData = tDoc.SelectSingleNode("data")
                If Not dummyData Is Nothing Then
                    rootNode.RemoveChild(dummyData)

                End If

            Else

                tDoc.Load(FileName)
            End If

            tDataNodes = tDoc.SelectNodes("root/data")
            If tDataNodes.Count = 0 Then
                'Need  to create the Node
                CreateNewDataNode(tDoc, KeyName)
                tDataNodes = tDoc.SelectNodes("root/data")
            End If

            Dim foreignNode = (From c As XmlNode In tDataNodes Where c.Attributes(0).Value = KeyName Select c)
            If foreignNode.Count = 0 Then
                CreateNewDataNode(tDoc, KeyName)
                tDataNodes = tDoc.SelectNodes("root/data")
                foreignNode = (From c As XmlNode In tDataNodes Where c.Attributes(0).Value = KeyName Select c)

            End If
            If foreignNode.First.ChildNodes.Count > 0 Then
                '  If String.IsNullOrEmpty(Value) Then
                '  foreignNode.First.RemoveAll()

                ' Else
                For Each child As XmlNode In foreignNode.First.ChildNodes
                    If child.Name = "value" Then

                        child.InnerText = Value



                    End If
                Next
                ' End If

            Else
                If Not String.IsNullOrEmpty(Value) Then
                    Dim newNode As XmlElement = tDoc.CreateElement("value")
                    newNode.InnerText = Value
                    foreignNode.First.AppendChild(newNode)
                End If


            End If


            tDoc.Save(FileName)
        End Sub

        Protected Sub ddlRoot_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlRoot.SelectedIndexChanged
            If ddlRoot.SelectedValue = "Core" Then
                pnlDnnTranslation.Visible = True
                pnlTranslation.Visible = False
                LoadValues()


                Return
            End If
            pnlDnnTranslation.Visible = False
            pnlTranslation.Visible = True
            Dim rootPath As String = ddlRoot.SelectedValue

            Dim Modules As New DirectoryInfo(Server.MapPath(rootPath))

            Dim theModules = (From c In Modules.GetDirectories Where Not exlList.Contains(c.Name) Select c.Name).ToList


            If ddlRoot.SelectedIndex <> 0 Then
                lbModules.DataSource = theModules.Where(Function(x) (New DirectoryInfo(Server.MapPath(rootPath & x))).GetFiles("*.ascx.resx", SearchOption.AllDirectories).Count).OrderBy(Function(x) x)
            Else
                theModules.Add("StaffRmb (Expense Types)")
                lbModules.DataSource = theModules.Where(Function(x) (New DirectoryInfo(Server.MapPath(rootPath & IIf(x.Contains("Rmb"), "StaffRmb", x)))).GetFiles("*.ascx.resx", SearchOption.AllDirectories).Count).OrderBy(Function(x) x)

            End If
            lbModules.DataBind()
            lbModules_SelectedIndexChanged(Me, Nothing)
        End Sub


        Protected Sub lbModules_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles lbModules.SelectedIndexChanged



            phHeaders.Controls.Clear()
            phTabs.Controls.Clear()



            If lbModules.SelectedIndex >= 0 Then
                Dim rootPath = Server.MapPath(ddlRoot.SelectedValue & lbModules.SelectedValue)
                If lbModules.SelectedValue = "StaffRmb (Expense Types)" Then
                    rootPath = Server.MapPath("/DesktopModules/AgapeConnect/StaffRmb/Controls")
                    pnlExpenses.Visible = True
                    'cbSaveAll.Checked = False
                Else
                    pnlExpenses.Visible = False
                    cbSaveAll.Checked = False
                End If

                Dim theModule = New DirectoryInfo(rootPath)
                Dim ResxFiles = From c In theModule.GetFiles("*.ascx.resx", SearchOption.AllDirectories)

                If lbModules.SelectedValue = "StaffRmb (Expense Types)" Then
                    ResxFiles = ResxFiles.Where(Function(x) x.Name.StartsWith("Rmb") And Not (x.Name.StartsWith("RmbSettings") Or x.Name.StartsWith("RmbPrintout")))
                ElseIf lbModules.SelectedValue = "StaffRmb" Then
                    ResxFiles = ResxFiles.Where(Function(x) (Not x.Name.StartsWith("Rmb")) Or x.Name.StartsWith("RmbSettings") Or x.Name.StartsWith("RmbPrintout")).OrderByDescending(Function(x) x.Name.StartsWith(lbModules.SelectedValue & ".")).ThenBy(Function(x) x.Name)
                Else
                    ResxFiles = ResxFiles.OrderByDescending(Function(x) x.Name.StartsWith(lbModules.SelectedValue & ".")).ThenBy(Function(x) x.Name)

                End If



                For Each File In ResxFiles

                    Dim thePane As New Panel()
                    thePane.ID = File.Name.Replace(".ascx.resx", "")

                    thePane.ClientIDMode = System.Web.UI.ClientIDMode.Static


                    Dim le As DesktopModules_AgapeConnect_Translate_LanuguageEditor = Page.LoadControl("/DesktopModules/AgapeConnect/Translate/Controls/LanguageEditor.ascx")
                    le.ClientIDMode = System.Web.UI.ClientIDMode.AutoID
                    le.OrigResx = File.FullName
                    le.Language = ddlLanguages.SelectedValue
                    le.PS = PortalSettings
                    If DotNetNuke.Services.Localization.Localization.SystemLocale <> ddlLanguages.SelectedValue Then
                        le.TranslateResx = le.OrigResx.Replace(".resx", "." & ddlLanguages.SelectedValue & ".Portal-" & PortalId & ".resx")
                    Else
                        le.TranslateResx = le.OrigResx.Replace(".resx", ".Portal-" & PortalId & ".resx")
                    End If
                    le.LoadEditor()


                    thePane.Controls.Add(le)
                    Dim title = File.Name.Replace(".ascx.resx", "")
                    If lbModules.SelectedValue = "StaffRmb (Expense Types)" Then
                        title = GetExpenseTypeName(title)
                    End If
                    phHeaders.Controls.Add(New LiteralControl("<li><a href='#" & thePane.ClientID & "'>" & title & "</a></li>"))
                    phTabs.Controls.Add(thePane)


                Next




                'leMain.OrigResx = Server.MapPath("/DesktopModules/AgapeConnect/" & lbModules.SelectedValue & "/App_LocalResources/" & lbModules.SelectedValue & ".ascx.resx")
                'If DotNetNuke.Services.Localization.Localization.SystemLocale <> ddlLanguages.SelectedValue Then
                '    leMain.TranslateResx = leMain.OrigResx.Replace(".resx", "." & ddlLanguages.SelectedValue & ".resx")
                'Else
                '    leMain.TranslateResx = leMain.OrigResx
                'End If

                'leMain.LoadEditor()
            End If

        End Sub
        Private Function GetExpenseTypeName(ByVal title As String) As String
            Dim d As New StaffRmb.StaffRmbDataContext
            Dim q = From c In d.AP_Staff_RmbLineTypes Where c.ControlPath.EndsWith(title & ".ascx")

            If q.Count > 0 Then
                Dim local = From c In q.First.AP_StaffRmb_PortalLineTypes Where c.PortalId = PortalId

                If local.Count > 0 Then
                    Return local.First.LocalName
                End If
                Return q.First.TypeName
            End If
            Return title
        End Function
        'Protected Sub LoadModule()
        '    Dim rootPath = Server.MapPath("/DesktopModules/AgapeConnect/" & lbModules.SelectedValue)

        '    Dim theModule = New DirectoryInfo(rootPath)
        '    Dim ResxFiles = theModule.GetFiles("*.resx", SearchOption.AllDirectories)
        '    Dim t As New ArrayList()
        '    For Each file In ResxFiles







        '        Dim doc As New XmlDocument()
        '        doc.Load(file.FullName)


        '        For Each row As XmlNode In (From c As XmlNode In doc.SelectNodes("root/data") Order By c.Attributes(0).Value)
        '            If row.NodeType <> XmlNodeType.Comment Then
        '                Dim name As String = file.Name & ": <b>" & row.Attributes(0).Value & "</b>"


        '                Dim value = row.SelectSingleNode("value")
        '                Dim valueString As String = ""
        '                If Not value Is Nothing Then
        '                    valueString = value.InnerText
        '                End If
        '                Dim comment = row.SelectSingleNode("comment")
        '                Dim commentString As String = ""
        '                If Not comment Is Nothing Then
        '                    commentString = comment.InnerText
        '                End If
        '                Dim foreignString As String = DotNetNuke.Services.Localization.Localization.GetString(row.Attributes(0).Value, file.FullName)
        '                t.Add(New Translation(name, valueString, foreignString, commentString))

        '            End If
        '        Next
        '    Next
        '    dlEditor.DataSource = t
        '    dlEditor.DataBind()

        'End Sub


        Protected Sub cbLocalNames_CheckedChanged(sender As Object, e As System.EventArgs) Handles cbLocalNames.CheckedChanged
            Dim locales = DotNetNuke.Services.Localization.LocaleController.Instance.GetLocales(PortalId)
            ddlLanguages.DataSource = From c In locales Select c.Value.EnglishName, c.Value.NativeName, c.Value.Code

            ddlLanguages.DataTextField = IIf(cbLocalNames.Checked, "EnglishName", "NativeName")
            ddlLanguages.DataValueField = "Code"
            ddlLanguages.DataBind()
        End Sub


        'Public Function GetDataPart(ByVal ClientId As String) As String


        'End Function


        Protected Sub ddlLanguages_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlLanguages.SelectedIndexChanged
            If ddlRoot.SelectedValue = "Core" Then
                pnlDnnTranslation.Visible = True
                pnlTranslation.Visible = False
                LoadValues()


                Return
            End If
            lbModules_SelectedIndexChanged(Me, Nothing)
        End Sub

        Protected Sub LoadValues()
            Dim coreFile = Server.MapPath("/Portals/_default/Skins/AgapeBlue/App_LocalResources/index.ascx.resx")

            tbSearch.Text = DotNetNuke.Services.Localization.Localization.GetString("SearchText.Text", coreFile, PortalSettings, ddlLanguages.SelectedValue)
            tbBreadcrumb.Text = DotNetNuke.Services.Localization.Localization.GetString("Breadcrumb.Text", coreFile, PortalSettings, ddlLanguages.SelectedValue)
            


            coreFile = Server.MapPath("/admin/Skins/App_LocalResources/Login.ascx.resx")
            tbLogin.Text = DotNetNuke.Services.Localization.Localization.GetString("Login.Text", coreFile, PortalSettings, ddlLanguages.SelectedValue)

         
        End Sub


        Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
            'Store the core elements
            Dim coreFile = Server.MapPath("/Portals/_default/Skins/AgapeBlue/App_LocalResources/index.ascx.resx")

           
            If DotNetNuke.Services.Localization.Localization.SystemLocale <> ddlLanguages.SelectedValue Then
                coreFile = coreFile.Replace(".resx", "." & ddlLanguages.SelectedValue & ".Portal-" & PortalId & ".resx")
            Else
                coreFile = coreFile.Replace(".resx", ".Portal-" & PortalId & ".resx")
            End If
           
            SaveTranslation(coreFile, "SearchText.Text", tbSearch.Text)
            SaveTranslation(coreFile, "Breadcrumb.Text", tbBreadcrumb.Text)


            coreFile = Server.MapPath("/admin/Skins/App_LocalResources/Login.ascx.resx")

            If DotNetNuke.Services.Localization.Localization.SystemLocale <> ddlLanguages.SelectedValue Then
                coreFile = coreFile.Replace(".resx", "." & ddlLanguages.SelectedValue & ".Portal-" & PortalId & ".resx")
            Else
                coreFile = coreFile.Replace(".resx", ".Portal-" & PortalId & ".resx")
            End If

            SaveTranslation(coreFile, "Login.Text", tbLogin.Text)
        End Sub
    End Class
End Namespace
