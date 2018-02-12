<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RecentWorkOrders.aspx.vb" Inherits="UtilityWizards.Builder.RecentWorkOrders" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../styles/utilityWizards.css" rel="stylesheet" />
    <style type="text/css">
        .wrap-table {
            width: 100%;
            display: block;
        }

            .wrap-table td {
                display: inline-block;
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
            <Scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
            </Scripts>
        </telerik:RadScriptManager>
        <table style="width: 100%;">
            <tr>
                <td>
                    <telerik:RadGrid ID="RadSearchGrid" runat="server" AllowSorting="True" GroupPanelPosition="Top" Skin="Metro" CellSpacing="-1" GridLines="Both" DataSourceID="SqlDataSource1" AutoGenerateEditColumn="true" BorderColor="#e5e5e5" BorderStyle="Solid" BorderWidth="1px">
                        <MasterTableView AutoGenerateColumns="False" DataKeyNames="ID" DataSourceID="SqlDataSource1">
                            <Columns>
                                <telerik:GridBoundColumn DataField="ID" DataType="System.Int32" FilterControlAltText="Filter ID column" HeaderText="ID" ReadOnly="True" SortExpression="ID" UniqueName="ID">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="CustomerAccount" FilterControlAltText="Filter CustomerAccount column" HeaderText="Customer Account" ReadOnly="True" SortExpression="CustomerAccount" UniqueName="CustomerAccount">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="CustomerName" FilterControlAltText="Filter CustomerName column" HeaderText="Customer Name" SortExpression="CustomerName" UniqueName="CustomerName">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ServiceAddress" FilterControlAltText="Filter ServiceAddress column" HeaderText="Service Address" SortExpression="ServiceAddress" UniqueName="ServiceAddress">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Priority" FilterControlAltText="Filter Priority column" HeaderText="Priority" ReadOnly="True" SortExpression="Priority" UniqueName="Priority">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Status" DataType="System.Int32" FilterControlAltText="Filter Status column" HeaderText="Status" ReadOnly="True" SortExpression="Status" UniqueName="Status">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ModuleID" DataType="System.Int32" FilterControlAltText="Filter ModuleID column" HeaderText="ModuleID" ReadOnly="True" SortExpression="ModuleID" UniqueName="ModuleID" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ModuleName" FilterControlAltText="Filter ModuleName column" HeaderText="Module Name" ReadOnly="True" SortExpression="ModuleName" UniqueName="ModuleName">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ModuleIcon" FilterControlAltText="Filter ModuleIcon column" HeaderText="ModuleIcon" ReadOnly="True" SortExpression="ModuleIcon" UniqueName="ModuleIcon" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="xFolderID" DataType="System.Int32" FilterControlAltText="Filter xFolderID column" HeaderText="xFolderID" ReadOnly="True" SortExpression="xFolderID" UniqueName="xFolderID" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="SupervisorID" DataType="System.Int32" FilterControlAltText="Filter SupervisorID column" HeaderText="SupervisorID" ReadOnly="True" SortExpression="SupervisorID" UniqueName="SupervisorID" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Updated" DataType="System.DateTime" FilterControlAltText="Filter Updated column" HeaderText="Updated" SortExpression="Updated" UniqueName="Updated">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="UpdatedBy" FilterControlAltText="Filter UpdatedBy column" HeaderText="Updated By" ReadOnly="True" SortExpression="UpdatedBy" UniqueName="UpdatedBy" Display="false">
                                </telerik:GridBoundColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                    <telerik:RadGrid ID="RadSearchGridMobile" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1" GroupPanelPosition="Top" Skin="MetroTouch" CellSpacing="-1" GridLines="Both">
                        <ClientSettings Selecting-AllowRowSelect="true" EnablePostBackOnRowClick="true">
                        </ClientSettings>
                        <MasterTableView DataKeyNames="ID" DataSourceID="SqlDataSource1" ShowHeader="false">
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="Work Order Data" UniqueName="TemplateColumn">
                                    <ItemTemplate>
                                        <table style="width: 100%;">
                                            <tr>
                                                <td style="vertical-align: top; width: 55px;">
                                                    <asp:Image ID='imgIcon' runat='server' Height='45px' Width='45px' ImageUrl='<%# Eval("ModuleIcon") %>' />
                                                </td>
                                                <td style="vertical-align: top;">
                                                    <table style="width: 100%;">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID='lblModule' runat='server' Font-Bold='True' ForeColor='White' Text='<%# Eval("ModuleName") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID='Label1' runat='server' ForeColor='White' Text='Work Order #'></asp:Label><asp:Label ID='lblWorkOrderID' runat='server' ForeColor='White' Text='<%# Eval("ID") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID='lblStatus' runat='server' ForeColor='White' Text='<%# Eval("Status") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID='lblDescription' runat='server' ForeColor='White' Text='<%# Eval("CustomerName") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID='lblAddress' runat='server' ForeColor='White' Text='<%# Eval("ServiceAddress") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn HeaderText="Priority" UniqueName="Priority" DataField="Priority" Display="false" />
                                <telerik:GridBoundColumn HeaderText="ID" UniqueName="ID" DataField="ID" Display="false" />
                                <telerik:GridBoundColumn HeaderText="ModuleID" UniqueName="ModuleID" DataField="ModuleID" Display="false" />
                                <telerik:GridBoundColumn HeaderText="FolderID" UniqueName="FolderID" DataField="FolderID" Display="false" />
                                <telerik:GridBoundColumn HeaderText="SupervisorID" UniqueName="SupervisorID" DataField="SupervisorID" Display="false" />
                                <telerik:GridBoundColumn HeaderText="xCustAcctNum" UniqueName="xCustAcctNum" DataField="CustomerAccount" Display="false" />
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:UtilityWizardsConnectionString %>" SelectCommand="procRecentWorkOrders" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="lblClientID" Name="clientID" PropertyName="Text" Type="Int32" />
                            <asp:ControlParameter ControlID="lblUserID" Name="userID" PropertyName="Text" Type="Int32" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </td>
            </tr>
        </table>
        <asp:Panel runat="server" ID="pnlHidden" Visible="false" BackColor="#CC0000">
            <asp:Label ID="lblClientID" runat="server"></asp:Label>
            <asp:Label ID="lblUserID" runat="server"></asp:Label>
        </asp:Panel>
    </form>
</body>
</html>
