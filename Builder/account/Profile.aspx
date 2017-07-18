<%@ Page Title="Manage Profile" Language="vb" AutoEventWireup="false" MasterPageFile="~/masterPages/ContentPage.Master" CodeBehind="Profile.aspx.vb" Inherits="UtilityWizards.Builder.Profile" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ MasterType VirtualPath="~/masterPages/ContentPage.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <style type="text/css">
        .wrap-table-profile {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="menuContent" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="breadcrumbContent" runat="server">
    <h1>Dashboard
        <small>Version 2.2</small>
    </h1>
    <ol class="breadcrumb">
        <li><a href="..Default.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
        <li class="active">Manage Profile</li>
    </ol>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="pageContent_Ajax" runat="server">
    <table class="wrap-table-profile">
        <tr>
            <td style="width: 25px;">&nbsp;</td>
            <td>
                <asp:Image ID="imgAvatar" runat="server" Height="80px" />
            </td>
        </tr>
        <tr>
            <td style="width: 25px;">&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td style="width: 25px;">
                <asp:Image ID="imgHelp_Question" runat="server" ImageUrl="~/images/toolbar/icon_help.png" ToolTip="Enter the wording for your question" />
            </td>
            <td>Name</td>
        </tr>
        <tr>
            <td style="width: 25px;">&nbsp;</td>
            <td>
                <telerik:RadTextBox ID="txtName" runat="server" Skin="Metro" Width="100%">
                </telerik:RadTextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Image ID="Image1" runat="server" ImageUrl="~/images/toolbar/icon_help.png" ToolTip="Enter the wording for your question" />
            </td>
            <td>Email Address</td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>
                <telerik:RadTextBox ID="txtEmail" runat="server" AutoPostBack="True" Skin="Metro" Width="100%" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Image ID="Image6" runat="server" ImageUrl="~/images/toolbar/icon_help.png" ToolTip="Enter the wording for your question" />
            </td>
            <td>Mobile Username</td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>
                <telerik:RadTextBox ID="txtMobileUser" runat="server" AutoPostBack="True" Skin="Metro" Width="100%" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Image ID="Image2" runat="server" ImageUrl="~/images/toolbar/icon_help.png" ToolTip="Enter the wording for your question" />
            </td>
            <td>Password</td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>
                <telerik:RadTextBox ID="txtPassword" runat="server" Skin="Metro" Width="100%">
                </telerik:RadTextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Image ID="Image8" runat="server" ImageUrl="~/images/toolbar/icon_help.png" ToolTip="Enter the wording for your question" />
            </td>
            <td>Mobile Number</td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>
                <telerik:RadTextBox ID="txtMobileNumber" runat="server" Skin="Metro" Width="100%">
                </telerik:RadTextBox>
            </td>
        </tr>
    </table>
    <asp:Panel runat="server" ID="pnlHidden" Visible="false" BackColor="#CC0000">
        <asp:Label runat="server" ID="lblEditID" />
        <telerik:RadComboBox ID="ddlClient" runat="server" Skin="Metro" Width="100px">
        </telerik:RadComboBox>
        <asp:CheckBox ID="chkApiAccess" runat="server" Enabled="False" />
        <asp:CheckBox ID="chkWebAccess" runat="server" Enabled="False" />
        <telerik:RadTextBox ID="txtOneSignal" runat="server" Skin="Metro" Width="100px">
        </telerik:RadTextBox>
        <telerik:RadComboBox ID="ddlPermissions" runat="server" AutoPostBack="True" Enabled="False" Skin="Metro" Width="100px" />
        <telerik:RadComboBox ID="ddlSupervisor" runat="server" Enabled="False" Skin="Metro" Width="100px">
        </telerik:RadComboBox>
        <telerik:RadTextBox ID="txtDeviceID" runat="server" Enabled="False" Skin="Metro" Width="100px">
        </telerik:RadTextBox>
        <telerik:RadComboBox ID="ddlDeviceType" runat="server" AutoPostBack="True" Enabled="False" Skin="Metro" Width="100px" />
    </asp:Panel>
    <table>
        <tr>
            <td colspan="3">&nbsp;</td>
        </tr>
        <tr>
            <td style="width: 25px">&nbsp;</td>
            <td>
                <telerik:RadButton ID="btnSave" runat="server" Text="Save" Skin="Metro" />
            </td>
            <td>
                <telerik:RadButton ID="btnCancel" runat="server" Text="Cancel" Skin="Metro" />
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="pageContent_OutsideAjax" runat="server">
</asp:Content>
