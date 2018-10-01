<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/masterPages/Wizard_V2.master" CodeBehind="ModuleWizard.aspx.vb" Inherits="UtilityWizards.Builder.ModuleWizard" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ MasterType VirtualPath="~/masterPages/Wizard_V2.Master" %>

<%@ Register Assembly="UtilityWizards.CommonCore.Shared" Namespace="UtilityWizards.CommonCore.Shared.Controls.TextBoxes" TagPrefix="Builder" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/controls/ModuleView.ascx" TagPrefix="uw" TagName="ModuleView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="wizardHeadContent" runat="server">
    <style type="text/css">
        .wrap-table-wizard {
            width: 100%;
        }

        .rbLinkButton.fixedWidth {
            padding: 0;
        }

        .modalBackground {
            background-color: Black;
            filter: alpha(opacity=90);
            opacity: 0.8;
        }

        .modalPopup {
            background-color: #FFFFFF;
            border-width: 3px;
            border-style: solid;
            border-color: black;
            padding-top: 10px;
            padding-left: 10px;
            width: 300px;
            height: 140px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="wizardMenuContent" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="wizardBreadcrumbContent" runat="server">
    <h1>Dashboard
        <small>Version 2.2 <asp:Label runat="server" ID="lblSandbox" /></small>
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
                    <asp:Image ID="imgHelp_Preview" runat="server" ImageUrl="~/images/toolbar/icon_help.png" ToolTip="This table displays the fields you have added as they will appear on the Module screen." />
                </td>
                <td style="vertical-align: top;">
                    <table class="nav-justified">
                        <tr>
                            <td>Module Fields Preview</td>
                            <td style="width: 100px; text-align: center; background-color: #E4E4E4;">Left</td>
                            <td style="width: 100px; text-align: center; background-color: #C0C0C0;">Middle</td>
                            <td style="width: 100px; text-align: center; background-color: #808080;">Right</td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table class="wrap-table-wizard">
            <tr>
                <td style="width: 25px;"></td>
                <td style="border: 1px solid #C0C0C0; vertical-align: top; background-color: #E6E6E6;">
                    <uw:ModuleView runat="server" ID="ModuleView1" Mode="Edit" />
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
            <td colspan="4">&nbsp;</td>
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
        <asp:Label runat="server" ID="lblClientID" />
        <asp:Label ID="lblUserID" runat="server" />
        <asp:Label ID="lblID" runat="server" />
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="wizardInfoContent" runat="server">
    <table class="wrap-table-wizard">
        <tr>
            <td>
                <telerik:RadButton ID="btnAddNewQuestion" runat="server" ButtonType="LinkButton" CssClass="fixedWidth" Skin="Metro" Text="Add New Field" Width="100%" />
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
        </tr>
    </table>
    <asp:Panel runat="server" ID="pnlQuestionEditor" Visible="false">
        <telerik:RadPanelBar runat="server" ID="pbProperties" Width="100%" Skin="Metro">
            <Items>
                <telerik:RadPanelItem runat="server" Text="Definition">
                    <ContentTemplate>
                        <table class="wrap-table-wizard">
                            <tr>
                                <td style="width: 100px;">Display Text</td>
                                <td>
                                    <telerik:RadTextBox ID="txtQuestion" runat="server" Width="90%" Skin="Metro" />
                                    <asp:Label ID="lblDataField" runat="server"></asp:Label></td>
                            </tr>
                            <tr>
                                <td style="width: 100px;">Type of Control</td>
                                <td>
                                    <asp:DropDownList ID="ddlQuestionType" runat="server" Width="100%" AutoPostBack="true">
                                        <asp:ListItem Text="Check Box" Value="0" />
                                        <asp:ListItem Text="Drop-Down List" Value="1" />
                                        <asp:ListItem Text="Text Box" Value="2" />
                                        <asp:ListItem Text="Memo Field" Value="3" />
                                        <asp:ListItem Text="Numeric Text Field" Value="4" />
                                        <asp:ListItem Text="Currency Text Field" Value="5" />
                                    </asp:DropDownList></td>
                            </tr>
                        </table>
                        <table class="wrap-table-wizard">
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </telerik:RadPanelItem>
                <telerik:RadPanelItem runat="server" Text="Appearance">
                    <ContentTemplate>
                        <table class="wrap-table-wizard">
                            <tr>
                                <td style="width: 100px;">Visible</td>
                                <td>
                                    <asp:CheckBox ID="chkDisplay" runat="server" /></td>
                            </tr>
                            <tr>
                                <td style="width: 100px;">Location</td>
                                <td>
                                    <asp:DropDownList ID="ddlDisplay" runat="server" Width="100%">
                                        <asp:ListItem Text="Top-Left" Value="0" />
                                        <asp:ListItem Text="Top-Middle" Value="1" />
                                        <asp:ListItem Text="Top-Right" Value="2" />
                                        <asp:ListItem Text="Full Page" Value="3" />
                                        <asp:ListItem Text="Bottom-Left" Value="4" />
                                        <asp:ListItem Text="Bottom-Middle" Value="5" />
                                        <asp:ListItem Text="Bottom-Right" Value="6" />
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td style="width: 100px;">Sort Order</td>
                                <td>
                                    <telerik:RadTextBox runat="server" ID="txtSort" /></td>
                            </tr>
                        </table>
                        <asp:Panel runat="server" ID="pnlTextBoxAppearanceOptions">
                            <table class="wrap-table-wizard">
                                <tr>
                                    <td style="width: 100px;">Size</td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlTextBoxSize">
                                            <asp:ListItem Text="X-Small" Value="50" />
                                            <asp:ListItem Text="Small" Value="100" />
                                            <asp:ListItem Text="Medium" Value="200" />
                                            <asp:ListItem Text="Large" Value="300" />
                                        </asp:DropDownList></td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel runat="server" ID="pnlPlainTextBoxAppearanceOptions">
                            <table class="wrap-table-wizard">
                                <tr>
                                    <td style="width: 100px;">Display As Date</td>
                                    <td>
                                        <asp:CheckBox runat="server" ID="chkDisplayAsDate" /></td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel runat="server" ID="pnlNumericTextBoxAppearanceOptions">
                            <table class="wrap-table-wizard">
                                <tr>
                                    <td style="width: 100px;">Decimal Places</td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtNumbersAfterComma" Width="50px" /></td>
                                </tr>
                                <tr>
                                    <td style="width: 100px;">Thousands Separator</td>
                                    <td>
                                        <asp:CheckBox runat="server" ID="chkThousandsSeparator" /></td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel runat="server" ID="pnlMemoTextBoxAppearanceOptions">
                            <table class="wrap-table-wizard">
                                <tr>
                                    <td style="width: 100px;">Rows</td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtMemoRows" Width="50px" /></td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel runat="server" ID="pnlDropDownAppearanceOptions">
                            <table class="wrap-table-wizard">
                                <tr>
                                    <td style="width: 100px;">Size</td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlDropDownSize">
                                            <asp:ListItem Text="Auto" Value="0" />
                                            <asp:ListItem Text="Small" Value="100" />
                                            <asp:ListItem Text="Medium" Value="200" />
                                            <asp:ListItem Text="Large" Value="300" />
                                        </asp:DropDownList></td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <table class="wrap-table-wizard">
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </telerik:RadPanelItem>
                <telerik:RadPanelItem runat="server" Text="Data">
                    <ContentTemplate>
                        <table class="wrap-table-wizard">
                            <tr>
                                <td style="width: 100px;">Binding Type</td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlBindingType" AutoPostBack="true">
                                        <asp:ListItem Text="User Input" Value="0" />
                                        <asp:ListItem Text="Master Feed" Value="1" />
                                    </asp:DropDownList></td>
                            </tr>
                        </table>
                        <asp:Panel runat="server" ID="pnlMasterFeedField">
                            <table class="wrap-table-wizard">
                                <tr>
                                    <td style="font-weight: bold;">NOTE: Fields bound to the Master Feed table are automatically flagged as Read-Only</td>
                                </tr>
                                <tr>
                                    <td>Master Feed Field</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlMasterFeedField" /></td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel runat="server" ID="pnlNumericFormulaField">
                            <table class="wrap-table-wizard">
                                <tr>
                                    <td colspan="2" style="font-weight: bold;">NOTE: Fields bound to Formulas are automatically flagged as Read-Only</td>
                                </tr>
                                <tr>
                                    <td colspan="2">Formula</td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:TextBox runat="server" ID="txtNumericFormula" TextMode="MultiLine" Rows="5" Width="100%" /></td>
                                </tr>
                                <tr>
                                    <td colspan="2">Master Feed Field</td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:DropDownList runat="server" ID="ddlMasterFeedFieldNumericFormula" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadButton ID="btnAddFieldToNumericFormula" runat="server" ButtonType="LinkButton" CssClass="fixedWidth" Skin="Metro" Text="Add Field" Width="100%" />
                                    </td>
                                    <td>
                                        <telerik:RadButton ID="btnEvalNumericFormula" runat="server" ButtonType="LinkButton" CssClass="fixedWidth" Skin="Metro" Text="Test Formula" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel runat="server" ID="pnlTextFormulaField">
                            <table class="wrap-table-wizard">
                                <tr>
                                    <td style="font-weight: bold;">NOTE: Fields bound to Formulas are automatically flagged as Read-Only</td>
                                </tr>
                                <tr>
                                    <td>String Concatenation</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtTextFormula" TextMode="MultiLine" Rows="5" Width="100%" /></td>
                                </tr>
                                <tr>
                                    <td>Master Feed Field</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlMasterFeedFieldTextFormula" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadButton ID="btnAddFieldToTextFormula" runat="server" ButtonType="LinkButton" CssClass="fixedWidth" Skin="Metro" Text="Add Field" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel runat="server" ID="pnlTextBoxDataOptions">
                            <table class="wrap-table-wizard">
                                <tr>
                                    <td style="width: 100px;"></td>
                                    <td></td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel runat="server" ID="pnlDropDownDataOptions">
                            <table class="wrap-table-wizard">
                                <tr>
                                    <td style="width: 100px; vertical-align: top;">Values</td>
                                    <td>
                                        <table class="nav-justified">
                                            <tr>
                                                <td>Display</td>
                                                <td>&nbsp;</td>
                                                <td>Value</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <telerik:RadTextBox ID="txtDDLText" runat="server" Skin="Metro" Width="100%" /></td>
                                                <td style="text-align: center;">=</td>
                                                <td>
                                                    <telerik:RadTextBox ID="txtDDLValue" runat="server" Skin="Metro" Width="100%" /></td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <telerik:RadButton ID="btnAddValue" runat="server" Text="Add To List" ButtonType="LinkButton" Skin="Metro" Width="100%" CssClass="fixedWidth" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:ListBox ID="lstValues" runat="server" Width="100%"></asp:ListBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <telerik:RadButton ID="btnRemoveValue" runat="server" Text="Remove" ButtonType="LinkButton" Skin="Metro" Width="100%" CssClass="fixedWidth" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <table class="wrap-table-wizard">
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </telerik:RadPanelItem>
                <telerik:RadPanelItem runat="server" Text="Miscellaneous">
                    <ContentTemplate>
                        <table class="wrap-table-wizard">
                            <tr>
                                <td style="width: 100px;">Required</td>
                                <td>
                                    <asp:CheckBox ID="chkRequired" runat="server" /></td>
                            </tr>
                            <tr>
                                <td style="width: 100px;">Allow Reporting</td>
                                <td>
                                    <asp:CheckBox ID="chkReporting" runat="server" /></td>
                            </tr>
                            <tr>
                                <td style="width: 100px;">Allow Export</td>
                                <td>
                                    <asp:CheckBox ID="chkExport" runat="server" /></td>
                            </tr>
                        </table>
                        <table class="wrap-table-wizard">
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </telerik:RadPanelItem>
            </Items>
        </telerik:RadPanelBar>
        <table class="wrap-table-wizard">
            <tr>
                <td style="width: 25px;"></td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td style="width: 25px;">&nbsp;</td>
                <td style="vertical-align: top;">
                    <table class="wrap-table-wizard">
                        <tr>
                            <td>
                                <telerik:RadButton ID="btnAdd" runat="server" ButtonType="LinkButton" CssClass="fixedWidth" Skin="Metro" Text="Save Changes" Width="100%" />
                            </td>
                            <td>
                                <telerik:RadButton ID="btnDelete" runat="server" ButtonType="LinkButton" CssClass="fixedWidth" Skin="Metro" Text="Delete This Field" Width="100%" />
                            </td>
                            <td>
                                <telerik:RadButton ID="btnCancelEditor" runat="server" ButtonType="LinkButton" CssClass="fixedWidth" Skin="Metro" Text="Cancel" Width="100%" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="width: 25px;"></td>
                <td>&nbsp;&nbsp;</td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="wizardContent_OutsideAjax" runat="server">
</asp:Content>
