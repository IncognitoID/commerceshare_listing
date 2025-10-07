<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ContentApproval.aspx.cs" Inherits="BItemListing" MaintainScrollPositionOnPostback="true" ClientIDMode="AutoID" %>

<%@ Register Assembly="msgBox" Namespace="BunnyBear" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <meta charset="utf-8" />
    
    <script type="text/javascript" src="scripts/sweetalert2.all.min.js"></script>
    <style type="text/css">
        .clear {
            clear: both;
        }
    </style>
    <style type="text/css">
        .containerpromo {
            text-align:right;
            padding-right:10px;
        }

        .btnexport {
            background-color: #4CAF50; /* Green */
            border: none;
            color: white;
            text-align: center;
            text-decoration: none;
            display: inline-block;
            font-size: 14px;
            margin: 4px 2px;
            cursor: pointer;
            padding: 5px 20px;
            border-radius: 2px;
        }

        .btnpromo {
            position: relative;
            text-decoration: none;
            display: inline-block;
            background-color: #09a879;
            text-align: center;
            padding: 3px 10px;
            margin-bottom: 3px;
            border-radius: 4px;
            border-color: #46b8da;
        }

            .btnpromo:hover {
                background: #1B7ADD;
                 text-decoration: none;
            }

            .btnpromo .badge {
                position: absolute;
                top: -10px;
                right: -10px;
                padding: 4px 8px;
                border-radius: 50%;
                background: Red;
                color: white;
            }

        .tablepadding tr td
        {
            padding:10px;
        }

        .tablepadding tr th
        {
            padding:10px;
        }

        input::-webkit-outer-spin-button,
        input::-webkit-inner-spin-button {
            -webkit-appearance: none;
            margin: 0;
        }

        /* Firefox */
        input[type=number] {
            -moz-appearance: textfield;
        }
        #overlay {
            position: fixed;
            z-index: 98;
            top: 0px;
            left: 0px;
            background-color: #ffffff;
            width: 100%;
            height: 100%;
            filter: Alpha(Opacity=80);
            opacity: 0.80;
            -moz-opacity: 0.80;
        }

        #theprogress {
            background-color: transparent;
            width: 110px;
            height: 24px;
            text-align: center;
            filter: Alpha(Opacity=100);
            opacity: 1;
            -moz-opacity: 1;
        }

        #modalprogress {
            position: absolute;
            top: 50%;
            left: 50%;
            margin: -11px 0 0 -44px;
            color: White;
        }

        body > #modalprogress {
            position: fixed;
        }

        .btnsave {
            background-color: #09a879;
            color: white;
            text-align: center;
            padding: 0px 25px;
            border-radius: 4px;
            border-color: #46b8da;
        }

        @media (min-width: 992px) {
            .col-md-6 {
                width: 40%;
                float: left;
            }

            .col-md-3 {
                width: 20%;
            }
        }

        .btnback {
    background-color: cornflowerblue;
    border: none;
    color: white;
    text-align: center;
    text-decoration: none;
    display: inline-block;
    font-size: 14px;
    margin: 4px 2px;
    cursor: pointer;
    padding: 5px 20px;
    border-radius: 2px;

}

        .ghost-button-thick-border1 {
            display: inline-block;
            width: 100px;
            font-weight: bold;
            padding: 1px;
            color: grey;
            border: 2px solid grey;
            text-align: center;
            outline: none;
            text-decoration: none;
            background-color: White;
            float: right;
            margin-top: 0px;
        }
       
        .gridview{
            text-align:left;
        }
        .auto-style1 {
            height: 42px;
        }
    </style>

    <script type="text/javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode == 46) {
                return true;
            }
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
        function openModal() {
            $('[id*=PromoModal]').modal('show');
        }


        function RowClicked(this) {
            $(this).trigger('click');
        }

    </script>
    <script type = "text/javascript">
        function Confirm() {
            //Remove previous element
            var oldInput = document.getElementById('myInput');
            if (oldInput !== null) form.removeChild(oldInput);

            var confirm_value = document.createElement("INPUT");
            var tabName = $("[id*=costpercent]").val()
            confirm_value.setAttribute('id', 'myInput');
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Are you sure to markup " + tabName + "?")) {
                confirm_value.value = "";
                confirm_value.value = "Yes";
            } else {
                confirm_value.value = "";
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }

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


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel id="update1" runat="server" UpdateMode="Conditional"><ContentTemplate>

        <cc1:msgBox ID="MsgBox1" runat="server" Style="color: Red"></cc1:msgBox>
        <asp:Label ID="MF_Con" runat="server" Visible="false"></asp:Label>
        <asp:UpdatePanel ID="UpdatePanel15" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <table style="width: 100%" class="filtertb">
                    <tr>
                        <td colspan="3">
                            <asp:TextBox ID="txt_Search" runat="server" autocomplete="off" MaxLength="100" placeholder="Filter by item code / desc" OnTextChanged="txt_Search_TextChanged" AutoPostBack="true" CssClass="col-md-12 col-xs-12 txt_Search" Style="height: 40px; border: 1px solid #ccc; font-weight: bold; padding: 8px;"></asp:TextBox>
                        </td>
                    </tr>

                    <tr>
                        <td style="padding-right:15px;">
                            <label>Status</label>
                            <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged" CssClass="col-md-12 col-xs-12" Style="height: 34px; border: 1px solid #ccc;">
                                <asp:ListItem Text="All Approval" Value=""></asp:ListItem>
                                <asp:ListItem Text="New" Value="New"></asp:ListItem>
                                <asp:ListItem Text="Approved" Value="Approved"></asp:ListItem>
                                <asp:ListItem Text="Rejected" Value="Rejected"></asp:ListItem>
                            </asp:DropDownList>
                        </td>

                        <td style="padding-right:15px;">
                            <label>Publish</label>
                            <asp:DropDownList ID="ddlPublish" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPublish_SelectedIndexChanged" CssClass="col-md-12 col-xs-12" Style="height: 34px; border: 1px solid #ccc;">
                                <asp:ListItem Text="All Publish" Value=""></asp:ListItem>
                                <asp:ListItem Text="Publish" Value="Yes"></asp:ListItem>
                                <asp:ListItem Text="Unpublish" Value="No"></asp:ListItem>
                            </asp:DropDownList>
                        </td>

                        <td style="padding-right:15px;">
                            <label>Stock Status</label>
                            <asp:DropDownList ID="ddlSoldStatus" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSoldStatus_SelectedIndexChanged" CssClass="col-md-12 col-xs-12" Style="height: 34px; border: 1px solid #ccc;">
                                <asp:ListItem Text="All Stock Status" Value=""></asp:ListItem>
                                <asp:ListItem Text="Sold Out" Value="Yes"></asp:ListItem>
                                <asp:ListItem Text="Instock" Value="No"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>

                    <tr>
                        <td style="padding-right:15px;">
                            <label>Department</label>
                            <asp:DropDownList ID="ddlDepartment" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlDepartment_SelectedIndexChanged" CssClass="col-md-12 col-xs-12" Style="height: 34px; border: 1px solid #ccc;">
                                <asp:ListItem Text="All Department" Value=""></asp:ListItem>
                            </asp:DropDownList>
                        </td>

                        <td style="padding-right:15px;">
                            <label>Category</label>
                            <asp:DropDownList ID="ddlCategory" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" CssClass="col-md-12 col-xs-12" Style="height: 34px; border: 1px solid #ccc;">
                                <asp:ListItem Text="All Category" Value=""></asp:ListItem>
                            </asp:DropDownList>
                        </td>

                        <td style="padding-right:15px;">
                            <label>Cost Rate</label>
                            <asp:DropDownList ID="ddlcostrate" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlcostrate_SelectedIndexChanged" CssClass="col-md-12 col-xs-12" Style="height: 34px; border: 1px solid #ccc;">
                                <asp:ListItem Text="All Cost" Value=""></asp:ListItem>
                                <asp:ListItem Text="Zero Cost" Value="Zero"></asp:ListItem>
                                <asp:ListItem Text="Under Cost" Value="Under"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>

                    <tr>
                        <td class="auto-style1"></td>
                        <td colspan="2" style="text-align: right; padding-top:10px; vertical-align: bottom;" class="auto-style1">
                            <asp:LinkButton ID="btnnew" runat="server" CssClass="ghost-button-thick-border1" Text="New" OnClick="btn_new_Click"></asp:LinkButton>
                        </td>  
                    </tr>
                    <tr>
                        <td></td>
                        <td colspan="2" style="text-align: right; padding-top:10px; padding-bottom:10px; vertical-align: bottom; class="auto-style1">
                            <asp:Button ID="btnExportExcel" runat="server" CssClass="btn btn-success" Text="Export" OnClick="btnExportExcel_Click" Height="30px" Width="96px"/>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>

        <%--<asp:Label ID="LC" runat="server" Visible="false"></asp:Label>
    <asp:Label ID="Item" runat="server" Visible="false"></asp:Label>
    <asp:Label ID="Item_ID" runat="server" Visible="false"></asp:Label>--%>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Repeater ID="rpt_Item" runat="server" OnItemDataBound="CardItem_DataBound" OnItemCommand="rpt_Item_ItemCommand">
                    <ItemTemplate>
                        <div class="col-sm-12 col-md-4 col-lg-3">
                            <div id="line" runat="server" class="white-panel itemColumn" style="border: 2px solid black; padding-bottom: 5px;">
                                <%-- <div id="Div1" class="white-header pn itemHeight" runat="server"  style="background-color:#fff; margin-bottom: 5px;" >--%>
                                <div class="col-xs-12" style="margin-bottom: 5px;">
                                    <div class="col-xs-5" id="divAR" runat="server">
                                        <asp:Label ID="card_approveandreject" runat="server" Font-Size="12px" Font-Bold="true" ForeColor="black" Style="padding: 5px 0px 0px 5px; position: absolute;"></asp:Label>
                                    </div>
                                    <div class="col-xs-6" id="divYN" runat="server">
                                        <asp:Label ID="Card_Publish" runat="server" Font-Size="12px" Font-Bold="true" ForeColor="Black" Style="padding: 5px 0px 0px 5px; position: absolute;"></asp:Label>
                                    </div>
                                <div class="col-xs-1" id="div1" runat="server">
                                     <div class="dropdown">
                                        <p class="dropdown-toggle" data-toggle="dropdown">
                                            <a class="glyphicon glyphicon-option-vertical" style="font-size:20pt;pointer-events:auto;color:black; text-decoration:none; cursor:pointer"></a>
                                        </p>
                                        <ul class="dropdown-menu pull-right" style="z-index:99999">
                                            <li class="list" style="padding:5px;"><asp:LinkButton ID="lnkedit" style="display:inline-block;padding:6px 6px;width:100%;" Text="" runat="server" CommandArgument='<%# Eval("Item_Code") %>' CommandName="Edit"><span class="fa fa-edit" style="font-size:15pt;padding:6px 6px;"></span>Edit Item</asp:LinkButton></li>
                                            <li class="list" style="padding:5px;"><asp:LinkButton ID="lnkmodifier" style="display:inline-block;padding:6px 6px;width:100%;" Text="" runat="server" CommandArgument='<%# Eval("Item_Code") %>' CommandName="AssignModifier"><span class="fa fa-bars" style="font-size:15pt;padding:6px 6px;"></span>Assign Modifier</asp:LinkButton></li>
                                            <li class="list" style="padding:5px;"><asp:LinkButton ID="lnkAssignPrint" style="display:inline-block;padding:6px 6px;width:100%;" Text="" runat="server" CommandArgument='<%# Eval("Item_Code") %>' CommandName="AssignPrinter"><span class="fa fa-print" style="font-size:15pt;padding:6px 6px;"></span>Assign Printer</asp:LinkButton></li>
                                            <li class="list" style="padding:5px;"><asp:LinkButton  style="display:inline-block;padding:6px 6px;width:100%;" ID="lnkdel" Text="Delete Item" runat="server" CommandName="Delete" CommandArgument='<%# Eval("Item_Code") %>' OnClientClick="return confirm('Do you want to delete this item?');"><span class="fa fa-trash-o" style="font-size:15pt;padding:6px 6px;"></span>Delete</asp:LinkButton></li>
                                        </ul>
                                        </div>
                                    </div>
                            </div>
                            <div class="col-xs-12 imgCenter" style="margin-bottom: 5px; text-align: center;">
                                <asp:Image runat="server" ID="btnedit" Text="edit" class="imageMiddle" CssClass="imgSize" Style="border-radius: 5px 5px 5px 5px;" />
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12" style="margin-bottom: 5px; height: 180px;">
                                 <div id="Tabs" role="tabpanel">
                                            <!-- Nav tabs -->
                                            <ul class="nav nav-tabs" role="tablist" id="myTab<%=cLine%>">
                                                <li class="active"><a href="#member<%=cLine%>" style="line-height: 0.428571 !important;" aria-controls="member" role="tab" data-toggle="tab">Member</a></li>
                                                <li ><a href="#stockist<%=cLine%>" aria-controls="stockist" role="tab" data-toggle="tab" style="line-height: 0.428571 !important;">Stockist</a></li>
                                                
                                                        <div class="containerpromo">

                                                            <asp:LinkButton ID="btnpromo" runat="server" ForeColor="white" OnClick="btnpromo_Click" CssClass="btnpromo">
                                                        <span><b>Promotion</b></span>
                                                        <span class="badge" id="spcount" runat="server"></span>
                                                            </asp:LinkButton>
                                                        </div>
<%--                                                <asp:Button ID="btnpromo" runat="server" Text="Promotion" CssClass="btnsave" OnClick="btnpromo_Click" style="float:right;margin-right:10px" />--%>
                                            </ul>
                                            <!-- Tab panes -->
                                            <div class="tab-content">
                                                <!-- member tab -->
                                                <div role="tabpanel" class="tab-pane fade in active" id="member<%=cLine%>">
                                <div class="col-sm-12 col-xs-12 col-md-12 col-lg-12 goleft" style="color: Black; padding-left: 5px; padding-right: 5px;">
                                    <div style="text-align: center;">
                                        <p style="margin-bottom: 3px !important;">
                                            <asp:Label ID="Label3" runat="server" Text="Price /" Font-Size="12px" ForeColor="Black"></asp:Label>
                                            <asp:Label ID="Label12" runat="server" ForeColor="Black" Font-Bold="true" Font-Size="12px" Text='<%# Eval("UOM") %>'></asp:Label>
                                            <asp:Label ID="Label13" runat="server" Text="(RM)" Font-Size="12px" ForeColor="Black"></asp:Label>
                                        </p>
                                    </div>
                                    <div>
                                        <div class="col-xs-4 col-sm-4 col-md-4 col-lg-4" >
                                           
                                        </div>
                                        <div class="col-xs-4 col-sm-4 col-md-4 col-lg-4" style="text-align: center; border: 1px solid lightgrey;">
                                            <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                              <asp:Label ID="Label21" runat="server" Text="Sell Price" Font-Size="12px" Font-Bold="true" ForeColor="Black"></asp:Label>
                                            </p>
                                        </div>
                                         <div class="col-xs-4 col-sm-4 col-md-4 col-lg-4" style="text-align: center; border: 1px solid lightgrey;">
                                            <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                <asp:Label ID="Label22" runat="server" Text="Cashback" Font-Size="12px" Font-Bold="true" ForeColor="Black"></asp:Label>
                                            </p>
                                        </div>
                                    </div>
                                    <div>
                                        <div class="col-xs-4 col-sm-4 col-md-4 col-lg-4" style="border: 1px solid lightgrey; min-height: 35px;">
                                            <p style="margin-bottom: 3px !important; padding-left: 5px; padding-top: 6px;">
                                                <asp:Label ID="Label2" runat="server" Font-Size="12px" Text="Member : " ForeColor="Black"></asp:Label>

                                            </p>
                                        </div>
                                        <div class="col-xs-4 col-sm-4 col-md-4 col-lg-4" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                            <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                <asp:TextBox Style="border: none; text-align: right;" inputmode="decimal" ID="lblMPrice" onkeypress="return isNumberKey(event)" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="90px" Height="33px"></asp:TextBox>
                                            </p>
                                        </div>
                                         <div class="col-xs-4 col-sm-4 col-md-4 col-lg-4" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                            <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                <asp:TextBox Enabled="false" Style="border: none; text-align: right;" ID="txtMcashback" inputmode="decimal" onkeypress="return isNumberKey(event)" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="gray" Width="90px" Height="33px"></asp:TextBox></p>
                                        </div>
                                    </div>
                                    <div>
                                        <div class="col-xs-4 col-sm-4 col-md-4 col-lg-4" style="border: 1px solid lightgrey; min-height: 35px;">
                                            <p style="margin-bottom: 3px !important; padding-left: 5px; padding-top: 6px;">
                                                <asp:Label ID="Label4" runat="server" Font-Size="12px" Text="VIP : " ForeColor="Black"></asp:Label>

                                            </p>
                                        </div>
                                        <div class="col-xs-4 col-sm-4 col-md-4 col-lg-4" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                            <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                <asp:TextBox Style="border: none; text-align: right;" ID="lblVPrice" inputmode="decimal" onkeypress="return isNumberKey(event)" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="90px" Height="33px"></asp:TextBox></p>
                                        </div>
                                         <div class="col-xs-4 col-sm-4 col-md-4 col-lg-4" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                            <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                <asp:TextBox Enabled="false" Style="border: none; text-align: right;" ID="txtVcashback" inputmode="decimal" onkeypress="return isNumberKey(event)" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="gray" Width="90px" Height="33px"></asp:TextBox></p>
                                        </div>
                                    </div>
                                    <div>
                                        <div class="col-xs-4 col-sm-4 col-md-4 col-lg-4" style="border: 1px solid lightgrey; min-height: 35px;">
                                            <p style="margin-bottom: 3px !important; padding-left: 5px; padding-top: 6px;">
                                                <asp:Label ID="Label9" runat="server" Font-Size="12px" Text="VVIP : " ForeColor="Black"></asp:Label>

                                            </p>
                                        </div>
                                        <div class="col-xs-4 col-sm-4 col-md-4 col-lg-4" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                            <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                <asp:TextBox Style="border: none; text-align: right;" ID="lblVVIPPrice" inputmode="decimal" onkeypress="return isNumberKey(event)" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="90px" Height="33px"></asp:TextBox>
                                            </p>
                                        </div>
                                        <div class="col-xs-4 col-sm-4 col-md-4 col-lg-4" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                            <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                <asp:TextBox Style="border: none; text-align: right;" ID="txtVVIPcashback" inputmode="decimal" onkeypress="return isNumberKey(event)" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="90px" Height="33px"></asp:TextBox>
                                            </p>
                                        </div>
                                    </div>
                                    <div class="col-xs-12" style="margin-bottom: 5px; padding-left: 10px; margin-top: 7px;">
                                        <div class="col-xs-8">
                                            <asp:Label ID="Label35" runat="server" Text="Allow In Member : " Font-Size="12px" Font-Bold="true" ForeColor="Black"></asp:Label>
                                            <asp:DropDownList ID="ddlmember" runat="server" Font-Size="12px" AppendDataBoundItems="true" AutoPostBack="true">
                                                <asp:ListItem Value="No" Text="No"></asp:ListItem>
                                                <asp:ListItem Value="Yes" Text="Yes"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                                </div>
                                                    

                                                <!-- stockist tab -->
                                                 <div role="tabpanel" class="tab-pane fade in" id="stockist<%=cLine%>">
                                                     <div class="col-sm-12 col-xs-12 col-md-12 col-lg-12 goleft" style="color: Black; padding-left: 5px; padding-right: 5px;">
                                    <div style="text-align: center;">
                                        <p style="margin-bottom: 3px !important;">
                                            <asp:Label ID="Label17" runat="server" Text="Price /" Font-Size="12px" ForeColor="Black"></asp:Label>
                                            <asp:Label ID="Label18" runat="server" ForeColor="Black" Font-Bold="true" Font-Size="12px" Text='<%# Eval("UOM") %>'></asp:Label>
                                            <asp:Label ID="Label19" runat="server" Text="(RM)" Font-Size="12px" ForeColor="Black"></asp:Label>
                                        </p>
                                    </div>
                                                          <div>
                                        <div class="col-xs-4 col-sm-4 col-md-4 col-lg-4" >
                                           
                                        </div>
                                        <div class="col-xs-4 col-sm-4 col-md-4 col-lg-4" style="text-align: center; border: 1px solid lightgrey;">
                                            <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                              <asp:Label ID="Label23" runat="server" Text="Sell Price" Font-Size="12px" Font-Bold="true" ForeColor="Black"></asp:Label>
                                            </p>
                                        </div>
                                         <div class="col-xs-4 col-sm-4 col-md-4 col-lg-4" style="text-align: center; border: 1px solid lightgrey;">
                                            <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                <asp:Label ID="Label24" runat="server" Text="Cashback" Font-Size="12px" Font-Bold="true" ForeColor="Black"></asp:Label>
                                            </p>
                                        </div>
                                    </div>
                                                     <div>
                                        <div class="col-xs-4 col-sm-4 col-md-4 col-lg-4" style="border: 1px solid lightgrey; min-height: 35px;">
                                            <p style="margin-bottom: 3px !important; padding-left: 5px; padding-top: 6px;">
                                                <asp:Label ID="Label16" runat="server" Font-Size="12px" Text="Stockist : " ForeColor="Black"></asp:Label>

                                            </p>
                                        </div>
                                        <div class="col-xs-4 col-sm-4 col-md-4 col-lg-4" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                            <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                <asp:TextBox Style="border: none; text-align: right;" ID="lblSPrice" inputmode="decimal" onkeypress="return isNumberKey(event)" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="90px" Height="33px"></asp:TextBox></p>
                                        </div>
                                        <div class="col-xs-4 col-sm-4 col-md-4 col-lg-4" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                            <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                <asp:TextBox Style="border: none; text-align: right;" ID="txtScashback" inputmode="decimal" onkeypress="return isNumberKey(event)" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="90px" Height="33px"></asp:TextBox></p>
                                        </div>
                                    </div>
                                                         <div>
                                        <div class="col-xs-4 col-sm-4 col-md-4 col-lg-4" style="border: 1px solid lightgrey; min-height: 35px;">
                                            <p style="margin-bottom: 3px !important; padding-left: 5px; padding-top: 6px;">
                                                <asp:Label ID="Label20" runat="server" Font-Size="12px" Text="Master Stockist : " ForeColor="Black"></asp:Label>

                                            </p>
                                        </div>
                                        <div class="col-xs-4 col-sm-4 col-md-4 col-lg-4" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                            <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                <asp:TextBox Style="border: none; text-align: right;" ID="lblMSPrice" inputmode="decimal" onkeypress="return isNumberKey(event)" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="90px" Height="33px"></asp:TextBox></p>
                                        </div>
                                        <div class="col-xs-4 col-sm-4 col-md-4 col-lg-4" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                            <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                <asp:TextBox Style="border: none; text-align: right;" ID="txtMScashback" inputmode="decimal" onkeypress="return isNumberKey(event)" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="90px" Height="33px"></asp:TextBox></p>
                                        </div>
                                    </div>
                                                         </div>
                                      <div class="col-xs-12" style="margin-bottom: 5px; padding-left: 10px; margin-top: 7px;">
                                          <div class="col-xs-8">
                                              <asp:Label ID="Label36" runat="server" Text="Allow In Stockist : " Font-Size="12px" Font-Bold="true" ForeColor="Black"></asp:Label>
                                              <asp:DropDownList ID="ddlallowstockist" runat="server" Font-Size="12px" AppendDataBoundItems="true" AutoPostBack="true">
                                                  <asp:ListItem Value="No" Text="No"></asp:ListItem>
                                                  <asp:ListItem Value="Yes" Text="Yes"></asp:ListItem>
                                              </asp:DropDownList>
                                          </div>
                                      </div>
                                                    </div>
                                               
                                                </div>
                                     </div>
                                <%cLine++;%>
                            </div>
                            <h6>
                                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12" style="margin-top: 20px; padding: 10px; border-top: 2px; border-radius: 4px;">                                        
                                    <div class="col-xs-12 goleft" style="color: Black; padding-left: 10px; height: 30px;">
                                            <asp:Label ID="Label25" runat="server" Font-Bold="true" Text="Cost : " ForeColor="Black"></asp:Label>
                                            <asp:TextBox ID="txtcost" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="95px" Height="23px"></asp:TextBox>
                                            <asp:HiddenField ID="costpercent" runat="server" />
                                            <asp:Label ID="lbloverride" runat="server" Font-Size="12px" Font-Bold="true" ForeColor="Black" Visible="false"></asp:Label>
                                            <asp:Button ID="btnmarkup" runat="server" CssClass="btnsave" Text="Markup" OnClick="btnmarkup_Click" OnClientClick="Confirm();" />&nbsp;<asp:Label id="markups" runat="server"></asp:Label>
                                        </div>
                                    <div class="col-xs-10 goleft" style="color: Black; padding-left: 10px; height: 30px;">
                                        <asp:Label ID="lblitemid" runat="server" Text='<%# Eval("Item_ID") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="Card_ShortDesc" ToolTip='<%# Eval("LongDesc") %>' runat="server" Font-Bold="true" ForeColor="Black" Text='<%# Eval("LongDesc") %>'></asp:Label>
                                        
                                    </div>
                                    <br />
                                    <div class="col-xs-12 goleft" style="color: Black; padding-left: 10px">
                                        <asp:Label ID="Label6" runat="server" Text="Item Code : " ForeColor="Black"></asp:Label>
                                        <asp:Label ID="Card_ItemCode" runat="server" Font-Bold="true" ForeColor="Black" Text='<%# Eval("Item_Code") %>'></asp:Label>
                                        <br />
                                    </div>
                                    <br />
                                    <div class="col-xs-12 goleft" style="color: Black; padding-left: 10px">
                                        <asp:Label ID="Label5" runat="server" Text="Barcode : " ForeColor="Black"></asp:Label>
                                        <asp:Label ID="Card_Barcode" runat="server" Font-Bold="true" ForeColor="Black" Text='<%# Eval("Barcode") %>'></asp:Label>
                                    </div>
                                    <br />
                                    <div class="col-xs-12 goleft" style="color: Black; padding-left: 10px">
                                        <asp:Label ID="Label10" runat="server" Text="Department : " ForeColor="Black"></asp:Label>
                                        <asp:Label ID="Card_Dept" runat="server" ForeColor="Black" Text='<%# Eval("Department_Description") %>'></asp:Label>
                                    </div>
                                    <br />
                                    <div class="col-xs-12 goleft" style="color: Black; padding-left: 10px">
                                        <asp:Label ID="Label1" runat="server" Text="Category : " ForeColor="Black"></asp:Label>
                                        <asp:Label ID="Card_Cat" runat="server" ForeColor="Black" Text='<%# Eval("Category_Description") %>'></asp:Label>
                                    </div>
                                    <br />
                                    <div class="col-xs-12 goleft" style="color: Black; padding-left: 10px">
                                        <asp:Label ID="Label109" runat="server" Text="Brand : " ForeColor="Black"></asp:Label>
                                        <asp:Label ID="Card_Brand" runat="server" ForeColor="Black" Text='<%# Eval("brand_desc") %>'></asp:Label>
                                    </div>
                                    <br />
                                    <div class="col-xs-7 goleft" style="color: Black; padding-left: 10px; margin-top: 3px;">
                                        <asp:Label ID="lbl_D" runat="server" Text="Updated : " ForeColor="Black" Font-Size="10px"></asp:Label>
                                        <asp:Label ID="Card_Date" runat="server" ForeColor="Black" Font-Size="10px"></asp:Label>
                                    </div>
                                    <div class="col-xs-5" style="color: Black; padding-right: 10px; text-align: right; margin-top: 3px;">
                                        <asp:Label ID="Label11" runat="server" Font-Bold="true" Text="Weight : " ForeColor="Black"></asp:Label>
                                        <asp:Label ID="Card_Weight" runat="server" Font-Bold="true" ForeColor="Black"></asp:Label>
                                    </div>

                                </div>
                            </h6>
                            <div class="col-xs-12" style="margin-bottom: 5px; padding-left: 10px; margin-top: 7px;">
                                <div class="col-xs-6">
                                    <asp:Label ID="Label7" runat="server" Text="Status : " Font-Size="12px" Font-Bold="true" ForeColor="Black"></asp:Label>
                                    <asp:DropDownList ID="ddlAnR" runat="server" Font-Size="12px" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddlAnR_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-xs-6">
                                    <asp:Label Visible="true" ID="Label8" runat="server" Font-Size="12px" Font-Bold="true" Text="" ForeColor="Black"></asp:Label>
                                     <%--OnSelectedIndexChanged="ddlYnN_SelectedIndexChanged"--%>
                                    <asp:DropDownList Visible="true" ID="ddlYnN" runat="server" Font-Size="12px" AutoPostBack="true">
                                        <asp:ListItem Text="Publish" Value="Yes"></asp:ListItem>
                                        <asp:ListItem Text="Unpublish" Value="No"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-xs-12" style="margin-bottom: 5px; padding-left: 10px; margin-top: 7px;">
                                <div class="col-xs-6">
                                    <asp:Label ID="Label32" runat="server" Text="FOC : " Font-Size="12px" Font-Bold="true" ForeColor="Black"></asp:Label>
                                    <asp:DropDownList ID="ddlfoc" runat="server" Font-Size="12px" AppendDataBoundItems="true" AutoPostBack="true">
                                        <asp:ListItem Value="No" Text="No"></asp:ListItem>
                                        <asp:ListItem Value="Yes" Text="Yes"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-xs-12" style="margin-bottom: 5px; padding-left: 10px; margin-top: 7px;">
                                <div class="col-xs-8">
                                    <asp:Label ID="Label33" runat="server" Text="Allow In QR Page : " Font-Size="12px" Font-Bold="true" ForeColor="Black"></asp:Label>
                                    <asp:DropDownList ID="ddlordertable" runat="server" Font-Size="12px" AppendDataBoundItems="true" AutoPostBack="true">
                                        <asp:ListItem Value="No" Text="No"></asp:ListItem>
                                        <asp:ListItem Value="Yes" Text="Yes"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-xs-12" style="margin-bottom: 5px; padding-left: 10px; margin-top: 7px;">
                                <div class="col-xs-8">
                                    <asp:Label ID="Label34" runat="server" Text="Allow In Waiter Page : " Font-Size="12px" Font-Bold="true" ForeColor="Black"></asp:Label>
                                    <asp:DropDownList ID="ddlwaiterorder" runat="server" Font-Size="12px" AppendDataBoundItems="true" AutoPostBack="true">
                                        <asp:ListItem Value="No" Text="No"></asp:ListItem>
                                        <asp:ListItem Value="Yes" Text="Yes"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-xs-12" style="border: 1px solid gray;"></div>
                            <div class="col-xs-12" style="margin-bottom: 5px; padding-left: 10px; margin-top: 7px;">
                                <div class="col-xs-6">
                                    <asp:Label ID="Label14" runat="server" Text="Stock Control : " Font-Size="12px" Font-Bold="true" ForeColor="Black"></asp:Label>
                                    <asp:DropDownList ID="ddlStockControl" runat="server" OnSelectedIndexChanged="ddlStockControl_SelectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-xs-6" style="color: Black; padding-right: 10px; text-align: right;">
                                    <asp:Label ID="Label15" runat="server" Text="Sold out : " Font-Size="12px" Font-Bold="true" ForeColor="Black"></asp:Label>
                                    <asp:DropDownList ID="ddlsoldstatus" runat="server" OnSelectedIndexChanged="ddlsoldstatus_SelectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div id="background" runat="server" style="padding-left: 10px;">
                                <table style="width: 100%; padding: 10px;">
                                    <tr style="padding-bottom: 5px;">
                                        <td style="width: 140px; padding:   4px;">
                                            <asp:RadioButton ClientIDMode="AutoID" Enabled="true" ID="QtyServePerday" runat="server" Font-Size="Small" GroupName="radiobutton" AutoPostBack="true" OnCheckedChanged="RadioButton1_CheckedChanged" /><i style="font-family: Arial, Helvetica, sans-serif; font-weight: bold;">  Qty Serve Per Day : </i></td>
                                        <td style="width: 50px">
                                            <asp:Label ID="lbllimit" runat="server" Visible="false" Text="No Limit"></asp:Label>
                                            <asp:TextBox Enabled="false" runat="server" type="text" MaxLength="7" onkeypress='return event.charCode >= 48 && event.charCode <= 57 || event.charCode == 45' min="0" ID="txtQtyServePerday" Style="width: 50px"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 130px; padding: 4px;">
                                            <asp:RadioButton ClientIDMode="AutoID" Enabled="true" ID="StockQty" Font-Size="Small" runat="server" GroupName="radiobutton" AutoPostBack="true" OnCheckedChanged="RadioButton2_CheckedChanged" /><i style="font-family: Arial, Helvetica, sans-serif; font-weight: bold;">  Stock Qty : </i></td>
                                        <td style="width: 50px">
                                            <asp:Label ID="lbllimit2" runat="server" Visible="false" Text="No Limit"></asp:Label>
                                            <asp:TextBox Enabled="false" runat="server" type="text" onkeypress='return event.charCode >= 48 && event.charCode <= 57 || event.charCode == 45' min="0" ID="txtStockQty" Style="width: 50px"></asp:TextBox></td>
                                        <td style="width: 70px; text-align: center;">
                                            <asp:Button ID="btnSave" CssClass="btnsave" runat="server" Text="Save" OnClick="btnSave_Click" /></td>

                                    </tr>
                                </table>
                            </div>
                            <%-- </div>--%>
                        </div>
                        <h4 style="margin: 0px 100px 15px 500px;"></h4>
                    </div>
                </ItemTemplate>
                <FooterTemplate>
                    <div id="dvNoRecords" runat="server" visible="false" style="padding: 20px 20px; text-align: center; color: Red;">
                        <code>No Items Record Found!</code>
                    </div>
                </FooterTemplate>
                <SeparatorTemplate>
                </SeparatorTemplate>
            </asp:Repeater>
            <br />
            <div class="col-lg-12 col-md-12 col-sm-12">
                <div>
                    <asp:DropDownList ID="ddlPager" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPager_SelectedIndexChanged" Style="float: left; display: inline-block; width: 110px; height: 42px; font-weight: bold; padding: 8px; text-align: center; outline: none; text-decoration: none;"></asp:DropDownList>
                    <br />
                    <div style="height: 30px"></div>
                    <asp:Label ID="lbl_Record" runat="server" Text="No. of record: "></asp:Label>
                    <asp:Label ID="lbl_Record2" runat="server"></asp:Label>
                </div>
            </div>

            <div class="modal fade" id="ModifierModal" role="dialog">
                <div class="modal-dialog">
                    <!-- Modal content-->
                    <div class="modal-content" id="assignmodifiermodal" style="margin-top: 0px;width:600px">
                        <div class="modal-header" >
                            <button type="button" class="close elementgray" style="color: black" data-dismiss="modal">
                                &times;</button>
                            <h4 class="modal-title" style="font-weight: bold;">Item Modifier Setting</h4>
                        </div>
                        <div class="modal-body">
                            <asp:UpdatePanel ID="updatepanel" runat="server">
                                <ContentTemplate>

                                    <asp:HiddenField ID="hditem_code" runat="server" />
                                    <div class="col-xs-12 col-lg-12 col-md-12" style="margin-bottom: 5px; text-align: center">
                                        <asp:Image runat="server" ID="Image1" Text="edit" class="imageMiddle" CssClass="imgSize" Style="border-radius: 5px 5px 5px 5px;" />
                                        <br />
                                        <asp:Label ID="lbldesc" runat="server" Style="font-weight: bold; font-size: medium"></asp:Label>
                                        <br />
                                        <asp:Label ID="lblbarcode" runat="server" Style="font-weight: bold; font-size: small"></asp:Label>

                                    </div>
                                    <div class="col-xs-12 col-lg-12 col-md-12" style="overflow-y: auto; height: 250px;margin-bottom:10px">


                                        <asp:GridView ID="grd_viewmodifer" AutoGenerateColumns="false" GridLines="Horizontal" OnRowDataBound="grd_viewmodifer_RowDataBound" CssClass="table table-bordered table-responsive table-hover " runat="server">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkmodifier" runat="server"  onclick="trackModifierSeq(this)" />
                                                        <asp:HiddenField ID="hdSequence" runat="server" Value="0" />

                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Modifier Name" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdmodifierID" runat="server" Value='<%# Bind("modifier_id") %>' />
                                                        <asp:Label ID="lblmodifiername" runat="server" Text='<%# Bind("modifier_grp_name") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Option" ItemStyle-Width="500px" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblmodifieroption" runat="server" Text='<%# Bind("modifieroption") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>


                                    </div>
                                    <br />
                                    <asp:Button ID="btnupdatemodifier" OnClick="btnupdatemodifier_Click" runat="server" Text="Update" Style="width: 100%" CssClass="btn btn-default" />

                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnupdatemodifier" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>

            <div class="modal fade" id="PrinterModal" role="dialog">
                <div class="modal-dialog">
                    <!-- Modal content-->
                    <div class="modal-content" id="assignprintermodal" style="margin-top: 0px;width:600px">
                        <div class="modal-header" >
                            <button type="button" class="close elementgray" style="color: black" data-dismiss="modal">
                                &times;</button>
                            <h4 class="modal-title" style="font-weight: bold;">Item Printer Setting</h4>
                        </div>
                        <div class="modal-body">
                            <asp:UpdatePanel ID="updatepanel2" runat="server">
                                <ContentTemplate>

                                    <asp:HiddenField ID="HiddenField1" runat="server" />
                                    <div class="col-xs-12 col-lg-12 col-md-12" style="margin-bottom: 5px; text-align: center">
                                        <asp:Image runat="server" ID="Image2" Text="edit" class="imageMiddle" CssClass="imgSize" Style="border-radius: 5px 5px 5px 5px;" />
                                        <br />
                                        <asp:Label ID="lblitemdesc" runat="server" Style="font-weight: bold; font-size: medium"></asp:Label>
                                        <br />
                                        <asp:Label ID="lblitembarcode" runat="server" Style="font-weight: bold; font-size: small"></asp:Label>

                                    </div>
                                    <div class="col-xs-12 col-lg-12 col-md-12" style="overflow-y: auto; height: 250px;margin-bottom:10px">


                                        <asp:GridView ID="grd_viewprinter" AutoGenerateColumns="false" GridLines="Horizontal" OnRowDataBound="grd_viewprinter_RowDataBound" CssClass="table table-bordered table-responsive table-hover " runat="server">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkprinter" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Printer Name" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdprinterID" runat="server" Value='<%# Bind("ids") %>' />
                                                        <asp:Label ID="lblprintername" runat="server" Text='<%# Bind("print_name") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="IP Address" ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblipaddress" runat="server" Text='<%# Bind("IP_Address") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>

                                    </div>
                                    <br />
                                    <asp:Button ID="btnupdateprinter" OnClick="btnupdateprinter_Click" runat="server" Text="Update" Style="width: 100%" CssClass="btn btn-default" />

                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnupdateprinter" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>

            <div class="modal" id="PromoModal" role="dialog" aria-hidden="true">
                <div class="modal-dialog modal-xl"  style="width:100%;height: 100% !important; margin: auto !important;">

                    <!-- Modal content-->
                    <div class="modal-content"  style="width: auto;margin:0px !important">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>

                            <h4 class="modal-title">Item Promotion</h4>

                        </div>
                             <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <div class="modal-body" style="display: inline-block; width: 100%">
                                     <div class="col-xs-12 col-lg-12 col-md-12" style="margin-bottom: 5px;">
                                          <label id="Label26" runat="server" class="col-sm-10 control-label col-lg-4">Campaign</label>
                                            <div class="form-group col-sm-7">
                                                <asp:DropDownList ID="ddlcampaign" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlcampaign_SelectedIndexChanged" CssClass="col-md-12 col-xs-12 col-lg-12" Style="border: 2; height: 34px; padding: 6px 12px; font-size: 14px;"></asp:DropDownList>
                                            </div>
                                          <label id="Label27" runat="server" class="col-sm-10 control-label col-lg-4">Campaign Period</label>
                                         <div class="form-group col-sm-3">
                                             <asp:Label ID="txtstartdate" TextMode="DateTimeLocal" runat="server" AutoComplete="off" CssClass="col-md-12 col-xs-12 col-lg-12" Style="border: 2; height: 34px; padding: 6px 12px; font-size: 14px;"></asp:Label>
                                         </div>
                                         <div class="form-group col-sm-1" style="text-align: center; vertical-align: middle; height: 34px; padding: 6px 12px; font-size: 14px;">
                                             <span class="glyphicon glyphicon-minus"></span>
                                         </div>
                                         <div class="form-group col-sm-3">
                                             <asp:Label ID="txtenddate" TextMode="DateTimeLocal" runat="server" AutoComplete="off" CssClass="col-md-12 col-xs-12 col-lg-12" Style="border: 2; height: 34px; padding: 6px 12px; font-size: 14px;"></asp:Label>
                                         </div>
                                    </div>
                                    <div class="col-xs-3 col-lg-3 col-md-3" style="margin-bottom: 5px;">
                                        <asp:Image runat="server" ID="btnimage" Text="edit" class="imageMiddle" CssClass="imgSize" Style="border-radius: 5px 5px 5px 5px;" />
                                        <br />
                                        <asp:Label ID="lblpitem_code" runat="server" Style="font-weight: bold; font-size: medium" Visible="false"></asp:Label>
                                        <asp:Label ID="lblpitemname" runat="server" Style="font-weight: bold; font-size: medium"></asp:Label>
                                        <br />
                                        <asp:Label ID="lblpbarcode" runat="server" Style="font-weight: bold; font-size: small"></asp:Label>

                                    </div>


                                    <div class="col-sm-9 col-xs-9 col-md-9 col-lg-9 goleft" style="color: Black; padding-left: 5px; padding-right: 5px;">
                                        <div class="col-xs-12 col-lg-12 col-md-12">
                                            <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2">
                                            </div>
                                            <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2" style="text-align: center; border: 1px solid lightgrey;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                    <asp:Label ID="Label7" runat="server" Text="Sell Price" Font-Size="12px" Font-Bold="true" ForeColor="Black"></asp:Label>
                                                </p>
                                            </div>
                                             <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2" style="text-align: center; border: 1px solid lightgrey;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                    <asp:Label ID="Label30" runat="server" Text="Discount Type" Font-Size="12px" Font-Bold="true" ForeColor="Black"></asp:Label>
                                                </p>
                                            </div>
                                             <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2" style="text-align: center; border: 1px solid lightgrey;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                    <asp:Label ID="Label31" runat="server" Text="Discount" Font-Size="12px" Font-Bold="true" ForeColor="Black"></asp:Label>
                                                </p>
                                            </div>
                                            <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2" style="text-align: center; border: 1px solid lightgrey;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                    <asp:Label ID="Label21" runat="server" Text="Promotion Price" Font-Size="12px" Font-Bold="true" ForeColor="Black"></asp:Label>
                                                </p>
                                            </div>
                                            <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2" style="text-align: center; border: 1px solid lightgrey;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                    <asp:Label ID="Label22" runat="server" Text="Cashback" Font-Size="12px" Font-Bold="true" ForeColor="Black"></asp:Label>
                                                </p>
                                            </div>
                                        </div>
                                        <div class="col-xs-12 col-lg-12 col-md-12">
                                            <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2" style="border: 1px solid lightgrey; min-height: 35px;">
                                                <p style="margin-bottom: 3px !important; padding-left: 5px; padding-top: 6px;">
                                                    <asp:Label ID="Label2" runat="server" Font-Size="12px" Text="Member : " ForeColor="Black"></asp:Label>

                                                </p>
                                            </div>
                                            <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                    <asp:Label Style="border: none; text-align: right;" inputmode="decimal" ID="lblpMprice" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="100%" Height="33px"></asp:Label>
                                                </p>
                                            </div>
                                             <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                    <asp:Label Style="border: none; text-align: right;" inputmode="decimal" ID="lblMdiscType" runat="server" Font-Size="12px" Font-Bold="true" ForeColor="Black" Width="100%" Height="33px"></asp:Label>
                                                </p>
                                            </div>
                                             <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                    <asp:Label Style="border: none; text-align: right;" inputmode="decimal" ID="lblMdiscAmt" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="100%" Height="33px"></asp:Label>
                                                </p>
                                            </div>
                                            <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                    <asp:Label Style="border: thin; text-align: right;" inputmode="decimal" ID="lblpMpromoPrice" onkeypress="return IsNumberWithOneDecimal(this,event);" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="60%" Height="33px"></asp:Label>
                                                </p>
                                            </div>
                                            <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                    <asp:Label Style="border: none; text-align: right;" ID="txtpMpromocashback" Enabled="false" inputmode="decimal" onkeypress="return IsNumberWithOneDecimal(this,event);" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="gray" Width="100%" Height="33px"></asp:Label>
                                                </p>
                                            </div>
                                        </div>
                                        <div class="col-xs-12 col-lg-12 col-md-12">
                                            <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2" style="border: 1px solid lightgrey; min-height: 35px;">
                                                <p style="margin-bottom: 3px !important; padding-left: 5px; padding-top: 6px;">
                                                    <asp:Label ID="Label4" runat="server" Font-Size="12px" Text="VIP : " ForeColor="Black"></asp:Label>

                                                </p>
                                            </div>
                                            <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                    <asp:Label Style="border: none; text-align: right;" inputmode="decimal" ID="lblpVPrice" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="100%" Height="33px"></asp:Label>
                                                </p>
                                            </div>
                                             <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                    <asp:Label Style="border: none; text-align: right;" inputmode="decimal" ID="lblVDiscType" runat="server" Font-Size="12px" Font-Bold="true" ForeColor="Black" Width="100%" Height="33px"></asp:Label>
                                                </p>
                                            </div>
                                             <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                    <asp:Label Style="border: none; text-align: right;" inputmode="decimal" ID="lblVDiscAmt" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="100%" Height="33px"></asp:Label>
                                                </p>
                                            </div>
                                            <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                    <asp:Label Style="border: thin; text-align: right;" ID="lblpVpromoPrice" inputmode="decimal" onkeypress="return IsNumberWithOneDecimal(this,event);" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="60%" Height="33px"></asp:Label>
                                                </p>
                                            </div>
                                            <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                    <asp:Label Style="border: none; text-align: right;" ID="txtpVpromocashback" Enabled="false" inputmode="decimal" onkeypress="return IsNumberWithOneDecimal(this,event);" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="gray" Width="100%" Height="33px"></asp:Label>
                                                </p>
                                            </div>
                                        </div>
                                        <div class="col-xs-12 col-lg-12 col-md-12">
                                            <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2" style="border: 1px solid lightgrey; min-height: 35px;">
                                                <p style="margin-bottom: 3px !important; padding-left: 5px; padding-top: 6px;">
                                                    <asp:Label ID="Label9" runat="server" Font-Size="12px" Text="VVIP : " ForeColor="Black"></asp:Label>

                                                </p>
                                            </div>
                                            <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                    <asp:Label Style="border: none; text-align: right;" inputmode="decimal" ID="lblpVVIPPrice" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="100%" Height="33px"></asp:Label>
                                                </p>
                                            </div>
                                            <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                    <asp:Label Style="border: none; text-align: right;" inputmode="decimal" ID="lblVVIPDiscType" runat="server" Font-Size="12px" Font-Bold="true" ForeColor="Black" Width="100%" Height="33px"></asp:Label>
                                                </p>
                                            </div>
                                             <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                    <asp:Label Style="border: none; text-align: right;" inputmode="decimal" ID="lblVVIPDiscAmt" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="100%" Height="33px"></asp:Label>
                                                </p>
                                            </div>
                                            <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                    <asp:Label Style="border: thin; text-align: right;" ID="lblpVVIPpromoPrice" inputmode="decimal" onkeypress="return IsNumberWithOneDecimal(this,event);" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="60%" Height="33px"></asp:Label>
                                                </p>
                                            </div>
                                            <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                    <asp:Label Style="border: none; text-align: right;" ID="txtpVVIPpromocashback" inputmode="decimal" onkeypress="return IsNumberWithOneDecimal(this,event);" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="100%" Height="33px"></asp:Label>
                                                </p>
                                            </div>
                                        </div>
                                        <div class="col-xs-12 col-lg-12 col-md-12">
                                            <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2" style="border: 1px solid lightgrey; min-height: 35px;">
                                                <p style="margin-bottom: 3px !important; padding-left: 5px; padding-top: 6px;">
                                                    <asp:Label ID="Label16" runat="server" Font-Size="12px" Text="Stockist : " ForeColor="Black"></asp:Label>

                                                </p>
                                            </div>
                                            <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                    <asp:Label Style="border: none; text-align: right;" inputmode="decimal" ID="lblpSPrice" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="100%" Height="33px"></asp:Label>
                                                </p>
                                            </div>
                                            <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                    <asp:Label Style="border: none; text-align: right;" inputmode="decimal" ID="lblSDiscType" runat="server" Font-Size="12px" Font-Bold="true" ForeColor="Black" Width="100%" Height="33px"></asp:Label>
                                                </p>
                                            </div>
                                             <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                    <asp:Label Style="border: none; text-align: right;" inputmode="decimal" ID="lblSDiscAmt" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="100%" Height="33px"></asp:Label>
                                                </p>
                                            </div>
                                            <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                    <asp:Label Style="border: thin; text-align: right;" ID="lblpSpromoPrice" inputmode="decimal" onkeypress="return IsNumberWithOneDecimal(this,event);" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="60%" Height="33px"></asp:Label>
                                                </p>
                                            </div>
                                            <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                    <asp:Label Style="border: none; text-align: right;" ID="txtpSpromocashback" inputmode="decimal" onkeypress="return IsNumberWithOneDecimal(this,event);" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="100%" Height="33px"></asp:Label>
                                                </p>
                                            </div>
                                        </div>
                                        <div class="col-xs-12 col-lg-12 col-md-12">
                                            <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2" style="border: 1px solid lightgrey; min-height: 35px;">
                                                <p style="margin-bottom: 3px !important; padding-left: 5px; padding-top: 6px;">
                                                    <asp:Label ID="Label20" runat="server" Font-Size="12px" Text="Master Stockist : " ForeColor="Black"></asp:Label>

                                                </p>
                                            </div>
                                            <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                    <asp:Label Style="border: none; text-align: right;" inputmode="decimal" ID="lblpMSPrice" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="100%" Height="33px"></asp:Label>
                                                </p>
                                            </div>
                                            <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                    <asp:Label Style="border: none; text-align: right;" inputmode="decimal" ID="lblMSDiscType" runat="server" Font-Size="12px" Font-Bold="true" ForeColor="Black" Width="100%" Height="33px"></asp:Label>
                                                </p>
                                            </div>
                                             <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                    <asp:Label Style="border: none; text-align: right;" inputmode="decimal" ID="lblMSDiscAmt" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="100%" Height="33px"></asp:Label>
                                                </p>
                                            </div>
                                            <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                    <asp:Label Style="border: thin; text-align: right;" ID="lblpMSpromoPrice" inputmode="decimal" onkeypress="return IsNumberWithOneDecimal(this,event);" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="60%" Height="33px"></asp:Label>
                                                </p>
                                            </div>
                                            <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2" style="text-align: right; border: 1px solid lightgrey; min-height: 30px;">
                                                <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                    <asp:Label Style="border: none; text-align: right;" ID="txtpMSpromocashback" inputmode="decimal" onkeypress="return IsNumberWithOneDecimal(this,event);" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="100%" Height="33px"></asp:Label>
                                                </p>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-sm-12 col-xs-12 col-md-12 col-lg-12 goleft" style="height: 20px">
                                    </div>
                                    <div id="Tabs" role="tabpanel">
                                        <!-- Nav tabs -->
                                        <ul class="nav nav-tabs" role="tablist" id="myTab<%=promoLine%>">
                                            <li class="active"><a href="#reward<%=promoLine%>" style="line-height: 0.428571 !important;" aria-controls="member" role="tab" data-toggle="tab">Reward & Redemption</a></li>
                                            <li><a href="#other<%=promoLine%>" aria-controls="stockist" role="tab" data-toggle="tab" style="line-height: 0.428571 !important;">Others</a></li>

                                            <%--                                                <asp:Button ID="btnpromo" runat="server" Text="Promotion" CssClass="btnsave" OnClick="btnpromo_Click" style="float:right;margin-right:10px" />--%>
                                        </ul>
                                        <!-- Tab panes -->
                                        <div class="tab-content">
                                            <!-- member tab -->
                                            <div role="tabpanel" class="tab-pane fade in active" id="reward<%=promoLine%>">
                                                <div class="col-sm-12 col-xs-12 col-md-12 col-lg-12" style="color: Black; padding-left: 5px; padding-right: 5px;">
                                                    <div class="has-success">
                                                        <label class="col-sm-10 col-lg-4">Redemption Item</label>

                                                    </div>
                                                </div>

                                                <div class="col-sm-12 col-xs-12 col-md-12 col-lg-12" style="color: Black; padding-left: 5px; padding-right: 5px;">
                                                    <div>
                                                        <div class="col-xs-3 col-sm-3 col-md-3 col-lg-3" style="border: 1px solid lightgrey; height: 30px; text-align: center;">
                                                            <p style="margin-bottom: 1px !important; padding-right: 2px; margin: 5px;">
                                                                <asp:Label ID="Label8" runat="server" Text="Member Level" Font-Size="12px" Font-Bold="true" ForeColor="Black"></asp:Label>
                                                            </p>
                                                        </div>
                                                        <div class="col-xs-4 col-sm-4 col-md-4 col-lg-4" style="border: 1px solid lightgrey; height: 30px; text-align: center;">
                                                            <p style="margin-bottom: 1px !important; padding-right: 2px; margin: 5px;">
                                                                <asp:Label ID="Label11" runat="server" Text="Redemption Point" Font-Size="12px" Font-Bold="true" ForeColor="Black"></asp:Label>
                                                            </p>
                                                        </div>
                                                        <div class="col-xs-4 col-sm-4 col-md-4 col-lg-4" style="border: 1px solid lightgrey; height: 30px; text-align: center;">
                                                            <p style="margin-bottom: 1px !important; padding-right: 2px; margin: 5px;">
                                                                <asp:Label ID="Label12" runat="server" Text="Member Reward Point" Font-Size="12px" Font-Bold="true" ForeColor="Black"></asp:Label>
                                                            </p>
                                                        </div>
                                                    </div>
                                                    <div>
                                                        <div class="col-xs-3 col-sm-3 col-md-3 col-lg-3" style="border: 1px solid lightgrey; min-height: 35px;">
                                                            <p style="margin-bottom: 3px !important; padding-left: 5px; padding-top: 6px;">
                                                                <asp:Label ID="Label15" runat="server" Font-Size="12px" Text="Member : " ForeColor="Black"></asp:Label>

                                                            </p>
                                                        </div>
                                                        <div class="col-xs-4 col-sm-4 col-md-4 col-lg-4" style="border: 1px solid lightgrey; text-align: right; min-height: 30px;">
                                                            <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                                <asp:Label Style="border: none; text-align: right;" inputmode="decimal" ID="txtMredemptpoint" Text="0" onkeypress="return IsNumberWithOneDecimal(this,event);" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="60%" Height="33px"></asp:Label>
                                                            </p>
                                                        </div>
                                                        <div class="col-xs-4 col-sm-4 col-md-4 col-lg-4" style="border: 1px solid lightgrey; text-align: right; min-height: 30px;">
                                                            <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                                <asp:Label Style="border: none; text-align: right;" ID="txtMrewardpoint" inputmode="decimal" Text="0" onkeypress="return IsNumberWithOneDecimal(this,event);" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="60%" Height="33px"></asp:Label>
                                                            </p>
                                                        </div>
                                                    </div>
                                                    <div>
                                                        <div class="col-xs-3 col-sm-3 col-md-3 col-lg-3" style="border: 1px solid lightgrey; min-height: 35px;">
                                                            <p style="margin-bottom: 3px !important; padding-left: 5px; padding-top: 6px;">
                                                                <asp:Label ID="Label19" runat="server" Font-Size="12px" Text="VIP : " ForeColor="Black"></asp:Label>

                                                            </p>
                                                        </div>
                                                        <div class="col-xs-4 col-sm-4 col-md-4 col-lg-4" style="border: 1px solid lightgrey; text-align: right; min-height: 30px;">
                                                            <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                                <asp:Label Style="border: none; text-align: right;" ID="txtVIPredemptpoint" inputmode="decimal" Text="0" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="60%" Height="33px"></asp:Label>
                                                            </p>
                                                        </div>
                                                        <div class="col-xs-4 col-sm-4 col-md-4 col-lg-4" style="border: 1px solid lightgrey; text-align: right; min-height: 30px;">
                                                            <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                                <asp:Label Style="border: none; text-align: right;" ID="txtVIPrewardpoint" inputmode="decimal" Text="0" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="60%" Height="33px"></asp:Label>
                                                            </p>
                                                        </div>
                                                    </div>
                                                    <div>
                                                        <div class="col-xs-3 col-sm-3 col-md-3 col-lg-3" style="border: 1px solid lightgrey; min-height: 35px;">
                                                            <p style="margin-bottom: 3px !important; padding-left: 5px; padding-top: 6px;">
                                                                <asp:Label ID="Label24" runat="server" Font-Size="12px" Text="VVIP : " ForeColor="Black"></asp:Label>

                                                            </p>
                                                        </div>
                                                        <div class="col-xs-4 col-sm-4 col-md-4 col-lg-4" style="border: 1px solid lightgrey; text-align: right; min-height: 30px;">
                                                            <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                                <asp:Label Style="border: none; text-align: right;" ID="txtVVIPredemptpoint" inputmode="decimal" Text="0" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="60%" Height="33px"></asp:Label>
                                                            </p>
                                                        </div>
                                                        <div class="col-xs-4 col-sm-4 col-md-4 col-lg-4" style="border: 1px solid lightgrey; text-align: right; min-height: 30px;">
                                                            <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                                <asp:Label Style="border: none; text-align: right;" ID="txtVVIPrewardpoint" inputmode="decimal" Text="0" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="60%" Height="33px"></asp:Label>
                                                            </p>
                                                        </div>
                                                    </div>
                                                    <div>
                                                        <div class="col-xs-3 col-sm-3 col-md-3 col-lg-3" style="border: 1px solid lightgrey; min-height: 35px;">
                                                            <p style="margin-bottom: 3px !important; padding-left: 5px; padding-top: 6px;">
                                                                <asp:Label ID="Label28" runat="server" Font-Size="12px" Text="Stockist : " ForeColor="Black"></asp:Label>

                                                            </p>
                                                        </div>
                                                        <div class="col-xs-4 col-sm-4 col-md-4 col-lg-4" style="border: 1px solid lightgrey; text-align: right; min-height: 30px;">
                                                            <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                                <asp:Label Style="border: none; text-align: right;" ID="txtSredemptpoint" inputmode="decimal" Text="0" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="60%" Height="33px"></asp:Label>
                                                            </p>
                                                        </div>
                                                        <div class="col-xs-4 col-sm-4 col-md-4 col-lg-4" style="border: 1px solid lightgrey; text-align: right; min-height: 30px;">
                                                            <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                                <asp:Label Style="border: none; text-align: right;" ID="txtSrewardpoint" inputmode="decimal" Text="0" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="60%" Height="33px"></asp:Label>
                                                            </p>
                                                        </div>
                                                    </div>
                                                    <div>
                                                        <div class="col-xs-3 col-sm-3 col-md-3 col-lg-3" style="border: 1px solid lightgrey; min-height: 35px;">
                                                            <p style="margin-bottom: 3px !important; padding-left: 5px; padding-top: 6px;">
                                                                <asp:Label ID="Label29" runat="server" Font-Size="12px" Text="Master Stockist : " ForeColor="Black"></asp:Label>

                                                            </p>
                                                        </div>
                                                        <div class="col-xs-4 col-sm-4 col-md-4 col-lg-4" style="border: 1px solid lightgrey; text-align: right; min-height: 30px;">
                                                            <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                                <asp:Label Style="border: none; text-align: right;" ID="txtMSredemptpoint" inputmode="decimal" Text="0" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="60%" Height="33px"></asp:Label>
                                                            </p>
                                                        </div>
                                                        <div class="col-xs-4 col-sm-4 col-md-4 col-lg-4" style="border: 1px solid lightgrey; text-align: right; min-height: 30px;">
                                                            <p style="margin-bottom: 1px !important; padding-right: 2px;">
                                                                <asp:Label Style="border: none; text-align: right;" ID="txtMSrewardpoint" inputmode="decimal" Text="0" runat="server" Font-Size="16px" Font-Bold="true" ForeColor="Black" Width="60%" Height="33px"></asp:Label>
                                                            </p>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-sm-12 col-xs-12 col-md-12 col-lg-12 goleft" style="height: 20px">
                                            </div>
                                            <!-- stockist tab -->
                                            <div role="tabpanel" class="tab-pane fade in" id="other<%=promoLine%>">
                                                <div class="col-sm-12 col-xs-12 col-md-12 col-lg-12 goleft" style="color: Black; padding-left: 5px; padding-right: 5px;">
                                                    <div class="has-success">
                                                        <label id="Label1" runat="server" class="col-sm-10 control-label col-lg-7">SPP</label>
                                                        <div class="form-group col-sm-5">
                                                            <asp:TextBox ID="txtSPP" runat="server" Enabled="false" onkeypress="return IsNumberWithOneDecimal(this,event);" AutoComplete="off" Text="0" CssClass="col-md-12 col-xs-12 col-lg-12" Style="border: 2; height: 34px; padding: 6px 12px; font-size: 14px;"></asp:TextBox>
                                                        </div>
                                                        <br />
                                                        <label id="Label10" runat="server" class="col-sm-10 control-label col-lg-7">Referral Bonus <span id="spprofit" runat="server"></span></label>
                                                        <div class="form-group col-sm-5">
                                                            <asp:Label ID="lblRefProfitBonusPercent" runat="server"></asp:Label>
                                                            <asp:TextBox ID="txtRefProfitBonusPercent" Enabled="false" runat="server" onkeypress="return IsNumberWithOneDecimal(this,event);" AutoComplete="off" Text="0" CssClass="col-md-12 col-xs-12 col-lg-12" Style="border: 2; height: 34px; padding: 6px 12px; font-size: 14px;"></asp:TextBox>
                                                        </div>
                                                        <br />
                                                        <label id="Label17" runat="server" class="col-sm-10 control-label col-lg-7">Network Plus Bonus (Level 1)</label>
                                                        <div class="form-group col-sm-5">
                                                            <asp:TextBox ID="txtTeamKPI2" runat="server" Enabled="false" onkeypress="return IsNumberWithOneDecimal(this,event);" Text="0" AutoComplete="off" CssClass="col-md-12 col-xs-12 col-lg-12" Style="border: 2; height: 34px; padding: 6px 12px; font-size: 14px;"></asp:TextBox>
                                                        </div>
                                                        <br />
                                                        <label id="Label13" runat="server" class="col-sm-10 control-label col-lg-7">Network Plus Bonus (Level 2)</label>
                                                        <div class="form-group col-sm-5">
                                                            <asp:TextBox ID="txtTeamKPI" runat="server" Enabled="false" onkeypress="return IsNumberWithOneDecimal(this,event);" Text="0" AutoComplete="off" CssClass="col-md-12 col-xs-12 col-lg-12" Style="border: 2; height: 34px; padding: 6px 12px; font-size: 14px;"></asp:TextBox>
                                                        </div>
                                                        <br />
                                                        <label id="Label3" runat="server" class="col-sm-10 control-label col-lg-7">Merchant Brand Dev. Incentive (VVIP Only)</label>
                                                        <div class="form-group col-sm-5">
                                                            <asp:TextBox ID="txtMBD" runat="server" Enabled="false" onkeypress="return IsNumberWithOneDecimal(this,event);" Text="0" AutoComplete="off" CssClass="col-md-12 col-xs-12 col-lg-12" Style="border: 2; height: 34px; padding: 6px 12px; font-size: 14px;"></asp:TextBox>
                                                        </div>
                                                        <br />
                                                        <label id="Label14" runat="server" class="col-sm-10 control-label col-lg-7">Month End Bonus</label>
                                                        <div class="form-group col-sm-5">
                                                            <asp:TextBox ID="txtMEB" runat="server" Enabled="false" onkeypress="return isNumberKey(this,event)" Text="0" AutoComplete="off" CssClass="col-md-12 col-xs-12 col-lg-12" Style="border: 2; height: 34px; padding: 6px 12px; font-size: 14px;"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <%promoLine++;%>
                                    </div>

                                </div>
                                <div class="modal-footer">
                                           <asp:Button ID="btnPromotion" runat="server" Text="Promotion Setting" CssClass="btn btn-success" OnClick="btnPromotion_Click" Style="padding: 6px; width: 100%; margin-left: 5px; text-align: center; text-decoration: none; display: inline-block; cursor: pointer;" />
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                         
                    </div>
                </div>
            </div>

            <asp:UpdateProgress ID="ppp1" runat="server" AssociateUpdatePanelID="UpdatePanel1">
                <ProgressTemplate>
                    <div id="overlay">
                        <div id="modalprogress">
                            <div id="theprogress">
                                <p>
                                    <img alt="Loading" src="Images/GIF/spin.gif" width="60px" height="60px" />
                                </p>
                            </div>
                        </div>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="txt_Search" EventName="TextChanged" />
            <asp:AsyncPostBackTrigger ControlID="ddlStatus" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="ddlPublish" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="ddlSoldStatus" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="ddlDepartment" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="ddlCategory" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="ddlcostrate" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="ddlPager" EventName="SelectedIndexChanged" />
            <asp:PostBackTrigger ControlID="btnExportExcel" />
        </Triggers>
    </asp:UpdatePanel>
        </ContentTemplate>
    </asp:UpdatePanel>

<script type="text/javascript">
    var modifierSeq = 0;

    function trackModifierSeq(chk) {
        var hdSeq = chk.parentNode.querySelector("input[type=hidden][id*='hdSequence']");

        if (chk.checked) {
            modifierSeq++;
            hdSeq.value = modifierSeq;  // assign order number
        } else {
            hdSeq.value = 0;  // reset when unchecked
        }

        normalizeSequences();
    }

    function normalizeSequences() {
        var allSeq = document.querySelectorAll("input[type=hidden][id*='hdSequence']");
        var seqList = [];

        allSeq.forEach(function (el) {
            if (parseInt(el.value) > 0) {
                seqList.push(el);
            }
        });

        // sort by assigned sequence
        seqList.sort(function (a, b) { return a.value - b.value; });

        for (var i = 0; i < seqList.length; i++) {
            seqList[i].value = i + 1;
        }
    }
</script>

<script>

    function sweetalert_warning(message, messagetype) {
        console.log(messagetype);
        Swal.fire({
            position: 'center',
            icon: messagetype,
            title: message,
            showConfirmButton: true
        })
        setTimeout(function () { redirect(); }, 1500);

    }


    function sweetalert_error(message, messagetype) {
        Swal.fire({
            position: 'center',
            icon: messagetype,
            title: message,
            showConfirmButton: true
        })

    }
</script>
</asp:Content>
