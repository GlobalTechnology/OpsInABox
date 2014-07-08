Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports StaffBroker
Imports System.Linq
' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class AgapeRmb
     Inherits System.Web.Services.WebService
    Private theUsername As String = "AgapeRmb"
    Private thePassword As String = "thecatsaysmeow3"

    Structure RmbListItem
        Dim RmbNo As Integer
        Dim RmbTitle As String
    End Structure
   

    Structure Rmb
        Dim RmbNo As Integer
        Dim UserId As Integer
        Dim User As String
        Dim CostCentre As String
        Dim UserRef As String
        Dim RmbDate As Date
        Dim Status As Integer
        Dim StatusName As String
        Dim UserComment As String
        Dim ApprComment As String
        Dim AcctComment As String
        Dim ApprUserId As Integer
        Dim Approver As String
        Dim Locked As Boolean
        Dim ApprDate As Date
        Dim ProcDate As Date
        Dim Period As Integer
        Dim YEar As Integer
        Dim SupplierCode As String
        Dim EmailSent As Boolean
        Dim PersonalCC As String
        Dim AdvanceRequest As Double
        Dim RmbLines() As RmbLine
    End Structure
  
    Structure RmbLine
        Dim RmbLineNo As Integer
        Dim RmbNo As Integer
        Dim LineType As Integer
        Dim LineTypeName As String
        Dim GrossAmount As Double
        Dim VATAmount As Double
        Dim VATCode As String
        Dim VATRate As Double
        Dim TransDate As Date
        Dim Comment As String
        Dim Taxable As Boolean
        Dim Receipt As Boolean
        Dim VATReceipt As Boolean
        Dim ReceiptNo As Integer
        Dim Spare1 As String
        Dim Spare2 As String
        Dim Spare3 As String
        Dim Spare4 As String
        Dim Spare5 As String
        Dim AnalysisCode As String
        Dim OutOfDate As Boolean
        Dim LargeTransaction As Boolean
        Dim Split As Boolean
        
    End Structure
    Structure AgapeUser
        Dim UserId As Integer
        Dim FirstName As String
        Dim LastName As String
        Dim DisplayName As String
        Dim Email As String
    End Structure
    Structure AdvanceRequest
        Dim AdvanceId As Integer
        Dim User As String
        Dim Approver As String
        Dim RequestText As String
        Dim RequestAmount As Double
        Dim SubDate As Date
        Dim ApprDate As Date
        Dim ProcDate As Date
        Dim AdvanceTitle As String
        Dim Status As Integer
        Dim Period As Integer
        Dim Year As Integer
        Dim SupplierCode As String
        Dim UserId As Integer
    End Structure
    Structure UpdateAdvance
        Dim AdvanceId As Integer
        Dim RequestText As String
        Dim RequestAmount As Double
        Dim Status As Integer
        Dim Period As Integer
        Dim Year As Integer
    End Structure


    <WebMethod()> _
    Public Function HelloWorld() As String
        Return "Hello World"
    End Function
    
   

    '<WebMethod()> _
    'Public Function GetRecentRmbs(ByVal UserName As String, ByVal Password As String, ByVal Count As Integer, ByVal Status As Integer) As RmbListItem()
    '    If (UserName <> theUsername Or Password <> thePassword) Then
    '        Return Nothing
    '    End If
    '    Dim st As Integer = Status
    '    If st = -2 Then
    '        st = 2
    '    End If

    '    Dim d As New AgapeStaffDataContext
    '    Dim q = (From c In d.Agape_Staff_Rmbs Where ((c.Agape_Staff_Rmb_Status.StatusNo = st) Or st = -1) And c.PortalId = 0 Order By c.RMBNo Descending Select c).Take(100)

    '    If Status = 2 Then
    '        q = From c In q Where (From b In c.Agape_Staff_RmbLines Where b.Receipt = True).Count = 0
    '    ElseIf Status = -2 Then
    '        q = From c In q Where (From b In c.Agape_Staff_RmbLines Where b.Receipt = True).Count > 0
    '    End If


    '    Dim rtn As RmbListItem()
    '    rtn = New RmbListItem(q.Count - 1) {}
    '    Dim i As Integer = 0
    '    For Each row In q
    '        rtn(i).RmbNo = row.RMBNo
    '        If row.RmbDate Is Nothing Then
    '            rtn(i).RmbTitle = "Rmb#" & row.RMBNo & " - " & UserController.GetUser(0, row.UserId, False).DisplayName

    '        Else
    '            rtn(i).RmbTitle = "Rmb#" & row.RMBNo & " - " & UserController.GetUser(0, row.UserId, False).DisplayName & " " & row.RmbDate.Value.ToString("dd/MM/yyyy")

    '        End If
    '        i += 1


    '    Next
    '    Return rtn

    'End Function



    '<WebMethod()> _
    'Public Function SearchRmbs(ByVal UserName As String, ByVal Password As String, ByVal Count As Integer, ByVal theStatus As Integer, ByVal search As String, ByVal mode As Integer) As RmbListItem()
    '    If (UserName <> theUsername Or Password <> thePassword) Then
    '        Return Nothing
    '    End If
    '    Dim st As Integer = theStatus
    '    If st = -2 Then
    '        st = 2
    '    End If

    '    Dim d As New AgapeStaffDataContext
    '    Dim q As IQueryable(Of AgapeStaff.Agape_Staff_Rmb)


    '    Select Case mode
    '        Case 1
    '            ' RMB#
    '            Try
    '                Dim searchNum As Integer = search
    '                q = (From c In d.Agape_Staff_Rmbs Where c.PortalId = 0 And c.RMBNo = searchNum Order By c.RmbDate Descending).Take(50)

    '            Catch ex As Exception
    '                q = From c In d.Agape_Staff_Rmbs Where 1 = 0
    '            End Try
    '        Case 2
    '            ' Staff Name


    '            Dim objRC As DotNetNuke.Security.Roles.RoleController = New DotNetNuke.Security.Roles.RoleController

    '            Dim staff = From c In objRC.GetUserRolesByRoleName(0, "Staff") Join b In d.Users On c.UserID Equals b.UserID Select b

    '            Dim searchStaff = From c In staff Where 1 = 0 Select c.UserID

    '            For Each word In Split(search)
    '                Dim searchWord = word
    '                searchStaff = searchStaff.Union(From c In staff Where c.FirstName.StartsWith(searchWord) Or c.LastName.StartsWith(searchWord) Select c.UserID)

    '            Next

    '            q = (From c In d.Agape_Staff_Rmbs Where c.PortalId = 0 And searchStaff.Contains(c.UserId) And ((c.Agape_Staff_Rmb_Status.StatusNo = st) Or st = -1) Order By c.RMBNo Descending).Take(50)




    '        Case Else
    '            ' mm/yyyy
    '            Try
    '                Dim searchMonth As Integer = Left(search, 2)
    '                Dim SearchYear As Integer = Right(search, 4)
    '                q = (From c In d.Agape_Staff_Rmbs Where c.PortalId = 0 And c.RmbDate.Value.Month = searchMonth And c.RmbDate.Value.Year = SearchYear And ((c.Agape_Staff_Rmb_Status.StatusNo = st) Or st = -1) Order By c.RMBNo Descending).Take(50)

    '            Catch ex As Exception
    '                q = From c In d.Agape_Staff_Rmbs Where 1 = 0
    '            End Try

    '    End Select

    '    If theStatus = 2 Then
    '        q = From c In q Where (From b In c.Agape_Staff_RmbLines Where b.Receipt = True).Count = 0
    '    ElseIf theStatus = -2 Then
    '        q = From c In q Where (From b In c.Agape_Staff_RmbLines Where b.Receipt = True).Count > 0
    '    End If
    '    'Dim q = (From c In d.Agape_Staff_Rmbs Where c.Status = Status And c.PortalId = 0 Order By c.RmbDate Descending Select c.RMBNo, c.UserId).Take(100)
    '    q = From c In q Order By c.RMBNo

    '    Dim rtn As RmbListItem()
    '    rtn = New RmbListItem(q.Count - 1) {}
    '    Dim i As Integer = 0
    '    For Each row In q
    '        rtn(i).RmbNo = row.RMBNo
    '        rtn(i).RmbTitle = "Rmb#" & row.RMBNo & " - " & UserController.GetUser(0, row.UserId, False).DisplayName
    '        i += 1


    '    Next
    '    Return rtn

    'End Function




    '<WebMethod()> _
    '    Public Function GetRmb(ByVal UserName As String, ByVal Password As String, ByVal RmbNo As Integer) As Rmb
    '    If (UserName <> theUsername Or Password <> thePassword) Then
    '        Return Nothing
    '    End If
    '    Dim d As New AgapeStaffDataContext
    '    Dim q = From c In d.Agape_Staff_Rmbs Where c.RMBNo = RmbNo
    '    If q.Count > 0 Then
    '        Dim rtn As New Rmb
    '        rtn.RmbNo = q.First.RMBNo
    '        rtn.UserId = q.First.UserId
    '        rtn.User = UserController.GetUser(0, q.First.UserId, False).DisplayName
    '        rtn.CostCentre = q.First.CostCenter
    '        rtn.UserRef = q.First.UserRef
    '        If Not q.First.RmbDate Is Nothing Then
    '            rtn.RmbDate = q.First.RmbDate
    '        End If

    '        rtn.Status = q.First.Agape_Staff_Rmb_Status.StatusNo
    '        rtn.StatusName = q.First.Agape_Staff_Rmb_Status.Status
    '        rtn.UserComment = q.First.UserComment
    '        rtn.ApprComment = q.First.ApprComment
    '        rtn.AcctComment = q.First.AcctComment
    '        rtn.PersonalCC = q.First.PersonalCC
    '        If Not q.First.ApprUserId Is Nothing Then
    '            rtn.ApprUserId = q.First.ApprUserId

    '            rtn.Approver = UserController.GetUser(0, q.First.ApprUserId, False).DisplayName
    '        End If

    '        rtn.Locked = q.First.Locked

    '        If Not q.First.ApprDate Is Nothing Then
    '            rtn.ApprDate = q.First.ApprDate
    '        End If
    '        If Not q.First.ProcDate Is Nothing Then
    '            rtn.ProcDate = q.First.ProcDate
    '        End If
    '        If Not q.First.Period Is Nothing Then
    '            rtn.Period = q.First.Period
    '        End If
    '        If Not q.First.Year Is Nothing Then
    '            rtn.YEar = q.First.Year
    '        End If
    '        If Not q.First.SupplierCode Is Nothing Then
    '            rtn.SupplierCode = q.First.SupplierCode
    '        End If

    '        If Not q.First.EmailSent Is Nothing Then
    '            rtn.EmailSent = q.First.EmailSent
    '        End If

    '        If Not q.First.AdvanceRequest Is Nothing Then
    '            rtn.AdvanceRequest = q.First.AdvanceRequest
    '        End If

    '        rtn.RmbLines = New RmbLine(q.First.Agape_Staff_RmbLines.Count - 1) {}

    '        Dim i As Integer = 0
    '        For Each row In From c In q.First.Agape_Staff_RmbLines Order By c.ReceiptNo
    '            rtn.RmbLines(i).RmbLineNo = row.RmbLineNo
    '            rtn.RmbLines(i).RmbNo = row.RmbNo
    '            rtn.RmbLines(i).LineType = row.LineType
    '            rtn.RmbLines(i).LineTypeName = row.Agape_Staff_RmbLineType.TypeName
    '            rtn.RmbLines(i).GrossAmount = row.GrossAmount
    '            rtn.RmbLines(i).TransDate = row.TransDate
    '            rtn.RmbLines(i).Comment = row.Comment
    '            rtn.RmbLines(i).Taxable = row.Taxable
    '            rtn.RmbLines(i).Receipt = row.Receipt
    '            rtn.RmbLines(i).VATReceipt = row.VATReceipt

    '            If Not row.ReceiptNo Is Nothing Then
    '                rtn.RmbLines(i).ReceiptNo = row.ReceiptNo
    '            End If

    '            rtn.RmbLines(i).Spare1 = row.Spare1
    '            rtn.RmbLines(i).Spare2 = row.Spare2
    '            rtn.RmbLines(i).Spare3 = row.Spare3
    '            rtn.RmbLines(i).Spare4 = row.Spare4
    '            rtn.RmbLines(i).Spare5 = row.Spare5
    '            If Not row.AnalysisCode Is Nothing Then
    '                rtn.RmbLines(i).AnalysisCode = row.AnalysisCode
    '            End If

    '            rtn.RmbLines(i).VATAmount = row.VATAmount
    '            rtn.RmbLines(i).VATCode = row.VATCode
    '            rtn.RmbLines(i).VATRate = row.VATRate

    '            rtn.RmbLines(i).OutOfDate = row.OutOfDate
    '            rtn.RmbLines(i).LargeTransaction = row.LargeTransaction
    '            rtn.RmbLines(i).Split = row.Split


    '            i += 1
    '        Next
    '        Return rtn
    '    Else
    '        Return Nothing
    '    End If

    'End Function

    'Protected Function GetAnalysisCode(ByVal CC As String, ByVal LineType As Integer) As String
    '    Dim d As New AgapeStaffDataContext
    '    Dim Acc = From c In d.Agape_Staff_RmbLineTypes Where c.TypeNo = LineType Select c.DCode, c.PCode



    '    If CC.Count > 0 And Acc.Count > 0 Then
    '        If Right(CC, 1) = "X" Then
    '            Return "D-" & Left(CC, 3) & "X-" & Acc.First.DCode
    '        Else
    '            Return "P-" & Left(CC, 3) & "0-" & Acc.First.PCode
    '        End If

    '    Else
    '        Return CC
    '    End If


    'End Function

    '<WebMethod()> _
    'Public Function UpdateRmb(ByVal UserName As String, ByVal Password As String, ByVal updateRmbLine As RmbLine) As Boolean
    '    If (UserName <> theUsername Or Password <> thePassword) Then
    '        Return Nothing
    '    End If

    '    Dim d As New AgapeStaffDataContext
    '    Dim q = From c In d.Agape_Staff_RmbLines Where c.RmbLineNo = updateRmbLine.RmbLineNo
    '    If q.Count > 0 Then
    '        q.First.GrossAmount = updateRmbLine.GrossAmount
    '        q.First.TransDate = updateRmbLine.TransDate
    '        q.First.Comment = updateRmbLine.Comment
    '        q.First.Taxable = updateRmbLine.Taxable
    '        q.First.Receipt = updateRmbLine.Receipt
    '        q.First.ReceiptNo = updateRmbLine.ReceiptNo
    '        q.First.VATReceipt = updateRmbLine.VATReceipt
    '        q.First.Spare1 = updateRmbLine.Spare1
    '        q.First.Spare2 = updateRmbLine.Spare2
    '        q.First.Spare3 = updateRmbLine.Spare3
    '        q.First.Spare4 = updateRmbLine.Spare4
    '        q.First.Spare5 = updateRmbLine.Spare5

    '        If updateRmbLine.AnalysisCode.Length < 6 Then
    '            q.First.AnalysisCode = GetAnalysisCode(updateRmbLine.AnalysisCode, updateRmbLine.LineType)
    '        Else
    '            q.First.AnalysisCode = updateRmbLine.AnalysisCode
    '        End If





    '        q.First.VATAmount = updateRmbLine.VATAmount
    '        q.First.VATCode = updateRmbLine.VATCode
    '        q.First.VATRate = updateRmbLine.VATRate
    '        q.First.OutOfDate = updateRmbLine.OutOfDate
    '        q.First.LargeTransaction = updateRmbLine.LargeTransaction
    '        q.First.Split = updateRmbLine.Split





    '        d.SubmitChanges()
    '        Return True
    '    Else
    '        Return False
    '    End If



    'End Function
    '<WebMethod()> _
    'Public Function GetUser(ByVal UserName As String, ByVal Password As String, ByVal UserId As Integer) As AgapeUser
    '    If (UserName <> theUsername Or Password <> thePassword) Then
    '        Return Nothing
    '    End If
    '    Dim rtn As AgapeUser
    '    rtn.UserId = UserId
    '    Dim theUser = UserController.GetUser(0, UserId, False)
    '    rtn.FirstName = theUser.FirstName
    '    rtn.LastName = theUser.LastName
    '    rtn.DisplayName = theUser.DisplayName
    '    rtn.Email = theUser.Email
    '    Return rtn

    'End Function
    '<WebMethod()> _
    'Public Function UpdateRmbHeader(ByVal UserName As String, ByVal Password As String, ByVal RmbNo As Integer, ByVal acctComment As String, ByVal ProcDate As Date, ByVal Status As Integer, ByVal Period As Integer, ByVal theYear As Integer, ByVal SupplierCode As String, ByVal EmailSent As Boolean) As Boolean
    '    If (UserName <> theUsername Or Password <> thePassword) Then
    '        Return Nothing
    '    End If

    '    Dim d As New AgapeStaffDataContext
    '    Dim q = From c In d.Agape_Staff_Rmbs Where c.RMBNo = RmbNo
    '    If q.Count > 0 Then
    '        q.First.AcctComment = acctComment
    '        If Not ProcDate = Nothing Then
    '            q.First.ProcDate = ProcDate
    '        End If

    '        q.First.Status = (From c In d.Agape_Staff_Rmb_Status Where c.StatusNo = Status Select c.StatusId).First
    '        q.First.Period = Period
    '        q.First.Year = theYear
    '        q.First.SupplierCode = SupplierCode
    '        q.First.EmailSent = EmailSent

    '        If Status = 3 Or Status = 4 Then
    '            'if processed/complete approved - then lock
    '            q.First.Locked = True
    '        Else
    '            'if cancelled or more info - then unlock
    '            q.First.Locked = False

    '        End If
    '        d.SubmitChanges()
    '        Return True
    '    Else
    '        Return False
    '    End If



    'End Function
    '<WebMethod()> _
    'Public Sub AddRmb(ByVal UserName As String, ByVal Password As String, ByVal updateRmbLine As RmbLine)
    '    If (UserName <> theUsername Or Password <> thePassword) Then
    '        Return
    '    End If

    '    Dim d As New AgapeStaffDataContext
    '    Dim insert As New Agape_Staff_RmbLine
    '    insert.LineType = updateRmbLine.LineType
    '    insert.RmbNo = updateRmbLine.RmbNo
    '    insert.GrossAmount = updateRmbLine.GrossAmount
    '    insert.TransDate = updateRmbLine.TransDate
    '    insert.Comment = updateRmbLine.Comment
    '    insert.Taxable = updateRmbLine.Taxable
    '    insert.Receipt = updateRmbLine.Receipt
    '    insert.ReceiptNo = updateRmbLine.ReceiptNo
    '    insert.Spare1 = updateRmbLine.Spare1
    '    insert.Spare2 = updateRmbLine.Spare2
    '    insert.Spare3 = updateRmbLine.Spare3
    '    insert.Spare4 = updateRmbLine.Spare4
    '    insert.Spare5 = updateRmbLine.Spare5
    '    If updateRmbLine.AnalysisCode.Length < 6 Then
    '        insert.AnalysisCode = GetAnalysisCode(updateRmbLine.AnalysisCode, updateRmbLine.LineType)
    '    Else
    '        insert.AnalysisCode = updateRmbLine.AnalysisCode
    '    End If
    '    insert.VATReceipt = updateRmbLine.VATReceipt
    '    insert.VATAmount = updateRmbLine.VATAmount
    '    insert.VATCode = updateRmbLine.VATCode
    '    insert.VATRate = updateRmbLine.VATRate
    '    insert.OutOfDate = updateRmbLine.OutOfDate
    '    insert.LargeTransaction = updateRmbLine.LargeTransaction
    '    insert.Split = updateRmbLine.Split


    '    d.Agape_Staff_RmbLines.InsertOnSubmit(insert)

    '    d.SubmitChanges()





    'End Sub


    '<WebMethod()> _
    'Public Sub DeleteRmb(ByVal UserName As String, ByVal Password As String, ByVal RmbLineNo As Integer)
    '    If (UserName <> theUsername Or Password <> thePassword) Then
    '        Return
    '    End If

    '    Dim d As New AgapeStaffDataContext
    '    Dim q = From c In d.Agape_Staff_RmbLines Where c.RmbLineNo = RmbLineNo
    '    d.Agape_Staff_RmbLines.DeleteAllOnSubmit(q)
    '    d.SubmitChanges()


    'End Sub
    '<WebMethod()> _
    'Public Function GetLineTypes(ByVal UserName As String, ByVal Password As String) As RmbListItem()
    '    If (UserName <> theUsername Or Password <> thePassword) Then
    '        Return Nothing
    '    End If
    '    Dim d As New AgapeStaffDataContext
    '    Dim rtn As RmbListItem()
    '    rtn = New RmbListItem(d.Agape_Staff_RmbLineTypes.Count - 1) {}
    '    Dim i As Integer = 0

    '    For Each row In d.Agape_Staff_RmbLineTypes
    '        rtn(i).RmbNo = row.TypeNo
    '        rtn(i).RmbTitle = row.TypeName

    '        i += 1

    '    Next
    '    Return rtn
    'End Function

    '<WebMethod()> _
    'Public Function GetStatuses(ByVal UserName As String, ByVal Password As String) As RmbListItem()
    '    If (UserName <> theUsername Or Password <> thePassword) Then
    '        Return Nothing
    '    End If
    '    Dim d As New AgapeStaffDataContext
    '    Dim rtn As RmbListItem()
    '    rtn = New RmbListItem(d.Agape_Staff_Rmb_Status.Count - 1) {}
    '    Dim i As Integer = 0

    '    For Each row In d.Agape_Staff_Rmb_Status
    '        rtn(i).RmbNo = row.StatusNo
    '        rtn(i).RmbTitle = row.Status

    '        i += 1

    '    Next
    '    Return rtn
    'End Function
    '<WebMethod()> _
    'Public Function GetRecentAdvanceRequests(ByVal UserName As String, ByVal Password As String, ByVal Count As Integer, ByVal Status As Integer) As AdvanceRequest()
    '    If (UserName <> theUsername Or Password <> thePassword) Then
    '        Return Nothing
    '    End If
    '    Dim st As Integer = Status
    '    Dim d As New AgapeStaffDataContext
    '    Dim q = From c In d.Agape_Main_AdvanceRequests Where 1 = 0
    '    If st = -1 Then
    '        q = (From c In d.Agape_Main_AdvanceRequests Order By c.RequestDate Descending Select c).Take(100)
    '    Else
    '        q = (From c In d.Agape_Main_AdvanceRequests Where c.RequestStatus = st Order By c.RequestDate Descending Select c).Take(100)
    '        If Count <> -1 Then
    '            q = (From c In d.Agape_Main_AdvanceRequests Where c.RequestStatus = st Order By c.RequestDate Descending Select c).Take(Count)
    '        End If
    '    End If




    '    Dim rtn As AdvanceRequest()
    '    rtn = New AdvanceRequest(q.Count - 1) {}
    '    Dim i As Integer = 0
    '    For Each request In q
    '        rtn(i).AdvanceId = request.AdvanceId
    '        If request.ApprovedDate Is Nothing Then
    '            rtn(i).ApprDate = Nothing
    '        Else
    '            rtn(i).ApprDate = request.ApprovedDate
    '        End If
    '        Dim r = From c In d.Users Where c.UserID = request.ApproverId Select c.DisplayName
    '        If r.Count > 0 Then
    '            rtn(i).Approver = r.First
    '        Else
    '            rtn(i).Approver = ""
    '        End If
    '        rtn(i).RequestAmount = CDbl(request.RequestAmount)
    '        rtn(i).RequestText = request.RequestText
    '        rtn(i).SubDate = request.RequestDate
    '        rtn(i).UserId = request.StaffId
    '        Dim s = From c In d.Users Where c.UserID = request.StaffId Select c
    '        If s.Count > 0 Then
    '            rtn(i).User = s.First.DisplayName
    '        Else
    '            rtn(i).User = ""
    '        End If

    '        If request.ProcessedDate Is Nothing Then
    '            rtn(i).ProcDate = Nothing
    '        Else
    '            rtn(i).ProcDate = request.ProcessedDate
    '        End If
    '        rtn(i).AdvanceTitle = rtn(i).User & "-" & rtn(i).SubDate.ToString("dd/MM/yyyy")
    '        rtn(i).Status = request.RequestStatus
    '        If request.Period Is Nothing Then
    '            rtn(i).Period = 0
    '        Else
    '            rtn(i).Period = request.Period
    '        End If
    '        If request.Year Is Nothing Then
    '            rtn(i).Year = 0
    '        Else
    '            rtn(i).Year = request.Year
    '        End If

    '        Dim t = From c In d.Agape_Staff_Finances Where c.UserId1 = request.StaffId
    '        t = t.Union(From c In d.Agape_Staff_Finances Where c.USerId2 = request.StaffId)
    '        If t.Count > 0 Then
    '            rtn(i).SupplierCode = t.First.CostCentre
    '        End If


    '        i = i + 1
    '    Next

    '    Return rtn

    'End Function
    '<WebMethod()> _
    'Public Function GetAdvanceRequest(ByVal UserName As String, ByVal Password As String, ByVal AdvanceNo As Integer) As AdvanceRequest
    '    If (UserName <> theUsername Or Password <> thePassword) Then
    '        Return Nothing
    '    End If
    '    Dim rtn As AdvanceRequest = Nothing
    '    Dim d As New AgapeStaffDataContext
    '    Dim q = From c In d.Agape_Main_AdvanceRequests Where c.AdvanceId = AdvanceNo Select c
    '    If q.Count > 0 Then
    '        rtn.AdvanceId = AdvanceNo
    '        If q.First.ApprovedDate Is Nothing Then
    '            rtn.ApprDate = Nothing
    '        Else
    '            rtn.ApprDate = q.First.ApprovedDate
    '        End If
    '        If q.First.ProcessedDate Is Nothing Then
    '            rtn.ProcDate = Nothing
    '        Else
    '            rtn.ProcDate = q.First.ProcessedDate
    '        End If
    '        If q.First.ApproverId Is Nothing Then
    '            rtn.Approver = ""
    '        Else
    '            Dim r = From c In d.Users Where c.UserID = q.First.ApproverId Select c.DisplayName
    '            If r.Count > 0 Then
    '                rtn.Approver = r.First
    '            Else
    '                rtn.Approver = ""
    '            End If
    '        End If
    '        rtn.RequestAmount = CDbl(q.First.RequestAmount)
    '        rtn.RequestText = q.First.RequestText
    '        rtn.SubDate = q.First.RequestDate
    '        rtn.UserId = q.First.StaffId
    '        Dim s = From c In d.Users Where c.UserID = q.First.StaffId Select c.DisplayName
    '        If s.Count > 0 Then
    '            rtn.User = s.First
    '        Else
    '            rtn.User = ""
    '        End If
    '        rtn.AdvanceTitle = rtn.User & "-" & rtn.SubDate.ToString("dd/MM/yyyy")
    '        rtn.Status = q.First.RequestStatus
    '        If q.First.Period Is Nothing Then
    '            rtn.Period = 0
    '        Else
    '            rtn.Period = q.First.Period
    '        End If
    '        If q.First.Year Is Nothing Then
    '            rtn.Year = 0
    '        Else
    '            rtn.Year = q.First.Year
    '        End If
    '        Dim t = From c In d.Agape_Staff_Finances Where c.UserId1 = q.First.StaffId
    '        t = t.Union(From c In d.Agape_Staff_Finances Where c.USerId2 = q.First.StaffId)
    '        If t.Count > 0 Then
    '            rtn.SupplierCode = "P-" & Strings.Left(t.First.CostCentre, 3) & "0"
    '        End If
    '    End If

    '    Return rtn
    'End Function
    '<WebMethod()> _
    'Public Function UpdateRequest(ByVal UserName As String, ByVal Password As String, ByVal updateAdvance As UpdateAdvance) As Boolean
    '    If (UserName <> theUsername Or Password <> thePassword) Then
    '        Return Nothing
    '    End If
    '    Dim Success As Boolean = False
    '    Dim d As New AgapeStaffDataContext
    '    Dim q = From c In d.Agape_Main_AdvanceRequests Where c.AdvanceId = updateAdvance.AdvanceId Select c
    '    If q.Count > 0 Then
    '        q.First.RequestAmount = updateAdvance.RequestAmount
    '        q.First.RequestText = updateAdvance.RequestText
    '        If updateAdvance.Status > 0 Then
    '            q.First.RequestStatus = updateAdvance.Status
    '        End If
    '        If updateAdvance.Status = 4 Then
    '            q.First.ProcessedDate = CDate(Now())
    '        End If
    '        If Not q.First.ProcessedDate Is Nothing And updateAdvance.Status <> 4 Then
    '            q.First.ProcessedDate = Nothing
    '        End If
    '        If Not updateAdvance.Period = 0 Then
    '            q.First.Period = updateAdvance.Period
    '        End If
    '        If Not updateAdvance.Year = 0 Then
    '            q.First.Year = updateAdvance.Year
    '        End If
    '        d.SubmitChanges()
    '        Success = True

    '    End If

    '    Return Success
    'End Function

End Class
