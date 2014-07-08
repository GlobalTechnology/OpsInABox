Imports System
Imports System.Collections
Imports System.Configuration
Imports System.Data
Imports System.Linq
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.IO
Imports System.Net
Imports DotNetNuke
Imports DotNetNuke.Security

'Imports AgapeStaff
Imports StaffBroker
Imports StaffBrokerFunctions

Namespace DotNetNuke.Modules.AgapeConnect
    Partial Class mpdItem
        Inherits Entities.Modules.PortalModuleBase

        'Public Property Amount() As Double
        '    Get
        '        Return Double.Parse(tbAmount.Text, New CultureInfo("en-US").NumberFormat)
        '    End Get
        '    Set(ByVal value As Double)
        '        tbAmount.Text = value.ToString("n2", New CultureInfo("en-US"))
        '    End Set
        'End Property


        Public Property SectionId() As Integer
            Get
                Return _hfSectionId.Value
            End Get
            Set(ByVal value As Integer)
                hfSectionId.Value = value
            End Set
        End Property



        Public Property Monthly() As String
            Get
                If tbMonthly.Enabled Or tbYearly.Enabled Then
                    Return IIf(tbMonthly.Text = "", 0, tbMonthly.Text)
                Else
                    Return IIf(tbMonthly.Text = "", 0, hfMonthlyCalc.Value)
                End If





            End Get
            Set(ByVal value As String)

                If (value <> "") Then
                    tbYearly.Text = (12 * CDbl(value)).ToString("f0", New CultureInfo("en-US").NumberFormat)
                    tbMonthly.Text = CDbl(value).ToString("f0", New CultureInfo("en-US").NumberFormat)

                End If


            End Set
        End Property

        Private _mode As String
        Public Property Mode() As String
            Get
                Return _mode
            End Get
            Set(ByVal value As String)
                _mode = value
                If value.Contains("MONTH") Then
                    tbMonthly.Enabled = True
                ElseIf value.Contains("YEAR") Then
                    tbYearly.Enabled = True
                ElseIf value.Contains("CALCULATED") Then
                    tbMonthly.CssClass &= " calculated"
                ElseIf (value = "INSERT") Then
                    tbMonthly.Enabled = True
                    _mode = "BASIC_MONTH"
                    pnlDisplay.Visible = False
                    pnlInsert.Visible = IsEditMode()
                    pnlControlGroup.Attributes("class") &= " mpd-insert-mode"
                    btnDelete.Visible = False
                End If

                If value.Contains("NET") Then
                    pnlNetTax.Visible = True
                    pnlNetTax2.Visible = True
                    tbMonthly.CssClass &= " net"
                    tbYearly.CssClass &= " net"
                End If


            End Set
        End Property

        Private _formula As String
        Public Property Formula() As String
            Get
                Return _formula
            End Get
            Set(ByVal value As String)
                _formula = value
                hfFormula.Value = value
            End Set
        End Property


        Public Property Yearly() As String
            Get

                Return IIf(tbYearly.Text = "", 0, tbYearly.Text)
                'Return Double.Parse(tbYearly.Text, New CultureInfo("en-US").NumberFormat)
            End Get
            Set(ByVal value As String)

                If (value <> "") Then
                    tbMonthly.Text = (CDbl(value) / 12).ToString("f0", New CultureInfo("en-US").NumberFormat)
                    tbYearly.Text = CDbl(value).ToString("n2", New CultureInfo("en-US").NumberFormat)
                End If

            End Set
        End Property

        Public Property Min() As String
            Get
                Return ""

            End Get
            Set(ByVal value As String)
                If (value <> "") Then
                    tbMonthly.Attributes.Add("data-min", value)
                    tbYearly.Attributes.Add("data-min", "(" & value & ") * 12")

                End If
                tbMin.Text = value
            End Set
        End Property
        Public Property Max() As String
            Get
                Return ""

            End Get
            Set(ByVal value As String)
                If (value <> "") Then
                    tbMonthly.Attributes.Add("data-max", value)
                    tbYearly.Attributes.Add("data-max", "(" & value & ") * 12")
                End If
                tbMax.Text = value
            End Set
        End Property

        Private _isCurrentSupport As Boolean = False
        Public Property IsCurrentSupport() As Boolean
            Get
                Return False
            End Get
            Set(ByVal value As Boolean)
                _isCurrentSupport = value
                If value = True Then
                    tbMonthly.CssClass &= " currentSupport"
                    'tbMonthly.Attributes("class") &= " currentSupport"

                    tbYearly.Attributes("style") = "display: none"
                    lblCur2.Visible = False

                End If

            End Set
        End Property


        Public Property Tax() As Integer
            Get
                Return IIf(hftax.Value = "", 0, hftax.Value)
            End Get
            Set(ByVal value As Integer)

            End Set
        End Property



        Public Property ItemName() As String
            Get
                Return IIf(lblItemName.Text = "", tbEventName.Text, lblItemName.Text)
            End Get
            Set(ByVal value As String)
                If (value = "") Then
                    lblItemName.Visible = False
                    tbEventName.Visible = True
                ElseIf (value.StartsWith("!!")) Then
                    lblItemName.Visible = False
                    tbEventName.Visible = True
                    tbEventName.Text = value.Substring(2)

                Else
                    lblItemName.Text = value
                End If

            End Set
        End Property

        Public Property Help() As String
            Get
                Return lblHelp.Text
            End Get
            Set(ByVal value As String)
                lblHelp.Text = value
            End Set
        End Property


        Public Property ItemId() As String
            Get
                Return lblItemId.Text
            End Get
            Set(ByVal value As String)
                lblItemId.Text = value
            End Set
        End Property


        Public Property QuestionId() As Integer
            Get
                Return hfQuestionId.Value
            End Get
            Set(ByVal value As Integer)
                hfQuestionId.Value = value
            End Set
        End Property


        Public Property Threshold1() As Nullable(Of Double)
            Get
                Return 0
            End Get
            Set(ByVal value As Nullable(Of Double))
                If Not value Is Nothing Then
                    tbThreshold1.Text = value
                    tbAllowance.Text = value
                End If
            End Set
        End Property
        Public Property Threshold2() As Nullable(Of Double)
            Get
                Return 0
            End Get
            Set(ByVal value As Nullable(Of Double))
                If Not value Is Nothing Then
                    tbThreshold2.Text = value
                End If
            End Set
        End Property
        Public Property Threshold3() As Nullable(Of Double)
            Get
                Return 0
            End Get
            Set(ByVal value As Nullable(Of Double))
                If Not value Is Nothing Then
                    tbThreshold3.Text = value
                End If

            End Set
        End Property
        Public Property Rate1() As Nullable(Of Double)
            Get
                Return 0
            End Get
            Set(ByVal value As Nullable(Of Double))
                If Not value Is Nothing Then
                    tbRate1.Text = value
                    tbRate.Text = value
                End If
            End Set
        End Property
        Public Property Rate2() As Nullable(Of Double)
            Get
                Return 0
            End Get
            Set(ByVal value As Nullable(Of Double))
                If Not value Is Nothing Then
                    tbRate2.Text = value
                    tbAllowanceRate.Text = value
                End If
            End Set
        End Property
        Public Property Rate3() As Nullable(Of Double)
            Get
                Return 0
            End Get
            Set(ByVal value As Nullable(Of Double))
                If Not value Is Nothing Then
                    tbRate3.Text = value
                End If

            End Set
        End Property
        Public Property Rate4() As Nullable(Of Double)
            Get
                Return 0
            End Get
            Set(ByVal value As Nullable(Of Double))
                If Not value Is Nothing Then
                    tbRate4.Text = value
                End If
            End Set
        End Property
        Public Property Fixed() As Double
            Get
                Return 0
            End Get
            Set(ByVal value As Double)
                tbAmount.Text = value
            End Set
        End Property

        Public Property TaxSystem() As String
            Get
                Return 0
            End Get
            Set(ByVal value As String)
                If Not TaxOptions.Items.FindByValue(value) Is Nothing Then
                    TaxOptions.SelectedValue = value
                End If
            End Set
        End Property
        Public Property AccountCode() As String
            Get
                Return ddlAccount.SelectedValue
            End Get
            Set(ByVal value As String)

                If Not ddlAccount.Items.FindByValue(value) Is Nothing Then
                    ddlAccount.SelectedValue = value
                End If

            End Set
        End Property

        Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
            Dim FileName As String = System.IO.Path.GetFileNameWithoutExtension(Me.AppRelativeVirtualPath)
            If Not (Me.ID Is Nothing) Then
                'this will fix it when its placed as a ChildUserControl 
                Me.LocalResourceFile = Me.LocalResourceFile.Replace(Me.ID, FileName)
            Else
                ' this will fix it when its dynamically loaded using LoadControl method 
                Me.LocalResourceFile = Me.LocalResourceFile & FileName & ".ascx.resx"
                Dim Locale = System.Threading.Thread.CurrentThread.CurrentCulture.Name
                Dim AppLocRes As New System.IO.DirectoryInfo(Me.LocalResourceFile.Replace(FileName & ".ascx.resx", ""))
                If Locale = PortalSettings.CultureCode Then
                    'look for portal varient
                    If AppLocRes.GetFiles(FileName & ".ascx.Portal-" & PortalId & ".resx").Count > 0 Then
                        Me.LocalResourceFile = Me.LocalResourceFile.Replace("resx", "Portal-" & PortalId & ".resx")
                    End If
                Else

                    If AppLocRes.GetFiles(FileName & ".ascx." & Locale & ".Portal-" & PortalId & ".resx").Count > 0 Then
                        'lookFor a CulturePortalVarient
                        Me.LocalResourceFile = Me.LocalResourceFile.Replace("resx", Locale & ".Portal-" & PortalId & ".resx")
                    ElseIf AppLocRes.GetFiles(FileName & ".ascx." & Locale & ".resx").Count > 0 Then
                        'look for a CultureVarient
                        Me.LocalResourceFile = Me.LocalResourceFile.Replace("resx", Locale & ".resx")
                    ElseIf AppLocRes.GetFiles(FileName & ".ascx.Portal-" & PortalId & ".resx").Count > 0 Then
                        'lookFor a PortalVarient
                        Me.LocalResourceFile = Me.LocalResourceFile.Replace("resx", "Portal-" & PortalId & ".resx")
                    End If
                End If
            End If

            Dim d As New StaffBrokerDataContext
            ddlAccount.DataSource = From c In d.AP_StaffBroker_AccountCodes Where c.PortalId = PortalId Order By c.AccountCode Select c.AccountCode, Name = c.AccountCode & "-" & c.AccountCodeName

            ddlAccount.DataTextField = "Name"
            ddlAccount.DataValueField = "AccountCode"
            ddlAccount.DataBind()
        End Sub


        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then

                pnlAccountCode.Visible = Not StaffBrokerFunctions.GetSetting("NonDynamics", PortalId) = "True"

                If _mode.Contains("NET") Then
                    lblCur.Text = "NET"
                    lblCur2.Text = "NET"

                Else
                    lblCur.Text = StaffBrokerFunctions.GetSetting("Currency", PortalId)
                    lblCur2.Text = lblCur.Text

                End If


                If IsEditMode() Then
                    btnEdit.Visible = Not _isCurrentSupport



                    ddlMode.SelectedValue = _mode
                    If _mode.Contains("NET_MONTH") Then
                        hfTaxFormula.Value = _formula
                        tbTaxFormula.Text = _formula
                    ElseIf _mode = "CALCULATED" Then

                        tbFormula.Text = _formula
                    End If

                    If lblItemId.Text.Contains(".") Then
                        tbNumber.Text = lblItemId.Text.Substring(lblItemId.Text.IndexOf(".") + 1)
                        lblIdPrefix.Text = lblItemId.Text.Substring(0, lblItemId.Text.IndexOf(".") + 1)
                    End If

                    tbHelp.Text = lblHelp.Text
                    tbName.Text = lblItemName.Text


                End If
            End If
        End Sub







        Public Function Translate(ByVal ResourceString As String) As String
            Return DotNetNuke.Services.Localization.Localization.GetString(ResourceString & ".Text", LocalResourceFile)

        End Function





        Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
            Dim d As New MPD.MPDDataContext


            If QuestionId = -1 And SectionId > 0 Then

                Dim insert As New MPD.AP_mpdCalc_Question
                insert.SectionId = SectionId

                insert.AccountCode = ddlAccount.SelectedValue
                insert.Name = tbName.Text
                insert.QuestionNumber = tbNumber.Text
                insert.Type = ddlMode.SelectedValue

                If insert.Type = "CALCULATED" Then
                    insert.Formula = tbFormula.Text
                ElseIf ddlMode.SelectedValue.Contains("NET") Then
                    insert.TaxSystem = TaxOptions.SelectedValue



                    Select Case TaxOptions.SelectedValue
                        Case "FIXED_RATE"
                            insert.Rate1 = tbRate.Text
                            insert.Formula = hfTaxFormula.Value
                        Case "FIXED_AMOUNT"
                            insert.Fixed = tbAmount.Text
                            insert.Formula = hfTaxFormula.Value
                        Case "ALLOWANCE"
                            insert.Threshold1 = tbAllowance.Text
                            insert.Rate1 = 0.0
                            insert.Rate2 = tbAllowanceRate.Text
                            insert.Formula = hfTaxFormula.Value
                        Case "BANDS"
                            If tbThreshold1.Text <> "" Then
                                insert.Threshold1 = CDbl(tbThreshold1.Text)
                            End If
                            If tbThreshold2.Text <> "" Then
                                insert.Threshold2 = CDbl(tbThreshold2.Text)
                            End If
                            If tbThreshold3.Text <> "" Then
                                insert.Threshold3 = CDbl(tbThreshold3.Text)
                            End If
                            If tbRate1.Text <> "" Then
                                insert.Rate1 = CDbl(tbRate1.Text)
                            End If
                            If tbRate2.Text <> "" Then
                                insert.Rate2 = CDbl(tbRate2.Text)
                            End If
                            If tbRate3.Text <> "" Then
                                insert.Rate3 = CDbl(tbRate3.Text)
                            End If
                            If tbRate4.Text <> "" Then
                                insert.Rate4 = CDbl(tbRate4.Text)
                            End If

                            insert.Formula = hfTaxFormula.Value
                        Case Else
                            insert.Formula = tbTaxFormula.Text

                    End Select



                Else
                    insert.Formula = ""
                End If
                If tbMin.Text = "" Then
                    tbMin.Text = 0
                End If


                insert.Min = tbMin.Text
                insert.Max = tbMax.Text

                d.AP_mpdCalc_Questions.InsertOnSubmit(insert)

            Else
                Dim q = From c In d.AP_mpdCalc_Questions Where c.QuestionId = QuestionId

                If q.Count > 0 Then
                    q.First.Name = tbName.Text
                    q.First.QuestionNumber = tbNumber.Text
                    q.First.Help = tbHelp.Text

                    q.First.Type = ddlMode.SelectedValue
                    If q.First.Type = "CALCULATED" Then
                        q.First.Formula = tbFormula.Text
                    ElseIf ddlMode.SelectedValue.Contains("NET") Then
                        q.First.TaxSystem = TaxOptions.SelectedValue
                        q.First.Threshold1 = Nothing
                        q.First.Threshold2 = Nothing
                        q.First.Threshold3 = Nothing
                        q.First.Rate1 = Nothing
                        q.First.Rate2 = Nothing
                        q.First.Rate3 = Nothing
                        q.First.Rate4 = Nothing
                        q.First.Fixed = Nothing


                        Select Case TaxOptions.SelectedValue
                            Case "FIXED_RATE"
                                q.First.Rate1 = tbRate.Text
                                q.First.Formula = hfTaxFormula.Value
                            Case "FIXED_AMOUNT"
                                q.First.Fixed = tbAmount.Text
                                q.First.Formula = hfTaxFormula.Value
                            Case "ALLOWANCE"
                                q.First.Threshold1 = tbAllowance.Text
                                q.First.Rate1 = 0.0
                                q.First.Rate2 = tbAllowanceRate.Text
                                q.First.Formula = hfTaxFormula.Value
                            Case "BANDS"
                                If tbThreshold1.Text <> "" Then
                                    q.First.Threshold1 = CDbl(tbThreshold1.Text)
                                End If
                                If tbThreshold2.Text <> "" Then
                                    q.First.Threshold2 = CDbl(tbThreshold2.Text)
                                End If
                                If tbThreshold3.Text <> "" Then
                                    q.First.Threshold3 = CDbl(tbThreshold3.Text)
                                End If
                                If tbRate1.Text <> "" Then
                                    q.First.Rate1 = CDbl(tbRate1.Text)
                                End If
                                If tbRate2.Text <> "" Then
                                    q.First.Rate2 = CDbl(tbRate2.Text)
                                End If
                                If tbRate3.Text <> "" Then
                                    q.First.Rate3 = CDbl(tbRate3.Text)
                                End If
                                If tbRate4.Text <> "" Then
                                    q.First.Rate4 = CDbl(tbRate4.Text)
                                End If

                                q.First.Formula = hfTaxFormula.Value
                            Case Else
                                q.First.Formula = tbTaxFormula.Text

                        End Select



                    Else
                        q.First.Formula = ""
                    End If
                    If tbMin.Text = "" Then
                        tbMin.Text = 0
                    End If

                    q.First.AccountCode = ddlAccount.SelectedValue
                    q.First.Min = tbMin.Text
                    q.First.Max = tbMax.Text
                End If
            End If

            d.SubmitChanges()
            Response.Redirect(Request.Url.ToString)
        End Sub

        Protected Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
            Dim d As New MPD.MPDDataContext
            Dim q = From c In d.AP_mpdCalc_Questions Where c.QuestionId = QuestionId


            d.AP_mpdCalc_Questions.DeleteAllOnSubmit(q)

            d.SubmitChanges()
            Response.Redirect(Request.Url.ToString)
        End Sub
    End Class
End Namespace

