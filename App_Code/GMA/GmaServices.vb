Imports Microsoft.VisualBasic
Imports System
Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Xml





Public Class GmaServices
    Public Structure gma_node
        Public nodeId As Integer
        Public shortName As String
        Public Reports As List(Of gma_Report)
        Public DirectorReports As List(Of gma_Report)
        Public AdvancedReportData As Dictionary(Of String, Integer())
    End Structure

    Public Structure gmaServer
        Public name As String
        Public URL As String
        Public gma As GmaServices
        Public nodes As List(Of gma_node)
    End Structure

    Public Structure gma_Report
        Public ReportId As Integer
        Public startDate As Date
        Public endDate As Date
        Public nodeId As Integer
        Public reportType As String
        Public submitted As Boolean

        Public measurements As List(Of gma_measurements)
        Public StaffReports As List(Of gma_Report)
        Public nodeReports As List(Of gma_Report)

    End Structure




    Public Structure gma_measurements
        Public measurementId As Integer
        Public measurementValue As String
        Public measurementType As String
        Public measurementName As String
        Public measurementDescription As String
        Public viewOrder
        Public mcc As String
        Public renId As Integer
        Public self As Boolean



    End Structure


    Private _endPoint As String
    Private CASHOST As String = "https://thekey.me/cas/"
    Private myCookieContainer As New CookieContainer()
    Public Property EndPoint() As String
        Get
            Return _endPoint
        End Get
        Set(ByVal value As String)
            _endPoint = value
        End Set
    End Property



    Public Sub New(ByVal ServiceURL As String, ByVal PGTIOU As String)
        _endPoint = ServiceURL
        Login(PGTIOU)
    End Sub
    Public Function Login(ByVal PGTIOU As String) As String

        Dim newURL As New Uri("https://agapeconnect.me/MobileCAS/MobileCAS.svc/AuthenticateWithTheKey?username=jon@vellacott.co.uk&password=Iowa2001&targetService=" & HttpContext.Current.Server.UrlEncode("http%3A%2F%2Fgma.agapeconnect.me%2F%3Fq%3Dgmaservices%26destination%3Dgmaservices"))



        Dim PGT = New theKeyProxyTicket.PGTCallBack().RetrievePGTCallback("CASAUTH", "thecatsaysmeow3", PGTIOU)

        Dim service = GetTargetService()
        Dim proxyurl As New Uri(CASHOST & "proxy?" & "targetService=" & service & "&pgt=" & PGT)

        Dim rdr As New StreamReader(New WebClient().OpenRead(proxyurl))
        Dim doc As XmlDocument = New XmlDocument
        doc.Load(rdr)
        Dim NamespaceMgr As XmlNamespaceManager = New XmlNamespaceManager(doc.NameTable)
        NamespaceMgr.AddNamespace("cas", "http://www.yale.edu/tp/cas")


        Dim successNode As XmlNode = doc.SelectSingleNode("/cas:serviceResponse/cas:proxySuccess", NamespaceMgr)

        Dim proxyTicket As XmlNode = successNode.SelectSingleNode("./cas:proxyTicket", NamespaceMgr)

        '        Return proxyTicket.InnerText & "proxyURL: " & proxyurl.AbsoluteUri


        Dim PT = proxyTicket.InnerText
        'Dim PT = TestLogin()


        Dim method = "?q=gmaservices&ticket=" & PT


        Dim authURL = _endPoint & method



        Dim request As HttpWebRequest = DirectCast(WebRequest.Create(authURL), HttpWebRequest)
        request.CookieContainer = myCookieContainer

        Dim response As HttpWebResponse = DirectCast(request.GetResponse(), HttpWebResponse)
        Dim reader As New StreamReader(response.GetResponseStream())
        Dim json = reader.ReadToEnd()
        Return json
    End Function

    

    Private Function GetTargetService() As String
        Dim method = "?q=gmaservices"

        Dim request As HttpWebRequest = DirectCast(WebRequest.Create(_endPoint & method), HttpWebRequest)

        request.AllowAutoRedirect = False

        Dim response As HttpWebResponse = DirectCast(request.GetResponse(), HttpWebResponse)
        If response.StatusCode = HttpStatusCode.Redirect Then
            Dim redr = response.Headers("Location")
            Return redr.Substring(redr.IndexOf("service=") + 8)


        End If
        Return ""
    End Function



    Public Function GetUserNodes(ByVal Reports As List(Of gma_Report), ByVal DirectorReports As List(Of gma_Report)) As List(Of gma_node)


        Dim method = "?q=gmaservices/gma_node"




        Dim Nodes As New List(Of gma_node)
        Dim request As HttpWebRequest = DirectCast(WebRequest.Create(_endPoint & method), HttpWebRequest)
        request.CookieContainer = myCookieContainer

        Dim response As HttpWebResponse = DirectCast(request.GetResponse(), HttpWebResponse)
        Dim reader As New StreamReader(response.GetResponseStream())
        Dim json = reader.ReadToEnd()

        Dim jss = New System.Web.Script.Serialization.JavaScriptSerializer()
        Dim dict = jss.Deserialize(Of Object)(json)

        If dict("success") = "true" Then

            For Each row In dict("data")("nodeList")
                Dim insert As New gma_node
                insert.nodeId = row("nodeId")
                insert.shortName = row("shortName")
                insert.Reports = Reports.Where(Function(c) c.nodeId = insert.nodeId).ToList
                insert.DirectorReports = DirectorReports.Where(Function(c) c.nodeId = insert.nodeId).ToList
                If insert.Reports.Count + insert.DirectorReports.Count > 0 Then
                    Nodes.Add(insert)
                End If

            Next
            
        End If

        Return Nodes
    End Function

    Public Function GetStaffReports() As List(Of gma_Report)


        Dim method = "?q=gmaservices/gma_staffReport/searchOwn"





        Dim request As HttpWebRequest = DirectCast(WebRequest.Create(_endPoint & method), HttpWebRequest)
        request.CookieContainer = myCookieContainer
        request.Method = "POST"

        Dim post As String = "{""maxResult"":0,""orderBy"":""startDate""}"
        Dim bytes As Byte() = Encoding.UTF8.GetBytes(post)
        request.ContentLength = bytes.Length
        request.ContentType = "application/json"
        Dim requestStream = request.GetRequestStream()
        requestStream.Write(bytes, 0, bytes.Length)



        Dim response As HttpWebResponse = DirectCast(request.GetResponse(), HttpWebResponse)

        Dim reader As New StreamReader(response.GetResponseStream())
        Dim json = reader.ReadToEnd()

        Dim jss = New System.Web.Script.Serialization.JavaScriptSerializer()
        Dim dict = jss.Deserialize(Of Object)(json)
        Dim Reports As New List(Of gma_Report)
        If dict("success") = "true" Then

            For Each row In dict("data")("staffReports")
                Dim insert As New gma_Report
                insert.ReportId = row("staffReportId")
                insert.nodeId = row("node")("nodeId")
                insert.submitted = row("submitted")
                Dim sd As String = row("startDate")
                Dim ed As String = row("endDate")
                insert.startDate = New Date(CInt(Left(sd, 4)), CInt(sd.Substring(4, 2)), CInt(Right(sd, 2)))
                insert.endDate = New Date(CInt(Left(ed, 4)), CInt(ed.Substring(4, 2)), CInt(Right(ed, 2)))
                insert.reportType = "Staff"

                Reports.Add(insert)

            Next

        End If
        Return Reports

    End Function

    Public Function GetDirectorReports() As List(Of gma_Report)


        Dim method = "?q=gmaservices/gma_directorReport/searchOwn"





        Dim request As HttpWebRequest = DirectCast(WebRequest.Create(_endPoint & method), HttpWebRequest)
        request.CookieContainer = myCookieContainer
        request.Method = "POST"

        Dim post As String = "{""maxResult"":0,""orderBy"":""startDate""}"
        Dim bytes As Byte() = Encoding.UTF8.GetBytes(post)
        request.ContentLength = bytes.Length
        request.ContentType = "application/json"
        Dim requestStream = request.GetRequestStream()
        requestStream.Write(bytes, 0, bytes.Length)



        Dim response As HttpWebResponse = DirectCast(request.GetResponse(), HttpWebResponse)

        Dim reader As New StreamReader(response.GetResponseStream())
        Dim json = reader.ReadToEnd()

        Dim jss = New System.Web.Script.Serialization.JavaScriptSerializer()
        Dim dict = jss.Deserialize(Of Object)(json)
        Dim Reports As New List(Of gma_Report)
        If dict("success") = "true" Then

            For Each row In dict("data")("directorReports")
                Dim insert As New gma_Report
                insert.ReportId = row("directorReportId")
                insert.nodeId = row("node")("nodeId")
                insert.submitted = row("submitted")
                Dim sd As String = row("startDate")
                Dim ed As String = row("endDate")
                insert.startDate = New Date(CInt(Left(sd, 4)), CInt(sd.Substring(4, 2)), CInt(Right(sd, 2)))
                insert.endDate = New Date(CInt(Left(ed, 4)), CInt(ed.Substring(4, 2)), CInt(Right(ed, 2)))
                insert.reportType = "Director"

                Reports.Add(insert)

            Next

        End If
        Return Reports

    End Function


    Public Function GetStaffReport(ByVal StaffReportId As Integer) As List(Of gma_measurements)


        Dim method = "?q=gmaservices/gma_staffReport/" & StaffReportId





        Dim request As HttpWebRequest = DirectCast(WebRequest.Create(_endPoint & method), HttpWebRequest)
        request.CookieContainer = myCookieContainer

        Dim response As HttpWebResponse = DirectCast(request.GetResponse(), HttpWebResponse)
        Dim reader As New StreamReader(response.GetResponseStream())
        Dim json = reader.ReadToEnd()

        Dim jss = New System.Web.Script.Serialization.JavaScriptSerializer()
        Dim dict = jss.Deserialize(Of Object)(json)
        Dim Report As New List(Of gma_measurements)
        If dict("success") = "true" Then
            Dim out As New List(Of gma_measurements)
            Dim numeric = dict("data")("numericMeasurements")
            Dim calc = dict("data")("calculatedMeasurements")
            Dim text = dict("data")("textMeasurement")

            Dim viewOrder As Integer = 0

            If (Not numeric Is Nothing) Then
                For Each mccs As Dictionary(Of String, Object) In numeric
                    For Each mcc In mccs.Keys
                        For Each m In mccs(mcc)

                            Dim insert As New gma_measurements
                            insert.measurementId = m("measurementId")
                            insert.measurementValue = m("measurementValue")
                            insert.measurementType = "numeric"
                            insert.measurementName = m("measurementName")
                            insert.measurementDescription = m("measurementDescription")
                            insert.mcc = mcc
                            insert.viewOrder = viewOrder
                            insert.self = True

                            Report.Add(insert)
                            viewOrder += 1

                        Next
                    Next
                Next
            End If

            If (Not calc Is Nothing) Then
                For Each mccs As Dictionary(Of String, Object) In calc
                    For Each mcc In mccs.Keys
                        For Each m In mccs(mcc)

                            Dim insert As New gma_measurements
                            insert.measurementId = m("measurementId")
                            insert.measurementValue = m("measurementValue")
                            insert.measurementType = "calculated"
                            insert.measurementName = m("measurementName")
                            insert.measurementDescription = m("measurementDescription")
                            insert.mcc = mcc
                            insert.viewOrder = viewOrder
                            insert.self = True
                            Report.Add(insert)
                            viewOrder += 1

                        Next
                    Next
                Next
            End If

            If Not text Is Nothing Then


                For Each m In text

                    Dim insert As New gma_measurements
                    insert.measurementId = m("measurementId")
                    insert.measurementValue = m("measurementValue")
                    insert.measurementType = "text"
                    insert.measurementName = m("measurementName")
                    insert.measurementDescription = m("measurementDescription")

                    insert.viewOrder = viewOrder
                    insert.self = True
                    Report.Add(insert)
                    viewOrder += 1

                Next
            End If


        End If

        Return Report
    End Function


    Public Function GetDirectorReport(ByVal DirectorReportId As Integer) As List(Of gma_measurements)


        Dim method = "?q=gmaservices/gma_directorReport/" & DirectorReportId





        Dim request As HttpWebRequest = DirectCast(WebRequest.Create(_endPoint & method), HttpWebRequest)
        request.CookieContainer = myCookieContainer

        Dim response As HttpWebResponse = DirectCast(request.GetResponse(), HttpWebResponse)
        Dim reader As New StreamReader(response.GetResponseStream())
        Dim json = reader.ReadToEnd()

        Dim jss = New System.Web.Script.Serialization.JavaScriptSerializer()
        Dim dict = jss.Deserialize(Of Object)(json)
        Dim Report As New List(Of gma_measurements)
        If dict("success") = "true" Then
            Dim out As New List(Of gma_measurements)
            Dim numeric = dict("data")("numericMeasurements")
            Dim calc = dict("data")("calculatedMeasurements")
            Dim text = dict("data")("textMeasurement")

            Dim viewOrder As Integer = 0

            If (Not numeric Is Nothing) Then
                For Each mccs As Dictionary(Of String, Object) In numeric
                    For Each mcc In mccs.Keys
                        For Each m In mccs(mcc)

                            Dim insert As New gma_measurements
                            insert.measurementId = m("measurementId")
                            insert.measurementValue = m("measurementValue")
                            insert.measurementType = "numeric"
                            insert.measurementName = m("measurementName")
                            insert.measurementDescription = m("measurementDescription")
                            insert.mcc = mcc
                            insert.viewOrder = viewOrder
                            insert.self = True

                            Report.Add(insert)
                            viewOrder += 1

                        Next
                    Next
                Next
            End If

            If (Not calc Is Nothing) Then
                For Each mccs As Dictionary(Of String, Object) In calc
                    For Each mcc In mccs.Keys
                        For Each m In mccs(mcc)

                            Dim insert As New gma_measurements
                            insert.measurementId = m("measurementId")
                            insert.measurementValue = m("measurementValue")
                            insert.measurementType = "calculated"
                            insert.measurementName = m("measurementName")
                            insert.measurementDescription = m("measurementDescription")
                            insert.mcc = mcc
                            insert.viewOrder = viewOrder
                            insert.self = True
                            Report.Add(insert)
                            viewOrder += 1

                        Next
                    Next
                Next
            End If

            If Not text Is Nothing Then


                For Each m In text

                    Dim insert As New gma_measurements
                    insert.measurementId = m("measurementId")
                    insert.measurementValue = m("measurementValue")
                    insert.measurementType = "text"
                    insert.measurementName = m("measurementName")
                    insert.measurementDescription = m("measurementDescription")

                    insert.viewOrder = viewOrder
                    insert.self = True
                    Report.Add(insert)
                    viewOrder += 1

                Next
            End If


        End If

        Return Report
    End Function


    Public Function SaveReport(ByVal Report As gma_Report, Optional ByVal Type As String = "Staff") As Boolean


        Dim method = "?q=gmaservices/gma_" & IIf(Type = "Staff", "staff", "director") & "Report/" & Report.ReportId





        Dim request As HttpWebRequest = DirectCast(WebRequest.Create(_endPoint & method), HttpWebRequest)
        request.CookieContainer = myCookieContainer
        request.Method = "PUT"

        Dim post As String = "["

        For Each m In Report.measurements.Where(Function(c) c.measurementType <> "calculated")
            Dim value As String = IIf(m.measurementType = "text", """" & m.measurementValue & """", m.measurementValue)

            post &= "{""measurementId"":" & m.measurementId & ",""type"":""" & m.measurementType & """,""value"":" & value & "},"
        Next

        post = post.TrimEnd(",") & "]"


        Dim bytes As Byte() = Encoding.UTF8.GetBytes(post)
        request.ContentLength = bytes.Length
        request.ContentType = "application/json"
        Dim requestStream = request.GetRequestStream()
        requestStream.Write(bytes, 0, bytes.Length)



        Dim response As HttpWebResponse = DirectCast(request.GetResponse(), HttpWebResponse)

        Dim reader As New StreamReader(response.GetResponseStream())
        Dim json = reader.ReadToEnd()

        Dim jss = New System.Web.Script.Serialization.JavaScriptSerializer()
        Dim dict = jss.Deserialize(Of Object)(json)
        Return dict("success")

    End Function


    Public Function getReportData(ByVal NodeId As Integer, ByVal numerics As String(), ByVal calculateds As String()) As String
        Dim method = "?q=gmaservices/gma_advancedReport/" & NodeId & "/generate"
        Dim nString As String = ""
        For Each item In numerics
            nString &= """" & item & ""","
        Next
        nString = nString.TrimEnd(",")
        Dim cString As String = ""
        For Each item In calculateds
            cString &= """" & item & ""","
        Next
        cString = cString.TrimEnd(",")


        Dim post As String = "{ ""dateRange"": {""relative"": ""3""}, ""reportFormat"": { ""byReportingInterval"": {""granularity"":""2"", ""showTotalColumn"": false} }, ""organizationSelection"": [""" & NodeId & """], ""strategySelection"": [""1""], ""measurementSelection"": { ""numericList"": [" & nString & "], ""calculatedList"":[" & cString & "] } }"
        ' Return post
        Dim request As HttpWebRequest = DirectCast(WebRequest.Create(_endPoint & method), HttpWebRequest)
        request.CookieContainer = myCookieContainer
        request.Method = "POST"



        Dim bytes As Byte() = Encoding.UTF8.GetBytes(post)
        request.ContentLength = bytes.Length
        request.ContentType = "application/json"
        Dim requestStream = request.GetRequestStream()
        requestStream.Write(bytes, 0, bytes.Length)



        Dim response As HttpWebResponse = DirectCast(request.GetResponse(), HttpWebResponse)

        Dim reader As New StreamReader(response.GetResponseStream())
        Dim json = reader.ReadToEnd()

        Dim jss = New System.Web.Script.Serialization.JavaScriptSerializer()
        Dim dict = jss.Deserialize(Of Object)(json)
        Dim xml = dict("data")
        Return xml
        Dim doc As New System.Xml.XmlDocument
        doc.LoadXml(xml)

        Dim list = doc.FirstChild.OuterXml
        Return list




    End Function





    Public Function GetReportGraph(ByVal NodeId As Integer, ByVal MeasurementId As Integer) As String
        Dim method = "?q=gmaservices/gma_advancedReport/" & NodeId & "/graph"


        Dim post As String = "{ ""dateRange"": {""relative"": ""3""}, ""reportFormat"": { ""byReportingInterval"": {""granularity"":""2"", ""showTotalColumn"": false} }, ""organizationSelection"": [""21""], ""strategySelection"": [""1""], ""measurementSelection"": { ""numericList"": [""497""], ""calculatedList"":[] } }"
        Dim request As HttpWebRequest = DirectCast(WebRequest.Create(_endPoint & method), HttpWebRequest)
        request.CookieContainer = myCookieContainer
        request.Method = "POST"



        Dim bytes As Byte() = Encoding.UTF8.GetBytes(post)
        request.ContentLength = bytes.Length
        request.ContentType = "application/json"
        Dim requestStream = request.GetRequestStream()
        requestStream.Write(bytes, 0, bytes.Length)



        Dim response As HttpWebResponse = DirectCast(request.GetResponse(), HttpWebResponse)

        Dim reader As New StreamReader(response.GetResponseStream())
        Dim json = reader.ReadToEnd()

        Dim jss = New System.Web.Script.Serialization.JavaScriptSerializer()
        Dim dict = jss.Deserialize(Of Object)(json)


        Return dict("data")


    End Function


End Class
