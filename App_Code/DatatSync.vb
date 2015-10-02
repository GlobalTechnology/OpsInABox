Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports StaffBrokerFunctions

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class DatatSync
    Inherits System.Web.Services.WebService
    Public Const VERSION_NUMBER As String = "1.1.0"   'The version of acDatalinks that this webservice is designed against 
    Public Const CRITICAL_VERSION_NUMBER As String = "1.0.66"  'The minimum acDatalinks version that this webservice requires

    Public Const UPDRAGE_AVAILABLE As String = "<p>There is a new version of ACDatalinks available.(ACDatalinks is the datapump that downloads " _
                                                & "transactions from the website and inserts them into your financial package. Your website has been configured to work with " & VERSION_NUMBER _
    & "but you are currently running version [CURRENTVERSION]. Whilst this update is optional, we strongly recommend installing the new version as soon as possible. </p>" _
    & "<p>You can download the latest version of ACDatalinks here: <br>" _
    & "<a href=""https://agapeconnect.me"">https://www.agape.org.uk</a></p>"

    Public Const UPDRAGE_CRITICAL As String = "<p>A critical update for ACDatalinks has been released.(ACDatalinks is the datapump that downloads " _
                                                & "transactions from the website and inserts them into your financial package. Your website has been configured to work with " & VERSION_NUMBER _
    & "but you are currently running version [CURRENTVERSION]. Unfortunately these two versions are not compatible, and the datalink will be disabled until you install the update. You can download the latest version here: <br>" _
    & "<a href=""https://agapeconnect.me"">https://www.agape.org.uk</a></p>"

    Structure ACUnit
        Public Code As String
        Public Name As String
        Public Type As Integer
    End Structure

    Structure UpdateResponse
        Public TntStatus As StatusDescription
        Public Rmbs As StatusDescription()
        Public Budgets As Budget.AP_Budget_Summary1()



    End Structure

    Structure SPResponse
        Public Company As String

        Public Control As String
        Public ExpensesPayable As String
        Public PayrolPayable As String
        Public TaxablePayable As String
        Public SalaryAcc As String
        Public Format As Integer
        Public SuggestedPayments() As SuggestedPayment
    End Structure


    Structure APBalanceInfo
        Public CostCenter As String
        Public ExpensesPayable As Double 'Payable (including NET taxable exenses)
        Public SpSalaryAdv As Double 'Taxable Expenses
        Public PreviousSalary As Double 'Taxable
        Public AccountBalance As Double
        Public AdvanceBalance As Double

    End Structure

    Structure StatusDescription
        Public Status As String
        Public Message As String
        Public RowId As Integer
        Public ActualPeriod As Integer
        Public ActualYear As Integer
        Public BatchId As String
    End Structure

    Structure SuggestedPayment
        Public RC As String
        Public Name As String
        Public ExpPayable As Double
        Public ExpTaxable As Double
        Public Salary As Double
    End Structure


    Structure DownloadResponse
        Public Status As String
        Public Message As String
        Public Value As Integer
        Public WebUsers As WebUser()
        Public Rmbs As Rmb()
        Public Advances As Adv()
        Public ChangedBudgets As Budget.AP_Budget_Summary1()
        Public AcctsReceivable As String
        Public AcctsPayable As String
        Public TaxableAcctsReceivable As String
        Public ControlAccount As String
        Public APIVersion As String
        Public VersionStatus As String

    End Structure


    Structure WebUser
        Public Name As String
        Public GCXID As String
        Public GCXUserName As String
        Public UserId As Integer
        Public AccountsUser As Boolean

        Public PersonalAccounts As WebProfileAccount()
        Public TeamAccounts As WebProfileAccount()
        Public DepartmentAccounts As WebProfileAccount()

        Public OtherAccounts As WebProfileAccount()
    End Structure


    Structure WebProfileAccount
        Public Name As String
        Public CostCenter As String
        Public Designations As String
    End Structure


    Structure Rmb
        Public RmbNo As Integer
        Public UserName As String
        Public UserInitials As String
        Public StaffName As String
        Public PersonalCostCenter As String
        Public PersonalAccountCode As String
        Public Status As Integer
        Public ApproverName As String
        Public AccountsName As String
        Public SubmittedDate As Date
        Public ApprovedDate As Date
        Public ProcessDate As Date
        Public Period As Integer
        Public Year As Integer
        Public SupplierCode As String
        Public AdvanceRequest As Double
        Public ErrorDesc As String
        Public Department As Boolean
        Public RID As Integer
        Public Lines As RmbLine()
    End Structure

    Structure RmbLine
        Public RmbLineNo As Integer
        Public RmbNo As Integer
        Public LineType As String
        Public AccountCode As String
        Public CostCenter As String
        Public TransDate As Date
        Public Comment As String
        Public GrossAmount As Double
        Public Taxable As Boolean
        Public VATRate As Boolean
        Public VATCode As String
        Public Receipt As Boolean
        Public ReceiptNo As Integer
        Public Mileage As Integer
        Public MileageRate As Integer
        Public AnalysisCode As String
        Public Department As Boolean
        Public VAT As Boolean

    End Structure
    Structure Adv
        Public AdvanceId As Integer
        Public LocalAdvanceId As Integer
        Public Amount As Double
        Public Reason As String
        Public UserName As String
        Public UserInitials As String
        Public StaffName As String
        Public PersonalCostCenter As String

        Public Status As Integer
        Public ApproverName As String
        Public AccountsName As String
        Public SubmittedDate As Date
        Public ApprovedDate As Date
        Public ProcessDate As Date
        Public Period As Integer
        Public Year As Integer
        Public OrigCurrency As String

    End Structure


    Structure SetupInfo
        Public urlDataserverPortal As String
        Public AdvSuffix As String
        Public AdvPrefix As String
        Public CurrencyCode As String
        Public CompanyId As String
        Public acDatalink_DatalinkId As String
        Public acDatalink_Version As String
        Public acDatalink_DataserverVersion As String
        Public acDatalink_WindowsVersion As String

        Public acDatalink_SQLVersion As String
        Public acDatalink_TNT_InstallPath As String
        Public acDatalink_acDatalink_Error As String
        Public acDatalink_Active As Boolean
        Public acDatalink_ComputerName As String
        Public acDatalink_PollDelayInSeconds As Integer
        Public currentFiscalPeriod As String

        Public changedBudgets As Budget.AP_Budget_Summary1()

        Public FirstFiscalMonth As Integer



    End Structure




    Private Function Validate(ByVal Password As String) As Boolean
        Return GetPassword() = Password
    End Function

    Public Function GetPassword() As String
        Dim PS = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)
        Return GetPassword(PS.PortalId)


    End Function

    Public Function GetPassword(ByVal Portalid As Integer) As String

        Dim Password = StaffBrokerFunctions.GetSetting("wsPassword", Portalid)
        Return AgapeEncryption.AgapeEncrypt.Decrypt(Password)

    End Function

    Public Sub SetPassword()

        Dim PS = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)
        SetPassword(PS.PortalId)

    End Sub

    Public Sub SetPassword(ByVal PortalId As Integer)

        StaffBrokerFunctions.SetSetting("wsPassword", AgapeEncryption.AgapeEncrypt.Encrypt(GetRandomCode(16)), PortalId)
    End Sub
    Private Function GetRandomCode(ByVal Length As Integer) As String

        Dim allChars As String = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNPQRSTUVWXYZ123456789"

        Dim GotUniqueCode As Boolean = False
        Dim uniqueCode As String = ""
        Dim str As New System.Text.StringBuilder
        Dim xx As Integer
        For i As Byte = 1 To Length 'length of req key

            Randomize()
            xx = Rnd() * (Len(allChars) - 1) 'number of rawchars
            str.Append(allChars.Trim.Chars(xx))
        Next
        uniqueCode = str.ToString
        Return uniqueCode

    End Function

    <WebMethod()> _
    Public Function TriggerEmails(ByVal Password As String) As String
        Dim PS = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)
        If PS.PortalId <> 0 Then
            Return Nothing
        End If
        If Password <> GetPassword() Then
            Return Nothing
        End If
        Dim rtn As String = ""

        Dim dr As New StaffRmb.StaffRmbDataContext
        Dim d As New StaffBroker.StaffBrokerDataContext

        Dim prmbs = From c In dr.AP_Staff_Rmbs Where c.Status = StaffRmb.RmbStatus.Submitted Group By c.PortalId Into Group

        For Each portal In prmbs
            Try
                If GetSetting("Nagape", portal.PortalId) = "ON" Then



                    Dim GiveUp = StaffBrokerFunctions.GetTemplate("NagapeGiveUp", portal.PortalId)
                    Dim ChaseUp = StaffBrokerFunctions.GetTemplate("NagapeChaseUp", portal.PortalId)

                    Dim mc As New DotNetNuke.Entities.Modules.ModuleController

                    Dim x = mc.GetModuleByDefinition(portal.PortalId, "acStaffRmb")

                    Dim RmbSettings = x.TabModuleSettings
                    Dim AuthUser = UserController.GetUserById(portal.PortalId, RmbSettings("AuthUser"))

                    Dim AuthAuthUser = UserController.GetUserById(portal.PortalId, RmbSettings("AuthAuthUser"))
                    Dim rem1 As Integer = 2
                    Dim rem2 As Integer = 4
                    Dim rem3 As Integer = 7

                    Try
                        rem1 = RmbSettings("Reminder1")
                        rem2 = RmbSettings("Reminder2")
                        rem3 = RmbSettings("GiveUp")

                    Catch ex As Exception

                    End Try


                    rtn &= "Sending Approval Reminders for Portal: " & portal.PortalId & vbNewLine
                    For Each rmb In portal.Group
                        rtn &= "Rmb " & rmb.RMBNo & rmb.UserRef & ": "


                        Dim diff = CInt(DateDiff("d", rmb.RmbDate, Now))
                        Try



                            If diff >= rem3 And (Not rmb.SpareField5 Is Nothing) And CInt(rmb.SpareField5) = 2 Then
                                rtn &= "Sending Give-Up  " & vbNewLine
                                SendReminder(rmb, diff, GiveUp, AuthUser, AuthAuthUser, portal.PortalId, True)
                                rmb.SpareField5 = 9

                            ElseIf diff >= rem2 And (Not rmb.SpareField5 Is Nothing) And CInt(rmb.SpareField5) = 1 Then
                                rtn &= "Sending " & rem2 & "-day reminder  " & vbNewLine
                                SendReminder(rmb, diff, ChaseUp, AuthUser, AuthAuthUser, portal.PortalId, False)
                                rmb.SpareField5 = 2
                            ElseIf diff >= rem1 And rmb.SpareField5 Is Nothing Then
                                rtn &= "Sending " & rem1 & "-day reminder  " & vbNewLine
                                SendReminder(rmb, diff, ChaseUp, AuthUser, AuthAuthUser, portal.PortalId, False)
                                rmb.SpareField5 = 1
                            Else
                                rtn &= "Nothing to do!  " & vbNewLine
                                'Console.Write("Nothing" & vbNewLine)
                            End If
                            'We tell user we will no longer chase up the team leader...




                        Catch ex As Exception

                            rtn &= "Error Sending Approve Reminder on Rmb#" & rmb.RMBNo & ": " & ex.ToString & vbNewLine
                        End Try
                    Next
                    dr.SubmitChanges()
                End If
            Catch ex As Exception
                rtn &= "Error Sending Approve Reminders on Portal#" & portal.PortalId & ": " & ex.ToString & vbNewLine
            End Try

        Next

        Dim padvs = From c In dr.AP_Staff_AdvanceRequests Where c.RequestStatus = StaffRmb.RmbStatus.Submitted Group By c.PortalId Into Group

        For Each portal In padvs
            Try
                If GetSetting("Nagape", portal.PortalId) = "ON" Then



                    Dim GiveUp = StaffBrokerFunctions.GetTemplate("NagapeGiveUpAdvance", portal.PortalId)
                    Dim ChaseUp = StaffBrokerFunctions.GetTemplate("NagapeChaseUpAdvance", portal.PortalId)

                    Dim mc As New DotNetNuke.Entities.Modules.ModuleController

                    Dim x = mc.GetModuleByDefinition(portal.PortalId, "acStaffRmb")

                    Dim RmbSettings = x.TabModuleSettings
                    Dim AuthUser = UserController.GetUserById(portal.PortalId, RmbSettings("AuthUser"))

                    Dim AuthAuthUser = UserController.GetUserById(portal.PortalId, RmbSettings("AuthAuthUser"))
                    Dim rem1 As Integer = 2
                    Dim rem2 As Integer = 4
                    Dim rem3 As Integer = 7

                    Try
                        rem1 = RmbSettings("Reminder1")
                        rem2 = RmbSettings("Reminder2")
                        rem3 = RmbSettings("GiveUp")

                    Catch ex As Exception

                    End Try
                    rtn &= "Sending Advance Approval Reminders for Portal: " & portal.PortalId & vbNewLine
                    For Each adv In portal.Group
                        rtn &= "Adv " & adv.LocalAdvanceId


                        Dim diff = CInt(DateDiff("d", adv.RequestDate, Now))
                        Try



                            If diff >= rem3 And (Not adv.SpareField5 Is Nothing) And CInt(adv.SpareField5) = 2 Then
                                rtn &= "Sending Give-Up  " & vbNewLine
                                SendReminderAdv(adv, diff, GiveUp, AuthUser, AuthAuthUser, portal.PortalId, True, RmbSettings("LargeTransaction"))
                                adv.SpareField5 = 9

                            ElseIf diff >= rem2 And (Not adv.SpareField5 Is Nothing) And CInt(adv.SpareField5) = 1 Then
                                rtn &= "Sending 10-day reminder  " & vbNewLine
                                SendReminderAdv(adv, diff, ChaseUp, AuthUser, AuthAuthUser, portal.PortalId, False, RmbSettings("LargeTransaction"))
                                adv.SpareField5 = 2
                            ElseIf diff >= rem1 And adv.SpareField5 Is Nothing Then
                                rtn &= "Sending 5-day reminder  " & vbNewLine
                                SendReminderAdv(adv, diff, ChaseUp, AuthUser, AuthAuthUser, portal.PortalId, False, RmbSettings("LargeTransaction"))
                                adv.SpareField5 = 1
                            Else
                                'Console.Write("Nothing" & vbNewLine)
                                rtn &= "Nothing to do!  " & vbNewLine
                            End If
                            'We tell user we will no longer chase up the team leader...




                        Catch ex As Exception

                            rtn &= "Error Sending Approve Reminder on Adv#" & adv.LocalAdvanceId & ": " & ex.ToString & vbNewLine
                        End Try
                    Next
                    dr.SubmitChanges()
                End If
            Catch ex As Exception
                rtn &= "Error Sending Approve Reminders on Portal#" & portal.PortalId & ": " & ex.ToString & vbNewLine
            End Try

        Next


        'Now do the same for Advances

        rtn &= "Finished..."


        Return rtn

    End Function
    Private Sub SendReminder(ByVal theRmb As StaffRmb.AP_Staff_Rmb, ByVal diff As String, ByVal template1 As String, ByVal AuthUser As UserInfo, ByVal AuthAuthUser As UserInfo, ByVal portalId As Integer, ByVal isGiveUp As Boolean)
        'Find the managers:
        Dim dr As New StaffRmb.StaffRmbDataContext
        Dim d As New StaffBroker.StaffBrokerDataContext
        Dim Subject As String = "Rmb #" & theRmb.RID & IIf(theRmb.UserRef <> "", " - " & theRmb.UserRef, "")
        Dim Approvers = ""
        Dim ccEmail As String = ""
        Dim app = StaffRmb.StaffRmbFunctions.getApprovers(theRmb, AuthUser, AuthAuthUser)

        For Each row In app.UserIds
            ccEmail = ccEmail & row.Email & ","

            Approvers &= row.FirstName & " " & row.LastName & "<br />"
        Next
        ccEmail = Left(ccEmail, ccEmail.Length - 1)





        template1 = template1.Replace("[APPROVERS]", Approvers)
        Dim theUser = UserController.GetUserById(portalId, theRmb.UserId)
        template1 = template1.Replace("[STAFFNAME]", theUser.DisplayName)

        template1 = template1.Replace("[DIFF]", diff)

        If isGiveUp Then
            DotNetNuke.Services.Mail.Mail.SendMail("donotreply@agapeconnect.me", theUser.Email, ccEmail, "", DotNetNuke.Services.Mail.MailPriority.Normal, Subject, DotNetNuke.Services.Mail.MailFormat.Html, System.Text.Encoding.UTF8, template1, "", "", "", "", "")
        Else

            DotNetNuke.Services.Mail.Mail.SendMail("donotreply@agapeconnect.me", ccEmail, theUser.Email, "", DotNetNuke.Services.Mail.MailPriority.Normal, Subject, DotNetNuke.Services.Mail.MailFormat.Html, System.Text.Encoding.UTF8, template1, "", "", "", "", "")
        End If




    End Sub

    Private Sub SendReminderAdv(ByVal theAdv As StaffRmb.AP_Staff_AdvanceRequest, ByVal diff As String, ByVal template1 As String, ByVal AuthUser As UserInfo, ByVal AuthAuthUser As UserInfo, ByVal portalId As Integer, ByVal isGiveUp As Boolean, ByVal LargeTransaction As Double)
        'Find the managers:
        Dim dr As New StaffRmb.StaffRmbDataContext
        Dim d As New StaffBroker.StaffBrokerDataContext
        Dim Subject As String = "Adv #" & theAdv.LocalAdvanceId
        Dim Approvers = ""
        Dim ccEmail As String = ""
        Dim app = StaffRmb.StaffRmbFunctions.getAdvApprovers(theAdv, LargeTransaction, AuthUser, AuthAuthUser)

        For Each row In app.UserIds
            ccEmail = ccEmail & row.Email & ","

            Approvers &= row.FirstName & " " & row.LastName & "<br />"
        Next
        ccEmail = Left(ccEmail, ccEmail.Length - 1)





        template1 = template1.Replace("[APPROVERS]", Approvers)
        Dim theUser = UserController.GetUserById(portalId, theAdv.UserId)
        template1 = template1.Replace("[STAFFNAME]", theUser.DisplayName)

        template1 = template1.Replace("[DIFF]", diff)
        If isGiveUp Then
            DotNetNuke.Services.Mail.Mail.SendMail("donotreply@agapeconnect.me", theUser.Email, ccEmail, "", DotNetNuke.Services.Mail.MailPriority.Normal, Subject, DotNetNuke.Services.Mail.MailFormat.Html, System.Text.Encoding.UTF8, template1, "", "", "", "", "")
        Else

            DotNetNuke.Services.Mail.Mail.SendMail("donotreply@agapeconnect.me", ccEmail, theUser.Email, "", DotNetNuke.Services.Mail.MailPriority.Normal, Subject, DotNetNuke.Services.Mail.MailFormat.Html, System.Text.Encoding.UTF8, template1, "", "", "", "", "")
        End If




    End Sub

    <WebMethod()> _
    Public Function HelloWorld(ByVal Password As String) As String
        If Password <> GetPassword() Then
            Return Nothing
        End If

        Return "HelloWorld"
    End Function

    <WebMethod()> _
    Public Function TestAPBalances(ByVal Password As String) As Boolean
        Dim bals As New List(Of APBalanceInfo)
        Dim ins As New APBalanceInfo
        ins.AccountBalance = 0
        ins.AdvanceBalance = 0
        ins.CostCenter = "1102"
        ins.ExpensesPayable = 0
        ins.PreviousSalary = 0
        ins.SpSalaryAdv = 0
        bals.Add(ins)
        Return SetAPBalances(Password, bals.ToArray)





    End Function


    <WebMethod()> _
    Public Function SetAPBalances(ByVal Password As String, ByVal Balances As APBalanceInfo()) As Boolean
        If Password <> GetPassword() Then
            Return Nothing
        End If
        Dim d As New StaffBroker.StaffBrokerDataContext
        Dim PS = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)
        Dim q = From c In d.AP_Staff_SuggestedPayments Where c.PortalId = PS.PortalId

        d.AP_Staff_SuggestedPayments.DeleteAllOnSubmit(q)
        d.SubmitChanges()

        For Each row In Balances
            Dim insert As New StaffBroker.AP_Staff_SuggestedPayment
            insert.CostCenter = row.CostCenter
            insert.ExpPayable = row.ExpensesPayable
            insert.PrevSalary = row.PreviousSalary
            insert.ExpTaxable = row.SpSalaryAdv
            insert.AccountBalance = row.AccountBalance
            insert.AdvanceBalance = row.AdvanceBalance
            insert.PortalId = PS.PortalId
            d.AP_Staff_SuggestedPayments.InsertOnSubmit(insert)
        Next

        d.SubmitChanges()
        Return True

    End Function

    <WebMethod()> _
    Public Function GetSuggestedPayment(ByVal Password As String) As SPResponse
        If Password <> GetPassword() Then
            Return Nothing
        End If
        Dim PS = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)
        Dim d As New StaffBroker.StaffBrokerDataContext

        Dim mc As New DotNetNuke.Entities.Modules.ModuleController

        Dim x = mc.GetModuleByDefinition(PS.PortalId, "acStaffRmb")
        Dim RmbSettings = x.TabModuleSettings

        Dim rtn As New SPResponse
        rtn.Control = RmbSettings("ControlAccount")
        rtn.ExpensesPayable = RmbSettings("AccountsPayable")
        Select Case RmbSettings("DownloadFormat")
            Case "DDC" : rtn.Format = 1
            Case "DCD" : rtn.Format = 2
            Case "CDDC" : rtn.Format = 3
            Case "CDCD" : rtn.Format = 4
        End Select

        rtn.PayrolPayable = RmbSettings("PayrollPayable")
        rtn.SalaryAcc = RmbSettings("SalaryAccount")
        rtn.TaxablePayable = RmbSettings("TaxAccountsReceivable")



        Dim q = From c In d.AP_Staff_SuggestedPayments Join b In d.AP_StaffBroker_CostCenters On c.CostCenter Equals b.CostCentreCode Where c.PortalId = PS.PortalId And b.PortalId = PS.PortalId Order By c.CostCenter
        rtn.SuggestedPayments = New SuggestedPayment(q.Count - 1) {}
        Dim i As Integer = 0
        For Each row In q

            rtn.SuggestedPayments(i).RC = row.c.CostCenter
            rtn.SuggestedPayments(i).Name = row.b.CostCentreName
            rtn.SuggestedPayments(i).ExpPayable = row.c.ExpPayable
            rtn.SuggestedPayments(i).ExpTaxable = row.c.ExpTaxable
            rtn.SuggestedPayments(i).Salary = row.c.PrevSalary

            i += 1
        Next







        Return rtn
    End Function



    Private Function IsDataDirty() As Boolean
        Dim PS = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)

        SetSetting("TransBroker", "Clean", PS.PortalId)
        Return GetSetting("TransBroker", PS.PortalId) = "Dirty"


    End Function

    '<WebMethod()> _
    'Public Sub SetupDataLink(ByVal pword As String, ByVal config As SetupInfo, ByVal ccs As ACUnit(), ByVal accs As ACUnit())
    '    If Validate(pword) = False Then
    '        Return
    '    End If

    '    Dim PS = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)
    '    Dim Currency = config.CurrencyCode
    '    If Currency = "EUR" Then
    '        Currency = "€"
    '    ElseIf Currency = "GBP" Then
    '        Currency = "£"
    '    ElseIf Currency = "USD" Then
    '        Currency = "$"
    '    End If

    '    SetSetting("AdvancePrefix", config.AdvPrefix, PS.PortalId)
    '    SetSetting("AdvanceSuffix", config.AdvSuffix, PS.PortalId)
    '    SetSetting("Currency", Currency, PS.PortalId)
    '    SetSetting("DataserverURL", config.urlDataserverPortal, PS.PortalId)

    '    AccountRefresh(ccs, accs)
    '    DatapumpOK()
    'End Sub



    <WebMethod()> _
    Public Function RequestUpdate(ByVal pword As String, ByVal Settings As SetupInfo) As DownloadResponse
        If Validate(pword) = False Then
            Return Nothing
        End If
        Dim rtn As New DownloadResponse()
        IsVersionValid(Settings.acDatalink_Version, rtn)
        If rtn.VersionStatus = "CRITICAL" Then
            Return rtn
        End If
        UpdateSetupInfo(Settings)
        doRequestUpdate(rtn)
        DatapumpOK()

        Return rtn
    End Function
    <WebMethod()> _
    Public Function TestRequestUpdate(ByVal pword As String) As DownloadResponse
        If Validate(pword) = False Then
            Return Nothing
        End If
        Dim rtn As New DownloadResponse()

        doRequestUpdate(rtn)

        Return rtn
    End Function

    <WebMethod()> _
    Public Sub UpdateResponses(ByVal pword As String, ByVal uResp As UpdateResponse)
        If Validate(pword) = False Then
            Return
        End If


        Dim PS = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)
        Dim objEventLog As New DotNetNuke.Services.Log.EventLog.EventLogController

        If Not uResp.Rmbs Is Nothing Then


            Dim d As New StaffRmb.StaffRmbDataContext
            For Each Rmb In uResp.Rmbs

                If Rmb.RowId > 0 Then 'An Rmb

                    Dim theRmb = From c In d.AP_Staff_Rmbs Where c.RMBNo = Rmb.RowId And c.PortalId = PS.PortalId

                    If theRmb.Count > 0 Then

                        If Rmb.Status = "SUCCESS" Then
                            theRmb.First.Status = StaffRmb.RmbStatus.Processed
                            theRmb.First.Period = Rmb.ActualPeriod
                            theRmb.First.Year = Rmb.ActualYear
                            theRmb.First.SpareField5 = Rmb.BatchId
                            theRmb.First.Error = False
                            theRmb.First.ErrorMessage = ""
                            objEventLog.AddLog("Rmb" & theRmb.First.RID, "DOWNLOADED", PS, 1, Log.EventLog.EventLogController.EventLogType.ADMIN_ALERT)

                        ElseIf Rmb.Status = "ERROR" Then
                            theRmb.First.Status = StaffRmb.RmbStatus.DownloadFailed
                            theRmb.First.Error = True
                            theRmb.First.ErrorMessage = Rmb.Message
                            objEventLog.AddLog("ERROR- Rmb" & theRmb.First.RID, "DOWNLOAD FAILED", PS, 1, Log.EventLog.EventLogController.EventLogType.ADMIN_ALERT)

                        End If
                        d.SubmitChanges()
                    End If
                Else ' An Advance
                    Dim theAdv = From c In d.AP_Staff_AdvanceRequests Where c.AdvanceId = -Rmb.RowId And c.PortalId = PS.PortalId
                    If theAdv.Count > 0 Then
                        If Rmb.Status = "SUCCESS" Then
                            theAdv.First.RequestStatus = StaffRmb.RmbStatus.Processed
                            theAdv.First.Period = Rmb.ActualPeriod
                            theAdv.First.Year = Rmb.ActualYear
                            theAdv.First.ErrorMessage = "(Processed in Batch" & Rmb.BatchId & ")"
                            theAdv.First.Error = False


                            objEventLog.AddLog("Adv" & theAdv.First.LocalAdvanceId, "DOWNLOADED", PS, 1, Log.EventLog.EventLogController.EventLogType.ADMIN_ALERT)

                        ElseIf Rmb.Status = "ERROR" Then
                            theAdv.First.RequestStatus = StaffRmb.RmbStatus.DownloadFailed
                            theAdv.First.Error = True
                            theAdv.First.ErrorMessage = Rmb.Message
                            objEventLog.AddLog("ERROR- Adv" & theAdv.First.LocalAdvanceId, "DOWNLOAD FAILED", PS, 1, Log.EventLog.EventLogController.EventLogType.ADMIN_ALERT)

                        End If
                        d.SubmitChanges()
                    End If
                End If
            Next
        End If
        If Not uResp.Budgets Is Nothing Then
            Dim db As New Budget.BudgetDataContext
            For Each bud In uResp.Budgets
                Dim theBud = From c In db.AP_Budget_Summaries Where c.Portalid = PS.PortalId And c.BudgetSummaryId = bud.BudgetSummaryId

                If theBud.Count > 0 Then
                    If bud.Error Is Nothing Then
                        bud.Error = False
                    End If
                    If bud.ErrorMessage Is Nothing Then
                        bud.ErrorMessage = ""
                    End If
                    If ((bud.Error And bud.ErrorMessage.ToLower.Contains("combination")) And bud.P1 = 0 And bud.P2 = 0 And bud.P3 = 0 And bud.P4 = 0 And bud.P5 = 0 And bud.P6 = 0 And bud.P7 = 0 And bud.P8 = 0 And bud.P9 = 0 And bud.P10 = 0 And bud.P11 = 0 And bud.P12 = 0) _
                        Or ((Not bud.Error) And bud.P1 = 0 And bud.P2 = 0 And bud.P3 = 0 And bud.P4 = 0 And bud.P5 = 0 And bud.P6 = 0 And bud.P7 = 0 And bud.P8 = 0 And bud.P9 = 0 And bud.P10 = 0 And bud.P11 = 0 And bud.P12 = 0) Then
                        db.AP_Budget_Summaries.DeleteAllOnSubmit(theBud)
                    Else
                        theBud.First.Changed = bud.Changed
                        theBud.First.Error = bud.Error
                        theBud.First.ErrorMessage = bud.ErrorMessage

                    End If




                    db.SubmitChanges()
                End If

            Next
        End If

    End Sub


    Private Sub AccountRefresh(ByVal ccs As ACUnit(), ByVal accs As ACUnit())

        Dim d As New StaffBroker.StaffBrokerDataContext
        Dim PS = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)
        Dim _ccs = From c In d.AP_StaffBroker_CostCenters Where c.PortalId = PS.PortalId

        Dim _accs = From c In d.AP_StaffBroker_AccountCodes Where c.PortalId = PS.PortalId

        For Each row In accs
            Dim theRow = row
            Dim test = From c In _accs Where c.AccountCode = theRow.Code And c.PortalId = PS.PortalId
            If test.Count > 0 Then
                If test.First.AccountCodeName <> theRow.Name Or test.First.AccountCodeType <> theRow.Type Then
                    test.First.AccountCodeName = theRow.Name
                    test.First.AccountCodeType = theRow.Type

                End If
            Else
                Dim insert As New StaffBroker.AP_StaffBroker_AccountCode
                insert.PortalId = PS.PortalId
                insert.AccountCode = theRow.Code
                insert.AccountCodeName = theRow.Name
                insert.AccountCodeType = theRow.Type
                d.AP_StaffBroker_AccountCodes.InsertOnSubmit(insert)
            End If
        Next

        For Each row In ccs
            Dim theRow = row
            Dim test = From c In _ccs Where c.CostCentreCode = theRow.Code And c.PortalId = PS.PortalId
            If test.Count > 0 Then
                If test.First.CostCentreName <> theRow.Name Or test.First.Type <> theRow.Type Then
                    test.First.CostCentreName = theRow.Name
                    test.First.CostCentreName = theRow.Type

                End If
            Else
                Dim insert As New StaffBroker.AP_StaffBroker_CostCenter
                insert.PortalId = PS.PortalId
                insert.CostCentreCode = theRow.Code
                insert.CostCentreName = theRow.Name
                insert.Type = theRow.Type
                d.AP_StaffBroker_CostCenters.InsertOnSubmit(insert)
            End If
        Next

        d.SubmitChanges()
    End Sub


    Private Sub SyncBudgetsChangedInDynamics(ByVal changed As Budget.AP_Budget_Summary1())
        If Not changed Is Nothing Then


            Dim PS = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)
            Dim d As New Budget.BudgetDataContext
            For Each row In changed
                row.Portalid = PS.PortalId
                Dim q = From c In d.AP_Budget_Summary1s Where c.Portalid = PS.PortalId And c.Account = row.Account And c.RC = row.RC And c.FiscalYear = row.FiscalYear


                If q.Count > 0 Then ' Already a budget value exists
                    If row.LastUpdated > q.First.LastUpdated Then
                        'Value in Dynamics is most recent
                        d.AP_Budget_Summary1s.DeleteAllOnSubmit(q)
                        d.SubmitChanges()
                        d.AP_Budget_Summary1s.InsertOnSubmit(row)
                        d.SubmitChanges()
                    Else ' Value on Website is most recent
                        q.First.Changed = True
                        d.SubmitChanges()
                    End If
                Else 'new budget value entered in Dynamics
                    d.AP_Budget_Summary1s.InsertOnSubmit(row)
                    d.SubmitChanges()
                End If


            Next
        End If
    End Sub


    Private Sub UpdateSetupInfo(ByVal settings As SetupInfo)
        Dim PS = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)
        Dim Currency = settings.CurrencyCode
        If Currency = "EUR" Then
            Currency = "€"
        ElseIf Currency = "GBP" Then
            Currency = "£"
        ElseIf Currency = "USD" Then
            Currency = "$"
        End If
        If GetSetting("Currency", PS.PortalId) = "" Then
            'only set the currency if there is no current value
            SetSetting("Currency", Currency, PS.PortalId)
        End If
        'SetSetting("AdvancePrefix", settings.AdvPrefix, PS.PortalId)
        ' SetSetting("AdvanceSuffix", settings.AdvSuffix, PS.PortalId)
        ' SetSetting("DataserverURL", settings.urlDataserverPortal, PS.PortalId)
        SetSetting("acDatalink_Version", settings.acDatalink_Version, PS.PortalId)
        SetSetting("acDatalink_DataserverVersion", settings.acDatalink_DataserverVersion, PS.PortalId)
        SetSetting("acDatalink_SQLVersion", settings.acDatalink_SQLVersion, PS.PortalId)
        SetSetting("acDatalink_WindowsVersion", settings.acDatalink_WindowsVersion, PS.PortalId)
        SetSetting("acDatalink_TNT_InstallPath", settings.acDatalink_TNT_InstallPath, PS.PortalId)
        SetSetting("acDatalink_DatalinkId", settings.acDatalink_DatalinkId, PS.PortalId)
        SetSetting("acDatalink_ComputerName", settings.acDatalink_ComputerName, PS.PortalId)
        SetSetting("acDatalink_Active", settings.acDatalink_Active, PS.PortalId)
        SetSetting("acDatalink_acDatalink_Error", settings.acDatalink_acDatalink_Error, PS.PortalId)

        SetSetting("CurrentFiscalPeriod", settings.currentFiscalPeriod, PS.PortalId)
        If Not settings.FirstFiscalMonth = Nothing Then
            If settings.FirstFiscalMonth > 0 Then
                SetSetting("FirstFiscalMonth", settings.FirstFiscalMonth, PS.PortalId)
            End If
        End If



        ' SetSetting("CompanyName", settings.CompanyId, PS.PortalId)
        SyncBudgetsChangedInDynamics(settings.changedBudgets)


    End Sub
    Protected Sub IsVersionValid(ByVal acDatalink_Version As String, ByRef rtn As DownloadResponse)
        If acDatalink_Version < CRITICAL_VERSION_NUMBER Then
            rtn.VersionStatus = "CRITICAL"
            rtn.Status = "ERROR"
            rtn.Message = "Critical Update Required"
            '?Email Administrator
        ElseIf acDatalink_Version < VERSION_NUMBER Then
            rtn.VersionStatus = "AVAILABE"
        Else
            rtn.VersionStatus = "OK"


        End If
        rtn.APIVersion = VERSION_NUMBER

    End Sub


    <WebMethod()> _
    Public Function RequestUpdateWithAccountRefresh(ByVal pword As String, ByVal ccs As ACUnit(), ByVal accs As ACUnit(), ByVal settings As SetupInfo) As DownloadResponse
        If Validate(pword) = False Then
            Return Nothing
        End If
        Dim rtn As New DownloadResponse()
        IsVersionValid(settings.acDatalink_Version, rtn)
        If rtn.VersionStatus = "CRITICAL" Then
            Return rtn
        End If

        AccountRefresh(ccs, accs)
        UpdateSetupInfo(settings)

        doRequestUpdate(rtn)
        DatapumpOK()
        Return rtn
    End Function

    Private Sub DatapumpOK()
        Dim PS = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)
        SetSetting("LastPump", Now.ToString, PS.PortalId)
        SetSetting("Datapump", "Unlocked", PS.PortalId)

    End Sub





    Private Sub ProcessTNT(ByRef rtn As DownloadResponse)
        Dim PS = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)


        Dim d As New StaffBroker.StaffBrokerDataContext
        Dim tnt As New List(Of WebUser)
        Dim allstaff = GetStaff()

        Dim mc As New DotNetNuke.Entities.Modules.ModuleController

        Dim x = mc.GetModuleByDefinition(PS.PortalId, "acStaffRmb")
        If x Is Nothing Then
            rtn.Message &= "Error: Could not find the RMB Module; "
            rtn.Status = "Error"
        ElseIf CBool(x.TabModuleSettings("isLoaded") <> "Yes") Then
            rtn.Message &= "Error: The RmbModule has not yet been setup (Go to the Rmb Page (in Edit Mode) and click on Settings); "
            rtn.Status = "Error"
        End If
        Dim RmbSettings = x.TabModuleSettings

        For Each member In allstaff

            Dim staff = GetStaffMember(member.UserID)

            Dim WU As New WebUser
            WU.Name = member.FirstName & " " & member.LastName
            Dim theUser = UserController.GetUserById(PS.PortalId, member.UserID)

            WU.GCXID = theUser.Profile.GetPropertyValue("ssoGUID")

            WU.GCXUserName = Left(member.Username, member.Username.Length - 1)
            WU.UserId = theUser.UserID
            WU.AccountsUser = False

            For Each role In CStr(RmbSettings("AccountsRoles")).Split(";")
                If (theUser.Roles().Contains(role)) Then
                    WU.AccountsUser = True
                End If
            Next


            Dim Personal As New List(Of WebProfileAccount)
            Dim wpa As New WebProfileAccount
            wpa.CostCenter = staff.CostCenter.Replace("-", "")
            wpa.Name = member.FirstName & " " & member.LastName
            Try


                wpa.Designations = GetStaffProfileProperty(GetStaffMember(member.UserID), "Designation(s)")
            Catch ex As Exception

            End Try
            If wpa.CostCenter <> "" Then
                Personal.Add(wpa)
            End If
            WU.PersonalAccounts = Personal.ToArray

            Dim Deptartment As New List(Of WebProfileAccount)
            Dim depts = From c In GetDepartments(member.UserID) Select c Distinct
            For Each row In depts
                Dim wpaDept As New WebProfileAccount
                wpaDept.CostCenter = row.CostCentre.Replace("-", "")
                wpaDept.Name = row.Name
                wpaDept.Designations = row.PayType
                If (From c In Deptartment Where c.CostCenter = row.CostCentre.Replace("-", "")).Count = 0 Then
                    Deptartment.Add(wpaDept)
                End If

            Next
            WU.DepartmentAccounts = Deptartment.ToArray
            Dim theTeam As New List(Of WebProfileAccount)
            Dim team = GetTeam(member.UserID)
            For Each row In team
                Dim wpaTeam As New WebProfileAccount
                wpaTeam.CostCenter = GetStaffMember(row.UserID).CostCenter.Replace("-", "")
                wpaTeam.Name = row.FirstName & " " & row.LastName
                wpaTeam.Designations = ""
                If wpaTeam.CostCenter <> "" And (From c In theTeam Where c.CostCenter = wpaTeam.CostCenter).Count = 0 Then
                    theTeam.Add(wpaTeam)
                End If


            Next
            WU.TeamAccounts = theTeam.ToArray

            If (WU.PersonalAccounts.Count > 0 Or WU.DepartmentAccounts.Count > 0 Or WU.TeamAccounts.Count > 0 Or WU.AccountsUser) Then
                tnt.Add(WU)
            End If


        Next

        rtn.WebUsers = tnt.ToArray
        rtn.Status = "New Data"
        SetSetting("tntFlag", "Downloading", PS.PortalId)
    End Sub

    Private Sub set_if(ByRef setting As Object, ByVal value As Object)
        If value Is Nothing Then
            Return
        Else
            setting = value

        End If
    End Sub

    Protected Function GetAccountCode(ByVal LineTypeId As Integer, ByVal CostCenter As String, ByVal Settings As Hashtable) As String
        Dim PS = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)
        Dim d As New StaffRmb.StaffRmbDataContext
        Dim q = From c In d.AP_StaffRmb_PortalLineTypes Where c.LineTypeId = LineTypeId And c.PortalId = PS.PortalId
        If q.Count > 0 Then
            If StaffBrokerFunctions.IsDept(PS.PortalId, CostCenter) And CBool(Settings("UseDCode")) And q.First.DCode.Length > 0 Then

                Return q.First.DCode
            Else
                Return q.First.PCode
            End If
        End If
        Return ""

    End Function







    Private Sub ProcessRMB(ByRef rtn As DownloadResponse)


        'AgapeLogger.WriteEventLog(0, "RmbDownload start")

        Dim d As New StaffRmb.StaffRmbDataContext
        Dim PS = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)






        Dim Rmbs As New List(Of Rmb)


        Dim mc As New DotNetNuke.Entities.Modules.ModuleController

        Dim x = mc.GetModuleByDefinition(PS.PortalId, "acStaffRmb")
        If x Is Nothing Then
            rtn.Message &= "Error: Could not find the RMB Module; "
            rtn.Status = "Error"
        ElseIf CBool(x.TabModuleSettings("isLoaded") <> "Yes") Then
            rtn.Message &= "Error: The RmbModule has not yet been setup (Go to the Rmb Page (in Edit Mode) and click on Settings); "
            rtn.Status = "Error"
        End If
        Dim RmbSettings = x.TabModuleSettings
        set_if(rtn.AcctsPayable, RmbSettings("AccountsPayable"))
        set_if(rtn.AcctsReceivable, RmbSettings("AccountsReceivable"))
        set_if(rtn.TaxableAcctsReceivable, RmbSettings("TaxAccountsReceivable"))
        set_if(rtn.ControlAccount, RmbSettings("ControlAccount"))

        If VerifyAccountCode(PS.PortalId, rtn.AcctsPayable) = False Then
            rtn.Message &= "Error: The Accounts Payable account does not exist. This can be changed in the Rmb Settings; "
            rtn.Status = "Error"
        End If
        If VerifyAccountCode(PS.PortalId, rtn.AcctsReceivable) = False Then
            rtn.Message &= "Error: The Accounts Receivable account does not exist. This can be changed in the Rmb Settings; "
            rtn.Status = "Error"
        End If
        If VerifyAccountCode(PS.PortalId, rtn.TaxableAcctsReceivable) = False Then
            rtn.Message &= "Error: The Taxable Accounts Receivable account does not exist. This can be changed in the Rmb Settings; "
            rtn.Status = "Error"
        End If
        If VerifyCostCenter(PS.PortalId, rtn.ControlAccount) = False Then
            rtn.Message &= "Error: The Control Account (Responsibility Center) does not exist. This can be changed in the Rmb Settings; "
            rtn.Status = "Error"
        End If

        If Not ((GetSetting("RmbDownload", PS.PortalId) <> "False" Or GetSetting("RmbSinglePump", PS.PortalId) = "True")) Then
            Return
        End If
        If GetSetting("RmbSinglePump", PS.PortalId) = "True" Then
            SetSetting("RmbSinglePump", False, PS.PortalId)
        End If

        Dim q = From c In d.AP_Staff_Rmbs Where (c.Status >= StaffRmb.RmbStatus.PendingDownload) And c.PortalId = PS.PortalId

        Dim r = From c In d.AP_Staff_AdvanceRequests Where (c.RequestStatus >= StaffRmb.RmbStatus.PendingDownload) And c.PortalId = PS.PortalId

        If q.Count + r.Count = 0 Then
            'AgapeLogger.WriteEventLog(0, "RmbDownload Error nothing")
            Return
        End If

        If rtn.Status = "Error" Then

            'AgapeLogger.WriteEventLog(0, "RmbDownload Error" & rtn.Message)
            Return
        End If
        For Each row In q
            ' AgapeLogger.WriteEventLog(0, "RmbDownload Rmb" & row.RID)
            Dim newRmb As New Rmb
            newRmb.RmbNo = row.RMBNo
            newRmb.RID = row.RID
            set_if(newRmb.AdvanceRequest, row.AdvanceRequest)
            set_if(newRmb.ApprovedDate, row.ApprDate)
            set_if(newRmb.Department, row.Department)
            set_if(newRmb.ErrorDesc, row.ErrorMessage)
            set_if(newRmb.Period, row.Period)
            set_if(newRmb.ProcessDate, row.ProcDate)
            set_if(newRmb.Status, row.Status)
            set_if(newRmb.SubmittedDate, row.RmbDate)
            set_if(newRmb.SupplierCode, row.SupplierCode)
            set_if(newRmb.Year, row.Year)


            Dim User = UserController.GetUserById(PS.PortalId, row.UserId)
            newRmb.UserName = User.FirstName & " " & User.LastName
            newRmb.UserInitials = Left(User.FirstName.First, 1) & Left(User.LastName.First, 1)
            Dim staff = GetStaffMember(User.UserID)
            newRmb.StaffName = staff.DisplayName
            newRmb.PersonalCostCenter = staff.CostCenter
            newRmb.PersonalAccountCode = GetStaffProfileProperty(staff.StaffId, "PersonalAccountCode")

            If Not row.ApprUserId Is Nothing Then
                newRmb.ApproverName = UserController.GetUserById(PS.PortalId, row.ApprUserId).DisplayName
            End If


            Dim lines As New List(Of RmbLine)
            For Each line In row.AP_Staff_RmbLines
                Dim l As New RmbLine
                l.RmbLineNo = line.RmbLineNo
                l.RmbNo = line.RmbNo
                If line.AccountCode Is Nothing Then
                    l.AccountCode = GetAccountCode(line.LineType, line.CostCenter, RmbSettings)
                Else
                    set_if(l.AccountCode, line.AccountCode)

                End If
                set_if(l.AccountCode, line.AccountCode)
                set_if(l.AnalysisCode, line.AnalysisCode)
                If String.IsNullOrEmpty(line.ShortComment) Then
                    set_if(l.Comment, line.Comment)
                Else
                    set_if(l.Comment, line.ShortComment)
                End If


                set_if(l.CostCenter, line.CostCenter)
                set_if(l.Department, line.Department)
                set_if(l.GrossAmount, line.GrossAmount)
                set_if(l.LineType, line.AP_Staff_RmbLineType.TypeName)
                set_if(l.Mileage, line.Mileage)
                set_if(l.MileageRate, line.MileageRate)
                set_if(l.Receipt, line.Receipt)
                set_if(l.ReceiptNo, line.ReceiptNo)
                set_if(l.Taxable, line.Taxable)
                set_if(l.TransDate, line.TransDate)
                set_if(l.VATCode, line.VATCode)
                set_if(l.VATRate, line.VATRate)
                set_if(l.VAT, line.VATReceipt)
                lines.Add(l)

            Next
            newRmb.Lines = lines.ToArray
            Rmbs.Add(newRmb)

        Next
        rtn.Rmbs = Rmbs.ToArray


        Dim Advs As New List(Of Adv)
        For Each row In r
            AgapeLogger.WriteEventLog(0, "RmbDownload Adv" & row.LocalAdvanceId)
            Dim newAdv As New Adv
            newAdv.AdvanceId = row.AdvanceId
            newAdv.LocalAdvanceId = row.LocalAdvanceId
            newAdv.OrigCurrency = ""
            set_if(newAdv.ApprovedDate, row.ApprovedDate)
            set_if(newAdv.Period, row.Period)
            set_if(newAdv.ProcessDate, row.ProcessedDate)
            set_if(newAdv.Status, row.RequestStatus)
            set_if(newAdv.SubmittedDate, row.RequestDate)
            set_if(newAdv.Year, row.Year)
            set_if(newAdv.Amount, row.RequestAmount.Value)

            If Not String.IsNullOrEmpty(row.OrigCurrency) Then
                If row.OrigCurrency <> StaffBrokerFunctions.GetSetting("AccountingCurrency", PS.PortalId) Then
                    newAdv.OrigCurrency = "-" & row.OrigCurrency & row.OrigCurrencyAmount.Value.ToString("f2")
                    newAdv.OrigCurrency = newAdv.OrigCurrency.Replace(".00", "")

                End If
            End If



            set_if(newAdv.Reason, UnidecodeSharpFork.Unidecoder.Unidecode(row.RequestText))

            Dim User = UserController.GetUserById(PS.PortalId, row.UserId)
            newAdv.UserName = User.FirstName & " " & User.LastName
            newAdv.UserInitials = UnidecodeSharpFork.Unidecoder.Unidecode(Left(User.FirstName.First, 1) & Left(User.LastName.First, 1))
            Dim staff = GetStaffMember(User.UserID)
            newAdv.StaffName = staff.DisplayName
            newAdv.PersonalCostCenter = staff.CostCenter
            If Not row.ApproverId Is Nothing Then
                newAdv.ApproverName = UserController.GetUserById(PS.PortalId, row.ApproverId).DisplayName
            End If

            Advs.Add(newAdv)
        Next
        rtn.Advances = Advs.ToArray


        rtn.Status = "New Data"
        'SetSetting("rmbFlag", "Clean", PS.PortalId)
        'AgapeLogger.WriteEventLog(0, "RmbDownload " & rtn.Rmbs.Count & ":" & rtn.Advances.Count)
    End Sub

    Private Sub GetBudgets(ByRef rtn As DownloadResponse, ByVal PortalId As Integer)
        Dim db As New Budget.BudgetDataContext
        Dim toDownload = (From c In db.AP_Budget_Summary1s Where c.Portalid = PortalId And c.Changed)

        If toDownload.Count > 0 Then
            rtn.Status = "New Data"
            rtn.ChangedBudgets = toDownload.ToArray

        End If



    End Sub

    Private Sub doRequestUpdate(ByRef rtn As DownloadResponse)
        Dim PS = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)

        rtn.Status = "Nothing to do"


        GetBudgets(rtn, PS.PortalId)


        ProcessRMB(rtn)

        DatapumpOK()

    End Sub


End Class