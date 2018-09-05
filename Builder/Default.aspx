<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/masterPages/Dashboard.Master" CodeBehind="Default.aspx.vb" Inherits="UtilityWizards.Builder._Default4" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ MasterType VirtualPath="~/masterPages/Dashboard.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <script src="scripts/radWindowScripts.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="menuContent" runat="server">
    <ul class="sidebar-menu">
        <asp:PlaceHolder runat="server" ID="pnlRootOptions">
            <li class="header">DASHBOARD OPTIONS</li>
            <li runat="server" id="liNewModule">
                <asp:LinkButton runat="server" ID="lnkNewModule"><i class="fa fa-puzzle-piece"></i><span>Add Module</span></asp:LinkButton>
            </li>
        </asp:PlaceHolder>
        <asp:PlaceHolder runat="server" ID="pnlRecordOptions">
            <li class="header">RECORD OPTIONS</li>
            <li>
                <asp:LinkButton runat="server" ID="lnkSearch"><i class="fa fa-search"></i><span>Search</span></asp:LinkButton></li>
            <li>
                <asp:LinkButton runat="server" ID="lnkPrint"><i class="fa fa-print"></i><span>Print</span></asp:LinkButton></li>
            <asp:PlaceHolder runat="server" ID="pnlModuleOptions">
                <li class="header">MODULE OPTIONS</li>
                <li runat="server" id="liEditModule">
                    <asp:LinkButton runat="server" ID="lnkEditModule"><i class="fa fa-pencil"></i><span>Edit Module</span></asp:LinkButton></li>
                <li runat="server" id="liDeleteModule">
                    <asp:LinkButton runat="server" ID="lnkDeleteModule"><i class="fa fa-trash-o"></i><span>Delete Module</span></asp:LinkButton></li>
                <li runat="server" id="liCopyModule">
                    <asp:LinkButton runat="server" ID="lnkCopyModule"><i class="fa fa-files-o"></i><span>Copy Module</span></asp:LinkButton></li>
            </asp:PlaceHolder>
        </asp:PlaceHolder>
    </ul>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="breadcrumbContent" runat="server">
    <h1>Dashboard
        <small>Version 2.2</small>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>Home</a></li>
        <li class="active">Dashboard</li>
    </ol>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pageContent_Ajax" runat="server">
    <!-- Info boxes -->
    <asp:PlaceHolder runat="server" ID="pnlBadges">
        <%--<div class="row">
            <div class="col-md-3 col-sm-6 col-xs-12">
                <div class="info-box">
                    <span class="info-box-icon bg-red"></span>

                    <div class="info-box-content">
                        <span class="info-box-text">
                            <asp:LinkButton runat="server" ID="lnkOpenWorkOrders">Open Work Orders</asp:LinkButton></span>
                        <span class="info-box-number">
                            <asp:Label runat="server" ID="lblOpenWorkOrders" /></span>
                    </div>
                    <!-- /.info-box-content -->
                </div>
                <!-- /.info-box -->
            </div>
            <!-- /.col -->

            <!-- fix for small devices only -->
            <div class="clearfix visible-sm-block"></div>

            <div class="col-md-3 col-sm-6 col-xs-12">
                <div class="info-box">
                    <span class="info-box-icon bg-green"></span>

                    <div class="info-box-content">
                        <span class="info-box-text">
                            <asp:LinkButton runat="server" ID="lnkCompletedWorkOrders">Completed Work Orders</asp:LinkButton></span>
                        <span class="info-box-number">
                            <asp:Label runat="server" ID="lblCompletedWorkOrders" /></span>
                    </div>
                    <!-- /.info-box-content -->
                </div>
                <!-- /.info-box -->
            </div>
            <!-- /.col -->
        </div>--%>
    </asp:PlaceHolder>
    <!-- /.row -->
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-header with-border">
                    <%--<h3 class="box-title">
                        <asp:Label ID="lblHeader" runat="server" Text="System Modules"></asp:Label></h3>--%>

                    <div class="box-tools pull-right">
                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                            <i class="fa fa-minus"></i>
                        </button>
                        <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                    </div>
                </div>
                <!-- /.box-header -->
                <div class="box-body">
                    <telerik:RadAjaxPanel ID="MainAjaxPanel" runat="server" Width="100%" HorizontalAlign="NotSet" LoadingPanelID="MainAjaxLoadingPanel">
                        <asp:Literal runat="server" ID="lblAppUrl" />
                        <asp:HiddenField runat="server" ID="hfSearchDone" />
                        <telerik:RadTabStrip ID="tabSearch" runat="server" SelectedIndex="0" Skin="Metro" MultiPageID="RadMultiPage2">
                            <Tabs>
                                <telerik:RadTab Text="Customer Search" Value="search" />
                            </Tabs>
                        </telerik:RadTabStrip>
                        <telerik:RadTabStrip ID="tabModules" runat="server" SelectedIndex="0" Skin="Metro" MultiPageID="RadMultiPage2" />
                        <telerik:RadMultiPage runat="server" ID="RadMultiPage2" Width="100%" SelectedIndex="0" ><!--Height="550px"-->
                            <telerik:RadPageView runat="server" ID="RadPageView1">
                                <asp:Panel runat="server" ID="pnlCustomerSearch" DefaultButton="btnSearch">
                                    <div class="row">
                                        <!-- Left col -->
                                        <div class="col-md-8">
                                            <div class="row">
                                                <div class="col-md-6">
                                                </div>
                                                <!-- /.col -->

                                                <div class="col-md-6">
                                                </div>
                                                <!-- /.col -->
                                            </div>
                                            <!-- /.row -->

                                            <!-- MAIN CONTENT -->
                                            <div class="box box-info">
                                                <div class="box-body">
                                                    <table class="nav-justified">
                                                        <tr>
                                                            <td>Criteria</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadTextBox ID="txtSearch" runat="server" Skin="Metro" Width="100%">
                                                                </telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadButton ID="btnSearch" runat="server" ButtonType="LinkButton" CssClass="fixedWidth" Skin="Metro" Text="Search" Width="100%" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadGrid ID="RadSearchGrid" runat="server" AllowSorting="True" GroupPanelPosition="Top" AutoGenerateEditColumn="True" Skin="Metro" DataSourceID="SqlDataSource1">
                                                                    <MasterTableView AutoGenerateColumns="True" DataSourceID="SqlDataSource1">
                                                                        <%--<Columns>
                                                                        <telerik:GridBoundColumn DataField="ACCOUNT_NUMBER" FilterControlAltText="Filter ACCOUNT_NUMBER column" HeaderText="ACCOUNT_NUMBER" ReadOnly="True" SortExpression="ACCOUNT_NUMBER" UniqueName="ACCOUNT_NUMBER">
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridBoundColumn DataField="BORROWER_PRIMARY_NAME" FilterControlAltText="Filter BORROWER_PRIMARY_NAME column" HeaderText="BORROWER_PRIMARY_NAME" SortExpression="BORROWER_PRIMARY_NAME" UniqueName="BORROWER_PRIMARY_NAME">
                                                                        </telerik:GridBoundColumn>
                                                                    </Columns>--%>
                                                                    </MasterTableView>
                                                                </telerik:RadGrid>
                                                                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:UtilityWizardsConnectionString %>" SelectCommand="procSearchAccounts" SelectCommandType="StoredProcedure">
                                                                    <SelectParameters>
                                                                        <asp:ControlParameter ControlID="txtSearch" DefaultValue="" Name="search" PropertyName="Text" Type="String" />
                                                                    </SelectParameters>
                                                                </asp:SqlDataSource>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <!-- /.box-body -->
                                            </div>
                                            <!-- /.box -->
                                        </div>
                                        <!-- /.col -->

                                    </div>
                                </asp:Panel>
                            </telerik:RadPageView>
                            <telerik:RadPageView runat="server" ID="RadPageView2" ContentUrl="~/account/SearchTab.aspx" /><%--Height="550px" />--%>
                        </telerik:RadMultiPage>
                        <asp:Table runat="server" ID="tblModules" CellPadding="1" CellSpacing="2" Width="100%" Style="display: block;" Visible="false" />
                    </telerik:RadAjaxPanel>
                </div>
                <!-- ./box-body -->
            </div>
            <!-- /.box -->
        </div>
        <!-- /.col -->
    </div>
    <!-- /.row -->

    <!-- Main row -->
    <div class="row">
        <!-- Left col -->
        <div class="col-md-8">
            <div class="row">
                <div class="col-md-6">
                </div>
                <!-- /.col -->

                <div class="col-md-6">
                </div>
                <!-- /.col -->
            </div>
            <!-- /.row -->

            <!-- TABLE: RECENT WORK ORDERS -->
            <%--<div class="box box-info">
                <div class="box-header with-border">
                    <h3 class="box-title">Recent Work Orders</h3>

                    <div class="box-tools pull-right">
                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                            <i class="fa fa-minus"></i>
                        </button>
                        <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                    </div>
                </div>
                <!-- /.box-header -->
                <div class="box-body">
                    <telerik:RadMultiPage runat="server" ID="RadMultiPage1" Width="100%" Height="350px" SelectedIndex="0">
                        <telerik:RadPageView runat="server" ID="RadPageView1" ContentUrl="~/includes/RecentWorkOrders.aspx" Height="350px" />
                    </telerik:RadMultiPage>
                </div>
                <!-- /.box-body -->
            </div>--%>
            <!-- /.box -->
        </div>
        <!-- /.col -->

        <div class="col-md-4">

            <!-- ACTIVITY LIST -->
            <%--<asp:PlaceHolder runat="server" ID="pnlActivity">
                <div class="box box-primary">
                    <div class="box-header with-border">
                        <h3 class="box-title">Today's Activity</h3>

                        <div class="box-tools pull-right">
                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                <i class="fa fa-minus"></i>
                            </button>
                            <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                        </div>
                    </div>
                    <!-- /.box-header -->
                    <div class="box-body">
                        <telerik:RadMultiPage runat="server" ID="RadMultiPage2" Width="100%" Height="350px" SelectedIndex="0">
                            <telerik:RadPageView runat="server" ID="RadPageView2" ContentUrl="~/includes/History.aspx" Height="350px" />
                        </telerik:RadMultiPage>
                    </div>
                    <!-- /.box-body -->
                </div>
            </asp:PlaceHolder>--%>
            <!-- /.box -->
        </div>
        <!-- /.col -->
    </div>
    <!-- /.row -->
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="pageContent_OutsideAjax" runat="server">
</asp:Content>
