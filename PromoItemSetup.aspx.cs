using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PromoItemSetup : System.Web.UI.Page
{

    public int cLine = 0;
    protected static String DBCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            MF_Con.Text = DBCon;
            SqlConnection con = new SqlConnection(MF_Con.Text);

            txtMredemptpoint.Attributes.Add("onfocus", "this.select();");
            txtMrewardpoint.Attributes.Add("onfocus", "this.select();");
            txtVIPredemptpoint.Attributes.Add("onfocus", "this.select();");
            txtVIPrewardpoint.Attributes.Add("onfocus", "this.select();");
            txtVVIPredemptpoint.Attributes.Add("onfocus", "this.select();");
            txtVVIPrewardpoint.Attributes.Add("onfocus", "this.select();");
            txtSredemptpoint.Attributes.Add("onfocus", "this.select();");
            txtSrewardpoint.Attributes.Add("onfocus", "this.select();");
            txtMSredemptpoint.Attributes.Add("onfocus", "this.select();");
            txtMSrewardpoint.Attributes.Add("onfocus", "this.select();");




            if (Request.QueryString["groupid"].ToString() != "")
            {
                showgroupitemlist();
                showitemlist();


                using (SqlCommand myTxcmd = new SqlCommand("select * from MF_PromoCampaign where ids=@groupid and merchantid=@MerchantID", con))
                {
                    myTxcmd.Parameters.AddWithValue("@groupid", Request.QueryString["groupid"].ToString());
                    myTxcmd.Parameters.AddWithValue("@MerchantID", Request.QueryString["merchant"].ToString());

                    SqlDataAdapter adp = new SqlDataAdapter(myTxcmd);
                    DataTable dt = new DataTable();
                    adp.Fill(dt);

                    con.Open();

                    try
                    {
                        if (dt.Rows.Count > 0)
                        {
                            txtcampaignname.Text = dt.Rows[0]["CampaignTitle"].ToString();
                            txtstartdate.Text = Convert.ToDateTime(dt.Rows[0]["start_date"]).ToString("yyyy-MM-ddTHH:mm");
                            txtenddate.Text = Convert.ToDateTime(dt.Rows[0]["end_date"]).ToString("yyyy-MM-ddTHH:mm");
                        }


                    }
                    catch (Exception ex)
                    {
                        throw (ex);
                    }
                    finally
                    {
                        myTxcmd.Dispose();
                        con.Close();
                    }

                }
            }
            else
            {
                showitemlist();
            }
        }
    }

    public void showgroupitemlist()
    {
        using (SqlConnection con = new SqlConnection(DBCon))
        {
            using (SqlCommand cmd = new SqlCommand("select a.item_code,b.Longdesc,a.PromoID,b.FilePath from MF_AgentPromo as a left join mf_item as b on b.item_code=a.item_code where a.PromoId='" + Request.QueryString["groupid"].ToString() + "' and a.deleteind <> 'X' and b.deleteind <> 'X' group by a.item_code,b.Longdesc,a.PromoID,b.FilePath ", con))
            {
                con.Open();
                SqlDataReader idr = cmd.ExecuteReader();

                if (idr.HasRows == false)
                {
                    DataTable dt = new DataTable();

                    dt.Load(idr);
                    bangetotaladdlist.InnerText = dt.Rows.Count.ToString();
                    promotionitem_grdview.DataSource = dt;
                    promotionitem_grdview.DataBind();

                }
                else
                {
                    
                    DataTable dt = new DataTable();

                    dt.Load(idr);

                    bangetotaladdlist.InnerText = dt.Rows.Count.ToString();

                    promotionitem_grdview.DataSource = dt;
                    promotionitem_grdview.DataBind();
                    //promotionitem_grdview.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
                showitemlist();
                updateee.Update();

                idr.Close();
                con.Close();
            }
        }
    }


    protected void btnsearch_Click(object sender, EventArgs e)
    {
        showitemlist();
    }

    protected void item_grdview_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        item_grdview.PageIndex = e.NewPageIndex;
        this.showitemlist();
    }

    public void showitemlist()
    {
        using (SqlConnection con = new SqlConnection(DBCon))
        {
            using (SqlCommand cmd = new SqlCommand("select b.item_code, b.longdesc , b.category_code, b.filepath from mf_item as b where " +
            "NOT EXISTS(select item_code from MF_AgentPromo as a where a.Item_Code = b.Item_Code and " +
            "promoid = '" + Request.QueryString["groupid"].ToString() + "' and Deleteind <> 'x') and ( b.item_code like '%" + txt_search.Text + "%' or b.longdesc like '%" + txt_search.Text + "%') and b.supplier_code='" + Request.QueryString["merchant"].ToString() + "' and b.deleteind <> 'X' order by created_dt desc", con))
            {
                con.Open();

                SqlDataReader idr = cmd.ExecuteReader();

                if (idr.HasRows == false)
                {
                    DataTable v = new DataTable();

                    v.Load(idr);

                    item_grdview.DataSource = v;
                    item_grdview.DataBind();


                }
                else
                {
                    DataTable v = new DataTable();

                    v.Load(idr);
                    item_grdview.DataSource = v;
                    item_grdview.DataBind();
                    item_grdview.HeaderRow.TableSection = TableRowSection.TableHeader;
                }

                idr.Close();
                con.Close();
            }
        }
    }


    protected void item_grdview_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "addpromo")
        {
            btnupdatepromotion.Visible=false;
            btnaddtopromotion.Visible=true;

            using (SqlConnection con = new SqlConnection(MF_Con.Text))
            {
                using (SqlCommand cmd = new SqlCommand("select a.item_code,a.LongDesc,a.Barcode,a.FilePath,a.SPP,a.ReferrerProfitBonusPercent,a.TeamKPIBonus,a.TeamKPIBonusMS,a.MEB,a.MBD " +
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
                "from mf_item AS a " +
                "LEFT JOIN [MF_AgentCommi] AS p ON p.Item_Code = a.Item_Code and p.[AgentLevelCode]='D02' and p.[DeleteInd] <> 'X' " +
	            "LEFT JOIN [MF_AgentCommi] AS q ON q.Item_Code = a.Item_Code and q.[AgentLevelCode]='D01' and q.[DeleteInd] <> 'X' " +
                "LEFT JOIN [MF_AgentCommi] AS r ON r.Item_Code = a.Item_Code and r.[AgentLevelCode]='D03' and r.[DeleteInd] <> 'X' " +
                "LEFT JOIN [MF_AgentCommi] AS s ON s.Item_Code = a.Item_Code and s.[AgentLevelCode]='D04' and s.[DeleteInd] <> 'X' " +
                "LEFT JOIN [MF_AgentCommi] AS t ON t.Item_Code = a.Item_Code and t.[AgentLevelCode]='D05' and t.[DeleteInd] <> 'X'  " +
                "where a.deleteind <> 'X' and a.item_code='" + e.CommandArgument.ToString() + "'", con))
                {
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adp.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        lblitem_code.Text = dt.Rows[0]["item_code"].ToString();
                        lblitemname.Text = dt.Rows[0]["longdesc"].ToString();
                        lblbarcode.Text = dt.Rows[0]["barcode"].ToString();
                        lblMpromoPrice.Text = Convert.ToDecimal(dt.Rows[0]["MemberPrice"]).ToString("###0.00");
                        lblMprice.Text = Convert.ToDecimal(dt.Rows[0]["MemberPrice"]).ToString("###0.00");
                        txtMpromocashback.Text = Convert.ToDecimal(dt.Rows[0]["MemberCashback"]).ToString("###0.00");
                        lblVpromoPrice.Text = Convert.ToDecimal(dt.Rows[0]["VIPPrice"]).ToString("###0.00");
                        lblVPrice.Text = Convert.ToDecimal(dt.Rows[0]["VIPPrice"]).ToString("###0.00");
                        txtVpromocashback.Text = Convert.ToDecimal(dt.Rows[0]["VIPCashback"]).ToString("###0.00");
                        lblVVIPpromoPrice.Text = Convert.ToDecimal(dt.Rows[0]["VVIPPrice"]).ToString("###0.00");
                        lblVVIPPrice.Text = Convert.ToDecimal(dt.Rows[0]["VVIPPrice"]).ToString("###0.00");
                        txtVVIPpromocashback.Text = Convert.ToDecimal(dt.Rows[0]["VVIPCashback"]).ToString("###0.00");
                        lblSpromoPrice.Text = Convert.ToDecimal(dt.Rows[0]["StockistPrice"]).ToString("###0.00");
                        lblSPrice.Text = Convert.ToDecimal(dt.Rows[0]["StockistPrice"]).ToString("###0.00");
                        txtSpromocashback.Text = Convert.ToDecimal(dt.Rows[0]["StockistCashback"]).ToString("###0.00");
                        lblMSpromoPrice.Text = Convert.ToDecimal(dt.Rows[0]["MStockistPrice"]).ToString("###0.00");
                        lblMSPrice.Text = Convert.ToDecimal(dt.Rows[0]["MStockistPrice"]).ToString("###0.00");
                        txtMSpromocashback.Text = Convert.ToDecimal(dt.Rows[0]["MStockistCashback"]).ToString("###0.00");
                        txtRefProfitBonusPercent.Text = Convert.ToDecimal(dt.Rows[0]["ReferrerProfitBonusPercent"]).ToString("0.00");
                        txtSPP.Text = dt.Rows[0]["SPP"].ToString();
                        txtMBD.Text = dt.Rows[0]["MBD"].ToString();
                        txtMEB.Text = dt.Rows[0]["MEB"].ToString();
                        txtTeamKPI2.Text = dt.Rows[0]["TeamKPIBonusMS"].ToString();
                        txtTeamKPI.Text = dt.Rows[0]["TeamKPIBonus"].ToString();



                        if (dt.Rows[0]["FilePath"] != DBNull.Value)
                        {
                            btnimage.ImageUrl = dt.Rows[0]["FilePath"].ToString();
                        }
                        else
                        {
                            btnimage.ImageUrl = "Images/NoPic.png";
                        }

                       
                    }
                }
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "pop", "$('#ItemAddModal').modal('show');", true);

        }

    }

    protected void ddlShownstatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = sender as DropDownList;

        string ddlselection = ddl.SelectedValue;

        TextBox txtstartdateEdit = ddl.FindControl("txtstartdateEdit") as TextBox;
        TextBox txtenddateEdit = ddl.FindControl("txtenddateEdit") as TextBox;

        if (ddlselection == "Date Range")
        {
            txtstartdateEdit.Enabled = true;
            txtenddateEdit.Enabled = true;
        }
        else
        {
            txtstartdateEdit.Enabled = false;
            txtenddateEdit.Enabled = false;
            txtstartdateEdit.Text = "";
            txtenddateEdit.Text = "";
        }
    }

    protected void ddlShownstatus_SelectedIndexChanged1(object sender, EventArgs e)
    {
        DropDownList ddl = sender as DropDownList;

        string ddlselection = ddl.SelectedValue;

        TextBox txtstartdate = ddl.FindControl("txtstartdate") as TextBox;
        TextBox txtenddate = ddl.FindControl("txtenddate") as TextBox;

        if (ddlselection == "Date Range")
        {
            txtstartdate.Enabled = true;
            txtenddate.Enabled = true;
        }
        else
        {
            txtstartdate.Enabled = false;
            txtenddate.Enabled = false;
            txtstartdate.Text = "";
            txtenddate.Text = "";
        }
    }

    protected void btnupdate_Click(object sender, EventArgs e)
    {
        using (SqlConnection con = new SqlConnection(MF_Con.Text))
        {
            con.Open();

            SqlCommand updatemaindetail = new SqlCommand("update MF_PromoCampaign set CampaignTitle='"+ txtcampaignname.Text +"',start_date=@startdate,end_date=@enddate,modified_dt=dateadd(hour,(8),CONVERT([varchar](20),getutcdate(),(120))) where ids='"+ Request.QueryString["groupid"].ToString() +"' and deleteind <> 'X'", con);

            updatemaindetail.Parameters.AddWithValue("@startdate", Convert.ToDateTime(txtstartdate.Text));
            updatemaindetail.Parameters.AddWithValue("@enddate", Convert.ToDateTime(txtenddate.Text));
            try
            {
                updatemaindetail.ExecuteNonQuery();

                SqlCommand updatesubitemdetail = new SqlCommand("update MF_AgentPromo set PromoCampaignTitle='"+ txtcampaignname.Text +"',start_date=@startdate,end_date=@enddate,modified_dt=dateadd(hour,(8),CONVERT([varchar](20),getutcdate(),(120))) where promoid='"+ Request.QueryString["groupid"].ToString() +"' and deleteind <> 'X'", con);

                updatesubitemdetail.Parameters.AddWithValue("@startdate", Convert.ToDateTime(txtstartdate.Text));
                updatesubitemdetail.Parameters.AddWithValue("@enddate", Convert.ToDateTime(txtenddate.Text));
                updatesubitemdetail.ExecuteNonQuery();

                ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Promotion Campaign Updated. Click OK to back to campaign menu');window.location='PromoCampaign.aspx?username=" + Request.QueryString["username"] + "&merchant=" + Request.QueryString["merchant"] + "'", true);
                
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Failed to update please try again after few minutes.');", true);
            }

        }
    }

    protected void btnback_Click(object sender, EventArgs e)
    {
        Response.Redirect("PromoCampaign.aspx?username=" + Request.QueryString["username"] + "&merchant=" + Request.QueryString["merchant"] );
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        showitemlist();
        ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "$('#ItemAddModal').modal('show');", true);
    }

    protected void btnaddtopromotion_Click(object sender, EventArgs e)
    {
        using (SqlConnection con = new SqlConnection(MF_Con.Text))
        {
            con.Open();

            SqlCommand itemchkcmd = new SqlCommand("select * from MF_AgentPromo where promoid = '"+ Request.QueryString["groupid"].ToString() +"' and deleteind <> 'X' and item_code='"+ lblitem_code.Text +"'", con);
            SqlDataAdapter itemchkadp = new SqlDataAdapter(itemchkcmd);
            DataTable itemchkdt = new DataTable();
            itemchkadp.Fill(itemchkdt);

            if (itemchkdt.Rows.Count <= 0)
            {

                ///Member Promo Price

                SqlCommand insertcmd = new SqlCommand("Insert_PromoItem", con);
                insertcmd.CommandType = CommandType.StoredProcedure;
                insertcmd.Parameters.AddWithValue("@grouptitle", txtcampaignname.Text.Trim().Replace("'", "`"));
                insertcmd.Parameters.AddWithValue("@MerchantID", Request.QueryString["merchant"].ToString());
                insertcmd.Parameters.AddWithValue("@ItemCode", lblitem_code.Text);
                insertcmd.Parameters.AddWithValue("@AgentLevelCode", "D02");
                if (string.IsNullOrEmpty(lblMpromoPrice.Text))
                {
                    insertcmd.Parameters.AddWithValue("@Promo_Price", "0");
                } else
                {
                    insertcmd.Parameters.AddWithValue("@Promo_Price", lblMpromoPrice.Text);
                }
                if (string.IsNullOrEmpty(txtMDiscountAmt.Text))
                {
                    insertcmd.Parameters.AddWithValue("@Discount_Amt", "0");
                }
                else
                {
                    insertcmd.Parameters.AddWithValue("@Discount_Amt", txtMDiscountAmt.Text);
                }
                insertcmd.Parameters.AddWithValue("@Discount_Type", ddlMdiscountType.SelectedValue);
                insertcmd.Parameters.AddWithValue("@promoid", Request.QueryString["groupid"].ToString());
                if (string.IsNullOrEmpty(txtMpromocashback.Text))
                {
                    insertcmd.Parameters.AddWithValue("@CashBackAmt", "0");
                }
                else
                {
                    insertcmd.Parameters.AddWithValue("@CashBackAmt", txtMpromocashback.Text);
                }
                insertcmd.Parameters.AddWithValue("@EarnRPWhenBuy", txtMrewardpoint.Text);
                insertcmd.Parameters.AddWithValue("@RedemptionPoint", txtMredemptpoint.Text);
                if (string.IsNullOrEmpty(txtSPP.Text))
                {
                    insertcmd.Parameters.AddWithValue("@SPP", "0");
                }
                else
                {
                    insertcmd.Parameters.AddWithValue("@SPP", txtSPP.Text);
                }
                if (string.IsNullOrEmpty(txtRefProfitBonusPercent.Text))
                {
                    insertcmd.Parameters.AddWithValue("@ReferrerProfitBonusPercent", "0");
                }
                else
                {
                    insertcmd.Parameters.AddWithValue("@ReferrerProfitBonusPercent", txtRefProfitBonusPercent.Text);
                }
                if (string.IsNullOrEmpty(txtTeamKPI.Text))
                {
                    insertcmd.Parameters.AddWithValue("@TeamKPIBonus", "0");
                }
                else
                {
                    insertcmd.Parameters.AddWithValue("@TeamKPIBonus", txtTeamKPI.Text);
                }
                if (string.IsNullOrEmpty(txtTeamKPI2.Text))
                {
                    insertcmd.Parameters.AddWithValue("@TeamKPIBonusMS", "0");
                }
                else
                {
                    insertcmd.Parameters.AddWithValue("@TeamKPIBonusMS", txtTeamKPI2.Text);
                }
                if (string.IsNullOrEmpty(txtMEB.Text))
                {
                    insertcmd.Parameters.AddWithValue("@MEB", "0");
                }
                else
                {
                    insertcmd.Parameters.AddWithValue("@MEB", txtMEB.Text);

                }
                if (string.IsNullOrEmpty(txtMBD.Text))
                {
                    insertcmd.Parameters.AddWithValue("@MBD", "0");
                }
                else
                {
                    insertcmd.Parameters.AddWithValue("@MBD", txtMBD.Text);
                }
                insertcmd.Parameters.AddWithValue("@startdate", Convert.ToDateTime(txtstartdate.Text));
                insertcmd.Parameters.AddWithValue("@enddate", Convert.ToDateTime(txtenddate.Text));


                try
                {
                    insertcmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
                finally
                {
                    insertcmd.Dispose();
                }

                ///VIP Promo Price

                SqlCommand VIPinsertcmd = new SqlCommand("Insert_PromoItem", con);
                VIPinsertcmd.CommandType = CommandType.StoredProcedure;
                VIPinsertcmd.Parameters.AddWithValue("@grouptitle", txtcampaignname.Text.Trim().Replace("'", "`"));
                VIPinsertcmd.Parameters.AddWithValue("@MerchantID", Request.QueryString["merchant"].ToString());
                VIPinsertcmd.Parameters.AddWithValue("@ItemCode", lblitem_code.Text);
                VIPinsertcmd.Parameters.AddWithValue("@AgentLevelCode", "D01");
                if (string.IsNullOrEmpty(lblVpromoPrice.Text))
                {
                    VIPinsertcmd.Parameters.AddWithValue("@Promo_Price", "0");
                }
                else
                {
                    VIPinsertcmd.Parameters.AddWithValue("@Promo_Price", lblVpromoPrice.Text);
                }
                if (string.IsNullOrEmpty(txtVDiscountAmt.Text))
                {
                    VIPinsertcmd.Parameters.AddWithValue("@Discount_Amt", "0");
                }
                else
                {
                    VIPinsertcmd.Parameters.AddWithValue("@Discount_Amt", txtVDiscountAmt.Text);
                }
                VIPinsertcmd.Parameters.AddWithValue("@Discount_Type", ddlVdiscountType.SelectedValue);
                VIPinsertcmd.Parameters.AddWithValue("@promoid", Request.QueryString["groupid"].ToString());
                if (string.IsNullOrEmpty(txtVpromocashback.Text))
                {
                    VIPinsertcmd.Parameters.AddWithValue("@CashBackAmt", "0");
                }
                else
                {
                    VIPinsertcmd.Parameters.AddWithValue("@CashBackAmt", txtVpromocashback.Text);
                }
                VIPinsertcmd.Parameters.AddWithValue("@EarnRPWhenBuy", txtVIPrewardpoint.Text);
                VIPinsertcmd.Parameters.AddWithValue("@RedemptionPoint", txtVIPredemptpoint.Text);
                if (string.IsNullOrEmpty(txtSPP.Text))
                {
                    VIPinsertcmd.Parameters.AddWithValue("@SPP", "0");
                }
                else
                {
                    VIPinsertcmd.Parameters.AddWithValue("@SPP", txtSPP.Text);
                }
                if (string.IsNullOrEmpty(txtRefProfitBonusPercent.Text))
                {
                    VIPinsertcmd.Parameters.AddWithValue("@ReferrerProfitBonusPercent", "0");
                }
                else
                {
                    VIPinsertcmd.Parameters.AddWithValue("@ReferrerProfitBonusPercent", txtRefProfitBonusPercent.Text);
                }
                if (string.IsNullOrEmpty(txtTeamKPI.Text))
                {
                    VIPinsertcmd.Parameters.AddWithValue("@TeamKPIBonus", "0");
                }
                else
                {
                    VIPinsertcmd.Parameters.AddWithValue("@TeamKPIBonus", txtTeamKPI.Text);
                }
                if (string.IsNullOrEmpty(txtTeamKPI2.Text))
                {
                    VIPinsertcmd.Parameters.AddWithValue("@TeamKPIBonusMS", "0");
                }
                else
                {
                    VIPinsertcmd.Parameters.AddWithValue("@TeamKPIBonusMS", txtTeamKPI2.Text);
                }
                if (string.IsNullOrEmpty(txtMEB.Text))
                {
                    VIPinsertcmd.Parameters.AddWithValue("@MEB", "0");
                }
                else
                {
                    VIPinsertcmd.Parameters.AddWithValue("@MEB", txtMEB.Text);

                }
                if (string.IsNullOrEmpty(txtMBD.Text))
                {
                    VIPinsertcmd.Parameters.AddWithValue("@MBD", "0");
                }
                else
                {
                    VIPinsertcmd.Parameters.AddWithValue("@MBD", txtMBD.Text);
                }
                VIPinsertcmd.Parameters.AddWithValue("@startdate", Convert.ToDateTime(txtstartdate.Text));
                VIPinsertcmd.Parameters.AddWithValue("@enddate", Convert.ToDateTime(txtenddate.Text));


                try
                {
                    VIPinsertcmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
                finally
                {
                    VIPinsertcmd.Dispose();
                }


                ///VVIP Promo Price

                SqlCommand VVIPinsertcmd = new SqlCommand("Insert_PromoItem", con);
                VVIPinsertcmd.CommandType = CommandType.StoredProcedure;
                VVIPinsertcmd.Parameters.AddWithValue("@grouptitle", txtcampaignname.Text.Trim().Replace("'", "`"));
                VVIPinsertcmd.Parameters.AddWithValue("@MerchantID", Request.QueryString["merchant"].ToString());
                VVIPinsertcmd.Parameters.AddWithValue("@ItemCode", lblitem_code.Text);
                VVIPinsertcmd.Parameters.AddWithValue("@AgentLevelCode", "D04");
                if (string.IsNullOrEmpty(lblVVIPpromoPrice.Text))
                {
                    VVIPinsertcmd.Parameters.AddWithValue("@Promo_Price", "0");
                }
                else
                {
                    VVIPinsertcmd.Parameters.AddWithValue("@Promo_Price", lblVVIPpromoPrice.Text);
                }
                if (string.IsNullOrEmpty(txtVVIPDiscountAmt.Text))
                {
                    VVIPinsertcmd.Parameters.AddWithValue("@Discount_Amt", "0");
                }
                else
                {
                    VVIPinsertcmd.Parameters.AddWithValue("@Discount_Amt", txtVVIPDiscountAmt.Text);
                }
                VVIPinsertcmd.Parameters.AddWithValue("@Discount_Type", ddlVVIPdiscountType.SelectedValue);
                VVIPinsertcmd.Parameters.AddWithValue("@promoid", Request.QueryString["groupid"].ToString());
                if (string.IsNullOrEmpty(txtVVIPpromocashback.Text))
                {
                    VVIPinsertcmd.Parameters.AddWithValue("@CashBackAmt", "0");
                }
                else
                {
                    VVIPinsertcmd.Parameters.AddWithValue("@CashBackAmt", txtVVIPpromocashback.Text);
                }
                VVIPinsertcmd.Parameters.AddWithValue("@EarnRPWhenBuy", txtVVIPrewardpoint.Text);
                VVIPinsertcmd.Parameters.AddWithValue("@RedemptionPoint", txtVVIPredemptpoint.Text);
                if (string.IsNullOrEmpty(txtSPP.Text))
                {
                    VVIPinsertcmd.Parameters.AddWithValue("@SPP", "0");
                }
                else
                {
                    VVIPinsertcmd.Parameters.AddWithValue("@SPP", txtSPP.Text);
                }
                if (string.IsNullOrEmpty(txtRefProfitBonusPercent.Text))
                {
                    VVIPinsertcmd.Parameters.AddWithValue("@ReferrerProfitBonusPercent", "0");
                }
                else
                {
                    VVIPinsertcmd.Parameters.AddWithValue("@ReferrerProfitBonusPercent", txtRefProfitBonusPercent.Text);
                }
                if (string.IsNullOrEmpty(txtTeamKPI.Text))
                {
                    VVIPinsertcmd.Parameters.AddWithValue("@TeamKPIBonus", "0");
                }
                else
                {
                    VVIPinsertcmd.Parameters.AddWithValue("@TeamKPIBonus", txtTeamKPI.Text);
                }
                if (string.IsNullOrEmpty(txtTeamKPI2.Text))
                {
                    VVIPinsertcmd.Parameters.AddWithValue("@TeamKPIBonusMS", "0");
                }
                else
                {
                    VVIPinsertcmd.Parameters.AddWithValue("@TeamKPIBonusMS", txtTeamKPI2.Text);
                }
                if (string.IsNullOrEmpty(txtMEB.Text))
                {
                    VVIPinsertcmd.Parameters.AddWithValue("@MEB", "0");
                }
                else
                {
                    VVIPinsertcmd.Parameters.AddWithValue("@MEB", txtMEB.Text);

                }
                if (string.IsNullOrEmpty(txtMBD.Text))
                {
                    VVIPinsertcmd.Parameters.AddWithValue("@MBD", "0");
                }
                else
                {
                    VVIPinsertcmd.Parameters.AddWithValue("@MBD", txtMBD.Text);
                }
                VVIPinsertcmd.Parameters.AddWithValue("@startdate", Convert.ToDateTime(txtstartdate.Text));
                VVIPinsertcmd.Parameters.AddWithValue("@enddate", Convert.ToDateTime(txtenddate.Text));


                try
                {
                    VVIPinsertcmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
                finally
                {
                    VVIPinsertcmd.Dispose();
                }

                ///Stockist Promo Price

                SqlCommand Stockistinsertcmd = new SqlCommand("Insert_PromoItem", con);
                Stockistinsertcmd.CommandType = CommandType.StoredProcedure;
                Stockistinsertcmd.Parameters.AddWithValue("@grouptitle", txtcampaignname.Text.Trim().Replace("'", "`"));
                Stockistinsertcmd.Parameters.AddWithValue("@MerchantID", Request.QueryString["merchant"].ToString());
                Stockistinsertcmd.Parameters.AddWithValue("@ItemCode", lblitem_code.Text);
                Stockistinsertcmd.Parameters.AddWithValue("@AgentLevelCode", "D03");
                if (string.IsNullOrEmpty(lblSpromoPrice.Text))
                {
                    Stockistinsertcmd.Parameters.AddWithValue("@Promo_Price", "0");
                }
                else
                {
                    Stockistinsertcmd.Parameters.AddWithValue("@Promo_Price", lblSpromoPrice.Text);
                }
                if (string.IsNullOrEmpty(txtSDiscountAmt.Text))
                {
                    Stockistinsertcmd.Parameters.AddWithValue("@Discount_Amt", "0");
                }
                else
                {
                    Stockistinsertcmd.Parameters.AddWithValue("@Discount_Amt", txtSDiscountAmt.Text);
                }
                Stockistinsertcmd.Parameters.AddWithValue("@Discount_Type", ddlSdiscountType.SelectedValue);
                Stockistinsertcmd.Parameters.AddWithValue("@promoid", Request.QueryString["groupid"].ToString());
                if (string.IsNullOrEmpty(txtSpromocashback.Text))
                {
                    Stockistinsertcmd.Parameters.AddWithValue("@CashBackAmt", "0");
                }
                else
                {
                    Stockistinsertcmd.Parameters.AddWithValue("@CashBackAmt", txtSpromocashback.Text);
                }
                Stockistinsertcmd.Parameters.AddWithValue("@EarnRPWhenBuy", txtSrewardpoint.Text);
                Stockistinsertcmd.Parameters.AddWithValue("@RedemptionPoint", txtSredemptpoint.Text);
                if (string.IsNullOrEmpty(txtSPP.Text))
                {
                    Stockistinsertcmd.Parameters.AddWithValue("@SPP", "0");
                }
                else
                {
                    Stockistinsertcmd.Parameters.AddWithValue("@SPP", txtSPP.Text);
                }
                if (string.IsNullOrEmpty(txtRefProfitBonusPercent.Text))
                {
                    Stockistinsertcmd.Parameters.AddWithValue("@ReferrerProfitBonusPercent", "0");
                }
                else
                {
                    Stockistinsertcmd.Parameters.AddWithValue("@ReferrerProfitBonusPercent", txtRefProfitBonusPercent.Text);
                }
                if (string.IsNullOrEmpty(txtTeamKPI.Text))
                {
                    Stockistinsertcmd.Parameters.AddWithValue("@TeamKPIBonus", "0");
                }
                else
                {
                    Stockistinsertcmd.Parameters.AddWithValue("@TeamKPIBonus", txtTeamKPI.Text);
                }
                if (string.IsNullOrEmpty(txtTeamKPI2.Text))
                {
                    Stockistinsertcmd.Parameters.AddWithValue("@TeamKPIBonusMS", "0");
                }
                else
                {
                    Stockistinsertcmd.Parameters.AddWithValue("@TeamKPIBonusMS", txtTeamKPI2.Text);
                }
                if (string.IsNullOrEmpty(txtMEB.Text))
                {
                    Stockistinsertcmd.Parameters.AddWithValue("@MEB", "0");
                }
                else
                {
                    Stockistinsertcmd.Parameters.AddWithValue("@MEB", txtMEB.Text);

                }
                if (string.IsNullOrEmpty(txtMBD.Text))
                {
                    Stockistinsertcmd.Parameters.AddWithValue("@MBD", "0");
                }
                else
                {
                    Stockistinsertcmd.Parameters.AddWithValue("@MBD", txtMBD.Text);
                }
                Stockistinsertcmd.Parameters.AddWithValue("@startdate", Convert.ToDateTime(txtstartdate.Text));
                Stockistinsertcmd.Parameters.AddWithValue("@enddate", Convert.ToDateTime(txtenddate.Text));


                try
                {
                    Stockistinsertcmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
                finally
                {
                    Stockistinsertcmd.Dispose();
                }

                ///Master Stockist Promo Price

                SqlCommand MStockistinsertcmd = new SqlCommand("Insert_PromoItem", con);
                MStockistinsertcmd.CommandType = CommandType.StoredProcedure;
                MStockistinsertcmd.Parameters.AddWithValue("@grouptitle", txtcampaignname.Text.Trim().Replace("'", "`"));
                MStockistinsertcmd.Parameters.AddWithValue("@MerchantID", Request.QueryString["merchant"].ToString());
                MStockistinsertcmd.Parameters.AddWithValue("@ItemCode", lblitem_code.Text);
                MStockistinsertcmd.Parameters.AddWithValue("@AgentLevelCode", "D05");
                if (string.IsNullOrEmpty(lblMSpromoPrice.Text))
                {
                    MStockistinsertcmd.Parameters.AddWithValue("@Promo_Price", "0");
                }
                else
                {
                    MStockistinsertcmd.Parameters.AddWithValue("@Promo_Price", lblMSpromoPrice.Text);
                }
                if (string.IsNullOrEmpty(txtMSDiscountAmt.Text))
                {
                    MStockistinsertcmd.Parameters.AddWithValue("@Discount_Amt", "0");
                }
                else
                {
                    MStockistinsertcmd.Parameters.AddWithValue("@Discount_Amt", txtMSDiscountAmt.Text);
                }
                MStockistinsertcmd.Parameters.AddWithValue("@Discount_Type", ddlMSdiscountType.SelectedValue);
                MStockistinsertcmd.Parameters.AddWithValue("@promoid", Request.QueryString["groupid"].ToString());
                if (string.IsNullOrEmpty(txtMSpromocashback.Text))
                {
                    MStockistinsertcmd.Parameters.AddWithValue("@CashBackAmt", "0");
                }
                else
                {
                    MStockistinsertcmd.Parameters.AddWithValue("@CashBackAmt", txtMSpromocashback.Text);
                }
                MStockistinsertcmd.Parameters.AddWithValue("@EarnRPWhenBuy", txtMSrewardpoint.Text);
                MStockistinsertcmd.Parameters.AddWithValue("@RedemptionPoint", txtMSredemptpoint.Text);
                if (string.IsNullOrEmpty(txtSPP.Text))
                {
                    MStockistinsertcmd.Parameters.AddWithValue("@SPP", "0");
                }
                else
                {
                    MStockistinsertcmd.Parameters.AddWithValue("@SPP", txtSPP.Text);
                }
                if (string.IsNullOrEmpty(txtRefProfitBonusPercent.Text))
                {
                    MStockistinsertcmd.Parameters.AddWithValue("@ReferrerProfitBonusPercent", "0");
                }
                else
                {
                    MStockistinsertcmd.Parameters.AddWithValue("@ReferrerProfitBonusPercent", txtRefProfitBonusPercent.Text);
                }
                if (string.IsNullOrEmpty(txtTeamKPI.Text))
                {
                    MStockistinsertcmd.Parameters.AddWithValue("@TeamKPIBonus", "0");
                }
                else
                {
                    MStockistinsertcmd.Parameters.AddWithValue("@TeamKPIBonus", txtTeamKPI.Text);
                }
                if (string.IsNullOrEmpty(txtTeamKPI2.Text))
                {
                    MStockistinsertcmd.Parameters.AddWithValue("@TeamKPIBonusMS", "0");
                }
                else
                {
                    MStockistinsertcmd.Parameters.AddWithValue("@TeamKPIBonusMS", txtTeamKPI2.Text);
                }
                if (string.IsNullOrEmpty(txtMEB.Text))
                {
                    MStockistinsertcmd.Parameters.AddWithValue("@MEB", "0");
                }
                else
                {
                    MStockistinsertcmd.Parameters.AddWithValue("@MEB", txtMEB.Text);

                }
                if (string.IsNullOrEmpty(txtMBD.Text))
                {
                    MStockistinsertcmd.Parameters.AddWithValue("@MBD", "0");
                }
                else
                {
                    MStockistinsertcmd.Parameters.AddWithValue("@MBD", txtMBD.Text);
                }
                MStockistinsertcmd.Parameters.AddWithValue("@startdate", Convert.ToDateTime(txtstartdate.Text));
                MStockistinsertcmd.Parameters.AddWithValue("@enddate", Convert.ToDateTime(txtenddate.Text));


                try
                {
                    MStockistinsertcmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
                finally
                {
                    MStockistinsertcmd.Dispose();
                }

               
                ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "$('#ItemAddModal').modal('hide');$('body').removeClass('modal-open');$('.modal-backdrop').remove();", true);


            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Item already added in this campaign.');", true);
            }

            showgroupitemlist();

            con.Close();
        }
    }

    protected void promotionitem_grdview_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        using (SqlConnection con = new SqlConnection(MF_Con.Text))
        {
            con.Open();
            if (e.CommandName == "EditItem")
            {
                exampleModalLabel.InnerText = "Edit Promotion Setting";

                btnupdatepromotion.Visible = true;
                btnaddtopromotion.Visible = false;

                using (SqlCommand cmd = new SqlCommand("select a.item_code,a.LongDesc,a.Barcode,a.FilePath,b.SPP,b.ReferrerProfitBonusPercent,b.TeamKPIBonus,b.TeamKPIBonusMS,b.MEB,b.MBD " +
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
                "ae.discount_type as [MSDiscountType]," +
                "ac.discount_type as [SDiscountType]," +
                "ad.discount_type as [VVIPDiscountType]," +
                "ab.discount_type as [VDiscountType] , " +
                "aa.discount_type as [MDiscountType]" + 
                "from MF_Item AS a " +
                "LEFT JOIN [MF_AgentPromo] AS b ON b.item_code=a.item_code and b.[AgentLevelCode]='D02' and b.[DeleteInd] <> 'X' and b.promoid='"+ Request.QueryString["groupid"].ToString() +"'" +
                "LEFT JOIN [MF_AgentCommi] AS p ON p.Item_Code = a.Item_Code and p.[AgentLevelCode]='D02' and p.[DeleteInd] <> 'X' " +
                "LEFT JOIN [MF_AgentCommi] AS q ON q.Item_Code = a.Item_Code and q.[AgentLevelCode]='D01' and q.[DeleteInd] <> 'X' " +
                "LEFT JOIN [MF_AgentCommi] AS r ON r.Item_Code = a.Item_Code and r.[AgentLevelCode]='D03' and r.[DeleteInd] <> 'X' " +
                "LEFT JOIN [MF_AgentCommi] AS s ON s.Item_Code = a.Item_Code and s.[AgentLevelCode]='D04' and s.[DeleteInd] <> 'X' " +
                "LEFT JOIN [MF_AgentCommi] AS t ON t.Item_Code = a.Item_Code and t.[AgentLevelCode]='D05' and t.[DeleteInd] <> 'X'  " +
                "LEFT JOIN [MF_AgentPromo] AS aa ON aa.Item_Code = a.Item_Code and aa.[AgentLevelCode]='D02' and aa.[DeleteInd] <> 'X' and aa.promoid='"+ Request.QueryString["groupid"].ToString() +"' " +
                "LEFT JOIN [MF_AgentPromo] AS ab ON ab.Item_Code = a.Item_Code and ab.[AgentLevelCode]='D01' and ab.[DeleteInd] <> 'X' and ab.promoid='"+ Request.QueryString["groupid"].ToString() +"' " +
                "LEFT JOIN [MF_AgentPromo] AS ac ON ac.Item_Code = a.Item_Code and ac.[AgentLevelCode]='D03' and ac.[DeleteInd] <> 'X' and ac.promoid='"+ Request.QueryString["groupid"].ToString() +"' " +
                "LEFT JOIN [MF_AgentPromo] AS ad ON ad.Item_Code = a.Item_Code and ad.[AgentLevelCode]='D04' and ad.[DeleteInd] <> 'X' and ad.promoid='"+ Request.QueryString["groupid"].ToString() +"' " +
                "LEFT JOIN [MF_AgentPromo] AS ae ON ae.Item_Code = a.Item_Code and ae.[AgentLevelCode]='D05' and ae.[DeleteInd] <> 'X' and ae.promoid='"+ Request.QueryString["groupid"].ToString() +"'  " +
                "where a.deleteind <> 'X' and a.item_code='" + e.CommandArgument.ToString() + "' " +
                " group by a.item_code,a.LongDesc,a.Barcode,a.FilePath, " +
                "b.SPP,b.ReferrerProfitBonusPercent,b.TeamKPIBonus,b.TeamKPIBonusMS,b.MEB,b.MBD,p.CashBackAmount,q.CashBackAmount,r.CashBackAmount," +
                "s.CashBackAmount,t.CashBackAmount,p.BuyPrice,q.BuyPrice,r.BuyPrice,s.BuyPrice,t.BuyPrice ,aa.RedemptionPoint,ab.RedemptionPoint,ac.RedemptionPoint,ad.RedemptionPoint,ae.RedemptionPoint" +
                ",aa.EarnRPWhenBuy,ab.EarnRPWhenBuy,ac.EarnRPWhenBuy,ad.EarnRPWhenBuy,ae.EarnRPWhenBuy,"+
                "aa.CashBackAmt,ab.CashBackAmt,ac.CashBackAmt,ad.CashBackAmt,ae.CashBackAmt,aa.Promo_price,ab.Promo_price,ac.Promo_price,ad.Promo_price,ae.Promo_price,aa.discount_type,ab.discount_type," +
                "ac.discount_type,ad.discount_type,ae.discount_type,aa.discount_amt,ab.discount_amt,ac.discount_amt,ad.discount_amt,ae.discount_amt", con))
                {
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adp.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        lblitem_code.Text = dt.Rows[0]["item_code"].ToString();
                        lblitemname.Text = dt.Rows[0]["longdesc"].ToString();
                        lblbarcode.Text = dt.Rows[0]["barcode"].ToString();
                        lblMpromoPrice.Text = Convert.ToDecimal(dt.Rows[0]["PMemberPrice"]).ToString("###0.00");
                        lblMprice.Text = Convert.ToDecimal(dt.Rows[0]["MemberPrice"]).ToString("###0.00");
                        txtMpromocashback.Text = Convert.ToDecimal(dt.Rows[0]["PMemberCashback"]).ToString("###0.00");
                        lblVpromoPrice.Text = Convert.ToDecimal(dt.Rows[0]["PVIPPrice"]).ToString("###0.00");
                        lblVPrice.Text = Convert.ToDecimal(dt.Rows[0]["VIPPrice"]).ToString("###0.00");
                        txtVpromocashback.Text = Convert.ToDecimal(dt.Rows[0]["PVIPCashback"]).ToString("###0.00");
                        lblVVIPpromoPrice.Text = Convert.ToDecimal(dt.Rows[0]["PVVIPPrice"]).ToString("###0.00");
                        lblVVIPPrice.Text = Convert.ToDecimal(dt.Rows[0]["VVIPPrice"]).ToString("###0.00");
                        txtVVIPpromocashback.Text = Convert.ToDecimal(dt.Rows[0]["PVVIPCashback"]).ToString("###0.00");
                        lblSpromoPrice.Text = Convert.ToDecimal(dt.Rows[0]["PStockistPrice"]).ToString("###0.00");
                        lblSPrice.Text = Convert.ToDecimal(dt.Rows[0]["StockistPrice"]).ToString("###0.00");
                        txtSpromocashback.Text = Convert.ToDecimal(dt.Rows[0]["PStockistCashback"]).ToString("###0.00");
                        lblMSpromoPrice.Text = Convert.ToDecimal(dt.Rows[0]["PMStockistPrice"]).ToString("###0.00");
                        lblMSPrice.Text = Convert.ToDecimal(dt.Rows[0]["MStockistPrice"]).ToString("###0.00");
                        txtMSpromocashback.Text = Convert.ToDecimal(dt.Rows[0]["PMStockistCashback"]).ToString("###0.00");
                        txtRefProfitBonusPercent.Text = Convert.ToDecimal(dt.Rows[0]["ReferrerProfitBonusPercent"]).ToString("0.00");
                        txtSPP.Text = dt.Rows[0]["SPP"].ToString();
                        txtMBD.Text = dt.Rows[0]["MBD"].ToString();
                        txtMEB.Text = dt.Rows[0]["MEB"].ToString();
                        txtTeamKPI2.Text = dt.Rows[0]["TeamKPIBonusMS"].ToString();
                        txtTeamKPI.Text = dt.Rows[0]["TeamKPIBonus"].ToString();
                        txtMredemptpoint.Text = Convert.ToDecimal(dt.Rows[0]["PMRedemptionPoint"]).ToString("###0.00");
                        txtMrewardpoint.Text = Convert.ToDecimal(dt.Rows[0]["PMemberEarnRPWhenBuy"]).ToString("###0.00");
                        txtMSredemptpoint.Text  = Convert.ToDecimal(dt.Rows[0]["PMStockistRedemptionPoint"]).ToString("###0.00");
                        txtMSrewardpoint.Text = Convert.ToDecimal(dt.Rows[0]["PMStockistEarnRPWhenBuy"]).ToString("###0.00");
                        txtVIPredemptpoint.Text = Convert.ToDecimal(dt.Rows[0]["PVIPRedemptionPoint"]).ToString("###0.00");
                        txtVIPrewardpoint.Text = Convert.ToDecimal(dt.Rows[0]["PVIPEarnRPWhenBuy"]).ToString("###0.00");
                        txtVVIPredemptpoint.Text = Convert.ToDecimal(dt.Rows[0]["PVVIPRedemptionPoint"]).ToString("###0.00");
                        txtVVIPrewardpoint.Text = Convert.ToDecimal(dt.Rows[0]["PVVIPEarnRPWhenBuy"]).ToString("###0.00");
                        txtSredemptpoint.Text = Convert.ToDecimal(dt.Rows[0]["PStockistRedemptionPoint"]).ToString("###0.00");
                        txtSrewardpoint.Text = Convert.ToDecimal(dt.Rows[0]["PStockistEarnRPWhenBuy"]).ToString("###0.00");
                        ddlMdiscountType.SelectedValue = dt.Rows[0]["MDiscountType"].ToString();
                        ddlSdiscountType.SelectedValue = dt.Rows[0]["SDiscountType"].ToString();
                        ddlVdiscountType.SelectedValue = dt.Rows[0]["VDiscountType"].ToString();
                        ddlVVIPdiscountType.SelectedValue = dt.Rows[0]["VVIPDiscountType"].ToString();
                        ddlMSdiscountType.SelectedValue = dt.Rows[0]["MSDiscountType"].ToString();
                        txtMDiscountAmt.Text = dt.Rows[0]["MDiscountAmt"].ToString();
                        txtVDiscountAmt.Text = dt.Rows[0]["VDiscountAmt"].ToString();
                        txtVVIPDiscountAmt.Text = dt.Rows[0]["VVIPDiscountAmt"].ToString();
                        txtSDiscountAmt.Text = dt.Rows[0]["SDiscountAmt"].ToString();
                        txtMSDiscountAmt.Text = dt.Rows[0]["MSDiscountAmt"].ToString();

                        if (dt.Rows[0]["FilePath"] != DBNull.Value)
                        {
                            btnimage.ImageUrl = dt.Rows[0]["FilePath"].ToString();
                        }
                        else
                        {
                            btnimage.ImageUrl = "Images/NoPic.png";
                        }


                    }
                }



                ScriptManager.RegisterStartupScript(this, GetType(), "pop", "$('#ItemAddModal').modal('show');", true);

            }

            if (e.CommandName == "DelItem")
            {
                SqlCommand updatemaindetail = new SqlCommand("update MF_AgentPromo set deleteind='X',modified_dt=dateadd(hour,(8),CONVERT([varchar](20),getutcdate(),(120))) where promoid='"+ Request.QueryString["groupid"].ToString() +"' and deleteind <> 'X' and item_code='"+ e.CommandArgument.ToString() +"'", con);

                try
                {
                    updatemaindetail.ExecuteNonQuery();
                    showgroupitemlist();

                    updateee.Update();

                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Failed to update please try again after few minutes.');", true);
                }
            }
        }
    }

    protected void btnupdatepromotion_Click(object sender, EventArgs e)
    {
        using (SqlConnection con = new SqlConnection(MF_Con.Text))
        {
            con.Open();

            ///Member Promo Price

            SqlCommand insertcmd = new SqlCommand("update MF_AgentPromo set discount_Amt=@Discount_Amt,discount_type=@Discount_Type,EarnRPWhenBuy=@EarnRPWhenBuy,RedemptionPoint=@RedemptionPoint,promo_price=@Promo_Price,Cashbackamt=@CashBackAmt,SPP=@SPP,ReferrerProfitBonusPercent=@ReferrerProfitBonusPercent,TeamKPIBonus=@TeamKPIBonus,TeamKPIBonusMS=@TeamKPIBonusMS,MEB=@MEB,MBD=@MBD,modified_dt=dateadd(hour,(8),CONVERT([varchar](20),getutcdate(),(120))) where promoid=@promoid and item_code=@ItemCode and AgentLevelCode=@AgentLevelCode and deleteind <> 'X'", con);

            insertcmd.Parameters.AddWithValue("@ItemCode", lblitem_code.Text);
            insertcmd.Parameters.AddWithValue("@AgentLevelCode", "D02");
            if (string.IsNullOrEmpty(lblMpromoPrice.Text))
            {
                insertcmd.Parameters.AddWithValue("@Promo_Price", "0");
            }
            else
            {
                insertcmd.Parameters.AddWithValue("@Promo_Price", lblMpromoPrice.Text);
            }
            if (string.IsNullOrEmpty(txtMDiscountAmt.Text))
            {
                insertcmd.Parameters.AddWithValue("@Discount_Amt", "0");
            }
            else
            {
                insertcmd.Parameters.AddWithValue("@Discount_Amt", txtMDiscountAmt.Text);
            }
            insertcmd.Parameters.AddWithValue("@Discount_Type", ddlMdiscountType.SelectedValue);
            insertcmd.Parameters.AddWithValue("@promoid", Request.QueryString["groupid"].ToString());
            if (string.IsNullOrEmpty(txtMpromocashback.Text))
            {
                insertcmd.Parameters.AddWithValue("@CashBackAmt", "0");
            }
            else
            {
                insertcmd.Parameters.AddWithValue("@CashBackAmt", txtMpromocashback.Text);
            }
            insertcmd.Parameters.AddWithValue("@EarnRPWhenBuy", txtMrewardpoint.Text);
            insertcmd.Parameters.AddWithValue("@RedemptionPoint", txtMredemptpoint.Text);
            if (string.IsNullOrEmpty(txtSPP.Text))
            {
                insertcmd.Parameters.AddWithValue("@SPP", "0");
            }
            else
            {
                insertcmd.Parameters.AddWithValue("@SPP", txtSPP.Text);
            }
            if (string.IsNullOrEmpty(txtRefProfitBonusPercent.Text))
            {
                insertcmd.Parameters.AddWithValue("@ReferrerProfitBonusPercent", "0");
            }
            else
            {
                insertcmd.Parameters.AddWithValue("@ReferrerProfitBonusPercent", txtRefProfitBonusPercent.Text);
            }
            if (string.IsNullOrEmpty(txtTeamKPI.Text))
            {
                insertcmd.Parameters.AddWithValue("@TeamKPIBonus", "0");
            }
            else
            {
                insertcmd.Parameters.AddWithValue("@TeamKPIBonus", txtTeamKPI.Text);
            }
            if (string.IsNullOrEmpty(txtTeamKPI2.Text))
            {
                insertcmd.Parameters.AddWithValue("@TeamKPIBonusMS", "0");
            }
            else
            {
                insertcmd.Parameters.AddWithValue("@TeamKPIBonusMS", txtTeamKPI2.Text);
            }
            if (string.IsNullOrEmpty(txtMEB.Text))
            {
                insertcmd.Parameters.AddWithValue("@MEB", "0");
            }
            else
            {
                insertcmd.Parameters.AddWithValue("@MEB", txtMEB.Text);

            }
            if (string.IsNullOrEmpty(txtMBD.Text))
            {
                insertcmd.Parameters.AddWithValue("@MBD", "0");
            }
            else
            {
                insertcmd.Parameters.AddWithValue("@MBD", txtMBD.Text);
            }
            insertcmd.Parameters.AddWithValue("@startdate", Convert.ToDateTime(txtstartdate.Text));
            insertcmd.Parameters.AddWithValue("@enddate", Convert.ToDateTime(txtenddate.Text));


            try
            {
                insertcmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                insertcmd.Dispose();
            }

            ///VIP Promo Price

            SqlCommand VIPinsertcmd = new SqlCommand("update MF_AgentPromo set discount_Amt=@Discount_Amt,discount_type=@Discount_Type,EarnRPWhenBuy=@EarnRPWhenBuy,RedemptionPoint=@RedemptionPoint,promo_price=@Promo_Price,Cashbackamt=@CashBackAmt,SPP=@SPP,ReferrerProfitBonusPercent=@ReferrerProfitBonusPercent,TeamKPIBonus=@TeamKPIBonus,TeamKPIBonusMS=@TeamKPIBonusMS,MEB=@MEB,MBD=@MBD,modified_dt=dateadd(hour,(8),CONVERT([varchar](20),getutcdate(),(120))) where promoid=@promoid and item_code=@ItemCode and AgentLevelCode=@AgentLevelCode and deleteind <> 'X'", con);

            VIPinsertcmd.Parameters.AddWithValue("@ItemCode", lblitem_code.Text);
            VIPinsertcmd.Parameters.AddWithValue("@AgentLevelCode", "D01");
            if (string.IsNullOrEmpty(lblVpromoPrice.Text))
            {
                VIPinsertcmd.Parameters.AddWithValue("@Promo_Price", "0");
            }
            else
            {
                VIPinsertcmd.Parameters.AddWithValue("@Promo_Price", lblVpromoPrice.Text);
            }
            if (string.IsNullOrEmpty(txtVDiscountAmt.Text))
            {
                VIPinsertcmd.Parameters.AddWithValue("@Discount_Amt", "0");
            }
            else
            {
                VIPinsertcmd.Parameters.AddWithValue("@Discount_Amt", txtVDiscountAmt.Text);
            }
            VIPinsertcmd.Parameters.AddWithValue("@Discount_Type", ddlVdiscountType.SelectedValue);
            VIPinsertcmd.Parameters.AddWithValue("@promoid", Request.QueryString["groupid"].ToString());
            if (string.IsNullOrEmpty(txtVpromocashback.Text))
            {
                VIPinsertcmd.Parameters.AddWithValue("@CashBackAmt", "0");
            }
            else
            {
                VIPinsertcmd.Parameters.AddWithValue("@CashBackAmt", txtVpromocashback.Text);
            }
            VIPinsertcmd.Parameters.AddWithValue("@EarnRPWhenBuy", txtVIPrewardpoint.Text);
            VIPinsertcmd.Parameters.AddWithValue("@RedemptionPoint", txtVIPredemptpoint.Text);
            if (string.IsNullOrEmpty(txtSPP.Text))
            {
                VIPinsertcmd.Parameters.AddWithValue("@SPP", "0");
            }
            else
            {
                VIPinsertcmd.Parameters.AddWithValue("@SPP", txtSPP.Text);
            }
            if (string.IsNullOrEmpty(txtRefProfitBonusPercent.Text))
            {
                VIPinsertcmd.Parameters.AddWithValue("@ReferrerProfitBonusPercent", "0");
            }
            else
            {
                VIPinsertcmd.Parameters.AddWithValue("@ReferrerProfitBonusPercent", txtRefProfitBonusPercent.Text);
            }
            if (string.IsNullOrEmpty(txtTeamKPI.Text))
            {
                VIPinsertcmd.Parameters.AddWithValue("@TeamKPIBonus", "0");
            }
            else
            {
                VIPinsertcmd.Parameters.AddWithValue("@TeamKPIBonus", txtTeamKPI.Text);
            }
            if (string.IsNullOrEmpty(txtTeamKPI2.Text))
            {
                VIPinsertcmd.Parameters.AddWithValue("@TeamKPIBonusMS", "0");
            }
            else
            {
                VIPinsertcmd.Parameters.AddWithValue("@TeamKPIBonusMS", txtTeamKPI2.Text);
            }
            if (string.IsNullOrEmpty(txtMEB.Text))
            {
                VIPinsertcmd.Parameters.AddWithValue("@MEB", "0");
            }
            else
            {
                VIPinsertcmd.Parameters.AddWithValue("@MEB", txtMEB.Text);

            }
            if (string.IsNullOrEmpty(txtMBD.Text))
            {
                VIPinsertcmd.Parameters.AddWithValue("@MBD", "0");
            }
            else
            {
                VIPinsertcmd.Parameters.AddWithValue("@MBD", txtMBD.Text);
            }
            VIPinsertcmd.Parameters.AddWithValue("@startdate", Convert.ToDateTime(txtstartdate.Text));
            VIPinsertcmd.Parameters.AddWithValue("@enddate", Convert.ToDateTime(txtenddate.Text));


            try
            {
                VIPinsertcmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                VIPinsertcmd.Dispose();
            }


            ///VVIP Promo Price

            SqlCommand VVIPinsertcmd = new SqlCommand("update MF_AgentPromo set discount_Amt=@Discount_Amt,discount_type=@Discount_Type,EarnRPWhenBuy=@EarnRPWhenBuy,RedemptionPoint=@RedemptionPoint,promo_price=@Promo_Price,Cashbackamt=@CashBackAmt,SPP=@SPP,ReferrerProfitBonusPercent=@ReferrerProfitBonusPercent,TeamKPIBonus=@TeamKPIBonus,TeamKPIBonusMS=@TeamKPIBonusMS,MEB=@MEB,MBD=@MBD,modified_dt=dateadd(hour,(8),CONVERT([varchar](20),getutcdate(),(120))) where promoid=@promoid and item_code=@ItemCode and AgentLevelCode=@AgentLevelCode and deleteind <> 'X'", con);

            VVIPinsertcmd.Parameters.AddWithValue("@ItemCode", lblitem_code.Text);
            VVIPinsertcmd.Parameters.AddWithValue("@AgentLevelCode", "D04");
            if (string.IsNullOrEmpty(lblVVIPpromoPrice.Text))
            {
                VVIPinsertcmd.Parameters.AddWithValue("@Promo_Price", "0");
            }
            else
            {
                VVIPinsertcmd.Parameters.AddWithValue("@Promo_Price", lblVVIPpromoPrice.Text);
            }
            if (string.IsNullOrEmpty(txtVVIPDiscountAmt.Text))
            {
                VVIPinsertcmd.Parameters.AddWithValue("@Discount_Amt", "0");
            }
            else
            {
                VVIPinsertcmd.Parameters.AddWithValue("@Discount_Amt", txtVVIPDiscountAmt.Text);
            }
            VVIPinsertcmd.Parameters.AddWithValue("@Discount_Type", ddlVVIPdiscountType.SelectedValue);
            VVIPinsertcmd.Parameters.AddWithValue("@promoid", Request.QueryString["groupid"].ToString());
            if (string.IsNullOrEmpty(txtVVIPpromocashback.Text))
            {
                VVIPinsertcmd.Parameters.AddWithValue("@CashBackAmt", "0");
            }
            else
            {
                VVIPinsertcmd.Parameters.AddWithValue("@CashBackAmt", txtVVIPpromocashback.Text);
            }
            VVIPinsertcmd.Parameters.AddWithValue("@EarnRPWhenBuy", txtVVIPrewardpoint.Text);
            VVIPinsertcmd.Parameters.AddWithValue("@RedemptionPoint", txtVVIPredemptpoint.Text);
            if (string.IsNullOrEmpty(txtSPP.Text))
            {
                VVIPinsertcmd.Parameters.AddWithValue("@SPP", "0");
            }
            else
            {
                VVIPinsertcmd.Parameters.AddWithValue("@SPP", txtSPP.Text);
            }
            if (string.IsNullOrEmpty(txtRefProfitBonusPercent.Text))
            {
                VVIPinsertcmd.Parameters.AddWithValue("@ReferrerProfitBonusPercent", "0");
            }
            else
            {
                VVIPinsertcmd.Parameters.AddWithValue("@ReferrerProfitBonusPercent", txtRefProfitBonusPercent.Text);
            }
            if (string.IsNullOrEmpty(txtTeamKPI.Text))
            {
                VVIPinsertcmd.Parameters.AddWithValue("@TeamKPIBonus", "0");
            }
            else
            {
                VVIPinsertcmd.Parameters.AddWithValue("@TeamKPIBonus", txtTeamKPI.Text);
            }
            if (string.IsNullOrEmpty(txtTeamKPI2.Text))
            {
                VVIPinsertcmd.Parameters.AddWithValue("@TeamKPIBonusMS", "0");
            }
            else
            {
                VVIPinsertcmd.Parameters.AddWithValue("@TeamKPIBonusMS", txtTeamKPI2.Text);
            }
            if (string.IsNullOrEmpty(txtMEB.Text))
            {
                VVIPinsertcmd.Parameters.AddWithValue("@MEB", "0");
            }
            else
            {
                VVIPinsertcmd.Parameters.AddWithValue("@MEB", txtMEB.Text);

            }
            if (string.IsNullOrEmpty(txtMBD.Text))
            {
                VVIPinsertcmd.Parameters.AddWithValue("@MBD", "0");
            }
            else
            {
                VVIPinsertcmd.Parameters.AddWithValue("@MBD", txtMBD.Text);
            }
            VVIPinsertcmd.Parameters.AddWithValue("@startdate", Convert.ToDateTime(txtstartdate.Text));
            VVIPinsertcmd.Parameters.AddWithValue("@enddate", Convert.ToDateTime(txtenddate.Text));


            try
            {
                VVIPinsertcmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                VVIPinsertcmd.Dispose();
            }

            ///Stockist Promo Price

            SqlCommand Stockistinsertcmd = new SqlCommand("update MF_AgentPromo set discount_Amt=@Discount_Amt,discount_type=@Discount_Type,EarnRPWhenBuy=@EarnRPWhenBuy,RedemptionPoint=@RedemptionPoint,promo_price=@Promo_Price,Cashbackamt=@CashBackAmt,SPP=@SPP,ReferrerProfitBonusPercent=@ReferrerProfitBonusPercent,TeamKPIBonus=@TeamKPIBonus,TeamKPIBonusMS=@TeamKPIBonusMS,MEB=@MEB,MBD=@MBD,modified_dt=dateadd(hour,(8),CONVERT([varchar](20),getutcdate(),(120))) where promoid=@promoid and item_code=@ItemCode and AgentLevelCode=@AgentLevelCode and deleteind <> 'X'", con);

            Stockistinsertcmd.Parameters.AddWithValue("@ItemCode", lblitem_code.Text);
            Stockistinsertcmd.Parameters.AddWithValue("@AgentLevelCode", "D03");
            if (string.IsNullOrEmpty(lblSpromoPrice.Text))
            {
                Stockistinsertcmd.Parameters.AddWithValue("@Promo_Price", "0");
            }
            else
            {
                Stockistinsertcmd.Parameters.AddWithValue("@Promo_Price", lblSpromoPrice.Text);
            }
            if (string.IsNullOrEmpty(txtSDiscountAmt.Text))
            {
                Stockistinsertcmd.Parameters.AddWithValue("@Discount_Amt", "0");
            }
            else
            {
                Stockistinsertcmd.Parameters.AddWithValue("@Discount_Amt", txtSDiscountAmt.Text);
            }
            Stockistinsertcmd.Parameters.AddWithValue("@Discount_Type", ddlSdiscountType.SelectedValue);
            Stockistinsertcmd.Parameters.AddWithValue("@promoid", Request.QueryString["groupid"].ToString());
            if (string.IsNullOrEmpty(txtSpromocashback.Text))
            {
                Stockistinsertcmd.Parameters.AddWithValue("@CashBackAmt", "0");
            }
            else
            {
                Stockistinsertcmd.Parameters.AddWithValue("@CashBackAmt", txtSpromocashback.Text);
            }
            Stockistinsertcmd.Parameters.AddWithValue("@EarnRPWhenBuy", txtSrewardpoint.Text);
            Stockistinsertcmd.Parameters.AddWithValue("@RedemptionPoint", txtSredemptpoint.Text);
            if (string.IsNullOrEmpty(txtSPP.Text))
            {
                Stockistinsertcmd.Parameters.AddWithValue("@SPP", "0");
            }
            else
            {
                Stockistinsertcmd.Parameters.AddWithValue("@SPP", txtSPP.Text);
            }
            if (string.IsNullOrEmpty(txtRefProfitBonusPercent.Text))
            {
                Stockistinsertcmd.Parameters.AddWithValue("@ReferrerProfitBonusPercent", "0");
            }
            else
            {
                Stockistinsertcmd.Parameters.AddWithValue("@ReferrerProfitBonusPercent", txtRefProfitBonusPercent.Text);
            }
            if (string.IsNullOrEmpty(txtTeamKPI.Text))
            {
                Stockistinsertcmd.Parameters.AddWithValue("@TeamKPIBonus", "0");
            }
            else
            {
                Stockistinsertcmd.Parameters.AddWithValue("@TeamKPIBonus", txtTeamKPI.Text);
            }
            if (string.IsNullOrEmpty(txtTeamKPI2.Text))
            {
                Stockistinsertcmd.Parameters.AddWithValue("@TeamKPIBonusMS", "0");
            }
            else
            {
                Stockistinsertcmd.Parameters.AddWithValue("@TeamKPIBonusMS", txtTeamKPI2.Text);
            }
            if (string.IsNullOrEmpty(txtMEB.Text))
            {
                Stockistinsertcmd.Parameters.AddWithValue("@MEB", "0");
            }
            else
            {
                Stockistinsertcmd.Parameters.AddWithValue("@MEB", txtMEB.Text);

            }
            if (string.IsNullOrEmpty(txtMBD.Text))
            {
                Stockistinsertcmd.Parameters.AddWithValue("@MBD", "0");
            }
            else
            {
                Stockistinsertcmd.Parameters.AddWithValue("@MBD", txtMBD.Text);
            }
            Stockistinsertcmd.Parameters.AddWithValue("@startdate", Convert.ToDateTime(txtstartdate.Text));
            Stockistinsertcmd.Parameters.AddWithValue("@enddate", Convert.ToDateTime(txtenddate.Text));


            try
            {
                Stockistinsertcmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                Stockistinsertcmd.Dispose();
            }

            ///Master Stockist Promo Price

            SqlCommand MStockistinsertcmd = new SqlCommand("update MF_AgentPromo set discount_Amt=@Discount_Amt,discount_type=@Discount_Type,EarnRPWhenBuy=@EarnRPWhenBuy,RedemptionPoint=@RedemptionPoint,promo_price=@Promo_Price,Cashbackamt=@CashBackAmt,SPP=@SPP,ReferrerProfitBonusPercent=@ReferrerProfitBonusPercent,TeamKPIBonus=@TeamKPIBonus,TeamKPIBonusMS=@TeamKPIBonusMS,MEB=@MEB,MBD=@MBD,modified_dt=dateadd(hour,(8),CONVERT([varchar](20),getutcdate(),(120))) where promoid=@promoid and item_code=@ItemCode and AgentLevelCode=@AgentLevelCode and deleteind <> 'X'", con);

            MStockistinsertcmd.Parameters.AddWithValue("@ItemCode", lblitem_code.Text);
            MStockistinsertcmd.Parameters.AddWithValue("@AgentLevelCode", "D05");
            if (string.IsNullOrEmpty(lblMSpromoPrice.Text))
            {
                MStockistinsertcmd.Parameters.AddWithValue("@Promo_Price", "0");
            }
            else
            {
                MStockistinsertcmd.Parameters.AddWithValue("@Promo_Price", lblMSpromoPrice.Text);
            }
            if (string.IsNullOrEmpty(txtMSDiscountAmt.Text))
            {
                MStockistinsertcmd.Parameters.AddWithValue("@Discount_Amt", "0");
            }
            else
            {
                MStockistinsertcmd.Parameters.AddWithValue("@Discount_Amt", txtMSDiscountAmt.Text);
            }
            MStockistinsertcmd.Parameters.AddWithValue("@Discount_Type", ddlMSdiscountType.SelectedValue);
            MStockistinsertcmd.Parameters.AddWithValue("@promoid", Request.QueryString["groupid"].ToString());
            if (string.IsNullOrEmpty(txtMSpromocashback.Text))
            {
                MStockistinsertcmd.Parameters.AddWithValue("@CashBackAmt", "0");
            }
            else
            {
                MStockistinsertcmd.Parameters.AddWithValue("@CashBackAmt", txtMSpromocashback.Text);
            }
            MStockistinsertcmd.Parameters.AddWithValue("@EarnRPWhenBuy", txtMSrewardpoint.Text);
            MStockistinsertcmd.Parameters.AddWithValue("@RedemptionPoint", txtMSredemptpoint.Text);
            if (string.IsNullOrEmpty(txtSPP.Text))
            {
                MStockistinsertcmd.Parameters.AddWithValue("@SPP", "0");
            }
            else
            {
                MStockistinsertcmd.Parameters.AddWithValue("@SPP", txtSPP.Text);
            }
            if (string.IsNullOrEmpty(txtRefProfitBonusPercent.Text))
            {
                MStockistinsertcmd.Parameters.AddWithValue("@ReferrerProfitBonusPercent", "0");
            }
            else
            {
                MStockistinsertcmd.Parameters.AddWithValue("@ReferrerProfitBonusPercent", txtRefProfitBonusPercent.Text);
            }
            if (string.IsNullOrEmpty(txtTeamKPI.Text))
            {
                MStockistinsertcmd.Parameters.AddWithValue("@TeamKPIBonus", "0");
            }
            else
            {
                MStockistinsertcmd.Parameters.AddWithValue("@TeamKPIBonus", txtTeamKPI.Text);
            }
            if (string.IsNullOrEmpty(txtTeamKPI2.Text))
            {
                MStockistinsertcmd.Parameters.AddWithValue("@TeamKPIBonusMS", "0");
            }
            else
            {
                MStockistinsertcmd.Parameters.AddWithValue("@TeamKPIBonusMS", txtTeamKPI2.Text);
            }
            if (string.IsNullOrEmpty(txtMEB.Text))
            {
                MStockistinsertcmd.Parameters.AddWithValue("@MEB", "0");
            }
            else
            {
                MStockistinsertcmd.Parameters.AddWithValue("@MEB", txtMEB.Text);

            }
            if (string.IsNullOrEmpty(txtMBD.Text))
            {
                MStockistinsertcmd.Parameters.AddWithValue("@MBD", "0");
            }
            else
            {
                MStockistinsertcmd.Parameters.AddWithValue("@MBD", txtMBD.Text);
            }
            MStockistinsertcmd.Parameters.AddWithValue("@startdate", Convert.ToDateTime(txtstartdate.Text));
            MStockistinsertcmd.Parameters.AddWithValue("@enddate", Convert.ToDateTime(txtenddate.Text));


            try
            {
                MStockistinsertcmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                MStockistinsertcmd.Dispose();
            }


            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "$('#ItemAddModal').modal('hide');$('body').removeClass('modal-open');$('.modal-backdrop').remove();", true);



            showgroupitemlist();

            updateee.Update();
            con.Close();
        }
    }
}