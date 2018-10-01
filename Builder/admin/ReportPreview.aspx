<%@ Page Title="Report Preview" Language="vb" AutoEventWireup="false" MasterPageFile="~/masterPages/ContentPage.Master" CodeBehind="ReportPreview.aspx.vb" Inherits="UtilityWizards.Builder.ReportPreview" %>

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
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="breadcrumbContent" runat="server">
    <h1>Dashboard
        <small>Version 2.2 <asp:Label runat="server" ID="lblSandbox" /></small>
    </h1>
    <ol class="breadcrumb">
        <li><a href="..Default.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
        <li><a href="#">Admin</a></li>
        <li><a href="Reports.aspx">System Reports</a></li>
        <li class="active">Report Preview</li>
    </ol>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="pageContent_Ajax" runat="server">
    <telerik:RadGrid ID="RadReportGrid" runat="server" AllowFilteringByColumn="True" AllowSorting="True" GroupPanelPosition="Top" DataSourceID="SqlDataSource1" Skin="Metro">
        <MasterTableView DataSourceID="SqlDataSource1">
        </MasterTableView>
    </telerik:RadGrid>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:UtilityWizardsConnectionString %>" SelectCommand="procReportGrid" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:ControlParameter ControlID="lblFields" DefaultValue="*" Name="fields" PropertyName="Text" Type="String" />
            <asp:ControlParameter ControlID="lblModule" DefaultValue="0" Name="moduleID" PropertyName="Text" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:Panel runat="server" ID="pnlHidden" Visible="false" BackColor="#CC0000">
        <asp:Label runat="server" ID="lblFields" />
        <asp:Label runat="server" ID="lblModule" />
        <asp:Label ID="lblID" runat="server" />
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="pageContent_OutsideAjax" runat="server">
</asp:Content>
