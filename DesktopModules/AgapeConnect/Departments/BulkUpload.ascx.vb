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

                    MyCommand.CommandText = "Select * from [Depts$A3:D1000] "

                    Dim data = MyCommand.ExecuteReader()

                    While data.Read
                        Try

                      
                        If IsDBNull(data.Item(0)) Then
                            Exit While
                        End If

                            AddNewDept(IIf(IsDBNull(data.Item(0)), "", data.Item(0)),
                                   IIf(IsDBNull(data.Item(1)), "", data.Item(1)),
                                   IIf(IsDBNull(data.Item(2)), "", data.Item(2)),
                                   IIf(IsDBNull(data.Item(3)), "", data.Item(3)))

                        Catch ex As Exception
                            lblResponse.Text &= "Error: " & ex.Message
                        End Try


                    End While




                Catch ex As Exception



                Finally
                    MyConnection.Close()
                End Try


            Else
                lblResponse.Text &= "Please select a file before clicking process."
            End If

        End Sub
        Private Function AddNewDept(ByVal Name As String, ByVal RC As String, ByVal UID1 As String, ByVal UID2 As String) As Boolean
            Dim rcCode As String = RC
            If RC.Contains("-") Then
                rcCode = Left(RC, RC.IndexOf("-")).Trim.ToLower
            End If


            Dim id1 As Integer
            Try
                id1 = UID1.Substring(UID1.IndexOf("UID:") + 4).TrimEnd(")")
            Catch ex As Exception

            End Try


            Dim id2 As Integer
            If Not String.IsNullOrEmpty(UID2) Then
                Try
                    id2 = UID2.Substring(UID2.IndexOf("UID:") + 4).TrimEnd(")")
                Catch ex As Exception

                End Try
            End If
            Dim d As New StaffBrokerDataContext

            If (Not (StaffBrokerFunctions.GetSetting("NonDynamics", PortalId) = "True")) And d.AP_StaffBroker_CostCenters.Where(Function(c) c.CostCentreCode.Trim().ToLower = rcCode And c.PortalId = PortalId).Count = 0 Then
                lblResponse.Text &= "Error adding " & Name & " (" & rcCode & ") - RC does not exists.<br />"
            ElseIf id1 = Nothing Then
                lblResponse.Text &= "Error adding " & Name & " (" & rcCode & ") - No Manager.<br />"

            ElseIf UserController.GetUserById(PortalId, id1) Is Nothing Then
                lblResponse.Text &= "Error adding " & Name & " (" & rcCode & ") - User & " & UID1 & " does not exists.<br />"

            Else
                'Process the Dept

                Dim dept = StaffBrokerFunctions.CreateDept(Name, rcCode, id1, id2)
                lblResponse.Text &= "Created " & Name & " (" & rcCode & ") - ID:" & dept.CostCenterId & "<br />"

                Return True
            End If

            Return False

        End Function
      
        Protected Sub btnBulkUploadTemplate_Click(sender As Object, e As EventArgs) Handles btnBulkUploadTemplate.Click
            Dim staff = StaffBrokerFunctions.GetStaff()
            Dim d As New StaffBrokerDataContext()
            Dim RCs = From c In d.AP_StaffBroker_CostCenters Where c.PortalId = PortalId
            Dim connectionString = ""


            'Copy File to Portals Directory
            Dim File As New FileInfo(Server.MapPath("/DesktopModules/AgapeConnect/Departments/UploadDepartments.xls"))
            Dim Filename = Path.Combine(PortalSettings.HomeDirectoryMapPath, File.Name)
            File.CopyTo(Filename, True)


            connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Filename + ";Extended Properties=""Excel 8.0;HDR=NO;IMEX=2"""


            Using MyConnection As New OleDbConnection(connectionString)


                MyConnection.Open()

                For Each row In staff.OrderBy(Function(c) c.LastName).ThenBy(Function(c) c.FirstName)

                    Using oCmd As New OleDbCommand("Insert Into [Users$](F1, F2) Values(@Name, @Id)", MyConnection)
                        oCmd.Parameters.AddWithValue("@Name", row.LastName & ", " & row.FirstName & "(UID:" & row.UserID & ")")

                        oCmd.Parameters.AddWithValue("@Id", row.UserID)
                        oCmd.ExecuteNonQuery()
                    End Using

                Next

                For Each row In RCs
                    Using oCmd As New OleDbCommand("Insert Into [RCs$](F1, F2) Values(@Name,@Code)", MyConnection)
                        oCmd.Parameters.AddWithValue("@Name", row.CostCentreCode & "-" & row.CostCentreName)

                        oCmd.Parameters.AddWithValue("@Code", row.CostCentreCode)

                        oCmd.ExecuteNonQuery()
                    End Using

                Next
            End Using

            'return the file:

            Response.Clear()
            Response.ClearContent()
            Response.ClearHeaders()
            Response.AppendHeader("content-disposition", "attachment; filename=UploadDepartments.xls")
            Response.ContentType = "application/vnd.ms-excel"

            Response.WriteFile(Filename)

            Response.End()
        End Sub

    
    End Class
End Namespace
