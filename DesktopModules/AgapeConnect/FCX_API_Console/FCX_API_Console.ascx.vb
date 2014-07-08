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
    Partial Class FCX_API_Console
        Inherits Entities.Modules.PortalModuleBase



        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            hfPortalId.Value = PortalId
            If Not Page.IsPostBack Then
                If StaffBrokerFunctions.GetSetting("TB-KEY", PortalId) = "" Then
                    StaffBrokerFunctions.SetSetting("TB-KEY", Guid.NewGuid.ToString(), PortalId)
                End If
                lblTBKEY.Text = StaffBrokerFunctions.GetSetting("TB-KEY", PortalId)

                tbIP.Text = StaffBrokerFunctions.GetSetting("TB-IP", PortalId)
            End If
          
        End Sub


        Protected Sub DataList1_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataListCommandEventArgs) Handles DataList1.ItemCommand
            Try

            
            Dim d As New FCX.FCXDataContext
            Dim q = From c In d.FCX_API_Keys Where c.DeveloperId = CInt(e.CommandArgument) And c.PortalId = PortalId
            If q.Count > 0 Then
                If e.CommandName = "Enable" Then
                    q.First.Active = True
                ElseIf e.CommandName = "Disable" Then
                    q.First.Active = False

                ElseIf e.CommandName = "ReKey" Then
                    q.First.API_KEY = Guid.NewGuid()
                ElseIf e.CommandName = "SaveITN" Then
                    Dim ITN As TextBox = e.Item.FindControl("tbITN")
                    q.First.ITN = ITN.Text
                ElseIf e.CommandName = "AddWhitelist" Then
                    Dim AW As TextBox = e.Item.FindControl("tbAddWhiteList")
                    q.First.WhiteList &= IIf(String.IsNullOrEmpty(q.First.WhiteList), "", ",") & AW.Text

                End If

                d.SubmitChanges()
                    DataList1.DataBind()

            End If
            Catch ex As Exception

            End Try

        End Sub
        Protected Sub WhilstlistCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)
          
            If e.CommandName = "Remove" Then
                Dim arg = CStr(e.CommandArgument)
                Dim DeveloperId As Integer = arg.Substring(0, arg.IndexOf(";"))
                Dim Value = arg.Substring(arg.IndexOf(";") + 1)


                Dim d As New FCX.FCXDataContext
                Dim q = From c In d.FCX_API_Keys Where c.DeveloperId = DeveloperId And c.PortalId = PortalId
                Dim newWhitelist = ""
                If q.Count > 0 Then

                    For Each row In q.First.WhiteList.Split(",")
                        If row.Trim(",").Trim(" ") <> Value Then
                            newWhitelist &= IIf(String.IsNullOrEmpty(newWhitelist), "", ",") & row.Trim(",").Trim(" ")
                        End If
                    Next
                    q.First.WhiteList = newWhitelist
                    d.SubmitChanges()
                    DataList1.DataBind()
                End If


            End If


        End Sub




        Protected Sub btnSaveIp_Click(sender As Object, e As System.EventArgs) Handles btnSaveIp.Click
            StaffBrokerFunctions.SetSetting("TB-IP", tbIP.Text, PortalId)
        End Sub

        Protected Sub btnTBRekey_Click(sender As Object, e As System.EventArgs) Handles btnTBRekey.Click
            StaffBrokerFunctions.SetSetting("TB-KEY", Guid.NewGuid().ToString, PortalId)
            lblTBKEY.Text = StaffBrokerFunctions.GetSetting("TB-KEY", PortalId)

        End Sub

        Protected Sub btnAddNewApiKey_Click(sender As Object, e As EventArgs) Handles btnAddNewApiKey.Click
            Dim d As New FCX.FCXDataContext
            Dim insert As New FCX.FCX_API_Key
            insert.Active = True
            insert.API_KEY = Guid.NewGuid()
            insert.ProductName = tbNewName.Text
            insert.PortalId = PortalId
            insert.TrustLevel = 1
            insert.FirstName = ""
            insert.LastName = ""
            insert.WhiteList = Request.Url.Host

            d.FCX_API_Keys.InsertOnSubmit(insert)
            d.SubmitChanges()
            DataList1.DataBind()

        End Sub
    End Class
End Namespace
