<%@ Control Language="VB" AutoEventWireup="false" CodeFile="AddDocument.ascx.vb" Inherits="DotNetNuke.Modules.AgapeDocuments.AddDocument" %>
<%@ Register src="~/controls/urlcontrol.ascx" tagname="urlcontrol" tagprefix="uc1" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx"%>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<table>
    <tr>
        <td><b><dnn:label id="lblTitle" runat="server" controlname="Title"  resourcekey="lblTitle"  /></b></td>
        <td><asp:TextBox ID="Title" runat="server" Width="300px"></asp:TextBox></td>
    </tr>
    <tr>
        <td><b><dnn:label id="lblSubtitle" runat="server" controlname="Subtitle"  resourcekey="lblSubtitle"  /></b></td>
        <td><asp:TextBox ID="Subtitle" runat="server" Width="300px"></asp:TextBox></td>
    </tr>
     
    <tr>
        <td><b><dnn:label id="lblFile" runat="server" controlname="theFile"  resourcekey="lblFile"  /></b></td>
        <td>
            <uc1:urlcontrol ID="theFile" runat="server" FileFilter="jpg,gif,png,pdf,mp3,doc,xls,txt,mp4,zip"
            ShowFiles="true" ShowTrack="false" ShowLog="false"  />
        </td>
    </tr>
   
</table>

<asp:LinkButton ID="AddBtn" runat="server" resourcekey="btnAdd"></asp:LinkButton> &nbsp; &nbsp;
<asp:LinkButton ID="CancelBtn" runat="server" resourcekey="Cancel"></asp:LinkButton>
