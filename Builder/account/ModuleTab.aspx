<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/masterPages/TabPage.Master" CodeBehind="ModuleTab.aspx.vb" Inherits="UtilityWizards.Builder.ModuleTab" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ MasterType VirtualPath="~/masterPages/TabPage.Master" %>

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
                <%--<div class="box-header with-border">
                    <h3 class="box-title">
                        <asp:Label ID="lblHeader" runat="server" Text="Wizard Title"></asp:Label></h3>

                    <div class="box-tools pull-right">
                        <h3 class="box-title"></h3>
                    </div>
                </div>--%>
                <!-- /.box-header -->
                <div class="box-body">
                    <asp:Table ID="tblFolderQuestions" runat="server" CellPadding="1" CellSpacing="2" Width="100%" />
                    <table class="nav-justified">
                        <tr>
                            <td style="background-color: #E4E4E4; vertical-align: top;">
                                <asp:Table ID="tblModuleQuestions_TopLeft" runat="server" CellPadding="1" CellSpacing="2" Width="100%" />
                            </td>
                            <td style="background-color: #C0C0C0; vertical-align: top;">
                                <asp:Table ID="tblModuleQuestions_TopMiddle" runat="server" CellPadding="1" CellSpacing="2" Width="100%" />
                            </td>
                            <td style="background-color: #808080; vertical-align: top;">
                                <asp:Table ID="tblModuleQuestions_TopRight" runat="server" CellPadding="1" CellSpacing="2" Width="100%" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" style="background-color: #FFFFFF; vertical-align: top;">
                                <asp:Table ID="tblModuleQuestions_FullPage" runat="server" CellPadding="1" CellSpacing="2" Width="100%" />
                            </td>
                        </tr>
                        <tr>
                            <td style="background-color: #E4E4E4; vertical-align: top;">
                                <asp:Table ID="tblModuleQuestions_BottomLeft" runat="server" CellPadding="1" CellSpacing="2" Width="100%" />
                            </td>
                            <td style="background-color: #C0C0C0; vertical-align: top;">
                                <asp:Table ID="tblModuleQuestions_BottomMiddle" runat="server" CellPadding="1" CellSpacing="2" Width="100%" />
                            </td>
                            <td style="background-color: #808080; vertical-align: top;">
                                <asp:Table ID="tblModuleQuestions_BottomRight" runat="server" CellPadding="1" CellSpacing="2" Width="100%" />
                            </td>
                        </tr>
                    </table>
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
                    <telerik:RadTabStrip ID="RadTabStrip1" runat="server" SelectedIndex="0" MultiPageID="RadMultiPage1" Skin="Metro">
                        <Tabs>
                            <telerik:RadTab runat="server" Text="Header &amp; Customer Details">
                            </telerik:RadTab>
                            <telerik:RadTab runat="server" Text="Location Map">
                            </telerik:RadTab>
                        </Tabs>
                    </telerik:RadTabStrip>

                    <div class="box-tools pull-right">
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
                                        <td>Primary Name</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <Builder:TextBox ID="txtCustomerName" runat="server" DataFieldName="CustomerName" ReadOnly="True" Width="100%">
                                            </Builder:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align: top;">Primary Address</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <Builder:TextBox ID="txtCustomerServiceAddress" runat="server" DataFieldName="CustomerServiceAddress" ReadOnly="True" Rows="3" TextMode="MultiLine" Width="100%">
                                            </Builder:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
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
<asp:Content ID="Content4" ContentPlaceHolderID="pageContent_OutsideAjax" runat="server">
</asp:Content>
