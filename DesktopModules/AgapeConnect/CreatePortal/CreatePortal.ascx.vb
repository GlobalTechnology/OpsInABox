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
Imports DotNetNuke.Services.FileSystem
'Imports Resources


Namespace DotNetNuke.Modules.AgapeConnect
    Partial Class CreatePortal
        Inherits Entities.Modules.PortalModuleBase
       

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            
        End Sub

        
        Protected Sub btnCreate_Click(sender As Object, e As System.EventArgs) Handles btnCreate.Click
            'SetupIcons(48)
            'SetupRmb(48)
            'Return

            Dim NewPortalId = CreatePortal()
            ' Dim NewPortalId = 4
            If NewPortalId > 0 Then
                SetupAdminPages(NewPortalId)


                SetupTemplates(NewPortalId)
                SetupStaffProfileProperties(NewPortalId)
                SetupAgapeConnectSettings(NewPortalId)

                SetupIcons(NewPortalId)
                SetupRmb(NewPortalId)

               


                'Dim pc As New PortalController
                'Dim p = pc.GetPortal(NewPortalId)




                If cbGenerateUsers.Checked Then
                    createUserFromTnT()
                End If

            End If

        End Sub
      

        Private Function CreateUser(ByVal thePortalid As Integer, ByVal username As String, ByVal FirstName As String, ByVal LastName As String, ByVal StaffType As Integer) As StaffBroker.AP_StaffBroker_Staff
            lblStatus.Text &= "Creating User: " & FirstName & "<br />"

            Dim user = StaffBrokerFunctions.CreateUser(thePortalid, username, FirstName, LastName)

            Dim NewUser = UserController.GetUserByName(thePortalid, username & thePortalid)
            lblStatus.Text &= "Created UserId: " & NewUser.UserID & "<br />"
            Dim staff = StaffBrokerFunctions.CreateStaffMember(thePortalid, NewUser, StaffType)
            lblStatus.Text &= "Created Staff Member: " & staff.StaffId & "<br />"
            Return staff
        End Function

        Private Sub SetupRmb(ByVal thePortalId As Integer)

            Dim mc As New DotNetNuke.Entities.Modules.ModuleController

            Dim x = mc.GetModuleByDefinition(thePortalId, "acStaffRmb")
       


            Dim objModules As New Entities.Modules.ModuleController


            objModules.UpdateTabModuleSetting(x.TabModuleID, "NoReceipt", 5)
            objModules.UpdateTabModuleSetting(TabModuleId, "VatAttrib", False)
            objModules.UpdateTabModuleSetting(TabModuleId, "Expire", 3)
            objModules.UpdateTabModuleSetting(TabModuleId, "TeamLeaderLimit", 1000)

            objModules.UpdateTabModuleSetting(TabModuleId, "DistanceUnit", "km")

           
            objModules.UpdateTabModuleSetting(TabModuleId, "AccountsRoles", "Administrators;Accounts Team;")


            objModules.UpdateTabModuleSetting(TabModuleId, "AccountsEmail", "")
            objModules.UpdateTabModuleSetting(TabModuleId, "AccountsName", "")
            objModules.UpdateTabModuleSetting(TabModuleId, "DownloadFormat", "DCD")
          
            objModules.UpdateTabModuleSetting(TabModuleId, "Sub1Name", "Normal")
            objModules.UpdateTabModuleSetting(TabModuleId, "Sub2Name", "Overseas")
           
            objModules.UpdateTabModuleSetting(TabModuleId, "Sub1Value", 15)
            objModules.UpdateTabModuleSetting(TabModuleId, "Sub2Value", 20)
      


            objModules.UpdateTabModuleSetting(TabModuleId, "MRate1Name", "Car")
          

            objModules.UpdateTabModuleSetting(TabModuleId, "MRate1", 0.4)
      
            objModules.UpdateTabModuleSetting(TabModuleId, "EntBreakfast", 5)
            objModules.UpdateTabModuleSetting(TabModuleId, "EntLunch", 10)
            objModules.UpdateTabModuleSetting(TabModuleId, "EntDinner", 15)
            objModules.UpdateTabModuleSetting(TabModuleId, "EntOvernight", 5)
            objModules.UpdateTabModuleSetting(TabModuleId, "EntDay", 35)
            objModules.UpdateTabModuleSetting(TabModuleId, "MenuSize", 30)
            objModules.UpdateTabModuleSetting(TabModuleId, "UseDCode", True)
            objModules.UpdateTabModuleSetting(TabModuleId, "ElectronicReceipts", False)
            objModules.UpdateTabModuleSetting(TabModuleId, "ShowRemBal", False)
            objModules.UpdateTabModuleSetting(TabModuleId, "WarnIfNegative", False)
            StaffBrokerFunctions.SetSetting("RmbDownload", False, thePortalId)




            ' objModules.UpdateTabModuleSetting(TabModuleId, "CurConverter", cbCurConverter.Checked)
            StaffBrokerFunctions.SetSetting("CurConverter", False, thePortalId)


            objModules.UpdateTabModuleSetting(TabModuleId, "isLoaded", "Yes")
            Dim d As New StaffRmb.StaffRmbDataContext
            Dim all = From c In d.AP_Staff_RmbLineTypes Where c.SpareField1 = "True"


            For Each row In all
                Dim LineType As Integer = row.LineTypeId
                Dim q = From c In d.AP_StaffRmb_PortalLineTypes Where c.PortalId = thePortalId And c.LineTypeId = LineType

                If q.Count = 0 Then
                    Dim insert = New StaffRmb.AP_StaffRmb_PortalLineType
                    insert.LineTypeId = LineType
                    insert.LocalName = row.TypeName
                    insert.PCode = ""
                    insert.DCode = ""
                    insert.PortalId = thePortalId
                    d.AP_StaffRmb_PortalLineTypes.InsertOnSubmit(insert)


                End If

            Next

            d.SubmitChanges()
            StaffBrokerFunctions.SetSetting("RmbTabModuleId", x.TabModuleID, thePortalId)

            ' refresh cache
            SynchronizeModule()

        End Sub


        Private Function CreatePortal() As Integer
            Dim p As New PortalController
            '    Dim nextPId As Integer = CType(p.GetPortals(p.GetPortals.Count - 1), PortalInfo).PortalID + 1
            ' Dim AdminUsername = StripNumber(UserInfo.Username) & "[PORTALID]"

            lblStatus.Text = ""
            Dim rtn = -1

            If tbCountryISOCode.Text.Length = 0 Then
                lblStatus.Text &= "Country Code is required"
            End If
            Dim ac As New PortalAliasController()

            'Dim PortalAlias As String = "localhost:65323" 'tbCountryISOCode.Text & ".agapeconnect.me"
            Dim PortalAlias = tbCountryISOCode.Text & ".agapeconnect.me"
            Dim aliasCollection = ac.GetPortalAliases()
            If ac.GetPortalAliases().Contains(PortalAlias) Then
                lblStatus.Text &= "Country (ISO) code already exists"
            End If






            Dim newPid = p.CreatePortal("AgapeConnect - " & tbCountryName.Text, _
                            firstName:=tbCountryISOCode.Text, lastName:="Admin", _
                             username:=tbCountryISOCode.Text & "_Admin", _
                              password:=UserController.GeneratePassword(8),
                              email:="donotreply@agapeconnect.me",
                               description:="Agape Connect Site for " & tbCountryName.Text,
                               keyWords:="", templatePath:=HostMapPath, templateFile:="AgapeConnect2014.template",
                              homeDirectory:="", portalAlias:=PortalAlias,
           serverPath:=GetAbsoluteServerPath(Request), childPath:="", isChildPortal:=False)

            If newPid > 0 Then
                'Success
                rtn = newPid
                Dim defaultStaffType = SetupStaffTypes(newPid)

                'get the Admin User and change his username
                lblStatus.Text &= "Created Portal: " & newPid & vbNewLine
                Dim rc As New DotNetNuke.Security.Roles.RoleController
              
                        

                        Dim AdminRole = rc.GetRoleByName(newPid, "Administrators")
                        If Not AdminRole Is Nothing Then
                            lblStatus.Text &= "Added current user to role" & vbNewLine

                            Try
                        Dim Jon = CreateUser(newPid, "jon.vellacott@ccci.org", "Jon", "Vellacott", defaultStaffType)


                                rc.AddUserRole(newPid, Jon.UserId1, AdminRole.RoleID, Null.NullDate)
                                If (tbAdminEmail.Text <> "" And tbAdminFirstname.Text <> "" And tbAdminLastname.Text <> "") Then
                                    Dim Admin = CreateUser(newPid, tbAdminEmail.Text, tbAdminFirstname.Text, tbAdminLastname.Text, defaultStaffType)
                                    rc.AddUserRole(newPid, Admin.UserId1, AdminRole.RoleID, Null.NullDate)
                                    Dim d As New StaffBroker.StaffBrokerDataContext
                                    Dim insert As New StaffBroker.AP_StaffBroker_LeaderMeta
                                    insert.UserId = Jon.UserId1
                                    insert.LeaderId = Admin.UserId1
                                    d.AP_StaffBroker_LeaderMetas.InsertOnSubmit(insert)

                                    Dim insert2 As New StaffBroker.AP_StaffBroker_LeaderMeta
                                    insert2.UserId = Admin.UserId1
                                    insert.LeaderId = Jon.UserId1
                                    d.AP_StaffBroker_LeaderMetas.InsertOnSubmit(insert2)

                                    d.SubmitChanges()
                                End If
                            Catch ex As Exception
                                lblStatus.Text &= "Error Creating Users"
                            End Try


                        Else
                            lblStatus.Text &= "Can't Find admin Role" & vbNewLine
                        End If












            Else
                lblStatus.Text &= "Error Creating Portal" & vbNewLine
            End If
            Return rtn
        End Function

        Private Sub SetupTemplates(ByVal thePortalId As Integer)
            Dim d As New StaffBroker.TemplatesDataContext

            Dim q = From c In d.AP_StaffBroker_Templates Where c.PortalId Is Nothing   ' The Default temlates are stored here!
            Dim r = From c In d.AP_StaffBroker_Templates Where c.PortalId = thePortalId

            d.AP_StaffBroker_Templates.DeleteAllOnSubmit(r)
            d.SubmitChanges()
            For Each row In q
                Dim insert As New StaffBroker.AP_StaffBroker_Template
                insert.PortalId = thePortalId
                insert.TemplateDescription = row.TemplateDescription
                insert.TemplateHTML = row.TemplateHTML
                insert.TemplateName = row.TemplateName
                d.AP_StaffBroker_Templates.InsertOnSubmit(insert)

            Next
            d.SubmitChanges()

        End Sub

        Private Sub SetupStaffProfileProperties(ByVal thePortalId As Integer)
            Dim d As New StaffBroker.StaffBrokerDataContext

            Dim q = From c In d.AP_StaffBroker_StaffPropertyDefinitions Where c.PortalId Is Nothing And Not (c.FixedFieldName Is Nothing)   ' The Default temlates are stored here!
           
            Dim r = From c In d.AP_StaffBroker_StaffPropertyDefinitions Where c.PortalId = thePortalId

            d.AP_StaffBroker_StaffPropertyDefinitions.DeleteAllOnSubmit(r)
            d.SubmitChanges()
            For Each row In q
                Dim insert As New StaffBroker.AP_StaffBroker_StaffPropertyDefinition
                insert.PortalId = thePortalId
                insert.Display = row.Display
                insert.FixedFieldName = row.FixedFieldName
                insert.PropertyHelp = row.PropertyHelp
                insert.PropertyName = row.PropertyName
                insert.Type = row.Type
                insert.ViewOrder = row.ViewOrder



                d.AP_StaffBroker_StaffPropertyDefinitions.InsertOnSubmit(insert)

            Next
            d.SubmitChanges()

        End Sub
        Private Function SetupStaffTypes(ByVal thePortalId As Integer) As Integer
            Dim d As New StaffBroker.StaffBrokerDataContext

            Dim q = From c In d.AP_StaffBroker_StaffTypes Where c.PortalId Is Nothing    ' The Default temlates are stored here!
            Dim r = From c In d.AP_StaffBroker_StaffTypes Where c.PortalId = thePortalId

            d.AP_StaffBroker_StaffTypes.DeleteAllOnSubmit(r)
            d.SubmitChanges()
            For Each row In q
                Dim insert As New StaffBroker.AP_StaffBroker_StaffType
                insert.PortalId = thePortalId
                insert.Name = row.Name
                d.AP_StaffBroker_StaffTypes.InsertOnSubmit(insert)

            Next
            d.SubmitChanges()

            Return d.AP_StaffBroker_StaffTypes.Where(Function(c) c.PortalId = thePortalId).First.StaffTypeId



        End Function


        Private Sub SetupAgapeConnectSettings(ByVal thePortalId As Integer)
             StaffBrokerFunctions.SetSetting("Currency", tbCurrencySymbol.Text, thePortalId)
            StaffBrokerFunctions.SetSetting("Datapump", "Unlocked", thePortalId)
            StaffBrokerFunctions.SetSetting("NextRID", "1", thePortalId)
            StaffBrokerFunctions.SetSetting("NextAdvID", "1", thePortalId)
            StaffBrokerFunctions.SetSetting("tntFlag", "Clean", thePortalId)
            If tbAdvancesSuffix.Text <> "" Then
                StaffBrokerFunctions.SetSetting("AdvanceSuffix", tbAdvancesSuffix.Text, thePortalId)
            End If
            If tbDataServer.Text <> "" And tbDataServer.Text.EndsWith("/") Then
                StaffBrokerFunctions.SetSetting("DataserverURL", tbDataServer.Text, thePortalId)
            End If
            StaffBrokerFunctions.SetSetting("AccountingCurrency", ddlAccountingCurrency.SelectedValue, thePortalId)
            StaffBrokerFunctions.SetSetting("LocalCurrency", ddlLocalCurrency.SelectedValue, thePortalId)
            If tbCompany.Text <> "" Then
                StaffBrokerFunctions.SetSetting("CompanyName", tbCompany.Text, thePortalId)
            End If
            If ddlAccountingCurrency.SelectedValue = ddlLocalCurrency.SelectedValue Then
                StaffBrokerFunctions.SetSetting("CurConverter", "False", thePortalId)
            Else
                StaffBrokerFunctions.SetSetting("CurConverter", "True", thePortalId)
            End If
            StaffBrokerFunctions.SetSetting("tntWebLinkActive", "False", thePortalId)
            StaffBrokerFunctions.SetSetting("FirstFiscalMonth", "7", thePortalId)
            StaffBrokerFunctions.SetSetting("CurrentFiscalPeriod", Today.AddMonths(-7).ToString("yyyyMM"), thePortalId)
            StaffBrokerFunctions.SetSetting("NonDynamics", "False", thePortalId)
            StaffBrokerFunctions.SetSetting("Nagape", "ON", thePortalId)


            Dim d As New DatatSync
            Try
                d.GetPassword(thePortalId)
            Catch ex As Exception
                d.SetPassword(thePortalId)
            End Try


        End Sub

        Private Sub SetupAdminPages(ByVal thePortalId As Integer)
            Dim tc As New TabController
            Dim AgapeConnectTab = tc.GetTabByName("AgapeConnect", thePortalId)
            Dim AdminTab = tc.GetTabByName("Admin", thePortalId)
            If AgapeConnectTab Is Nothing Or AdminTab Is Nothing Then

            Else
                AgapeConnectTab.Level = 1
                AgapeConnectTab.ParentId = AdminTab.TabID
                
                tc.UpdateTab(AgapeConnectTab)
                Dim SubTabs = TabController.GetTabsByParent(AgapeConnectTab.TabID, thePortalId)

                For Each row In SubTabs
                    row.Level = 2
                    tc.UpdateTab(row)
                Next





            End If



        End Sub

       
        Private Sub SetupIcons(ByVal thePortalId As Integer)
            Dim d As New AgapeIconAdmin.AgapeIconsDataContext
            Dim Directory As New System.IO.DirectoryInfo(Server.MapPath("/DesktopModules/AgapeConnect/AgapeIconAdmin/images/"))
            If Not Directory Is Nothing Then

                Dim s = From c In d.Agape_Skin_AgapeIcons Where c.PortalId = thePortalId

                d.Agape_Skin_AgapeIcons.DeleteAllOnSubmit(s)
                d.SubmitChanges()

                Dim insertRmb As New AgapeIconAdmin.Agape_Skin_AgapeIcon
                Dim insertAccount As New AgapeIconAdmin.Agape_Skin_AgapeIcon
                For Each row In Directory.GetFiles("*.png")
                    Dim folder As IFolderInfo

                    If Not FolderManager.Instance.FolderExists(thePortalId, "acIcons") Then
                        folder = FolderManager.Instance.AddFolder(thePortalId, "acIcons")
                    Else
                        folder = FolderManager.Instance.GetFolder(thePortalId, "acIcons")
                    End If
                    Using FileStream As New System.IO.FileStream(row.FullName, FileMode.Open)
                        Dim theFile = FileManager.Instance.AddFile(folder, row.Name, FileStream, False, False, "image/png")
                        Select Case row.Name
                            Case "Rmb.png"
                                insertRmb.IconFile = theFile.FileId

                            Case "Rmb-2.png"
                                insertRmb.HovrIconFile = theFile.FileId
                            Case "ViewAccount.png"
                                insertAccount.IconFile = theFile.FileId
                            Case "ViewAccount2.png"
                                insertAccount.HovrIconFile = theFile.FileId
                        End Select


                    End Using
                   
                Next
                Dim mc As New DotNetNuke.Entities.Modules.ModuleController
                Dim x = mc.GetModuleByDefinition(PortalId, "acStaffRmb")
                If Not x Is Nothing Then ' The Module exists
                    insertRmb.LinkLoc = x.TabID
                End If
                x = mc.GetModuleByDefinition(PortalId, "acAccounts")
                If Not x Is Nothing Then ' The Module exists
                    insertAccount.LinkLoc = x.TabID
                End If

                insertRmb.LinkType = "T"
                insertRmb.Title = "Expenses"
                insertRmb.ViewOrder = 0
                insertRmb.PortalId = thePortalId


                insertAccount.LinkType = "T"
                insertAccount.Title = "Accounts"
                insertAccount.ViewOrder = 1
                insertAccount.PortalId = thePortalId
                d.Agape_Skin_AgapeIcons.InsertOnSubmit(insertRmb)
                d.Agape_Skin_AgapeIcons.InsertOnSubmit(insertAccount)
                d.SubmitChanges()
            End If


            Dim r = From c In d.Agape_Skin_IconSettings Where c.PortalId = thePortalId

            d.Agape_Skin_IconSettings.DeleteAllOnSubmit(r)
            d.SubmitChanges()
            Dim insert As New AgapeIconAdmin.Agape_Skin_IconSetting
            insert.PortalId = thePortalId
            insert.IconHeight = 80
            insert.ShowTitles = True
            insert.Padding = 5
            d.Agape_Skin_IconSettings.InsertOnSubmit(insert)

            d.SubmitChanges()

        End Sub
        

        Function StripNumber(stdText As String) As String
            Dim str As String = "", b As Integer = 1
            'strips the number from a longer text string
            stdText = Trim(stdText)

            For i As Integer = Len(stdText) To 1 Step -1
                If IsNumeric(Mid(stdText, i, 1)) Then
                    str = Left(stdText, Len(stdText) - b)
                    b = b + 1
                Else
                    Exit For

                End If
            Next i

            Return Trim(str)
        End Function

        Protected Sub btnTest_Click(sender As Object, e As System.EventArgs) Handles btnTest.Click
            'Dim d As New StaffBroker.StaffBrokerDataContext

            'Dim staff = From c In d.AP_StaffBroker_Staffs Where c.PortalId = 2 And c.Active
            'Dim objPortals As New PortalController()
            'Dim objPortal As PortalInfo = objPortals.GetPortal(PortalId)
            'For Each row In staff
            '    'Get Users

            '    Dim user1 = UserController.GetUserById(2, row.UserId1)
            '    Dim newUser1 = user1
            '    newUser1.PortalID = 4
            '    newUser1.Username = user1.Username.TrimEnd("2") & "4"
            '    UserController.CreateUser(newUser1)


            '    If row.UserId2 > 0 Then
            '        Dim user2 = UserController.GetUserById(2, row.UserId2)
            '        Dim newUser2 = user1
            '        newUser2.PortalID = 4
            '        newUser2.Username = user1.Username.TrimEnd("2") & "4"
            '        UserController.CreateUser(newUser2)

            '    End If



            '    'Create Users and Roles

            '    'Create Staff 

            '    'Create Children

            '    'Create StaffProfilePropertyValues

            '    'Create UserProfilePropertyValues

            'Next



            ''Copy Leadership relationships
            ''Set Dept Managers

        End Sub



        Protected Sub btnFCX_Click(sender As Object, e As System.EventArgs) Handles btnFCX.Click
            'Dim objFCX As New FCX_API
            'Dim myAPIKey As New Guid("36bf6bfb-f152-4a5d-ac6b-4ee00cbea6ea")
            'Dim transactions() = New FCX_API.FinanctialTransaction(0) {}
            'Dim insert As New FCX_API.FinanctialTransaction
            'insert.Account = "4010"
            'insert.Amount = "100.00"
            'insert.Description = "Test Donation"
            'insert.RC = "VJ12"
            'insert.TransactionId = "1"
            'insert.TrxDate = Now
            'transactions(0) = insert


            'Dim resp = objFCX.AddFinanicialTransactions(myAPIKey, "4", "ACTUAL", "This is a test", transactions)
            'lblStatus.Text = resp.Status & ": " & resp.Message



        End Sub


        Private Sub createUserFromTnT()
            Dim tnt As New tntWebUsers()
            tnt.CreateUsersFromTnt()
        End Sub

        Protected Sub btnCreateAllUsers_Click(sender As Object, e As System.EventArgs) Handles btnCreateAllUsers.Click
            createUserFromTnT()

        End Sub

        Protected Sub btnproxy_Click(sender As Object, e As System.EventArgs) Handles btnproxy.Click

        End Sub

        Protected Sub btnCreateEntropy_Click(sender As Object, e As System.EventArgs) Handles btnCreateEntropy.Click
            StaffBrokerFunctions.SetSetting("TestUserPassword", "Thisisatestaccount", 0)

        End Sub
    End Class
End Namespace
