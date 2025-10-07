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

public partial class DepartmentSetup : System.Web.UI.Page
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

            txt_DeptCode.Attributes["onkeydown"] = "doNext('" + txt_DeptDesc.ClientID + "',event)";

            txt_DeptCode.Attributes.Add("onfocus", "this.select();");
            txt_DeptDesc.Attributes.Add("onfocus", "this.select();");

            btn_Insert.Visible = true;
            btn_Update.Visible = false;

            if (Request.QueryString["Deptid"].ToString() != "")
            {
                btn_Insert.Visible = false;
                btn_Update.Visible = true;

                using (SqlConnection mySConn = new SqlConnection(MF_Con.Text))
                {
                    using (SqlCommand myScmd = new SqlCommand("Load_Dept_Info", mySConn))
                    {
                        myScmd.CommandType = CommandType.StoredProcedure;
                        myScmd.Parameters.AddWithValue("@DeptID", Request.QueryString["Deptid"].ToString());

                        mySConn.Open();

                        try
                        {
                            SqlDataReader idr = myScmd.ExecuteReader();

                            if (idr.HasRows == true)
                            {
                                DataTable g = new DataTable();
                                g.Load(idr);

                                txt_DeptCode.Text = g.Rows[0]["Department_Code"].ToString();
                                txt_DeptDesc.Text = g.Rows[0]["Department_Description"].ToString();

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
                    SqlCommand cmd = new SqlCommand("SELECT Dept_runno as abc from Deptcode_runno where Supplier_Code = '" + Request.QueryString["merchant"] + "' ", mySConn);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                                count = Convert.ToInt32((reader["abc"]).ToString());
                                count += 1;
                            txt_DeptCode.Text += Request.QueryString["merchant"] + "-D" + count.ToString();
                           
                        }
                        reader.Close();
                        reader.Dispose();
                        mySConn.Close();
                    }
                    else
                    {
                            txt_DeptCode.Text = Request.QueryString["merchant"] + "-D1001";
                        using (SqlConnection conn = new SqlConnection(MF_Con.Text))
                        {
                            conn.Open();
                            using (SqlCommand comd = new SqlCommand("insert into deptcode_runno(supplier_Code,Dept_runno) values('" + Request.QueryString["merchant"] + "', @deptrunno)", conn))
                            {
                                comd.Parameters.AddWithValue("@deptrunno", "1001");
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
        if (txt_DeptCode.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Department code cannot be empty.');", true);
            txt_DeptCode.Focus();
        }
        else if (txt_DeptDesc.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Department description cannot be empty.');", true);
            txt_DeptDesc.Focus();
        }
        else
        {
            Int32 rv = 1;

            using (SqlConnection myTxConn = new SqlConnection(MF_Con.Text))
            {
                using (SqlCommand myTxcmd = new SqlCommand("Check_Dept2", myTxConn))
                {
                    myTxcmd.CommandType = CommandType.StoredProcedure;
                    myTxcmd.Parameters.AddWithValue("@DeptCode", txt_DeptCode.Text.Trim().Replace("'", "`"));
                    myTxcmd.Parameters.AddWithValue("@supplier_code", Request.QueryString["merchant"].ToString());

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

                ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Department code exist in database before.');", true);
                txt_DeptCode.Focus();
            }
            else
            {
               


                Int32 tv = 50;

                using (SqlConnection myTxConn = new SqlConnection(MF_Con.Text))
                {
                   

                    using (SqlCommand myTxcmd = new SqlCommand("Insert_Dept2", myTxConn))
                    {
                        myTxcmd.CommandType = CommandType.StoredProcedure;
                        myTxcmd.Parameters.AddWithValue("@DeptCode", txt_DeptCode.Text.Trim().Replace("'", "`"));
                        myTxcmd.Parameters.AddWithValue("@DeptDesc", txt_DeptDesc.Text.Trim().Replace("'", "`"));
                        myTxcmd.Parameters.AddWithValue("@supplier_code", Request.QueryString["merchant"].ToString());

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
                            SqlCommand cmd = new SqlCommand("SELECT Dept_runno as abc from Deptcode_runno where Supplier_Code = '" + Request.QueryString["merchant"].ToString() + "' ", mySConn);

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
                                        using (SqlCommand comd = new SqlCommand("update deptcode_runno set Dept_runno= @deptrunno where supplier_code='" + Request.QueryString["merchant"].ToString() + "'", conn1))
                                        {
                                            comd.Parameters.AddWithValue("@deptrunno", count);
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
                        ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Insert success.');window.location ='DepartmentSetupList.aspx?merchant="+ Request.QueryString["merchant"].ToString() + "';", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Insert failed. Please try again later.');", true);
                        txt_DeptCode.Focus();
                    }
                }
            }
        }
    }

    protected void Back_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("DepartmentSetupList.aspx?merchant=" + Request.QueryString["merchant"].ToString());
    }

    protected void Update_OnClick(object sender, EventArgs e)
    {
        if (txt_DeptCode.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Department code cannot be empty.');", true);
            txt_DeptCode.Focus();
        }
        else if (txt_DeptDesc.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Department description cannot be empty.');", true);
            txt_DeptDesc.Focus();
        }
        else
        {
            Int32 rv = 1;

            //using (SqlConnection myTxConn = new SqlConnection(MF_Con.Text))
            //{
            //    using (SqlCommand myTxcmd = new SqlCommand("Check_Dept2", myTxConn))
            //    {
            //        myTxcmd.CommandType = CommandType.StoredProcedure;
            //        myTxcmd.Parameters.AddWithValue("@DeptCode", txt_DeptCode.Text.Trim().Replace("'", "`"));
            //        myTxcmd.Parameters.AddWithValue("@DeptID", Session["Department_ID"].ToString());

            //        SqlParameter SReturnValue = myTxcmd.Parameters.Add("returnValue", SqlDbType.Int, 4);
            //        SReturnValue.Direction = ParameterDirection.ReturnValue;

            //        myTxConn.Open();

            //        try
            //        {
            //            myTxcmd.ExecuteNonQuery();
            //        }
            //        catch (Exception ex)
            //        {
            //            throw (ex);
            //        }
            //        finally
            //        {
            //            myTxcmd.Dispose();
            //            myTxConn.Close();
            //        }

            //        rv = Convert.ToInt32(SReturnValue.Value.ToString());
            //    }
            //}

            //if (rv == 1)
            //{
            //    ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Department code exist in database before.');", true);
            //    txt_DeptCode.Focus();
            //}
            //else
            //{
                Int32 tv = 50;

                using (SqlConnection myTxConn = new SqlConnection(MF_Con.Text))
                {
                    using (SqlCommand myTxcmd = new SqlCommand("Update_Dept2", myTxConn))
                    {
                        myTxcmd.CommandType = CommandType.StoredProcedure;
                        myTxcmd.Parameters.AddWithValue("@DeptCode", txt_DeptCode.Text.Trim().Replace("'", "`"));
                        myTxcmd.Parameters.AddWithValue("@DeptDesc", txt_DeptDesc.Text.Trim().Replace("'", "`"));
                        myTxcmd.Parameters.AddWithValue("@supplier_code", Request.QueryString["merchant"].ToString());
                        myTxcmd.Parameters.AddWithValue("@DeptID", Convert.ToInt32(Request.QueryString["DeptID"].ToString()));

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
                        ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Update success.');window.location ='DepartmentSetupList.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "';", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Update failed. Please try again later.');", true);
                        txt_DeptCode.Focus();
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