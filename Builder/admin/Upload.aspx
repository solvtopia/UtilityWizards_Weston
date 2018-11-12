<%@ Page Title="File Processor Uploads" Language="vb" AutoEventWireup="false" MasterPageFile="~/masterPages/ContentPage.master" CodeBehind="Upload.aspx.vb" Inherits="UtilityWizards.Builder.Upload" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ MasterType VirtualPath="~/masterPages/ContentPage.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <style type="text/css">
        .wrap-table {
            width: 100%;
            display: block;
        }

            .wrap-table td {
                display: inline-block;
            }

        .DropZone {
            width: 100%;
            height: 300px;
            background-color: #CCCCCC;
            border-color: #CCCCCC;
            color: #767676;
            float: left;
            text-align: center;
            vertical-align:middle;
            font-size: 16px;
            color: white;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="menuContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="breadcrumbContent" runat="server">
    <h1>Dashboard
        <small>Version 2.2
            <asp:Label runat="server" ID="lblSandbox" /></small>
    </h1>
    <ol class="breadcrumb">
        <li><a href="..Default.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
        <li><a href="#">Admin</a></li>
        <li class="active">File Processor Uploads</li>
    </ol>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pageContent_Ajax" runat="server">
    <telerik:RadTabStrip ID="RadTabStrip1" runat="server" SelectedIndex="0" MultiPageID="RadMultiPage1" Skin="Metro">
        <Tabs>
            <telerik:RadTab runat="server" Text="Upload Files" PageViewID="RadPageView1" Selected="true">
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="File Processor Log" PageViewID="RadPageView2">
            </telerik:RadTab>
        </Tabs>
    </telerik:RadTabStrip>
    <telerik:RadMultiPage ID="RadMultiPage1" runat="server" SelectedIndex="0" Width="100%">
        <telerik:RadPageView ID="RadPageView1" runat="server" Selected="true">
            <telerik:RadAsyncUpload RenderMode="Lightweight" runat="server" OnClientFilesUploaded="OnClientFilesUploaded" ID="RadAsyncUpload" MultipleFileSelection="Automatic" EnableInlineProgress="true" Skin="Metro" Width="100%" DropZones=".DropZone" />
            <telerik:RadProgressManager runat="server" ID="RadProgressManager" Skin="Metro" Width="100%" />
            <telerik:RadProgressArea RenderMode="Lightweight" runat="server" ID="RadProgressArea" Skin="Metro" Width="100%" />
            <asp:Button runat="server" ID="Upload" Style="display: none;" />
            <div class="DropZone">
                <p>Drop Files Here</p>
            </div>
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView2" runat="server">
        </telerik:RadPageView>
    </telerik:RadMultiPage>
    <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
        <script type="text/javascript">
            function OnClientFilesUploaded(sender) {
                var $ = $telerik.$;
                //$('#Upload').click();
                __doPostBack('<%=Upload.ClientID %>', '');
            }

            //<![CDATA[
            Sys.Application.add_load(function () {
                demo.initialize();
            });
            //]]>

            (function () {
                var $;
                var demo = window.demo = window.demo || {};

                demo.initialize = function () {
                    $ = $telerik.$;

                    if (!Telerik.Web.UI.RadAsyncUpload.Modules.FileApi.isAvailable()) {
                        $(".DropZone").html("<strong>Your browser does not support Drag and Drop. Please take a look at the info box for additional information.</strong>");
                    }
                    else {
                        $(document).bind({ "drop": function (e) { e.preventDefault(); } });

                        var dropZone1 = $(document).find(".DropZone");
                        dropZone1.bind({ "dragenter": function (e) { dragEnterHandler(e, dropZone1); } })
                            .bind({ "dragleave": function (e) { dragLeaveHandler(e, dropZone1); } })
                            .bind({ "drop": function (e) { dropHandler(e, dropZone1); } });

                        var dropZone2 = $(document).find("#DropZone");
                        dropZone2.bind({ "dragenter": function (e) { dragEnterHandler(e, dropZone2); } })
                            .bind({ "dragleave": function (e) { dragLeaveHandler(e, dropZone2); } })
                            .bind({ "drop": function (e) { dropHandler(e, dropZone2); } });
                    }
                };

                function dropHandler(e, dropZone) {
                    dropZone[0].style.backgroundColor = "#CCCCCC";
                }

                function dragEnterHandler(e, dropZone) {
                    var dt = e.originalEvent.dataTransfer;
                    var isFile = (dt.types !== null && (dt.types.indexOf ? dt.types.indexOf('Files') !== -1 : dt.types.contains('application/x-moz-file')));
                    if (isFile || $telerik.isSafari5 || $telerik.isIE10Mode || $telerik.isOpera)
                        dropZone[0].style.backgroundColor = "#0094ff";
                }

                function dragLeaveHandler(e, dropZone) {
                    if (!$telerik.isMouseOverElement(dropZone[0], e.originalEvent))
                        dropZone[0].style.backgroundColor = "#CCCCCC";
                }


            })();
        </script>
    </telerik:RadScriptBlock>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="pageContent_OutsideAjax" runat="server">
</asp:Content>
