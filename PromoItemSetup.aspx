<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PromoItemSetup.aspx.cs" Inherits="PromoItemSetup" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtoolkit" %>
<%@ Register Assembly="msgBox" Namespace="BunnyBear" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .clear {
            clear: both;
        }

        .flex-container {
            display: grid;
        }

        .GridStyle td {
            border: none;
            border-bottom: 2px solid lightgray;
        }


        .GridStyle {
            background: white !important;
            border: none !important;
        }

            .GridStyle td {
                border-bottom: 1px solid lightgray !important;
                padding-left: 5px;
                font-weight: normal !important;
                max-height: 20px !important;
                padding: 7px !important;
            }

            .GridStyle tr th {
                background-color: #696969;
                background: #696969 !important;
                color: white !important;
                border: none;
                text-align: left !important;
                cursor: default;
                font-size: 14px !important;
                font-weight: normal !important;
                font-family: Arial, Helvetica, sans-serif;
            }


            .GridStyle tbody tr td {
                text-align: left;
                font-size: 12px !important;
            }

            .GridStyle tbody tr:nth-child(even) {
                background-color: white !important
            }

            .GridStyle tbody tr:nth-child(odd) {
                background-color: white !important
            }


        .Hoverable tbody tr:hover, .Hoverable tbody li:hover {
            background-color: rgb(241, 241, 241) !important;
            cursor: default;
        }


        .lnkbtn i:hover {
            color: #B9B9B9;
            padding: 3px;
            border-radius: 10px;
        }

        .table table tbody tr td a,
        .table table tbody tr td span {
            position: relative;
            float: left;
            padding: 6px 12px;
            margin-left: -1px;
            line-height: 1.42857143;
            color: #337ab7;
            text-decoration: none;
            background-color: #fff;
            border: 1px solid #ddd;
        }

        .table table > tbody > tr > td > span {
            z-index: 3;
            color: #fff;
            cursor: default;
            background-color: #337ab7;
            border-color: #337ab7;
        }

        .table table > tbody > tr > td:first-child > a,
        .table table > tbody > tr > td:first-child > span {
            margin-left: 0;
            border-top-left-radius: 4px;
            border-bottom-left-radius: 4px;
        }

        .table table > tbody > tr > td:last-child > a,
        .table table > tbody > tr > td:last-child > span {
            border-top-right-radius: 4px;
            border-bottom-right-radius: 4px;
        }

        .table table > tbody > tr > td > a:hover,
        .table table > tbody > tr > td > span:hover,
        .table table > tbody > tr > td > a:focus,
        .table table > tbody > tr > td > span:focus {
            z-index: 2;
            color: #23527c;
            background-color: #eee;
            border-color: #ddd;
        }
    </style>
    <script>
        function IsNumberWithOneDecimal(txt, evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57) && !(charCode == 46 || charCode == 8)) {
                return false;
            } else {
                var len = txt.value.length;
                var index = txt.value.indexOf('.');
                if (index > 0 && charCode == 46) {
                    return false;
                }
                if (index > 0) {
                    if ((len + 1) - index > 4) {
                        return false;
                    }
                }
            }
            return true;
        }

        function Calculation()
        {

        }

    </script>

    <script>
        function calculate() {
            var disctype = document.getElementById('<%= ddlMdiscountType.ClientID %>').value;
            var number1 = parseFloat(document.getElementById('<%= txtMDiscountAmt.ClientID %>').value);
            var number2 = parseFloat(document.getElementById('<%= lblMprice.ClientID %>').innerText);

            if (disctype == "DBP") {
                var result = number2 - (number2 * (number1 / 100));

                if (isNaN(result)) {
                    result = number2;
                }
            }
            else if (disctype == "DBA") {
                var result = number2 - number1;

                if (isNaN(result)) {
                    result = number2;
                }
            }

            document.getElementById('<%= lblMpromoPrice.ClientID %>').value = result.toFixed(2);
        }

        function calculate1() {
            var disctype = document.getElementById('<%= ddlVdiscountType.ClientID %>').value;
             var number1 = parseFloat(document.getElementById('<%= txtVDiscountAmt.ClientID %>').value);
            var number2 = parseFloat(document.getElementById('<%= lblVPrice.ClientID %>').innerText);

            if (disctype == "DBP") {
                var result = number2 - (number2 * (number1 / 100));

                if (isNaN(result)) {
                    result = number2;
                }
            }
            else if (disctype == "DBA") {
                var result = number2 - number1;

                if (isNaN(result)) {
                    result = number2;
                }
            }

            document.getElementById('<%= lblVpromoPrice.ClientID %>').value = result.toFixed(2);
        }

        function calculate2() {
            var disctype = document.getElementById('<%= ddlVVIPdiscountType.ClientID %>').value;
             var number1 = parseFloat(document.getElementById('<%= txtVVIPDiscountAmt.ClientID %>').value);
            var number2 = parseFloat(document.getElementById('<%= lblVVIPPrice.ClientID %>').innerText);

            if (disctype == "DBP") {
                var result = number2 - (number2 * (number1 / 100));

                if (isNaN(result)) {
                    result = number2;
                }
            }
            else if (disctype == "DBA") {
                var result = number2 - number1;

                if (isNaN(result)) {
                    result = number2;
                }
            }

            document.getElementById('<%= lblVVIPpromoPrice.ClientID %>').value = result.toFixed(2);
        }

        function calculate3() {
            var disctype = document.getElementById('<%= ddlSdiscountType.ClientID %>').value;
             var number1 = parseFloat(document.getElementById('<%= txtSDiscountAmt.ClientID %>').value);
            var number2 = parseFloat(document.getElementById('<%= lblSPrice.ClientID %>').innerText);

            if (disctype == "DBP") {
                var result = number2 - (number2 * (number1 / 100));

                if (isNaN(result)) {
                    result = number2;
                }
            }
            else if (disctype == "DBA") {
                var result = number2 - number1;

                if (isNaN(result)) {
                    result = number2;
                }
            }

            document.getElementById('<%= lblSpromoPrice.ClientID %>').value = result.toFixed(2);
        }

        function calculate4() {
            var disctype = document.getElementById('<%= ddlMSdiscountType.ClientID %>').value;
             var number1 = parseFloat(document.getElementById('<%= txtMSDiscountAmt.ClientID %>').value);
            var number2 = parseFloat(document.getElementById('<%= lblMSPrice.ClientID %>').innerText);

            if (disctype == "DBP") {
                var result = number2 - (number2 * (number1 / 100));

                if (isNaN(result)) {
                    result = number2;
                }
            }
            else if (disctype == "DBA") {
                var result = number2 - number1;

                if (isNaN(result)) {
                    result = number2;
                }
            }

            document.getElementById('<%= lblMSpromoPrice.ClientID %>').value = result.toFixed(2);
         }
    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Label ID="MF_Con" runat="server" Text="Label" Visible="false"></asp:Label>
    <asp:Label ID="lblrunno" runat="server" Visible="false"></asp:Label>

    <a data-toggle="collapse" data-parent="#accordion" href="#collapse1" id="groupingtitle" runat="server"></a>

    <div class="has-success">
        <label class="col-sm-10 col-lg-4" id="lblcampaigntitle" runat="server"></label>
    </div>

    <div class="clear flex-container" style="background-color: white; border-radius: 5px; margin: 0px; padding: 20px;">
        <asp:UpdatePanel ID="updateee" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div style="display: inline-block; width: 100%">
                    <label id="Label5" runat="server" class="col-sm-10 control-label col-lg-4">Campaign Name </label>
                    <div class="form-group col-sm-7">
                        <asp:TextBox ID="txtcampaignname" runat="server" AutoComplete="off" CssClass="col-md-12 col-xs-12 col-lg-12" Style="border: 2; height: 34px; padding: 6px 12px; font-size: 14px;"></asp:TextBox>
                    </div>
                    <br />
                    <label id="Label6" runat="server" class="col-sm-10 control-label col-lg-4">Campaign Period <span id="Span1" runat="server"></span></label>
                    <div class="form-group col-sm-3">
                        <asp:TextBox ID="txtstartdate" TextMode="DateTimeLocal" runat="server" AutoComplete="off" CssClass="col-md-12 col-xs-12 col-lg-12" Style="border: 2; height: 34px; padding: 6px 12px; font-size: 14px;"></asp:TextBox>
                    </div>
                    <div class="form-group col-sm-1" style="text-align:center;vertical-align:middle; height: 34px; padding: 6px 12px; font-size: 14px;">
                       <span class="glyphicon glyphicon-minus"></span>
                    </div>
                    <div class="form-group col-sm-3">
                        <asp:TextBox ID="txtenddate" TextMode="DateTimeLocal" runat="server" AutoComplete="off" CssClass="col-md-12 col-xs-12 col-lg-12" Style="border: 2; height: 34px; padding: 6px 12px; font-size: 14px;"></asp:TextBox>
                    </div>

                </div>
                <div class="panel panel-default" style="padding: 5px; margin: 5px; border: none !important">
                    <div id="dvTab">
                        <ul id="myTab" class="nav nav-tabs bar_tabs" role="tablist">
                            <li role="presentation" id="t3" class="active"><a href="#tab3" role="tab" id="tab33" data-toggle="tab" aria-expanded="false" style="color: black">Promotion Item&nbsp;<span id="bangetotaladdlist" style="color: White; background-color: #ae7ac4" runat="server" class="badge badge-success">0</span></a>
                            </li>
                            <li role="presentation" id="t1" class=""><a href="#tab1" id="tab11" role="tab" data-toggle="tab" aria-expanded="true" style="color: black">Add Item</a>
                            </li>
                            
                        </ul>
                        <div id="myTabContent" class="tab-content">

                            <div role="tabpanel" class="tab-pane fade  in" id="tab1" style="background-color: transparent !important" aria-labelledby="home-tab">
                                <asp:TextBox ID="txt_search" runat="server" placeholder="Filter By Item code/ Category Code / Description" Style="border: 2; height: 34px; padding: 6px 12px; font-size: 14px;" CssClass="col-md-8 col-xs-8"></asp:TextBox>
                                <asp:Button ID="btnsearch" Text="Search" OnClick="btnsearch_Click" runat="server" CssClass="btn" Style="padding: 6px; width: 100px; margin-left: 5px; text-align: center; text-decoration: none; display: inline-block; cursor: pointer;" />
                                <br />
                                <br />
                                <div style="overflow: auto; height: 400px;">
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <asp:GridView ID="item_grdview" runat="server" Width="100%" AutoGenerateColumns="false" AllowPaging="true" OnPageIndexChanging="item_grdview_PageIndexChanging" OnRowCommand="item_grdview_RowCommand" CssClass="footableThis GridStyle Hoverable popoutGrid">
                                                <Columns>
                                                    <asp:TemplateField ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkadditem" CssClass="btn btn-default" Text="Add" Style="background-color: mediumpurple; color: white" runat="server" CommandArgument='<%# Eval("Item_code") %>' CommandName="addpromo"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:ImageField DataImageUrlField="FilePath" ControlStyle-Width="100" ItemStyle-Width="120" ControlStyle-Height="100" />
                                                    <asp:TemplateField HeaderText="Item Code">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblitemcode" runat="server" Text='<%# Eval("Item_code") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Description" ItemStyle-Width="150">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbldescription" runat="server" Text='<%# Eval("longdesc") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                </Columns>
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>

                            </div>
                            <div role="tabpanel" class="tab-pane fade active in" id="tab3" aria-labelledby="home-tab" style="padding: 5px;background-color: transparent !important;">
                                <div class="flex-container">
                                    <asp:GridView ID="promotionitem_grdview" runat="server" Width="100%" EmptyDataText="No records has been added." OnRowCommand="promotionitem_grdview_RowCommand" AutoGenerateColumns="false" GridLines="None" DataKeyNames="Item_code" CssClass="footableThis GridStyle Hoverable popoutGrid">
                                        <Columns>
                                            <asp:TemplateField ItemStyle-Width="150px">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkedit" runat="server" Text="Edit Promotion" CommandArgument='<%# Eval("Item_Code") %>' CommandName="EditItem"></asp:LinkButton>
                                                    <br />
                                                    <asp:LinkButton ID="lnkDel" runat="server" Text="Delete Promotion" CommandArgument='<%# Eval("Item_Code") %>' CommandName="DelItem"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:ImageField DataImageUrlField="FilePath" ControlStyle-Width="100" ItemStyle-Width="120" ControlStyle-Height="100" />
                                           <asp:BoundField DataField="Item_Code" HeaderText="Item Code" />
                                           <asp:BoundField DataField="LongDesc" HeaderText="Item Name" />
                                        </Columns>
                                        <RowStyle BackColor="White" />
                                        <SelectedRowStyle BackColor="#444444" Font-Bold="True" ForeColor="White" Height="45px" />
                                        <HeaderStyle BackColor="#444444" ForeColor="White" Height="45px" />
                                        <EditRowStyle BackColor="#999999" Height="45px" />
                                        <AlternatingRowStyle BackColor="White" Height="45px" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <br />
    <div style="float: right">
        <asp:Button ID="btnupdate" runat="server" Text="Update" CssClass="btn btn-success" OnClick="btnupdate_Click" Style="padding: 6px; width: 100px; margin-left: 5px; text-align: center; text-decoration: none; display: inline-block; cursor: pointer;" />
        <asp:Button ID="btnback" runat="server" Text="Back" CssClass="btn btn-warning" OnClick="btnback_Click" Style="padding: 6px; width: 100px; margin-left: 5px; text-align: center; text-decoration: none; display: inline-block; cursor: pointer;" />
    </div>
    <asp:UpdatePanel ID="updatepanel2" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <!--start modal-->
            <div class="modal fade" id="ItemAddModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
                <div class="modal-dialog model-xl" role="document" style="width:100%;height: 100% !important; margin: auto !important;">
                    <div class="modal-content" style="width: auto;margin:0px !important">
                        <div class="modal-header">
                            <h5 class="modal-title" id="exampleModalLabel" runat="server" style="font-size: large!important">Promotion Item Setting  
                                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                            <span aria-hidden="true">&times;</span>
                                        </button>
                            </h5>

                        </div>
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <div class="modal-body" style="display: inline-block; width: 100%">
                                    <div class="col-xs-2 col-lg-2 col-md-2" style="margin-bottom: 5px;">
                                        <asp:Image runat="server" ID="btnimage" Text="edit" class="imageMiddle" CssClass="imgSize" Style="border-radius: 5px 5px 5px 5px;" />
                                        <br />
                                        <asp:Label ID="lblitem_code" runat="server" Style="font-weight: bold; font-size: medium" Visible="false"></asp:Label>
                                        <asp:Label ID="lblitemname" runat="server" Style="font-weight: bold; font-size: medium"></asp:Label>
                                        <br />
                                        <asp:Label ID="lblbarcode" runat="server" Style="font-weight: bold; font-size: small"></asp:Label>
                                    </div>
                                    <div class="col-sm-10 col-xs-10 col-md-10 col-lg-10 goleft" style="color: Black; padding-left: 5px; padding-right: 5px;">
                                        <div>
                                            <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2">
                                            </div>
                                            <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2" style="text-align: center; border: 1px solid lightgrey;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                    <asp:Label ID="Label7" runat="server" Text="Sell Price" Font-Size="12px" Font-Bold="true" ForeColor="Black"></asp:Label>
                                                </p>
                                            </div>
                                            <div class="col-xs-3 col-sm-3 col-md-3 col-lg-3" style="text-align: center; border: 1px solid lightgrey;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                    <asp:Label ID="Label18" runat="server" Text="Discount Type" Font-Size="12px" Font-Bold="true" ForeColor="Black"></asp:Label>
                                                </p>
                                            </div>
                                            <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2" style="text-align: center; border: 1px solid lightgrey;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                    <asp:Label ID="Label23" runat="server" Text="Discount" Font-Size="12px" Font-Bold="true" ForeColor="Black"></asp:Label>
                                                </p>
                                            </div>
                                            <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2" style="text-align: center; border: 1px solid lightgrey;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                    <asp:Label ID="Label21" runat="server" Text="Promotion Price" Font-Size="12px" Font-Bold="true" ForeColor="Black"></asp:Label>
                                                </p>
                                            </div>
                                            <div class="col-xs-1 col-sm-1 col-md-1 col-lg-1" style="text-align: center; border: 1px solid lightgrey;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                    <asp:Label ID="Label22" runat="server" Text="Cashback" Font-Size="12px" Font-Bold="true" ForeColor="Black"></asp:Label>
                                                </p>
                                            </div>
                                        </div>
                                        <div>
                                            <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2" style="border: 1px solid lightgrey; min-height: 35px;">
                                                <p style="margin-bottom: 3px !important; padding-left: 5px; padding-top: 6px;">
                                                    <asp:Label ID="Label2" runat="server" Font-Size="12px" Text="Member : " ForeColor="Black"></asp:Label>
                                                </p>
                                            </div>
                                            <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                    <asp:Label Style="border: none; text-align: right;" inputmode="decimal" ID="lblMprice" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="100%" Height="33px"></asp:Label>
                                                </p>
                                            </div>
                                             <div class="col-xs-3 col-sm-3 col-md-3 col-lg-3" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                    <asp:DropDownList ID="ddlMdiscountType" runat="server" onchange="calculate();" style="width: 100%; height: 33px; padding: 6px 12px; font-size: 14px; line-height: 1.42857143; color: #555; background-color: #fff; background-image: none; border: 1px solid #ccc; border-radius: 4px;">
                                                        <asp:ListItem Text="Discount By Percentage (%)" Value="DBP"></asp:ListItem>
                                                        <asp:ListItem Text="Discount By Amount" Value="DBA"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </p>
                                            </div>
                                             <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                    <asp:TextBox Style="border: 1px solid; text-align: right;" inputmode="decimal" ID="txtMDiscountAmt" onkeyup="calculate();" onkeypress=" return IsNumberWithOneDecimal(this,event); " Text="0" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="60%" Height="33px"></asp:TextBox>
                                                </p>
                                            </div>
                                            <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                    <asp:TextBox Style="border: 1px solid; text-align: right;" inputmode="decimal" ID="lblMpromoPrice" onkeypress="return IsNumberWithOneDecimal(this,event);" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="60%" Height="33px"></asp:TextBox>
                                                </p>
                                            </div>
                                            <div class="col-xs-1 col-sm-1 col-md-1 col-lg-1" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                    <asp:TextBox Style="border: none; text-align: right;" ID="txtMpromocashback" Enabled="false" inputmode="decimal" onkeypress="return IsNumberWithOneDecimal(this,event);" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="gray" Width="60%" Height="33px"></asp:TextBox>
                                                </p>
                                            </div>
                                        </div>
                                        <div>
                                            <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2" style="border: 1px solid lightgrey; min-height: 35px;">
                                                <p style="margin-bottom: 3px !important; padding-left: 5px; padding-top: 6px;">
                                                    <asp:Label ID="Label4" runat="server" Font-Size="12px" Text="VIP : " ForeColor="Black"></asp:Label>
                                                </p>
                                            </div>
                                            <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                    <asp:Label Style="border: none; text-align: right;" inputmode="decimal" ID="lblVPrice" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="100%" Height="33px"></asp:Label>
                                                </p>
                                            </div>
                                               <div class="col-xs-3 col-sm-3 col-md-3 col-lg-3" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                    <asp:DropDownList ID="ddlVdiscountType" runat="server"  onchange="calculate1();" style="width: 100%; height: 33px; padding: 6px 12px; font-size: 14px; line-height: 1.42857143; color: #555; background-color: #fff; background-image: none; border: 1px solid #ccc; border-radius: 4px;">
                                                        <asp:ListItem Text="Discount By Percentage (%)" Value="DBP"></asp:ListItem>
                                                        <asp:ListItem Text="Discount By Amount" Value="DBA"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </p>
                                            </div>
                                             <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                    <asp:TextBox Style="border: 1px solid; text-align: right;" inputmode="decimal" onkeyup="calculate1();" ID="txtVDiscountAmt" Text="0" onkeypress="return IsNumberWithOneDecimal(this,event);" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="60%" Height="33px"></asp:TextBox>
                                                </p>
                                            </div>
                                            <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                    <asp:TextBox Style="border: 1px solid; text-align: right;" ID="lblVpromoPrice" inputmode="decimal" onkeypress="return IsNumberWithOneDecimal(this,event);" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="60%" Height="33px"></asp:TextBox>
                                                </p>
                                            </div>
                                            <div class="col-xs-1 col-sm-1 col-md-1 col-lg-1" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                    <asp:TextBox Style="border: none; text-align: right;" ID="txtVpromocashback" Enabled="false" inputmode="decimal" onkeypress="return IsNumberWithOneDecimal(this,event);" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="gray" Width="60%" Height="33px"></asp:TextBox>
                                                </p>
                                            </div>
                                        </div>
                                        <div>
                                            <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2" style="border: 1px solid lightgrey; min-height: 35px;">
                                                <p style="margin-bottom: 3px !important; padding-left: 5px; padding-top: 6px;">
                                                    <asp:Label ID="Label9" runat="server" Font-Size="12px" Text="VVIP : " ForeColor="Black"></asp:Label>

                                                </p>
                                            </div>
                                            <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                    <asp:Label Style="border: none; text-align: right;" inputmode="decimal" ID="lblVVIPPrice" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="100%" Height="33px"></asp:Label>
                                                </p>
                                            </div>
                                              <div class="col-xs-3 col-sm-3 col-md-3 col-lg-3" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                       <asp:DropDownList ID="ddlVVIPdiscountType" runat="server" onchange="calculate2();" style="width: 100%; height: 33px; padding: 6px 12px; font-size: 14px; line-height: 1.42857143; color: #555; background-color: #fff; background-image: none; border: 1px solid #ccc; border-radius: 4px;">
                                                        <asp:ListItem Text="Discount By Percentage (%)" Value="DBP"></asp:ListItem>
                                                        <asp:ListItem Text="Discount By Amount" Value="DBA"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </p>
                                            </div>
                                             <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                    <asp:TextBox Style="border: 1px solid; text-align: right;" onkeyup="calculate2();" inputmode="decimal" ID="txtVVIPDiscountAmt" Text="0" onkeypress="return IsNumberWithOneDecimal(this,event);" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="60%" Height="33px"></asp:TextBox>
                                                </p>
                                            </div>
                                            <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                    <asp:TextBox Style="border: 1px solid; text-align: right;" ID="lblVVIPpromoPrice" inputmode="decimal" onkeypress="return IsNumberWithOneDecimal(this,event);" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="60%" Height="33px"></asp:TextBox>
                                                </p>
                                            </div>
                                            <div class="col-xs-1 col-sm-1 col-md-1 col-lg-1" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                    <asp:TextBox Style="border: 1px solid; text-align: right;" ID="txtVVIPpromocashback" inputmode="decimal" onkeypress="return IsNumberWithOneDecimal(this,event);" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="60%" Height="33px"></asp:TextBox>
                                                </p>
                                            </div>
                                        </div>
                                        <div>
                                            <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2" style="border: 1px solid lightgrey; min-height: 35px;">
                                                <p style="margin-bottom: 3px !important; padding-left: 5px; padding-top: 6px;">
                                                    <asp:Label ID="Label16" runat="server" Font-Size="12px" Text="Stockist : " ForeColor="Black"></asp:Label>

                                                </p>
                                            </div>
                                            <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                    <asp:Label Style="border: none; text-align: right;" inputmode="decimal" ID="lblSPrice" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="100%" Height="33px"></asp:Label>
                                                </p>
                                            </div>
                                             <div class="col-xs-3 col-sm-3 col-md-3 col-lg-3" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                     <asp:DropDownList ID="ddlSdiscountType" runat="server" onchange="calculate3();" style="width: 100%; height: 33px; padding: 6px 12px; font-size: 14px; line-height: 1.42857143; color: #555; background-color: #fff; background-image: none; border: 1px solid #ccc; border-radius: 4px;">
                                                        <asp:ListItem Text="Discount By Percentage (%)" Value="DBP"></asp:ListItem>
                                                        <asp:ListItem Text="Discount By Amount" Value="DBA"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </p>
                                            </div>
                                             <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                    <asp:TextBox Style="border: 1px solid; text-align: right;" onkeyup="calculate3();" inputmode="decimal" ID="txtSDiscountAmt" Text="0" onkeypress="return IsNumberWithOneDecimal(this,event);" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="60%" Height="33px"></asp:TextBox>
                                                </p>
                                            </div>
                                            <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                    <asp:TextBox Style="border: 1px solid; text-align: right;" ID="lblSpromoPrice" inputmode="decimal" onkeypress="return IsNumberWithOneDecimal(this,event);" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="60%" Height="33px"></asp:TextBox>
                                                </p>
                                            </div>
                                            <div class="col-xs-1 col-sm-1 col-md-1 col-lg-1" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                    <asp:TextBox Style="border: 1px solid; text-align: right;" ID="txtSpromocashback" inputmode="decimal" onkeypress="return IsNumberWithOneDecimal(this,event);" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="60%" Height="33px"></asp:TextBox>
                                                </p>
                                            </div>
                                        </div>
                                        <div>
                                            <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2" style="border: 1px solid lightgrey; min-height: 35px;">
                                                <p style="margin-bottom: 3px !important; padding-left: 5px; padding-top: 6px;">
                                                    <asp:Label ID="Label20" runat="server" Font-Size="12px" Text="Master Stockist : " ForeColor="Black"></asp:Label>

                                                </p>
                                            </div>
                                            <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                    <asp:Label Style="border: none; text-align: right;" inputmode="decimal" ID="lblMSPrice" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="100%" Height="33px"></asp:Label>
                                                </p>
                                            </div>
                                              <div class="col-xs-3 col-sm-3 col-md-3 col-lg-3" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                       <asp:DropDownList ID="ddlMSdiscountType" runat="server" onchange="calculate4();" style="width: 100%; height: 33px; padding: 6px 12px; font-size: 14px; line-height: 1.42857143; color: #555; background-color: #fff; background-image: none; border: 1px solid #ccc; border-radius: 4px;">
                                                        <asp:ListItem Text="Discount By Percentage (%)" Value="DBP"></asp:ListItem>
                                                        <asp:ListItem Text="Discount By Amount" Value="DBA"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </p>
                                            </div>
                                             <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                    <asp:TextBox Style="border: 1px solid; text-align: right;" onkeyup="calculate4();" inputmode="decimal" ID="txtMSDiscountAmt" Text="0" onkeypress="return IsNumberWithOneDecimal(this,event);" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="60%" Height="33px"></asp:TextBox>
                                                </p>
                                            </div>
                                            <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                    <asp:TextBox Style="border: 1px solid; text-align: right;" ID="lblMSpromoPrice" inputmode="decimal" onkeypress="return IsNumberWithOneDecimal(this,event);" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="60%" Height="33px"></asp:TextBox>
                                                </p>
                                            </div>
                                            <div class="col-xs-1 col-sm-1 col-md-1 col-lg-1" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                    <asp:TextBox Style="border: 1px solid; text-align: right;" ID="txtMSpromocashback" inputmode="decimal" onkeypress="return IsNumberWithOneDecimal(this,event);" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="60%" Height="33px"></asp:TextBox>
                                                </p>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="Tabs" role="tabpanel">
                                        <!-- Nav tabs -->
                                        <ul class="nav nav-tabs" role="tablist" id="myTab<%=cLine%>">
                                            <li class="active"><a href="#member<%=cLine%>" style="line-height: 0.428571 !important;" aria-controls="member" role="tab" data-toggle="tab">Reward & Redemption</a></li>
                                            <li><a href="#stockist<%=cLine%>" aria-controls="stockist" role="tab" data-toggle="tab" style="line-height: 0.428571 !important;">Others</a></li>

                                            <%--                                                <asp:Button ID="btnpromo" runat="server" Text="Promotion" CssClass="btnsave" OnClick="btnpromo_Click" style="float:right;margin-right:10px" />--%>
                                        </ul>
                                        <!-- Tab panes -->
                                        <div class="tab-content">
                                            <!-- member tab -->
                                            <div role="tabpanel" class="tab-pane fade in active" id="member<%=cLine%>">
                                                <div class="col-sm-12 col-xs-12 col-md-12 col-lg-12" style="color: Black; padding-left: 5px; padding-right: 5px;">
                                                    <div class="has-success">
                                                        <label class="col-sm-10 col-lg-4">Redemption Item</label>
                                                        
                                                    </div>
                                                </div>

                                                <div class="col-sm-12 col-xs-12 col-md-12 col-lg-12" style="color: Black; padding-left: 5px; padding-right: 5px;">
                                                    <div>
                                                        <div class="col-xs-3 col-sm-3 col-md-3 col-lg-3" style="height: 30px;text-align: center;">
                                                            <p style="margin-bottom: 1px !important; padding-right: 2px;margin: 5px;">
                                                                <asp:Label ID="Label8" runat="server" Text="Member Level" Font-Size="12px" Font-Bold="true" ForeColor="Black"></asp:Label>
                                                            </p>
                                                        </div>
                                                        <div class="col-xs-4 col-sm-4 col-md-4 col-lg-4" style="height: 30px;text-align: center;">
                                                            <p style="margin-bottom: 1px !important; padding-right: 2px;margin: 5px;">
                                                                <asp:Label ID="Label11" runat="server" Text="Redemption Point" Font-Size="12px" Font-Bold="true" ForeColor="Black"></asp:Label>
                                                            </p>
                                                        </div>
                                                        <div class="col-xs-4 col-sm-4 col-md-4 col-lg-4" style="height: 30px;text-align: center;">
                                                            <p style="margin-bottom: 1px !important; padding-right: 2px;margin: 5px;">
                                                                <asp:Label ID="Label12" runat="server" Text="Member Reward Point" Font-Size="12px" Font-Bold="true" ForeColor="Black"></asp:Label>
                                                            </p>
                                                        </div>
                                                    </div>
                                                    <div>
                                                        <div class="col-xs-3 col-sm-3 col-md-3 col-lg-3" style=" min-height: 33px;">
                                                            <p style="margin-bottom: 3px !important; padding-left: 5px; padding-top: 6px;">
                                                                <asp:Label ID="Label15" runat="server" Font-Size="12px" Text="Member : " ForeColor="Black"></asp:Label>

                                                            </p>
                                                        </div>
                                                        <div class="col-xs-4 col-sm-4 col-md-4 col-lg-4" style="text-align: right; min-height: 30px;">
                                                            <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                                <asp:TextBox Style="border: 1px solid; text-align: right;" inputmode="decimal" ID="txtMredemptpoint" Text="0" onkeypress="return IsNumberWithOneDecimal(this,event);" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="60%" Height="33px"></asp:TextBox>
                                                            </p>
                                                        </div>
                                                        <div class="col-xs-4 col-sm-4 col-md-4 col-lg-4" style="text-align: right; min-height: 30px;">
                                                            <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                                <asp:TextBox Style="border: 1px solid; text-align: right;" ID="txtMrewardpoint" inputmode="decimal" Text="0" onkeypress="return IsNumberWithOneDecimal(this,event);" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="60%" Height="33px"></asp:TextBox>
                                                            </p>
                                                        </div>
                                                    </div>
                                                    <div>
                                                        <div class="col-xs-3 col-sm-3 col-md-3 col-lg-3" style=" min-height: 33px;">
                                                            <p style="margin-bottom: 3px !important; padding-left: 5px; padding-top: 6px;">
                                                                <asp:Label ID="Label19" runat="server" Font-Size="12px" Text="VIP : " ForeColor="Black"></asp:Label>

                                                            </p>
                                                        </div>
                                                        <div class="col-xs-4 col-sm-4 col-md-4 col-lg-4" style="text-align: right; min-height: 30px;">
                                                            <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                                <asp:TextBox Style="border: 1px solid; text-align: right;" ID="txtVIPredemptpoint" inputmode="decimal" Text="0" onkeypress="return IsNumberWithOneDecimal(this,event);" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="60%" Height="33px"></asp:TextBox>
                                                            </p>
                                                        </div>
                                                        <div class="col-xs-4 col-sm-4 col-md-4 col-lg-4" style="text-align: right; min-height: 30px;">
                                                            <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                                <asp:TextBox Style="border: 1px solid; text-align: right;" ID="txtVIPrewardpoint" inputmode="decimal" Text="0" onkeypress="return IsNumberWithOneDecimal(this,event);" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="60%" Height="33px"></asp:TextBox>
                                                            </p>
                                                        </div>
                                                    </div>
                                                    <div>
                                                        <div class="col-xs-3 col-sm-3 col-md-3 col-lg-3" style=" min-height: 33px;">
                                                            <p style="margin-bottom: 3px !important; padding-left: 5px; padding-top: 6px;">
                                                                <asp:Label ID="Label24" runat="server" Font-Size="12px" Text="VVIP : " ForeColor="Black"></asp:Label>

                                                            </p>
                                                        </div>
                                                        <div class="col-xs-4 col-sm-4 col-md-4 col-lg-4" style="text-align: right; min-height: 30px;">
                                                            <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                                <asp:TextBox Style="border: 1px solid; text-align: right;" ID="txtVVIPredemptpoint" inputmode="decimal" Text="0" onkeypress="return IsNumberWithOneDecimal(this,event);" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="60%" Height="33px"></asp:TextBox>
                                                            </p>
                                                        </div>
                                                        <div class="col-xs-4 col-sm-4 col-md-4 col-lg-4" style="text-align: right; min-height: 30px;">
                                                            <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                                <asp:TextBox Style="border: 1px solid; text-align: right;" ID="txtVVIPrewardpoint" inputmode="decimal" Text="0" onkeypress="return IsNumberWithOneDecimal(this,event);" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="60%" Height="33px"></asp:TextBox>
                                                            </p>
                                                        </div>
                                                    </div>
                                                    <div>
                                                        <div class="col-xs-3 col-sm-3 col-md-3 col-lg-3" style=" min-height: 33px;">
                                                            <p style="margin-bottom: 3px !important; padding-left: 5px; padding-top: 6px;">
                                                                <asp:Label ID="Label26" runat="server" Font-Size="12px" Text="Stockist : " ForeColor="Black"></asp:Label>

                                                            </p>
                                                        </div>
                                                        <div class="col-xs-4 col-sm-4 col-md-4 col-lg-4" style="text-align: right; min-height: 30px;">
                                                            <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                                <asp:TextBox Style="border: 1px solid; text-align: right;" ID="txtSredemptpoint" inputmode="decimal" Text="0" onkeypress="return IsNumberWithOneDecimal(this,event);" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="60%" Height="33px"></asp:TextBox>
                                                            </p>
                                                        </div>
                                                        <div class="col-xs-4 col-sm-4 col-md-4 col-lg-4" style="text-align: right; min-height: 30px;">
                                                            <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                                <asp:TextBox Style="border: 1px solid; text-align: right;" ID="txtSrewardpoint" inputmode="decimal" Text="0" onkeypress="return IsNumberWithOneDecimal(this,event);" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="60%" Height="33px"></asp:TextBox>
                                                            </p>
                                                        </div>
                                                    </div>
                                                    <div>
                                                        <div class="col-xs-3 col-sm-3 col-md-3 col-lg-3" style=" min-height: 33px;">
                                                            <p style="margin-bottom: 3px !important; padding-left: 5px; padding-top: 6px;">
                                                                <asp:Label ID="Label28" runat="server" Font-Size="12px" Text="Master Stockist : " ForeColor="Black"></asp:Label>

                                                            </p>
                                                        </div>
                                                        <div class="col-xs-4 col-sm-4 col-md-4 col-lg-4" style="text-align: right; min-height: 30px;">
                                                            <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                                <asp:TextBox Style="border: 1px solid; text-align: right;" ID="txtMSredemptpoint" inputmode="decimal" Text="0" onkeypress="return IsNumberWithOneDecimal(this,event);" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="60%" Height="33px"></asp:TextBox>
                                                            </p>
                                                        </div>
                                                        <div class="col-xs-4 col-sm-4 col-md-4 col-lg-4" style="text-align: right; min-height: 30px;">
                                                            <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                                <asp:TextBox Style="border: 1px solid; text-align: right;" ID="txtMSrewardpoint" inputmode="decimal" Text="0" onkeypress="return IsNumberWithOneDecimal(this,event);" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="60%" Height="33px"></asp:TextBox>
                                                            </p>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <!-- stockist tab -->
                                            <div role="tabpanel" class="tab-pane fade in" id="stockist<%=cLine%>">

                                                <div class="col-sm-12 col-xs-12 col-md-12 col-lg-12" style="height: 20px">
                                                </div>
                                                <div class="col-sm-12 col-xs-12 col-md-12 col-lg-12" style="color: Black; padding-left: 5px; padding-right: 5px;">
                                                    <div class="has-success">
                                                        <label id="Label1" runat="server" class="col-sm-7 control-label col-lg-7">SPP</label>
                                                        <div class="form-group col-sm-5">
                                                            <asp:TextBox ID="txtSPP" runat="server" onkeypress="return IsNumberWithOneDecimal(this,event);" AutoComplete="off" Text="0" CssClass="col-md-12 col-xs-12 col-lg-12" Style="border: 2; height: 34px; padding: 6px 12px; font-size: 14px;"></asp:TextBox>
                                                        </div>
                                                        <br />
                                                        <label id="Label10" runat="server" class="col-sm-7 control-label col-lg-7">Referral Bonus <span id="spprofit" runat="server"></span></label>
                                                        <div class="form-group col-sm-5">
                                                            <asp:Label ID="lblRefProfitBonusPercent" runat="server"></asp:Label>
                                                            <asp:TextBox ID="txtRefProfitBonusPercent" runat="server" onkeypress="return IsNumberWithOneDecimal(this,event);" AutoComplete="off" Text="0" CssClass="col-md-12 col-xs-12 col-lg-12" Style="border: 2; height: 34px; padding: 6px 12px; font-size: 14px;"></asp:TextBox>
                                                        </div>
                                                        <br />
                                                        <label id="Label17" runat="server" class="col-sm-7 control-label col-lg-7">Network Plus Bonus (Level 1)</label>
                                                        <div class="form-group col-sm-5">
                                                            <asp:TextBox ID="txtTeamKPI2" runat="server" onkeypress="return IsNumberWithOneDecimal(this,event);" Text="0" AutoComplete="off" CssClass="col-md-12 col-xs-12 col-lg-12" Style="border: 2; height: 34px; padding: 6px 12px; font-size: 14px;"></asp:TextBox>
                                                        </div>
                                                        <br />
                                                        <label id="Label13" runat="server" class="col-sm-7 control-label col-lg-7">Network Plus Bonus (Level 2)</label>
                                                        <div class="form-group col-sm-5">
                                                            <asp:TextBox ID="txtTeamKPI" runat="server" onkeypress="return IsNumberWithOneDecimal(this,event);" Text="0" AutoComplete="off" CssClass="col-md-12 col-xs-12 col-lg-12" Style="border: 2; height: 34px; padding: 6px 12px; font-size: 14px;"></asp:TextBox>
                                                        </div>
                                                        <br />
                                                        <label id="Label3" runat="server" class="col-sm-7 control-label col-lg-7">Merchant Brand Dev. Incentive (VVIP Only)</label>
                                                        <div class="form-group col-sm-5">
                                                            <asp:TextBox ID="txtMBD" runat="server" onkeypress="return IsNumberWithOneDecimal(this,event);" Text="0" AutoComplete="off" CssClass="col-md-12 col-xs-12 col-lg-12" Style="border: 2; height: 34px; padding: 6px 12px; font-size: 14px;"></asp:TextBox>
                                                        </div>
                                                        <br />
                                                        <label id="Label14" runat="server" class="col-sm-7 control-label col-lg-7">Month End Bonus</label>
                                                        <div class="form-group col-sm-5">
                                                            <asp:TextBox ID="txtMEB" runat="server" onkeypress="return isNumberKey(this,event)" Text="0" AutoComplete="off" CssClass="col-md-12 col-xs-12 col-lg-12" Style="border: 2; height: 34px; padding: 6px 12px; font-size: 14px;"></asp:TextBox>
                                                        </div>


                                                    </div>
                                                </div>

                                            </div>
                                        </div>
                                        <%cLine++;%>
                                    </div>
                                    
                                </div>
                                <div class="modal-footer">
                                    <asp:Button ID="btnaddtopromotion" runat="server" Text="Add Item" OnClick="btnaddtopromotion_Click" CssClass="btn btn-primary" />
                                    <asp:Button ID="btnupdatepromotion" runat="server" Visible="false" Text="Update Item" OnClick="btnupdatepromotion_Click" CssClass="btn btn-primary" />
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>


                    </div>
                </div>

            </div>
            <!--end modal-->


        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

