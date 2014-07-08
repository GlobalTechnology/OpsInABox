Imports System.ServiceModel
Imports System.ServiceModel.Web
Imports System.ServiceModel.Activation
Imports System.Net
Imports System.Text.RegularExpressions
' NOTE: You can use the "Rename" command on the context menu to change the class name "MobileCAS" in code, svc and config file together.
<AspNetCompatibilityRequirements(RequirementsMode:=AspNetCompatibilityRequirementsMode.Allowed)>
Public Class MobileCAS
    Implements IMobileCAS

    Function AuthenticateWithTheKey(ByVal username As String, ByVal password As String, ByVal targetService As String) As CASforMobile.KeyUserDetails Implements IMobileCAS.AuthenticateWithTheKey
        Dim rtn As New CASforMobile.KeyUserDetails()
        rtn.LoginSuccess = False

        Dim objKey As New KeyUser.KeyAuthentication(username, password, targetService, "https://thekey.me/cas/")
        If Not (String.IsNullOrEmpty(objKey.KeyGuid)) Then
            rtn.LoginSuccess = True
            rtn.GUID = objKey.KeyGuid
            rtn.ProxyTicket = objKey.ProxyTicket


        End If

        Return rtn
    End Function

    Function AuthenticateWithSpecifiedCAS(ByVal username As String, ByVal password As String, ByVal targetService As String, ByVal casServer As String) As CASforMobile.KeyUserDetails Implements IMobileCAS.AuthenticateWithSpecifiedCAS
        Dim rtn As New CASforMobile.KeyUserDetails()
        rtn.LoginSuccess = False

        Dim objKey As New KeyUser.KeyAuthentication(username, password, targetService, casServer)
        If Not (String.IsNullOrEmpty(objKey.KeyGuid)) Then
            rtn.LoginSuccess = True
            rtn.GUID = objKey.KeyGuid
            rtn.ProxyTicket = objKey.ProxyTicket
            rtn.ProxyTicket = objKey.ServiceTicket
        End If

        Return rtn
    End Function

    Function GetAccountBalance(ByVal CountryURL As String, ByVal GUID As String, ByVal PGTIOU As String, ByVal Account As String) As Double Implements IMobileCAS.GetAccountBalance
   
        Return DSAccount.getAccountBalance(GUID, PGTIOU, CountryURL, Account)


    End Function
    Function ConvertCurrency(ByVal FromCur As String, ByVal ToCur As String) As Double Implements IMobileCAS.ConvertCurrency
        Dim web As New WebClient()
        If FromCur Is Nothing Or ToCur Is Nothing Then
            Return 1
        End If
        Try

            Dim provider As String = "Yahoo"
            If provider = "Google" Then




                Dim url As String = "http://www.google.com/ig/calculator?hl=en&q=1" & FromCur.ToUpper & "%3D%3F" & ToCur.ToUpper

                Dim response = web.DownloadString(url)
                Dim split = response.Split(",")
                Dim temp = split(1).Replace("rhs: """, "")

                Dim culture As New CultureInfo("en-US")

                Dim s As Double = Double.Parse(temp.Substring(0, temp.IndexOf(" ")), culture.NumberFormat)

                Return s
            Else
                Dim url As String = "http://download.finance.yahoo.com/d/quotes.csv?s=" & FromCur.ToUpper & ToCur.ToUpper & "=X&f=l1"



                Dim response As String = web.DownloadString(url)
                Dim rate As Double = Double.Parse(response, New CultureInfo(""))

                Return rate
            End If





        Catch ex As Exception
            Return 1.0
        End Try


    End Function

End Class
