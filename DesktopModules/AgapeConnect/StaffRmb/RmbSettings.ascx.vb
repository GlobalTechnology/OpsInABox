Imports DotNetNuke
Imports System.Web.UI
Imports System.Linq

Imports StaffRmb



Namespace DotNetNuke.Modules.StaffRmb

    Partial Class RmbSettings
        Inherits Entities.Modules.ModuleSettingsBase

#Region "Base Method Implementations"
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
            Try


                If (Page.IsPostBack = False) Then
                    hfPortalId.Value = PortalId


                    Dim d As New StaffBroker.StaffBrokerDataContext


                    Dim stafflist = (From c In d.AP_StaffBroker_Staffs Where c.PortalId = PortalId And c.Active Select UID = c.UserId1, DisplayName = c.DisplayName).AsEnumerable
                    ' Dim dr As New StaffRmbDataContext

                    'cblExpenseTypes.DataSource = From c In dr.AP_Staff_RmbLineTypes Order By c.ViewOrder

                    'cblExpenseTypes.DataTextField = "TypeName"
                    'cblExpenseTypes.DataValueField = "LineTypeId"
                    'cblExpenseTypes.DataBind()

                    cbNagape.Checked = (StaffBrokerFunctions.GetSetting("Nagape", PortalId) = "ON")

                    stafflist = stafflist.Union(From c In d.AP_StaffBroker_Staffs Where c.UserId2 > 0 And c.PortalId = PortalId And c.Active Select UID = CInt(c.UserId2), DisplayName = c.DisplayName)

                    ddlAuthUser.Items.Clear()
                    ddlAuthAuthUser.Items.Clear()



                    For Each row In (From c In stafflist Order By c.DisplayName)
                        Try
                            Dim user = UserController.GetUserById(PortalId, row.UID).LastName & ", " & UserController.GetUserById(PortalId, row.UID).FirstName
                            ddlAuthUser.Items.Add(New ListItem(user, row.UID))
                            If CType(TabModuleSettings("AuthUser"), String) <> "" Then
                                ddlAuthUser.SelectedValue = CType(TabModuleSettings("AuthUser"), Integer)
                            End If
                            ddlAuthAuthUser.Items.Add(New ListItem(user, row.UID))
                            If CType(TabModuleSettings("AuthAuthUser"), String) <> "" Then
                                ddlAuthAuthUser.SelectedValue = CType(TabModuleSettings("AuthAuthUser"), Integer)
                            End If
                        Catch ex As Exception

                        End Try
                    Next



                    If CType(TabModuleSettings("Reminder1"), String) <> "" Then
                        tbReminder1.Text = CType(TabModuleSettings("Reminder1"), String)
                    End If
                    If CType(TabModuleSettings("Reminder2"), String) <> "" Then
                        tbReminder2.Text = CType(TabModuleSettings("Reminder2"), String)
                    End If
                    If CType(TabModuleSettings("GiveUp"), String) <> "" Then
                        tbGiveUp.Text = CType(TabModuleSettings("GiveUp"), String)
                    End If

                    If CType(TabModuleSettings("NoReceipt"), String) <> "" Then
                        tbNoReceipt.Text = CType(TabModuleSettings("NoReceipt"), String)
                    End If
                    If CType(TabModuleSettings("ElectronicReceipts"), String) <> "" Then
                        cbElectronicReceipts.Checked = CType(TabModuleSettings("ElectronicReceipts"), Boolean)
                    End If
                    If CType(TabModuleSettings("VatAttrib"), String) <> "" Then
                        cbVAT.Checked = CType(TabModuleSettings("VatAttrib"), Boolean)
                    End If
                    If CType(TabModuleSettings("Expire"), String) <> "" Then
                        tbExpire.Text = CType(TabModuleSettings("Expire"), String)
                    End If
                    If CType(TabModuleSettings("TeamLeaderLimit"), String) <> "" Then
                        tbTeamLeaderLimit.Text = CType(TabModuleSettings("TeamLeaderLimit"), String)
                    End If
                    If CType(TabModuleSettings("DescriptionLength"), String) <> "" Then
                        tbDescriptionLength.Text = CType(TabModuleSettings("DescriptionLength"), String)
                    End If
                    If CType(TabModuleSettings("MRate1"), String) <> "" Then
                        tbMRate1.Text = CType(TabModuleSettings("MRate1"), String)
                    End If
                    'If CType(TabModuleSettings("MRate2"), String) <> "" Then
                    '    tbMRate2.Text = CType(TabModuleSettings("MRate2"), String)
                    'End If
                    'If CType(TabModuleSettings("MThreshold"), String) <> "" Then
                    '    tbMThreshold.Text = CType(TabModuleSettings("MThreshold"), String)
                    'End If
                    'If CType(TabModuleSettings("AddPass"), String) <> "" Then
                    '    tbAddPass.Text = CType(TabModuleSettings("AddPass"), String)
                    'End If
                    'If CType(TabModuleSettings("Motorcycle"), String) <> "" Then
                    '    tbMotorcycle.Text = CType(TabModuleSettings("Motorcycle"), String)
                    'End If
                    'If CType(TabModuleSettings("Bicycle"), String) <> "" Then
                    '    tbBicycle.Text = CType(TabModuleSettings("Bicycle"), String)
                    'End If
                    If CType(TabModuleSettings("DistanceUnit"), String) <> "" Then
                        ddlDistance.SelectedValue = CType(TabModuleSettings("DistanceUnit"), String)
                    End If

                    Dim selectedRoles As New ArrayList

                    If CType(TabModuleSettings("AccountsRoles"), String) <> "" Then


                        'rsgAccountsRoles.SelectedRoleNames.Clear()
                        For Each role In CStr(TabModuleSettings("AccountsRoles")).Split(";")
                            ' rsgAccountsRoles.SelectedRoleNames.Add(role)
                            selectedRoles.Add(role)

                        Next

                    End If
                    rsgAccountsRoles.SelectedRoleNames = selectedRoles

                    If CType(TabModuleSettings("AccountsEmail"), String) <> "" Then
                        tbAccountsEmail.Text = CType(TabModuleSettings("AccountsEmail"), String)
                    End If
                    If CType(TabModuleSettings("AccountsName"), String) <> "" Then
                        tbAccountsName.Text = CType(TabModuleSettings("AccountsName"), String)
                    End If
                    If CType(TabModuleSettings("DownloadFormat"), String) <> "" Then
                        ddlDownloadFormat.SelectedValue = CType(TabModuleSettings("DownloadFormat"), String)
                    End If


                    If CType(TabModuleSettings("MRate1Name"), String) <> "" Then
                        tbMRate1Name.Text = CType(TabModuleSettings("MRate1Name"), String)
                    End If
                    If CType(TabModuleSettings("MRate2Name"), String) <> "" Then
                        tbMRate2Name.Text = CType(TabModuleSettings("MRate2Name"), String)
                    End If
                    If CType(TabModuleSettings("MRate3Name"), String) <> "" Then
                        tbMRate3Name.Text = CType(TabModuleSettings("MRate3Name"), String)
                    End If
                    If CType(TabModuleSettings("MRate4Name"), String) <> "" Then
                        tbMRate4Name.Text = CType(TabModuleSettings("MRate4Name"), String)
                    End If

                    If CType(TabModuleSettings("MRate1"), String) <> "" Then
                        tbMRate1.Text = CType(TabModuleSettings("MRate1"), String)
                    End If
                    If CType(TabModuleSettings("MRate2"), String) <> "" Then
                        tbMRate2.Text = CType(TabModuleSettings("MRate2"), String)
                    End If
                    If CType(TabModuleSettings("MRate3"), String) <> "" Then
                        tbMRate3.Text = CType(TabModuleSettings("MRate3"), String)
                    End If
                    If CType(TabModuleSettings("MRate4"), String) <> "" Then
                        tbMRate4.Text = CType(TabModuleSettings("MRate4"), String)
                    End If


                    If CType(TabModuleSettings("Sub1Name"), String) <> "" Then
                        tbPD1Name.Text = CType(TabModuleSettings("Sub1Name"), String)
                    End If
                    If CType(TabModuleSettings("Sub2Name"), String) <> "" Then
                        tbPD2Name.Text = CType(TabModuleSettings("Sub2Name"), String)
                    End If
                    If CType(TabModuleSettings("Sub3Name"), String) <> "" Then
                        tbPD3Name.Text = CType(TabModuleSettings("Sub3Name"), String)
                    End If
                    If CType(TabModuleSettings("Sub4Name"), String) <> "" Then
                        tbPD4Name.Text = CType(TabModuleSettings("Sub4Name"), String)
                    End If
                    If CType(TabModuleSettings("Sub5Name"), String) <> "" Then
                        tbPD5Name.Text = CType(TabModuleSettings("Sub5Name"), String)
                    End If
                    If CType(TabModuleSettings("Sub6Name"), String) <> "" Then
                        tbPD6Name.Text = CType(TabModuleSettings("Sub6Name"), String)
                    End If
                    If CType(TabModuleSettings("Sub1Value"), String) <> "" Then
                        tbPD1Value.Text = CType(TabModuleSettings("Sub1Value"), String)
                    End If
                    If CType(TabModuleSettings("Sub2Value"), String) <> "" Then
                        tbPD2Value.Text = CType(TabModuleSettings("Sub2Value"), String)
                    End If
                    If CType(TabModuleSettings("Sub3Value"), String) <> "" Then
                        tbPD3Value.Text = CType(TabModuleSettings("Sub3Value"), String)
                    End If
                    If CType(TabModuleSettings("Sub4Value"), String) <> "" Then
                        tbPD4Value.Text = CType(TabModuleSettings("Sub4Value"), String)
                    End If
                    If CType(TabModuleSettings("Sub5Value"), String) <> "" Then
                        tbPD5Value.Text = CType(TabModuleSettings("Sub5Value"), String)
                    End If
                    If CType(TabModuleSettings("Sub6Value"), String) <> "" Then
                        tbPD6Value.Text = CType(TabModuleSettings("Sub6Value"), String)
                    End If


                    If CType(TabModuleSettings("EntBreakfast"), String) <> "" Then
                        tbEntBreakfast.Text = CType(TabModuleSettings("EntBreakfast"), String)
                    End If
                    If CType(TabModuleSettings("EntLunch"), String) <> "" Then
                        tbEntLunch.Text = CType(TabModuleSettings("EntLunch"), String)
                    End If
                    If CType(TabModuleSettings("EntDinner"), String) <> "" Then
                        tbEntDinner.Text = CType(TabModuleSettings("EntDinner"), String)
                    End If
                    If CType(TabModuleSettings("EntOvernight"), String) <> "" Then
                        tbEntOvernight.Text = CType(TabModuleSettings("EntOvernight"), String)
                    End If
                    If CType(TabModuleSettings("EntDay"), String) <> "" Then
                        tbEntDay.Text = CType(TabModuleSettings("EntDay"), String)
                    End If
                    If CType(TabModuleSettings("MenuSize"), String) <> "" Then
                        tbMenuSize.Text = CType(TabModuleSettings("MenuSize"), String)
                    End If
                    'If CType(TabModuleSettings("UseDCode"), String) <> "" Then
                    '    cbUseDCode.Checked = CType(TabModuleSettings("UseDCode"), Boolean)
                    'End If

                    If CType(TabModuleSettings("ShowRemBal"), String) <> "" Then
                        cbRemBal.Checked = CType(TabModuleSettings("ShowRemBal"), Boolean)
                    End If

                    If CType(TabModuleSettings("WarnIfNegative"), String) <> "" Then
                        cbWarnIfNegative.Checked = CType(TabModuleSettings("WarnIfNegative"), Boolean)
                    End If

                    'If CType(TabModuleSettings("CurConverter"), String) <> "" Then
                    '    cbCurConverter.Checked = CType(TabModuleSettings("CurConverter"), Boolean)
                    'End If
                    cbCurConverter.Checked = StaffBrokerFunctions.GetSetting("CurConverter", PortalId) = "True"
                    cbDatapump.Checked = StaffBrokerFunctions.GetSetting("RmbDownload", PortalId) <> "False"
                    pnlSingle.Visible = Not cbDatapump.Checked
                    If StaffBrokerFunctions.GetSetting("RmbSinglePump", PortalId) <> "True" Then
                        btnDownload.Enabled = Not cbDatapump.Checked
                        btnDownload.Text = "Download"
                        lblDownloading.Visible = False
                    Else
                        btnDownload.Enabled = False
                        btnDownload.Text = "Downloading"
                        lblDownloading.Visible = True
                    End If

                    If StaffBrokerFunctions.GetSetting("NonDynamics", PortalId) <> "True" Then

                        ddlAccountsPayable.DataBind()
                        ddlAccountsReceivable.DataBind()
                        ddlTaxAccountsReceivable.DataBind()
                        ddlControlAccount.DataBind()
                        ddlPayrollPayable.DataBind()
                        ddlSalaryAccount.DataBind()
                        ddlBankAccount.DataBind()
                        ddlHoldingAccount.DataBind()
                        'Control Account
                        If ddlControlAccount.Items.Count = 0 Then
                            oopsControlAccount.Text = "Oops! - There are no Responsibility Centers setup. Please ensure that you have setup the datapump on the same server as your accounts package - as this will automatically upload a list of cost centers from your accounts package."
                        ElseIf ddlControlAccount.Items.FindByValue(CType(TabModuleSettings("ControlAccount"), String)) Is Nothing Then
                            oopsControlAccount.Text = "Oops! Account " & TabModuleSettings("ControlAccount") & " does not appear in your Responsibility Centers."
                        Else
                            ddlControlAccount.SelectedValue = CType(TabModuleSettings("ControlAccount"), String)
                            oopsControlAccount.Text = ""

                        End If


                        'Advance Account
                        If ddlAccountsReceivable.Items.Count = 0 Then
                            oopsAccountsReceivable.Text = "Oops! - you have no Asset Accounts in your accounts package"
                        ElseIf ddlAccountsReceivable.Items.FindByValue(CType(TabModuleSettings("AccountsReceivable"), String)) Is Nothing Then
                            oopsAccountsReceivable.Text = "Oops! Account " & TabModuleSettings("AccountsReceivable") & " does not appear in your Assests Accounts."
                        Else
                            ddlAccountsReceivable.SelectedValue = CType(TabModuleSettings("AccountsReceivable"), String)
                            oopsAccountsReceivable.Text = ""
                        End If


                        'Taxable Loan Account
                        If ddlTaxAccountsReceivable.Items.Count = 0 Then
                            oopsTaxAccountsReceivable.Text = "Oops! - you have no Asset Accounts in your accounts package"
                        ElseIf ddlTaxAccountsReceivable.Items.FindByValue(CType(TabModuleSettings("TaxAccountsReceivable"), String)) Is Nothing Then
                            oopsTaxAccountsReceivable.Text = "Oops! Account " & TabModuleSettings("TaxAccountsReceivable") & " does not appear in your Assests Accounts."
                        Else
                            ddlTaxAccountsReceivable.SelectedValue = CType(TabModuleSettings("TaxAccountsReceivable"), String)
                            oopsTaxAccountsReceivable.Text = ""
                        End If




                        'Accounts Payable
                        If ddlAccountsPayable.Items.Count = 0 Then
                            oopsAccountsPayable.Text = "Oops! - you have no Liability Accounts in your accounts package"
                        ElseIf ddlAccountsPayable.Items.FindByValue(CType(TabModuleSettings("AccountsPayable"), String)) Is Nothing Then
                            oopsAccountsPayable.Text = "Oops! Account " & TabModuleSettings("AccountsPayable") & " does not appear to exist in your Liability Accounts."
                        Else
                            ddlAccountsPayable.SelectedValue = CType(TabModuleSettings("AccountsPayable"), String)
                            oopsAccountsPayable.Text = ""
                        End If

                        'Payroll Payable
                        If ddlPayrollPayable.Items.Count = 0 Then
                            lblOopsPayroll.Text = "Oops! - you have no Liability Accounts in your accounts package"
                        ElseIf ddlPayrollPayable.Items.FindByValue(CType(TabModuleSettings("PayrollPayable"), String)) Is Nothing Then
                            lblOopsPayroll.Text = "Oops! Account " & TabModuleSettings("PayrollPayable") & " does not appear to exist in your Liability Accounts."
                        Else
                            ddlPayrollPayable.SelectedValue = CType(TabModuleSettings("PayrollPayable"), String)
                            lblOopsPayroll.Text = ""
                        End If


                        'Salary Account
                        If ddlSalaryAccount.Items.Count = 0 Then
                            lblOopsSalary.Text = "Oops! - you have no Expense Accounts in your accounts package"
                        ElseIf ddlSalaryAccount.Items.FindByValue(CType(TabModuleSettings("SalaryAccount"), String)) Is Nothing Then
                            lblOopsSalary.Text = "Oops! Account " & TabModuleSettings("SalaryAccount") & " does not appear to exist in your Expense Accounts."
                        Else
                            ddlSalaryAccount.SelectedValue = CType(TabModuleSettings("SalaryAccount"), String)
                            lblOopsSalary.Text = ""
                        End If
                        'Bank Account
                        If ddlBankAccount.Items.Count = 0 Then
                            lblOopsBank.Text = "Oops! - you have no Asset Accounts in your accounts package"
                        ElseIf ddlBankAccount.Items.FindByValue(CType(TabModuleSettings("BankAccount"), String)) Is Nothing Then
                            lblOopsBank.Text = "Oops! Account " & TabModuleSettings("BankAccount") & " does not appear to exist in your Asset Accounts."
                        Else
                            ddlBankAccount.SelectedValue = CType(TabModuleSettings("BankAccount"), String)
                            lblOopsBank.Text = ""
                        End If

                        'Holding Account
                        If ddlHoldingAccount.Items.Count = 0 Then
                            lblOopsHoldingAccount.Text = "Oops! - you have no Asset Accounts in your accounts package"
                        ElseIf ddlHoldingAccount.Items.FindByValue(CType(TabModuleSettings("HoldingAccount"), String)) Is Nothing Then
                            ddlHoldingAccount.SelectedIndex = 0
                        Else
                            ddlHoldingAccount.SelectedValue = CType(TabModuleSettings("HoldingAccount"), String)
                            lblOopsHoldingAccount.Text = ""
                        End If
                    Else
                        tbControlAccount.Text = CType(TabModuleSettings("ControlAccount"), String)
                        tbAccountsReceivable.Text = CType(TabModuleSettings("AccountsReceivable"), String)
                        tbTaxAccountsReceivable.Text = CType(TabModuleSettings("TaxAccountsReceivable"), String)
                        tbAccountsPayable.Text = CType(TabModuleSettings("AccountsPayable"), String)
                        tbPayrollPayable.Text = CType(TabModuleSettings("PayrollPayable"), String)
                        tbSalaryAccount.Text = CType(TabModuleSettings("SalaryAccount"), String)
                        tbBankAccount.Text = CType(TabModuleSettings("BankAccount"), String)
                        tbHoldingAccount.Text = CType(TabModuleSettings("HoldingAccount"), String)
                        tbControlAccount.Visible = True
                        tbAccountsReceivable.Visible = True
                        tbTaxAccountsReceivable.Visible = True
                        tbAccountsPayable.Visible = True
                        tbPayrollPayable.Visible = True
                        tbSalaryAccount.Visible = True
                        tbBankAccount.Visible = True
                        ddlAccountsPayable.Visible = False
                        ddlAccountsReceivable.Visible = False
                        ddlTaxAccountsReceivable.Visible = False
                        ddlControlAccount.Visible = False
                        ddlPayrollPayable.Visible = False
                        ddlSalaryAccount.Visible = False
                        ddlBankAccount.Visible = False
                        pnlDatapump.Visible = False
                    End If
                End If

                '  GridView1.Columns(4).Visible = cbUseDCode.Checked




            Catch exc As Exception           'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try



        End Sub


        'Public Overrides Sub UpdateSettings()


        'End Sub
#End Region
        Private Function SetIfNumber(ByVal Text As String) As String
            Try
                Return CDbl(Text)
            Catch ex As Exception
                Return ""
            End Try
        End Function

        Protected Sub SaveBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SaveBtn.Click
            Dim objModules As New Entities.Modules.ModuleController


            objModules.UpdateTabModuleSetting(TabModuleId, "NoReceipt", tbNoReceipt.Text)

            objModules.UpdateTabModuleSetting(TabModuleId, "ElectronicReceipts", cbElectronicReceipts.Checked)
            objModules.UpdateTabModuleSetting(TabModuleId, "VatAttrib", cbVAT.Checked)
            objModules.UpdateTabModuleSetting(TabModuleId, "Expire", tbExpire.Text)
            objModules.UpdateTabModuleSetting(TabModuleId, "TeamLeaderLimit", tbTeamLeaderLimit.Text)
            objModules.UpdateTabModuleSetting(TabModuleId, "DescriptionLength", tbDescriptionLength.Text)
            'objModules.UpdateTabModuleSetting(TabModuleId, "MRate1", tbMRate1.Text)
            'objModules.UpdateTabModuleSetting(TabModuleId, "MRate2", tbMRate2.Text)
            'objModules.UpdateTabModuleSetting(TabModuleId, "MThreshold", tbMThreshold.Text)
            'objModules.UpdateTabModuleSetting(TabModuleId, "AddPass", tbAddPass.Text)
            'objModules.UpdateTabModuleSetting(TabModuleId, "Motorcycle", tbMotorcycle.Text)
            'objModules.UpdateTabModuleSetting(TabModuleId, "Bicycle", tbBicycle.Text)
            objModules.UpdateTabModuleSetting(TabModuleId, "DistanceUnit", ddlDistance.SelectedValue)

            Dim roles As String = ""
            For Each role As String In rsgAccountsRoles.SelectedRoleNames
                roles &= role & ";"
            Next
            objModules.UpdateTabModuleSetting(TabModuleId, "AccountsRoles", roles)


            objModules.UpdateTabModuleSetting(TabModuleId, "AccountsEmail", tbAccountsEmail.Text)
            objModules.UpdateTabModuleSetting(TabModuleId, "AccountsName", tbAccountsName.Text)
            objModules.UpdateTabModuleSetting(TabModuleId, "DownloadFormat", ddlDownloadFormat.SelectedValue)
            objModules.UpdateTabModuleSetting(TabModuleId, "AuthUser", ddlAuthUser.SelectedValue)
            objModules.UpdateTabModuleSetting(TabModuleId, "AuthAuthUser", ddlAuthAuthUser.SelectedValue)

            objModules.UpdateTabModuleSetting(TabModuleId, "Sub1Name", tbPD1Name.Text)
            objModules.UpdateTabModuleSetting(TabModuleId, "Sub2Name", tbPD2Name.Text)
            objModules.UpdateTabModuleSetting(TabModuleId, "Sub3Name", tbPD3Name.Text)
            objModules.UpdateTabModuleSetting(TabModuleId, "Sub4Name", tbPD4Name.Text)
            objModules.UpdateTabModuleSetting(TabModuleId, "Sub5Name", tbPD5Name.Text)
            objModules.UpdateTabModuleSetting(TabModuleId, "Sub6Name", tbPD6Name.Text)
            objModules.UpdateTabModuleSetting(TabModuleId, "Sub1Value", SetIfNumber(tbPD1Value.Text))
            objModules.UpdateTabModuleSetting(TabModuleId, "Sub2Value", SetIfNumber(tbPD2Value.Text))
            objModules.UpdateTabModuleSetting(TabModuleId, "Sub3Value", SetIfNumber(tbPD3Value.Text))
            objModules.UpdateTabModuleSetting(TabModuleId, "Sub4Value", SetIfNumber(tbPD4Value.Text))
            objModules.UpdateTabModuleSetting(TabModuleId, "Sub5Value", SetIfNumber(tbPD5Value.Text))
            objModules.UpdateTabModuleSetting(TabModuleId, "Sub6Value", SetIfNumber(tbPD6Value.Text))


            objModules.UpdateTabModuleSetting(TabModuleId, "MRate1Name", tbMRate1Name.Text)
            objModules.UpdateTabModuleSetting(TabModuleId, "MRate2Name", tbMRate2Name.Text)
            objModules.UpdateTabModuleSetting(TabModuleId, "MRate3Name", tbMRate3Name.Text)
            objModules.UpdateTabModuleSetting(TabModuleId, "MRate4Name", tbMRate4Name.Text)

            objModules.UpdateTabModuleSetting(TabModuleId, "MRate1", SetIfNumber(tbMRate1.Text))
            objModules.UpdateTabModuleSetting(TabModuleId, "MRate2", SetIfNumber(tbMRate2.Text))
            objModules.UpdateTabModuleSetting(TabModuleId, "MRate3", SetIfNumber(tbMRate3.Text))
            objModules.UpdateTabModuleSetting(TabModuleId, "MRate4", SetIfNumber(tbMRate4.Text))

            objModules.UpdateTabModuleSetting(TabModuleId, "EntBreakfast", tbEntBreakfast.Text)
            objModules.UpdateTabModuleSetting(TabModuleId, "EntLunch", tbEntLunch.Text)
            objModules.UpdateTabModuleSetting(TabModuleId, "EntDinner", tbEntDinner.Text)
            objModules.UpdateTabModuleSetting(TabModuleId, "EntOvernight", tbEntOvernight.Text)
            objModules.UpdateTabModuleSetting(TabModuleId, "EntDay", tbEntDay.Text)
            objModules.UpdateTabModuleSetting(TabModuleId, "MenuSize", tbMenuSize.Text)
            '  objModules.UpdateTabModuleSetting(TabModuleId, "UseDCode", cbUseDCode.Checked)


            If StaffBrokerFunctions.GetSetting("NonDynamics", PortalId) = "True" Then
                objModules.UpdateTabModuleSetting(TabModuleId, "ControlAccount", tbControlAccount.Text)
                objModules.UpdateTabModuleSetting(TabModuleId, "AccountsReceivable", tbAccountsReceivable.Text)

                objModules.UpdateTabModuleSetting(TabModuleId, "TaxAccountsReceivable", tbTaxAccountsReceivable.Text)
                objModules.UpdateTabModuleSetting(TabModuleId, "AccountsPayable", tbAccountsPayable.Text)
                objModules.UpdateTabModuleSetting(TabModuleId, "PayrollPayable", tbPayrollPayable.Text)
                objModules.UpdateTabModuleSetting(TabModuleId, "SalaryAccount", tbSalaryAccount.Text)
                objModules.UpdateTabModuleSetting(TabModuleId, "BankAccount", tbBankAccount.Text)
                objModules.UpdateTabModuleSetting(TabModuleId, "HoldingAccount", tbHoldingAccount.Text)
            Else
                objModules.UpdateTabModuleSetting(TabModuleId, "ControlAccount", ddlControlAccount.SelectedValue)
                objModules.UpdateTabModuleSetting(TabModuleId, "AccountsReceivable", ddlAccountsReceivable.SelectedValue)

                objModules.UpdateTabModuleSetting(TabModuleId, "TaxAccountsReceivable", ddlTaxAccountsReceivable.SelectedValue)
                objModules.UpdateTabModuleSetting(TabModuleId, "AccountsPayable", ddlAccountsPayable.SelectedValue)
                objModules.UpdateTabModuleSetting(TabModuleId, "PayrollPayable", ddlPayrollPayable.SelectedValue)
                objModules.UpdateTabModuleSetting(TabModuleId, "SalaryAccount", ddlSalaryAccount.SelectedValue)
                objModules.UpdateTabModuleSetting(TabModuleId, "BankAccount", ddlBankAccount.SelectedValue)


                objModules.UpdateTabModuleSetting(TabModuleId, "HoldingAccount", ddlHoldingAccount.SelectedValue)


            End If


            ' objModules.UpdateTabModuleSetting(TabModuleId, "CurConverter", cbCurConverter.Checked)
            StaffBrokerFunctions.SetSetting("CurConverter", cbCurConverter.Checked, PortalId)

            objModules.UpdateTabModuleSetting(TabModuleId, "ShowRemBal", cbRemBal.Checked)
            objModules.UpdateTabModuleSetting(TabModuleId, "WarnIfNegative", cbWarnIfNegative.Checked)

            StaffBrokerFunctions.SetSetting("RmbDownload", cbDatapump.Checked, PortalId)



            objModules.UpdateTabModuleSetting(TabModuleId, "isLoaded", "Yes")
            Dim d As New StaffRmbDataContext


            For Each row As GridViewRow In GridView1.Rows
                Dim LineType As Integer = CType(row.FindControl("hfLineTypeId"), HiddenField).Value
                Dim q = From c In d.AP_StaffRmb_PortalLineTypes Where c.PortalId = PortalId And c.LineTypeId = LineType

                If CType(row.FindControl("cbEnable"), CheckBox).Checked Then
                    Dim DisplayName = CType(row.FindControl("tbDisplayName"), TextBox).Text
                    Dim TypeName = CType(row.FindControl("hfTypeName"), HiddenField).Value
                    Dim PCode As String = ""
                    Dim DCode As String = ""


                    If StaffBrokerFunctions.GetSetting("NonDynamics", PortalId) = "True" Then
                        PCode = CType(row.FindControl("tbPCode"), TextBox).Text
                        DCode = CType(row.FindControl("tbDCode"), TextBox).Text
                    Else
                        PCode = CType(row.FindControl("ddlPCode"), DropDownList).SelectedValue
                        DCode = CType(row.FindControl("ddlDCode"), DropDownList).SelectedValue
                    End If
                    If (TypeName = "Non-Donation Income") Then
                        PCode = DCode
                    ElseIf (TypeName = "Donation Income") Then
                        PCode = "N/A"
                        DCode = "N/A"
                    End If

                    If q.Count = 0 Then
                        Dim insert = New AP_StaffRmb_PortalLineType
                        insert.LineTypeId = LineType
                        insert.LocalName = DisplayName
                        insert.PCode = PCode
                        insert.DCode = DCode
                        insert.PortalId = PortalId
                        d.AP_StaffRmb_PortalLineTypes.InsertOnSubmit(insert)
                    Else
                        q.First.LocalName = DisplayName
                        q.First.PCode = PCode
                        q.First.DCode = DCode
                    End If
                Else

                    d.AP_StaffRmb_PortalLineTypes.DeleteAllOnSubmit(q)
                End If
            Next

            d.SubmitChanges()
            StaffBrokerFunctions.SetSetting("RmbTabModuleId", TabModuleId, PortalId)
            StaffBrokerFunctions.SetSetting("Nagape", IIf(cbNagape.Checked, "ON", "OFF"), PortalId)

            Try
                objModules.UpdateTabModuleSetting(TabModuleId, "Reminder1", CInt(tbReminder1.Text))
                If (tbReminder2.Text <> "") Then
                    If (CInt(tbReminder2.Text) > CInt(tbReminder1.Text)) Then
                        objModules.UpdateTabModuleSetting(TabModuleId, "Reminder2", tbReminder2.Text)
                        If (tbGiveUp.Text <> "") Then
                            If (CInt(tbGiveUp.Text) > CInt(tbReminder2.Text)) Then
                                objModules.UpdateTabModuleSetting(TabModuleId, "GiveUp", tbGiveUp.Text)
                            Else
                                objModules.UpdateTabModuleSetting(TabModuleId, "Reminder2", CInt(tbReminder1.Text) + 1)
                            End If
                        Else
                            objModules.UpdateTabModuleSetting(TabModuleId, "Reminder2", CInt(tbReminder1.Text) + 1)
                        End If


                    Else
                        objModules.UpdateTabModuleSetting(TabModuleId, "Reminder2", CInt(tbReminder1.Text) + 1)
                        objModules.UpdateTabModuleSetting(TabModuleId, "GiveUp", CInt(tbReminder1.Text) + 2)
                    End If
                End If
            Catch ex As Exception
                objModules.UpdateTabModuleSetting(TabModuleId, "Reminder1", 2)
                objModules.UpdateTabModuleSetting(TabModuleId, "Reminder2", 4)
                objModules.UpdateTabModuleSetting(TabModuleId, "GiveUp", 7)
            End Try








            ' refresh cache
            SynchronizeModule()
            Response.Redirect(NavigateURL())
        End Sub

        Public Function IsEnabled(ByVal LineTypeId As Integer) As Boolean
            Dim d As New StaffRmbDataContext
            Dim q = From c In d.AP_StaffRmb_PortalLineTypes Where c.PortalId = PortalId And c.LineTypeId = LineTypeId

            Return q.Count > 0

        End Function

        Public Function GetDisplayName(ByVal LineTypeId As Integer, ByVal TypeName As String) As String
            Dim d As New StaffRmbDataContext
            Dim q = From c In d.AP_StaffRmb_PortalLineTypes Where c.PortalId = PortalId And c.LineTypeId = LineTypeId
            If q.Count > 0 Then
                Return q.First.LocalName
            Else
                Return TypeName
            End If

        End Function
        Public Function GetPCode(ByVal LineTypeId As Integer) As String
            Dim d As New StaffRmbDataContext
            Dim q = From c In d.AP_StaffRmb_PortalLineTypes Where c.PortalId = PortalId And c.LineTypeId = LineTypeId
            If q.Count > 0 Then

                Return StaffBrokerFunctions.ValidateAccountCode(q.First.PCode, PortalId)




            Else
                Return ""
            End If

        End Function

        Public Function GetPCodeText(ByVal LineTypeId As Integer) As String
            Dim d As New StaffRmbDataContext
            Dim q = From c In d.AP_StaffRmb_PortalLineTypes Where c.PortalId = PortalId And c.LineTypeId = LineTypeId
            If q.Count > 0 Then
                Return q.First.PCode
            Else
                Return ""
            End If
        End Function
        Public Function GetDCodeText(ByVal LineTypeId As Integer) As String
            Dim d As New StaffRmbDataContext
            Dim q = From c In d.AP_StaffRmb_PortalLineTypes Where c.PortalId = PortalId And c.LineTypeId = LineTypeId
            If q.Count > 0 Then
                Return q.First.DCode
            Else
                Return ""
            End If
        End Function

        Public Function GetDCode(ByVal LineTypeId As Integer) As String
            Dim d As New StaffRmbDataContext
            Dim q = From c In d.AP_StaffRmb_PortalLineTypes Where c.PortalId = PortalId And c.LineTypeId = LineTypeId
            If q.Count > 0 Then
                'Look to see if this code exists




                Dim r = From c In d.AP_StaffBroker_AccountCodes Where c.AccountCode = q.First.DCode And c.PortalId = PortalId

                If r.Count > 0 Then
                    Return StaffBrokerFunctions.ValidateAccountCode(q.First.DCode, PortalId)
                Else
                    Return ""
                End If

            Else
                Return ""
            End If

        End Function

        Protected Sub CancelBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CancelBtn.Click
            Response.Redirect(NavigateURL())
        End Sub

        Protected Sub gvPerDiemMulti_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvPerDiemMulti.RowCommand
            If e.CommandName = "myInsert" Then

                Dim d As New StaffRmbDataContext
                Dim insert = New AP_Staff_Rmb_PerDeimMuliType
                insert.Name = CType(gvPerDiemMulti.FooterRow.Controls(0).FindControl("tbNameI"), TextBox).Text
                insert.Value = CType(gvPerDiemMulti.FooterRow.Controls(0).FindControl("tbAmountI"), TextBox).Text
                insert.Currency = CType(gvPerDiemMulti.FooterRow.Controls(0).FindControl("ddlCurrenciesI"), DropDownList).SelectedValue
                insert.PortalId = PortalId


                d.AP_Staff_Rmb_PerDeimMuliTypes.InsertOnSubmit(insert)
                d.SubmitChanges()
                gvPerDiemMulti.DataBind()
            ElseIf e.CommandName = "myEInsert" Then
                Dim d As New StaffRmbDataContext
                Dim insert = New AP_Staff_Rmb_PerDeimMuliType

                insert.Name = CType(gvPerDiemMulti.Controls(0).Controls(1).FindControl("tbNameE"), TextBox).Text
                insert.Value = CType(gvPerDiemMulti.Controls(0).Controls(1).FindControl("tbValueE"), TextBox).Text
                insert.Currency = CType(gvPerDiemMulti.Controls(0).Controls(1).FindControl("ddlCurrenciesE"), DropDownList).SelectedValue
                insert.PortalId = PortalId

                d.AP_Staff_Rmb_PerDeimMuliTypes.InsertOnSubmit(insert)
                d.SubmitChanges()
                gvPerDiemMulti.DataBind()
            End If
        End Sub

        Protected Sub btnDownload_Click(sender As Object, e As EventArgs) Handles btnDownload.Click

            StaffBrokerFunctions.SetSetting("RmbSinglePump", True, PortalId)
            btnDownload.Enabled = False
            btnDownload.Text = "Downloading"
            lblDownloading.Visible = True

        End Sub
    End Class

End Namespace

