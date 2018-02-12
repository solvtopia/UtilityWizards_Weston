<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/masterPages/Dashboard.Master" CodeBehind="Module.aspx.vb" Inherits="UtilityWizards.Builder._Module" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ MasterType VirtualPath="~/masterPages/Dashboard.Master" %>

<%@ Register Assembly="UtilityWizards.CommonCore.Shared" Namespace="UtilityWizards.CommonCore.Shared.Controls.TextBoxes" TagPrefix="Builder" %>
<%@ Register Assembly="UtilityWizards.CommonCore.Shared" Namespace="UtilityWizards.CommonCore.Shared.Controls.CheckBoxes" TagPrefix="Builder" %>
<%@ Register Assembly="UtilityWizards.CommonCore.Shared" Namespace="UtilityWizards.CommonCore.Shared.Controls.DropDownLists" TagPrefix="Builder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <style type="text/css">
        .wrap-table-module {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="menuContent" runat="server">
    <ul class="sidebar-menu">
        <li class="header">WORK ORDER OPTIONS</li>
        <li>
            <asp:LinkButton runat="server" ID="lnkNew"><i class="fa fa-file-text-o"></i><span>New Work Order</span></asp:LinkButton></li>
        <li>
            <asp:LinkButton runat="server" ID="lnkSearch"><i class="fa fa-search"></i><span>Search</span></asp:LinkButton></li>
        <li>
            <asp:LinkButton runat="server" ID="lnkSave"><i class="fa fa-floppy-o"></i><span>Save Changes</span></asp:LinkButton></li>
        <li>
            <asp:LinkButton runat="server" ID="lnkReset"><i class="fa fa-asdf"></i><span>Clear Form</span></asp:LinkButton></li>
        <li>
            <asp:LinkButton runat="server" ID="lnkPrint"><i class="fa fa-print"></i><span>Print</span></asp:LinkButton></li>
        <li>
            <asp:LinkButton runat="server" ID="lnkSaveHome"><i class="fa fa-window-close-o"></i><span>Save & Close</span></asp:LinkButton></li>
        <asp:PlaceHolder runat="server" ID="pnlModuleOptions">
            <li class="header">MODULE OPTIONS</li>
            <li runat="server" id="liEditModule">
                <asp:LinkButton runat="server" ID="lnkEditModule"><i class="fa fa-pencil"></i><span>Edit Module</span></asp:LinkButton></li>
            <li runat="server" id="liDeleteModule">
                <asp:LinkButton runat="server" ID="lnkDeleteModule"><i class="fa fa-trash-o"></i><span>Delete Module</span></asp:LinkButton></li>
            <li runat="server" id="liCopyModule">
                <asp:LinkButton runat="server" ID="lnkCopyModule"><i class="fa fa-files-o"></i><span>Copy Module</span></asp:LinkButton></li>
            <li runat="server" id="liMoveModule">
                <asp:LinkButton runat="server" ID="lnkMoveModule"><i class="fa fa-stack-overflow"></i><span>Move to Folder</span></asp:LinkButton></li>
        </asp:PlaceHolder>
    </ul>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="breadcrumbContent" runat="server">
    <h1>Dashboard
        <small>Version 2.2</small>
    </h1>
    <ol class="breadcrumb">
        <li><a href="..Default.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
        <li class="active">System Modules</li>
    </ol>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="pageContent_Ajax" runat="server">
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
                <div class="box-header with-border">
                    <h3 class="box-title">
                        <asp:Label ID="lblHeader" runat="server" Text="Wizard Title"></asp:Label></h3>

                    <div class="box-tools pull-right">
                        <%--<h3 class="box-title"></h3>--%>
                    </div>
                </div>
                <!-- /.box-header -->
                <div class="box-body">
                    <asp:Table ID="tblFolderQuestions" runat="server" CellPadding="1" CellSpacing="2" Width="100%" />
                    <asp:Table ID="tblModuleQuestions" runat="server" CellPadding="1" CellSpacing="2" Width="100%" />
                </div>
                <!-- /.box-body -->
            </div>
            <!-- /.box -->
        </div>
        <!-- /.col -->

        <div class="col-md-4">

            <!-- HEADER & DETAILS -->
            <div class="box box-primary">
                <div class="box-header with-border">
                    <%--<button type="button" class="btn btn-box-tool" data-widget="collapse">
                                <i class="fa fa-minus"></i>
                            </button>
                            <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>--%>
                    <telerik:RadTabStrip ID="RadTabStrip1" runat="server" SelectedIndex="0" MultiPageID="RadMultiPage1" Skin="Metro">
                        <Tabs>
                            <telerik:RadTab runat="server" Text="Header &amp; Customer Details">
                            </telerik:RadTab>
                            <telerik:RadTab runat="server" Text="Location Map">
                            </telerik:RadTab>
                            <telerik:RadTab runat="server" Text="Ticket Text">
                            </telerik:RadTab>
                        </Tabs>
                    </telerik:RadTabStrip>

                    <div class="box-tools pull-right">
                        <%--<button type="button" class="btn btn-box-tool" data-widget="collapse">
                                <i class="fa fa-minus"></i>
                            </button>
                            <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>--%>
                    </div>
                </div>
                <!-- /.box-header -->
                <div class="box-body">
                    <telerik:RadMultiPage ID="RadMultiPage1" runat="server" SelectedIndex="0" Width="100%">
                        <telerik:RadPageView ID="RadPageView1" runat="server" Selected="true">
                            <asp:Panel runat="server" ID="pnlCustomerDetails">
                                <table class="wrap-table-module">
                                    <tr>
                                        <td>
                                            <asp:Label runat="server" ID="lblAcctNum">Customer Account Number</asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <Builder:TextBox ID="txtAcctNumber" runat="server" DataFieldName="CustAcctNum" ReadOnly="True" Width="100%" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Name</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <Builder:TextBox ID="txtCustomerName" runat="server" DataFieldName="CustomerName" ReadOnly="True" Width="100%">
                                            </Builder:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label runat="server" ID="lblLocationNum">Location Number</asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <Builder:TextBox ID="txtLocationNum" runat="server" DataFieldName="LocationNum" ReadOnly="True" Width="100%" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align: top;">Service Address</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <Builder:TextBox ID="txtCustomerServiceAddress" runat="server" DataFieldName="CustomerServiceAddress" ReadOnly="True" Rows="3" TextMode="MultiLine" Width="100%">
                                            </Builder:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <table class="wrap-table-module">
                                <tr>
                                    <td>
                                        <asp:Table runat="server" ID="tbl811SignOff" Width="100%" CellPadding="1" CellSpacing="2" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>Status</td>
                                </tr>
                                <tr>
                                    <td>
                                        <Builder:DropDownList ID="ddlStatus" runat="server" DataFieldName="Status" Width="100%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>Priority</td>
                                </tr>
                                <tr>
                                    <td>
                                        <Builder:DropDownList ID="ddlPriority" runat="server" DataFieldName="Priority" Width="100%" AutoPostBack="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>Notify Administrators</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkNotifyAdmins" runat="server" />
                                        &nbsp;<span style="font-size: small;"><em>(Email all administrators with updates)</em></span></td>
                                </tr>
                                <tr>
                                    <td>Supervisor</td>
                                </tr>
                                <tr>
                                    <td>
                                        <Builder:DropDownList ID="ddlSupervisor" runat="server" DataFieldName="Supervisor" Width="100%" AutoPostBack="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>Technician</td>
                                </tr>
                                <tr>
                                    <td>
                                        <Builder:DropDownList ID="ddlTechnician" runat="server" DataFieldName="Technician" Width="100%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>Technician Comments</td>
                                </tr>
                                <tr>
                                    <td>
                                        <Builder:TextBox ID="txtTechComments" runat="server" DataFieldName="TechnicianComments" Rows="3" TextMode="MultiLine" Width="100%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                </tr>
                            </table>
                        </telerik:RadPageView>
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
                        <telerik:RadPageView ID="RadPageView3" runat="server">
                            <table class="wrap-table-module">
                                <tr>
                                    <td>
                                        <Builder:TextBox ID="txtPrintView" runat="server" DataFieldName="printable_text" ReadOnly="True" Rows="30" TextMode="MultiLine" Width="100%">
                                        </Builder:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </telerik:RadPageView>
                    </telerik:RadMultiPage>
                </div>
                <!-- /.box-body -->
            </div>
            <!-- /.box -->
        </div>
        <!-- /.col -->
    </div>
    <!-- /.row -->
    <asp:Panel runat="server" ID="pnlHidden" Visible="false" BackColor="#CC0000">
        <Builder:TextBox ID="txtID" runat="server" DataFieldName="ID" Enabled="False" Width="80px" />
        <Builder:TextBox ID="txtUserEmail" runat="server" DataFieldName="UserEmail" Enabled="False" Width="250px" />
        <Builder:TextBox ID="txtViewed" runat="server" DataFieldName="ViewedOnMobile" Enabled="False" Width="80px" />
        <Builder:TextBox ID="txtCustomerServiceAddressLat" runat="server" DataFieldName="CustomerServiceAddressLat" Enabled="False" Visible="False">
        </Builder:TextBox>
        <Builder:TextBox ID="txtCustomerServiceAddressLon" runat="server" DataFieldName="CustomerServiceAddressLon" Enabled="False" Visible="False">
        </Builder:TextBox>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="pageContent_OutsideAjax" runat="server">
</asp:Content>
