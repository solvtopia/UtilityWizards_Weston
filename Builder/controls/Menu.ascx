<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Menu.ascx.vb" Inherits="UtilityWizards.Builder.Menu" %>
<ul class="sidebar-menu">
    <li class="header">MAIN NAVIGATION</li>
    <li runat="server" id="liDashboard">
        <asp:LinkButton runat="server" ID="lnkDashboard">
                <i class="fa fa-dashboard"></i><span>Dashboard</span>
        </asp:LinkButton>
    </li>
    <%--<li runat="server" id="liRecentWorkOrders">
        <asp:LinkButton runat="server" ID="lnkRecentWorkOrders">
                <i class="fa fa-asdf"></i><span>Recent Work Orders</span>
        </asp:LinkButton>
    </li>--%>
    <asp:PlaceHolder runat="server" ID="pnlMainOptions">
        <li runat="server" id="liHelp">
            <asp:LinkButton runat="server" ID="lnkHelp"><i class="fa fa-book"></i><span>Help Me With This</span></asp:LinkButton></li>
        <li runat="server" id="liFAQ">
            <asp:LinkButton runat="server" ID="lnkFaq"><i class="fa fa-book"></i><span>F.A.Q.</span></asp:LinkButton></li>
        <asp:PlaceHolder runat="server" ID="pnlReportOptions">
            <li class="header">REPORT OPTIONS</li>
            <li runat="server" id="liNewReport">
                <asp:LinkButton runat="server" ID="lnkNewReport"><i class="fa fa-file-text-o"></i><span>New Report</span></asp:LinkButton>
            </li>
        </asp:PlaceHolder>
        <asp:PlaceHolder runat="server" ID="pnlAdminTools">
            <li class="header">ADMINISTRATOR TOOLS</li>
            <li runat="server" id="liCustomers">
                <asp:LinkButton runat="server" ID="lnkCustomers"><i class="fa fa-address-book-o"></i><span>Customers</span></asp:LinkButton>
            </li>
            <li runat="server" id="liUsers">
                <asp:LinkButton runat="server" ID="lnkUsers"><i class="fa fa-users"></i><span>Users Management</span></asp:LinkButton>
            </li>
            <li runat="server" id="liReports">
                <asp:LinkButton runat="server" ID="lnkReports"><i class="fa fa-file-text-o"></i><span>System Reports</span></asp:LinkButton>
            </li>
            <li runat="server" id="liUpload">
                <asp:LinkButton runat="server" ID="lnkUpload"><i class="fa fa-cloud-upload"></i><span>Uploads</span></asp:LinkButton>
            </li>
            <li runat="server" id="liLogs">
                <asp:LinkButton runat="server" ID="lnkLogs"><i class="fa fa-file-code-o"></i><span>System Logs</span></asp:LinkButton>
            </li>
            <li runat="server" id="liHelpTopics">
                <asp:LinkButton runat="server" ID="lnkHelpTopics"><i class="fa fa-book"></i><span>Help Topics</span></asp:LinkButton>
            </li>
        </asp:PlaceHolder>
    </asp:PlaceHolder>
    <asp:PlaceHolder runat="server" ID="pnlLoginOptions">
        <li runat="server" id="liLogin">
            <asp:LinkButton runat="server" ID="lnkLogin"><i class="fa fa-asdf"></i><span>Return to Login</span></asp:LinkButton></li>
        <li runat="server" id="liForgotPassword">
            <asp:LinkButton runat="server" ID="lnkForgotPassword"><i class="fa fa-asdf"></i><span>Forgot Password</span></asp:LinkButton></li>
        <li runat="server" id="liRegister">
            <asp:LinkButton runat="server" ID="lnkRegister"><i class="fa fa-asdf"></i><span>Register</span></asp:LinkButton></li>
    </asp:PlaceHolder>
</ul>
<asp:Panel runat="server" ID="pnlHidden" BackColor="#CC0000" Visible="false">
    <asp:Label runat="server" ID="lblUrlString" />
</asp:Panel>
