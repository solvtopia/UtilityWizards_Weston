﻿<%@ Page Title="UtilityWizards.API &gt; OutputController" Language="vb" AutoEventWireup="false" MasterPageFile="~/masterPages/InformationPopUp.master" CodeBehind="OutputController.aspx.vb" Inherits="UtilityWizards.API.OutputController1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="informationHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="informationContent_Ajax" runat="server">
    <table class="auto-style1">
        <tr>
            <td style="width: 20%;">Namespace</td>
            <td style="border: 1px solid #C0C0C0; vertical-align: top; background-color: #E6E6E6; padding-left: 10px;">UtilityWizards.API</td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td style="width: 20%;">Service Name</td>
            <td style="border: 1px solid #C0C0C0; vertical-align: top; background-color: #E6E6E6; padding-left: 10px;">
                <asp:HyperLink ID="HyperLink1" runat="server" ForeColor="Black" NavigateUrl="OutputController.aspx">OutputController</asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td style="width: 20%;">Service Url</td>
            <td style="border: 1px solid #C0C0C0; vertical-align: top; background-color: #E6E6E6; padding-left: 10px;">https://westonapi.utilitywizards.com/OutputController.asmx</td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td style="width: 20%; vertical-align: top;">Available Methods</td>
            <td style="border: 1px solid #C0C0C0; vertical-align: top; background-color: #E6E6E6; padding-left: 10px;">
                <table cellpadding="0" cellspacing="0" class="auto-style1">
                    <tr>
                        <td style="vertical-align: top; width: 25%;">
                            <asp:HyperLink ID="HyperLink5" runat="server" ForeColor="Black" NavigateUrl="GetAllCustomers.aspx">GetAllCustomers</asp:HyperLink>
                        </td>
                        <td style="vertical-align: top;">&nbsp;</td>
                        <td style="vertical-align: top; width: 50%;">&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 25%;">
                            <asp:HyperLink ID="HyperLink8" runat="server" ForeColor="Black" NavigateUrl="GetCustomerByAcctNum.aspx">GetCustomerByAcctNum</asp:HyperLink>
                        </td>
                        <td style="vertical-align: top;">&nbsp;</td>
                        <td style="vertical-align: top; width: 50%;">&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 25%;">
                            <asp:HyperLink ID="HyperLink9" runat="server" ForeColor="Black" NavigateUrl="GetApiKey.aspx">GetApiKey</asp:HyperLink>
                        </td>
                        <td style="vertical-align: top;">&nbsp;</td>
                        <td style="vertical-align: top; width: 50%;">&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 25%;">
                            <asp:HyperLink ID="HyperLink6" runat="server" ForeColor="Black" NavigateUrl="CheckLogin_M.aspx">CheckLogin_M</asp:HyperLink>
                        </td>
                        <td style="vertical-align: top;" colspan="2">* For use on Mobile Devices</td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 25%;">
                            <asp:HyperLink ID="HyperLink7" runat="server" ForeColor="Black" NavigateUrl="GetOpenWorkOrders_M.aspx">GetOpenWorkOrders_M</asp:HyperLink>
                        </td>
                        <td style="vertical-align: top;" colspan="2">* For use on Mobile Devices</td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 25%;">
                            <asp:HyperLink ID="HyperLink10" runat="server" ForeColor="Black" NavigateUrl="GetOpenWorkOrderCount_M.aspx">GetOpenWorkOrderCount_M</asp:HyperLink>
                        </td>
                        <td style="vertical-align: top;" colspan="2">* For use on Mobile Devices</td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td colspan="2">Sample VB.NET Code</td>
        </tr>
        <tr>
            <td colspan="2" style="border: 1px solid #C0C0C0; vertical-align: top; background-color: #E6E6E6; padding-left: 10px; font-family: 'Courier New', Courier, monospace;">Dim svcOut As New UWOutput.OutputController</td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td colspan="2">Sample C#.NET Code</td>
        </tr>
        <tr>
            <td colspan="2" style="border: 1px solid #C0C0C0; vertical-align: top; background-color: #E6E6E6; padding-left: 10px; font-family: 'Courier New', Courier, monospace;">UWOutput.OutputController svcOut = new UWOutput.OutputController();</td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="informationInfoContent" runat="server">
    <asp:Panel ID="Panel1" runat="server">
        The OutputController service is used to transfer data from within the Utility Wizards system to a 3rd Party Vendor without giving full access to the main system databases.<br />
        <br />
        Methods that are designed for use on Mobile Devices are designated by an &quot;_M&quot; at the end of the routine name.<br />
        <br />
        Some classes and objects within the API have been specifically designed for use with the Mobile routines and are automatically converted by the OutputController to standard Utility Wizards objects.
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="informationContent_OutsideAjax" runat="server">
</asp:Content>
