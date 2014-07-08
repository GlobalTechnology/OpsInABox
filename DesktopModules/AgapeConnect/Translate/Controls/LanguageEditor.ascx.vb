Imports DotNetNuke
Imports DotNetNuke.Security
Imports StaffBroker
Imports System.Xml
Imports System.IO


Public Class Translation

    Private _name As String
    Public Property Name() As String
        Get
            Return _name
        End Get
        Set(ByVal value As String)
            _name = value
        End Set
    End Property

    Private _foreignName As String
    Public Property ForeignName() As String
        Get
            Return _foreignName
        End Get
        Set(ByVal value As String)
            _foreignName = value
        End Set
    End Property


    Private _english As String
    Public Property English() As String
        Get
            Return _English
        End Get
        Set(ByVal value As String)
            _English = value
        End Set
    End Property

    Private _foreign As String
    Public Property Foreign() As String
        Get
            Return _foreign
        End Get
        Set(ByVal value As String)
            _foreign = value
        End Set
    End Property

    Private _comment As String
    Public Property Comment() As String
        Get
            Return _comment
        End Get
        Set(ByVal value As String)
            _comment = value
        End Set
    End Property

    Private _portalVarient As Boolean
    Public Property PortalVarient() As Boolean
        Get
            Return _portalVarient
        End Get
        Set(ByVal value As Boolean)
            _portalVarient = value
        End Set
    End Property
   




    Public Sub New(ByVal inName As String, ByVal inEnglish As String, ByVal inForeign As String, ByVal inComment As String, ByVal inForeignName As String, ByVal inPortalVarient As Boolean)
        Name = inName
        English = inEnglish
        Foreign = inForeign
        Comment = inComment
        ForeignName = inForeignName
        PortalVarient = inPortalVarient
    End Sub
End Class


Partial Class DesktopModules_AgapeConnect_Translate_LanuguageEditor
    Inherits System.Web.UI.UserControl

    Private _origResx As String = ""
    Public Property OrigResx() As String
        Get
            Return _origResx
        End Get
        Set(ByVal value As String)
            _origResx = value
        End Set
    End Property

    Private _translateResx As String = ""
    Public Property TranslateResx() As String
        Get
            Return _translateResx
        End Get
        Set(ByVal value As String)
            _translateResx = value
        End Set
    End Property

    Private _langauge As String
    Public Property Language() As String
        Get
            Return _langauge
        End Get
        Set(ByVal value As String)
            _langauge = value
        End Set
    End Property


    Private _ps As PortalSettings
    Public Property PS() As PortalSettings
        Get
            Return _ps
        End Get
        Set(ByVal value As PortalSettings)
            _ps = value
        End Set
    End Property



    Dim tDataNodes As XmlNodeList

    Dim tpDataNodes As XmlNodeList


    

    Public Sub LoadEditor()

        hfTranslateResx.Value = TranslateResx
        Dim t As ArrayList
        Dim resx As New FileInfo(OrigResx)



        t = New ArrayList()
        Dim doc As New XmlDocument()
        doc.Load(OrigResx)
        Dim tpDoc As New XmlDocument()

        If File.Exists(TranslateResx) Then
            tpDoc.Load(TranslateResx)
            tpDataNodes = tpDoc.SelectNodes("root/data")
        End If
        Dim tDoc As New XmlDocument()
        Dim LangResx As String = TranslateResx.Replace("Portal-" & PS.PortalId & ".", "")
        If File.Exists(LangResx) Then
            tDoc.Load(LangResx)
            tDataNodes = tDoc.SelectNodes("root/data")
        End If

        For Each row As XmlNode In (From c As XmlNode In doc.SelectNodes("root/data") Order By c.Attributes(0).Value)
            If row.NodeType <> XmlNodeType.Comment Then
                Dim name As String = resx.Name & ": <b>" & row.Attributes(0).Value & "</b>"
                Dim foreignName As String = TranslateResx & "::" & row.Attributes(0).Value

                Dim value = row.SelectSingleNode("value")
                Dim valueString As String = ""
                If Not value Is Nothing Then
                    valueString = value.InnerText
                End If
                Dim comment = row.SelectSingleNode("comment")
                Dim commentString As String = ""
                If Not comment Is Nothing Then
                    commentString = comment.InnerText
                End If
                Dim foreignString As String = GetLocalizedString(row.Attributes(0).Value, "")
                'Dim foreignString As String = DotNetNuke.Services.Localization.Localization.GetString(row.Attributes(0).Value, OrigResx, PS, Language)
                ' Dim foreignString As String = DotNetNuke.Services.Localization.Localization.GetString(row.Attributes(0).Value, OrigResx, Language)


                
                t.Add(New Translation(name, valueString, foreignString, commentString,foreignName, false ))

            End If
        Next

        dlEditor.DataSource = t
        dlEditor.DataBind()


    End Sub


    Private Function GetLocalizedString(ByVal Key As String, ByVal Fallback As String) As String

        Try

        
        If Not tpDataNodes Is Nothing Then
            Dim foreignNode = (From c As XmlNode In tpDataNodes Where c.Attributes(0).Value = Key Select c.SelectSingleNode("value").InnerText)
            If foreignNode.Count > 0 Then
                Return foreignNode.First()
            End If
        End If
        If Not tDataNodes Is Nothing Then
            Dim foreignNode = (From c As XmlNode In tDataNodes Where c.Attributes(0).Value = Key Select c.SelectSingleNode("value").InnerText)
            If foreignNode.Count > 0 Then
                Return foreignNode.First()
            End If
        End If
        Catch ex As Exception
            Return Fallback
        End Try
        Return Fallback
    End Function
    Private Sub CreateNewDataNode(ByRef tdoc As XmlDocument, ByVal KeyName As String)
        Dim rootNode = tdoc.SelectSingleNode("root")
        Dim newNode As XmlElement = tdoc.CreateElement("data")
        Dim nameAtt As XmlAttribute = tdoc.CreateAttribute("name")
        nameAtt.Value = KeyName
        newNode.Attributes.Append(nameAtt)
        rootNode.AppendChild(newNode)

    End Sub
  

    

End Class
