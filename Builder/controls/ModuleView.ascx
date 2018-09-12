<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ModuleView.ascx.vb" Inherits="UtilityWizards.Builder.ModuleView" %>
<table class="nav-justified" style="padding: 20px;">
    <tr>
        <td style="vertical-align: top;">
            <div class="box box-info" runat="server" id="boxTopLeft">
                <div class="box-header with-border">
                    <h3 class="box-title">
                        <asp:TextBox runat="server" ID="txtTopLeftTitle" Width="100%" placeholder="Top-Left Title" /><asp:Label runat="server" ID="lblTopLeftTitle" />
                    </h3>
                </div>
                <asp:Table ID="tblModuleQuestions_TopLeft" runat="server" CellPadding="1" CellSpacing="2" Width="100%" />
            </div>
        </td>
        <td>
            <img src="../images/spacer.png" style="width: 20px; height: 10px;" alt="" /></td>
        <td style="vertical-align: top;">
            <div class="box box-info" runat="server" id="boxTopMiddle">
                <div class="box-header with-border">
                    <h3 class="box-title">
                        <asp:TextBox runat="server" ID="txtTopMiddleTitle" Width="100%" placeholder="Top-Middle Title" /><asp:Label runat="server" ID="lblTopMiddleTitle" /></h3>
                </div>
                <asp:Table ID="tblModuleQuestions_TopMiddle" runat="server" CellPadding="1" CellSpacing="2" Width="100%" />
            </div>
        </td>
        <td>
            <img src="../images/spacer.png" style="width: 20px; height: 10px;" alt="" /></td>
        <td style="vertical-align: top;">
            <div class="box box-info" runat="server" id="boxTopRight">
                <div class="box-header with-border">
                    <h3 class="box-title">
                        <asp:TextBox runat="server" ID="txtTopRightTitle" Width="100%" placeholder="Top-Right Title" /><asp:Label runat="server" ID="lblTopRightTitle" /></h3>
                </div>
                <asp:Table ID="tblModuleQuestions_TopRight" runat="server" CellPadding="1" CellSpacing="2" Width="100%" />
            </div>
        </td>
    </tr>
    <tr>
        <td colspan="3" style="vertical-align: top;">
            <div class="box box-info" runat="server" id="boxFullPage">
                <div class="box-header with-border">
                    <h3 class="box-title">
                        <asp:TextBox runat="server" ID="txtFullPageTitle" Width="100%" placeholder="Full Page Title" /><asp:Label runat="server" ID="lblFullPageTitle" /></h3>
                </div>
                <asp:Table ID="tblModuleQuestions_FullPage" runat="server" CellPadding="1" CellSpacing="2" Width="100%" />
            </div>
        </td>
    </tr>
    <tr>
        <td style="vertical-align: top;">
            <div class="box box-info" runat="server" id="boxBottomLeft">
                <div class="box-header with-border">
                    <h3 class="box-title">
                        <asp:TextBox runat="server" ID="txtBottomLeftTitle" Width="100%" placeholder="Bottom-Left Title" /><asp:Label runat="server" ID="lblBottomLeftTitle" /></h3>
                </div>
                <asp:Table ID="tblModuleQuestions_BottomLeft" runat="server" CellPadding="1" CellSpacing="2" Width="100%" />
            </div>
        </td>
        <td>
            <img src="../images/spacer.png" style="width: 20px; height: 10px;" alt="" /></td>
        <td style="vertical-align: top;">
            <div class="box box-info" runat="server" id="boxBottomMiddle">
                <div class="box-header with-border">
                    <h3 class="box-title">
                        <asp:TextBox runat="server" ID="txtBottomMiddleTitle" Width="100%" placeholder="Bottom-Middle Title" /><asp:Label runat="server" ID="lblBottomMiddleTitle" /></h3>
                </div>
                <asp:Table ID="tblModuleQuestions_BottomMiddle" runat="server" CellPadding="1" CellSpacing="2" Width="100%" />
            </div>
        </td>
        <td>
            <img src="../images/spacer.png" style="width: 20px; height: 10px;" alt="" /></td>
        <td style="vertical-align: top;">
            <div class="box box-info" runat="server" id="boxBottomRight">
                <div class="box-header with-border">
                    <h3 class="box-title">
                        <asp:TextBox runat="server" ID="txtBottomRightTitle" Width="100%" placeholder="Bottom-Right Title" /><asp:Label runat="server" ID="lblBottomRightTitle" /></h3>
                </div>
                <asp:Table ID="tblModuleQuestions_BottomRight" runat="server" CellPadding="1" CellSpacing="2" Width="100%" />
            </div>
        </td>
    </tr>
</table>
