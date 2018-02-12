<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Print.aspx.vb" Inherits="UtilityWizards.Builder.Print" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<%@ Register Assembly="UtilityWizards.CommonCore.Shared" Namespace="UtilityWizards.CommonCore.Shared.Controls.TextBoxes" TagPrefix="Builder" %>
<%@ Register Assembly="UtilityWizards.CommonCore.Shared" Namespace="UtilityWizards.CommonCore.Shared.Controls.CheckBoxes" TagPrefix="Builder" %>
<%@ Register Assembly="UtilityWizards.CommonCore.Shared" Namespace="UtilityWizards.CommonCore.Shared.Controls.DropDownLists" TagPrefix="Builder" %>
<%@ Register Assembly="UtilityWizards.CommonCore.Shared" Namespace="UtilityWizards.CommonCore.Shared.Controls.Labels" TagPrefix="Builder" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../styles/utilityWizards.css" rel="stylesheet" />
    <style type="text/css">
        .wrap-table-module {
            width: 100%;
            display: block;
        }

            .wrap-table-module td {
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
        <asp:Table ID="tblFolderQuestions" runat="server" CellPadding="1" CellSpacing="2" Width="100%" />
        <asp:Table ID="tblModuleQuestions" runat="server" CellPadding="1" CellSpacing="2" Width="100%" />
        <table class="wrap-table-module">
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblAcctNum">Customer Account Number</asp:Label></td>
            </tr>
            <tr>
                <td>
                    <Builder:DataLabel ID="txtAcctNumber" runat="server" DataFieldName="CustAcctNum" ReadOnly="True" Width="100%" />
                </td>
            </tr>
            <tr>
                <td>Name</td>
            </tr>
            <tr>
                <td>
                    <Builder:DataLabel ID="txtCustomerName" runat="server" DataFieldName="CustomerName" ReadOnly="True" Width="100%">
                    </Builder:DataLabel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblLocationNum">Location Number</asp:Label></td>
            </tr>
            <tr>
                <td>
                    <Builder:DataLabel ID="txtLocationNum" runat="server" DataFieldName="LocationNum" ReadOnly="True" Width="100%" />
                </td>
            </tr>
            <tr>
                <td style="vertical-align: top;">Service Address</td>
            </tr>
            <tr>
                <td>
                    <Builder:DataLabel ID="txtCustomerServiceAddress" runat="server" DataFieldName="CustomerServiceAddress" ReadOnly="True" Rows="3" TextMode="MultiLine" Width="100%">
                    </Builder:DataLabel>
                </td>
            </tr>
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
                    <Builder:DataLabel ID="ddlStatus" runat="server" DataFieldName="Status" Width="100%" />
                </td>
            </tr>
            <tr>
                <td>Priority</td>
            </tr>
            <tr>
                <td>
                    <Builder:DataLabel ID="ddlPriority" runat="server" DataFieldName="Priority" Width="100%" AutoPostBack="true" />
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
                    <Builder:DataLabel ID="ddlSupervisor" runat="server" DataFieldName="Supervisor" Width="100%" AutoPostBack="true" />
                </td>
            </tr>
            <tr>
                <td>Technician</td>
            </tr>
            <tr>
                <td>
                    <Builder:DataLabel ID="ddlTechnician" runat="server" DataFieldName="Technician" Width="100%" />
                </td>
            </tr>
            <tr>
                <td>Technician Comments</td>
            </tr>
            <tr>
                <td>
                    <Builder:DataLabel ID="txtTechComments" runat="server" DataFieldName="TechnicianComments" Rows="3" TextMode="MultiLine" Width="100%" />
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
            </tr>
        </table>
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
        <table class="wrap-table-module">
            <tr><td>Ticket Text</td></tr>
            <tr>
                <td>
                    <Builder:DataLabel ID="txtPrintView" runat="server" DataFieldName="printable_text" ReadOnly="True" Rows="30" TextMode="MultiLine" Width="100%">
                    </Builder:DataLabel>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
