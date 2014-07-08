Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Linq

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class PGTCallBack
     Inherits System.Web.Services.WebService
    Private theUsername As String = "CASAUTH"
    Private thePassword As String = "thecatsaysmeow3"
   
    <WebMethod()> _
    Public Function RetrievePGTCallback(ByVal Username As String, ByVal Password As String, ByVal PGTIOU As String) As String
        If (UserName <> theUsername Or Password <> thePassword) Then
            Return "AUTHERROR"

        End If
        Dim d As New GCX.GCXDataContext

        Dim q = From c In d.Agape_GCX_Proxies Where c.PGTIOU = PGTIOU Select c.PGTID

        If q.Count > 0 Then
            Return q.First
        End If


       


        Return "ERROR"



    End Function
End Class