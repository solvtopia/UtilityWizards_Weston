<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/masterPages/TabPage.Master" CodeBehind="ModuleTab.aspx.vb" Inherits="UtilityWizards.Builder.ModuleTab" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ MasterType VirtualPath="~/masterPages/TabPage.Master" %>

<%@ Register Assembly="UtilityWizards.CommonCore.Shared" Namespace="UtilityWizards.CommonCore.Shared.Controls.TextBoxes" TagPrefix="Builder" %>
<%@ Register Assembly="UtilityWizards.CommonCore.Shared" Namespace="UtilityWizards.CommonCore.Shared.Controls.CheckBoxes" TagPrefix="Builder" %>
<%@ Register Assembly="UtilityWizards.CommonCore.Shared" Namespace="UtilityWizards.CommonCore.Shared.Controls.DropDownLists" TagPrefix="Builder" %>
<%@ Register Src="~/controls/ModuleView.ascx" TagPrefix="uw" TagName="ModuleView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <style type="text/css">
        .wrap-table-module {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="pageContent_Ajax" runat="server">
    <asp:Literal runat="server" ID="litNoData" />
    <asp:Panel runat="server" ID="pnlDataNav">
        <table cellpadding="1" cellspacing="2">
            <tr>
                <td>Record:&nbsp;&nbsp;</td>
                <td>
                    <asp:LinkButton ID="lnkFirstRecord" runat="server">First</asp:LinkButton>&nbsp;</td>
                <td>
                    <asp:LinkButton ID="lnkPreviousRecord" runat="server">Previous</asp:LinkButton>&nbsp;&nbsp;</td>
                <td>
                    <asp:TextBox ID="txtRecordNum" runat="server" Width="30px" ReadOnly="true">1</asp:TextBox>
                </td>
                <td>&nbsp;&nbsp;<asp:LinkButton ID="lnkNextRecord" runat="server">Next</asp:LinkButton></td>
                <td>&nbsp;<asp:LinkButton ID="lnkLastRecord" runat="server">Last</asp:LinkButton></td>
                <td>&nbsp;&nbsp;of
                    <asp:Label ID="lblTotalRecords" runat="server" Text="100"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td colspan="5">&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
        </table>
    </asp:Panel>
    <uw:ModuleView runat="server" ID="ModuleView1" Mode="View" />
    <asp:Panel runat="server" ID="pnlHidden" Visible="false" BackColor="#CC0000">
        <Builder:TextBox ID="txtID" runat="server" DataFieldName="ID" Enabled="False" Width="80px" />
        <Builder:TextBox ID="txtUserEmail" runat="server" DataFieldName="UserEmail" Enabled="False" Width="250px" />
        <Builder:TextBox ID="txtViewed" runat="server" DataFieldName="ViewedOnMobile" Enabled="False" Width="80px" />
        <Builder:TextBox ID="txtCustomerServiceAddressLat" runat="server" DataFieldName="CustomerServiceAddressLat" Enabled="False" Visible="False">
        </Builder:TextBox>
        <Builder:TextBox ID="txtCustomerServiceAddressLon" runat="server" DataFieldName="CustomerServiceAddressLon" Enabled="False" Visible="False">
        </Builder:TextBox>
    </asp:Panel>
    <script type="text/javascript">
        function resize() {
            var height = getDocHeight();
            var elements = window.top.document.getElementsByTagName("div");

            for (var i = 0; i < elements.length; i++) {
                var containerPageViewID = "pvModules";

                if (elements[i].id.indexOf(containerPageViewID) > -1) {
                    elements[i].style.height = height + "px";
                    break;
                }
            }
        }

        if (window.addEventListener)
            window.addEventListener("load", resize, false);
        else if (window.attachEvent)
            window.attachEvent("onload", resize);
        else window.onload = resize;

        function getDocHeight() {
            var D = document;
            return Math.max(
                Math.max(D.body.scrollHeight, D.documentElement.scrollHeight),
                Math.max(D.body.offsetHeight, D.documentElement.offsetHeight),
                Math.max(D.body.clientHeight, D.documentElement.clientHeight)
            );
        }
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pageContent_OutsideAjax" runat="server">
</asp:Content>
