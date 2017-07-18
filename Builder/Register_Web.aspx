<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Register_Web.aspx.vb" Inherits="UtilityWizards.Builder.Register_Web" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>Utility Wizards by Solvtopia, LLC.</title>
    <!-- Tell the browser to be responsive to screen width -->
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport">
    <style type="text/css">
        .wrap-table-login {
            width: 100%;
        }

        .rbLinkButton.fixedWidth {
            padding: 0;
        }

        body {
            font-family: "Source Sans Pro", sans-serif;
            color: #fff;
            /*font-family: 'Trebuchet MS', 'Lucida Sans Unicode', 'Lucida Grande', 'Lucida Sans', Arial, sans-serif;*/
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager" runat="server">
            <Scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
            </Scripts>
        </telerik:RadScriptManager>
        <%--<telerik:RadFormDecorator ID="RadFormDecorator" runat="server" Skin="Metro" />--%>
        <telerik:RadAjaxLoadingPanel ID="MainAjaxLoadingPanel" runat="server" Skin="Metro">
        </telerik:RadAjaxLoadingPanel>
        <telerik:RadAjaxPanel ID="MainAjaxPanel" runat="server" Width="100%" HorizontalAlign="NotSet" LoadingPanelID="MainAjaxLoadingPanel">
            <asp:Panel runat="server" ID="pnlStep1">
                <table class="wrap-table-login">
                    <tr>
                        <td style="font-weight: bold; text-decoration: underline;">Organization Information</td>
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
                        <td style="font-weight: bold; text-decoration: underline;">Administrative Contact</td>
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
                        <td style="font-weight: bold; text-decoration: underline;">System Administrator User Information</td>
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
                <span style="text-align: center; font-weight: bold; text-decoration: underline; font-size: 12pt;">Registration complete!</span>
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
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
