using DocumentFormat.OpenXml.Office.Word;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class Printer_MaintenanceListing : System.Web.UI.Page
{
    protected static string DBCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            MF_Con.Text = DBCon;
            PrinterLoadList(1);
        }
    }

    private void PrinterLoadList(int pageIndex)
    {
        using (SqlConnection con = new SqlConnection(MF_Con.Text))
        {
            using (SqlCommand cmd = new SqlCommand("Printer_Listing2", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PageIndex", pageIndex);
                cmd.Parameters.AddWithValue("@PageSize", int.Parse("10"));
                cmd.Parameters.AddWithValue("@merchant_code", Request.QueryString["merchant"].ToString());
                if (Request.QueryString["user"] != null)
                {
                    cmd.Parameters.AddWithValue("@user_code", Request.QueryString["user"].ToString());
                }
                else
                {
                    cmd.Parameters.AddWithValue("@user_code", Request.QueryString["merchant"].ToString());
                }
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
                }
                else
                {
                    DataTable v = new DataTable();

                    v.Load(idr);

                    grd_View.DataSource = v;
                    grd_View.DataBind();
                    lbl_record3.Text = v.Rows.Count.ToString();

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
            PrinterLoadList(1);
        }
    }
    protected void btn_New_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["user"] != null)
        {
            Response.Redirect("Printer_Maintenance.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "&user=" + Request.QueryString["user"].ToString() + "&id=");
        }
        else
        {
            Response.Redirect("Printer_Maintenance.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "&id=");
        }

    }

    protected void txt_Search_TextChanged(object sender, EventArgs e)
    {
        this.PrinterLoadList(Convert.ToInt32(ddlPager.SelectedValue));
    }

    protected void ddlPager_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.PrinterLoadList(Convert.ToInt32(ddlPager.SelectedValue));
    }

    protected void View_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "edit")
        {
            if (Request.QueryString["user"] != null)
            {
                Response.Redirect("Printer_Maintenance.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "&user=" + Request.QueryString["user"].ToString() + "&id=" + e.CommandArgument.ToString());
            }
            else
            {
                Response.Redirect("Printer_Maintenance.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "&id=" + e.CommandArgument.ToString());
            }

        }

        if (e.CommandName == "assgn")
        {
            hdPrinterId.Value = e.CommandArgument.ToString();
            LoadAssignItemModal_Printer();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", "$('.modal-backdrop').remove(); $('#PrinterAssignModal').modal({show:true, backdrop:false, keyboard:false});", true);
            return;
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

                    using (SqlCommand cmd = new SqlCommand("Update PosSys_Printer set modified_dt=dateadd(hour,(8),CONVERT([varchar](20),getdate(),(120))),DeleteInd=@delete where ids = '" + e.CommandArgument.ToString() + "'", con))
                    {
                        cmd.Parameters.AddWithValue("@delete", del.Trim().Replace("'", "`"));
                        cmd.ExecuteNonQuery();

                        if (Request.QueryString["user"] != null)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Successful Deleted.');window.location.href='Printer_MaintenanceListing.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "&user=" + Request.QueryString["user"].ToString() + "'", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Successful Deleted.');window.location.href='Printer_MaintenanceListing.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "'", true);

                        }
                        ;



                    }
                    con.Close();
                }

            }
            else
            {

            }
        }
    }

    private HashSet<string> AssignedPrinterItems
    {
        get
        {
            if (ViewState["AssignedPrinterItems"] == null)
                ViewState["AssignedPrinterItems"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            return (HashSet<string>)ViewState["AssignedPrinterItems"];
        }
        set { ViewState["AssignedPrinterItems"] = value; }
    }

    private string CurrentMerchantCode
    {
        get { return ViewState["CurrentMerchantCode"] as string ?? ""; }
        set { ViewState["CurrentMerchantCode"] = value; }
    }


    private void LoadAssignItemModal_Printer()
    {
        using (var con = new SqlConnection(DBCon))
        {
            con.Open();

            string merchantCode = "";
            using (var h = new SqlCommand(@"
            SELECT TOP 1 Print_Name, Merchant_Code
            FROM dbo.PosSys_Printer
            WHERE ids = @pid AND (DeleteInd IS NULL OR DeleteInd <> 'X');", con))
            {
                h.Parameters.AddWithValue("@pid", hdPrinterId.Value);
                using (var r = h.ExecuteReader())
                {
                    if (r.Read())
                    {
                        var name = r["Print_Name"] as string;
                        lblPrinterHdr.Text = string.IsNullOrEmpty(name) ? "" : "(" + name + ")";
                        //merchantCode = r["Merchant_Code"] == DBNull.Value ? "" : (string)r["Merchant_Code"];
                        merchantCode = r["Merchant_Code"] == DBNull.Value ? "" : (string)r["Merchant_Code"];
                        CurrentMerchantCode = merchantCode;
                    }
                }
            }

            var assigned = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            using (var a = new SqlCommand(@"
            SELECT Item_Code
            FROM dbo.PosSys_PrinterItem
            WHERE Printer_ID = @pid AND Merchant_Code = @mcode;", con))
            {
                a.Parameters.AddWithValue("@pid", hdPrinterId.Value);
                a.Parameters.AddWithValue("@mcode", merchantCode);

                using (var r = a.ExecuteReader())
                    while (r.Read()) assigned.Add(r.GetString(0));
            }

            AssignedPrinterItems = assigned;

            // List all items from merchant
            var adp = new SqlDataAdapter(@"
            SELECT item_code,
                   longdesc,
                   barcode,
                   FilePath,
                   CAST(NULL AS nvarchar(255)) AS shortdesc
            FROM dbo.mf_item
            WHERE (deleteind IS NULL OR deleteind <> 'X')
              AND (@m = '' OR ISNULL(supplier_code,'') = @m)
            ORDER BY longdesc;", con);
            adp.SelectCommand.Parameters.AddWithValue("@m", merchantCode);

            var dt = new DataTable();
            adp.Fill(dt);

            grd_viewitem.DataSource = dt;
            grd_viewitem.DataBind();
        }
        
        BindDepartments(CurrentMerchantCode);
        BindCategories(CurrentMerchantCode);

        // Restore filters safely
        if (ViewState["SelectedDept"] != null)
        {
            var deptValue = ViewState["SelectedDept"].ToString();
            if (ddlDept.Items.FindByValue(deptValue) != null)
                ddlDept.SelectedValue = deptValue;
        }

        if (ViewState["SelectedCat"] != null)
        {
            var catValue = ViewState["SelectedCat"].ToString();
            if (ddlCat.Items.FindByValue(catValue) != null)
                ddlCat.SelectedValue = catValue;
        }

        if (ViewState["SearchText"] != null)
            txtItemSearch.Text = ViewState["SearchText"].ToString();

        
        BindItemsForPrinter();

    ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", "$('.modal-backdrop').remove(); $('#PrinterAssignModal').modal({show:true, backdrop:false, keyboard:false});", true);
    
    }


    private void BindDepartments(string supplierCode)
    {
        using (var con = new SqlConnection(DBCon))
        {
            string sql = @"SELECT Department_Code, Department_Description
                         FROM MF_Department_miso
                         WHERE (DeleteInd IS NULL OR DeleteInd <> 'X')
                         AND (@s = '' OR ISNULL(supplier_code,'') = @s)
                         ORDER BY Department_Description";
            using (var adp = new SqlDataAdapter(sql, con))
            {
                adp.SelectCommand.Parameters.AddWithValue("@s", supplierCode);
                var dt = new DataTable();
                adp.Fill(dt);

                ddlDept.DataSource = dt;
                ddlDept.DataTextField = "Department_Description";
                ddlDept.DataValueField = "Department_Code";
                ddlDept.DataBind();
            }
        }
        ddlDept.Items.Insert(0, new ListItem("-- Select Department --", ""));
    }


    private void BindCategories(string supplierCode)
    {
        using (var con = new SqlConnection(DBCon))
        {
            string sql = @"
            SELECT Category_Code, Category_Description
            FROM MF_Category_miso
            WHERE (DeleteInd IS NULL OR DeleteInd <> 'X')
              AND (@s = '' OR ISNULL(supplier_code,'') = @s)
            ORDER BY Category_Description";

            using (var adp = new SqlDataAdapter(sql, con))
            {
                adp.SelectCommand.Parameters.AddWithValue("@s", supplierCode);
                var dt = new DataTable();
                adp.Fill(dt);

                ddlCat.DataSource = dt;
                ddlCat.DataTextField = "Category_Description";
                ddlCat.DataValueField = "Category_Code";
                ddlCat.DataBind();
            }
        }

        ddlCat.Items.Insert(0, new ListItem("-- Select Category --", ""));
    }


    private void BindItemsForPrinter()
    {
        using (var con = new SqlConnection(DBCon))
        {
            string sql = @"
        SELECT Item_Code, LongDesc, Barcode, FilePath
        FROM MF_Item
        WHERE (DeleteInd IS NULL OR DeleteInd <> 'X')
          AND (@s = '' OR ISNULL(supplier_code,'') = @s)
          AND (@d = '' OR Category_Code LIKE @d + '%')
          AND (@c = '' OR Category_Code = @c)
          AND (LongDesc LIKE @search OR Item_Code LIKE @search OR Barcode LIKE @search)
        ORDER BY LongDesc";

            using (var adp = new SqlDataAdapter(sql, con))
            {
                adp.SelectCommand.Parameters.AddWithValue("@s", CurrentMerchantCode);
                adp.SelectCommand.Parameters.AddWithValue("@d", ddlDept.SelectedValue ?? "");
                adp.SelectCommand.Parameters.AddWithValue("@c", ddlCat.SelectedValue ?? "");
                adp.SelectCommand.Parameters.AddWithValue("@search", "%" + txtItemSearch.Text.Trim() + "%");

                var dt = new DataTable();
                adp.Fill(dt);

                grd_viewitem.DataSource = dt;
                grd_viewitem.DataBind();
            }
        }
    }


    protected void btnUpdatePrinterItems_Click(object sender, EventArgs e)
    {
        using (var con = new SqlConnection(DBCon))
        {
            con.Open();

            string merchantCode = Request.QueryString["merchant"].ToString();
            string userCode = Request.QueryString["user"] ?? merchantCode;

            var assigned = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            using (var cmd = new SqlCommand(@"
            SELECT Item_Code
            FROM dbo.PosSys_PrinterItem
            WHERE Printer_ID = @pid AND Merchant_Code = @mcode;", con))
            {
                cmd.Parameters.AddWithValue("@pid", hdPrinterId.Value);
                cmd.Parameters.AddWithValue("@mcode", merchantCode);

                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                        assigned.Add(r.GetString(0));
                }
            }

            foreach (GridViewRow row in grd_viewitem.Rows)

            {
                if (row.RowType != DataControlRowType.DataRow) continue;

                var chk = (CheckBox)row.FindControl("chkitem");
                var hd = (HiddenField)row.FindControl("hditem_code");

                if (chk != null && hd != null && !string.IsNullOrEmpty(hd.Value))
                {
                    if (chk.Checked && !assigned.Contains(hd.Value))
                    {
                        using (var insert = new SqlCommand(@"
                        INSERT INTO dbo.PosSys_PrinterItem
                            (Printer_ID, Merchant_Code, User_Code, Item_Code, Created_DT)
                        VALUES
                            (@pid, @mcode, @user, @icode, DATEADD(HOUR, 8, GETUTCDATE()));", con))
                        {
                            insert.Parameters.AddWithValue("@pid", hdPrinterId.Value);
                            insert.Parameters.AddWithValue("@mcode", merchantCode);
                            insert.Parameters.AddWithValue("@user", userCode);
                            insert.Parameters.AddWithValue("@icode", hd.Value);
                            insert.ExecuteNonQuery();
                        }
                    }
                    else if (!chk.Checked && assigned.Contains(hd.Value))
                    {
                        using (var del = new SqlCommand(@"
                        DELETE FROM dbo.PosSys_PrinterItem
                        WHERE Printer_ID = @pid AND Merchant_Code = @mcode AND Item_Code = @icode;", con))
                        {
                            del.Parameters.AddWithValue("@pid", hdPrinterId.Value);
                            del.Parameters.AddWithValue("@mcode", merchantCode);
                            del.Parameters.AddWithValue("@icode", hd.Value);
                            del.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        LoadAssignItemModal_Printer();
        ScriptManager.RegisterStartupScript(this, GetType(), "AssignSavedPrinter", "if (window.Swal) { Swal.fire({icon:'success', title:'Printer Items Updated'}); } else { alert('Printer Items Updated'); }", true);
    }


    protected void ddlDept_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["SelectedDept"] = ddlDept.SelectedValue;
        BindItemsForPrinter();

        ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", "$('.modal-backdrop').remove(); $('#PrinterAssignModal').modal({show:true, backdrop:false, keyboard:false});", true);

    }

    protected void ddlCat_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["SelectedCat"] = ddlCat.SelectedValue;
        BindItemsForPrinter();

        ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", "$('.modal-backdrop').remove(); $('#PrinterAssignModal').modal({show:true, backdrop:false, keyboard:false});", true);

    }


    protected void btnSearchItems_Click(object sender, EventArgs e)
    {
        ViewState["SearchText"] = txtItemSearch.Text.Trim();
        BindItemsForPrinter();

        ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", "$('.modal-backdrop').remove(); $('#PrinterAssignModal').modal({show:true, backdrop:false, keyboard:false});", true);

    }

    protected void btnClearSearch_Click(object sender, EventArgs e)
    {
        txtItemSearch.Text = "";
        ddlDept.SelectedIndex = 0;
        ddlCat.Items.Clear();
        ddlCat.Items.Insert(0, new ListItem("-- Select Category --", ""));
        ddlCat.Enabled = true;

        BindItemsForPrinter();

        ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", "$('.modal-backdrop').remove(); $('#PrinterAssignModal').modal({show:true, backdrop:false, keyboard:false});", true);

    }


    protected void grd_viewitem_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow) return;

        // Checkbox binding
        var hd = (HiddenField)e.Row.FindControl("hditem_code");
        var chk = (CheckBox)e.Row.FindControl("chkitem");
        var assigned = AssignedPrinterItems;
        if (assigned != null && hd != null && chk != null)
            chk.Checked = !string.IsNullOrEmpty(hd.Value) && assigned.Contains(hd.Value);

        var drv = (DataRowView)e.Row.DataItem;
        string filePath = "";
        if (drv["FilePath"] != DBNull.Value)
            filePath = drv["FilePath"].ToString();

        var img = (System.Web.UI.WebControls.Image)e.Row.FindControl("imgItem");
        if (img != null)
        {
            img.ImageUrl = string.IsNullOrEmpty(filePath) ? "Images/NoPic.png" : filePath;
            img.Width = 90;
            img.Height = 90;
        }
    }


    private void UpdateAssignedPrinterItemsFromGrid()
    {
        foreach (GridViewRow row in grd_viewitem.Rows)
        {
            if (row.RowType != DataControlRowType.DataRow) continue;
            var chk = (CheckBox)row.FindControl("chkitem");
            var hd = (HiddenField)row.FindControl("hditem_code");
            if (chk != null && hd != null && !string.IsNullOrEmpty(hd.Value))
            {
                if (chk.Checked)
                    AssignedPrinterItems.Add(hd.Value);
                else
                    AssignedPrinterItems.Remove(hd.Value);
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