<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EventList.aspx.vb" Inherits="EventList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link href="vendors/bootsrap4.4.1/bootstrap.min.css" rel="stylesheet" />
    <link href="vendors/googlefont/icon.css" rel="stylesheet" />
    <script src="vendors/jquery3.4.1/jquery.min.js"></script>
    <script src="vendors/bootsrap4.4.1/bootstrap.min.js"></script>
    <link href="css/form.budle.css" rel="stylesheet" />
    <script src="js/form.budle.js"></script>
    <script src="js/form.submit.js"></script>
    <link href="vendors/Font-Awesome-4.7.0/css/font-awesome.min.css" rel="stylesheet" />
    <style>
        /*CSS*/

        body, html {
            background-image: url(https://www.nseed2u.com/webcore/img/intro-carousel/website2.jpg);
            font-family: -apple-system,BlinkMacSystemFont,"Segoe UI",Roboto,"Helvetica Neue",Arial,"Noto Sans",sans-serif,"Apple Color Emoji","Segoe UI Emoji","Segoe UI Symbol","Noto Color Emoji";
        }

        .event-div {
        padding:10px;
       
        }
    </style>


</head>

<body>
    <form id="form1" runat="server">
    </form>
    <div id="err_msgbox" class="modal">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header hidden" style="background: #F44336; color: white; padding: 10px 20px 10px 20px;">
                </div>
                <div class="modal-body" style="color: #555; text-align: center; padding-top: 20px">
                    <%--<img src="images/Msgbox/icons8-error-64.png" />--%>
                    <p style="margin-bottom: 0; font-size: 30px">
                        Sorry
                    </p>

                    <div style="margin-bottom: 0;">

                        <label id="err_msg" style="width: 90%; font-size: 13px; vertical-align: middle; margin: 0; display: inline;"></label>
                    </div>

                    <button type="button" class=" " data-dismiss="modal" style="margin-top: 10px; background: #F44336; border: none; font-size: 13px; padding: 7px; color: white; width: 100px; border-radius: 5px;">
                        Ok</button>
                </div>

            </div>
        </div>
    </div>
    <div class="containbody-form" style="border: 0px solid; /*max-width: 1014px; */ margin: 0 auto;">
        <%--  <label style="padding: 10px; font-weight: bold;">
                NEW / EDIT EXPO</label>
        --%>
        <div id="expoContainer" style="text-align: center;">
        </div>
        <div style="text-align: center;" id="viewmoreDiv">
            <span style="background: green; font-size: 16px; color: white; padding: 5px 15px; border-radius: 4px; display: inline-block; width: auto; cursor: pointer;"
                onclick="loadExpo('yes');">View more</span>
        </div>
    </div>

    <script>
        //Javascript
        $(document).ready(function () {
            loadExpo('no');
            // parent.hideloading();
            setTimeout(function () {
                parent.adjustIframeHeight();
                alignModal();
            }, 3000);

            // Align modal when it is displayed
            $(".modal").on("shown.bs.modal", alignModal);

            // Align modal when user resizes the window or iframe
            $(window).on("resize", function () {
                $(".modal:visible").each(alignModal);
            });
        });

        function alignModal() {
            var modalDialog = $(this).find(".modal-dialog");

            // Calculate the top margin to vertically center the modal dialog within the iframe
            var iframe = $(window.frameElement);
            var iframeHeight = iframe.height();
            var modalDialogHeight = modalDialog.height();
            var marginTop = Math.max(0, (iframeHeight - modalDialogHeight) / 2);

            // Applying the top margin on the modal dialog to align it vertically center
            modalDialog.css("margin-top", marginTop);
        }
        //$(document).ready(function () {

        //    loadExpo();
        //   // parent.hideloading();
        //     setTimeout(function () {
        //             parent.adjustIfameheight();
        //        }, 3000);
        //    // Align modal when it is displayed
        //    $(".modal").on("shown.bs.modal", alignModal);

        //    // Align modal when user resize the window
        //    $(window).on("resize", function () {
        //        $(".modal:visible").each(alignModal);
        //    });
        //});

        // function alignModal() {
        //    var modalDialog = $(this).find(".modal-dialog");

        //    // Applying the top margin on modal dialog to align it vertically center
        //    modalDialog.css("margin-top", Math.max(0, ($(window).height() - modalDialog.height()) / 2));
        //}

        function setmaxnumber(obj, maxno) {
            var value = $(obj).val();
            if ((value !== '') && (value.indexOf('.') === -1)) {

                var result = Math.max(Math.min(value, maxno), maxno - (maxno * 2));

                if (result <= 0) {
                    result = 0;
                }

                $(obj).val(result);
            }

        }

        function loadExpo(viewmore) {

            var code = getParameterByName('id');
            $.ajax({
                type: "POST",
                url: "EventList.aspx/loadExpo",
                data: JSON.stringify({
                    code: code,
                    viewmore: viewmore
                }),
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (s) {

                    var data = JSON.parse(s.d);
                    if (data.length > 0) {
                        $.each(data, function (i, item) {

                            //$('#fromDateDaylbl').text(item.FDay);
                            //$('#fromDateMonthlbl').text(item.FMonth);
                            //$('#fromDateYearlbl').text(item.FYear);


                            //$('#toDateDaylbl').text(item.TDay);
                            //$('#toDateMonthlbl').text(item.TMonth);
                            //$('#toDateYearlbl').text(item.TYear);

                            //$('#expoNamelbl').text(item.exponame);
                            //$('#venuelbl').text(item.venue);

                            //$('#expoDesclbl').text(item.expodesc);

                            //$('#expoBanner').attr('src', item.ExpoBannerPath);

                            var freebtn = '';
                            var labelStatus = 'Free Registration';

                            if (item.Validity == 'Expired') labelStatus = 'Event Ended';

                            // if (item.chargeable == '0') {
                            freebtn = '<label class="eventStatuslbl" style="color: red; font-size: 17px; background: white; padding: 0px 0px; border-radius: 4px;">' + labelStatus + '</label>';
                            //}

                            var expo = ' <div class="row" style="margin-bottom:10px; border: 1px solid #e3dbdb;position:relative;display:flex; border-radius: 5px;overflow: hidden;" onclick="viewDetails(\'EventDetails?id=' + item.expocode + '\',\'' + labelStatus + '\')"> ' +
                                '<div class="col-md-4 col-sm-12 col-xs-12 event-div" style=" " >' +
                                '<img id="expoBanner" style="border-radius: 8px;object-fit: contain;background-size: contain !important; background-position: center !important; max-width: 100%;  "' +
                                'src="' + item.ExpoBannerPath + '" />' +
                                '</div>' +
                                '<div class="col-md-8 col-sm-12 col-xs-12" style=" ; height: auto;  position: relative; ">' +
                                '<div class=" " style="line-height: 1;color: white;width: 100%;text-align: left;padding: 10px;">' +
                                '<div style="display: flex; margin-bottom: 15px;">' +
                                '<div style="font-size: 20px;color:#333;display: block; ">Event Date :  ' +
                                '<span id="fromDateDaylbl" style="font-size: 20px;margin-right:5px;font-weight: 500">' + item.FDay + '</span>' +
                                '<span id="fromDateMonthlbl" style="font-size: 20px;margin-right:5px ;font-weight: 500;">' + item.FMonth + '</span>' +
                                '<span id="fromDateYearlbl" style="font-size: 20px;font-weight: 500">' + item.FYear + '</span><span style="white-space:nowrap">(' + item.fromTime + ' )</span>' +
                                '  to <span id="toDateDaylbl" style="font-size: 20px;margin-right:5px;font-weight: 500">' + item.TDay + '</span>' +
                                '<span id="toDateMonthlbl" style="font-size: 20px; font-weight: 500;margin-right:5px;font-weight: 500">' + item.TMonth + '</span>' +
                                '<span id="toDateYearlbl" style="font-size: 20px;font-weight: 500">' + item.TYear + '</span><span style="white-space:nowrap">(' + item.toTime + ' )</span>' +

                                '</div>' +

                                '</div>' +
                                '<h5 id="expoNamelbl" style="font-size: 15px;color: #333; font-weight: 700;">' + item.exponame + '</h5>' +
                                '<h5 style="font-size: 18px;color:#808080"><span style="font-weight: 400">VENUE:</span>  <span id=" ">' + item.venue + '</span></h5>' +
                                '<div id="expoDesclbl" style="margin-bottom:10px;font-size: 18px;padding: 0px;color:#808080;font-style: inherit;font-weight: 400;overflow: hidden;text-overflow: ellipsis;display: -webkit-box;-webkit-line-clamp: 3;-webkit-box-orient: vertical;line-height: 1;">' +
                                item.expodesc +
                                '</div>' +
                                freebtn +
                                '</div>' +
                                '</div>' +
                                '</div>';

                            $(expo).appendTo('#expoContainer');
                        });
                        if (viewmore == 'yes') {
                            $('#viewmoreDiv').hide();
                        }
                    }
                }
            });
            setTimeout(function(){
            parent.resizeIframe();
            },500);
            
        }

        function showregisterform() {

            $('#totalregtxt').val('');
            // $('#registercontainer').html('');

            $('#mainbtn').show();
            $('#mainform').show();

            $('#formsbtns').hide();
            $('#registerforms').hide();

            $('#buyTicket').modal('show');
            $('#nextbtn').show();
            setTimeout(function () { $('#totalregtxt').select() }, 500);

        }

        function freeRegister() {
            $('#registerForm').modal('show');
            $('#buyTicket').modal('hide');
        }

        function fillform() {

            $('#registerForm').modal('show');
            $('#buyTicket').modal('hide');


        }

        function viewDetails(url, status) {

            //if (status == 'Event Ended') {
            //    msgbox('This event already ended.', 'err');
            //    return;
            //}

            window.open(url);

        }

        function msgbox(msg, type) {

            switch (type) {
                case 'err':
                    $('#err_msg').text(msg);
                    $('#err_msgbox').modal('show');
                    break;
                case 'reload':
                    $('#success_msg').text(msg);
                    $('#success_msgbox').modal('show');
                    $('#successokbtn').attr('onclick', "$('#NewMerchant_li').click()");
                    break;
                case 'merchantlist':
                    $('#success_msg').text(msg);
                    $('#success_msgbox').modal('show');
                    $('#successokbtn').attr('onclick', "$('#MerchantApplicationList_li').click()");

                    break;
                case 'logout':
                    $('#success_msg').text(msg);
                    $('#success_msgbox').modal('show');
                    $('#successokbtn').attr('onclick', "location.href='login'");

                    break;
                case 'emailsent':
                    $('#success_msg').text(msg);
                    $('#success_msgbox').modal('show');
                    $('#successokbtn').attr('onclick', $('.navpage').attr('src', 'merchant/WelcomePage.aspx').scrollTop());

                    break;
                default:
                    $('#success_msg').text(msg);
                    $('#success_msgbox').modal('show');
                    $('#successokbtn').attr('onclick', "");
                    break;

            }

        }

    </script>


</body>
</html>
