<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/masterPages/ContentPage.Master" CodeBehind="Search.aspx.vb" Inherits="UtilityWizards.Builder.Search" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ MasterType VirtualPath="~/masterPages/ContentPage.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
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
<asp:Content ID="Content4" ContentPlaceHolderID="menuContent" runat="server">
    <ul class="sidebar-menu">
        <li class="header">WORK ORDER OPTIONS</li>
        <li>
            <asp:LinkButton runat="server" ID="lnkNew"><i class="fa fa-file-text-o"></i><span>New Work Order</span></asp:LinkButton></li>
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
    <table style="width: 100%;">
        <tr>
            <td>
                <table class="nav-justified">
                    <tr>
                        <td>
                            <telerik:RadButton ID="btnNewWorkOrder" runat="server" ButtonType="LinkButton" CssClass="fixedWidth" Skin="Metro" Text="New Work Order" Width="100%" />
                        </td>
                        <td style="text-align: right;">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;</td>
                        <td style="text-align: right;">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox runat="server" ID="chkShowCompleted" Text="Show Completed" AutoPostBack="true" />
                        </td>
                        <td style="text-align: right;">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;</td>
                        <td style="text-align: right;">
                            &nbsp;</td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <telerik:RadGrid ID="RadSearchGrid" runat="server" AllowSorting="True" GroupPanelPosition="Top" AutoGenerateEditColumn="True" Skin="Metro">
                </telerik:RadGrid>
                <telerik:RadGrid ID="RadSearchGridMobile" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1" GroupPanelPosition="Top" Skin="MetroTouch">
                    <ClientSettings Selecting-AllowRowSelect="true" EnablePostBackOnRowClick="true">
                    </ClientSettings>
                    <MasterTableView DataKeyNames="ID" DataSourceID="SqlDataSource1" ShowHeader="false">
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Work Order Data" UniqueName="TemplateColumn">
                                <ItemTemplate>
                                    <table style="width: 100%;">
                                        <tr>
                                            <td style="vertical-align: top; width: 55px;">
                                                <asp:Image ID='imgIcon' runat='server' Height='45px' Width='45px' ImageUrl='<%# Eval("ModuleIcon") %>' />
                                            </td>
                                            <td style="vertical-align: top;">
                                                <table style="width: 100%;">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID='lblModule' runat='server' Font-Bold='True' ForeColor='White' Text='<%# Eval("ModuleName") %>'></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID='Label1' runat='server' ForeColor='White' Text='Work Order #'></asp:Label><asp:Label ID='lblWorkOrderID' runat='server' ForeColor='White' Text='<%# Eval("ID") %>'></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID='lblDescription' runat='server' ForeColor='White' Text='<%# Eval("CustomerAddress") %>'></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID='lblAssignedTo' runat='server' ForeColor='White' Text='<%# Eval("AssignedToName") %>'></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn HeaderText="Priority" UniqueName="Priority" DataField="xPriority" Display="false" />
                            <telerik:GridBoundColumn HeaderText="ID" UniqueName="ID" DataField="ID" Display="false" />
                            <telerik:GridBoundColumn HeaderText="ModuleID" UniqueName="xModuleID" DataField="xModuleID" Display="false" />
                            <telerik:GridBoundColumn HeaderText="FolderID" UniqueName="xFolderID" DataField="xFolderID" Display="false" />
                            <telerik:GridBoundColumn HeaderText="SupervisorID" UniqueName="SupervisorID" DataField="SupervisorID" Display="false" />
                            <telerik:GridBoundColumn HeaderText="TechnicianID" UniqueName="TechnicianID" DataField="TechnicianID" Display="false" />
                            <telerik:GridBoundColumn HeaderText="xCustAcctNum" UniqueName="xCustAcctNum" DataField="xCustAcctNum" Display="false" />
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:UtilityWizardsConnectionString %>"></asp:SqlDataSource>
            </td>
        </tr>
    </table>
    <asp:Panel runat="server" ID="pnlHidden" Visible="false" BackColor="#CC0000">
        <asp:Label ID="lblFields" runat="server"></asp:Label>
        <asp:Label ID="lblClientID" runat="server"></asp:Label>
        <asp:Label ID="lblModuleID" runat="server"></asp:Label>
        <asp:Label ID="lblCustAcctNum" runat="server"></asp:Label>
        <asp:Label ID="lblFilterID" runat="server"></asp:Label>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="pageContent_OutsideAjax" runat="server">
</asp:Content>
