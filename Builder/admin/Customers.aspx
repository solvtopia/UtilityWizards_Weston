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
        <small>Version 2.2</small>
    </h1>
    <ol class="breadcrumb">
        <li><a href="..Default.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
        <li><a href="#">Admin</a></li>
        <li class="active">System Customers</li>
    </ol>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pageContent_Ajax" runat="server">
    <telerik:RadGrid ID="RadCustomerGrid" runat="server" GroupPanelPosition="Top" RenderMode="Auto" Skin="Metro" Width="100%" AllowFilteringByColumn="True" AutoGenerateColumns="false">
        <GroupingSettings CaseSensitive="false" />
        <MasterTableView>
            <Columns>
                <telerik:GridBoundColumn DataField="Account ID" DataType="System.Int32" FilterControlAltText="Filter Account ID column" HeaderText="Account ID" ReadOnly="True" SortExpression="Account ID" UniqueName="AccountID" AllowFiltering="false">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Location ID" FilterControlAltText="Filter Location ID column" HeaderText="Location ID" ReadOnly="True" SortExpression="Location ID" UniqueName="LocationID" AllowFiltering="false">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Full Name" FilterControlAltText="Filter Full Name column" HeaderText="Full Name" SortExpression="Full Name" UniqueName="FullName" AllowFiltering="false">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Service Address" FilterControlAltText="Filter Service Address column" HeaderText="Service Address" ReadOnly="True" SortExpression="Service Address" UniqueName="ServiceAddress" AllowFiltering="true" CurrentFilterFunction="Contains" AutoPostBackOnFilter="true">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="City" DataType="System.Int32" FilterControlAltText="Filter City column" HeaderText="City" ReadOnly="True" SortExpression="City" UniqueName="City" AllowFiltering="false">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="State" DataType="System.Int32" FilterControlAltText="Filter State column" HeaderText="State" ReadOnly="True" SortExpression="State" UniqueName="State" AllowFiltering="false">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Zip Code" FilterControlAltText="Filter Zip Code column" HeaderText="Zip Code" ReadOnly="True" SortExpression="Zip Code" UniqueName="ZipCode" AllowFiltering="false">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Receptacle 01" FilterControlAltText="Filter Receptacle 01 column" HeaderText="Receptacle 01" ReadOnly="True" SortExpression="Receptacle 01" UniqueName="Receptacle01" AllowFiltering="false">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Receptacle 02" DataType="System.Int32" FilterControlAltText="Filter Receptacle 02 column" HeaderText="Receptacle 02" ReadOnly="True" SortExpression="Receptacle 02" UniqueName="Receptacle02" AllowFiltering="false">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Receptacle 03" DataType="System.Int32" FilterControlAltText="Filter Receptacle 03 column" HeaderText="Receptacle 03" ReadOnly="True" SortExpression="Receptacle 03" UniqueName="Receptacle03" AllowFiltering="false">
                </telerik:GridBoundColumn>
            </Columns>
        </MasterTableView>
        <FilterMenu RenderMode="Auto"></FilterMenu>
        <HeaderContextMenu RenderMode="Auto"></HeaderContextMenu>
    </telerik:RadGrid>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="pageContent_OutsideAjax" runat="server">
</asp:Content>
