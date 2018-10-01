<%@ Page Title="System Reports" Language="vb" AutoEventWireup="false" MasterPageFile="~/masterPages/ContentPage.Master" CodeBehind="Reports.aspx.vb" Inherits="UtilityWizards.Builder.Reports" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ MasterType VirtualPath="~/masterPages/ContentPage.Master" %>

<%@ Register Assembly="UtilityWizards.CommonCore.Shared" Namespace="UtilityWizards.CommonCore.Shared.Controls.TextBoxes" TagPrefix="Builder" %>
<%@ Register Assembly="UtilityWizards.CommonCore.Shared" Namespace="UtilityWizards.CommonCore.Shared.Controls.CheckBoxes" TagPrefix="Builder" %>
<%@ Register Assembly="UtilityWizards.CommonCore.Shared" Namespace="UtilityWizards.CommonCore.Shared.Controls.DropDownLists" TagPrefix="Builder" %>

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
        <li class="header">REPORT OPTIONS</li>
        <li runat="server" id="liNewReport" visible="false">
            <asp:LinkButton runat="server" ID="lnkNewReport"><i class="fa fa-file-text-o"></i><span>New Report</span></asp:LinkButton>
        </li>
    </ul>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="breadcrumbContent" runat="server">
    <h1>Dashboard
        <small>Version 2.2 <asp:Label runat="server" ID="lblSandbox" /></small>
    </h1>
    <ol class="breadcrumb">
        <li><a href="..Default.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
        <li><a href="#">Admin</a></li>
        <li class="active">System Reports</li>
    </ol>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="pageContent_Ajax" runat="server">
    <table class="wrap-table">
        <tr>
            <td style="border: 1px solid #C0C0C0; vertical-align: top; background-color: #E6E6E6;">
                <asp:Table runat="server" ID="tblReports" CellPadding="1" CellSpacing="2" Width="100%" Style="display: block;" />
            </td>
        </tr>
    </table>
    <asp:Panel runat="server" ID="pnlHidden" Visible="false" BackColor="#CC0000">
        <asp:Label runat="server" ID="lblEditID" />
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="pageContent_OutsideAjax" runat="server">
</asp:Content>
