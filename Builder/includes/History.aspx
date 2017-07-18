<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="History.aspx.vb" Inherits="UtilityWizards.Builder.History" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="refresh" content="45">
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
        <div>
            <asp:Table runat="server" ID="tblHistory" Width="100%" CellPadding="1" CellSpacing="2" />
        </div>
    </form>
</body>
</html>
