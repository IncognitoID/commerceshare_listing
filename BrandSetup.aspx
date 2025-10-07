<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="BrandSetup.aspx.cs" Inherits="BrandSetup" %>
<%@ Register Assembly="msgBox" Namespace="BunnyBear" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
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
                <div class="panel-group" id="Item_Page" >
                    <div class="panel panel-default">
                      
                            
                        <div class="panel-heading" >
                            <h4 class="panel-title">
                                <a data-toggle="collapse" data-parent="#accordion" href="#collapse3" > Brand Details</a>
                            </h4>
                        </div>
                        <div id="collapse3" class="panel-collapse collapse">
                            <div class="panel-body">

                             <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>

                                       <div class="has-success">
                                            <label class="col-sm-10 control-label col-lg-4" >Brand Code</label>
                                            <div class="form-group col-sm-8">
                                                <asp:TextBox ID="txt_BrdCode" Enabled="false" runat="server" AutoComplete="off" MaxLength="25" placeholder="Example : SD001" CssClass="col-md-12 col-xs-12" style="border:2; height:34px; padding:6px 12px; font-size:14px;"></asp:TextBox>
                                                <ajaxtoolkit:filteredtextboxextender ID="FilteredTextBoxExtender1" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Numbers, Custom" ValidChars=",.'()`+-*/& " TargetControlID="txt_BrdCode" />
                                            </div>
                                        </div>

                                <br />
                                <div style="height: 30px" ></div>
                                <div class="has-success">
                                    <label class="col-sm-10 control-label col-lg-4" >Brand Description</label>
                                    <div class="form-group col-sm-8">
                                        <asp:TextBox ID="txt_BrdDesc" runat="server" AutoComplete="off" MaxLength="100" placeholder="Example : F&N" CssClass="col-md-12 col-xs-12" style="border:2; height:34px; padding:6px 12px; font-size:14px;"></asp:TextBox>                                        
                                    </div>
                                </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            </div>
                    </div>
                </div>
                <div style="width:100%;">
                <asp:Button ID="btn_Back" runat="server" Text="Back" OnClick="Back_OnClick" CssClass="ghost-button-thick-border" style="float:right" Visible="true"/>
                    <asp:Button ID="btn_Insert" runat="server" Text="Insert" OnClick="Insert_OnClick" CssClass="ghost-button-thick-border" />
                    <asp:Button ID="btn_Update" runat="server" Text="Update" OnClick="Update_OnClick" CssClass="ghost-button-thick-border" Visible="false"/>
                </div>
            </div>
        </section>
    </section>
</asp:Content>

