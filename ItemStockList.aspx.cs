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
using System.IO;

public partial class ItemStockList : System.Web.UI.Page
{
    Business BL = new Business();
    DBClass DL = new DBClass();
    protected static String DBCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
    public static string Logout;
    public int count;
    public static string merchant;
    //protected static String DBCon = ConfigurationManager.AppSettings["ConnectionString"].ToString();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["merchant"] != null)
            {
                merchant = Request.QueryString["merchant"].ToString();
            }
            else
            {
                merchant = "";
            }
            DeptLoadList(1);

            Session["PName"] = "Item Stock Listing";

            using (SqlConnection con = new SqlConnection(DBCon))
            {
                using (SqlCommand cmd = new SqlCommand("Select * FROM[MF_Item] WHERE[DeleteInd] <> 'X' and[supplier_code] = '"+ Request.QueryString["merchant"] +"'", con))
                {
                    con.Open();
                    SqlDataReader idr = cmd.ExecuteReader();
                    if (idr.HasRows == false)
                    {
                        DataTable v = new DataTable();

                        v.Load(idr);
                        dvNoRecords.Visible = true;
                        lbl_record3.Text = v.Rows.Count.ToString();
                    }
                    else
                    {
                        DataTable v = new DataTable();

                        v.Load(idr);
                        GridView1.DataSource = v;
                        GridView1.DataBind();

                        // grd_View.HeaderRow.Cells[0].Attributes["data-class"] = "expand";
                        // grd_View.HeaderRow.Cells[2].Attributes["data-hide"] = "phone";

                        GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;
                        lbl_record3.Text = v.Rows.Count.ToString();
                    }

                    //DataTable v = new DataTable();

                    //v.Load(idr);

                    //GridView1.DataSource = v;
                    //GridView1.DataBind();
                }
            }
        }
    }

    private void DeptLoadList(int pageIndex)
    {
        using (SqlConnection con = new SqlConnection(DBCon))
        {
            using (SqlCommand cmd = new SqlCommand("Stock_Listing", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PageIndex", pageIndex);
                cmd.Parameters.AddWithValue("@merchant", merchant.ToString());
                cmd.Parameters.AddWithValue("@PageSize", int.Parse("10"));

                if (txt_Search.Text.Trim() == "")
                {
                    cmd.Parameters.AddWithValue("@filtertext", "%%");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@filtertext", "%" + txt_Search.Text.Trim().Replace("'", "`") + "%");
                }

                cmd.Parameters.Add("@RecordCount", SqlDbType.Int, 4);
                cmd.Parameters["@RecordCount"].Direction = ParameterDirection.Output;
                con.Open();
                SqlDataReader idr = cmd.ExecuteReader();

                if (idr.HasRows == false)
                {
                    DataTable v = new DataTable();

                    v.Load(idr);

                    dvNoRecords.Visible = true;

                    //grd_View.HeaderRow.TableSection = TableRowSection.TableHeader;
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


    protected void View_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='#cccccc'");
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c");


        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        //Export the GridView to Excel  
        PrepareGridViewForExport(GridView1);
        ExportGridView();
    }
    private void ExportGridView()
    {
        string attachment = "attachment; filename=ProductStock.xls";
        Response.ClearContent();
        Response.AddHeader("content-disposition", attachment);
        Response.ContentType = "application/ms-excel";
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        GridView1.RenderControl(htw);
        Response.Write(sw.ToString());
        Response.End();
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
    }
    private void PrepareGridViewForExport(Control gv)
    {
        LinkButton lb = new LinkButton();
        Literal l = new Literal();
        string name = String.Empty;
        for (int i = 0; i < gv.Controls.Count; i++)
        {
            if (gv.Controls[i].GetType() == typeof(LinkButton))
            {
                l.Text = (gv.Controls[i] as LinkButton).Text;
                gv.Controls.Remove(gv.Controls[i]);
                gv.Controls.AddAt(i, l);
            }
            else if (gv.Controls[i].GetType() == typeof(DropDownList))
            {
                l.Text = (gv.Controls[i] as DropDownList).SelectedItem.Text;
                gv.Controls.Remove(gv.Controls[i]);
                gv.Controls.AddAt(i, l);
            }
            else if (gv.Controls[i].GetType() == typeof(CheckBox))
            {
                l.Text = (gv.Controls[i] as CheckBox).Checked ? "True" : "False";
                gv.Controls.Remove(gv.Controls[i]);
                gv.Controls.AddAt(i, l);
            }
            if (gv.Controls[i].HasControls())
            {
                PrepareGridViewForExport(gv.Controls[i]);
            }
        }
    }

    protected void txt_Search_TextChanged(object sender, EventArgs e)
    {
        DeptLoadList(1);
    }
}