<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="BrandSetupList.aspx.cs" Inherits="BrandSetupList" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtoolkit" %>
<%@ Register Assembly="msgBox" Namespace="BunnyBear" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <style>
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

        .btn {
            background-color: #4CAF50;
            border: none;
            color: white;
            padding: 15px 32px;
            text-align: center;
            text-decoration: none;
            display: inline-block;
            font-size: 16px;
            margin: 4px 2px;
            cursor: pointer;
        }

            .btn:hover {
                background-color: #48a34c;
            }
.txt_Search
        {
            background: url(Images/search2.png) no-repeat;
            padding-left: 18px;
            margin-top: 10px;
            border:1px solid #ccc;
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
        .grdview_header > thead > tr > th{
           padding:10px 2px 10px 2px;
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


    </script>
    <cc1:msgbox id="MsgBox1" runat="server"></cc1:msgbox>
    <section id="main-content">
        <section class="wrapper">
            <asp:HiddenField ID="PaneName" runat="server" />
            <div class="col-md-12 col-sm-12" style="color:Black">
            <asp:Label ID="MF_Con" runat="server" Text="Label" Visible="false"></asp:Label>
<%--            <asp:LinkButton ID="btn_Add" runat="server" OnClick="btn_New_Click " style="float:right;"><i class="glyphicon fa-2x glyphicon-plus-sign addItem" style="color: #4c8262"></i></asp:LinkButton>--%>
                <div style="padding-bottom: 10px; height: 50px; margin-top: 10px;">
                    <h2 style="font-size: x-large; font-weight: bold; margin-top: 13px; margin-bottom: 0px; width: 40%; float: left;">Brand Listing</h2>
                <asp:TextBox ID="txt_Search" runat="server" AutoComplete="off" MaxLength="50" placeholder="Filter by brand code / description" AutoPostBack="true" OnTextChanged="txt_Search_TextChanged" CssClass="col-md-5 col-sm-5 txt_Search" style="display: inline-block;font-weight: bold;padding: 8px; padding-left: 35px; text-align: left;outline: none;text-decoration: none;float:right;"></asp:TextBox>
                <br /><br /><br />
                    </div>
                 <div style="padding-top: 12px;padding-left:12px;padding-bottom:12px; margin-bottom: 5px;">
                    <asp:DropDownList ID="ddlPager" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPager_SelectedIndexChanged" Style="float: left; display: inline-block; width: 90px; height: 20px; font-weight: bold; text-align: center; text-decoration: none; border: none;"></asp:DropDownList>
                    &nbsp&nbsp<asp:Label ID="lbl_Record" runat="server" Text="No. Of Record: "></asp:Label><span><asp:Label ID="lbl_record3" runat="server"></asp:Label> of <asp:Label ID="lbl_Record2" runat="server"></asp:Label></span>
                    <asp:Button ID="btn_New" runat="server" Text="New" OnClick="btn_New_Click" CssClass="ghost-button-thick-border1" />
                </div>
                <asp:GridView ID="grd_View" ShowFooter="false" runat="server" AutoGenerateColumns="False" DataKeyNames="ID"
                    OnRowCommand="View_RowCommand" GridLines="Horizontal" OnRowDataBound="View_RowDataBound"
                    CssClass="footableThis" ForeColor="#333333" BorderStyle="double" BorderColor="#bbbbbb">
                    <Columns>
                        <asp:TemplateField>
                            <ItemStyle Width="35px" Height="50px" />
                            <ItemTemplate>
                                 <div class="dropdown">
                                        <p class="dropdown-toggle" data-toggle="dropdown">
                                            <a class="glyphicon glyphicon-option-vertical" style="font-size:20pt;cursor:pointer; text-decoration:none;"></a>
                                        </p>
                                        <ul class="dropdown-menu">
                                            <li><asp:LinkButton ID="lnkedit" Text="Edit" runat="server" CommandArgument='<%# Eval("ID") %>' CommandName="edit"></asp:LinkButton></li>
                                            <li><asp:LinkButton ID="lnkdel" Text="Delete" runat="server" CommandArgument='<%# Eval("Brand_Code") %>' CommandName="del" OnClientClick="Confirm()"></asp:LinkButton></li>
                                        </ul>
                                     </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Department">
                            <ItemStyle Height="50px" />
                            <ItemTemplate>
                               <asp:Label ID="lblbrdCode" runat="server" Font-Bold="true" Text='<%# Eval("Brand_Code") %>'></asp:Label>
                               <br />
                               <asp:Label ID="lblbrdDesc" runat="server" Text='<%# Eval("Brand_Desc") %>'></asp:Label>
                               <br />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Supplier_code" HeaderText="Merchant ID" />
                    </Columns>
                    <RowStyle BackColor="White" Height="50px" />
                    <SelectedRowStyle BackColor="#e7e7e7" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#696969" Font-Bold="True"  Height="45px" ForeColor="White" />
                    <EditRowStyle BackColor="#696969" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
                <br />
                <%--<asp:DropDownList ID="ddlPager" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPager_SelectedIndexChanged" style="float:left;display: inline-block;width: 100px; height: 42px;font-weight: bold;padding: 8px;text-align: center;outline: none;text-decoration: none;"></asp:DropDownList>
                <asp:Button ID="btn_New" runat="server" Text="New" OnClick="btn_New_Click " CssClass="ghost-button-thick-border" />
                <br />
                <div style="height: 30px"></div>
                <asp:Label ID="lbl_Record" runat="server" Text="No. of record: "></asp:Label>
                <asp:Label ID="lbl_Record2" runat="server"></asp:Label>--%>
                
            </div>
        </section><!-- wrapper -->
    </section><!-- MAIN CONTENT -->
</asp:Content>

