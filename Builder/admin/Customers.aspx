<%@ Page Title="System Customers" Language="vb" AutoEventWireup="false" MasterPageFile="~/masterPages/ContentPage.master" CodeBehind="Customers.aspx.vb" Inherits="UtilityWizards.Builder.Customers" %>

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
<asp:Content ID="Content2" ContentPlaceHolderID="menuContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="breadcrumbContent" runat="server">
    <h1>Dashboard
        <small>Version 2.2 <asp:Label runat="server" ID="lblSandbox" /></small>
    </h1>
    <ol class="breadcrumb">
        <li><a href="..Default.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
        <li><a href="#">Admin</a></li>
        <li class="active">System Customers</li>
    </ol>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pageContent_Ajax" runat="server">
    <table cellpadding="1" cellspacing="2">
        <tr>
            <td>Search Type</td>
            <td>&nbsp;</td>
            <td>
                <telerik:RadDropDownList ID="ddlSearch" runat="server" SelectedText="Account Number" SelectedValue="ACCOUNT_NUMBER">
                    <Items>
                        <telerik:DropDownListItem runat="server" Selected="True" Text="Account Number" Value="ACCOUNT_NUMBER" />
                        <telerik:DropDownListItem runat="server" Text="Borrower Name" Value="BORROWER_PRIMARY_NAME" />
                        <telerik:DropDownListItem runat="server" Text="Property Address" Value="PROPERTY_ADDRESS_1" />
                    </Items>
                </telerik:RadDropDownList>
            </td>
        </tr>
        <tr>
            <td>Search Value</td>
            <td>&nbsp;</td>
            <td>
                <telerik:RadTextBox ID="txtSearch" runat="server" Width="300px">
                </telerik:RadTextBox>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>
                <telerik:RadButton ID="btnSearch" runat="server" Text="Search">
                </telerik:RadButton>
            </td>
        </tr>
        <tr>
            <td colspan="3">&nbsp;</td>
        </tr>
    </table>
    <telerik:RadGrid ID="RadCustomerGrid" runat="server" GroupPanelPosition="Top" RenderMode="Auto" Skin="Metro" Width="100%" AllowFilteringByColumn="True" AutoGenerateColumns="false">
        <GroupingSettings CaseSensitive="false" />
        <MasterTableView>
            <Columns>
                <telerik:GridBoundColumn DataField="ACCOUNT_NUMBER" DataType="System.Int32" FilterControlAltText="Filter ACCOUNT_NUMBER column" HeaderText="Account Number" ReadOnly="True" SortExpression="ACCOUNT_NUMBER" UniqueName="ACCOUNT_NUMBER" AllowFiltering="false">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="BORROWER_PRIMARY_NAME" FilterControlAltText="Filter BORROWER_PRIMARY_NAME column" HeaderText="Borrower Name" SortExpression="BORROWER_PRIMARY_NAME" UniqueName="BORROWER_PRIMARY_NAME" AllowFiltering="false">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="PROPERTY_ADDRESS_1" FilterControlAltText="Filter PROPERTY_ADDRESS_1 column" HeaderText="Property Address" SortExpression="PROPERTY_ADDRESS_1" UniqueName="PROPERTY_ADDRESS_1" AllowFiltering="false">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="PROPERTY_CITY" FilterControlAltText="Filter PROPERTY_CITY column" HeaderText="City" SortExpression="PROPERTY_CITY" UniqueName="PROPERTY_CITY" AllowFiltering="false"> <%--CurrentFilterFunction="Contains" AutoPostBackOnFilter="true"--%>
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="PROPERTY_STATE" FilterControlAltText="Filter PROPERTY_STATE column" HeaderText="State" SortExpression="PROPERTY_STATE" UniqueName="PROPERTY_STATE" AllowFiltering="false">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="PROPERTY_ZIP" DataType="System.Int32" FilterControlAltText="Filter PROPERTY_ZIP column" HeaderText="Zip Code" SortExpression="PROPERTY_ZIP" UniqueName="PROPERTY_ZIP" AllowFiltering="false">
                </telerik:GridBoundColumn>
            </Columns>
        </MasterTableView>
        <FilterMenu RenderMode="Auto"></FilterMenu>
        <HeaderContextMenu RenderMode="Auto"></HeaderContextMenu>
    </telerik:RadGrid>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="pageContent_OutsideAjax" runat="server">
</asp:Content>
