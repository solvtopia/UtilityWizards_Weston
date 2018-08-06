<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/masterPages/TabPage.Master" CodeBehind="SearchTab.aspx.vb" Inherits="UtilityWizards.Builder.SearchTab" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ MasterType VirtualPath="~/masterPages/TabPage.Master" %>

<%@ Register Assembly="UtilityWizards.CommonCore.Shared" Namespace="UtilityWizards.CommonCore.Shared.Controls.TextBoxes" TagPrefix="Builder" %>
<%@ Register Assembly="UtilityWizards.CommonCore.Shared" Namespace="UtilityWizards.CommonCore.Shared.Controls.CheckBoxes" TagPrefix="Builder" %>
<%@ Register Assembly="UtilityWizards.CommonCore.Shared" Namespace="UtilityWizards.CommonCore.Shared.Controls.DropDownLists" TagPrefix="Builder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <style type="text/css">
        .wrap-table-search {
            width: 100%;
        }

        .fixedWidth {
            top: 1px;
            left: 1px;
        }
    </style>
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
                        <h3 class="box-title">Customer Search</h3>

                        <div class="box-tools pull-right">
                            <%--<h3 class="box-title"></h3>--%>
                        </div>
                    </div>
                    <!-- /.box-header -->
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
                                <td>
                                    <telerik:RadGrid ID="RadSearchGrid" runat="server" AllowSorting="True" GroupPanelPosition="Top" AutoGenerateEditColumn="True" Skin="Metro" DataSourceID="SqlDataSource1">
                                        <MasterTableView AutoGenerateColumns="False" DataSourceID="SqlDataSource1">
                                            <Columns>
                                                <telerik:GridBoundColumn DataField="ACCOUNT_NUMBER" FilterControlAltText="Filter ACCOUNT_NUMBER column" HeaderText="ACCOUNT_NUMBER" ReadOnly="True" SortExpression="ACCOUNT_NUMBER" UniqueName="ACCOUNT_NUMBER">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="BORROWER_PRIMARY_NAME" FilterControlAltText="Filter BORROWER_PRIMARY_NAME column" HeaderText="BORROWER_PRIMARY_NAME" SortExpression="BORROWER_PRIMARY_NAME" UniqueName="BORROWER_PRIMARY_NAME">
                                                </telerik:GridBoundColumn>
                                            </Columns>
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
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="pageContent_OutsideAjax" runat="server">
</asp:Content>
