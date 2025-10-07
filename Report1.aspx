<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Report1.aspx.cs" Inherits="GroupListing" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="msgBox" Namespace="BunnyBear" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        .txt_Search
        {
            background: url(Images/calendar2.png) no-repeat;
            padding-left: 18px;
          
            border:1px solid #ccc;
            background-color:White;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<cc1:msgbox id="MsgBox1" runat="server"></cc1:msgbox>
    <section id="main-content">
        <section class="wrapper">
            <asp:HiddenField ID="PaneName" runat="server" />
                <div class="col-md-8 col-sm-10" style="color:Black;">
                    <asp:Label ID="Label2" runat="server" Text="Sales Performance Report" style="font-size:16px; font-weight: bold;" ></asp:Label>
                </div>
                <div class="col-md-8 col-sm-10" style="color:Black">
                    <br /><br />
                    <asp:Label ID="MF_Con" runat="server" Text="Label" Visible="false"></asp:Label>
                    <asp:TextBox ID="txt_FDate" runat="server" autocomplete="off" CssClass="col-md-5 col-xs-11 txt_Search" placeholder="From (yyyy/mm/dd)" style=" border:2; height:34px; padding:6px 12px; padding-left:35px; font-size:14px;" ></asp:TextBox>
                    <asp:Label ID="Label1" runat="server" CssClass="col-md-1 col-xs-1" ></asp:Label>
                    <asp:TextBox ID="txt_TDate" runat="server" autocomplete="off" CssClass="col-md-5 col-xs-11 txt_Search" placeholder="To (yyyy/mm/dd)" style=" border:2; height:34px; padding:6px 12px; padding-left:35px; font-size:14px;" ></asp:TextBox>
                    <br /><br />
                </div>
                <div class="col-md-8 col-sm-10" style="color:Black;">
                    <br /><br />
                    <asp:Button ID="btn_Excel" runat="server" style="float: left !important;" Text="Export" OnClick="btn_Excel_Click" CssClass="ghost-button-thick-border" />
                </div>
        </section><!-- wrapper -->
    </section><!-- MAIN CONTENT -->
</asp:Content>