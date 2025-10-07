using DocumentFormat.OpenXml.Office2010.Excel;
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
public partial class ItemGroupingSetup : System.Web.UI.Page
{
    protected static String DBCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            MF_Con.Text = DBCon;
            SqlConnection con = new SqlConnection(MF_Con.Text);

            if (Request.QueryString["groupid"].ToString() != "")
            {
                showgroupitemlist();
                showitemlist();


                using (SqlCommand myTxcmd = new SqlCommand("load_groupinfo", con))
                {
                    myTxcmd.CommandType = CommandType.StoredProcedure;
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
                            groupingtitle.InnerHtml = dt.Rows[0]["grouptitle"].ToString();
                            txtgrouptitle.Text = dt.Rows[0]["grouptitle"].ToString();

                            if (dt.Rows[0]["banner_path"].ToString() != "")
                            {
                                Img2.Src = dt.Rows[0]["banner_path"].ToString();
                            }
                            else
                            {
                                Img2.Src = "Images/1080x409.png";
                            }

                            if (dt.Rows[0]["icon_path"].ToString() != "")
                            {
                                Img1.Src = dt.Rows[0]["icon_path"].ToString();
                            }
                            else
                            {
                                Img1.Src = "Images/55x55.png";
                            }
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
            using (SqlCommand cmd = new SqlCommand("select a.ids,a.ItemCode, c.LongDesc, a.Fdate,a.Tdate, c.filepath,a.Status,c.ShortDesc,a.daterangeallow from MF_GroupingItem as a left join " +
                            "MF_Item as c on c.Item_Code = a.ItemCode where " +
                            "a.MerchantID = '" + Request.QueryString["merchant"].ToString() + "' and " +
                            "a.GroupID = '" + Request.QueryString["groupid"].ToString() + "' and a.Deleteind <> 'X' and c.DeleteInd <> 'X'  order by Status asc , daterangeallow asc , coalesce(case when a.tdate < dateadd(hour,(8),CONVERT([varchar](20),getutcdate(),(120))) then a.tdate end, a.fdate) desc ", con))
            {
                con.Open();
                SqlDataReader idr = cmd.ExecuteReader();

                if (idr.HasRows == false)
                {
                    DataTable dt = new DataTable();

                    dt.Load(idr);

                    grpitem_grdview.DataSource = dt;
                    grpitem_grdview.DataBind();



                }
                else
                {
                    DataTable dt = new DataTable();

                    dt.Load(idr);
                    grpitem_grdview.DataSource = dt;
                    grpitem_grdview.DataBind();
                    grpitem_grdview.HeaderRow.TableSection = TableRowSection.TableHeader;
                }

                idr.Close();
                con.Close();
            }
        }
    }

    protected void grpitem_grdview_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "ItemEdit")
        {
            using (SqlConnection con = new SqlConnection(DBCon))
            {
                using (SqlCommand cmd = new SqlCommand("select a.ids,a.ItemCode, c.LongDesc, a.Fdate,a.Tdate, c.filepath,a.Status,c.ShortDesc,a.daterangeallow from MF_GroupingItem as a left join " +
                                "MF_Item as c on c.Item_Code = a.ItemCode where " +
                                "a.MerchantID = '" + Request.QueryString["merchant"].ToString() + "' and " +
                                "a.GroupID = '" + Request.QueryString["groupid"].ToString() + "' and a.ids='"+ e.CommandArgument.ToString() +"' and a.Deleteind <> 'X' and c.DeleteInd <> 'X' ", con))
                {
                    con.Open();
                    SqlDataAdapter chkitemadp = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    chkitemadp.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        lblitem_code.Text = dt.Rows[0]["ItemCode"].ToString();
                        lblitem_name.Text = dt.Rows[0]["longdesc"].ToString();
                        ddlShownstatus.SelectedValue = dt.Rows[0]["daterangeallow"].ToString();
                        ddlstatusEdit.SelectedValue = dt.Rows[0]["Status"].ToString();
                        if (dt.Rows[0]["Fdate"].ToString() != "")
                        {
                            txtstartdateEdit.Text= Convert.ToDateTime(dt.Rows[0]["Fdate"]).ToString("yyyy-MM-dd");
                        }
                        if (dt.Rows[0]["Tdate"].ToString() != "")
                        {
                            txtenddateEdit.Text= Convert.ToDateTime(dt.Rows[0]["Tdate"]).ToString("yyyy-MM-dd");
                        }
                        hditemids.Value = e.CommandArgument.ToString();

                        if (dt.Rows[0]["filepath"] != DBNull.Value)
                        {
                            btnimage.ImageUrl = dt.Rows[0]["filepath"].ToString();
                        }
                        else
                        {
                            btnimage.ImageUrl = "Images/NoPic.png";
                        }

                        if (ddlShownstatus.SelectedValue == "Date Range")
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

                    ScriptManager.RegisterStartupScript(this, GetType(), "pop", "$('#GroupItemEditModal').modal('show');", true);

                    con.Close();
                }
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
            "NOT EXISTS(select itemcode from MF_GroupingItem as a where a.ItemCode = b.Item_Code and " +
            "GroupID = '" + Request.QueryString["groupid"].ToString() + "' and Deleteind <> 'x') and ( b.item_code like '%" + txt_search.Text + "%' or b.longdesc like '%" + txt_search.Text + "%') and b.supplier_code='" + Request.QueryString["merchant"].ToString() + "' and b.deleteind <> 'X' order by created_dt desc", con))
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
        if (e.CommandName == "addtolist")
        {
            using (SqlConnection con = new SqlConnection(MF_Con.Text))
            {
                using (SqlCommand cmd = new SqlCommand("select * from mf_item where deleteind <> 'X' and item_code='" + e.CommandArgument.ToString() + "'", con))
                {
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adp.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {

                    }
                }
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "pop", "$('#GroupModal').modal('show');$('#ItemAddModal').modal('show');", true);

        }

    }

    protected void btnaddtlist_Click(object sender, EventArgs e)
    {
        SqlDateTime sqldatenull;
        foreach (GridViewRow row in item_grdview.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkEmployee = (CheckBox)row.FindControl("chkitem");
                if (chkEmployee.Checked)
                {
                    Label lblitemcode = (Label)row.FindControl("lblitemcode");
                    TextBox txtstartdate = (TextBox)row.FindControl("txtstartdate");
                    TextBox txtenddate = (TextBox)row.FindControl("txtenddate");
                    DropDownList ddlstatus = (DropDownList)row.FindControl("ddlstatus");
                    DropDownList daterangeallow = (DropDownList)row.FindControl("ddlShownstatus");
                    sqldatenull = SqlDateTime.Null;
                    using (SqlConnection con = new SqlConnection(DBCon))
                    {
                        using (SqlCommand cmd = new SqlCommand("INSERT INTO MF_GroupingItem(itemcode,groupid,fdate,tdate,status,merchantid,created_dt,modified_dt,deleteind,daterangeallow) VALUES(@itemcode,@groupid,@startdate,@enddate,@status,@merchant,dateadd(hour,(8),CONVERT([varchar](20),getutcdate(),(120))),dateadd(hour,(8),CONVERT([varchar](20),getutcdate(),(120))),'',@daterangeallow)", con))
                        {
                            cmd.Parameters.AddWithValue("@groupid", Request.QueryString["groupid"].ToString());
                            cmd.Parameters.AddWithValue("@itemcode", lblitemcode.Text.Trim());
                            cmd.Parameters.AddWithValue("@daterangeallow", daterangeallow.SelectedValue);
                            
                            if (daterangeallow.SelectedValue == "Date Range")
                            {
                                cmd.Parameters.AddWithValue("@startdate", txtstartdate.Text);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@startdate", sqldatenull);
                            }
                            if (daterangeallow.SelectedValue == "Date Range")
                            {
                                cmd.Parameters.AddWithValue("@enddate", txtenddate.Text);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@enddate", sqldatenull);
                            }

                            cmd.Parameters.AddWithValue("@status", ddlstatus.Text.Trim());
                            cmd.Parameters.AddWithValue("@merchant", Request.QueryString["merchant"].ToString());
                            con.Open();
                            cmd.ExecuteNonQuery();
                            
                            
                            con.Close();
                        }
                    }
                }
            }
        }
        showitemlist();
        showgroupitemlist();
        updateee.Update();
        ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "$('#GroupModal').modal('hide');$('body').removeClass('modal-open');$('.modal-backdrop').remove();", true);
        
    }

    protected void grpitem_grdview_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        SqlDateTime sqldatenull;
        GridView grid = (GridView)sender;
        GridViewRow row = grid.Rows[e.RowIndex];
        string id = grid.DataKeys[e.RowIndex].Value.ToString();
        TextBox txtstartdateEdit = row.FindControl("txtstartdateEdit") as TextBox;
        TextBox txtenddateEdit = row.FindControl("txtenddateEdit") as TextBox;
        DropDownList ddlstatusEdit = row.FindControl("ddlstatusEdit") as DropDownList;
        DropDownList ddlShownstatus = (DropDownList)row.FindControl("ddlShownstatus");
        using (SqlConnection con = new SqlConnection(MF_Con.Text))
        {
            con.Open();
            sqldatenull = SqlDateTime.Null;
            using (SqlCommand checkcmd = new SqlCommand("update MF_GroupingItem set fdate=@fdate,tdate=@tdate,status =@status,daterangeallow=@daterangeallow where ids='" + id + "'", con))
            {
                checkcmd.Parameters.AddWithValue("@daterangeallow", ddlShownstatus.SelectedValue);
                if (ddlShownstatus.SelectedValue == "Date Range")
                {
                    checkcmd.Parameters.AddWithValue("@fdate", txtstartdateEdit.Text);
                }
                else
                {
                    checkcmd.Parameters.AddWithValue("@fdate", sqldatenull);
                }
                if (ddlShownstatus.SelectedValue == "Date Range")
                {
                    checkcmd.Parameters.AddWithValue("@tdate", txtenddateEdit.Text);
                }
                else
                {
                    checkcmd.Parameters.AddWithValue("@tdate", sqldatenull);
                }
                checkcmd.Parameters.AddWithValue("@status", ddlstatusEdit.SelectedValue);
                checkcmd.ExecuteNonQuery();


                grid.EditIndex = -1;
                showgroupitemlist();

            }
        }

    }

    protected void grpitem_grdview_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView grid = (GridView)sender;
        grid.EditIndex = -1;
        showgroupitemlist();
    }

    protected void grpitem_grdview_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView grid = (GridView)sender;
        grid.EditIndex = e.NewEditIndex;
        showgroupitemlist();
    }

    protected void grpitem_grdview_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        GridView grid = (GridView)sender;
        GridViewRow row = grid.Rows[e.RowIndex];
        string id = grid.DataKeys[e.RowIndex].Value.ToString();

        using (SqlConnection con = new SqlConnection(DBCon))
        {
            con.Open();
            using (SqlCommand cmd = new SqlCommand("update MF_GroupingItem set deleteind = 'X' where ids = '" + id + "' and deleteind <> 'X'", con))
            {
                cmd.ExecuteNonQuery();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Successful Deleted.');", true);
                showgroupitemlist();
            }
            con.Close();
        }
    }

    protected void grpitem_grdview_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label status = (Label)e.Row.FindControl("lblstatus");
            Label shownstatus = (Label)e.Row.FindControl("lblShownstatus");
            Label lblstartdate = (Label)e.Row.FindControl("lblstartdate");
            Label lblenddate = (Label)e.Row.FindControl("lblenddate");

            string format = "dd/MM/yyyy";
            string stringDate = DateTime.Now.ToString(format, CultureInfo.InvariantCulture);
            DateTime dateTime = DateTime.ParseExact(stringDate.Replace("-", "/"), format, CultureInfo.InvariantCulture);

            string status1 = DataBinder.Eval(e.Row.DataItem, "Status").ToString();
            //Label status = e.Row.FindControl("lblstatus") as Label;
            //Label shownstatus = e.Row.FindControl("lblShownstatus") as Label;


            if ((e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                DropDownList ddlShownstatus = (DropDownList)e.Row.FindControl("ddlShownstatus");
                DropDownList ddlstatusEdit = (DropDownList)e.Row.FindControl("ddlstatusEdit");

                try
                {
                    ddlstatusEdit.Items.FindByValue(status.Text).Selected = true;
                    status1 = status.Text;
                }
                catch (NullReferenceException err)
                {
                    //ddlstatusEdit.Items.FindByValue(status.Text).Selected = true;
                }

                try
                {
                    ddlShownstatus.Items.FindByValue(shownstatus.Text).Selected = true;
                }
                catch (NullReferenceException err)
                {
                }
            }

            string daterangeallow = DataBinder.Eval(e.Row.DataItem, "daterangeallow").ToString();

            if (daterangeallow == "Always Show")
            {
                if (status1.ToString() == "Active")
                {
                    e.Row.Attributes.Add("style", "background-color:#B0FFBE !important;");
                }
                else
                {
                    e.Row.Attributes.Add("style", "background-color:#FFB0B0 !important;");
                }
            }
            else
            {
                if (lblstartdate.Text != "")
                {
                    DateTime startdate = DateTime.ParseExact(lblstartdate.Text.Replace("-", "/"), format, CultureInfo.InvariantCulture);
                    if (lblenddate.Text != "")
                    {
                        DateTime enddate = DateTime.ParseExact(lblenddate.Text.Replace("-", "/"), format, CultureInfo.InvariantCulture);

                        if (dateTime >= Convert.ToDateTime(startdate) && dateTime <= Convert.ToDateTime(enddate))
                        {
                            if (status.Text == "Active")
                            {
                                e.Row.Attributes.Add("style", "background-color:#B0FFBE !important;");
                            }
                            else
                            {
                                e.Row.Attributes.Add("style", "background-color:#FFB0B0 !important;");
                            }
                        }
                        else
                        {
                            e.Row.Attributes.Add("style", "background-color:#FFB0B0 !important;");
                        }
                    }
                }
            }

        }
    }

    protected void ddlShownstatus_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (ddlShownstatus.SelectedValue == "Date Range")
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
        if (txtgrouptitle.Text == "")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Insert Image success.');", true);
        }
        else
        {
            if ((fileupload2.PostedFile != null) && (fileupload2.PostedFile.ContentLength > 0))
            {
                if (lblrunno.Text == "")
                {
                    using (SqlConnection myTxConn2 = new SqlConnection(DBCon))
                    {
                        using (SqlCommand myTxcmd2 = new SqlCommand("Insert_Img_runno", myTxConn2))
                        {
                            myTxcmd2.CommandType = CommandType.StoredProcedure;
                            myTxcmd2.Parameters.AddWithValue("@runno", "1");

                            myTxConn2.Open();

                            try
                            {
                                myTxcmd2.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                throw (ex);
                            }
                            finally
                            {
                                myTxcmd2.Dispose();
                                myTxConn2.Close();
                            }
                        }
                    }
                }
                else
                {
                    using (SqlConnection myTxConn3 = new SqlConnection(DBCon))
                    {
                        using (SqlCommand myTxcmd3 = new SqlCommand("Update_Img_runno", myTxConn3))
                        {
                            myTxcmd3.CommandType = CommandType.StoredProcedure;

                            myTxcmd3.Parameters.AddWithValue("@runno", "1");


                            myTxConn3.Open();

                            try
                            {
                                myTxcmd3.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                throw (ex);
                            }
                            finally
                            {
                                myTxcmd3.Dispose();
                                myTxConn3.Close();
                            }
                        }
                    }
                }
                SqlConnection con100 = new SqlConnection(DBCon);
                SqlCommand cmd100 = new SqlCommand("Load_Img_runno", con100);
                con100.Open();
                SqlDataAdapter da100 = new SqlDataAdapter(cmd100);
                DataTable dt100 = new DataTable();
                da100.Fill(dt100);

                lblrunno.Text = dt100.Rows[0]["runno"].ToString();

                con100.Close();

                string runno = lblrunno.Text;
                string iconextension = Path.GetExtension(fileupload2.PostedFile.FileName);

                string IconFileNameU = "ICON" + "-" + DateTime.UtcNow.AddHours(8).ToString("yyMMddHHmmss") + runno.ToString() + iconextension;
                string iconpath = Server.MapPath("GroupingPic/" + "Icon-IMG" + "/");

                if (Directory.Exists(iconpath))
                {
                    fileupload2.PostedFile.SaveAs(Server.MapPath("GroupingPic/" + "Icon-IMG" + "/" + IconFileNameU));
                    ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Insert Image success.');", true);
                    using (SqlConnection myQRConn = new SqlConnection(DBCon))
                    {
                        using (SqlCommand QRcmd = new SqlCommand("update MF_ItemGroupingList set Icon_Path=@icon where ids='" + Request.QueryString["groupid"].ToString() + "' and DeleteInd <> 'X'", myQRConn))
                        {
                            QRcmd.Parameters.AddWithValue("@icon", @"https://ezyshare.online/EzyShareListing/GroupingPic/" + "Icon-IMG" + "/" + IconFileNameU);
                            myQRConn.Open();

                            try
                            {
                                QRcmd.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                throw (ex);
                            }
                            finally
                            {
                                myQRConn.Close();
                            }
                        }

                    }

                }
                else
                {

                    System.IO.Directory.CreateDirectory(Server.MapPath("GroupingPic/" + "Icon-IMG" + "/"));

                    fileupload2.PostedFile.SaveAs(Server.MapPath("GroupingPic/" + "Icon-IMG" + "/" + IconFileNameU));

                }


            }
            else
            {
            }

            if ((fileupload1.PostedFile != null) && (fileupload1.PostedFile.ContentLength > 0))
            {
                if (lblrunno.Text == "")
                {
                    using (SqlConnection myTxConn2 = new SqlConnection(DBCon))
                    {
                        using (SqlCommand myTxcmd2 = new SqlCommand("Insert_Img_runno", myTxConn2))
                        {
                            myTxcmd2.CommandType = CommandType.StoredProcedure;
                            myTxcmd2.Parameters.AddWithValue("@runno", "1");

                            myTxConn2.Open();

                            try
                            {
                                myTxcmd2.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                throw (ex);
                            }
                            finally
                            {
                                myTxcmd2.Dispose();
                                myTxConn2.Close();
                            }
                        }
                    }
                }
                else
                {
                    using (SqlConnection myTxConn3 = new SqlConnection(DBCon))
                    {
                        using (SqlCommand myTxcmd3 = new SqlCommand("Update_Img_runno", myTxConn3))
                        {
                            myTxcmd3.CommandType = CommandType.StoredProcedure;

                            myTxcmd3.Parameters.AddWithValue("@runno", "1");


                            myTxConn3.Open();

                            try
                            {
                                myTxcmd3.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                throw (ex);
                            }
                            finally
                            {
                                myTxcmd3.Dispose();
                                myTxConn3.Close();
                            }
                        }
                    }
                }

                SqlConnection con100 = new SqlConnection(DBCon);
                SqlCommand cmd100 = new SqlCommand("Load_Img_runno", con100);
                con100.Open();
                SqlDataAdapter da100 = new SqlDataAdapter(cmd100);
                DataTable dt100 = new DataTable();
                da100.Fill(dt100);

                lblrunno.Text = dt100.Rows[0]["runno"].ToString();

                con100.Close();

                string runno = lblrunno.Text;
                string bannerextension = Path.GetExtension(fileupload1.PostedFile.FileName);

                string BannerFileNameU = "BANNER" + "-" + DateTime.UtcNow.AddHours(8).ToString("yyMMddHHmmss") + runno.ToString() + bannerextension;
                string bannerpath = Server.MapPath("GroupingPic/" + "Banner-IMG" + "/");

                if (Directory.Exists(bannerpath))
                {
                    fileupload1.PostedFile.SaveAs(Server.MapPath("GroupingPic/" + "Banner-IMG" + "/" + BannerFileNameU));
                    ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Insert Image success.');", true);
                    using (SqlConnection myQRConn = new SqlConnection(DBCon))
                    {
                        using (SqlCommand QRcmd = new SqlCommand("update MF_ItemGroupingList set Banner_Path=@banner where ids='" + Request.QueryString["groupid"].ToString() + "' and DeleteInd <> 'X'", myQRConn))
                        {
                            QRcmd.Parameters.AddWithValue("@banner", @"https://ezyshare.online/EzyShareListing/GroupingPic/" + "Banner-IMG" + "/" + BannerFileNameU);
                            myQRConn.Open();

                            try
                            {
                                QRcmd.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                throw (ex);
                            }
                            finally
                            {
                                myQRConn.Close();
                            }
                        }

                    }

                }
                else
                {

                    System.IO.Directory.CreateDirectory(Server.MapPath("GroupingPic/" + "Banner-IMG" + "/"));

                    fileupload1.PostedFile.SaveAs(Server.MapPath("GroupingPic/" + "Banner-IMG" + "/" + BannerFileNameU));



                }

                ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Insert Image success.');window.location.href='ItemGroupingSetup.aspx?groupid="+ Request.QueryString["groupid"] +"&merchant=" + Request.QueryString["merchant"] + "'", true);

            }
            else
            {
            }
        }
    }

    protected void btnback_Click(object sender, EventArgs e)
    {
        Response.Redirect("ItemGroupingListing.aspx?username=" + Request.QueryString["username"] + "&merchant=" + Request.QueryString["merchant"]);
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        showitemlist();
        ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "$('#GroupModal').modal('show');", true);
    }

    protected void btn_updateitem_Click(object sender, EventArgs e)
    {
        SqlDateTime sqldatenull;
        sqldatenull = SqlDateTime.Null;
        using (SqlConnection con = new SqlConnection(DBCon))
        {
            con.Open();
            using (SqlCommand checkcmd = new SqlCommand("update MF_GroupingItem set fdate=@fdate,tdate=@tdate,status =@status,daterangeallow=@daterangeallow where ids='" + hditemids.Value + "'", con))
            {
                checkcmd.Parameters.AddWithValue("@daterangeallow", ddlShownstatus.SelectedValue);
                if (ddlShownstatus.SelectedValue == "Date Range")
                {
                    checkcmd.Parameters.AddWithValue("@fdate", txtstartdateEdit.Text);
                }
                else
                {
                    checkcmd.Parameters.AddWithValue("@fdate", sqldatenull);
                }
                if (ddlShownstatus.SelectedValue == "Date Range")
                {
                    checkcmd.Parameters.AddWithValue("@tdate", txtenddateEdit.Text);
                }
                else
                {
                    checkcmd.Parameters.AddWithValue("@tdate", sqldatenull);
                }
                checkcmd.Parameters.AddWithValue("@status", ddlstatusEdit.SelectedValue);
                checkcmd.ExecuteNonQuery();


                showgroupitemlist();
                updateee.Update();
                ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "$('#GroupItemEditModal').modal('hide');$('body').removeClass('modal-open');$('.modal-backdrop').remove();", true);

            }
            con.Close();
        }
    }
}