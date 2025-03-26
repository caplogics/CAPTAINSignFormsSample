$(document).ready(function () {
    //    $body = $("body");
    //    $(document).on({
    //        ajaxStart: function () { $('#load').css("display", "block"); },//$body.addClass("loading");
    //        ajaxStop: function () {  $('#load').css("display", "nonw");  } //$body.removeClass("loading");
    //    });


});

/************************************** POST Functions*************************/

function fn_InsUpDel(strUrl, parameters) {
    //debugger;
    $('#load').css("display", "block");
    var jqXHR = $.ajax({
        async: false,
        cache: false,
        type: "POST",
        url: strUrl,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(parameters),
        dataType: "json",
        success: function (response) {
            if (response.d == 'success') {
            }
        },
        complete: function () {
            $('#load').css("display", "none");
        },
        failure: function (response) {
           // debugger;
            // alert(response.d);
            //if ($("#hdnAcntTyp").val() == "Rep")
            //    location.replace("../index.aspx?Agy=APC&typ=Rep");
            //else
            //    location.replace("../index.aspx?Agy=APC&typ=Admn");
        },
        error: function (response) {
           // debugger;
            // alert(response);
            //if ($("#hdnAcntTyp").val() == "Rep")
            //    location.replace("../index.aspx?Agy=APC&typ=Rep");
            //else
            //    location.replace("../index.aspx?Agy=APC&typ=Admn");
        }
    });
    return jqXHR.responseText;
}
function fn_GetData(strUrl, parameters) {
    //debugger;
    $('#load').css("display", "block");
    var jqXHR = $.ajax({
        async: false,
        cache: false,
        type: "POST",
        url: strUrl,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(parameters),
        dataType: "json",
        success: function (response) {
            if (response.d != '' && response.d != 'null') {
            }
        },
        complete: function () {
            $('#load').css("display", "none");
        },
        failure: function (response) {
            //debugger;
            //if ($("#hdnAcntTyp").val() == "Rep")
            //    location.replace("../index.aspx?Agy=APC&typ=Rep");
            //else
            //    location.replace("../index.aspx?Agy=APC&typ=Admn");
            // alert(response.d);
        },
        error: function (response) {
            //debugger;
            
           
            //if ($("#hdnAcntTyp").val() == "Rep")
            //    location.replace("../index.aspx?Agy=APC&typ=Rep");
            //else
            //    location.replace("../index.aspx?Agy=APC&typ=Admn");
            //alert(response);

            //console.log(response.responseText);
        }
    });
    //console.clear()
   // console.log(JSON.parse(jqXHR.responseText).d);
    return JSON.parse(jqXHR.responseText).d;
}
function fn_GetData_noParams(strUrl) {
    //debugger;
    var jqXHR = $.ajax({
        async: false,
        cache: false,
        type: "POST",
        url: strUrl,
        contentType: "application/json; charset=utf-8",
        data: "",
        dataType: "json",
        success: function (response) {
            if (response.d != '' && response.d != 'null') {
            }
        },
        failure: function (response) {
            //debugger;
            ////alert(response.d);
            //if ($("#hdnAcntTyp").val() == "Rep")
            //    location.replace("../index.aspx?Agy=APC&typ=Rep");
            //else
            //    location.replace("../index.aspx?Agy=APC&typ=Admn");
        },
        error: function (response) {
            //debugger;
            //// alert(response);
            //if ($("#hdnAcntTyp").val() == "Rep")
            //    location.replace("../index.aspx?Agy=APC&typ=Rep");
            //else
            //    location.replace("../index.aspx?Agy=APC&typ=Admn");
        }
    });
    return JSON.parse(jqXHR.responseText).d;
}
function fn_set_Session_Variables(strUrl, parameters) {
    // debugger;
    var jqXHR = $.ajax({
        async: false,
        cache: false,
        type: "POST",
        url: strUrl,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(parameters),
        dataType: "json",
        success: function (response) {
            if (response.d != 'success') {
            }
        },
        failure: function (response) {
            //debugger;
            ////alert(response.d);
            //if ($("#hdnAcntTyp").val() == "Rep")
            //    location.replace("../index.aspx?Agy=APC&typ=Rep");
            //else
            //    location.replace("../index.aspx?Agy=APC&typ=Admn");
        },
        error: function (response) {
            //debugger;
            //// alert(response);
            //if ($("#hdnAcntTyp").val() == "Rep")
            //    location.replace("../index.aspx?Agy=APC&typ=Rep");
            //else
            //    location.replace("../index.aspx?Agy=APC&typ=Admn");
        }
    });
    return jqXHR.responseText;
}

/************************************************************************************/////

function GetQueryStringValues(param) {
    var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < url.length; i++) {
        var urlparam = url[i].split('=');
        if (urlparam[0] == param) {
            return urlparam[1];
        }
    }
}

//function GetMultipleQueryStringValues(hash) {
//    // Multiple parameters 
//    var vars = [], hash;
//    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
//    for (var i = 0; i < hashes.length; i++) {
//        hash = hashes[i].split('=');
//        vars.push(hash[0]);
//        vars[hash[0]] = hash[1];
//    }
//    return vars;
//}

function chkSelectAll(objchkboxAll, chktbl) {
    debugger;
    if ($(objchkboxAll).is(':checked')) {
        $(chktbl).find('tr').each(function () {
            var row = $(this);
            $("td", row).find('input[type="checkbox"]').prop('checked', true);
        });
    } else {
        $(chktbl).find('tr').each(function () {
            var row = $(this);
            $("td", row).find('input[type="checkbox"]').prop('checked', false);
        });
    }
}

function fnDateConvertion(strDate) {
    var date = new Date(strDate);
    date = ((date.getMonth() > 8) ? (date.getMonth() + 1) : ('0' + (date.getMonth() + 1))) + '/' + ((date.getDate() > 9) ? date.getDate() : ('0' + date.getDate())) + '/' + date.getFullYear();

    return date;
}
function onlyNumbers(evt) {
    var e = event || evt; // for trans-browser compatibility
    var charCode = e.which || e.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    return true;
}
function DecimalsKey(evt) {
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode != 46 && charCode > 31
      && (charCode < 48 || charCode > 57))
        return false;

    return true;
}
function SpecialChars(e) {
    debugger;
    var strInput = $(e).val();
    //var regexprn = new RegExp("^[a-zA-Z0-9]+$");
    var regexprn = /[`~!@#$%^&*()|+\=?;:'",.<>\{\}\[\]\\\/]/gi;
    var isValid = regexprn.test((strInput));
    if (isValid) {
        //alert("spchar");
        var no_spl_char = strInput.replace(/[`~!@#$%^&*()|+\=?;:'",.<>\{\}\[\]\\\/]/gi, '');
        $(e).val(no_spl_char);
        // $(e).val(strInput.slice(0, -1));
    }
    return isValid;
}
function pad(d) {
    return (d < 10) ? '0' + d.toString() : d.toString();
}
function camelize(str) {
    return str.replace(/(?:^|\s)\w/g, function (match) {
        return match.toUpperCase();
    });
}
function camelize1(str) {
    return str.replace(/(?:^\w|[A-Z]|\b\w)/g, function (word, index) {
        return index === 0 ? word.toUpperCase() : word.toLowerCase();
    });
}
function keyTabpress(event) {
    const eventCode = event.which || event.keyCode;
    if (eventCode === 9 || eventCode === 13) {
        var $canfocus = $(':focusable');
        var index = $canfocus.index(document.activeElement);
        if (index >= $canfocus.length) index = 0;
        $canfocus.eq(index).focus();
    }
}
function _calculateAge(birthday) {
    return ~~((new Date() - new Date(birthday)) / (31556952000))
}


/***************Message Boxes****************************************************/////////////
function ConfirmAlert(txtMsg) {
    MBox_cATC("<div style='text-align:center;'><div>" + txtMsg + "<div/><div><button onclick='btnyesno_Click(true)' style='padding:5px; border:1px solid #fff;    background-color: transparent;color: #fff;margin-right: 10px;'>Yes</button><button  onclick='btnyesno_Click(false)' style='padding:5px; border:1px solid #fff;    background-color: transparent;color: #fff;margin-right: 10px;'>No</button><div/><div/>");

}
function btnyesno_Click(status) {
    //event.preventDefault();
    if (status == false) {
        $(".jq-toast-single").css("display", "none");
    }

    return status;
}

function MBox_sTC(textMessage) {
    $.toast({
        heading: 'Success',
        text: textMessage,
        position: 'center',
        showHideTransition: 'slide',
        icon: 'success',
        loader: false
    });
}
function MBox_eTC(textMessage) {
    $.toast({
        heading: 'Error',
        text: textMessage,
        position: 'mid-center',
        showHideTransition: 'slide',
        icon: 'error',
        loader: false
    });
}
function MBox_vTC(textMessage) {
    $.toast({
        // heading: 'Alert',
        text: textMessage,
        bgColor: '#eb065b',
        position: 'mid-center',
        showHideTransition: 'slide',
        icon: 'warning',
        loader: false
    });
}
function MBox_dTC(textMessage) {
    $.toast({
        heading: 'Deleted',
        text: textMessage,
        position: 'center',
        showHideTransition: 'slide',
        icon: 'error',
        loader: false
    });
}
function MBox_cATC(textMessage) {
    $.toast({
        heading: 'Confirm',
        text: textMessage,
        position: 'mid-center',
        icon: 'error',
        loader: false,
        hideAfter: true
    });
}
function MBox_wTC(textMessage) {
    $.toast({
        heading: 'Warning',
        text: textMessage,
        position: 'top-center',
        showHideTransition: 'slide',
        icon: 'warning',
        loader: false

    });
}
function MBox_iTC(textMessage) {
    $.toast({
        bgColor: '#f93876',
        text: textMessage,
        position: 'mid-center',
        showHideTransition: 'slide',
        loader: false,
        hideAfter: true
    });
}
function MBox_nTC(textMessage) {
    $.toast({
        // heading: 'Alert',
        text: textMessage,
        bgColor: '#ff9900',
        position: 'mid-center',
        showHideTransition: 'slide',
        icon: 'info',
        hideAfter: true,
        textColor: '#fff',
        loader: false

    });
}
function MBox_Notification(textMessage, HeadText) {
    $.toast({
        heading: HeadText,
        text: textMessage,
        hideAfter: false,
        bgColor: '#feffc3',
        textColor: 'Black',
        position: 'bottom-center',
        stack: false
    });
}

/**********************************************************************************////////////