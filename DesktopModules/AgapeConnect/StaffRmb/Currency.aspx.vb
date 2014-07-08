
Partial Class DesktopModules_AgapeConnect_StaffRmb_Currency
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        ltOut.Text = StaffBrokerFunctions.CurrencyConvert(1, Request.QueryString("FromCur"), Request.QueryString("ToCur"))

    End Sub
End Class
