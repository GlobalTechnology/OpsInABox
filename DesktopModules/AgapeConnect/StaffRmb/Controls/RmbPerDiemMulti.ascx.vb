
Partial Class controls_RmbPerDiemMulti
    Inherits Entities.Modules.PortalModuleBase
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


    Public Property Comment() As String
        Get
            Return tbDesc.Text
        End Get
        Set(ByVal value As String)
            tbDesc.Text = value
        End Set
    End Property
    Public Property theDate() As Date
        Get
            Return CDate(dtDate.Text)
        End Get
        Set(ByVal value As Date)
            If value = Nothing Then
                dtDate.Text = Today.ToShortDateString
            Else
                dtDate.Text = value
            End If
        End Set
    End Property
    Public Property VAT() As Boolean
        Get
            Return False
        End Get
        Set(ByVal value As Boolean)



        End Set
    End Property
    Public Property Taxable() As Boolean
        Get
            Return False
        End Get
        Set(ByVal value As Boolean)

        End Set
    End Property
    Public Property Amount() As Double
        Get
            Return IIf(lblMaxAmt.Text = "", 0, lblMaxAmt.Text)
            

        End Get
        Set(ByVal value As Double)

            lblMaxAmt.Text = value.ToString("n2", New CultureInfo("en-US"))
        End Set
    End Property
    Public Property Spare1() As String
        Get
            Return DropDownList1.SelectedValue
        End Get
        Set(ByVal value As String)
            If value = "" Then
                DropDownList1.SelectedValue = 1
            Else
                DropDownList1.SelectedValue = value
            End If
        End Set
    End Property
    Public Property Spare2() As String
        Get
            Return DropDownList3.SelectedValue
        End Get
        Set(ByVal value As String)
            If value = "" Then
                DropDownList3.SelectedValue = 1
            Else
                Try
                    If (From c As ListItem In DropDownList3.Items Where c.Value = value).Count > 0 Then
                        DropDownList3.SelectedValue = value
                    End If
                Catch ex As Exception

                End Try
               

            End If
        End Set
    End Property
    Public Property Spare3() As String
        Get
            Return DropDownList2.SelectedValue

        End Get
        Set(ByVal value As String)
            If value = "" Or (From c As ListItem In DropDownList2.Items Where c.Value = value).Count = 0 Then
                DropDownList2.SelectedIndex = 1
            Else
                DropDownList2.SelectedValue = value
            End If
        End Set
    End Property
    Public Property Spare4() As String
        Get
            Return Nothing
        End Get
        Set(ByVal value As String)

        End Set
    End Property
    Public Property Spare5() As String
        Get
            Return Nothing
        End Get
        Set(ByVal value As String)

        End Set
    End Property
    Public Property Receipt() As Boolean
        Get
            Return False
        End Get
        Set(ByVal value As Boolean)

        End Set
    End Property
    Public Property ErrorText() As String
        Get
            Return ""
        End Get
        Set(ByVal value As String)
            ErrorLbl.Text = value
        End Set
    End Property
    Public Function ValidateForm(ByVal userId As Integer) As Boolean
        If tbDesc.Text = "" Then
            ErrorLbl.Text = DotNetNuke.Services.Localization.Localization.GetString("Description.Error", LocalResourceFile)
            Return False
        End If
        Try
            Dim theDate As Date = dtDate.Text
            If theDate > Today Then
                ErrorLbl.Text = DotNetNuke.Services.Localization.Localization.GetString("OldDate.Error", LocalResourceFile)
                Return False
            End If
        Catch ex As Exception
            ErrorLbl.Text = DotNetNuke.Services.Localization.Localization.GetString("Date.Error", LocalResourceFile)
            Return False
        End Try

        Try
            Dim theAmount As Double = Double.Parse(lblMaxAmt.Text, New CultureInfo("en-US").NumberFormat)
           
            If theAmount <= 0 Then
                ErrorLbl.Text = DotNetNuke.Services.Localization.Localization.GetString("Amount.Error", LocalResourceFile)
                Return False
            End If


        Catch ex As Exception
            ErrorLbl.Text = DotNetNuke.Services.Localization.Localization.GetString("Amount.Error", LocalResourceFile)
            Return False
        End Try
        ErrorLbl.Text = ""
        Return True
    End Function

    Public Sub Initialize(ByVal Settings As Hashtable)
        'DropDownList2.Items(0).Value = Settings("SubDay")
        'DropDownList2.Items(1).Value = Settings("SubBreakfast")
        'DropDownList2.Items(2).Value = Settings("SubLunch")
        'DropDownList2.Items(3).Value = Settings("SubDinner")
        
        

        Dim d As New StaffRmb.StaffRmbDataContext

        DropDownList2.DataSource = From c In d.AP_Staff_Rmb_PerDeimMuliTypes Where c.PortalId = PortalId Select c.Name, Value = c.Value & "-" & c.Currency

        DropDownList2.DataTextField = "Name"
        DropDownList2.DataValueField = "Value"

        DropDownList2.DataBind()
        Try

        
        If (Not String.IsNullOrEmpty(Settings("DescriptionLength"))) And CInt(Settings("DescriptionLength")) > 0 Then
            tbDesc.Attributes("maxLength") = CInt(Settings("DescriptionLength"))
            If CInt(Settings("DescriptionLength")) < 50 Then
                tbDesc.Columns = CInt(Settings("DescriptionLength")) + 3
                tbDesc.Width = Nothing
            End If
            End If
        Catch ex As Exception

        End Try

    End Sub

    Protected Sub UpdatePanel1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles UpdatePanel1.PreRender
        Dim mc As New MobileCAS
        Dim x = DropDownList2.SelectedValue.Split("-")
        Dim y = mc.ConvertCurrency(x(1).Trim("-"), StaffBrokerFunctions.GetSetting("AccountingCurrency", PortalId))
        lblMaxAmt.Text = (CDbl(x(0).Trim("-")) * DropDownList1.SelectedValue * y * DropDownList3.SelectedValue).ToString("0.00")
        ' Dim x = DropDownList2.SelectedValue.Split("-")
        'Dim y = StaffBrokerFunctions.CurrencyConvert(CDbl(x(0).Trim("-")), x(1).Trim("-"), StaffBrokerFunctions.GetSetting("AccountingCurrency", PortalId))
        'lblMaxAmt.Text = y
        'lblMaxAmt.Text = (DropDownList1.SelectedValue * y * DropDownList3.SelectedValue).ToString("0.00")


    End Sub
End Class


