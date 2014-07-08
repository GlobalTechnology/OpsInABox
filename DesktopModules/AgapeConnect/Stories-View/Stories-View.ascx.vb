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
Imports System.Linq
Imports Stories

Namespace DotNetNuke.Modules.FullStory


    Partial Class acViewFullStory
        Inherits Entities.Modules.PortalModuleBase
        Public IsBoosted As Boolean = False
        Public IsBlocked As Boolean = False
        Public location As String = ""
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Dim d As New StoriesDataContext
            Dim r = (From c In d.AP_Stories Where c.StoryId = Request.QueryString("StoryID")).First
            Dim thecache = From c In d.AP_Stories_Module_Channel_Caches Where c.AP_Stories_Module_Channel.AP_Stories_Module.TabModuleId = r.TabModuleId And c.Link.EndsWith("StoryId=" & r.StoryId)

            If Not String.IsNullOrEmpty(Request.Form("Boosted")) Then

                If thecache.Count > 0 Then
                    thecache.First.Block = CBool(Request.Form("Blocked"))
                    If Not thecache.First.Block Then

                        If CBool(Request.Form("Boosted")) Then
                            If Not thecache.First.BoostDate Is Nothing Then
                                If thecache.First.BoostDate < Today Then
                                    thecache.First.BoostDate = Today.AddDays(7)

                                End If
                            Else
                                thecache.First.BoostDate = Today.AddDays(7)

                            End If
                        Else
                            thecache.First.BoostDate = Nothing
                        End If
                    Else
                        thecache.First.BoostDate = Nothing
                    End If
                    d.SubmitChanges()
                End If
                Return
            End If



            If String.IsNullOrEmpty(Request.QueryString("StoryID")) Then
                PagePanel.Visible = False
                NotFoundLabel.Visible = True

            Else

                '!!!!!!!btnEdit.Visible = IsEditable
                '!!!!!!!btnNew.Visible = IsEditable

                If Me.UserInfo.IsSuperUser And IsEditable() Then
                    'SuperPowers.Visible = True
                End If


                PagePanel.Visible = True
                NotFoundLabel.Visible = False
                StoryIdHF.Value = Request.QueryString("StoryId")

                Dim sv As String = StaffBrokerFunctions.GetTemplate("StoryView", PortalId)




                ReplaceField(sv, "[HEADLINE]", r.Headline)
                location = r.Latitude.Value.ToString(New CultureInfo("")) & ", " & r.Longitude.Value.ToString(New CultureInfo(""))
                ReplaceField(sv, "[MAP]", " <div id=""map_canvas""></div>")

                ReplaceField(sv, "[STORYTEXT]", r.StoryText)
                Dim thePhoto = DotNetNuke.Services.FileSystem.FileManager.Instance.GetFile(r.PhotoId)


                'ReplaceField(sv, "[STORYTEXT]", r.StoryText)
                ' Dim thePhoto = DotNetNuke.Services.FileSystem.FileManager.Instance.GetFile(r.PhotoId)

                Dim URL = "http://" & PortalSettings.PortalAlias.HTTPAlias & DotNetNuke.Services.FileSystem.FileManager.Instance.GetUrl(thePhoto)
                ReplaceField(sv, "[IMAGEURL]", URL)
                Dim meta As New HtmlMeta
                meta.Attributes.Add("property", "og:image")
                meta.Content = URL
                Page.Header.Controls.AddAt(0, meta)
                Dim meta2 As New HtmlMeta
                meta2.Attributes.Add("property", "og:title")
                meta2.Content = r.Headline
                Page.Header.Controls.AddAt(0, meta2)

                Dim permalink = EditUrl("ViewStory") & "?StoryId=" & Request.QueryString("StoryId")
                ReplaceField(sv, "[DATAHREF]", permalink)
                Dim meta3 As New HtmlMeta
                meta3.Attributes.Add("property", "og:url")

                meta3.Content = permalink
                Page.Header.Controls.AddAt(0, meta3)

                Dim meta4 As New HtmlMeta
                meta4.Attributes.Add("property", "og:description")
                meta4.Content = r.TextSample
                Page.Header.Controls.AddAt(0, meta4)

                Dim meta5 As New HtmlMeta
                meta5.Attributes.Add("property", "og:site_name")
                meta5.Content = PortalSettings.PortalName
                Page.Header.Controls.AddAt(0, meta5)

                Dim Fid = StaffBrokerFunctions.GetSetting("FacebookId", PortalId)
                If Not String.IsNullOrEmpty(Fid) Then
                    Dim meta6 As New HtmlMeta
                    meta6.Attributes.Add("property", "fb:app_id")
                    meta6.Content = Fid
                    Page.Header.Controls.AddAt(0, meta6)
                End If

                ReplaceField(sv, "[FACEBOOKID]", Fid)



                'ReplaceField(sv, "[IMAGEURL]", DotNetNuke.Services.FileSystem.FileManager.Instance.GetUrl(thePhoto))


                ReplaceField(sv, "[AUTHOR]", r.Author)
                ReplaceField(sv, "[DATE]", r.StoryDate.ToString("d MMM yyyy"))
                If (Not r.UpdatedDate Is Nothing) Then
                    If (DateDiff(DateInterval.Day, r.StoryDate, r.UpdatedDate.Value) > 14) Then
                        'Only show updated date if the update was more than two weeks after the creation date
                        ReplaceField(sv, "[UPDATEDDATE]", "(updated " & r.UpdatedDate.Value.ToString("d MMM yyyy") & ")")
                    End If

                End If
                ReplaceField(sv, "[UPDATEDDATE]", "")


                ReplaceField(sv, "[RSSURL]", "/DesktopModules/AgapeConnect/Stories/Feed.aspx?channel=" & TabModuleId)
                ReplaceField(sv, "[SAMPLE]", r.TextSample)
                ReplaceField(sv, "[SUBTITLE]", r.Subtitle)
                ReplaceField(sv, "[FIELD1]", r.Field1)
                ReplaceField(sv, "[FIELD2]", r.Field2)

                If r.Field3.Contains("#selAuth#") Then
                    Dim authID = r.Field3.Substring(9)
                    Dim uc As New UserController()
                    Dim auth = uc.GetUser(Me.PortalId, authID)
                    Dim thisPhoto As String = ""
                    Dim thisBio As String = ""
                    Try
                        Dim FileID = auth.Profile.GetPropertyValue("Photo")
                        Dim _theFile = DotNetNuke.Services.FileSystem.FileManager.Instance.GetFile(FileID)
                        thisPhoto = DotNetNuke.Services.FileSystem.FileManager.Instance.GetUrl(_theFile)
                        thisBio = auth.Profile.GetPropertyValue("Biography")
                    Catch ex As Exception
                        thisPhoto = "/images/no_avatar.gif"
                        thisBio = "No Bio information"
                    End Try
                    ReplaceField(sv, "[AUTHPHOTO]", thisPhoto)
                    ReplaceField(sv, "[AUTHBIO]", thisBio)
                Else
                    ReplaceField(sv, "[FIELD3]", r.Field3)
                End If



                PhotoIdHF.Value = r.PhotoId



                If Not r.TranslationGroup Is Nothing Then

                    'TranslationGroupHF.Value = r.TranslationGroup
                    SuperPowers.TranslationGroupId = r.TranslationGroup

                    Dim Translist = From c In d.AP_Stories Where c.TranslationGroup = r.TranslationGroup And c.PortalID = r.PortalID And c.StoryId <> r.StoryId Select c.Language, c.StoryId

                    If Translist.Count > 0 Then
                        Dim Flags As String = "<div style=""width: 100%;""><i>This story is also available in:</i> <div style=""margin: 4px 0 12px 0;"">"


                        For Each row In Translist
                            Dim Lang = GetLanguageName(row.Language)

                            Flags &= "<a href=""" & NavigateURL(CInt(Request.QueryString("origTabId"))) & "?StoryId=" & row.StoryId & """ target=""_self""><span title=""" & Lang & """><img  src=""" & GetFlag(row.Language) & """ alt=""" & Lang & """  /></span></a>"

                        Next

                        Flags &= "</div> </div>"


                        ReplaceField(sv, "[LANGUAGES]", Flags)
                    End If


                End If
                ReplaceField(sv, "[LANGUAGES]", "")
                If (sv.IndexOf("[SUPERPOWERS]") < 0) Then
                    ltStory1.Text = sv
                    ltStory2.Text = ""
                Else
                    ltStory1.Text = sv.Substring(0, sv.IndexOf("[SUPERPOWERS]"))

                    ltStory2.Text = sv.Substring(sv.IndexOf("[SUPERPOWERS]") + 13)
                End If




                If IsEditable Then
                    SuperPowers.Visible = True
                    If thecache.Count > 0 Then

                        SuperPowers.CacheId = thecache.First.CacheId
                        SuperPowers.SuperEditor = UserInfo.IsSuperUser
                        SuperPowers.EditUrl = NavigateURL(CInt(Request.QueryString("origTabId")), "AddEditStory", {"mid", Request.QueryString("origModId")})
                        SuperPowers.PortalId = PortalId
                        SuperPowers.SetControls()

                    End If

                End If


                'Get Current Channel 



                'If thecache.Count > 0 Then
                '    If thecache.First.Block Then
                '        lblPowerStatus.Text = "This story has been blocked, and won't appear in the channel feed."
                '        IsBlocked = True
                '    ElseIf Not thecache.First.BoostDate Is Nothing Then
                '        If thecache.First.BoostDate >= Today Then
                '            IsBoosted = True
                '            lblPowerStatus.Text = "Boosted until " & thecache.First.BoostDate.Value.ToString("dd MMM yyyy")

                '        End If
                '    End If
                'End If



            End If





        End Sub

        Private Sub ReplaceField(ByRef sv As String, ByVal fieldName As String, ByVal fieldValue As String)
            If Not String.IsNullOrEmpty(fieldValue) Then
                sv = sv.Replace(fieldName, fieldValue)
            Else
                sv = sv.Replace(fieldName, "")
            End If

        End Sub

        'Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEdit.Click
        '    Response.Redirect(EditUrl("AddEditStory") & "?StoryID=" & Request.QueryString("StoryID"))

        'End Sub


        Public Function GetLanguageName(ByVal language As String) As String

            Dim thename = CultureInfo.GetCultures(CultureTypes.AllCultures).Where(Function(x) x.Name.ToLower = language.ToLower).Select(Function(x) x.EnglishName & " / " & x.NativeName)
            If thename.Count > 0 Then
                Return thename.First()
            Else
                Return ""
            End If
        End Function

        Public Function GetFlag(ByVal language As String) As String
            If String.IsNullOrEmpty(language) Then
                Return ""
            End If
            If language = "en" Then
                language = "en-GB"

            ElseIf language.Length = 2 Then
                language = language.ToLower & "-" & language.ToUpper

            End If


            Dim flagDir = New DirectoryInfo(Server.MapPath("/images/Flags/"))
            If Not flagDir Is Nothing Then

                Dim flags = flagDir.GetFiles().Where(Function(x) x.Name.ToLower.Contains(language.ToLower))

                If flags.Count = 0 Then
                    Return ""  ' couldn't find flag
                Else
                    Return "/images/Flags/" & flags.First.Name

                End If
            Else
                Return ""
            End If

        End Function







        'Protected Sub btnNew_Click(sender As Object, e As System.EventArgs) Handles btnNew.Click
        '    Response.Redirect(EditUrl("AddEditStory"))
        'End Sub

        'Protected Sub btnTranslate_Click(sender As Object, e As EventArgs) Handles btnTranslate.Click
        '    Dim tg As Integer
        '    If String.IsNullOrEmpty(TranslationGroupHF.Value) Then
        '        'generate new Translation group
        '        Dim d As New StoriesDataContext
        '        Dim maxTransGroupId = d.AP_Stories.Where(Function(x) x.PortalID = PortalId And Not String.IsNullOrEmpty(x.TranslationGroup))
        '        If maxTransGroupId.Count = 0 Then
        '            tg = 1
        '        Else
        '            tg = maxTransGroupId.Max(Function(x) x.TranslationGroup)
        '        End If
        '    Else
        '        tg = CInt(TranslationGroupHF.Value)
        '    End If
        '    Response.Redirect(EditUrl("AddEditStory") & "?tg=" & tg)
        'End Sub
    End Class
End Namespace