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
Imports System.IO
Imports System.Net
Imports DotNetNuke
Imports DotNetNuke.Security

'Imports AgapeStaff
Imports StaffBroker
Imports StaffBrokerFunctions

Namespace DotNetNuke.Modules.AgapeConnect
    Partial Class staffTreeview
        Inherits Entities.Modules.PortalModuleBase

        Private _mode As String
        Public Property Mode() As String
            Get
                Return _mode
            End Get
            Set(ByVal value As String)
                _mode = value
            End Set
        End Property

        Private _url As String
        Public Property URL() As String
            Get
                Return _url
            End Get
            Set(ByVal value As String)
                _url = value
            End Set
        End Property
        

        Private _status As Integer = -1
        Public Property Status() As Integer
            Get
                Return _status
            End Get
            Set(ByVal value As Integer)
                _status = value
            End Set
        End Property



        Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
            Dim FileName As String = System.IO.Path.GetFileNameWithoutExtension(Me.AppRelativeVirtualPath)
            If Not (Me.ID Is Nothing) Then
                'this will fix it when its placed as a ChildUserControl 
                Me.LocalResourceFile = Me.LocalResourceFile.Replace(Me.ID, FileName)
            Else
                ' this will fix it when its dynamically loaded using LoadControl method 
                Me.LocalResourceFile = Me.LocalResourceFile & FileName & ".ascx.resx"
                Dim Locale = System.Threading.Thread.CurrentThread.CurrentCulture.Name
                Dim AppLocRes As New System.IO.DirectoryInfo(Me.LocalResourceFile.Replace(FileName & ".ascx.resx", ""))
                If Locale = PortalSettings.CultureCode Then
                    'look for portal varient
                    If AppLocRes.GetFiles(FileName & ".ascx.Portal-" & PortalId & ".resx").Count > 0 Then
                        Me.LocalResourceFile = Me.LocalResourceFile.Replace("resx", "Portal-" & PortalId & ".resx")
                    End If
                Else

                    If AppLocRes.GetFiles(FileName & ".ascx." & Locale & ".Portal-" & PortalId & ".resx").Count > 0 Then
                        'lookFor a CulturePortalVarient
                        Me.LocalResourceFile = Me.LocalResourceFile.Replace("resx", Locale & ".Portal-" & PortalId & ".resx")
                    ElseIf AppLocRes.GetFiles(FileName & ".ascx." & Locale & ".resx").Count > 0 Then
                        'look for a CultureVarient
                        Me.LocalResourceFile = Me.LocalResourceFile.Replace("resx", Locale & ".resx")
                    ElseIf AppLocRes.GetFiles(FileName & ".ascx.Portal-" & PortalId & ".resx").Count > 0 Then
                        'lookFor a PortalVarient
                        Me.LocalResourceFile = Me.LocalResourceFile.Replace("resx", "Portal-" & PortalId & ".resx")
                    End If
                End If
            End If
        End Sub


        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
            If Status >= 0 Then


                Dim ds As New StaffBrokerDataContext

                Dim allStaff = From c In ds.AP_StaffBroker_Staffs Where c.PortalId = PortalId And c.Active
                Dim AllStaffNode As New TreeNode("All Staff")

                If _mode = "Team" Then
                    allStaff = StaffBrokerFunctions.GetTeam(UserId).Select(Function(c) c.AP_StaffBroker_Staffs).AsQueryable
                    AllStaffNode = New TreeNode("Team")

                End If





                AllStaffNode.SelectAction = TreeNodeSelectAction.Expand
                AllStaffNode.Expanded = True
                Dim d As New MPD.MPDDataContext
                Dim allBudgets = From c In d.AP_mpdCalc_StaffBudgets
                              Where c.AP_mpdCalc_Definition.PortalId = PortalId And c.Status = _status
                             Select c.StaffBudgetId, c.BudgetYearStart, c.StaffId, c.Status

                For Each s In allStaff
                    Dim node As New TreeNode(s.DisplayName)
                    node.Expanded = False
                    node.SelectAction = TreeNodeSelectAction.Expand
                    For Each row In allBudgets.Where(Function(x) x.StaffId = s.StaffId)
                        Dim node2 As New TreeNode()
                        node2.Text = "<span class='mpd-menu-tvtitle'>" & row.BudgetYearStart & " - " & row.BudgetYearStart + 1 & "</span>"
                        node2.NavigateUrl = URL & "?sb=" & row.StaffBudgetId

                        node.ChildNodes.Add(node2)


                    Next
                    AllStaffNode.ChildNodes.Add(node)
                Next
                tvStaff.Nodes.Clear()
                tvStaff.Nodes.Add(AllStaffNode)
            End If
        End Sub








    End Class
End Namespace

