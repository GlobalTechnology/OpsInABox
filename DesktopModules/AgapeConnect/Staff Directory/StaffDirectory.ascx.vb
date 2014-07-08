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
Imports DotNetNuke
Imports DotNetNuke.Security

Imports StaffBroker

Imports StaffBrokerFunctions


Namespace DotNetNuke.Modules.StaffDirectory
    Partial Class StaffDirectory
        Inherits Entities.Modules.PortalModuleBase

        'Dim dStaff As New AgapeStaffDataContext
        Dim dBroke As New StaffBrokerDataContext
        '  Dim d As New DNNProfileDataContextDataContext


        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
            btnSettings.Visible = IsEditable
            If Not Page.IsPostBack Then
                'Dim d As New DNNProfileDataContextDataContext
                Dim staffTypes() As String = {""}
                If CStr(Settings("StaffTypes")) <> "" Then
                    staffTypes = CStr(Settings("StaffTypes")).Split(";")
                End If
                For Each row In staffTypes
                    row = row.TrimEnd(";")
                Next


                Dim staffMemberList = From c In dBroke.AP_StaffBroker_Staffs Where c.PortalId = PortalId And staffTypes.Contains(c.StaffTypeId)
                Dim StaffList = From c In staffMemberList Select DisplayName = c.User.LastName & ", " & c.User.FirstName, UserId = c.UserId1
                StaffList = StaffList.Union(From c In staffMemberList Where c.UserId2 > 0 Select DisplayName = c.User2.LastName & ", " & c.User2.FirstName, UserId = CInt(c.UserId2))
              
                If Request.QueryString("uid") <> "" Then
                    ListBox1.DataSource = From c In StaffList Order By c.DisplayName
                    ListBox1.DataBind()
                    If ListBox1.Items.Count > 0 Then
                        ListBox1.SelectedValue = CInt(Request.QueryString("uid"))
                    Else
                        ContactPanel.Visible = False
                    End If

                ElseIf Request.QueryString("search") <> "" Then
                    ListBox1.DataSource = From c In StaffList Where c.DisplayName.Contains(Request.QueryString("search")) Order By c.DisplayName
                    SearchBox.Text = Request.QueryString("search")
                    ListBox1.DataBind()
                    If ListBox1.Items.Count > 0 Then
                        ListBox1.SelectedIndex = 0
                    Else
                        ContactPanel.Visible = False
                    End If
                Else
                    ListBox1.DataSource = StaffList
                    ListBox1.DataBind()
                    If ListBox1.Items.Count > 0 Then
                        ListBox1.SelectedIndex = 0
                    Else
                        ContactPanel.Visible = False
                    End If
                End If
            End If

            If (ListBox1.SelectedValue <> "") Then
                LoadValues()
            End If
        End Sub




        Public Function GetProfileImage(ByVal UserId As Integer) As String
            Dim FileId = UserController.GetUserById(PortalId, UserId).Profile.GetPropertyValue("Photo")
            If FileId Is Nothing Or FileId = "" Then
                Return "/images/no_avatar.gif"
            Else
                Dim theFile = DotNetNuke.Services.FileSystem.FileManager.Instance.GetFile(FileId)
                If Not theFile Is Nothing Then
                    Return DotNetNuke.Services.FileSystem.FileManager.Instance.GetUrl(theFile)
                Else
                    Return "/images/no_avatar.gif"
                End If

            End If

        End Function
        Public Function Translate(ByVal ResourceString As String) As String
            Return DotNetNuke.Services.Localization.Localization.GetString(ResourceString & ".Text", LocalResourceFile)

        End Function
        Protected Sub LoadValues()
            Dim theUserId = CInt(ListBox1.SelectedValue)

            Dim ProfProps As New ArrayList()
            If Not String.IsNullOrEmpty(CStr(Settings("ProfProps"))) Then
                Dim pp = CStr(Settings("ProfProps")).Split(";")

                For Each row In pp
                    Dim thisProp = DotNetNuke.Entities.Profile.ProfileController.GetPropertyDefinition(row.TrimStart(";"), PortalId)
                    ProfProps.Add(thisProp)
                Next
            End If
         
            dlProfileProps.DataSource = (From c As DotNetNuke.Entities.Profile.ProfilePropertyDefinition In ProfProps Group By c.PropertyCategory Into Props = Group)
            dlProfileProps.DataBind()


            Dim StaffProps = From c In dBroke.AP_StaffBroker_StaffPropertyDefinitions Where c.PortalId = PortalId
            Dim StaffPropValues As New Dictionary(Of String, String)

            Dim StaffMember = GetStaffMember(theUserId)

            For Each row In StaffProps
                If CStr(Settings("StaffProps")).Contains(row.StaffPropertyDefinitionId) Then
                    StaffPropValues.Add(row.PropertyName, GetStaffProfileProperty(StaffMember.StaffId, row.PropertyName))

                End If
            Next

          

            dlStaffProps.DataSource = StaffPropValues
            dlStaffProps.DataBind()

            imgProfileImage.ImageUrl = GetProfileImage(theUserId)


            Dim AllLeaders = GetLeadersDetailed(theUserId, PortalId)
            Dim strLeaders As String = ""
            Dim Leaders = From c In AllLeaders Where c.isDelegate = False
            For Each row In Leaders
                strLeaders &= row.DisplayName & ", "
            Next
            'now append the delegates
            Dim delegates = From c In AllLeaders Where c.isDelegate = True

            strLeaders = strLeaders.TrimEnd(" ").TrimEnd(", ")

            If delegates.Count > 0 Then
                strLeaders &= "&nbsp;<span style=""color: #999; font-style: italic;"">("
                For Each row In delegates
                    strLeaders &= row.DisplayName & ", "
                Next
                strLeaders = strLeaders.TrimEnd(" ").TrimEnd(", ")
                strLeaders &= ")</span>"
            End If
            lblReportsTo.Text = strLeaders



                Dim r = From c In dBroke.Users Where c.UserID = theUserId
                If r.Count > 0 Then
                    FirstName.Text = r.First.FirstName
                    LastName.Text = r.First.LastName

                End If

                FamilyPanel.Visible = False



                Dim theOtherUserId = GetSpouseId(theUserId)
                If theOtherUserId > 0 Then

                    Spouse.Text = UserController.GetUserById(PortalId, theOtherUserId).FirstName
                    FamilyPanel.Visible = True
                ElseIf StaffMember.UserId2 = -1 Then

                    Spouse.Text = GetStaffProfileProperty(StaffMember.UserId2, "SpouseName")
                    FamilyPanel.Visible = True


                End If

                Dim Children = From c In dBroke.AP_StaffBroker_Childrens Where c.StaffId = StaffMember.StaffId Order By c.Birthday

                If Children.Count > 0 Then
                    ChildrenPanel.Visible = True
                    For Each row In Children
                        Dim Age As Integer
                        If DateTime.Today.Month < row.Birthday.Month Or DateTime.Today.Month = row.Birthday.Month And DateTime.Today.Day < row.Birthday.Day Then
                            Age = DateTime.Today.Year - row.Birthday.Year - 1
                        Else
                            Age = DateTime.Today.Year - row.Birthday.Year
                        End If

                        ChildrenPH.Controls.Add(New LiteralControl("<tr><td>" & row.FirstName & "</td><td>" & row.Birthday.ToString("dd MMM") & "</td><td>" & IIf(Age < 21, Age, "") & "</td></tr>"))
                    Next
                Else
                    ChildrenPanel.Visible = False
                End If



        End Sub


        Protected Sub SearchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchBtn.Click

            Dim staffTypes() As String = {""}
            If CStr(Settings("StaffTypes")) <> "" Then
                staffTypes = CStr(Settings("StaffTypes")).Split(";")
            End If
            For Each row In staffTypes
                row = row.TrimEnd(";")
            Next
            Dim staffMemberList = From c In dBroke.AP_StaffBroker_Staffs Where c.PortalId = PortalId And staffTypes.Contains(c.StaffTypeId)
            Dim StaffList = From c In staffMemberList Select DisplayName = c.User.LastName & ", " & c.User.FirstName, UserId = c.UserId1
            StaffList = StaffList.Union(From c In staffMemberList Where c.UserId2 > 0 Select DisplayName = c.User2.LastName & ", " & c.User2.FirstName, UserId = CInt(c.UserId2))

            StaffList = From c In StaffList Where c.DisplayName.Contains(SearchBox.Text) Order By c.DisplayName
            
            ListBox1.DataSource = StaffList

            ListBox1.DataBind()
            If ListBox1.Items.Count > 0 Then
                ListBox1.SelectedIndex = 0
                LoadValues()
            Else
                ContactPanel.Visible = False
            End If

        End Sub

        Protected Sub btnSettings_Click(sender As Object, e As System.EventArgs) Handles btnSettings.Click
            Dim temp = EditUrl("StaffProfileSettings")
            Response.Redirect(EditUrl("StaffProfileSettings"))



        End Sub




        Public Function GetProfileValue(ByVal PropertyName As String, ByVal Spouse As Boolean, Optional ByVal Type As Integer = -1) As String
            Dim User1 = UserController.GetUserById(PortalId, CInt(ListBox1.SelectedValue))
            Dim ukCulture As CultureInfo = New CultureInfo("en-GB")

            Dim val = User1.Profile.GetPropertyValue(PropertyName)
            If Type = 359 And val <> "" Then
                Dim value As DateTime = DateTime.Parse(val, ukCulture.DateTimeFormat)

                Return value.ToString("dd MMMM")
            Else
                Return val
            End If

            Return ""
        End Function

    End Class
End Namespace
