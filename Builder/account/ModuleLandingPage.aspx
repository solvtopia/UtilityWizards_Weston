<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/masterPages/ContentPage.Master" CodeBehind="ModuleLandingPage.aspx.vb" Inherits="UtilityWizards.Builder.ModuleLandingPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ MasterType VirtualPath="~/masterPages/ContentPage.Master" %>

<%@ Register Assembly="UtilityWizards.CommonCore" Namespace="UtilityWizards.CommonCore.Controls.TextBoxes" TagPrefix="Builder" %>
<%@ Register Assembly="UtilityWizards.CommonCore" Namespace="UtilityWizards.CommonCore.Controls.DropDownLists" TagPrefix="Builder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <style type="text/css">
        .wrap-landing-table {
            width: 100%;
            /*display: block;*/
        }

        /*.wrap-landing-table td {
                display: inline-block;
            }*/

        .rbLinkButton.fixedWidth {
            padding: 0;
        }
    </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="menuContent" runat="server">
    <ul class="sidebar-menu">
        <li class="header">MODULE OPTIONS</li>
        <li runat="server" id="liEditModule">
            <asp:LinkButton runat="server" ID="lnkEditModule"><i class="fa fa-pencil"></i><span>Edit Module</span></asp:LinkButton></li>
        <li runat="server" id="liDeleteModule">
            <asp:LinkButton runat="server" ID="lnkDeleteModule"><i class="fa fa-trash-o"></i><span>Delete Module</span></asp:LinkButton></li>
        <li runat="server" id="liCopyModule">
            <asp:LinkButton runat="server" ID="lnkCopyModule"><i class="fa fa-files-o"></i><span>Copy Module</span></asp:LinkButton></li>
        <li runat="server" id="liMoveModule">
            <asp:LinkButton runat="server" ID="lnkMoveModule"><i class="fa fa-stack-overflow"></i><span>Move to Folder</span></asp:LinkButton></li>
    </ul>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="breadcrumbContent" runat="server">
    <h1>Dashboard
        <small>Version 2.2</small>
    </h1>
    <ol class="breadcrumb">
        <li><a href="..Default.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
        <li class="active">System Modules</li>
    </ol>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="pageContent_Ajax" runat="server">
    <table class="wrap-landing-table">
        <tr>
            <td>
                <asp:Panel runat="server" ID="pnlOptions">
                    <table class="wrap-landing-table">
                        <tr>
                            <td style="text-align: center;">What type of transaction are you performing?<br />
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center;">
                                <table class="wrap-landing-table">
                                    <tr>
                                        <td>
                                            <telerik:RadButton ID="btnNew" runat="server" Text="New Transaction" Icon-PrimaryIconUrl="~/images/toolbar/icon_new.png" Skin="Metro" Width="100%" CssClass="fixedWidth" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <telerik:RadButton ID="btnSearch" runat="server" Text="Existing Transaction" Icon-PrimaryIconUrl="~/images/toolbar/icon_search.png" Skin="Metro" Width="100%" CssClass="fixedWidth" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;&nbsp;</td>
                        </tr>
                    </table>
                </asp:Panel>
                <table class="wrap-landing-table">
                    <tr>
                        <td>
                            <asp:Panel ID="pnlNew" runat="server" DefaultButton="btnSearch_New">
                                <table class="wrap-landing-table">
                                    <tr>
                                        <td colspan="2">For new transactions you must first select a customer account to associate this record with.<br />
                                            <br />
                                            You can search for customer accounts by either their billing account number, customer name, or their service address. Use the form below to find the customer record you are creating this transaction for:<br />
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Search Field</td>
                                        <td style="text-align: right;">
                                            <asp:Image ID="imgHelp_AcctNum_New1" runat="server" ImageUrl="~/images/toolbar/icon_help.png" ToolTip="Enter all or part of the customer account number you are looking for." />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <Builder:DropDownList ID="ddlSearchField_New" runat="server" Width="100%">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="Customer Name" Value="name" />
                                                    <telerik:RadComboBoxItem Text="Account Number" Value="acctnum" />
                                                    <telerik:RadComboBoxItem Text="Service Address" Value="address" />
                                                </Items>
                                            </Builder:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Search Value</td>
                                        <td style="text-align: right;">
                                            <asp:Image ID="imgHelp_AcctNum_New0" runat="server" ImageUrl="~/images/toolbar/icon_help.png" ToolTip="Enter all or part of the customer account number you are looking for." />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <telerik:RadTextBox ID="txtSearch_New" runat="server" Skin="Metro" Width="100%">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <telerik:RadButton ID="btnSearch_New" runat="server" CssClass="fixedWidth" Icon-PrimaryIconUrl="~/images/toolbar/icon_search.png" Skin="Metro" Text="Search" Width="100%" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <telerik:RadGrid ID="RadCustomerGrid_New" runat="server" AutoGenerateEditColumn="True" GroupPanelPosition="Top" RenderMode="Auto" Skin="Metro" Width="100%">
                                            </telerik:RadGrid>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnlSearch" runat="server" DefaultButton="btnSearch_Existing">
                                <table class="wrap-landing-table">
                                    <tr>
                                        <td colspan="2">To search for existing transactions in the system enter either the customer account number, customer name associated with the record or the transaction ID.
                                                <br />
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Search Field</td>
                                        <td style="text-align: right;">
                                            <asp:Image ID="Image1" runat="server" ImageUrl="~/images/toolbar/icon_help.png" ToolTip="Enter all or part of the customer account number you are looking for." />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <Builder:DropDownList ID="ddlSearchField_Existing" runat="server" Width="100%">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="Transaction ID" Value="id" />
                                                    <telerik:RadComboBoxItem Text="Customer Name" Value="name" />
                                                    <telerik:RadComboBoxItem Text="Account Number" Value="acctnum" />
                                                </Items>
                                            </Builder:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Search Value</td>
                                        <td style="text-align: right;">
                                            <asp:Image ID="Image2" runat="server" ImageUrl="~/images/toolbar/icon_help.png" ToolTip="Enter all or part of the customer account number you are looking for." />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <telerik:RadTextBox ID="txtSearch_Existing" runat="server" Skin="Metro" Width="100%">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <telerik:RadButton ID="btnSearch_Existing" runat="server" CssClass="fixedWidth" Icon-PrimaryIconUrl="~/images/toolbar/icon_search.png" Skin="Metro" Text="Search" Width="100%" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <telerik:RadButton ID="btnShowAll" runat="server" CssClass="fixedWidth" Icon-PrimaryIconUrl="~/images/toolbar/icon_search.png" Skin="Metro" Text="Show All Transactions" Width="100%" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td>&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td style="vertical-align: top; text-align: right;">
                                            <asp:Image ID="imgHelp_Customers_Existing" runat="server" ImageUrl="~/images/toolbar/icon_help.png" ToolTip="Click on the &lt;Select&gt; link next to the customer you would like to search transactions for." />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <telerik:RadGrid ID="RadCustomerGrid_Existing" runat="server" AutoGenerateEditColumn="True" GroupPanelPosition="Top" RenderMode="Auto" Skin="Metro" Width="100%">
                                            </telerik:RadGrid>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="pageContent_OutsideAjax" runat="server">
</asp:Content>
