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
    <telerik:RadTabStrip ID="RadTabStrip1" runat="server" SelectedIndex="0" MultiPageID="RadMultiPage1" Skin="Metro">
        <Tabs>
            <telerik:RadTab runat="server" Text="User Details">
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="811 Setup">
            </telerik:RadTab>
        </Tabs>
    </telerik:RadTabStrip>
    <telerik:RadMultiPage ID="RadMultiPage1" runat="server" SelectedIndex="0" Width="100%">
        <telerik:RadPageView ID="RadPageView1" runat="server" Selected="true">
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
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView2" runat="server">
            <table class="wrap-table-profile">
                <tr>
                    <td style="width: 25px;">&nbsp;</td>
                    <td>811 Module</td>
                </tr>
                <tr>
                    <td style="width: 25px;">&nbsp;</td>
                    <td>
                        <telerik:RadComboBox ID="ddl811Module" runat="server" Skin="Metro" Width="100%">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 25px;">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 25px;">&nbsp;</td>
                    <td>System Administrator Account</td>
                </tr>
                <tr>
                    <td style="width: 25px;">&nbsp;</td>
                    <td>
                        <telerik:RadComboBox ID="ddl811AdminAcount" runat="server" Skin="Metro" Width="100%">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 25px;">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 25px;">&nbsp;</td>
                    <td>
                        <strong>811 Ticket Email</strong></td>
                </tr>
                <tr>
                    <td style="width: 25px;">&nbsp;</td>
                    <td>&nbsp;Address</td>
                </tr>
                <tr>
                    <td style="width: 25px;">&nbsp;</td>
                    <td>
                        <telerik:RadTextBox ID="txt811EmailAddress" runat="server" Skin="Metro" Width="100%">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 25px;">&nbsp;</td>
                    <td>Server Address</td>
                </tr>
                <tr>
                    <td style="width: 25px;">&nbsp;</td>
                    <td>
                        <telerik:RadTextBox ID="txt811EmailServer" runat="server" Skin="Metro" Width="100%">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 25px;">&nbsp;</td>
                    <td>Password</td>
                </tr>
                <tr>
                    <td style="width: 25px;">&nbsp;</td>
                    <td>
                        <telerik:RadTextBox ID="txt811EmailPass" runat="server" Skin="Metro" Width="100%">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 25px;">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 25px;">&nbsp;</td>
                    <td>
                        <strong>811 Ticket FTP</strong></td>
                </tr>
                <tr>
                    <td style="width: 25px;">&nbsp;</td>
                    <td>Address</td>
                </tr>
                <tr>
                    <td style="width: 25px;">&nbsp;</td>
                    <td>
                        <telerik:RadTextBox ID="txt811FtpServer" runat="server" Skin="Metro" Width="100%">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 25px;">&nbsp;</td>
                    <td>Username</td>
                </tr>
                <tr>
                    <td style="width: 25px;">&nbsp;</td>
                    <td>
                        <telerik:RadTextBox ID="txt811FtpUser" runat="server" Skin="Metro" Width="100%">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 25px;">&nbsp;</td>
                    <td>Password</td>
                </tr>
                <tr>
                    <td style="width: 25px;">&nbsp;</td>
                    <td>
                        <telerik:RadTextBox ID="txt811FtpPass" runat="server" Skin="Metro" Width="100%">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 25px;">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 25px;">&nbsp;</td>
                    <td>Notify the following people of new tickets</td>
                </tr>
                <tr>
                    <td style="width: 25px;">&nbsp;</td>
                    <td>
                        <asp:CheckBoxList ID="cbl811Notify" runat="server" Font-Bold="False" Width="100%">
                        </asp:CheckBoxList>
                    </td>
                </tr>
            </table>
        </telerik:RadPageView>
    </telerik:RadMultiPage>
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
        <asp:CheckBox ID="chk811Record" runat="server" />
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
