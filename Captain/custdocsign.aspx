<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="custdocsign.aspx.cs" Inherits="Captain.custdocsign" ValidateRequest="false" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <%--<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.7.1/jquery.min.js"></script>--%>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
    <script src="Resources/js/jquery.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
    <script src="Resources/js/APP_JS_Default_functions.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
    <link href="Resources/js/toaster/jquery.toast.css" rel="stylesheet" />
    <script src="Resources/js/toaster/jquery.toast.js"></script>
    <script src="Resources/js/moment.min.js"></script>

    <meta name="viewport" content="width=device-width, initial-scale=1, minimum-scale=1, maximum-scale=1, user-scalable=no" />

    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />

    <link rel="stylesheet" href="Resources/Css/signature-pad-custdocsign.css" />

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

    <title>::Doc Signature::</title>
    <style>
        html {
            height: 100%;
        }

        body {
            /*font-family: Verdana;*/
            /*  font-size: 1em;*/
            /* margin: 0px;
            padding: 0px;
            min-height: 100%;
            display: flex;
            flex-direction: column;*/
        }

        .logoTitle {
            color: #fff;
            font-weight: bold;
            font-size: 1.5em;
            padding-left: 20px;
            text-transform: uppercase;
        }

        .badge {
            background-color: #ffd800;
            color: #333;
            border-radius: 20px;
            padding: 3px 12px 5px 12px;
            /*margin-top: 8px;*/
            cursor: pointer;
        }

            .badge:hover {
                background-color: #ff9c00;
            }

        .badgedown {
            background-color: #40ef25;
            color: #333;
            border-radius: 20px;
            padding: 3px 12px 5px 12px;
           margin-top: 0px;
            cursor: pointer;
        }

            .badgedown:hover {
                background-color: #29de0d;
            }

        .btnDone {
            padding: 3px 12px 3px 12px;
            background-color: #ff6a00;
            color: #fff;
            border: 0px solid #ff6a00;
            border-radius: 20px;
            font-size: 0.80em;
            margin-top: 5px;
        }

        table {
            border-spacing: 0px;
        }

        th, td {
            border: 0px solid #333;
        }

        .hgt {
            /* height: 100vh;*/
            height: calc(100vh - 100px);
        }

        .content {
            flex: 1;
        }

        .pnlTabs {
            padding: 5px;
            background-color: #e5f2fe;
            border: 1px solid #cbe6ff;
            cursor: pointer;
        }

        .pnlTabsbody {
            background-color: #fff;
            padding: 5px;
            border: 1px solid #cbe6ff;
        }

        .divActive {
            background-color: #fff;
        }

        .divInActive {
            background-color: #e5f2fe;
        }

        .lstdocs ul {
            list-style: none;
            font-size: 13px;
            font-weight: 600;
            text-indent: 0px;
            padding: 0;
            padding-left: 5px;
        }

            .lstdocs ul li {
                padding: 5px;
                cursor: pointer;
            }

                .lstdocs ul li:hover {
                    background-color: #fbf7d9;
                    font-weight: bold;
                }

                .lstdocs ul li span {
                    padding-right: 5px;
                }

        .lstdocs .Active {
            background-color: #fbf7d9;
            font-weight: bold;
        }

        .clstblHist {
        }

            .clstblHist tr {
            }

                .clstblHist tr th {
                    background-color: #f6f6f6;
                    padding: 5px;
                    border: 1px solid #ccc;
                    font-size: 15px;
                    font-weight: 600;
                }

                .clstblHist tr td {
                    padding: 5px;
                    border: 1px solid #ccc;
                    font-size: 14px;
                }
    </style>
    <%--   <link rel="stylesheet" href="Resources/Css/signature-pad.css" />--%>
</head>
<body onselectstart="return false">
    <form id="form1" runat="server">
        <asp:HiddenField ID="hdnAppno" runat="server" />
        <asp:HiddenField ID="hdnDBShortname" runat="server" />

        <div class="container-fluid" style="background-color: #fff;">
            <div class="row" style="background-color: #fff;">
                <div class="col-sm-2"  >
                 <div id="dvtitle" class="logoTitle">
                    <asp:Image ID="mainlogo" runat="server" Width="100%" />
                   </div>

                     <div class="" style="background-color: #fafafa; padding: 0px;">
     
     <div class="lstdocs" id="dvlstdocs">
     </div>
 </div>
                
                </div>
                <div class="col-sm-10"  >
                    <div class="row" style="background-color: #3baae2; height:30px;">
                        <div class="col-sm-3">
                            <span class="badge badgedown " onclick="DownloadFiles()" style="margin-right: 10px;">Download</span>&nbsp;&nbsp;&nbsp;
                            <div id="dvSignBadge" class="pull-left">
                                <span id="spnSignBadge" class="badge" onclick="showSignPopup()">Click here to sign</span>
                            </div>
                        </div>
                        <div class="col-sm-3">
                            <input type="radio" id="rbtnSigndoc" name="docssign" value="docsign" onclick="rbtnSigndoc_CheckedChanged()">
                            <label for="docsign" onclick="rbtnSigndoc_CheckedChanged()">Sign on document</label>
                            &nbsp;&nbsp;&nbsp;  &nbsp;&nbsp;&nbsp; 
                        </div>
                       
                    <div class="col-sm-3">
                        <asp:Button ID="btnDone" runat="server" Text="Done & Send Signed Pdf" CssClass="btnDone" Visible="false" />
                    </div>
            </div>


                    <div class="row">
   
    <div class="hgt" style="background-color: #eaeaea;">
        <div style="background-color: #7a7a7a; padding: 3px 0 0 0;">
            <div id="TabdvdocPrv" style="display: inline-block; border: 1px solid #fff; margin-left: 8px; padding: 2px 8px 2px 8px; cursor: pointer; background-color: #fff; border-radius: 8px 8px 0 0;" data-type="docprv" onclick="TabPage_Click(this)">Document Preview</div>
            <div id="TabdvHistory" style="display: inline-block; border: 1px solid #fff; padding: 2px 8px 2px 8px; cursor: pointer; background-color: #e5f2fe; border-radius: 8px 8px 0 0;" data-type="dochist" onclick="TabPage_Click(this)">History</div>
        </div>
        <div id="dvSigndoc" style="display: none; width: 100%; height: 100%;">
            <iframe id="ifrmdoc" style="width: 100%; height: 100%; border: none; display: block;"></iframe>
        </div>
        <div id="dvSignupd" style="display: none; width: 100%; height: 100%; padding: 25px; background-color: #fff;">
            <div id="dvlstdocsHist">
            </div>
        </div>
    </div>
</div>
                </div>
            </div>
        </div>

       
        <footer class="footer" style="position: fixed; bottom: 0; background-color: #333; width: 100%; height: 40px; color: #ffd800; text-align: center;">2024@CAPTAIN</footer>
        <%--<div class="content">
            <table width="100%" style="border: 0px solid #ccc;" cell-spacing="0px">
                <tr>
                    <td style="height: 35px; background-color: #3baae2; padding: 0px; margin: 0px;">
                        <table style="width: 100%; padding: 0px; margin: 0px;">
                            <tr>
                                <td width="10%" class="logoTitle">ACCORD</td>
                                <td width="20%"><span class="badge">Click on the Document to sign</span></td>
                                <td width="15%">
                                    <asp:RadioButton ID="rbtnSigndoc" runat="server" Text="Sign on document" OnCheckedChanged="rbtnSigndoc_CheckedChanged" /></td>
                                <td width="25%">
                                    <asp:RadioButton ID="rbtnSign" runat="server" Text="Signature" OnCheckedChanged="rbtnSign_CheckedChanged" /></td>
                                <td width="30%">
                                    <asp:Button ID="btnDone" runat="server" Text="Done & Send Signed Pdf" CssClass="btnDone" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top;">
                        <table style="width: 100%" class="hgt">
                            <td style="width: 15%; background-color: #fff;"></td>
                            <td style="width: 85%; background-color: #eaeaea;"></td>
                        </table>
                    </td>
                </tr>
               
            </table>
        </div>
        <footer class="footer" style="position: fixed; bottom: 0; background-color: #333; width: 100%; height: 40px;">saasd</footer>--%>


        <!-- Modal -->
        <div class="modal" id="myModal" data-backdrop="static" data-keyboard="false">
            <div class="modal-dialog modal-dialog-centered modal-lg">
                <div class="modal-content">

                    <!-- Modal Header -->
                    <div class="modal-header">
                        <h4 class="modal-title">Signature</h4>
                        <button type="button" class="close" data-dismiss="modal" onclick="btnClose_Click()">&times;</button>
                    </div>

                    <!-- Modal body -->
                    <div class="modal-body">

                        <div class="row">
                            <div id="btndvSign" class="col-sm-6 pnlTabs" onclick="btndvSign_Click()">Draw Sign</div>
                            <%--<div id="btndvUpldSign" class="col-sm-6 pnlTabs" onclick="btndvUpldSign_Click()">Upload Sign</div>--%>
                        </div>
                        <div class="row">
                            <div id="dvSignDraw" class="pnlTabsbody">
                                <div id="signature-pad" class="signature-pad">
                                    <div style="height: 50px; margin-bottom: 10px;">
                                        <img id="imgSignView" runat="server" width="200" />
                                    </div>
                                    <div id="divCanvas" runat="server" class="signature-pad--body" style="background-color: #808080; height: 300px;">
                                        <canvas id="canvasImg" style="background-color: #d6fefe !important; width: 742px !important; height: 143px !important;" width="742" height="143"></canvas>
                                    </div>
                                    <div class="signature-pad--footer">
                                        <div id="dvSignNote" runat="server" class="description">Sign above</div>

                                        <div class="signature-pad--actions">
                                            <div class="" id="divclear" runat="server" style="width: 100%;">
                                                <button type="button" class="button clear" data-action="clear" title="Clear Signature" style="padding: 5px; background-color: #ff0000; border: 1px solid #a00606; border-radius: 8px; width: 70px; color: #fff; cursor: pointer; float: left;">Clear</button>
                                                <button type="button" class="button" data-action="change-color" style="display: none;">Change color</button>
                                                <button type="button" class="button" data-action="undo" style="display: none;">Undo</button>


                                                <button id="btnSignSave" type="button" class="btn btn-primary pull-right" style="padding: 5px; float: right; display: none;" onclick="btnSignSave_Click()">Save Signature</button>

                                                <button id="btnSignVerify" type="button" class="btn btn-warning pull-right" style="padding: 5px; float: right; margin-right: 10px;" onclick="btnSignVerify_Click()">Validate signature</button>
                                                &nbsp;  &nbsp;  &nbsp;  &nbsp;
                                            </div>

                                            <div>

                                                <asp:HiddenField ID="hdnImg" runat="server" />
                                                <img id="imgsign" runat="server" visible="false" />

                                            </div>

                                        </div>

                                    </div>
                                    <div style="align-content: center; font-size: 15px;" align="center">
                                        <asp:Label ID="lblmsg" runat="server" ForeColor="Red"></asp:Label>
                                    </div>

                                </div>
                            </div>

                            <div id="dvUploadSign" class="pnlTabsbody" style="display: none;">

                                <table width="100%">
                                    <tr>
                                        <td>File:</td>
                                        <td>
                                            <input type="file" id="file" /></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <input type="button" id="btnUpload" value="Upload" onclick="btnUpload_Click()" /></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2"><progress id="fileProgress" style="display: none"></progress></td>
                                    </tr>
                                </table>
                                <hr />
                                <span id="lblMessage" style="color: Green"></span>
                            </div>
                        </div>

                    </div>

                    <!-- Modal footer -->
                    <div class="modal-footer">
                        <%--<button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>--%>

                        <button type="button" class="btn btn-secondary" onclick="btnClose_Click()">Close</button>
                    </div>

                </div>
            </div>
        </div>

    </form>
    <script src="Resources/js/signature_pad.umd.js"></script>
    <script src="Resources/js/app.js"></script>

    <script type="text/javascript">

        var docMode = "";
        $(document).ready(function () {

            rbtnSigndoc_CheckedChanged();
            $("#btndvSign").removeClass("divInActive");
            $("#btndvSign").removeClass("divActive");

            $("#btndvUpldSign").addClass("divInActive");
            $("#btndvSign").addClass("divActive");
            docMode = "SD";

            //$("#dvtitle").html($("#hdnDBShortname").val());
            getFiles($("#hdnAppno").val());

        });

        function getFiles(Appno) {
            debugger;
            var dvlstdocs = $("#dvlstdocs");
            $('#dvlstdocs').empty();
            var params = {};
            params.Appno = Appno;
            // var strData = fn_GetData("custdocsign.aspx/WM_getAllFilesfromFolder", params);
            var strData = fn_GetData("custdocsign.aspx/WM_getAllFilesfromDB", params);
            if (strData != "" && strData != 'null') {
                var _dtFileslist = JSON.parse(strData);

                /********* Divide documents list into two signed and download docs ********/
                /*************************************************************************/
                var _dtSignDocs = _dtFileslist.filter(x => x.DCSN_HIS_SIGN_REQ == 'Y');
                var _dtDownldDocs = _dtFileslist.filter(x => x.DCSN_HIS_SIGN_REQ == 'N');
                /*************************************************************************/

                var strHTML = ""; var i = 0;
                strHTML = '<div style="height:300px;"><div style="background-color:#e5f2fe; padding:5px;"><b>Documents Require Signature</b></div>';
                strHTML += '<ul>';
                $.each(_dtSignDocs, function (index, value) {

                    var _iconHTML = '<span class="fa fa fa-file-text-o"></span>'; var _isActiveliColor = "style='color:#000;'";
                    if ($.trim(value.DCSN_HIS_SIGND_ON) != "") {
                        _iconHTML = '<span class="fa fa fa-check" style="color:#9d9ea0;"></span>';
                        _isActiveliColor = "style='color:#9d9ea0;'";
                    }

                    //var filename = value.split('//').pop();
                    strHTML += ' <li class="lstitems" ' + _isActiveliColor + '  data-hisid="' + value.DCSN_HIS_ID + '" data-agy="' + value.DCSN_HIS_AGY + '" data-dept="' + value.DCSN_HIS_DEPT + '" data-prog="' + value.DCSN_HIS_PROG + '" data-year="' + value.DCSN_HIS_YEAR + '" data-appno="' + value.DCSN_HIS_APPNO + '"  data-doccode="' + value.DCSN_HIS_DOC_CODE + '" data-signreq="' + value.DCSN_HIS_SIGN_REQ + '" data-signdate="' + value.DCSN_HIS_SIGND_ON + '" data-filename="' + value.DOCNAME + '" onclick="openpdfFile(this)">' + _iconHTML + '<a >' + value.DOCNAME + '</a></li>';
                });
                strHTML += ' </ul></div>';
                //dvlstdocs.append(strHTML);


                strHTML += '<div style="height:300px;"><div style="background-color:#e5f2fe; padding:5px;"><b>Download the Documents</b></div>';
                strHTML += '<ul>';
                $.each(_dtDownldDocs, function (index, value) {

                    var _iconHTML = '<span class="fa fa fa-file-text-o"></span>';
                    if ($.trim(value.DCSN_HIS_SIGND_ON) != "")
                        _iconHTML = '<span class="fa fa fa-check" style="color:green;"></span>';

                    //var filename = value.split('//').pop();
                    strHTML += ' <li class="lstitems"  data-hisid="' + value.DCSN_HIS_ID + '" data-agy="' + value.DCSN_HIS_AGY + '" data-dept="' + value.DCSN_HIS_DEPT + '" data-prog="' + value.DCSN_HIS_PROG + '" data-year="' + value.DCSN_HIS_YEAR + '" data-appno="' + value.DCSN_HIS_APPNO + '"  data-doccode="' + value.DCSN_HIS_DOC_CODE + '" data-signreq="' + value.DCSN_HIS_SIGN_REQ + '" data-signdate="' + value.DCSN_HIS_SIGND_ON + '" data-filename="' + value.DOCNAME + '" onclick="openpdfFile(this)">' + _iconHTML + '<a >' + value.DOCNAME + '</a></li>';
                });
                strHTML += ' </ul></div>';
                dvlstdocs.append(strHTML);



                openpdfFile($("ul li:first"));
            }
        }

        var _currdocPath = ""; var _currentli;
        function showSignPopup() {

            debugger;
            $("#myModal").modal("show");
            if (docMode == "SD")
                $("#btnSignSave").html("Sign on Document");
            else
                $("#btnSignSave").html("Save Signature");

            SignaturePadLoad();
        }
        var signaturePad = new SignaturePad(canvas, {
            backgroundColor: 'rgb(255, 255, 255)' // necessary for saving image as JPEG; can be removed is only saving as PNG or SVG
        });
        function btnSignVerify_Click() {



            if (signaturePad.isEmpty()) {
                return alert("Please provide a signature first.");
            }
            else {
                var c = document.getElementById("canvasImg");
                //c.width = "742";
                //c.height = "143";

                var strImage = c.toDataURL("image/png");
                if (strImage != "") {
                    var params = {};
                    params.oImage = strImage;
                    var strSignData = fn_GetData("custdocsign.aspx/WM_VerifySIGN", params);
                    if (strSignData == "success") {
                        $("#btnSignSave").css("display", "");
                    }
                    else {
                        $("#btnSignSave").css("display", "none");
                        alert("Please provide valid signature.");
                    }

                }
            }
        }


        function btnSignSave_Click() {
            debugger;
            var c = document.getElementById("canvasImg");
            var strImage = c.toDataURL("image/png");
            if (strImage != "") {

                var params = {};
                params.HISID = lstDocDets[0].hisid;
                params.Appno = $("#hdnAppno").val();
                params.oImage = strImage;
                params.odocPath = _currdocPath;
                params.odocCode = lstDocDets[0].doccode;
                params.docMode = docMode;
                params.docName = lstDocDets[0].docname;

                var strSignData = fn_GetData("custdocsign.aspx/WM_TakeSIGN", params);
                if (strSignData == "success") {
                    getFiles($("#hdnAppno").val());
                    //openpdfFile(_currentli);
                    MBox_sTC("Saved Successfully");
                    $("#myModal").modal("hide");
                }
                else {
                    alert("*Please provide a signature first.");
                }
            }

        }

        function btnClose_Click() {
            $("#myModal").modal("hide");
            if (docMode == "SN") {
                rbtnSigndoc_CheckedChanged();
            }
        }
        function btndvSign_Click() {

            $("#dvSignDraw").css("display", "block");
            $("#dvUploadSign").css("display", "none");

            $("#btndvSign").removeClass("divInActive");
            $("#btndvSign").removeClass("divActive");

            $("#btndvUpldSign").addClass("divInActive");
            $("#btndvSign").addClass("divActive");

        }

        function btndvUpldSign_Click() {
            $("#dvSignDraw").css("display", "none");
            $("#dvUploadSign").css("display", "block");


            $("#btndvUpldSign").removeClass("divInActive");
            $("#btndvUpldSign").removeClass("divActive");

            $("#btndvUpldSign").addClass("divActive");
            $("#btndvSign").addClass("divInActive");

        }

        function btnUpload_Click() {
            //var formData = new FormData();
            //formData.append("fileName", $("#fileName").val());
            //formData.append("file", $("#file")[0].files[0]);
            //$.ajax({
            //    url: 'UploadService.asmx/UploadFiles',
            //type: 'POST',
            //data: formData,
            //cache: false,
            //contentType: false,
            //processData: false,
            //success: function (fileName) {
            //    $("#fileProgress").hide();
            //$("#lblMessage").html("<b>" + fileName + "</b> has been uploaded.");
            //    },
            //xhr: function () {
            //        var fileXhr = $.ajaxSettings.xhr();
            //if (fileXhr.upload) {
            //    $("progress").show();
            //fileXhr.upload.addEventListener("progress", function (e) {
            //                if (e.lengthComputable) {
            //    $("#fileProgress").attr({
            //        value: e.loaded,
            //        max: e.total
            //    });
            //                }
            //            }, false);
            //        }
            //return fileXhr;
            //    }
            //});

        };

        function TabPage_Click(_currDiv) {


            var _typ = $(_currDiv).attr("data-type");
            if (_typ == 'docprv') {

                $("#dvSigndoc").css("display", "block");
                $("#dvSignupd").css("display", "none");
                docMode = "SD";

                $("#TabdvHistory").css("background-color", "#e5f2fe");

            }
            else if (_typ == 'dochist') {
                $("#dvSigndoc").css("display", "none");
                $("#dvSignupd").css("display", "block");

                //docMode = "SN";
                $("#TabdvdocPrv").css("background-color", "#e5f2fe");
                getDocsHistory();
            }
            $(_currDiv).css("background-color", "#fff");
        }

        function getDocsHistory() {

            var dvlstdocsHist = $("#dvlstdocsHist");
            $('#dvlstdocsHist').empty();
            var params = {};
            params.Appno = "";
            // var strData = fn_GetData("custdocsign.aspx/WM_getAllFilesfromFolder", params);
            var strData = fn_GetData("custdocsign.aspx/WM_getAllFilesfromDB", params);
            if (strData != "" && strData != 'null') {
                var _dtFileslist = JSON.parse(strData);

                /********* Divide documents list into two signed and download docs ********/
                /*************************************************************************/
                var _dtSignDocs = _dtFileslist.filter(x => x.DCSN_HIS_SIGN_REQ == 'Y');
                var _dtDownldDocs = _dtFileslist.filter(x => x.DCSN_HIS_SIGN_REQ == 'N');

                var strHTML = "";

                strHTML += "<div></div>";
                strHTML += "<div><table width='60%' class='clstblHist'>";
                strHTML += "<tr><td colspan='4' style='border:0px solid #ccc; padding:0px;'><h6 style='background-color:#cee4ff; padding:5px;margin-bottom: 0px;'>Documents that Require Signature</h6></td></tr>";
                strHTML += "<tr><th style='width:60%'> </th><th style='width:10%'>Signed</th><th style='width:20%'>Received</th><th style='width:10%'>Download</th></tr > "
                var SignOn = ""; var DocdwnldOn = "", strRecevideDate="";
                $.each(_dtSignDocs, function (index, value) {
                    SignOn = ""; DocdwnldOn = ""; strRecevideDate = "";
                    if (value.DCSN_HIS_SIGND_ON != null && $.trim(value.DCSN_HIS_SIGND_ON) != "")
                        SignOn = moment($.trim(value.DCSN_HIS_SIGND_ON)).format('MM/DD/YYYY');

                    if (value.DCSN_HIS_DOWNLOAD != null && $.trim(value.DCSN_HIS_DOWNLOAD) != "")
                        DocdwnldOn = moment($.trim(value.DCSN_HIS_DOWNLOAD)).format('MM/DD/YYYY');

                    if (value.DCSN_HIS_ADD_DATE != null && $.trim(value.DCSN_HIS_ADD_DATE) != "")
                        strRecevideDate = moment($.trim(value.DCSN_HIS_ADD_DATE)).format('MM/DD/YYYY hh:mm:ss A'); 

                    strHTML += "<tr>";
                    strHTML += "<td>" + value.DOCNAME + "</td>";
                    strHTML += "<td>" + SignOn + "</td>";
                    strHTML += "<td>" + strRecevideDate + "</td>";
                    strHTML += "<td>" + DocdwnldOn + "</td>";
                    strHTML += "</tr>";
                });

                strHTML += "<tr><td colspan='4' style='border:0px solid #ccc; padding:0px;'><h6 style='background-color:#cee4ff; padding:5px;margin-bottom: 0px; margin-top:5px;'>Documents for Client</h6></td></tr>";

                $.each(_dtDownldDocs, function (index, value) {
                    DocdwnldOn = ""; strRecevideDate = "";
                    if (value.DCSN_HIS_DOWNLOAD != null && $.trim(value.DCSN_HIS_DOWNLOAD) != "")
                        DocdwnldOn = moment($.trim(value.DCSN_HIS_DOWNLOAD)).format('MM/DD/YYYY');

                    if (value.DCSN_HIS_ADD_DATE != null && $.trim(value.DCSN_HIS_ADD_DATE) != "")
                        strRecevideDate = moment($.trim(value.DCSN_HIS_ADD_DATE)).format('MM/DD/YYYY hh:mm:ss A'); 

                    strHTML += "<tr>";
                    strHTML += "<td>" + value.DOCNAME + "</td>";
                    strHTML += "<td>N/A</td>";
                    strHTML += "<td>" + strRecevideDate + "</td>";
                    strHTML += "<td>" + DocdwnldOn + "</td>";
                    strHTML += "</tr>";
                });
                strHTML += "</table></div>";
                dvlstdocsHist.append(strHTML);
            };
        }

        function rbtnSigndoc_CheckedChanged() {
            $("#rbtnSigndoc").prop("checked", true);
            $("#rbtnSign").prop("checked", false);

            $("#dvSigndoc").css("display", "block");
            $("#dvSignupd").css("display", "none");
            docMode = "SD";
        }
        function rbtnSign_CheckedChanged() {
            $("#rbtnSign").prop("checked", true);
            $("#rbtnSigndoc").prop("checked", false);

            $("#dvSigndoc").css("display", "none");
            $("#dvSignupd").css("display", "block");

            docMode = "SN";
            $("#myModal").modal("show");

            SignaturePadLoad();
        }
        function openpdfFile(_lidoc) {
            debugger;
            _currentli = _lidoc;
            var _filename = $(_lidoc).attr("data-filename");
            _currdocPath = "/SignForms/" + $("#hdnAppno").val() + "/" + _filename + "";
            var _filepath = "/SignForms/" + $("#hdnAppno").val() + "/" + _filename + "";
            var params = {};
            params.filepath = _filepath;
            params.fileName = _filename;
            var strfileRootPath = fn_GetData("custdocsign.aspx/WM_getPath", params);
            $("#ifrmdoc").attr("src", strfileRootPath);
            $('#ifrmdoc').attr("src", $('#ifrmdoc').attr("src"));
            $(".lstitems").removeClass("Active");
            $(_lidoc).addClass("Active");

            var SignRequired = $(_lidoc).attr("data-signreq");
            var signedDate = $.trim($(_lidoc).attr("data-signdate"));

            lstDocDets = [];
            var _hisID = $(_lidoc).attr("data-hisid");
            var _agy = $(_lidoc).attr("data-agy");
            var _dept = $(_lidoc).attr("data-dept");
            var _prog = $(_lidoc).attr("data-prog");
            var _year = $(_lidoc).attr("data-year");
            var _appno = $(_lidoc).attr("data-appno");
            var _doccode = $(_lidoc).attr("data-doccode");

            lstDocDets.push({
                "hisid": _hisID,
                "agy": _agy,
                "dept": _dept,
                "prog": _prog,
                "year": _year,
                "appno": _appno,
                "doccode": _doccode,
                "signreq": SignRequired,
                "signdate": signedDate,
                "docname": _filename
            });

            debugger;
            if (SignRequired == "Y") {
                if (signedDate == "" || signedDate == 'null') {
                    $("#dvSignBadge").css("display", "block");

                }
                else
                    $("#dvSignBadge").css("display", "none");
            }
            else {
                $("#dvSignBadge").css("display", "none");
            }


        }

        function DownloadFiles() {

            var strfolderAppno = lstDocDets[0].agy + lstDocDets[0].dept + lstDocDets[0].prog + lstDocDets[0].appno;
            var filename = lstDocDets[0].docname;
            var hisID = lstDocDets[0].hisid;
            window.open("downloadfiles.aspx?typ=SD&fld=" + strfolderAppno + "&file=" + filename + "&hisid=" + hisID + "", "_blank");
            getDocsHistory();
        }


        var lstDocDets = [];

        function RefreshPdf() {
            // Get the current source of the iframe
            var src = $("#ifrmdoc").attr("src");
            // Set the source again to refresh the iframe
            $("#ifrmdoc").attr("src", src);
        }
    </script>
</body>
</html>
