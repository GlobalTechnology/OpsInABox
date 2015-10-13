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
Imports System.Xml

Imports UK.OnlineForm

Namespace DotNetNuke.Modules.OnlineForm



    Partial Class ViewONlineForm
        Inherits Entities.Modules.PortalModuleBase
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            If IsEditable Then
                EditButton.Visible = True
                ResultsButton.Visible = True
            End If
            If True Then

                Dim d As New OnlineFormDataContext
                Try
                    Dim q = (From c In d.Agape_Public_OnlineForms Where c.ModuleId = Me.ModuleId Select c).First
                    PrefixLabel.Text = q.Intro
                    SuffixLabel.Text = q.FootNote
                    EmailPanel.Visible = q.Ack
                    ReqEmail.Enabled = q.ReqEmail
                    SubmitButton.Visible = True

                    If UserInfo.Email <> "" And Not Page.IsPostBack Then
                        Email.Text = UserInfo.Email
                    End If

                    Dim questions = From c In d.Agape_Public_OnlineForm_Questions Where c.FormId = q.FormId

                    For Each question In questions
                        QuPlaceHolder.Controls.Add(New LiteralControl("<tr><td valign=""top"" style=""font-weight: bolder; font-size: 10pt;"" width=""350"">" & question.QuestionText & "</td><td>"))
                        Select Case question.QuestionType
                            Case 0 'TextBox
                                Dim c As New TextBox()
                                c.ID = "Q" & question.FormQuestionId
                                c.Width = 300
                                QuPlaceHolder.Controls.Add(c)

                            Case 1 ' Multiline Text Box
                                Dim c As New TextBox()
                                c.ID = "Q" & question.FormQuestionId
                                c.TextMode = TextBoxMode.MultiLine
                                c.Rows = 5
                                c.Width = 300
                                QuPlaceHolder.Controls.Add(c)
                            Case 2 ' Yes/No
                                Dim c As New DropDownList
                                c.ID = "Q" & question.FormQuestionId
                                c.Items.Add("No")
                                c.Items.Add("Yes")

                                c.DataBind()
                                c.SelectedValue = "Yes"
                                QuPlaceHolder.Controls.Add(c)
                            Case 3 ' DropDownList
                                Dim c As New DropDownList
                                c.ID = "Q" & question.FormQuestionId
                                Dim qid As Integer = question.FormQuestionId
                                Dim ddl = From b In d.Agape_Public_OnlineForm_DDLs Where b.QuestionId = qid
                                For Each row In ddl
                                    c.Items.Add(New ListItem(row.RowText))

                                Next
                                c.DataBind()
                                QuPlaceHolder.Controls.Add(c)
                            Case 4 'checkbox
                                Dim c As New CheckBox
                                c.ID = "Q" & question.FormQuestionId
                                QuPlaceHolder.Controls.Add(c)


                            Case 5 'radiobutton
                                Dim c As New RadioButtonList
                                c.ID = "Q" & question.FormQuestionId
                                Dim qid As Integer = question.FormQuestionId
                                Dim ddl = From b In d.Agape_Public_OnlineForm_DDLs Where b.QuestionId = qid
                                For Each row In ddl
                                    c.Items.Add(New ListItem(row.RowText))

                                Next
                                c.DataBind()
                                QuPlaceHolder.Controls.Add(c)
                        End Select
                        If (question.Required) Then
                            Dim req As New RequiredFieldValidator()
                            req.ID = "req" & question.FormQuestionId
                            req.ControlToValidate = "Q" & question.FormQuestionId
                            req.Text = "* Required."
                            req.ErrorMessage = "Please give an answer to this question: " & question.QuestionText
                            QuPlaceHolder.Controls.Add(req)
                            'SubmitButton.Visible = False

                        End If

                        QuPlaceHolder.Controls.Add(New LiteralControl("</td></tr>"))


                    Next



                Catch ex As Exception
                    PrefixLabel.Text = "This module has not yet been configured..."
                    SuffixLabel.Text = ""
                    SubmitButton.Visible = False
                End Try

            End If

        End Sub

        Protected Sub EditButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles EditButton.Click
            Response.Redirect(EditUrl("Edit"))
        End Sub

        Protected Sub SubmitButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SubmitButton.Click
            Dim Firstname As String = ""
            Dim Lastname As String = ""

            Dim d As New OnlineFormDataContext
            Dim q = (From b In d.Agape_Public_OnlineForms Where b.ModuleId = Me.ModuleId).First
            Dim questions = From b In d.Agape_Public_OnlineForm_Questions Where b.FormId = q.FormId
            Dim Answerset As Integer = 1
            Try
                Answerset = (From b In d.Agape_Public_OnlineForm_Answers Order By b.AnswerId Descending).First.AnswerSet + 1
            Catch ex As Exception
            End Try
            


            For Each question In questions
                Dim answer As New Agape_Public_OnlineForm_Answer
                Select Case question.QuestionType
                    Case 0 To 1 'TextBox
                        Dim c As TextBox = FindControl("Q" & question.FormQuestionId)
                        answer.AnswerText = c.Text
                        If question.QuestionText = "First Name" Then
                            Firstname = c.Text
                        ElseIf question.QuestionText = "Last Name" Then
                            Lastname = c.Text
                        End If

                    Case 2 To 3 ' Yes/No
                        Dim c As DropDownList = FindControl("Q" & question.FormQuestionId)

                        answer.AnswerText = c.SelectedValue
                    Case 4
                        Dim c As CheckBox = FindControl("Q" & question.FormQuestionId)
                        answer.AnswerText = IIf(c.Checked, "Yes", "No")
                    Case 5
                        Dim c As RadioButtonList = FindControl("Q" & question.FormQuestionId)
                        answer.AnswerText = c.SelectedValue
                End Select
                Try
                    answer.UserId = Me.UserId
                Catch ex As Exception

                End Try
                answer.AnswerSet = Answerset
                answer.DateSubmitted = Date.Now
                answer.FormId = q.FormId
                answer.Questionid = question.FormQuestionId
                answer.UserIP = Request.ServerVariables("remote_addr")
                d.Agape_Public_OnlineForm_Answers.InsertOnSubmit(answer)
                d.SubmitChanges()

            Next
            Dim message As String = "The following form was submitted via the Agap&eacute; website: <br />"

            If Not String.IsNullOrEmpty(q.EmailTo) Then
                Try
                    message = message & "From: " & Me.UserInfo.DisplayName & "<br />"
                Catch ex As Exception
                End Try
                If Email.Text = "" Then
                    message = message & "Email: Not Supplied<br />"
                Else
                    message = message & "Email: " & Email.Text & "<br />"
                End If
                message = message & "IP Address: " & Request.ServerVariables("remote_addr") & "<br />"
                message = message & "Date Submitted: " & Date.Now.ToString & "<br />"

                message = message & "<table cellpadding=""10"">"
                Dim answers = From b In d.Agape_Public_OnlineForm_Answers Where b.AnswerSet = Answerset
                For Each row In answers
                    Dim qid = row.Questionid
                    message = message & "<tr><td valign=""top"" style=""font-weight: bolder;"" width=""200""> "
                    message = message & (From b In d.Agape_Public_OnlineForm_Questions Where b.FormQuestionId = qid).First.QuestionText & "</td><td>"
                    
                    message = message & row.AnswerText

                    message = message & "</td></tr>"





                Next


                message = message & "</table>"


                'Dim myMessage As New MailMessage
                'Dim mySmtpClient As SmtpClient

                'myMessage.From = New MailAddress("donotreply@agape.org.uk", "Agape Online Form")
                'myMessage.To.Add(New MailAddress(q.EmailTo))
                'myMessage.Subject = "Online Form - " & ModuleConfiguration.ModuleTitle
                'myMessage.IsBodyHtml = True
                'myMessage.Body = message


                'mySmtpClient = New SmtpClient("62.105.76.34")
                'mySmtpClient = New SmtpClient("mail.agape.org.uk")
                'mySmtpClient.Credentials = New System.Net.NetworkCredential("SendMail", "easc2002", "ds3079.dedicated.turbodns.co.uk")
                'mySmtpClient.UseDefaultCredentials = False
                'mySmtpClient.Send(myMessage)
                DotNetNuke.Services.Mail.Mail.SendMail("donotreply@agape.org.uk", q.EmailTo, "", "Online Form - " & ModuleConfiguration.ModuleTitle, message, "", "HTML", "", "", "", "")

            End If


            'If Not q.RDSLoginName Is Nothing Then
            '    If q.RDSLoginName <> "" Then
            '        'Also Send this form to RDS via the web service
            '        Dim RDSId = CheckAddSimpleUser(Firstname, Lastname, Email.Text)
            '        Dim RDSM As New RDSContact.GenericMessageWebserviceService

            '        Dim msg As String = ""
            '        Dim answers = From b In d.Agape_Public_OnlineForm_Answers Where b.AnswerSet = Answerset

            '        For Each row In answers
            '            Dim qid = row.Questionid
            '            msg &= row.Agape_Public_OnlineForm_Question.QuestionText & ":   " & row.AnswerText & vbNewLine
            '        Next
            '        RDSM.addMessage(RDSId, msg, "RDS Test", "webservicedemo", "wsd1234")

            '    End If
            'End If





            Try

                If q.Ack And Email.Text <> "" Then
                    message = q.AckText

                    message = message & "<table cellpadding=""10"">"
                    Dim answers = From b In d.Agape_Public_OnlineForm_Answers Where b.AnswerSet = Answerset
                    For Each row In answers
                        Dim qid = row.Questionid
                        message = message & "<tr><td valign=""top"" style=""font-weight: bolder;"" width=""200""> "
                        message = message & (From b In d.Agape_Public_OnlineForm_Questions Where b.FormQuestionId = qid).First.QuestionText & "</td><td>"
                        message = message & row.AnswerText
                        message = message & "</td></tr>"
                    Next


                    message = message & "</table>"


                    'Dim myMessage As New MailMessage
                    'Dim mySmtpClient As SmtpClient

                    'myMessage.From = New MailAddress("donotreply@agape.org.uk", "Agape Online Form")
                    'myMessage.To.Add(New MailAddress(Email.Text))
                    'myMessage.Subject = "Thank you for contacting Agapé."
                    'myMessage.IsBodyHtml = True
                    'myMessage.Body = message


                    'mySmtpClient = New SmtpClient("62.105.76.34")
                    'mySmtpClient = New SmtpClient("mail.agape.org.uk")
                    'mySmtpClient.Credentials = New System.Net.NetworkCredential("SendMail", "easc2002", "ds3079.dedicated.turbodns.co.uk")
                    'mySmtpClient.UseDefaultCredentials = False
                    'mySmtpClient.Send(myMessage)
                    DotNetNuke.Services.Mail.Mail.SendMail("donotreply@agape.org.uk", Email.Text, "", "Thank you for contacting Agapé.", message, "", "HTML", "", "", "", "")

                End If

            Catch ex As Exception

            End Try


            SentLabel.Text = "Thank you! Your message has been sent. You will soon hear from us."
            SentLabel.Visible = True

            For Each ctrl In QuPlaceHolder.Controls
                If TypeOf ctrl Is TextBox Then

                    ctrl.Text = ""
                End If

            Next
            Email.Text = ""



        End Sub

        Public Function CheckAddSimpleUser(ByVal FirstName As String, ByVal LastName As String, ByVal Email As String) As Integer
            'Dim r1 As String = CheckEmail(Email)

            'Try
            '    Return CInt(r1)
            'Catch ex As Exception
            '    If r1.StartsWith("Cannot find contact") Then
            '        'Create the contact
            '        Dim CWS As New RDSContact.ContactWebserviceService

            '        Return CWS.saveSimpleContact("webservicedemo", "wsd1234", "FirstName", "LastName", "JonVellacott@agape.org.uk", False)
            '    Else
            '        'raise the error again - but this time don't catch it - it really is an error
            '        Return CInt(r1)
            '    End If
            'End Try

            Return 0
        End Function


        Public Function CheckEmail(ByVal email As String) As String

            Dim PostURL = "http://demo.rdsnetwork.org/RdsServer/webservices/contact/searchByEmail?username=webservicedemo&password=wsd1234&email=" & email
            Dim Reader1 As StreamReader = New StreamReader(New WebClient().OpenRead(PostURL))
            Dim doc As New XmlDocument()
            doc.Load(Reader1)
            Dim NamespaceMgr As New XmlNamespaceManager(doc.NameTable)
            NamespaceMgr.AddNamespace("ns1", "http://webservices.rds.cvc.tv/")
            NamespaceMgr.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/")
            Try
                Dim Values = doc.SelectSingleNode("soap:Envelope/soap:Body/ns1:searchByEmailResponse/ns1:result", NamespaceMgr)
                Return Values.LastChild.InnerText

            Catch ex As Exception

            End Try
            Try
                Return doc.SelectSingleNode("soap:Envelope/soap:Body/soap:Fault/faultstring", NamespaceMgr).InnerText
            Catch ex2 As Exception
                Return ex2.Message

            End Try
        End Function


        Protected Sub ResultsButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ResultsButton.Click
            Response.Redirect(EditUrl("Results"))
        End Sub
    End Class
End Namespace
