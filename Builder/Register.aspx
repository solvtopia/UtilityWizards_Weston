<%@ Page Title="New User Registration" Language="vb" AutoEventWireup="false" MasterPageFile="~/masterPages/ContentPage.master" CodeBehind="Register.aspx.vb" Inherits="UtilityWizards.Builder.Register" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ MasterType VirtualPath="~/masterPages/ContentPage.Master" %>

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
<asp:Content ID="Content2" ContentPlaceHolderID="menuContent" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="breadcrumbContent" runat="server">
    <h1>Dashboard
        <small>Version 2.2 <asp:Label runat="server" ID="lblSandbox" /></small>
    </h1>
    <ol class="breadcrumb">
        <li class="active">New User Registration</li>
    </ol>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pageContent_Ajax" runat="server">
    <asp:Panel runat="server" ID="pnlStep1">
        <table class="wrap-table-login">
            <tr>
                <td style="font-weight: bold;">Organization Information</td>
            </tr>
            <tr>
                <td>Name</td>
            </tr>
            <tr>
                <td>
                    <telerik:RadTextBox ID="txtClientName" runat="server" Skin="Metro" Width="100%">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>Address Line 1</td>
            </tr>
            <tr>
                <td>
                    <telerik:RadTextBox ID="txtClientAddress1" runat="server" Skin="Metro" Width="100%">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>Address Line 2</td>
            </tr>
            <tr>
                <td>
                    <telerik:RadTextBox ID="txtClientAddress2" runat="server" Skin="Metro" Width="100%">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>City</td>
            </tr>
            <tr>
                <td>
                    <telerik:RadTextBox ID="txtClientCity" runat="server" Skin="Metro" Width="100%">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>State</td>
            </tr>
            <tr>
                <td>
                    <telerik:RadTextBox ID="txtClientState" runat="server" Skin="Metro" Width="100%">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>Zip Code</td>
            </tr>
            <tr>
                <td>
                    <telerik:RadTextBox ID="txtClientZipCode" runat="server" Skin="Metro" Width="100%">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td style="font-weight: bold;">Administrative Contact</td>
            </tr>
            <tr>
                <td>Name</td>
            </tr>
            <tr>
                <td>
                    <telerik:RadTextBox ID="txtClientContactName" runat="server" Skin="Metro" Width="100%">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>Phone Number</td>
            </tr>
            <tr>
                <td>
                    <telerik:RadTextBox ID="txtClientContactPhone" runat="server" Skin="Metro" Width="100%">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>Email Address</td>
            </tr>
            <tr>
                <td>
                    <telerik:RadTextBox ID="txtClientContactEmail" runat="server" Skin="Metro" Width="100%">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td style="font-weight: bold;">System Administrator User Information</td>
            </tr>
            <tr>
                <td>Name</td>
            </tr>
            <tr>
                <td>
                    <telerik:RadTextBox ID="txtAdminName" runat="server" Skin="Metro" Width="100%">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>Email Address</td>
            </tr>
            <tr>
                <td>
                    <telerik:RadTextBox ID="txtAdminEmail" runat="server" Skin="Metro" Width="100%">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>Password</td>
            </tr>
            <tr>
                <td>
                    <telerik:RadTextBox ID="txtAdminPassword" runat="server" Skin="Metro" TextMode="Password" Width="100%">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>Confirm Password</td>
            </tr>
            <tr>
                <td>
                    <telerik:RadTextBox ID="txtAdminPasswordConfirm" runat="server" Skin="Metro" TextMode="Password" Width="100%">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
                    <telerik:RadButton ID="btnRegister" runat="server" Text="Register" ButtonType="LinkButton" Skin="Metro" Width="100%" CssClass="fixedWidth" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlDone">
        <span style="text-align: center; font-weight: bold; font-size: 12pt;">Registration complete!</span>
        <br />
        <br />
        <span style="text-align: center;">Congratulations! You have completed the registration process and your demo account has been created.<br />
            <br />
            You will be contacted shortly by one of our sales representatives just in case you have any questions or need assistance.<br />
            <br />
            Thank you for your interest in Utility Wizards by Solvtopia, LLC!!</span>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlHidden" Visible="false" BackColor="#CC0000">
        <asp:Label runat="server" ID="lblClientID" />
        <asp:Label ID="lblUserID" runat="server" />
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="pageContent_OutsideAjax" runat="server">
</asp:Content>
