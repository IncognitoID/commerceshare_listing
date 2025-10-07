<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ModifierSetting.aspx.cs" Inherits="ModifierSetting" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtoolkit" %>
<%@ Register Assembly="msgBox" Namespace="BunnyBear" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .switch {
            position: relative;
            display: inline-block;
            width: 60px;
            height: 34px;
        }

            .switch input {
                opacity: 0;
                width: 0;
                height: 0;
            }

        .slider {
            position: absolute;
            cursor: pointer;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background-color: #ccc;
            -webkit-transition: .4s;
            transition: .4s;
        }

            .slider:before {
                position: absolute;
                content: "";
                height: 26px;
                width: 26px;
                left: 4px;
                bottom: 4px;
                background-color: white;
                -webkit-transition: .4s;
                transition: .4s;
            }

        input:checked + .slider {
            background-color: #2196F3;
        }

        input:focus + .slider {
            box-shadow: 0 0 1px #2196F3;
        }

        input:checked + .slider:before {
            -webkit-transform: translateX(26px);
            -ms-transform: translateX(26px);
            transform: translateX(26px);
        }

        /* Rounded sliders */
        .slider.round {
            border-radius: 34px;
        }

            .slider.round:before {
                border-radius: 50%;
            }

        .boxes {
            float: right;
            padding: 9px;
            width: 35%;
            border: solid 1px #cfcfcf;
        }

        .btn1 {
            background-color: #eb4034;
            border: none;
            color: white;
            padding: 15px 32px;
            text-align: center;
            text-decoration: none;
            display: inline-block;
            font-size: 16px;
            margin: 4px 2px;
            cursor: pointer;
            border-radius: 4px;
        }

            .btn1:hover {
                background-color: #c7342a;
            }


        .txt_Search {
            background: url(Images/search2.png) no-repeat;
            padding-left: 18px;
            margin-top: 10px;
            border: 1px solid #ccc;
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

        .grdview_header > thead > tr > th {
            padding: 10px 2px 10px 2px;
        }


        .footableThis {
            border-collapse: inherit;
            border-spacing: 0;
            width: 100%;
            border: 0px;
            font-family: "Open Sans", Arial, Helvetica, sans-serif;
            color: #444444;
            border: #e9e9e9 !important;
            background: #efefef;
            -moz-border-radius: 0;
            -webkit-border-radius: 0;
            border-radius: 0;
        }

            .footableThis > thead > tr > th {
                border-bottom: 1px solid #F3F3F3;
                padding: 10px 2px 10px 2px;
                text-align: left;
            }

            .footableThis tr:hover {
                background-color: #e3e3e3;
            }


            .footableThis > thead > tr > th, .footable > thead > tr > td {
                background-color: #696969;
                border: 1px solid #696969;
                color: #ffffff;
                border-top: none;
                border-left: none;
                font-weight: normal;
                border-right: none;
                /*padding:15px !important;*/
            }
    </style>
    <script type="text/javascript">
        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Are you sure you want to delete this record?")) {
                confirm_value.value = "OK";
            } else {
                confirm_value.value = "Cancel";
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

        function toggleSelectAll(source) {
            var grid = document.getElementById('<%= grd_viewitem.ClientID %>');
            if (!grid) return;

            var checkboxes = grid.querySelectorAll("input[type='checkbox'][id*='chkitem']");
            for (var i = 0; i < checkboxes.length; i++) {
                checkboxes[i].checked = source.checked;
            }
        }
    </script>


    </script>
    <cc1:msgBox ID="MsgBox1" runat="server"></cc1:msgBox>
    <asp:HiddenField ID="PaneName" runat="server" />
    <asp:UpdatePanel ID="updatepanel" runat="server">
        <ContentTemplate>
            <div class="col-md-12 col-sm-12" style="color: Black">
                <asp:Label ID="MF_Con" runat="server" Text="Label" Visible="false"></asp:Label>
                <div style="padding-bottom: 10px; height: 50px; margin-top: 10px;">
                    <h2 style="font-size: x-large; font-weight: bold; margin-top: 13px; margin-bottom: 0px; width: 40%; float: left;">Modifier Group</h2>
                    <asp:TextBox ID="txt_Search" runat="server" AutoComplete="off" MaxLength="50" placeholder="Filter by Modifier Group" AutoPostBack="true" OnTextChanged="txt_Search_TextChanged" CssClass="col-md-5 col-sm-5 txt_Search" Style="display: inline-block; font-weight: bold; padding: 8px; padding-left: 35px; text-align: left; outline: none; text-decoration: none; float: right;"></asp:TextBox>
                    <br />
                    <br />
                    <br />
                </div>
                <div style="padding-top: 12px; padding-left: 12px; padding-bottom: 12px; margin-bottom: 5px;">
                    <asp:DropDownList ID="ddlPager" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPager_SelectedIndexChanged" Style="float: left; display: inline-block; width: 90px; height: 20px; font-weight: bold; text-align: center; text-decoration: none; border: none;"></asp:DropDownList>
                    &nbsp&nbsp<asp:Label ID="lbl_Record" runat="server" Text="No. Of Record: "></asp:Label><span><asp:Label ID="lbl_record3" runat="server"></asp:Label>
                        of
                        <asp:Label ID="lbl_Record2" runat="server"></asp:Label></span>
                    <asp:Button ID="btn_New" runat="server" Text="Create Modifer Group" OnClick="btn_New_Click" Style="width: 190px !important;" CssClass="ghost-button-thick-border1" />
                </div>
                <asp:GridView ID="grd_View" ShowFooter="false" runat="server" AutoGenerateColumns="False" DataKeyNames="modifier_id"
                    OnRowCommand="View_RowCommand" GridLines="Horizontal" OnRowDataBound="View_RowDataBound"
                    CssClass="footableThis" ForeColor="#333333" BorderStyle="double" BorderColor="#bbbbbb">
                    <Columns>
                        <asp:TemplateField>
                            <ItemStyle Width="35px" Height="50px" />
                            <ItemTemplate>
                                <div class="dropdown">
                                    <p class="dropdown-toggle" data-toggle="dropdown">
                                        <a class="glyphicon glyphicon-option-vertical" style="font-size: 20pt; cursor: pointer; text-decoration: none;"></a>
                                    </p>
                                    <ul class="dropdown-menu">
                                        <li>
                                            <asp:LinkButton ID="lnkedit" Text="Edit" runat="server" CommandArgument='<%# Eval("modifier_id") %>' CommandName="edit"></asp:LinkButton></li>
                                        <li>
                                            <asp:LinkButton ID="lnkassignitem" Text="Assign Item" runat="server" CommandArgument='<%# Eval("modifier_id") %>' CommandName="assgn"></asp:LinkButton></li>
                                        <li>
                                            <asp:LinkButton ID="lnkdel" Text="Delete" runat="server" CommandArgument='<%# Eval("modifier_id") %>' CommandName="del" OnClientClick="Confirm()"></asp:LinkButton></li>
                                    </ul>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Modifier Name">
                            <ItemStyle Height="50px" />
                            <ItemTemplate>
                                <asp:Label ID="lblmodifierGrp" runat="server" Text='<%# Eval("Modifier_Grp_Name") %>'></asp:Label>
                                <br />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="option_name" HeaderText="Option(s)" />
                    </Columns>
                    <RowStyle BackColor="White" Height="50px" />
                    <SelectedRowStyle BackColor="#e7e7e7" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#696969" Font-Bold="True" Height="45px" ForeColor="White" />
                    <EditRowStyle BackColor="#696969" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
                <br />

            <asp:HiddenField ID="hdModifierId" runat="server" />
            <asp:HiddenField ID="hdAssignModifierName" runat="server" />


            <div class="modal fade" id="ModifierModal" tabindex="-5" role="dialog">
                <div class="modal-dialog">
                    <!-- Modal content-->
                    <div class="modal-content" style="margin-top: 0px; width: 700px">
                        <div class="modal-header">
                            <button type="button" class="close elementgray" style="color: black" data-dismiss="modal">×</button>
                            <h4 class="modal-title" style="font-weight: bold;">Item Modifier Setting
                    <asp:Label ID="lblAssignModifierName" runat="server" Style="font-weight: normal; font-size: 12px; margin-left: 8px;"></asp:Label>
                            </h4>
                        </div>

                        <div class="modal-body">
                            <asp:UpdatePanel ID="updAssignItems" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>

                                    <asp:Panel ID="pnlSearch" runat="server" DefaultButton="btnSearchItems">
                                        <div class="col-xs-12 col-lg-12" "row">

                                            <!-- Search -->
                                            <div class="col-xs-12">
                                                <div class="form-group">
                                                    <label class="control-label">Search</label>
                                                    <div class="input-group input-group-sm"> 
                                                        <asp:TextBox ID="txtItemSearch" runat="server" CssClass="form-control"
                                                            MaxLength="100" placeholder="e.g. ABC123, Coke..." />
                                                        <span class="input-group-btn">
                                                            <asp:Button ID="btnSearchItems" runat="server" Text="Search"
                                                                CssClass="btn btn-success btn-sm" OnClick="btnSearchItems_Click" />                                                       
                                                            <asp:Button ID="btnClearSearch" runat="server" Text="Clear"
                                                                CssClass="btn btn-success btn-sm" OnClick="btnClearSearch_Click"  />
                                                        </span>
                                                    </div>
                                                </div>
                                            </div>


                                            <!-- Department -->
                                            <div class="col-xs-12 col-md-5">
                                                <div class="form-group">
                                                    <label class="control-label">Department</label>
                                                    <asp:DropDownList ID="ddlDept" runat="server" CssClass="form-control input-sm"
                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlDept_SelectedIndexChanged" />
                                                    </div>
                                                </div>

                                                <!-- Category -->
                                                <div class="col-xs-12 col-md-6 col-md-offset-1">
                                                    <div class="form-group">
                                                        <label class="control-label">Category</label>
                                                        <asp:DropDownList ID="ddlCat" runat="server" CssClass="form-control input-sm"
                                                            Enabled="false"
                                                            AutoPostBack="true" OnSelectedIndexChanged="ddlCat_SelectedIndexChanged" />
                                                    </div>
                                                </div>

                                            </div>
                                        </asp:Panel>

                                        <div class="col-xs-12 col-lg-12 col-md-12" style="margin-bottom: 5px; text-align: center">
                                            <br />
                                            <span id="ctl00_ContentPlaceHolder1_lbldesc" style="font-weight: bold; font-size: medium"></span>
                                            <br />
                                            <span id="ctl00_ContentPlaceHolder1_lblbarcode" style="font-weight: bold; font-size: small"></span>
                                        </div>

                                        <div class="col-xs-12 col-lg-12 col-md-12" style="overflow-y: auto; height: 250px; margin-bottom: 10px">
                                            <asp:GridView ID="grd_viewitem" runat="server" AutoGenerateColumns="false" GridLines="Horizontal" CssClass="table table-bordered table-responsive table-hover" OnRowDataBound="grd_viewitem_RowDataBound">
                                                
                                                <Columns>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        All
                                                        <asp:CheckBox ID="chkSelectAll" runat="server" onclick="toggleSelectAll(this)" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkitem" runat="server" />
                                                        <asp:HiddenField ID="hditem_code" runat="server" Value='<%# Bind("item_code") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Barcode" ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblbarcode" runat="server" Text='<%# Bind("barcode") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Item" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbldesc" runat="server" Text='<%# Bind("longdesc") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Image" ItemStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <asp:Image runat="server" ID="imgItem" CssClass="imgSize" Style="border-radius: 5px; width: 80px; height: 80px;" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                        </asp:GridView>
                                    </div>

                                    <br />
                                    <asp:Button ID="btnupdateitem" runat="server" Text="Update"
                                        CssClass="btn btn-success" Style="width: 100%" OnClick="btnupdateitem_Click" />

                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnupdateitem" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnSearchItems" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnClearSearch" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </ContentTemplate>
</asp:UpdatePanel>
    <!-- <script type="text/jscript">
    function showmodal() {
        $('#PrinterAssignModal').modal('show');
    }
</script> -->
    <script type="text/javascript">
        $(document).ready(function () {
            // Initialize modal once
            $('#PrinterAssignModal').modal({
                show: false,
                backdrop: 'static',
                keyboard: false
            });
        });
    </script>
</asp:Content>

