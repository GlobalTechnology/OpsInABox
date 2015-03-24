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


Namespace DotNetNuke.Modules.AgapeFR.OnlineForm

    Partial Class EditOnlineForm
        Inherits Entities.Modules.PortalModuleBase
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            ModuleHF.Value = Me.ModuleId

            Dim d As New OnlineFormDataContext


            If Not Page.IsPostBack Then


                Try
                    Dim q = (From c In d.Agape_Public_OnlineForms Where c.ModuleId = Me.ModuleId Select c).First

                    PrefixBox.Text = q.Intro
                    SuffixBox.Text = q.FootNote
                    EmailTo.Text = q.EmailTo
                    Ack.Checked = q.Ack
                    AckText.Text = q.AckText
                    EmailReq.Checked = q.ReqEmail

                    FormIdHF.Value = q.FormId
                    QuestionsGridView.DataBind()
                Catch ex As Exception
                    Dim insert As New Agape_Public_OnlineForm
                    insert.ModuleId = Me.ModuleId
                    d.Agape_Public_OnlineForms.InsertOnSubmit(insert)
                    d.SubmitChanges()
                    FormIdHF.Value = (From c In d.Agape_Public_OnlineForms Where c.ModuleId = Me.ModuleId).First.FormId
                    QuestionsGridView.DataBind()

                End Try


            End If
        End Sub

        Protected Sub CancelButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CancelButton.Click
            Response.Redirect(NavigateURL())
        End Sub

        Protected Sub SaveMenuButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SaveMenuButton.Click
            Dim d As New OnlineFormDataContext

            Dim q = (From c In d.Agape_Public_OnlineForms Where c.ModuleId = ModuleId).First
            q.Intro = PrefixBox.Text
            q.FootNote = SuffixBox.Text
            q.EmailTo = EmailTo.Text
            q.Ack = Ack.Checked
            q.AckText = AckText.Text
            q.ReqEmail = EmailReq.Checked
            d.SubmitChanges()



            Response.Redirect(NavigateURL())

        End Sub




        Protected Sub QuestionsGridView_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles QuestionsGridView.RowCommand
            DDLPanel.Visible = False
            If e.CommandName = "Insert" Then
                Dim d As New OnlineFormDataContext
                Dim insert As New Agape_Public_OnlineForm_Question
                Dim tb As TextBox = QuestionsGridView.FooterRow.Controls(0).FindControl("FooterText")
                insert.QuestionText = tb.Text
                'insert.QuestionText = "Hello"
                Dim ddl As DropDownList
                ddl = QuestionsGridView.FooterRow.Controls(0).FindControl("FooterType")
                insert.QuestionType = ddl.SelectedValue
                Dim req As CheckBox
                req = QuestionsGridView.FooterRow.Controls(0).FindControl("FooterReq")
                insert.Required = req.Checked
                insert.FormId = FormIdHF.Value
                d.Agape_Public_OnlineForm_Questions.InsertOnSubmit(insert)
                d.SubmitChanges()

                QuestionsGridView.DataBind()

            ElseIf e.CommandName = "AddNew" Then
                Dim d As New OnlineFormDataContext
                Dim insert As New Agape_Public_OnlineForm_Question
                Dim tb As TextBox = QuestionsGridView.Controls(0).Controls(0).FindControl("NewText")
                insert.QuestionText = tb.Text
                'insert.QuestionText = "Hello"
                Dim ddl As DropDownList
                ddl = QuestionsGridView.Controls(0).Controls(0).FindControl("NewType")
                insert.QuestionType = ddl.SelectedValue
                Dim req As CheckBox
                req = QuestionsGridView.Controls(0).Controls(0).FindControl("NewReq")
                insert.Required = req.Checked
                insert.FormId = FormIdHF.Value
                d.Agape_Public_OnlineForm_Questions.InsertOnSubmit(insert)
                d.SubmitChanges()

                QuestionsGridView.DataBind()


            ElseIf e.CommandName = "DDL" Then
                DDLPanel.Visible = True

                QuestionIdHF.Value = e.CommandArgument
                DDLListBox.DataBind()



            End If


        End Sub

        Protected Sub AddDDL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AddNewDDL.Click
            If Not QuestionIdHF.Value Is Nothing Then
                Dim d As New OnlineFormDataContext
                Dim insert As New Agape_Public_OnlineForm_DDL
                insert.QuestionId = QuestionIdHF.Value
                insert.RowText = DDLText.Text
                d.Agape_Public_OnlineForm_DDLs.InsertOnSubmit(insert)
                d.SubmitChanges()
                DDLListBox.DataBind()
                DDLText.Text = ""
            End If



        End Sub

        Protected Sub RemoveDDL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RemoveDDL.Click
            Dim d As New OnlineFormDataContext
            Dim selectedID As Int16 = System.Convert.ToInt16(DDLListBox.SelectedValue)

            Dim toDelete As Agape_Public_OnlineForm_DDL = d.Agape_Public_OnlineForm_DDLs.Single(Function(p) p.DDLRowId = selectedID)
            d.Agape_Public_OnlineForm_DDLs.DeleteOnSubmit(toDelete)
            d.SubmitChanges()
            DDLListBox.DataBind()





        End Sub
    End Class
End Namespace
