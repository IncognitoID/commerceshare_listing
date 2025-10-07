<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="HQReport.aspx.cs" Inherits="GroupListing" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="msgBox" Namespace="BunnyBear" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <title>Ezyshare | HQ Report</title>
    <style type="text/css">
        .txt_Search {
            background: url(Images/calendar2.png) no-repeat;
            border:1px solid #ccc;
            background-color:White;
        }

        .dropdown_filter {
            padding-left: 2px;
            border:1px solid #ccc;
            background-color:White;
        }
    </style>
    <style type="text/css">
        @media(max-width:479px) {
            .head-tittle {
                font-size: 18px;
            }
        }

        @media(max-width:767px) {
            .head-tittle {
                font-size: 22px;
            }
        }

        @media (min-width: 767px) {
            .head-tittle {
                font-size: 28px;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div>
        <div class="col-xs-12 col-sm-10 col-lg-8" style="margin-left: auto; margin-right: auto; text-align: left; margin-top: 10px;">
            <a class="head-tittle" style="font-weight: bold;">Sales Performance Report</a>
        </div>
        <div class="col-xs-12 col-sm-10 col-lg-6" style="margin-left: auto; margin-right: auto;">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:Label ID="MF_Con" runat="server" Text="Label" Visible="false"></asp:Label>
                    <br />
                    <asp:Label ID="Label2" runat="server" Text="Merchant" Font-Size="14px" Font-Bold="true" ></asp:Label>
                    <br />
                    <asp:DropDownList ID="ddlMerchant" runat="server" CssClass="col-md-11 col-xs-11 dropdown_filter" AutoPostBack="true" OnSelectedIndexChanged="ddlMerchant_SelectedIndexChanged" style="height:34px; padding:6px 12px; font-size:14px;" >
                    </asp:DropDownList>
                    <br /><br /><br />
                    <asp:Label ID="Label3" runat="server" Text="Outlet" Font-Size="14px" Font-Bold="true" ></asp:Label>
                    <br />
                    <asp:DropDownList ID="ddlOutlet" runat="server" CssClass="col-md-11 col-xs-11 dropdown_filter" style="height:34px; padding:6px 12px; font-size:14px;" >
                    </asp:DropDownList>
                    <br /><br /><br />
                    <asp:Label ID="Label4" runat="server" Text="Date range" Font-Size="14px" Font-Bold="true" ></asp:Label>
                    <br />
                    <asp:TextBox ID="txt_FDate" runat="server" autocomplete="off" CssClass="col-md-5 col-xs-11 txt_Search" placeholder="From (yyyy/mm/dd)" style="height:34px; padding:6px 12px; padding-left:35px; font-size:14px;" ></asp:TextBox>
                    <asp:Label ID="Label1" runat="server" CssClass="col-md-1 col-xs-1" ></asp:Label>
                    <asp:TextBox ID="txt_TDate" runat="server" autocomplete="off" CssClass="col-md-5 col-xs-11 txt_Search" placeholder="To (yyyy/mm/dd)" style="height:34px; padding:6px 12px; padding-left:35px; font-size:14px;" ></asp:TextBox>
                    <br /><br /><br />
                    <asp:CheckBox ID="chk_office" runat="server" />
                    <asp:Label ID="officeChk_Label" Text="For Office Use" Font-Size="14px" Font-Bold="true" runat="server"></asp:Label>
                    <br /><br /><br />
                    <asp:Button ID="btn_Excel" runat="server" style="float: left !important;" Text="Export" OnClick="btn_Excel_Click" CssClass="ghost-button-thick-border" />
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btn_Excel" />
                    <asp:AsyncPostBackTrigger ControlID="ddlMerchant" EventName="SelectedIndexChanged" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>