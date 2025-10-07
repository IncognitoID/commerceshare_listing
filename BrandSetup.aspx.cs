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
public partial class BrandSetup : System.Web.UI.Page
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

            txt_BrdCode.Attributes["onkeydown"] = "doNext('" + txt_BrdDesc.ClientID + "',event)";

            txt_BrdCode.Attributes.Add("onfocus", "this.select();");
            txt_BrdDesc.Attributes.Add("onfocus", "this.select();");

            btn_Insert.Visible = true;
            btn_Update.Visible = false;

            if (Request.QueryString["id"].ToString() != "")
            {
                btn_Insert.Visible = false;
                btn_Update.Visible = true;

                using (SqlConnection mySConn = new SqlConnection(MF_Con.Text))
                {
                    using (SqlCommand myScmd = new SqlCommand("Load_Brd_Info", mySConn))
                    {
                        myScmd.CommandType = CommandType.StoredProcedure;
                        myScmd.Parameters.AddWithValue("@BrdID", Request.QueryString["id"].ToString());

                        mySConn.Open();

                        try
                        {
                            SqlDataReader idr = myScmd.ExecuteReader();

                            if (idr.HasRows == true)
                            {
                                DataTable g = new DataTable();
                                g.Load(idr);

                                txt_BrdCode.Text = g.Rows[0]["Brand_Code"].ToString();
                                txt_BrdDesc.Text = g.Rows[0]["Brand_Desc"].ToString();

                            }
                        }
                        catch (Exception ex)
                        {
                            throw (ex);
                        }
                        finally
                        {
                            myScmd.Dispose();
                            mySConn.Close();
                        }
                    }
                }
            }
            else
            {
                using (SqlConnection mySConn = new SqlConnection(MF_Con.Text))
                {
                    mySConn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT Brd_runno as abc from BrdCode_runno where Supplier_Code = '" + Request.QueryString["merchant"] + "' ", mySConn);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            count = Convert.ToInt32((reader["abc"]).ToString());
                            count += 1;
                            txt_BrdCode.Text += Request.QueryString["merchant"] + "-B" + count.ToString();

                        }
                        reader.Close();
                        reader.Dispose();
                        mySConn.Close();
                    }
                    else
                    {
                        txt_BrdCode.Text = Request.QueryString["merchant"] + "-B10001";
                        using (SqlConnection conn = new SqlConnection(MF_Con.Text))
                        {
                            conn.Open();
                            using (SqlCommand comd = new SqlCommand("insert into BrdCode_runno(supplier_Code,Brd_runno) values('" + Request.QueryString["merchant"] + "', @Brdrunno)", conn))
                            {
                                comd.Parameters.AddWithValue("@Brdrunno", "10000");
                                comd.ExecuteNonQuery();
                            }
                            conn.Close();
                        }

                    }

                }
            }

            using (SqlConnection myTxConn = new SqlConnection(MF_Con.Text))
            {
                using (SqlCommand myTxcmd = new SqlCommand("Load_Img_runno", myTxConn))
                {
                    myTxcmd.CommandType = CommandType.StoredProcedure;
                    //myTxcmd.Parameters.AddWithValue("@PublishID", Session["PublishID"].ToString());

                    myTxConn.Open();
                    try
                    {
                        SqlDataReader idr = myTxcmd.ExecuteReader();

                        if (idr.HasRows == true)
                        {
                            DataTable g = new DataTable();
                            g.Load(idr);

                            lblrunno.Text = g.Rows[0]["runno"].ToString();

                        }
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
            }
        }
    }

    protected void Insert_OnClick(object sender, EventArgs e)
    {
        if (txt_BrdCode.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Brand code cannot be empty.');", true);
            txt_BrdCode.Focus();
        }
        else if (txt_BrdDesc.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Brand description cannot be empty.');", true);
            txt_BrdDesc.Focus();
        }
        else
        {
            Int32 rv = 1;

            using (SqlConnection myTxConn = new SqlConnection(MF_Con.Text))
            {
                using (SqlCommand myTxcmd = new SqlCommand("Check_Brd", myTxConn))
                {
                    myTxcmd.CommandType = CommandType.StoredProcedure;
                    myTxcmd.Parameters.AddWithValue("@BrdCode", txt_BrdCode.Text.Trim().Replace("'", "`"));
                    myTxcmd.Parameters.AddWithValue("@supplier", Request.QueryString["merchant"].ToString());

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

                ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Brand code exist in database before.');", true);
                txt_BrdCode.Focus();
            }
            else
            {



                Int32 tv = 50;

                using (SqlConnection myTxConn = new SqlConnection(MF_Con.Text))
                {


                    using (SqlCommand myTxcmd = new SqlCommand("Insert_Brd2", myTxConn))
                    {
                        myTxcmd.CommandType = CommandType.StoredProcedure;
                        myTxcmd.Parameters.AddWithValue("@BrdCode", txt_BrdCode.Text.Trim().Replace("'", "`"));
                        myTxcmd.Parameters.AddWithValue("@BrdDesc", txt_BrdDesc.Text.Trim().Replace("'", "`"));
                        myTxcmd.Parameters.AddWithValue("@supplier", Request.QueryString["merchant"].ToString());

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

                        tv = Convert.ToInt32(TReturnValue.Value.ToString());
                    }


                    if (tv == 100)
                    {
                        using (SqlConnection mySConn = new SqlConnection(MF_Con.Text))
                        {
                            mySConn.Open();
                            SqlCommand cmd = new SqlCommand("SELECT Brd_runno as abc from BrdCode_runno where Supplier_Code = '" + Request.QueryString["merchant"] + "' ", mySConn);

                            SqlDataReader reader = cmd.ExecuteReader();
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    count = Convert.ToInt32((reader["abc"]).ToString());
                                    count += 1;
                                    using (SqlConnection conn1 = new SqlConnection(MF_Con.Text))
                                    {
                                        conn1.Open();
                                        using (SqlCommand comd = new SqlCommand("update BrdCode_runno set Brd_runno= @Brdrunno where supplier_code='" + Request.QueryString["merchant"] + "'", conn1))
                                        {
                                            comd.Parameters.AddWithValue("@Brdrunno", count);
                                            comd.ExecuteNonQuery();
                                        }
                                        conn1.Close();
                                    }

                                }
                                reader.Close();
                                reader.Dispose();
                                mySConn.Close();


                            }
                        }

                        Session["PName"] = "Department Listing";
                        ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Insert success.');window.location ='BrandSetupList.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "';", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Insert failed. Please try again later.');", true);
                        txt_BrdCode.Focus();
                    }
                }
            }
        }
    }

    protected void Back_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("BrandSetupList.aspx?merchant=" + Request.QueryString["merchant"].ToString());
    }

    protected void Update_OnClick(object sender, EventArgs e)
    {
        if (txt_BrdCode.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Brand code cannot be empty.');", true);
            txt_BrdCode.Focus();
        }
        else if (txt_BrdDesc.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Brand description cannot be empty.');", true);
            txt_BrdDesc.Focus();
        }
        else
        {          
            Int32 tv = 50;

            using (SqlConnection myTxConn = new SqlConnection(MF_Con.Text))
            {
                using (SqlCommand myTxcmd = new SqlCommand("Update_Brd2", myTxConn))
                {
                    myTxcmd.CommandType = CommandType.StoredProcedure;
                    myTxcmd.Parameters.AddWithValue("@BrdCode", txt_BrdCode.Text.Trim().Replace("'", "`"));
                    myTxcmd.Parameters.AddWithValue("@BrdDesc", txt_BrdDesc.Text.Trim().Replace("'", "`"));
                    myTxcmd.Parameters.AddWithValue("@supplier", Request.QueryString["merchant"].ToString());
                    myTxcmd.Parameters.AddWithValue("@BrdID", Convert.ToInt32(Request.QueryString["id"].ToString()));

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

                    tv = Convert.ToInt32(TReturnValue.Value.ToString());
                }


                if (tv == 100)
                {


                    Session["PName"] = "Department Listing";
                    ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Update success.');window.location ='BrandSetupList.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "';", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Update failed. Please try again later.');", true);
                    txt_BrdCode.Focus();
                }
            }
            //}
        }
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