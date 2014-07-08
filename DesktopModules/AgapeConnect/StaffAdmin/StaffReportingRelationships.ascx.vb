Imports System
Imports System.Collections
Imports System.Configuration
Imports System.Data
Imports System.Linq

Imports DotNetNuke
Imports DotNetNuke.Security
Imports StaffBroker



Namespace DotNetNuke.Modules.StaffAdmin
    Partial Class StaffReportingRelationships
        Inherits Entities.Modules.PortalModuleBase

      

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If ddlType.SelectedValue = 1 Then
                cbIncDelegates.Checked = True
                cbIncDelegates.Enabled = False
                cbIncDelLeaders.Checked = True
                cbIncDelLeaders.Enabled = False
            End If
            If Not Page.IsPostBack Then


                Dim d As New StaffBrokerDataContext
                Dim staffTypes = From c In d.AP_StaffBroker_StaffTypes Where c.PortalId = PortalId Order By c.Name

                cblStaffType.DataSource = staffTypes
                cblStaffType.DataTextField = "Name"
                cblStaffType.DataValueField = "StaffTypeId"
                cblStaffType.DataBind()
                For Each row As ListItem In cblStaffType.Items
                    If row.Text = "National Staff" Then
                        row.Selected = True

                    End If
                Next
            End If
            btnLoad_Click(Me, Nothing)
        End Sub

        
        Protected Sub btnLoad_Click(sender As Object, e As System.EventArgs) Handles btnLoad.Click
            Dim StaffTypes As New ArrayList()



            For Each row As ListItem In cblStaffType.Items
                If row.Selected Then
                    StaffTypes.Add(row.Text)
                End If
            Next
            Dim ST() As String = New String(StaffTypes.Count) {}
            For i As Integer = 0 To StaffTypes.Count - 1
                ST(i) = StaffTypes(i)
            Next

            Dim q = StaffBrokerFunctions.GetStaffIncl(PortalId, ST)


            dlStaff.DataSource = From c In q Order By c.LastName
            dlStaff.DataBind()

        End Sub

        Public Function GetLeaders(ByVal theUserid As Integer) As String
            If ddlType.SelectedValue = 0 Then

                Dim theList As IEnumerable(Of StaffBrokerFunctions.LeaderInfo)
                Dim rtn As String = "<span style=""color: Gray; font-style: Italic; "">reports to -> </span>"
                If cbIncDelegates.Checked Then
                    theList = From c In StaffBrokerFunctions.GetLeadersDetailed(theUserid, PortalId) Order By c.isDelegate, c.DisplayName Select c

                Else
                    theList = From c In StaffBrokerFunctions.GetLeadersDetailed(theUserid, PortalId) Where c.isDelegate = False Order By c.isDelegate, c.DisplayName Select c

                End If
                If cbIncDelLeaders.Checked = False Then
                    theList = From c In theList Where Not (c.hasDelegated = True And c.isDelegate = False) Select c
                End If
                
                For Each row In theList
                    If row.isDelegate Then
                        rtn &= "<span style=""font-style: Italic;"">" & row.DisplayName & "</span>, "
                    ElseIf row.hasDelegated Then
                        rtn &= "<span style=""color:Gray; "">(" & row.DisplayName & ")</span>, "
                    Else
                        rtn &= row.DisplayName & ", "
                    End If

                Next
                If theList.Count = 0 Then
                    Return ""
                Else
                    Return rtn.Substring(0, rtn.Count - 2)
                End If
            Else
                Dim theList = StaffBrokerFunctions.GetTeam(theUserid)
                Dim rtn As String = "<span style=""color: Gray; font-style: Italic; "">leads -> </span>"
                For Each row In theList
                    rtn &= row.DisplayName & ", "
                Next
                If theList.Count = 0 Then
                    Return ""
                Else
                    Return rtn.Substring(0, rtn.Count - 2)
                End If
            End If
        End Function

    End Class
End Namespace
