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

public partial class BrandSetupList : System.Web.UI.Page
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
            DeptLoadList(1);

            Session["PName"] = "Brand Listing";
        }
    }

    private void DeptLoadList(int pageIndex)
    {
        using (SqlConnection con = new SqlConnection(MF_Con.Text))
        {
            using (SqlCommand cmd = new SqlCommand("Brd_Listing", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PageIndex", pageIndex);
                cmd.Parameters.AddWithValue("@supplier", Request.QueryString["merchant"].ToString());
                cmd.Parameters.AddWithValue("@PageSize", int.Parse("10"));

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

                    //grd_View.HeaderRow.Cells[0].Attributes["data-class"] = "expand";
                    //grd_View.HeaderRow.Cells[2].Attributes["data-hide"] = "phone";

                    grd_View.HeaderRow.TableSection = TableRowSection.TableHeader;
                    lbl_record3.Text = v.Rows.Count.ToString();
                }
                else
                {
                    DataTable v = new DataTable();

                    v.Load(idr);
                    grd_View.DataSource = v;
                    grd_View.DataBind();

                    // grd_View.HeaderRow.Cells[0].Attributes["data-class"] = "expand";
                    // grd_View.HeaderRow.Cells[2].Attributes["data-hide"] = "phone";

                    grd_View.HeaderRow.TableSection = TableRowSection.TableHeader;
                    lbl_record3.Text = v.Rows.Count.ToString();
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
            DeptLoadList(1);
        }
    }

    protected void ddlPager_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.DeptLoadList(Convert.ToInt32(ddlPager.SelectedValue));
    }

    protected void txt_Search_TextChanged(object sender, EventArgs e)
    {
        this.DeptLoadList(Convert.ToInt32(ddlPager.SelectedValue));
    }

    protected void btn_New_Click(object sender, EventArgs e)
    {
        Session["Brand_ID"] = "";
        Session["PName"] = "Brand Setup - New";
        Response.Redirect("BrandSetup.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "&id=");
    }

    protected void View_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "edit")
        {
            Response.Redirect("BrandSetup.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "&id=" + e.CommandArgument.ToString());
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
                    using (SqlCommand checkitem = new SqlCommand("Select a.*, B.* from MF_Item as a inner join MF_Brand as b on b.brand_code = a.Brand where b.brand_code = '" + e.CommandArgument.ToString() + "' and a.DeleteInd <> 'X'", con))
                    {
                        SqlDataAdapter adp = new SqlDataAdapter(checkitem);
                        DataTable dt = new DataTable();
                        adp.Fill(dt);
                        if (dt.Rows.Count <= 0)
                        {
                            using (SqlCommand cmd = new SqlCommand("Update MF_Brand set DeleteInd=@delete where Brand_Code = '" + e.CommandArgument.ToString() + "'", con))
                            {
                                cmd.Parameters.AddWithValue("@delete", del.Trim().Replace("'", "`"));
                                cmd.ExecuteNonQuery();

                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Successful Deleted.');window.location.href='BrandSetupList.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "'", true);
                            }
                        }
                        else
                        {
                            MsgBox1.alert("Delete failed. Please delete all Item under this Brand first.");
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