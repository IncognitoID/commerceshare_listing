<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CategorySetup.aspx.cs" Inherits="CategorySetup" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtoolkit" %>
<%@ Register Assembly="msgBox" Namespace="BunnyBear" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script>
        function showimagepreview() {
            var fileName = document.getElementById('<%= fileupload2.ClientID %>').value;
            var ext = fileName.substring(fileName.lastIndexOf('.') + 1);
            if (ext == "gif" || ext == "GIF" || ext == "PNG" || ext == "png" || ext == "jpg" || ext == "JPG" || ext == "bmp" || ext == "BMP" || ext == "jpeg" || ext == "JPEG") {
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
            else {
                document.getElementById('<%= fileupload2.ClientID %>').value = "";
                alert("Please upload a valid image file.");
                return false;
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <cc1:msgBox ID="MsgBox1" runat="server"></cc1:msgBox>
    <section id="main-content">
        <section class="wrapper">
            <asp:HiddenField ID="PaneName" runat="server" />
            <asp:HiddenField ID="PaneNameS" runat="server" />
            <div class="col-md-12 col-sm-12" style="color: Black">
                <asp:Label ID="lblrunno" runat="server" Visible="false"></asp:Label>
                <asp:Label ID="MF_Con" runat="server" Text="Label" Visible="false"></asp:Label>
                <asp:LinkButton ID="btn_Add" runat="server" OnClick="Back_OnClick" Style="float: right;"><i class="glyphicon fa-2x glyphicon-circle-arrow-left addItem" style="color: #e10808"></i></asp:LinkButton>
                <div class="panel-group" id="Item_Page">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h4 class="panel-title">
                                <a data-toggle="collapse" data-parent="#accordion" href="#collapse1">Category Details</a>
                            </h4>
                        </div>
                        <div id="collapse1" class="panel-collapse collapse">
                            <div class="panel-body" style="display: inline-block; width: 100%">

                                <div class="form-group has-success" style="margin-bottom: 10px; display: flex">

                                    <label class="col-sm-10 control-label col-lg-4">Category Image</label>
                                    <div class="col-lg-8" style="text-align: center;">
                                        <%--<asp:FileUpload ID="FileUpload1" runat="server" CssClass="col-md-12 col-xs-12" />--%>
                                        <input id="fileupload2" runat="server" type="file" name="file" onchange="showimagepreview()" accept="image/*"></input>
                                        <b style="color: red">* Image Format In gif , png , jpg , bmp , jpeg</b>
                                        <br />
                                        <br />
                                        <img id="Img1" runat="server" src="Images/NoPic.png" class="noImage" style="width: 250px; height: 250px; border-radius: 5px 5px 5px 5px;" />
                                    </div>
                                </div>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div class="form-group has-success">
                                            <label class="col-sm-10 control-label col-lg-4"></label>
                                            <div class="col-lg-8">
                                                <div class="panel panel-default">
                                                    <div class="panel-heading">
                                                        <h4 class="panel-title">
                                                            <i data-toggle="collapse" data-parent="#accordion1" href="#collapse9" style="font-style: normal; cursor: pointer;">Select Department</i>
                                                        </h4>
                                                    </div>
                                                    <div id="collapse9" class="panel-collapse collapse in">
                                                        <div class="panel-body" style="background-color: #f9f9f9">
                                                            <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>--%>
                                                            <div class="form-group has-success">
                                                                <label class="col-sm-10 control-label col-sm-3">Filter </label>
                                                                <div class="col-sm-8">
                                                                    <asp:TextBox ID="txt_Filter" runat="server" AutoComplete="off" MaxLength="50" OnTextChanged="txt_Filter_TextChanged" placeholder="Filter by dept. code / description" AutoPostBack="true" CssClass="col-md-12 col-xs-12" Style="border: 2; height: 34px; padding: 6px 12px; font-size: 14px;"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <br />
                                                            <div style="height: 30px"></div>
                                                            <div class="form-group has-success">
                                                                <label class="col-sm-10 control-label col-sm-3">Select Dept. </label>
                                                                <div class="col-sm-8">
                                                                    <asp:DropDownList ID="ddlDept" runat="server" Style="border: 2; height: 34px; padding: 6px 12px; font-size: 14px;" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddlDept_SelectedIndexChanged" AppendDataBoundItems="true">
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <br />
                                        <br />
                                        <div style="height: 30px"></div>
                                        <div class="has-success">
                                            <label class="col-sm-10 control-label col-lg-4">Department Code</label>
                                            <div class="form-group col-sm-8">
                                                <asp:TextBox ID="txt_Dept" runat="server" AutoComplete="off" CssClass="col-md-12 col-xs-12" Style="border: 2; height: 34px; padding: 6px 12px; font-size: 14px;" Enabled="false"></asp:TextBox>

                                            </div>
                                        </div>
                                        <br />
                                        <br />

                                        <div class="has-success">

                                            <label class="col-sm-10 control-label col-lg-4">Category Code</label>
                                            <div class="form-group col-sm-8">
                                                <asp:TextBox ID="txt_CategoryCode" runat="server" Enabled="false" AutoComplete="off" MaxLength="30" placeholder="Example : SD00101" CssClass="col-md-12 col-xs-12" Style="border: 2; height: 34px; padding: 6px 12px; font-size: 14px;"></asp:TextBox>
                                            </div>
                                        </div>


                                        <br />
                                        <div style="height: 30px"></div>
                                        <div class="has-success">
                                            <label class="col-sm-10 control-label col-lg-4">Category Description</label>
                                            <div class="form-group col-sm-8">
                                                <asp:TextBox ID="txt_CategoryDesc" runat="server" AutoComplete="off" MaxLength="100" placeholder="Example : IMPORTED S. DRINK" CssClass="col-md-12 col-xs-12" Style="border: 2; height: 34px; padding: 6px 12px; font-size: 14px;"></asp:TextBox>


                                            </div>
                                        </div>





                                    </ContentTemplate>
                                    <Triggers>
                                        <%--<asp:AsyncPostBackTrigger ControlID="btn_Insert" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btn_Update" EventName="Click" />--%>
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
                <br />

                <div style="width: 100%;">
                    <asp:Button ID="btn_Back" runat="server" Text="Back" OnClick="Back_OnClick" CssClass="ghost-button-thick-border" Style="float: right" Visible="true" />
                    <asp:Button ID="btn_Insert" runat="server" Text="Insert" OnClick="Insert_OnClick" CssClass="ghost-button-thick-border" />
                    <asp:Button ID="btn_Update" runat="server" Text="Update" OnClick="Update_OnClick" CssClass="ghost-button-thick-border" Visible="false" />
                </div>
            </div>
        </section>
        <!-- wrapper -->
    </section>
    <!-- MAIN CONTENT -->
</asp:Content>

