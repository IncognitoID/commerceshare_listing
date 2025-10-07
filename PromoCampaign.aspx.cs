using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PromoCampaign : System.Web.UI.Page
{
    protected static String DBCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            MF_Con.Text = DBCon;
            GroupLoadList(1);
        }
    }

    private void GroupLoadList(int pageIndex)
    {
        using (SqlConnection con = new SqlConnection(MF_Con.Text))
        {
            using (SqlCommand cmd = new SqlCommand("PromoCampaign_Listing", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PageIndex", pageIndex);
                cmd.Parameters.AddWithValue("@PageSize", int.Parse("10"));
                cmd.Parameters.AddWithValue("@merchant", Request.QueryString["merchant"].ToString());
                if (txt_Search.Text.Trim() == "")
                {
                    cmd.Parameters.AddWithValue("@FilterText", "%%");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@FilterText", "%" + txt_Search.Text.Trim().Replace("'", "`") + "%");
                }

                cmd.Parameters.Add("@RecordCount", SqlDbType.Int, 4);
                cmd.Parameters["@RecordCount"].Direction = ParameterDirection.Output;
                con.Open();
                SqlDataReader idr = cmd.ExecuteReader();

                if (idr.HasRows == false)
                {
                    DataTable v = new DataTable();

                    v.Load(idr);
                    v.Rows.Add(v.NewRow());

                    grd_View.DataSource = v;
                    grd_View.DataBind();
                    int columnCount = grd_View.Rows[0].Cells.Count;

                    grd_View.Rows[0].Cells.Clear();
                    grd_View.Rows[0].Cells.Add(new TableCell());
                    grd_View.Rows[0].Cells[0].ColumnSpan = columnCount;
                    grd_View.Rows[0].Cells[0].Text = "No Record Found.";

                    lbl_record3.Text = v.Rows.Count.ToString();

                    //grd_View.HeaderRow.Cells[0].Attributes["data-class"] = "expand";
                    grd_View.HeaderRow.Cells[2].Attributes["data-hide"] = "phone";
                    grd_View.HeaderRow.Cells[3].Attributes["data-hide"] = "phone";

                    grd_View.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
                else
                {
                    DataTable v = new DataTable();

                    v.Load(idr);

                    grd_View.DataSource = v;
                    grd_View.DataBind();
                    lbl_record3.Text = v.Rows.Count.ToString();
                    // grd_View.HeaderRow.Cells[0].Attributes["data-class"] = "expand";
                    grd_View.HeaderRow.Cells[2].Attributes["data-hide"] = "phone";
                    grd_View.HeaderRow.Cells[3].Attributes["data-hide"] = "phone";

                    grd_View.HeaderRow.TableSection = TableRowSection.TableHeader;
                }

                idr.Close();
                con.Close();
                int recordCount = Convert.ToInt32(cmd.Parameters["@RecordCount"].Value);

                lbl_Record2.Text = recordCount.ToString();

                if (recordCount == 0)
                {
                    recordCount = 1;
                }

                this.PopulatePager(recordCount, pageIndex);
            }
        }
    }

    private void PopulatePager(int recordCount, int currentPage)
    {
        ddlPager.Items.Clear();

        double dblPageCount = (double)((decimal)recordCount / decimal.Parse("10"));
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
            GroupLoadList(1);
        }
    }



    protected void ddlPager_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.GroupLoadList(Convert.ToInt32(ddlPager.SelectedValue));
    }

    protected void txt_Search_TextChanged(object sender, EventArgs e)
    {
        this.GroupLoadList(Convert.ToInt32(ddlPager.SelectedValue));
    }


    protected void btn_add_Click(object sender, EventArgs e)
    {
        Int32 rv = 1;
        using (SqlConnection myTxConn = new SqlConnection(DBCon))
        {
            using (SqlCommand myTxcmd = new SqlCommand("Check_CampaignTitle", myTxConn))
            {
                myTxcmd.CommandType = CommandType.StoredProcedure;
                myTxcmd.Parameters.AddWithValue("@CampaignTitle", txt_Campaigntitle.Value);
                myTxcmd.Parameters.AddWithValue("@MerchantID", Request.QueryString["merchant"].ToString());
                SqlParameter SReturnValue = myTxcmd.Parameters.Add("returnValue", SqlDbType.Int, 4);
                SReturnValue.Direction = ParameterDirection.ReturnValue;

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

                rv = Convert.ToInt32(SReturnValue.Value.ToString());
            }
        }

        if (rv == 1)
        {
            txt_Campaigntitle.Value = String.Empty;
            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Group title already existed.');", true);
        }
        else
        {
            using (SqlConnection myTxConn = new SqlConnection(DBCon))
            {

                //SqlCommand checkitem1cmd = new SqlCommand("select * from MF_PromoCampaign where MerchantID='" + Request.QueryString["merchant"].ToString() + "' and start_date >= '" + txtstartDate.Text + "' and end_date <= '" + txtendDate.Text + "' and deleteind <> 'X' ", myTxConn);
                //SqlDataAdapter checkitem1adp = new SqlDataAdapter(checkitem1cmd);
                //DataTable checkitem1dt = new DataTable();
                //checkitem1adp.Fill(checkitem1dt);

                //if (checkitem1dt.Rows.Count <= 0)
                //{
                //    SqlCommand checkitem2cmd = new SqlCommand("select * from MF_PromoCampaign where MerchantID='" + Request.QueryString["merchant"].ToString() + "' and '" + txtstartDate.Text + "' >= start_date and '" + txtendDate.Text + "' <= end_date and deleteind <> 'X'", myTxConn);
                //    SqlDataAdapter checkitem2adp = new SqlDataAdapter(checkitem2cmd);
                //    DataTable checkitem2dt = new DataTable();
                //    checkitem2adp.Fill(checkitem2dt);

                //    if (checkitem2dt.Rows.Count <= 0)
                //    {
                //        SqlCommand checkitem3cmd = new SqlCommand("select * from MF_PromoCampaign where MerchantID='" + Request.QueryString["merchant"].ToString() + "' and start_date >= '" + txtstartDate.Text + "' and start_date <= '" + txtendDate.Text + "' and deleteind <> 'X'", myTxConn);
                //        SqlDataAdapter checkitem3adp = new SqlDataAdapter(checkitem3cmd);
                //        DataTable checkitem3dt = new DataTable();
                //        checkitem3adp.Fill(checkitem3dt);

                //        if (checkitem3dt.Rows.Count <= 0)
                //        {
                //            SqlCommand checkitem4cmd = new SqlCommand("select * from MF_PromoCampaign where MerchantID='" + Request.QueryString["merchant"].ToString() + "' and end_date >= '" + txtstartDate.Text + "' and end_date <= '" + txtendDate.Text + "' and deleteind <> 'X'", myTxConn);
                //            SqlDataAdapter checkitem4adp = new SqlDataAdapter(checkitem4cmd);
                //            DataTable checkitem4dt = new DataTable();
                //            checkitem4adp.Fill(checkitem4dt);

                //            if (checkitem4dt.Rows.Count <= 0)
                //            {
                //            }
                //            else
                //            {
                //                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Date range already existed.')", true);
                //            }
                //        }
                //        else
                //        {
                //            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Date range already existed.')", true);
                //        }
                //    }
                //    else
                //    {
                //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Date range already existed.')", true);

                //    }
                //}
                //else
                //{
                //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Date range already existed.')", true);

                //}
                using (SqlCommand myTxcmd = new SqlCommand("Insert_PromoCampaign", myTxConn))
                {
                    myTxcmd.CommandType = CommandType.StoredProcedure;
                    myTxcmd.Parameters.AddWithValue("@grouptitle", txt_Campaigntitle.Value.Trim().Replace("'", "`"));
                    myTxcmd.Parameters.AddWithValue("@MerchantID", Request.QueryString["merchant"].ToString());
                    myTxcmd.Parameters.AddWithValue("@startdate", Convert.ToDateTime(txtstartDate.Text));
                    myTxcmd.Parameters.AddWithValue("@enddate", Convert.ToDateTime(txtendDate.Text));

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

                }

                txt_Campaigntitle.Value = String.Empty;

                Session["PName"] = " Listing";
                GroupLoadList(1);
                ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Insert success.');$('#GroupModal').modal('hide');$('body').removeClass('modal-open');$('.modal-backdrop').remove();", true);

            }
        }
    }

    protected void View_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Update")
        {
            Response.Redirect("PromoItemSetup.aspx?username=" + Request.QueryString["username"] + "&groupid=" + e.CommandArgument.ToString() + "&merchant=" + Request.QueryString["merchant"] );
        }
        if (e.CommandName == "delete")
        {
            using (SqlConnection con = new SqlConnection(DBCon))
            {
                using (SqlCommand cmd = new SqlCommand("select * from MF_GroupingItem where groupid='" + e.CommandArgument.ToString() + "' and deleteind <> 'X'", con))
                {
                    con.Open();
                    SqlDataReader idr = cmd.ExecuteReader();

                    if (idr.HasRows == false)
                    {
                        using (SqlConnection dcon = new SqlConnection(DBCon))
                        {
                            dcon.Open();
                            using (SqlCommand dcmd = new SqlCommand("update MF_ItemGroupingList set deleteind = 'X' where ids = '" + e.CommandArgument.ToString() + "' and deleteind <> 'X'", dcon))
                            {
                                dcmd.ExecuteNonQuery();
                                ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Delete success.');window.location.href='ItemGroupingListing.aspx?merchant=" + Request.QueryString["merchant"]  + "'", true);
                            }
                            dcon.Close();
                        }
                    }
                    else
                    {
                        using (SqlConnection dcon = new SqlConnection(DBCon))
                        {
                            dcon.Open();
                            using (SqlCommand dcmd1 = new SqlCommand("update MF_GroupingItem set deleteind = 'X' where groupid = '" + e.CommandArgument.ToString() + "' and deleteind <> 'X'", dcon))
                            {
                                dcmd1.ExecuteNonQuery();
                                //ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Delete success.');;window.location.href='GroupItemListing.aspx?username=" + Request.QueryString["username"] + "'", true);
                            }

                            using (SqlCommand dcmd = new SqlCommand("update MF_ItemGroupingList set deleteind = 'X' where ids = '" + e.CommandArgument.ToString() + "' and deleteind <> 'X'", dcon))
                            {
                                dcmd.ExecuteNonQuery();

                            }
                            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Delete success.');window.location.href='ItemGroupingListing.aspx?merchant=" + Request.QueryString["merchant"] + "'", true);
                            dcon.Close();
                        }
                    }

                    idr.Close();
                    con.Close();
                }
            }
            GroupLoadList(1);
        }
    }

    protected void grp_grdview_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }


    protected void btn_New_Click(object sender, EventArgs e)
    {

    }

    protected void grd_View_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
}