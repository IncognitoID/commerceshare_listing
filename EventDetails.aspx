<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EventDetails.aspx.vb" Inherits="EventDetails" %>

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
    <link href="css/style.css" rel="stylesheet" />
    <style>
        /*CSS*/

        body, html {
            background: #f7f5f5;
            font-family: -apple-system,BlinkMacSystemFont,"Segoe UI",Roboto,"Helvetica Neue",Arial,"Noto Sans",sans-serif,"Apple Color Emoji","Segoe UI Emoji","Segoe UI Symbol","Noto Color Emoji";
            font-size: 1rem;
        }

        .selection-business {
            background: white;
            padding: 10px 15px;
            font-size: 14px;
            cursor: pointer
        }

            .selection-business:hover {
                background: rgba(0, 0, 0, 0.04);
            }

        .selection-business-active {
            background: #ebebeb;
        }

        #dropdown_business {
            display: none;
        }

        .error-msg {
            font-size: 14px;
            color: red;
        }

        .cd-timeline__date {
            text-align: left;
        }

        .cd-timeline__block {
            z-index: 0
        }

        .img-expo {
            width: 175px;
            height: 175px;
            object-fit: contain;
            background: #f3f3f3;
            border: 1px solid #cfcfcf;
            border-radius: 15px;
            overflow: hidden;
        }

        .img-div {
            padding: 9px;
            display: flex;
            justify-content: center;
            position: relative;
        }

        .img-container {
            display: flex;
            flex-wrap: wrap;
            padding: 5px;
        }

        .close-icon {
            position: absolute;
            top: 23px;
            right: 35px;
            font-size: 45px;
            cursor: pointer;
            color: #a39b9b;
            z-index: 1000;
        }

            .close-icon:hover {
                color: white; /* Change the color on hover if desired */
            }

        .enlarged-image-container {
            display: none;
        }

        @media (max-width: 576px) {
            .img-expo {
                height: 175px;
                object-fit: contain;
                background: #f3f3f3;
                border: 1px solid #cfcfcf;
                border-radius: 15px;
                overflow: hidden;
            }

            .img-div {
                width: 50%;
                padding: 9px;
                display: flex;
                justify-content: center;
                position: relative;
            }
        }
    </style>


</head>

<body>
    <form id="form1" runat="server">
    </form>
    <div class="enlarged-image-container">
        <div style="position: fixed; width: 100%; height: 100%; display: flex; align-items: center; justify-content: center; z-index: 1; background: #9598a180;">
            <div class="close-icon">×</div>
            <i onclick="leftImg(this,event)" class="material-icons" style="z-index: 1;cursor: pointer; font-size: 48px; color: white; left: 0px; position: fixed;">chevron_left</i>
            <img onclick=" event.stopPropagation();" style="object-fit: contain; height: 100%; width: 100%;"
                class="enlarged-image" src="" alt="Enlarged Image" />
            <video onclick=" event.stopPropagation();" controls  autoplay class="enlarged-image" src="" alt="Enlarged Image"></video>
            <i onclick="rightImg(this,event)" class="material-icons" style="z-index: 1;cursor: pointer; font-size: 48px; color: white; right: 0px; position: fixed;">chevron_right</i>

        </div>
    </div>
    <div id="buyTicket" class="modal  " role="dialog">
        <div class="modal-dialog modal-md add-item-modal">
            <!-- Modal content-->
            <div class="modal-content">
                <div style="border-bottom: 1px #cfd7df solid;"> 
                    <label style="padding: 10px 12px; font-size: 15px; margin-bottom: 0px;">Premium Ticket</label><i class="material-icons" style="float: right; font-size: 27px; color: #b5a3a3; padding: 10px; cursor: pointer" data-dismiss="modal">clear</i>
                </div>
                <div class="modal-body" style="padding: 0;">
                    <div id="mainform" style="text-align: center; padding: 5px;">
                        <table border="0" style="width: 100%; margin-bottom: 10px; border-color: silver" class="EXPO00000110_adult">
                            <tbody>
                                <tr>
                                    <td>
                                        <label class="package" style="display: none">0</label><label>Outdoor Attraction (Adult)</label>
                                        <br />
                                        <div style="display: inline-flex;">

                                            <label style="color: #f72b2b !important; font-size: 24px;">RM</label><label class="ticketprice" style="color: #f72b2b !important; font-size: 24px;">20.00</label>
                                        </div>
                                    </td>
                                    <td style="width: 123px;">
                                        <input class="qtyinputtxt" type="number" onchange="setmaxnumber(this,4);" onkeyup="setmaxnumber(this,4);" placeholder="0" style="text-align: center; font-size: 20px; width: 100px; padding: 10px;" /></td>
                                </tr>
                            </tbody>
                        </table>
                        <table border="0" style="width: 100%; margin-bottom: 10px; border-color: silver" class="EXPO00000110_child">
                            <tbody>
                                <tr>
                                    <td>
                                        <label class="package" style="display: none">0</label><label>Outdoor Attraction (Child)</label>
                                        <br />
                                        <div style="display: inline-flex;">
                                            <label style="color: #f72b2b !important; font-size: 24px;">RM</label><label class="ticketprice" style="color: #f72b2b !important; font-size: 24px;">8.00</label>
                                        </div>
                                    </td>
                                    <td style="width: 123px;">
                                        <input class="qtyinputtxt" type="number" onchange="setmaxnumber(this,4);" onkeyup="setmaxnumber(this,4);" placeholder="0" style="text-align: center; font-size: 20px; width: 100px; padding: 10px;" /></td>
                                </tr>
                            </tbody>
                        </table>
                        <table border="0" style="width: 100%; margin-bottom: 10px; border-color: silver" class="EXPO00000110_family">
                            <tbody>
                                <tr>
                                    <td>
                                        <label class="package" style="display: none">1</label><label>Outdoor Attraction (Family Package 2 adults + 2 childs) </label>
                                        <br />
                                        <div style="display: inline-flex;">
                                            <label style="color: #f72b2b !important; font-size: 24px;">RM</label><label class="ticketprice" style="color: #f72b2b !important; font-size: 24px;">40.00</label>
                                        </div>
                                    </td>
                                    <td style="width: 123px;">
                                        <input class="qtyinputtxt" type="number" onchange="setmaxnumber(this,2);" onkeyup="setmaxnumber(this,2);" placeholder="0" style="text-align: center; font-size: 20px; width: 100px; padding: 10px;" /></td>
                                </tr>
                            </tbody>
                        </table>
                        <hr />
                        <span style="font-size: 30px; color: #f72b2b !important">Total: RM</span><span style="font-size: 30px; color: #f72b2b !important" id="totalpricetxt">116.00</span>
                    </div>
                    <div>
                        <div class="row  " style="margin-bottom: 11px;">

                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <div style="text-align: center">
                                    <button type="button" id="additembtn" class="btn btn-success  " onclick="fillform()" style="width: 100%; margin-top: 13px; margin-right: 10px; display: inline-block;"><span style="vertical-align: middle;">Next</span></button>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div style="text-align: center; padding: 10px; background: #f8f9fd;">
                        <label style="margin-bottom: 0px; color: #787b86;">
                            Enter total ticket</label>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="registerForm" class="modal  " role="dialog">
        <div class="modal-dialog modal-md add-item-modal">
            <!-- Modal content-->
            <div class="modal-content">
                <div style="border-bottom: 1px #cfd7df solid;">
                    <label style="padding: 10px 12px; font-size: 20px; margin-bottom: 0px; font-weight: bold; color: rgb(66, 66, 66)">Registration</label><i class="material-icons" style="float: right; font-size: 27px; color: #b5a3a3; padding: 10px; cursor: pointer" data-dismiss="modal">clear</i>
                </div>
                <div class="modal-body" style="padding: 0;">
                    <div id="registercontainer">
                        <div class="maindivregister registerdiv1" style="display: block; padding: 20px;">
                            <%-- <table style="width: 100%">
                                <tbody>
                                    <tr>
                                        <td style="text-align: right; font-size: 20px; width: 25px">
                                            <i class="fa fa-user"></i>

                                        </td>
                                        <td style="text-align: left; font-weight: 400; font-size: 20px; line-height: 1.5; color: #666666; border-collapse: collapse; border-spacing: 0; box-sizing: inherit; margin: 0; padding: .5rem .625rem .625rem;">
                                            <input type="text" placeholder="Full name" class="fullnametxt" style="width: 100%;" />
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <table style="width: 100%">
                                <tbody>
                                    <tr>
                                        <td style="text-align: right; font-size: 20px; width: 25px">
                                            <i class="fa fa-envelope"></i>

                                        </td>
                                        <td style="text-align: left; font-weight: 400; font-size: 20px; line-height: 1.5; color: #666666; border-collapse: collapse; border-spacing: 0; box-sizing: inherit; margin: 0; padding: .5rem .625rem .625rem;">
                                            <input type="text" placeholder="Email Address" class="emailtxt" style="width: 100%;" />
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <table style="width: 100%">
                                <tbody>
                                    <tr>
                                        <td style="text-align: right; font-size: 20px; width: 25px">
                                            <i class="fa fa-phone"></i>

                                        </td>
                                        <td style="text-align: left; font-weight: 400; font-size: 20px; line-height: 1.5; color: #666666; border-collapse: collapse; border-spacing: 0; box-sizing: inherit; margin: 0; padding: .5rem .625rem .625rem;">
                                            <input type="number" placeholder="Phone No. (e.g. 0123456789)" class="phonetxt" style="width: 100%;" />
                                        </td>
                                    </tr>
                                </tbody>
                            </table>--%>
                            <div style="position: relative; border: 1px solid #E0E0E0; border-radius: 8px; padding: 5px 10px; margin-bottom: 10px; line-height: 1;">
                                <label style="color: #A3A3A3; font-size: 15px; margin: 0 !important; padding: 0px; font-weight: normal;">Business</label>
                                <br>
                                <div style="display: flex; cursor: pointer" class="business-combo">

                                    <input disabled id="business_type" type="text" style="color: #333; background: white; width: 100%; font-size: 16px; border: none; outline: none" value="Personal" />
                                    <i class="material-icons">arrow_drop_down</i>
                                </div>
                                <div id="dropdown_business" style="left: 0; width: 100%; border: 1px solid #d9d5d5; background: white; position: absolute; top: 60px; padding: 8px 0px; border-radius: 8px;">
                                    <div class="selection-business selection-business-active">Personal</div>
                                    <div class="selection-business">Company</div>
                                    <div class="selection-business">Association</div>
                                    <div class="selection-business">NGO</div>
                                </div>
                            </div>
                            <div class="input-div" style="border: 1px solid #E0E0E0; border-radius: 8px; padding: 5px 10px; margin-bottom: 10px; line-height: 1;">
                                <label style="color: #A3A3A3; font-size: 15px; margin: 0 !important; padding: 0px; font-weight: normal;">Name</label>
                                <br>
                                <input id="nameval" type="text" style="/* padding: 5px; */width: 100%; font-size: 16px; border: none; outline: none" />
                            </div>
                            <div class="input-div" style="border: 1px solid #E0E0E0; border-radius: 8px; padding: 5px 10px; margin-bottom: 10px; line-height: 1;">
                                <label style="color: #A3A3A3; font-size: 15px; margin: 0 !important; padding: 0px; font-weight: normal;">Website</label>
                                <br>
                                <input id="websiteval" type="text" style="/* padding: 5px; */width: 100%; font-size: 16px; border: none; outline: none" />
                            </div>
                            <div class="input-div" style="border: 1px solid #E0E0E0; border-radius: 8px; padding: 5px 10px; margin-bottom: 10px; line-height: 1;">
                                <label style="color: #A3A3A3; font-size: 15px; margin: 0 !important; padding: 0px; font-weight: normal;">Email Address</label>
                                <br>
                                <input id="emailval" type="text" style="/* padding: 5px; */width: 100%; font-size: 16px; border: none; outline: none" />
                            </div>
                            <div class="input-div" style="border: 1px solid #E0E0E0; border-radius: 8px; padding: 5px 10px; margin-bottom: 10px; line-height: 1;">
                                <label style="color: #A3A3A3; font-size: 15px; margin: 0 !important; padding: 0px; font-weight: normal;">Phone No. (e.g. 0123456789)</label>
                                <br>
                                <input id="phoneval" type="text" style="/* padding: 5px; */width: 100%; font-size: 16px; border: none; outline: none" />
                            </div>
                        </div>
                    </div>
                    <div>
                        <div class="row  " style="margin-bottom: 11px;">

                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <div style="text-align: center">
                                    <%-- <button type="button" id=" " class="btn btn-success  " onclick="AddItemDetails()" style="width: 40%; margin-top: 13px; margin-right: 10px; display: inline-block;"><span style="vertical-align: middle; background: gray;">Back</span></button>--%>
                                    <button type="button" id="registerbtn" class="btn btn-success  " onclick="registerUser()" style="width: 100%; margin-top: 13px; margin-right: 10px; display: inline-block;"><span style="vertical-align: middle;">Proceed</span></button>
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--<div style="text-align: center; padding: 10px; background: #f8f9fd;">
                        <label style="margin-bottom: 0px; color: #787b86;">
                            Enter your details</label>
                    </div>--%>
                </div>
            </div>
        </div>
    </div>
    <div class="containbody-form" style="border: 0px solid; max-width: 100%; margin: 0 auto; max-width: 1430px;">
        <%--  <label style="padding: 10px; font-weight: bold;">
                NEW / EDIT EXPO</label>
        --%>
        <div id="eventContainer" style="text-align: center; margin-bottom: 100px;">
            <div style="">
                <img id="expoBanner" style="background-size: contain !important; background-position: center !important; max-width: 100%; height: auto;"
                    src="" />
            </div>
            <div style="width: 100%; position: relative; text-align: center">
                <div class=" " style="color: white">
                    <h1 id="expoNamelbl" style="font-size: 30px; color: #333; font-weight: 700;"></h1>
                    <br />
                    <div><span style="font-weight: bolder; color: #808080;">VENUE:</span>  <span style="color: #808080;" id="venuelbl"></span></div>
                    <div style="display: flex; margin: 0 auto; margin-top: 10px; color: #333">
                        <div style="margin: 0 auto; padding: 10px; display: block;">
                            <span style="font-size: 30px;">Event Date : </span>
                            <span id="fromDateDaylbl" style="font-size: 30px; font-weight: 500"></span>
                            <span id="fromDateMonthlbl" style="font-size: 30px; font-weight: 500;"></span>
                            <span id="fromDateYearlbl" style="font-size: 30px; font-weight: 500"></span>
                            <span id="fromTimelbl" style="white-space: nowrap; font-size: 30px; font-weight: 500"></span>
                            <span id="" style="font-size: 30px;">to</span>
                            <span id="toDateDaylbl" style="font-size: 30px; font-weight: 500"></span>
                            <span id="toDateMonthlbl" style="font-size: 30px; font-weight: 500;"></span>
                            <span id="toDateYearlbl" style="font-size: 30px; font-weight: 500"></span>
                            <span id="toTimelbl" style="white-space: nowrap; font-size: 30px; font-weight: 500"></span>
                        </div>
                    </div>
                </div>
            </div>
            <div style="border-top: 1px solid gray; border-bottom: 1px solid gray; margin-top: 30px; margin-bottom: 30px">
                <span style="font-size: 20px; font-weight: 500; color: #666;">About The Event</span>

            </div>
            <img id="aboutExpoBanner" style="background-size: contain !important; background-position: center !important; max-width: 100%; height: auto;" />

            <div id="expoDesclbl" style="font-size: 25px; padding: 30px;">
            </div>
            <div style="border-top: 1px solid gray; border-bottom: 1px solid gray; margin-top: 30px; margin-bottom: 30px">
                <span style="font-size: 20px; font-weight: 500; color: #666;">Event Details</span>


            </div>
            <section class="cd-timeline js-cd-timeline">
                <div class="container max-width-lg cd-timeline__container">
                </div>
            </section>
            <div id="postEventDiv" style="display: none; border-top: 1px solid gray; border-bottom: 1px solid gray; margin-top: 30px; margin-bottom: 30px">
                <span style="font-size: 20px; font-weight: 500; color: #666;">Post Event Update</span>
            </div>
            <div class="col-md-12 col-sm-12 col-xs-12">

                <div class="post-desc">
                </div>
                <div class="img-container">
                </div>
            </div>
            <!-- cd-timeline -->
            <script src="js/main.js"></script>
        </div>
        <div style="position: fixed; bottom: 0; left: 0; right: 0; padding: 10px; text-align: center;">
            <a id="EventEndedBtn" style="display: none; background: #cb2860; color: white; font-weight: bold;" class="btn btn-success btnEventEnded">EVENT ENDED</a>
            <a id="FreeRegisterBtn" style="display: none; background: green; color: white; font-weight: bold;" class="btn btn-success btnRegister" onclick="freeRegister()">FREE REGISTRATION</a>
            <a id="BuyTicketBtn" style="display: none; background: green; color: white; font-weight: bold;" class="btn btn-success btnRegister" onclick="showregisterform()">BUY TICKET</a>
        </div>
    </div>

    <script>
        //Javascript

        $(document).ready(function () {

            loadImg();
            var $timeline_block = $(".cd-timeline-block");

            //hide timeline blocks which are outside the viewport
            $timeline_block.each(function () {
                if (
                    $(this).offset().top >
                    $(window).scrollTop() + $(window).height() * 0.75
                ) {
                    $(this)
                        .find(".cd-timeline-img, .cd-timeline-content")
                        .addClass("is-hidden");
                }
            });

            //on scolling, show/animate timeline blocks when enter the viewport
            $(window).on("scroll", function () {
                $timeline_block.each(function () {
                    if (
                        $(this).offset().top <=
                        $(window).scrollTop() + $(window).height() * 0.75 &&
                        $(this)
                            .find(".cd-timeline-img")
                            .hasClass("is-hidden")
                    ) {
                        $(this)
                            .find(".cd-timeline-img, .cd-timeline-content")
                            .removeClass("is-hidden")
                            .addClass("bounce-in");
                    }
                });
            });

            if (getParameterByName('id')) {
                edit();

            } else {

            }

            $('.qtyinputtxt').change(function () {
                var subtotal = 0;
                $('.qtyinputtxt').each(function () {
                    var p = $(this).closest('table').find('.ticketprice').text();
                    var q = $(this).val();
                    //alert(q);
                    if (q == '') {
                        q = 0;

                    }
                    var total = parseFloat(p) * q;

                    subtotal += total;

                });

                //  total = parseFloat(total) + parseFloat(curtotal);
                $('#totalpricetxt').text(subtotal.toFixed(2));

            });

            $(".business-combo").click(function () {
                $("#dropdown_business").toggle();
            });

            $(".selection-business").click(function () {
                $('#business_type').val($(this).text());
                $("#dropdown_business").hide();
            });
            loadEvent();

            const $closeIcon = $('.close-icon');
            const $enlargedImageContainer = $('.enlarged-image-container');

            $closeIcon.click(function () {
                $enlargedImageContainer.hide();
                $('.enlarged-image').attr('src','');
            });
            $enlargedImageContainer.click(function () {
                $enlargedImageContainer.hide();
                $('.enlarged-image').attr('src','');
            });

            //parent.hideloading();

        });

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

        function edit() {

            var code = getParameterByName('id');
            $.ajax({
                type: "POST",
                url: "EventDetails.aspx/loadExpo",
                data: JSON.stringify({
                    code: code
                }),
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (s) {
                    var data = JSON.parse(s.d);
                    if (data.length > 0) {
                        $.each(data, function (i, item) {

                            $('#fromDateDaylbl').text(item.FDay);
                            $('#fromDateMonthlbl').text(item.FMonth);
                            $('#fromDateYearlbl').text(item.FYear);
                            $('#fromTimelbl').text('(' + item.fromTime + ' )');


                            $('#toDateDaylbl').text(item.TDay);
                            $('#toDateMonthlbl').text(item.TMonth);
                            $('#toDateYearlbl').text(item.TYear);
                            $('#toTimelbl').text('(' + item.toTime + ' )');

                            $('#expoNamelbl').text(item.exponame);
                            $('#venuelbl').text(item.venue);

                            $('#expoDesclbl').text(item.expodesc);
                            $('#aboutExpoBanner').attr('src', item.AboutExpoBannerPath);

                            $('#expoBanner').attr('src', item.ExpoBannerPath);



                            if (item.chargeable == '0') {
                                $('#FreeRegisterBtn').show();
                                $('#BuyTicketBtn').hide();
                            } else {
                                $('#FreeRegisterBtn').hide();
                                $('#BuyTicketBtn').show();
                            }

                            if (item.Validity == 'Expired') {
                                $('#FreeRegisterBtn').hide();
                                $('#BuyTicketBtn').hide();
                                $('#EventEndedBtn').show();
                            }
                        });
                    }
                }
            });
        }

        $(document).ready(function () {
        });
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

            //if (parseFloat($('#totalpricetxt').text()) <= 0) {

            //    var totaltable = $('#mainform table').length;

            //    if (totaltable == 1) {
            //        var ticketprice = $('.ticketprice').text();
            //        if (ticketprice == '0.00' && ($('.qtyinputtxt').val() > 0)) {
            //            console.log('Free ticket');
            //            loadregister(1);
            //            $('#mainbtn').hide();
            //            $('#mainform').hide();

            //            $('#formsbtns').show();
            //            $('#registerforms').show();
            //        } else {
            //            alert('Please enter ticket quantity');
            //        }

            //    } else {
            //        alert('Please enter ticket quantity');
            //    }

            //} else {
            //    console.log('Pay ticket');
            //    loadregister(1);
            //    $('#mainbtn').hide();
            //    $('#mainform').hide();

            //    $('#formsbtns').show();
            //    $('#registerforms').show();
            //}

            $('#registerForm').modal('show');
            $('#buyTicket').modal('hide');


        }

        function registerUser() {

            $('.error-msg').remove();

            if ($('#nameval').val().length <= 0) {
                $('#nameval').closest('.input-div').append('<div class="error-msg">Please fill name</div>');
                $('#nameval').focus();
                return
            }

            if ($('#emailval').val().length <= 0) {
                $('#emailval').closest('.input-div').append('<div class="error-msg">Please fill email</div>');
                $('#emailval').focus();
                return
            }

            if ($('#phoneval').val().length <= 0) {
                $('#phoneval').closest('.input-div').append('<div class="error-msg">Please fill Phone no</div>');
                $('#phoneval').focus();
                return
            }

            if ($('.error-msg').length == 0) {
                var eventcode = getParameterByName('id');
                $('#registerbtn').prop('disabled', true).css('cursor', 'wait');
                setTimeout(function () {
                    $.ajax({
                        type: "POST",
                        url: "eventdetails.aspx/register",
                        data: JSON.stringify({
                            business_type: $('#business_type').val(),
                            eventcode: eventcode,
                            nameval: $('#nameval').val(),
                            websiteval: $('#websiteval').val(),
                            emailval: $('#emailval').val(),
                            phoneval: $('#phoneval').val()
                        }),
                        async: false,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (s) {
                            if (s.d == 'success') {
                                $('#registerForm').modal('hide');
                                alert('Successully Registered');
                                $('#business_type').val('Personal');
                                $('#nameval').val('');
                                $('#websiteval').val('');
                                $('#emailval').val('');
                                $('#phoneval').val('');
                            }
                            else {
                                alert(s.d);
                            }
                            $('#registerbtn').prop('disabled', false).css('cursor', 'pointer');
                        }
                    });
                }, 1000);


            }
        }

        function loadEvent() {
            $.ajax({
                type: "POST",
                url: "EventDetails.aspx/loadEvent",
                data: JSON.stringify({
                    code: getParameterByName('id')
                }),
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (s) {
                    var data = JSON.parse(s.d);
                    if (data.length > 0) {
                        $.each(data, function (i, item) {
                            var event = `<div class="cd-timeline__block">
                        <div class="cd-timeline__img cd-timeline__img--picture">
                            <img src="img/cd-icon-picture.svg" alt="Picture">
                        </div>
                        <!-- cd-timeline__img -->

                        <div class="cd-timeline__content text-component">
                            <h2 style="font-size: 20px;">` + item.eventname + `</h2>
                            <img src="` + item.EventBannerPath + `" style="height: 250px; object-fit: contain; margin-bottom: 10px;" />
                            <p class="color-contrast-medium">` + item.eventdesc + `</p>

                            <div class="flex justify-between items-center">
                                <span class="cd-timeline__date" >` + item.fromdate_ + ` to ` + item.todate_ + `</span>
                                
                            </div>
                        </div>
                        <!-- cd-timeline__content -->
                    </div>`;

                            $(event).appendTo('.cd-timeline__container');
                        });
                    } else {
                        $('.cd-timeline').replaceWith('<div style="text-align:center">No event details</div>');

                    }
                }
            });
        }

        function viewImage(e) {

            $('.enlarged-image').attr('src', $(e).attr('src')).attr('data-id', $(e).attr('data-id'));

            if (getFileType($(e).attr('src')) == 'video') {
                $('img.enlarged-image').hide();
                $('video.enlarged-image').show();
            } else {
                $('video.enlarged-image').hide();
                $('img.enlarged-image').show();
            }

            $('.enlarged-image-container').show();
        }

        function rightImg(e, event) {
            var id = parseInt($(e).parent().find('.enlarged-image').attr('data-id'));

            if ((id + 1) < parseInt($('.img-expo').length)) {
                $('.enlarged-image').attr('src', $('.img-container [data-id=' + (id + 1) + ']').attr('src')).attr('data-id', (id + 1));

                if (getFileType($('.img-container [data-id=' + (id + 1) + ']').attr('src')) == 'video') {
                    $('img.enlarged-image').hide();
                    $('video.enlarged-image').show();
                } else {
                    $('video.enlarged-image').hide();
                    $('img.enlarged-image').show();
                }
            }

            event.stopPropagation();
        }

        function leftImg(e, event) {
            var id = parseInt($(e).parent().find('.enlarged-image').attr('data-id'));



            if ((id - 1) >= 0) {
                $('.enlarged-image').attr('src', $('.img-container [data-id=' + (id - 1) + ']').attr('src')).attr('data-id', (id - 1));

                if (getFileType($('.img-container [data-id=' + (id - 1) + ']').attr('src')) == 'video') {
                    $('img.enlarged-image').hide();
                    $('video.enlarged-image').show();

                } else {
                    $('video.enlarged-image').hide();
                    $('img.enlarged-image').show();
                }
            }

            event.stopPropagation();
        }

        function loadImg() {

            $.ajax({
                type: "POST",
                url: "EventDetails.aspx/getPostExpoImg",
                async: false,
                data: JSON.stringify({
                    expocode: getParameterByName('id')
                }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (s) {
                    var data = JSON.parse(s.d);

                    $('.img-container').html('');
                    console.log(data.length);
                    if (data.length > 0) {
                        $('#postEventDiv').show();

                        $.each(data, function (i, item) {
                            $('.post-desc').text(item.PostExpoDesc);

                            if (getFileType(item.PostExpoPath) == 'video') {
                                var e = ` <div class="img-div">
                                    <video controls onclick="viewImage(this)" data-id="` + i + `" class="img-expo" src="` + item.PostExpoPath + `" ></video>
                                 </div>`;

                                $(e).appendTo('.img-container');
                            } else {
                                var e = ` <div class="img-div">
                                    <img onclick="viewImage(this)" data-id="` + i + `" class="img-expo" src="` + item.PostExpoPath + `" />
                                 </div>`;

                                $(e).appendTo('.img-container');
                            }

                        });
                    }
                }
            });
        }

        function getFileType(filePath) {
            var videoExtensions = [".mp4", ".avi", ".mov", ".mkv", ".webm"];
            var imageExtensions = [".jpg", ".jpeg", ".png", ".gif", ".bmp"];

            var fileExtension = filePath.toLowerCase().slice((filePath.lastIndexOf(".") - 1 >>> 0) + 2);

            if (videoExtensions.includes("." + fileExtension)) {
                return "video";
            } else if (imageExtensions.includes("." + fileExtension)) {
                return "image";
            } else {
                return "unknown or not supported";
            }
        }
    </script>


</body>
</html>
