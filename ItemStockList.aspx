<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="ItemStockList.aspx.cs" Inherits="ItemStockList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="https://code.jquery.com/jquery-3.3.1.slim.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.7/umd/popper.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js"></script>
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css" />
    <title></title>
    <style>
        body
        {
            padding:10px;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
    <section id="main-content">
        <section class="wrapper">
            <asp:HiddenField ID="PaneName" runat="server" />
            <div class="col-md-12 col-sm-12" style="color:Black">
            <asp:Label ID="MF_Con" runat="server" Text="Label" Visible="false"></asp:Label>
<%--            <asp:LinkButton ID="btn_Add" runat="server" OnClick="btn_New_Click " style="float:right;"><i class="glyphicon fa-2x glyphicon-plus-sign addItem" style="color: #4c8262"></i></asp:LinkButton>--%>
                <div style="padding-bottom: 10px; height: 50px; margin-top: 10px;">
                    <h2 style="font-size: x-large; font-weight: bold; margin-top: 13px; margin-bottom: 0px; width: 40%; float: left;">Item Stock</h2>
                <asp:TextBox ID="txt_Search" runat="server" AutoComplete="off" MaxLength="50" placeholder="Filter by Item code / description" AutoPostBack="true" OnTextChanged="txt_Search_TextChanged" CssClass="col-md-5 col-sm-5 txt_Search" style="display: inline-block;font-weight: bold;padding: 8px; padding-left: 35px; text-align: left;outline: none;text-decoration: none;float:right;"></asp:TextBox>
                <br /><br /><br />
                    </div>
                 <div style="padding-top: 12px;padding-left:12px;padding-bottom:12px; margin-bottom: 5px;">
                    <asp:DropDownList ID="ddlPager" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPager_SelectedIndexChanged" Style="float: left; display: inline-block; width: 90px; height: 20px; font-weight: bold; text-align: center; text-decoration: none; border: none;"></asp:DropDownList>
                    &nbsp&nbsp<asp:Label ID="lbl_Record" runat="server" Text="No. Of Record: "></asp:Label><span><asp:Label ID="lbl_record3" runat="server"></asp:Label> of <asp:Label ID="lbl_Record2" runat="server"></asp:Label></span>
                     <asp:Button ID="btnexport" OnClick="Button1_Click" runat="server" Text="Export" style=" background-color: #4CAF50; border: none; color: white; padding: 2px 18px; text-align: center; text-decoration: none; display: inline-block; cursor: pointer; float:right" />
                </div>
                    
                <asp:GridView ID="grd_View" ShowFooter="false" runat="server" AutoGenerateColumns="False" DataKeyNames="Item_ID"
                    GridLines="Horizontal" OnRowDataBound="View_RowDataBound" CssClass="footableThis" ForeColor="#333333" BorderStyle="none" BorderColor="#bbbbbb" Width="100%">
                    <Columns>
                        <asp:TemplateField>
                            <ItemStyle Width="70px" Height="50px" />
                            <ItemTemplate>
                               <img id="img1" runat="server" alt="" src='<%# Eval("FilePath") %>' width="64" height="64" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Item Code">
                            <ItemStyle Width="120px" Height="50px" />
                            <ItemTemplate>
                               <asp:Label ID="lblitemcode" runat="server" Text='<%# Eval("Item_Code") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Item Name">
                            <ItemStyle Height="50px" />
                            <ItemTemplate>
                                <asp:Label ID="lblitemname" runat="server" Text='<%# Eval("LongDesc") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="Member">
                            <ItemStyle Height="50px" />
                            <ItemTemplate>
                                <asp:Label ID="lblmemname" runat="server" Text='<%# Eval("MemberPrice") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="VIP">
                            <ItemStyle Height="50px" />
                            <ItemTemplate>
                                <asp:Label ID="lblvipname" runat="server" Text='<%# Eval("VIPPrice") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="VVIP">
                            <ItemStyle Height="50px" />
                            <ItemTemplate>
                                <asp:Label ID="lblvvipname" runat="server" Text='<%# Eval("VVIPPrice") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="Stockist">
                            <ItemStyle Height="50px" />
                            <ItemTemplate>
                                <asp:Label ID="lblstockistname" runat="server" Text='<%# Eval("StockistPrice") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="M.Stockist">
                            <ItemStyle Height="50px" />
                            <ItemTemplate>
                                <asp:Label ID="lblmstockistname" runat="server" Text='<%# Eval("MStockistPrice") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Status(Publish)">
                            <ItemStyle Height="50px"/>
                            <ItemTemplate>
                                <asp:Label ID="lblitemstatus" runat="server" Text='<%# Eval("Publish") %>' ForeColor='<%# Eval("Publish").ToString() == "No" ? System.Drawing.Color.Red : System.Drawing.Color.Green %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Stock_Qty" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:N0}" HeaderText="Stock QTY"/>
                        
                    </Columns>
                    <RowStyle BackColor="White" Height="50px" />
                    <SelectedRowStyle BackColor="#e7e7e7" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#4a4d5c" Font-Bold="True"  Height="45px" ForeColor="White" />
                    <EditRowStyle BackColor="#4a4d5c" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
                 <div id="dvNoRecords" runat="server" visible="false" style="padding: 20px 20px; text-align: center; color: Red;">
                        <span>No Items Record Found!</span>
                    </div>


                <br />
                <div style="display:none;">
                 <asp:GridView ID="GridView1" ShowFooter="false" runat="server" AutoGenerateColumns="False" DataKeyNames="Item_ID"
                    GridLines="Horizontal" OnRowDataBound="View_RowDataBound" CssClass="footableThis" ForeColor="#333333" BorderStyle="none" BorderColor="#bbbbbb" Width="100%">
                    <Columns>
                        <asp:TemplateField>
                            <ItemStyle Width="70px" Height="50px" />
                            <ItemTemplate>
                               <img id="img1" runat="server" alt="" src='<%# Eval("FilePath") %>' width="64" height="64" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Item Code">
                            <ItemStyle Width="120px" Height="50px" />
                            <ItemTemplate>
                               <asp:Label ID="lblitemcode" runat="server" Text='<%# Eval("Item_Code") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Item Name">
                            <ItemStyle Height="50px" />
                            <ItemTemplate>
                                <asp:Label ID="lblitemname" runat="server" Text='<%# Eval("LongDesc") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Status(Publish)">
                            <ItemStyle Height="50px"/>
                            <ItemTemplate>
                                <asp:Label ID="lblitemstatus" runat="server" Text='<%# Eval("Publish") %>' ForeColor='<%# Eval("Publish").ToString() == "No" ? System.Drawing.Color.Red : System.Drawing.Color.Green %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Stock_Qty" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:N0}" HeaderText="Stock QTY"/>
                        
                    </Columns>
                    <RowStyle BackColor="White" Height="50px" />
                    <SelectedRowStyle BackColor="#e7e7e7" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#4a4d5c" Font-Bold="True"  Height="45px" ForeColor="White" />
                    <EditRowStyle BackColor="#4a4d5c" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
                    </div>
            </div>
        </section><!-- wrapper -->
    </section><!-- MAIN CONTENT -->
         </form>
</body>
</html>


