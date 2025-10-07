using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.SqlClient;
using System.Globalization;
using System.Collections.Generic;
using System.Xml.Linq;
using System.IO;
using AjaxControlToolkit;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Text;

public partial class Supplier : System.Web.UI.Page
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
            MF_Con.Text = DBCon;

            txt_SupplierCode.Attributes["onkeydown"] = "doNext('" + txt_SupplierName.ClientID + "',event)";
            txt_SupplierCode.Attributes.Add("onfocus", "this.select();");
            txt_SupplierName.Attributes.Add("onfocus", "this.select();");

            btn_Insert.Visible = true;
            btn_Update.Visible = false;

            //string supplierCode = Request.QueryString["supplier"]; 

            /*if (!string.IsNullOrEmpty(merchant))
            {
                btn_Insert.Visible = false;
                btn_Update.Visible = true;
                LoadSupplierByCode(merchant); // Load all data for that Supplier_Code
            }
            else if (!string.IsNullOrEmpty(supplierCode))
            {
                btn_Insert.Visible = false;
                btn_Update.Visible = true;

                using (SqlConnection conn = new SqlConnection(DBCon))
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT * FROM MF_Supplier WHERE Supplier_Code=@Supplier_Code", conn))
                {
                    cmd.Parameters.AddWithValue("@Supplier_Code", supplierCode); 
                    conn.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        txt_SupplierCode.Text = rdr["Supplier_Code"].ToString();
                        txt_SupplierName.Text = rdr["Supplier_Name"].ToString();
                        txt_ContactNo.Text = rdr["Contact_No"].ToString();
                        txt_Email.Text = rdr["Email"].ToString();
                        txt_TIN.Text = rdr["Supplier_TIN_No"].ToString();

                        hf_OriginalSupplierCode.Value = rdr["Supplier_Code"].ToString();
                    }
                }
            }
            else
            {
                txt_SupplierCode.Text = GenerateNextSupplierCode();
            }*/

            string supplier = Request.QueryString["supplier"];

            if (!string.IsNullOrEmpty(supplier))
            {
                btn_Insert.Visible = false;
                btn_Update.Visible = true;

                using (SqlConnection conn = new SqlConnection(DBCon))
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM MF_Supplier WHERE Supplier_Code=@Supplier_Code", conn))
                {
                    cmd.Parameters.AddWithValue("@Supplier_Code", supplier);
                    conn.Open();

                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        txt_SupplierCode.Text = rdr["Supplier_Code"].ToString();
                        txt_SupplierName.Text = rdr["Supplier_Name"].ToString();
                        txt_ContactNo.Text = rdr["Contact_No"].ToString();
                        txt_Email.Text = rdr["Email"].ToString();
                        txt_TIN.Text = rdr["Supplier_TIN_No"].ToString();

                        hf_OriginalSupplierCode.Value = rdr["Supplier_Code"].ToString();
                    }

                }

            }
            else
            {
                string merchant = Request.QueryString["merchant"];
                txt_SupplierCode.Text = GenerateNextSupplierCode(merchant);

                btn_Insert.Visible = true;
                btn_Update.Visible = false;
            }

        }
    }



    private void LoadSupplierByCode(string code)
    {

        using (SqlConnection conn = new SqlConnection(DBCon))
        using (SqlCommand cmd = new SqlCommand("SELECT * FROM MF_Supplier WHERE Supplier_Code = @SupplierCode", conn))
        {
            cmd.Parameters.AddWithValue("@SupplierCode", code);
            conn.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                txt_SupplierCode.Text = rdr["Supplier_Code"].ToString();
                txt_SupplierName.Text = rdr["Supplier_Name"].ToString();
                txt_ContactNo.Text = rdr["Contact_No"].ToString();
                txt_Email.Text = rdr["Email"].ToString();
                txt_TIN.Text = rdr["Supplier_TIN_No"].ToString();
            }
        }
    }

    private void LoadSupplierById(string id)
    {
        btn_Insert.Visible = false;
        btn_Update.Visible = true;

        using (SqlConnection conn = new SqlConnection(DBCon))
        using (SqlCommand cmd = new SqlCommand("Load_Supplier_Info", conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Ids", id);

            conn.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.HasRows)
            {
                DataTable dt = new DataTable();
                dt.Load(rdr);

                txt_SupplierCode.Text = dt.Rows[0]["Supplier_Code"].ToString();
                txt_SupplierName.Text = dt.Rows[0]["Supplier_Name"].ToString();
                txt_ContactNo.Text = dt.Rows[0]["Contact_No"].ToString();
                txt_Email.Text = dt.Rows[0]["Email"].ToString();
                txt_TIN.Text = dt.Rows[0]["Supplier_TIN_No"].ToString();
            }
        }
    }

    private void LoadImageRunNo()
    {
        using (SqlConnection conn = new SqlConnection(DBCon))
        using (SqlCommand cmd = new SqlCommand("Load_Img_runno", conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.HasRows)
            {
                DataTable dt = new DataTable();
                dt.Load(rdr);
                lblrunno.Text = dt.Rows[0]["runno"].ToString();
            }
        }
    }

    private string GenerateNextSupplierCode(string merchant)
    {
        if (string.IsNullOrEmpty(merchant))
            throw new Exception("Merchant code is required to generate Supplier Code.");

        using (SqlConnection con = new SqlConnection(DBCon))
        using (SqlCommand cmd = new SqlCommand(@"
        ;WITH c AS
        (
            SELECT Supplier_Code,
                   TRY_CAST(SUBSTRING(Supplier_Code, LEN(@Merchant) + 2, LEN(Supplier_Code)) AS INT) AS num
            FROM MF_Supplier
            WHERE Supplier_Code LIKE @Merchant + '-%'
        )
        SELECT ISNULL(MAX(num),0) FROM c;", con)) // extract numeric part of suppliercode after merchant
        {
            cmd.Parameters.AddWithValue("@Merchant", merchant);

            con.Open();
            int maxNum = Convert.ToInt32(cmd.ExecuteScalar());
            int next = maxNum + 1;

            return merchant + "-" + next.ToString("D5");
        }
    }




    protected void Insert_OnClick(object sender, EventArgs e)
    {
        if (txt_SupplierCode.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Supplier code cannot be empty.');", true);
            txt_SupplierCode.Focus();
            return;
        }
        else if (txt_SupplierName.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Supplier Name cannot be empty.');", true);
            txt_SupplierName.Focus();
            return;
        }
        else if (!IsValidEmail(txt_Email.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Please enter a valid email.');", true);
            txt_Email.Focus();
            return;
        }

        using (SqlConnection conn = new SqlConnection(DBCon))
        {
            using (SqlCommand cmd = new SqlCommand("INSERT INTO MF_Supplier " +
                "(Supplier_Code, Supplier_Name, Contact_No, Email, Supplier_TIN_No, Deleteind, Created_Dt) " +
                "VALUES (@Supplier_Code, @Supplier_Name, @Contact_No, @Email, @Supplier_TIN_No, '', GETDATE())", conn))
            {
                cmd.Parameters.AddWithValue("@Supplier_Code", txt_SupplierCode.Text.Trim());
                cmd.Parameters.AddWithValue("@Supplier_Name", txt_SupplierName.Text.Trim());
                cmd.Parameters.AddWithValue("@Contact_No", txt_ContactNo.Text.Trim());
                cmd.Parameters.AddWithValue("@Email", txt_Email.Text.Trim());
                cmd.Parameters.AddWithValue("@Supplier_TIN_No", string.IsNullOrEmpty(txt_TIN.Text.Trim()) ? (object)DBNull.Value : txt_TIN.Text.Trim());

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        string merchant = Request.QueryString["merchant"] ?? "";
        ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode","alert('Insert success.');window.location ='suppliersetuplist.aspx?merchant=" + merchant + "';", true);
    }


    protected void Update_OnClick(object sender, EventArgs e)
    {
        string originalCode = hf_OriginalSupplierCode.Value;

        if (string.IsNullOrEmpty(txt_SupplierName.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Supplier Name cannot be empty.');", true);
            return;
        }

        using (SqlConnection conn = new SqlConnection(DBCon))
        using (SqlCommand cmd = new SqlCommand(
            @"UPDATE MF_Supplier 
          SET Supplier_Name=@Supplier_Name, 
              Contact_No=@Contact_No, 
              Email=@Email, 
              Supplier_TIN_No=@Supplier_TIN_No, 
              Modified_Dt=GETDATE() 
          WHERE Supplier_Code=@Supplier_Code", conn))
        {
            cmd.Parameters.AddWithValue("@Supplier_Name", txt_SupplierName.Text.Trim());
            cmd.Parameters.AddWithValue("@Contact_No", txt_ContactNo.Text.Trim());
            cmd.Parameters.AddWithValue("@Email", txt_Email.Text.Trim());
            cmd.Parameters.AddWithValue("@Supplier_TIN_No", string.IsNullOrEmpty(txt_TIN.Text.Trim()) ? (object)DBNull.Value : txt_TIN.Text.Trim());
            cmd.Parameters.AddWithValue("@Supplier_Code", originalCode);

            conn.Open();
            int rowsAffected = cmd.ExecuteNonQuery();
            conn.Close();

            if (rowsAffected > 0)
            {
                string merchant = Request.QueryString["merchant"] ?? "";
                ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode","alert('Update success.');window.location ='suppliersetuplist.aspx?merchant=" + merchant + "';", true);

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode",
                    "alert('Update failed. Supplier not found.');", true);
            }
        }
    }


    protected void Delete_OnClick(object sender, EventArgs e)
    {
        using (SqlConnection conn = new SqlConnection(DBCon))
        {
            using (SqlCommand cmd = new SqlCommand("UPDATE MF_Supplier SET Deleteind='x', Modified_Dt=GETDATE() WHERE Ids=@Ids", conn))
            {
                cmd.Parameters.AddWithValue("@Ids", Convert.ToInt32(Request.QueryString["Ids"].ToString()));

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        string merchant = Request.QueryString["merchant"] ?? "";
        ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode","alert('Deleted successfully.');window.location ='suppliersetuplist.aspx?merchant=" + merchant + "';", true);

    }

    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    protected void Back_OnClick(object sender, EventArgs e)
    {
        string merchant = Request.QueryString["merchant"] ?? "";
        Response.Redirect("suppliersetuplist.aspx?merchant=" + merchant);
    }

    public static System.Drawing.Image ScaleImage(System.Drawing.Image image, int maxHeight)
    {
        var ratio = 1.00;

        if (image.Height > image.Width)
        {
            ratio = (double)maxHeight / image.Height;
        }
        else
        {
            ratio = (double)maxHeight / image.Width;

        }

        //var newWidth = (int)(size.Width);
        //var newHeight = (int)(size.Height);
        var newWidth = (int)(image.Width * ratio);
        var newHeight = (int)(image.Height * ratio);
        var newImage = new Bitmap(newWidth, newHeight);
        using (var g = Graphics.FromImage(newImage))
        {
            g.DrawImage(image, 0, 0, newWidth, newHeight);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.CompositingQuality = CompositingQuality.HighQuality;

        }
        return newImage;
    }

}