<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ItemGroupingSetup.aspx.cs" Inherits="ItemGroupingSetup" %>

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
    </style>
    <script language="javascript" type="text/javascript">
        function previewFile2() {
            var preview = document.querySelector('#<%=Img1.ClientID %>');
            var file = document.querySelector('#<%=fileupload2.ClientID %>').files[0];
            var reader = new FileReader();

            reader.onloadend = function () {
                preview.src = reader.result;
            }

            if (file) {
                reader.readAsDataURL(file);
            } else {
                preview.src = "Images/NoPic.png";
            }
        }

        function previewFile1() {
            var preview = document.querySelector('#<%=Img2.ClientID %>');
            var file = document.querySelector('#<%=fileupload1.ClientID %>').files[0];
            var reader = new FileReader();

            reader.onloadend = function () {
                preview.src = reader.result;
            }

            if (file) {
                reader.readAsDataURL(file);
            } else {
                preview.src = "Images/NoPic.png";
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Label ID="MF_Con" runat="server" Text="Label" Visible="false"></asp:Label>
    <asp:Label ID="lblrunno" runat="server" Visible="false"></asp:Label>
    <div class="panel-group" id="Item_Page">
        <div class="panel panel-default">
            <div class="panel-heading">
                <h4 class="panel-title">
                    <a data-toggle="collapse" data-parent="#accordion" href="#collapse1" id="groupingtitle" runat="server"></a>
                </h4>
            </div>
            <div id="collapse1" class="panel-collapse">
                <div class="panel-body">
                    <div class="has-success">
                        <label class="col-sm-10 col-lg-4">Group Title</label>
                        <div class="form-group col-sm-8">
                            <asp:TextBox ID="txtgrouptitle" runat="server" AutoComplete="off" placeholder="Example : SD001" CssClass="col-md-12 col-xs-12" Style="border: 2; height: 34px; padding: 6px 12px; font-size: 14px;"></asp:TextBox>
                            <ajaxtoolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Numbers, Custom" ValidChars=",.'()`+-*/& " TargetControlID="txtgrouptitle" />
                        </div>
                    </div>

                    <div class="col-sm-10 control-label col-lg-4">
                        <label class="col-sm-10 col-lg-4">Icon</label>
                        <%--<asp:FileUpload ID="FileUpload1" runat="server" CssClass="col-md-12 col-xs-12" />--%>
                        <input id="fileupload2" runat="server" type="file" name="file" onchange="previewFile2()" accept="image/*" />
                        <br />

                    </div>
                    <div class="form-group col-sm-8">
                        <img id="Img1" runat="server" src="Images/55x55.png" class="noImage" style="width: 70px; height: 70px; border-radius: 5px 5px 5px 5px;" />
                    </div>

                    <div class="col-sm-10 control-label col-lg-4">
                        <label class="col-sm-10 col-lg-4">Banner</label>
                        <%--<asp:FileUpload ID="FileUpload1" runat="server" CssClass="col-md-12 col-xs-12" />--%>
                        <input id="fileupload1" runat="server" type="file" name="file" onchange="previewFile1()" accept="image/*" />
                        <br />

                    </div>
                    <div class="form-group col-sm-8">
                        <img id="Img2" runat="server" src="Images/1080x409.png" class="noImage" style="width: 250px; height: 70px; border-radius: 5px 5px 5px 5px;" />
                    </div>


                    <div class="clear flex-container" style="background-color: white; border-radius: 5px; margin: 0px; padding: 20px;">
                        <asp:UpdatePanel ID="updateee" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="flex-container">
                                    <label class="col-sm-3 control-label col-lg-4 " style="color: black !important; font-size: x-large!important">Item List</label>
                                    <asp:Button ID="btnAdd" runat="server" data-toggle="modal" data-target="#GroupModal" CssClass="btn" Text="Add Item" Style="float: right; width: 100px; margin-top: 10px; margin-bottom: 10px; padding: 5px 5px;" />
                                </div>
                                <br />
                                <div class="flex-container">
                                    <asp:GridView ID="grpitem_grdview" runat="server" Width="100%" EmptyDataText="No records has been added." OnRowDataBound="grpitem_grdview_RowDataBound" OnRowUpdating="grpitem_grdview_RowUpdating" OnRowCancelingEdit="grpitem_grdview_RowCancelingEdit" OnRowEditing="grpitem_grdview_RowEditing" OnRowDeleting="grpitem_grdview_RowDeleting" AutoGenerateColumns="false" GridLines="None" DataKeyNames="ids" CssClass="footableThis GridStyle Hoverable" OnRowCommand="grpitem_grdview_RowCommand">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemStyle Width="70px" />
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDelete" runat="server" Text="Delete" CommandName="Delete"
                                                        ToolTip="Delete" OnClientClick='return confirm("Are you sure you want to delete this Item?");'
                                                        CommandArgument='<%# Bind("Ids") %>'><i class="glyphicon glyphicon-trash lnkbtn" ></i></asp:LinkButton>
                                                    &nbsp;&nbsp;
                                                            <asp:LinkButton ID="LinkButton1" runat="server" Text="" CommandName="ItemEdit" ToolTip="Edit"
                                                                CommandArgument='<%# Bind("Ids") %>'><i class="glyphicon glyphicon-pencil lnkbtn"></i></asp:LinkButton>


                                                </ItemTemplate>

                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Width="120px">
                                                <ItemTemplate>
                                                    <img id="imgitem" runat="server" src='<%# Eval("FilePath") %>' width="110" height="110" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Item Code" ItemStyle-Width="150px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblitemcode" runat="server" Text='<%# Eval("ItemCode") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Description" ItemStyle-Width="170px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblitemname" runat="server" Text='<%# Eval("shortdesc") %>' ToolTip='<%# Eval("LongDesc") %>'></asp:Label>
                                                    <span>...</span>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Shown Status" ItemStyle-Width="120px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblShownstatus" runat="server" Text='<%# Eval("daterangeallow") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="From Date" ItemStyle-Width="160px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblstartdate" runat="server" Text='<%# Eval("fdate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="To Date" ItemStyle-Width="160px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblenddate" runat="server" Text='<%# Eval("tdate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Status" ItemStyle-Width="120px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblstatus" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <RowStyle BackColor="White" />
                                        <SelectedRowStyle BackColor="#444444" Font-Bold="True" ForeColor="White" Height="45px" />
                                        <HeaderStyle BackColor="#444444" ForeColor="White" Height="45px" />
                                        <EditRowStyle BackColor="#999999" Height="45px" />
                                        <AlternatingRowStyle BackColor="White" Height="45px" />
                                    </asp:GridView>
                                </div>

                                  <div class="modal fade" id="GroupItemEditModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
                                                                <div class="modal-dialog" role="document">
                                                                    <div class="modal-content" style="width: auto">
                                                                        <div class="modal-header">
                                                                            <h5 class="modal-title" id="examplegModalLabel" style="font-size: large; color: black; font-weight: bold">Group Item Edit  
                                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                                    <span aria-hidden="true">&times;</span>
                                                </button>
                                                                            </h5>

                                                                        </div>
                                                                        <div class="modal-body flex-container">
                                                                            <asp:UpdatePanel runat="server">
                                                                                <ContentTemplate>


                                                                                    <div class="col-xs-4 col-lg-4 col-md-4" style="margin-bottom: 5px;">
                                                                                        <asp:Image runat="server" ID="btnimage" Text="edit" class="imageMiddle" CssClass="imgSize" Style="border-radius: 5px 5px 5px 5px;" />
                                                                                        <br />
                                                                                        <asp:Label ID="lblitem_code" runat="server" Style="font-weight: bold; font-size: medium" Visible="false"></asp:Label>
                                                                                        <asp:Label ID="lblitem_name" runat="server" Style="font-weight: bold; font-size: medium"></asp:Label>
                                                                                        <asp:HiddenField ID="hditemids" runat="server" />
                                                                                    </div>
                                                                                    <div class="col-sm-8 col-xs-8 col-md-8 col-lg-8 goleft" style="color: Black; padding-left: 5px; padding-right: 5px;">
                                                                                        <div class="has-success">
                                                                                            <label id="Label1" runat="server" class="col-sm-7 control-label col-lg-7">Shown Status</label>
                                                                                            <div class="form-group col-sm-5">
                                                                                                <asp:DropDownList ID="ddlShownstatus" CssClass="form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlShownstatus_SelectedIndexChanged">
                                                                                                    <asp:ListItem Value="Always Show" Text="Always Show"></asp:ListItem>
                                                                                                    <asp:ListItem Value="Date Range" Text="Date Range"></asp:ListItem>
                                                                                                </asp:DropDownList>
                                                                                            </div>
                                                                                            <br />
                                                                                            <label id="Label10" runat="server" class="col-sm-7 control-label col-lg-7">From Date <span id="spprofit" runat="server"></span></label>
                                                                                            <div class="form-group col-sm-5">
                                                                                                <asp:TextBox ID="txtstartdateEdit" runat="server" CssClass="form-control" TextMode="date" Text='<%# Eval("fdate","{0:yyyy-MM-dd}") %>'></asp:TextBox>
                                                                                            </div>
                                                                                            <br />
                                                                                            <label id="Label17" runat="server" class="col-sm-7 control-label col-lg-7">To Date</label>
                                                                                            <div class="form-group col-sm-5">
                                                                                                <asp:TextBox ID="txtenddateEdit" runat="server" CssClass="form-control" TextMode="date" Text='<%# Eval("tdate","{0:yyyy-MM-dd}") %>'></asp:TextBox>
                                                                                            </div>
                                                                                            <br />
                                                                                            <label id="Label13" runat="server" class="col-sm-7 control-label col-lg-7">Status</label>
                                                                                            <div class="form-group col-sm-5">
                                                                                                <asp:DropDownList ID="ddlstatusEdit" CssClass="form-control" runat="server">
                                                                                                    <asp:ListItem Value="Active" Text="Active"></asp:ListItem>
                                                                                                    <asp:ListItem Value="Inactive" Text="Inactive"></asp:ListItem>
                                                                                                </asp:DropDownList>
                                                                                            </div>
                                                                                            <br />
                                                                                        </div>
                                                                                    </div>
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>
                                                                        </div>
                                                                        <div class="modal-footer">
                                                                            <asp:UpdatePanel runat="server">
                                                                                <ContentTemplate>
                                                                                    <asp:Button ID="btn_updateitem" runat="server" Text="Update" OnClick="btn_updateitem_Click" CssClass="ghost-button-thick-border1" Style="font-weight: bold; padding: 5px 10px 5px 10px;" CausesValidation="false" />
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>

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
                            <div class="modal fade" id="GroupModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
                                <div class="modal-dialog model-lx" role="document" style="width: 100% !important; height: 100% !important">
                                    <div class="modal-content" style="width: auto; margin-top: 0px !important">
                                        <div class="modal-header">
                                            <h5 class="modal-title" id="exampleModalLabel" style="font-size: large!important">Item List  
                                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                            <span aria-hidden="true">&times;</span>
                                        </button>
                                            </h5>

                                        </div>
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>
                                                <div class="modal-body">

                                                    <label id="lblhotid" runat="server" visible="false"></label>

                                                    <asp:TextBox ID="txt_search" runat="server" placeholder="Filter By Item code/ Category Code / Description" Style="border: 2; height: 34px; padding: 6px 12px; font-size: 14px;" CssClass="col-md-8 col-xs-8"></asp:TextBox>
                                                    <asp:Button ID="btnsearch" Text="Search" OnClick="btnsearch_Click" runat="server" CssClass="btn" Style="padding: 6px; width: 100px; margin-left: 5px; text-align: center; text-decoration: none; display: inline-block; cursor: pointer;" />
                                                    <br />
                                                    <br />
                                                    <div style="overflow: auto; height: 400px;">
                                                        <asp:GridView ID="item_grdview" runat="server" Width="100%" AutoGenerateColumns="false" AllowPaging="true" OnPageIndexChanging="item_grdview_PageIndexChanging" OnRowCommand="item_grdview_RowCommand" CssClass="footableThis GridStyle Hoverable popoutGrid">
                                                            <Columns>
                                                                <asp:TemplateField ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkitem" runat="server" />
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
                                                                <asp:TemplateField HeaderText="Shown Status" ItemStyle-Width="120px">
                                                                    <ItemTemplate>
                                                                        <asp:DropDownList ID="ddlShownstatus" CssClass="form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlShownstatus_SelectedIndexChanged1">
                                                                            <asp:ListItem Value="Always Show" Text="Always Show"></asp:ListItem>
                                                                            <asp:ListItem Value="Date Range" Text="Date Range"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Start Date">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtstartdate" runat="server" Enabled="false" CssClass="form-control" TextMode="Date"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="End Date">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtenddate" runat="server" Enabled="false" CssClass="form-control" TextMode="Date"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Status">
                                                                    <ItemTemplate>
                                                                        <asp:DropDownList ID="ddlstatus" runat="server" CssClass="form-control">
                                                                            <asp:ListItem Value="Active" Text="Active"></asp:ListItem>
                                                                            <asp:ListItem Value="Inactive" Text="Inactive"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>

                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>

                                        <div class="modal-footer">
                                            <asp:Button ID="btnaddtlist" runat="server" Text="Add" OnClick="btnaddtlist_Click" CssClass="btn btn-success" />
                                        </div>
                                    </div>
                                </div>

                            </div>
                            <!--end modal-->

                           

                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

