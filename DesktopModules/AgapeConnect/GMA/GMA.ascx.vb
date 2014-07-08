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
Imports GmaServices

Namespace DotNetNuke.Modules.GMA
    Partial Class GMA
        Inherits Entities.Modules.PortalModuleBase

        'Dim dStaff As New AgapeStaffDataContext
        ' Dim dBroke As New StaffBrokerDataContext
        '  Dim d As New DNNProfileDataContextDataContext
        Private gmaServers As New List(Of gmaServer)
        Public ReportData As New Dictionary(Of String, Integer())

      

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load

            If Not Page.IsPostBack Then
                gmaServers.Clear()
                Dim insert As New gmaServer
                insert.name = "AgapeConnect"

                insert.URL = "http://gma.agapeconnect.me/index.php"
                insert.gma = New GmaServices(insert.URL, UserInfo.Profile.GetPropertyValue("GCXPGTIOU"))
                Dim Reports = insert.gma.GetStaffReports()
                Dim DirectorReports = insert.gma.GetDirectorReports()

                insert.nodes = insert.gma.GetUserNodes(Reports, DirectorReports)



                gmaServers.Add(insert)


                Session("gmaServers") = gmaServers


                rpGmaServers.DataSource = From c In gmaServers Select name = c.name, url = c.URL, nodes = (From b In c.nodes Select shortName = b.shortName, nodeId = b.nodeId, url = c.URL)

                rpGmaServers.DataBind()

                Try
                    Dim firstServer = gmaServers.Where(Function(c) c.nodes.Count > 0).First

                    SelectNode(firstServer.URL & ":::" & firstServer.nodes.First.nodeId)
                Catch ex As Exception
                    Label1.Text = ex.Message
                End Try




            End If
          

        End Sub

        Private Sub SelectNode(ByVal ref As String)
            If gmaServers.Count = 0 Then
                gmaServers = Session("gmaServers")
            End If


            Dim myNodeId = ref.Substring(ref.IndexOf(":::") + 3)
            Dim myUrl = ref.Substring(0, ref.IndexOf(":::"))

            hfNodeId.Value = myNodeId
            hfURL.Value = myUrl
            Dim gmaServer = (From c In gmaServers Where c.URL = myUrl).First

            Dim gmaNode = (From c In gmaServer.nodes Where c.nodeId = myNodeId).First
            lblNodeTitle.Text = gmaNode.shortName

            If Not gmaNode.AdvancedReportData Is Nothing Then
                ReportData = gmaNode.AdvancedReportData
            End If

            ddlPeriods.DataSource = From c In gmaNode.Reports Select LabelName = c.startDate.ToString("dd MMM yy") & " - " & c.endDate.ToString("dd MMM yy"), c.ReportId
            ddlPeriods.DataBind()
            If gmaNode.Reports.Count > 0 Then
                ddlPeriods.SelectedValue = gmaNode.Reports.Last.ReportId
                ddlPeriods_SelectedIndexChanged(Me, Nothing)
                tabStaff.Visible = True
            Else
                tabStaff.Visible = False
                Dim m As New List(Of gma_measurements)
                rpStaffMeasurements.DataSource = m
                rpStaffMeasurements.DataBind()
                lbPrevPeriod.Enabled = Not (ddlPeriods.SelectedIndex = 0 Or ddlPeriods.Items.Count = 0)

                lbNextPeriod.Enabled = Not (ddlPeriods.SelectedIndex = ddlPeriods.Items.Count - 1 Or ddlPeriods.Items.Count = 0)
            End If

            ddlPeriodsD.DataSource = From c In gmaNode.DirectorReports Select LabelName = c.startDate.ToString("dd MMM yy") & " - " & c.endDate.ToString("dd MMM yy"), c.ReportId
            ddlPeriodsD.DataBind()

            If (gmaNode.DirectorReports.Count > 0) Then



                ddlPeriodsD.SelectedValue = gmaNode.DirectorReports.Last.ReportId
                ddlPeriodsD_SelectedIndexChanged(Me, Nothing)
                tabDirector.Visible = True





            Else
                tabDirector.Visible = False
                Dim m As New List(Of gma_measurements)
                rpDirectorMeasuremts.DataSource = m
                rpDirectorMeasuremts.DataBind()
                lbPrevPeriod.Enabled = Not (ddlPeriods.SelectedIndex = 0 Or ddlPeriods.Items.Count = 0)

                lbNextPeriod.Enabled = Not (ddlPeriods.SelectedIndex = ddlPeriods.Items.Count - 1 Or ddlPeriods.Items.Count = 0)
            End If







            ' Label1.Text = Report.InnerXml
            '  imgWin.Src = "data:image/jpg;base64," & gmaServer.gma.GetReportGraph(myNodeId, 497) & ""
        End Sub








        Protected Sub rpNodes_ItemCommand(source As Object, e As RepeaterCommandEventArgs)
            If e.CommandName = "nodeSelected" Then
                SelectNode(e.CommandArgument)

                'Label1.Text = gma.GetStaffReport(reports.Last.ReportId).First.measurementName
                If gmaServers.Count = 0 Then
                    gmaServers = Session("gmaServers")
                End If

                rpGmaServers.DataSource = From c In gmaServers Select name = c.name, url = c.URL, nodes = (From b In c.nodes Select shortName = b.shortName, nodeId = b.nodeId, url = c.URL)

                rpGmaServers.DataBind()


            End If
        End Sub

        Protected Sub ddlPeriods_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlPeriods.SelectedIndexChanged
            If gmaServers.Count = 0 Then
                gmaServers = Session("gmaServers")
            End If

            ' Dim myNodeId As Integer = hfNodeId.Value
            Dim myURL As String = hfURL.Value
            Dim gmaServer = (From c In gmaServers Where c.URL = myURL).First
            Dim sr = (From c In gmaServer.gma.GetStaffReport(CInt(ddlPeriods.SelectedValue))).ToList

            If ReportData.Count = 0 And sr.Count > 0 Then


                Dim numericMeasurements = sr.Where(Function(c) c.measurementType = "numeric").Select(Function(c) CStr(c.measurementId))

                Dim calculatedMeasurements = sr.Where(Function(c) c.measurementType = "calculated").Select(Function(c) CStr(c.measurementId))



                Dim xml As String = gmaServer.gma.getReportData(CInt(hfNodeId.Value), numericMeasurements.ToArray, calculatedMeasurements.ToArray)
                'Label1.Text = xml

                xml = xml.Replace("<?xml version=""1.0"" encoding=""UTF-8""?><?mso-application progid=""Excel.Sheet""?>", "")
                Dim doc As New System.Xml.XmlDocument
                doc.LoadXml(xml)
                Label1.Text = xml
                Dim Report = doc.FirstChild.ChildNodes(2).FirstChild



                For Each row As System.Xml.XmlNode In Report.ChildNodes
                    If row.ChildNodes.Count > 1 Then
                        Dim Name = row.FirstChild.FirstChild.InnerText
                        If Not ReportData.ContainsKey(Name) Then
                            Dim Values As New List(Of Integer)

                            For Each cell As System.Xml.XmlNode In row.ChildNodes
                                If cell.HasChildNodes Then



                                    Dim value As Integer
                                    If cell.FirstChild.InnerText = Name Then

                                    ElseIf cell.FirstChild.InnerText = "" Or cell.FirstChild.InnerText = "-" Then
                                        'Values.Add(0)
                                    ElseIf Integer.TryParse(cell.FirstChild.InnerText, value) Then
                                        Values.Add(value)
                                    End If
                                End If



                            Next
                            ReportData.Add(Name, Values.ToArray)
                        End If
                        '  Label1.Text &= Name & Values.Count & vbNewLine

                    End If
                Next


                Dim myNode = gmaServer.nodes.Where(Function(c) c.nodeId = CInt(hfNodeId.Value)).First
                myNode.AdvancedReportData = ReportData
                Session("gmaServers") = gmaServers
           
            End If
            LoadStaffReport(sr)
            rpStaffMeasurements.DataSource = From c In sr Select c.measurementName, c.measurementValue, c.measurementDescription, c.measurementType, c.measurementId
            rpStaffMeasurements.DataBind()

            lbPrevPeriod.Enabled = Not (ddlPeriods.SelectedIndex = 0 Or ddlPeriods.Items.Count = 0)

            lbNextPeriod.Enabled = Not (ddlPeriods.SelectedIndex = ddlPeriods.Items.Count - 1 Or ddlPeriods.Items.Count = 0)


        End Sub

        Protected Sub LoadStaffReport(ByVal measurements As List(Of GmaServices.gma_measurements))
            Dim c As Integer = measurements.Count

            LoadTextbox({"Mass Exposures", "Mass"}, tbMass, dvMass, hfMass, measurements)
            LoadTextbox({"Personal Exposures", "Personal"}, tbExposures, dvExposures, hfExposures, measurements)
            LoadTextbox({"Presenting the Gospel", "Gospel"}, tbPresGosp, dvPresGosp, hfPresGosp, measurements)
            LoadTextbox({"Following Up", "Followed Up"}, tbFollowup, dvFollowup, hfFollowup, measurements)
            LoadTextbox({"Holy Spirit Presentations", "Holy Spirit"}, tbHSPres, dvHSPres, hfHSPres, measurements)
            LoadTextbox({"Training for Action", "Training"}, tbTraining, dvTraining, hfTraining, measurements)
            LoadTextbox({"Sending Lifetime Laborors", "Sending"}, tbSendLifeLab, dvSendLifeLab, hfSendLifeLab, measurements)
            LoadTextbox({"New Believers", "New"}, tbNewBel, dvNewBel, hfNewBel, measurements)
            LoadTextbox({"Engaged Disciples", "Engaged"}, tbEngagedDisc, dvEngagedDisc, hfEngagedDisc, measurements)
            LoadTextbox({"Multiplying Disciples", "Multiplying"}, tbMultDisc, dvMultDisc, hfMultDisc, measurements)
            LoadTextbox({"Developing Local Resources", "Developing"}, tbDevLocRes, dvDevLocRes, hfDevLocRes, measurements)
            LoadTextbox({"Locally Generated Resources", "Generated"}, tbLocGenRes, dvLocGenRes, hfLocGenRes, measurements)
            LoadTextbox({"Movement", "communities"}, tbmovement, dvmovement, hfmovement, measurements)
            If measurements.Count = c Then
                pnlLmiGrid.Visible = False
            End If

        End Sub

        Protected Sub LoadAnalysis()

        End Sub


        Protected Sub LoadTextbox(ByVal name As String(), ByRef tb As TextBox, ByRef DivControl As HtmlGenericControl, ByRef hfId As HiddenField, ByVal measurements As List(Of GmaServices.gma_measurements))
            For Each n In name


                Dim q = From c In measurements Where c.measurementName.ToLower.Contains(n.ToLower)
                If q.Count > 0 Then
                    tb.Text = q.First.measurementValue
                    hfId.Value = q.First.measurementId
                    measurements.Remove(q.First)
                    Return
                End If
            Next
            DivControl.Visible = False
            tb.Text = 0
            hfId.Value = ""
        End Sub


        Protected Sub lbPrevPeriod_Click(sender As Object, e As EventArgs) Handles lbPrevPeriod.Click
            If ddlPeriods.SelectedIndex > 0 Then
                ddlPeriods.SelectedIndex = ddlPeriods.SelectedIndex - 1
                ddlPeriods_SelectedIndexChanged(Me, Nothing)
            End If


        End Sub

        Protected Sub lbNextPeriod_Click(sender As Object, e As EventArgs) Handles lbNextPeriod.Click
            If ddlPeriods.SelectedIndex < ddlPeriods.Items.Count() - 1 Then
                ddlPeriods.SelectedIndex = ddlPeriods.SelectedIndex + 1
                ddlPeriods_SelectedIndexChanged(Me, Nothing)
            End If
        End Sub
        Protected Sub lbPrevPeriodD_Click(sender As Object, e As EventArgs) Handles lbPrevPeriodD.Click
            If ddlPeriodsD.SelectedIndex > 0 Then
                ddlPeriodsD.SelectedIndex = ddlPeriodsD.SelectedIndex - 1
                ddlPeriodsD_SelectedIndexChanged(Me, Nothing)
            End If


        End Sub

        Protected Sub lbNextPeriodD_Click(sender As Object, e As EventArgs) Handles lbNextPeriodD.Click
            If ddlPeriodsD.SelectedIndex < ddlPeriodsD.Items.Count() - 1 Then
                ddlPeriodsD.SelectedIndex = ddlPeriodsD.SelectedIndex + 1
                ddlPeriodsD_SelectedIndexChanged(Me, Nothing)
            End If
        End Sub

        Protected Sub ddlPeriodsD_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlPeriodsD.SelectedIndexChanged
            If gmaServers.Count = 0 Then
                gmaServers = Session("gmaServers")
            End If

            ' Dim myNodeId As Integer = hfNodeId.Value
            Dim myURL As String = hfURL.Value
            Dim gmaServer = (From c In gmaServers Where c.URL = myURL).First
            Dim dr = From c In gmaServer.gma.GetDirectorReport(CInt(ddlPeriodsD.SelectedValue)) Select c.measurementName, c.measurementValue, c.measurementDescription, c.measurementType, c.measurementId



            If ReportData.Count = 0 And dr.Count > 0 Then


                Dim numericMeasurements = dr.Where(Function(c) c.measurementType = "numeric").Select(Function(c) CStr(c.measurementId))

                Dim calculatedMeasurements = dr.Where(Function(c) c.measurementType = "calculated").Select(Function(c) CStr(c.measurementId))



                Dim xml As String = gmaServer.gma.getReportData(CInt(hfNodeId.Value), numericMeasurements.ToArray, calculatedMeasurements.ToArray)
                'Label1.Text = xml

                xml = xml.Replace("<?xml version=""1.0"" encoding=""UTF-8""?><?mso-application progid=""Excel.Sheet""?>", "")
                Dim doc As New System.Xml.XmlDocument
                doc.LoadXml(xml)

                Dim Report = doc.FirstChild.ChildNodes(2).FirstChild



                For Each row As System.Xml.XmlNode In Report.ChildNodes
                    If row.ChildNodes.Count > 1 Then
                        Dim Name = row.FirstChild.FirstChild.InnerText

                        If Not ReportData.ContainsKey(Name) Then
                            Dim Values As New List(Of Integer)

                            For Each cell As System.Xml.XmlNode In row.ChildNodes

                                If cell.HasChildNodes Then
                                    Dim value As Integer
                                    If cell.FirstChild.InnerText = Name Then

                                    ElseIf cell.FirstChild.InnerText = "" Or cell.FirstChild.InnerText = "-" Then
                                        '   Values.Add(0)
                                    ElseIf Integer.TryParse(cell.FirstChild.InnerText, value) Then
                                        Values.Add(value)
                                    End If
                                End If

                            Next
                            ReportData.Add(Name, Values.ToArray)
                        End If
                        '  Label1.Text &= Name & Values.Count & vbNewLine

                    End If
                Next
                Dim myNode = gmaServer.nodes.Where(Function(c) c.nodeId = CInt(hfNodeId.Value)).First
                myNode.AdvancedReportData = ReportData
                Session("gmaServers") = gmaServers

            End If


            rpDirectorMeasuremts.DataSource = dr
            rpDirectorMeasuremts.DataBind()






            lbPrevPeriodD.Enabled = Not (ddlPeriodsD.SelectedIndex = 0 Or ddlPeriodsD.Items.Count = 0)

            lbNextPeriodD.Enabled = Not (ddlPeriodsD.SelectedIndex = ddlPeriodsD.Items.Count - 1 Or ddlPeriodsD.Items.Count = 0)


        End Sub

        Protected Sub AddMeasurement(ByRef SaveReport As gma_Report, ByVal measurementType As String, ByVal measurementId As Integer, ByVal Answer As String)
            Dim m As New gma_measurements

            m.measurementType = measurementType
            m.measurementId = measurementId

            m.measurementValue = Answer
           
            SaveReport.measurements.Add(m)
        End Sub


        Protected Sub rpStaffMeasurements_ItemCommand(source As Object, e As DataListCommandEventArgs) Handles rpStaffMeasurements.ItemCommand
            If e.CommandName = "SaveReport" Then
                Dim SaveReport As New gma_Report
                SaveReport.measurements = New List(Of gma_measurements)
                SaveReport.nodeId = hfNodeId.Value
                SaveReport.ReportId = ddlPeriods.SelectedValue
                For Each row As DataListItem In rpStaffMeasurements.Items
                    Dim answer As String = ""
                    Dim type As String = CType(row.FindControl("hfAnswerType"), HiddenField).Value
                    If type = "numeric" Then
                        answer = CType(row.FindControl("tbAnswer"), TextBox).Text

                    ElseIf type = "text" Then
                        answer = CType(row.FindControl("tbAnswerText"), TextBox).Text
                    End If
                    AddMeasurement(SaveReport, type, CType(row.FindControl("hfMeasurementId"), HiddenField).Value, answer)
                  

                Next
                If dvMass.Visible Then
                    AddMeasurement(SaveReport, "numeric", hfMass.Value, tbMass.Text)
                End If
                If dvExposures.Visible Then
                    AddMeasurement(SaveReport, "numeric", hfExposures.Value, tbExposures.Text)
                End If
                If dvPresGosp.Visible Then
                    AddMeasurement(SaveReport, "numeric", hfPresGosp.Value, tbPresGosp.Text)
                End If
                If dvFollowup.Visible Then
                    AddMeasurement(SaveReport, "numeric", hfFollowup.Value, tbFollowup.Text)
                End If
                If dvHSPres.Visible Then
                    AddMeasurement(SaveReport, "numeric", hfHSPres.Value, tbHSPres.Text)
                End If
                If dvTraining.Visible Then
                    AddMeasurement(SaveReport, "numeric", hfTraining.Value, tbTraining.Text)
                End If
                If dvSendLifeLab.Visible Then
                    AddMeasurement(SaveReport, "numeric", hfSendLifeLab.Value, tbSendLifeLab.Text)
                End If
                If dvDevLocRes.Visible Then
                    AddMeasurement(SaveReport, "numeric", hfDevLocRes.Value, tbDevLocRes.Text)
                End If
                If dvNewBel.Visible Then
                    AddMeasurement(SaveReport, "numeric", hfNewBel.Value, tbNewBel.Text)
                End If
                If dvEngagedDisc.Visible Then
                    AddMeasurement(SaveReport, "numeric", hfEngagedDisc.Value, tbEngagedDisc.Text)
                End If
                If dvMultDisc.Visible Then
                    AddMeasurement(SaveReport, "numeric", hfMultDisc.Value, tbMultDisc.Text)
                End If
                If dvLocGenRes.Visible Then
                    AddMeasurement(SaveReport, "numeric", hfLocGenRes.Value, tbLocGenRes.Text)
                End If
                If dvmovement.Visible Then
                    AddMeasurement(SaveReport, "numeric", hfmovement.Value, tbmovement.Text)
                End If


                If gmaServers.Count = 0 Then
                    gmaServers = Session("gmaServers")
                End If
                Dim myURL As String = hfURL.Value
                Dim gmaServer = (From c In gmaServers Where c.URL = myURL).First
                gmaServer.gma.SaveReport(SaveReport)
            End If
        End Sub

        Protected Sub rpDirectorMeasuremts_ItemCommand(source As Object, e As DataListCommandEventArgs) Handles rpDirectorMeasuremts.ItemCommand
            If e.CommandName = "SaveReport" Then
                Dim SaveReport As New gma_Report
                SaveReport.measurements = New List(Of gma_measurements)
                SaveReport.nodeId = hfNodeId.Value
                SaveReport.ReportId = ddlPeriodsD.SelectedValue
                For Each row As DataListItem In rpDirectorMeasuremts.Items
                    Dim m As New gma_measurements

                    m.measurementType = CType(row.FindControl("hfAnswerType"), HiddenField).Value
                    m.measurementId = CType(row.FindControl("hfMeasurementId"), HiddenField).Value


                    If m.measurementType = "numeric" Then
                        m.measurementValue = CType(row.FindControl("tbAnswer"), TextBox).Text

                    ElseIf m.measurementType = "text" Then
                        m.measurementValue = CType(row.FindControl("tbAnswerText"), TextBox).Text
                    End If
                    SaveReport.measurements.Add(m)

                Next

                If gmaServers.Count = 0 Then
                    gmaServers = Session("gmaServers")
                End If
                Dim myURL As String = hfURL.Value
                Dim gmaServer = (From c In gmaServers Where c.URL = myURL).First
                gmaServer.gma.SaveReport(SaveReport, "Director")
            End If
        End Sub



        Protected Function getTagFromName(ByVal Name As String) As String
            If Name.Contains("mass exposures") Or Name.Contains("mass") Then
                Return "Mass"
            ElseIf Name.Contains("personal exposures") Or Name.Contains("personal") Then
                Return "Exposures"
            ElseIf Name.Contains("presenting the gospel") Or Name.Contains("gospel") Then
                Return "PresGosp"
            ElseIf Name.Contains("following up") Or Name.Contains("followed up") Then
                Return "Followup"
            ElseIf Name.Contains("holy spirit presentations") Or Name.Contains("holy spirit") Then
                Return "HSPres"
            ElseIf Name.Contains("training for action") Or Name.Contains("training") Then
                Return "Training"
            ElseIf Name.Contains("sending lifetime laborors") Or Name.Contains("sending") Then
                Return "SendLifeLab"
            ElseIf Name.Contains("new believers") Or Name.Contains("new") Then
                Return "NewBel"
            ElseIf Name.Contains("engaged disciples") Or Name.Contains("engaged") Then
                Return "EngagedDisc"
            ElseIf Name.Contains("movement") Or Name.Contains("communities") Then
                Return "Movement"
            ElseIf Name.Contains("multiplying disciples") Or Name.Contains("multiplying") Then
                Return "MultDisc"
            ElseIf Name.Contains("developing local resources") Or Name.Contains("developing") Then
                Return "DevLocRes"
            ElseIf Name.Contains("locally generated resources") Or Name.Contains("generated") Then
                Return "LocGenRes"
            
            End If
            Return ""
           
        End Function


        Public Function GetReportString() As String
            Dim rtn As String = ""
            Dim j As Integer = 0
            For Each row In ReportData
                Dim tag = getTagFromName(row.Key.ToLower)
                rtn &= "//" & row.Key & vbNewLine
                If (tag <> "" And row.Value.Count > 0) Then
                    Dim high As Integer = row.Value.First
                    Dim low As Integer = row.Value.First

                    rtn &= "var d" & j & " = new google.visualization.DataTable();d" & j & ".addColumn('string', 'period');d" & j & ".addColumn('number', 'value');" & vbNewLine
                    Dim i As Integer = 0
                    For Each d In row.Value
                        low = Math.Min(low, d)
                        high = Math.Max(high, d)

                        rtn &= "d" & j & ".addRow(['" & i & "'," & d & "]);" & vbNewLine
                        i += 1
                    Next
                    rtn &= "var chart = new google.visualization.LineChart(document.getElementById('g" & tag & "'));chart.draw(d" & j & ", options);" & vbNewLine

                    rtn &= "$('#h" & tag & "').text('High: " & high & "');" & vbNewLine
                    rtn &= "$('#l" & tag & "').text('Low: " & low & "');" & vbNewLine


                    j += 1
                End If

            Next

            Return rtn




        End Function


    End Class
End Namespace
