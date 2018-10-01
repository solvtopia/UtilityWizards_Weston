<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/masterPages/ContentPage.master" CodeBehind="ModuleManager.aspx.vb" Inherits="UtilityWizards.Builder.ModuleManager" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ MasterType VirtualPath="~/masterPages/ContentPage.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <script src="scripts/radWindowScripts.js"></script>
    <style type="text/css">
        .wrap-table-module {
            width: 100%;
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
        <li class="active">Module Manager</li>
    </ol>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pageContent_Ajax" runat="server">
    <telerik:RadButton ID="btnAddModule" runat="server" ButtonType="LinkButton" CssClass="fixedWidth" Skin="Metro" Text="Add New Tab" Width="100%" />
    <br />
    <br />
    <asp:Table runat="server" ID="tblModules" Width="100%" CellPadding="1" CellSpacing="2" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="pageContent_OutsideAjax" runat="server">
</asp:Content>
