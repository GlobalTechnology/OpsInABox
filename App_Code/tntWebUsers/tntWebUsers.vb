Imports Microsoft.VisualBasic
Imports System.Net
Imports System.Text
Imports System.IO
Imports StaffBrokerFunctions
Public Class tntWebUsers

    Private dt As New dynamicTnT2.TntMPDDataServerWebService2
    Private PortalId As Integer
    Private SessionId As String
    Private _allWebUsers As dynamicTnT2.WebUserInfo()
    Dim PS As PortalSettings
    Dim objEventLog As DotNetNuke.Services.Log.EventLog.EventLogController
    Public Property AllWebUsers As dynamicTnT2.WebUserInfo()
        Get
            Return _allWebUsers
        End Get
        Set(ByVal value As dynamicTnT2.WebUserInfo())
            _allWebUsers = value
        End Set



    End Property



    Structure connectionResponse
        Dim connectionSuccess As Boolean
        Dim hasTrustedUser As Boolean
        Dim ErrorMessage As String
    End Structure


    Public Shared Function TestDataserverConnection(ByVal URL As String) As connectionResponse
        Dim rtn As New connectionResponse

        rtn.connectionSuccess = False
        rtn.hasTrustedUser = False
        Try





            Dim ds As New StaffBroker.StaffBrokerDataContext


            Dim tuPass = (From c In ds.AP_StaffBroker_Settings Where c.PortalId = 0 And c.SettingName = "TrustedUserPassword" Select c.SettingValue).First




            Dim service = "https://agapeconnect.me"
            Dim restServer As String = "https://thekey.me/cas/v1/tickets"
            Dim postData = "service=" & service & "&username=trusteduser@agapeconnect.me&password=" & tuPass

            Dim request As WebRequest = WebRequest.Create(restServer)
            Dim byteArray As Byte() = Encoding.UTF8.GetBytes(postData)
            request.ContentLength = byteArray.Length
            request.Method = "POST"
            request.ContentType = "application/x-www-form-urlencoded"

            Dim datastream As Stream = request.GetRequestStream
            datastream.Write(byteArray, 0, byteArray.Length)
            datastream.Close()
            Dim response As WebResponse = request.GetResponse
            restServer = response.Headers.GetValues("Location").ToArray()(0)


            Dim tntURL = URL.Replace("http://", "https://")
            If Right(tntURL, 1) <> "/" Then
                tntURL &= "/"
            End If



            Dim t As New dynamicTnT2.TntMPDDataServerWebService2()
            t.Url = tntURL & "dataquery/dataqueryservice2.asmx"
            t.Discover()


            rtn.connectionSuccess = True
            postData = "service=" & tntURL
            request = WebRequest.Create(restServer)
            byteArray = Encoding.UTF8.GetBytes(postData)
            request.ContentLength = byteArray.Length
            request.Method = "POST"
            request.ContentType = "application/x-www-form-urlencoded"

            datastream = request.GetRequestStream
            datastream.Write(byteArray, 0, byteArray.Length)
            datastream.Close()

            response = request.GetResponse

            response.Headers.GetValues("location")
            datastream = response.GetResponseStream
            Dim reader As New StreamReader(datastream)
            Dim ST = reader.ReadToEnd()

            Dim sessionId = t.Auth_Login(tntURL, ST, False).SessionID

            If Not String.IsNullOrEmpty(sessionId) Then
                rtn.hasTrustedUser = True
            End If


        Catch ex As Exception

            rtn.ErrorMessage = ex.ToString
        End Try





        Return rtn
    End Function


    Public Sub New()
        PS = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)
        'objEventLog = New DotNetNuke.Services.Log.EventLog.EventLogController()
        Dim PortalId = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings).PortalId

        dt.Url = GetSetting("DataserverURL", PortalId) & "dataquery/dataqueryservice2.asmx"
        dt.Discover()
        Dim tuPass = StaffBrokerFunctions.GetSetting("TrustedUserPassword", 0)
        If String.IsNullOrEmpty(System.Web.HttpContext.Current.Session("tntSessionId")) Then
            Dim objKey As New KeyUser.KeyAuthentication("trusteduser@agapeconnect.me", tuPass, GetSetting("DataserverURL", PortalId))

            Dim Login = dt.Auth_Login(GetSetting("DataserverURL", PortalId), objKey.ProxyTicket, False)
            SessionId = Login.SessionID
            System.Web.HttpContext.Current.Session("tntSessionId") = SessionId
        Else
            SessionId = System.Web.HttpContext.Current.Session("tntSessionId")
        End If

        Try
            AllWebUsers = dt.WebUser_GetAllInfo(SessionId)
        Catch ex As Exception
            Dim objKey As New KeyUser.KeyAuthentication("trusteduser@agapeconnect.me", tuPass, GetSetting("DataserverURL", PortalId))

            Dim Login = dt.Auth_Login(GetSetting("DataserverURL", PortalId), objKey.ProxyTicket, False)
            SessionId = Login.SessionID
            System.Web.HttpContext.Current.Session("tntSessionId") = SessionId
            AllWebUsers = dt.WebUser_GetAllInfo(SessionId)
        End Try

        'objEventLog.AddLog("URL:", dt.Url, PS, 1, Log.EventLog.EventLogController.EventLogType.ADMIN_ALERT)
        'objEventLog.AddLog("AllWebUsers:", AllWebUsers.Count.ToString, PS, 1, Log.EventLog.EventLogController.EventLogType.ADMIN_ALERT)
        'objEventLog.AddLog("Setup", "PortalId: " & PortalId, PS, 1, Log.EventLog.EventLogController.EventLogType.ADMIN_ALERT)

    End Sub

    Public Sub CreateUsersFromTnt()
        PS = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)
        PortalId = PS.PortalId

        For Each row In AllWebUsers

            Dim casGUID As String = row.SsoCode

            'Lookup Vlaues from Russ' list!




        Next

    End Sub





    Public Sub RefreshWebUsers(Optional UserList As Integer() = Nothing)



        'SYNC WEB USERS
        PS = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)
        PortalId = PS.PortalId

        If StaffBrokerFunctions.GetSetting("tntWebLinkActive", PortalId) <> "True" Then
            Return
        End If
        'objEventLog.AddLog("DEBUG", "PortalId: " & PortalId, PS, 1, Log.EventLog.EventLogController.EventLogType.ADMIN_ALERT)

        'objEventLog.AddLog("DEBUG", "Started", PS, 1, Log.EventLog.EventLogController.EventLogType.ADMIN_ALERT)

        Dim stafflist As IQueryable(Of StaffBroker.User)
        If UserList Is Nothing Then
            stafflist = GetStaff()
        Else
            Dim d As New StaffBroker.StaffBrokerDataContext
            stafflist = d.Users.Where(Function(x) UserList.Contains(x.UserID))
        End If
        ' stafflist = From c In GetStaff() Where c.UserID = 2

        Dim rc As New DotNetNuke.Security.Roles.RoleController
        Dim accTeam = rc.GetUserRolesByRoleName(PortalId, "Accounts Team")
        If accTeam.Count = 0 Then
            accTeam = rc.GetUserRolesByRoleName(PortalId, "Accounts")
        End If

        For Each row In stafflist '.Where(Function(x) x.LastName = "Vellacott")
            'objEventLog.AddLog("DEBUG 1 User Id:", row.UserID.ToString, PS, 1, Log.EventLog.EventLogController.EventLogType.ADMIN_ALERT)
            Try


                CheckOrCreateWebUser(row.UserID)
                Dim thisWebUser As dynamicTnT2.WebUserInfo
                If AllWebUsers.Where(Function(x) x.Name = row.DisplayName).Count > 0 Then
                    thisWebUser = AllWebUsers.Where(Function(x) x.Name = row.DisplayName).First
                    'objEventLog.AddLog("DEBUG 2:", thisWebUser.Name, PS, 1, Log.EventLog.EventLogController.EventLogType.ADMIN_ALERT)

                End If



                Dim staff = StaffBrokerFunctions.GetStaffMember(row.UserID)
                'PERSONAL ACCOUNTS
                Dim CC = ""
                Dim designations() As String
                Try
                    designations = GetStaffProfileProperty(staff.StaffId, "Designation(s)").Split(";")

                Catch ex As Exception

                End Try

                If Not String.IsNullOrEmpty(staff.CostCenter) Then
                    CC = staff.CostCenter.Replace("-", "")
                    CheckOrCreateTNTProfile(row.UserID, "", "Personal Accounts")
                    CheckOrCreateTNTAccount(row.UserID, "", CC)
                    Dim suffix = GetSetting("AdvanceSuffix", PortalId)
                    If suffix <> "" Then
                        CheckOrCreateTNTAccount(row.UserID, "", CC & "-" & suffix)
                    End If

                    For Each desig In designations
                        desig = desig.Trim(";").Trim()
                        If desig.Length > 0 Then
                            CheckOrCreateDesignation(row.UserID, "", desig)
                        End If
                    Next
                End If
                'objEventLog.AddLog("DEBUG 3:", "", PS, 1, Log.EventLog.EventLogController.EventLogType.ADMIN_ALERT)


                'DEPARTMENT ACCOUNTS

                Dim depts = From c In GetDepartments(row.UserID) Where Not String.IsNullOrEmpty(c.CostCentre) Select c Distinct


                If depts.Count > 0 Then
                    CheckOrCreateTNTProfile(row.UserID, "1", "Department Accounts")
                Else
                    CheckOrDeleteTNTProfile(row.UserID, "1", "Department Accounts")
                End If
                For Each dept In depts
                    CheckOrCreateTNTAccount(row.UserID, "1", dept.CostCentre.Replace("-", ""))
                Next



                'TEAM ACCOUNTS
                Dim team = GetTeam(row.UserID)
                If team.Count > 0 Then
                    CheckOrCreateTNTProfile(row.UserID, "2", "Team Accounts")
                Else
                    CheckOrDeleteTNTProfile(row.UserID, "2", "Team Accounts")
                End If

                Dim CCs As New ArrayList()
                For Each member In team
                    CC = GetStaffMember(member.UserID).CostCenter.Replace("-", "")
                    CheckOrCreateTNTAccount(row.UserID, "2", CC)
                    CCs.Add(CC)
                Next


                If (From c As UserRoleInfo In accTeam Where c.UserID = row.UserID).Count > 0 Then
                    'ACCOUNTS USER
                    CheckOrCreateTNTProfile(row.UserID, "3", "All Accounts")
                    dt.WebUserMgmt_StaffProfile_AddFinancialAccount(SessionId, row.DisplayName, "3", "", True, "")

                Else
                    CheckOrDeleteTNTProfile(row.UserID, "3", "All Accounts")
                End If




                'Remove All Unwanted Accounts
                If Not thisWebUser Is Nothing Then
                    Dim profPers = thisWebUser.StaffProfiles.Where(Function(x) x.Code = "")
                    If profPers.Count > 0 Then
                        For Each acc In profPers.First.FinancialAccounts
                            If Not acc.Code.StartsWith(staff.CostCenter.Replace("-", "")) Then
                                dt.WebUserMgmt_StaffProfile_RemoveFinancialAccount(SessionId, row.DisplayName, "", acc.Code, False, "")
                            End If
                        Next

                        For Each Desig In profPers.First.Designations
                            If Not designations.Contains(Desig.Code) Then
                                dt.WebUserMgmt_StaffProfile_RemoveFinancialAccount(SessionId, row.DisplayName, "", Desig.Code, False, "")
                            End If
                        Next
                    End If


                    Dim profDept = thisWebUser.StaffProfiles.Where(Function(x) x.Code = "1")
                    If profDept.Count > 0 Then
                        For Each acc In profDept.First.FinancialAccounts
                            If depts.Where(Function(x) x.CostCentre = acc.Code).Count = 0 Then
                                dt.WebUserMgmt_StaffProfile_RemoveFinancialAccount(SessionId, row.DisplayName, "1", acc.Code, False, "")
                            End If
                        Next
                    End If

                    Dim profTeam = thisWebUser.StaffProfiles.Where(Function(x) x.Code = "2")
                    If profTeam.Count > 0 Then
                        For Each acc In profTeam.First.FinancialAccounts
                            If Not CCs.Contains(acc.Code) Then
                                dt.WebUserMgmt_StaffProfile_RemoveFinancialAccount(SessionId, row.DisplayName, "2", acc.Code, False, "")
                            End If
                        Next
                    End If


                End If
            Catch ex As Exception
                objEventLog.AddLog("TnTLink Error for user: " & row.DisplayName, ex.Message, PS, 1, Log.EventLog.EventLogController.EventLogType.ADMIN_ALERT)
            End Try


        Next





    End Sub




    Public Sub CheckOrCreateWebUser(ByVal UserId As String)
        'objEventLog.AddLog("DB0:", "UserId: " & UserId, PS, 1, Log.EventLog.EventLogController.EventLogType.ADMIN_ALERT)
        'objEventLog.AddLog("DB0:", "PortalId: " & PortalId, PS, 1, Log.EventLog.EventLogController.EventLogType.ADMIN_ALERT)
        If StaffBrokerFunctions.GetSetting("tntWebLinkActive", PortalId) <> "True" Then
            Return
        End If
        Dim objUser = UserController.GetUserById(PortalId, UserId)
        If Not objUser Is Nothing Then
            'objEventLog.AddLog("DB1:", "", PS, 1, Log.EventLog.EventLogController.EventLogType.ADMIN_ALERT)


            Dim ssoGUID = ""
            Try
                ssoGUID = objUser.Profile.GetPropertyValue("ssoGUID")
            Catch ex As Exception

            End Try
            'objEventLog.AddLog("DB2:", "", PS, 1, Log.EventLog.EventLogController.EventLogType.ADMIN_ALERT)

            If ssoGUID Is Nothing Then
                ssoGUID = ""
            End If
            Dim thisWebUser As dynamicTnT2.WebUserInfo
            If AllWebUsers.Where(Function(x) x.Name = objUser.DisplayName).Count = 0 Then
                Dim gcxEmail = objUser.Email

                If objUser.Username.Contains("@") Then
                    gcxEmail = (objUser.Username.TrimEnd(PortalId.ToString))
                End If
                'objEventLog.AddLog("DB3:", gcxEmail, PS, 1, Log.EventLog.EventLogController.EventLogType.ADMIN_ALERT)


                dt.WebUserMgmt_CreateWebUser(SessionId, objUser.DisplayName, ssoGUID, gcxEmail, gcxEmail)
                'objEventLog.AddLog("Created:", objUser.DisplayName, PS, 1, Log.EventLog.EventLogController.EventLogType.ADMIN_ALERT)

            Else
                thisWebUser = AllWebUsers.Where(Function(x) x.Name = objUser.DisplayName).First
                'objEventLog.AddLog("Found:", objUser.DisplayName, PS, 1, Log.EventLog.EventLogController.EventLogType.ADMIN_ALERT)

            End If
        Else
            'objEventLog.AddLog("Error:", " could not find user " & UserId, PS, 1, Log.EventLog.EventLogController.EventLogType.ADMIN_ALERT)

        End If
    End Sub


    Public Sub CheckOrCreateTNTProfile(ByVal UserId As String, ByVal ProfileCode As String, ByVal ProfileDescription As String)
        If StaffBrokerFunctions.GetSetting("tntWebLinkActive", PortalId) <> "True" Then
            Return
        End If
        Dim objUser = UserController.GetUserById(PortalId, UserId)
        If Not objUser Is Nothing Then

            Dim thisWebUser As dynamicTnT2.WebUserInfo
            If AllWebUsers.Where(Function(x) x.Name = objUser.DisplayName).Count > 0 Then
                thisWebUser = AllWebUsers.Where(Function(x) x.Name = objUser.DisplayName).First
                If thisWebUser.StaffProfiles.Where(Function(x) x.Code = ProfileCode).Count = 0 Then
                    dt.WebUserMgmt_AddStaffProfile(SessionId, objUser.DisplayName, ProfileCode, ProfileDescription)
                End If
            Else
                dt.WebUserMgmt_AddStaffProfile(SessionId, objUser.DisplayName, ProfileCode, ProfileDescription)
            End If

        End If

    End Sub



    Public Sub CheckOrDeleteTNTProfile(ByVal UserId As String, ByVal ProfileCode As String, ByVal ProfileDescription As String)
        If StaffBrokerFunctions.GetSetting("tntWebLinkActive", PortalId) <> "True" Then
            Return
        End If
        Dim objUser = UserController.GetUserById(PortalId, UserId)
        If Not objUser Is Nothing Then



            Dim thisWebUser As dynamicTnT2.WebUserInfo
            If AllWebUsers.Where(Function(x) x.Name = objUser.DisplayName).Count > 0 Then
                thisWebUser = AllWebUsers.Where(Function(x) x.Name = objUser.DisplayName).First
                If thisWebUser.StaffProfiles.Where(Function(x) x.Code = ProfileCode).Count > 0 Then
                    dt.WebUserMgmt_RemoveStaffProfile(SessionId, objUser.DisplayName, ProfileCode)
                End If

            End If

        End If
    End Sub

    Public Sub CheckOrCreateTNTAccount(ByVal UserId As String, ByVal ProfileCode As String, ByVal FinancialAccount As String)
        If StaffBrokerFunctions.GetSetting("tntWebLinkActive", PortalId) <> "True" Then
            Return
        End If
        If String.IsNullOrEmpty(FinancialAccount) Then
            Return
        End If


        Dim objUser = UserController.GetUserById(PortalId, UserId)
        If Not objUser Is Nothing Then



            Dim thisWebUser As dynamicTnT2.WebUserInfo
            If AllWebUsers.Where(Function(x) x.Name = objUser.DisplayName).Count > 0 Then
                thisWebUser = AllWebUsers.Where(Function(x) x.Name = objUser.DisplayName).First
                Dim q = thisWebUser.StaffProfiles.Where(Function(x) x.Code = ProfileCode)
                If q.Count > 0 Then
                    If q.First.FinancialAccounts.Where(Function(x) x.Code = FinancialAccount).Count > 0 Then
                        'already exists - so return
                        Return
                    End If
                End If
            End If
            Try


                dt.WebUserMgmt_StaffProfile_AddFinancialAccount(SessionId, objUser.DisplayName, ProfileCode, FinancialAccount, False, "")
            Catch ex As Exception

            End Try
        End If
    End Sub

    Public Sub CheckOrCreateDesignation(ByVal UserId As String, ByVal ProfileCode As String, ByVal Designation As String)
        If StaffBrokerFunctions.GetSetting("tntWebLinkActive", PortalId) <> "True" Then
            Return
        End If
        If String.IsNullOrEmpty(Designation) Then
            Return
        End If
        Dim objUser = UserController.GetUserById(PortalId, UserId)
        If Not objUser Is Nothing Then


            Dim thisWebUser As dynamicTnT2.WebUserInfo
            If AllWebUsers.Where(Function(x) x.Name = objUser.DisplayName).Count > 0 Then
                thisWebUser = AllWebUsers.Where(Function(x) x.Name = objUser.DisplayName).First
                Dim q = thisWebUser.StaffProfiles.Where(Function(x) x.Code = ProfileCode)
                If q.Count > 0 Then
                    If q.First.Designations.Where(Function(x) x.Code = Designation).Count > 0 Then
                        'already exists - so return
                        Return
                    End If
                End If
            End If
            Try
                dt.WebUserMgmt_StaffProfile_AddDesig(SessionId, objUser.DisplayName, ProfileCode, Designation, False, "")
            Catch ex As Exception

            End Try
        End If
    End Sub

    Public Sub CheckOrDeleteFinancialAccount(ByVal UserId As String, ByVal ProfileCode As String, ByVal FinancialAccount As String)
        If StaffBrokerFunctions.GetSetting("tntWebLinkActive", PortalId) <> "True" Then
            Return
        End If
        If String.IsNullOrEmpty(FinancialAccount) Then
            Return
        End If
        Dim objUser = UserController.GetUserById(PortalId, UserId)
        If Not objUser Is Nothing Then


            Dim thisWebUser As dynamicTnT2.WebUserInfo
            If AllWebUsers.Where(Function(x) x.Name = objUser.DisplayName).Count > 0 Then
                thisWebUser = AllWebUsers.Where(Function(x) x.Name = objUser.DisplayName).First
                Dim q = thisWebUser.StaffProfiles.Where(Function(x) x.Code = ProfileCode)
                If q.Count > 0 Then
                    If q.First.FinancialAccounts.Where(Function(x) x.Code = FinancialAccount).Count > 0 Then

                        dt.WebUserMgmt_StaffProfile_RemoveFinancialAccount(SessionId, objUser.DisplayName, ProfileCode, FinancialAccount, False, "")
                    End If
                End If
            End If
        End If
    End Sub

    Public Sub CheckOrDeleteDesignation(ByVal UserId As String, ByVal ProfileCode As String, ByVal Designation As String)
        If StaffBrokerFunctions.GetSetting("tntWebLinkActive", PortalId) <> "True" Then
            Return
        End If
        If String.IsNullOrEmpty(Designation) Then
            Return
        End If
        Dim objUser = UserController.GetUserById(PortalId, UserId)
        If Not objUser Is Nothing Then


            Dim thisWebUser As dynamicTnT2.WebUserInfo
            If AllWebUsers.Where(Function(x) x.Name = objUser.DisplayName).Count > 0 Then
                thisWebUser = AllWebUsers.Where(Function(x) x.Name = objUser.DisplayName).First
                Dim q = thisWebUser.StaffProfiles.Where(Function(x) x.Code = ProfileCode)
                If q.Count > 0 Then
                    If q.First.Designations.Where(Function(x) x.Code = Designation).Count > 0 Then

                        dt.WebUserMgmt_StaffProfile_RemoveDesig(SessionId, objUser.DisplayName, ProfileCode, Designation, False, "")
                    End If
                End If
            End If
        End If
    End Sub

End Class
