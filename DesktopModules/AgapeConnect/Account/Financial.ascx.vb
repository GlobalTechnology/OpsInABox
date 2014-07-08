Imports System.Linq

Namespace DotNetNuke.Modules.tntSuperSite

    Partial Class Financial
        Inherits Entities.Modules.PortalModuleBase

        Dim tnt As New MinistryViewDS.MinistryViewDSServices
        Dim MyAccount As MinistryViewDS.myAccounts
        Dim GoogleGraph As String
        Dim DateRange As String
        Dim i As MinistryViewDS.FinancialTransaction()

        Private Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Init
            Try
                If Session("UID") <> UserId Then
                    Session("UID") = UserId
                    Session("tntSummary") = Nothing
                End If

                If Session("tntSummary") Is Nothing Then
                    IdentifyViaGCXTNT()
                End If
            Catch ex As Exception
                lbltest.Text = "Page_Init " & ex.Message
            End Try
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try
                MyAccount = Session("tntsummary")

                If Not Page.IsPostBack Then
                    If MyAccount.Countries.Count > 0 Then
                        MyCountries.Visible = True
                        MyCountries.DataSource = MyAccount.Countries
                        MyCountries.DataTextField = "Name"
                        MyCountries.DataValueField = "URL"
                        MyCountries.DataBind()
                        MyCountries.SelectedIndex = 0
                        MyCountries_SelectedIndexChanged(Me, Nothing)

                        'Disable the Country dropdown if there is only 1 value.  It is not necessary but provides a more fluid user experience
                        'that a control is not available if it is not needed.
                        'If MyCountries.Attributes.Count = 1 Then
                        'MyCountries.Visible = False
                        'End If

                        For Each row In MyAccount.Countries
                            If row.Profiles.Count = 0 Then
                                MyCountries.Items.FindByValue(row.URL).Enabled = False
                            Else
                                MyCountries.Items.FindByValue(row.URL).Enabled = False
                                For Each row2 In row.Profiles
                                    If row2.Accounts.Count > 0 Then
                                        MyCountries.Items.FindByValue(row.URL).Enabled = True
                                        Exit For

                                    End If
                                Next
                            End If
                        Next
                    Else
                        MyCountries.Visible = False
                        MyProfiles.Visible = False
                        MyAccounts.Visible = False
                    End If
                    MyAccounts_SelectedIndexChanged(Me, Nothing)
                End If
            Catch ex As Exception
                lbltest.Text = "Page Load " & ex.Message
            End Try
        End Sub

        Protected Sub IdentifyViaGCXTNT()
            'Try
            '    Dim dd As New DNNProfile.DNNProfileDataContextDataContext

            '    Dim PGTIOU = From c In dd.UserProfiles Where c.UserID = UserId And c.ProfilePropertyDefinition.PropertyName = "GCXPGTIOU" Select c.PropertyValue
            '    Dim ssoGUID = From c In dd.UserProfiles Where c.UserID = UserId And c.ProfilePropertyDefinition.PropertyName = "ssoGUID" Select c.PropertyValue

            '    'Dim pgt As String = "TGT-752163-JYhv3S0fX4Dpa59MwkTxIoftrThadaZgWNoIPAqdk9n7TlcXfv-cas"
            '    'Dim ssoGUIDHc As String = "2953538D-2175-C702-5FC6-ADF98DA25507"

            '    'Session("tntSummary") = tnt.GetSummary(pgt, ssoGUIDHc)
            '    'Session("ssoGUID") = ssoGUIDHc

            '    If PGTIOU.Count > 0 Then
            '        Try
            '            Dim d As New GCX.GCXDataContext
            '            Dim q = From c In d.Agape_GCX_Proxies Where c.PGTIOU = PGTIOU.First Select c.PGTID

            '            If q.Count > 0 And ssoGUID.Count > 0 Then 'Check that there is a PGTID
            '                Session("tntSummary") = tnt.GetSummary(q.First.ToString(), ssoGUID.First)
            '                Session("ssoGUID") = ssoGUID.First
            '            Else
            '                'Could not find a proxyticketiou  from the database. Probable no proxy ticket - relolgin via gcx
            '                'lblError.Text = "There is a problem. This could be caused by your GCX ticket expiring. Re-log into the website and try again. "
            '            End If
            '        Catch ex As Exception
            '            'lblError.Text = lblError.Text & "<br>" & errorcount & ex.Message
            '        End Try
            '    Else
            '        'NO PGTIOU! Probably not a GCX account
            '        'lblError.Text = lblError.Text & "No ProxyTicketIOU. This could be becuase you did not log into this website using GCX. You must be logged in with GCX to use MinistryView Finance.  "
            '    End If

            'Catch ex As Exception
            '    lbltest.Text = ex.Message
            'End Try
        End Sub

        Protected Sub GetFinancials(ByVal DateFrom As Date, ByVal DateTo As Date)
            Try
                Dim ssoGUID As String
                Dim AccountBalance As Decimal
                Dim PortalCurrency As String

                Dim sc = From c In MyAccount.Countries Where c.URL = MyCountries.SelectedValue Select c.URL, c.Profiles, c.Currency

                If (sc.Count > 0) Then
                    Dim SelectedCountry = sc.First
                    Dim sp = From c In SelectedCountry.Profiles Where c.ProfileCode = MyProfiles.SelectedValue Select c.ProfileCode, c.Accounts

                    If (sp.Count > 0) Then
                        Dim SelProfiles = sp.First

                        PortalCurrency = SelectedCountry.Currency

                        AccountBalance = 0

                        If Session("ssoGUID") = "" Then
                            Return
                        Else
                            ssoGUID = Session("ssoGUID")
                        End If

                        AccountBalance = (From c In SelProfiles.Accounts Where c.AccountID = MyAccounts.SelectedValue Select c.Balance).Sum()

                        Dim FinancialAccountFilter As String = MyAccounts.SelectedValue

                        If FinancialAccountFilter = "All Accounts" Then
                            FinancialAccountFilter = ""
                            AccountBalance = (From c In SelProfiles.Accounts Select c.Balance).Sum()
                        Else
                            AccountBalance = (From c In SelProfiles.Accounts Where c.AccountID = MyAccounts.SelectedValue Select c.Balance).Sum()
                        End If

                        EndingBalance.Text = Decimal.Round(AccountBalance, 2) & " (" & PortalCurrency & ")"

                        Dim FinancialAccount() As MinistryViewDS.FinancialAccount

                        Dim GetFinancialTransactionsService = tnt.GetFinancialTransactions(SelectedCountry.URL, ssoGUID, SelProfiles.ProfileCode, _
                            DateFrom, DateTo, FinancialAccountFilter, False, FinancialAccount)

                        DateRange = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames.GetValue(DateFrom.Month - 1).ToString() & _
                            " " & DateFrom.Year & " - " & _
                            CultureInfo.CurrentCulture.DateTimeFormat.MonthNames.GetValue(DateTo.Month - 1).ToString() & _
                            " " & DateTo.Year

                        If GetFinancialTransactionsService.Count > 0 Then
                            Try
                                Dim CurrentPeriod, CurrentYear, Counter, periodDiff As Integer
                                Dim TotalIncome, TotalExpense, StartingAccountBalance As Decimal

                                Const numMonths As Integer = 13
                                'Dim numMonths As Integer
                                'numMonths = Convert.ToInt32(DateDiff(DateInterval.Month, DateFrom, DateTo))

                                TotalIncome = Decimal.Round((From c In GetFinancialTransactionsService Where c.GLAccountIsIncome = True Select c.Amount).Sum(), 2)
                                TotalExpense = Decimal.Round((From c In GetFinancialTransactionsService Where c.GLAccountIsIncome = False Select c.Amount).Sum(), 2)

                                StartingAccountBalance = Decimal.Round(AccountBalance - TotalExpense - TotalIncome, 2)
                                StartingBalance.Text = StartingAccountBalance.ToString() & " (" & PortalCurrency & ")"

                                CurrentPeriod = 0
                                CurrentYear = 0
                                Counter = 0

                                GoogleGraph = ""

                                Dim ChartData(4, numMonths) As Decimal
                                Dim ieTable As New DataTable

                                ieTable.Columns.Add("Type")

                                Dim ieGroup = From c In GetFinancialTransactionsService _
                                    Group c By c.FiscalYear, c.FiscalPeriod, c.GLAccountIsIncome Into grouping = Group _
                                    Order By FiscalYear, FiscalPeriod, GLAccountIsIncome _
                                    Select FiscalPeriod, FiscalYear, GLAccountIsIncome, monthlySum = grouping.Sum(Function(t) t.Amount)

                                Dim rawdata As String
                                rawdata = ""
                                For Counter = 0 To numMonths - 1
                                    ChartData(1, Counter) = 0
                                    ChartData(2, Counter) = 0
                                    ChartData(3, Counter) = 0
                                Next

                                If ieGroup.Count > 0 Then
                                    CurrentPeriod = ieGroup.First.FiscalPeriod
                                    CurrentYear = ieGroup.First.FiscalYear
                                    ChartData(0, 0) = CurrentPeriod
                                    ieTable.Columns.Add(CurrentPeriod & "/" & CurrentYear)
                                End If

                                Counter = 0

                                For Each row In (ieGroup)
                                    If (CurrentPeriod = row.FiscalPeriod) Then
                                        If (row.GLAccountIsIncome) Then
                                            ChartData(1, Counter) = row.monthlySum
                                        Else
                                            ChartData(2, Counter) = Decimal.Negate(row.monthlySum)
                                        End If
                                    Else
                                        If (ChartData(1, Counter) = Nothing) Then
                                            ChartData(1, Counter) = 0
                                        End If

                                        If (ChartData(2, Counter) = Nothing) Then
                                            ChartData(2, Counter) = 0
                                        End If

                                        If (Counter > 0) Then
                                            ChartData(3, Counter) = ChartData(3, Counter - 1) + ChartData(1, Counter) - ChartData(2, Counter)
                                        Else
                                            ChartData(3, Counter) = StartingAccountBalance + ChartData(1, Counter) - ChartData(2, Counter)
                                        End If

                                        'Skip months with no income and expense data
                                        If (CurrentYear <> row.FiscalYear) Then
                                            periodDiff = 12 - CurrentPeriod + row.FiscalPeriod
                                        Else
                                            periodDiff = (row.FiscalPeriod - CurrentPeriod)
                                        End If

                                        'Fill in months with no income and expense
                                        For c = (Counter + 1) To (Counter + periodDiff - 1)
                                            ChartData(3, c) = ChartData(3, c - 1)
                                        Next

                                        Counter += periodDiff

                                        ChartData(0, Counter) = row.FiscalPeriod
                                        CurrentPeriod = row.FiscalPeriod
                                        CurrentYear = row.FiscalYear

                                        ieTable.Columns.Add(row.FiscalPeriod & "/" & row.FiscalYear)

                                        If (row.GLAccountIsIncome) Then
                                            ChartData(1, Counter) = row.monthlySum
                                        Else
                                            ChartData(2, Counter) = Decimal.Negate(row.monthlySum)
                                        End If
                                    End If
                                Next

                                If Counter > 1 Then
                                    For c = Counter To numMonths - 1
                                        ChartData(0, c) = ChartData(0, c - 1) + 1
                                        ChartData(3, c) = ChartData(3, c - 1) + ChartData(1, c) - ChartData(2, c)
                                    Next
                                End If

                                Try
                                    For r = 0 To 2
                                        ieTable.Rows.Add()
                                        'Adds the first column of "data" which is the row descriptions
                                        Select Case r
                                            Case 0
                                                ieTable.Rows(r)(0) = "Income"
                                            Case 1
                                                ieTable.Rows(r)(0) = "Expenses"
                                            Case 2
                                                ieTable.Rows(r)(0) = "Ending Balance"
                                        End Select

                                        For cols = 1 To numMonths
                                            ieTable.Rows(r)(cols) = Decimal.Round(ChartData(r + 1, cols - 1), 2)
                                        Next
                                    Next

                                    'IEBReport.DataSource = ieTable
                                    'IEBReport.DataBind()
                                Catch ex As Exception
                                    'lbltest.Text = ex.Message & " - " & ex.StackTrace
                                End Try

                                GoogleGraph = ""
                                For Counter = 0 To numMonths - 1
                                    GoogleGraph = GoogleGraph & "data.addRow(['" & ChartData(0, Counter) & "', " & ChartData(1, Counter) & ", " & ChartData(2, Counter) & ", " & ChartData(3, Counter) & "]);" & vbCrLf
                                Next
                            Catch ex As Exception
                                lbltest.Text = ex.Message & " - " & ex.StackTrace
                            End Try


                            FinancialIncomeSummary.DataSource = From c In GetFinancialTransactionsService _
                                Where c.GLAccountIsIncome = True _
                                Group c By c.GLAccountDescription Into g = Group _
                                Order By GLAccountDescription _
                                Select New With {.glAccount = GLAccountDescription, _
                                                 .glSubTotal = Decimal.Round(g.Sum(Function(t) t.Amount), 2), _
                                                 .glTransactions = g}

                            FinancialIncomeSummary.DataBind()

                            FinancialExpenseSummary.DataSource = From c In GetFinancialTransactionsService _
                                Where c.GLAccountIsIncome = False _
                                Group c By c.GLAccountDescription Into g = Group _
                                Order By GLAccountDescription _
                                Select New With {.glAccount = GLAccountDescription, _
                                                 .glSubTotal = Decimal.Round(g.Sum(Function(t) t.Amount), 2), _
                                                 .glTransactions = g}

                            FinancialExpenseSummary.DataBind()
                        End If
                    
                Else
                    lbltest.Text = "No Profile setup."
                End If
                Else
                lbltest.Text = "No Country setup."
                End If
            Catch ex5 As Exception
                lbltest.Text = "All " & ex5.StackTrace
            End Try
        End Sub

        Public Function GetGoogleData() As String
            Return GoogleGraph
        End Function

        Public Function GetDateRange() As String
            Return DateRange
        End Function

        Protected Sub MyCountries_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyCountries.SelectedIndexChanged
            Try
                Dim q = (From c In MyAccount.Countries Where c.URL = MyCountries.SelectedValue Select c.Profiles).First

                Dim r = From c In q Where c.Accounts.Count > 0 Order By c.ProfileCode = "" Descending, c.ProfileName Ascending
                If r.Count > 0 Then

                    MyProfiles.Visible = True
                    MyProfiles.DataSource = r
                    MyProfiles.DataTextField = "ProfileName"
                    MyProfiles.DataValueField = "ProfileCode"
                    MyProfiles.DataBind()
                    'For Each row In r
                    '    If row.Donations.Count = 0 Then
                    '        MyProfiles.Items.FindByValue(row.ProfileCode).Enabled = False
                    '    End If
                    'Next
                    MyProfiles_SelectedIndexChanged(Me, Nothing)
                Else
                    MyProfiles.Visible = False
                    MyAccounts.Visible = False
                End If

                MyProfiles_SelectedIndexChanged(Me, Nothing)
            Catch ex As Exception
                lbltest.Text = "Country Changed " & ex.Message
            End Try
        End Sub

        Protected Sub MyProfiles_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyProfiles.SelectedIndexChanged
            Try
                Dim SelectedProfile = (From c In MyAccount.Countries Where c.URL = MyCountries.SelectedValue Select c.Profiles)
                Dim q = (From c In SelectedProfile.First Where c.ProfileCode = MyProfiles.SelectedValue Select c.Accounts).First
                If q.Count > 0 Then

                    MyAccounts.Visible = True
                    MyAccounts.DataSource = q
                    MyAccounts.DataTextField = "Description"
                    MyAccounts.DataValueField = "AccountID"
                    MyAccounts.DataBind()

                    'Add "All Accounts" if there are more than 1
                    If q.Count > 1 Then
                        MyAccounts.Items.Insert(0, "All Accounts")
                    End If
                Else
                    MyAccounts.Visible = False

                End If

                MyAccounts_SelectedIndexChanged(Me, Nothing)
            Catch ex As Exception
                lbltest.Text = "Profile Changed " & ex.Message
            End Try
        End Sub

        Protected Sub MyAccounts_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyAccounts.SelectedIndexChanged
            Try
                Dim BeginTemp As Date = CDate(DateAdd(DateInterval.Month, -13, Today))

                'Dim BeginMonth = CDate("1/" & BeginTemp.Month & "/" & BeginTemp.Year)
                Dim BeginMonth = CDate(BeginTemp.Month & "/1/" & BeginTemp.Year)
                Dim EndofMonth = Today
                'lblCostCenter.Text = MyAccounts.SelectedItem.Text
                GetFinancials(BeginMonth, EndofMonth)
            Catch ex As Exception
                lbltest.Text = "Account Changed " & ex.Message
            End Try

        End Sub

        'Protected Sub btnDonors_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDonors.Click
        '    Response.Redirect(EditUrl("Donors"))
        'End Sub

        Protected Sub btnHome_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHome.Click
            Response.Redirect(NavigateURL())
        End Sub

        'Protected Sub btnDonations_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDonations.Click
        '    Response.Redirect(EditUrl("Donations"))
        'End Sub
    End Class
End Namespace

