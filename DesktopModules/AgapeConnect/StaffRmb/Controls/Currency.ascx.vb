
Partial Class DesktopModules_AgapeConnect_StaffRmb_Controls_Currency
    Inherits Entities.Modules.PortalModuleBase

    Private _advMode As Boolean
    Public Property AdvMode() As Boolean
        Get
            Return _advMode
        End Get
        Set(ByVal value As Boolean)
            _advMode = value
        End Set
    End Property
    Private _advPayoffMode As Boolean
    Public Property AdvPayOffMode() As Boolean
        Get
            Return _advPayoffMode
        End Get
        Set(ByVal value As Boolean)
            _advPayoffMode = value
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
    End Sub
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        If Not ((AdvMode Or AdvPayOffMode) And Page.IsPostBack) Then


            Dim suffix = ""
            Dim amount = "rmbAmount"
            If AdvMode And Not btnCurrency.CssClass.EndsWith("Adv") Then
                btnCurrency.CssClass &= "Adv"
                tbCurrency.Attributes("class") &= "Adv"
                ddlCurrencies.Attributes("class") &= "Adv"
                dCurrency.Attributes("class") &= "Adv"
                suffix = "Adv"
                amount = "advAmount"
            ElseIf AdvPayOffMode And Not btnCurrency.CssClass.EndsWith("AdvPO") Then
                btnCurrency.CssClass &= "AdvPO"
                tbCurrency.Attributes("class") &= "AdvPO"
                ddlCurrencies.Attributes("class") &= "AdvPO"
                dCurrency.Attributes("class") &= "AdvPO"
                suffix = "AdvPO"
                amount = "advPOAmount"
            End If



            Dim PS = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)
            Dim lc = StaffBrokerFunctions.GetSetting("LocalCurrency", PS.PortalId)
            Dim ac = StaffBrokerFunctions.GetSetting("AccountingCurrency", PS.PortalId)
            Dim x As New MobileCAS
            Dim xrate = x.ConvertCurrency(lc, ac)
            If lc = "" Then
                StaffBrokerFunctions.SetSetting("LocalCurrency", "USD", PS.PortalId)
                lc = "USD"
            End If
            ddlCurrencies.SelectedValue = lc




            'sb.Append(" var jsonCall= ""/MobileCAS/MobileCAS.svc/ConvertCurrency?FromCur=" + lc + "&ToCur=" + ac + """;")
            ' sb.Append("$.getJSON( jsonCall ,function(x) { setXRate(x);});")



            ' sb.Append(" $('.ddlCur" & suffix & "').change();")
            'sb.Append("setXRate(" & xrate & ");")



            If StaffBrokerFunctions.GetSetting("CurConverter", PortalId) = "True" Then

                Dim t As Type = Me.GetType()
                Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()
                sb.Append("<script language='javascript'>")
                If suffix = "" Then
                    sb.Append(" var tempValue=$('." & amount & "').val();   $('.ddlCur" & suffix & "').change(); $('." & amount & "').val(tempValue);   $('.currency" & suffix & "').val((parseFloat(tempValue) / parseFloat(" & xrate.ToString("n8", New CultureInfo("en-US")) & ")).toFixed(2));  $('.divCur" & suffix & "').show(); $('#" & btnCurrency.ClientID & "').hide();")
                Else
                    sb.Append("$('.divCur" & suffix & "').show(); $('#" & btnCurrency.ClientID & "').hide();")

                End If

                sb.Append("$('.hfCurOpen').val('true');")
                sb.Append("</script>")
                ScriptManager.RegisterStartupScript(Page, t, "cur" & suffix, sb.ToString, False)
            End If


        End If
    End Sub
End Class
