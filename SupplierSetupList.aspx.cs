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

public partial class suppliersetuplist : System.Web.UI.Page
{
    Business BL = new Business();
    DBClass DL = new DBClass();
    protected static string DBCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
    public static string Logout;
    public int count;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (string.IsNullOrEmpty(Request.QueryString["merchant"]))
            {
                string defaultMerchant = "S-0622"; // can change
                Response.Redirect("suppliersetuplist.aspx?merchant=" + defaultMerchant, false);
                return; 
            }

            MF_Con.Text = DBCon;
            SupplierLoadList(1);

            Session["PName"] = "Supplier Listing";
        }
    }


    private void SupplierLoadList(int pageIndex)
    {
        using (SqlConnection con = new SqlConnection(MF_Con.Text))
        {
            string query = @"
        WITH SupplierResult AS
        (
            SELECT 
                Ids,
                Supplier_Code,
                Supplier_Name,
                Contact_No,
                Email,
                Created_Dt,
                Modified_Dt,
                ROW_NUMBER() OVER (ORDER BY Ids DESC) AS RowNum
            FROM MF_Supplier
            WHERE ISNULL(DeleteInd, '') <> 'X'
              AND (
                    Supplier_Code LIKE @FilterText
                    OR Supplier_Name LIKE @FilterText
                    OR Contact_No LIKE @FilterText
                    OR Email LIKE @FilterText
                  )
        )
        SELECT *
        FROM SupplierResult
        WHERE RowNum BETWEEN ((@PageIndex - 1) * @PageSize + 1) AND (@PageIndex * @PageSize);

        SELECT COUNT(*)
        FROM MF_Supplier
        WHERE ISNULL(DeleteInd, '') <> 'X'
          AND (
                Supplier_Code LIKE @FilterText
                OR Supplier_Name LIKE @FilterText
                OR Contact_No LIKE @FilterText
                OR Email LIKE @FilterText
              );
    ";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@PageIndex", pageIndex);
                cmd.Parameters.AddWithValue("@PageSize", 10);

                // Search text
                string filter = string.IsNullOrWhiteSpace(txt_Search.Text)
                    ? "%%"
                    : "%" + txt_Search.Text.Trim().Replace("'", "`") + "%";
                cmd.Parameters.AddWithValue("@FilterText", filter);

                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);

                DataTable v = ds.Tables[0];  // Paged result
                int recordCount = Convert.ToInt32(ds.Tables[1].Rows[0][0]); // Count

                if (v.Rows.Count == 0)
                {
                    v.Rows.Add(v.NewRow());
                    grd_View.DataSource = v;
                    grd_View.DataBind();

                    int columnCount = grd_View.Rows[0].Cells.Count;
                    grd_View.Rows[0].Cells.Clear();
                    grd_View.Rows[0].Cells.Add(new TableCell());
                    grd_View.Rows[0].Cells[0].ColumnSpan = columnCount;
                    grd_View.Rows[0].Cells[0].Text = "No Record Found.";

                    lbl_record3.Text = "0";
                }
                else
                {
                    grd_View.DataSource = v;
                    grd_View.DataBind();
                    lbl_record3.Text = v.Rows.Count.ToString();
                }

                if (grd_View.HeaderRow != null)
                    grd_View.HeaderRow.TableSection = TableRowSection.TableHeader;

                lbl_Record2.Text = recordCount.ToString();

                if (recordCount == 0)
                    recordCount = 1;

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
            SupplierLoadList(1);
        }
    }

    protected void ddlPager_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.SupplierLoadList(Convert.ToInt32(ddlPager.SelectedValue));
    }

    protected void txt_Search_TextChanged(object sender, EventArgs e)
    {
        this.SupplierLoadList(Convert.ToInt32(ddlPager.SelectedValue));
    }

    protected void btn_New_Click(object sender, EventArgs e)
    {
        string merchant = Request.QueryString["merchant"];

        Session["Supplier_Code"] = "";
        Session["PName"] = "Supplier Setup - New";

        Response.Redirect("supplier.aspx?merchant=" + Server.UrlEncode(merchant) + "&supplier=");
    }



    protected void View_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "edit")
        {
            string merchant = Request.QueryString["merchant"] ?? "";
            string supplierCode = Convert.ToString(e.CommandArgument);

            Response.Redirect("supplier.aspx?merchant=" + Server.UrlEncode(merchant) + "&supplier=" + Server.UrlEncode(supplierCode));
        }


        if (e.CommandName == "del")
        {
            var supplierCode = Convert.ToString(e.CommandArgument);
            using (SqlConnection con = new SqlConnection(MF_Con.Text))
            using (SqlCommand cmd = new SqlCommand(
                "UPDATE MF_Supplier SET DeleteInd='X', Modified_Dt=GETDATE() WHERE Supplier_Code=@code", con))
            {
                cmd.Parameters.AddWithValue("@code", supplierCode);
                con.Open();
                cmd.ExecuteNonQuery();
            }

            int page = 1;
            if (ddlPager != null && ddlPager.SelectedItem != null)
                int.TryParse(ddlPager.SelectedValue, out page);
            SupplierLoadList(page <= 0 ? 1 : page);
        }
    }

    protected void lnkedit_Command(object sender, CommandEventArgs e)
    {
        string supplierCode = e.CommandArgument.ToString();
        Response.Redirect("supplier.aspx?supplier=" + supplierCode);
    }

    protected void EditSupplier_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        string supplierCode = btn.CommandArgument;
        Response.Redirect("supplier.aspx?merchant=" + supplierCode);
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