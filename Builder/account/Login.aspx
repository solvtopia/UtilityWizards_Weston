<%@ Page Title="System Login" Language="vb" AutoEventWireup="false" MasterPageFile="~/masterPages/ContentPage.Master" CodeBehind="Login.aspx.vb" Inherits="UtilityWizards.Builder.Login" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ MasterType VirtualPath="~/masterPages/ContentPage.Master" %>

<%@ Register assembly="UtilityWizards.CommonCore" namespace="UtilityWizards.CommonCore.Controls.CheckBoxes" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <style type="text/css">
        .wrap-table-login {
            width: 100%;
        }

        .rbLinkButton.fixedWidth {
            padding: 0;
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
        <li class="active">System Login</li>
    </ol>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="pageContent_Ajax" runat="server">
    <asp:Panel runat="server" ID="pnlForm" DefaultButton="btnLogin">
        <table class="wrap-table-login">
            <tr>
                <td>
                    <center>
                        <asp:Image ID="imgLogo" runat="server" ImageUrl="~/images/full_logo_mobile.png" Width="250px" /></center>
                </td>
            </tr>
            <tr>
                <td>&nbsp;&nbsp;</td>
            </tr>
            <tr>
                <td>Email Address</td>
            </tr>
            <tr>
                <td>
                    <telerik:RadTextBox ID="txtEmail" runat="server" Width="100%" Skin="Metro"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>Password</td>
            </tr>
            <tr>
                <td>
                    <telerik:RadTextBox ID="txtPassword" runat="server" TextMode="Password" Width="100%" Skin="Metro"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>&nbsp;&nbsp;</td>
            </tr>
            <tr>
                <td>
                    <cc1:CheckBox ID="chkRememberMe" runat="server" Checked="True" Text="Keep me logged in" />
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
                    <telerik:RadButton ID="btnLogin" runat="server" ButtonType="LinkButton" CssClass="fixedWidth" Skin="Metro" Text="Login" Width="100%" />
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
                    <br />
                    <asp:Label ID="lblMessage" runat="server" ForeColor="#CC0000"></asp:Label><br />
                    <br />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="pageContent_OutsideAjax" runat="server">
</asp:Content>
