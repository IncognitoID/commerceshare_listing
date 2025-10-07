using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class ModifierSetting : System.Web.UI.Page
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
            ModifierGrpLoad(1);

            Session["PName"] = "Modifier Group Listing";
        }
    }

    private void ModifierGrpLoad(int pageIndex)
    {
        using (SqlConnection con = new SqlConnection(MF_Con.Text))
        {
            using (SqlCommand cmd = new SqlCommand("Modifier_Grp_Listing", con))
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

                    grd_View.HeaderRow.TableSection = TableRowSection.TableHeader;
                    lbl_record3.Text = v.Rows.Count.ToString();
                }
                else
                {
                    DataTable v = new DataTable();

                    v.Load(idr);
                    grd_View.DataSource = v;
                    grd_View.DataBind();

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
            ModifierGrpLoad(1);
        }
    }

    protected void ddlPager_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.ModifierGrpLoad(Convert.ToInt32(ddlPager.SelectedValue));
    }

    protected void txt_Search_TextChanged(object sender, EventArgs e)
    {
        this.ModifierGrpLoad(Convert.ToInt32(ddlPager.SelectedValue));
    }

    protected void btn_New_Click(object sender, EventArgs e)
    {
        ///Modal popup create new modifier
        ///

        Response.Redirect("ModifierOptionSetting.aspx?merchant="+ Request.QueryString["merchant"].ToString() + "&ModifierID="  );
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert1", "$('#modalNew').modal('show');", true);

    }


    protected void View_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "edit")
        {
            ///Modal popup Edit Modifier
            ///

            Response.Redirect("ModifierOptionSetting.aspx?merchant="+ Request.QueryString["merchant"].ToString() + "&ModifierID=" + e.CommandArgument.ToString());

        }


        if (e.CommandName == "assgn")
        {
            hdModifierId.Value = e.CommandArgument.ToString();
            LoadAssignItemModal();   
            updAssignItems.Update();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "KeepAssignModal", "$('.modal-backdrop').remove(); $('#ModifierModal').modal({show:true, backdrop:false, keyboard:false});", true);

            return;
        }



        if (e.CommandName == "del")
        {
            string del = "X";
            string confirmValue = Request.Form["confirm_value"];
            string newconfirm = "";
            if (!string.IsNullOrEmpty(confirmValue))
            {
                // Split the confirmValue string by commas
                string[] values = confirmValue.Split(',');

                // Get the last value
                string lastValue = values[values.Length - 1];

                newconfirm = lastValue;
            }

            if (newconfirm == "OK")
            {
                using (SqlConnection con = new SqlConnection(DBCon))
                {
                    con.Open();
                    using (SqlCommand checkitem = new SqlCommand("select * from mf_modifier_group where modifier_id = '" + e.CommandArgument.ToString() + "' and DeleteInd <> 'X'", con))
                    {
                        SqlDataAdapter adp = new SqlDataAdapter(checkitem);
                        DataTable dt = new DataTable();
                        adp.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            SqlCommand delmaincmd = new SqlCommand("Update mf_modifier_group set DeleteInd='X' where modifier_id = '" + e.CommandArgument.ToString() + "' and deleteind <> 'X'", con);
                            delmaincmd.ExecuteNonQuery();


                            using (SqlCommand delsubcmd = new SqlCommand("Update mf_modifier_option set DeleteInd=@delete where modifier_id = '" + e.CommandArgument.ToString() + "' and deleteind <> 'X'", con))
                            {
                                delsubcmd.Parameters.AddWithValue("@delete", del.Trim().Replace("'", "`"));
                                delsubcmd.ExecuteNonQuery();

                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Successful Deleted.');window.location.href='ModifierSetting.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "'", true);
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

    /*private string MerchantId
    {
        get { return Request.QueryString["merchant"] ?? ""; }
    }*/

    private HashSet<string> AssignedItemCodes
    {
        get
        {
            if (ViewState["AssignedItemCodes"] == null)
                ViewState["AssignedItemCodes"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            return (HashSet<string>)ViewState["AssignedItemCodes"];
        }
        set { ViewState["AssignedItemCodes"] = value; }
    }


    private string CurrentMerchantCode
    {
        get { return ViewState["CurrentMerchantCode"] as string ?? ""; }
        set { ViewState["CurrentMerchantCode"] = value; }
    }


    private void LoadAssignItemModal()
    {
        using (var con = new SqlConnection(DBCon))
        {
            con.Open();

            string merchantCode = "";

            // Load modifier info
            using (var h = new SqlCommand(@"
            SELECT TOP 1 Modifier_Grp_Name, Merchant_Code
            FROM dbo.MF_Modifier_Group
            WHERE Modifier_ID = @mid AND (DeleteInd IS NULL OR DeleteInd <> 'X');", con))
            {
                h.Parameters.AddWithValue("@mid", hdModifierId.Value);
                using (var r = h.ExecuteReader())
                {
                    if (r.Read())
                    {
                        var name = r["Modifier_Grp_Name"] as string;
                        lblAssignModifierName.Text = string.IsNullOrEmpty(name) ? "" : "(" + name + ")";
                        merchantCode = r["Merchant_Code"] == DBNull.Value ? "" : (string)r["Merchant_Code"];
                        CurrentMerchantCode = merchantCode;
                    }
                }
            }

            var assigned = new HashSet<string>(StringComparer.OrdinalIgnoreCase);            // Load assigned items from db

            using (var a = new SqlCommand(@"
            SELECT Item_Code
            FROM dbo.mf_Item_Modifier_Assign
            WHERE modifier_ID = @mid AND (deleteind IS NULL OR deleteind <> 'X');", con))
            {
                a.Parameters.AddWithValue("@mid", hdModifierId.Value);
                using (var r = a.ExecuteReader())
                    while (r.Read()) assigned.Add(r.GetString(0));
            }
            AssignedItemCodes = assigned; 

            // Load all items for this merchant
            var adp = new SqlDataAdapter(@"
            SELECT item_code, longdesc, barcode, FilePath
            FROM dbo.mf_item
            WHERE (deleteind IS NULL OR deleteind <> 'X')
              AND (@s = '' OR ISNULL(supplier_code,'') = @s)
            ORDER BY longdesc;", con);
            adp.SelectCommand.Parameters.AddWithValue("@s", merchantCode);

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

        
        BindItemsGrid();

        ScriptManager.RegisterStartupScript(this, this.GetType(), "KeepAssignModal", "$('.modal-backdrop').remove(); $('#ModifierModal').modal({show:true, backdrop:false, keyboard:false});", true);
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
        string sql = @"SELECT Category_Code, Category_Description
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
    ddlCat.Enabled = true; 
}


    private void BindItemsGrid()
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


    protected void ddlDept_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["SelectedDept"] = ddlDept.SelectedValue;
        BindItemsGrid();

        updAssignItems.Update();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "KeepAssignModal", "$('.modal-backdrop').remove(); $('#ModifierModal').modal({show:true, backdrop:false, keyboard:false});", true);
    }

    protected void ddlCat_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["SelectedCat"] = ddlCat.SelectedValue;
        BindItemsGrid();

        updAssignItems.Update();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "KeepAssignModal", "$('.modal-backdrop').remove(); $('#ModifierModal').modal({show:true, backdrop:false, keyboard:false});", true);

    }

    protected void btnSearchItems_Click(object sender, EventArgs e)
    {
        ViewState["SearchText"] = txtItemSearch.Text.Trim();
        BindItemsGrid();

        updAssignItems.Update();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "KeepAssignModal", "$('.modal-backdrop').remove(); $('#ModifierModal').modal({show:true, backdrop:false, keyboard:false});", true);

    }

    protected void btnClearSearch_Click(object sender, EventArgs e)
    {
        txtItemSearch.Text = "";
        ddlDept.SelectedIndex = 0;
        ddlCat.Items.Clear();
        ddlCat.Items.Insert(0, new ListItem("-- Select Category --", ""));
        ddlCat.Enabled = true;

        BindItemsGrid();

        updAssignItems.Update();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "KeepAssignModal", "$('.modal-backdrop').remove(); $('#ModifierModal').modal({show:true, backdrop:false, keyboard:false});", true);

    }



    protected void btnupdateitem_Click(object sender, EventArgs e)
    {
        using (var con = new SqlConnection(DBCon))
        {
            con.Open();

            var assigned = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            using (var cmd = new SqlCommand(@"
            SELECT Item_Code 
            FROM dbo.mf_Item_Modifier_Assign 
            WHERE modifier_ID = @mid AND (deleteind IS NULL OR deleteind <> 'X');", con))
            {
                cmd.Parameters.AddWithValue("@mid", hdModifierId.Value);
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                        assigned.Add(r.GetString(0));
                }
            }

            foreach (GridViewRow row in grd_viewitem.Rows)            // Loop through grid and insert new checked items only

            {
                if (row.RowType != DataControlRowType.DataRow) continue;

                var chk = (CheckBox)row.FindControl("chkitem");
                var hd = (HiddenField)row.FindControl("hditem_code");

                if (chk != null && hd != null && !string.IsNullOrEmpty(hd.Value))
                {
                    if (chk.Checked && !assigned.Contains(hd.Value))
                    {
                        using (var ins = new SqlCommand(@"
                        INSERT INTO dbo.mf_Item_Modifier_Assign
                            (modifier_ID, Item_code, assign_date, deleteind)
                        VALUES
                            (@mid, @icode, DATEADD(HOUR, 8, GETUTCDATE()), '');", con))
                        {
                            ins.Parameters.AddWithValue("@mid", hdModifierId.Value);
                            ins.Parameters.AddWithValue("@icode", hd.Value);
                            ins.ExecuteNonQuery();
                        }
                    }
                    else if (!chk.Checked && assigned.Contains(hd.Value))
                    {
                        using (var del = new SqlCommand(@"
                        UPDATE dbo.mf_Item_Modifier_Assign
                        SET deleteind = 'X'
                        WHERE modifier_ID = @mid AND Item_code = @icode;", con))
                        {
                            del.Parameters.AddWithValue("@mid", hdModifierId.Value);
                            del.Parameters.AddWithValue("@icode", hd.Value);
                            del.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        LoadAssignItemModal();

        ScriptManager.RegisterStartupScript(this, GetType(), "AssignSaved", "if (window.Swal && Swal.fire) { Swal.fire({icon:'success', title:'Item Modifier Updated'}); } " + "else { alert('Item Modifier Updated'); }", true);
    }



    protected void grd_viewitem_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow) return;

        // Checkbox binding
        var hd = (HiddenField)e.Row.FindControl("hditem_code");
        var chk = (CheckBox)e.Row.FindControl("chkitem");
        var assigned = ViewState["AssignedItemCodes"] as HashSet<string>;
        if (assigned != null && hd != null && chk != null)
            chk.Checked = !string.IsNullOrEmpty(hd.Value) && assigned.Contains(hd.Value);

        // Image binding
        var drv = (DataRowView)e.Row.DataItem;
        string filePath = "";
        if (drv["FilePath"] != DBNull.Value)
            filePath = drv["FilePath"].ToString();

        var img = (System.Web.UI.WebControls.Image)e.Row.FindControl("imgItem");
        if (img != null)
        {
            if (string.IsNullOrEmpty(filePath))
                img.ImageUrl = "Images/NoPic.png";
            else
                img.ImageUrl = filePath;

            img.Width = 90;
            img.Height = 90;
        }
    }

    private void UpdateAssignedModifierItemsFromGrid()
    {
        foreach (GridViewRow row in grd_viewitem.Rows)
        {
            if (row.RowType != DataControlRowType.DataRow) continue;
            var chk = (CheckBox)row.FindControl("chkitem");
            var hd = (HiddenField)row.FindControl("hditem_code");
            if (chk != null && hd != null && !string.IsNullOrEmpty(hd.Value))
            {
                if (chk.Checked)
                    AssignedItemCodes.Add(hd.Value);
                else
                    AssignedItemCodes.Remove(hd.Value);
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
