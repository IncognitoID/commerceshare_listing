<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PromoCampaign.aspx.cs" Inherits="PromoCampaign" %>

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

        .clear {
            clear: both;
        }

        .flex-container {
            display: flex;
            /*flex-direction: column;*/
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
        $(function () {
            $('#txtstartDate').datetimepicker();
        });

    </script>
    <section id="main-content">
        <section class="wrapper">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
            <asp:HiddenField ID="PaneName" runat="server" />
            <div class="col-md-12 col-sm-12" style="color:Black">
            <asp:Label ID="MF_Con" runat="server" Text="Label" Visible="false"></asp:Label>
            <asp:Label ID="lblCatCode" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="lblCatID" runat="server" Visible="false"></asp:Label>
                <div style="padding-bottom: 10px; height: 50px; margin-top: 10px;">
                <h2 style="font-size: x-large; font-weight: bold; margin-top: 13px; margin-bottom: 0px; width: 40%; float: left;">Promotion Campaign Listing</h2>
                <asp:TextBox ID="txt_Search" runat="server" AutoComplete="off" MaxLength="50" placeholder="Filter by Campaign Title"  AutoPostBack="true" OnTextChanged="txt_Search_TextChanged" CssClass="col-md-5 col-sm-5 txt_Search" style="display: inline-block;font-weight: bold;padding: 8px; padding-left:35px; text-align: left;outline: none;text-decoration: none; float:right"></asp:TextBox>
                <br />
                   </div>
<%--                <asp:LinkButton ID="btn_Add" runat="server" OnClick="btn_New_Click" style="float:right;"><i class="glyphicon fa-2x glyphicon-plus-sign addItem" style="color: #4c8262"></i></asp:LinkButton>--%>
                
                <div style="padding-top: 12px;padding-left:12px;padding-bottom:12px; margin-bottom: 5px;">
                    <asp:DropDownList ID="ddlPager" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPager_SelectedIndexChanged" Style="float: left; display: inline-block; width: 90px; height: 20px; font-weight: bold; text-align: center; text-decoration: none; border: none;"></asp:DropDownList>
                    &nbsp&nbsp<asp:Label ID="lbl_Record" runat="server" Text="No. Of Record: "></asp:Label><span><asp:Label ID="lbl_record3" runat="server"></asp:Label> of <asp:Label ID="lbl_Record2" runat="server"></asp:Label></span>
                    <asp:Button ID="btn_New" runat="server" Text="New" OnClick="btn_New_Click" CssClass="ghost-button-thick-border1" data-toggle="modal" data-target="#GroupModal" />
                </div>
                <asp:GridView ID="grd_View" ShowFooter="false" runat="server" AutoGenerateColumns="False" DataKeyNames="ids"
                    OnRowCommand="View_RowCommand" GridLines="Horizontal" OnRowDataBound="grd_View_RowDataBound"
                    CssClass="footableThis" ForeColor="#333333" BorderStyle="double" BorderColor="#bbbbbb" >
                    <Columns>
                        <asp:TemplateField>
                            <ItemStyle Width="40px" />
                            <ItemTemplate>
                                <div class="dropdown">
                                        <p class="dropdown-toggle" data-toggle="dropdown">
                                            <a class="glyphicon glyphicon-option-vertical" style="font-size:20pt;pointer-events:auto;"></a>
                                        </p>
                                        <ul class="dropdown-menu">
                                            <li><asp:LinkButton ID="lnkupdate" Text="Update" runat="server" CommandArgument='<%# Eval("ids") %>' CommandName="Update"></asp:LinkButton></li>
                                            <li><asp:LinkButton ID="lnkdel" Text="Delete" runat="server" CommandArgument='<%# Eval("ids") %>' CommandName="delete" OnClientClick="Confirm()"></asp:LinkButton></li>
                                        </ul>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Campaign Title" SortExpression="grouptitle">
                            <ItemTemplate>
                               <asp:Label ID="lblgrouptitle" runat="server" style="font-weight:bold" Text='<%# Eval("CampaignTitle") %>'></asp:Label>
                               <br />
                            </ItemTemplate>
                        </asp:TemplateField>
                             <asp:TemplateField HeaderText="Start Date" SortExpression="Start_Date">
                            <ItemTemplate>
                               <asp:Label ID="lblstartdate" runat="server" style="font-weight:bold" Text='<%# Eval("Start_Date") %>'></asp:Label>
                               <br />
                            </ItemTemplate>
                        </asp:TemplateField>
                             <asp:TemplateField HeaderText="End Date" SortExpression="End_Date">
                            <ItemTemplate>
                               <asp:Label ID="lblenddate" runat="server" style="font-weight:bold" Text='<%# Eval("End_Date") %>'></asp:Label>
                               <br />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <RowStyle BackColor="White"  />
                    <SelectedRowStyle BackColor="#e7e7e7" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#696969" Font-Bold="True" Height="45px" ForeColor="White" CssClass="grdview_header" />
                    <EditRowStyle BackColor="#696969" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
                <br />
            </div>
 </ContentTemplate>
            </asp:UpdatePanel>
             <div class="modal fade" id="GroupModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content" style="width:auto">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel" style="font-size:large;  color:black;font-weight:bold">Add Campaign   <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button></h5>
                  
                </div>
                <div class="modal-body" style="display:inline-block;width:100%">
                    <label class="col-sm-3 control-label col-lg-4">Campaign Title</label>
                    <div class="form-group col-sm-9">
                        <input ID="txt_Campaigntitle" runat="server" placeholder="" class="col-md-12 col-xs-12" style="border: 2; height: 34px; padding: 6px 12px; font-size: 14px;"/>
                    </div>
                    <br />
                        <label class="col-sm-3 control-label col-lg-4">Start Date</label>
                    <div class="form-group col-sm-9">
                        <asp:TextBox ID="txtstartDate" runat="server" TextMode="DateTimeLocal" class="col-md-12 col-xs-12" style="border: 2; height: 34px; padding: 6px 12px; font-size: 14px;"></asp:TextBox>
                    </div>
                    <br />
                        <label class="col-sm-3 control-label col-lg-4">End Date</label>
                    <div class="form-group col-sm-9">
                        <asp:TextBox ID="txtendDate" runat="server" TextMode="DateTimeLocal" class="col-md-12 col-xs-12" style="border: 2; height: 34px; padding: 6px 12px; font-size: 14px;"></asp:TextBox>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <asp:Button ID="btn_add" runat="server" Text="Add" OnClick="btn_add_Click" CssClass="ghost-button-thick-border1" style="font-weight:bold;padding:5px 10px 5px 10px;" CausesValidation="false" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    
                </div>
            </div>
        </div>
    </div>

        </section><!-- wrapper -->
    </section>
    <!-- MAIN CONTENT -->
</asp:Content>

