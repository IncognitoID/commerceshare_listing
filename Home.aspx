<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="css/swiper.min.css" rel="stylesheet" />
    <script type="text/javascript">
        $(document).ready(function () {
            $('.u-slick').slick({
                draggable: true,
                autoplay: true, /* this is the new line */
                autoplaySpeed: 3000,
                infinite: true,
                slidesToShow: 1,
                slidesToScroll: 1,
            });

            $('#newsinfoframe').on('load', function () {
                setTimeout(function () {
                    var iframe = $('#newsinfoframe');
                    var iframeWindow = iframe.get(0).contentWindow;
                    var iframeBody = $(iframeWindow.document).find('body').height();

                    iframe.height(iframeBody);
                }, 2000);
            });

        });
        function adjustIfameheight_news() {

            var iframe = $('#newsinfoframe');
            var iframeWindow = iframe.get(0).contentWindow;
            var iframeBody = $(iframeWindow.document).find('body').height();

            iframe.height(iframeBody);
        }
        function adjustIfameheight() {

            var iframe = $('#eventframe');
            var iframeWindow = iframe.get(0).contentWindow;
            var iframeBody = $(iframeWindow.document).find('body').height();

            iframe.height(iframeBody);
        }
    </script>

    <style>
        .u-slick .slick-arrow {
            display: none !important;
        }

        /*@media (max-width: 992px) {
            .row {
                margin-right: 0 !important;
                margin-left: 0 !important;
            }
        }*/

        .sweet-alert h2 {
            text-align: left;
            font-size: 14px;
        }

        .u-slick.slick-initialized .js-slide, .u-slick.slick-initialized .js-thumb {
            visibility: visible;
            height: auto;
            overflow: hidden;
            border-radius: 10px;
        }

        @media only screen and (max-width: 767px) {
            /* Smaller font size for h1 tag on mobile devices */
            .brand-title-padding > h1 {
                font-size: 1.5rem;
            }

            .pad-top-0 {
                padding-top: 0px !important;
            }
        }

        .border-radius-10 {
            border-radius: 10px;
        }
    </style>
    <!-- ========== MAIN CONTENT ==========->
    <main id="content" role="main">

            <!-- Slider Section -->

    <%-- <div class="bg-img-hero bg-gray-1 ">
        <div class="container overflow-hidden">
            <a href="smesignup.aspx">
                <img class="img-fluid" data-scs-animation-in="fadeInUp"
                    data-scs-animation-delay="500" src="sme-assets/banner/banner2.jpg" />

            </a>
        </div>
    </div>--%>
    <div class="mb-4" style="background: #f3f3f3;">
        <div class="container">
            <div class="row">
                <div class="col-md-6 col-12 p-3 ">
                    <%--<a href="smesignup.aspx">--%>
                    <a href="smesignup3.aspx">
                        <img class="img-fluid border-radius-10" src="sme-assets/banner/hometop1v3.png" />
                    </a>
                </div>
                <div class="col-md-6 col-12 p-3 pad-top-0">
                    <a href="marketplace.aspx">
                        <img class="img-fluid border-radius-10" src="sme-assets/banner/hometop2v3.png" />
                    </a>
                </div>
            </div>
        </div>
    </div>
    <div class="mb-4">
        <%--<div class="bg-img-hero" style="background-image: url(https://qcplusmart.com/qcplus/new-assets/img/qcplus/img2.jpg);">--%>
        <div class="bg-img-hero">
            <div class="container overflow-hidden">
                <div class="js-slick-carousel u-slick"
                    data-pagi-classes="text-center position-absolute right-0 bottom-0 left-0 u-slick__pagination u-slick__pagination--long justify-content-center mb-3 mb-md-4">
                    <div class="js-slide">
                        <div class="row py-md-0" data-scs-animation-in="fadeInRight" data-scs-animation-delay="500">
                            <a href="smesignup3.aspx">
                                <img class="img-fluid" data-scs-animation-in="fadeInUp"
                                    data-scs-animation-delay="500" src="sme-assets/banner/3B1.png" />

                            </a>
                        </div>
                    </div>
                    <div class="js-slide">
                        <div class="row py-md-0" data-scs-animation-in="fadeInRight" data-scs-animation-delay="500">
                            <%--<a href="smesignup.aspx">--%>
                            <img class="img-fluid" data-scs-animation-in="fadeInUp"
                                data-scs-animation-delay="500" src="sme-assets/banner/brief.png" />

                            <%--</a>--%>
                        </div>
                    </div>
                    <%--<div class="js-slide">
                        <div class="row py-md-0" data-scs-animation-in="fadeInRight" data-scs-animation-delay="500">
                            <a href="smesignup.aspx">
                            <img class="img-fluid" data-scs-animation-in="fadeInUp"
                                data-scs-animation-delay="500" src="sme-assets/banner/banner3.jpg" />

                            </a>
                        </div>
                    </div>--%>
                    <%-- <div class="js-slide ">
                        <div class="row pt-7 py-md-0">
                            <div class="d-none d-wd-block offset-1"></div>
                            <div class="col-xl col col-md-6 mt-md-8 mt-lg-10">
                                <div class="ml-xl-4">
                                    <h6 class="font-size-15 font-weight-bold mb-2 text-gray-100"
                                        data-scs-animation-in="fadeInUp">Join the Digital Revolution
                                            </h6>
                                    <h1 class="font-size-46 text-lh-50 font-weight-light mb-8"
                                        data-scs-animation-in="fadeInUp"
                                        data-scs-animation-delay="500">Empower Your Business Today!
                                            </h1>
                                    <a href="smesignup.aspx" class="btn btn-primary transition-3d-hover rounded-lg font-weight-normal py-2 px-md-7 px-3 font-size-16 text-black font-weight-bold "
                                        data-scs-animation-in="fadeInUp"
                                        data-scs-animation-delay="600">Join Now
                                            </a>
                                </div>
                            </div>
                            <!-- <div class="col-xl-7 col-6 d-flex align-items-end ml-auto ml-md-0" -->
                            <div class="col-xl-7 ml-auto ml-md-0"
                                data-scs-animation-in="fadeInRight"
                                data-scs-animation-delay="500">
                                <img class="img-fluid ml-auto mr-10 mr-wd-auto" src="sme-assets/banner/2.png" alt="Image Description">
                            </div>
                        </div>
                    </div>--%>
                </div>
            </div>
        </div>
    </div>
    <div class="container brand-section-padding">
        <!-- Banners -->
        <div class="row">
            <div class="col-md-12  mb-md-0" id="news">
                <div class="flex-content-center  flex-column mx-auto text-center brand-title-padding">
                    <h1 class="font-size-26 font-weight-bold ourbrand-title">News & Info</h1>
                    <span class="ourbrand-title-bottom"></span>
                    <iframe id="newsinfoframe" src="https://ezyshare.online/ezysharefrontend/newsneventlist.aspx" style="border: none; overflow: hidden; width: 100%; height: 76vh;" scrolling="yes" frameborder="0" allowfullscreen="true" allow="autoplay; clipboard-write; encrypted-media; picture-in-picture; web-share"></iframe>
                </div>
            </div>
            <div class="col-md-12 mb-md-0" id="event">
                <div class="flex-content-center flex-column mx-auto text-center brand-title-padding">
                    <h1 class="font-size-26 font-weight-bold ourbrand-title">Latest Events</h1>
                    <span class="ourbrand-title-bottom"></span>
                    <iframe id="eventframe" src=" https://ezyshare.online/ezyshare-expo/eventlist" style="border: none; overflow: hidden; width: 100%; height: 600px" scrolling="no" frameborder="0" allowfullscreen="true" allow="autoplay; clipboard-write; encrypted-media; picture-in-picture; web-share"></iframe>
                </div>
            </div>
        </div>
    </div>
    <!-- End Banners -->

    <div class="container brand-section-padding">
        <!-- Banners -->
        <div class="row">
            <div class="col-md-12 mb-md-0">
                <div class="flex-content-center flex-column mx-auto text-center brand-title-padding">
                    <h1 class=" font-size-26 font-weight-bold ourbrand-title">Introduction Video</h1>
                    <span class="ourbrand-title-bottom"></span>
                    <!-- <p class="text-gray-39 font-size-18 text-lh-default">We create 3 brands to fulfill business and home needs</p> -->
                </div>
            </div>
            <div class="col-md-6 mb-md-0 ">
                <div class="flex-content-center  flex-column mx-auto text-center  video-container">
                    <iframe width="100%" height="400" src="https://www.youtube.com/embed/z-b3CgiRebA?rel=0" title="YouTube video player" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share" allowfullscreen></iframe>
                </div>
            </div>
            <div class="col-md-6 mb-md-0 mt-5-mobile">
                <div class="flex-content-center  flex-column mx-auto text-center video-container">
                    <iframe width="100%" height="400" src="https://www.youtube.com/embed/z1ArOW2kpcc?rel=0" title="YouTube video player" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share" allowfullscreen></iframe>
                </div>
            </div>
        </div>
    </div>
    <%--<div class="content col-md-10 col-lg-11 container topmargin" style="float: none !important; padding-left: 2px !important; padding-right: 2px !important;">
        <div class="swiper-container swiper_container" style="z-index: 0;">
            <div class="swiper-wrapper">
                <div class="swiper-slide">
                    <img id="img11" runat="server" class="swiperimg" src="qcplus/banner/newbanner1.jpg" style="width: 100%;" />
                </div>
            </div>
            <!-- Add Pagination -->
            <div class="swiper-pagination"></div>
            <!-- Add Arrows -->
            <div class="swiper-button-next swiper-button-white"></div>
            <div class="swiper-button-prev swiper-button-white"></div>
        </div>
    </div>--%>
    <!-- End Slider Section -->
    <%--<div class="container brand-section-padding">
        <div class="row">
            <div class="col-md-12  mb-md-0">
                <div class="flex-content-center  flex-column mx-auto text-center brand-title-padding">
                    <h1 class="h1 font-weight-bold ourbrand-title">Our Brand</h1>
                    <span class="ourbrand-title-bottom"></span>
                    <!-- <p class="text-gray-39 font-size-18 text-lh-default">We create 3 brands to fulfill business and home needs</p> -->
                </div>
            </div>
            <div class="col-md-4 mb-4 mb-md-0">
                <div class="card mb-3 border-0 text-center rounded-0">
                    <img class="img-fluid mb-3 border-brand" src="https://qcplusmart.com/qcplus/new-assets/img/qcplus/choose1.jpg" alt="Card image cap">
                    <div class="card-body">
                        <img class="img-fluid mb-3 brand-img" src="https://qcplusmart.com/qcplus/new-assets/img/qcplus/logo1.png" alt="Card image cap">
                        <!-- <h5 class="font-size-18 font-weight-semi-bold mb-3 brand-title">QC Plus</h5> -->
                        <p class="text-gray-90 max-width-334 mx-auto">QC Plus focus supplying variety of eggs. We sell high-quality and varieties of eggs size available and offer a lower price to our customers.</p>
                    </div>
                </div>
            </div>
            <div class="col-md-4 mb-4 mb-md-0">
                <div class="card mb-3 border-0 text-center rounded-0">
                    <img class="img-fluid mb-3 border-brand" src="https://qcplusmart.com/qcplus/new-assets/img/qcplus/choose4.jpg" alt="Card image cap">
                    <div class="card-body">
                        <img class="img-fluid mb-3 brand-img " src="https://qcplusmart.com/qcplus/new-assets/img/qcplus/logo3.png" alt="Card image cap">
                        <!-- <h5 class="font-size-18 font-weight-semi-bold mb-3 brand-title">QC's Mart</h5> -->
                        <p class="text-gray-90 max-width-334 mx-auto">QC mart focus supplying grocery. We sell a variety of business & home grocery needs in our store including various types of retail items, plastic bags, plastic food wraps, and more.</p>
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="card mb-3 border-0 text-center rounded-0">
                    <img class="img-fluid mb-3 border-brand" src="https://qcplusmart.com/qcplus/new-assets/img/qcplus/choose2.jpg" alt="Card image cap">
                    <div class="card-body">
                        <img class="img-fluid mb-3 brand-img " src="https://qcplusmart.com/qcplus/new-assets/img/qcplus/logo4.png" alt="Card image cap">
                        <!-- <h5 class="font-size-18 font-weight-semi-bold mb-3 brand-title">QC Fresco</h5> -->
                        <p class="text-gray-90 max-width-334 mx-auto">
                            QC Fresco focus supplying fresh fruits and vegetables. We sell a variety of the freshest fruits and vegetables directly from the farm.
                                    and healthiest vegetables you deserve.
                        </p>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="container brand-section-padding2">
        <div class="row">
            <div class="flex-content-center  flex-column mx-auto text-center brand-title-padding">
                <h1 class="h1 font-weight-bold ourbrand-title">Why Choose Us?</h1>
                <span class="ourbrand-title-bottom"></span>
                <p class="text-gray-39 font-size-18 text-lh-default">We are your neighborhood source for the best quality goods, healthy & freshest food and always cheaper price. We will do the best to derive satisfaction from meeting the multiple needs of today's grocery shopper.</p>
            </div>

            <div class="col-md-4 mb-4 mb-md-0">
                <div class="card mb-3 border-0 text-center rounded-0">
                    <img class="img-fluid mb-3 chooseus-img" src="https://qcplusmart.com/qcplus/new-assets/img/qcplus/why1.jpg" alt="Card image cap">
                    <div class="card-body">
                        <h5 class="font-size-18 font-weight-semi-bold mb-3 brand-title">High Quality</h5>
                        <p class="text-gray-90 max-width-334 mx-auto">We make sure our product is high-quality standard to make sure your family and business enjoy quality food.</p>
                    </div>
                </div>
            </div>
            <div class="col-md-4 mb-4 mb-md-0">
                <div class="card mb-3 border-0 text-center rounded-0">
                    <img class="img-fluid mb-3 chooseus-img" src="https://qcplusmart.com/qcplus/new-assets/img/qcplus/why2.jpg" alt="Card image cap">
                    <div class="card-body">
                        <h5 class="font-size-18 font-weight-semi-bold mb-3 brand-title">Fresh & Healthy</h5>
                        <p class="text-gray-90 max-width-334 mx-auto">
                            Our product mostly is directly from the farm in less than 24 hours to make sure you get the best freshness and healthy food.
                        </p>
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="card mb-3 border-0 text-center rounded-0">
                    <img class="img-fluid mb-3 chooseus-img" src="https://qcplusmart.com/qcplus/new-assets/img/qcplus/why3.jpg" alt="Card image cap">
                    <div class="card-body">
                        <h5 class="font-size-18 font-weight-semi-bold mb-3 brand-title">Lower Price</h5>
                        <p class="text-gray-90 max-width-334 mx-auto">We provide competitive pricing compared to our competitors and allow you to save more, the more you buy.</p>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="container">
        <!-- <div class="row"> -->
        <div class="flex-content-center  flex-column mx-auto text-center brand-title-padding">
            <h1 class="h1 font-weight-bold ourbrand-title" id="ouroutlet">Our Outlet</h1>
            <span class="ourbrand-title-bottom"></span>
            <!-- <p class="text-gray-39 font-size-18 text-lh-default">Passion may be a friendly or eager interest in or admiration for a proposal, cause, discovery, or activity or love to a feeling of unusual excitement.</p> -->
        </div>
        <!-- Banner -->
        <!-- <div class="row mb-6">
                            <div class="col-md-6 mb-3 mb-xl-0 col-wd-3">
                                <a href="../shop/shophtml" class="d-black text-gray-90">
                                    <div class="min-height-166 py-1 py-xl-2 py-wd-4 d-flex bg-gray-1 align-items-center">
                                        <div class="col-6 col-xl-7 col-wd-6 pr-0">
                                            <img class="img-fluid outlet-img" src="https://qcplusmart.com/qcplus/new-assets/img/246X176/img1.jpg" alt="Image Description">
                                        </div>
                                        <div class="col-6 col-xl-5 col-wd-6 pr-xl-4 pr-wd-3">
                                            <div class="mb-2 pb-1 font-size-18 font-weight-light text-ls-n1 text-lh-23">
                                                <strong>PUCHONG(HQ)</strong> 
                                            </div>
                                            <div class="link text-gray-90 font-weight-bold font-size-15" href="#">
                                               View More
                                                <span class="link__icon ml-1">
                                                    <span class="link__icon-inner"><i class="ec ec-arrow-right-categproes"></i></span>
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                </a>
                            </div>
                            <div class="col-md-6 mb-3 mb-xl-0 col-wd-3">
                                <a href="../shop/shophtml" class="d-black text-gray-90">
                                    <div class="min-height-166 py-1 py-xl-2 py-wd-4 d-flex bg-gray-1 align-items-center">
                                        <div class="col-6 col-xl-7 col-wd-6 pr-0">
                                            <img class="img-fluid outlet-img" src="https://qcplusmart.com/qcplus/new-assets/img/246X176/img2.jpg" alt="Image Description">
                                        </div>
                                        <div class="col-6 col-xl-5 col-wd-6 pr-xl-4 pr-wd-3">
                                            <div class="mb-2 pb-1 font-size-18 font-weight-light text-ls-n1 text-lh-23">
                                                <strong>QC Mart</strong> ON THE CAMERAS
                                            </div>
                                            <div class="link text-gray-90 font-weight-bold font-size-15" href="#">
                                                Shop now
                                                <span class="link__icon ml-1">
                                                    <span class="link__icon-inner"><i class="ec ec-arrow-right-categproes"></i></span>
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                </a>
                            </div>
                            <div class="col-md-6 mb-3 mb-xl-0 col-wd-3 d-md-none d-wd-block">
                                <a href="../shop/shophtml" class="d-black text-gray-90">
                                    <div class="min-height-166 py-1 py-xl-2 py-wd-4 d-flex bg-gray-1 align-items-center">
                                        <div class="col-6 col-xl-7 col-wd-6 pr-0">
                                            <img class="img-fluid outlet-img" src="https://qcplusmart.com/qcplus/new-assets/img/246X176/img3.jpg" alt="Image Description">
                                        </div>
                                        <div class="col-6 col-xl-5 col-wd-6 pr-xl-4 pr-wd-3">
                                            <div class="mb-2 pb-1 font-size-18 font-weight-light text-ls-n1 text-lh-23">
                                                <strong>QC Fresco</strong> ON THE CAMERAS
                                            </div>
                                            <div class="link text-gray-90 font-weight-bold font-size-15" href="#">
                                                Shop now
                                                <span class="link__icon ml-1">
                                                    <span class="link__icon-inner"><i class="ec ec-arrow-right-categproes"></i></span>
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                </a>
                            </div>
                        </div> -->
        <!-- End Banner -->
        <!-- Tab Prodcut Section -->
        <div class="mb-6">
            <!-- Nav Classic -->
            <!-- <div class="position-relative bg-white text-center z-index-2">
                                <ul class="nav nav-classic nav-tab justify-content-center" id="pills-tab" role="tablist">
                                    <li class="nav-item">
                                        <a class="nav-link active js-animation-link" id="pills-one-example1-tab" data-toggle="pill" href="#pills-one-example1" role="tab" aria-controls="pills-one-example1" aria-selected="true"
                                            data-target="#pills-one-example1"
                                            data-link-group="groups"
                                            data-animation-in="slideInUp">
                                            <div class="d-md-flex justify-content-md-center align-items-md-center">
                                                Featured
                                            </div>
                                        </a>
                                    </li>
                                    <li class="nav-item">
                                        <a class="nav-link js-animation-link" id="pills-two-example1-tab" data-toggle="pill" href="#pills-two-example1" role="tab" aria-controls="pills-two-example1" aria-selected="false"
                                            data-target="#pills-two-example1"
                                            data-link-group="groups"
                                            data-animation-in="slideInUp">
                                            <div class="d-md-flex justify-content-md-center align-items-md-center">
                                                On Sale
                                            </div>
                                        </a>
                                    </li>
                                    <li class="nav-item">
                                        <a class="nav-link js-animation-link" id="pills-three-example1-tab" data-toggle="pill" href="#pills-three-example1" role="tab" aria-controls="pills-three-example1" aria-selected="false"
                                            data-target="#pills-three-example1"
                                            data-link-group="groups"
                                            data-animation-in="slideInUp">
                                            <div class="d-md-flex justify-content-md-center align-items-md-center">
                                                Top Rated
                                            </div>
                                        </a>
                                    </li>
                                </ul>
                            </div> -->
            <!-- End Nav Classic -->
            <!-- Tab Content -->
            <div class="tab-content" id="pills-tabContent">
                <div class="tab-pane fade pt-2 show active" id="pills-one-example1" role="tabpanel" aria-labelledby="pills-one-example1-tab" data-target-group="groups">
                    <ul class="row list-unstyled products-group no-gutters">
                        <asp:Repeater runat="server" ID="outlet_preview" OnItemCommand="outlet_preview_ItemCommand" OnItemDataBound="outlet_preview_ItemDataBound">
                            <ItemTemplate>
                                <li class="col-6 col-md-3  product-item">
                                    <div class="product-item__outer ">
                                        <div class="product-item__inner px-xl-4 p-3">
                                            <div class="product-item__body pb-xl-2">
                                                <!-- <div class="mb-2"><a href="../shop/product-categories-7-column-full-widthhtml" class="font-size-12 text-gray-5">018-944 8387 / 018-963 8387 / 03-8051 3618<br>8:30AM - 6:30PM
                                                        </a></div>
                                                        <div class="mb-2"><a href="../shop/product-categories-7-column-full-widthhtml" class="font-size-12 text-gray-5">8:30AM - 6:30PM
                                                        </a></div> -->
                                                <div class="mb-2">
                                                    <img runat="server" class="img-fluid outlet-img" id="outlet_img" src="" alt="Image Description">
                                                </div>
                                                <h5 class="mb-1 product-item__title font-size-17 font-weight-bold">
                                                    <asp:Label runat="server" ID="lblOutletBranch"></asp:Label>,
                                                        <asp:Label runat="server" ID="lblOutletState"></asp:Label></h5>
                                                <div class="flex-center-between mb-1">
                                                    <asp:LinkButton runat="server" CommandArgument='<%# Eval("outletCode") %>'>
                                                        <div class="prodcut-price">
                                                            <div class="text-gray-2 font-size-18">View More</div>
                                                        </div></asp:LinkButton>
                                                    <div class="d-none d-xl-block prodcut-add-cart">
                                                        <asp:LinkButton runat="server" CommandArgument='<%# Eval("outletCode") %>' class="btn-add-cart btn-primary transition-3d-hover"><i class="ec ec-arrow-right-categproes"></i></asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>
                                            <!-- <div class="product-item__footer">
                                                        <div class="border-top pt-2 flex-center-between flex-wrap">
                                                            <a href="../shop/comparehtml" class="text-gray-6 font-size-13"><i class="ec ec-compare mr-1 font-size-15"></i> Compare</a>
                                                            <a href="../shop/wishlisthtml" class="text-gray-6 font-size-13"><i class="ec ec-favorites mr-1 font-size-15"></i> Wishlist</a>
                                                        </div>
                                                    </div> -->
                                        </div>
                                    </div>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                </div>
            </div>
            <!-- End Tab Content -->
        </div>
        <!-- End Tab Prodcut Section -->
        <!-- </div> -->
    </div>--%>
    </main>
       
    <!-- ========== END MAIN CONTENT ========== -->
    <%--<script src="scripts/swiper.min.js" type="text/javascript"></script>--%>

    <%--    <script type="text/javascript">
        var swiper = new Swiper('.swiper_container', {
            //spaceBetween: 30,
            centeredSlides: true,
            loop: true,
            autoplay: {
                delay: 3000,
                disableOnInteraction: false,
            },
            navigation: {
                nextEl: '.swiper-button-next',
                prevEl: '.swiper-button-prev',
            },
        });
        var swiper = new Swiper('.swiper_container1', {
            //spaceBetween: 30,
            centeredSlides: true,
            loop: true,
            autoplay: {
                delay: 4000,
                disableOnInteraction: false,
            },
            navigation: {
                nextEl: '.swiper-button-next',
                prevEl: '.swiper-button-prev',
            },
        });
        var swiper = new Swiper('.swiper_container2', {
            //spaceBetween: 30,
            centeredSlides: true,
            loop: true,
            autoplay: {
                delay: 3000,
                disableOnInteraction: false,
            },
            navigation: {
                nextEl: '.swiper-button-next',
                prevEl: '.swiper-button-prev',
            },
        });
    </script>--%>

    <script type="text/javascript">
        function sweetalert_success(message, messagetype) {
            swal({
                title: message,
                icon: "success",
                button: "OK",
            }).then(function () {
                event.preventDefault();
            });
        }

        //function sweetalert_warning(message, messagetype) {
        //    swal({
        //        title: message,
        //        icon: "warning",
        //        button: "OK",
        //    }).then(function () {
        //        window.location.href = "http://qcplusmart.com/QCPlus/LoginPage.aspx";
        //    });
        //}


        function sweetalert_warning2(message, messagetype) {
            swal({
                title: message,
                icon: "warning",
                button: "OK",
                html: true
            });
        }

        //const iframe = document.getElementById('newsinfoframe');

        //function resizeIframe() {
        //    iframe.style.height = iframe.contentWindow.document.body.scrollHeight + 'px';
        //}

        //iframe.onload = resizeIframe;
        //window.addEventListener('resize', resizeIframe);

        function resizeIframe() {
            var iframe = document.getElementById('eventframe');
            iframe.style.height = iframe.contentWindow.document.documentElement.scrollHeight + 'px';
        }


    </script>
</asp:Content>
