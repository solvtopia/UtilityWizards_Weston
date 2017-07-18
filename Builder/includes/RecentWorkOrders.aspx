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
                    <telerik:RadGrid ID="RadSearchGrid" runat="server" AllowSorting="True" GroupPanelPosition="Top" AutoGenerateEditColumn="True" Skin="Metro" CellSpacing="-1" GridLines="Both">
                    </telerik:RadGrid>
                    <telerik:RadGrid ID="RadSearchGridMobile" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1" GroupPanelPosition="Top" Skin="MetroTouch">
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
                                                                <asp:Label ID='lblDescription' runat='server' ForeColor='White' Text='<%# Eval("CustomerAddress") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID='lblAssignedTo' runat='server' ForeColor='White' Text='<%# Eval("AssignedToName") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn HeaderText="Priority" UniqueName="Priority" DataField="xPriority" Display="false" />
                                <telerik:GridBoundColumn HeaderText="ID" UniqueName="ID" DataField="ID" Display="false" />
                                <telerik:GridBoundColumn HeaderText="ModuleID" UniqueName="xModuleID" DataField="xModuleID" Display="false" />
                                <telerik:GridBoundColumn HeaderText="FolderID" UniqueName="xFolderID" DataField="xFolderID" Display="false" />
                                <telerik:GridBoundColumn HeaderText="SupervisorID" UniqueName="SupervisorID" DataField="SupervisorID" Display="false" />
                                <telerik:GridBoundColumn HeaderText="TechnicianID" UniqueName="TechnicianID" DataField="TechnicianID" Display="false" />
                                <telerik:GridBoundColumn HeaderText="xCustAcctNum" UniqueName="xCustAcctNum" DataField="xCustAcctNum" Display="false" />
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:UtilityWizardsConnectionString %>"></asp:SqlDataSource>
                </td>
            </tr>
        </table>
        <asp:Panel runat="server" ID="pnlHidden" Visible="false" BackColor="#CC0000">
            <asp:Label ID="lblFields" runat="server"></asp:Label>
            <asp:Label ID="lblClientID" runat="server"></asp:Label>
            <asp:Label ID="lblModuleID" runat="server"></asp:Label>
            <asp:Label ID="lblCustAcctNum" runat="server"></asp:Label>
            <asp:Label ID="lblFilterID" runat="server"></asp:Label>
        </asp:Panel>
    </form>
</body>
</html>
