<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ModifierOptionSetting.aspx.cs" Inherits="ModifierOptionSetting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .flex-container {
            display: grid;
        }

        .switch {
            position: relative;
            display: inline-block;
            width: 60px;
            height: 34px;
        }

            .switch input {
                opacity: 0;
                width: 0;
                height: 0;
            }

        .slider {
            position: absolute;
            cursor: pointer;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background-color: #ccc;
            -webkit-transition: .4s;
            transition: .4s;
        }

            .slider:before {
                position: absolute;
                content: "";
                height: 26px;
                width: 26px;
                left: 4px;
                bottom: 4px;
                background-color: white;
                -webkit-transition: .4s;
                transition: .4s;
            }

        input:checked + .slider {
            background-color: #2196F3;
        }

        input:focus + .slider {
            box-shadow: 0 0 1px #2196F3;
        }

        input:checked + .slider:before {
            -webkit-transform: translateX(26px);
            -ms-transform: translateX(26px);
            transform: translateX(26px);
        }

        /* Rounded sliders */
        .slider.round {
            border-radius: 34px;
        }

            .slider.round:before {
                border-radius: 50%;
            }
    </style>
    <script type="text/javascript" src="scripts/sweetalert2.all.min.js"></script>
    <script>
        function IsNumberWithOneDecimal(txt, evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode

            var inputText = document.getElementById('<%= txtoptionprice.ClientID %>').value;

            if (charCode > 31 && (charCode < 48 || charCode > 57) && !(charCode == 46 || charCode == 8)) {
                return false;
            } else {
                var len = txt.value.length;
                var index = txt.value.indexOf('.');

                if (charCode === 46 && inputText.length === 0) {
                    return false;
                }

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
        function sweetalert_warning(message, messagetype) {
            Swal.fire({
                position: 'center',
                icon: messagetype,
                title: message,
                showConfirmButton: false
            })
            setTimeout(function () { redirect(); }, 1500);

        }


        function sweetalert_error(message, messagetype) {
            Swal.fire({
                position: 'center',
                icon: messagetype,
                title: message,
                showConfirmButton: true
            })

        }
    </script>
    <asp:UpdatePanel ID="updatepanel1" runat="server">
        <ContentTemplate>
            <asp:Label ID="MF_Con" runat="server" Text="Label" Visible="false"></asp:Label>
            <asp:HiddenField ID="hd_repairerID" runat="server" />
            <div class="col-xs-6 col-lg-6" style="background-color: white; height: auto; width: 100%"></div>
            <div class="form-group has-success clear flex-container">
                <label class="col-sm-10  col-sm-4">Modifier Group Name </label>
                <div class="col-sm-8">
                    <asp:HiddenField runat="server" ID="hdnModifirrID" />
                    <asp:TextBox ID="txtModifierName" runat="server" CssClass="col-md-12 col-xs-12" Style="border: 2; height: 34px; padding: 6px 12px; font-size: 14px;"></asp:TextBox>
                    <asp:RequiredFieldValidator EnableClientScript="true" ID="RequiredFieldValidator1" runat="server" ValidationGroup="delidate" ControlToValidate="txtModifierName" ForeColor="Red" ErrorMessage="* Panel repairer name required" Display="Dynamic" />
                </div>
            </div>
            <div class="form-group has-success clear flex-container">
                
                <label class="col-sm-10  col-sm-4">Remark </label>
                <div class="col-sm-8">
                <asp:TextBox ID="txtremarks" runat="server" CssClass="col-md-12 col-xs-12" Style="border: 2; height: 34px; padding: 6px 12px; font-size: 14px;"></asp:TextBox>
                </div>
            </div>
             <div class="form-group has-success clear flex-container col-lg-6">
                <table style="padding:5px">
                    <tr style="padding:5px">
                        <td style="padding:5px">
                            <label class="col-sm-6 col-sm-6">Min Selection <span style="font-size: small; color: red"></span></label>
                        </td>
                        <td style="padding:5px">
                            <label class="col-sm-6 col-sm-6">Max Selection <span style="font-size: small; color: red"></span></label>
                        </td>
                    </tr>
                    <tr style="padding:5px">
                        <td style="padding:5px">
                            <asp:TextBox ID="txtminselection" runat="server" MaxLength="2" onkeypress="return IsNumberWithNoDecimal(this,event);" placeholder="" CssClass="col-md-6 col-xs-6" Style="border: 2; height: 34px; padding: 6px 12px; font-size: 14px;"></asp:TextBox>
                            <asp:RequiredFieldValidator EnableClientScript="true" ID="RequiredFieldValidator3" runat="server" ValidationGroup="delidate" ControlToValidate="txtminselection" ForeColor="Red" ErrorMessage="* Min selection required" Display="Dynamic" />
                        </td>
                        <td style="padding:5px">
                            <asp:TextBox ID="txtmaxselection" runat="server" MaxLength="2" onkeypress="return IsNumberWithNoDecimal(this,event);" placeholder="" CssClass="col-md-6 col-xs-6" Style="border: 2; height: 34px; padding: 6px 12px; font-size: 14px;"></asp:TextBox>
                            <asp:RequiredFieldValidator EnableClientScript="true" ID="RequiredFieldValidator4" runat="server" ValidationGroup="delidate" ControlToValidate="txtmaxselection" ForeColor="Red" ErrorMessage="* Max selection required" Display="Dynamic" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span style="color:red">* Min selection set 0 as optional select</span>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="form-group has-success clear flex-container">
                <label class="col-sm-12 col-sm-12">Option</label>
                <div class="col-lg-12 col-sm-12">
                    <table style="width: 100%">
                        <tr>
                            <td style="padding: 5px;">
                                <label class="col-sm-12 col-sm-12">Option Name</label>
                            </td>
                            <td style="padding: 5px;">
                                <label class="col-sm-12 col-sm-12">Price</label>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td style="padding: 5px;">
                                <asp:TextBox ID="txtoption" runat="server" CssClass="col-md-12 col-xs-12" Style="border: 2; height: 34px; padding: 6px 12px; font-size: 14px;"></asp:TextBox>
                                <asp:RequiredFieldValidator EnableClientScript="true" ID="RequiredFieldValidator2" runat="server" ValidationGroup="option" ControlToValidate="txtoption" ForeColor="Red" ErrorMessage="* Option required" Display="Dynamic" />
                            </td>
                            <td style="padding: 5px;">
                                <asp:TextBox ID="txtoptionprice" runat="server" onkeypress="return IsNumberWithOneDecimal(this,event);" CssClass="col-md-12 col-xs-12" Style="border: 2; height: 34px; padding: 6px 12px; font-size: 14px;" Text="0"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button ID="btnadd" runat="server" CssClass="btn btn-success" ValidationGroup="option" Text="Add" Style="padding: 5px 32px !important" OnClick="btnadd_Click" />

                            </td>
                        </tr>
                    </table>
                </div>
                <div class="col-sm-12" style="margin-top: 10px">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h4 class="panel-title">
                                <i data-toggle="collapse" data-parent="#accordion1" href="#collapse9" style="font-style: normal; cursor: pointer;">Option</i>
                            </h4>
                        </div>
                        <div id="collapse9" class="panel-collapse collapse in">
                            <div class="panel-body" style="background-color: #f9f9f9">
                                  <asp:GridView ID="grdoptionlist" runat="server" OnRowDataBound="grdoptionlist_RowDataBound"
                                    OnRowDeleting="grdoptionlist_RowDeleting" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover"
                                    ShowHeaderWhenEmpty="true" EmptyDataText="No record found">
                                    <Columns>
                                        <asp:CommandField ButtonType="Button" ShowDeleteButton="True" DeleteImageUrl="~/Images/Deleteimg.png" />
                                        <asp:TemplateField ItemStyle-Width="120" HeaderText="Option">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdrunno" runat="server" Value='<%# Bind("runno") %>' />
                                                <asp:Label ID="lbloption" runat="server" Text='<%# Bind("Option_Name") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Price">
                                            <ItemTemplate>
                                                <asp:Label ID="lbloptionPrice" runat="server" Text='<%# Bind("Option_Price") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>

                                <asp:GridView ID="grdoptionlist2" runat="server" OnRowDataBound="grdoptionlist_RowDataBound" OnRowCommand="grdoptionlist2_RowCommand"
                                    OnRowDeleting="grdoptionlist_RowDeleting" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover"
                                    ShowHeaderWhenEmpty="true" EmptyDataText="No record found">
                                    <Columns>
                                           <asp:TemplateField HeaderText="Price">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkbtndelete" CommandArgument='<%# Bind("ids") %>' CommandName="optiondelete" Text="Delete" CssClass="btn btn-danger" runat="server"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-Width="120" HeaderText="Option">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdrunno" runat="server" Value='<%# Bind("runno") %>' />
                                                <asp:Label ID="lbloption" runat="server" Text='<%# Bind("Option_Name") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Price">
                                            <ItemTemplate>
                                                <asp:Label ID="lbloptionPrice" runat="server" Text='<%# Bind("Option_Price") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
           
            <div class="form-group has-success clear flex-container">
                <div style="text-align: center;">
                    <asp:Button ID="btnAddC" runat="server" Text="Create" OnClick="btnAddC_Click" ValidationGroup="delidate" CssClass="ghost-button-thick-border" Style="float: right" />
                    <asp:Button ID="btnUpdateC" runat="server" Text="Update" OnClick="btnUpdateC_Click" ValidationGroup="delidate" CssClass="ghost-button-thick-border" Visible="false" Style="float: right" />
                    <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" CssClass="ghost-button-thick-border" Style="float: left" />
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

