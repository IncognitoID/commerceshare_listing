using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Globalization;

public partial class CategorySetupList : System.Web.UI.Page
{
    Business BL = new Business();
    DBClass DL = new DBClass();
    protected static string DBCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
    public static string Logout;
    public int count;

    //protected static String DBCon = ConfigurationManager.AppSettings["ConnectionString"].ToString();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            MF_Con.Text = DBCon;
            Session["PName"] = "Category Listing";
            txt_Search.Text = Request.QueryString["searchresult"];
            CatLoadList(1);
        }
        //}
    }

    private void CatLoadList(int pageIndex)
    {
        using (SqlConnection con = new SqlConnection(MF_Con.Text))
        {
            using (SqlCommand cmd = new SqlCommand("Cat_Listing2", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PageIndex", pageIndex);
                cmd.Parameters.AddWithValue("@PageSize", int.Parse("10"));
                cmd.Parameters.AddWithValue("@supplier_code", Request.QueryString["merchant"].ToString());
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
            CatLoadList(1);
        }
    }



    protected void ddlPager_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.CatLoadList(Convert.ToInt32(ddlPager.SelectedValue));
    }

    protected void txt_Search_TextChanged(object sender, EventArgs e)
    {
        this.CatLoadList(Convert.ToInt32(ddlPager.SelectedValue));
    }

    protected void btn_New_Click(object sender, EventArgs e)
    {
        Session["Category_ID"] = "";
        Session["PName"] = "Category Setup - New";
        Response.Redirect("CategorySetup.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "&catid=");
    }

    protected void View_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "edit")
        {
            Response.Redirect("CategorySetup.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "&catid=" + e.CommandArgument.ToString());
        }
        if (e.CommandName == "del")
        {
            string del = "X";
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "OK")
            {
                using (SqlConnection con = new SqlConnection(DBCon))
                {
                    con.Open();
                    using (SqlCommand checkitem = new SqlCommand("Select a.*, B.* from MF_Item as a inner join MF_Category_miso as b on b.Category_Code = a.Category_Code where b.Category_Code = '" + e.CommandArgument.ToString() + "' and a.DeleteInd <> 'X'", con))
                    {
                        SqlDataAdapter adp = new SqlDataAdapter(checkitem);
                        DataTable dt = new DataTable();
                        adp.Fill(dt);
                        if (dt.Rows.Count <= 0)
                        {
                            using (SqlCommand cmd = new SqlCommand("Update Mf_Category_miso set modified_dt=dateadd(hour,(8),CONVERT([varchar](20),getdate(),(120))),DeleteInd=@delete where Category_Code = '" + e.CommandArgument.ToString() + "'", con))
                            {
                                cmd.Parameters.AddWithValue("@delete", del.Trim().Replace("'", "`"));
                                cmd.ExecuteNonQuery();

                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Successful Deleted.');window.location.href='CategorySetupList.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "'", true) ;
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Delete failed. Please delete all Item under this Category first.');", true);
                        }
                    }
                    con.Close();
                }

            }
            else
            {

            }
        }
    }



    protected void View_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='#cccccc'");
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c");
        }
    }
}