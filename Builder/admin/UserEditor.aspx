<%@ Page Title="User Editor" Language="vb" AutoEventWireup="false" MasterPageFile="~/masterPages/Wizard_V2.Master" CodeBehind="UserEditor.aspx.vb" Inherits="UtilityWizards.Builder.AdminUserEditor" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ MasterType VirtualPath="~/masterPages/ContentPage.Master" %>

<%@ Register Assembly="UtilityWizards.CommonCore.Shared" Namespace="UtilityWizards.CommonCore.Shared.Controls.TextBoxes" TagPrefix="Builder" %>
<%@ Register Assembly="UtilityWizards.CommonCore.Shared" Namespace="UtilityWizards.CommonCore.Shared.Controls.CheckBoxes" TagPrefix="Builder" %>
<%@ Register Assembly="UtilityWizards.CommonCore.Shared" Namespace="UtilityWizards.CommonCore.Shared.Controls.DropDownLists" TagPrefix="Builder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="wizardheadContent" runat="server">
    <style type="text/css">
        .wrap-table-users {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="wizardmenuContent" runat="server">
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
<asp:Content ID="Content6" ContentPlaceHolderID="wizardbreadcrumbContent" runat="server">
    <h1>Dashboard
        <small>Version 2.2</small>
    </h1>
    <ol class="breadcrumb">
        <li><a href="..Default.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
        <li><a href="#">Admin</a></li>
        <li><a href="Users.aspx">Users & Technicians</a></li>
        <li class="active">User Editor</li>
    </ol>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="wizardContent_Ajax" runat="server">
    <asp:Panel runat="server" ID="pnlStep1">
        <table class="wrap-table-users">
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
                <td style="width: 25px;">&nbsp;</td>
                <td>Client</td>
            </tr>
            <tr>
                <td style="width: 25px;">&nbsp;</td>
                <td>
                    <telerik:RadComboBox ID="ddlClient" runat="server" Skin="Metro" Width="100%">
                    </telerik:RadComboBox>
                </td>
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
                <td style="width: 25px;">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/images/toolbar/icon_help.png" ToolTip="Enter the wording for your question" />
                </td>
                <td>Email Address</td>
            </tr>
            <tr>
                <td style="width: 25px;">&nbsp;</td>
                <td>
                    <telerik:RadTextBox ID="txtEmail" runat="server" AutoPostBack="True" Skin="Metro" Width="100%" />
                </td>
            </tr>
            <tr>
                <td style="width: 25px;">
                    <asp:Image ID="Image6" runat="server" ImageUrl="~/images/toolbar/icon_help.png" ToolTip="Enter the wording for your question" />
                </td>
                <td>Mobile Username</td>
            </tr>
            <tr>
                <td style="width: 25px;">&nbsp;</td>
                <td>
                    <telerik:RadTextBox ID="txtMobileUser" runat="server" AutoPostBack="True" Skin="Metro" Width="100%" />
                </td>
            </tr>
            <tr>
                <td style="width: 25px;">
                    <asp:Image ID="Image2" runat="server" ImageUrl="~/images/toolbar/icon_help.png" ToolTip="Enter the wording for your question" />
                </td>
                <td>Password</td>
            </tr>
            <tr>
                <td style="width: 25px;">&nbsp;</td>
                <td>
                    <telerik:RadTextBox ID="txtPassword" runat="server" Skin="Metro" Width="100%">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 25px;">
                    <asp:Image ID="Image8" runat="server" ImageUrl="~/images/toolbar/icon_help.png" ToolTip="Enter the wording for your question" />
                </td>
                <td>Mobile Number</td>
            </tr>
            <tr>
                <td style="width: 25px;">&nbsp;</td>
                <td>
                    <telerik:RadTextBox ID="txtMobileNumber" runat="server" Skin="Metro" Width="100%">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 25px;">&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td style="width: 25px;">
                    <asp:Image ID="Image3" runat="server" ImageUrl="~/images/toolbar/icon_help.png" ToolTip="Enter the wording for your question" />
                </td>
                <td>Permissions</td>
            </tr>
            <tr>
                <td style="width: 25px;">&nbsp;</td>
                <td>
                    <telerik:RadComboBox ID="ddlPermissions" runat="server" AutoPostBack="True" Skin="Metro" Width="100%" />
                </td>
            </tr>
            <asp:Panel runat="server" ID="pnlSupervisor">
                <tr>
                    <td style="width: 25px;">
                        <asp:Image ID="Image7" runat="server" ImageUrl="~/images/toolbar/icon_help.png" ToolTip="Enter the wording for your question" />
                    </td>
                    <td>Supervisor</td>
                </tr>
                <tr>
                    <td style="width: 25px;">&nbsp;</td>
                    <td>
                        <telerik:RadComboBox ID="ddlSupervisor" runat="server" Skin="Metro" Width="100%">
                        </telerik:RadComboBox>
                    </td>
                </tr>
            </asp:Panel>
            <tr>
                <td style="width: 25px;">&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td style="width: 25px;">
                    <asp:Image ID="Image5" runat="server" ImageUrl="~/images/toolbar/icon_help.png" ToolTip="Enter the wording for your question" />
                </td>
                <td>API Access Allowed</td>
            </tr>
            <tr>
                <td style="width: 25px;">&nbsp;</td>
                <td>
                    <asp:CheckBox ID="chkApiAccess" runat="server" Enabled="False" />
                </td>
            </tr>
            <tr>
                <td style="width: 25px;">&nbsp;</td>
                <td>Web Access Allowed</td>
            </tr>
            <tr>
                <td style="width: 25px;">&nbsp;</td>
                <td>
                    <asp:CheckBox ID="chkWebAccess" runat="server" Enabled="False" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlStep2">
        <table class="wrap-table-users">
            <tr>
                <td style="width: 25px;">&nbsp;</td>
                <td>Select the modules you would like this user to have access to from the list below:</td>
            </tr>
            <tr>
                <td style="width: 25px;">&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td style="width: 25px;">&nbsp;</td>
                <td>
                    <telerik:RadTreeView ID="tvModules" runat="server" Width="100%" CheckBoxes="true" CheckChildNodes="true">
                    </telerik:RadTreeView>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlStep3">
        <table class="wrap-table-users">
            <tr>
                <td style="width: 25px;">&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td style="width: 25px;">&nbsp;</td>
                <td>Client</td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlStep4">
        Step 4
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlStep5">
        Step 5
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlStep6">
        Step 6
    </asp:Panel>
    <table>
        <tr>
            <td colspan="4">&nbsp;&nbsp;</td>
        </tr>
        <tr>
            <td style="width: 25px;">&nbsp;</td>
            <td>
                <telerik:RadButton ID="btnBack" runat="server" Text="Back" Skin="Metro" />
            </td>
            <td>
                <telerik:RadButton ID="btnNext" runat="server" Text="Next" Skin="Metro" />
            </td>
            <td>
                <telerik:RadButton ID="btnCancel" runat="server" Text="Cancel" Skin="Metro" />
            </td>
        </tr>
    </table>
    <asp:Panel runat="server" ID="pnlHidden" Visible="false" BackColor="#CC0000">
        <asp:Label runat="server" ID="lblEditID" />
        <telerik:RadTextBox ID="txtDeviceID" runat="server" Skin="Metro" Width="100%">
        </telerik:RadTextBox>
        <telerik:RadTextBox ID="txtOneSignal" runat="server" Skin="Metro" Width="100%">
        </telerik:RadTextBox>
        <telerik:RadComboBox ID="ddlDeviceType" runat="server" AutoPostBack="True" Enabled="False" Skin="Metro" Width="100%" />
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="wizardInfoContent" runat="server">
    <asp:Panel ID="pnlStepInfo1" runat="server">
        <asp:Panel runat="server" ID="pnlFolderStepInfo1">
            Just like on your computer, folders are used to organize items into groups so they can be quickly and easily accessed.<br />
            <br />
            Lets start by defining the basics of your Folder.<br />
            <br />
            When you&#39;re finished, hit the &lt;Next&gt; button at the bottom to continue.
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlModuleStepInfo1">
            Lets start by defining the basics of your Module.<br />
            <br />
            When you&#39;re finished, hit the &lt;Next&gt; button at the bottom to continue.
        </asp:Panel>
        <br />
    </asp:Panel>
    <asp:Panel ID="pnlStepInfo2" runat="server">
        <asp:Panel runat="server" ID="pnlFolderStepInfo2">
            All folders in the Utility Wizards system allow you to define basic questions that will apply to all Modules placed inside this folder.<br />
            <br />
            This is an optional step and you are not required to add any questions. If you don&#39;t want to add any questions to this Folder simply click &lt;Next&gt; at the bottom. If you would like to add a set of standard questions to this Folder we need to know what questions you would like your users to answer.
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlModuleStepInfo2">
            Ok, now that we have the basics down, lets start working on what makes your new
        <asp:Label runat="server" ID="lblModuleName" />
            Module work.<br />
            <br />
            To start gathering information from your users we need to know what questions you would like them to answer.<br />
            <br />
            <span style="font-style: italic">Please note that all Modules contain standard questions including Customer Account Number, Technician, Technician Comments, and Status (Open, Pending, Completed).</span><br />
        </asp:Panel>
        <br />
        Click the &lt;Add Question&gt; button to add this question to the list and start another one.
        <br />
        <br />
        Add as many questions as you like ... There&#39;s no limit to what you can do!
        <br />
        <br />
        If you made a mistake on a question that you&#39;ve already added simply click the red trash can next to it in the list and it will be removed.<br />
        <br />
        When you&#39;re finished, hit the &lt;Next&gt; button at the bottom to continue.
        <br />
    </asp:Panel>
    <br />
    <asp:Panel ID="pnlStepInfo3" runat="server">
        You&#39;re Done!
        <br />
    </asp:Panel>
    <asp:Panel ID="pnlStepInfo4" runat="server">
        Step 4 Info
    </asp:Panel>
    <asp:Panel ID="pnlStepInfo5" runat="server">
        Step 5 Info
    </asp:Panel>
    <asp:Panel ID="pnlStepInfo6" runat="server">
        Step 6 Info
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="wizardContent_OutsideAjax" runat="server">
</asp:Content>
