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
using System.IO;
using System.Drawing;
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
public partial class CategorySetup : System.Web.UI.Page
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

            txt_CategoryCode.Attributes["onkeydown"] = "doNext('" + txt_CategoryDesc.ClientID + "',event)";

            txt_CategoryCode.Attributes.Add("onfocus", "this.select();");
            txt_CategoryDesc.Attributes.Add("onfocus", "this.select();");
            txt_Filter.Attributes.Add("onfocus", "this.select();");

            txt_Dept.Text = ddlDept.SelectedValue;
            btn_Insert.Visible = true;
            btn_Update.Visible = false;

            LoadDepartment();

            if (Request.QueryString["catid"].ToString() != "")
            {
                btn_Insert.Visible = false;
                btn_Update.Visible = true;

                using (SqlConnection mySConn = new SqlConnection(MF_Con.Text))
                {
                    using (SqlCommand myScmd = new SqlCommand("Load_Cat_Info", mySConn))
                    {
                        myScmd.CommandType = CommandType.StoredProcedure;
                        myScmd.Parameters.AddWithValue("@CatID", Request.QueryString["catid"].ToString());

                        mySConn.Open();

                        try
                        {
                            SqlDataReader idr = myScmd.ExecuteReader();

                            if (idr.HasRows == true)
                            {
                                DataTable g = new DataTable();
                                g.Load(idr);

                                txt_CategoryCode.Text = g.Rows[0]["Category_Code"].ToString();
                                txt_CategoryDesc.Text = g.Rows[0]["Category_Description"].ToString();
                                txt_Dept.Text = g.Rows[0]["Department_Code"].ToString();
                                ddlDept.Text = g.Rows[0]["Department_Code"].ToString();

                                if (String.IsNullOrEmpty(g.Rows[0]["ImgFilePath"].ToString()) == true)
                                {
                                    Img1.Src = "Images/NoPic.png";
                                    Img1.Attributes.Add("Width", "185px");
                                }
                                else
                                {

                                    Img1.Src = g.Rows[0]["ImgFilePath"].ToString();
                                }

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

                txt_CategoryCode.Focus();
            }
            else
            {
                txt_CategoryCode.Focus();
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
        // }
    }

    protected void Insert_OnClick(object sender, EventArgs e)
    {
        if (txt_CategoryCode.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Category code cannot be empty.');", true);
            txt_CategoryCode.Focus();
        }
        else if (txt_CategoryDesc.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Category description cannot be empty.');", true);
            txt_CategoryDesc.Focus();
        }
        else if (txt_Dept.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Department code cannot be empty.');", true);
            txt_Dept.Focus();
        }
        else
        {
            Int32 rv = 1;

            using (SqlConnection myTxConn = new SqlConnection(MF_Con.Text))
            {
                using (SqlCommand myTxcmd = new SqlCommand("Check_Cat2", myTxConn))
                {
                    myTxcmd.CommandType = CommandType.StoredProcedure;
                    myTxcmd.Parameters.AddWithValue("@CatCode", txt_CategoryCode.Text.Trim().Replace("'", "`"));
                    myTxcmd.Parameters.AddWithValue("@CatID", "");

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
                ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Category code exist in database before.');", true);
                txt_CategoryCode.Focus();
            }
            else
            {
                Int32 tv = 50;

                using (SqlConnection myTxConn = new SqlConnection(MF_Con.Text))
                {
                    using (SqlCommand myTxcmd = new SqlCommand("Insert_Cat2", myTxConn))
                    {
                        myTxcmd.CommandType = CommandType.StoredProcedure;
                        myTxcmd.Parameters.AddWithValue("@CatCode", txt_CategoryCode.Text.Trim().Replace("'", "`"));
                        myTxcmd.Parameters.AddWithValue("@CatDesc", txt_CategoryDesc.Text.Trim().Replace("'", "`"));
                        myTxcmd.Parameters.AddWithValue("@DeptCode", txt_Dept.Text.Trim().Replace("'", "`"));
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
                        if ((fileupload2.PostedFile != null) && (fileupload2.PostedFile.ContentLength > 0))
                        {

                            if (lblrunno.Text == "")
                            {
                                using (SqlConnection myTxConn2 = new SqlConnection(DBCon))
                                {
                                    using (SqlCommand myTxcmd2 = new SqlCommand("Insert_Img_runno", myTxConn2))
                                    {
                                        myTxcmd2.CommandType = CommandType.StoredProcedure;
                                        myTxcmd2.Parameters.AddWithValue("@runno", "11");

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

                                        myTxcmd3.Parameters.AddWithValue("@runno", "11");


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

                                SqlConnection con100 = new SqlConnection(DBCon);
                                SqlCommand cmd100 = new SqlCommand("Load_Img_runno", con100);
                                con100.Open();
                                SqlDataAdapter da100 = new SqlDataAdapter(cmd100);
                                DataTable dt100 = new DataTable();
                                da100.Fill(dt100);

                                lblrunno.Text = dt100.Rows[0]["runno"].ToString();

                                con100.Close();

                                string runno = lblrunno.Text;
                                string extension = Path.GetExtension(fileupload2.PostedFile.FileName);
                                string FileNameU = "IMG-CATE" + "-" + DateTime.UtcNow.AddHours(8).ToString("yyMMddHHmmss") + runno.ToString() + extension;
                                string TbmFileNameU = "Tbm-IMG-CATE" + "-" + DateTime.UtcNow.AddHours(8).ToString("yyMMddHHmmss") + runno.ToString() + extension;
                                string TbmFileNameRESize = "Tbm-IMG-CATE" + "-" + DateTime.UtcNow.AddHours(8).ToString("yyMMddHHmmss") + runno.ToString();


                                string path = Server.MapPath("CategoryImg/" + "Normal-Img" + "/");
                                string delpath = Server.MapPath("CategoryImg/" + "Thumbnail-Img" + "/" + TbmFileNameU);

                                if (Directory.Exists(path))
                                {
                                    fileupload2.PostedFile.SaveAs(Server.MapPath("CategoryImg/" + "Normal-Img" + "/" + FileNameU));
                                    fileupload2.PostedFile.SaveAs(Server.MapPath("CategoryImg/" + "Thumbnail-Img" + "/" + TbmFileNameU));
                                    string pth = Server.MapPath("CategoryImg/" + "Thumbnail-Img" + "/" + TbmFileNameU);
                                    resizeImageAndSave(pth);


                                    using (SqlConnection myQRConn = new SqlConnection(DBCon))
                                    {
                                        using (SqlCommand QRcmd = new SqlCommand("update MF_Category_miso set Thumbnail_FilePath=@thumbnail,ImgFilePath=@photo where Category_Code='" + txt_CategoryCode.Text.Trim().Replace("'", "`") + "' and DeleteInd <> 'X'", myQRConn))
                                        {
                                            QRcmd.Parameters.AddWithValue("@photo", @"https://ezyshare.online/EzyShareListing/CategoryImg/" + "Normal-Img" + "/" + FileNameU);
                                            QRcmd.Parameters.AddWithValue("@thumbnail", @"https://ezyshare.online/EzyShareListing/CategoryImg/" + "Thumbnail-Img" + "/" + TbmFileNameRESize + "_Thumbnail" + extension);
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

                                    //using (SqlConnection myQRConn = new SqlConnection(DBCon))
                                    //{
                                    //    using (SqlCommand QRcmd = new SqlCommand("Insert into MF_Multi_Img_miso (Item_Code, Main_Indicator, Default_Indicator,Modified_DT,DeleteInd,FilePath,Thumbnail_FilePath)" +
                                    //        "Values(@ItemCode,@Main_Indicator,@Default_Indicator,DATEADD(HOUR, 8, CONVERT(varchar(20),GETUTCDATE(),120)),'',@FilePath,@Thumbnail_FilePath)", myQRConn))
                                    //    {
                                    //        QRcmd.Parameters.AddWithValue("@ItemCode", txt_CategoryCode.Text.Trim().Replace("'", "`"));



                                    //        QRcmd.Parameters.AddWithValue("@Main_Indicator", "True");
                                    //        if (fileupload2.PostedFile != null)
                                    //        {
                                    //            QRcmd.Parameters.AddWithValue("@Default_Indicator", "1");
                                    //        }
                                    //        else
                                    //        {
                                    //            QRcmd.Parameters.AddWithValue("@Default_Indicator", "0");
                                    //        }
                                    //        QRcmd.Parameters.AddWithValue("@FilePath", @"https://ezyshare.online/EzyShareListing/CategoryImg/" + "Normal-Img" + "/" + FileNameU);
                                    //        QRcmd.Parameters.AddWithValue("@Thumbnail_FilePath", @"https://ezyshare.online/EzyShareListing/CategoryImg/" + "Thumbnail-Img" + "/" + TbmFileNameRESize + "_Thumbnail" + extension);
                                    //        myQRConn.Open();

                                    //        try
                                    //        {
                                    //            QRcmd.ExecuteNonQuery();
                                    //        }
                                    //        catch (Exception ex)
                                    //        {
                                    //            throw (ex);
                                    //        }
                                    //        finally
                                    //        {
                                    //            myQRConn.Close();
                                    //        }

                                    //    }


                                    //}

                                }
                                else
                                {
                                    System.IO.Directory.CreateDirectory(Server.MapPath("CategoryImg/" + "Normal-Img" + "/"));
                                    System.IO.Directory.CreateDirectory(Server.MapPath("CategoryImg/" + "Thumbnail-Img" + "/"));

                                    fileupload2.PostedFile.SaveAs(Server.MapPath("CategoryImg/" + "Normal-Img" + "/" + FileNameU));
                                    fileupload2.PostedFile.SaveAs(Server.MapPath("CategoryImg/" + "Thumbnail-Img" + "/" + TbmFileNameU));
                                    string pth = Server.MapPath("CategoryImg/" + "Thumbnail-Img" + "/" + TbmFileNameU);
                                    resizeImageAndSave(pth);


                                    using (SqlConnection myQRConn = new SqlConnection(DBCon))
                                    {
                                        using (SqlCommand QRcmd = new SqlCommand("update MF_Category_miso set Thumbnail_FilePath=@thumbnail,ImgFilePath=@photo where Item_Code='" + txt_CategoryCode.Text.Trim().Replace("'", "`") + "' and DeleteInd <> 'X'", myQRConn))
                                        {
                                            QRcmd.Parameters.AddWithValue("@photo", @"https://ezyshare.online/EzyShareListing/CategoryImg/" + "Normal-Img" + "/" + FileNameU);
                                            QRcmd.Parameters.AddWithValue("@thumbnail", @"https://ezyshare.online/EzyShareListing/CategoryImg/" + "Thumbnail-Img" + "/" + TbmFileNameRESize + "_Thumbnail" + extension);

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

                                    //using (SqlConnection myQRConn = new SqlConnection(DBCon))
                                    //{
                                    //    using (SqlCommand QRcmd = new SqlCommand("Insert into MF_Multi_Img_miso (Item_Code, Main_Indicator, Default_Indicator,Modified_DT,DeleteInd,FilePath,Thumbnail_FilePath)" +
                                    //        "Values(@ItemCode,@Main_Indicator,@Default_Indicator,DATEADD(HOUR, 8, CONVERT(varchar(20),GETUTCDATE(),120)),'',@FilePath, @Thumbnail_FilePath)", myQRConn))
                                    //    {
                                    //        QRcmd.Parameters.AddWithValue("@ItemCode", txt_CategoryCode.Text.Trim().Replace("'", "`"));



                                    //        QRcmd.Parameters.AddWithValue("@Main_Indicator", "True");
                                    //        if (fileupload2.PostedFile != null)
                                    //        {
                                    //            QRcmd.Parameters.AddWithValue("@Default_Indicator", "1");
                                    //        }
                                    //        else
                                    //        {
                                    //            QRcmd.Parameters.AddWithValue("@Default_Indicator", "0");
                                    //        }

                                    //        QRcmd.Parameters.AddWithValue("@FilePath", @"https://ezyshare.online/EzyShareListing/CategoryImg/" + "Normal-Img" + "/" + FileNameU);
                                    //        QRcmd.Parameters.AddWithValue("@Thumbnail_FilePath", @"https://ezyshare.online/EzyShareListing/CategoryImg/" + "Thumbnail-Img" + "/" + TbmFileNameRESize + "_Thumbnail" + extension);
                                    //        myQRConn.Open();

                                    //        try
                                    //        {
                                    //            QRcmd.ExecuteNonQuery();
                                    //        }
                                    //        catch (Exception ex)
                                    //        {
                                    //            throw (ex);
                                    //        }
                                    //        finally
                                    //        {
                                    //            myQRConn.Close();
                                    //        }

                                    //    }


                                    //}

                                }
                            }

                        }

                        using (SqlConnection mySConn = new SqlConnection(MF_Con.Text))
                        {
                            mySConn.Open();
                            SqlCommand cmd = new SqlCommand("SELECT cat_runno as abc from catecode_runno where dept_code = '" + txt_Dept.Text + "' and Supplier_Code = '" + Request.QueryString["merchant"] + "'", mySConn);

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
                                        using (SqlCommand comd = new SqlCommand("update catecode_runno set cat_runno= @caterunno where dept_code='"+ txt_Dept.Text +"' and supplier_code='" + Request.QueryString["merchant"] + "'", conn1))
                                        {
                                            comd.Parameters.AddWithValue("@caterunno", count);
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


                        Session["PName"] = "Category Listing";
                        ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Insert success.');window.location ='CategorySetupList.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "';", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Insert failed. Please try again later.');", true);
                        txt_CategoryCode.Focus();
                    }
                }
            }
        }
    }

    protected void Back_OnClick(object sender, EventArgs e)
    {
        Session["NC"] = "";
        Session["PName"] = "Category Listing";
        Response.Redirect("CategorySetupList.aspx?merchant=" + Request.QueryString["merchant"].ToString());
    }

    protected void Update_OnClick(object sender, EventArgs e)
    {
        if (txt_CategoryCode.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Category code cannot be empty.');", true);
            txt_CategoryCode.Focus();
        }
        else if (txt_CategoryDesc.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Category description cannot be empty.');", true);
            txt_CategoryDesc.Focus();
        }
        else if (txt_Dept.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Department code cannot be empty.');", true);
            txt_Dept.Focus();
        }
        else
        {
            //Int32 rv = 1;

            //using (SqlConnection myTxConn = new SqlConnection(MF_Con.Text))
            //{
            //    using (SqlCommand myTxcmd = new SqlCommand("Check_Cat", myTxConn))
            //    {
            //        myTxcmd.CommandType = CommandType.StoredProcedure;
            //        myTxcmd.Parameters.AddWithValue("@CatCode", txt_CategoryCode.Text.Trim().Replace("'", "`"));
            //        myTxcmd.Parameters.AddWithValue("@CatID", Request.QueryString["catid"].ToString());

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
            //    ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Category code exist in database before.');", true);
            //    txt_CategoryCode.Focus();
            //}
            //else
            //{
            Int32 tv = 50;

            using (SqlConnection myTxConn = new SqlConnection(MF_Con.Text))
            {
                using (SqlCommand myTxcmd = new SqlCommand("Update_Cat2", myTxConn))
                {
                    myTxcmd.CommandType = CommandType.StoredProcedure;
                    myTxcmd.Parameters.AddWithValue("@CatCode", txt_CategoryCode.Text.Trim().Replace("'", "`"));
                    myTxcmd.Parameters.AddWithValue("@CatDesc", txt_CategoryDesc.Text.Trim().Replace("'", "`"));
                    myTxcmd.Parameters.AddWithValue("@DeptCode", txt_Dept.Text.Trim().Replace("'", "`"));
                    myTxcmd.Parameters.AddWithValue("@supplier_code", Request.QueryString["merchant"].ToString());
                    myTxcmd.Parameters.AddWithValue("@CatID", Convert.ToInt32(Request.QueryString["catid"].ToString()));

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
                    if ((fileupload2.PostedFile != null) && (fileupload2.PostedFile.ContentLength > 0))
                    {

                        if (lblrunno.Text == "")
                        {
                            using (SqlConnection myTxConn2 = new SqlConnection(DBCon))
                            {
                                using (SqlCommand myTxcmd2 = new SqlCommand("Insert_Img_runno", myTxConn2))
                                {
                                    myTxcmd2.CommandType = CommandType.StoredProcedure;
                                    myTxcmd2.Parameters.AddWithValue("@runno", "11");

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

                                    myTxcmd3.Parameters.AddWithValue("@runno", "11");


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

                            SqlConnection con100 = new SqlConnection(DBCon);
                            SqlCommand cmd100 = new SqlCommand("Load_Img_runno", con100);
                            con100.Open();
                            SqlDataAdapter da100 = new SqlDataAdapter(cmd100);
                            DataTable dt100 = new DataTable();
                            da100.Fill(dt100);

                            lblrunno.Text = dt100.Rows[0]["runno"].ToString();

                            con100.Close();

                            string runno = lblrunno.Text;
                            string extension = Path.GetExtension(fileupload2.PostedFile.FileName);
                            string FileNameU = "IMG-CATE" + "-" + DateTime.UtcNow.AddHours(8).ToString("yyMMddHHmmss") + runno.ToString() + extension;
                            string TbmFileNameU = "Tbm-IMG-CATE" + "-" + DateTime.UtcNow.AddHours(8).ToString("yyMMddHHmmss") + runno.ToString() + extension;
                            string TbmFileNameRESize = "Tbm-IMG-CATE" + "-" + DateTime.UtcNow.AddHours(8).ToString("yyMMddHHmmss") + runno.ToString();


                            string path = Server.MapPath("CategoryImg/" + "Normal-Img" + "/");
                            string delpath = Server.MapPath("CategoryImg/" + "Thumbnail-Img" + "/" + TbmFileNameU);

                            if (Directory.Exists(path))
                            {
                                fileupload2.PostedFile.SaveAs(Server.MapPath("CategoryImg/" + "Normal-Img" + "/" + FileNameU));
                                fileupload2.PostedFile.SaveAs(Server.MapPath("CategoryImg/" + "Thumbnail-Img" + "/" + TbmFileNameU));
                                string pth = Server.MapPath("CategoryImg/" + "Thumbnail-Img" + "/" + TbmFileNameU);
                                resizeImageAndSave(pth);


                                using (SqlConnection myQRConn = new SqlConnection(DBCon))
                                {
                                    using (SqlCommand QRcmd = new SqlCommand("update MF_Category_miso set Thumbnail_FilePath=@thumbnail,ImgFilePath=@photo where Category_Code='" + txt_CategoryCode.Text.Trim().Replace("'", "`") + "' and DeleteInd <> 'X'", myQRConn))
                                    {
                                        QRcmd.Parameters.AddWithValue("@photo", @"https://ezyshare.online/EzyShareListing/CategoryImg/" + "Normal-Img" + "/" + FileNameU);
                                        QRcmd.Parameters.AddWithValue("@thumbnail", @"https://ezyshare.online/EzyShareListing/CategoryImg/" + "Thumbnail-Img" + "/" + TbmFileNameRESize + "_Thumbnail" + extension);
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

                                //using (SqlConnection myQRConn = new SqlConnection(DBCon))
                                //{
                                //    using (SqlCommand QRcmd = new SqlCommand("Insert into MF_Multi_Img_miso (Item_Code, Main_Indicator, Default_Indicator,Modified_DT,DeleteInd,FilePath,Thumbnail_FilePath)" +
                                //        "Values(@ItemCode,@Main_Indicator,@Default_Indicator,DATEADD(HOUR, 8, CONVERT(varchar(20),GETUTCDATE(),120)),'',@FilePath,@Thumbnail_FilePath)", myQRConn))
                                //    {
                                //        QRcmd.Parameters.AddWithValue("@ItemCode", txt_CategoryCode.Text.Trim().Replace("'", "`"));



                                //        QRcmd.Parameters.AddWithValue("@Main_Indicator", "True");
                                //        if (fileupload2.PostedFile != null)
                                //        {
                                //            QRcmd.Parameters.AddWithValue("@Default_Indicator", "1");
                                //        }
                                //        else
                                //        {
                                //            QRcmd.Parameters.AddWithValue("@Default_Indicator", "0");
                                //        }
                                //        QRcmd.Parameters.AddWithValue("@FilePath", @"https://ezyshare.online/EzyShareListing/CategoryImg/" + "Normal-Img" + "/" + FileNameU);
                                //        QRcmd.Parameters.AddWithValue("@Thumbnail_FilePath", @"https://ezyshare.online/EzyShareListing/CategoryImg/" + "Thumbnail-Img" + "/" + TbmFileNameRESize + "_Thumbnail" + extension);
                                //        myQRConn.Open();

                                //        try
                                //        {
                                //            QRcmd.ExecuteNonQuery();
                                //        }
                                //        catch (Exception ex)
                                //        {
                                //            throw (ex);
                                //        }
                                //        finally
                                //        {
                                //            myQRConn.Close();
                                //        }

                                //    }


                                //}

                            }
                            else
                            {
                                System.IO.Directory.CreateDirectory(Server.MapPath("CategoryImg/" + "Normal-Img" + "/"));
                                System.IO.Directory.CreateDirectory(Server.MapPath("CategoryImg/" + "Thumbnail-Img" + "/"));

                                fileupload2.PostedFile.SaveAs(Server.MapPath("CategoryImg/" + "Normal-Img" + "/" + FileNameU));
                                fileupload2.PostedFile.SaveAs(Server.MapPath("CategoryImg/" + "Thumbnail-Img" + "/" + TbmFileNameU));
                                string pth = Server.MapPath("CategoryImg/" + "Thumbnail-Img" + "/" + TbmFileNameU);
                                resizeImageAndSave(pth);


                                using (SqlConnection myQRConn = new SqlConnection(DBCon))
                                {
                                    using (SqlCommand QRcmd = new SqlCommand("update MF_Category_miso set Thumbnail_FilePath=@thumbnail,ImgFilePath=@photo where Item_Code='" + txt_CategoryCode.Text.Trim().Replace("'", "`") + "' and DeleteInd <> 'X'", myQRConn))
                                    {
                                        QRcmd.Parameters.AddWithValue("@photo", @"https://ezyshare.online/EzyShareListing/CategoryImg/" + "Normal-Img" + "/" + FileNameU);
                                        QRcmd.Parameters.AddWithValue("@thumbnail", @"https://ezyshare.online/EzyShareListing/CategoryImg/" + "Thumbnail-Img" + "/" + TbmFileNameRESize + "_Thumbnail" + extension);

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

                                //using (SqlConnection myQRConn = new SqlConnection(DBCon))
                                //{
                                //    using (SqlCommand QRcmd = new SqlCommand("Insert into MF_Multi_Img_miso (Item_Code, Main_Indicator, Default_Indicator,Modified_DT,DeleteInd,FilePath,Thumbnail_FilePath)" +
                                //        "Values(@ItemCode,@Main_Indicator,@Default_Indicator,DATEADD(HOUR, 8, CONVERT(varchar(20),GETUTCDATE(),120)),'',@FilePath, @Thumbnail_FilePath)", myQRConn))
                                //    {
                                //        QRcmd.Parameters.AddWithValue("@ItemCode", txt_CategoryCode.Text.Trim().Replace("'", "`"));



                                //        QRcmd.Parameters.AddWithValue("@Main_Indicator", "True");
                                //        if (fileupload2.PostedFile != null)
                                //        {
                                //            QRcmd.Parameters.AddWithValue("@Default_Indicator", "1");
                                //        }
                                //        else
                                //        {
                                //            QRcmd.Parameters.AddWithValue("@Default_Indicator", "0");
                                //        }

                                //        QRcmd.Parameters.AddWithValue("@FilePath", @"https://ezyshare.online/EzyShareListing/CategoryImg/" + "Normal-Img" + "/" + FileNameU);
                                //        QRcmd.Parameters.AddWithValue("@Thumbnail_FilePath", @"https://ezyshare.online/EzyShareListing/CategoryImg/" + "Thumbnail-Img" + "/" + TbmFileNameRESize + "_Thumbnail" + extension);
                                //        myQRConn.Open();

                                //        try
                                //        {
                                //            QRcmd.ExecuteNonQuery();
                                //        }
                                //        catch (Exception ex)
                                //        {
                                //            throw (ex);
                                //        }
                                //        finally
                                //        {
                                //            myQRConn.Close();
                                //        }

                                //    }


                                //}

                            }
                        }

                    }


                    Session["PName"] = "Category Listing";
                    ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Update success.');window.location ='CategorySetupList.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "';", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Update failed. Please try again later.');", true);
                    txt_CategoryCode.Focus();
                }
            }
            // }
        }
    }

    protected void txt_Filter_TextChanged(object sender, EventArgs e)
    {
        LoadDepartment();
    }

    private string resizeImageAndSave(string imagePath)
    {

        System.Drawing.Image fullSizeImg = System.Drawing.Image.FromFile(imagePath);
        //var thumbnailImg = new Bitmap(150, 130);
        var thumbnailImg = new Bitmap(180, 180);
        var thumbGraph = Graphics.FromImage(thumbnailImg);
        thumbGraph.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
        thumbGraph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        thumbGraph.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        //var imageRectangle = new Rectangle(0, 0, 150, 130);
        var imageRectangle = new Rectangle(0, 0, 180, 180);
        thumbGraph.DrawImage(fullSizeImg, imageRectangle);
        //string targetPath = imagePath.Replace(Path.GetFileNameWithoutExtension(imagePath), Path.GetFileNameWithoutExtension(imagePath) + "-resize");
        string targetPath = imagePath.Replace(Path.GetFileNameWithoutExtension(imagePath), Path.GetFileNameWithoutExtension(imagePath) + "_Thumbnail");
        thumbnailImg.Save(targetPath, System.Drawing.Imaging.ImageFormat.Jpeg); //(A generic error occurred in GDI+) Error occur here !
        thumbnailImg.Dispose();

        return targetPath;
    }

    protected void ddlDept_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Request.QueryString["catid"].ToString() != "")
        {
            txt_Dept.Text = ddlDept.SelectedValue;
        }
        else
        {
            txt_Dept.Text = ddlDept.SelectedValue;
            using (SqlConnection mySConn = new SqlConnection(MF_Con.Text))
            {
                mySConn.Open();
                SqlCommand cmd = new SqlCommand("SELECT cat_runno as abc from catecode_runno where dept_code = '" + txt_Dept.Text + "' and Supplier_Code = '" + Request.QueryString["merchant"].ToString() + "' ", mySConn);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        //if (reader["abc"].ToString() == "" )
                        //{
                        //    txt_CategoryCode.Text = String.Empty;
                        //    txt_CategoryCode.Text =  txt_Dept.Text + "-C1001";
                        //}
                        //else
                        //{
                        //string idcount = (reader["abc"]).ToString();
                        txt_CategoryCode.Text = String.Empty;
                        count = Convert.ToInt32((reader["abc"]).ToString());
                        count += 1;
                        txt_CategoryCode.Text += txt_Dept.Text + "-C" + count.ToString();
                        // }
                    }
                }
                else
                {
                    txt_CategoryCode.Text = String.Empty;
                    txt_CategoryCode.Text = txt_Dept.Text + "-C1001";
                    using (SqlConnection conn = new SqlConnection(MF_Con.Text))
                    {
                        conn.Open();
                        using (SqlCommand comd = new SqlCommand("insert into catecode_runno(supplier_Code,cat_runno,Dept_Code) values('" + Request.QueryString["merchant"] + "', @caterunno,@Dept_code)", conn))
                        {
                            comd.Parameters.AddWithValue("@caterunno", "1001");
                            comd.Parameters.AddWithValue("@Dept_code", txt_Dept.Text);
                            comd.ExecuteNonQuery();
                        }
                        conn.Close();
                    }


                }
                mySConn.Close();
            }
        }
    }

    protected void LoadDepartment()
    {
        using (SqlConnection mySConn = new SqlConnection(MF_Con.Text))
        {
            mySConn.Open();

            try
            {
                using (SqlDataAdapter adp = new SqlDataAdapter("SELECT Department_Code, Department_Code + ' -- ' + Department_Description AS rst FROM MF_Department_miso WHERE supplier_code = '"+ Request.QueryString["merchant"].ToString() + "' and DeleteInd <> 'X' and (Department_Code like '%" + txt_Filter.Text.Replace("'", "`") + "%' or Department_Description like '%" + txt_Filter.Text.Replace("'", "`") + "%') ORDER BY Department_Code ASC", mySConn))
                {
                    ddlDept.Items.Clear();
                    ddlDept.Items.Add(new ListItem("- Select Department -", ""));
                    DataTable dtDepartment = new DataTable();
                    adp.Fill(dtDepartment);
                    ddlDept.DataSource = dtDepartment;
                    ddlDept.DataTextField = "rst";
                    ddlDept.DataValueField = "Department_Code";
                    ddlDept.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                mySConn.Close();
            }
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