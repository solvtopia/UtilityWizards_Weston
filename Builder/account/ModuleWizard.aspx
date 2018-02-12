<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/masterPages/Wizard_V2.master" CodeBehind="ModuleWizard.aspx.vb" Inherits="UtilityWizards.Builder.ModuleWizard" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ MasterType VirtualPath="~/masterPages/Wizard_V2.Master" %>

<%@ Register Assembly="UtilityWizards.CommonCore.Shared" Namespace="UtilityWizards.CommonCore.Shared.Controls.TextBoxes" TagPrefix="Builder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="wizardHeadContent" runat="server">
    <style type="text/css">
        .wrap-table-wizard {
            width: 100%;
        }

        .rbLinkButton.fixedWidth {
            padding: 0;
        }
    </style>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="wizardMenuContent" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="wizardBreadcrumbContent" runat="server">
    <h1>Dashboard
        <small>Version 2.2</small>
    </h1>
    <ol class="breadcrumb">
        <li><a href="..Default.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
        <li><a href="#">System Modules</a></li>
        <li class="active">Module & Folder Wizard</li>
    </ol>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="wizardContent_Ajax" runat="server">
    <asp:Panel runat="server" ID="pnlStep1">
        <table class="wrap-table-wizard">
            <tr>
                <td style="width: 25px;">
                    <asp:Image ID="imgHelp_Name" runat="server" ImageUrl="~/images/toolbar/icon_help.png" ToolTip="This is the name that will appear on the dashboard (i.e. Water, Sewer, Sanitation, etc.)" />
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
                    <asp:Image ID="imgHelp_Desc" runat="server" ImageUrl="~/images/toolbar/icon_help.png" ToolTip="Add an optional Description if you want" />
                </td>
                <td>Description</td>
            </tr>
            <tr>
                <td style="width: 25px;">&nbsp;</td>
                <td>
                    <telerik:RadTextBox ID="txtDescription" runat="server" Rows="3" Skin="Metro" TextMode="MultiLine" Width="100%">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 25px;">&nbsp;</td>
                <td>Supervisor</td>
            </tr>
            <tr>
                <td style="width: 25px;">&nbsp;</td>
                <td>
                    <telerik:RadComboBox ID="ddlSupervisor" runat="server" Skin="Metro" Width="100%">
                    </telerik:RadComboBox>
                </td>
            </tr>
        </table>
        <asp:Panel runat="server" ID="pnlModuleStep1Questions">
            <table class="wrap-table-wizard">
                <tr>
                    <td style="width: 25px;">
                        <asp:Image ID="imgHelp_Folder" runat="server" ImageUrl="~/images/toolbar/icon_help.png" ToolTip="Place this Module in a folder or on the dashboard" />
                    </td>
                    <td>Folder</td>
                </tr>
                <tr>
                    <td style="width: 25px;">&nbsp;</td>
                    <td>
                        <telerik:RadComboBox ID="ddlFolder" runat="server" Skin="Metro" Width="100%">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 25px;">
                        <asp:Image ID="imgHelp_Icon" runat="server" ImageUrl="~/images/toolbar/icon_help.png" ToolTip="Choose an Icon from our collection to show on the dashboard. Your selected icon will display below." />
                    </td>
                    <td>Icon</td>
                </tr>
                <tr>
                    <td style="width: 25px;">&nbsp;</td>
                    <td>
                        <telerik:RadComboBox ID="ddlIcon" runat="server" AutoPostBack="True" Skin="Metro" Width="100%">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 25px;">&nbsp;</td>
                    <td>Selected Icon</td>
                </tr>
                <tr>
                    <td style="width: 25px;">&nbsp;</td>
                    <td>
                        <asp:Image ID="imgIcon" runat="server" Height="64px" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlStep2">
        <table class="wrap-table-wizard">
            <tr>
                <td style="width: 25px;">
                    <asp:Image ID="imgHelp_Question" runat="server" ImageUrl="~/images/toolbar/icon_help.png" ToolTip="Enter the wording for your question" />
                </td>
                <td>Question</td>
            </tr>
            <tr>
                <td style="width: 25px;"></td>
                <td>
                    <telerik:RadComboBox ID="ddlQuestion" runat="server" AllowCustomText="True" Width="100%" Skin="Metro">
                    </telerik:RadComboBox>
                    <asp:Label ID="lblDataField" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 25px;">
                    <asp:Image ID="imgHelp_Type" runat="server" ImageUrl="~/images/toolbar/icon_help.png" ToolTip="What Type of question is this? Options for this include Check Boxes (Yes/No), Drop-Down Lists (Multiple Controlled Responses), Text Boxes (Single-Line data entry with a maximum of 255 characters), Memo Fields (Multi-Line data entry with no maximum of characters)" />
                </td>
                <td>Type</td>
            </tr>
            <tr>
                <td style="width: 25px;"></td>
                <td>
                    <telerik:RadComboBox ID="ddlQuestionType" runat="server" Width="100%" Skin="Metro" AutoPostBack="True">
                        <Items>
                            <telerik:RadComboBoxItem Text="Check Box" Value="0" />
                            <telerik:RadComboBoxItem Text="Drop-Down List" Value="1" />
                            <telerik:RadComboBoxItem Text="Text Box" Value="2" />
                            <telerik:RadComboBoxItem Text="Memo Field" Value="3" />
                            <telerik:RadComboBoxItem Text="Numeric Text Field" Value="4" />
                        </Items>
                    </telerik:RadComboBox>
                </td>
            </tr>
            <tr>
                <td style="width: 25px;">
                    <asp:Image ID="imgHelp_Required" runat="server" ImageUrl="~/images/toolbar/icon_help.png" ToolTip="Is the user required to answer this question?" />
                </td>
                <td>Required</td>
            </tr>
            <tr>
                <td style="width: 25px;"></td>
                <td>
                    <asp:CheckBox ID="chkRequired" runat="server" />
                </td>
            </tr>
            <tr>
                <td style="width: 25px;">
                    <asp:Image ID="imgHelp_Search" runat="server" ImageUrl="~/images/toolbar/icon_help.png" ToolTip="Do you want to show this field in the Search grid?" />
                </td>
                <td>Include on Search Grid</td>
            </tr>
            <tr>
                <td style="width: 25px;"></td>
                <td>
                    <asp:CheckBox ID="chkSearch" runat="server" />
                </td>
            </tr>
            <tr>
                <td style="width: 25px;">
                    <asp:Image ID="imgHelp_Reporting" runat="server" ImageUrl="~/images/toolbar/icon_help.png" ToolTip="Do you want to allow reporting on this field?" />
                </td>
                <td>Allow Reporting</td>
            </tr>
            <tr>
                <td style="width: 25px;"></td>
                <td>
                    <asp:CheckBox ID="chkReporting" runat="server" />
                </td>
            </tr>
            <tr>
                <td style="width: 25px;">
                    <asp:Image ID="imgHelp_Export" runat="server" ImageUrl="~/images/toolbar/icon_help.png" ToolTip="Can this field be exported for input into other 3rd Party programs?" />
                </td>
                <td>Can be Exported</td>
            </tr>
            <tr>
                <td style="width: 25px;"></td>
                <td>
                    <asp:CheckBox ID="chkExport" runat="server" />
                </td>
            </tr>
            <tr>
                <td style="width: 25px;">&nbsp;</td>
                <td>Allow Mobile Changes</td>
            </tr>
            <tr>
                <td style="width: 25px;"></td>
                <td>
                    <asp:CheckBox ID="chkMobile" runat="server" />
                </td>
            </tr>
        </table>
        <asp:Panel runat="server" ID="pnlDropDownValues">
            <table class="wrap-table-wizard">
                <tr>
                    <td style="width: 25px;">&nbsp;</td>
                    <td style="vertical-align: top;">&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 25px;">
                        <asp:Image ID="imgHelp_Values" runat="server" ImageUrl="~/images/toolbar/icon_help.png" ToolTip="If you've selected Drop-Down List as the type for your question specify the values you would like to appear in the list." />
                    </td>
                    <td style="vertical-align: top;">Values</td>
                </tr>
                <tr>
                    <td style="width: 25px;"></td>
                    <td>
                        <table class="nav-justified">
                            <tr>
                                <td>
                                    <telerik:RadTextBox ID="txtDDLValue" runat="server" Skin="Metro" Width="100%">
                                    </telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadButton ID="btnAddValue" runat="server" Text="Add To List" ButtonType="LinkButton" Skin="Metro" Width="100%" CssClass="fixedWidth" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:ListBox ID="lstValues" runat="server" Width="100%"></asp:ListBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadButton ID="btnRemoveValue" runat="server" Text="Remove" ButtonType="LinkButton" Skin="Metro" Width="100%" CssClass="fixedWidth" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="width: 25px;">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
            </table>
        </asp:Panel>
        <table class="wrap-table-wizard">
            <tr>
                <td style="width: 25px;"></td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td style="width: 25px;">&nbsp;</td>
                <td style="vertical-align: top;">
                    <telerik:RadButton ID="btnAdd" runat="server" ButtonType="LinkButton" CssClass="fixedWidth" Skin="Metro" Text="Add Question" Width="100%" />
                </td>
            </tr>
            <tr>
                <td style="width: 25px;"></td>
                <td>&nbsp;&nbsp;</td>
            </tr>
            <tr>
                <td style="width: 25px;">
                    <asp:Image ID="imgHelp_Preview" runat="server" ImageUrl="~/images/toolbar/icon_help.png" ToolTip="This table displays the questions you have added as they will appear on the Module screen." />
                </td>
                <td style="vertical-align: top;">Module Questions Preview</td>
            </tr>
            <tr>
                <td style="width: 25px;"></td>
                <td style="border: 1px solid #C0C0C0; vertical-align: top; background-color: #E6E6E6;">
                    <asp:Table ID="tblQuestions" runat="server" CellPadding="1" CellSpacing="2" Width="100%">
                    </asp:Table>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlStep3">
        <table class="wrap-table-wizard">
            <tr>
                <td>Congratulations! You&#39;ve entered all the information we need to create your new
                    <asp:Label ID="lblModuleName1" runat="server" />
                    &nbsp;Module.<br />
                    <br />
                    Click the &lt;Finish&gt; button below to save your new Module and start using it.</td>
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
        <asp:Label runat="server" ID="lblClientID" />
        <asp:Label ID="lblUserID" runat="server" />
        <asp:Label ID="lblID" runat="server" />
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="wizardInfoContent" runat="server">
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
<asp:Content ID="Content4" ContentPlaceHolderID="wizardContent_OutsideAjax" runat="server">
</asp:Content>
