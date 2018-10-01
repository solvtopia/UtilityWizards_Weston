<%@ Page Title="Report Editor" Language="vb" AutoEventWireup="false" MasterPageFile="~/masterPages/Wizard_V2.master" CodeBehind="ReportEditor.aspx.vb" Inherits="UtilityWizards.Builder.ReportEditor" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ MasterType VirtualPath="~/masterPages/Wizard_V2.Master" %>

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
        <li class="header">THIS REPORT</li>
        <li runat="server" id="liPrintReport">
            <asp:LinkButton runat="server" ID="lnkPrintReport"><i class="fa fa-print"></i><span>Print Preview</span></asp:LinkButton>
        </li>
        <li runat="server" id="liDeleteReport">
            <asp:LinkButton runat="server" ID="lnkDeleteReport"><i class="fa fa-trash-o"></i><span>Delete Report</span></asp:LinkButton>
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
        <li><a href="Reports.aspx">System Reports</a></li>
        <li class="active">Report Editor</li>
    </ol>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="wizardContent_Ajax" runat="server">
    <asp:Panel runat="server" ID="pnlStep1">
        <table class="wrap-table">
            <tr>
                <td>Name</td>
                <td style="text-align: right; vertical-align: top;">
                    <asp:Image ID="imgHelp_Name" runat="server" ImageUrl="~/images/toolbar/icon_help.png" ToolTip="This is the name that will appear on the dashboard (i.e. Water, Sewer, Sanitation, etc.)" />
                </td>
                <td style="width: 80%">
                    <telerik:RadTextBox ID="txtName" runat="server" Width="250px" Skin="Metro">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td style="vertical-align: top;">Description</td>
                <td style="text-align: right; vertical-align: top;">
                    <asp:Image ID="imgHelp_Name0" runat="server" ImageUrl="~/images/toolbar/icon_help.png" ToolTip="This is the name that will appear on the dashboard (i.e. Water, Sewer, Sanitation, etc.)" />
                </td>
                <td style="width: 80%">
                    <telerik:RadTextBox ID="txtDescription" runat="server" Rows="3" TextMode="MultiLine" Width="250px" Skin="Metro">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td style="text-align: right; vertical-align: top;">&nbsp;</td>
                <td style="width: 80%">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td style="text-align: right; vertical-align: top;">&nbsp;</td>
                <td style="width: 80%">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td style="text-align: right; vertical-align: top;">&nbsp;</td>
                <td style="width: 80%">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td style="text-align: right; vertical-align: top;">&nbsp;</td>
                <td style="width: 80%">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td style="text-align: right; vertical-align: top;">&nbsp;</td>
                <td style="width: 80%">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td style="text-align: right; vertical-align: top;">&nbsp;</td>
                <td style="width: 80%">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td style="text-align: right; vertical-align: top;">&nbsp;</td>
                <td style="width: 80%">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td style="text-align: right; vertical-align: top;">&nbsp;</td>
                <td style="width: 80%">&nbsp;</td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlStep2">
        <table class="wrap-table">
            <tr>
                <td>Folder</td>
                <td style="text-align: right; vertical-align: top;">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/images/toolbar/icon_help.png" ToolTip="This is the name that will appear on the dashboard (i.e. Water, Sewer, Sanitation, etc.)" />
                </td>
                <td style="width: 80%">
                    <telerik:RadComboBox ID="ddlFolder" runat="server" Width="350px" AutoPostBack="True" Skin="Metro">
                    </telerik:RadComboBox>
                </td>
            </tr>
            <tr>
                <td>Module</td>
                <td style="text-align: right; vertical-align: top;">
                    <asp:Image ID="imgHelp_Name1" runat="server" ImageUrl="~/images/toolbar/icon_help.png" ToolTip="This is the name that will appear on the dashboard (i.e. Water, Sewer, Sanitation, etc.)" />
                </td>
                <td style="width: 80%">
                    <telerik:RadComboBox ID="ddlModule" runat="server" Width="350px" AutoPostBack="True" Skin="Metro">
                    </telerik:RadComboBox>
                </td>
            </tr>
            <tr>
                <td style="vertical-align: top;">Questions</td>
                <td style="text-align: right; vertical-align: top;">
                    <asp:Image ID="imgHelp_Name2" runat="server" ImageUrl="~/images/toolbar/icon_help.png" ToolTip="This is the name that will appear on the dashboard (i.e. Water, Sewer, Sanitation, etc.)" />
                </td>
                <td style="width: 80%">
                    <table>
                        <tr>
                            <td>
                                <telerik:RadComboBox ID="ddlQuestion" runat="server" Width="350px" Skin="Metro">
                                </telerik:RadComboBox>
                            </td>
                            <td>
                                <telerik:RadButton ID="btnAddQuestion" runat="server" Text="Add To List" Skin="Metro" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:ListBox ID="lstValues" runat="server" Rows="6" Width="350px"></asp:ListBox>
                            </td>
                            <td style="vertical-align: top;">
                                <telerik:RadButton ID="btnRemoveQuestion" runat="server" Text="Remove" Skin="Metro" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td style="text-align: right; vertical-align: top;">&nbsp;</td>
                <td style="width: 80%">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td style="text-align: right; vertical-align: top;">&nbsp;</td>
                <td style="width: 80%">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td style="text-align: right; vertical-align: top;">&nbsp;</td>
                <td style="width: 80%">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td style="text-align: right; vertical-align: top;">&nbsp;</td>
                <td style="width: 80%">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td style="text-align: right; vertical-align: top;">&nbsp;</td>
                <td style="width: 80%">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td style="text-align: right; vertical-align: top;">&nbsp;</td>
                <td style="width: 80%">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td style="text-align: right; vertical-align: top;">&nbsp;</td>
                <td style="width: 80%">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td style="text-align: right; vertical-align: top;">&nbsp;</td>
                <td style="width: 80%">&nbsp;</td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlStep3">
        <%--<asp:Panel ID="pnlScrollGrid" runat="server">--%>
        <telerik:RadGrid ID="RadReportGrid" runat="server" AllowFilteringByColumn="True" AllowSorting="True" GroupPanelPosition="Top" DataSourceID="SqlDataSource1" Skin="Metro">
            <MasterTableView DataSourceID="SqlDataSource1">
            </MasterTableView>
        </telerik:RadGrid>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:UtilityWizardsConnectionString %>" SelectCommand="procReportGrid" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:ControlParameter ControlID="lblFields" DefaultValue="*" Name="fields" PropertyName="Text" Type="String" />
                <asp:ControlParameter ControlID="ddlModule" DefaultValue="0" Name="moduleID" PropertyName="SelectedValue" Type="Int32" />
            </SelectParameters>
        </asp:SqlDataSource>
        <%--</asp:Panel>--%>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlStep4">
        <table class="wrap-table">
            <tr>
                <td colspan="3">Congratulations! You&#39;ve entered all the information we need to create your new
                    <asp:Label ID="lblReportName" runat="server" />
                    &nbsp;Report.<br />
                    <br />
                    Click the &lt;Finish&gt; button below to save your new Report and start using it.</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td style="text-align: right; vertical-align: top;">&nbsp;</td>
                <td style="width: 80%">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td style="text-align: right; vertical-align: top;">&nbsp;</td>
                <td style="width: 80%">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td style="text-align: right; vertical-align: top;">&nbsp;</td>
                <td style="width: 80%">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td style="text-align: right; vertical-align: top;">&nbsp;</td>
                <td style="width: 80%">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td style="text-align: right; vertical-align: top;">&nbsp;</td>
                <td style="width: 80%">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td style="text-align: right; vertical-align: top;">&nbsp;</td>
                <td style="width: 80%">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td style="text-align: right; vertical-align: top;">&nbsp;</td>
                <td style="width: 80%">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td style="text-align: right; vertical-align: top;">&nbsp;</td>
                <td style="width: 80%">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td style="text-align: right; vertical-align: top;">&nbsp;</td>
                <td style="width: 80%">&nbsp;</td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlStep5">
        <table class="wrap-table">
            <tr>
                <td>Step 5</td>
                <td style="text-align: right; vertical-align: top;">&nbsp;</td>
                <td style="width: 80%">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td style="text-align: right; vertical-align: top;">&nbsp;</td>
                <td style="width: 80%">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td style="text-align: right; vertical-align: top;">&nbsp;</td>
                <td style="width: 80%">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td style="text-align: right; vertical-align: top;">&nbsp;</td>
                <td style="width: 80%">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td style="text-align: right; vertical-align: top;">&nbsp;</td>
                <td style="width: 80%">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td style="text-align: right; vertical-align: top;">&nbsp;</td>
                <td style="width: 80%">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td style="text-align: right; vertical-align: top;">&nbsp;</td>
                <td style="width: 80%">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td style="text-align: right; vertical-align: top;">&nbsp;</td>
                <td style="width: 80%">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td style="text-align: right; vertical-align: top;">&nbsp;</td>
                <td style="width: 80%">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td style="text-align: right; vertical-align: top;">&nbsp;</td>
                <td style="width: 80%">&nbsp;</td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlStep6">
        <table class="wrap-table">
            <tr>
                <td>Step 6</td>
                <td style="text-align: right; vertical-align: top;">&nbsp;</td>
                <td style="width: 80%">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td style="text-align: right; vertical-align: top;">&nbsp;</td>
                <td style="width: 80%">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td style="text-align: right; vertical-align: top;">&nbsp;</td>
                <td style="width: 80%">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td style="text-align: right; vertical-align: top;">&nbsp;</td>
                <td style="width: 80%">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td style="text-align: right; vertical-align: top;">&nbsp;</td>
                <td style="width: 80%">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td style="text-align: right; vertical-align: top;">&nbsp;</td>
                <td style="width: 80%">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td style="text-align: right; vertical-align: top;">&nbsp;</td>
                <td style="width: 80%">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td style="text-align: right; vertical-align: top;">&nbsp;</td>
                <td style="width: 80%">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td style="text-align: right; vertical-align: top;">&nbsp;</td>
                <td style="width: 80%">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td style="text-align: right; vertical-align: top;">&nbsp;</td>
                <td style="width: 80%">&nbsp;</td>
            </tr>
        </table>
    </asp:Panel>
    <table>
        <tr>
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
        <asp:Label runat="server" ID="lblFields" />
        <asp:Label ID="lblID" runat="server" />
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="wizardInfoContent" runat="server">
    <asp:Panel ID="pnlStepInfo1" runat="server">
        Lets start by defining the basics of your Module.<br />
        <br />
        When you&#39;re finished, hit the &lt;Next&gt; button at the bottom to continue.
    </asp:Panel>
    <asp:Panel ID="pnlStepInfo2" runat="server">
        Now lets choose the fields you would like to show on your Report.&nbsp; You can add as many fields as you want from any module in your system.<br />
        <br />
        Please note that you can only choose from questions that have been flagged as Reportable while configuring modules.<br />
        <br />
        When you&#39;re finished, hit the &lt;Next&gt; button at the bottom to continue.
    </asp:Panel>
    <asp:Panel ID="pnlStepInfo3" runat="server">
        Check out your new Report!<br />
        <br />
        If you don&#39;t like what you see, hit the &lt;Back&gt; button at the bottom to change it.<br />
        <br />
        If you like what you&#39;ve created then You&#39;re Done! Click &lt;Next&gt; to complete the wizard.
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
