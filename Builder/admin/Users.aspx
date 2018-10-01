<%@ Page Title="Users & Technicians" Language="vb" AutoEventWireup="false" MasterPageFile="~/masterPages/Wizard_V2.master" CodeBehind="Users.aspx.vb" Inherits="UtilityWizards.Builder.AdminUsers" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ MasterType VirtualPath="~/masterPages/Wizard_V2.Master" %>

<%@ Register Assembly="UtilityWizards.CommonCore.Shared" Namespace="UtilityWizards.CommonCore.Shared.Controls.TextBoxes" TagPrefix="Builder" %>
<%@ Register Assembly="UtilityWizards.CommonCore.Shared" Namespace="UtilityWizards.CommonCore.Shared.Controls.CheckBoxes" TagPrefix="Builder" %>
<%@ Register Assembly="UtilityWizards.CommonCore.Shared" Namespace="UtilityWizards.CommonCore.Shared.Controls.DropDownLists" TagPrefix="Builder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="wizardHeadContent" runat="server">
    <style type="text/css">
        .wrap-table {
            width: 100%;
            display: block;
        }

            .wrap-table td {
                display: inline-block;
            }
    </style>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="wizardMenuContent" runat="server">
    <ul class="sidebar-menu">
        <li class="header">USER OPTIONS</li>
        <li runat="server" id="liNewUser">
            <asp:LinkButton runat="server" ID="lnkNewUser"><i class="fa fa-file-text-o"></i><span>New User</span></asp:LinkButton>
        </li>
        <li runat="server" id="liDeleteUser">
            <asp:LinkButton runat="server" ID="lnkDeleteUser"><i class="fa fa-trash-o"></i><span>Delete This User</span></asp:LinkButton>
        </li>
        <li runat="server" id="liClearUser">
            <asp:LinkButton runat="server" ID="lnkClearUser"><i class="fa fa-asdf"></i><span>Clear Form</span></asp:LinkButton>
        </li>
    </ul>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="wizardBreadcrumbContent" runat="server">
    <h1>Dashboard
        <small>Version 2.2 <asp:Label runat="server" ID="lblSandbox" /></small>
    </h1>
    <ol class="breadcrumb">
        <li><a href="..Default.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
        <li><a href="#">Admin</a></li>
        <li class="active">Users & Technicians</li>
    </ol>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="wizardContent_Ajax" runat="server">
    <table class="wrap-table">
        <tr>
            <td style="font-weight: bold;">Administrators/System Administrators</td>
        </tr>
        <tr>
            <td style="border: 1px solid #C0C0C0; vertical-align: top; background-color: #E6E6E6;">
                <asp:Table ID="tblAdmins" runat="server" CellPadding="1" CellSpacing="2" Style="display: block;" Width="100%" />
            </td>
        </tr>
        <tr>
            <td style="font-weight: bold;">System Users</td>
        </tr>
        <tr>
            <td style="border: 1px solid #C0C0C0; vertical-align: top; background-color: #E6E6E6;">
                <asp:Table runat="server" ID="tblUsers" CellPadding="1" CellSpacing="2" Width="100%" Style="display: block;" />
            </td>
        </tr>
        <tr>
            <td style="font-weight: bold;">Technicians</td>
        </tr>
        <tr>
            <td style="border: 1px solid #C0C0C0; vertical-align: top; background-color: #E6E6E6;">
                <asp:Table runat="server" ID="tblTechnicians" CellPadding="1" CellSpacing="2" Width="100%" Style="display: block;" />
            </td>
        </tr>
        <tr>
            <td style="font-weight: bold;">Supervisors</td>
        </tr>
        <tr>
            <td style="border: 1px solid #C0C0C0; vertical-align: top; background-color: #E6E6E6;">
                <asp:Table runat="server" ID="tblSupervisors" CellPadding="1" CellSpacing="2" Width="100%" Style="display: block;" />
            </td>
        </tr>
        <tr>
            <td style="font-weight: bold;">Solvtopia Users</td>
        </tr>
        <tr>
            <td style="border: 1px solid #C0C0C0; vertical-align: top; background-color: #E6E6E6;">
                <asp:Table runat="server" ID="tblSolvtopia" CellPadding="1" CellSpacing="2" Width="100%" Style="display: block;" />
            </td>
        </tr>
    </table>
    <asp:Panel runat="server" ID="pnlHidden" Visible="false" BackColor="#CC0000">
        <asp:Label runat="server" ID="lblEditID" />
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="wizardContent_OutsideAjax" runat="server">
</asp:Content>
<asp:Content ID="Content4" runat="server" ContentPlaceHolderID="wizardInfoContent">
    <table class="wrap-table">
        <tr>
            <td colspan="2">Icon Legend</td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td style="vertical-align: middle;">
                <asp:Image ID="Image1" runat="server" ImageUrl="~/images/icon_administrator_avatar.png" Width="80px" />
            </td>
            <td style="vertical-align: middle;">Administrators/System Administrators</td>
        </tr>
        <tr>
            <td style="vertical-align: middle;">
                <asp:Image ID="Image2" runat="server" ImageUrl="~/images/icon_user_avatar.png" Width="80px" />
            </td>
            <td style="vertical-align: middle;">System Users</td>
        </tr>
        <tr>
            <td style="vertical-align: middle;">
                <asp:Image ID="Image3" runat="server" ImageUrl="~/images/icon_technician_avatar.png" Width="80px" />
            </td>
            <td style="vertical-align: middle;">Technicians</td>
        </tr>
        <tr>
            <td style="vertical-align: middle;">
                <asp:Image ID="Image5" runat="server" ImageUrl="~/images/icon_supervisor_avatar.png" Width="80px" />
            </td>
            <td style="vertical-align: middle;">Supervisors</td>
        </tr>
        <tr>
            <td style="vertical-align: middle;">
                <asp:Image ID="Image4" runat="server" ImageUrl="~/images/icon_72.png" Width="80px" />
            </td>
            <td style="vertical-align: middle;">Solvtopia Users</td>
        </tr>
        <tr>
            <td style="vertical-align: middle;">&nbsp;</td>
            <td style="vertical-align: middle;">&nbsp;</td>
        </tr>
        <tr>
            <td style="vertical-align: middle;">&nbsp;</td>
            <td style="vertical-align: middle;">&nbsp;</td>
        </tr>
        <tr>
            <td style="vertical-align: middle;">&nbsp;</td>
            <td style="vertical-align: middle;">&nbsp;</td>
        </tr>
    </table>
</asp:Content>

