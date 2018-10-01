<%@ Page Title="User Confirmation Required" Language="vb" AutoEventWireup="false" MasterPageFile="~/masterPages/ContentPage.Master" CodeBehind="Confirmation.aspx.vb" Inherits="UtilityWizards.Builder.Confirmation" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ MasterType VirtualPath="~/masterPages/ContentPage.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <style type="text/css">
        .wrap-confirmation-table {
            width: 100%;
            display: block;
        }

            .wrap-confirmation-table td {
                display: inline-block;
            }
    </style>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="breadcrumbContent" runat="server">
    <h1>Dashboard
        <small>Version 2.2 <asp:Label runat="server" ID="lblSandbox" /></small>
    </h1>
    <ol class="breadcrumb">
        <li><a href="..Default.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
    </ol>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="pageContent_Ajax" runat="server">
    <table class="wrap-confirmation-table">
        <tr>
            <td rowspan="5" style="width: 42px; vertical-align: top;">
                <asp:Image ID="imgIcon" runat="server" ImageUrl="~/images/icon_information.png" />
            </td>
            <td style="font-size: medium; font-weight: bold">
                <asp:Label runat="server" ID="lblTitle" /></td>
        </tr>
        <tr>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="litMessage" runat="server"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="pnlChooseFolder" runat="server">
                    <table class="wrap-confirmation-table">
                        <tr>
                            <td>Folder</td>
                            <td style="width: 80%">
                                <telerik:RadComboBox ID="ddlFolder" runat="server" Width="250px" Skin="Metro">
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td></td>
        </tr>
    </table>
    <table>
        <tr>
            <td>
                <telerik:RadButton ID="btnYes" runat="server" Text="Yes" Skin="Metro" />
            </td>
            <td>
                <telerik:RadButton ID="btnNo" runat="server" Text="No" Skin="Metro" />
            </td>
            <td>&nbsp;
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="pageContent_OutsideAjax" runat="server">
</asp:Content>
