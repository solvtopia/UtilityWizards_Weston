<%@ Page Title="Recent Work Orders" Language="vb" AutoEventWireup="false" MasterPageFile="~/masterPages/ContentPage.master" CodeBehind="MobileLandingPage.aspx.vb" Inherits="UtilityWizards.Builder.MobileLandingPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="menuContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="breadcrumbContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pageContent_Ajax" runat="server">
    <telerik:RadMultiPage runat="server" ID="RadMultiPage1" Width="100%" Height="3000px" SelectedIndex="0">
        <telerik:RadPageView runat="server" ID="RadPageView1" ContentUrl="~/includes/RecentWorkOrders.aspx" Height="3000px" />
    </telerik:RadMultiPage>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="pageContent_OutsideAjax" runat="server">
</asp:Content>
