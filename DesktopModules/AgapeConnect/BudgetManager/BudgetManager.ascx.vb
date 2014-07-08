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
Imports System.Data
Imports System.Data.OleDb
Imports Budget
Namespace DotNetNuke.Modules.Budget



    Partial Class BudgetManager
        Inherits Entities.Modules.PortalModuleBase
        Dim d As New BudgetDataContext

        Dim currentFiscalYear As Integer
        Dim firstFiscalMonth As Integer
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            hfPortalId.Value = PortalId
            lblError.Text = ""
            Dim tmp = StaffBrokerFunctions.GetSetting("FirstFiscalMonth", PortalId)
            If Not String.IsNullOrEmpty(tmp) Then
                firstFiscalMonth = tmp
            End If
            tmp = StaffBrokerFunctions.GetSetting("CurrentFiscalPeriod", PortalId)
            If Not String.IsNullOrEmpty(tmp) Then
                currentFiscalYear = Left(tmp, 4)

            End If
            If (Not Page.IsPostBack) Then
                Dim RCs = From c In d.AP_StaffBroker_CostCenters Where c.PortalId = PortalId Select c.CostCentreCode, Name = c.CostCentreCode & " (" & c.CostCentreName & ")" Order By CostCentreCode

                ddlRC.DataSource = RCs

                ddlRC.DataBind()
                ddlRCNew.DataSource = RCs
                ddlRCNew.DataBind()

                Dim Accs = From c In d.AP_StaffBroker_AccountCodes Where c.PortalId = PortalId And c.AccountCodeType > 2 Select c.AccountCode, Name = c.AccountCode & " (" & c.AccountCodeName & ")" Order By AccountCode

                ddlAC.DataSource = Accs
                ddlAC.DataBind()


                ddlAccountNew.DataSource = Accs
                ddlAccountNew.DataBind()
                If firstFiscalMonth <> 1 Then
                    ddlFiscalYear.Items.Add(New ListItem((currentFiscalYear - 3) & "-" & (currentFiscalYear - 2), currentFiscalYear - 2))
                    ddlFiscalYear.Items.Add(New ListItem((currentFiscalYear - 2) & "-" & (currentFiscalYear - 1), currentFiscalYear - 1))
                    ddlFiscalYear.Items.Add(New ListItem((currentFiscalYear - 1) & "-" & (currentFiscalYear), currentFiscalYear))
                    ddlFiscalYear.Items.Add(New ListItem((currentFiscalYear) & "-" & (currentFiscalYear + 1), currentFiscalYear + 1))
                    ddlFiscalYear.Items.Add(New ListItem((currentFiscalYear + 1) & "-" & (currentFiscalYear + 2), currentFiscalYear + 2))
                Else
                    ddlFiscalYear.Items.Add(New ListItem((currentFiscalYear - 2), currentFiscalYear - 2))
                    ddlFiscalYear.Items.Add(New ListItem((currentFiscalYear - 1), currentFiscalYear - 1))
                    ddlFiscalYear.Items.Add(New ListItem((currentFiscalYear), currentFiscalYear))
                    ddlFiscalYear.Items.Add(New ListItem((currentFiscalYear + 1), currentFiscalYear + 1))
                    ddlFiscalYear.Items.Add(New ListItem((currentFiscalYear + 2), currentFiscalYear + 2))
                End If



                ddlFiscalYear.SelectedValue = currentFiscalYear
            End If


        End Sub

        Protected Sub GridView1_DataBound(sender As Object, e As EventArgs) Handles GridView1.DataBound
            Dim q = From c In d.AP_Budget_Summaries Where c.Portalid = PortalId And c.AP_StaffBroker_AccountCode.AccountCodeType > 2 And c.FiscalYear = CInt(ddlFiscalYear.SelectedValue) And (c.RC = ddlRC.SelectedValue Or ddlRC.SelectedValue = "All" Or (ddlRC.SelectedValue = "AllStaff" And c.AP_StaffBroker_CostCenter.Type = 1) Or (ddlRC.SelectedValue = "AllNonStaff" And c.AP_StaffBroker_CostCenter.Type <> 1)) And (ddlAC.SelectedValue = "All" Or c.Account = ddlAC.SelectedValue Or ((ddlAC.SelectedValue = "3" Or ddlAC.SelectedValue = "IE") And c.AP_StaffBroker_AccountCode.AccountCodeType = 3) Or ((ddlAC.SelectedValue = "4" Or ddlAC.SelectedValue = "IE") And c.AP_StaffBroker_AccountCode.AccountCodeType = 4))
            If q.Count > 0 Then
                Dim ACT = {AccountType.Income, AccountType.AccountsPayable}
                Dim P1 = q.Sum(Function(c) CDbl(IIf(ACT.Contains(c.AP_StaffBroker_AccountCode.AccountCodeType), c.P1, -c.P1)))
                Dim P2 = q.Sum(Function(c) CDbl(IIf(ACT.Contains(c.AP_StaffBroker_AccountCode.AccountCodeType), c.P2, -c.P2)))
                Dim P3 = q.Sum(Function(c) CDbl(IIf(ACT.Contains(c.AP_StaffBroker_AccountCode.AccountCodeType), c.P3, -c.P3)))
                Dim P4 = q.Sum(Function(c) CDbl(IIf(ACT.Contains(c.AP_StaffBroker_AccountCode.AccountCodeType), c.P4, -c.P4)))
                Dim P5 = q.Sum(Function(c) CDbl(IIf(ACT.Contains(c.AP_StaffBroker_AccountCode.AccountCodeType), c.P5, -c.P5)))
                Dim P6 = q.Sum(Function(c) CDbl(IIf(ACT.Contains(c.AP_StaffBroker_AccountCode.AccountCodeType), c.P6, -c.P6)))
                Dim P7 = q.Sum(Function(c) CDbl(IIf(ACT.Contains(c.AP_StaffBroker_AccountCode.AccountCodeType), c.P7, -c.P7)))
                Dim P8 = q.Sum(Function(c) CDbl(IIf(ACT.Contains(c.AP_StaffBroker_AccountCode.AccountCodeType), c.P8, -c.P8)))
                Dim P9 = q.Sum(Function(c) CDbl(IIf(ACT.Contains(c.AP_StaffBroker_AccountCode.AccountCodeType), c.P9, -c.P9)))
                Dim P10 = q.Sum(Function(c) CDbl(IIf(ACT.Contains(c.AP_StaffBroker_AccountCode.AccountCodeType), c.P10, -c.P10)))
                Dim P11 = q.Sum(Function(c) CDbl(IIf(ACT.Contains(c.AP_StaffBroker_AccountCode.AccountCodeType), c.P11, -c.P11)))
                Dim P12 = q.Sum(Function(c) CDbl(IIf(ACT.Contains(c.AP_StaffBroker_AccountCode.AccountCodeType), c.P12, -c.P12)))



                lblPTD1.Text = FormatCurrency(P1)
                lblPTD2.Text = FormatCurrency(P2)
                lblPTD3.Text = FormatCurrency(P3)
                lblPTD4.Text = FormatCurrency(P4)
                lblPTD5.Text = FormatCurrency(P5)
                lblPTD6.Text = FormatCurrency(P6)
                lblPTD7.Text = FormatCurrency(P7)
                lblPTD8.Text = FormatCurrency(P8)
                lblPTD9.Text = FormatCurrency(P9)
                lblPTD10.Text = FormatCurrency(P10)
                lblPTD11.Text = FormatCurrency(P11)
                lblPTD12.Text = FormatCurrency(P12)

                lblYTD1.Text = FormatCurrency(P1)
                lblYTD2.Text = FormatCurrency(P1 + P2)
                lblYTD3.Text = FormatCurrency(P1 + P2 + P3)
                lblYTD4.Text = FormatCurrency(P1 + P2 + P3 + P4)
                lblYTD5.Text = FormatCurrency(P1 + P2 + P3 + P4 + P5)
                lblYTD6.Text = FormatCurrency(P1 + P2 + P3 + P4 + P5 + P6)
                lblYTD7.Text = FormatCurrency(P1 + P2 + P3 + P4 + P5 + P6 + P7)
                lblYTD8.Text = FormatCurrency(P1 + P2 + P3 + P4 + P5 + P6 + P7 + P8)
                lblYTD9.Text = FormatCurrency(P1 + P2 + P3 + P4 + P5 + P6 + P7 + P8 + P9)
                lblYTD10.Text = FormatCurrency(P1 + P2 + P3 + P4 + P5 + P6 + P7 + P8 + P9 + P10)
                lblYTD11.Text = FormatCurrency(P1 + P2 + P3 + P4 + P5 + P6 + P7 + P8 + P9 + P10 + P11)
                lblYTD12.Text = FormatCurrency(P1 + P2 + P3 + P4 + P5 + P6 + P7 + P8 + P9 + P10 + P11 + P12)

                lblTotal.Text = lblYTD12.Text
            Else
                lblPTD1.Text = 0
                lblPTD2.Text = 0
                lblPTD3.Text = 0
                lblPTD4.Text = 0
                lblPTD5.Text = 0
                lblPTD6.Text = 0
                lblPTD7.Text = 0
                lblPTD8.Text = 0
                lblPTD9.Text = 0
                lblPTD10.Text = 0
                lblPTD11.Text = 0
                lblPTD12.Text = 0

                lblYTD1.Text = 0
                lblYTD2.Text = 0
                lblYTD3.Text = 0
                lblYTD4.Text = 0
                lblYTD5.Text = 0
                lblYTD6.Text = 0
                lblYTD7.Text = 0
                lblYTD8.Text = 0
                lblYTD9.Text = 0
                lblYTD10.Text = 0
                lblYTD11.Text = 0
                lblYTD12.Text = 0
                lblTotal.Text = 0
            End If
            If Not firstFiscalMonth = Nothing Then

                lblP1.Text = GetCalendarStartForPeriod(1, firstFiscalMonth, ddlFiscalYear.SelectedValue).ToString("MMM ""'""yy")
                lblP2.Text = GetCalendarStartForPeriod(2, firstFiscalMonth, ddlFiscalYear.SelectedValue).ToString("MMM ""'""yy")
                lblP3.Text = GetCalendarStartForPeriod(3, firstFiscalMonth, ddlFiscalYear.SelectedValue).ToString("MMM ""'""yy")
                lblP4.Text = GetCalendarStartForPeriod(4, firstFiscalMonth, ddlFiscalYear.SelectedValue).ToString("MMM ""'""yy")
                lblP5.Text = GetCalendarStartForPeriod(5, firstFiscalMonth, ddlFiscalYear.SelectedValue).ToString("MMM ""'""yy")
                lblP6.Text = GetCalendarStartForPeriod(6, firstFiscalMonth, ddlFiscalYear.SelectedValue).ToString("MMM ""'""yy")
                lblP7.Text = GetCalendarStartForPeriod(7, firstFiscalMonth, ddlFiscalYear.SelectedValue).ToString("MMM ""'""yy")
                lblP8.Text = GetCalendarStartForPeriod(8, firstFiscalMonth, ddlFiscalYear.SelectedValue).ToString("MMM ""'""yy")
                lblP9.Text = GetCalendarStartForPeriod(9, firstFiscalMonth, ddlFiscalYear.SelectedValue).ToString("MMM ""'""yy")
                lblP10.Text = GetCalendarStartForPeriod(10, firstFiscalMonth, ddlFiscalYear.SelectedValue).ToString("MMM ""'""yy")
                lblP11.Text = GetCalendarStartForPeriod(11, firstFiscalMonth, ddlFiscalYear.SelectedValue).ToString("MMM ""'""yy")
                lblP12.Text = GetCalendarStartForPeriod(12, firstFiscalMonth, ddlFiscalYear.SelectedValue).ToString("MMM ""'""yy")


            End If

        End Sub


        Protected Function GetCalendarStartForPeriod(ByVal period As Integer, ByVal firstMonth As Integer, ByVal FiscalYear As Integer) As Date
            If period + firstMonth - 1 <= 12 Then
                Return New Date(FiscalYear - 1, period + firstMonth - 1, 1)
            Else
                Return New Date(FiscalYear, period + firstMonth - 13, 1)
            End If
        End Function

        Protected Sub btnInsertRow_Click(sender As Object, e As EventArgs) Handles btnInsertRow.Click
            Dim q = From c In d.AP_Budget_Summaries Where c.Portalid = PortalId And c.FiscalYear = ddlFiscalYear.SelectedValue And c.Account = ddlAccountNew.SelectedValue And c.RC = ddlRCNew.SelectedValue
            If q.Count = 0 Then
                Dim insert As New AP_Budget_Summary
                insert.Portalid = PortalId
                insert.FiscalYear = ddlFiscalYear.SelectedValue
                insert.Account = ddlAccountNew.SelectedValue
                insert.RC = ddlRCNew.SelectedValue
                insert.P1 = tbP1new.Text
                insert.P2 = tbP2new.Text
                insert.P3 = tbP3new.Text
                insert.P4 = tbP4new.Text
                insert.P5 = tbP5new.Text
                insert.P6 = tbP6new.Text
                insert.P7 = tbP7new.Text
                insert.P8 = tbP8new.Text
                insert.P9 = tbP9new.Text
                insert.P10 = tbP10new.Text
                insert.P11 = tbP11new.Text
                insert.P12 = tbP12new.Text
                insert.Changed = True
                insert.LastUpdated = Now
                d.AP_Budget_Summaries.InsertOnSubmit(insert)
                d.SubmitChanges()
                GridView1.DataBind()

                btnCancelInsert_Click(Me, Nothing)


            Else
                If q.First.P1 = 0 And q.First.P2 = 0 And q.First.P3 = 0 And q.First.P4 = 0 And q.First.P5 = 0 And q.First.P6 = 0 And q.First.P7 = 0 And q.First.P8 = 0 And q.First.P9 = 0 And q.First.P10 = 0 And q.First.P11 = 0 And q.First.P12 = 0 Then
                    btnAddTo_Click(Me, Nothing)
                End If

                'Budget already exists... replace or addto.
                WarningRow.Visible = True
                btnInsertRow.Visible = False
                btnInsertAutoSplit.Visible = False

            End If



        End Sub


        Protected Sub btnCancelInsert_Click(sender As Object, e As EventArgs) Handles btnCancelInsert.Click
            ddlRCNew.SelectedIndex = 0
            ddlAccountNew.SelectedIndex = 0
            tbP1new.Text = "0"
            tbP2new.Text = "0"
            tbP3new.Text = "0"
            tbP4new.Text = "0"
            tbP5new.Text = "0"
            tbP6new.Text = "0"
            tbP7new.Text = "0"
            tbP8new.Text = "0"
            tbP9new.Text = "0"
            tbP10new.Text = "0"
            tbP11new.Text = "0"
            tbP12new.Text = "0"
            lblTotalNew.Text = "0"
            tbTotalNew.Text = "0"
            WarningRow.Visible = False
            btnInsertRow.Visible = True
            btnInsertAutoSplit.Visible = True
        End Sub



        Protected Sub btnReplace_Click(sender As Object, e As EventArgs) Handles btnReplace.Click
            Dim q = From c In d.AP_Budget_Summaries Where c.Portalid = PortalId And c.FiscalYear = ddlFiscalYear.SelectedValue And c.Account = ddlAccountNew.SelectedValue And c.RC = ddlRCNew.SelectedValue
            If q.Count > 0 Then
                q.First.Portalid = PortalId
                q.First.FiscalYear = ddlFiscalYear.SelectedValue
                q.First.Account = ddlAccountNew.SelectedValue
                q.First.RC = ddlRCNew.SelectedValue
                q.First.P1 = tbP1new.Text
                q.First.P2 = tbP2new.Text
                q.First.P3 = tbP3new.Text
                q.First.P4 = tbP4new.Text
                q.First.P5 = tbP5new.Text
                q.First.P6 = tbP6new.Text
                q.First.P7 = tbP7new.Text
                q.First.P8 = tbP8new.Text
                q.First.P9 = tbP9new.Text
                q.First.P10 = tbP10new.Text
                q.First.P11 = tbP11new.Text
                q.First.P12 = tbP12new.Text
                q.First.Changed = True
                q.First.LastUpdated = Now
                d.SubmitChanges()
                GridView1.DataBind()
                btnCancelInsert_Click(Me, Nothing)


            Else
                'Existing Budget no longer exists... Insert the new row
                btnInsertRow_Click(Me, Nothing)



            End If
        End Sub

        Protected Sub btnAddTo_Click(sender As Object, e As EventArgs) Handles btnAddTo.Click
            Dim q = From c In d.AP_Budget_Summaries Where c.Portalid = PortalId And c.FiscalYear = ddlFiscalYear.SelectedValue And c.Account = ddlAccountNew.SelectedValue And c.RC = ddlRCNew.SelectedValue
            If q.Count > 0 Then
                q.First.Portalid = PortalId
                q.First.FiscalYear = ddlFiscalYear.SelectedValue
                q.First.Account = ddlAccountNew.SelectedValue
                q.First.RC = ddlRCNew.SelectedValue
                q.First.P1 += tbP1new.Text
                q.First.P2 += tbP2new.Text
                q.First.P3 += tbP3new.Text
                q.First.P4 += tbP4new.Text
                q.First.P5 += tbP5new.Text
                q.First.P6 += tbP6new.Text
                q.First.P7 += tbP7new.Text
                q.First.P8 += tbP8new.Text
                q.First.P9 += tbP9new.Text
                q.First.P10 += tbP10new.Text
                q.First.P11 += tbP11new.Text
                q.First.P12 += tbP12new.Text
                q.First.Changed = True
                q.First.LastUpdated = Now
                d.SubmitChanges()
                GridView1.DataBind()
                btnCancelInsert_Click(Me, Nothing)


            Else
               
                'Existing Budget no longer exists... Insert the new row
                btnInsertRow_Click(Me, Nothing)



            End If
        End Sub

        Protected Sub ddlRC_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlRC.SelectedIndexChanged



            Dim RCs = From c In d.AP_StaffBroker_CostCenters Where c.PortalId = PortalId And (c.CostCentreCode = ddlRC.SelectedValue Or ddlRC.SelectedValue = "All" Or (ddlRC.SelectedValue = "AllStaff" And c.Type = 1) Or (ddlRC.SelectedValue = "AllNonStaff" And c.Type <> 1))
                  Select c.CostCentreCode, Name = c.CostCentreCode & " (" & c.CostCentreName & ")" Order By CostCentreCode

            ddlRCNew.DataSource = RCs
            ddlRCNew.DataBind()
        End Sub

        Protected Sub ddlAC_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlAC.SelectedIndexChanged
            Dim Accs = From c In d.AP_StaffBroker_AccountCodes Where c.PortalId = PortalId And c.AccountCodeType > 2 And (ddlAC.SelectedValue = "All" Or c.AccountCode = ddlAC.SelectedValue Or ((ddlAC.SelectedValue = "3" Or ddlAC.SelectedValue = "IE") And c.AccountCodeType = 3) Or ((ddlAC.SelectedValue = "4" Or ddlAC.SelectedValue = "IE") And c.AccountCodeType = 4))
                  Select c.AccountCode, Name = c.AccountCode & " (" & c.AccountCodeName & ")" Order By AccountCode

            ddlAccountNew.DataSource = Accs
            ddlAccountNew.DataBind()
        End Sub

        Protected Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click

            Dim filename As String = "Budget.xls"

            File.Copy(Server.MapPath("/DesktopModules/AgapeConnect/BudgetManager/Budget.xls"), PortalSettings.HomeDirectoryMapPath & filename, True)



            Dim connStr As String = "provider=Microsoft.Jet.OLEDB.4.0;Data Source='" & PortalSettings.HomeDirectoryMapPath & filename & "';Extended Properties='Excel 8.0;HDR=NO;'"
            Dim MyConnection As OleDbConnection
            MyConnection = New OleDbConnection(connStr)

            MyConnection.Open()

            'Dim sql = ""
            Dim MyCommand As New OleDbCommand()
            MyCommand.Connection = MyConnection


            Try

                'Clear the form
                '  Dim sql2 = "Update [Budget$A2:P99] Set F1='', F2='', F3='', F4='', F5='',F6='', F7='', F8='', F10='', F11='', F12='', F13='', F14='', F16='' ;"
                ' MyCommand.CommandText = sql2
                'MyCommand.ExecuteNonQuery()

                Dim sql2 = "Update [Budget$R2:R2] Set F1=@Filter"
                MyCommand.Parameters.AddWithValue("@Filter", "Fiscal Year: " & ddlFiscalYear.SelectedValue & "; R/C: " & ddlRC.SelectedItem.Text & "; A/C: " & ddlAC.SelectedItem.Text & ";")

                MyCommand.CommandText = sql2
                MyCommand.ExecuteNonQuery()
                MyCommand.Parameters.Clear()
                'Get the Current Selection
                Dim q = From c In d.AP_Budget_Summaries Where c.Portalid = PortalId And c.FiscalYear = CInt(ddlFiscalYear.SelectedValue) And (c.RC = ddlRC.SelectedValue Or ddlRC.SelectedValue = "All" Or (ddlRC.SelectedValue = "AllStaff" And c.AP_StaffBroker_CostCenter.Type = 1) Or (ddlRC.SelectedValue = "AllNonStaff" And c.AP_StaffBroker_CostCenter.Type <> 1)) And (ddlAC.SelectedValue = "All" Or c.Account = ddlAC.SelectedValue Or ((ddlAC.SelectedValue = "3" Or ddlAC.SelectedValue = "IE") And c.AP_StaffBroker_AccountCode.AccountCodeType = 3) Or ((ddlAC.SelectedValue = "4" Or ddlAC.SelectedValue = "IE") And c.AP_StaffBroker_AccountCode.AccountCodeType = 4))
                Dim i As Integer = 2


                For Each row In q
                    Dim sql = "Update[Budget$A" & i & ":P" & i & "] set F1=@Account, F2=@RC, "
                    For j = 1 To 12
                        sql &= "F" & (j + 2) & "=@P" & j & ", "

                    Next
                    sql &= "F16=@Notes;"
                    MyCommand.Parameters.AddWithValue("@Account", row.Account)
                    MyCommand.Parameters.AddWithValue("@RC", row.Account)
                    MyCommand.Parameters.AddWithValue("@P1", row.P1)
                    MyCommand.Parameters.AddWithValue("@P2", row.P2)
                    MyCommand.Parameters.AddWithValue("@P3", row.P3)
                    MyCommand.Parameters.AddWithValue("@P4", row.P4)
                    MyCommand.Parameters.AddWithValue("@P5", row.P5)
                    MyCommand.Parameters.AddWithValue("@P6", row.P6)
                    MyCommand.Parameters.AddWithValue("@P7", row.P7)
                    MyCommand.Parameters.AddWithValue("@P8", row.P8)
                    MyCommand.Parameters.AddWithValue("@P9", row.P9)
                    MyCommand.Parameters.AddWithValue("@P10", row.P10)
                    MyCommand.Parameters.AddWithValue("@P11", row.P11)
                    MyCommand.Parameters.AddWithValue("@P12", row.P12)
                    If row.ErrorMessage Is Nothing Then
                        MyCommand.Parameters.AddWithValue("@Notes", "")
                    Else
                        MyCommand.Parameters.AddWithValue("@Notes", row.ErrorMessage)
                    End If



                    MyCommand.CommandText = sql






                    MyCommand.ExecuteNonQuery()
                    MyCommand.Parameters.Clear()
                    i += 1

                Next







                MyConnection.Close()
                Dim attachment As String = "attachment; filename=Budget-" & ddlFiscalYear.SelectedValue & ".xls"

                HttpContext.Current.Response.Clear()
                HttpContext.Current.Response.ClearHeaders()
                HttpContext.Current.Response.ClearContent()
                HttpContext.Current.Response.AddHeader("content-disposition", attachment)
                HttpContext.Current.Response.ContentType = "application/vnd.ms-excel"
                HttpContext.Current.Response.AddHeader("Pragma", "public")
                HttpContext.Current.Response.WriteFile(PortalSettings.HomeDirectoryMapPath & filename)
                HttpContext.Current.Response.End()


            Catch ex As Exception
                StaffBrokerFunctions.EventLog("Budget", "Could Not export budget to excel: " & ex.ToString, UserId)
                MyConnection.Close()
                ' File.Delete(PortalSettings.HomeDirectoryMapPath & filename)
            Finally


            End Try




        End Sub

        Private Sub soft_set(ByRef item As Object, ByVal Value As Object, ByVal def As Object)
            Try
                item = Value

            Catch ex As Exception
                item = def
            End Try
        End Sub

        Private Sub Import(ByVal Overwrite As Boolean)
            If (fuImport.HasFile) Then
                Dim BudgetImport As New List(Of AP_Budget_Summary)
                Dim fileName = Path.GetFileName(fuImport.PostedFile.FileName)
                Dim fileExtension = Path.GetExtension(fuImport.PostedFile.FileName)
                Dim fileLocation As String = PortalSettings.HomeDirectoryMapPath & fileName
                fuImport.SaveAs(fileLocation)
                Dim connectionString = ""

                If (fileExtension = ".xls") Then
                    connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileLocation + ";Extended Properties=""Excel 8.0;HDR=NO;IMEX=2"""
                ElseIf fileExtension = ".xlsx" Then
                    connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=""Excel 12.0;HDR=NO;IMEX=2"""
                End If

                Dim MyConnection As OleDbConnection
                MyConnection = New OleDbConnection(connectionString)

                MyConnection.Open()
                Dim errorCount As Integer = 0
                Dim ErrorString As String = ""
                Try

                    Dim MyCommand As New OleDbCommand()
                    MyCommand.Connection = MyConnection

                    MyCommand.CommandText = "Select * from [Budget$A2:N99]"

                    Dim data = MyCommand.ExecuteReader()

                    While data.Read
                        Try

                       
                        If IsDBNull(data.Item(0)) Or IsDBNull(data.Item(1)) Then
                            Exit While
                        End If



                        Dim insert As New AP_Budget_Summary

                        insert.Account = data.Item(0)
                        insert.RC = data.Item(1)

                        Try
                            insert.P1 = CDbl(data.Item(2))
                        Catch ex As Exception
                            insert.P1 = 0.0
                        End Try
                        Try
                            insert.P2 = CDbl(data.Item(3))
                        Catch ex As Exception
                            insert.P2 = 0.0
                        End Try
                        Try
                            insert.P3 = CDbl(data.Item(4))
                        Catch ex As Exception
                            insert.P3 = 0.0
                        End Try
                        Try
                            insert.P4 = CDbl(data.Item(5))
                        Catch ex As Exception
                            insert.P4 = 0.0
                        End Try
                        Try
                            insert.P5 = CDbl(data.Item(6))
                        Catch ex As Exception
                            insert.P5 = 0.0
                        End Try
                        Try
                            insert.P6 = CDbl(data.Item(7))
                        Catch ex As Exception
                            insert.P6 = 0.0
                        End Try
                        Try
                            insert.P7 = CDbl(data.Item(8))
                        Catch ex As Exception
                            insert.P7 = 0.0
                        End Try
                        Try
                            insert.P8 = CDbl(data.Item(9))
                        Catch ex As Exception
                            insert.P8 = 0.0
                        End Try
                        Try
                            insert.P9 = CDbl(data.Item(10))
                        Catch ex As Exception
                            insert.P9 = 0.0
                        End Try
                        Try
                            insert.P10 = CDbl(data.Item(11))
                        Catch ex As Exception
                            insert.P10 = 0.0
                        End Try
                        Try
                            insert.P11 = CDbl(data.Item(12))
                        Catch ex As Exception
                            insert.P11 = 0.0
                        End Try
                        Try
                            insert.P12 = CDbl(data.Item(13))
                        Catch ex As Exception
                            insert.P12 = 0.0
                        End Try




                        insert.FiscalYear = ddlFiscalYear.SelectedValue
                        insert.Error = False
                        insert.Changed = True
                        insert.Portalid = PortalId
                        insert.LastUpdated = Now
                        BudgetImport.Add(insert)
                        Catch ex As Exception
                            errorCount += 1
                            ErrorString &= data.Item(0) & " - " & data.Item(1) & "<br />"
                        End Try

                    End While


                    If errorCount > 0 Then
                        lblError.Text = errorCount & " lines failed to import: " & ErrorString
                    Else
                        lblError.Text = ""
                    End If


                Catch ex As Exception
                    StaffBrokerFunctions.EventLog("Budget", "Import Failed" & ex.ToString, UserId)
                Finally
                    MyConnection.Close()
                End Try




                'Now save these values
                For Each row In BudgetImport
                    Dim q = From c In d.AP_Budget_Summaries Where c.Portalid = row.Portalid And c.Account = row.Account And c.RC = row.RC And c.FiscalYear = ddlFiscalYear.SelectedValue
                    If q.Count > 0 Then
                        If Overwrite Then
                            d.AP_Budget_Summaries.DeleteAllOnSubmit(q)
                            d.AP_Budget_Summaries.InsertOnSubmit(row)
                        Else
                            Try
                                q.First.P1 += row.P1
                            Catch ex As Exception
                            End Try
                            Try
                                q.First.P2 += row.P2
                            Catch ex As Exception
                            End Try
                            Try
                                q.First.P3 += row.P3
                            Catch ex As Exception
                            End Try
                            Try
                                q.First.P4 += row.P4
                            Catch ex As Exception
                            End Try
                            Try
                                q.First.P5 += row.P5
                            Catch ex As Exception
                            End Try
                            Try
                                q.First.P6 += row.P6
                            Catch ex As Exception
                            End Try
                            Try
                                q.First.P7 += row.P7
                            Catch ex As Exception
                            End Try
                            Try
                                q.First.P8 += row.P8
                            Catch ex As Exception
                            End Try
                            Try
                                q.First.P9 += row.P9
                            Catch ex As Exception
                            End Try
                            Try
                                q.First.P10 += row.P10
                            Catch ex As Exception
                            End Try
                            Try
                                q.First.P11 += row.P11
                            Catch ex As Exception
                            End Try
                            Try
                                q.First.P12 += row.P12
                            Catch ex As Exception
                            End Try
                            q.First.LastUpdated = Now
                            q.First.Changed = True
                        End If
                    Else
                        d.AP_Budget_Summaries.InsertOnSubmit(row)
                    End If
                    d.SubmitChanges()
                Next

                GridView1.DataBind()



            Else

            End If
        End Sub

        Public Function FormatCurrency(ByVal amount As Double) As String
            If amount = 0 Then
                Return "-"
            End If
            Dim rtn = amount.ToString("0.00")
            If rtn.EndsWith("00") Then
                Return amount.ToString("#,###,###")
            Else
                Return amount.ToString("#,###,###.00")
            End If
        End Function



        Protected Sub btnImpOverwrite_Click(sender As Object, e As EventArgs) Handles btnImpOverwrite.Click
            Import(True)
        End Sub

        Protected Sub btnImpAddTo_Click(sender As Object, e As EventArgs) Handles btnImpAddTo.Click
            Import(False)
        End Sub

        Protected Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand
            If e.CommandName = "myDelete" Then
                Dim d As New BudgetDataContext
                Dim q = From c In d.AP_Budget_Summaries Where c.BudgetSummaryId = CInt(e.CommandArgument)

                If q.Count > 0 Then
                    If q.First.Error = True And q.First.ErrorMessage.ToLower.Contains("combination") Then
                        d.AP_Budget_Summaries.DeleteAllOnSubmit(q)
                    Else
                        q.First.P1 = 0
                        q.First.P2 = 0
                        q.First.P3 = 0
                        q.First.P4 = 0
                        q.First.P5 = 0
                        q.First.P6 = 0
                        q.First.P7 = 0
                        q.First.P8 = 0
                        q.First.P9 = 0
                        q.First.P10 = 0
                        q.First.P11 = 0
                        q.First.P12 = 0

                    End If
                End If
                d.SubmitChanges()
                GridView1.DataBind()
            End If
        End Sub

        Protected Sub GridView1_RowUpdated(sender As Object, e As GridViewUpdatedEventArgs) Handles GridView1.RowUpdated
            Dim q = From c In d.AP_Budget_Summaries Where c.BudgetSummaryId = CInt(GridView1.DataKeys(GridView1.EditIndex).Value.ToString)

            If q.Count > 0 Then
                q.First.Changed = True
                q.First.Error = False
                q.First.ErrorMessage = ""
                d.SubmitChanges()
                GridView1.DataBind()

            End If
        End Sub

        Protected Sub btnInsertAutoSplit_Click(sender As Object, e As EventArgs) Handles btnInsertAutoSplit.Click
            If tbTotalNew.Text = "" Then
                Return
            End If
            Dim q = From c In d.AP_Budget_Summaries Where c.Portalid = PortalId And c.FiscalYear = ddlFiscalYear.SelectedValue And c.Account = ddlAccountNew.SelectedValue And c.RC = ddlRCNew.SelectedValue



            Dim MonthTotal As Double = CDbl(tbTotalNew.Text) / 12

            If q.Count = 0 Then
                Dim insert As New AP_Budget_Summary
                insert.Portalid = PortalId
                insert.FiscalYear = ddlFiscalYear.SelectedValue
                insert.Account = ddlAccountNew.SelectedValue
                insert.RC = ddlRCNew.SelectedValue
                insert.P1 = MonthTotal
                insert.P2 = MonthTotal
                insert.P3 = MonthTotal
                insert.P4 = MonthTotal
                insert.P5 = MonthTotal
                insert.P6 = MonthTotal
                insert.P7 = MonthTotal
                insert.P8 = MonthTotal
                insert.P9 = MonthTotal
                insert.P10 = MonthTotal
                insert.P11 = MonthTotal
                insert.P12 = MonthTotal
                insert.Changed = True
                insert.LastUpdated = Now
                d.AP_Budget_Summaries.InsertOnSubmit(insert)
                d.SubmitChanges()
                GridView1.DataBind()

                btnCancelInsert_Click(Me, Nothing)


            Else
                tbP1new.Text = MonthTotal
                tbP2new.Text = MonthTotal
                tbP3new.Text = MonthTotal
                tbP4new.Text = MonthTotal
                tbP5new.Text = MonthTotal
                tbP6new.Text = MonthTotal
                tbP7new.Text = MonthTotal
                tbP8new.Text = MonthTotal
                tbP9new.Text = MonthTotal
                tbP10new.Text = MonthTotal
                tbP11new.Text = MonthTotal
                tbP12new.Text = MonthTotal
                If q.First.P1 = 0 And q.First.P2 = 0 And q.First.P3 = 0 And q.First.P4 = 0 And q.First.P5 = 0 And q.First.P6 = 0 And q.First.P7 = 0 And q.First.P8 = 0 And q.First.P9 = 0 And q.First.P10 = 0 And q.First.P11 = 0 And q.First.P12 = 0 Then
                    btnAddTo_Click(Me, Nothing)

                End If




                'Budget already exists... replace or addto.
                WarningRow.Visible = True
                btnInsertRow.Visible = False
                btnInsertAutoSplit.Visible = False

            End If

        End Sub
    End Class
End Namespace
