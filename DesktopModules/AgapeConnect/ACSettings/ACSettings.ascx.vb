Imports System
Imports System.Collections
Imports System.Configuration
Imports System.Data
Imports System.Linq

Imports DotNetNuke
Imports DotNetNuke.Security
Imports StaffBroker
Imports StaffBrokerFunctions


Namespace DotNetNuke.Modules.AgapeConnect
    Partial Class ACSettings
        Inherits Entities.Modules.PortalModuleBase



        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            hfPortalId.Value = PortalId

            lblCurrency.Text = GetSetting("Currency", PortalId)
            Dim d As New DatatSync
            Try
                lblPassword.Text = d.GetPassword()
            Catch ex As Exception
                lblPassword.Text = ""
            End Try

            Dim x = System.Globalization.CultureInfo.CurrentCulture


           


            If GetSetting("Datapump", PortalId) = "Unlocked" Then
                If GetSetting("LastPump", PortalId) = "" Then
                    lblStatus.Text = "OK - but I have no record of a datapump successfully completing"
                Else
                    Dim uiCUL = CType(System.Configuration.ConfigurationManager.GetSection("system.web/globalization"), System.Web.Configuration.GlobalizationSection).UICulture
                    Dim format As IFormatProvider = New System.Globalization.CultureInfo(uiCUL, True)

                    Dim lastPump As Date = Date.Parse(GetSetting("LastPump", PortalId), Format)
                    Dim LastSync As String = ""

                    If DateDiff(DateInterval.Minute, lastPump, Now) < 60 Then
                        lblStatus.Text = "OK - Last sync was " & CInt(DateDiff(DateInterval.Minute, lastPump, Now)) & " minutes ago."
                    ElseIf DateDiff(DateInterval.Hour, lastPump, Now) < 24 Then
                        lblStatus.Text = "WARNING - The last successful sync was " & CInt(DateDiff(DateInterval.Hour, lastPump, Now)) & " hours ago."
                    Else
                        lblStatus.Text = "WARNING - The last successful sync was " & CInt(DateDiff(DateInterval.Day, lastPump, Now)) & " days ago."
                    End If

                End If

            ElseIf GetSetting("Datapump", PortalId).StartsWith("Error") Then
                lblStatus.Text = GetSetting("Datapump", PortalId)
            Else
                lblStatus.Text = "We have not yet received a datapump request"


            End If
        End Sub




        
        'Protected Sub btnPassword_Click(sender As Object, e As System.EventArgs) Handles btnPassword.Click

        'End Sub

        Protected Sub btnOK_Click(sender As Object, e As System.EventArgs) Handles btnOK.Click
            Dim d As New DatatSync
            d.SetPassword()
            GridView1.DataBind()
            lblPassword.Text = d.GetPassword
        End Sub
    End Class
End Namespace
