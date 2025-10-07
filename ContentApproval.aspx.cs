using AjaxControlToolkit;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office.Word;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Xceed.Wpf.Toolkit;

public partial class BItemListing : System.Web.UI.Page
{
    Business BL = new Business();
    DBClass DL = new DBClass();
    protected static string DBCon;
    public static string Logout;
    public int cLine = 0;
    public int pLine = 0;
    public int promoLine = 0;
    public int count;
    public string id;
    public static string merchant;
    public string searchresult;
    protected static String strConStringrp = ConfigurationManager.AppSettings["ConnectionString"].ToString();
    public enum MessageType { success, error, info, warning };
    //public int cLine = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["merchant"] != null)
            {
                merchant = Request.QueryString["merchant"].ToString();

                BindDepartments(merchant);
                BindCategories(merchant, "");
                ItemLoadList(1);

            }
            else
            {
                merchant = Request.QueryString["merchant"].ToString();
            }

        }
    }

    private void ItemLoadList(int pageIndex)
    {
        using (SqlConnection con = new SqlConnection(strConStringrp))
        {
            using (SqlCommand cmd = new SqlCommand("Content_Listing", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PageIndex", pageIndex);
                cmd.Parameters.AddWithValue("@PageSize", int.Parse("20"));
                cmd.Parameters.AddWithValue("@FilterStatus", "%" + ddlStatus.SelectedValue + "%");
                cmd.Parameters.AddWithValue("@FilterPublish", "%" + ddlPublish.SelectedValue + "%");
                cmd.Parameters.AddWithValue("@FilterSoldStatus", "%" + ddlSoldStatus.SelectedValue + "%");
                cmd.Parameters.AddWithValue("@FilterDepartment", "%" + ddlDepartment.SelectedValue + "%");
                cmd.Parameters.AddWithValue("@FilterCategory", "%" + ddlCategory.SelectedValue + "%");
                cmd.Parameters.AddWithValue("@FilterText", "%" + txt_Search.Text.Trim().Replace("'", "`") + "%");
                cmd.Parameters.AddWithValue("@FilterMerchant", Request.QueryString["merchant"].ToString());

                cmd.Parameters.Add("@RecordCount", SqlDbType.Int, 4);
                cmd.Parameters["@RecordCount"].Direction = ParameterDirection.Output;
                con.Open();

                SqlDataReader idr = cmd.ExecuteReader();

                DataTable v = new DataTable();
                v.Load(idr);
                rpt_Item.DataSource = v;
                rpt_Item.DataBind();

                idr.Close();
                con.Close();

                int recordCount = Convert.ToInt32(cmd.Parameters["@RecordCount"].Value);

                lbl_Record2.Text = recordCount.ToString();

                if (recordCount > 20)
                {
                    this.PopulatePager(recordCount, pageIndex);
                }
                else
                {
                    this.PopulatePager(20, pageIndex);
                }
            }
        }
    }

    private void ZeroItemLoadList(int pageIndex)
    {
        using (SqlConnection con = new SqlConnection(strConStringrp))
        {
            using (SqlCommand cmd = new SqlCommand("ContentZeroCost_Listing", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PageIndex", pageIndex);
                cmd.Parameters.AddWithValue("@PageSize", int.Parse("20"));
                cmd.Parameters.AddWithValue("@FilterStatus", "%" + ddlStatus.SelectedValue + "%");
                cmd.Parameters.AddWithValue("@FilterPublish", "%" + ddlPublish.SelectedValue + "%");
                cmd.Parameters.AddWithValue("@FilterSoldStatus", "%" + ddlSoldStatus.SelectedValue + "%");
                cmd.Parameters.AddWithValue("@FilterDepartment", "%" + ddlDepartment.SelectedValue + "%");
                cmd.Parameters.AddWithValue("@FilterCategory", "%" + ddlCategory.SelectedValue + "%");
                cmd.Parameters.AddWithValue("@FilterText", "%" + txt_Search.Text.Trim().Replace("'", "`") + "%");
                cmd.Parameters.AddWithValue("@FilterMerchant", Request.QueryString["merchant"].ToString());

                cmd.Parameters.Add("@RecordCount", SqlDbType.Int, 4);
                cmd.Parameters["@RecordCount"].Direction = ParameterDirection.Output;
                con.Open();

                SqlDataReader idr = cmd.ExecuteReader();

                DataTable v = new DataTable();
                v.Load(idr);
                rpt_Item.DataSource = v;
                rpt_Item.DataBind();

                idr.Close();
                con.Close();

                int recordCount = Convert.ToInt32(cmd.Parameters["@RecordCount"].Value);

                lbl_Record2.Text = recordCount.ToString();

                if (recordCount > 20)
                {
                    this.PopulatePager(recordCount, pageIndex);
                }
                else
                {
                    this.PopulatePager(20, pageIndex);
                }
            }
        }
    }

    private void UnderItemLoadList(int pageIndex)
    {
        using (SqlConnection con = new SqlConnection(strConStringrp))
        {
            using (SqlCommand cmd = new SqlCommand("ContentUnderCost_Listing", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PageIndex", pageIndex);
                cmd.Parameters.AddWithValue("@PageSize", int.Parse("20"));
                cmd.Parameters.AddWithValue("@FilterStatus", "%" + ddlStatus.SelectedValue + "%");
                cmd.Parameters.AddWithValue("@FilterPublish", "%" + ddlPublish.SelectedValue + "%");
                cmd.Parameters.AddWithValue("@FilterSoldStatus", "%" + ddlSoldStatus.SelectedValue + "%");
                cmd.Parameters.AddWithValue("@FilterDepartment", "%" + ddlDepartment.SelectedValue + "%");
                cmd.Parameters.AddWithValue("@FilterCategory", "%" + ddlCategory.SelectedValue + "%");
                cmd.Parameters.AddWithValue("@FilterText", "%" + txt_Search.Text.Trim().Replace("'", "`") + "%");
                cmd.Parameters.AddWithValue("@FilterMerchant", Request.QueryString["merchant"].ToString());

                cmd.Parameters.Add("@RecordCount", SqlDbType.Int, 4);
                cmd.Parameters["@RecordCount"].Direction = ParameterDirection.Output;
                con.Open();

                SqlDataReader idr = cmd.ExecuteReader();

                DataTable v = new DataTable();
                v.Load(idr);
                rpt_Item.DataSource = v;
                rpt_Item.DataBind();

                idr.Close();
                con.Close();

                int recordCount = Convert.ToInt32(cmd.Parameters["@RecordCount"].Value);

                lbl_Record2.Text = recordCount.ToString();

                if (recordCount > 20)
                {
                    this.PopulatePager(recordCount, pageIndex);
                }
                else
                {
                    this.PopulatePager(20, pageIndex);
                }
            }
        }
    }

    private void PopulatePager(int recordCount, int currentPage)
    {
        ddlPager.Items.Clear();

        double dblPageCount = (double)((decimal)recordCount / decimal.Parse("20"));
        int pageCount = (int)Math.Ceiling(dblPageCount);

        if (pageCount > 0)
        {
            for (int i = 1; i <= pageCount; i++)
            {
                ddlPager.Items.Add(new ListItem("Page " + i.ToString(), i.ToString()));
            }
        }

        if (ddlPager.Items.FindByValue(currentPage.ToString()) != null)
        {
            ddlPager.SelectedValue = currentPage.ToString();
        }
        else
        {
            ddlPager.SelectedValue = "1";
            ItemLoadList(1);
        }
    }

    protected void ddlPager_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.ItemLoadList(Convert.ToInt32(ddlPager.SelectedValue));
    }

    protected void txt_Search_TextChanged(object sender, EventArgs e)
    {
        this.ItemLoadList(Convert.ToInt32(ddlPager.SelectedValue));
    }

    protected void CardItem_DataBound(object sender, RepeaterItemEventArgs e)
    {
        //if (!IsPostBack)
        //{
        //ImageButton btnedit = (ImageButton)e.Item.FindControl("btnedit");
        //ImageButton ImageButton2 = (ImageButton)e.Item.FindControl("ImageButton2");
        Label lblitemid = (Label)e.Item.FindControl("lblitemid");
        TextBox lblMPrice = (TextBox)e.Item.FindControl("lblMPrice");
        TextBox lblVPrice = (TextBox)e.Item.FindControl("lblVPrice");
        TextBox lblSPrice = (TextBox)e.Item.FindControl("lblSPrice");
        TextBox lblVVIPPrice = (TextBox)e.Item.FindControl("lblVVIPPrice");
        TextBox lblMSPrice = (TextBox)e.Item.FindControl("lblMSPrice");
        TextBox txtMcashback = (TextBox)e.Item.FindControl("txtMcashback");
        TextBox txtVcashback = (TextBox)e.Item.FindControl("txtVcashback");
        TextBox txtScashback = (TextBox)e.Item.FindControl("txtScashback");
        TextBox txtVVIPcashback = (TextBox)e.Item.FindControl("txtVVIPcashback");
        TextBox txtMScashback = (TextBox)e.Item.FindControl("txtMScashback");
        Label UOM = (Label)e.Item.FindControl("UOM");
        Label PPV = (Label)e.Item.FindControl("PPV");
        Label SPV = (Label)e.Item.FindControl("SPV");
        Label RP = (Label)e.Item.FindControl("RP");
        Label Card_Type = (Label)e.Item.FindControl("Card_Type");
        Label Modified_DT = (Label)e.Item.FindControl("Card_Date");
        Label Card_ShortDesc = (Label)e.Item.FindControl("Card_ShortDesc");
        Label Card_ItemCode = (Label)e.Item.FindControl("Card_ItemCode");
        Label Serial = (Label)e.Item.FindControl("Card_Serial");
        Label Redemption = (Label)e.Item.FindControl("Card_Redemption");
        Label lblName = (Label)e.Item.FindControl("lbl_Name");
        Label Menu_PurRate = (Label)e.Item.FindControl("Menu_PurRate");
        Label Menu_SalRate = (Label)e.Item.FindControl("Menu_SalRate");
        Label Menu_Type = (Label)e.Item.FindControl("Menu_Type");
        Label Menu_Linkcode = (Label)e.Item.FindControl("Menu_Linkcode");
        Label Label6 = (Label)e.Item.FindControl("Label6");
        Label Menu_Create = (Label)e.Item.FindControl("Menu_Create");
        Label Menu_Update = (Label)e.Item.FindControl("Menu_Update");
        LinkButton price = (LinkButton)e.Item.FindControl("Price");
        LinkButton promotion = (LinkButton)e.Item.FindControl("Promotion");
        Label lbl_desc = (Label)e.Item.FindControl("lbl_desc");
        Label Card_Publish = (Label)e.Item.FindControl("Card_Publish");
        Label Card_Weight = (Label)e.Item.FindControl("Card_Weight");
        Label card_approveandreject = (Label)e.Item.FindControl("card_approveandreject");
        TextBox txtStockQty = (TextBox)e.Item.FindControl("txtStockQty");
        TextBox txtQtyServePerday = (TextBox)e.Item.FindControl("txtQtyServePerday");
        RadioButton RadioButton1 = (RadioButton)e.Item.FindControl("QtyServePerday");
        RadioButton RadioButton2 = (RadioButton)e.Item.FindControl("StockQty");
        DropDownList ddlAnR = (DropDownList)e.Item.FindControl("ddlAnR");
        DropDownList ddlYnN = (DropDownList)e.Item.FindControl("ddlYnN");
        DropDownList ddlfoc = (DropDownList)e.Item.FindControl("ddlfoc");
        DropDownList ddlordertable = (DropDownList)e.Item.FindControl("ddlordertable");
        DropDownList ddlwaiterorder = (DropDownList)e.Item.FindControl("ddlwaiterorder");
        DropDownList ddlmember = (DropDownList)e.Item.FindControl("ddlmember");
        DropDownList ddlallowstockist = (DropDownList)e.Item.FindControl("ddlallowstockist");
        DropDownList ddlSCYnN = (DropDownList)e.Item.FindControl("ddlStockControl");
        DropDownList ddlSoldstatus = (DropDownList)e.Item.FindControl("ddlsoldstatus");
        Button btnSave = (Button)e.Item.FindControl("btnSave");
        Label lbllimit = (Label)e.Item.FindControl("lbllimit");
        Label lbllimit2 = (Label)e.Item.FindControl("lbllimit2");
        TextBox txtcost = (TextBox)e.Item.FindControl("txtcost");
        HiddenField costpercent = (HiddenField)e.Item.FindControl("costpercent");
        Button btnmarkup = (Button)e.Item.FindControl("btnmarkup");
        Label markups = (Label)e.Item.FindControl("markups");
        Label lbloverride = (Label)e.Item.FindControl("lbloverride");
        LinkButton btnpromo = (LinkButton)e.Item.FindControl("btnpromo");

        HtmlGenericControl spcount = (HtmlGenericControl)e.Item.FindControl("spcount");
        //System.Web.UI.HtmlControls.HtmlControl spcount = (System.Web.UI.HtmlControls.HtmlControl)e.Item.FindControl("spcount");

        System.Web.UI.HtmlControls.HtmlControl divAR = (System.Web.UI.HtmlControls.HtmlControl)e.Item.FindControl("divAR");
        System.Web.UI.HtmlControls.HtmlControl divYN = (System.Web.UI.HtmlControls.HtmlControl)e.Item.FindControl("divYN");
        //System.Web.UI.HtmlControls.HtmlControl divBackground = (System.Web.UI.HtmlControls.HtmlControl)e.Item.FindControl("background");
        System.Web.UI.HtmlControls.HtmlControl divline = (System.Web.UI.HtmlControls.HtmlControl)e.Item.FindControl("line");

        System.Web.UI.WebControls.Image btnedit = (System.Web.UI.WebControls.Image)e.Item.FindControl("btnedit");
        System.Web.UI.WebControls.Image ImageButton2 = (System.Web.UI.WebControls.Image)e.Item.FindControl("ImageButton2");





        if (rpt_Item.Items.Count < 1)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                HtmlGenericControl dvNoRec = e.Item.FindControl("dvNoRecords") as HtmlGenericControl;

                if (dvNoRec != null)
                {
                    dvNoRec.Visible = true;
                }
            }
        }

        ImageButton img_info = (ImageButton)e.Item.FindControl("img_info");

        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            if (Request.QueryString["user"] != null)
            {
                if (Request.QueryString["user"].ToUpper() == "S-0021-001")
                {
                    btnpromo.Visible = true;
                }
                else
                {
                    btnpromo.Visible = false;
                }
            }
            else
            {
                if (merchant.Substring(0, 6).ToUpper() == "S-0001")
                {
                    btnpromo.Visible = true;
                }
                else
                {
                    btnpromo.Visible = false;
                }
            }



            SqlConnection con = new SqlConnection(strConStringrp);
            con.Open();
            SqlCommand checkpromocmd = new SqlCommand("select b.campaigntitle,b.ids as sum from MF_AgentPromo as a left join MF_PromoCampaign as b on b.ids=a.PromoId where a.item_code='" + Card_ItemCode.Text + "' and a.deleteind <> 'X' and b.DeleteInd <> 'X' and b.MerchantID = '" + Request.QueryString["merchant"].ToString() + "' group by b.campaigntitle,b.ids", con);
            SqlDataAdapter checkpromoadp = new SqlDataAdapter(checkpromocmd);
            DataTable checkpromodt = new DataTable();
            checkpromoadp.Fill(checkpromodt);

            if (checkpromodt.Rows.Count > 0)
            {
                spcount.InnerText = checkpromodt.Rows.Count.ToString();
            }
            else
            {
                spcount.InnerText = "0";
            }

            SqlCommand checkcmd = new SqlCommand("select * from ap_merchant where merchantid = '" + Request.QueryString["merchant"].ToString() + "'", con);
            SqlDataAdapter checkadp = new SqlDataAdapter(checkcmd);
            DataTable checkdt = new DataTable();
            checkadp.Fill(checkdt);

            if (checkdt.Rows[0]["MerchantType"].ToString() == "Markup")
            {
                decimal markup = Convert.ToDecimal(checkdt.Rows[0]["MarkupPercentage"].ToString());
                costpercent.Value = markup.ToString() + "%";
                markups.Text = markup.ToString() + "%";
            }
            else
            {
                btnmarkup.Visible = false;
                lbloverride.Visible = true;
                lbloverride.Text = "Overriding :" + checkdt.Rows[0]["MDR"].ToString() + "%";
            }

            DataRowView drv = (DataRowView)(e.Item.DataItem);

            if (drv.Row["Qty_ServePerDay_ActiveYesNo"].ToString().ToUpper() == "YES")
            {
                RadioButton1.Checked = true;
                txtQtyServePerday.Enabled = true;
                lbllimit.Attributes.Add("Display", "none");
            }
            else if (drv.Row["Qty_ServePerDay_ActiveYesNo"].ToString().ToUpper() == "NO")
            {
                RadioButton1.Checked = false;
                txtQtyServePerday.Enabled = false;
            }


            if (drv.Row["Stock_Qty_ActiveYesNo"].ToString().ToUpper() == "YES")
            {
                RadioButton2.Checked = true;
                txtStockQty.Enabled = true;
            }
            else if (drv.Row["Stock_Qty_ActiveYesNo"].ToString().ToUpper() == "NO")
            {
                RadioButton2.Checked = false;
                txtStockQty.Enabled = false;
            }

            if (drv.Row["ApproveReject"].ToString().ToUpper() == "NEW")
            {
                ddlAnR.Items.Insert(0, new ListItem("Reject", "Rejected"));
                ddlAnR.Items.Insert(0, new ListItem("Approve", "Approved"));
                ddlAnR.Items.Insert(0, new ListItem("New", "New"));
            }
            else
            {
                ddlAnR.Items.Insert(0, new ListItem("Reject", "Rejected"));
                ddlAnR.Items.Insert(0, new ListItem("Approve", "Approved"));
            }

            if (drv.Row["publish"].ToString().ToUpper() == "YES")
            {
                //Card_Publish.BackColor = System.Drawing.ColorTranslator.FromHtml("#09a879");
                divYN.Attributes.Add("style", "background-color: #09a879; min-height: 27px; border: 1px solid black;");
                Card_Publish.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                Card_Publish.Text = "Publish : " + drv.Row["Publish"].ToString();
                ddlYnN.SelectedValue = drv.Row["Publish"].ToString();
            }
            else
            {
                //Card_Publish.BackColor = System.Drawing.ColorTranslator.FromHtml("#e42f51");
                divYN.Attributes.Add("style", "background-color: #e42f51; min-height: 27px; border: 1px solid black;");
                Card_Publish.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                Card_Publish.Text = "Publish : " + drv.Row["Publish"].ToString();
                ddlYnN.SelectedValue = drv.Row["Publish"].ToString();
            }

            if (drv.Row["ApproveReject"].ToString() == "Approved")
            {
                //card_approveandreject.BackColor = System.Drawing.ColorTranslator.FromHtml("#09a879");
                divAR.Attributes.Add("style", "background-color: #09a879; min-height: 27px; border: 1px solid black;");
                card_approveandreject.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                card_approveandreject.Text = "Status : " + drv.Row["ApproveReject"].ToString();
                ddlAnR.SelectedValue = drv.Row["ApproveReject"].ToString();
            }
            else
            {
                //card_approveandreject.BackColor = System.Drawing.ColorTranslator.FromHtml("#e42f51");
                divAR.Attributes.Add("style", "background-color: #e42f51; min-height: 27px; border: 1px solid black;");
                card_approveandreject.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                card_approveandreject.Text = "Status : " + drv.Row["ApproveReject"].ToString();
                DropDownList MyList = (DropDownList)e.Item.FindControl("ddlYnN");
                MyList.Visible = false;
                Label L = (Label)e.Item.FindControl("Label8");
                L.Visible = false;
                ddlAnR.SelectedValue = drv.Row["ApproveReject"].ToString();
            }

            if (drv.Row["FOC"].ToString().ToUpper() == "YES")
            {
                ddlfoc.SelectedValue = drv.Row["FOC"].ToString();
            }
            else
            {
                ddlfoc.SelectedValue = drv.Row["FOC"].ToString();
            }

            if (drv.Row["Self_Order_Menu"].ToString().ToUpper() == "YES")
            {
                ddlordertable.SelectedValue = drv.Row["Self_Order_Menu"].ToString();
            }
            else
            {
                ddlordertable.SelectedValue = drv.Row["Self_Order_Menu"].ToString();
            }

            if (drv.Row["Waiter_Order_Menu"].ToString().ToUpper() == "YES")
            {
                ddlwaiterorder.SelectedValue = drv.Row["Waiter_Order_Menu"].ToString();
            }
            else
            {
                ddlwaiterorder.SelectedValue = drv.Row["Waiter_Order_Menu"].ToString();
            }

            if (drv.Row["Allow_In_Member"].ToString().ToUpper() == "YES")
            {
                ddlmember.SelectedValue = drv.Row["Allow_In_Member"].ToString();
            }
            else
            {
                ddlmember.SelectedValue = drv.Row["Allow_In_Member"].ToString();
            }

            if (drv.Row["Allow_In_Stockist"].ToString().ToUpper() == "YES")
            {
                ddlallowstockist.SelectedValue = drv.Row["Allow_In_Stockist"].ToString();
            }
            else
            {
                ddlallowstockist.SelectedValue = drv.Row["Allow_In_Stockist"].ToString();
            }


            if (drv.Row["ShoppingCart_StockControl_YesNo"].ToString().ToUpper() == "YES")
            {
                RadioButton1.Enabled = true;
                RadioButton2.Enabled = true;


                //if (RadioButton2.Checked == true)
                //{
                //    txtStockQty.Enabled = true;
                //}
                //else
                //{
                //    txtStockQty.Enabled = false;
                //}


                //else
                //{
                //    txtQtyServePerday.Enabled = false;
                //}

                string stockqty = Convert.ToDecimal(drv.Row["Stock_Qty"].ToString()).ToString("#0");
                string QtyServePerday = Convert.ToDecimal(drv.Row["Qty_ServePerDay"].ToString()).ToString("#0");

                //servePerDay no limit display
                if (RadioButton1.Checked == true)
                {
                    txtQtyServePerday.Enabled = true;
                    lbllimit.Attributes.Add("Display", "none");

                    if (drv.Row["Qty_ServePerDay"].ToString() != "9999999999")
                    {
                        txtQtyServePerday.Text = Convert.ToDecimal(drv.Row["Qty_ServePerDay"].ToString()).ToString("#0");
                    }
                    else
                    {
                        txtQtyServePerday.Text = "0";
                    }
                }
                else if (RadioButton1.Checked == false)
                {
                    if (QtyServePerday.Length >= 10)
                    {
                        lbllimit.Visible = true;
                        txtQtyServePerday.Visible = false;
                        txtQtyServePerday.Text = "0";
                        //txtStockQty.Text = Convert.ToDecimal(drv.Row["Stock_Qty"].ToString()).ToString("#0");
                        ddlSCYnN.SelectedValue = drv.Row["ShoppingCart_StockControl_YesNo"].ToString();
                    }
                    else if (QtyServePerday.Length < 10)
                    {
                        lbllimit.Attributes.Add("Display", "none");
                        txtQtyServePerday.Text = Convert.ToDecimal(drv.Row["Qty_ServePerDay"].ToString()).ToString("#0");
                        //txtStockQty.Text = Convert.ToDecimal(drv.Row["Stock_Qty"].ToString()).ToString("#0");
                        ddlSCYnN.SelectedValue = drv.Row["ShoppingCart_StockControl_YesNo"].ToString();
                    }
                }

                if (stockqty.Length >= 10)
                {
                    //txtQtyServePerday.Text = Convert.ToDecimal(drv.Row["Qty_ServePerDay"].ToString()).ToString("#0");
                    txtStockQty.Text = "No Limit";
                    ddlSCYnN.SelectedValue = drv.Row["ShoppingCart_StockControl_YesNo"].ToString();
                }
                else if (stockqty.Length <= 10)
                {
                    //txtQtyServePerday.Text = Convert.ToDecimal(drv.Row["Qty_ServePerDay"].ToString()).ToString("#0");
                    txtStockQty.Text = Convert.ToDecimal(drv.Row["Stock_Qty"].ToString()).ToString("#0");
                    ddlSCYnN.SelectedValue = drv.Row["ShoppingCart_StockControl_YesNo"].ToString();
                }
            }
            else if (drv.Row["ShoppingCart_StockControl_YesNo"].ToString().ToUpper() == "NO")
            {
                RadioButton1.Enabled = false;
                RadioButton2.Enabled = false;

                txtStockQty.Enabled = false;
                txtQtyServePerday.Enabled = false;

                string stockqty = Convert.ToDecimal(drv.Row["Stock_Qty"].ToString()).ToString("#0");
                string QtyServePerday = Convert.ToDecimal(drv.Row["Qty_ServePerDay"].ToString()).ToString("#0");

                //servePerDay no limit display
                if (QtyServePerday.Length >= 10)
                {
                    lbllimit.Visible = true;
                    txtQtyServePerday.Text = Convert.ToDecimal(drv.Row["Qty_ServePerDay"].ToString()).ToString("#0");
                    txtQtyServePerday.Visible = false;
                    //txtStockQty.Text = Convert.ToDecimal(drv.Row["Stock_Qty"].ToString()).ToString("#0");
                    ddlSCYnN.SelectedValue = drv.Row["ShoppingCart_StockControl_YesNo"].ToString();
                }
                else if (QtyServePerday.Length <= 10)
                {
                    txtQtyServePerday.Text = Convert.ToDecimal(drv.Row["Qty_ServePerDay"].ToString()).ToString("#0");
                    //txtStockQty.Text = Convert.ToDecimal(drv.Row["Stock_Qty"].ToString()).ToString("#0");
                    ddlSCYnN.SelectedValue = drv.Row["ShoppingCart_StockControl_YesNo"].ToString();
                }

                ddlSCYnN.SelectedValue = drv.Row["ShoppingCart_StockControl_YesNo"].ToString();
                //txtQtyServePerday.Text = Convert.ToDecimal(drv.Row["Qty_ServePerDay"].ToString()).ToString("#0");
                txtStockQty.Text = Convert.ToDecimal(drv.Row["Stock_Qty"].ToString()).ToString("#0");
            }

            if (drv.Row["SoldOut_Status_YesNo"].ToString().ToUpper() == "YES")
            {
                ddlSoldstatus.SelectedValue = drv.Row["SoldOut_Status_YesNo"].ToString();
                //divBackground.Attributes.Add("style", "background-color: #FFD2D2;");
                divline.Attributes.Add("style", "background-color: #ffc7db; border: 2px solid black; padding-bottom:5px;");
            }
            else if (drv.Row["SoldOut_Status_YesNo"].ToString().ToUpper() == "NO")
            {
                ddlSoldstatus.SelectedValue = drv.Row["SoldOut_Status_YesNo"].ToString();


            }

            if (drv.Row["FilePath"] != DBNull.Value)
            {
                btnedit.ImageUrl = drv.Row["FilePath"].ToString();
            }
            else
            {
                btnedit.ImageUrl = "Images/NoPic.png";
            }

            //lblPrice.Text = "Price : RM" + Math.Round(Convert.ToDouble(drv.Row["BuyPrice"].ToString()), 0);
            lblVVIPPrice.Text = Convert.ToDecimal(drv.Row["VVIPPrice"].ToString()).ToString("#,##0.00");
            lblMSPrice.Text = Convert.ToDecimal(drv.Row["MStockistPrice"].ToString()).ToString("#,##0.00");
            txtMcashback.Text = Convert.ToDecimal(drv.Row["MemberCashback"].ToString()).ToString("#,##0.00");
            txtVcashback.Text = Convert.ToDecimal(drv.Row["VIPCashback"].ToString()).ToString("#,##0.00");
            txtScashback.Text = Convert.ToDecimal(drv.Row["StockistCashback"].ToString()).ToString("#,##0.00");
            txtVVIPcashback.Text = Convert.ToDecimal(drv.Row["VVIPCashback"].ToString()).ToString("#,##0.00");
            txtMScashback.Text = Convert.ToDecimal(drv.Row["MStockistCashback"].ToString()).ToString("#,##0.00");

            txtcost.Text = Convert.ToDecimal(drv.Row["vendorcost"].ToString()).ToString("#,##0.00");
            lblMPrice.Text = Convert.ToDecimal(drv.Row["MemberPrice"].ToString()).ToString("#,##0.00");
            lblVPrice.Text = Convert.ToDecimal(drv.Row["VIPPrice"].ToString()).ToString("#,##0.00");
            lblSPrice.Text = Convert.ToDecimal(drv.Row["StockistPrice"].ToString()).ToString("#,##0.00");
            Card_Weight.Text = Convert.ToDecimal(drv.Row["weight"].ToString()).ToString("#,##0.0000") + "KG";

            if (drv.Row["LongDesc"].ToString().Length > 45)
            {

                Card_ShortDesc.Text = drv.Row["LongDesc"].ToString().Substring(0, 45) + " ...";
            }
            else
            {
                Card_ShortDesc.Text = drv.Row["LongDesc"].ToString();
            }

            Modified_DT.Text = Convert.ToDateTime(drv.Row["Modified_DT"]).ToString("dd MMM yyy HH:mm tt");
        }
        //}
    }

    protected void ddlAnR_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = sender as DropDownList;
        var lblitemid = (Label)ddl.FindControl("lblitemid");
        var Item_ID = lblitemid.Text;
        var label6 = (Label)ddl.FindControl("Card_ItemCode");
        var Item_Code = label6.Text;

        if (ddl.SelectedValue == "New")
        {
            return;
        }
        else if (ddl.SelectedValue == "Approved")
        {
            //string p = "Yes";
            //SqlConnection myQRConn = new SqlConnection(strConStringrp);
            //myQRConn.Open();

            //try
            //{
            //    //Code to approve
            //    //SqlCommand QRcmd = new SqlCommand("update MF_Item set ApproveReject=@ApproveReject where Item_ID='" + Item_ID.Text.Trim().Replace("'", "`") + "' and DeleteInd <> 'X'", myQRConn);
            //    SqlCommand QRcmd = new SqlCommand("update MF_Item set ApproveReject=@ApproveReject where Item_ID='" + Item_ID + "' and DeleteInd <> 'X'", myQRConn);

            //    QRcmd.Parameters.AddWithValue("@ApproveReject", ddl.SelectedValue);
            //    QRcmd.ExecuteNonQuery();
            //    SqlCommand QRcmd2 = new SqlCommand("insert into MF_ItemStatus_ActionLog (Item_Code,Publish,ApproveReject,LogDT) Values(@Item_Code,@Publish,@ApproveReject, @LogDT) ", myQRConn);
            //    QRcmd2.Parameters.AddWithValue("@ApproveReject", ddl.SelectedValue);
            //    QRcmd2.Parameters.AddWithValue("@Publish", "");
            //    QRcmd2.Parameters.AddWithValue("@Item_Code", Item_Code);
            //    QRcmd2.Parameters.AddWithValue("@LogDT", DateTime.Now);
            //    QRcmd2.ExecuteNonQuery();
            //}
            //finally
            //{
            //    myQRConn.Close();
            //    //Response.Redirect("BItemListing.aspx");
            //    this.ItemLoadList(Convert.ToInt32(ddlPager.SelectedValue));
            //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "applyFootable", "applyFootable();", true);
            //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "onpageload('');", true);
            //}
        }
        else if (ddl.SelectedValue == "Rejected")
        {
            string n = "No";
            SqlConnection myQRConn = new SqlConnection(strConStringrp);
            myQRConn.Open();

            //try
            //{
            //    //Code to reject
            //    //SqlCommand QRcmd = new SqlCommand("update MF_Item set ApproveReject=@ApproveReject where Item_ID='" + Item_ID.Text.Trim().Replace("'", "`") + "' and DeleteInd <> 'X'", myQRConn);
            //    SqlCommand QRcmd = new SqlCommand("update MF_Item set ApproveReject=@ApproveReject, Publish=@Publish where Item_ID='" + Item_ID + "' and DeleteInd <> 'X'", myQRConn);
            //    QRcmd.Parameters.AddWithValue("@ApproveReject", ddl.SelectedValue);
            //    QRcmd.Parameters.AddWithValue("@Publish", n);
            //    QRcmd.ExecuteNonQuery();
            //    SqlCommand QRcmd2 = new SqlCommand("insert into MF_ItemStatus_ActionLog (Item_Code,Publish,ApproveReject,LogDT) Values(@Item_Code,@Publish,@ApproveReject, @LogDT) ", myQRConn);
            //    QRcmd2.Parameters.AddWithValue("@ApproveReject", ddl.SelectedValue);
            //    QRcmd2.Parameters.AddWithValue("@Publish", "");
            //    QRcmd2.Parameters.AddWithValue("@Item_Code", Item_Code);
            //    QRcmd2.Parameters.AddWithValue("@LogDT", DateTime.Now);
            //    QRcmd2.ExecuteNonQuery();
            //}
            //finally
            //{
            //    myQRConn.Close();
            //    //Response.Redirect("BItemListing.aspx");
            //    this.ItemLoadList(Convert.ToInt32(ddlPager.SelectedValue));
            //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "applyFootable", "applyFootable();", true);
            //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "onpageload('');", true);
            //}
        }
    }

    protected void ddlYnN_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = sender as DropDownList;
        var lblitemid = (Label)ddl.FindControl("lblitemid");
        var Item_ID = lblitemid.Text;
        var label6 = (Label)ddl.FindControl("Card_ItemCode");
        var Item_Code = label6.Text;

        if (ddl.SelectedValue == "Yes")
        {
            SqlConnection myQRConn = new SqlConnection(strConStringrp);
            myQRConn.Open();

            try
            {
                //Code to approve
                //SqlCommand QRcmd = new SqlCommand("update MF_Item set ApproveReject=@ApproveReject where Item_ID='" + Item_ID.Text.Trim().Replace("'", "`") + "' and DeleteInd <> 'X'", myQRConn);
                SqlCommand QRcmd = new SqlCommand("update MF_Item set Publish=@Publish where Item_ID='" + Item_ID + "' and DeleteInd <> 'X'", myQRConn);

                QRcmd.Parameters.AddWithValue("@Publish", ddl.SelectedValue);
                QRcmd.ExecuteNonQuery();
                SqlCommand QRcmd2 = new SqlCommand("insert into MF_ItemStatus_ActionLog (Item_Code,ApproveReject,Publish,LogDT) Values(@Item_Code,'',@Publish, @LogDT) ", myQRConn);
                QRcmd2.Parameters.AddWithValue("@Publish", ddl.SelectedValue);
                QRcmd2.Parameters.AddWithValue("@Item_Code", Item_Code);
                QRcmd2.Parameters.AddWithValue("@LogDT", DateTime.Now);
                QRcmd2.ExecuteNonQuery();
            }
            finally
            {
                myQRConn.Close();
                //Response.Redirect("BItemListing.aspx");
                this.ItemLoadList(Convert.ToInt32(ddlPager.SelectedValue));
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "applyFootable", "applyFootable();", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "onpageload('');", true);
            }
        }
        else if (ddl.SelectedValue == "No")
        {
            SqlConnection myQRConn = new SqlConnection(strConStringrp);
            myQRConn.Open();

            try
            {
                //Code to reject
                //SqlCommand QRcmd = new SqlCommand("update MF_Item set ApproveReject=@ApproveReject where Item_ID='" + Item_ID.Text.Trim().Replace("'", "`") + "' and DeleteInd <> 'X'", myQRConn);
                SqlCommand QRcmd = new SqlCommand("update MF_Item set Publish=@Publish where Item_ID='" + Item_ID + "' and DeleteInd <> 'X'", myQRConn);
                QRcmd.Parameters.AddWithValue("@Publish", ddl.SelectedValue);
                QRcmd.ExecuteNonQuery();
                SqlCommand QRcmd2 = new SqlCommand("insert into MF_ItemStatus_ActionLog (Item_Code,ApproveReject,Publish,LogDT) Values(@Item_Code,'',@Publish,@LogDT) ", myQRConn);
                QRcmd2.Parameters.AddWithValue("@Publish", ddl.SelectedValue);
                QRcmd2.Parameters.AddWithValue("@Item_Code", Item_Code);
                QRcmd2.Parameters.AddWithValue("@LogDT", DateTime.Now);
                QRcmd2.ExecuteNonQuery();
            }
            finally
            {
                myQRConn.Close();
                //Response.Redirect("BItemListing.aspx");
                this.ItemLoadList(Convert.ToInt32(ddlPager.SelectedValue));
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "applyFootable", "applyFootable();", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "onpageload('');", true);
            }
        }
    }

    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.ItemLoadList(Convert.ToInt32(ddlPager.SelectedValue));
    }

    protected void ddlPublish_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.ItemLoadList(Convert.ToInt32(ddlPager.SelectedValue));
    }

    protected void ddlSoldStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.ItemLoadList(Convert.ToInt32(ddlPager.SelectedValue));
    }

    protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindCategories(merchant, ddlDepartment.SelectedValue);
        this.ItemLoadList(Convert.ToInt32(ddlPager.SelectedValue));
    }

    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.ItemLoadList(Convert.ToInt32(ddlPager.SelectedValue));
    }

    private void BindDepartments(string supplierCode)
    {
        var sql = @"SELECT Department_Code, Department_Description FROM MF_Department_miso WHERE supplier_code = @SupplierCode AND (DeleteInd IS NULL OR DeleteInd <> 'X') ORDER BY Department_Description";

        using (var con = new SqlConnection(strConStringrp))
        using (var cmd = new SqlCommand(sql, con))
        {
            cmd.Parameters.AddWithValue("@SupplierCode", supplierCode ?? "");
            var dt = new DataTable();
            using (var da = new SqlDataAdapter(cmd))
                da.Fill(dt);

            ddlDepartment.DataSource = dt;
            ddlDepartment.DataTextField = "Department_Description";
            ddlDepartment.DataValueField = "Department_Code";
            ddlDepartment.DataBind();
        }
        ddlDepartment.Items.Insert(0, new ListItem("All Department", ""));
    }

    private void BindCategories(string supplierCode, string departmentCode)
    {
        // departmentCode can be "" to get all categories for the supplier
        var sql = @"SELECT DISTINCT Category_Code, Category_Description FROM MF_Category_miso WHERE supplier_code = @SupplierCode AND (DeleteInd IS NULL OR DeleteInd <> 'X') AND (@DepartmentCode = '' OR Department_Code = @DepartmentCode) ORDER BY Category_Description";

        using (var con = new SqlConnection(strConStringrp))
        using (var cmd = new SqlCommand(sql, con))
        {
            cmd.Parameters.AddWithValue("@SupplierCode", supplierCode ?? "");
            cmd.Parameters.AddWithValue("@DepartmentCode", departmentCode ?? "");
            var dt = new DataTable();
            using (var da = new SqlDataAdapter(cmd))
                da.Fill(dt);

            ddlCategory.DataSource = dt;
            ddlCategory.DataTextField = "Category_Description";
            ddlCategory.DataValueField = "Category_Code";
            ddlCategory.DataBind();
        }
        ddlCategory.Items.Insert(0, new ListItem("All Category", ""));
    }


    protected void ddlStockControl_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlSC = sender as DropDownList;
        var RadioButton1 = (RadioButton)ddlSC.FindControl("QtyServePerday");
        var RadioButton2 = (RadioButton)ddlSC.FindControl("StockQty");
        var txtQtyServePerday = (TextBox)ddlSC.FindControl("txtQtyServePerday");
        var txtStockQty = (TextBox)ddlSC.FindControl("txtStockQty");
        var lblitemid = (Label)ddlSC.FindControl("lblitemid");
        var lbllimit = (Label)ddlSC.FindControl("lbllimit");
        var Item_ID = lblitemid.Text;
        string i = "";
        string d = "0";

        if (ddlSC.SelectedValue == "Yes")
        {
            SqlConnection myQRConn = new SqlConnection(strConStringrp);
            myQRConn.Open();

            SqlCommand QRcmd = new SqlCommand("update MF_Item set ShoppingCart_StockControl_YesNo=@SCYesNo where Item_ID='" + Item_ID + "' and DeleteInd <> 'X'", myQRConn);
            QRcmd.Parameters.AddWithValue("@SCYesNo", ddlSC.SelectedValue);
            QRcmd.ExecuteNonQuery();

            RadioButton1.Enabled = true;
            RadioButton2.Enabled = true;

            if (RadioButton1.Checked == true)
            {
                txtQtyServePerday.Visible = true;
                txtQtyServePerday.Enabled = true;
                lbllimit.Visible = false;
            }

        }
        else if (ddlSC.SelectedValue == "No")
        {
            SqlConnection myQRConn = new SqlConnection(strConStringrp);
            myQRConn.Open();

            SqlCommand QRcmd = new SqlCommand("update MF_Item set ShoppingCart_StockControl_YesNo=@SCYesNo where Item_ID='" + Item_ID + "' and DeleteInd <> 'X'", myQRConn);
            QRcmd.Parameters.AddWithValue("@SCYesNo", ddlSC.SelectedValue);
            QRcmd.ExecuteNonQuery();

            RadioButton1.Enabled = false;
            RadioButton2.Enabled = false;

            txtStockQty.Enabled = false;
            txtQtyServePerday.Enabled = false;

            //txtQtyServePerday.Text = i;
            //txtStockQty.Text = i;
        }
    }

    protected void RadioButton1_CheckedChanged(object sender, EventArgs e)
    {
        RadioButton txt = sender as RadioButton;
        var RadioButton1 = (RadioButton)txt.FindControl("QtyServePerday");
        //var RadioButton2 = (RadioButton)txt.FindControl("StockQty");
        var txtQtyServePerday = (TextBox)txt.FindControl("txtQtyServePerday");
        var txtStockQty = (TextBox)txt.FindControl("txtStockQty");
        var lbllimit = (Label)txt.FindControl("lbllimit");
        //var Item_ID = lblitemid.Text;

        if (RadioButton1.Checked == true)
        {
            txtQtyServePerday.Enabled = true;
            txtStockQty.Enabled = false;
            lbllimit.Visible = false;
            txtQtyServePerday.Visible = true;
        }
        else
        {
            txtQtyServePerday.Enabled = false;
        }
    }

    protected void RadioButton2_CheckedChanged(object sender, EventArgs e)
    {
        RadioButton txt = sender as RadioButton;
        var RadioButton2 = (RadioButton)txt.FindControl("StockQty");
        //var RadioButton1 = (RadioButton)txt.FindControl("QtyServePerday");
        var txtStockQty = (TextBox)txt.FindControl("txtStockQty");
        var txtQtyServePerday = (TextBox)txt.FindControl("txtQtyServePerday");
        //var lblitemid = (Label)txt.FindControl("lblitemid");
        //var Item_ID = lblitemid.Text;

        if (RadioButton2.Checked == true)
        {
            txtStockQty.Enabled = true;
            txtQtyServePerday.Enabled = false;
        }
        else
        {
            txtStockQty.Enabled = false;
        }
    }


    protected void ddlsoldstatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlSoldstatus = sender as DropDownList;
        var lblitemid = (Label)ddlSoldstatus.FindControl("lblitemid");
        var Item_ID = lblitemid.Text;

        //System.Web.UI.HtmlControls.HtmlControl divBackground = (System.Web.UI.HtmlControls.HtmlControl)ddlSoldstatus.FindControl("background");
        System.Web.UI.HtmlControls.HtmlControl divline = (System.Web.UI.HtmlControls.HtmlControl)ddlSoldstatus.FindControl("line");


        if (ddlSoldstatus.SelectedValue == "Yes")
        {
            //divBackground.Attributes.Add("style", "background-color: #FFD2D2;");
            divline.Attributes.Add("style", "background-color: #ffc7db; border: 2px solid black; padding-bottom:5px;");

        }
        else if (ddlSoldstatus.SelectedValue == "No")
        {
            //divBackground.Attributes.Add("style", "background-color: #FFFFFF;");
            divline.Attributes.Add("style", "background-color: #FFFFFF; border: 2px solid black; padding-bottom:5px;");
        }

    }


    //--Stock Control--//
    protected void btnSave_Click(object sender, EventArgs e)
    {
        Button txt = sender as Button;
        var txtStockQty = (TextBox)txt.FindControl("txtStockQty");
        var txtQtyServePerday = (TextBox)txt.FindControl("txtQtyServePerday");
        var Card_ItemCode = (Label)txt.FindControl("Card_ItemCode");
        var Item_Code = Card_ItemCode.Text;
        var txtNewMprice = (TextBox)txt.FindControl("lblMPrice");
        var txtNewVprice = (TextBox)txt.FindControl("lblVPrice");
        var txtNewSprice = (TextBox)txt.FindControl("lblSPrice");
        var lblVVIPPrice = (TextBox)txt.FindControl("lblVVIPPrice");
        var lblMSPrice = (TextBox)txt.FindControl("lblMSPrice");
        var txtMcashback = (TextBox)txt.FindControl("txtMcashback");
        var txtVcashback = (TextBox)txt.FindControl("txtVcashback");
        var txtScashback = (TextBox)txt.FindControl("txtScashback");
        var txtcost = (TextBox)txt.FindControl("txtcost");
        var txtVVIPcashback = (TextBox)txt.FindControl("txtVVIPcashback");
        var txtMScashback = (TextBox)txt.FindControl("txtMScashback");
        var lblitemid = (Label)txt.FindControl("lblitemid");
        var Item_ID = lblitemid.Text;
        var ddlSoldstatus = (DropDownList)txt.FindControl("ddlsoldstatus");
        var ddlstockcontrol = (DropDownList)txt.FindControl("ddlStockControl");
        var ddlYnN = (DropDownList)txt.FindControl("ddlYnN");
        var ddlAnR = (DropDownList)txt.FindControl("ddlAnR");
        var ddlfoc = (DropDownList)txt.FindControl("ddlfoc");
        var ddlordertable = (DropDownList)txt.FindControl("ddlordertable");
        var ddlwaiterorder = (DropDownList)txt.FindControl("ddlwaiterorder");
        var ddlmember = (DropDownList)txt.FindControl("ddlmember");
        var ddlallowstockist = (DropDownList)txt.FindControl("ddlallowstockist");

        var radiobtn1 = (RadioButton)txt.FindControl("QtyServePerday");
        var radiobtn2 = (RadioButton)txt.FindControl("StockQty");

        string MStockistprice = "D05";
        string VVIPprice = "D04";
        string Stockistprice = "D03";
        string VIPprice = "D01";
        string memberprice = "D02";
        string y = "Yes";
        string n = "No";


        SqlConnection con = new SqlConnection(strConStringrp);
        con.Open();

        //Update Cost
        SqlCommand getcost = new SqlCommand("select vendorcost from mf_item where item_id = '" + Item_ID + "' and DeleteInd <> 'X'", con);
        SqlDataAdapter getcostadp = new SqlDataAdapter(getcost);
        DataTable getcostdt = new DataTable();
        getcostadp.Fill(getcostdt);

        if (getcostdt.Rows.Count > 0)
        {
            SqlCommand Loginsertcmd = new SqlCommand("insert into MF_Item_ActionLog (Item_Code,UpdateDesc,UpdateUser,Agent_LevelCode,Old_Price,New_Price,Old_CashBack,New_CashBack,LogDT) Values(@Item_Code,@Desc,@UpdateUser,'Vendor','" + getcostdt.Rows[0]["vendorcost"].ToString() + "','" + txtcost.Text.Replace(",", "") + "',0,0, @LogDT) ", con);
            Loginsertcmd.Parameters.AddWithValue("@Desc", "Old Vendor Cost : " + getcostdt.Rows[0]["vendorcost"].ToString() + ", New Vendor Cost : " + txtcost.Text.Replace(",", "") + "");
            if (Request.QueryString["user"] != null)
            {
                Loginsertcmd.Parameters.AddWithValue("@UpdateUser", Request.QueryString["user"].ToString());
            }
            else
            {
                Loginsertcmd.Parameters.AddWithValue("@UpdateUser", Request.QueryString["merchant"].ToString());
            }
            Loginsertcmd.Parameters.AddWithValue("@Item_Code", Item_Code);
            Loginsertcmd.Parameters.AddWithValue("@LogDT", DateTime.UtcNow.AddHours(8));
            Loginsertcmd.ExecuteNonQuery();

            SqlCommand costcmd1 = new SqlCommand("update mf_item set vendorcost=@vendorcost where Item_ID='" + Item_ID + "' and DeleteInd <> 'X'", con);
            costcmd1.Parameters.AddWithValue("@vendorcost", txtcost.Text.Replace(",", ""));
            costcmd1.ExecuteNonQuery();
        }

        //Sold out
        //System.Web.UI.HtmlControls.HtmlControl divBackground = (System.Web.UI.HtmlControls.HtmlControl)txt.FindControl("background");
        System.Web.UI.HtmlControls.HtmlControl divline = (System.Web.UI.HtmlControls.HtmlControl)txt.FindControl("line");


        if (ddlstockcontrol.SelectedValue == "Yes")
        {
            SqlCommand cmd1 = new SqlCommand("update mf_item set ShoppingCart_StockControl_YesNo=@ShoppingCart_StockControl_YesNo where Item_ID='" + Item_ID + "' and DeleteInd <> 'X'", con);
            cmd1.Parameters.AddWithValue("@ShoppingCart_StockControl_YesNo", y);
            cmd1.ExecuteNonQuery();
        }
        else if (ddlstockcontrol.SelectedValue == "No")
        {
            SqlCommand cmd1 = new SqlCommand("update mf_item set ShoppingCart_StockControl_YesNo=@ShoppingCart_StockControl_YesNo where Item_ID='" + Item_ID + "' and DeleteInd <> 'X'", con);
            cmd1.Parameters.AddWithValue("@ShoppingCart_StockControl_YesNo", n);
            cmd1.ExecuteNonQuery();
        }

        if (radiobtn1.Checked == true && radiobtn2.Checked == false)
        {
            SqlCommand QRcmd24 = new SqlCommand("update MF_Item set Stock_Qty_ActiveYesNo=@Stock_Qty_ActiveYesNo, Qty_ServePerDay_ActiveYesNo=@Qty_ServePerDay_ActiveYesNo where Item_ID='" + Item_ID + "' and DeleteInd <> 'X'", con);
            QRcmd24.Parameters.AddWithValue("@Stock_Qty_ActiveYesNo", n);
            QRcmd24.Parameters.AddWithValue("@Qty_ServePerDay_ActiveYesNo", y);
            QRcmd24.ExecuteNonQuery();
        }
        else if (radiobtn2.Checked == true && radiobtn1.Checked == false)
        {
            SqlCommand QRcmd24 = new SqlCommand("update MF_Item set Stock_Qty_ActiveYesNo=@Stock_Qty_ActiveYesNo, Qty_ServePerDay_ActiveYesNo=@Qty_ServePerDay_ActiveYesNo where Item_ID='" + Item_ID + "' and DeleteInd <> 'X'", con);
            QRcmd24.Parameters.AddWithValue("@Stock_Qty_ActiveYesNo", y);
            QRcmd24.Parameters.AddWithValue("@Qty_ServePerDay_ActiveYesNo", n);
            QRcmd24.ExecuteNonQuery();
        }

        if (ddlSoldstatus.SelectedValue == "Yes")
        {
            SqlCommand QRcmd24 = new SqlCommand("update MF_Item set SoldOut_Status_YesNo=@SoldOut_Status_YesNo where Item_ID='" + Item_ID + "' and DeleteInd <> 'X'", con);
            QRcmd24.Parameters.AddWithValue("@SoldOut_Status_YesNo", ddlSoldstatus.SelectedValue);
            QRcmd24.ExecuteNonQuery();
            //divBackground.Attributes.Add("style", "background-color: #FFD2D2;");
            divline.Attributes.Add("style", "background-color: #ffc7db; border: 2px solid black;");

        }
        else if (ddlSoldstatus.SelectedValue == "No")
        {
            SqlCommand QRcmd23 = new SqlCommand("update MF_Item set SoldOut_Status_YesNo=@SoldOut_Status_YesNo where Item_ID='" + Item_ID + "' and DeleteInd <> 'X'", con);
            QRcmd23.Parameters.AddWithValue("@SoldOut_Status_YesNo", ddlSoldstatus.SelectedValue);
            QRcmd23.ExecuteNonQuery();
        }


        //Check All row if have row then proceed to save price
        SqlCommand cmdr4 = new SqlCommand("select *  from MF_AgentCommi where Item_Code='" + Item_Code + "'", con);
        SqlDataReader dv12 = cmdr4.ExecuteReader();

        if (dv12.HasRows == true)
        {
            dv12.Close();
            dv12.Dispose();

            //Check AgentLevelCode If have row then store otherwise show error message
            SqlCommand cmdr = new SqlCommand("select *  from MF_AgentCommi where Item_Code='" + Item_Code + "' and AgentLevelCode='" + memberprice + "'", con);
            SqlDataAdapter cmdradp = new SqlDataAdapter(cmdr);
            DataTable dtcmdr = new DataTable();
            cmdradp.Fill(dtcmdr);
            //SqlDataReader dv = cmdr.ExecuteReader();

            if (dtcmdr.Rows.Count > 0)
            {
                //Update Member Price 
                SqlConnection CON1 = new SqlConnection(strConStringrp);
                CON1.Open();

                SqlCommand Loginsertcmd1 = new SqlCommand("insert into MF_Item_ActionLog (Item_Code,UpdateDesc,UpdateUser,Agent_LevelCode,Old_Price,New_Price,Old_CashBack,New_CashBack,LogDT) Values(@Item_Code,@Desc,@UpdateUser,'D02','" + dtcmdr.Rows[0]["BuyPrice"].ToString() + "','" + Convert.ToDecimal(txtNewMprice.Text) + "',0,0, @LogDT) ", con);
                Loginsertcmd1.Parameters.AddWithValue("@Desc", "Old Member Price : " + dtcmdr.Rows[0]["BuyPrice"].ToString() + ", New Member Price : " + Convert.ToDecimal(txtNewMprice.Text) + "");
                if (Request.QueryString["user"] != null)
                {
                    Loginsertcmd1.Parameters.AddWithValue("@UpdateUser", Request.QueryString["user"].ToString());
                }
                else
                {
                    Loginsertcmd1.Parameters.AddWithValue("@UpdateUser", Request.QueryString["merchant"].ToString());
                }
                Loginsertcmd1.Parameters.AddWithValue("@Item_Code", Item_Code);
                Loginsertcmd1.Parameters.AddWithValue("@LogDT", DateTime.UtcNow.AddHours(8));
                Loginsertcmd1.ExecuteNonQuery();

                SqlCommand cmd = new SqlCommand("update MF_AgentCommi set BuyPrice=@BuyPrice,CashBackAmount=@CashBackAmount, Modified_DT=@Modified_DT where Item_Code='" + Item_Code + "' and AgentLevelCode='" + memberprice + "'", CON1);
                cmd.Parameters.AddWithValue("@BuyPrice", Convert.ToDecimal(txtNewMprice.Text));
                cmd.Parameters.AddWithValue("@CashBackAmount", Convert.ToDecimal(txtMcashback.Text));
                cmd.Parameters.AddWithValue("@Modified_DT", DateTime.UtcNow.AddHours(8));
                cmd.ExecuteNonQuery();

                //update promotion
                SqlCommand updatepromocmd = new SqlCommand("update MF_AgentPromo set Promo_Price= case when discount_type = 'DBP' then @promoprice - (@promoprice * (discount_amt / 100)) else '' - discount_amt end , Modified_DT=@Modified_DT where Item_Code='" + Item_Code + "' and AgentLevelCode='" + memberprice + "'", CON1);
                updatepromocmd.Parameters.AddWithValue("@promoprice", Convert.ToDecimal(txtNewMprice.Text));
                updatepromocmd.Parameters.AddWithValue("@Modified_DT", DateTime.UtcNow.AddHours(8));

                try
                {
                    updatepromocmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                }

            }
            else
            {
                if (Request.QueryString["user"] != null)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Item Updated. There is no member price.\\\nPlease contact to administrator to add member buy price.');window.location ='ContentApproval.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "&user=" + Request.QueryString["user"].ToString() + "';", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Item Updated. There is no member price.\\\nPlease contact to administrator to add member buy price.');window.location ='ContentApproval.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "';", true);

                }
                ;

            }

            //dv.Close();
            //dv.Dispose();

            //Check AgentLevelCode If have row then store otherwise show error message
            SqlCommand cmdr1 = new SqlCommand("select *  from MF_AgentCommi where Item_Code='" + Item_Code + "' and AgentLevelCode='" + VIPprice + "'", con);
            //SqlDataReader dv1 = cmdr1.ExecuteReader();
            SqlDataAdapter cmdradp1 = new SqlDataAdapter(cmdr1);
            DataTable dtcmdr1 = new DataTable();
            cmdradp1.Fill(dtcmdr1);
            //SqlDataReader dv = cmdr.ExecuteReader();

            if (dtcmdr1.Rows.Count > 0)
            {
                //Update VIP Price
                SqlConnection CON1 = new SqlConnection(strConStringrp);
                CON1.Open();

                SqlCommand Loginsertcmd2 = new SqlCommand("insert into MF_Item_ActionLog (Item_Code,UpdateDesc,UpdateUser,Agent_LevelCode,Old_Price,New_Price,Old_CashBack,New_CashBack,LogDT) Values(@Item_Code,@Desc,@UpdateUser,'D01','" + dtcmdr1.Rows[0]["BuyPrice"].ToString() + "','" + Convert.ToDecimal(txtNewVprice.Text) + "','" + dtcmdr1.Rows[0]["CashBackAmount"].ToString() + "','" + Convert.ToDecimal(txtVcashback.Text) + "', @LogDT) ", con);
                Loginsertcmd2.Parameters.AddWithValue("@Desc", "Old VIP Price : " + dtcmdr1.Rows[0]["BuyPrice"].ToString() + ", New VIP Price : " + Convert.ToDecimal(txtNewVprice.Text) + ", Old CashBack : " + dtcmdr1.Rows[0]["CashBackAmount"].ToString() + ", New CashBack : " + Convert.ToDecimal(txtVcashback.Text) + "");
                if (Request.QueryString["user"] != null)
                {
                    Loginsertcmd2.Parameters.AddWithValue("@UpdateUser", Request.QueryString["user"].ToString());
                }
                else
                {
                    Loginsertcmd2.Parameters.AddWithValue("@UpdateUser", Request.QueryString["merchant"].ToString());
                }
                Loginsertcmd2.Parameters.AddWithValue("@Item_Code", Item_Code);
                Loginsertcmd2.Parameters.AddWithValue("@LogDT", DateTime.UtcNow.AddHours(8));
                Loginsertcmd2.ExecuteNonQuery();

                SqlCommand cmd1 = new SqlCommand("update MF_AgentCommi set BuyPrice=@BuyPrice,CashBackAmount=@CashBackAmount, Modified_DT=@Modified_DT where Item_Code='" + Item_Code + "' and AgentLevelCode='" + VIPprice + "'", CON1);
                cmd1.Parameters.AddWithValue("@BuyPrice", Convert.ToDecimal(txtNewVprice.Text));
                cmd1.Parameters.AddWithValue("@CashBackAmount", Convert.ToDecimal(txtVcashback.Text));
                cmd1.Parameters.AddWithValue("@Modified_DT", DateTime.UtcNow.AddHours(8));
                cmd1.ExecuteNonQuery();

                //update promotion
                SqlCommand updatepromocmd = new SqlCommand("update MF_AgentPromo set Promo_Price= case when discount_type = 'DBP' then @promoprice - (@promoprice * (discount_amt / 100)) else '' - discount_amt end , Modified_DT=@Modified_DT where Item_Code='" + Item_Code + "' and AgentLevelCode='" + VIPprice + "'", CON1);
                updatepromocmd.Parameters.AddWithValue("@promoprice", Convert.ToDecimal(txtNewVprice.Text));
                updatepromocmd.Parameters.AddWithValue("@Modified_DT", DateTime.UtcNow.AddHours(8));
                try
                {
                    updatepromocmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                }
            }
            else
            {
                if (Request.QueryString["user"] != null)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Item Updated. There is no VIP price.\\\nPlease contact to administrator to add VIP buy price.');window.location ='ContentApproval.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "&user=" + Request.QueryString["user"].ToString() + "';", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Item Updated. There is no VIP price.\\\nPlease contact to administrator to add VIP buy price.');window.location ='ContentApproval.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "';", true);

                }
                ;

            }

            //dv1.Close();
            //dv1.Dispose();

            //Check AgentLevelCode If have row then store otherwise show error message
            SqlCommand cmdr5 = new SqlCommand("select *  from MF_AgentCommi where Item_Code='" + Item_Code + "' and AgentLevelCode='" + VVIPprice + "'", con);
            //SqlDataReader dv5 = cmdr5.ExecuteReader();
            SqlDataAdapter cmdradp2 = new SqlDataAdapter(cmdr5);
            DataTable dtcmdr2 = new DataTable();
            cmdradp2.Fill(dtcmdr2);
            //SqlDataReader dv = cmdr.ExecuteReader();

            if (dtcmdr2.Rows.Count > 0)
            {
                //Update VVIP Price
                SqlConnection CON5 = new SqlConnection(strConStringrp);
                CON5.Open();

                SqlCommand Loginsertcmd3 = new SqlCommand("insert into MF_Item_ActionLog (Item_Code,UpdateDesc,UpdateUser,Agent_LevelCode,Old_Price,New_Price,Old_CashBack,New_CashBack,LogDT) Values(@Item_Code,@Desc,@UpdateUser,'D04','" + dtcmdr2.Rows[0]["BuyPrice"].ToString() + "','" + Convert.ToDecimal(lblVVIPPrice.Text) + "','" + dtcmdr2.Rows[0]["CashBackAmount"].ToString() + "','" + Convert.ToDecimal(txtVVIPcashback.Text) + "', @LogDT) ", con);
                Loginsertcmd3.Parameters.AddWithValue("@Desc", "Old VVIP Price : " + dtcmdr2.Rows[0]["BuyPrice"].ToString() + ", New VVIP Price : " + Convert.ToDecimal(lblVVIPPrice.Text) + ", Old CashBack : " + dtcmdr2.Rows[0]["CashBackAmount"].ToString() + ", New CashBack : " + Convert.ToDecimal(txtVVIPcashback.Text) + "");
                if (Request.QueryString["user"] != null)
                {
                    Loginsertcmd3.Parameters.AddWithValue("@UpdateUser", Request.QueryString["user"].ToString());
                }
                else
                {
                    Loginsertcmd3.Parameters.AddWithValue("@UpdateUser", Request.QueryString["merchant"].ToString());
                }
                Loginsertcmd3.Parameters.AddWithValue("@Item_Code", Item_Code);
                Loginsertcmd3.Parameters.AddWithValue("@LogDT", DateTime.UtcNow.AddHours(8));
                Loginsertcmd3.ExecuteNonQuery();

                SqlCommand cmd5 = new SqlCommand("update MF_AgentCommi set BuyPrice=@BuyPrice,CashBackAmount=@CashBackAmount, Modified_DT=@Modified_DT where Item_Code='" + Item_Code + "' and AgentLevelCode='" + VVIPprice + "'", CON5);
                cmd5.Parameters.AddWithValue("@BuyPrice", Convert.ToDecimal(lblVVIPPrice.Text));
                cmd5.Parameters.AddWithValue("@CashBackAmount", Convert.ToDecimal(txtVVIPcashback.Text));
                cmd5.Parameters.AddWithValue("@Modified_DT", DateTime.UtcNow.AddHours(8));
                cmd5.ExecuteNonQuery();

                //update promotion
                SqlCommand updatepromocmd = new SqlCommand("update MF_AgentPromo set Promo_Price= case when discount_type = 'DBP' then @promoprice - (@promoprice * (discount_amt / 100)) else '' - discount_amt end , Modified_DT=@Modified_DT where Item_Code='" + Item_Code + "' and AgentLevelCode='" + VVIPprice + "'", CON5);
                updatepromocmd.Parameters.AddWithValue("@promoprice", Convert.ToDecimal(lblVVIPPrice.Text));
                updatepromocmd.Parameters.AddWithValue("@Modified_DT", DateTime.UtcNow.AddHours(8));
                try
                {
                    updatepromocmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                }
            }
            else
            {
                if (Request.QueryString["user"] != null)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Item Updated. There is no VVIP price.\\\nPlease contact to administrator to add VIP buy price.');window.location ='ContentApproval.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "&user=" + Request.QueryString["user"].ToString() + "';", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Item Updated. There is no VVIP price.\\\nPlease contact to administrator to add VIP buy price.');window.location ='ContentApproval.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "';", true);

                }
                ;

            }

            //dv5.Close();
            //dv5.Dispose();

            //Check AgentLevelCode If have row then store otherwise show error message
            SqlCommand cmdr6 = new SqlCommand("select *  from MF_AgentCommi where Item_Code='" + Item_Code + "' and AgentLevelCode='" + MStockistprice + "'", con);
            //SqlDataReader dv6 = cmdr6.ExecuteReader();
            SqlDataAdapter cmdradp3 = new SqlDataAdapter(cmdr6);
            DataTable dtcmdr3 = new DataTable();
            cmdradp3.Fill(dtcmdr3);
            //SqlDataReader dv = cmdr.ExecuteReader();

            if (dtcmdr3.Rows.Count > 0)
            {
                //Stockist Update 
                SqlConnection CON6 = new SqlConnection(strConStringrp);
                CON6.Open();

                SqlCommand Loginsertcmd4 = new SqlCommand("insert into MF_Item_ActionLog (Item_Code,UpdateDesc,UpdateUser,Agent_LevelCode,Old_Price,New_Price,Old_CashBack,New_CashBack,LogDT) Values(@Item_Code,@Desc,@UpdateUser,'D05','" + dtcmdr3.Rows[0]["BuyPrice"].ToString() + "','" + Convert.ToDecimal(lblMSPrice.Text) + "','" + dtcmdr3.Rows[0]["CashBackAmount"].ToString() + "','" + Convert.ToDecimal(txtMScashback.Text) + "', @LogDT) ", con);
                Loginsertcmd4.Parameters.AddWithValue("@Desc", "Old Master Stockist Price : " + dtcmdr3.Rows[0]["BuyPrice"].ToString() + ", New Master Stockist Price : " + Convert.ToDecimal(lblMSPrice.Text) + ", Old CashBack : " + dtcmdr3.Rows[0]["CashBackAmount"].ToString() + ", New CashBack : " + Convert.ToDecimal(txtMScashback.Text) + "");
                if (Request.QueryString["user"] != null)
                {
                    Loginsertcmd4.Parameters.AddWithValue("@UpdateUser", Request.QueryString["user"].ToString());
                }
                else
                {
                    Loginsertcmd4.Parameters.AddWithValue("@UpdateUser", Request.QueryString["merchant"].ToString());
                }
                Loginsertcmd4.Parameters.AddWithValue("@Item_Code", Item_Code);
                Loginsertcmd4.Parameters.AddWithValue("@LogDT", DateTime.UtcNow.AddHours(8));
                Loginsertcmd4.ExecuteNonQuery();

                SqlCommand cmd6 = new SqlCommand("update MF_AgentCommi set BuyPrice=@BuyPrice,CashBackAmount=@CashBackAmount, Modified_DT=@Modified_DT where Item_Code='" + Item_Code + "' and AgentLevelCode='" + MStockistprice + "'", CON6);
                cmd6.Parameters.AddWithValue("@BuyPrice", Convert.ToDecimal(lblMSPrice.Text));
                cmd6.Parameters.AddWithValue("@CashBackAmount", Convert.ToDecimal(txtMScashback.Text));
                cmd6.Parameters.AddWithValue("@Modified_DT", DateTime.UtcNow.AddHours(8));
                cmd6.ExecuteNonQuery();

                //update promotion
                SqlCommand updatepromocmd = new SqlCommand("update MF_AgentPromo set Promo_Price= case when discount_type = 'DBP' then @promoprice - (@promoprice * (discount_amt / 100)) else '' - discount_amt end , Modified_DT=@Modified_DT where Item_Code='" + Item_Code + "' and AgentLevelCode='" + MStockistprice + "'", CON6);
                updatepromocmd.Parameters.AddWithValue("@promoprice", Convert.ToDecimal(lblMSPrice.Text));
                updatepromocmd.Parameters.AddWithValue("@Modified_DT", DateTime.UtcNow.AddHours(8));
                try
                {
                    updatepromocmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                }
            }
            else
            {
                if (Request.QueryString["user"] != null)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Item Updated. There is no stockist price.\\\nPlease contact to administrator to add stockist buy price.');window.location ='ContentApproval.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "&user=" + Request.QueryString["user"].ToString() + "';", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Item Updated. There is no stockist price.\\\nPlease contact to administrator to add stockist buy price.');window.location ='ContentApproval.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "';", true);

                }
                ;

            }

            //dv6.Close();
            //dv6.Dispose();

            //Check AgentLevelCode If have row then store otherwise show error message
            SqlCommand cmdr7 = new SqlCommand("select *  from MF_AgentCommi where Item_Code='" + Item_Code + "' and AgentLevelCode='" + Stockistprice + "'", con);
            //SqlDataReader dv7 = cmdr7.ExecuteReader();
            SqlDataAdapter cmdradp4 = new SqlDataAdapter(cmdr7);
            DataTable dtcmdr4 = new DataTable();
            cmdradp4.Fill(dtcmdr4);
            //SqlDataReader dv = cmdr.ExecuteReader();

            if (dtcmdr4.Rows.Count > 0)
            {
                //Stockist Update 
                SqlConnection CON7 = new SqlConnection(strConStringrp);
                CON7.Open();

                SqlCommand Loginsertcmd5 = new SqlCommand("insert into MF_Item_ActionLog (Item_Code,UpdateDesc,UpdateUser,Agent_LevelCode,Old_Price,New_Price,Old_CashBack,New_CashBack,LogDT) Values(@Item_Code,@Desc,@UpdateUser,'D03','" + dtcmdr4.Rows[0]["BuyPrice"].ToString() + "','" + Convert.ToDecimal(txtNewSprice.Text) + "','" + dtcmdr4.Rows[0]["CashBackAmount"].ToString() + "','" + Convert.ToDecimal(txtScashback.Text) + "', @LogDT) ", con);
                Loginsertcmd5.Parameters.AddWithValue("@Desc", "Old Stockist Price : " + dtcmdr4.Rows[0]["BuyPrice"].ToString() + ", New Stockist Price : " + Convert.ToDecimal(txtNewSprice.Text) + ", Old CashBack : " + dtcmdr4.Rows[0]["CashBackAmount"].ToString() + ", New CashBack : " + Convert.ToDecimal(txtScashback.Text) + "");
                if (Request.QueryString["user"] != null)
                {
                    Loginsertcmd5.Parameters.AddWithValue("@UpdateUser", Request.QueryString["user"].ToString());
                }
                else
                {
                    Loginsertcmd5.Parameters.AddWithValue("@UpdateUser", Request.QueryString["merchant"].ToString());
                }
                Loginsertcmd5.Parameters.AddWithValue("@Item_Code", Item_Code);
                Loginsertcmd5.Parameters.AddWithValue("@LogDT", DateTime.UtcNow.AddHours(8));
                Loginsertcmd5.ExecuteNonQuery();

                SqlCommand cmd7 = new SqlCommand("update MF_AgentCommi set BuyPrice=@BuyPrice,CashBackAmount=@CashBackAmount, Modified_DT=@Modified_DT where Item_Code='" + Item_Code + "' and AgentLevelCode='" + Stockistprice + "'", CON7);
                cmd7.Parameters.AddWithValue("@BuyPrice", Convert.ToDecimal(txtNewSprice.Text));
                cmd7.Parameters.AddWithValue("@CashBackAmount", Convert.ToDecimal(txtScashback.Text));
                cmd7.Parameters.AddWithValue("@Modified_DT", DateTime.UtcNow.AddHours(8));
                cmd7.ExecuteNonQuery();

                //update promotion
                SqlCommand updatepromocmd = new SqlCommand("update MF_AgentPromo set Promo_Price= case when discount_type = 'DBP' then @promoprice - (@promoprice * (discount_amt / 100)) else '' - discount_amt end , Modified_DT=@Modified_DT where Item_Code='" + Item_Code + "' and AgentLevelCode='" + Stockistprice + "'", CON7);
                updatepromocmd.Parameters.AddWithValue("@promoprice", Convert.ToDecimal(txtNewSprice.Text));
                updatepromocmd.Parameters.AddWithValue("@Modified_DT", DateTime.UtcNow.AddHours(8));
                try
                {
                    updatepromocmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                }
            }
            else
            {
                if (Request.QueryString["user"] != null)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Item Updated. There is no stockist price.\\\nPlease contact to administrator to add stockist buy price.');window.location ='ContentApproval.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "&user=" + Request.QueryString["user"].ToString() + "';", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Item Updated. There is no stockist price.\\\nPlease contact to administrator to add stockist buy price.');window.location ='ContentApproval.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "';", true);

                }
                ;

            }

            //dv7.Close();
            //dv7.Dispose();

        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Item Updated. You Cannot update price because there is no buy price for Member, VIP and Stockist.\\nPlease contact to administrator to add buy price.');window.location ='ContentApproval.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "';", true);
        }

        dv12.Close();
        dv12.Dispose();

        //Store Stock Qty
        if (txtStockQty.Text != "")
        {
            SqlCommand QRcmd = new SqlCommand("update MF_Item set Stock_Qty=@Stock_Qty where Item_ID='" + Item_ID + "' and DeleteInd <> 'X'", con);
            QRcmd.Parameters.AddWithValue("@Stock_Qty", txtStockQty.Text);
            QRcmd.ExecuteNonQuery();
        }
        else
        {
            //SqlCommand QRcmd = new SqlCommand("update MF_Item set Stock_Qty=@Stock_Qty where Item_ID='" + Item_ID + "' and DeleteInd <> 'X'", con);
            //QRcmd.Parameters.AddWithValue("@Stock_Qty", d);
            //QRcmd.ExecuteNonQuery();
        }


        //Store Qty Serve Per day
        if (txtQtyServePerday.Text != "")
        {
            SqlCommand QRcmd1 = new SqlCommand("update MF_Item set Qty_ServePerDay=@Qty_ServePerDay where Item_ID='" + Item_ID + "' and DeleteInd <> 'X'", con);
            if (txtQtyServePerday.Text != "0")
            {
                QRcmd1.Parameters.AddWithValue("@Qty_ServePerDay", txtQtyServePerday.Text);
            }
            else
            {
                QRcmd1.Parameters.AddWithValue("@Qty_ServePerDay", "9999999999");
            }
            QRcmd1.ExecuteNonQuery();
        }
        else
        {
            //SqlCommand QRcmd1 = new SqlCommand("update MF_Item set Qty_ServePerDay=@Qty_ServePerDay where Item_ID='" + Item_ID + "' and DeleteInd <> 'X'", con);
            //QRcmd1.Parameters.AddWithValue("@Qty_ServePerDay", d);
            //QRcmd1.ExecuteNonQuery();
        }

        //var lblitemid = (Label)txt.FindControl("lblitemid");
        //var Item_ID = lblitemid.Text;
        //var label6 = (Label)txt.FindControl("Card_ItemCode");
        //var Item_Code = label6.Text;

        ///
        /// Approve product or reject
        ///
        if (ddlAnR.SelectedValue == "New")
        {
            return;
        }
        else if (ddlAnR.SelectedValue == "Approved")
        {
            string p = "Yes";
            SqlConnection myQRConn = new SqlConnection(strConStringrp);
            myQRConn.Open();

            try
            {
                //Code to approve
                //SqlCommand QRcmd = new SqlCommand("update MF_Item set ApproveReject=@ApproveReject where Item_ID='" + Item_ID.Text.Trim().Replace("'", "`") + "' and DeleteInd <> 'X'", myQRConn);
                SqlCommand QRcmd = new SqlCommand("update MF_Item set ApproveReject=@ApproveReject where Item_ID='" + Item_ID + "' and DeleteInd <> 'X'", myQRConn);

                QRcmd.Parameters.AddWithValue("@ApproveReject", ddlAnR.SelectedValue);
                QRcmd.ExecuteNonQuery();
                SqlCommand QRcmd2 = new SqlCommand("insert into MF_ItemStatus_ActionLog (Item_Code,Publish,ApproveReject,LogDT) Values(@Item_Code,@Publish,@ApproveReject, @LogDT) ", myQRConn);
                QRcmd2.Parameters.AddWithValue("@ApproveReject", ddlAnR.SelectedValue);
                QRcmd2.Parameters.AddWithValue("@Publish", "");
                QRcmd2.Parameters.AddWithValue("@Item_Code", Item_Code);
                QRcmd2.Parameters.AddWithValue("@LogDT", DateTime.Now);
                QRcmd2.ExecuteNonQuery();
            }
            finally
            {
                myQRConn.Close();
                //Response.Redirect("BItemListing.aspx");
                //this.ItemLoadList(Convert.ToInt32(ddlPager.SelectedValue));
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "applyFootable", "applyFootable();", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "onpageload('');", true);
            }
        }
        else if (ddlAnR.SelectedValue == "Rejected")
        {
            string na = "No";
            SqlConnection myQRConn = new SqlConnection(strConStringrp);
            myQRConn.Open();

            try
            {
                //Code to reject
                //SqlCommand QRcmd = new SqlCommand("update MF_Item set ApproveReject=@ApproveReject where Item_ID='" + Item_ID.Text.Trim().Replace("'", "`") + "' and DeleteInd <> 'X'", myQRConn);
                SqlCommand QRcmd = new SqlCommand("update MF_Item set ApproveReject=@ApproveReject, Publish=@Publish where Item_ID='" + Item_ID + "' and DeleteInd <> 'X'", myQRConn);
                QRcmd.Parameters.AddWithValue("@ApproveReject", ddlAnR.SelectedValue);
                QRcmd.Parameters.AddWithValue("@Publish", na.ToString());
                QRcmd.ExecuteNonQuery();
                SqlCommand QRcmd2 = new SqlCommand("insert into MF_ItemStatus_ActionLog (Item_Code,Publish,ApproveReject,LogDT) Values(@Item_Code,@Publish,@ApproveReject, @LogDT) ", myQRConn);
                QRcmd2.Parameters.AddWithValue("@ApproveReject", ddlAnR.SelectedValue);
                QRcmd2.Parameters.AddWithValue("@Publish", "");
                QRcmd2.Parameters.AddWithValue("@Item_Code", Item_Code);
                QRcmd2.Parameters.AddWithValue("@LogDT", DateTime.Now);
                QRcmd2.ExecuteNonQuery();
            }
            finally
            {
                myQRConn.Close();
                //Response.Redirect("BItemListing.aspx");
                //this.ItemLoadList(Convert.ToInt32(ddlPager.SelectedValue));
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "applyFootable", "applyFootable();", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "onpageload('');", true);
            }
        }

        ///
        /// Publish and unpublish
        /// 

        if (ddlAnR.SelectedValue == "Approved")
        {
            if (ddlYnN.SelectedValue == "Yes")
            {
                SqlConnection myQRConn = new SqlConnection(strConStringrp);
                myQRConn.Open();

                try
                {
                    //Code to approve
                    //SqlCommand QRcmd = new SqlCommand("update MF_Item set ApproveReject=@ApproveReject where Item_ID='" + Item_ID.Text.Trim().Replace("'", "`") + "' and DeleteInd <> 'X'", myQRConn);
                    SqlCommand QRcmd = new SqlCommand("update MF_Item set Publish=@Publish where Item_ID='" + Item_ID + "' and DeleteInd <> 'X'", myQRConn);

                    QRcmd.Parameters.AddWithValue("@Publish", ddlYnN.SelectedValue);
                    QRcmd.ExecuteNonQuery();
                    SqlCommand QRcmd2 = new SqlCommand("insert into MF_ItemStatus_ActionLog (Item_Code,ApproveReject,Publish,LogDT) Values(@Item_Code,'',@Publish, @LogDT) ", myQRConn);
                    QRcmd2.Parameters.AddWithValue("@Publish", ddlYnN.SelectedValue);
                    QRcmd2.Parameters.AddWithValue("@Item_Code", Item_Code);
                    QRcmd2.Parameters.AddWithValue("@LogDT", DateTime.Now);
                    QRcmd2.ExecuteNonQuery();
                }
                finally
                {
                    myQRConn.Close();
                    //Response.Redirect("BItemListing.aspx");
                    //this.ItemLoadList(Convert.ToInt32(ddlPager.SelectedValue));
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "applyFootable", "applyFootable();", true);
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "onpageload('');", true);
                }
            }
            else if (ddlYnN.SelectedValue == "No")
            {
                SqlConnection myQRConn = new SqlConnection(strConStringrp);
                myQRConn.Open();

                try
                {
                    //Code to reject
                    //SqlCommand QRcmd = new SqlCommand("update MF_Item set ApproveReject=@ApproveReject where Item_ID='" + Item_ID.Text.Trim().Replace("'", "`") + "' and DeleteInd <> 'X'", myQRConn);
                    SqlCommand QRcmd = new SqlCommand("update MF_Item set Publish=@Publish where Item_ID='" + Item_ID + "' and DeleteInd <> 'X'", myQRConn);
                    QRcmd.Parameters.AddWithValue("@Publish", ddlYnN.SelectedValue);
                    QRcmd.ExecuteNonQuery();
                    SqlCommand QRcmd2 = new SqlCommand("insert into MF_ItemStatus_ActionLog (Item_Code,ApproveReject,Publish,LogDT) Values(@Item_Code,'',@Publish,@LogDT) ", myQRConn);
                    QRcmd2.Parameters.AddWithValue("@Publish", ddlYnN.SelectedValue);
                    QRcmd2.Parameters.AddWithValue("@Item_Code", Item_Code);
                    QRcmd2.Parameters.AddWithValue("@LogDT", DateTime.Now);
                    QRcmd2.ExecuteNonQuery();
                }
                finally
                {
                    myQRConn.Close();
                    //Response.Redirect("BItemListing.aspx");
                    //this.ItemLoadList(Convert.ToInt32(ddlPager.SelectedValue));
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "applyFootable", "applyFootable();", true);
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "onpageload('');", true);
                }
            }
        }

        ///
        /// FOC 
        /// 


        if (ddlfoc.SelectedValue == "Yes")
        {
            SqlConnection myQRConn = new SqlConnection(strConStringrp);
            myQRConn.Open();

            try
            {
                //Code to approve
                //SqlCommand QRcmd = new SqlCommand("update MF_Item set ApproveReject=@ApproveReject where Item_ID='" + Item_ID.Text.Trim().Replace("'", "`") + "' and DeleteInd <> 'X'", myQRConn);
                SqlCommand QRcmd = new SqlCommand("update MF_Item set FOC=@FOC where Item_ID='" + Item_ID + "' and DeleteInd <> 'X'", myQRConn);

                QRcmd.Parameters.AddWithValue("@FOC", ddlfoc.SelectedValue);
                QRcmd.ExecuteNonQuery();
            }
            finally
            {
                myQRConn.Close();
                //Response.Redirect("BItemListing.aspx");
                //this.ItemLoadList(Convert.ToInt32(ddlPager.SelectedValue));
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "applyFootable", "applyFootable();", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "onpageload('');", true);
            }
        }
        else if (ddlfoc.SelectedValue == "No")
        {
            SqlConnection myQRConn = new SqlConnection(strConStringrp);
            myQRConn.Open();

            try
            {
                //Code to reject
                //SqlCommand QRcmd = new SqlCommand("update MF_Item set ApproveReject=@ApproveReject where Item_ID='" + Item_ID.Text.Trim().Replace("'", "`") + "' and DeleteInd <> 'X'", myQRConn);
                SqlCommand QRcmd = new SqlCommand("update MF_Item set FOC=@FOC where Item_ID='" + Item_ID + "' and DeleteInd <> 'X'", myQRConn);
                QRcmd.Parameters.AddWithValue("@FOC", ddlfoc.SelectedValue);
                QRcmd.ExecuteNonQuery();
            }
            finally
            {
                myQRConn.Close();
                //Response.Redirect("BItemListing.aspx");
                //this.ItemLoadList(Convert.ToInt32(ddlPager.SelectedValue));
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "applyFootable", "applyFootable();", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "onpageload('');", true);
            }
        }

        ///
        ///  allow order table
        /// 


        if (ddlordertable.SelectedValue == "Yes")
        {
            SqlConnection myQRConn = new SqlConnection(strConStringrp);
            myQRConn.Open();

            try
            {
                //Code to approve
                //SqlCommand QRcmd = new SqlCommand("update MF_Item set ApproveReject=@ApproveReject where Item_ID='" + Item_ID.Text.Trim().Replace("'", "`") + "' and DeleteInd <> 'X'", myQRConn);
                SqlCommand QRcmd = new SqlCommand("update MF_Item set Self_Order_Menu=@Self_Order_Menu where Item_ID='" + Item_ID + "' and DeleteInd <> 'X'", myQRConn);

                QRcmd.Parameters.AddWithValue("@Self_Order_Menu", ddlordertable.SelectedValue);
                QRcmd.ExecuteNonQuery();
            }
            finally
            {
                myQRConn.Close();
                //Response.Redirect("BItemListing.aspx");
                //this.ItemLoadList(Convert.ToInt32(ddlPager.SelectedValue));
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "applyFootable", "applyFootable();", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "onpageload('');", true);
            }
        }
        else if (ddlordertable.SelectedValue == "No")
        {
            SqlConnection myQRConn = new SqlConnection(strConStringrp);
            myQRConn.Open();

            try
            {
                //Code to reject
                //SqlCommand QRcmd = new SqlCommand("update MF_Item set ApproveReject=@ApproveReject where Item_ID='" + Item_ID.Text.Trim().Replace("'", "`") + "' and DeleteInd <> 'X'", myQRConn);
                SqlCommand QRcmd = new SqlCommand("update MF_Item set Self_Order_Menu=@Self_Order_Menu where Item_ID='" + Item_ID + "' and DeleteInd <> 'X'", myQRConn);
                QRcmd.Parameters.AddWithValue("@Self_Order_Menu", ddlordertable.SelectedValue);
                QRcmd.ExecuteNonQuery();
            }
            finally
            {
                myQRConn.Close();
                //Response.Redirect("BItemListing.aspx");
                //this.ItemLoadList(Convert.ToInt32(ddlPager.SelectedValue));
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "applyFootable", "applyFootable();", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "onpageload('');", true);
            }
        }

        if (ddlwaiterorder.SelectedValue == "Yes")
        {
            SqlConnection myQRConn = new SqlConnection(strConStringrp);
            myQRConn.Open();

            try
            {
                SqlCommand QRcmd = new SqlCommand("update MF_Item set Waiter_Order_Menu=@Waiter_Order_Menu where Item_ID='" + Item_ID + "' and DeleteInd <> 'X'", myQRConn);

                QRcmd.Parameters.AddWithValue("@Waiter_Order_Menu", ddlwaiterorder.SelectedValue);
                QRcmd.ExecuteNonQuery();
            }
            finally
            {
                myQRConn.Close();
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "applyFootable", "applyFootable();", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "onpageload('');", true);
            }
        }
        else if (ddlwaiterorder.SelectedValue == "No")
        {
            SqlConnection myQRConn = new SqlConnection(strConStringrp);
            myQRConn.Open();

            try
            {
                SqlCommand QRcmd = new SqlCommand("update MF_Item set Waiter_Order_Menu=@Waiter_Order_Menu where Item_ID='" + Item_ID + "' and DeleteInd <> 'X'", myQRConn);
                QRcmd.Parameters.AddWithValue("@Waiter_Order_Menu", ddlwaiterorder.SelectedValue);
                QRcmd.ExecuteNonQuery();
            }
            finally
            {
                myQRConn.Close();
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "applyFootable", "applyFootable();", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "onpageload('');", true);
            }
        }

        if (ddlmember.SelectedValue == "Yes")
        {
            SqlConnection myQRConn = new SqlConnection(strConStringrp);
            myQRConn.Open();

            try
            {
                SqlCommand QRcmd = new SqlCommand("update MF_Item set Allow_In_Member=@Allow_In_Member where Item_ID='" + Item_ID + "' and DeleteInd <> 'X'", myQRConn);

                QRcmd.Parameters.AddWithValue("@Allow_In_Member", ddlmember.SelectedValue);
                QRcmd.ExecuteNonQuery();
            }
            finally
            {
                myQRConn.Close();
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "applyFootable", "applyFootable();", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "onpageload('');", true);
            }
        }
        else if (ddlmember.SelectedValue == "No")
        {
            SqlConnection myQRConn = new SqlConnection(strConStringrp);
            myQRConn.Open();

            try
            {
                SqlCommand QRcmd = new SqlCommand("update MF_Item set Allow_In_Member=@Allow_In_Member where Item_ID='" + Item_ID + "' and DeleteInd <> 'X'", myQRConn);
                QRcmd.Parameters.AddWithValue("@Allow_In_Member", ddlmember.SelectedValue);
                QRcmd.ExecuteNonQuery();
            }
            finally
            {
                myQRConn.Close();
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "applyFootable", "applyFootable();", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "onpageload('');", true);
            }
        }

        if (ddlallowstockist.SelectedValue == "Yes")
        {
            SqlConnection myQRConn = new SqlConnection(strConStringrp);
            myQRConn.Open();

            try
            {
                SqlCommand QRcmd = new SqlCommand("update MF_Item set Allow_In_Stockist=@Allow_In_Stockist where Item_ID='" + Item_ID + "' and DeleteInd <> 'X'", myQRConn);

                QRcmd.Parameters.AddWithValue("@Allow_In_Stockist", ddlallowstockist.SelectedValue);
                QRcmd.ExecuteNonQuery();
            }
            finally
            {
                myQRConn.Close();
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "applyFootable", "applyFootable();", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "onpageload('');", true);
            }
        }
        else if (ddlallowstockist.SelectedValue == "No")
        {
            SqlConnection myQRConn = new SqlConnection(strConStringrp);
            myQRConn.Open();

            try
            {
                SqlCommand QRcmd = new SqlCommand("update MF_Item set Allow_In_Stockist=@Allow_In_Stockist where Item_ID='" + Item_ID + "' and DeleteInd <> 'X'", myQRConn);
                QRcmd.Parameters.AddWithValue("@Allow_In_Stockist", ddlallowstockist.SelectedValue);
                QRcmd.ExecuteNonQuery();
            }
            finally
            {
                myQRConn.Close();
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "applyFootable", "applyFootable();", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "onpageload('');", true);
            }
        }

        SqlCommand Qcmd = new SqlCommand("update MF_Item set Modified_DT=@Modified_DT where Item_ID='" + Item_ID + "' and DeleteInd <> 'X'", con);
        Qcmd.Parameters.AddWithValue("@Modified_DT", DateTime.UtcNow.AddHours(8));
        Qcmd.ExecuteNonQuery();

        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Item Updated')", true);
        ItemLoadList(Convert.ToInt32(ddlPager.SelectedValue));
        con.Close();
    }

    protected void btn_new_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["user"] != null)
        {
            Response.Redirect("additem.aspx?item_code=&merchant=" + Request.QueryString["merchant"].ToString() + "&user=" + Request.QueryString["user"].ToString());
        }
        else
        {
            Response.Redirect("additem.aspx?item_code=&merchant=" + Request.QueryString["merchant"].ToString());

        }
        ;
    }

    protected void rpt_Item_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Edit")
        {
            if (Request.QueryString["user"] != null)
            {
                Response.Redirect("additem.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "&user=" + Request.QueryString["user"].ToString() + "&Item_code=" + e.CommandArgument.ToString());
            }
            else
            {
                Response.Redirect("additem.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "&Item_code=" + e.CommandArgument.ToString());

            }
            ;

        }

        if (e.CommandName == "AssignModifier")
        {
            hditem_code.Value = e.CommandArgument.ToString();

            loadmodifier();

            ScriptManager.RegisterStartupScript(this, GetType(), "pop", "$('#ModifierModal').modal('show');", true);
        }

        if (e.CommandName == "AssignPrinter")
        {
            hditem_code.Value = e.CommandArgument.ToString();

            loadprinter();

            ScriptManager.RegisterStartupScript(this, GetType(), "pop", "$('#PrinterModal').modal('show');", true);
        }

        if (e.CommandName == "Delete")
        {
            Label Item_ID = (Label)e.Item.FindControl("lblitemid");

            Int32 tv = 50;

            using (SqlConnection myTxConn = new SqlConnection(strConStringrp))
            {
                using (SqlCommand myTxcmd = new SqlCommand("Delete_Item", myTxConn))
                {
                    myTxcmd.CommandType = CommandType.StoredProcedure;
                    myTxcmd.Parameters.AddWithValue("@ItemID", Convert.ToInt32(Item_ID.Text));
                    myTxcmd.Parameters.AddWithValue("@ItemCode", e.CommandArgument.ToString());

                    SqlParameter TReturnValue = myTxcmd.Parameters.Add("returnValue", SqlDbType.Int, 4);
                    TReturnValue.Direction = ParameterDirection.ReturnValue;

                    myTxConn.Open();

                    try
                    {
                        myTxcmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw (ex);
                    }
                    finally
                    {
                        myTxcmd.Dispose();
                        myTxConn.Close();
                    }

                    tv = Convert.ToInt32(TReturnValue.Value.ToString());
                }

                if (tv == 100)
                {
                    if (Request.QueryString["user"] != null)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Delete success.');window.location ='ContentApproval.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "&user=" + Request.QueryString["user"].ToString() + "';", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Delete success.');window.location ='ContentApproval.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "';", true);

                    }
                    ;



                }
                else if (tv == 80)
                {
                    if (Request.QueryString["user"] != null)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Delete success.');window.location ='ContentApproval.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "&user=" + Request.QueryString["user"].ToString() + "';", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Delete success.');window.location ='ContentApproval.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "';", true);
                    }
                    ;

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Delete failed. Please try again later.');", true);
                }
            }

            this.ItemLoadList(Convert.ToInt32(ddlPager.SelectedValue));
        }
    }

    protected void ddlcostrate_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlcostrate.SelectedValue == "")
        {
            this.ItemLoadList(Convert.ToInt32(ddlPager.SelectedValue));
        }
        else if (ddlcostrate.SelectedValue == "Zero")
        {
            this.ZeroItemLoadList(Convert.ToInt32(ddlPager.SelectedValue));
        }
        else if (ddlcostrate.SelectedValue == "Under")
        {
            this.UnderItemLoadList(Convert.ToInt32(ddlPager.SelectedValue));
        }

    }

    protected void btnmarkup_Click(object sender, EventArgs e)
    {
        Button txt = sender as Button;
        var Card_ItemCode = (Label)txt.FindControl("Card_ItemCode");
        var Item_Code = Card_ItemCode.Text;
        var txtNewMprice = (TextBox)txt.FindControl("lblMPrice");
        var txtNewVprice = (TextBox)txt.FindControl("lblVPrice");
        var txtNewSprice = (TextBox)txt.FindControl("lblSPrice");
        var lblVVIPPrice = (TextBox)txt.FindControl("lblVVIPPrice");
        var lblMSPrice = (TextBox)txt.FindControl("lblMSPrice");
        var txtcost = (TextBox)txt.FindControl("txtcost");
        var costpercent = (HiddenField)txt.FindControl("costpercent");

        string MStockistprice = "D05";
        string VVIPprice = "D04";
        string Stockistprice = "D03";
        string VIPprice = "D01";
        string memberprice = "D02";

        SqlConnection con = new SqlConnection(strConStringrp);
        SqlCommand checkcmd = new SqlCommand("select * from ap_merchant where merchantid = '" + Request.QueryString["merchant"].ToString() + "'", con);
        SqlDataAdapter checkadp = new SqlDataAdapter(checkcmd);
        DataTable checkdt = new DataTable();
        checkadp.Fill(checkdt);
        con.Open();
        if (checkdt.Rows[0]["MerchantType"].ToString() == "Markup")
        {
            decimal markup = Convert.ToDecimal(checkdt.Rows[0]["MarkupPercentage"].ToString());
            costpercent.Value = markup.ToString();

            string aftermarkup = (Convert.ToDecimal(txtcost.Text.Replace(",", "")) + (Convert.ToDecimal(txtcost.Text.Replace(",", "")) * markup / 100)).ToString();

            decimal roundup = System.Math.Ceiling(Convert.ToDecimal(aftermarkup) * 10) / 10;

            string confirmValue = "";
            confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {

                txtNewMprice.Text = roundup.ToString("0.00");
                txtNewVprice.Text = roundup.ToString("0.00");
                txtNewSprice.Text = roundup.ToString("0.00");
                lblVVIPPrice.Text = roundup.ToString("0.00");
                lblMSPrice.Text = roundup.ToString("0.00");


                SqlCommand costcmd = new SqlCommand("update MF_item set vendorcost=@cost, Modified_DT=@Modified_DT where Item_Code='" + Item_Code + "' and deleteind <> 'x'", con);
                costcmd.Parameters.AddWithValue("@cost", Convert.ToDecimal(txtcost.Text.Replace(",", "")));
                costcmd.Parameters.AddWithValue("@Modified_DT", DateTime.UtcNow.AddHours(8));
                costcmd.ExecuteNonQuery();

                //Check All row if have row then proceed to save price
                SqlCommand cmdr4 = new SqlCommand("select *  from MF_AgentCommi where Item_Code='" + Item_Code + "'", con);
                SqlDataReader dv12 = cmdr4.ExecuteReader();

                if (dv12.HasRows == true)
                {
                    dv12.Close();
                    dv12.Dispose();

                    //Check AgentLevelCode If have row then store otherwise show error message
                    SqlCommand cmdr = new SqlCommand("select *  from MF_AgentCommi where Item_Code='" + Item_Code + "' and AgentLevelCode='" + memberprice + "'", con);
                    SqlDataReader dv = cmdr.ExecuteReader();

                    if (dv.HasRows == true)
                    {
                        //Update Member Price 
                        SqlConnection CON1 = new SqlConnection(strConStringrp);
                        CON1.Open();
                        SqlCommand cmd = new SqlCommand("update MF_AgentCommi set BuyPrice=@BuyPrice, Modified_DT=@Modified_DT where Item_Code='" + Item_Code + "' and AgentLevelCode='" + memberprice + "'", CON1);
                        cmd.Parameters.AddWithValue("@BuyPrice", Convert.ToDecimal(txtNewMprice.Text));
                        cmd.Parameters.AddWithValue("@Modified_DT", DateTime.UtcNow.AddHours(8));
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        if (Request.QueryString["user"] != null)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Item Updated. There is no member price.\\\nPlease contact to administrator to add member buy price.');window.location ='ContentApproval.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "&user=" + Request.QueryString["user"].ToString() + "';", true);
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Item Updated. There is no member price.\\\nPlease contact to administrator to add member buy price.');window.location ='ContentApproval.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "';", true);
                        }
                        ;

                    }

                    dv.Close();
                    dv.Dispose();

                    //Check AgentLevelCode If have row then store otherwise show error message
                    SqlCommand cmdr1 = new SqlCommand("select *  from MF_AgentCommi where Item_Code='" + Item_Code + "' and AgentLevelCode='" + VIPprice + "'", con);
                    SqlDataReader dv1 = cmdr1.ExecuteReader();

                    if (dv1.HasRows == true)
                    {
                        //Update VIP Price
                        SqlConnection CON1 = new SqlConnection(strConStringrp);
                        CON1.Open();
                        SqlCommand cmd1 = new SqlCommand("update MF_AgentCommi set BuyPrice=@BuyPrice, Modified_DT=@Modified_DT where Item_Code='" + Item_Code + "' and AgentLevelCode='" + VIPprice + "'", CON1);
                        cmd1.Parameters.AddWithValue("@BuyPrice", Convert.ToDecimal(txtNewVprice.Text));
                        cmd1.Parameters.AddWithValue("@Modified_DT", DateTime.UtcNow.AddHours(8));
                        cmd1.ExecuteNonQuery();
                    }
                    else
                    {
                        if (Request.QueryString["user"] != null)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Item Updated. There is no VIP price.\\\nPlease contact to administrator to add VIP buy price.');window.location ='ContentApproval.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "&user=" + Request.QueryString["user"].ToString() + "';", true);
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Item Updated. There is no VIP price.\\\nPlease contact to administrator to add VIP buy price.');window.location ='ContentApproval.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "';", true);

                        }
                        ;

                    }

                    dv1.Close();
                    dv1.Dispose();

                    //Check AgentLevelCode If have row then store otherwise show error message
                    SqlCommand cmdr5 = new SqlCommand("select *  from MF_AgentCommi where Item_Code='" + Item_Code + "' and AgentLevelCode='" + VVIPprice + "'", con);
                    SqlDataReader dv5 = cmdr5.ExecuteReader();

                    if (dv5.HasRows == true)
                    {
                        //Update VIP Price
                        SqlConnection CON5 = new SqlConnection(strConStringrp);
                        CON5.Open();
                        SqlCommand cmd5 = new SqlCommand("update MF_AgentCommi set BuyPrice=@BuyPrice, Modified_DT=@Modified_DT where Item_Code='" + Item_Code + "' and AgentLevelCode='" + VVIPprice + "'", CON5);
                        cmd5.Parameters.AddWithValue("@BuyPrice", Convert.ToDecimal(lblVVIPPrice.Text));
                        cmd5.Parameters.AddWithValue("@Modified_DT", DateTime.UtcNow.AddHours(8));
                        cmd5.ExecuteNonQuery();
                    }
                    else
                    {
                        if (Request.QueryString["user"] != null)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Item Updated. There is no VVIP price.\\\nPlease contact to administrator to add VIP buy price.');window.location ='ContentApproval.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "&user=" + Request.QueryString["user"].ToString() + "';", true);
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Item Updated. There is no VVIP price.\\\nPlease contact to administrator to add VIP buy price.');window.location ='ContentApproval.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "';", true);
                        }
                        ;

                    }

                    dv5.Close();
                    dv5.Dispose();

                    //Check AgentLevelCode If have row then store otherwise show error message
                    SqlCommand cmdr6 = new SqlCommand("select *  from MF_AgentCommi where Item_Code='" + Item_Code + "' and AgentLevelCode='" + MStockistprice + "'", con);
                    SqlDataReader dv6 = cmdr6.ExecuteReader();

                    if (dv6.HasRows == true)
                    {
                        //Stockist Update 
                        SqlConnection CON6 = new SqlConnection(strConStringrp);
                        CON6.Open();
                        SqlCommand cmd6 = new SqlCommand("update MF_AgentCommi set BuyPrice=@BuyPrice, Modified_DT=@Modified_DT where Item_Code='" + Item_Code + "' and AgentLevelCode='" + MStockistprice + "'", CON6);
                        cmd6.Parameters.AddWithValue("@BuyPrice", Convert.ToDecimal(lblMSPrice.Text));
                        cmd6.Parameters.AddWithValue("@Modified_DT", DateTime.UtcNow.AddHours(8));
                        cmd6.ExecuteNonQuery();
                    }
                    else
                    {
                        if (Request.QueryString["user"] != null)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Item Updated. There is no stockist price.\\\nPlease contact to administrator to add stockist buy price.');window.location ='ContentApproval.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "&user=" + Request.QueryString["user"].ToString() + "';", true);
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Item Updated. There is no stockist price.\\\nPlease contact to administrator to add stockist buy price.');window.location ='ContentApproval.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "';", true);
                        }
                        ;

                    }

                    dv6.Close();
                    dv6.Dispose();

                    //Check AgentLevelCode If have row then store otherwise show error message
                    SqlCommand cmdr7 = new SqlCommand("select *  from MF_AgentCommi where Item_Code='" + Item_Code + "' and AgentLevelCode='" + Stockistprice + "'", con);
                    SqlDataReader dv7 = cmdr7.ExecuteReader();

                    if (dv7.HasRows == true)
                    {
                        //Stockist Update 
                        SqlConnection CON7 = new SqlConnection(strConStringrp);
                        CON7.Open();
                        SqlCommand cmd7 = new SqlCommand("update MF_AgentCommi set BuyPrice=@BuyPrice, Modified_DT=@Modified_DT where Item_Code='" + Item_Code + "' and AgentLevelCode='" + Stockistprice + "'", CON7);
                        cmd7.Parameters.AddWithValue("@BuyPrice", Convert.ToDecimal(txtNewSprice.Text));
                        cmd7.Parameters.AddWithValue("@Modified_DT", DateTime.UtcNow.AddHours(8));
                        cmd7.ExecuteNonQuery();
                    }
                    else
                    {
                        if (Request.QueryString["user"] != null)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Item Updated. There is no stockist price.\\\nPlease contact to administrator to add stockist buy price.');window.location ='ContentApproval.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "&user=" + Request.QueryString["user"].ToString() + "';", true);
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Item Updated. There is no stockist price.\\\nPlease contact to administrator to add stockist buy price.');window.location ='ContentApproval.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "';", true);
                        }
                        ;

                    }

                    dv7.Close();
                    dv7.Dispose();

                }
                else
                {
                    if (Request.QueryString["user"] != null)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Item Updated. You Cannot update price because there is no buy price for Member, VIP and Stockist.\\nPlease contact to administrator to add buy price.');window.location ='ContentApproval.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "&user=" + Request.QueryString["user"].ToString() + "';", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Item Updated. You Cannot update price because there is no buy price for Member, VIP and Stockist.\\nPlease contact to administrator to add buy price.');window.location ='ContentApproval.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "';", true);
                    }
                    ;

                }
            }
            else
            {

            }
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('This merchant is not set markup.')", true);
        }
    }

    protected void btnpromo_Click(object sender, EventArgs e)
    {

        LinkButton txt = sender as LinkButton;

        var Card_ShortDesc = (Label)txt.FindControl("Card_ShortDesc");
        var Card_Barcode = (Label)txt.FindControl("Card_Barcode");
        var Card_ItemCode = (Label)txt.FindControl("Card_ItemCode");
        var Item_Code = Card_ItemCode.Text;
        var txtNewMprice = (TextBox)txt.FindControl("lblMPrice");
        var txtNewVprice = (TextBox)txt.FindControl("lblVPrice");
        var txtNewSprice = (TextBox)txt.FindControl("lblSPrice");
        var lblVVIPPrice = (TextBox)txt.FindControl("lblVVIPPrice");
        var lblMSPrice = (TextBox)txt.FindControl("lblMSPrice");
        var txtMcashback = (TextBox)txt.FindControl("txtMcashback");
        var txtVcashback = (TextBox)txt.FindControl("txtVcashback");
        var txtScashback = (TextBox)txt.FindControl("txtScashback");
        var txtcost = (TextBox)txt.FindControl("txtcost");
        var txtVVIPcashback = (TextBox)txt.FindControl("txtVVIPcashback");
        var txtMScashback = (TextBox)txt.FindControl("txtMScashback");
        var lblitemid = (Label)txt.FindControl("lblitemid");
        var Item_ID = lblitemid.Text;

        lblpbarcode.Text = Card_Barcode.Text;
        lblpitemname.Text = Card_ShortDesc.Text;
        lblpitem_code.Text = Card_ItemCode.Text;



        SqlConnection con = new SqlConnection(strConStringrp);

        SqlCommand cmditem = new SqlCommand("select *  from MF_Item where Item_Code='" + Item_Code + "' and deleteind <> 'X'", con);
        SqlDataAdapter adpitem = new SqlDataAdapter(cmditem);
        DataTable dtitem = new DataTable();
        adpitem.Fill(dtitem);

        if (dtitem.Rows.Count > 0)
        {
            SqlCommand cmd = new SqlCommand("select  b.campaigntitle,b.ids from MF_AgentPromo as a left join MF_PromoCampaign as b on b.ids=a.PromoId where a.Item_Code='" + Item_Code + "' and a.deleteind <> 'X' and b.DeleteInd <> 'X' group by b.campaigntitle,b.ids ", con);
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataTable dcmdt = new DataTable();
            adp.Fill(dcmdt);
            if (dcmdt.Rows.Count > 0)
            {
                ddlcampaign.DataSource = dcmdt;
                ddlcampaign.DataTextField = "campaigntitle";
                ddlcampaign.DataValueField = "ids";
                ddlcampaign.DataBind();
            }

            if (dtitem.Rows[0]["FilePath"] != DBNull.Value)
            {
                btnimage.ImageUrl = dtitem.Rows[0]["FilePath"].ToString();
            }
            else
            {

            }
            {
                btnimage.ImageUrl = "Images/NoPic.png";
            }

            using (SqlCommand loadcampaigncmd = new SqlCommand("select a.item_code,a.LongDesc,a.Barcode,a.FilePath,b.SPP,b.ReferrerProfitBonusPercent,b.TeamKPIBonus,b.TeamKPIBonusMS,b.MEB,b.MBD,b.start_date,b.end_date " +
                ",ISNULL(p.[BuyPrice], 0) as [MemberPrice] " +
                ", ISNULL(q.[BuyPrice], 0) as [VIPPrice] " +
                ", ISNULL(r.[BuyPrice], 0) as [StockistPrice] " +
                ", ISNULL(s.[BuyPrice], 0) as [VVIPPrice] " +
                ", ISNULL(t.[BuyPrice], 0) as [MStockistPrice] " +
                ", ISNULL(p.[CashBackAmount], 0) as [MemberCashback] " +
                ", ISNULL(q.[CashBackAmount], 0) as [VIPCashback] " +
                ", ISNULL(r.[CashBackAmount], 0) as [StockistCashback] " +
                ", ISNULL(s.[CashBackAmount], 0) as [VVIPCashback] " +
                ", ISNULL(t.[CashBackAmount], 0) as [MStockistCashback] " +
                ", ISNULL(aa.[Promo_price], 0) as [PMemberPrice] " +
                ", ISNULL(ab.[Promo_price], 0) as [PVIPPrice] " +
                ", ISNULL(ac.[Promo_price], 0) as [PStockistPrice] " +
                ", ISNULL(ad.[Promo_price], 0) as [PVVIPPrice] " +
                ", ISNULL(ae.[Promo_price], 0) as [PMStockistPrice] " +
                ", ISNULL(aa.[CashBackAmt], 0) as [PMemberCashback] " +
                ", ISNULL(ab.[CashBackAmt], 0) as [PVIPCashback] " +
                ", ISNULL(ac.[CashBackAmt], 0) as [PStockistCashback] " +
                ", ISNULL(ad.[CashBackAmt], 0) as [PVVIPCashback] " +
                ", ISNULL(ae.[CashBackAmt], 0) as [PMStockistCashback] " +
                ", ISNULL(aa.[RedemptionPoint], 0) as [PMRedemptionPoint] " +
                ", ISNULL(ab.[RedemptionPoint], 0) as [PVIPRedemptionPoint] " +
                ", ISNULL(ac.[RedemptionPoint], 0) as [PStockistRedemptionPoint] " +
                ", ISNULL(ad.[RedemptionPoint], 0) as [PVVIPRedemptionPoint] " +
                ", ISNULL(ae.[RedemptionPoint], 0) as [PMStockistRedemptionPoint] " +
                ", ISNULL(aa.[EarnRPWhenBuy], 0) as [PMemberEarnRPWhenBuy] " +
                ", ISNULL(ab.[EarnRPWhenBuy], 0) as [PVIPEarnRPWhenBuy] " +
                ", ISNULL(ac.[EarnRPWhenBuy], 0) as [PStockistEarnRPWhenBuy] " +
                ", ISNULL(ad.[EarnRPWhenBuy], 0) as [PVVIPEarnRPWhenBuy] " +
                ", ISNULL(ae.[EarnRPWhenBuy], 0) as [PMStockistEarnRPWhenBuy] " +
                ",ae.discount_Amt as [MSDiscountAmt]," +
                "ac.discount_Amt as [SDiscountAmt]," +
                "ad.discount_Amt as [VVIPDiscountAmt]," +
                "ab.discount_Amt as [VDiscountAmt] , " +
                "aa.discount_Amt as [MDiscountAmt]," +
                "case when ae.discount_type = 'DBP' then 'Discount By Percentage (%)' else 'Discount By Amount' End as [MSDiscountType]," +
                "case when ac.discount_type = 'DBP' then 'Discount By Percentage (%)' else 'Discount By Amount' End as [SDiscountType]," +
                "case when ad.discount_type = 'DBP' then 'Discount By Percentage (%)' else 'Discount By Amount' End as [VVIPDiscountType]," +
                "case when ab.discount_type = 'DBP' then 'Discount By Percentage (%)' else 'Discount By Amount' End as [VDiscountType] , " +
                "case when aa.discount_type = 'DBP' then 'Discount By Percentage (%)' else 'Discount By Amount' End as [MDiscountType]" +
                "from MF_Item AS a " +
                "LEFT JOIN [MF_AgentPromo] AS b ON b.item_code=a.item_code and b.[AgentLevelCode]='D02' and b.[DeleteInd] <> 'X' and b.promoid='" + ddlcampaign.SelectedValue + "'" +
                "LEFT JOIN [MF_AgentCommi] AS p ON p.Item_Code = a.Item_Code and p.[AgentLevelCode]='D02' and p.[DeleteInd] <> 'X' " +
                "LEFT JOIN [MF_AgentCommi] AS q ON q.Item_Code = a.Item_Code and q.[AgentLevelCode]='D01' and q.[DeleteInd] <> 'X' " +
                "LEFT JOIN [MF_AgentCommi] AS r ON r.Item_Code = a.Item_Code and r.[AgentLevelCode]='D03' and r.[DeleteInd] <> 'X' " +
                "LEFT JOIN [MF_AgentCommi] AS s ON s.Item_Code = a.Item_Code and s.[AgentLevelCode]='D04' and s.[DeleteInd] <> 'X' " +
                "LEFT JOIN [MF_AgentCommi] AS t ON t.Item_Code = a.Item_Code and t.[AgentLevelCode]='D05' and t.[DeleteInd] <> 'X'  " +
                "LEFT JOIN [MF_AgentPromo] AS aa ON aa.Item_Code = a.Item_Code and aa.[AgentLevelCode]='D02' and aa.[DeleteInd] <> 'X' and aa.promoid='" + ddlcampaign.SelectedValue + "' " +
                "LEFT JOIN [MF_AgentPromo] AS ab ON ab.Item_Code = a.Item_Code and ab.[AgentLevelCode]='D01' and ab.[DeleteInd] <> 'X' and ab.promoid='" + ddlcampaign.SelectedValue + "' " +
                "LEFT JOIN [MF_AgentPromo] AS ac ON ac.Item_Code = a.Item_Code and ac.[AgentLevelCode]='D03' and ac.[DeleteInd] <> 'X' and ac.promoid='" + ddlcampaign.SelectedValue + "' " +
                "LEFT JOIN [MF_AgentPromo] AS ad ON ad.Item_Code = a.Item_Code and ad.[AgentLevelCode]='D04' and ad.[DeleteInd] <> 'X' and ad.promoid='" + ddlcampaign.SelectedValue + "' " +
                "LEFT JOIN [MF_AgentPromo] AS ae ON ae.Item_Code = a.Item_Code and ae.[AgentLevelCode]='D05' and ae.[DeleteInd] <> 'X' and ae.promoid='" + ddlcampaign.SelectedValue + "'  " +
                "where a.deleteind <> 'X' and a.item_code='" + lblpitem_code.Text + "' " +
                " group by a.item_code,a.LongDesc,a.Barcode,a.FilePath,b.start_date,b.end_date , " +
                "b.SPP,b.ReferrerProfitBonusPercent,b.TeamKPIBonus,b.TeamKPIBonusMS,b.MEB,b.MBD,p.CashBackAmount,q.CashBackAmount,r.CashBackAmount," +
                "s.CashBackAmount,t.CashBackAmount,p.BuyPrice,q.BuyPrice,r.BuyPrice,s.BuyPrice,t.BuyPrice ,aa.RedemptionPoint,ab.RedemptionPoint,ac.RedemptionPoint,ad.RedemptionPoint,ae.RedemptionPoint" +
                ",aa.EarnRPWhenBuy,ab.EarnRPWhenBuy,ac.EarnRPWhenBuy,ad.EarnRPWhenBuy,ae.EarnRPWhenBuy," +
                "aa.CashBackAmt,ab.CashBackAmt,ac.CashBackAmt,ad.CashBackAmt,ae.CashBackAmt,aa.Promo_price,ab.Promo_price,ac.Promo_price,ad.Promo_price,ae.Promo_price,aa.discount_type,ab.discount_type," +
                "ac.discount_type,ad.discount_type,ae.discount_type,aa.discount_amt,ab.discount_amt,ac.discount_amt,ad.discount_amt,ae.discount_amt", con))
            {
                SqlDataAdapter loadcampaignadp = new SqlDataAdapter(loadcampaigncmd);
                DataTable loadcampaigndt = new DataTable();
                loadcampaignadp.Fill(loadcampaigndt);

                if (loadcampaigndt.Rows.Count > 0)
                {
                    txtstartdate.Text = loadcampaigndt.Rows[0]["start_date"].ToString();
                    txtenddate.Text = loadcampaigndt.Rows[0]["end_date"].ToString();
                    lblpitem_code.Text = loadcampaigndt.Rows[0]["item_code"].ToString();
                    lblpitemname.Text = loadcampaigndt.Rows[0]["longdesc"].ToString();
                    lblpbarcode.Text = loadcampaigndt.Rows[0]["barcode"].ToString();
                    lblpMpromoPrice.Text = Convert.ToDecimal(loadcampaigndt.Rows[0]["PMemberPrice"]).ToString("###0.00");
                    lblpMprice.Text = Convert.ToDecimal(loadcampaigndt.Rows[0]["MemberPrice"]).ToString("###0.00");
                    txtpMpromocashback.Text = Convert.ToDecimal(loadcampaigndt.Rows[0]["PMemberCashback"]).ToString("###0.00");
                    lblpVpromoPrice.Text = Convert.ToDecimal(loadcampaigndt.Rows[0]["PVIPPrice"]).ToString("###0.00");
                    lblpVPrice.Text = Convert.ToDecimal(loadcampaigndt.Rows[0]["VIPPrice"]).ToString("###0.00");
                    txtpVpromocashback.Text = Convert.ToDecimal(loadcampaigndt.Rows[0]["PVIPCashback"]).ToString("###0.00");
                    lblpVVIPpromoPrice.Text = Convert.ToDecimal(loadcampaigndt.Rows[0]["PVVIPPrice"]).ToString("###0.00");
                    lblpVVIPPrice.Text = Convert.ToDecimal(loadcampaigndt.Rows[0]["VVIPPrice"]).ToString("###0.00");
                    txtpVVIPpromocashback.Text = Convert.ToDecimal(loadcampaigndt.Rows[0]["PVVIPCashback"]).ToString("###0.00");
                    lblpSpromoPrice.Text = Convert.ToDecimal(loadcampaigndt.Rows[0]["PStockistPrice"]).ToString("###0.00");
                    lblpSPrice.Text = Convert.ToDecimal(loadcampaigndt.Rows[0]["StockistPrice"]).ToString("###0.00");
                    txtpSpromocashback.Text = Convert.ToDecimal(loadcampaigndt.Rows[0]["PStockistCashback"]).ToString("###0.00");
                    lblpMSpromoPrice.Text = Convert.ToDecimal(loadcampaigndt.Rows[0]["PMStockistPrice"]).ToString("###0.00");
                    lblpMSPrice.Text = Convert.ToDecimal(loadcampaigndt.Rows[0]["MStockistPrice"]).ToString("###0.00");
                    txtpMSpromocashback.Text = Convert.ToDecimal(loadcampaigndt.Rows[0]["PMStockistCashback"]).ToString("###0.00");
                    lblMdiscType.Text = loadcampaigndt.Rows[0]["MDiscountType"].ToString();
                    lblSDiscType.Text = loadcampaigndt.Rows[0]["SDiscountType"].ToString();
                    lblVDiscType.Text = loadcampaigndt.Rows[0]["VDiscountType"].ToString();
                    lblVVIPDiscType.Text = loadcampaigndt.Rows[0]["VVIPDiscountType"].ToString();
                    lblMSDiscType.Text = loadcampaigndt.Rows[0]["MSDiscountType"].ToString();
                    lblMdiscAmt.Text = loadcampaigndt.Rows[0]["MDiscountAmt"].ToString();
                    lblVDiscAmt.Text = loadcampaigndt.Rows[0]["VDiscountAmt"].ToString();
                    lblVVIPDiscAmt.Text = loadcampaigndt.Rows[0]["VVIPDiscountAmt"].ToString();
                    lblSDiscAmt.Text = loadcampaigndt.Rows[0]["SDiscountAmt"].ToString();
                    lblMSDiscAmt.Text = loadcampaigndt.Rows[0]["MSDiscountAmt"].ToString();

                    object teamkpibonusms = loadcampaigndt.Rows[0]["TeamKPIBonusMS"];// retrieve value from database

                    if (teamkpibonusms != DBNull.Value)
                    {
                        txtTeamKPI2.Text = Convert.ToDecimal(loadcampaigndt.Rows[0]["TeamKPIBonusMS"]).ToString("###0.00");
                    }
                    else
                    {
                        txtTeamKPI2.Text = "";
                    }

                    object teamkpibonus = loadcampaigndt.Rows[0]["TeamKPIBonus"];// retrieve value from database

                    if (teamkpibonus != DBNull.Value)
                    {
                        txtTeamKPI.Text = Convert.ToDecimal(loadcampaigndt.Rows[0]["TeamKPIBonus"]).ToString("###0.00");
                    }
                    else
                    {
                        txtTeamKPI.Text = "";
                    }

                    txtMredemptpoint.Text = Convert.ToDecimal(loadcampaigndt.Rows[0]["PMRedemptionPoint"]).ToString("###0.00");
                    txtMrewardpoint.Text = Convert.ToDecimal(loadcampaigndt.Rows[0]["PMemberEarnRPWhenBuy"]).ToString("###0.00");
                    txtMSredemptpoint.Text = Convert.ToDecimal(loadcampaigndt.Rows[0]["PMStockistRedemptionPoint"]).ToString("###0.00");
                    txtMSrewardpoint.Text = Convert.ToDecimal(loadcampaigndt.Rows[0]["PMStockistEarnRPWhenBuy"]).ToString("###0.00");
                    txtVIPredemptpoint.Text = Convert.ToDecimal(loadcampaigndt.Rows[0]["PVIPRedemptionPoint"]).ToString("###0.00");
                    txtVIPrewardpoint.Text = Convert.ToDecimal(loadcampaigndt.Rows[0]["PVIPEarnRPWhenBuy"]).ToString("###0.00");
                    txtVVIPredemptpoint.Text = Convert.ToDecimal(loadcampaigndt.Rows[0]["PVVIPRedemptionPoint"]).ToString("###0.00");
                    txtVVIPrewardpoint.Text = Convert.ToDecimal(loadcampaigndt.Rows[0]["PVVIPEarnRPWhenBuy"]).ToString("###0.00");
                    txtSredemptpoint.Text = Convert.ToDecimal(loadcampaigndt.Rows[0]["PStockistRedemptionPoint"]).ToString("###0.00");
                    txtSrewardpoint.Text = Convert.ToDecimal(loadcampaigndt.Rows[0]["PStockistEarnRPWhenBuy"]).ToString("###0.00");

                    if (string.IsNullOrEmpty(loadcampaigndt.Rows[0]["ReferrerProfitBonusPercent"].ToString()))
                    {
                        txtRefProfitBonusPercent.Text = "0";
                    }
                    else
                    {
                        txtRefProfitBonusPercent.Text = Convert.ToDecimal(loadcampaigndt.Rows[0]["ReferrerProfitBonusPercent"]).ToString("0.00");
                    }
                    if (string.IsNullOrEmpty(loadcampaigndt.Rows[0]["SPP"].ToString()))
                    {
                        txtSPP.Text = "0";
                    }
                    else
                    {
                        txtSPP.Text = loadcampaigndt.Rows[0]["SPP"].ToString();
                    }
                    if (string.IsNullOrEmpty(loadcampaigndt.Rows[0]["MBD"].ToString()))
                    {
                        txtMBD.Text = "0";
                    }
                    else
                    {
                        txtMBD.Text = loadcampaigndt.Rows[0]["MBD"].ToString();
                    }
                    if (string.IsNullOrEmpty(loadcampaigndt.Rows[0]["MEB"].ToString()))
                    {
                        txtMEB.Text = "0";
                    }
                    else
                    {
                        txtMEB.Text = loadcampaigndt.Rows[0]["MEB"].ToString();
                    }
                    if (string.IsNullOrEmpty(loadcampaigndt.Rows[0]["TeamKPIBonusMS"].ToString()))
                    {
                        txtTeamKPI2.Text = "0";
                    }
                    else
                    {
                        txtTeamKPI2.Text = loadcampaigndt.Rows[0]["TeamKPIBonusMS"].ToString();
                    }
                    if (string.IsNullOrEmpty(loadcampaigndt.Rows[0]["TeamKPIBonus"].ToString()))
                    {
                        txtTeamKPI.Text = "0";
                    }
                    else
                    {
                        txtTeamKPI.Text = loadcampaigndt.Rows[0]["TeamKPIBonus"].ToString();
                    }



                }
            }

        }

        //ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
        ScriptManager.RegisterStartupScript(this, GetType(), "pop", "$('#PromoModal').modal('show');", true);
    }


    protected void grdview_MemPromo_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView dt = (DataRowView)e.Row.DataItem;
            if (dt[4].ToString() != "")
            {
                string date = dt[4].ToString();

                if (DateTime.UtcNow.AddHours(8) >= Convert.ToDateTime(dt[4].ToString()) && DateTime.UtcNow.AddHours(8) <= Convert.ToDateTime(dt[5].ToString()))
                {
                    e.Row.BackColor = Color.LightGreen;
                }
                else
                {
                    e.Row.BackColor = Color.Transparent;
                }
            }


        }

        if (e.Row.RowType == DataControlRowType.Footer)
        {
            using (SqlConnection con = new SqlConnection(strConStringrp))
            {
                DropDownList ddl = null;
                ddl = e.Row.FindControl("ddlpromocampaignNew") as DropDownList;
                if (ddl != null)
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("select * from mf_promocampaign where deleteind <> 'X'", con);
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    DataTable dcmdt = new DataTable();
                    adp.Fill(dcmdt);
                    if (dcmdt.Rows.Count > 0)
                    {
                        ddl.DataSource = dcmdt;
                        ddl.DataTextField = "campaigntitle";
                        ddl.DataValueField = "ids";
                        ddl.DataBind();
                        ddl.Items.Insert(0, new ListItem("-Select Promotion-", ""));
                    }

                    con.Close();
                }
            }

        }
    }

    protected void grdview_vipPromo_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView dt = (DataRowView)e.Row.DataItem;
            if (dt[4].ToString() != "")
            {
                if (DateTime.UtcNow.AddHours(8) >= Convert.ToDateTime(dt[4].ToString()) && DateTime.UtcNow.AddHours(8) <= Convert.ToDateTime(dt[5].ToString()))
                {
                    e.Row.BackColor = Color.LightGreen;
                }
                else
                {
                    e.Row.BackColor = Color.Transparent;
                }
            }


        }

        if (e.Row.RowType == DataControlRowType.Footer)
        {
            using (SqlConnection con = new SqlConnection(strConStringrp))
            {
                DropDownList ddl = null;
                ddl = e.Row.FindControl("ddlpromovipcampaignNew") as DropDownList;
                if (ddl != null)
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("select * from mf_promocampaign where deleteind <> 'X'", con);
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    DataTable dcmdt = new DataTable();
                    adp.Fill(dcmdt);
                    if (dcmdt.Rows.Count > 0)
                    {
                        ddl.DataSource = dcmdt;
                        ddl.DataTextField = "campaigntitle";
                        ddl.DataValueField = "ids";
                        ddl.DataBind();
                        ddl.Items.Insert(0, new ListItem("-Select Promotion-", ""));
                    }

                    con.Close();
                }
            }
        }
    }

    protected void gridview_vvippromo_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView dt = (DataRowView)e.Row.DataItem;
            if (dt[4].ToString() != "")
            {
                if (DateTime.UtcNow.AddHours(8) >= Convert.ToDateTime(dt[4].ToString()) && DateTime.UtcNow.AddHours(8) <= Convert.ToDateTime(dt[5].ToString()))
                {
                    e.Row.BackColor = Color.LightGreen;
                }
                else
                {
                    e.Row.BackColor = Color.Transparent;
                }
            }


        }

        if (e.Row.RowType == DataControlRowType.Footer)
        {
            using (SqlConnection con = new SqlConnection(strConStringrp))
            {
                DropDownList ddl = null;
                ddl = e.Row.FindControl("ddlpromovvipcampaignNew") as DropDownList;
                if (ddl != null)
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("select * from mf_promocampaign where deleteind <> 'X'", con);
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    DataTable dcmdt = new DataTable();
                    adp.Fill(dcmdt);
                    if (dcmdt.Rows.Count > 0)
                    {
                        ddl.DataSource = dcmdt;
                        ddl.DataTextField = "campaigntitle";
                        ddl.DataValueField = "ids";
                        ddl.DataBind();
                        ddl.Items.Insert(0, new ListItem("-Select Promotion-", ""));
                    }

                    con.Close();
                }
            }
        }
    }

    protected void gridview_stockistpromo_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView dt = (DataRowView)e.Row.DataItem;
            if (dt[4].ToString() != "")
            {

                if (DateTime.UtcNow.AddHours(8) >= Convert.ToDateTime(dt[4].ToString()) && DateTime.UtcNow.AddHours(8) <= Convert.ToDateTime(dt[5].ToString()))
                {
                    e.Row.BackColor = Color.LightGreen;
                }
                else
                {
                    e.Row.BackColor = Color.Transparent;
                }
            }


        }

        if (e.Row.RowType == DataControlRowType.Footer)
        {
            using (SqlConnection con = new SqlConnection(strConStringrp))
            {
                DropDownList ddl = null;
                ddl = e.Row.FindControl("ddlpromoskcampaignNew") as DropDownList;
                if (ddl != null)
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("select * from mf_promocampaign where deleteind <> 'X'", con);
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    DataTable dcmdt = new DataTable();
                    adp.Fill(dcmdt);
                    if (dcmdt.Rows.Count > 0)
                    {
                        ddl.DataSource = dcmdt;
                        ddl.DataTextField = "campaigntitle";
                        ddl.DataValueField = "ids";
                        ddl.DataBind();
                        ddl.Items.Insert(0, new ListItem("-Select Promotion-", ""));
                    }

                    con.Close();
                }
            }
        }
    }

    protected void gridview_masterstockist_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView dt = (DataRowView)e.Row.DataItem;
            if (dt[4].ToString() != "")
            {
                if (DateTime.UtcNow.AddHours(8) >= Convert.ToDateTime(dt[4].ToString()) && DateTime.UtcNow.AddHours(8) <= Convert.ToDateTime(dt[5].ToString()))
                {
                    e.Row.BackColor = Color.LightGreen;
                }
                else
                {
                    e.Row.BackColor = Color.Transparent;
                }
            }


        }

        if (e.Row.RowType == DataControlRowType.Footer)
        {
            using (SqlConnection con = new SqlConnection(strConStringrp))
            {
                DropDownList ddl = null;
                ddl = e.Row.FindControl("ddlpromomskcampaignNew") as DropDownList;
                if (ddl != null)
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("select * from mf_promocampaign where deleteind <> 'X'", con);
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    DataTable dcmdt = new DataTable();
                    adp.Fill(dcmdt);
                    if (dcmdt.Rows.Count > 0)
                    {
                        ddl.DataSource = dcmdt;
                        ddl.DataTextField = "campaigntitle";
                        ddl.DataValueField = "ids";
                        ddl.DataBind();
                        ddl.Items.Insert(0, new ListItem("-Select Promotion-", ""));
                    }

                    con.Close();
                }
            }
        }
    }

    protected void ddlpromocampaign_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlmempromoNew = (DropDownList)sender;
        GridViewRow row = (GridViewRow)ddlmempromoNew.NamingContainer;
        Label lblfooterstartdate = (Label)row.FindControl("lblfooterstartdate");
        Label lblfooterenddate = (Label)row.FindControl("lblfooterenddate");

        using (SqlConnection con = new SqlConnection(strConStringrp))
        {
            SqlCommand cmd = new SqlCommand("select * from mf_promocampaign where ids = '" + ddlmempromoNew.SelectedValue + "' and deleteind <> 'X'", con);
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataTable dcmdt = new DataTable();
            adp.Fill(dcmdt);
            if (dcmdt.Rows.Count > 0)
            {
                lblfooterstartdate.Text = dcmdt.Rows[0]["start_date"].ToString();
                lblfooterenddate.Text = dcmdt.Rows[0]["end_date"].ToString();

            }

        }
    }

    protected void ddlpromovipcampaign_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlvippromoNew = (DropDownList)sender;
        GridViewRow row = (GridViewRow)ddlvippromoNew.NamingContainer;
        Label lblfooterstartdate = (Label)row.FindControl("lblfooterstartdate");
        Label lblfooterenddate = (Label)row.FindControl("lblfooterenddate");

        using (SqlConnection con = new SqlConnection(strConStringrp))
        {
            SqlCommand cmd = new SqlCommand("select * from mf_promocampaign where ids = '" + ddlvippromoNew.SelectedValue + "' and deleteind <> 'X'", con);
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataTable dcmdt = new DataTable();
            adp.Fill(dcmdt);
            if (dcmdt.Rows.Count > 0)
            {
                lblfooterstartdate.Text = dcmdt.Rows[0]["start_date"].ToString();
                lblfooterenddate.Text = dcmdt.Rows[0]["end_date"].ToString();

            }

        }
    }

    protected void ddlpromovvipcampaign_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlvvippromoNew = (DropDownList)sender;
        GridViewRow row = (GridViewRow)ddlvvippromoNew.NamingContainer;
        Label lblfooterstartdate = (Label)row.FindControl("lblfooterstartdate");
        Label lblfooterenddate = (Label)row.FindControl("lblfooterenddate");

        using (SqlConnection con = new SqlConnection(strConStringrp))
        {
            SqlCommand cmd = new SqlCommand("select * from mf_promocampaign where ids = '" + ddlvvippromoNew.SelectedValue + "' and deleteind <> 'X'", con);
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataTable dcmdt = new DataTable();
            adp.Fill(dcmdt);
            if (dcmdt.Rows.Count > 0)
            {
                lblfooterstartdate.Text = dcmdt.Rows[0]["start_date"].ToString();
                lblfooterenddate.Text = dcmdt.Rows[0]["end_date"].ToString();

            }

        }
    }

    protected void ddlpromoskcampaign_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlskpromoNew = (DropDownList)sender;
        GridViewRow row = (GridViewRow)ddlskpromoNew.NamingContainer;
        Label lblfooterstartdate = (Label)row.FindControl("lblfooterstartdate");
        Label lblfooterenddate = (Label)row.FindControl("lblfooterenddate");

        using (SqlConnection con = new SqlConnection(strConStringrp))
        {
            SqlCommand cmd = new SqlCommand("select * from mf_promocampaign where ids = '" + ddlskpromoNew.SelectedValue + "' and deleteind <> 'X'", con);
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataTable dcmdt = new DataTable();
            adp.Fill(dcmdt);
            if (dcmdt.Rows.Count > 0)
            {
                lblfooterstartdate.Text = dcmdt.Rows[0]["start_date"].ToString();
                lblfooterenddate.Text = dcmdt.Rows[0]["end_date"].ToString();

            }

        }
    }

    protected void ddlpromomskcampaign_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlmskpromoNew = (DropDownList)sender;
        GridViewRow row = (GridViewRow)ddlmskpromoNew.NamingContainer;
        Label lblfooterstartdate = (Label)row.FindControl("lblfooterstartdate");
        Label lblfooterenddate = (Label)row.FindControl("lblfooterenddate");

        using (SqlConnection con = new SqlConnection(strConStringrp))
        {
            SqlCommand cmd = new SqlCommand("select * from mf_promocampaign where ids = '" + ddlmskpromoNew.SelectedValue + "' and deleteind <> 'X'", con);
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataTable dcmdt = new DataTable();
            adp.Fill(dcmdt);
            if (dcmdt.Rows.Count > 0)
            {
                lblfooterstartdate.Text = dcmdt.Rows[0]["start_date"].ToString();
                lblfooterenddate.Text = dcmdt.Rows[0]["end_date"].ToString();

            }

        }
    }

    protected void ddlcampaign_SelectedIndexChanged(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(strConStringrp);

        using (SqlCommand loadcampaigncmd = new SqlCommand("select a.item_code,a.LongDesc,a.Barcode,a.FilePath,b.SPP,b.ReferrerProfitBonusPercent,b.TeamKPIBonus,b.TeamKPIBonusMS,b.MEB,b.MBD,b.start_date,b.end_date " +
                ",ISNULL(p.[BuyPrice], 0) as [MemberPrice] " +
                ", ISNULL(q.[BuyPrice], 0) as [VIPPrice] " +
                ", ISNULL(r.[BuyPrice], 0) as [StockistPrice] " +
                ", ISNULL(s.[BuyPrice], 0) as [VVIPPrice] " +
                ", ISNULL(t.[BuyPrice], 0) as [MStockistPrice] " +
                ", ISNULL(p.[CashBackAmount], 0) as [MemberCashback] " +
                ", ISNULL(q.[CashBackAmount], 0) as [VIPCashback] " +
                ", ISNULL(r.[CashBackAmount], 0) as [StockistCashback] " +
                ", ISNULL(s.[CashBackAmount], 0) as [VVIPCashback] " +
                ", ISNULL(t.[CashBackAmount], 0) as [MStockistCashback] " +
                ", ISNULL(aa.[Promo_price], 0) as [PMemberPrice] " +
                ", ISNULL(ab.[Promo_price], 0) as [PVIPPrice] " +
                ", ISNULL(ac.[Promo_price], 0) as [PStockistPrice] " +
                ", ISNULL(ad.[Promo_price], 0) as [PVVIPPrice] " +
                ", ISNULL(ae.[Promo_price], 0) as [PMStockistPrice] " +
                ", ISNULL(aa.[CashBackAmt], 0) as [PMemberCashback] " +
                ", ISNULL(ab.[CashBackAmt], 0) as [PVIPCashback] " +
                ", ISNULL(ac.[CashBackAmt], 0) as [PStockistCashback] " +
                ", ISNULL(ad.[CashBackAmt], 0) as [PVVIPCashback] " +
                ", ISNULL(ae.[CashBackAmt], 0) as [PMStockistCashback] " +
                "from MF_Item AS a " +
                "LEFT JOIN [MF_AgentPromo] AS b ON b.item_code=a.item_code and b.[AgentLevelCode]='D02' and b.[DeleteInd] <> 'X' and b.promoid='" + ddlcampaign.SelectedValue + "'" +
                "LEFT JOIN [MF_AgentCommi] AS p ON p.Item_Code = a.Item_Code and p.[AgentLevelCode]='D02' and p.[DeleteInd] <> 'X' " +
                "LEFT JOIN [MF_AgentCommi] AS q ON q.Item_Code = a.Item_Code and q.[AgentLevelCode]='D01' and q.[DeleteInd] <> 'X' " +
                "LEFT JOIN [MF_AgentCommi] AS r ON r.Item_Code = a.Item_Code and r.[AgentLevelCode]='D03' and r.[DeleteInd] <> 'X' " +
                "LEFT JOIN [MF_AgentCommi] AS s ON s.Item_Code = a.Item_Code and s.[AgentLevelCode]='D04' and s.[DeleteInd] <> 'X' " +
                "LEFT JOIN [MF_AgentCommi] AS t ON t.Item_Code = a.Item_Code and t.[AgentLevelCode]='D05' and t.[DeleteInd] <> 'X'  " +
                "LEFT JOIN [MF_AgentPromo] AS aa ON aa.Item_Code = a.Item_Code and aa.[AgentLevelCode]='D02' and aa.[DeleteInd] <> 'X' and aa.promoid='" + ddlcampaign.SelectedValue + "' " +
                "LEFT JOIN [MF_AgentPromo] AS ab ON ab.Item_Code = a.Item_Code and ab.[AgentLevelCode]='D01' and ab.[DeleteInd] <> 'X' and ab.promoid='" + ddlcampaign.SelectedValue + "' " +
                "LEFT JOIN [MF_AgentPromo] AS ac ON ac.Item_Code = a.Item_Code and ac.[AgentLevelCode]='D03' and ac.[DeleteInd] <> 'X' and ac.promoid='" + ddlcampaign.SelectedValue + "' " +
                "LEFT JOIN [MF_AgentPromo] AS ad ON ad.Item_Code = a.Item_Code and ad.[AgentLevelCode]='D04' and ad.[DeleteInd] <> 'X' and ad.promoid='" + ddlcampaign.SelectedValue + "' " +
                "LEFT JOIN [MF_AgentPromo] AS ae ON ae.Item_Code = a.Item_Code and ae.[AgentLevelCode]='D05' and ae.[DeleteInd] <> 'X' and ae.promoid='" + ddlcampaign.SelectedValue + "'  " +
                "where a.deleteind <> 'X' and a.item_code='" + lblpitem_code.Text + "' " +
                " group by a.item_code,a.LongDesc,a.Barcode,a.FilePath,b.start_date,b.end_date, " +
                "b.SPP,b.ReferrerProfitBonusPercent,b.TeamKPIBonus,b.TeamKPIBonusMS,b.MEB,b.MBD,p.CashBackAmount,q.CashBackAmount,r.CashBackAmount," +
                "s.CashBackAmount,t.CashBackAmount,p.BuyPrice,q.BuyPrice,r.BuyPrice,s.BuyPrice,t.BuyPrice ," +
                "aa.CashBackAmt,ab.CashBackAmt,ac.CashBackAmt,ad.CashBackAmt,ae.CashBackAmt,aa.Promo_price,ab.Promo_price,ac.Promo_price,ad.Promo_price,ae.Promo_price", con))
        {
            SqlDataAdapter loadcampaignadp = new SqlDataAdapter(loadcampaigncmd);
            DataTable loadcampaigndt = new DataTable();
            loadcampaignadp.Fill(loadcampaigndt);

            if (loadcampaigndt.Rows.Count > 0)
            {
                txtstartdate.Text = loadcampaigndt.Rows[0]["start_date"].ToString();
                txtenddate.Text = loadcampaigndt.Rows[0]["end_date"].ToString();
                lblpitem_code.Text = loadcampaigndt.Rows[0]["item_code"].ToString();
                lblpitemname.Text = loadcampaigndt.Rows[0]["longdesc"].ToString();
                lblpbarcode.Text = loadcampaigndt.Rows[0]["barcode"].ToString();
                lblpMpromoPrice.Text = Convert.ToDecimal(loadcampaigndt.Rows[0]["PMemberPrice"]).ToString("###0.00");
                lblpMprice.Text = Convert.ToDecimal(loadcampaigndt.Rows[0]["MemberPrice"]).ToString("###0.00");
                txtpMpromocashback.Text = Convert.ToDecimal(loadcampaigndt.Rows[0]["PMemberCashback"]).ToString("###0.00");
                lblpVpromoPrice.Text = Convert.ToDecimal(loadcampaigndt.Rows[0]["PVIPPrice"]).ToString("###0.00");
                lblpVPrice.Text = Convert.ToDecimal(loadcampaigndt.Rows[0]["VIPPrice"]).ToString("###0.00");
                txtpVpromocashback.Text = Convert.ToDecimal(loadcampaigndt.Rows[0]["PVIPCashback"]).ToString("###0.00");
                lblpVVIPpromoPrice.Text = Convert.ToDecimal(loadcampaigndt.Rows[0]["PVVIPPrice"]).ToString("###0.00");
                lblpVVIPPrice.Text = Convert.ToDecimal(loadcampaigndt.Rows[0]["VVIPPrice"]).ToString("###0.00");
                txtpVVIPpromocashback.Text = Convert.ToDecimal(loadcampaigndt.Rows[0]["PVVIPCashback"]).ToString("###0.00");
                lblpSpromoPrice.Text = Convert.ToDecimal(loadcampaigndt.Rows[0]["PStockistPrice"]).ToString("###0.00");
                lblpSPrice.Text = Convert.ToDecimal(loadcampaigndt.Rows[0]["StockistPrice"]).ToString("###0.00");
                txtpSpromocashback.Text = Convert.ToDecimal(loadcampaigndt.Rows[0]["PStockistCashback"]).ToString("###0.00");
                lblpMSpromoPrice.Text = Convert.ToDecimal(loadcampaigndt.Rows[0]["PMStockistPrice"]).ToString("###0.00");
                lblpMSPrice.Text = Convert.ToDecimal(loadcampaigndt.Rows[0]["MStockistPrice"]).ToString("###0.00");
                txtpMSpromocashback.Text = Convert.ToDecimal(loadcampaigndt.Rows[0]["PMStockistCashback"]).ToString("###0.00");
                txtRefProfitBonusPercent.Text = Convert.ToDecimal(loadcampaigndt.Rows[0]["ReferrerProfitBonusPercent"]).ToString("0.00");
                txtSPP.Text = loadcampaigndt.Rows[0]["SPP"].ToString();
                txtMBD.Text = loadcampaigndt.Rows[0]["MBD"].ToString();
                txtMEB.Text = loadcampaigndt.Rows[0]["MEB"].ToString();
                txtTeamKPI2.Text = loadcampaigndt.Rows[0]["TeamKPIBonusMS"].ToString();
                txtTeamKPI.Text = loadcampaigndt.Rows[0]["TeamKPIBonus"].ToString();



            }
        }
    }

    protected void btnPromotion_Click(object sender, EventArgs e)
    {
        if (ddlcampaign.SelectedValue != "")
        {
            Response.Redirect("PromoItemSetup.aspx?username=" + Request.QueryString["username"] + "&groupid=" + ddlcampaign.SelectedValue + "&merchant=" + Request.QueryString["merchant"]);
        }
        else
        {
            Response.Redirect("PromoCampaign.aspx?merchant=" + Request.QueryString["merchant"]);
        }
    }

    protected void btnupdatemodifier_Click(object sender, EventArgs e)
    {
        using (SqlConnection con = new SqlConnection(strConStringrp))
        {
            con.Open();

            // First, soft delete old assignments
            SqlCommand delcmd = new SqlCommand("UPDATE MF_Item_Modifier_Assign SET deleteind='X' WHERE item_code=@itemcode AND deleteind <> 'X'", con);
            delcmd.Parameters.AddWithValue("@itemcode", hditem_code.Value);
            delcmd.ExecuteNonQuery();

            // Insert checked modifiers with sequence
            foreach (GridViewRow row in grd_viewmodifer.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkmodifier = (CheckBox)row.FindControl("chkmodifier");
                    HiddenField hdmodifierID = (HiddenField)row.FindControl("hdmodifierID");
                    HiddenField hdSequence = (HiddenField)row.FindControl("hdSequence");

                    if (chkmodifier.Checked && !string.IsNullOrEmpty(hdSequence.Value) && hdSequence.Value != "0")
                    {
                        SqlCommand insertmodifiercmd = new SqlCommand(
                            "INSERT INTO MF_Item_Modifier_Assign (modifier_ID, Item_Code, assign_date, deleteind, sequence) " +
                            "VALUES (@modifierID, @itemcode, DATEADD(HOUR, 8, GETUTCDATE()), '', @sequence)", con);

                        insertmodifiercmd.Parameters.AddWithValue("@modifierID", hdmodifierID.Value);
                        insertmodifiercmd.Parameters.AddWithValue("@itemcode", hditem_code.Value);
                        insertmodifiercmd.Parameters.AddWithValue("@sequence", hdSequence.Value);

                        insertmodifiercmd.ExecuteNonQuery();
                    }
                }
            }

            loadmodifier();
            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "sweetalert_warning('Item Modifier Updated', 'success');", true);
        }
    }


    protected void loadmodifier()
    {
        using (SqlConnection con = new SqlConnection(strConStringrp))
        {
            SqlCommand itemcmd = new SqlCommand("select longdesc,barcode,FilePath from mf_item where item_code='" + hditem_code.Value + "' and deleteind <> 'X'", con);

            SqlDataAdapter itemadp = new SqlDataAdapter(itemcmd);
            DataTable itemdt = new DataTable();
            itemadp.Fill(itemdt);

            if (itemdt.Rows.Count > 0)
            {
                if (itemdt.Rows[0]["FilePath"] != DBNull.Value)
                {
                    Image1.ImageUrl = itemdt.Rows[0]["FilePath"].ToString();
                }
                else
                {

                }
                {
                    Image1.ImageUrl = "Images/NoPic.png";
                }

                lbldesc.Text = itemdt.Rows[0]["longdesc"].ToString();
                lblbarcode.Text = itemdt.Rows[0]["barcode"].ToString();

            }

            using (SqlCommand cmd = new SqlCommand("select modifier_id,modifier_grp_name,case when right(rtrim((select Option_Name + ',' from MF_Modifier_Option where MF_Modifier_Option.Modifier_ID=MF_Modifier_Group.Modifier_ID " +
                "and MF_Modifier_Option.Deleteind <> 'X' FOR XML PATH(''))),1) = ',' then substring(rtrim((select Option_Name +',' from MF_Modifier_Option where MF_Modifier_Option.Modifier_ID=MF_Modifier_Group.Modifier_ID " +
                "	and MF_Modifier_Option.Deleteind <> 'X' FOR XML PATH(''))),1,len(rtrim((select Option_Name +',' from MF_Modifier_Option where MF_Modifier_Option.Modifier_ID=MF_Modifier_Group.Modifier_ID " +
                "and MF_Modifier_Option.Deleteind <> 'X' FOR XML PATH(''))))-1) else (select Option_Name + '(RM'+ Cast(Option_Price as varchar(255)) +')' + ',' from MF_Modifier_Option where MF_Modifier_Option.Modifier_ID=MF_Modifier_Group.Modifier_ID " +
                "	and MF_Modifier_Option.Deleteind <> 'X' FOR XML PATH('')) END AS modifieroption from MF_Modifier_Group where deleteind <> 'X' and merchant_code='" + Request.QueryString["merchant"] + "'", con))
            {
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter idr = new SqlDataAdapter(cmd);
                DataTable v = new DataTable();
                idr.Fill(v);

                if (v.Rows.Count > 0)
                {
                    grd_viewmodifer.DataSource = v;
                    grd_viewmodifer.DataBind();
                }


            }
        }
    }

    protected void loadprinter()
    {
        using (SqlConnection con = new SqlConnection(strConStringrp))
        {
            string user = "";
            if (Request.QueryString["user"] != null)
            {
                user = Request.QueryString["user"].ToString();
            }
            else
            {
                user = Request.QueryString["merchant"].ToString();

            }
            ;

            SqlCommand itemcmd = new SqlCommand("select longdesc,barcode,FilePath from mf_item where item_code='" + hditem_code.Value + "' and deleteind <> 'X'", con);

            SqlDataAdapter itemadp = new SqlDataAdapter(itemcmd);
            DataTable itemdt = new DataTable();
            itemadp.Fill(itemdt);

            if (itemdt.Rows.Count > 0)
            {
                if (itemdt.Rows[0]["FilePath"] != DBNull.Value)
                {
                    Image2.ImageUrl = itemdt.Rows[0]["FilePath"].ToString();
                }
                else
                {

                }
                {
                    Image2.ImageUrl = "Images/NoPic.png";
                }

                lblitemdesc.Text = itemdt.Rows[0]["longdesc"].ToString();
                lblitembarcode.Text = itemdt.Rows[0]["barcode"].ToString();

            }

            using (SqlCommand cmd = new SqlCommand("select Ids,Print_Name,IP_Address from PosSys_Printer where deleteind <> 'X' and merchant_code='" + Request.QueryString["merchant"] + "' and user_code='" + user + "'", con))
            {
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter idr = new SqlDataAdapter(cmd);
                DataTable v = new DataTable();
                idr.Fill(v);

                if (v.Rows.Count > 0)
                {
                    grd_viewprinter.DataSource = v;
                    grd_viewprinter.DataBind();
                }
                else
                {
                    grd_viewprinter.DataSource = v;
                    grd_viewprinter.DataBind();

                }


            }
        }
    }

    protected void ShowMessage_warning(string Message, MessageType type)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "sweetalert_warning('" + Message + "','" + type + "');", true);
    }

    protected void ShowMessage_error(string Message, MessageType type)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "sweetalert_error('" + Message + "','" + type + "');", true);
    }


    protected void grd_viewmodifer_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            SqlConnection con = new SqlConnection(strConStringrp);
            con.Open();

            HiddenField hdmodifierID = (HiddenField)e.Row.FindControl("hdmodifierID");
            CheckBox chkmodifier = (CheckBox)e.Row.FindControl("chkmodifier");

            SqlCommand loadmodifiercmd = new SqlCommand("select * from mf_item_modifier_assign where item_code='" + hditem_code.Value + "' and modifier_id = '" + hdmodifierID.Value + "' and deleteind <> 'X'", con);
            SqlDataAdapter loadmodifieradp = new SqlDataAdapter(loadmodifiercmd);
            DataTable loadmodifierdt = new DataTable();
            loadmodifieradp.Fill(loadmodifierdt);

            if (loadmodifierdt.Rows.Count > 0)
            {

                if (loadmodifierdt.Rows[0]["modifier_id"].ToString() == hdmodifierID.Value)
                {
                    chkmodifier.Checked = true;
                }
                else
                {
                    chkmodifier.Checked = false;
                }
            }

        }
    }

    protected void grd_viewprinter_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            SqlConnection con = new SqlConnection(strConStringrp);
            con.Open();

            HiddenField hdprinterID = (HiddenField)e.Row.FindControl("hdprinterID");
            CheckBox chkprinter = (CheckBox)e.Row.FindControl("chkprinter");

            SqlCommand loadprintercmd = new SqlCommand("select * from PosSys_PrinterItem where item_code='" + hditem_code.Value + "' and printer_id = '" + hdprinterID.Value + "'", con);
            SqlDataAdapter loadprinteradp = new SqlDataAdapter(loadprintercmd);
            DataTable loadprinterdt = new DataTable();
            loadprinteradp.Fill(loadprinterdt);

            if (loadprinterdt.Rows.Count > 0)
            {

                if (loadprinterdt.Rows[0]["printer_id"].ToString() == hdprinterID.Value)
                {
                    chkprinter.Checked = true;
                }
                else
                {
                    chkprinter.Checked = false;
                }
            }

        }
    }

    protected void btnupdateprinter_Click(object sender, EventArgs e)
    {
        string user = "";
        if (Request.QueryString["user"] != null)
        {
            user = Request.QueryString["user"].ToString();
        }
        else
        {
            user = Request.QueryString["merchant"].ToString();

        }
        ;

        SqlConnection con = new SqlConnection(strConStringrp);
        con.Open();

        SqlCommand delcmd = new SqlCommand("delete from PosSys_PrinterItem where item_code='" + hditem_code.Value + "'", con);
        delcmd.ExecuteNonQuery();

        foreach (GridViewRow row in grd_viewprinter.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkprinter = (CheckBox)row.FindControl("chkprinter");
                if (chkprinter.Checked)
                {
                    HiddenField hdprinterID = (HiddenField)row.FindControl("hdprinterID");

                    SqlCommand insertmodifiercmd = new SqlCommand("insert into PosSys_PrinterItem(Item_Code,Printer_ID,Merchant_Code,User_Code,Created_DT) values(@itemcode,@printerid,@merchantcode,@usercode,dateadd(hour,(8),CONVERT([varchar](20),getutcdate(),(120))))", con);
                    insertmodifiercmd.Parameters.AddWithValue("@printerid", hdprinterID.Value);
                    insertmodifiercmd.Parameters.AddWithValue("@itemcode", hditem_code.Value);
                    insertmodifiercmd.Parameters.AddWithValue("@merchantcode", Request.QueryString["merchant"].ToString());
                    insertmodifiercmd.Parameters.AddWithValue("@usercode", user);
                    try
                    {
                        insertmodifiercmd.ExecuteNonQuery();

                        //ShowMessage_warning("Item Modifier Updated", MessageType.success);
                        loadprinter();
                        ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "sweetalert_warning('Printer Assign Updated', 'success');", true);
                        //updatepanel.Update();
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

        }
    }

    private void ExportGridToExcel()
    {
        using (SqlConnection con = new SqlConnection(strConStringrp))
        {
            using (SqlCommand cmd = new SqlCommand("Content_Listing", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@PageIndex", 1);
                cmd.Parameters.AddWithValue("@PageSize", 1000);

                cmd.Parameters.AddWithValue("@FilterText", string.IsNullOrEmpty(txt_Search.Text.Trim()) ? "%%" : "%" + txt_Search.Text.Trim().Replace("'", "`") + "%");
                cmd.Parameters.AddWithValue("@FilterStatus", string.IsNullOrEmpty(ddlStatus.SelectedValue) ? "%%" : "%" + ddlStatus.SelectedValue + "%");
                cmd.Parameters.AddWithValue("@FilterPublish", string.IsNullOrEmpty(ddlPublish.SelectedValue) ? "%%" : "%" + ddlPublish.SelectedValue + "%");
                cmd.Parameters.AddWithValue("@FilterSoldStatus", string.IsNullOrEmpty(ddlSoldStatus.SelectedValue) ? "%%" : "%" + ddlSoldStatus.SelectedValue + "%");
                cmd.Parameters.AddWithValue("@FilterDepartment", string.IsNullOrEmpty(ddlDepartment.SelectedValue) ? "%%" : "%" + ddlDepartment.SelectedValue + "%");
                cmd.Parameters.AddWithValue("@FilterCategory", string.IsNullOrEmpty(ddlCategory.SelectedValue) ? "%%" : "%" + ddlCategory.SelectedValue + "%");
                cmd.Parameters.AddWithValue("@FilterMerchant", Request.QueryString["merchant"].ToString());
                cmd.Parameters.Add("@RecordCount", SqlDbType.Int).Direction = ParameterDirection.Output;

                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adp.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(dt, "ContentListing");

                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        string FileName = "ContentListing_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".xlsx";
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment;filename=" + FileName);

                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                    }
                }
            }
        }
    }

    protected void btnExportExcel_Click(object sender, EventArgs e)
    {
        ExportGridToExcel();
    }
}
