Imports System
Imports System.Collections
Imports System.Configuration
Imports System.Data
Imports System.Linq

Imports DotNetNuke
Imports DotNetNuke.Security
Imports StaffBroker
Imports StaffBrokerFunctions


Namespace DotNetNuke.Modules.AgapeConnect
    Partial Class Europe
        Inherits Entities.Modules.PortalModuleBase

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            saveCountryData()

        End Sub

        Private Function GetCountryValue(ByVal countryCode As String, ByVal valueType As String) As String
            Dim x = DotNetNuke.Services.Localization.Localization.GetString(countryCode & "." & valueType, LocalResourceFile)
            If Not x Is Nothing Then
                x = x.Replace(vbNewLine, "")
            End If

            Return x
        End Function

        Protected Sub saveCountryData()
            Dim Countries() As String = {"AL", "AL", "AM", "AT", "AZ", "BA", "BE", "BG", "BY", "CH", "CY", "CZ", "DE", "DK", "DZ", "EE", "ES", "FI", "FR", "GB", "GE", "GL", "GR", "HR", "HU", "IE", "IL", "IQ", "IR", "IS", "IT", "JO", "KZ", "LB", "LI", "LT", "LU", "LV", "MA", "MC", "MD", "ME", "MK", "MT", "NL", "NO", "PL", "PT", "RO", "RU", "SA", "SE", "SI", "SK", "SM", "SR", "SY", "TM", "TN", "TR", "UA", "EEU"}


            Dim data As String = "["
            For Each row In Countries
                data &= "{""Code"": """ & row & """,""Name"": """ & GetCountryValue(row, "Name") & """, ""URL"": """ & HttpUtility.JavaScriptStringEncode(GetCountryValue(row, "URL")) & """, ""Text"": """ & HttpUtility.JavaScriptStringEncode(GetCountryValue(row, "Text")) & """}," & Environment.NewLine

            Next
            data = data.Substring(0, data.LastIndexOf(","))

            hfCountryData.Value = data & "]"
        End Sub
        


    End Class
End Namespace
