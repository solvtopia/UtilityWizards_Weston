<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/masterPages/ContentPage.master" CodeBehind="Modules.aspx.vb" Inherits="UtilityWizards.Builder.Modules" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ MasterType VirtualPath="~/masterPages/ContentPage.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <script src="scripts/radWindowScripts.js"></script>
    <style type="text/css">
        .wrap-table-module {
            width: 100%;
            position: relative;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="menuContent" runat="server">
    <ul class="sidebar-menu">
        <asp:PlaceHolder runat="server" ID="pnlRootOptions">
            <li class="header">DASHBOARD OPTIONS</li>
            <li runat="server" id="liModules">
                <asp:LinkButton runat="server" ID="lnkModules"><i class="fa fa-puzzle-piece"></i><span>Manage Tabs</span></asp:LinkButton>
            </li>
        </asp:PlaceHolder>
        <asp:PlaceHolder runat="server" ID="pnlRecordOptions">
            <li class="header">RECORD OPTIONS</li>
            <li>
                <asp:LinkButton runat="server" ID="lnkSearch"><i class="fa fa-search"></i><span>Search</span></asp:LinkButton></li>
        </asp:PlaceHolder>
    </ul>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="breadcrumbContent" runat="server">
    <h1>Dashboard
        <small>Version 2.2 <asp:Label runat="server" ID="lblSandbox" /></small>
    </h1>
    <ol class="breadcrumb">
        <li><a href="..Default.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
        <li class="active">Modules</li>
    </ol>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pageContent_Ajax" runat="server">
    <%--<div class="row">
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
            <div class="box box-info">--%>
                <%--<div class="box-header with-border">
                    <h3 class="box-title">
                        <asp:Label ID="lblHeader" runat="server" Text="Wizard Title"></asp:Label></h3>

                    <div class="box-tools pull-right">
                        <h3 class="box-title"></h3>
                    </div>
                </div>--%>
                <!-- /.box-header -->
                <%--<div class="box-body">--%>
                    <telerik:RadTabStrip ID="tabModules" runat="server" SelectedIndex="0" Skin="Metro" />
                    <telerik:RadMultiPage ID="mpModules" runat="server" Width="100%" SelectedIndex="0">
                        <telerik:RadPageView ID="pvModules" runat="server" Selected="true" />
                    </telerik:RadMultiPage>
                <%--</div>
                <!-- /.box-body -->
            </div>
            <!-- /.box -->
            </div>
            <!-- /.col -->

            <div class="col-md-4">

                <!-- HEADER & DETAILS -->
                <div class="box box-primary">
                    <div class="box-header with-border">
                        <telerik:RadTabStrip ID="RadTabStrip1" runat="server" SelectedIndex="0" MultiPageID="RadMultiPage1" Skin="Metro" Visible="false">
                            <Tabs>
                                <telerik:RadTab runat="server" Text="Location Map">
                                </telerik:RadTab>
                            </Tabs>
                        </telerik:RadTabStrip>

                        <div class="box-tools pull-right">
                        </div>
                    </div>
                    <!-- /.box-header -->
                    <div class="box-body">
                        <telerik:RadMultiPage ID="RadMultiPage1" runat="server" SelectedIndex="0" Width="100%" Visible="false">
                            <telerik:RadPageView ID="RadPageView2" runat="server">
                                <telerik:RadMap RenderMode="Auto" runat="server" ID="RadMap1" Zoom="16" Skin="Metro" Height="350px" Width="100%">
                                    <DataBindings>
                                        <MarkerBinding DataShapeField="Shape" DataTitleField="City" DataLocationLatitudeField="Latitude" DataLocationLongitudeField="Longitude" />
                                    </DataBindings>
                                    <LayersCollection>
                                        <telerik:MapLayer Type="Tile" Subdomains="a,b,c"
                                            UrlTemplate="http://#= subdomain #.tile.openstreetmap.org/#= zoom #/#= x #/#= y #.png"
                                            Attribution="&copy; <a href='http://osm.org/copyright' title='OpenStreetMap contributors' target='_blank'>OpenStreetMap contributors</a>.">
                                        </telerik:MapLayer>
                                    </LayersCollection>
                                </telerik:RadMap>
                            </telerik:RadPageView>
                        </telerik:RadMultiPage>
                    </div>
                    <!-- /.box-body -->
                </div>
                <!-- /.box -->
            </div>
            <!-- /.col -->
        </div>--%>
        <!-- /.row -->
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="pageContent_OutsideAjax" runat="server">
</asp:Content>
