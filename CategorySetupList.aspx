<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CategorySetupList.aspx.cs" Inherits="CategorySetupList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <style>
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
    <section id="main-content">
        <section class="wrapper">
            <asp:HiddenField ID="PaneName" runat="server" />
            <div class="col-md-12 col-sm-12" style="color:Black">
            <asp:Label ID="MF_Con" runat="server" Text="Label" Visible="false"></asp:Label>
            <asp:Label ID="lblCatCode" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="lblCatID" runat="server" Visible="false"></asp:Label>
                <div style="padding-bottom: 10px; height: 50px; margin-top: 10px;">
                <h2 style="font-size: x-large; font-weight: bold; margin-top: 13px; margin-bottom: 0px; width: 40%; float: left;">Category Listing</h2>
                <asp:TextBox ID="txt_Search" runat="server" AutoComplete="off" MaxLength="50" placeholder="Filter by category code / description"  AutoPostBack="true" OnTextChanged="txt_Search_TextChanged" CssClass="col-md-5 col-sm-5 txt_Search" style="display: inline-block;font-weight: bold;padding: 8px; padding-left:35px; text-align: left;outline: none;text-decoration: none; float:right"></asp:TextBox>
                <br />
                   </div>
<%--                <asp:LinkButton ID="btn_Add" runat="server" OnClick="btn_New_Click" style="float:right;"><i class="glyphicon fa-2x glyphicon-plus-sign addItem" style="color: #4c8262"></i></asp:LinkButton>--%>
                
                <div style="padding-top: 12px;padding-left:12px;padding-bottom:12px; margin-bottom: 5px;">
                    <asp:DropDownList ID="ddlPager" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPager_SelectedIndexChanged" Style="float: left; display: inline-block; width: 90px; height: 20px; font-weight: bold; text-align: center; text-decoration: none; border: none;"></asp:DropDownList>
                    &nbsp&nbsp<asp:Label ID="lbl_Record" runat="server" Text="No. Of Record: "></asp:Label><span><asp:Label ID="lbl_record3" runat="server"></asp:Label> of <asp:Label ID="lbl_Record2" runat="server"></asp:Label></span>
                    <asp:Button ID="Button1" runat="server" Text="New" OnClick="btn_New_Click" CssClass="ghost-button-thick-border1" />
                </div>
                <asp:GridView ID="grd_View" ShowFooter="false" runat="server" AutoGenerateColumns="False" DataKeyNames="Category_ID"
                    OnRowCommand="View_RowCommand" GridLines="Horizontal" OnRowDataBound="View_RowDataBound"
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
                                            <li><asp:LinkButton ID="lnkedit" Text="Edit" runat="server" CommandArgument='<%# Eval("Category_ID") %>' CommandName="edit"></asp:LinkButton></li>
                                            <li><asp:LinkButton ID="lnkdel" Text="Delete" runat="server" CommandArgument='<%# Eval("Category_Code") %>' CommandName="del" OnClientClick="Confirm()"></asp:LinkButton></li>
                                        </ul>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Category" SortExpression="Category_Description">
                            <ItemTemplate>
                               <asp:Label ID="lblCatCode" runat="server" style="font-weight:bold" Text='<%# Eval("Category_Code") %>'></asp:Label>
                               <div style="height: -3px"></div>
                               <asp:Label ID="lblCatName" runat="server" Text='<%# Eval("Category_Description") %>'></asp:Label>
                               <br />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Department_Code" HeaderText="Department Code" SortExpression="Department_Code" />
                        <asp:BoundField DataField="Department_Description" HeaderText="Department Desc." SortExpression="Department_Description"/>
                    </Columns>
                    <RowStyle BackColor="White"  />
                    <SelectedRowStyle BackColor="#e7e7e7" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#696969" Font-Bold="True" Height="45px" ForeColor="White" CssClass="grdview_header" />
                    <EditRowStyle BackColor="#696969" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
                <br />
            </div>
        </section><!-- wrapper -->
    </section><!-- MAIN CONTENT -->
</asp:Content>

