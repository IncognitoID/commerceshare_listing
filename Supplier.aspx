<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Supplier.aspx.cs" Inherits="Supplier" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtoolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <section id="main-content">
        <section class="wrapper">
            <asp:HiddenField ID="PaneName" runat="server" />
            <asp:Label ID="MF_Con" runat="server" Text="Label" Visible="false"></asp:Label>
            <asp:Label ID="lblrunno" runat="server" Visible="false"></asp:Label>

            <asp:LinkButton ID="btn_Add" runat="server" OnClick="Back_OnClick" style="float:right;"><i class="glyphicon fa-2x glyphicon-circle-arrow-left addItem" style="color: #e10808"></i></asp:LinkButton>
            <div class="col-md-12 col-sm-12" style="color:Black">

                 <asp:Panel ID="pnlDetails" runat="server" Visible="true">
        <div class="col-md-12 col-sm-12" style="color:Black">
          <div class="panel-group" id="Item_Page">
            <div class="panel panel-default">

              <div class="panel-heading">
                <h4 class="panel-title">Supplier Details</h4>
              </div>

              <div class="panel-body">

                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        
                                        <div class="has-success">
                                            <label class="col-sm-10 control-label col-lg-4">Supplier Code</label>
                                            <div class="form-group col-sm-8">      
                                                <asp:HiddenField ID="hf_OriginalSupplierCode" runat="server" />
                                                <asp:TextBox ID="txt_SupplierCode" Enabled="false" runat="server"
                                                    AutoComplete="off" MaxLength="25" placeholder="SQ-0001"
                                                    CssClass="col-md-12 col-xs-12"
                                                    style="border:2; height:34px; padding:6px 12px; font-size:14px;">
                                                </asp:TextBox>
                                            </div>
                                        </div>

                                        <br />

                                        <div class="has-success">
                                            <label class="col-sm-10 control-label col-lg-4">Supplier Name</label>
                                            <div class="form-group col-sm-8">
                                                <asp:TextBox ID="txt_SupplierName" runat="server" AutoComplete="off"
                                                    MaxLength="100" placeholder="Example : SOFT DRINKS"
                                                    CssClass="col-md-12 col-xs-12"
                                                    style="border:2; height:34px; padding:6px 12px; font-size:14px;">
                                                </asp:TextBox>
                                            </div>
                                        </div>

                                        <br />

                                        <div class="has-success">
                                            <label class="col-sm-10 control-label col-lg-4">Contact No</label>
                                            <div class="form-group col-sm-8">
                                                <asp:TextBox ID="txt_ContactNo" runat="server" AutoComplete="off"
                                                    MaxLength="50" placeholder="Example : +60123456789"
                                                    CssClass="col-md-12 col-xs-12"
                                                    style="border:2; height:34px; padding:6px 12px; font-size:14px;">
                                                </asp:TextBox>
                                            </div>
                                        </div>

                                        <br />

                                        <div class="has-success">
                                            <label class="col-sm-10 control-label col-lg-4">Email</label>
                                            <div class="form-group col-sm-8">
                                                <asp:TextBox ID="txt_Email" runat="server" AutoComplete="off"
                                                    MaxLength="50" placeholder="Example : supplier@email.com"
                                                    CssClass="col-md-12 col-xs-12"
                                                    style="border:2; height:34px; padding:6px 12px; font-size:14px;">
                                                </asp:TextBox>
                                                <asp:RegularExpressionValidator ID="RegexEmail" runat="server"
                                                    ControlToValidate="txt_Email"
                                                    ErrorMessage="Invalid Email Format"
                                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                    ForeColor="Red" Display="Dynamic" />
                                            </div>
                                        </div>

                                        <br />

                                        <div class="has-success">
                                            <label class="col-sm-10 control-label col-lg-4">Supplier TIN No (Optional)</label>
                                            <div class="form-group col-sm-8">
                                                <asp:TextBox ID="txt_TIN" runat="server" AutoComplete="off"
                                                    MaxLength="255" placeholder="Example : 1234567890"
                                                    CssClass="col-md-12 col-xs-12"
                                                    style="border:2; height:34px; padding:6px 12px; font-size:14px;">
                                                </asp:TextBox>
                                            </div>
                                        </div>

                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>

                    </div>
                </div>
                    </asp:Panel>

                <div style="width:100%;">
                <asp:Button ID="btn_Back" runat="server" Text="Back" OnClick="Back_OnClick" CssClass="ghost-button-thick-border" style="float:right" Visible="true"/>
                    <asp:Button ID="btn_Insert" runat="server" Text="Insert" OnClick="Insert_OnClick" CssClass="ghost-button-thick-border" />
                    <asp:Button ID="btn_Update" runat="server" Text="Update" OnClick="Update_OnClick" CssClass="ghost-button-thick-border" Visible="false"/>
                </div>
            </div>
        </section>
    </section>
</asp:Content>

