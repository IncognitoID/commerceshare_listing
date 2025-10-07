<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Printer_MaintenanceListing.aspx.cs" Inherits="Printer_MaintenanceListing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .txt_Search {
            background: url(Images/search2.png) no-repeat;
            padding-left: 18px;
            margin-top: 10px;
            border: 1px solid #ccc;
        }

        .ghost-button-thick-border1 {
            display: inline-block;
            width: 120px;
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

        function toggleSelectAll(source) {
            var grid = document.getElementById('<%= grd_viewitem.ClientID %>');
            if (!grid) return;

            var checkboxes = grid.querySelectorAll("input[type='checkbox'][id*='chkitem']");
            checkboxes.forEach(function (cb) {
                cb.checked = source.checked;
            });

        }

    </script>



    <asp:HiddenField ID="PaneName" runat="server" />
    <div class="col-md-12 col-sm-12" style="color: Black">
        <asp:Label ID="MF_Con" runat="server" Text="Label" Visible="false"></asp:Label>
        <asp:Label ID="lblCatCode" runat="server" Visible="false"></asp:Label>
        <asp:Label ID="lblCatID" runat="server" Visible="false"></asp:Label>
        <div style="padding-bottom: 10px; height: 50px; margin-top: 10px;">
            <h2 style="font-size: x-large; font-weight: bold; margin-top: 13px; margin-bottom: 0px; width: 40%; float: left;">Printer Listing</h2>
            <asp:TextBox ID="txt_Search" runat="server" AutoComplete="off" MaxLength="50" placeholder="Filter by Printer Name / IP Address" AutoPostBack="true" OnTextChanged="txt_Search_TextChanged" CssClass="col-md-5 col-sm-5 txt_Search" Style="display: inline-block; font-weight: bold; padding: 8px; padding-left: 35px; text-align: left; outline: none; text-decoration: none; float: right"></asp:TextBox>
            <br />
        </div>
        <div style="padding-top: 12px; padding-left: 12px; padding-bottom: 12px; margin-bottom: 5px;">
            <asp:DropDownList ID="ddlPager" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPager_SelectedIndexChanged" Style="float: left; display: inline-block; width: 90px; height: 20px; font-weight: bold; text-align: center; text-decoration: none; border: none;"></asp:DropDownList>
            &nbsp&nbsp<asp:Label ID="lbl_Record" runat="server" Text="No. Of Record: "></asp:Label><span><asp:Label ID="lbl_record3" runat="server"></asp:Label>

                <asp:Label ID="lbl_Record2" runat="server"></asp:Label></span>
            <asp:Button ID="Button1" runat="server" Text="Add Printer" OnClick="btn_New_Click" CssClass="ghost-button-thick-border1" />
        </div>
        <asp:UpdatePanel ID="updatepanel1" runat="server">
            <ContentTemplate>
                <asp:GridView ID="grd_View" ShowFooter="false" runat="server" AutoGenerateColumns="False" DataKeyNames="ids"
                    OnRowCommand="View_RowCommand" GridLines="Horizontal" OnRowDataBound="View_RowDataBound"
                    CssClass="footableThis" ForeColor="#333333" BorderStyle="double" BorderColor="#bbbbbb">
                    <Columns>
                        <asp:TemplateField>
                            <ItemStyle Width="40px" />
                            <ItemTemplate>
                                <div class="dropdown">
                                    <p class="dropdown-toggle" data-toggle="dropdown">
                                        <a class="glyphicon glyphicon-option-vertical" style="font-size: 20pt; pointer-events: auto;"></a>
                                    </p>
                                    <ul class="dropdown-menu">
                                        <li>
                                            <asp:LinkButton ID="lnkedit" Text="Edit" runat="server" CommandArgument='<%# Eval("ids") %>' CommandName="edit"></asp:LinkButton></li>
                                        <li>
                                            <asp:LinkButton ID="lnkassignitem" Text="Assign Item" runat="server" CommandArgument='<%# Eval("ids") %>' CommandName="assgn"></asp:LinkButton></li>
                                        <li>
                                            <asp:LinkButton ID="lnkdel" Text="Delete" runat="server" CommandArgument='<%# Eval("ids") %>' CommandName="del" OnClientClick="Confirm()"></asp:LinkButton></li>
                                    </ul>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Printer Name" SortExpression="print_name">
                            <ItemTemplate>
                                <asp:Label ID="lblprinterName" runat="server" Text='<%# Eval("print_name") %>'></asp:Label>
                                <br />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="IP_Address" HeaderText="IP Address" SortExpression="IP_Address" />
                    </Columns>
                    <RowStyle BackColor="White" />
                    <SelectedRowStyle BackColor="#e7e7e7" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#696969" Font-Bold="True" Height="45px" ForeColor="White" CssClass="grdview_header" />
                    <EditRowStyle BackColor="#696969" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
                <br />


                <asp:HiddenField ID="hdMerchantCode" runat="server" />
                <asp:HiddenField ID="hdPrinterId" runat="server" />


                <div class="modal fade" id="PrinterAssignModal" tabindex="-5" role="dialog">
                    <div class="modal-dialog">
                        <div class="modal-content" style="margin-top: 0; width: 800px">
                            <div class="modal-header">
                                <button type="button" class="close elementgray" style="color: black" data-dismiss="modal">×</button>
                                <h4 class="modal-title" style="font-weight: bold;">Assign Items to Printer
                                <asp:Label ID="lblPrinterHdr" runat="server" Style="font-weight: normal; font-size: 12px; margin-left: 8px;"></asp:Label>
                                </h4>
                            </div>

                            <div class="modal-body">
                                           
                                <asp:UpdatePanel ID="updAssignItems" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>   
                                        
                                            <asp:Panel ID="pnlSearch" runat="server" DefaultButton="btnSearchItems">
                                            <div class="col-xs-13 col-lg-12" "row">

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
                                                                    CssClass="btn btn-success btn-sm" OnClick="btnClearSearch_Click" />
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
                                                            All <asp:CheckBox ID="chkSelectAll" runat="server" onclick="toggleSelectAll(this)" />
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

                                                    <asp:TemplateField HeaderText="Item Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblItemName" runat="server" Text='<%# Bind("longdesc") %>' />
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

                                <asp:Button ID="btnUpdatePrinterItems" runat="server" Text="Update"
                                    CssClass="btn btn-success" Style="width: 100%;" OnClick="btnUpdatePrinterItems_Click" />
                                    </ContentTemplate>
                                    </asp:UpdatePanel>
                                
                                   <asp:UpdatePanel>
                                       <Triggers>
                                           <asp:AsyncPostBackTrigger ControlID="ddlDept" EventName="SelectedIndexChanged" />
                                           <asp:AsyncPostBackTrigger ControlID="ddlCat" EventName="SelectedIndexChanged" />
                                           <asp:AsyncPostBackTrigger ControlID="btnSearchItems" EventName="Click" />
                                           <asp:AsyncPostBackTrigger ControlID="btnClearSearch" EventName="Click" />
                                           <asp:AsyncPostBackTrigger ControlID="btnUpdatePrinterItems" EventName="Click" />
                                       </Triggers>

                                </asp:UpdatePanel>

                            </div>
                    </div>    
             </div>
     </div>


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
