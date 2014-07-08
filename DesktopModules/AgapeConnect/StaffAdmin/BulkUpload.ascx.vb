Imports System
Imports System.Collections
Imports System.Configuration
Imports System.Data
Imports System.Linq

Imports DotNetNuke
Imports DotNetNuke.Security
Imports StaffBroker
Imports System.IO
Imports System.Data.OleDb




Namespace DotNetNuke.Modules.StaffAdmin
    Partial Class BulkUpload
        Inherits Entities.Modules.PortalModuleBase



        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        End Sub


        Protected Sub btnReturn_Click(sender As Object, e As EventArgs) Handles btnReturn.Click
            Response.Redirect(NavigateURL)
        End Sub

        Protected Sub btnProcess_Click(sender As Object, e As EventArgs) Handles btnProcess.Click
            If (fuUploadFile.HasFile) Then


                lblResponse.Text = ""

                Dim fileName = Path.GetFileName(fuUploadFile.PostedFile.FileName)
                Dim fileExtension = Path.GetExtension(fuUploadFile.PostedFile.FileName)
                Dim fileLocation As String = PortalSettings.HomeDirectoryMapPath & fileName
                fuUploadFile.SaveAs(fileLocation)
                Dim connectionString = ""

                If (fileExtension = ".xls") Then
                    connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileLocation + ";Extended Properties=""Excel 8.0;HDR=Yes;IMEX=2"""
                ElseIf fileExtension = ".xlsx" Then
                    connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=""Excel 12.0;HDR=Yes;IMEX=2"""
                End If

                Dim MyConnection As OleDbConnection
                MyConnection = New OleDbConnection(connectionString)

                MyConnection.Open()
                Try

                    Dim MyCommand As New OleDbCommand()
                    MyCommand.Connection = MyConnection

                    MyCommand.CommandText = "Select * from [Staff$A3:K1000] "

                    Dim data = MyCommand.ExecuteReader()

                    While data.Read
                        If IsDBNull(data.Item(0)) Then
                            Exit While
                        End If
                        Try
                            AddStaffRecord(IIf(IsDBNull(data.Item(0)), "", data.Item(0)),
                         IIf(IsDBNull(data.Item(1)), "", data.Item(1)),
                         IIf(IsDBNull(data.Item(2)), "", data.Item(2)),
                         IIf(IsDBNull(data.Item(3)), 1, data.Item(3)),
                         IIf(IsDBNull(data.Item(4)), 1, data.Item(4)),
                         IIf(IsDBNull(data.Item(5)), "", data.Item(5)),
                         IIf(IsDBNull(data.Item(6)), "N", data.Item(6)),
                         IIf(IsDBNull(data.Item(7)), "", data.Item(7)),
                         IIf(IsDBNull(data.Item(8)), "", data.Item(8)),
                         IIf(IsDBNull(data.Item(9)), "", data.Item(9)),
                         IIf(IsDBNull(data.Item(10)), "", data.Item(10)))
                        Catch ex As Exception
                            lblResponse.Text &= "There was a problem creating user: " & ex.ToString() & "<br />"

                        End Try
                     




                    End While




                Catch ex As Exception
                    lblResponse.Text &= "Error: " & ex.ToString()


                Finally
                    MyConnection.Close()
                End Try


            Else
                lblResponse.Text &= "Please select a file before clicking process."
            End If

        End Sub

        Private Sub AddStaffRecord(ByVal FirstName As String, ByVal LastName As String, ByVal GCXEmail As String, ByVal MaritalStatus As Integer, ByVal StaffType As Integer, ByVal RC As String, ByVal PayOnly As String, ByVal Designation As String, ByVal SpFirstName As String, ByVal SpLastName As String, ByVal SpGCXEmail As String)
            Dim d As New StaffBrokerDataContext
            lblResponse.Text &= "<br /><b>" & LastName & ", " & FirstName & ":</b> "

            Dim valStatus As Boolean = True
            Dim append As String = ""

            If FirstName = "" Or LastName = "" Or GCXEmail = "" Or Not {1, 2, 3, 4}.Contains(MaritalStatus) Or Not {1, 2, 3, 4}.Contains(StaffType) Then
                lblResponse.Text &= "<span style=""color: red ;"">ERROR - Missing required Fields</span"
                Return
            End If

            Dim user As UserInfo = UserController.GetUserByName(PortalId, GCXEmail & PortalId)
            Dim spouse As UserInfo

            If (MaritalStatus = 2) Then
                If String.IsNullOrEmpty(SpGCXEmail) Then
                    If String.IsNullOrEmpty(SpFirstName) Then
                        MaritalStatus = 1
                    Else
                        If String.IsNullOrEmpty(SpLastName) Then
                            SpLastName=LastName
                        End If
                        MaritalStatus = 3
                    End If

                End If
                spouse = UserController.GetUserByName(PortalId, SpGCXEmail & PortalId)
            End If


            If user Is Nothing Then
                user = StaffBrokerFunctions.CreateUser(PortalId, GCXEmail, FirstName, LastName)
            End If
            If spouse Is Nothing And MaritalStatus = 2 Then
               
                    spouse = StaffBrokerFunctions.CreateUser(PortalId, SpGCXEmail, SpFirstName, SpLastName)

            End If

            Dim theStaff1 = StaffBrokerFunctions.GetStaffMember(user.UserID)
            Dim theStaff2 As StaffBroker.AP_StaffBroker_Staff
            If MaritalStatus = 2 Then
                theStaff2 = StaffBrokerFunctions.GetStaffMember(spouse.UserID)
            End If

            Dim st As Integer = 1
            If StaffType = 1 Then
                st = d.AP_StaffBroker_StaffTypes.Where(Function(x) x.PortalId = PortalId And x.Name = "National Staff").First.StaffTypeId
            ElseIf StaffType = 2 Then
                st = d.AP_StaffBroker_StaffTypes.Where(Function(x) x.PortalId = PortalId And x.Name = "National Staff, Overseas").First.StaffTypeId
            ElseIf StaffType = 3 Then
                st = d.AP_StaffBroker_StaffTypes.Where(Function(x) x.PortalId = PortalId And x.Name = "Overseas Staff, in Country").First.StaffTypeId
            ElseIf StaffType = 4 Then
                st = d.AP_StaffBroker_StaffTypes.Where(Function(x) x.PortalId = PortalId And x.Name = "Overseas Staff, Overseas").First.StaffTypeId
            Else
                st = d.AP_StaffBroker_StaffTypes.Where(Function(x) x.PortalId = PortalId And x.Name = "Other").First.StaffTypeId


            End If

            Dim theStaff As StaffBroker.AP_StaffBroker_Staff



            If MaritalStatus = 2 And (Not theStaff1 Is Nothing) And (Not theStaff2 Is Nothing) Then
                '2 staff accounts exist. Delete 2 and keep 1


                theStaff = (From c In d.AP_StaffBroker_Staffs Where c.PortalId = PortalId And c.StaffId = theStaff1.StaffId).First
                Dim q2 = From c In d.AP_StaffBroker_Staffs Where c.PortalId = PortalId And c.StaffId = theStaff2.StaffId
                q2.First.Active = False
                theStaff.UserId2 = spouse.UserID

                theStaff.DisplayName = user.FirstName & " & " & spouse.FirstName & " " & user.LastName


            ElseIf MaritalStatus = 2 And ((Not theStaff1 Is Nothing) Or (Not theStaff2 Is Nothing)) Then
                'One of the staff accounts exists

                If Not theStaff1 Is Nothing Then
                    theStaff = From c In d.AP_StaffBroker_Staffs Where c.PortalId = PortalId And c.StaffId = theStaff1.StaffId
                Else
                    theStaff = From c In d.AP_StaffBroker_Staffs Where c.PortalId = PortalId And c.StaffId = theStaff2.StaffId
                End If


                theStaff.UserId2 = spouse.UserID


            ElseIf MaritalStatus = 2 Then
                'Need to create both
                theStaff = StaffBrokerFunctions.CreateStaffMember(PortalId, user, spouse, st)


            ElseIf MaritalStatus = 3 Then
                'spouse not on staff
                If theStaff1 Is Nothing Then
                    theStaff = StaffBrokerFunctions.CreateStaffMember(PortalId, user, SpFirstName, "01/01/2001", st)
                Else
                    theStaff = theStaff1
                End If


            Else
                'Single
                If theStaff1 Is Nothing Then
                    theStaff = StaffBrokerFunctions.CreateStaffMember(PortalId, user, st)
                Else
                    theStaff = theStaff1
                End If




            End If




            Dim s =( From c In d.AP_StaffBroker_Staffs Where c.StaffId = theStaff.StaffId).first
            s.StaffTypeId = st

            If RC <> "" Then
                If d.AP_StaffBroker_CostCenters.Where(Function(x) x.CostCentreCode.Trim() = RC.Trim() And x.PortalId = PortalId).Count > 0 Or (StaffBrokerFunctions.GetSetting("NonDynamics", PortalId) = "True") Then
                    s.CostCenter = RC
                Else
                    append &= "<span style=""color: red ;""> - but RC does not exists<span>"
                End If
            End If
            d.SubmitChanges()
            If Designation <> "" Then
                StaffBrokerFunctions.AddProfileValue(PortalId, theStaff.StaffId, "Designation(s)", Designation)
            End If
            If {"Y", "y", "N", "n"}.Contains(PayOnly) Then
                StaffBrokerFunctions.AddProfileValue(PortalId, theStaff.StaffId, "PayOnly", PayOnly.ToUpper = "Y")
            End If



            lblResponse.Text &= "<span style=""color: Green ;"">Success<span>" & append


            StaffBrokerFunctions.SetSetting("tntFlag", "Dirty", PortalId)

        End Sub


    
    End Class
End Namespace
