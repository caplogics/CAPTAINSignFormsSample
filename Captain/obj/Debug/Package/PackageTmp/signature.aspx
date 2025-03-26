<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="signature.aspx.cs" Inherits="Captain.signature" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title>Signature Pad</title>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js"></script>
    <meta name="description" content="Signature Pad - HTML5 canvas based smooth signature drawing using variable width spline interpolation." />

    <meta name="viewport" content="width=device-width, initial-scale=1, minimum-scale=1, maximum-scale=1, user-scalable=no" />

    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />

    <link rel="stylesheet" href="Resources/Css/signature-pad.css" />

    <!--[if IE]>
    <link rel="stylesheet" type="text/css" href="css/ie9.css">
  <![endif]-->

    <script type="text/javascript">
        var _gaq = _gaq || [];
        _gaq.push(['_setAccount', 'UA-39365077-1']);
        _gaq.push(['_trackPageview']);

        (function () {
            var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
            ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
            var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
        })();
    </script>
</head>
<body onselectstart="return false">


     <div id="signature-pad" class="signature-pad">
         <div style="height:50px; margin-bottom:10px;">
            <img id="imgSignView" runat="server" width="200" />
        </div>
        <div id="divCanvas" runat="server" class="signature-pad--body" style="background-color: #808080; height:300px;">
            <canvas id="canvasImg"  style="background-color: #d6fefe !important"></canvas>
        </div>
        <div class="signature-pad--footer">
            <div id="dvSignNote" runat="server" class="description">Sign above</div>
            <form runat="server">
                <div class="signature-pad--actions">
                    <div id="divclear" runat="server">
                        <button type="button" class="button clear" data-action="clear" title="Clear Signature" style="padding: 5px; background-color: #ff0000; border: 1px solid #a00606; border-radius: 8px; width: 70px; color: #fff; cursor: pointer;">Clear</button>
                        <button type="button" class="button" data-action="change-color" style="display: none;">Change color</button>
                        <button type="button" class="button" data-action="undo" style="display: none;">Undo</button>

                    </div>
                    <div>

                        <asp:HiddenField ID="hdnImg" runat="server" />
                        <img id="imgsign" runat="server" visible="false" />
                        <asp:Button ID="btnSaveImage" runat="server" Text="Save" OnClientClick="saveImage()" OnClick="btnSaveImage_Click" ToolTip="Save" Style="padding: 5px; background-color: #0eb568; border: 1px solid #08864c; border-radius: 8px; width: 70px; color: #fff; cursor: pointer;" />
                        <%--<a href="index.html">b</a>--%>
                    </div>

                </div>
            </form>
        </div>
        <div style="align-content: center; font-size: 15px;" align="center">
            <asp:Label ID="lblmsg" runat="server" ForeColor="Red"></asp:Label>
        </div>
       
    </div>






    <script src="Resources/js/signature_pad.umd.js"></script>
    <script src="Resources/js/app.js"></script>

    <script>
        function saveImage() {
            var c = document.getElementById("canvasImg");
            var d = c.toDataURL("image/png");
            $('#<%=hdnImg.ClientID%>').val(d);
            //var w = window.open('about:blank', 'image from canvas');
            //w.document.write(document.getElementById("imgsign"));
        }
        function CloseWindow() {
            open(location, '_self').close();

            close();
        }
    </script>

</body>
</html>
