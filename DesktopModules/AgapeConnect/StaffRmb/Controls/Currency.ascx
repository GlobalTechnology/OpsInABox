<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Currency.ascx.vb" Inherits="DesktopModules_AgapeConnect_StaffRmb_Controls_Currency" %>
<table>
    <tr>
        <td>
        <asp:HyperLink ID="btnCurrency" runat="server" CssClass="hlCur" ResourceKey="btnConvertCurrency">Convert Currency</asp:HyperLink>
        </td>
        <td> <div id="dCurrency" runat="server" class="divCur" style="display: none">
<fieldset>
<legend class="AgapeH5"><asp:Label ID="Label1" runat="server" ResourceKey="lblTitle"></asp:Label></legend>
<asp:TextBox ID="tbCurrency" runat="server" class="numeric currency" Width="90px"></asp:TextBox>
                <asp:DropDownList ID="ddlCurrencies" runat="server" class="ddlCur">
                    <asp:ListItem Value="ALL">Albanian Lek</asp:ListItem>
<asp:ListItem Value="DZD">Algerian Dinar</asp:ListItem>
<asp:ListItem Value="ARS">Argentine Peso</asp:ListItem>
<asp:ListItem Value="AWG">Aruba Florin</asp:ListItem>
<asp:ListItem Value="AUD">Australian Dollar</asp:ListItem>
<asp:ListItem Value="BSD">Bahamian Dollar</asp:ListItem>
<asp:ListItem Value="BHD">Bahraini Dinar</asp:ListItem>
<asp:ListItem Value="BDT">Bangladesh Taka</asp:ListItem>
<asp:ListItem Value="BBD">Barbados Dollar</asp:ListItem>
<asp:ListItem Value="BYR">Belarus Ruble</asp:ListItem>
<asp:ListItem Value="BZD">Belize Dollar</asp:ListItem>
<asp:ListItem Value="BMD">Bermuda Dollar</asp:ListItem>
<asp:ListItem Value="BTN">Bhutan Ngultrum</asp:ListItem>
<asp:ListItem Value="BOB">Bolivian Boliviano</asp:ListItem>
<asp:ListItem Value="BWP">Botswana Pula</asp:ListItem>
<asp:ListItem Value="BRL">Brazilian Real</asp:ListItem>
<asp:ListItem Value="GBP">British Pound</asp:ListItem>
<asp:ListItem Value="BND">Brunei Dollar</asp:ListItem>
<asp:ListItem Value="BGN">Bulgarian Lev</asp:ListItem>
<asp:ListItem Value="BIF">Burundi Franc</asp:ListItem>
<asp:ListItem Value="KHR">Cambodia Riel</asp:ListItem>
<asp:ListItem Value="CAD">Canadian Dollar</asp:ListItem>
<asp:ListItem Value="CVE">Cape Verde Escudo</asp:ListItem>
<asp:ListItem Value="KYD">Cayman Islands Dollar</asp:ListItem>
<asp:ListItem Value="XOF">CFA Franc (BCEAO)</asp:ListItem>
<asp:ListItem Value="XAF">CFA Franc (BEAC)</asp:ListItem>
<asp:ListItem Value="CLP">Chilean Peso</asp:ListItem>
<asp:ListItem Value="CNY">Chinese Yuan</asp:ListItem>
<asp:ListItem Value="COP">Colombian Peso</asp:ListItem>
<asp:ListItem Value="KMF">Comoros Franc</asp:ListItem>
<asp:ListItem Value="CRC">Costa Rica Colon</asp:ListItem>
<asp:ListItem Value="HRK">Croatian Kuna</asp:ListItem>
<asp:ListItem Value="CUP">Cuban Peso</asp:ListItem>
<asp:ListItem Value="CZK">Czech Koruna</asp:ListItem>
<asp:ListItem Value="DKK">Danish Krone</asp:ListItem>
<asp:ListItem Value="DJF">Dijibouti Franc</asp:ListItem>
<asp:ListItem Value="DOP">Dominican Peso</asp:ListItem>
<asp:ListItem Value="XCD">East Caribbean Dollar</asp:ListItem>
<asp:ListItem Value="ECS">Ecuador Sucre</asp:ListItem>
<asp:ListItem Value="EGP">Egyptian Pound</asp:ListItem>
<asp:ListItem Value="SVC">El Salvador Colon</asp:ListItem>
<asp:ListItem Value="ERN">Eritrea Nakfa</asp:ListItem>
<asp:ListItem Value="EEK">Estonian Kroon</asp:ListItem>
<asp:ListItem Value="ETB">Ethiopian Birr</asp:ListItem>
<asp:ListItem Value="EUR">Euro</asp:ListItem>
<asp:ListItem Value="FKP">Falkland Islands Pound</asp:ListItem>
<asp:ListItem Value="FJD">Fiji Dollar</asp:ListItem>
<asp:ListItem Value="GMD">Gambian Dalasi</asp:ListItem>
<asp:ListItem Value="GHC">Ghanian Cedi</asp:ListItem>
<asp:ListItem Value="GIP">Gibraltar Pound</asp:ListItem>
<asp:ListItem Value="GTQ">Guatemala Quetzal</asp:ListItem>
<asp:ListItem Value="GNF">Guinea Franc</asp:ListItem>
<asp:ListItem Value="GYD">Guyana Dollar</asp:ListItem>
<asp:ListItem Value="HTG">Haiti Gourde</asp:ListItem>
<asp:ListItem Value="HNL">Honduras Lempira</asp:ListItem>
<asp:ListItem Value="HKD">Hong Kong Dollar</asp:ListItem>
<asp:ListItem Value="HUF">Hungarian Forint</asp:ListItem>
<asp:ListItem Value="ISK">Iceland Krona</asp:ListItem>
<asp:ListItem Value="INR">Indian Rupee</asp:ListItem>
<asp:ListItem Value="IDR">Indonesian Rupiah</asp:ListItem>
<asp:ListItem Value="IRR">Iran Rial</asp:ListItem>
<asp:ListItem Value="IQD">Iraqi Dinar</asp:ListItem>
<asp:ListItem Value="ILS">Israeli Shekel</asp:ListItem>
<asp:ListItem Value="JMD">Jamaican Dollar</asp:ListItem>
<asp:ListItem Value="JPY">Japanese Yen</asp:ListItem>
<asp:ListItem Value="JOD">Jordanian Dinar</asp:ListItem>
<asp:ListItem Value="KZT">Kazakhstan Tenge</asp:ListItem>
<asp:ListItem Value="KES">Kenyan Shilling</asp:ListItem>
<asp:ListItem Value="KWD">Kuwaiti Dinar</asp:ListItem>
<asp:ListItem Value="LAK">Lao Kip</asp:ListItem>
<asp:ListItem Value="LVL">Latvian Lat</asp:ListItem>
<asp:ListItem Value="LBP">Lebanese Pound</asp:ListItem>
<asp:ListItem Value="LSL">Lesotho Loti</asp:ListItem>
<asp:ListItem Value="LRD">Liberian Dollar</asp:ListItem>
<asp:ListItem Value="LYD">Libyan Dinar</asp:ListItem>
<asp:ListItem Value="LTL">Lithuanian Lita</asp:ListItem>
<asp:ListItem Value="MOP">Macau Pataca</asp:ListItem>
<asp:ListItem Value="MKD">Macedonian Denar</asp:ListItem>
<asp:ListItem Value="MWK">Malawi Kwacha</asp:ListItem>
<asp:ListItem Value="MYR">Malaysian Ringgit</asp:ListItem>
<asp:ListItem Value="MVR">Maldives Rufiyaa</asp:ListItem>
<asp:ListItem Value="MTL">Maltese Lira</asp:ListItem>
<asp:ListItem Value="MRO">Mauritania Ougulya</asp:ListItem>
<asp:ListItem Value="MUR">Mauritius Rupee</asp:ListItem>
<asp:ListItem Value="MXN">Mexican Peso</asp:ListItem>
<asp:ListItem Value="MDL">Moldovan Leu</asp:ListItem>
<asp:ListItem Value="MNT">Mongolian Tugrik</asp:ListItem>
<asp:ListItem Value="MAD">Moroccan Dirham</asp:ListItem>
<asp:ListItem Value="MMK">Myanmar Kyat</asp:ListItem>
<asp:ListItem Value="NAD">Namibian Dollar</asp:ListItem>
<asp:ListItem Value="NPR">Nepalese Rupee</asp:ListItem>
<asp:ListItem Value="ANG">Neth Antilles Guilder</asp:ListItem>
<asp:ListItem Value="NZD">New Zealand Dollar</asp:ListItem>
<asp:ListItem Value="NIO">Nicaragua Cordoba</asp:ListItem>
<asp:ListItem Value="NGN">Nigerian Naira</asp:ListItem>
<asp:ListItem Value="KPW">North Korean Won</asp:ListItem>
<asp:ListItem Value="NOK">Norwegian Krone</asp:ListItem>
<asp:ListItem Value="OMR">Omani Rial</asp:ListItem>
<asp:ListItem Value="PKR">Pakistani Rupee</asp:ListItem>
<asp:ListItem Value="PAB">Panama Balboa</asp:ListItem>
<asp:ListItem Value="PGK">Papua New Guinea Kina</asp:ListItem>
<asp:ListItem Value="PYG">Paraguayan Guarani</asp:ListItem>
<asp:ListItem Value="PEN">Peruvian Nuevo Sol</asp:ListItem>
<asp:ListItem Value="PHP">Philippine Peso</asp:ListItem>
<asp:ListItem Value="PLN">Polish Zloty</asp:ListItem>
<asp:ListItem Value="QAR">Qatar Rial</asp:ListItem>
<asp:ListItem Value="RON">Romanian New Leu</asp:ListItem>
<asp:ListItem Value="RUB">Russian Rouble</asp:ListItem>
<asp:ListItem Value="RWF">Rwanda Franc</asp:ListItem>
<asp:ListItem Value="WST">Samoa Tala</asp:ListItem>
<asp:ListItem Value="STD">Sao Tome Dobra</asp:ListItem>
<asp:ListItem Value="SAR">Saudi Arabian Riyal</asp:ListItem>
<asp:ListItem Value="SCR">Seychelles Rupee</asp:ListItem>
<asp:ListItem Value="SLL">Sierra Leone Leone</asp:ListItem>
<asp:ListItem Value="SGD">Singapore Dollar</asp:ListItem>
<asp:ListItem Value="SKK">Slovak Koruna</asp:ListItem>
<asp:ListItem Value="SIT">Slovenian Tolar</asp:ListItem>
<asp:ListItem Value="SBD">Solomon Islands Dollar</asp:ListItem>
<asp:ListItem Value="SOS">Somali Shilling</asp:ListItem>
<asp:ListItem Value="ZAR">South African Rand</asp:ListItem>
<asp:ListItem Value="KRW">South Korean Won</asp:ListItem>
<asp:ListItem Value="LKR">Sri Lanka Rupee</asp:ListItem>
<asp:ListItem Value="SHP">St Helena Pound</asp:ListItem>
<asp:ListItem Value="SDG">Sudanese Pound</asp:ListItem>
<asp:ListItem Value="SZL">Swaziland Lilageni</asp:ListItem>
<asp:ListItem Value="SEK">Swedish Krona</asp:ListItem>
<asp:ListItem Value="CHF">Swiss Franc</asp:ListItem>
<asp:ListItem Value="SYP">Syrian Pound</asp:ListItem>
<asp:ListItem Value="TWD">Taiwan Dollar</asp:ListItem>
<asp:ListItem Value="TZS">Tanzanian Shilling</asp:ListItem>
<asp:ListItem Value="THB">Thai Baht</asp:ListItem>
<asp:ListItem Value="TOP">Tonga Pa'ang</asp:ListItem>
<asp:ListItem Value="TTD">Trinidad Tobago Dollar</asp:ListItem>
<asp:ListItem Value="TND">Tunisian Dinar</asp:ListItem>
<asp:ListItem Value="TRY">Turkish Lira</asp:ListItem>
<asp:ListItem Value="AED">UAE Dirham</asp:ListItem>
<asp:ListItem Value="UGX">Ugandan Shilling</asp:ListItem>
<asp:ListItem Value="UAH">Ukraine Hryvnia</asp:ListItem>
<asp:ListItem Value="USD">United States Dollar</asp:ListItem>
<asp:ListItem Value="UYU">Uruguayan New Peso</asp:ListItem>
<asp:ListItem Value="VUV">Vanuatu Vatu</asp:ListItem>
<asp:ListItem Value="VEF">Venezuelan Bolivar Fuerte</asp:ListItem>
<asp:ListItem Value="VND">Vietnam Dong</asp:ListItem>
<asp:ListItem Value="YER">Yemen Riyal</asp:ListItem>
<asp:ListItem Value="ZMK">Zambian Kwacha</asp:ListItem>
<asp:ListItem Value="ZWD">Zimbabwe Dollar</asp:ListItem>
                
                </asp:DropDownList>

          
            </fieldset>   </div> 
        
        </td>
    </tr>
</table>

