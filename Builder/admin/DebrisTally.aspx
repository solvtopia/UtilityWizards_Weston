<%@ Page Title="Debris Tally Report" Language="vb" AutoEventWireup="false" MasterPageFile="~/masterPages/ContentPage.master" CodeBehind="DebrisTally.aspx.vb" Inherits="UtilityWizards.Builder.DebrisTally" %>

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

        .rbLinkButton.fixedWidth {
            padding: 0;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="menuContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="breadcrumbContent" runat="server">
    <h1>Dashboard
        <small>Version 2.2</small>
    </h1>
    <ol class="breadcrumb">
        <li><a href="..Default.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
        <li><a href="#">Admin</a></li>
        <li><a href="Reports.aspx">System Reports</a></li>
        <li class="active">Debris Tally Report</li>
    </ol>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pageContent_Ajax" runat="server">
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
                    <h3 class="box-title">Debris Tally Report</h3>

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
                    <h3 class="box-title">Email This Report</h3>
                    <div class="box-tools pull-right">
                        <%--<button type="button" class="btn btn-box-tool" data-widget="collapse">
                                <i class="fa fa-minus"></i>
                            </button>
                            <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>--%>
                    </div>
                </div>
                <!-- /.box-header -->
                <div class="box-body">
                    <table class="wrap-table-module">
                        <tr>
                            <td>Recipients <em>(one per line)</em></td>
                        </tr>
                        <tr>
                            <td>
                                <Builder:TextBox ID="txtEmailRecipients" runat="server" Rows="5" TextMode="MultiLine" Width="100%">
                                </Builder:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadButton ID="btnSendMail" runat="server" ButtonType="LinkButton" CssClass="fixedWidth" Skin="Metro" Text="Send Email" Width="100%" />
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
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
<asp:Content ID="Content5" ContentPlaceHolderID="pageContent_OutsideAjax" runat="server">
</asp:Content>
