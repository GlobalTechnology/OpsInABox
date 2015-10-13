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
Imports UK.OnlineForm


Namespace DotNetNuke.Modules.OnlineForm

    Partial Class ResultsOnlineForm
        Inherits Entities.Modules.PortalModuleBase
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            ModuleHF.Value = Me.ModuleId

            Dim d As New OnlineFormDataContext


            If Not Page.IsPostBack Then



                Dim q = (From c In d.Agape_Public_OnlineForms Where c.ModuleId = Me.ModuleId Select c).First
                'FormIdHF.Value = q.FormId

                TablePH.Controls.Add(New LiteralControl("<tr><td width=""80"" style=""font-size: 10pt; font-weight: bolder"">User</td>"))
                Dim questions = From c In d.Agape_Public_OnlineForm_Questions Where c.FormId = q.FormId
                For Each qu In questions
                    TablePH.Controls.Add(New LiteralControl("<td width=""80"" style=""font-size: 10pt; font-weight: bolder"">" & qu.QuestionText & "</td>"))
                Next
                TablePH.Controls.Add(New LiteralControl("</tr>"))

                Dim Answers = From c In d.Agape_Public_OnlineForm_Answers Where c.FormId = q.FormId
                Dim currentSet As Integer = 0
                Dim DN As String
                For Each Answer In Answers
                    If currentSet = 0 Then
                        Try
                            Dim UID = Answer.UserId
                            DN = (From c In d.Users Where UserId = UID).First.DisplayName

                        Catch ex As Exception
                            DN = "Unknown"
                        End Try
                        TablePH.Controls.Add(New LiteralControl("<tr><td>" & DN & "<br />" & Answer.UserIP & "</td><td>"))


                    ElseIf Not Answer.AnswerSet = currentSet Then
                        Try
                            Dim UID = Answer.UserId
                            DN = (From c In d.Users Where UserId = UID).First.DisplayName

                        Catch ex As Exception
                            DN = "Unknown"
                        End Try
                        TablePH.Controls.Add(New LiteralControl("</tr><tr><td>" & DN & "<br />" & Answer.UserIP & "</td><td>"))
                    Else
                        TablePH.Controls.Add(New LiteralControl("<td>"))
                    End If

                    TablePH.Controls.Add(New LiteralControl(Answer.AnswerText))
                    TablePH.Controls.Add(New LiteralControl("</td>"))
                        currentSet = Answer.AnswerSet
                Next
                TablePH.Controls.Add(New LiteralControl("</tr>"))




            End If
        End Sub

      
        Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ReturnButton.Click
            Response.Redirect(NavigateURL())

        End Sub
    End Class
End Namespace
