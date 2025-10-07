<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="AddItem.aspx.cs" Inherits="AddItem" %>
<%@ Register Assembly="msgBox" Namespace="BunnyBear" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxControlToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="CSS/jquery-te-1.4.0.css" rel="stylesheet" type="text/css" />
   <%-- <script language="javascript" type="text/javascript">
        function previewFile() {
            var preview = document.querySelector('#<%=Img2.ClientID %>');
                var file = document.querySelector('#<%=FileUpload1.ClientID %>').files[0];
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

    </script>--%>
    <script language="javascript" type="text/javascript">
        
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

        <%-- function previewFile2() {
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
        }--%>

      
    </script>
    
    <script type="text/javascript">
        $(function () {
            var tabName = $("[id*=TabName]").val() != "" ? $("[id*=TabName]").val() : "employment";
            $('#Tabs a[href="#' + tabName + '"]').tab('show');
            $("#Tabs a").click(function () {
                $("[id*=TabName]").val($(this).attr("href").replace("#", ""));
            });
        });


    </script>
    <style>
    #rcorners2 {
    border-radius: 25px;
    border: 2px solid #73AD21;
    padding: 20px; 
    width: 100px;
    height: 100px;    
        }
        
        .btn-glyphicon {
    padding:8px;
    background:#ffffff;
    margin-right:4px;
    
}
.icon-btn {
    padding: 1px 15px 3px 2px;
    border-radius:50px;
    
}
    
    </style>
    
    <style type="text/css">
    
 #overlay {
            position: fixed;
            z-index: 98;
            top: 0px;
            left: 0px;
            background-color: #ffffff;
            width: 100%;
            height: 100%;
            filter: Alpha(Opacity=80);
            opacity: 0.80;
            -moz-opacity: 0.80;
        }

        #theprogress {
            background-color: transparent;
            width: 110px;
            height: 24px;
            text-align: center;
            filter: Alpha(Opacity=100);
            opacity: 1;
            -moz-opacity: 1;
        }

        #modalprogress {
            position: absolute;
            top: 50%;
            left: 50%;
            margin: -11px 0 0 -44px;
            color: White;
        }

        body > #modalprogress {
            position: fixed;
        }

     .costbutton1
    {
        padding: 5px 140px;
         background-color: #f2bb3c;
          border: none;
          color: black;
          text-align: center;
          text-decoration: none;
          display: inline-block;
          font-size: 16px;
          margin: 4px 2px;
          cursor: pointer;
    }

    .costbutton
    {
        padding: 5px 45px;
         background-color: #f2bb3c;
          border: none;
          color: black;
          text-align: center;
          text-decoration: none;
          display: inline-block;
          font-size: 16px;
          margin: 4px 2px;
          cursor: pointer;
    }

    .table table tbody tr td a,
        .table table tbody tr td span {
            position: relative;
            float: left;
            padding: 6px 12px;
            margin-left: -1px;
            line-height: 1.42857143;
            color: #337ab7;
            text-decoration: none;
            background-color: #fff;
            border: 1px solid #ddd;
        }

        .table table > tbody > tr > td > span {
            z-index: 3;
            color: #fff;
            cursor: default;
            background-color: #337ab7;
            border-color: #337ab7;
        }

        .table table > tbody > tr > td:first-child > a,
        .table table > tbody > tr > td:first-child > span {
            margin-left: 0;
            border-top-left-radius: 4px;
            border-bottom-left-radius: 4px;
        }

        .table table > tbody > tr > td:last-child > a,
        .table table > tbody > tr > td:last-child > span {
            border-top-right-radius: 4px;
            border-bottom-right-radius: 4px;
        }

        .table table > tbody > tr > td > a:hover,
        .table table > tbody > tr > td > span:hover,
        .table table > tbody > tr > td > a:focus,
        .table table > tbody > tr > td > span:focus {
            z-index: 2;
            color: #23527c;
            background-color: #eee;
            border-color: #ddd;
        }
    </style>
    <script type="text/javascript">
        function IsNumberWithOneDecimal2(txt, evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57) && !(charCode == 46 || charCode == 8)) {
                return false;
            } else {
                var len = txt.value.length;
                var index = txt.value.indexOf('.');
                if (index > 0 && charCode == 46) {
                    return false;
                }
                if (index > 0) {
                    if ((len + 1) - index > 5) {
                        return false;
                    }
                }

            }
            return true;
        }

        function allowAlphaNumericSpace(e) {
            var code = ('charCode' in e) ? e.charCode : e.keyCode;
            if (!(code == 32) && // space
                !(code > 47 && code < 58) && // numeric (0-9)
                !(code > 64 && code < 91) && // upper alpha (A-Z)
                !(code > 96 && code < 123)) { // lower alpha (a-z)
                e.preventDefault();
            }
        }

        function allowAlphaNumeric(e) {
            var code = ('charCode' in e) ? e.charCode : e.keyCode;
            if (!(code == 32) &&
                !(code > 47 && code < 58) &&
                !(code > 64 && code < 91) &&
                !(code > 96 && code < 123)) {
                return e.replace(/[^\d]/gi, '');
            }
        }

        function IsNumberWithOneDecimal(txt, evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57) && !(charCode == 46 || charCode == 8)) {
                return false;
            } else {
                var len = txt.value.length;
                var index = txt.value.indexOf('.');
                if (index > 0 && charCode == 46) {
                    return false;
                }
                if (index > 0) {
                    if ((len + 1) - index > 3) {
                        return false;
                    }
                }

            }
            return true;
        }

        function updateTextbox2() {
            // Get references to the textboxes
            var textbox1 = document.getElementById('<%= txt_Desc.ClientID %>');
             var textbox2 = document.getElementById('<%= txt_ShortDesc.ClientID %>');

             // Update the value of textbox2 with the current value of textbox1
            textbox2.value = textbox1.value.substring(0, 35);;
        }

        function IsNumberWithNoDecimal(txt, evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57) && !(charCode == 46 || charCode == 8)) {
                return false;
            } else {
                var len = txt.value.length;
                var index = txt.value.indexOf('.');
                if (index < 0 && charCode == 46) {
                    return false;
                }
                if (index > 0) {
                    if ((len + 1) - index > 4) {
                        return false;
                    }
                }
            }
            return true;
        }

    </script>

    <script src="http://js.nicedit.com/nicEdit-latest.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     

    <section id="main-content" >
        <section class="wrapper">
            <cc1:msgbox id="MsgBox1" runat="server"></cc1:msgbox>
            <asp:HiddenField ID="PaneName" runat="server" />
            <asp:HiddenField ID="PaneNameS" runat="server" />
  <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                    <ProgressTemplate>
                                    <div id="overlay">
                                        <div id="modalprogress">
                                            <div id="theprogress">
                                                <p><img alt="Loading" src="Images/GIF/spin.gif" width="60px" height="60px"/> 
                                                
                                                </p>
                                                
                                            </div>
                                        </div>
                                    </div>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
            <asp:UpdateProgress ID="ppp1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                                    <ProgressTemplate>
                                    <div id="overlay">
                                        <div id="modalprogress">
                                            <div id="theprogress">
                                                <p><img alt="Loading" src="Images/GIF/spin.gif" width="60px" height="60px"/> 
                                                
                                                </p>
                                                
                                            </div>
                                        </div>
                                    </div>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>

            <div class="col-md-12 col-sm-12" style="color:Black">
            
                <asp:Label ID="MF_Con" runat="server" Text="Label" Visible="false"></asp:Label>
                <asp:Label ID="lblrunno" runat="server" Visible="false"></asp:Label>
            <asp:LinkButton ID="btn_Add" runat="server" OnClick="Back_OnClick" style="float:right;"><i class="glyphicon fa-2x glyphicon-circle-arrow-left addItem" style="color: #e10808"></i></asp:LinkButton>    
                <asp:TextBox ID="TextBox1" runat="server" AutoComplete="off"  style="border:2; height:34px; padding:6px 12px; font-size:14px;" Visible="false"></asp:TextBox>
                <asp:TextBox ID="TextBox2" runat="server" AutoComplete="off" style="border:2; height:34px; padding:6px 12px; font-size:14px;" Visible="false"></asp:TextBox>
                <asp:TextBox ID="TextBox3" runat="server" AutoComplete="off" style="border:2; height:34px; padding:6px 12px; font-size:14px;" Visible="false"></asp:TextBox>
                <asp:TextBox ID="TextBox4" runat="server" AutoComplete="off" style="border:2; height:34px; padding:6px 12px; font-size:14px;" Visible="false"></asp:TextBox>
                
                <%--glyphicon glyphicon-circle-arrow-left--%>
                <div class="panel-group" id="Item_Page" >
                    <div class="panel panel-default">
                        <div class="panel-heading" >
                            <h4 class="panel-title">
                                <a data-toggle="collapse" data-parent="#accordion" href="#collapse1" >Product Details</a>
                            </h4>
                        </div>

                        <div id="collapse1" class="panel-collapse collapse">
                            <div class="panel-body">
                            <%-- <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>--%>
                                <div class="form-group has-success">
                                    <%--<label class="col-sm-10 control-label col-lg-4" >Upload Image</label>--%>

                                    <%--<div class="panel panel-default" >--%>
                                        <div id="Tabs" role="tabpanel">
                                            <!-- Nav tabs -->
                                            <ul class="nav nav-tabs" role="tablist">
                                                <li class="active"><a href="#employment" aria-controls="employment" role="tab" data-toggle="tab">Product Image</a></li>
                                                <li><a href="#Productspec" aria-controls="Productspec" role="tab" data-toggle="tab">Product Specification</a></li>
                                                <li><a href="#Rewardredempt" aria-controls="Rewardredempt" role="tab" data-toggle="tab">Reward & Redemption</a></li>
                                                <li><a href="#other" aria-controls="other" role="tab" data-toggle="tab">Others</a></li>
                                                <li id ="licommsetting" runat="server"><a href="#commsetting" aria-controls="commsetting" role="tab" data-toggle="tab">Comm Setting</a></li>
                                            </ul>
                                            <!-- Tab panes -->
                                            <div class="tab-content" style="padding-top: 20px">
                                                <div role="tabpanel" class="tab-pane fade in active" id="employment">
                                                    <br />
                                        <div class="col-lg-8" style="text-align:center;">
                                        <%--<asp:FileUpload ID="FileUpload1" runat="server" CssClass="col-md-12 col-xs-12" />--%>
                                        <input id="fileupload2" runat="server" type="file" name="file" onchange="showimagepreview()" accept="image/*"></input>
                                            <b style="color:red">* Image Format In gif , png , jpg , bmp , jpeg</b>
                                        <br /><br />
                                        
                                        </div>

                                        <div class="form-group has-success">
                                         <label class="col-sm-10 control-label col-lg-4" ></label>
                                            <div class="col-lg-12" style="text-align:center;">
                                            <img id="Img1" runat="server" src="Images/NoPic.png" class="noImage" style="width:250px; height:250px; border-radius: 5px 5px 5px 5px ;" /> 
                                        <br id="br1" runat="server"/>
                                        
                                         <asp:CheckBox ID="CheckBox4" runat="server"></asp:CheckBox>
                                         &nbsp;
                                        <%-- <asp:LinkButton ID="linkUpload2" runat="server" class="btn btn-info" OnClick="add_List_Click2" style="height:40px;">
                                         <span class="glyphicon glyphicon-upload" style="margin-top: 5%"></span> Upload Image
                                         </asp:LinkButton>--%>
                                         <asp:Button ID="linkUpload2" OnClick="add_List_Click2" Text="Upload Image" runat="server" CssClass="btn btn-primary" />
                                         <br id="br2" runat="server"/>
                                        <%-- <br id="br5" runat="server"/>--%><%--<br id="br5" runat="server"/>--%>
                                        

                                            <asp:UpdatePanel ID="UpdatePanel9" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                         <asp:Repeater ID="rpt_Item2" runat="server" OnItemDataBound="CardItem_DataBound2" OnItemCommand="Item_Details_Command2">
                                            <ItemTemplate>
                                                <%--<code><small><asp:Label ID="LabelRecord" runat="server" ForceColor="Red" Text="No Image Found!" Visible="false"></asp:Label></small></code>--%>
                                                    <div class="col-sm-12 col-md-3" >
                                                    
                                                   
                      		                        <div class="white-panel itemColumn" style="background-color: #fff; border-color: #ccc; border-radius: 5px 5px 5px 5px; border-style:none; box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19); transition:0.3s;">
                                                    
                                                        <div class="white-header pn itemHeight" style="background-color: #fff; border-color: #ccc; border-radius: 5px 5px 5px 5px; border-style:none; box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19); transition:0.3s;">
                                                   
                                                        <%--<span id="Span1" style="color:White;font-size: 15;z-index: 1; right: -10px;top:-10px;color: White;background-color: #cd3;position: absolute;" visible="true" runat="server" class="badge badge-success">Y</span>--%>
                                                            <h5>
                                                    <table style="width: 100%;">
                                                    <tr style="background-color:#fff; height: 35px;  text-align: center">
                                               
                                                        <td>
                                                           <asp:LinkButton id="addlistnoitem2" OnClientClick="return false;" style="color:White;font-size: 15px;z-index: 1; left: -10px;top:-10px;color: White;background-color: #e90b0b;position: absolute;" visible="false" runat="server" class="badge badge-success">
                                                        <span class="glyphicon glyphicon-picture"></span>
                                                        </asp:LinkButton>
                                                           <asp:ImageButton ID="Image1"  runat="server" height="90px" Width="90px" style="border-radius: 5px 5px 5px 5px ; float:left; margin-left:10px; margin-top:20px" />    
                                                           <asp:Label ID="btn_badge" class="badge bg-green" Visible="false" Text="Y" runat="server" />
                                                           <asp:Label ID="btn_badge2" class="badge bg-red" Visible="false" Text="X" runat="server" />
                                                           <div style="height:5px"></div>

                                                            <asp:LinkButton ID="UploadOn" class="btn icon-btn btn-success" runat="server" Visible="false" OnClientClick="return alert('This Image already set as default!');">
                                                            <span class="glyphicon btn-glyphicon glyphicon-picture img-circle text-muted"></span>Default
                                                            </asp:LinkButton>


                                                            <asp:LinkButton ID="UploadOff" class="btn icon-btn btn-default active" runat="server" Visible="false" CommandArgument='<%# Eval("ID") %>' CommandName="Default" OnClientClick="return confirm('Are you sure want to set this image as default?');">
                                                            <span class="glyphicon btn-glyphicon glyphicon-picture img-circle text-muted"></span>Default
                                                            </asp:LinkButton>
                                                           

                                                            <div style="height:5px"></div>
                                                            <asp:LinkButton ID="PublishOn" class="btn icon-btn btn-success" runat="server" Visible="false" CommandArgument='<%# Eval("ID") %>' CommandName="Update2" OnClientClick="return confirm('Are you sure want to unpublish this Image?');">
                                                            <span class="glyphicon btn-glyphicon glyphicon-open img-circle text-warning"></span>Publish
                                                            </asp:LinkButton>

                                                            <asp:LinkButton ID="PublishOff" class="btn icon-btn btn-default active" runat="server" Visible="false" CommandArgument='<%# Eval("ID") %>' CommandName="Update1" OnClientClick="return confirm('Are you sure want to publish this Image?');">
                                                            <span class="glyphicon btn-glyphicon glyphicon-open img-circle text-warning"></span>Publish
                                                            </asp:LinkButton>

                                                           <asp:CheckBox ID="CheckBox2" runat="server" Visible = "false" Text="Publish"></asp:CheckBox>
                                                           <asp:CheckBox ID="CheckBox3" runat="server" Visible = "false"></asp:CheckBox>

                                                          <div style="height:5px"></div>

                                                          <asp:LinkButton ID="DeleteOn" class="btn icon-btn btn-danger" runat="server" Visible="false" OnClientClick="return confirm('Do you want to delete this Image?');" CommandArgument='<%# Eval("ID") %>' CommandName="Delete">
                                                            <span class="glyphicon btn-glyphicon glyphicon-trash img-circle text-danger"></span>Delete&nbsp;&nbsp;
                                                            </asp:LinkButton>

                                                            <asp:LinkButton ID="DeleteOff" class="btn icon-btn btn-default active" runat="server" Visible="false" OnClientClick="return alert('This Image cannot be deleted by default!') false;">
                                                            <span class="glyphicon btn-glyphicon glyphicon-trash img-circle text-danger"></span>Delete&nbsp;&nbsp;
                                                            </asp:LinkButton>

                                                           <br /><div style="height:5px"></div>
                                                           <asp:ImageButton ID="ImageButton2" runat="server" Visible = "false" ImageUrl="Images/Update.png" Tooltip= "Update" height="35px" Width="35px" OnClientClick="return confirm('Do you want to Update?');" CommandArgument='<%# Eval("ID") %>' CommandName="Update"/>
                                                           <asp:ImageButton ID="ImageButton3" runat="server" Visible = "false" ImageUrl="Images/Upload.png" Tooltip= "Back" height="35px" Width="35px" CommandArgument='<%# Eval("ID") %>' CommandName="Default" OnClientClick="return confirm('Are you sure want to set this image as default?');"/>
                                                           <asp:ImageButton ID="Image2"  runat="server" Visible="false" ImageUrl="Images/Deleteimg.png" Tooltip= "Delete" height="35px" Width="35px" OnClientClick="return confirm('Do you want to delete this Image?');" CommandArgument='<%# Eval("ID") %>' CommandName="Delete"/>
                                                           <asp:ImageButton ID="Image3" Visible="false" runat="server" ImageUrl="Images/checked.png" height="30px" Width="30px" />
                                                           <br /><div style="height:8px"></div>
                                                            <asp:Button ID="Button1" class="btn btn-default" CommandArgument='<%# Eval("ID") %>' CommandName="Default" Visible="false" runat="server" Text="Default" OnClientClick="return confirm('Are you sure want to set this image as default?');"></asp:Button>
                                                           
                                                        </td>
                                                        
                                                      
                                                    </tr>
                                                       

                                                   </table>
                                                   
                                                   </h5>
                                                   </div>
                                                   </div>
                                                   <h4 style="margin: 0px 0px 15px 0px;"></h4>  
                                                   </div>
                                                
                                            </ItemTemplate>
                                             <FooterTemplate>
                                                           <div id="dvNoRecords" runat="server" visible="false" style="padding:20px 20px; text-align:center; color:Red;">
                                                            <code>No Images Record Found!</code>
                                                            </div>
                                                            </FooterTemplate>

                                             <SeparatorTemplate></SeparatorTemplate>
                                        </asp:Repeater>
                                        <div class="col-lg-12 col-md-12 col-sm-12">
                                        <div>
                                       
                                        <br id="br6" runat="server"/>
                                        <asp:DropDownList ID="ddlPager2" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPager2_SelectedIndexChanged" style="float:right;display: inline-block;width: 100px; height: 42px;font-weight: bold;padding: 8px;text-align: center;outline: none;text-decoration: none;"></asp:DropDownList>
                                        <br />
                                        <div style="height: 30px"></div>
                                        <asp:Label ID="Label1" runat="server" style="float:right;"></asp:Label>
                                        &nbsp;
                                        <asp:Label ID="Label7" runat="server" style="float:right;" Text="No. of record: "></asp:Label>
                                        </div>
                                        </div>
                                        </ContentTemplate>
                                        <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="rpt_Item2" />
                                        </Triggers>
                                       </asp:UpdatePanel>
                                         </div>
                                         </div>
                                                    
                                                </div>

                                                <div role="tabpanel" class="tab-pane fade in" id="Productspec">
                                                    <div class="panel-group" id="accordion">
                                                        <div class="panel panel-default">

                                                            <div class="panel-heading" style="background-color: #dfdfdf !important; border-color: Black; border: 1px;">
                                                                <h4 class="panel-title">
                                                                    <i style="color: black; font-style: normal; font-weight: bold">Product Spec Details</i>
                                                                </h4>
                                                            </div>
                                                            <div class="panel-collapse">
                                                                <div class="panel-body" style="background-color: #f9f9f9">
                                                                    <div class="textbox-group">
                                                                        <asp:TextBox ID="txtEditor" TextMode="MultiLine" runat="server" CssClass="textEditor" onblur="Test()"></asp:TextBox>

                                                                        <asp:Label ID="hdText" runat="server"></asp:Label>

                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="panel-heading" style="background-color: #dfdfdf !important; border-color: Black; border: 1px;">
                                                                <h4 class="panel-title">
                                                                    <i style="color: black; font-style: normal; font-weight: bold">Modifier </i>
                                                                </h4>
                                                            </div>
                                                            <div class="panel-collapse">
                                                                <div class="panel-body" style="background-color: #f9f9f9">
                                                                    <div class="col-xs-12 col-lg-12 col-md-12" style="overflow-y: auto; height: 250px; margin-bottom: 10px">
                                                                        <asp:GridView ID="grd_viewmodifer" AutoGenerateColumns="false" GridLines="Horizontal" OnRowDataBound="grd_viewmodifer_RowDataBound" CssClass="table table-bordered table-responsive table-hover " runat="server">
                                                                            <Columns>
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        <asp:CheckBox ID="chkmodifier" runat="server" />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Modifier Name" ItemStyle-HorizontalAlign="Left">
                                                                                    <ItemTemplate>
                                                                                        <asp:HiddenField ID="hdmodifierID" runat="server" Value='<%# Bind("modifier_id") %>' />
                                                                                        <asp:Label ID="lblmodifiername" runat="server" Text='<%# Bind("modifier_grp_name") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Option" ItemStyle-Width="500px" ItemStyle-HorizontalAlign="Left">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblmodifieroption" runat="server" Text='<%# Bind("modifieroption") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="panel-heading" style="background-color: #dfdfdf !important; border-color: Black; border: 1px;">
                                                                <h4 class="panel-title">
                                                                    <i style="color: black; font-style: normal; font-weight: bold">YouTube Video Link</i>
                                                                </h4>
                                                            </div>
                                                            <div class="panel-collapse">
                                                                <div class="panel-body" style="background-color: #f9f9f9">
                                                                    <div class="textbox-group">
                                                                        <asp:Label ID="lblvideo" Text="Video Link 1" runat="server" Font-Bold="true"></asp:Label>
                                                                        <asp:TextBox ID="txtVlink" runat="server" CssClass="form-control"></asp:TextBox>
                                                                    </div>
                                                                    <div class="textbox-group">
                                                                        <asp:Label ID="Label11" Text="Video Link 2" runat="server" Font-Bold="true"></asp:Label>
                                                                        <asp:TextBox ID="txtVlink2" runat="server" CssClass="form-control"></asp:TextBox>
                                                                    </div>
                                                                    <div class="textbox-group">
                                                                        <asp:Label ID="Label12" Text="Video Link 3" runat="server" Font-Bold="true"></asp:Label>
                                                                        <asp:TextBox ID="txtVlink3" runat="server" CssClass="form-control"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div role="tabpanel" class="tab-pane fade in" id="Rewardredempt">
                                                    <div class="panel-group" id="accordion">
                                                        <div class="panel panel-default">
                                                            <div class="panel-heading" style="background-color: #dfdfdf !important; border-color: Black; border: 1px;">
                                                                <h4 class="panel-title">
                                                                    <i style="color: black; font-style: normal; font-weight: bold">Reward & Redemption Setting</i>
                                                                </h4>
                                                            </div>
                                                            <div class="panel-collapse">
                                                                <div class="panel-body" style="background-color: #f9f9f9">

                                                                    <div class="has-success">
                                                                        <label class="col-sm-10 control-label col-lg-4">Redemption Item</label>
                                                                        <div class="form-group col-sm-8">
                                                                            <asp:DropDownList ID="ddl_RdmItem" runat="server" OnSelectedIndexChanged="ddl_RdmItem_SelectedIndexChanged" AutoPostBack="true" Style="border: 2; height: 34px; padding: 6px 12px; font-size: 14px;">
                                                                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                                <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <div class="has-success">
                                                                                <label id="lblredempt" runat="server" class="col-sm-10 control-label col-lg-4">Redemption Point</label>
                                                                                <div class="form-group col-sm-8">
                                                                                    <asp:TextBox ID="txt_RdmPoint" runat="server" onkeypress="return IsNumberWithOneDecimal(this,event);" Text="0" AutoComplete="off" CssClass="col-md-12 col-xs-12" Style="border: 2; height: 34px; padding: 6px 12px; font-size: 14px;"></asp:TextBox>
                                                                                </div>
                                                                                <br />
                                                                                <label id="lblreward" runat="server" class="col-sm-10 control-label col-lg-4">Reward Point</label>
                                                                                <div class="form-group col-sm-8">
                                                                                    <asp:TextBox ID="txtrewardpoint" runat="server" onkeypress="return IsNumberWithOneDecimal(this,event);" Text="0" AutoComplete="off" CssClass="col-md-12 col-xs-12" Style="border: 2; height: 34px; padding: 6px 12px; font-size: 14px;"></asp:TextBox>
                                                                                </div>

                                                                            </div>

                                                                        </ContentTemplate>
                                                                        <Triggers>
                                                                            <asp:AsyncPostBackTrigger ControlID="ddl_RdmItem" EventName="SelectedIndexChanged" />
                                                                        </Triggers>
                                                                    </asp:UpdatePanel>
                                                                    <br />
                                                                    <br />
                                                                    <div class="has-success">
                                                                        <div class="form-group col-sm-8">
                                                                            <asp:UpdatePanel UpdateMode="Conditional" ID="update1" runat="server">
                                                                                <ContentTemplate>

                                                                                    <asp:GridView ID="grd_view" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover">
                                                                                        <Columns>

                                                                                            <asp:TemplateField HeaderText="Member Level">
                                                                                                <ItemStyle Width="130px" />
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lbl_memberlvl" runat="server" Text='<%#Eval("levelname") %>'></asp:Label>
                                                                                                    <asp:Label ID="lblagentlvl" Visible="false" runat="server" Text='<%#Eval("agentlevelcode") %>'></asp:Label>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="Redemption Point">
                                                                                                <ItemStyle Width="130px" />
                                                                                                <ItemTemplate>
                                                                                                    <asp:TextBox ID="txt_redempt" runat="server" onkeypress="return IsNumberWithOneDecimal(this,event);" Text='<%#Eval("RedemptionPoint","{0:N}") %>' Width="125px"></asp:TextBox>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="Member Reward Point">
                                                                                                <ItemStyle Width="130px" />
                                                                                                <ItemTemplate>
                                                                                                    <asp:TextBox ID="txt_reward" runat="server" onkeypress="return IsNumberWithOneDecimal(this,event);" Text='<%#Eval("EarnRPWhenBuy","{0:N}") %>' Width="125px"></asp:TextBox>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                        </Columns>

                                                                                    </asp:GridView>

                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div role="tabpanel" class="tab-pane fade in" id="other">
                                                    <div class="panel-group" id="accordion">
                                                        <div class="panel panel-default">
                                                            <div class="panel-heading" style="background-color: #dfdfdf !important; border-color: Black; border: 1px;">
                                                                <h4 class="panel-title">
                                                                    <i style="color: black; font-style: normal; font-weight: bold">Others</i>
                                                                </h4>
                                                            </div>
                                                            <div class="panel-collapse">
                                                                <div class="panel-body" style="background-color: #f9f9f9">
                                                                    <div class="has-success">
                                                                        <label id="Label9" runat="server" style="display:none" class="col-sm-10 control-label col-lg-4">SPP</label>
                                                                        <div class="form-group col-sm-8" style="display:none">
                                                                            <asp:TextBox ID="txtSPP" runat="server" onkeypress="return IsNumberWithOneDecimal(this,event);" AutoComplete="off" Text="0" CssClass="col-md-12 col-xs-12 col-lg-8" Style="border: 2; height: 34px; padding: 6px 12px; font-size: 14px;"></asp:TextBox>
                                                                        </div>
                                                                        <br />
                                                                        <label id="Label10" runat="server" class="col-sm-10 control-label col-lg-4">Referral Bonus <span id="spprofit" runat="server"></span></label>
                                                                        <div class="form-group col-sm-8">
                                                                            <asp:Label ID="lblRefProfitBonusPercent" runat="server"></asp:Label>
                                                                            <asp:TextBox ID="txtRefProfitBonusPercent" runat="server" onkeypress="return IsNumberWithOneDecimal(this,event);" AutoComplete="off" CssClass="col-md-12 col-xs-12 col-lg-8" Style="border: 2; height: 34px; padding: 6px 12px; font-size: 14px;"></asp:TextBox>
                                                                        </div>
                                                                        <br />

                                                                        <label id="Label17" runat="server" class="col-sm-10 control-label col-lg-4">Network Plus Bonus (Level 1)</label>
                                                                        <div class="form-group col-sm-8">
                                                                            <asp:TextBox ID="txtTeamKPI2" runat="server" onkeypress="return IsNumberWithOneDecimal(this,event);" Text="0" AutoComplete="off" CssClass="col-md-12 col-xs-12 col-lg-8" Style="border: 2; height: 34px; padding: 6px 12px; font-size: 14px;"></asp:TextBox>
                                                                        </div>
                                                                        <br />
                                                                        <label id="Label13" runat="server" class="col-sm-10 control-label col-lg-4">Network Plus Bonus (Level 2)</label>
                                                                        <div class="form-group col-sm-8">
                                                                            <asp:TextBox ID="txtTeamKPI" runat="server" onkeypress="return IsNumberWithOneDecimal(this,event);" Text="0" AutoComplete="off" CssClass="col-md-12 col-xs-12 col-lg-8" Style="border: 2; height: 34px; padding: 6px 12px; font-size: 14px;"></asp:TextBox>
                                                                        </div>
                                                                        <br />
                                                                        <label id="Label16" runat="server" class="col-sm-10 control-label col-lg-4">Merchant Brand Dev. Incentive (VVIP Only)</label>
                                                                        <div class="form-group col-sm-8">
                                                                            <asp:TextBox ID="txtMBD" runat="server" onkeypress="return IsNumberWithOneDecimal(this,event);" Text="0" AutoComplete="off" CssClass="col-md-12 col-xs-12 col-lg-8" Style="border: 2; height: 34px; padding: 6px 12px; font-size: 14px;"></asp:TextBox>
                                                                        </div>
                                                                        <br />
                                                                        <label id="Label14" runat="server" class="col-sm-10 control-label col-lg-4">Month End Bonus</label>
                                                                        <div class="form-group col-sm-8">
                                                                            <asp:TextBox ID="txtMEB" runat="server" onkeypress="return IsNumberWithOneDecimal2(this,event)" Text="0" AutoComplete="off" CssClass="col-md-12 col-xs-12 col-lg-8" Style="border: 2; height: 34px; padding: 6px 12px; font-size: 14px;"></asp:TextBox>
                                                                        </div>
                                                                        <br />
                                                                        <label id="Label18" runat="server" style="display:none" class="col-sm-10 control-label col-lg-4">Team Growth Incentive (Level 1)</label>
                                                                        <div class="form-group col-sm-8" style="display:none">
                                                                            <asp:TextBox ID="txtteamgrowthinc1" runat="server" onkeypress="return IsNumberWithOneDecimal2(this,event)" Text="0" AutoComplete="off" CssClass="col-md-12 col-xs-12 col-lg-8" Style="border: 2; height: 34px; padding: 6px 12px; font-size: 14px;"></asp:TextBox>
                                                                        </div>
                                                                        <br />
                                                                        <label id="Label19" runat="server" style="display:none" class="col-sm-10 control-label col-lg-4">Team Growth Incentive (Level 2)</label>
                                                                        <div class="form-group col-sm-8" style="display:none">
                                                                            <asp:TextBox ID="txtteamgrowthinc2" runat="server" onkeypress="return IsNumberWithOneDecimal2(this,event)" Text="0" AutoComplete="off" CssClass="col-md-12 col-xs-12 col-lg-8" Style="border: 2; height: 34px; padding: 6px 12px; font-size: 14px;"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                     <div class="panel-group" id="accordion">
                                                        <div class="panel panel-default">
                                                            <div class="panel-heading" style="background-color: #dfdfdf !important; border-color: Black; border: 1px;">
                                                                <h4 class="panel-title">
                                                                    <i style="color: black; font-style: normal; font-weight: bold">Printer</i>
                                                                </h4>
                                                            </div>
                                                            <div class="panel-collapse">
                                                                <div class="panel-body" style="background-color: #f9f9f9">
                                                                    <div class="has-success">
                                                                         <div class="col-xs-12 col-lg-12 col-md-12" style="overflow-y: auto; height: 250px; margin-bottom: 10px">
                                                                        <asp:GridView ID="grd_viewprinter" AutoGenerateColumns="false" GridLines="Horizontal" OnRowDataBound="grd_viewprinter_RowDataBound" CssClass="table table-bordered table-responsive table-hover " runat="server">
                                                                            <Columns>
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        <asp:CheckBox ID="chkprinter" runat="server" />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Printer Name" ItemStyle-HorizontalAlign="Left">
                                                                                    <ItemTemplate>
                                                                                        <asp:HiddenField ID="hdprinterID" runat="server" Value='<%# Bind("ids") %>' />
                                                                                        <asp:Label ID="lblprintname" runat="server" Text='<%# Bind("print_name") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="IP Address" ItemStyle-Width="500px" ItemStyle-HorizontalAlign="Left">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblIPAddress" runat="server" Text='<%# Bind("IP_Address") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div role="tabpanel" class="tab-pane fade in" id="commsetting">
                                                    <div class="panel-group" id="accordion">
                                                        <div class="panel panel-default">
                                                            <div class="panel-heading" style="background-color: #dfdfdf !important; border-color: Black; border: 1px;">
                                                                <h4 class="panel-title">
                                                                    <i style="color: black; font-style: normal; font-weight: bold">Commission Setting</i>
                                                                </h4>
                                                            </div>
                                                            <div class="panel-collapse">
                                                                <div class="panel-body" style="background-color: #f9f9f9">
                                                                    <div class="has-success">
                                                                        <asp:UpdatePanel runat="server">
                                                                            <ContentTemplate>
                                                                                <label class="col-sm-10 control-label col-lg-4">Comm Type</label>
                                                                                <div class="form-group col-sm-8">
                                                                                    <asp:DropDownList ID="ddlcommtype" runat="server" Style="border: 2; height: 34px; padding: 6px 12px; font-size: 14px;">
                                                                                        <asp:ListItem Text="None" Value="None"></asp:ListItem>
                                                                                        <asp:ListItem Text="Service" Value="Service"></asp:ListItem>
                                                                                        <asp:ListItem Text="Product" Value="Product"></asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                </div>
                                                                                <br />
                                                                                <label class="col-sm-10 control-label col-lg-4">Comm By</label>
                                                                                <div class="form-group col-sm-8">
                                                                                    <asp:DropDownList ID="ddlcommby" runat="server" OnSelectedIndexChanged="ddlcommby_SelectedIndexChanged" AutoPostBack="true" Style="border: 2; height: 34px; padding: 6px 12px; font-size: 14px;">
                                                                                        <asp:ListItem Text="Amount" Value="Amount"></asp:ListItem>
                                                                                        <asp:ListItem Text="Percentage" Value="Percentage"></asp:ListItem>
                                                                                        <asp:ListItem Text="Group Tier" Value="Group Tier"></asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                </div>
                                                                                <br />
                                                                                <div id="commamt" runat="server">
                                                                                    <label id="lblcommamt" runat="server" class="col-sm-10 control-label col-lg-4">Comm Amount</label>
                                                                                    <div class="form-group col-sm-8">
                                                                                        <asp:TextBox ID="txtcommamt" runat="server" onkeypress="return IsNumberWithOneDecimal(this,event);" Text="0.00" AutoComplete="off"  Style="border: 2; height: 34px; padding: 6px 12px; font-size: 14px;"></asp:TextBox>
                                                                                    </div>
                                                                                </div>
                                                                                <br />

                                                                                <div id="commgroup" runat="server" visible="false">
                                                                                    <label class="col-sm-10 control-label col-lg-4">Comm Group</label>
                                                                                    <div class="form-group col-sm-8">
                                                                                        <asp:DropDownList ID="ddlcommgroup" AppendDataBoundItems="true" runat="server" Style="border: 2; height: 34px; padding: 6px 12px; font-size: 14px;">
                                                                                        </asp:DropDownList>
                                                                                    </div>
                                                                                </div>


                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                            <asp:HiddenField ID="TabName" runat="server" />
                                        
                                </div>
                                </div>
                                </div>


                                <div class="panel-heading" >
                                    <h4 class="panel-title">
                                        <a data-toggle="collapse" data-parent="#accordion" href="#collapse4" style="font-style:normal;" > Basic Item Details</a>
                                        </h4>
                                        </div>
                                        <div id="collapse4" class="panel-collapse collapse">
                                            <div class="panel-body">
                                         
                                
                                <div class="form-group has-success">
                                    <label class="col-sm-10 control-label col-lg-4" ></label>
                                    <div class="col-sm-8">
                                        <div>
                                            <div class="panel-group" id="accordion">
                                                 <div class="panel panel-default">
                                                    <div class="panel-heading" style="background-color: #696969; border-color: Black; border:1px; text-align:center">
                                                        <h4 class="panel-title">
                                                            <i data-toggle="collapse" data-parent="#accordion1" href="#collapse2" style="color: White; font-style:normal; cursor:pointer;">Select Category</i>
                                                        </h4>
                                                    </div>
                                                    <div id="collapse2" class="panel-collapse collapse">
                                                        <div class="panel-body" style=" background-color: #f9f9f9">
                                                            <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <div class="has-success">
                                                                        <label class="col-sm-10 control-label col-sm-3" >Department </label>
                                                                        <div class="form-group col-sm-8">
                                                                            <asp:DropDownList ID="ddlDept" runat="server" CssClass="col-md-12 col-xs-12" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlDept_SelectedIndexChanged" AutoPostBack="true" style="border:2; height:34px; padding:6px 12px; font-size:14px;"></asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                    <br />
                                                                    <div style="height: 30px" ></div>
                                                                    <div class="form-group has-success">
                                                                        <label class="col-sm-10 control-label col-sm-3" >Filter </label>
                                                                        <div class="col-sm-8">
                                                                            <asp:TextBox ID="txt_Filter" runat="server" AutoComplete="off" MaxLength="50" placeholder="Filter by category code / desc." OnTextChanged="txt_Filter_TextChanged" AutoPostBack="true" CssClass="col-md-12 col-xs-12" style="border:2; height:34px; padding:6px 12px; font-size:14px;"></asp:TextBox>
                                                                            <%--<AjaxControlToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Numbers" TargetControlID="txt_Filter" />--%>
                                                                        </div>
                                                                    </div>
                                                                    <br /><br />
                                                                    <div class="form-group has-success">
                                                                        <label class="col-sm-10 control-label col-sm-3" >Category </label>
                                                                        <div class="col-sm-8">
                                                                            <asp:DropDownList ID="ddlCat" runat="server" CssClass="col-md-12 col-xs-12" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlCat_SelectedIndexChanged" AutoPostBack="true" style="border:2; height:34px; padding:6px 12px; font-size:14px;" ></asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                    <br /><br />
                                                                    <div class="form-group has-success">
                                                                        <label class="col-sm-10 control-label col-sm-3" >Brand </label>
                                                                        <div class="col-sm-8">
                                                                            <asp:DropDownList ID="ddlBrd" runat="server" CssClass="col-md-12 col-xs-12" AppendDataBoundItems="true"  style="border:2; height:34px; padding:6px 12px; font-size:14px;" >
                                                                                <asp:ListItem Text="Others" Value="Others"></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </div>

                                                                    </div>


                                                                    <asp:Label ID="Label8" runat="server" ForeColor="Black" Font-Underline="false" Visible="false"></asp:Label>
                                                                  
                                                                    <label id="lblcatlast" style="color:red;display:none;">Last Selected Category: </label>
                                                                    <br />
                                                                    <asp:Label ID="Label2" runat="server" ForeColor="Black" style="display:none;" Font-Underline="false"  ></asp:Label>
                                                                    <asp:Label ID="Label4" runat="server" ForeColor="Black" style="display:none;" Font-Underline="false"  >--</asp:Label>
                                                                    <asp:Label ID="Label3" runat="server" ForeColor="Black" style="display:none;" Font-Underline="false"  ></asp:Label>

                                                                    <br />
                                                                    <asp:Label ID="Label5" runat="server" ForeColor="Black" Font-Underline="false"  ></asp:Label>
                                                                    <asp:Label ID="Label6" runat="server" ForeColor="Black" Font-Underline="false"  ></asp:Label>
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="ddlDept" EventName="SelectedIndexChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txt_Filter" EventName="TextChanged" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div> 
                                        </div>
                                    </div>
                                </div>
                                <br /><br />

                                                <!-- Supplier Selection Section -->
  
                                                <%-- <div class="form-group has-success">           
                                                    <label class="col-sm-10 control-label col-lg-4" ></label>           
                                                    <div class="col-sm-8">                   
                                                        <div>                          
                                                            <div class="panel-group" id="accordion">                                  
                                                                <div class="panel panel-default">                                          
                                                                    <div class="panel-heading" style="background-color: #696969; border-color: Black; border:1px; text-align:center">                                                  
                                                                        <h4 class="panel-title">                                                         
                                                                            <i data-toggle="collapse" data-parent="#accordion1" href="#collapse2" style="color: White; font-style:normal; cursor:pointer;">Select Supplier</i>                                                   
                                                                        </h4>                                         
                                                                        </div> 

                                                                    <!-- SUPPLIER 1 -->
                                                                    <div class="form-group has-success">    
                                                                        <label class="col-sm-10 control-label col-sm-3"> Supplier 1</label>    
                                                                        <div class="col-sm-8">        
                                                                            <asp:DropDownList ID="ddlSupplierName1" runat="server" CssClass="col-md-12 col-xs-12"  AppendDataBoundItems="true"  OnSelectedIndexChanged="ddlSupplierName1_SelectedIndexChanged" AutoPostBack="true" style="border:2; height:34px; padding:6px 12px; margin-top: 25px; margin-bottom: 25px; font-size:14px;">                                                                                        
                                                                            </asp:DropDownList>    

                                                                        </div>
                                                                    </div>
                                                                    <br /><br />
 
 
                                                                    <div class="form-group has-success">    
                                                                        <label class="col-sm-10 control-label col-sm-3">Supplier Code</label>      
                                                                        <div class="col-sm-8">    
                                                                            <asp:TextBox ID="txtSupplierCode1" runat="server"  CssClass="col-md-12 col-xs-12" ReadOnly="true"  style="border:2; height:34px; padding:6px 12px; margin-bottom: 25px; font-size:14px;">       
                                                                            </asp:TextBox>     
                                                                        </div>
                                                                    </div>
                                                                    <br /><br />
 
 
                                                                    <div class="form-group has-success">         
                                                                        <label class="col-sm-10 control-label col-sm-3">Default Supplier</label>         
                                                                        <div class="col-sm-8">                 
                                                                            <asp:RadioButtonList ID="rbDefaultSupplier1" runat="server" RepeatDirection="Horizontal" CssClass="col-md-12 col-xs-12" AutoPostBack="true" OnSelectedIndexChanged="rbDefaultSupplier1_SelectedIndexChanged" style="font-size:14px; padding:6px 12px; margin-bottom: 35px;">            
                                                                                <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>                                                                                         
                                                                                <asp:ListItem Text="No" Value="No" Selected="True"></asp:ListItem>                   
                                                                            </asp:RadioButtonList>         
                                                                        </div>
                                                                    </div>

                                                                    <!-- SUPPLIER 2 -->
                                                                    <div class="form-group has-success">        
                                                                        <label class="col-sm-10 control-label col-sm-3"> Supplier 2</label>        
                                                                        <div class="col-sm-8">                
                                                                            <asp:DropDownList ID="ddlSupplierName2" runat="server" CssClass="col-md-12 col-xs-12"  AppendDataBoundItems="true"  OnSelectedIndexChanged="ddlSupplierName2_SelectedIndexChanged" AutoPostBack="true" style="border:2; height:34px; padding:6px 12px; margin-top: 25px; margin-bottom: 25px; font-size:14px;">                                                                                                
                                                                            </asp:DropDownList>        
                                                                        </div>
                                                                    </div>
                                                                   <br /><br />
                                                
                                                <div class="form-group has-success">       
                                                    <label class="col-sm-10 control-label col-sm-3">Supplier Code</label>          
                                                    <div class="col-sm-8">            
                                                        <asp:TextBox ID="txtSupplierCode2" runat="server"  CssClass="col-md-12 col-xs-12" ReadOnly="true"  style="border:2; height:34px; padding:6px 12px; margin-bottom: 60px; font-size:14px;">               
                                                        </asp:TextBox>         
                                                    </div>
                                                </div>
                                                <br /><br />
                                                
                                                <div class="form-group has-success">         
                                                    <label class="col-sm-10 control-label col-sm-3">Default Supplier</label>         
    
                                                    <div class="col-sm-8">                         
                                                        <asp:RadioButtonList ID="rbDefaultSupplier2" runat="server" RepeatDirection="Horizontal" CssClass="col-md-12 col-xs-12" AutoPostBack="true" OnSelectedIndexChanged="rbDefaultSupplier2_SelectedIndexChanged" style="font-size:14px; padding:6px 12px; margin-bottom: 35px;">                        
                                                            <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>                                                                                         
                                                                        <asp:ListItem Text="No" Value="No" Selected="True"></asp:ListItem>              
                                                        </asp:RadioButtonList>           
                                                    </div>
                                                </div>
                                                                     <!-- SUPPLIER 3 -->
                                                                    <div class="form-group has-success">       
                                                                        <label class="col-sm-10 control-label col-sm-3"> Supplier 3</label>        
                                                                        <div class="col-sm-8">               
                                                                            <asp:DropDownList ID="ddlSupplierName3" runat="server" CssClass="col-md-12 col-xs-12"  AppendDataBoundItems="true"  OnSelectedIndexChanged="ddlSupplierName3_SelectedIndexChanged" AutoPostBack="true" style="border:2; height:34px; padding:6px 12px; margin-top: 25px; margin-bottom: 25px; font-size:14px;">                                                                                             
                                                                            </asp:DropDownList>        
                                                                        </div>
                                                                    </div>
                                                <br /><br />
 
 
                                                <div class="form-group has-success">      
                                                    <label class="col-sm-10 control-label col-sm-3">Supplier Code</label>         
                                                    <div class="col-sm-8">            
                                                        <asp:TextBox ID="txtSupplierCode3" runat="server"  CssClass="col-md-12 col-xs-12" ReadOnly="true"  style="border:2; height:34px; padding:6px 12px; margin-bottom: 25px; font-size:14px;">               
                                                        </asp:TextBox>        
                                                    </div>
                                                </div>
                                                <br /><br />
                                                
                                                <div class="form-group has-success">             
                                                    <label class="col-sm-10 control-label col-sm-3">Default Supplier</label>             
                                                    <div class="col-sm-8">                       
                                                        <asp:RadioButtonList ID="rbDefaultSupplier3" runat="server" RepeatDirection="Horizontal" CssClass="col-md-12 col-xs-12" AutoPostBack="true" OnSelectedIndexChanged="rbDefaultSupplier3_SelectedIndexChanged"  style="font-size:14px; padding:6px 12px; margin-bottom: 40px;">                        
                                                            <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>                                                                                                    
                                                            <asp:ListItem Text="No" Value="No" Selected="True"></asp:ListItem>                           
                                                        </asp:RadioButtonList>            
                                                    </div>
                                                </div> 
                                                     <br /><br /> --%>

<asp:UpdatePanel ID="updSuppliers" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:GridView ID="grdSuppliers" runat="server" ShowFooter="True" AutoGenerateColumns="false"
            DataKeyNames="Ids" ForeColor="#333333" GridLines="None" Width="100%"
            OnRowCommand="grdSuppliers_RowCommand" OnRowDataBound="grdSuppliers_RowDataBound"
            OnRowEditing="grdSuppliers_RowEditing" OnRowUpdating="grdSuppliers_RowUpdating"
            OnRowCancelingEdit="grdSuppliers_RowCancelingEdit" OnRowDeleting="grdSuppliers_RowDeleting">

            <Columns>
                <%-- Action buttons --%>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" ToolTip="Edit"
                            CommandArgument='<%# Eval("Supplier_Code") %>'><img src="Images/img/pencil.png" /></asp:LinkButton>
                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete"
                            ToolTip="Delete" CommandArgument='<%# Eval("Supplier_Code") %>'
                            OnClientClick="return confirm('Delete this supplier?');"><img src="Images/img/bin.png" /></asp:LinkButton>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:LinkButton ID="lnkSave" runat="server" CommandName="Update" ToolTip="Save"><img src="Images/img/checked.png" /></asp:LinkButton>
                        <asp:LinkButton ID="lnkCancel" runat="server" CommandName="Cancel" ToolTip="Cancel"><img src="Images/img/remove.png" /></asp:LinkButton>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:LinkButton ID="lnkInsert" runat="server" CommandName="InsertNew" ToolTip="Add Supplier"><img src="Images/img/plus.png" /></asp:LinkButton>
                    </FooterTemplate>
                </asp:TemplateField>

                <%-- Supplier Code --%>
                <asp:TemplateField HeaderText="Supplier Code">
                    <ItemTemplate>
                        <asp:Label ID="lblSupplierCode" runat="server" Text='<%# Bind("Supplier_Code") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtSupplierCodeNew" runat="server" ReadOnly="true" Width="120px" />
                    </FooterTemplate>
                </asp:TemplateField>

                <%-- Supplier Name --%>
                <asp:TemplateField HeaderText="Supplier Name">
                    <ItemTemplate>
                        <asp:Label ID="lblSupplierName" runat="server" Text='<%# Bind("Supplier_Name") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddlSupplierNameNew" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlSupplierNameNew_SelectedIndexChanged"
                            CssClass="select2supplier" Width="220px" />
                    </FooterTemplate>
                </asp:TemplateField>

                <%-- Default Supplier --%>
                <asp:TemplateField HeaderText="Default Supplier" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="lblIsDefault" runat="server" Text='<%# Bind("IsDefault") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlDefault" runat="server">
                            <asp:ListItem Text="No" Value="No"></asp:ListItem>
                            <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddlDefaultNew" runat="server" Width="80px">
                            <asp:ListItem Text="No" Value="No" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                        </asp:DropDownList>
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>

            <HeaderStyle BackColor="#696969" Font-Bold="True" Height="46px" ForeColor="White" />
            <RowStyle BackColor="#ffffff" />
            <FooterStyle BackColor="#ffffff" />
        </asp:GridView>
    </ContentTemplate>

    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="grdSuppliers" EventName="RowCommand" />
        <asp:AsyncPostBackTrigger ControlID="grdSuppliers" EventName="RowUpdating" />
        <asp:AsyncPostBackTrigger ControlID="grdSuppliers" EventName="RowDeleting" />
        <asp:AsyncPostBackTrigger ControlID="grdSuppliers" EventName="RowEditing" />
        <asp:AsyncPostBackTrigger ControlID="grdSuppliers" EventName="RowCancelingEdit" />
    </Triggers>
</asp:UpdatePanel>

 
                                                              
                                <div class="has-success">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                            <label class="col-sm-10 control-label col-lg-4" >Category Code</label>
                                            <div class="form-group col-sm-8">
                                                <asp:TextBox ID="txt_Category" runat="server" CssClass="col-md-12 col-xs-12" style="border:2; height:34px; padding:6px 12px; font-size:14px;" Enabled="false"></asp:TextBox>
                                            </div>
                                        </div>
                                    
                                <br />
                                <div style="height: 30px" ></div>
                                         </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlCat" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                                     <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                <div class="has-success">
                                    <label class="col-sm-10 control-label col-lg-4" >Item Code</label>
                                    <div class="form-group col-sm-8">
                                        <asp:TextBox ID="txt_ItemCode" runat="server" Enabled="false" placeholder="Example : 4122287731" AutoComplete="off" CssClass="col-md-12 col-xs-12" style="border:2; height:34px; padding:6px 12px; font-size:14px;"></asp:TextBox>
                                    </div>
                                </div>
                                <br />
                                <div style="height: 10px" ></div>
                                         </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlCat" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                          <div class="has-success">
                                            <label class="col-sm-10 control-label col-lg-4">Weight Item</label>
                                            <div class="form-group col-sm-8">
                                                <asp:DropDownList ID="ddlweightitem" runat="server" OnSelectedIndexChanged="ddlweightitem_SelectedIndexChanged" AutoPostBack="true" style="border:2; height:34px; padding:6px 12px; font-size:14px;">
                                                    <asp:ListItem Text="YES" Value="YES"></asp:ListItem>
                                                    <asp:ListItem Text="NO" Value="NO" Selected></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div id="divweighttype" runat="server" class="has-success" visible="false">
                                            <label class="col-sm-10 control-label col-lg-4">Weight Type</label>
                                            <div class="form-group col-sm-8">
                                               <asp:RadioButtonList ID="RDweighttype" OnSelectedIndexChanged="RDweighttype_SelectedIndexChanged" AutoPostBack="true" runat="server">
                                                   <asp:ListItem Text="KG" Value="KG"></asp:ListItem>
                                                   <asp:ListItem Text="PCS" Value="PCS"></asp:ListItem>
                                               </asp:RadioButtonList>
                                            </div>
                                        </div>
                                        <div class="has-success">
                                            <label class="col-sm-10 control-label col-lg-4">Barcode</label>
                                            <div class="form-group col-sm-8">
                                                <asp:TextBox ID="txt_Barcode" runat="server" placeholder="Example : 543293809" AutoComplete="off" CssClass="col-md-12 col-xs-12" style="border:2; height:34px; padding:6px 12px; font-size:14px;" ></asp:TextBox>
                                                <AjaxControlToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Numbers, Custom" ValidChars=",.'()`+-*/& " TargetControlID="txt_Barcode" /> 
                                            </div>
                                        </div>
                                       
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlCat" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                                <br />
                                  
                                    <br />
                                <div style="height: 30px" ></div>
                                <div class="has-success">
                                    <label class="col-sm-10 control-label col-lg-4" >Type</label>
                                    <div class="form-group col-sm-8">
                                        <asp:DropDownList ID="ddlType" runat="server" OnSelectedIndexChanged="ddlType_SelectedIndexChanged" AutoPostBack="true" style="border:2; height:34px; padding:6px 12px; font-size:14px;">
                                            <asp:ListItem Text="Standard" Value="1" Selected="True"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>       
                                </div>
                                    <br />
                                    <div style="height: 30px"></div>
                                    <div class="has-success">
                                        <label class="col-sm-10 control-label col-lg-4">FOC</label>
                                        <div class="form-group col-sm-8">
                                            <asp:DropDownList ID="ddlfoc" runat="server"  Style="border: 2; height: 34px; padding: 6px 12px; font-size: 14px;">
                                                <asp:ListItem Value="No" Text="No"></asp:ListItem>
                                                <asp:ListItem Value="Yes" Text="Yes"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <br />
                                    <div style="height: 30px"></div>
                                    <div class="has-success">
                                        <label class="col-sm-10 control-label col-lg-4">Allow In QR Page</label>
                                        <div class="form-group col-sm-8">
                                            <asp:DropDownList ID="ddlordertable" runat="server"  Style="border: 2; height: 34px; padding: 6px 12px; font-size: 14px;">
                                                <asp:ListItem Value="No" Text="No"></asp:ListItem>
                                                <asp:ListItem Value="Yes" Text="Yes"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <br />
                                <div style="height: 30px" ></div>
                                
                                <br />
                                    
                                <div style="height: 30px"></div>
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                <div id="LinkPanel" runat="server" class="has-success" visible="false">
                                    <label class="col-sm-10 control-label col-lg-4" ></label>
                                    <div class="form-group col-sm-8">
                                        <div>
                                            <div class="panel-group" id="Div1">
                                                <div class="panel panel-default">
                                                    <div class="panel-heading" style="background-color: #007A6E; border-color: Black; border:1px; text-align:center">
                                                        <h4 class="panel-title">
                                                            <i  style="color: White; font-style:normal; cursor:pointer;">Select Linkcode</i>
                                                        </h4>
                                                    </div>
                                                    <div id="collapse3" class="panel-collapse ">
                                                        <div class="panel-body" style=" background-color: #f9f9f9">
                                                            <div >
                                                                        <div class="form-group has-success">
                                                                            <label class="col-sm-10 control-label col-lg-4" >Filter </label>
                                                                            <div class="col-sm-8">
                                                                                <asp:TextBox ID="txt_Filter2" runat="server" placeholder="Filter by " OnTextChanged="txt_Filter2_TextChanged" AutoPostBack="true" CssClass="col-md-12 col-xs-12" style="border:2; height:34px; padding:6px 12px; font-size:14px;"></asp:TextBox>
                                                                                <AjaxControlToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Numbers, Custom" ValidChars=",.'()`+-*/& " TargetControlID="txt_Filter2" />
                                                                            </div>
                                                                        </div>
                                                                        <br />
                                                                        <div style="height: 30px" ></div>
                                                                        <div class="form-group has-success">
                                                                            <label class="col-sm-10 control-label col-lg-4" >Select Item </label>
                                                                            <div class="col-sm-8">
                                                                                <asp:DropDownList ID="ddlItem" runat="server" CssClass="col-md-12 col-xs-12" OnSelectedIndexChanged="ddlItem_SelectedIndexChanged" AutoPostBack="true" AppendDataBoundItems="true" style="border:2; height:34px; padding:6px 12px; font-size:14px;" ></asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div> 
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="txt_Filter2" EventName="TextChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="ddlType" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                                <br /><br />
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div class="has-success" style="display:none;">
                                            <label class="col-sm-10 control-label col-lg-4" for="inputSuccess">Linkcode</label>
                                            <div class="form-group col-sm-8">
                                                <asp:TextBox ID="txt_Subcode" runat="server" CssClass="col-md-12 col-xs-12" style="border:2; height:34px; padding:6px 12px; font-size:14px;" Enabled="false"></asp:TextBox>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlItem" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="ddlType" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="ddlcat" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="txt_ItemCode" EventName="TextChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                                <br />
                                <div style="height: 30px" ></div>
                                <div class="has-success">
                                    <label class="col-sm-10 control-label col-lg-4" >Description</label>
                                    <div class="form-group col-sm-8">
                                        <asp:TextBox ID="txt_Desc" runat="server" placeholder="Example : Curry Chicken" AutoComplete="off"  CssClass="col-md-12 col-xs-12" onkeydown="return (event.keycode!=13)" oninput="updateTextbox2()" style="border:2; height:34px; padding:6px 12px; font-size:14px;"  ></asp:TextBox>

                                    </div>       
                                </div>
                                     <br />
                                <div style="height: 30px" ></div>
                                <div class="has-success">
                                    <label class="col-sm-10 control-label col-lg-4" >Description 2 <span style="font-size:small; color:red;"> (Optional)</span></label>
                                    <div class="form-group col-sm-8">
                                        <asp:TextBox ID="txt_Desc2" runat="server" AutoComplete="off" CssClass="col-md-12 col-xs-12" onkeydown="return (event.keycode!=13)" style="border:2; height:34px; padding:6px 12px; font-size:14px;"  ></asp:TextBox>

                                    </div>       
                                </div>
                                <br /><br />
                                <div class="has-success">
                                    <label class="col-sm-10 control-label col-lg-4" >Short Description<span style="font-size:small; color:red;"> * Only show in POS</span></label>
                                    <div class="form-group col-sm-8">
                                        <asp:updatepanel ID="updateSD" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                        <asp:TextBox ID="txt_ShortDesc" runat="server" placeholder="Example : Chicken" MaxLength="35" CssClass="col-md-12 col-xs-12" onkeydown="return (event.keycode!=13)" style="border:2; height:34px; padding:6px 12px; font-size:14px;"  ></asp:TextBox>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="txt_Desc" EventName="TextChanged" />
                                        </Triggers>
                                        </asp:updatepanel>
                                    </div>
                                </div>
                                <br /><br />
                                <div class="has-success" style="display:none;">
                                    <label class="col-sm-10 control-label col-lg-4" for="inputSuccess">Other Description</label>
                                    <div class="form-group col-sm-8">
                                        <asp:TextBox ID="txt_OtherDesc" runat="server" placeholder="Example : Curry" AutoComplete="off" CssClass="col-md-12 col-xs-12" style="border:2; height:34px; padding:6px 12px; font-size:14px;" ></asp:TextBox>
                                        <%--<AjaxControlToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender17" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Numbers, Custom" ValidChars=",.'()`+-*/& " TargetControlID="txt_OtherDesc" />--%>
                                    </div>
                                </div>
                                <br /><br />
                                <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div class="has-success" style="display:none;">
                                            <label class="col-sm-10 control-label col-lg-4" >Pack Size</label>
                                            <div class="form-group col-sm-8">
                                                <asp:TextBox ID="txt_Pack" runat="server" AutoComplete="off" CssClass="col-md-12 col-xs-12" style="border:2; height:34px; padding:6px 12px; font-size:14px;" ></asp:TextBox>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlType" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="ddlType" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                                <br />
                                <div style="height: 30px" ></div>
                                    <asp:UpdatePanel ID="UpdatePanel10" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <div class="has-success">
                                                <label class="col-sm-10 control-label col-lg-4">Unit of Measurement</label>
                                                <div class="form-group col-sm-8">
                                                    <asp:DropDownList ID="ddlUM" runat="server" Style="border: 2; height: 34px; padding: 6px 12px; font-size: 14px;" AppendDataBoundItems="true"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="RDweighttype" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlweightitem" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                <br />
                                <div style="height: 30px" ></div>
                                <div class="has-success">
                                    <label class="col-sm-10 control-label col-lg-4" >Weight (KG)</label>
                                    <div class="form-group col-sm-8">
                                        <asp:TextBox ID="txtweight" runat="server" AutoComplete="off" onkeypress="return IsNumberWithOneDecimal2(this,event);" placeholder="e.g. 0.00" CssClass="col-md-12 col-xs-12" style="border:2; height:34px; padding:6px 12px; font-size:14px;" ></asp:TextBox>
                                        <asp:Label ID="lbllimit" runat="server" Visible="false" Text=" *Weight Limit is 1000 kg" ForeColor="red"></asp:Label>                 
                                    </div>
                                </div>
                                    <br />
                                    <div style="height: 30px" ></div>
                                <div class="has-success">
                                    <label class="col-sm-10 control-label col-lg-4" >Cost</label>
                                    <div class="form-group col-sm-8">
                                        <asp:TextBox ID="txtvendorcost" runat="server" AutoComplete="off" onkeypress="return IsNumberWithOneDecimal(this,event);" ondrop="return false;" onpaste="return false;" placeholder="e.g. 0.00" CssClass="col-md-12 col-xs-12" style="border:2; height:34px; padding:6px 12px; font-size:14px;" ></asp:TextBox>
                                    </div>
                                </div>
                                    <br />
                                     <div style="height: 30px" ></div>
                                <div class="has-success">
                                    <label class="col-sm-10 control-label col-lg-4" >Max Qty Per Transaction</label>
                                    <div class="form-group col-sm-8">
                                        <asp:TextBox ID="txtmaxbuy" runat="server" AutoComplete="off" onkeypress="return IsNumberWithOneDecimal(this,event);" ondrop="return false;" onpaste="return false;" placeholder="e.g. 0.00" CssClass="col-md-12 col-xs-12" style="border:2; height:34px; padding:6px 12px; font-size:14px;" ></asp:TextBox>
                                    </div>
                                </div>
                                     <br />
                                     <div class="has-success">
                                  <label id="Label15" runat="server" class="col-sm-10 control-label col-lg-4" >Ezyshare Overriding (%)</label>
                                  <div class="form-group col-sm-8">
                                       <asp:TextBox ID="txtESProfit" runat="server" MaxLength="6" onkeypress="return IsNumberWithOneDecimal2(this,event)" Text="0" AutoComplete="off" CssClass="col-md-12 col-xs-12 col-lg-12" style="border:2; height:34px; padding:6px 12px; font-size:14px;" ></asp:TextBox>
                                  </div>
                                         </div>
                                <br /><br />
                                <div class="has-success" style="display:none;">
                                    <label class="col-sm-10 control-label col-lg-4" >Purchase Tax</label>
                                    <div class="form-group col-sm-8">
                                        <asp:DropDownList ID="ddlPT" runat="server" AppendDataBoundItems="true" CssClass="col-md-12 col-xs-12" style="border:2; height:34px; padding:6px 12px; font-size:14px;"></asp:DropDownList>
                                    </div>       
                                </div>
                                <br /><br />
                                <div class="has-success" style="display:none;">
                                    <label class="col-sm-10 control-label col-lg-4">Sales Tax</label>
                                    <div class="form-group col-sm-8">
                                        <asp:DropDownList ID="ddlST" runat="server" AppendDataBoundItems="true" CssClass="col-md-12 col-xs-12" style="border:2; height:34px; padding:6px 12px; font-size:14px;"></asp:DropDownList>
                                    </div>
                                </div>
                              
                               
                            </div>
                        </div>
                 <div align="center">
                <asp:Button ID="btn_Back" runat="server" Text="Back" OnClick="Back_OnClick" CssClass="ghost-button-thick-border " CausesValidation="false" style="float:right" />
                    <asp:Button ID="btn_Insert" runat="server" Text="Insert" OnClientClick="ShowDiv()" OnClick="Insert_OnClick" CausesValidation="true" CssClass="ghost-button-thick-border" style="float:right"  />
                    <asp:Button ID="btn_Update" runat="server" Text="Update" OnClientClick="ShowDiv()" OnClick="Update_OnClick" CssClass="ghost-button-thick-border" style="float:right"/>
       
       </div>
       <div class="overlay" style="display: none;" id="PB">
        <div class="modalprogress">
            <div class="theprogress">
                <img alt="Loading" src="Images/GIF/spin.gif" width="80px" height="80px"/>
                                                             
            </div>
        </div>
    </div>

       <script type="text/javascript">
           function ShowDiv() { setTimeout('document.getElementById("PB").style.display ="inline";', 500); }

    </script>
                
                </div>
                </div>
                </div>
               </div>

        </section><!-- wrapper -->
    </section><!-- MAIN CONTENT -->
  <script src="js/jquery-1.10.2.min.js" type="text/javascript"></script>  
<script src="js/jquery-te-1.4.0.min.js" type="text/javascript"></script>  
<script language="javascript" type="text/javascript">  
    $('.textEditor1').jqte();
    $(".textEditor").jqte({
        blur: function () {
            document.getElementById('<%=hdText.ClientID %>').value = document.getElementById('<%=txtEditor.ClientID %>').value;
    }
});  
</script>
</asp:Content>

