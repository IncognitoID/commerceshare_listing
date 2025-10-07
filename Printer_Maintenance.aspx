<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Printer_Maintenance.aspx.cs" Inherits="Printer_Maintenance" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtoolkit" %>
<%@ Register Assembly="msgBox" Namespace="BunnyBear" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script>
        function IsNumberWithOneDecimal(txt, evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57) && !(charCode == 46 || charCode == 8)) {
                return false;
            } else {
                var len = txt.value.length;
                var index = txt.value.indexOf('.');
                if (index > 0 && charCode == 46) {
                    return true;
                }
            }
            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <cc1:msgBox ID="MsgBox1" runat="server"></cc1:msgBox>
    <asp:HiddenField ID="PaneName" runat="server" />
    <asp:HiddenField ID="PaneNameS" runat="server" />
    <div class="col-md-12 col-sm-12" style="color: Black">

        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Label ID="lblrunno" runat="server" Visible="false"></asp:Label>
                <asp:Label ID="MF_Con" runat="server" Text="Label" Visible="false"></asp:Label>
                <asp:LinkButton ID="btn_Add" runat="server" OnClick="Back_OnClick" Style="float: right;"><i class="glyphicon fa-2x glyphicon-circle-arrow-left addItem" style="color: #e10808"></i></asp:LinkButton>
                <div class="panel-group" id="Item_Page">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h4 class="panel-title">
                                <a data-toggle="collapse" data-parent="#accordion" href="#collapse1">Printer Details</a>
                            </h4>
                        </div>
                        <div id="collapse1" class="panel-collapse">
                            <div class="panel-body">
                                <div style="height: 30px"></div>
                                <div class="has-success">
                                    <label class="col-sm-10 control-label col-lg-4">Printer Name</label>
                                    <div class="form-group col-sm-8">
                                        <asp:TextBox ID="txt_printerName" runat="server" AutoComplete="off" placeholder="Printer Name" MaxLength="15" CssClass="col-md-12 col-xs-12" Style="border: 2; height: 34px; padding: 6px 12px; font-size: 14px;"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txt_printerName" ForeColor = "Red"
                                            ValidationExpression="[a-zA-Z0-9\s]*$" ErrorMessage="*Valid characters: Alphabets and Numbers." />
                                    </div>
                                </div>
                                <br />
                                <br />
                                <div class="has-success">
                                    <label class="col-sm-10 control-label col-lg-4">IP Address</label>
                                    <div class="form-group col-sm-8">
                                        <asp:TextBox ID="txt_IPAddress" runat="server" onkeypress="return IsNumberWithOneDecimal(this,event);" AutoComplete="off" MaxLength="30" placeholder="IP Address" CssClass="col-md-12 col-xs-12" Style="border: 2; height: 34px; padding: 6px 12px; font-size: 14px;"></asp:TextBox>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
                <br />

                <div style="width: 100%;">
                    <asp:Button ID="btn_Back" runat="server" Text="Back" OnClick="Back_OnClick" CssClass="ghost-button-thick-border" Style="float: right" Visible="true" />
                    <asp:Button ID="btn_Insert" runat="server" Text="Insert" OnClick="btn_Insert_Click" CssClass="ghost-button-thick-border" />
                    <asp:Button ID="btn_Update" runat="server" Text="Update" OnClick="btn_Update_Click" CssClass="ghost-button-thick-border" Visible="false" />
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

