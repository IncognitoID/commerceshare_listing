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
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Image = System.Drawing.Image;
using System.Net;
using System.Net.Cache;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

public partial class AddItem : System.Web.UI.Page
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
            string merchant = Request.QueryString["merchant"];

            //checking merchant salon
            if (!string.IsNullOrEmpty(Request.QueryString["merchant"]) && Request.QueryString["merchant"].ToUpper() == "S-0622")
            {
                licommsetting.Visible = true;
            }
            else
            {
                licommsetting.Visible = false;
            }

            TabName.Value = Request.Form[TabName.UniqueID];
            TextBox1.Text = Request.QueryString["searchresult"];
            TextBox2.Text = Request.QueryString["paging"];
            TextBox3.Text = Request.QueryString["item_code"];
            TextBox4.Text = Request.QueryString["item_id"];
            lbllimit.Visible = false;
            txt_Desc.Attributes["onkeydown"] = "doNext('" + txt_ShortDesc.ClientID + "',event)";
            txt_ShortDesc.Attributes["onkeydown"] = "doNext('" + txt_OtherDesc.ClientID + "',event)";
            txt_OtherDesc.Attributes["onkeydown"] = "doNext('" + ddlUM.ClientID + "',event)";
            ddlPT.Attributes["onkeydown"] = "doNext('" + ddlST.ClientID + "',event)";
            ddlST.Attributes["onkeydown"] = "doNext('" + ddl_RdmItem.ClientID + "',event)";

            txt_Filter.Attributes.Add("onfocus", "this.select();");
            txt_Filter2.Attributes.Add("onfocus", "this.select();");
            txt_Desc.Attributes.Add("onfocus", "this.select();");
            txt_ShortDesc.Attributes.Add("onfocus", "this.select();");
            txt_OtherDesc.Attributes.Add("onfocus", "this.select();");
            txt_RdmPoint.Attributes.Add("onfocus", "this.select();");
            txtrewardpoint.Attributes.Add("onfocus", "this.select();");

            CheckBox4.Checked = true;


            btn_Insert.Visible = true;
            btn_Update.Visible = false;

            checkMerchantMDR();
            LoadBrd();
            LoadUOM();
            LoadPTax();
            LoadSTax();
            LoadDept();
            LoadCat();
            LoadLinkItem();
            Loadcommgrp();

            txt_Pack.Text = "1.000";
            txt_Pack.Enabled = false;
            ddlItem.Enabled = false;

            using (SqlConnection myTxConn = new SqlConnection(DBCon))
            {
                using (SqlCommand myTxcmd = new SqlCommand("Load_Img_runno", myTxConn))
                {
                    myTxcmd.CommandType = CommandType.StoredProcedure;

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

            using (SqlConnection myLLConn = new SqlConnection(DBCon))
            {
                using (SqlCommand myLLcmd = new SqlCommand("SELECT * FROM MF_PurchaseTax WHERE DeleteInd<>'X' ORDER BY CDefault DESC ", myLLConn))
                {
                    myLLConn.Open();

                    try
                    {
                        SqlDataReader idr = myLLcmd.ExecuteReader();

                        if (idr.HasRows == true)
                        {
                            DataTable g = new DataTable();
                            g.Load(idr);

                            if (g.Rows[0]["CDefault"].ToString() == "True")
                            {
                                ddlPT.SelectedIndex = 1;

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw (ex);
                    }
                    finally
                    {
                        myLLcmd.Dispose();
                        myLLConn.Close();
                    }
                }

            }

            using (SqlConnection myLLConn = new SqlConnection(DBCon))
            {
                using (SqlCommand myLLcmd = new SqlCommand("SELECT * FROM MF_SalesTax WHERE DeleteInd<>'X' ORDER BY CDefault DESC ", myLLConn))
                {
                    myLLConn.Open();

                    try
                    {
                        SqlDataReader idr = myLLcmd.ExecuteReader();

                        if (idr.HasRows == true)
                        {
                            DataTable g = new DataTable();
                            g.Load(idr);

                            if (g.Rows[0]["CDefault"].ToString() == "True")
                            {
                                ddlST.SelectedIndex = 1;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw (ex);
                    }
                    finally
                    {
                        myLLcmd.Dispose();
                        myLLConn.Close();
                    }
                }

            }

            /*using (SqlConnection myLLConn = new SqlConnection(DBCon))
            {
                using (SqlCommand myLLcmd = new SqlCommand("Select top 1 * from MF_item inner join MF_Category_miso on MF_Category_miso.Category_Code = MF_Item.Category_Code and MF_Category_miso.DeleteInd<>'X' inner join MF_Department_miso on MF_Department_miso.Department_Code = MF_Category_miso.Department_Code and MF_Department_miso.DeleteInd <> 'X' where MF_Item.DeleteInd<>'X' and MF_Item.modified_DT <> '' order by MF_Item.Modified_DT desc", myLLConn))
                {
                    myLLConn.Open();

                    try
                    {
                        SqlDataReader idr = myLLcmd.ExecuteReader();

                        if (idr.HasRows == true)
                        {
                            DataTable g = new DataTable();
                            g.Load(idr);

                            Label2.Text = g.Rows[0]["Category_Code"].ToString();
                            Label3.Text = g.Rows[0]["Category_Description"].ToString();

                        }
                    }
                    catch (Exception ex)
                    {
                        throw (ex);
                    }
                    finally
                    {
                        myLLcmd.Dispose();
                        myLLConn.Close();
                    }
                }
            }*/

            using (SqlConnection myLLConn = new SqlConnection(DBCon))
            {
                using (SqlCommand myLLcmd = new SqlCommand("select * from mf_item inner join MF_Category_miso on MF_Category_miso.Category_Code = MF_Item.Category_Code and MF_Category_miso.DeleteInd<>'X' inner join MF_Department_miso on MF_Department_miso.Department_Code = MF_Category_miso.Department_Code and MF_Department_miso.DeleteInd<>'X' where MF_Item.item_code = '" + TextBox3.Text + "' and MF_Item.deleteind <>'x'", myLLConn))
                {
                    myLLConn.Open();

                    try
                    {
                        SqlDataReader idr = myLLcmd.ExecuteReader();

                        if (idr.HasRows == true)
                        {
                            DataTable g = new DataTable();
                            g.Load(idr);
                            Label8.Text = g.Rows[0]["Department_Code"].ToString();

                        }
                    }
                    catch (Exception ex)
                    {
                        throw (ex);
                    }
                    finally
                    {
                        myLLcmd.Dispose();
                        myLLConn.Close();
                    }
                }
            }

            using (SqlConnection myLLConn = new SqlConnection(DBCon))
            {
                using (SqlCommand myLLcmd = new SqlCommand("SELECT * FROM MF_UOM WHERE DeleteInd<>'X' ORDER BY CDefault DESC ", myLLConn))
                {
                    myLLConn.Open();

                    try
                    {
                        SqlDataReader idr = myLLcmd.ExecuteReader();

                        if (idr.HasRows == true)
                        {
                            DataTable g = new DataTable();
                            g.Load(idr);

                            if (g.Rows[0]["CDefault"].ToString() == "True")
                            {
                                ddlUM.SelectedIndex = 1;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw (ex);
                    }
                    finally
                    {
                        myLLcmd.Dispose();
                        myLLConn.Close();
                    }
                }

            }


            CheckBox4.Visible = false;
            linkUpload2.Visible = false;
            ddlPager2.Visible = false;
            Label1.Visible = false;
            Label7.Visible = false;
            br1.Visible = false;
            br2.Visible = false;
            br6.Visible = false;
            //ddlPager.Visible = false;
           //linkUpload.Visible = false;
            //lbl_Record2.Visible = false;
            //lbl_Record.Visible = false;


            if (!String.IsNullOrEmpty(Request.QueryString["Item_Code"]))
            {
                btn_Insert.Visible = false;
                btn_Update.Visible = true;
                CheckBox4.Visible = true;
                linkUpload2.Visible = true;
                br1.Visible = true;
                br2.Visible = true;
                br6.Visible = true;
                ddlPager2.Visible = true;
                Label1.Visible = true;
                Label7.Visible = true;
                txt_ItemCode.Enabled = false;
                LoadMerchantDetails();
                //ddlPager.Visible = true;
                //lbl_Record2.Visible = true;
                //lbl_Record.Visible = true;
                //linkUpload.Visible = true;
                LoadImgPath(1);
                //LoadImg(1);
                LoadItem();
                loadmodifier();
                loadprinter();

            }
            else
            {
                loadmodifier();
                loadprinter();
                LoadMerchantDetails();
                txt_ItemCode.Focus();
            }

            if (ddl_RdmItem.SelectedValue == "1")
            {
                txt_RdmPoint.Enabled = true;
                txtrewardpoint.Enabled = true;
                txt_RdmPoint.Focus();
            }
            else
            {
                txt_RdmPoint.Enabled = true;
                txtrewardpoint.Enabled = true;
            }
            ddlDept.SelectedValue = Label8.Text.ToString();
            //if (ddlDept.SelectedValue == Label8.Text.ToString())
            //{
            //    LoadCat();
            //}

            BindItemSuppliers();

        }
        //}
    }

    private void LoadMerchantDetails()
    {
        using (SqlConnection mySConn = new SqlConnection(DBCon))
        {
            using (SqlCommand myScmd = new SqlCommand("select * from AP_Merchant where merchantid = '"+ Request.QueryString["merchant"] +"'", mySConn))
            {
                SqlDataAdapter adp = new SqlDataAdapter(myScmd);
                DataTable dt = new DataTable();
                adp.Fill(dt);

                if(dt.Rows.Count > 0)
                {
                    if(dt.Rows[0]["referral_fee_apply"].ToString() == "allitem")
                    {
                        spprofit.InnerText = " (Pre Set %)";
                        txtRefProfitBonusPercent.Visible = false;
                        lblRefProfitBonusPercent.Text = Convert.ToDecimal(dt.Rows[0]["referral_fee_apply_val"]).ToString("0.00");
                    }
                    else if(dt.Rows[0]["referral_fee_apply"].ToString() == "individualitem")
                    {
                        spprofit.InnerText = " (Optional - $)";
                        lblRefProfitBonusPercent.Visible = false;
                        txtRefProfitBonusPercent.Text = Convert.ToDecimal(dt.Rows[0]["referral_fee_apply_val"]).ToString("0.00");
                    }
                    else
                    {
                        spprofit.InnerText = " (Optional - $)";
                        lblRefProfitBonusPercent.Visible = false;
                        txtRefProfitBonusPercent.Text = "0.00";
                    }

                    if (dt.Rows[0]["MerchantType"].ToString() != "Markup")
                    {
                        txtESProfit.Text = Convert.ToDecimal(dt.Rows[0]["MDR"]).ToString("0.00");
                    }
                    else
                    {
                        txtESProfit.Text = Convert.ToDecimal(dt.Rows[0]["MarkupPercentage"]).ToString("0.00");
                    }
                }

            }
        }
    }

    private void LoadItem()
    {
        string itemids = Request.QueryString["Item_Code"];
        this.ddlCat.SelectedIndexChanged -= new EventHandler(ddlCat_SelectedIndexChanged);
        using (SqlConnection mySConn = new SqlConnection(DBCon))
        {
            using (SqlCommand myScmd = new SqlCommand("Load_Item_Info", mySConn))
            {
                myScmd.CommandType = CommandType.StoredProcedure;
                myScmd.Parameters.AddWithValue("@ItemCode", itemids.ToString());

                mySConn.Open();

                try
                {
                    SqlDataReader idr = myScmd.ExecuteReader();

                    if (idr.HasRows == true)
                    {
                        DataTable g = new DataTable();
                        g.Load(idr);

                        ddlType.SelectedValue = g.Rows[0]["Type"].ToString();


                        if (ddlType.SelectedValue == "1")
                        {
                            LinkPanel.Visible = false;
                        }
                        else if (ddlType.SelectedValue == "2")
                        {
                            LinkPanel.Visible = true;
                        }

                        txt_ItemCode.Text = g.Rows[0]["Item_Code"].ToString();
                        if (ddlType.SelectedValue == "1")
                        {
                            txt_Pack.Enabled = false;
                            ddlItem.Enabled = false;

                            txt_Pack.Text = "1.000";
                            txt_Subcode.Text = txt_ItemCode.Text;
                        }
                        else
                        {
                            txt_Pack.Enabled = true;
                            ddlItem.Enabled = true;

                            txt_Pack.Text = Convert.ToDecimal(g.Rows[0]["PackSize"].ToString()).ToString("f3");
                            txt_Subcode.Text = g.Rows[0]["LinkCode"].ToString();
                            ddlItem.SelectedValue = txt_Subcode.Text;
                        }
                        txt_Barcode.Text = g.Rows[0]["Barcode"].ToString();
                        ddlCat.SelectedValue = g.Rows[0]["Category_Code"].ToString();
                        ddlDept.SelectedValue = g.Rows[0]["Department_Code"].ToString();
                        txt_Category.Text = g.Rows[0]["Category_Code"].ToString();
                        txt_Desc.Text = g.Rows[0]["LongDesc"].ToString();
                        txt_Desc2.Text = g.Rows[0]["LongDesc2"].ToString();
                        txt_ShortDesc.Text = g.Rows[0]["ShortDesc"].ToString();
                        txt_OtherDesc.Text = g.Rows[0]["OtherDesc"].ToString();
                        ddlPT.SelectedValue = g.Rows[0]["PurchaseTax"].ToString();
                        ddlST.SelectedValue = g.Rows[0]["SalesTax"].ToString();
                        ddlUM.SelectedValue = g.Rows[0]["UOM"].ToString().ToUpper();
                        ddlBrd.SelectedValue = g.Rows[0]["Brand"].ToString();
                        txtweight.Text = g.Rows[0]["weight"].ToString();
                        ddl_RdmItem.SelectedValue = g.Rows[0]["IsRedemption"].ToString();
                        txtEditor.Text = g.Rows[0]["overview"].ToString();
                        txt_RdmPoint.Text = Convert.ToDecimal(g.Rows[0]["RedemptionPoint"].ToString()).ToString("0");
                        txtSPP.Text = Convert.ToDecimal(g.Rows[0]["SPP"].ToString()).ToString("#,##0.00");
                        txtRefProfitBonusPercent.Text = Convert.ToDecimal(g.Rows[0]["ReferrerProfitBonusPercent"].ToString()).ToString("#,##0.00");
                        txtTeamKPI.Text = Convert.ToDecimal(g.Rows[0]["TeamKPIBonus"].ToString()).ToString("#,##0.00");
						txtTeamKPI2.Text = Convert.ToDecimal(g.Rows[0]["TeamKPIBonusMS"].ToString()).ToString("#,##0.00");
                        txtMBD.Text = Convert.ToDecimal(g.Rows[0]["MBD"].ToString()).ToString("#,##0.00");
                        txtMEB.Text = Convert.ToDecimal(g.Rows[0]["MEB"].ToString()).ToString("#,##0.00");
                        txtteamgrowthinc1.Text = Convert.ToDecimal(g.Rows[0]["TeamGrowthIncentive1"].ToString()).ToString("#,##0.00");
                        txtteamgrowthinc2.Text = Convert.ToDecimal(g.Rows[0]["TeamGrowthIncentive2"].ToString()).ToString("#,##0.00");
                        txtESProfit.Text = Convert.ToDecimal(g.Rows[0]["ESProfit"].ToString()).ToString("#,##0.00");
                        txtVlink.Text = g.Rows[0]["VideoLink"].ToString();
                        txtVlink2.Text = g.Rows[0]["VideoLink2"].ToString();
                        txtVlink3.Text = g.Rows[0]["VideoLink3"].ToString();
                        txtvendorcost.Text = g.Rows[0]["vendorcost"].ToString();
                        txtmaxbuy.Text = g.Rows[0]["MaxItemBuyLimitPerTrans"].ToString();
                        ddlweightitem.SelectedValue = g.Rows[0]["weight_item"].ToString();
                        ddlfoc.SelectedValue = g.Rows[0]["FOC"].ToString();
                        ddlordertable.SelectedValue = g.Rows[0]["Self_Order_Menu"].ToString();

                        //comm setting

                        if(g.Rows[0]["CommType"].ToString() == "")
                        {
                            ddlcommtype.SelectedValue = "None";
                        }
                        else
                        {
                            ddlcommtype.SelectedValue = g.Rows[0]["CommType"].ToString();
                        }

                        ddlcommby.SelectedValue = g.Rows[0]["CommBy"].ToString();
                        txtcommamt.Text = g.Rows[0]["CommAmt"].ToString();
                        ddlcommgroup.SelectedValue = g.Rows[0]["CommGroup"].ToString();

                        if (g.Rows[0]["CommBy"].ToString() == "Group Tier")
                        {
                            commamt.Visible = false;
                            commgroup.Visible = true;
                        }
                        else
                        {
                            commamt.Visible = true;
                            commgroup.Visible = false;
                        }

                        if (ddlweightitem.SelectedValue == "YES")
                        {
                            divweighttype.Visible = true;
                            txt_Barcode.Attributes.Add("onkeypress", "return IsNumberWithNoDecimal(this,event);");
                            txt_Barcode.Attributes.Add("MaxLength", "5");
                            RDweighttype.SelectedValue = g.Rows[0]["weight_type"].ToString();

                            if (RDweighttype.SelectedValue == "KG")
                            {
                                ddlUM.Items.Clear();
                                ddlUM.Items.Add(new ListItem("KG", "KG"));
                                ddlUM.Enabled= false;
                                ddlUM.SelectedValue="KG";
                            }
                            else if (RDweighttype.SelectedValue == "PCS")
                            {
                                ddlUM.SelectedValue="PCS";
                            }
                        }
                        else
                        {
                            LoadUOM();
                            divweighttype.Visible = false;
                            txt_Barcode.Attributes.Remove("onkeypress");
                            txt_Barcode.Attributes.Remove("MaxLength");
                        }
                        ShowData();
                        txt_RdmPoint.Visible = false;
                        txtrewardpoint.Visible = false;
                        lblredempt.Visible = false;
                        lblreward.Visible = false;

                        Int32 ty = 1;

                        using (SqlConnection myCLConn = new SqlConnection(DBCon))
                        {
                            using (SqlCommand myCLcmd = new SqlCommand("Check_LinkCode", myCLConn))
                            {
                                myCLcmd.CommandType = CommandType.StoredProcedure;
                                myCLcmd.Parameters.AddWithValue("@ItemCode", txt_ItemCode.Text.Trim().Replace("'", "`"));

                                SqlParameter SReturnValue = myCLcmd.Parameters.Add("returnValue", SqlDbType.Int, 4);
                                SReturnValue.Direction = ParameterDirection.ReturnValue;

                                myCLConn.Open();

                                try
                                {
                                    myCLcmd.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    throw (ex);
                                }
                                finally
                                {
                                    myCLcmd.Dispose();
                                    myCLConn.Close();
                                }

                                ty = Convert.ToInt32(SReturnValue.Value.ToString());
                            }
                        }

                        if (ty == 1)
                        {
                            ddlType.Enabled = false;
                        }

                        if (String.IsNullOrEmpty(g.Rows[0]["FilePath"].ToString()) == true)
                        {
                            Img1.Src = "Images/NoPic.png";
                            Img1.Attributes.Add("Width", "185px");
                        }
                        else
                        {
                           
                           Img1.Src = g.Rows[0]["FilePath"].ToString();
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

        txt_Barcode.Focus();
    }

    
    protected void add_List_Click2(object sender, EventArgs e)
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
                        myTxcmd2.Parameters.AddWithValue("@runno", "1");

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

                        myTxcmd3.Parameters.AddWithValue("@runno", "1");


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

            string FileNameU = "IMG" + "-" + DateTime.UtcNow.AddHours(8).ToString("yyMMddHHmmss") + runno.ToString() + extension;
            string TbmFileNameU = "Tbm-IMG" + "-" + DateTime.UtcNow.AddHours(8).ToString("yyMMddHHmmss") + runno.ToString() + extension;
            string TbmFileNameRESize = "Tbm-IMG" + "-" + DateTime.UtcNow.AddHours(8).ToString("yyMMddHHmmss") + runno.ToString();
            string path = Server.MapPath("ProductImg/" + "IMG - pos_MF_32323232" + "/");
            string delpath = "./ProductImg/" + "Thumbnail-IMG-pos_MF_32323232" + "/" + TbmFileNameU;
            if (Directory.Exists(path))
            {
                fileupload2.PostedFile.SaveAs(Server.MapPath("ProductImg/" + "IMG - pos_MF_32323232" + "/" + FileNameU));
                fileupload2.PostedFile.SaveAs(Server.MapPath("ProductImg/" + "Thumbnail-IMG-pos_MF_32323232" + "/" + TbmFileNameU));
                string pth = Server.MapPath("ProductImg/" + "Thumbnail-IMG-pos_MF_32323232" + "/" + TbmFileNameU);
                resizeImageAndSave(pth);
               

                using (SqlConnection myTxConn = new SqlConnection(DBCon))
                {
                    using (SqlCommand myTxcmd = new SqlCommand("Insert into MF_Multi_Img_miso (Item_Code, Main_Indicator, Default_Indicator,Modified_DT,DeleteInd, FilePath, Thumbnail_FilePath)" +
                    "Values(@ItemCode,@Main_Indicator,'0',DATEADD(HOUR, 8, CONVERT(varchar(20),GETUTCDATE(),120)),'',@FilePath, @Thumbnail_FilePath)", myTxConn))
                    {
                        myTxcmd.Parameters.AddWithValue("@ItemCode", txt_ItemCode.Text.Trim().Replace("'", "`"));
                        myTxcmd.Parameters.AddWithValue("@FilePath", @"https://ezyshare.online/EzyShareListing/ProductImg/" + "IMG - pos_MF_32323232" + "/" + FileNameU);
                        myTxcmd.Parameters.AddWithValue("@Thumbnail_FilePath", @"https://ezyshare.online/EzyShareListing/ProductImg/" + "Thumbnail-IMG-pos_MF_32323232" + "/" + TbmFileNameRESize + "_Thumbnail" + extension);
                        
                        if (CheckBox4.Checked == true)
                        {
                            myTxcmd.Parameters.AddWithValue("@Main_Indicator", "True");
                        }
                        else if (CheckBox4.Checked == false)
                        {
                            myTxcmd.Parameters.AddWithValue("@Main_Indicator", "False");
                        }

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


                    }
                }
                ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Insert Image success.');", true);

                LoadImgPath(1);
               
            }
            else
            {
                
                System.IO.Directory.CreateDirectory(Server.MapPath("ProductImg/" + "IMG - pos_MF_32323232" + "/"));
                System.IO.Directory.CreateDirectory(Server.MapPath("ProductImg/" + "Thumbnail-IMG-pos_MF_32323232" + "/"));

                fileupload2.PostedFile.SaveAs(Server.MapPath("ProductImg/" + "IMG - pos_MF_32323232" + "/" + FileNameU));
                fileupload2.PostedFile.SaveAs(Server.MapPath("ProductImg/" + "Thumbnail-IMG-pos_MF_32323232" + "/" + TbmFileNameU));
                string pth = Server.MapPath("ProductImg/" + "Thumbnail-IMG-pos_MF_32323232" + "/" + TbmFileNameU);
                resizeImageAndSave(pth);
               
                using (SqlConnection myTxConn = new SqlConnection(DBCon))
                {
                    using (SqlCommand myTxcmd = new SqlCommand("Insert into MF_Multi_Img_miso (Item_Code, Main_Indicator, Default_Indicator,Modified_DT,DeleteInd, FilePath, Thumbnail_FilePath)" +
                    "Values(@ItemCode,@Main_Indicator,'0',DATEADD(HOUR, 8, CONVERT(varchar(20),GETUTCDATE(),120)),'',@FilePath, @Thumbnail_FilePath)", myTxConn))
                    {
                        myTxcmd.Parameters.AddWithValue("@ItemCode", txt_ItemCode.Text.Trim().Replace("'", "`"));
                        myTxcmd.Parameters.AddWithValue("@FilePath", @"https://ezyshare.online/EzyShareListing/ProductImg/" + "IMG-pos-32323232" + "/" + FileNameU);
                        myTxcmd.Parameters.AddWithValue("@Thumbnail_FilePath", @"https://ezyshare.online/EzyShareListing/ProductImg/" + "Thumbnail-IMG-pos_MF_32323232" + "/" + TbmFileNameRESize + "_Thumbnail" + extension);
                        if (CheckBox4.Checked == true)
                        {
                            myTxcmd.Parameters.AddWithValue("@Main_Indicator", "True");
                        }
                        else if (CheckBox4.Checked == false)
                        {
                            myTxcmd.Parameters.AddWithValue("@Main_Indicator", "False");
                        }

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


                    }
                }
                ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Insert Image success.');", true);

                LoadImgPath(1);

            }
            //if (File.Exists(delpath))
            //{
            //    File.Delete(delpath);
            //}
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Image Empty, Please Insert Image!');", true);
        }
    }

    //protected void add_List_Click(object sender, EventArgs e)
    //{
    //    if ((FileUpload1.PostedFile != null) && (FileUpload1.PostedFile.ContentLength > 0))
    //    {
    //        if (lblrunno.Text == "")
    //        {
    //            using (SqlConnection myTxConn2 = new SqlConnection(DBCon))
    //            {
    //                using (SqlCommand myTxcmd2 = new SqlCommand("Insert_Img_runno", myTxConn2))
    //                {
    //                    myTxcmd2.CommandType = CommandType.StoredProcedure;
    //                    myTxcmd2.Parameters.AddWithValue("@runno", "1");

    //                    myTxConn2.Open();

    //                    try
    //                    {
    //                        myTxcmd2.ExecuteNonQuery();
    //                    }
    //                    catch (Exception ex)
    //                    {
    //                        throw (ex);
    //                    }
    //                    finally
    //                    {
    //                        myTxcmd2.Dispose();
    //                        myTxConn2.Close();
    //                    }
    //                }
    //            }
    //        }
    //        else
    //        {
    //            using (SqlConnection myTxConn3 = new SqlConnection(DBCon))
    //            {
    //                using (SqlCommand myTxcmd3 = new SqlCommand("Update_Img_runno", myTxConn3))
    //                {
    //                    myTxcmd3.CommandType = CommandType.StoredProcedure;

    //                    myTxcmd3.Parameters.AddWithValue("@runno", "1");


    //                    myTxConn3.Open();

    //                    try
    //                    {
    //                        myTxcmd3.ExecuteNonQuery();
    //                    }
    //                    catch (Exception ex)
    //                    {
    //                        throw (ex);
    //                    }
    //                    finally
    //                    {
    //                        myTxcmd3.Dispose();
    //                        myTxConn3.Close();
    //                    }
    //                }
    //            }
    //        }
    //        SqlConnection con100 = new SqlConnection(DBCon);
    //        SqlCommand cmd100 = new SqlCommand("Load_Img_runno", con100);
    //        con100.Open();
    //        SqlDataAdapter da100 = new SqlDataAdapter(cmd100);
    //        DataTable dt100 = new DataTable();
    //        da100.Fill(dt100);

    //        lblrunno.Text = dt100.Rows[0]["runno"].ToString();

    //        con100.Close();

    //        string runno = lblrunno.Text;
    //        string extension = Path.GetExtension(FileUpload1.PostedFile.FileName);
    //        string FileNameU = "IMG" + "-" + DateTime.UtcNow.AddHours(8).ToString("yyMMddHHmmss") + runno.ToString() + extension;

    //        string path = Server.MapPath("~/EzyShareListing/WebsiteImg/" + "IMG - pos_MF_32323232" + "/");

    //        if (Directory.Exists(path))
    //        {
    //            FileUpload1.PostedFile.SaveAs(Server.MapPath("~/EzyShareListing/WebsiteImg/" + "IMG - pos_MF_32323232" + "/" + FileNameU));

    //            using (SqlConnection myTxConn = new SqlConnection(DBCon))
    //            {
    //                using (SqlCommand myTxcmd = new SqlCommand("Insert into MF_Multi_Img_miso (Item_Code, Main_Indicator, Default_Indicator,Modified_DT,DeleteInd, FilePath)" +
    //                "Values(@ItemCode,'True','0',DATEADD(HOUR, 8, CONVERT(varchar(20),GETUTCDATE(),120)),'',@FilePath)", myTxConn))
    //                {
    //                    myTxcmd.Parameters.AddWithValue("@ItemCode", txt_ItemCode.Text.Trim().Replace("'", "`"));
    //                    myTxcmd.Parameters.AddWithValue("@FilePath", @"https://ezyshare.online/EzyShareListing/WebsiteImg/" + "IMG - pos_MF_32323232" + "/" + FileNameU);

    //                    myTxConn.Open();

    //                    try
    //                    {
    //                        myTxcmd.ExecuteNonQuery();
    //                    }
    //                    catch (Exception ex)
    //                    {
    //                        throw (ex);
    //                    }
    //                    finally
    //                    {
    //                        myTxcmd.Dispose();
    //                        myTxConn.Close();
    //                    }


    //                }
    //            }
    //            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Insert Image success.');", true);


    //            LoadImg(1);

    //        }
    //        else
    //        {

    //            System.IO.Directory.CreateDirectory(Server.MapPath("~/EzyShareListing/WebsiteImg/" + "IMG-pos_MF_pos_MF_32323232" + "/"));

    //            FileUpload1.PostedFile.SaveAs(Server.MapPath("~/EzyShareListing/WebsiteImg/" + "IMG-pos_MF_pos_MF_32323232" + "/" + FileNameU));

    //            using (SqlConnection myTxConn = new SqlConnection(DBCon))
    //            {
    //                using (SqlCommand myTxcmd = new SqlCommand("Insert into MF_Multi_Img_miso (Item_Code, Main_Indicator, Default_Indicator,Modified_DT,DeleteInd, FilePath)" +
    //                "Values(@ItemCode,'True','0',DATEADD(HOUR, 8, CONVERT(varchar(20),GETUTCDATE(),120)),'',@FilePath)", myTxConn))
    //                {
    //                    myTxcmd.Parameters.AddWithValue("@ItemCode", txt_ItemCode.Text.Trim().Replace("'", "`"));
    //                    myTxcmd.Parameters.AddWithValue("@FilePath", @"https://ezyshare.online/EzyShareListing/WebsiteImg/" + "IMG-pos_MF_pos_MF_32323232" + "/" + FileNameU);


    //                    myTxConn.Open();

    //                    try
    //                    {
    //                        myTxcmd.ExecuteNonQuery();
    //                    }
    //                    catch (Exception ex)
    //                    {
    //                        throw (ex);
    //                    }
    //                    finally
    //                    {
    //                        myTxcmd.Dispose();
    //                        myTxConn.Close();
    //                    }


    //                }
    //            }
    //            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Insert Image success.');", true);
    //            LoadImg(1);
    //        }

    //    }
    //    else
    //    {
    //        ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Image Empty, Please Insert Image!');", true);
    //    }


    //}

    private void checkMerchantMDR()
    {
        using (SqlConnection myTxConn = new SqlConnection(DBCon))
        {
            using (SqlCommand myTxcmd = new SqlCommand("select merchant_id from vms_AllowMerchantMDR where merchant_id = '"+ Request.QueryString["merchant"].ToString() +"'", myTxConn))
            {
                SqlDataAdapter adp = new SqlDataAdapter(myTxcmd);
                DataTable dt = new DataTable();
                adp.Fill(dt);

                if(dt.Rows.Count > 0)
                {
                    txtESProfit.Enabled = true;
                    txtRefProfitBonusPercent.Enabled = true;
                }
                else
                {
                    txtRefProfitBonusPercent.Enabled = true;
                    txtESProfit.Enabled = false;
                }

            }
        }
    }


    protected void Back_OnClick(object sender, EventArgs e)
    {
        Session["PName"] = "Item Listing";
        if (Request.QueryString["user"] != null)
        {
            Response.Redirect("ContentApproval.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "&user=" + Request.QueryString["user"].ToString());
            
        }
        else
        {
            Response.Redirect("ContentApproval.aspx?merchant=" + Request.QueryString["merchant"].ToString());
        }
    }

    protected void Insert_OnClick(object sender, EventArgs e)
    {
        
        if (txtTeamKPI.Text == "")
		{
			txtTeamKPI.Text = "0";
		}
		if (txtTeamKPI2.Text == "")
		{
			txtTeamKPI2.Text = "0";
		}
		if (txtMEB.Text == "")
		{
			txtMEB.Text = "0";
		}
		if (txtESProfit.Text == "")
		{
			txtESProfit.Text = "0";
		}
		if (txtMBD.Text == "")
		{
			txtMBD.Text = "0";
		}
        if (txtteamgrowthinc1.Text == "")
        {
            txtteamgrowthinc1.Text = "0";
        }
        if (txtteamgrowthinc2.Text == "")
        {
            txtteamgrowthinc2.Text = "0";
        }
        if (txtweight.Text != "")
        {
            if (Convert.ToDecimal(txtweight.Text) > 1000)
            {
                lbllimit.Visible = true;
            }
        }

        using (SqlConnection mySConn1 = new SqlConnection(DBCon))
        {
            using (SqlCommand myScmd = new SqlCommand("select * from AP_Merchant where merchantid = '" + Request.QueryString["merchant"] + "'", mySConn1))
            {
                SqlDataAdapter adp = new SqlDataAdapter(myScmd);
                DataTable dt = new DataTable();
                adp.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["MerchantType"].ToString() != "Markup")
                    {
                        if (ddlDept.SelectedValue == "")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Please select department.');", true);
                            txt_Category.Focus();
                        }
                        else if (txt_Category.Text.Trim() == "")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Category code cannot be empty.');", true);
                            txt_Category.Focus();
                        }
                        else if (txt_ItemCode.Text.Trim() == "")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Item code cannot be empty.');", true);
                            txt_ItemCode.Focus();
                        }
                        else if (txt_Barcode.Text.Trim() == "")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Barcode cannot be empty.');", true);
                            txt_Barcode.Focus();
                        }
                        else if (txt_Subcode.Text.Trim() == "")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Linkcode cannot be empty.');", true);
                            txt_Subcode.Focus();
                        }
                        else if (txt_Desc.Text.Trim() == "")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Long desc cannot be empty.');", true);
                            txt_Desc.Focus();
                        }
                        else if (txt_ShortDesc.Text.Trim() == "")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Short desc cannot be empty.');", true);
                            txt_ShortDesc.Focus();
                        }
                        else if (txt_Pack.Text.Trim() == "")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Pack size cannot be empty.');", true);
                            txt_Pack.Focus();
                        }
                        else if (CheckAmount(txt_Pack.Text) == false)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Please enter a valid amount for pack size.');", true);
                            txt_Pack.Focus();
                        }
                        else if (ddlUM.SelectedValue == "")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Please select UOM.');", true);
                            ddlUM.Focus();
                        }
                        else if (ddlPT.SelectedValue == "")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Please select purchase tax.');", true);
                            ddlPT.Focus();
                        }
                        else if (ddlST.SelectedValue == "")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Please select sales tax.');", true);
                            ddlST.Focus();
                        }
                        else if (ddl_RdmItem.SelectedValue == "1" && txt_RdmPoint.Text.Trim() == "")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Redemption point cannot be empty.');", true);
                            txt_RdmPoint.Focus();
                        }
                        else if (CheckAmount(txt_RdmPoint.Text) == false)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Please enter a valid amount for redemption point.');", true);
                            txt_RdmPoint.Focus();
                        }
                        else if (ddlBrd.SelectedValue == "")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Please Select Brand.');", true);
                        }

                        else
                        {
                            Int32 rv = 1;
                            Int32 hv = 1;

                            using (SqlConnection myTxConn = new SqlConnection(DBCon))
                            {
                                myTxConn.Open();
                                SqlCommand cmd = new SqlCommand("SELECT itemcode_runno as abc from itemcode_runno where cate_code = '" + txt_Category.Text + "' and Supplier_Code = '" + Request.QueryString["merchant"] + "'", myTxConn);

                                SqlDataReader reader = cmd.ExecuteReader();
                                if (reader.HasRows)
                                {
                                    if (reader.Read())
                                    {

                                        count = Convert.ToInt32((reader["abc"]).ToString());
                                        count += 1;
                                        // txt_Category.Text = String.Empty;
                                        txt_ItemCode.Text = String.Empty;
                                        //txt_Barcode.Text = String.Empty;
                                        txt_Subcode.Text = String.Empty;
                                        txt_ItemCode.Text += txt_Category.Text + "-" + count.ToString();
                                        //txt_Barcode.Text = txt_ItemCode.Text;
                                        if (ddlType.SelectedValue == "1")
                                        {
                                            txt_Subcode.Text = txt_ItemCode.Text;
                                        }

                                        reader.Close();
                                        reader.Dispose();

                                        using (SqlCommand comd = new SqlCommand("update itemcode_runno set itemcode_runno= @itemrunno where cate_code='" + txt_Category.Text + "' and supplier_code='" + Request.QueryString["merchant"] + "'", myTxConn))
                                        {
                                            comd.Parameters.AddWithValue("@itemrunno", count);
                                            comd.ExecuteNonQuery();
                                        }
                                        //}
                                    }
                                }
                                else
                                {
                                    txt_ItemCode.Text = txt_Category.Text + "-1000001";
                                    //txt_Barcode.Text = txt_ItemCode.Text;
                                    if (ddlType.SelectedValue == "1")
                                    {
                                        txt_Subcode.Text = txt_ItemCode.Text;
                                    }

                                    using (SqlConnection conn = new SqlConnection(DBCon))
                                    {
                                        conn.Open();
                                        using (SqlCommand comd = new SqlCommand("insert into itemcode_runno(supplier_Code,itemcode_runno,cate_Code) values('" + Request.QueryString["merchant"] + "', @itemcode_runno,@cat_code)", conn))
                                        {
                                            comd.Parameters.AddWithValue("@itemcode_runno", "1000001");
                                            comd.Parameters.AddWithValue("@cat_code", txt_Category.Text);
                                            comd.ExecuteNonQuery();
                                        }
                                        conn.Close();
                                    }
                                }

                                myTxConn.Close();
                                
                            }

                            using (SqlConnection myFxConn = new SqlConnection(DBCon))
                            {
                                using (SqlCommand myFxcmd = new SqlCommand("Check_Item", myFxConn))
                                {
                                    myFxcmd.CommandType = CommandType.StoredProcedure;
                                    myFxcmd.Parameters.AddWithValue("@ItemCode", txt_Barcode.Text.Trim().Replace("'", "`"));
                                    myFxcmd.Parameters.AddWithValue("@ItemID", "");
				    myFxcmd.Parameters.AddWithValue("@suppliercode", Request.QueryString["merchant"].ToString());

                                    SqlParameter SReturnValue = myFxcmd.Parameters.Add("returnValue", SqlDbType.Int, 4);
                                    SReturnValue.Direction = ParameterDirection.ReturnValue;

                                    myFxConn.Open();

                                    try
                                    {
                                        myFxcmd.ExecuteNonQuery();
                                    }
                                    catch (Exception ex)
                                    {
                                        throw (ex);
                                    }
                                    finally
                                    {
                                        myFxcmd.Dispose();
                                        myFxConn.Close();
                                    }

                                    hv = Convert.ToInt32(SReturnValue.Value.ToString());
                                }
                            }

                             if (hv == 1)
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Barcode exist in database before.');", true);
                                txt_Barcode.Focus();
                            }

                            else
                            {
                                Int32 tv = 50;

                                using (SqlConnection myTxConn = new SqlConnection(DBCon))
                                {
                                    using (SqlCommand myTxcmd = new SqlCommand("Insert_Item2", myTxConn))
                                    {
                                        myTxcmd.CommandType = CommandType.StoredProcedure;
                                        myTxcmd.Parameters.AddWithValue("@ItemCode", txt_ItemCode.Text.Trim().Replace("'", "`"));
                                        myTxcmd.Parameters.AddWithValue("@Barcode", txt_Barcode.Text.Trim().Replace("'", "`"));
                                        myTxcmd.Parameters.AddWithValue("@Linkcode", txt_Subcode.Text.Trim().Replace("'", "`"));
                                        myTxcmd.Parameters.AddWithValue("@Type", ddlType.SelectedValue);
                                        myTxcmd.Parameters.AddWithValue("@LongDesc", txt_Desc.Text.Trim().Replace("'", "`").Replace("\"","``"));
                                        myTxcmd.Parameters.AddWithValue("@LongDesc2", txt_Desc2.Text.Trim().Replace("'", "`").Replace("\"", "``"));
                                        myTxcmd.Parameters.AddWithValue("@ShortDesc", txt_ShortDesc.Text.Trim().Replace("'", "`"));
                                        myTxcmd.Parameters.AddWithValue("@OtherDesc", txt_OtherDesc.Text.Trim().Replace("'", "`"));
                                        myTxcmd.Parameters.AddWithValue("@CategoryCode", txt_Category.Text.Trim().Replace("'", "`"));
                                        myTxcmd.Parameters.AddWithValue("@Brand", ddlBrd.SelectedValue);
                                        myTxcmd.Parameters.AddWithValue("@PackSize", txt_Pack.Text.Trim().Replace("'", "`"));
                                        myTxcmd.Parameters.AddWithValue("@UOM", ddlUM.SelectedValue);
                                        myTxcmd.Parameters.AddWithValue("@FOC", ddlfoc.SelectedValue);
                                        myTxcmd.Parameters.AddWithValue("@Self_Order_Menu", ddlordertable.SelectedValue);
                                        if (txtweight.Text != "")
                                        {
                                            myTxcmd.Parameters.AddWithValue("@weight", Convert.ToDecimal(txtweight.Text.Trim().Replace("'", "`")));
                                        }
                                        else
                                        {
                                            myTxcmd.Parameters.AddWithValue("@weight", 0);
                                        }
                                        myTxcmd.Parameters.AddWithValue("@PurchaseTax", ddlPT.SelectedValue);
                                        myTxcmd.Parameters.AddWithValue("@SalesTax", ddlST.SelectedValue);
                                        myTxcmd.Parameters.AddWithValue("@IsRedemption", ddl_RdmItem.SelectedValue);
                                        if (txt_RdmPoint.Text != "")
                                        {
                                            myTxcmd.Parameters.AddWithValue("@RedemptionPoint", Convert.ToDecimal(txt_RdmPoint.Text.Trim().Replace("'", "`")));
                                        }
                                        else
                                        {
                                            myTxcmd.Parameters.AddWithValue("@RedemptionPoint", 0);
                                        }
                                        if (lblRefProfitBonusPercent.Visible == false)
                                        {
                                            myTxcmd.Parameters.AddWithValue("@RefProfitBonusPercent", Convert.ToDecimal(txtRefProfitBonusPercent.Text.Trim().Replace("'", "`")));
                                        }
                                        else
                                        {
                                            myTxcmd.Parameters.AddWithValue("@RefProfitBonusPercent", 0);
                                        }
                                        if (txtTeamKPI.Text != "")
                                        {
                                            myTxcmd.Parameters.AddWithValue("@TeamKPI", Convert.ToDecimal(txtTeamKPI.Text.Trim().Replace("'", "`")));
                                        }
                                        else
                                        {
                                            myTxcmd.Parameters.AddWithValue("@TeamKPI", 0);
                                        }
                                        if (txtTeamKPI2.Text != "")
                                        {
                                            myTxcmd.Parameters.AddWithValue("@TeamKPI2", Convert.ToDecimal(txtTeamKPI2.Text.Trim().Replace("'", "`")));
                                        }
                                        else
                                        {
                                            myTxcmd.Parameters.AddWithValue("@TeamKPI2", 0);
                                        }
                                        if (txtMBD.Text != "")
                                        {
                                            myTxcmd.Parameters.AddWithValue("@MBD", Convert.ToDecimal(txtMBD.Text.Trim().Replace("'", "`")));
                                        }
                                        else
                                        {
                                            myTxcmd.Parameters.AddWithValue("@MBD", 0);
                                        }
                                        if (txtMEB.Text != "")
                                        {
                                            myTxcmd.Parameters.AddWithValue("@MEB", Convert.ToDecimal(txtMEB.Text.Trim().Replace("'", "`")));
                                        }
                                        else
                                        {
                                            myTxcmd.Parameters.AddWithValue("@MEB", 0);
                                        }
                                        if (txtteamgrowthinc1.Text != "")
                                        {
                                            myTxcmd.Parameters.AddWithValue("@teamgrowthinc1", Convert.ToDecimal(txtteamgrowthinc1.Text.Trim().Replace("'", "`")));
                                        }
                                        else
                                        {
                                            myTxcmd.Parameters.AddWithValue("@teamgrowthinc1", 0);
                                        }
                                        if (txtteamgrowthinc2.Text != "")
                                        {
                                            myTxcmd.Parameters.AddWithValue("@teamgrowthinc2", Convert.ToDecimal(txtteamgrowthinc2.Text.Trim().Replace("'", "`")));
                                        }
                                        else
                                        {
                                            myTxcmd.Parameters.AddWithValue("@teamgrowthinc2", 0);
                                        }
                                        if (txtESProfit.Text != "")
                                        {
                                            myTxcmd.Parameters.AddWithValue("@ESProfit", Convert.ToDecimal(txtESProfit.Text.Trim().Replace("'", "`")));
                                        }
                                        else
                                        {
                                            myTxcmd.Parameters.AddWithValue("@ESProfit", 0);
                                        }
                                        if (txtSPP.Text != "")
                                        {
                                            myTxcmd.Parameters.AddWithValue("@SPP", Convert.ToDecimal(txtSPP.Text.Trim().Replace("'", "`")));
                                        }
                                        else
                                        {
                                            myTxcmd.Parameters.AddWithValue("@SPP", 0);
                                        }

                                        //comm setting
                                        myTxcmd.Parameters.AddWithValue("@commtype", ddlcommtype.SelectedValue);
                                        myTxcmd.Parameters.AddWithValue("@commby", ddlcommby.SelectedValue);
                                        if (txtcommamt.Text == "")
                                        {
                                            myTxcmd.Parameters.AddWithValue("@commamt", "0");
                                        }
                                        else
                                        {
                                            myTxcmd.Parameters.AddWithValue("@commamt", txtcommamt.Text);
                                        }
                                        myTxcmd.Parameters.AddWithValue("@commgrp", ddlcommgroup.SelectedValue);


                                        myTxcmd.Parameters.AddWithValue("@supplier", Request.QueryString["merchant"].ToString());
                                        myTxcmd.Parameters.AddWithValue("@overview", txtEditor.Text);

                                        if (txtvendorcost.Text != "")
                                        {
                                            myTxcmd.Parameters.AddWithValue("@cost", txtvendorcost.Text);
                                        }
                                        else
                                        {
                                            myTxcmd.Parameters.AddWithValue("@cost", "0");
                                        }

                                        if (txtmaxbuy.Text != "")
                                        {
                                            myTxcmd.Parameters.AddWithValue("@maxbuylimit", txtmaxbuy.Text);
                                        }
                                        else
                                        {
                                            myTxcmd.Parameters.AddWithValue("@maxbuylimit", "0");
                                        }
                                        //myTxcmd.Parameters.AddWithValue("@maxbuylimit", txtmaxbuy.Text);
                                        //myTxcmd.Parameters.AddWithValue("@videolink1", txtVlink.Text);
                                        if (!string.IsNullOrEmpty(txtVlink.Text))
                                        {
                                            string videourl1 = "";
                                            string youtubeurl1 = "";
                                            if (txtVlink.Text.EndsWith("?rel=0"))
                                            {
                                                youtubeurl1 = txtVlink.Text.Trim();
                                                myTxcmd.Parameters.AddWithValue("@videolink1", youtubeurl1);
                                            }
                                            else if (txtVlink.Text.StartsWith("https://youtu.be/"))
                                            {
                                                videourl1 = txtVlink.Text.Trim().Substring(17);
                                                videourl1 = videourl1.Substring(0, 11);
                                                youtubeurl1 = "https://www.youtube.com/embed/" + videourl1 + "?rel=0";

                                                myTxcmd.Parameters.AddWithValue("@videolink1", youtubeurl1);
                                            }
                                            else if (txtVlink.Text.StartsWith("https://www.youtube.com/shorts/"))
                                            {
                                                videourl1 = txtVlink.Text.Trim().Substring(31);
                                                videourl1 = videourl1.Substring(0, 11);
                                                youtubeurl1 = "https://www.youtube.com/embed/" + videourl1 + "?rel=0";

                                                myTxcmd.Parameters.AddWithValue("@videolink1", youtubeurl1);
                                            }
                                            else
                                            {
                                                videourl1 = txtVlink.Text.Trim().Substring(32);
                                                videourl1 = videourl1.Substring(0, 11);
                                                youtubeurl1 = "https://www.youtube.com/embed/" + videourl1 + "?rel=0";

                                                myTxcmd.Parameters.AddWithValue("@videolink1", youtubeurl1);
                                            }
                                        }
                                        else
                                        {
                                            myTxcmd.Parameters.AddWithValue("@videolink1", txtVlink.Text);
                                        }

                                        if (!string.IsNullOrEmpty(txtVlink2.Text))
                                        {
                                            string videourl2 = "";
                                            string youtubeurl2 = "";
                                            if (txtVlink2.Text.EndsWith("?rel=0"))
                                            {
                                                youtubeurl2 = txtVlink2.Text.Trim();
                                                myTxcmd.Parameters.AddWithValue("@videolink2", youtubeurl2);
                                            }
                                            else if (txtVlink2.Text.StartsWith("https://youtu.be/"))
                                            {
                                                videourl2 = txtVlink2.Text.Trim().Substring(17);
                                                videourl2 = videourl2.Substring(0, 11);
                                                youtubeurl2 = "https://www.youtube.com/embed/" + videourl2 + "?rel=0";

                                                myTxcmd.Parameters.AddWithValue("@videolink2", youtubeurl2);
                                            }
                                            else if (txtVlink2.Text.StartsWith("https://www.youtube.com/shorts/"))
                                            {
                                                videourl2 = txtVlink2.Text.Trim().Substring(31);
                                                videourl2 = videourl2.Substring(0, 11);
                                                youtubeurl2 = "https://www.youtube.com/embed/" + videourl2 + "?rel=0";

                                                myTxcmd.Parameters.AddWithValue("@videolink2", youtubeurl2);
                                            }
                                            else
                                            {
                                                videourl2 = txtVlink2.Text.Trim().Substring(32);
                                                videourl2 = videourl2.Substring(0, 11);
                                                youtubeurl2 = "https://www.youtube.com/embed/" + videourl2 + "?rel=0";

                                                myTxcmd.Parameters.AddWithValue("@videolink2", youtubeurl2);
                                            }
                                        }
                                        else
                                        {
                                            myTxcmd.Parameters.AddWithValue("@videolink2", txtVlink2.Text);
                                        }

                                        if (!string.IsNullOrEmpty(txtVlink3.Text))
                                        {
                                            string videourl3 = "";
                                            string youtubeurl3 = "";
                                            if (txtVlink3.Text.EndsWith("?rel=0"))
                                            {
                                                youtubeurl3 = txtVlink3.Text.Trim();
                                                myTxcmd.Parameters.AddWithValue("@videolink3", youtubeurl3);
                                            }
                                            else if (txtVlink3.Text.StartsWith("https://youtu.be/"))
                                            {
                                                videourl3 = txtVlink3.Text.Trim().Substring(17);
                                                videourl3 = videourl3.Substring(0, 11);
                                                youtubeurl3 = "https://www.youtube.com/embed/" + videourl3 + "?rel=0";

                                                myTxcmd.Parameters.AddWithValue("@videolink3", youtubeurl3);
                                            }
                                            else if (txtVlink3.Text.StartsWith("https://www.youtube.com/shorts/"))
                                            {
                                                videourl3 = txtVlink3.Text.Trim().Substring(31);
                                                videourl3 = videourl3.Substring(0, 11);
                                                youtubeurl3 = "https://www.youtube.com/embed/" + videourl3 + "?rel=0";

                                                myTxcmd.Parameters.AddWithValue("@videolink3", youtubeurl3);
                                            }
                                            else
                                            {
                                                videourl3 = txtVlink3.Text.Trim().Substring(32);
                                                videourl3 = videourl3.Substring(0, 11);
                                                youtubeurl3 = "https://www.youtube.com/embed/" + videourl3 + "?rel=0";

                                                myTxcmd.Parameters.AddWithValue("@videolink3", youtubeurl3);
                                            }
                                        }
                                        else
                                        {
                                            myTxcmd.Parameters.AddWithValue("@videolink3", txtVlink3.Text);
                                        }
                                        myTxcmd.Parameters.AddWithValue("@weightitem", ddlweightitem.SelectedValue);
                                        myTxcmd.Parameters.AddWithValue("@weighttype", RDweighttype.SelectedValue);

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
                                        SqlConnection con = new SqlConnection(DBCon);
                                        con.Open();

                                        string user = "";
                                        if (Request.QueryString["user"] != null)
                                        {
                                            user = Request.QueryString["user"].ToString();
                                        }
                                        else
                                        {
                                            user = Request.QueryString["merchant"].ToString();

                                        };

                                        foreach (GridViewRow row in grd_viewmodifer.Rows)
                                        {
                                            if (row.RowType == DataControlRowType.DataRow)
                                            {
                                                CheckBox chkmodifier = (CheckBox)row.FindControl("chkmodifier");
                                                if (chkmodifier.Checked)
                                                {
                                                    HiddenField hdmodifierID = (HiddenField)row.FindControl("hdmodifierID");

                                                    SqlCommand insertmodifiercmd = new SqlCommand("insert into MF_Item_Modifier_Assign(modifier_ID,Item_Code,assign_date,deleteind) values(@modifierID,@itemcode,dateadd(hour,(8),CONVERT([varchar](20),getutcdate(),(120))),'')", con);
                                                    insertmodifiercmd.Parameters.AddWithValue("@modifierID", hdmodifierID.Value);
                                                    insertmodifiercmd.Parameters.AddWithValue("@itemcode", txt_ItemCode.Text.Trim().Replace("'", "`"));
                                                    try
                                                    {
                                                        insertmodifiercmd.ExecuteNonQuery();
                                                    }
                                                    catch (Exception ex)
                                                    {

                                                    }
                                                }
                                            }

                                        }

                                        foreach (GridViewRow row in grd_viewprinter.Rows)
                                        {
                                            if (row.RowType == DataControlRowType.DataRow)
                                            {
                                                CheckBox chkprinter = (CheckBox)row.FindControl("chkprinter");
                                                if (chkprinter.Checked)
                                                {
                                                    HiddenField hdprinterID = (HiddenField)row.FindControl("hdprinterID");

                                                    SqlCommand insertmodifiercmd = new SqlCommand("insert into PosSys_PrinterItem(Item_Code,Printer_ID,Merchant_Code,User_Code,Created_DT) values(@itemcode,@printerid,@merchantcode,@usercode,dateadd(hour,(8),CONVERT([varchar](20),getutcdate(),(120))))", con);
                                                    insertmodifiercmd.Parameters.AddWithValue("@printerid", hdprinterID.Value);
                                                    insertmodifiercmd.Parameters.AddWithValue("@itemcode", txt_ItemCode.Text.Trim().Replace("'", "`"));
                                                    insertmodifiercmd.Parameters.AddWithValue("@merchantcode", Request.QueryString["merchant"].ToString());
                                                    insertmodifiercmd.Parameters.AddWithValue("@usercode", user);
                                                    try
                                                    {
                                                        insertmodifiercmd.ExecuteNonQuery();
                                                    }
                                                    catch (Exception ex)
                                                    {

                                                    }
                                                }
                                            }

                                        }

                                        con.Close();

                                        if ((fileupload2.PostedFile != null) && (fileupload2.PostedFile.ContentLength > 0))
                                        {

                                            if (lblrunno.Text == "")
                                            {
                                                using (SqlConnection myTxConn2 = new SqlConnection(DBCon))
                                                {
                                                    using (SqlCommand myTxcmd2 = new SqlCommand("Insert_Img_runno", myTxConn2))
                                                    {
                                                        myTxcmd2.CommandType = CommandType.StoredProcedure;
                                                        myTxcmd2.Parameters.AddWithValue("@runno", "1");

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

                                                        myTxcmd3.Parameters.AddWithValue("@runno", "1");


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
                                                string FileNameU = "IMG" + "-" + DateTime.UtcNow.AddHours(8).ToString("yyMMddHHmmss") + runno.ToString() + extension;
                                                string TbmFileNameU = "Tbm-IMG" + "-" + DateTime.UtcNow.AddHours(8).ToString("yyMMddHHmmss") + runno.ToString() + extension;
                                                string TbmFileNameRESize = "Tbm-IMG" + "-" + DateTime.UtcNow.AddHours(8).ToString("yyMMddHHmmss") + runno.ToString();


                                                string path = Server.MapPath("ProductImg/" + "IMG - pos_MF_32323232" + "/");
                                                string delpath = Server.MapPath("ProductImg/" + "Thumbnail-IMG-pos_MF_32323232" + "/" + TbmFileNameU);

                                                if (Directory.Exists(path))
                                                {
                                                    fileupload2.PostedFile.SaveAs(Server.MapPath("ProductImg/" + "IMG - pos_MF_32323232" + "/" + FileNameU));
                                                    fileupload2.PostedFile.SaveAs(Server.MapPath("ProductImg/" + "Thumbnail-IMG-pos_MF_32323232" + "/" + TbmFileNameU));
                                                    string pth = Server.MapPath("ProductImg/" + "Thumbnail-IMG-pos_MF_32323232" + "/" + TbmFileNameU);
                                                    resizeImageAndSave(pth);

                                                   
                                                    //Stream strm = fileupload2.PostedFile.InputStream;
                                                    //using (var image = System.Drawing.Image.FromStream(strm))
                                                    //{
                                                    //    int newWidth = 180;
                                                    //    int newHeight = 180;
                                                    //    var thumbImg = new Bitmap(newWidth, newHeight);
                                                    //    var thumbGraph = Graphics.FromImage(thumbImg);
                                                    //    thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                                                    //    thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                                                    //    thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                                    //    var imgRectangle = new Rectangle(0, 0, newWidth, newHeight);
                                                    //    thumbGraph.DrawImage(image, imgRectangle);
                                                    //    string targetPath = Server.MapPath(@"~\ProductImg\IMG - pos_MF_32323232\") + FileNameU;
                                                    //    string targetTbmPath = Server.MapPath(@"~\ProductImg\Thumbnail-IMG-pos_MF_32323232\") + TbmFileNameU;
                                                    //    thumbImg.Save(targetPath, ImageFormat.Jpeg);
                                                    //    thumbImg.Save(targetTbmPath, ImageFormat.Jpeg);
                                                    //    thumbImg.Dispose();
                                                    //}

                                                    using (SqlConnection myQRConn = new SqlConnection(DBCon))
                                                    {
                                                        using (SqlCommand QRcmd = new SqlCommand("update MF_Item set Thumbnail_FilePath=@thumbnail,FilePath=@photo where Item_Code='" + txt_ItemCode.Text.Trim().Replace("'", "`") + "' and DeleteInd <> 'X'", myQRConn))
                                                        {
                                                            QRcmd.Parameters.AddWithValue("@photo", @"https://ezyshare.online/EzyShareListing/ProductImg/" + "IMG - pos_MF_32323232" + "/" + FileNameU);
                                                            QRcmd.Parameters.AddWithValue("@thumbnail", @"https://ezyshare.online/EzyShareListing/ProductImg/" + "Thumbnail-IMG-pos_MF_32323232" + "/" + TbmFileNameRESize + "_Thumbnail" + extension);
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

                                                    using (SqlConnection myQRConn = new SqlConnection(DBCon))
                                                    {
                                                        using (SqlCommand QRcmd = new SqlCommand("Insert into MF_Multi_Img_miso (Item_Code, Main_Indicator, Default_Indicator,Modified_DT,DeleteInd,FilePath,Thumbnail_FilePath)" +
                                                            "Values(@ItemCode,@Main_Indicator,@Default_Indicator,DATEADD(HOUR, 8, CONVERT(varchar(20),GETUTCDATE(),120)),'',@FilePath,@Thumbnail_FilePath)", myQRConn))
                                                        {
                                                            QRcmd.Parameters.AddWithValue("@ItemCode", txt_ItemCode.Text.Trim().Replace("'", "`"));



                                                            QRcmd.Parameters.AddWithValue("@Main_Indicator", "True");
                                                            if (fileupload2.PostedFile != null)
                                                            {
                                                                QRcmd.Parameters.AddWithValue("@Default_Indicator", "1");
                                                            }
                                                            else
                                                            {
                                                                QRcmd.Parameters.AddWithValue("@Default_Indicator", "0");
                                                            }
                                                            QRcmd.Parameters.AddWithValue("@FilePath", @"https://ezyshare.online/EzyShareListing/ProductImg/" + "IMG - pos_MF_32323232" + "/" + FileNameU);
                                                            QRcmd.Parameters.AddWithValue("@Thumbnail_FilePath", @"https://ezyshare.online/EzyShareListing/ProductImg/" + "Thumbnail-IMG-pos_MF_32323232" + "/" + TbmFileNameRESize + "_Thumbnail" + extension);
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
                                                  
                                                }
                                                else
                                                {
                                                    System.IO.Directory.CreateDirectory(Server.MapPath("ProductImg/" + "IMG - pos_MF_32323232" + "/"));
                                                    System.IO.Directory.CreateDirectory(Server.MapPath("ProductImg/" + "Thumbnail-IMG-pos_MF_32323232" + "/"));

                                                    fileupload2.PostedFile.SaveAs(Server.MapPath("ProductImg/" + "IMG - pos_MF_32323232" + "/" + FileNameU));
                                                    fileupload2.PostedFile.SaveAs(Server.MapPath("ProductImg/" + "Thumbnail-IMG-pos_MF_32323232" + "/" + TbmFileNameU));
                                                    string pth = Server.MapPath("ProductImg/" + "Thumbnail-IMG-pos_MF_32323232" + "/" + TbmFileNameU);
                                                    resizeImageAndSave(pth);
                                                   
                                                    //Stream strm = fileupload2.PostedFile.InputStream;
                                                    //using (var image = System.Drawing.Image.FromStream(strm))
                                                    //{
                                                    //    int newWidth = 180;
                                                    //    int newHeight = 180;
                                                    //    var thumbImg = new Bitmap(newWidth, newHeight);
                                                    //    var thumbGraph = Graphics.FromImage(thumbImg);
                                                    //    thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                                                    //    thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                                                    //    thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                                    //    var imgRectangle = new Rectangle(0, 0, newWidth, newHeight);
                                                    //    thumbGraph.DrawImage(image, imgRectangle);
                                                    //    string targetPath = Server.MapPath(@"~\ProductImg\IMG - pos_MF_32323232\") + FileNameU;
                                                    //    string targetTbmPath = Server.MapPath(@"~\ProductImg\Thumbnail-IMG-pos_MF_32323232\") + TbmFileNameU;
                                                    //    thumbImg.Save(targetPath, ImageFormat.Jpeg);
                                                    //    thumbImg.Save(targetTbmPath, ImageFormat.Jpeg);
                                                    //    thumbImg.Dispose();
                                                    //}
                                                    using (SqlConnection myQRConn = new SqlConnection(DBCon))
                                                    {
                                                        using (SqlCommand QRcmd = new SqlCommand("update MF_Item set Thumbnail_FilePath=@thumbnail,FilePath=@photo where Item_Code='" + txt_ItemCode.Text.Trim().Replace("'", "`") + "' and DeleteInd <> 'X'", myQRConn))
                                                        {
                                                            QRcmd.Parameters.AddWithValue("@photo", @"https://ezyshare.online/EzyShareListing/ProductImg/" + "IMG - pos_MF_32323232" + "/" + FileNameU);
                                                            QRcmd.Parameters.AddWithValue("@thumbnail", @"https://ezyshare.online/EzyShareListing/ProductImg/" + "Thumbnail-IMG-pos_MF_32323232" + "/" + TbmFileNameRESize + "_Thumbnail" + extension);

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

                                                    using (SqlConnection myQRConn = new SqlConnection(DBCon))
                                                    {
                                                        using (SqlCommand QRcmd = new SqlCommand("Insert into MF_Multi_Img_miso (Item_Code, Main_Indicator, Default_Indicator,Modified_DT,DeleteInd,FilePath,Thumbnail_FilePath)" +
                                                            "Values(@ItemCode,@Main_Indicator,@Default_Indicator,DATEADD(HOUR, 8, CONVERT(varchar(20),GETUTCDATE(),120)),'',@FilePath, @Thumbnail_FilePath)", myQRConn))
                                                        {
                                                            QRcmd.Parameters.AddWithValue("@ItemCode", txt_ItemCode.Text.Trim().Replace("'", "`"));



                                                            QRcmd.Parameters.AddWithValue("@Main_Indicator", "True");
                                                            if (fileupload2.PostedFile != null)
                                                            {
                                                                QRcmd.Parameters.AddWithValue("@Default_Indicator", "1");
                                                            }
                                                            else
                                                            {
                                                                QRcmd.Parameters.AddWithValue("@Default_Indicator", "0");
                                                            }

                                                            QRcmd.Parameters.AddWithValue("@FilePath", @"https://ezyshare.online/EzyShareListing/ProductImg/" + "IMG - pos_MF_32323232" + "/" + FileNameU);
                                                            QRcmd.Parameters.AddWithValue("@Thumbnail_FilePath", @"https://ezyshare.online/EzyShareListing/ProductImg/" + "Thumbnail-IMG-pos_MF_32323232" + "/" + TbmFileNameRESize + "_Thumbnail" + extension);
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
                                                  
                                                }
                                                //if (File.Exists(delpath))
                                                //{
                                                //    File.Delete(delpath);
                                                //}
                                            }

                                        }
                                        using (SqlConnection myQRConn = new SqlConnection(DBCon))
                                        {
                                            myQRConn.Open();
                                            using (SqlCommand QRcmd = new SqlCommand("Insert into MF_AgentCommi ([Item_Code],[AgentLevelCode],[BuyPrice],[SalesCommission],[SponsorCommission],[BreakAwayCommission],[EarnRPWhenBuy],[RedemptionPoint],[Created_DT],[Modified_DT],[DeleteInd],[CashBackAmount])" +
                                                "Values(@ItemCode,'HQ','0','0','0','0',@EarnRPwhenBuy,@RedemptionPoint,dateadd(hour,(8),CONVERT([varchar](20),getutcdate(),(120))),dateadd(hour,(8),CONVERT([varchar](20),getutcdate(),(120))),'','0')", myQRConn))
                                            {
                                                QRcmd.Parameters.AddWithValue("@ItemCode", txt_ItemCode.Text.Trim().Replace("'", "`"));
                                                QRcmd.Parameters.AddWithValue("@RedemptionPoint", txt_RdmPoint.Text.Trim().Replace("'", "`"));
                                                QRcmd.Parameters.AddWithValue("@EarnRPwhenBuy", txtrewardpoint.Text.Trim().Replace("'", "`"));
                                                QRcmd.ExecuteNonQuery();
                                            }
                                            myQRConn.Close();
                                        }

                                        using (SqlConnection myQRConn = new SqlConnection(DBCon))
                                        {
                                            myQRConn.Open();
                                            using (SqlCommand QRcmd = new SqlCommand("Insert into MF_AgentCommi ([Item_Code],[AgentLevelCode],[BuyPrice],[SalesCommission],[SponsorCommission],[BreakAwayCommission],[EarnRPWhenBuy],[RedemptionPoint],[Created_DT],[Modified_DT],[DeleteInd],[CashBackAmount])" +
                                                "Values(@ItemCode,'D01','0','0','0','0',@EarnRPwhenBuy,@RedemptionPoint,dateadd(hour,(8),CONVERT([varchar](20),getutcdate(),(120))),dateadd(hour,(8),CONVERT([varchar](20),getutcdate(),(120))),'','0')", myQRConn))
                                            {
                                                QRcmd.Parameters.AddWithValue("@ItemCode", txt_ItemCode.Text.Trim().Replace("'", "`"));
                                                QRcmd.Parameters.AddWithValue("@RedemptionPoint", txt_RdmPoint.Text.Trim().Replace("'", "`"));
                                                QRcmd.Parameters.AddWithValue("@EarnRPwhenBuy", txtrewardpoint.Text.Trim().Replace("'", "`"));
                                                QRcmd.ExecuteNonQuery();
                                            }
                                            myQRConn.Close();
                                        }
                                        using (SqlConnection myQRConn = new SqlConnection(DBCon))
                                        {
                                            myQRConn.Open();
                                            using (SqlCommand QRcmd = new SqlCommand("Insert into MF_AgentCommi ([Item_Code],[AgentLevelCode],[BuyPrice],[SalesCommission],[SponsorCommission],[BreakAwayCommission],[EarnRPWhenBuy],[RedemptionPoint],[Created_DT],[Modified_DT],[DeleteInd],[CashBackAmount])" +
                                                "Values(@ItemCode,'D02','0','0','0','0',@EarnRPwhenBuy,@RedemptionPoint,dateadd(hour,(8),CONVERT([varchar](20),getutcdate(),(120))),dateadd(hour,(8),CONVERT([varchar](20),getutcdate(),(120))),'','0')", myQRConn))
                                            {
                                                QRcmd.Parameters.AddWithValue("@ItemCode", txt_ItemCode.Text.Trim().Replace("'", "`"));
                                                QRcmd.Parameters.AddWithValue("@RedemptionPoint", txt_RdmPoint.Text.Trim().Replace("'", "`"));
                                                QRcmd.Parameters.AddWithValue("@EarnRPwhenBuy", txtrewardpoint.Text.Trim().Replace("'", "`"));
                                                QRcmd.ExecuteNonQuery();
                                            }
                                            myQRConn.Close();
                                        }
                                        using (SqlConnection myQRConn = new SqlConnection(DBCon))
                                        {
                                            myQRConn.Open();
                                            using (SqlCommand QRcmd = new SqlCommand("Insert into MF_AgentCommi ([Item_Code],[AgentLevelCode],[BuyPrice],[SalesCommission],[SponsorCommission],[BreakAwayCommission],[EarnRPWhenBuy],[RedemptionPoint],[Created_DT],[Modified_DT],[DeleteInd],[CashBackAmount])" +
                                                "Values(@ItemCode,'D03','0','0','0','0',@EarnRPwhenBuy,@RedemptionPoint,dateadd(hour,(8),CONVERT([varchar](20),getutcdate(),(120))),dateadd(hour,(8),CONVERT([varchar](20),getutcdate(),(120))),'','0')", myQRConn))
                                            {
                                                QRcmd.Parameters.AddWithValue("@ItemCode", txt_ItemCode.Text.Trim().Replace("'", "`"));
                                                QRcmd.Parameters.AddWithValue("@RedemptionPoint", txt_RdmPoint.Text.Trim().Replace("'", "`"));
                                                QRcmd.Parameters.AddWithValue("@EarnRPwhenBuy", txtrewardpoint.Text.Trim().Replace("'", "`"));
                                                QRcmd.ExecuteNonQuery();
                                            }
                                            myQRConn.Close();
                                        }
                                        using (SqlConnection myQRConn = new SqlConnection(DBCon))
                                        {
                                            myQRConn.Open();
                                            using (SqlCommand QRcmd = new SqlCommand("Insert into MF_AgentCommi ([Item_Code],[AgentLevelCode],[BuyPrice],[SalesCommission],[SponsorCommission],[BreakAwayCommission],[EarnRPWhenBuy],[RedemptionPoint],[Created_DT],[Modified_DT],[DeleteInd],[CashBackAmount])" +
                                                "Values(@ItemCode,'D04','0','0','0','0',@EarnRPwhenBuy,@RedemptionPoint,dateadd(hour,(8),CONVERT([varchar](20),getutcdate(),(120))),dateadd(hour,(8),CONVERT([varchar](20),getutcdate(),(120))),'','0')", myQRConn))
                                            {
                                                QRcmd.Parameters.AddWithValue("@ItemCode", txt_ItemCode.Text.Trim().Replace("'", "`"));
                                                QRcmd.Parameters.AddWithValue("@RedemptionPoint", txt_RdmPoint.Text.Trim().Replace("'", "`"));
                                                QRcmd.Parameters.AddWithValue("@EarnRPwhenBuy", txtrewardpoint.Text.Trim().Replace("'", "`"));
                                                QRcmd.ExecuteNonQuery();
                                            }
                                            myQRConn.Close();
                                        }
                                        using (SqlConnection myQRConn = new SqlConnection(DBCon))
                                        {
                                            myQRConn.Open();
                                            using (SqlCommand QRcmd = new SqlCommand("Insert into MF_AgentCommi ([Item_Code],[AgentLevelCode],[BuyPrice],[SalesCommission],[SponsorCommission],[BreakAwayCommission],[EarnRPWhenBuy],[RedemptionPoint],[Created_DT],[Modified_DT],[DeleteInd],[CashBackAmount])" +
                                                "Values(@ItemCode,'D05','0','0','0','0',@EarnRPwhenBuy,@RedemptionPoint,dateadd(hour,(8),CONVERT([varchar](20),getutcdate(),(120))),dateadd(hour,(8),CONVERT([varchar](20),getutcdate(),(120))),'','0')", myQRConn))
                                            {
                                                QRcmd.Parameters.AddWithValue("@ItemCode", txt_ItemCode.Text.Trim().Replace("'", "`"));
                                                QRcmd.Parameters.AddWithValue("@RedemptionPoint", txt_RdmPoint.Text.Trim().Replace("'", "`"));
                                                QRcmd.Parameters.AddWithValue("@EarnRPwhenBuy", txtrewardpoint.Text.Trim().Replace("'", "`"));
                                                QRcmd.ExecuteNonQuery();
                                            }
                                            myQRConn.Close();
                                        }



                                        for (int yy = 0; yy < rpt_Item2.Items.Count; yy++)
                                        {

                                            using (SqlConnection PLUConn = new SqlConnection(DBCon))
                                            {
                                                using (SqlCommand PLUcmd = new SqlCommand("Insert_PLU", PLUConn))
                                                {
                                                    PLUcmd.CommandType = CommandType.StoredProcedure;
                                                    PLUcmd.Parameters.AddWithValue("@StoreCode", "HQ");
                                                    PLUcmd.Parameters.AddWithValue("@ItemCode", txt_ItemCode.Text.Trim().Replace("'", "`"));
                                                    PLUcmd.Parameters.AddWithValue("@Barcode", txt_Barcode.Text.Trim().Replace("'", "`"));
                                                    PLUcmd.Parameters.AddWithValue("@Linkcode", txt_Subcode.Text.Trim().Replace("'", "`"));


                                                    PLUConn.Open();

                                                    try
                                                    {
                                                        PLUcmd.ExecuteNonQuery();
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        throw (ex);
                                                    }
                                                    finally
                                                    {
                                                        PLUcmd.Dispose();
                                                        PLUConn.Close();
                                                    }
                                                }
                                            }
                                        }

                                        Session["Item_Price"] = txt_ItemCode.Text;
                                        Session["PName"] = "Price Setup";
                                        if (Request.QueryString["user"] != null)
                                        {
                                            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Insert success.');window.location ='ContentApproval.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "&user=" + Request.QueryString["user"].ToString() + "';", true);
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Insert success.');window.location ='ContentApproval.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "';", true);
                                        }
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Insert failed. Please try again later.');", true);
                                        txt_ItemCode.Focus();
                                    }
                                }
                            }
                        }

                    }
                    else
                    {
                        if (txt_Category.Text.Trim() == "")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Category code cannot be empty.');", true);
                            txt_Category.Focus();
                        }
                        //else if (!txtVlink.Text.StartsWith("https://www.youtube.com/watch?") || !txtVlink.Text.StartsWith("https://youtu.be/"))
                        //{
                        //    ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Video Link 1 is not a YouTube link.');", true);
                        //}       
                        //else if (!txtVlink2.Text.StartsWith("https://youtu.be/") || !txtVlink2.Text.StartsWith("https://www.youtube.com/watch?"))
                        //{
                        //    ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Video Link 2 is not a YouTube link.');", true);
                        //}
                        //else if (!txtVlink3.Text.StartsWith("https://youtu.be/") || !txtVlink3.Text.StartsWith("https://www.youtube.com/watch?"))
                        //{
                        //    ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Video Link 3 is not a YouTube link.');", true);
                        //}
                        else if (txt_ItemCode.Text.Trim() == "")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Item code cannot be empty.');", true);
                            txt_ItemCode.Focus();
                        }
                        else if (txt_Barcode.Text.Trim() == "")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Barcode cannot be empty.');", true);
                            txt_Barcode.Focus();
                        }
                        else if (txt_Subcode.Text.Trim() == "")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Linkcode cannot be empty.');", true);
                            txt_Subcode.Focus();
                        }
                        else if (txt_Desc.Text.Trim() == "")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Long desc cannot be empty.');", true);
                            txt_Desc.Focus();
                        }
                        else if (txt_ShortDesc.Text.Trim() == "")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Short desc cannot be empty.');", true);
                            txt_ShortDesc.Focus();
                        }
                        else if (txt_Pack.Text.Trim() == "")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Pack size cannot be empty.');", true);
                            txt_Pack.Focus();
                        }
                        else if (CheckAmount(txt_Pack.Text) == false)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Please enter a valid amount for pack size.');", true);
                            txt_Pack.Focus();
                        }
                        else if (ddlUM.SelectedValue == "")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Please select UOM.');", true);
                            ddlUM.Focus();
                        }
                        else if (ddlPT.SelectedValue == "")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Please select purchase tax.');", true);
                            ddlPT.Focus();
                        }
                        else if (ddlST.SelectedValue == "")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Please select sales tax.');", true);
                            ddlST.Focus();
                        }
                        else if (ddl_RdmItem.SelectedValue == "1" && txt_RdmPoint.Text.Trim() == "")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Redemption point cannot be empty.');", true);
                            txt_RdmPoint.Focus();
                        }
                        else if (CheckAmount(txt_RdmPoint.Text) == false)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Please enter a valid amount for redemption point.');", true);
                            txt_RdmPoint.Focus();
                        }
                        else if (ddlBrd.SelectedValue == "")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Please Select Brand.');", true);
                        }
                        
                        else
                        {
                            Int32 rv = 1;
                            Int32 hv = 1;

                            using (SqlConnection myTxConn = new SqlConnection(DBCon))
                            {
                                myTxConn.Open();
                                SqlCommand cmd = new SqlCommand("SELECT itemcode_runno as abc from itemcode_runno where cate_code = '" + txt_Category.Text + "' and Supplier_Code = '" + Request.QueryString["merchant"] + "'", myTxConn);

                                SqlDataReader reader = cmd.ExecuteReader();
                                if (reader.HasRows)
                                {
                                    if (reader.Read())
                                    {

                                        count = Convert.ToInt32((reader["abc"]).ToString());
                                        count += 1;
                                        // txt_Category.Text = String.Empty;
                                        txt_ItemCode.Text = String.Empty;
                                        //txt_Barcode.Text = String.Empty;
                                        txt_Subcode.Text = String.Empty;
                                        txt_ItemCode.Text += txt_Category.Text + "-" + count.ToString();
                                        //txt_Barcode.Text = txt_ItemCode.Text;
                                        if (ddlType.SelectedValue == "1")
                                        {
                                            txt_Subcode.Text = txt_ItemCode.Text;
                                        }

                                        reader.Close();
                                        reader.Dispose();

                                        using (SqlCommand comd = new SqlCommand("update itemcode_runno set itemcode_runno= @itemrunno where cate_code='" + txt_Category.Text + "' and supplier_code='" + Request.QueryString["merchant"] + "'", myTxConn))
                                        {
                                            comd.Parameters.AddWithValue("@itemrunno", count);
                                            comd.ExecuteNonQuery();
                                        }
                                        //}
                                    }
                                }
                                else
                                {
                                    txt_ItemCode.Text = txt_Category.Text + "-1000001";
                                    //txt_Barcode.Text = txt_ItemCode.Text;
                                    if (ddlType.SelectedValue == "1")
                                    {
                                        txt_Subcode.Text = txt_ItemCode.Text;
                                    }

                                    using (SqlConnection conn = new SqlConnection(DBCon))
                                    {
                                        conn.Open();
                                        using (SqlCommand comd = new SqlCommand("insert into itemcode_runno(supplier_Code,itemcode_runno,cate_Code) values('" + Request.QueryString["merchant"] + "', @itemcode_runno,@cat_code)", conn))
                                        {
                                            comd.Parameters.AddWithValue("@itemcode_runno", "1000001");
                                            comd.Parameters.AddWithValue("@cat_code", txt_Category.Text);
                                            comd.ExecuteNonQuery();
                                        }
                                        conn.Close();
                                    }
                                }

                                myTxConn.Close();

                            }

                            using (SqlConnection myTxConn = new SqlConnection(DBCon))
                            {
                                using (SqlCommand myTxcmd = new SqlCommand("Check_Item", myTxConn))
                                {
                                    myTxcmd.CommandType = CommandType.StoredProcedure;
                                    myTxcmd.Parameters.AddWithValue("@ItemCode", txt_ItemCode.Text.Trim().Replace("'", "`"));
                                    myTxcmd.Parameters.AddWithValue("@ItemID", "");
				    myTxcmd.Parameters.AddWithValue("@suppliercode", Request.QueryString["merchant"].ToString());

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

                            using (SqlConnection myFxConn = new SqlConnection(DBCon))
                            {
                                using (SqlCommand myFxcmd = new SqlCommand("Check_Item", myFxConn))
                                {
                                    myFxcmd.CommandType = CommandType.StoredProcedure;
                                    myFxcmd.Parameters.AddWithValue("@ItemCode", txt_Barcode.Text.Trim().Replace("'", "`"));
                                    myFxcmd.Parameters.AddWithValue("@ItemID", "");
				    myFxcmd.Parameters.AddWithValue("@suppliercode", Request.QueryString["merchant"].ToString());

                                    SqlParameter SReturnValue = myFxcmd.Parameters.Add("returnValue", SqlDbType.Int, 4);
                                    SReturnValue.Direction = ParameterDirection.ReturnValue;

                                    myFxConn.Open();

                                    try
                                    {
                                        myFxcmd.ExecuteNonQuery();
                                    }
                                    catch (Exception ex)
                                    {
                                        throw (ex);
                                    }
                                    finally
                                    {
                                        myFxcmd.Dispose();
                                        myFxConn.Close();
                                    }

                                    hv = Convert.ToInt32(SReturnValue.Value.ToString());
                                }
                            }

                            if (rv == 1)
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Item code exist in database before.');", true);
                                txt_ItemCode.Focus();
                            }
                            else if (hv == 1)
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Barcode exist in database before.');", true);
                                txt_Barcode.Focus();
                            }

                            else
                            {
                                Int32 tv = 50;

                                using (SqlConnection myTxConn = new SqlConnection(DBCon))
                                {
                                    using (SqlCommand myTxcmd = new SqlCommand("Insert_Item2", myTxConn))
                                    {
                                        myTxcmd.CommandType = CommandType.StoredProcedure;
                                        myTxcmd.Parameters.AddWithValue("@ItemCode", txt_ItemCode.Text.Trim().Replace("'", "`"));
                                        myTxcmd.Parameters.AddWithValue("@Barcode", txt_Barcode.Text.Trim().Replace("'", "`"));
                                        myTxcmd.Parameters.AddWithValue("@Linkcode", txt_Subcode.Text.Trim().Replace("'", "`"));
                                        myTxcmd.Parameters.AddWithValue("@Type", ddlType.SelectedValue);
                                        myTxcmd.Parameters.AddWithValue("@LongDesc", txt_Desc.Text.Trim().Replace("'", "`").Replace("\"", "``"));
                                        myTxcmd.Parameters.AddWithValue("@LongDesc2", txt_Desc2.Text.Trim().Replace("'", "`").Replace("\"", "``"));
                                        myTxcmd.Parameters.AddWithValue("@ShortDesc", txt_ShortDesc.Text.Trim().Replace("'", "`"));
                                        myTxcmd.Parameters.AddWithValue("@OtherDesc", txt_OtherDesc.Text.Trim().Replace("'", "`"));
                                        myTxcmd.Parameters.AddWithValue("@CategoryCode", txt_Category.Text.Trim().Replace("'", "`"));
                                        myTxcmd.Parameters.AddWithValue("@Brand", ddlBrd.SelectedValue);
                                        myTxcmd.Parameters.AddWithValue("@PackSize", txt_Pack.Text.Trim().Replace("'", "`"));
                                        myTxcmd.Parameters.AddWithValue("@UOM", ddlUM.SelectedValue);
                                        myTxcmd.Parameters.AddWithValue("@FOC", ddlfoc.SelectedValue);
                                        myTxcmd.Parameters.AddWithValue("@Self_Order_Menu", ddlordertable.SelectedValue);
                                        if (txtweight.Text != "")
                                        {
                                            myTxcmd.Parameters.AddWithValue("@weight", Convert.ToDecimal(txtweight.Text.Trim().Replace("'", "`")));
                                        }
                                        else
                                        {
                                            myTxcmd.Parameters.AddWithValue("@weight", 0);
                                        }
                                        myTxcmd.Parameters.AddWithValue("@PurchaseTax", ddlPT.SelectedValue);
                                        myTxcmd.Parameters.AddWithValue("@SalesTax", ddlST.SelectedValue);
                                        myTxcmd.Parameters.AddWithValue("@IsRedemption", ddl_RdmItem.SelectedValue);
                                        if (txt_RdmPoint.Text != "")
                                        {
                                            myTxcmd.Parameters.AddWithValue("@RedemptionPoint", Convert.ToDecimal(txt_RdmPoint.Text.Trim().Replace("'", "`")));
                                        }
                                        else
                                        {
                                            myTxcmd.Parameters.AddWithValue("@RedemptionPoint", 0);
                                        }
                                        if (lblRefProfitBonusPercent.Visible == false)
                                        {
                                            myTxcmd.Parameters.AddWithValue("@RefProfitBonusPercent", Convert.ToDecimal(txtRefProfitBonusPercent.Text.Trim().Replace("'", "`")));
                                        }
                                        else
                                        {
                                            myTxcmd.Parameters.AddWithValue("@RefProfitBonusPercent", 0);
                                        }
                                        if (txtTeamKPI.Text != "")
                                        {
                                            myTxcmd.Parameters.AddWithValue("@TeamKPI", Convert.ToDecimal(txtTeamKPI.Text.Trim().Replace("'", "`")));
                                        }
                                        else
                                        {
                                            myTxcmd.Parameters.AddWithValue("@TeamKPI", 0);
                                        }
                                        if (txtTeamKPI2.Text != "")
                                        {
                                            myTxcmd.Parameters.AddWithValue("@TeamKPI2", Convert.ToDecimal(txtTeamKPI2.Text.Trim().Replace("'", "`")));
                                        }
                                        else
                                        {
                                            myTxcmd.Parameters.AddWithValue("@TeamKPI2", 0);
                                        }
                                        if (txtMBD.Text != "")
                                        {
                                            myTxcmd.Parameters.AddWithValue("@MBD", Convert.ToDecimal(txtMBD.Text.Trim().Replace("'", "`")));
                                        }
                                        else
                                        {
                                            myTxcmd.Parameters.AddWithValue("@MBD", 0);
                                        }
                                        if (txtMEB.Text != "")
                                        {
                                            myTxcmd.Parameters.AddWithValue("@MEB", Convert.ToDecimal(txtMEB.Text.Trim().Replace("'", "`")));
                                        }
                                        else
                                        {
                                            myTxcmd.Parameters.AddWithValue("@MEB", 0);
                                        }
                                        if (txtteamgrowthinc1.Text != "")
                                        {
                                            myTxcmd.Parameters.AddWithValue("@teamgrowthinc1", Convert.ToDecimal(txtteamgrowthinc1.Text.Trim().Replace("'", "`")));
                                        }
                                        else
                                        {
                                            myTxcmd.Parameters.AddWithValue("@teamgrowthinc1", 0);
                                        }
                                        if (txtteamgrowthinc2.Text != "")
                                        {
                                            myTxcmd.Parameters.AddWithValue("@teamgrowthinc2", Convert.ToDecimal(txtteamgrowthinc2.Text.Trim().Replace("'", "`")));
                                        }
                                        else
                                        {
                                            myTxcmd.Parameters.AddWithValue("@teamgrowthinc2", 0);
                                        }
                                        if (txtESProfit.Text != "")
                                        {
                                            myTxcmd.Parameters.AddWithValue("@ESProfit", Convert.ToDecimal(txtESProfit.Text.Trim().Replace("'", "`")));
                                        }
                                        else
                                        {
                                            myTxcmd.Parameters.AddWithValue("@ESProfit", 0);
                                        }
                                        if (txtSPP.Text != "")
                                        {
                                            myTxcmd.Parameters.AddWithValue("@SPP", Convert.ToDecimal(txtSPP.Text.Trim().Replace("'", "`")));
                                        }
                                        else
                                        {
                                            myTxcmd.Parameters.AddWithValue("@SPP", 0);
                                        }

                                        //comm setting
                                        myTxcmd.Parameters.AddWithValue("@commtype", ddlcommtype.SelectedValue);
                                        myTxcmd.Parameters.AddWithValue("@commby", ddlcommby.SelectedValue);
                                        if (txtcommamt.Text == "")
                                        {
                                            myTxcmd.Parameters.AddWithValue("@commamt", "0");
                                        }
                                        else
                                        {
                                            myTxcmd.Parameters.AddWithValue("@commamt", txtcommamt.Text);
                                        }
                                        myTxcmd.Parameters.AddWithValue("@commgrp", ddlcommgroup.SelectedValue);

                                        myTxcmd.Parameters.AddWithValue("@supplier", Request.QueryString["merchant"].ToString());
                                        myTxcmd.Parameters.AddWithValue("@overview", txtEditor.Text);

                                        if (txtvendorcost.Text != "")
                                        {
                                            myTxcmd.Parameters.AddWithValue("@cost", txtvendorcost.Text);
                                        }
                                        else
                                        {
                                            myTxcmd.Parameters.AddWithValue("@cost", "0");
                                        }

                                        if (txtmaxbuy.Text != "")
                                        {
                                            myTxcmd.Parameters.AddWithValue("@maxbuylimit", txtmaxbuy.Text);
                                        }
                                        else
                                        {
                                            myTxcmd.Parameters.AddWithValue("@maxbuylimit", "0");
                                        }
                                        //myTxcmd.Parameters.AddWithValue("@maxbuylimit", txtmaxbuy.Text);
                                        //myTxcmd.Parameters.AddWithValue("@videolink1", txtVlink.Text);
                                        if (!string.IsNullOrEmpty(txtVlink.Text))
                                        {
                                            string videourl1 = "";
                                            string youtubeurl1 = "";
                                            if (txtVlink.Text.EndsWith("?rel=0"))
                                            {
                                                youtubeurl1 = txtVlink.Text.Trim();
                                                myTxcmd.Parameters.AddWithValue("@videolink1", youtubeurl1);
                                            }
                                            else if (txtVlink.Text.StartsWith("https://youtu.be/"))
                                            {
                                                videourl1 = txtVlink.Text.Trim().Substring(17);
                                                videourl1 = videourl1.Substring(0, 11);
                                                youtubeurl1 = "https://www.youtube.com/embed/" + videourl1 + "?rel=0";

                                                myTxcmd.Parameters.AddWithValue("@videolink1", youtubeurl1);
                                            }
                                            else if (txtVlink.Text.StartsWith("https://www.youtube.com/shorts/"))
                                            {
                                                videourl1 = txtVlink.Text.Trim().Substring(31);
                                                videourl1 = videourl1.Substring(0, 11);
                                                youtubeurl1 = "https://www.youtube.com/embed/" + videourl1 + "?rel=0";

                                                myTxcmd.Parameters.AddWithValue("@videolink1", youtubeurl1);
                                            }
                                            else
                                            {
                                                videourl1 = txtVlink.Text.Trim().Substring(32);
                                                videourl1 = videourl1.Substring(0, 11);
                                                youtubeurl1 = "https://www.youtube.com/embed/" + videourl1 + "?rel=0";

                                                myTxcmd.Parameters.AddWithValue("@videolink1", youtubeurl1);
                                            }
                                        }
                                        else
                                        {
                                            myTxcmd.Parameters.AddWithValue("@videolink1", txtVlink.Text);
                                        }

                                        if (!string.IsNullOrEmpty(txtVlink2.Text))
                                        {
                                            string videourl2 = "";
                                            string youtubeurl2 = "";
                                            if (txtVlink2.Text.EndsWith("?rel=0"))
                                            {
                                                youtubeurl2 = txtVlink2.Text.Trim();
                                                myTxcmd.Parameters.AddWithValue("@videolink2", youtubeurl2);
                                            }
                                            else if (txtVlink2.Text.StartsWith("https://youtu.be/"))
                                            {
                                                videourl2 = txtVlink2.Text.Trim().Substring(17);
                                                videourl2 = videourl2.Substring(0, 11);
                                                youtubeurl2 = "https://www.youtube.com/embed/" + videourl2 + "?rel=0";

                                                myTxcmd.Parameters.AddWithValue("@videolink2", youtubeurl2);
                                            }
                                            else if (txtVlink2.Text.StartsWith("https://www.youtube.com/shorts/"))
                                            {
                                                videourl2 = txtVlink2.Text.Trim().Substring(31);
                                                videourl2 = videourl2.Substring(0, 11);
                                                youtubeurl2 = "https://www.youtube.com/embed/" + videourl2 + "?rel=0";

                                                myTxcmd.Parameters.AddWithValue("@videolink2", youtubeurl2);
                                            }
                                            else
                                            {
                                                videourl2 = txtVlink2.Text.Trim().Substring(32);
                                                videourl2 = videourl2.Substring(0, 11);
                                                youtubeurl2 = "https://www.youtube.com/embed/" + videourl2 + "?rel=0";

                                                myTxcmd.Parameters.AddWithValue("@videolink2", youtubeurl2);
                                            }
                                        }
                                        else
                                        {
                                            myTxcmd.Parameters.AddWithValue("@videolink2", txtVlink2.Text);
                                        }

                                        if (!string.IsNullOrEmpty(txtVlink3.Text))
                                        {
                                            string videourl3 = "";
                                            string youtubeurl3 = "";
                                            if (txtVlink3.Text.EndsWith("?rel=0"))
                                            {
                                                youtubeurl3 = txtVlink3.Text.Trim();
                                                myTxcmd.Parameters.AddWithValue("@videolink3", youtubeurl3);
                                            }
                                            else if (txtVlink3.Text.StartsWith("https://youtu.be/"))
                                            {
                                                videourl3 = txtVlink3.Text.Trim().Substring(17);
                                                videourl3 = videourl3.Substring(0, 11);
                                                youtubeurl3 = "https://www.youtube.com/embed/" + videourl3 + "?rel=0";

                                                myTxcmd.Parameters.AddWithValue("@videolink3", youtubeurl3);
                                            }
                                            else if (txtVlink3.Text.StartsWith("https://www.youtube.com/shorts/"))
                                            {
                                                videourl3 = txtVlink3.Text.Trim().Substring(31);
                                                videourl3 = videourl3.Substring(0, 11);
                                                youtubeurl3 = "https://www.youtube.com/embed/" + videourl3 + "?rel=0";

                                                myTxcmd.Parameters.AddWithValue("@videolink3", youtubeurl3);
                                            }
                                            else
                                            {
                                                videourl3 = txtVlink3.Text.Trim().Substring(32);
                                                videourl3 = videourl3.Substring(0, 11);
                                                youtubeurl3 = "https://www.youtube.com/embed/" + videourl3 + "?rel=0";

                                                myTxcmd.Parameters.AddWithValue("@videolink3", youtubeurl3);
                                            }
                                        }
                                        else
                                        {
                                            myTxcmd.Parameters.AddWithValue("@videolink3", txtVlink3.Text);
                                        }

                                        myTxcmd.Parameters.AddWithValue("@weightitem", ddlweightitem.SelectedValue);
                                        myTxcmd.Parameters.AddWithValue("@weighttype", RDweighttype.SelectedValue);

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
                                                        myTxcmd2.Parameters.AddWithValue("@runno", "1");

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

                                                        myTxcmd3.Parameters.AddWithValue("@runno", "1");


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
                                                string FileNameU = "IMG" + "-" + DateTime.UtcNow.AddHours(8).ToString("yyMMddHHmmss") + runno.ToString() + extension;
                                                string TbmFileNameU = "Tbm-IMG" + "-" + DateTime.UtcNow.AddHours(8).ToString("yyMMddHHmmss") + runno.ToString() + extension;
                                                string TbmFileNameRESize = "Tbm-IMG" + "-" + DateTime.UtcNow.AddHours(8).ToString("yyMMddHHmmss") + runno.ToString();


                                                string path = Server.MapPath("ProductImg/" + "IMG - pos_MF_32323232" + "/");
                                                string delpath = Server.MapPath("ProductImg/" + "Thumbnail-IMG-pos_MF_32323232" + "/" + TbmFileNameU);

                                                if (Directory.Exists(path))
                                                {
                                                    fileupload2.PostedFile.SaveAs(Server.MapPath("ProductImg/" + "IMG - pos_MF_32323232" + "/" + FileNameU));
                                                    fileupload2.PostedFile.SaveAs(Server.MapPath("ProductImg/" + "Thumbnail-IMG-pos_MF_32323232" + "/" + TbmFileNameU));
                                                    string pth = Server.MapPath("ProductImg/" + "Thumbnail-IMG-pos_MF_32323232" + "/" + TbmFileNameU);
                                                    resizeImageAndSave(pth);

                                                    

                                                    //Stream strm = fileupload2.PostedFile.InputStream;
                                                    //using (var image = System.Drawing.Image.FromStream(strm))
                                                    //{
                                                    //    int newWidth = 180;
                                                    //    int newHeight = 180;
                                                    //    var thumbImg = new Bitmap(newWidth, newHeight);
                                                    //    var thumbGraph = Graphics.FromImage(thumbImg);
                                                    //    thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                                                    //    thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                                                    //    thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                                    //    var imgRectangle = new Rectangle(0, 0, newWidth, newHeight);
                                                    //    thumbGraph.DrawImage(image, imgRectangle);
                                                    //    string targetPath = Server.MapPath(@"~\ProductImg\IMG - pos_MF_32323232\") + FileNameU;
                                                    //    string targetTbmPath = Server.MapPath(@"~\ProductImg\Thumbnail-IMG-pos_MF_32323232\") + TbmFileNameU;
                                                    //    thumbImg.Save(targetPath, ImageFormat.Jpeg);
                                                    //    thumbImg.Save(targetTbmPath, ImageFormat.Jpeg);
                                                    //    thumbImg.Dispose();
                                                    //}

                                                    using (SqlConnection myQRConn = new SqlConnection(DBCon))
                                                    {
                                                        using (SqlCommand QRcmd = new SqlCommand("update MF_Item set Thumbnail_FilePath=@thumbnail,FilePath=@photo where Item_Code='" + txt_ItemCode.Text.Trim().Replace("'", "`") + "' and DeleteInd <> 'X'", myQRConn))
                                                        {
                                                            QRcmd.Parameters.AddWithValue("@photo", @"https://ezyshare.online/EzyShareListing/ProductImg/" + "IMG - pos_MF_32323232" + "/" + FileNameU);
                                                            QRcmd.Parameters.AddWithValue("@thumbnail", @"https://ezyshare.online/EzyShareListing/ProductImg/" + "Thumbnail-IMG-pos_MF_32323232" + "/" + TbmFileNameRESize + "_Thumbnail" + extension);
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

                                                    using (SqlConnection myQRConn = new SqlConnection(DBCon))
                                                    {
                                                        using (SqlCommand QRcmd = new SqlCommand("Insert into MF_Multi_Img_miso (Item_Code, Main_Indicator, Default_Indicator,Modified_DT,DeleteInd,FilePath,Thumbnail_FilePath)" +
                                                            "Values(@ItemCode,@Main_Indicator,@Default_Indicator,DATEADD(HOUR, 8, CONVERT(varchar(20),GETUTCDATE(),120)),'',@FilePath,@Thumbnail_FilePath)", myQRConn))
                                                        {
                                                            QRcmd.Parameters.AddWithValue("@ItemCode", txt_ItemCode.Text.Trim().Replace("'", "`"));



                                                            QRcmd.Parameters.AddWithValue("@Main_Indicator", "True");
                                                            if (fileupload2.PostedFile != null)
                                                            {
                                                                QRcmd.Parameters.AddWithValue("@Default_Indicator", "1");
                                                            }
                                                            else
                                                            {
                                                                QRcmd.Parameters.AddWithValue("@Default_Indicator", "0");
                                                            }
                                                            QRcmd.Parameters.AddWithValue("@FilePath", @"https://ezyshare.online/EzyShareListing/ProductImg/" + "IMG - pos_MF_32323232" + "/" + FileNameU);
                                                            QRcmd.Parameters.AddWithValue("@Thumbnail_FilePath", @"https://ezyshare.online/EzyShareListing/ProductImg/" + "Thumbnail-IMG-pos_MF_32323232" + "/" + TbmFileNameRESize + "_Thumbnail" + extension);
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
                                                  
                                                }
                                                else
                                                {
                                                    System.IO.Directory.CreateDirectory(Server.MapPath("ProductImg/" + "IMG - pos_MF_32323232" + "/"));
                                                    System.IO.Directory.CreateDirectory(Server.MapPath("ProductImg/" + "Thumbnail-IMG-pos_MF_32323232" + "/"));

                                                    fileupload2.PostedFile.SaveAs(Server.MapPath("ProductImg/" + "IMG - pos_MF_32323232" + "/" + FileNameU));
                                                    fileupload2.PostedFile.SaveAs(Server.MapPath("ProductImg/" + "Thumbnail-IMG-pos_MF_32323232" + "/" + TbmFileNameU));
                                                    string pth = Server.MapPath("ProductImg/" + "Thumbnail-IMG-pos_MF_32323232" + "/" + TbmFileNameU);
                                                    resizeImageAndSave(pth);

                                                   
                                                    //Stream strm = fileupload2.PostedFile.InputStream;
                                                    //using (var image = System.Drawing.Image.FromStream(strm))
                                                    //{
                                                    //    int newWidth = 180;
                                                    //    int newHeight = 180;
                                                    //    var thumbImg = new Bitmap(newWidth, newHeight);
                                                    //    var thumbGraph = Graphics.FromImage(thumbImg);
                                                    //    thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                                                    //    thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                                                    //    thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                                    //    var imgRectangle = new Rectangle(0, 0, newWidth, newHeight);
                                                    //    thumbGraph.DrawImage(image, imgRectangle);
                                                    //    string targetPath = Server.MapPath(@"~\ProductImg\IMG - pos_MF_32323232\") + FileNameU;
                                                    //    string targetTbmPath = Server.MapPath(@"~\ProductImg\Thumbnail-IMG-pos_MF_32323232\") + TbmFileNameU;
                                                    //    thumbImg.Save(targetPath, ImageFormat.Jpeg);
                                                    //    thumbImg.Save(targetTbmPath, ImageFormat.Jpeg);
                                                    //    thumbImg.Dispose();
                                                    //}
                                                    using (SqlConnection myQRConn = new SqlConnection(DBCon))
                                                    {
                                                        using (SqlCommand QRcmd = new SqlCommand("update MF_Item set Thumbnail_FilePath=@thumbnail,FilePath=@photo where Item_Code='" + txt_ItemCode.Text.Trim().Replace("'", "`") + "' and DeleteInd <> 'X'", myQRConn))
                                                        {
                                                            QRcmd.Parameters.AddWithValue("@photo", @"https://ezyshare.online/EzyShareListing/ProductImg/" + "IMG - pos_MF_32323232" + "/" + FileNameU);
                                                            QRcmd.Parameters.AddWithValue("@thumbnail", @"https://ezyshare.online/EzyShareListing/ProductImg/" + "Thumbnail-IMG-pos_MF_32323232" + "/" + TbmFileNameRESize + "_Thumbnail" + extension);

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

                                                    using (SqlConnection myQRConn = new SqlConnection(DBCon))
                                                    {
                                                        using (SqlCommand QRcmd = new SqlCommand("Insert into MF_Multi_Img_miso (Item_Code, Main_Indicator, Default_Indicator,Modified_DT,DeleteInd,FilePath,Thumbnail_FilePath)" +
                                                            "Values(@ItemCode,@Main_Indicator,@Default_Indicator,DATEADD(HOUR, 8, CONVERT(varchar(20),GETUTCDATE(),120)),'',@FilePath, @Thumbnail_FilePath)", myQRConn))
                                                        {
                                                            QRcmd.Parameters.AddWithValue("@ItemCode", txt_ItemCode.Text.Trim().Replace("'", "`"));



                                                            QRcmd.Parameters.AddWithValue("@Main_Indicator", "True");
                                                            if (fileupload2.PostedFile != null)
                                                            {
                                                                QRcmd.Parameters.AddWithValue("@Default_Indicator", "1");
                                                            }
                                                            else
                                                            {
                                                                QRcmd.Parameters.AddWithValue("@Default_Indicator", "0");
                                                            }

                                                            QRcmd.Parameters.AddWithValue("@FilePath", @"https://ezyshare.online/EzyShareListing/ProductImg/" + "IMG - pos_MF_32323232" + "/" + FileNameU);
                                                            QRcmd.Parameters.AddWithValue("@Thumbnail_FilePath", @"https://ezyshare.online/EzyShareListing/ProductImg/" + "Thumbnail-IMG-pos_MF_32323232" + "/" + TbmFileNameRESize + "_Thumbnail" + extension);
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
                                                   
                                                }
                                                //if (File.Exists(delpath))
                                                //{
                                                //    File.Delete(delpath);
                                                //}
                                            }

                                        }
                                        using (SqlConnection myQRConn = new SqlConnection(DBCon))
                                        {
                                            myQRConn.Open();
                                            using (SqlCommand QRcmd = new SqlCommand("Insert into MF_AgentCommi ([Item_Code],[AgentLevelCode],[BuyPrice],[SalesCommission],[SponsorCommission],[BreakAwayCommission],[EarnRPWhenBuy],[RedemptionPoint],[Created_DT],[Modified_DT],[DeleteInd],[CashBackAmount])" +
                                                "Values(@ItemCode,'HQ','0','0','0','0',@EarnRPwhenBuy,@RedemptionPoint,dateadd(hour,(8),CONVERT([varchar](20),getutcdate(),(120))),dateadd(hour,(8),CONVERT([varchar](20),getutcdate(),(120))),'','0')", myQRConn))
                                            {
                                                QRcmd.Parameters.AddWithValue("@ItemCode", txt_ItemCode.Text.Trim().Replace("'", "`"));
                                                QRcmd.Parameters.AddWithValue("@RedemptionPoint", txt_RdmPoint.Text.Trim().Replace("'", "`"));
                                                QRcmd.Parameters.AddWithValue("@EarnRPwhenBuy", txtrewardpoint.Text.Trim().Replace("'", "`"));
                                                QRcmd.ExecuteNonQuery();
                                            }
                                            myQRConn.Close();
                                        }

                                        using (SqlConnection myQRConn = new SqlConnection(DBCon))
                                        {
                                            myQRConn.Open();
                                            using (SqlCommand QRcmd = new SqlCommand("Insert into MF_AgentCommi ([Item_Code],[AgentLevelCode],[BuyPrice],[SalesCommission],[SponsorCommission],[BreakAwayCommission],[EarnRPWhenBuy],[RedemptionPoint],[Created_DT],[Modified_DT],[DeleteInd],[CashBackAmount])" +
                                                "Values(@ItemCode,'D01','0','0','0','0',@EarnRPwhenBuy,@RedemptionPoint,dateadd(hour,(8),CONVERT([varchar](20),getutcdate(),(120))),dateadd(hour,(8),CONVERT([varchar](20),getutcdate(),(120))),'','0')", myQRConn))
                                            {
                                                QRcmd.Parameters.AddWithValue("@ItemCode", txt_ItemCode.Text.Trim().Replace("'", "`"));
                                                QRcmd.Parameters.AddWithValue("@RedemptionPoint", txt_RdmPoint.Text.Trim().Replace("'", "`"));
                                                QRcmd.Parameters.AddWithValue("@EarnRPwhenBuy", txtrewardpoint.Text.Trim().Replace("'", "`"));
                                                QRcmd.ExecuteNonQuery();
                                            }
                                            myQRConn.Close();
                                        }
                                        using (SqlConnection myQRConn = new SqlConnection(DBCon))
                                        {
                                            myQRConn.Open();
                                            using (SqlCommand QRcmd = new SqlCommand("Insert into MF_AgentCommi ([Item_Code],[AgentLevelCode],[BuyPrice],[SalesCommission],[SponsorCommission],[BreakAwayCommission],[EarnRPWhenBuy],[RedemptionPoint],[Created_DT],[Modified_DT],[DeleteInd],[CashBackAmount])" +
                                                "Values(@ItemCode,'D02','0','0','0','0',@EarnRPwhenBuy,@RedemptionPoint,dateadd(hour,(8),CONVERT([varchar](20),getutcdate(),(120))),dateadd(hour,(8),CONVERT([varchar](20),getutcdate(),(120))),'','0')", myQRConn))
                                            {
                                                QRcmd.Parameters.AddWithValue("@ItemCode", txt_ItemCode.Text.Trim().Replace("'", "`"));
                                                QRcmd.Parameters.AddWithValue("@RedemptionPoint", txt_RdmPoint.Text.Trim().Replace("'", "`"));
                                                QRcmd.Parameters.AddWithValue("@EarnRPwhenBuy", txtrewardpoint.Text.Trim().Replace("'", "`"));
                                                QRcmd.ExecuteNonQuery();
                                            }
                                            myQRConn.Close();
                                        }
                                        using (SqlConnection myQRConn = new SqlConnection(DBCon))
                                        {
                                            myQRConn.Open();
                                            using (SqlCommand QRcmd = new SqlCommand("Insert into MF_AgentCommi ([Item_Code],[AgentLevelCode],[BuyPrice],[SalesCommission],[SponsorCommission],[BreakAwayCommission],[EarnRPWhenBuy],[RedemptionPoint],[Created_DT],[Modified_DT],[DeleteInd],[CashBackAmount])" +
                                                "Values(@ItemCode,'D03','0','0','0','0',@EarnRPwhenBuy,@RedemptionPoint,dateadd(hour,(8),CONVERT([varchar](20),getutcdate(),(120))),dateadd(hour,(8),CONVERT([varchar](20),getutcdate(),(120))),'','0')", myQRConn))
                                            {
                                                QRcmd.Parameters.AddWithValue("@ItemCode", txt_ItemCode.Text.Trim().Replace("'", "`"));
                                                QRcmd.Parameters.AddWithValue("@RedemptionPoint", txt_RdmPoint.Text.Trim().Replace("'", "`"));
                                                QRcmd.Parameters.AddWithValue("@EarnRPwhenBuy", txtrewardpoint.Text.Trim().Replace("'", "`"));
                                                QRcmd.ExecuteNonQuery();
                                            }
                                            myQRConn.Close();
                                        }
                                        using (SqlConnection myQRConn = new SqlConnection(DBCon))
                                        {
                                            myQRConn.Open();
                                            using (SqlCommand QRcmd = new SqlCommand("Insert into MF_AgentCommi ([Item_Code],[AgentLevelCode],[BuyPrice],[SalesCommission],[SponsorCommission],[BreakAwayCommission],[EarnRPWhenBuy],[RedemptionPoint],[Created_DT],[Modified_DT],[DeleteInd],[CashBackAmount])" +
                                                "Values(@ItemCode,'D04','0','0','0','0',@EarnRPwhenBuy,@RedemptionPoint,dateadd(hour,(8),CONVERT([varchar](20),getutcdate(),(120))),dateadd(hour,(8),CONVERT([varchar](20),getutcdate(),(120))),'','0')", myQRConn))
                                            {
                                                QRcmd.Parameters.AddWithValue("@ItemCode", txt_ItemCode.Text.Trim().Replace("'", "`"));
                                                QRcmd.Parameters.AddWithValue("@RedemptionPoint", txt_RdmPoint.Text.Trim().Replace("'", "`"));
                                                QRcmd.Parameters.AddWithValue("@EarnRPwhenBuy", txtrewardpoint.Text.Trim().Replace("'", "`"));
                                                QRcmd.ExecuteNonQuery();
                                            }
                                            myQRConn.Close();
                                        }
                                        using (SqlConnection myQRConn = new SqlConnection(DBCon))
                                        {
                                            myQRConn.Open();
                                            using (SqlCommand QRcmd = new SqlCommand("Insert into MF_AgentCommi ([Item_Code],[AgentLevelCode],[BuyPrice],[SalesCommission],[SponsorCommission],[BreakAwayCommission],[EarnRPWhenBuy],[RedemptionPoint],[Created_DT],[Modified_DT],[DeleteInd],[CashBackAmount])" +
                                                "Values(@ItemCode,'D05','0','0','0','0',@EarnRPwhenBuy,@RedemptionPoint,dateadd(hour,(8),CONVERT([varchar](20),getutcdate(),(120))),dateadd(hour,(8),CONVERT([varchar](20),getutcdate(),(120))),'','0')", myQRConn))
                                            {
                                                QRcmd.Parameters.AddWithValue("@ItemCode", txt_ItemCode.Text.Trim().Replace("'", "`"));
                                                QRcmd.Parameters.AddWithValue("@RedemptionPoint", txt_RdmPoint.Text.Trim().Replace("'", "`"));
                                                QRcmd.Parameters.AddWithValue("@EarnRPwhenBuy", txtrewardpoint.Text.Trim().Replace("'", "`"));
                                                QRcmd.ExecuteNonQuery();
                                            }
                                            myQRConn.Close();
                                        }



                                        for (int yy = 0; yy < rpt_Item2.Items.Count; yy++)
                                        {

                                            using (SqlConnection PLUConn = new SqlConnection(DBCon))
                                            {
                                                using (SqlCommand PLUcmd = new SqlCommand("Insert_PLU", PLUConn))
                                                {
                                                    PLUcmd.CommandType = CommandType.StoredProcedure;
                                                    PLUcmd.Parameters.AddWithValue("@StoreCode", "HQ");
                                                    PLUcmd.Parameters.AddWithValue("@ItemCode", txt_ItemCode.Text.Trim().Replace("'", "`"));
                                                    PLUcmd.Parameters.AddWithValue("@Barcode", txt_Barcode.Text.Trim().Replace("'", "`"));
                                                    PLUcmd.Parameters.AddWithValue("@Linkcode", txt_Subcode.Text.Trim().Replace("'", "`"));


                                                    PLUConn.Open();

                                                    try
                                                    {
                                                        PLUcmd.ExecuteNonQuery();
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        throw (ex);
                                                    }
                                                    finally
                                                    {
                                                        PLUcmd.Dispose();
                                                        PLUConn.Close();
                                                    }
                                                }
                                            }
                                        }

                                        Session["Item_Price"] = txt_ItemCode.Text;
                                        Session["PName"] = "Price Setup";

                                        if (Request.QueryString["user"] != null)
                                        {
                                            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Insert success.');window.location ='ContentApproval.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "&user=" + Request.QueryString["user"].ToString() + "';", true);
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Insert success.');window.location ='ContentApproval.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "';", true);
                                        }

                                        //ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Insert success.');window.location ='ContentApproval.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "&user=" + Request.QueryString["user"].ToString() + "';", true);
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Insert failed. Please try again later.');", true);
                                        txt_ItemCode.Focus();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    protected void ShowData()
    {
        var dt = new DataTable();
        var con = new SqlConnection(DBCon);
        con.Open();

        //Select c.vendor_name,b.costapply,a.supplier_1,a.store_code,b.store_name,a.standardcost,a.avgcost,a.lastcost,a.previouscost,a.branchcost from mf_plu as a inner join mf_store as b on a.store_code = b.store_code inner join vms_vendor as c on a.supplier_1 = c.vendor_code where a.item_code = '" + Request.QueryString["item_code"] + "' and a.Deleteind <> 'X'
        var adapt = new SqlDataAdapter("Select a.redemptionpoint, a.earnrpwhenbuy, b.levelname, a.AgentLevelCode from mf_agentcommi as a inner join erp_agentlvl as b on a.AgentLevelCode = b.levelcode where a.item_code = '" + Request.QueryString["item_code"] + "' and a.AgentLevelCode <> 'HQ' and a.Deleteind <> 'X'", con);
        adapt.Fill(dt);
        if (dt.Rows.Count > 0)
        {
            grd_view.DataSource = dt;
            //GridViewHelper helper = new GridViewHelper(this.grd_view);
            //helper.RegisterGroup("vendor_name", true, true);
            grd_view.DataBind();
        }
        con.Close();
    }

  
    protected void Update_OnClick(object sender, EventArgs e)
    {
        
        if (txtTeamKPI.Text == "")
		{
			txtTeamKPI.Text = "0";
		}
		if (txtTeamKPI2.Text == "")
		{
			txtTeamKPI2.Text = "0";
		}
		if (txtMEB.Text == "")
		{
			txtMEB.Text = "0";
		}
		if (txtESProfit.Text == "")
		{
			txtESProfit.Text = "0";
		}
		if (txtMBD.Text == "")
		{
			txtMBD.Text = "0";
		}
        if (txtteamgrowthinc1.Text == "")
        {
            txtteamgrowthinc1.Text = "0";
        }
        if (txtteamgrowthinc2.Text == "")
        {
            txtteamgrowthinc2.Text = "0";
        }
        using (SqlConnection mySConn1 = new SqlConnection(DBCon))
        {
            using (SqlCommand myScmd = new SqlCommand("select * from AP_Merchant where merchantid = '" + Request.QueryString["merchant"] + "'", mySConn1))
            {
                SqlDataAdapter adp = new SqlDataAdapter(myScmd);
                DataTable dt = new DataTable();
                adp.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["MerchantType"].ToString() != "Markup")
                    {
                         if(txt_Barcode.Text == "")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Barcode cannot be empty.');", true);
                            txt_Barcode.Focus();
                        }
                        else if (ddlUM.SelectedValue == "")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Please select UOM.');", true);
                            ddlUM.Focus();
                        }
                        else
                        {
                            Int32 tv = 50;

                            using (SqlConnection myTxConn = new SqlConnection(DBCon))
                            {
                                using (SqlCommand myTxcmd = new SqlCommand("Update_Item2", myTxConn))
                                {
                                    myTxcmd.CommandType = CommandType.StoredProcedure;
                                    myTxcmd.Parameters.AddWithValue("@ItemCode", txt_ItemCode.Text.Trim().Replace("'", "`"));
                                    myTxcmd.Parameters.AddWithValue("@Barcode", txt_Barcode.Text.Trim().Replace("'", "`"));
                                    myTxcmd.Parameters.AddWithValue("@Linkcode", txt_Subcode.Text.Trim().Replace("'", "`"));
                                    myTxcmd.Parameters.AddWithValue("@Type", ddlType.SelectedValue);
                                    myTxcmd.Parameters.AddWithValue("@LongDesc", txt_Desc.Text.Trim().Replace("'", "`").Replace("\"", "``"));
                                    myTxcmd.Parameters.AddWithValue("@LongDesc2", txt_Desc2.Text.Trim().Replace("'", "`").Replace("\"", "``"));
                                    myTxcmd.Parameters.AddWithValue("@ShortDesc", txt_ShortDesc.Text.Trim().Replace("'", "`"));
                                    myTxcmd.Parameters.AddWithValue("@OtherDesc", txt_OtherDesc.Text.Trim().Replace("'", "`"));
                                    myTxcmd.Parameters.AddWithValue("@CategoryCode", txt_Category.Text.Trim().Replace("'", "`"));
                                    myTxcmd.Parameters.AddWithValue("@Brand", ddlBrd.SelectedValue);
                                    myTxcmd.Parameters.AddWithValue("@PackSize", txt_Pack.Text.Trim().Replace("'", "`"));
                                    myTxcmd.Parameters.AddWithValue("@UOM", ddlUM.SelectedValue);
                                    myTxcmd.Parameters.AddWithValue("@PurchaseTax", ddlPT.SelectedValue);
                                    myTxcmd.Parameters.AddWithValue("@SalesTax", ddlST.SelectedValue);
                                    myTxcmd.Parameters.AddWithValue("@IsRedemption", ddl_RdmItem.SelectedValue);
                                    myTxcmd.Parameters.AddWithValue("@FOC", ddlfoc.SelectedValue);
                                    myTxcmd.Parameters.AddWithValue("@Self_Order_Menu", ddlordertable.SelectedValue);
                                    if (txt_RdmPoint.Text != "")
                                    {
                                        myTxcmd.Parameters.AddWithValue("@RedemptionPoint", Convert.ToDecimal(txt_RdmPoint.Text.Trim().Replace("'", "`")));
                                    }
                                    else
                                    {
                                        myTxcmd.Parameters.AddWithValue("@RedemptionPoint", 0);
                                    }
                                    //myTxcmd.Parameters.AddWithValue("@RedemptionPoint", Convert.ToDecimal(txt_RdmPoint.Text.Trim().Replace("'", "`")));
                                    if (lblRefProfitBonusPercent.Visible == false)
                                    {
                                        myTxcmd.Parameters.AddWithValue("@RefProfitBonusPercent", Convert.ToDecimal(txtRefProfitBonusPercent.Text.Trim().Replace("'", "`")));
                                    }
                                    else
                                    {
                                        myTxcmd.Parameters.AddWithValue("@RefProfitBonusPercent", 0);
                                    }
                                    if (txtTeamKPI.Text != "")
                                    {
                                        myTxcmd.Parameters.AddWithValue("@TeamKPI", Convert.ToDecimal(txtTeamKPI.Text.Trim().Replace("'", "`")));
                                    }
                                    else
                                    {
                                        myTxcmd.Parameters.AddWithValue("@TeamKPI", 0);
                                    }
                                    if (txtTeamKPI2.Text != "")
                                    {
                                        myTxcmd.Parameters.AddWithValue("@TeamKPI2", Convert.ToDecimal(txtTeamKPI2.Text.Trim().Replace("'", "`")));
                                    }
                                    else
                                    {
                                        myTxcmd.Parameters.AddWithValue("@TeamKPI2", 0);
                                    }
                                    if (txtMBD.Text != "")
                                    {
                                        myTxcmd.Parameters.AddWithValue("@MBD", Convert.ToDecimal(txtMBD.Text.Trim().Replace("'", "`")));
                                    }
                                    else
                                    {
                                        myTxcmd.Parameters.AddWithValue("@MBD", 0);
                                    }
                                    if (txtMEB.Text != "")
                                    {
                                        myTxcmd.Parameters.AddWithValue("@MEB", Convert.ToDecimal(txtMEB.Text.Trim().Replace("'", "`")));
                                    }
                                    else
                                    {
                                        myTxcmd.Parameters.AddWithValue("@MEB", 0);
                                    }
                                    if (txtteamgrowthinc1.Text != "")
                                    {
                                        myTxcmd.Parameters.AddWithValue("@teamgrowthinc1", Convert.ToDecimal(txtteamgrowthinc1.Text.Trim().Replace("'", "`")));
                                    }
                                    else
                                    {
                                        myTxcmd.Parameters.AddWithValue("@teamgrowthinc1", 0);
                                    }
                                    if (txtteamgrowthinc2.Text != "")
                                    {
                                        myTxcmd.Parameters.AddWithValue("@teamgrowthinc2", Convert.ToDecimal(txtteamgrowthinc2.Text.Trim().Replace("'", "`")));
                                    }
                                    else
                                    {
                                        myTxcmd.Parameters.AddWithValue("@teamgrowthinc2", 0);
                                    }
                                    if (txtESProfit.Text != "")
                                    {
                                        myTxcmd.Parameters.AddWithValue("@ESProfit", Convert.ToDecimal(txtESProfit.Text.Trim().Replace("'", "`")));
                                    }
                                    else
                                    {
                                        myTxcmd.Parameters.AddWithValue("@ESProfit", 0);
                                    }
                                    if (txtSPP.Text != "")
                                    {
                                        myTxcmd.Parameters.AddWithValue("@SPP", Convert.ToDecimal(txtSPP.Text.Trim().Replace("'", "`")));
                                    }
                                    else
                                    {
                                        myTxcmd.Parameters.AddWithValue("@SPP", 0);
                                    }
                                    //              myTxcmd.Parameters.AddWithValue("@TeamKPI", Convert.ToDecimal(txtTeamKPI.Text.Trim().Replace("'", "`")));
                                    //myTxcmd.Parameters.AddWithValue("@TeamKPI2", Convert.ToDecimal(txtTeamKPI2.Text.Trim().Replace("'", "`")));
                                    //                  myTxcmd.Parameters.AddWithValue("@MBD", Convert.ToDecimal(txtMBD.Text.Trim().Replace("'", "`")));
                                    //                  myTxcmd.Parameters.AddWithValue("@MEB", Convert.ToDecimal(txtMEB.Text.Trim().Replace("'", "`")));
                                    //                  myTxcmd.Parameters.AddWithValue("@ESProfit", Convert.ToDecimal(txtESProfit.Text.Trim().Replace("'", "`")));
                                    //                  myTxcmd.Parameters.AddWithValue("@SPP", Convert.ToDecimal(txtSPP.Text.Trim().Replace("'", "`")));

                                    if (txtvendorcost.Text != "")
                                    {
                                        myTxcmd.Parameters.AddWithValue("@cost", txtvendorcost.Text);
                                    }
                                    else
                                    {
                                        myTxcmd.Parameters.AddWithValue("@cost", "0");
                                    }

                                    if (txtmaxbuy.Text != "")
                                    {
                                        myTxcmd.Parameters.AddWithValue("@maxbuylimit", txtmaxbuy.Text);
                                    }
                                    else
                                    {
                                        myTxcmd.Parameters.AddWithValue("@maxbuylimit", "0");
                                    }

                                    //comm setting
                                    myTxcmd.Parameters.AddWithValue("@commtype", ddlcommtype.SelectedValue);
                                    myTxcmd.Parameters.AddWithValue("@commby", ddlcommby.SelectedValue);
                                    if (txtcommamt.Text == "")
                                    {
                                        myTxcmd.Parameters.AddWithValue("@commamt", "0");
                                    }
                                    else
                                    {
                                        myTxcmd.Parameters.AddWithValue("@commamt", txtcommamt.Text);
                                    }
                                    myTxcmd.Parameters.AddWithValue("@commgrp", ddlcommgroup.SelectedValue);

                                    if (txtweight.Text != "")
                                    {
                                        myTxcmd.Parameters.AddWithValue("@weight", Convert.ToDecimal(txtweight.Text.Trim().Replace("'", "`")));
                                    }
                                    else
                                    {
                                        myTxcmd.Parameters.AddWithValue("@weight", 0);
                                    }
                                    myTxcmd.Parameters.AddWithValue("@overview", txtEditor.Text);
                                    if (!string.IsNullOrEmpty(txtVlink.Text))
                                    {
                                        string videourl1 = "";
                                        string youtubeurl1 = "";
                                        if (txtVlink.Text.EndsWith("?rel=0"))
                                        {
                                            youtubeurl1 = txtVlink.Text.Trim();
                                            myTxcmd.Parameters.AddWithValue("@videolink1", youtubeurl1);
                                        }
                                        else if (txtVlink.Text.StartsWith("https://youtu.be/"))
                                        {
                                            videourl1 = txtVlink.Text.Trim().Substring(17);
                                            videourl1 = videourl1.Substring(0, 11);
                                            youtubeurl1 = "https://www.youtube.com/embed/" + videourl1 + "?rel=0";

                                            myTxcmd.Parameters.AddWithValue("@videolink1", youtubeurl1);
                                        }
                                        else if (txtVlink.Text.StartsWith("https://www.youtube.com/watch?"))
                                        {
                                            videourl1 = txtVlink.Text.Trim().Substring(32);
                                            videourl1 = videourl1.Substring(0, 11);
                                            youtubeurl1 = "https://www.youtube.com/embed/" + videourl1 + "?rel=0";

                                            myTxcmd.Parameters.AddWithValue("@videolink1", youtubeurl1);
                                        }
                                        else if(txtVlink.Text.StartsWith("https://www.youtube.com/shorts/"))
                                        {
                                            videourl1 = txtVlink.Text.Trim().Substring(30);
                                            youtubeurl1 = "https://www.youtube.com/watch?v=" + videourl1;

                                            myTxcmd.Parameters.AddWithValue("@videolink1", youtubeurl1);
                                        }
                                        else if (txtVlink.Text.StartsWith("https://youtube.com/shorts/"))
                                        {
                                            videourl1 = txtVlink.Text.Trim().Substring(27);
                                            youtubeurl1 = "https://www.youtube.com/watch?v=" + videourl1 ;

                                            myTxcmd.Parameters.AddWithValue("@videolink1", youtubeurl1);
                                        }
                                    }
                                    else
                                    {
                                        myTxcmd.Parameters.AddWithValue("@videolink1", txtVlink.Text);
                                    }

                                    if (!string.IsNullOrEmpty(txtVlink2.Text))
                                    {
                                        string videourl2 = "";
                                        string youtubeurl2 = "";
                                        if (txtVlink2.Text.EndsWith("?rel=0"))
                                        {
                                            youtubeurl2 = txtVlink2.Text.Trim();
                                            myTxcmd.Parameters.AddWithValue("@videolink2", youtubeurl2);
                                        }
                                        else if (txtVlink2.Text.StartsWith("https://youtu.be/"))
                                        {
                                            videourl2 = txtVlink2.Text.Trim().Substring(17);
                                            videourl2 = videourl2.Substring(0, 11);
                                            youtubeurl2 = "https://www.youtube.com/embed/" + videourl2 + "?rel=0";

                                            myTxcmd.Parameters.AddWithValue("@videolink2", youtubeurl2);
                                        }
                                        else if (txtVlink2.Text.StartsWith("https://www.youtube.com/watch?"))
                                        {
                                            videourl2 = txtVlink2.Text.Trim().Substring(32);
                                            videourl2 = videourl2.Substring(0, 11);
                                            youtubeurl2 = "https://www.youtube.com/embed/" + videourl2 + "?rel=0";

                                            myTxcmd.Parameters.AddWithValue("@videolink2", youtubeurl2);
                                        }
                                        else if (txtVlink2.Text.StartsWith("https://www.youtube.com/shorts/"))
                                        {
                                            videourl2 = txtVlink2.Text.Trim().Substring(30);
                                            youtubeurl2 = "https://www.youtube.com/watch?v=" + videourl2 ;

                                            myTxcmd.Parameters.AddWithValue("@videolink2", youtubeurl2);
                                        }
                                        else if (txtVlink2.Text.StartsWith("https://youtube.com/shorts/"))
                                        {
                                            videourl2 = txtVlink2.Text.Trim().Substring(27);
                                            youtubeurl2 = "https://www.youtube.com/watch?v=" + videourl2 ;

                                            myTxcmd.Parameters.AddWithValue("@videolink2", youtubeurl2);
                                        }
                                    }
                                    else
                                    {
                                        myTxcmd.Parameters.AddWithValue("@videolink2", txtVlink2.Text);
                                    }

                                    if (!string.IsNullOrEmpty(txtVlink3.Text))
                                    {
                                        string videourl3 = "";
                                        string youtubeurl3 = "";
                                        if (txtVlink3.Text.EndsWith("?rel=0"))
                                        {
                                            youtubeurl3 = txtVlink3.Text.Trim();
                                            myTxcmd.Parameters.AddWithValue("@videolink3", youtubeurl3);
                                        }
                                        else if (txtVlink3.Text.StartsWith("https://youtu.be/"))
                                        {
                                            videourl3 = txtVlink3.Text.Trim().Substring(17);
                                            videourl3 = videourl3.Substring(0, 11);
                                            youtubeurl3 = "https://www.youtube.com/embed/" + videourl3 + "?rel=0";

                                            myTxcmd.Parameters.AddWithValue("@videolink3", youtubeurl3);
                                        }
                                        else if (txtVlink3.Text.StartsWith("https://www.youtube.com/watch?"))
                                        {
                                            videourl3 = txtVlink3.Text.Trim().Substring(32);
                                            videourl3 = videourl3.Substring(0, 11);
                                            youtubeurl3 = "https://www.youtube.com/embed/" + videourl3 + "?rel=0";

                                            myTxcmd.Parameters.AddWithValue("@videolink3", youtubeurl3);
                                        }
                                        else if (txtVlink3.Text.StartsWith("https://www.youtube.com/shorts/"))
                                        {
                                            videourl3 = txtVlink3.Text.Trim().Substring(30);
                                            youtubeurl3 = "https://www.youtube.com/watch?v=" + videourl3;

                                            myTxcmd.Parameters.AddWithValue("@videolink3", youtubeurl3);
                                        }
                                        else if (txtVlink3.Text.StartsWith("https://youtube.com/shorts/"))
                                        {
                                            videourl3 = txtVlink3.Text.Trim().Substring(27);
                                            youtubeurl3 = "https://www.youtube.com/watch?v=" + videourl3;

                                            myTxcmd.Parameters.AddWithValue("@videolink3", youtubeurl3);
                                        }
                                    }
                                    else
                                    {
                                        myTxcmd.Parameters.AddWithValue("@videolink3", txtVlink3.Text);
                                    }
                                    myTxcmd.Parameters.AddWithValue("@weightitem", ddlweightitem.SelectedValue);
                                    myTxcmd.Parameters.AddWithValue("@weighttype", RDweighttype.SelectedValue);

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
                                    using (SqlConnection PLUConn = new SqlConnection(DBCon))
                                    {
                                        string user = "";
                                        if (Request.QueryString["user"] != null)
                                        {
                                            user = Request.QueryString["user"].ToString();
                                        }
                                        else
                                        {
                                            user = Request.QueryString["merchant"].ToString();

                                        };

                                        PLUConn.Open();
                                        SqlCommand delcmd = new SqlCommand("update MF_Item_Modifier_Assign set deleteind='X' where item_code='"+ Request.QueryString["Item_code"].ToString() +"' and deleteind <> 'X'", PLUConn);
                                        delcmd.ExecuteNonQuery();

                                        foreach (GridViewRow row in grd_viewmodifer.Rows)
                                        {
                                            if (row.RowType == DataControlRowType.DataRow)
                                            {
                                                CheckBox chkmodifier = (CheckBox)row.FindControl("chkmodifier");
                                                if (chkmodifier.Checked)
                                                {
                                                    HiddenField hdmodifierID = (HiddenField)row.FindControl("hdmodifierID");

                                                    SqlCommand insertmodifiercmd = new SqlCommand("insert into MF_Item_Modifier_Assign(modifier_ID,Item_Code,assign_date,deleteind) values(@modifierID,@itemcode,dateadd(hour,(8),CONVERT([varchar](20),getutcdate(),(120))),'')", PLUConn);
                                                    insertmodifiercmd.Parameters.AddWithValue("@modifierID", hdmodifierID.Value);
                                                    insertmodifiercmd.Parameters.AddWithValue("@itemcode", Request.QueryString["Item_code"].ToString());
                                                    try
                                                    {
                                                        insertmodifiercmd.ExecuteNonQuery();
                                                    }
                                                    catch (Exception ex)
                                                    {

                                                    }
                                                }
                                            }

                                        }

                                        SqlCommand delprintercmd = new SqlCommand("delete from PosSys_PrinterItem where item_code='"+ Request.QueryString["Item_code"].ToString() +"'", PLUConn);
                                        delprintercmd.ExecuteNonQuery();

                                        foreach (GridViewRow row in grd_viewprinter.Rows)
                                        {
                                            if (row.RowType == DataControlRowType.DataRow)
                                            {
                                                CheckBox chkprinter = (CheckBox)row.FindControl("chkprinter");
                                                if (chkprinter.Checked)
                                                {
                                                    HiddenField hdprinterID = (HiddenField)row.FindControl("hdprinterID");

                                                    SqlCommand insertmodifiercmd = new SqlCommand("insert into PosSys_PrinterItem(Item_Code,Printer_ID,Merchant_Code,User_Code,Created_DT) values(@itemcode,@printerid,@merchantcode,@usercode,dateadd(hour,(8),CONVERT([varchar](20),getutcdate(),(120))))", PLUConn);
                                                    insertmodifiercmd.Parameters.AddWithValue("@printerid", hdprinterID.Value);
                                                    insertmodifiercmd.Parameters.AddWithValue("@itemcode", Request.QueryString["Item_code"].ToString());
                                                    insertmodifiercmd.Parameters.AddWithValue("@merchantcode", Request.QueryString["merchant"].ToString());
                                                    insertmodifiercmd.Parameters.AddWithValue("@usercode", user);
                                                    try
                                                    {
                                                        insertmodifiercmd.ExecuteNonQuery();
                                                    }
                                                    catch (Exception ex)
                                                    {

                                                    }
                                                }
                                            }

                                        }

                                        PLUConn.Close();


                                        using (SqlCommand PLUcmd = new SqlCommand("Update_PLU", PLUConn))
                                        {
                                            PLUcmd.CommandType = CommandType.StoredProcedure;
                                            PLUcmd.Parameters.AddWithValue("@ItemCode", txt_ItemCode.Text.Trim().Replace("'", "`"));
                                            PLUcmd.Parameters.AddWithValue("@Barcode", txt_Barcode.Text.Trim().Replace("'", "`"));
                                            PLUcmd.Parameters.AddWithValue("@Linkcode", txt_Subcode.Text.Trim().Replace("'", "`"));

                                            PLUConn.Open();

                                            try
                                            {
                                                PLUcmd.ExecuteNonQuery();
                                            }
                                            catch (Exception ex)
                                            {
                                                throw (ex);
                                            }
                                            finally
                                            {
                                                PLUcmd.Dispose();
                                                PLUConn.Close();
                                            }
                                        }

                                        int totalRows = grd_view.Rows.Count;
                                        for (int RowIndex = 0; RowIndex < totalRows; RowIndex++)
                                        {
                                            GridViewRow row = grd_view.Rows[RowIndex];
                                            Label lblagentlvl = row.FindControl("lblagentlvl") as Label;
                                            TextBox redempt = row.FindControl("txt_redempt") as TextBox;
                                            TextBox reward = row.FindControl("txt_reward") as TextBox;
                                            SqlConnection myConnection = new SqlConnection();
                                            PLUConn.Open();
                                            SqlCommand cmd = new SqlCommand("Update mf_agentcommi set earnrpwhenbuy='" + reward.Text + "',redemptionpoint='" + redempt.Text + "' where item_Code='" + Request.QueryString["item_code"] + "' and AgentLevelCode = '" + lblagentlvl.Text + "' and Deleteind <> 'X'", PLUConn);
                                            cmd.ExecuteNonQuery();
                                            PLUConn.Close();
                                        }
                                        ShowData();
                                    }
                                    Session["PName"] = "Item Listing";

                                    if (Request.QueryString["user"] != null)
                                    {
                                        ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Update success.');window.location ='ContentApproval.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "&user=" + Request.QueryString["user"].ToString() + "';", true);
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Update success.');window.location ='ContentApproval.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "';", true);
                                    }

                                    //ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Update success.');window.location ='ContentApproval.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "&user=" + Request.QueryString["user"].ToString() + "';", true);
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Update failed. Please try again later.');", true);
                                    txt_Barcode.Focus();
                                }
                            }

                        }
                    }
                    else
                    {

                        if (txt_Category.Text.Trim() == "")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Category code cannot be empty.');", true);
                            txt_Category.Focus();
                        }
                        //else if (!txtVlink.Text.EndsWith("?rel=0"))
                        //{
                        //    ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Video Link 1 is not a YouTube link.');", true);
                        //}
                        //else if (!txtVlink2.Text.EndsWith("?rel=0"))
                        //{
                        //    ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Video Link 2 is not a YouTube link.');", true);
                        //}
                        //else if (!txtVlink3.Text.EndsWith("?rel=0"))
                        //{
                        //    ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Video Link 3 is not a YouTube link.');", true);
                        //}
                        else if (txt_ItemCode.Text.Trim() == "")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Item code cannot be empty.');", true);
                            txt_ItemCode.Focus();
                        }
                        else if (txt_Barcode.Text.Trim() == "")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Barcode cannot be empty.');", true);
                            txt_Barcode.Focus();
                        }
                        else if (txt_Subcode.Text.Trim() == "")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Linkcode cannot be empty.');", true);
                            txt_Subcode.Focus();
                        }
                        else if (txt_Desc.Text.Trim() == "")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Long desc cannot be empty.');", true);
                            txt_Desc.Focus();
                        }
                        else if (txt_ShortDesc.Text.Trim() == "")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Short desc cannot be empty.');", true);
                            txt_ShortDesc.Focus();
                        }
                        else if (ddlUM.SelectedValue == "")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Please select UOM.');", true);
                            ddlUM.Focus();
                        }
                        else if (ddl_RdmItem.SelectedValue == "1" && txt_RdmPoint.Text.Trim() == "")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Redemption point cannot be empty.');", true);
                            txt_RdmPoint.Focus();
                        }
                        else if (CheckAmount(txt_RdmPoint.Text) == false)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Please enter a valid amount for redemption point.');", true);
                            txt_RdmPoint.Focus();
                        }
                        else
                        {
                            Int32 tv = 50;

                            using (SqlConnection myTxConn = new SqlConnection(DBCon))
                            {
                                using (SqlCommand myTxcmd = new SqlCommand("Update_Item2", myTxConn))
                                {
                                    myTxcmd.CommandType = CommandType.StoredProcedure;
                                    myTxcmd.Parameters.AddWithValue("@ItemCode", txt_ItemCode.Text.Trim().Replace("'", "`"));
                                    myTxcmd.Parameters.AddWithValue("@Barcode", txt_Barcode.Text.Trim().Replace("'", "`"));
                                    myTxcmd.Parameters.AddWithValue("@Linkcode", txt_Subcode.Text.Trim().Replace("'", "`"));
                                    myTxcmd.Parameters.AddWithValue("@Type", ddlType.SelectedValue);
                                    myTxcmd.Parameters.AddWithValue("@LongDesc", txt_Desc.Text.Trim().Replace("'", "`").Replace("\"","``"));
                                    myTxcmd.Parameters.AddWithValue("@LongDesc2", txt_Desc2.Text.Trim().Replace("'", "`").Replace("\"", "``"));
                                    myTxcmd.Parameters.AddWithValue("@ShortDesc", txt_ShortDesc.Text.Trim().Replace("'", "`"));
                                    myTxcmd.Parameters.AddWithValue("@OtherDesc", txt_OtherDesc.Text.Trim().Replace("'", "`"));
                                    myTxcmd.Parameters.AddWithValue("@CategoryCode", txt_Category.Text.Trim().Replace("'", "`"));
                                    myTxcmd.Parameters.AddWithValue("@Brand", ddlBrd.SelectedValue);
                                    myTxcmd.Parameters.AddWithValue("@PackSize", txt_Pack.Text.Trim().Replace("'", "`"));
                                    myTxcmd.Parameters.AddWithValue("@UOM", ddlUM.SelectedValue);
                                    myTxcmd.Parameters.AddWithValue("@PurchaseTax", ddlPT.SelectedValue);
                                    myTxcmd.Parameters.AddWithValue("@SalesTax", ddlST.SelectedValue);
                                    myTxcmd.Parameters.AddWithValue("@IsRedemption", ddl_RdmItem.SelectedValue);
                                    myTxcmd.Parameters.AddWithValue("@FOC", ddlfoc.SelectedValue);
                                    myTxcmd.Parameters.AddWithValue("@Self_Order_Menu", ddlordertable.SelectedValue);
                                    if (txt_RdmPoint.Text != "")
                                    {
                                        myTxcmd.Parameters.AddWithValue("@RedemptionPoint", Convert.ToDecimal(txt_RdmPoint.Text.Trim().Replace("'", "`")));
                                    }
                                    else
                                    {
                                        myTxcmd.Parameters.AddWithValue("@RedemptionPoint", 0);
                                    }
                                    //myTxcmd.Parameters.AddWithValue("@RedemptionPoint", Convert.ToDecimal(txt_RdmPoint.Text.Trim().Replace("'", "`")));
                                    if (lblRefProfitBonusPercent.Visible == false)
                                    {
                                        myTxcmd.Parameters.AddWithValue("@RefProfitBonusPercent", Convert.ToDecimal(txtRefProfitBonusPercent.Text.Trim().Replace("'", "`")));
                                    }
                                    else
                                    {
                                        myTxcmd.Parameters.AddWithValue("@RefProfitBonusPercent", 0);
                                    }
                                    if (txtTeamKPI.Text != "")
                                    {
                                        myTxcmd.Parameters.AddWithValue("@TeamKPI", Convert.ToDecimal(txtTeamKPI.Text.Trim().Replace("'", "`")));
                                    }
                                    else
                                    {
                                        myTxcmd.Parameters.AddWithValue("@TeamKPI", 0);
                                    }
                                    if (txtTeamKPI2.Text != "")
                                    {
                                        myTxcmd.Parameters.AddWithValue("@TeamKPI2", Convert.ToDecimal(txtTeamKPI2.Text.Trim().Replace("'", "`")));
                                    }
                                    else
                                    {
                                        myTxcmd.Parameters.AddWithValue("@TeamKPI2", 0);
                                    }
                                    if (txtMBD.Text != "")
                                    {
                                        myTxcmd.Parameters.AddWithValue("@MBD", Convert.ToDecimal(txtMBD.Text.Trim().Replace("'", "`")));
                                    }
                                    else
                                    {
                                        myTxcmd.Parameters.AddWithValue("@MBD", 0);
                                    }
                                    if (txtMEB.Text != "")
                                    {
                                        myTxcmd.Parameters.AddWithValue("@MEB", Convert.ToDecimal(txtMEB.Text.Trim().Replace("'", "`")));
                                    }
                                    else
                                    {
                                        myTxcmd.Parameters.AddWithValue("@MEB", 0);
                                    }
                                    if (txtteamgrowthinc1.Text != "")
                                    {
                                        myTxcmd.Parameters.AddWithValue("@teamgrowthinc1", Convert.ToDecimal(txtteamgrowthinc1.Text.Trim().Replace("'", "`")));
                                    }
                                    else
                                    {
                                        myTxcmd.Parameters.AddWithValue("@teamgrowthinc1", 0);
                                    }
                                    if (txtteamgrowthinc2.Text != "")
                                    {
                                        myTxcmd.Parameters.AddWithValue("@teamgrowthinc2", Convert.ToDecimal(txtteamgrowthinc2.Text.Trim().Replace("'", "`")));
                                    }
                                    else
                                    {
                                        myTxcmd.Parameters.AddWithValue("@teamgrowthinc2", 0);
                                    }
                                    if (txtESProfit.Text != "")
                                    {
                                        myTxcmd.Parameters.AddWithValue("@ESProfit", Convert.ToDecimal(txtESProfit.Text.Trim().Replace("'", "`")));
                                    }
                                    else
                                    {
                                        myTxcmd.Parameters.AddWithValue("@ESProfit", 0);
                                    }
                                    if (txtSPP.Text != "")
                                    {
                                        myTxcmd.Parameters.AddWithValue("@SPP", Convert.ToDecimal(txtSPP.Text.Trim().Replace("'", "`")));
                                    }
                                    else
                                    {
                                        myTxcmd.Parameters.AddWithValue("@SPP", 0);
                                    }
                                    //              myTxcmd.Parameters.AddWithValue("@TeamKPI", Convert.ToDecimal(txtTeamKPI.Text.Trim().Replace("'", "`")));
                                    //myTxcmd.Parameters.AddWithValue("@TeamKPI2", Convert.ToDecimal(txtTeamKPI2.Text.Trim().Replace("'", "`")));
                                    //                  myTxcmd.Parameters.AddWithValue("@MBD", Convert.ToDecimal(txtMBD.Text.Trim().Replace("'", "`")));
                                    //                  myTxcmd.Parameters.AddWithValue("@MEB", Convert.ToDecimal(txtMEB.Text.Trim().Replace("'", "`")));
                                    //                  myTxcmd.Parameters.AddWithValue("@ESProfit", Convert.ToDecimal(txtESProfit.Text.Trim().Replace("'", "`")));
                                    //                  myTxcmd.Parameters.AddWithValue("@SPP", Convert.ToDecimal(txtSPP.Text.Trim().Replace("'", "`")));

                                    if (txtvendorcost.Text != "")
                                    {
                                        myTxcmd.Parameters.AddWithValue("@cost", txtvendorcost.Text);
                                    }
                                    else
                                    {
                                        myTxcmd.Parameters.AddWithValue("@cost", "0");
                                    }

                                    if (txtmaxbuy.Text != "")
                                    {
                                        myTxcmd.Parameters.AddWithValue("@maxbuylimit", txtmaxbuy.Text);
                                    }
                                    else
                                    {
                                        myTxcmd.Parameters.AddWithValue("@maxbuylimit", "0");
                                    }

                                    //comm setting
                                    myTxcmd.Parameters.AddWithValue("@commtype", ddlcommtype.SelectedValue);
                                    myTxcmd.Parameters.AddWithValue("@commby", ddlcommby.SelectedValue);
                                    if(txtcommamt.Text == "")
                                    {
                                        myTxcmd.Parameters.AddWithValue("@commamt", "0");
                                    }
                                    else
                                    {
                                        myTxcmd.Parameters.AddWithValue("@commamt", txtcommamt.Text);
                                    }
                                    myTxcmd.Parameters.AddWithValue("@commgrp", ddlcommgroup.SelectedValue);

                                    if (txtweight.Text != "")
                                    {
                                        myTxcmd.Parameters.AddWithValue("@weight", Convert.ToDecimal(txtweight.Text.Trim().Replace("'", "`")));
                                    }
                                    else
                                    {
                                        myTxcmd.Parameters.AddWithValue("@weight", 0);
                                    }
                                    myTxcmd.Parameters.AddWithValue("@overview", txtEditor.Text);
                                    if (!string.IsNullOrEmpty(txtVlink.Text))
                                    {
                                        string videourl1 = "";
                                        string youtubeurl1 = "";
                                        if (txtVlink.Text.EndsWith("?rel=0"))
                                        {
                                            youtubeurl1 = txtVlink.Text.Trim();
                                            myTxcmd.Parameters.AddWithValue("@videolink1", youtubeurl1);
                                        }
                                        else if (txtVlink.Text.StartsWith("https://youtu.be/"))
                                        {
                                            videourl1 = txtVlink.Text.Trim().Substring(17);
                                            videourl1 = videourl1.Substring(0, 11);
                                            youtubeurl1 = "https://www.youtube.com/embed/" + videourl1 + "?rel=0";

                                            myTxcmd.Parameters.AddWithValue("@videolink1", youtubeurl1);
                                        }
                                        else if (txtVlink.Text.StartsWith("https://www.youtube.com/watch?"))
                                        {
                                            videourl1 = txtVlink.Text.Trim().Substring(32);
                                            videourl1 = videourl1.Substring(0, 11);
                                            youtubeurl1 = "https://www.youtube.com/embed/" + videourl1 + "?rel=0";

                                            myTxcmd.Parameters.AddWithValue("@videolink1", youtubeurl1);
                                        }
                                        else if (txtVlink.Text.StartsWith("https://www.youtube.com/shorts/"))
                                        {
                                            videourl1 = txtVlink.Text.Trim().Substring(31);
                                            videourl1 = videourl1.Substring(0, 11);
                                            youtubeurl1 = "https://www.youtube.com/embed/" + videourl1 + "?rel=0";

                                            myTxcmd.Parameters.AddWithValue("@videolink1", youtubeurl1);
                                        }
                                    }
                                    else
                                    {
                                        myTxcmd.Parameters.AddWithValue("@videolink1", txtVlink.Text);
                                    }

                                    if (!string.IsNullOrEmpty(txtVlink2.Text))
                                    {
                                        string videourl2 = "";
                                        string youtubeurl2 = "";
                                        if (txtVlink2.Text.EndsWith("?rel=0"))
                                        {
                                            youtubeurl2 = txtVlink2.Text.Trim();
                                            myTxcmd.Parameters.AddWithValue("@videolink2", youtubeurl2);
                                        }
                                        else if (txtVlink2.Text.StartsWith("https://youtu.be/"))
                                        {
                                            videourl2 = txtVlink2.Text.Trim().Substring(17);
                                            videourl2 = videourl2.Substring(0, 11);
                                            youtubeurl2 = "https://www.youtube.com/embed/" + videourl2 + "?rel=0";

                                            myTxcmd.Parameters.AddWithValue("@videolink2", youtubeurl2);
                                        }
                                        else if (txtVlink2.Text.StartsWith("https://www.youtube.com/watch?"))
                                        {
                                            videourl2 = txtVlink2.Text.Trim().Substring(32);
                                            videourl2 = videourl2.Substring(0, 11);
                                            youtubeurl2 = "https://www.youtube.com/embed/" + videourl2 + "?rel=0";

                                            myTxcmd.Parameters.AddWithValue("@videolink2", youtubeurl2);
                                        }
                                        else if (txtVlink2.Text.StartsWith("https://www.youtube.com/shorts/"))
                                        {
                                            videourl2 = txtVlink2.Text.Trim().Substring(31);
                                            videourl2 = videourl2.Substring(0, 11);
                                            youtubeurl2 = "https://www.youtube.com/embed/" + videourl2 + "?rel=0";

                                            myTxcmd.Parameters.AddWithValue("@videolink2", youtubeurl2);
                                        }
                                    }
                                    else
                                    {
                                        myTxcmd.Parameters.AddWithValue("@videolink2", txtVlink2.Text);
                                    }

                                    if (!string.IsNullOrEmpty(txtVlink3.Text))
                                    {
                                        string videourl3 = "";
                                        string youtubeurl3 = "";
                                        if (txtVlink3.Text.EndsWith("?rel=0"))
                                        {
                                            youtubeurl3 = txtVlink3.Text.Trim();
                                            myTxcmd.Parameters.AddWithValue("@videolink3", youtubeurl3);
                                        }
                                        else if (txtVlink3.Text.StartsWith("https://youtu.be/"))
                                        {
                                            videourl3 = txtVlink3.Text.Trim().Substring(17);
                                            videourl3 = videourl3.Substring(0, 11);
                                            youtubeurl3 = "https://www.youtube.com/embed/" + videourl3 + "?rel=0";

                                            myTxcmd.Parameters.AddWithValue("@videolink3", youtubeurl3);
                                        }
                                        else if (txtVlink3.Text.StartsWith("https://www.youtube.com/watch?"))
                                        {
                                            videourl3 = txtVlink3.Text.Trim().Substring(32);
                                            videourl3 = videourl3.Substring(0, 11);
                                            youtubeurl3 = "https://www.youtube.com/embed/" + videourl3 + "?rel=0";

                                            myTxcmd.Parameters.AddWithValue("@videolink3", youtubeurl3);
                                        }
                                        else if (txtVlink3.Text.StartsWith("https://www.youtube.com/shorts/"))
                                        {
                                            videourl3 = txtVlink3.Text.Trim().Substring(31);
                                            videourl3 = videourl3.Substring(0, 11);
                                            youtubeurl3 = "https://www.youtube.com/embed/" + videourl3 + "?rel=0";

                                            myTxcmd.Parameters.AddWithValue("@videolink3", youtubeurl3);
                                        }
                                    }
                                    else
                                    {
                                        myTxcmd.Parameters.AddWithValue("@videolink3", txtVlink3.Text);
                                    }

                                    myTxcmd.Parameters.AddWithValue("@weightitem", ddlweightitem.SelectedValue);
                                    myTxcmd.Parameters.AddWithValue("@weighttype", RDweighttype.SelectedValue);

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
                                    using (SqlConnection PLUConn = new SqlConnection(DBCon))
                                    {
                                        using (SqlCommand PLUcmd = new SqlCommand("Update_PLU", PLUConn))
                                        {
                                            PLUcmd.CommandType = CommandType.StoredProcedure;
                                            PLUcmd.Parameters.AddWithValue("@ItemCode", txt_ItemCode.Text.Trim().Replace("'", "`"));
                                            PLUcmd.Parameters.AddWithValue("@Barcode", txt_Barcode.Text.Trim().Replace("'", "`"));
                                            PLUcmd.Parameters.AddWithValue("@Linkcode", txt_Subcode.Text.Trim().Replace("'", "`"));

                                            PLUConn.Open();

                                            try
                                            {
                                                PLUcmd.ExecuteNonQuery();
                                            }
                                            catch (Exception ex)
                                            {
                                                throw (ex);
                                            }
                                            finally
                                            {
                                                PLUcmd.Dispose();
                                                PLUConn.Close();
                                            }
                                        }

                                        int totalRows = grd_view.Rows.Count;
                                        for (int RowIndex = 0; RowIndex < totalRows; RowIndex++)
                                        {
                                            GridViewRow row = grd_view.Rows[RowIndex];
                                            Label lblagentlvl = row.FindControl("lblagentlvl") as Label;
                                            TextBox redempt = row.FindControl("txt_redempt") as TextBox;
                                            TextBox reward = row.FindControl("txt_reward") as TextBox;
                                            SqlConnection myConnection = new SqlConnection();
                                            PLUConn.Open();
                                            SqlCommand cmd = new SqlCommand("Update mf_agentcommi set earnrpwhenbuy='" + reward.Text + "',redemptionpoint='" + redempt.Text + "' where item_Code='" + Request.QueryString["item_code"] + "' and AgentLevelCode = '" + lblagentlvl.Text + "' and Deleteind <> 'X'", PLUConn);
                                            cmd.ExecuteNonQuery();
                                            PLUConn.Close();
                                        }
                                        ShowData();
                                    }
                                    Session["PName"] = "Item Listing";


                                    if (Request.QueryString["user"] != null)
                                    {
                                        ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Update success.');window.location ='ContentApproval.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "&user=" + Request.QueryString["user"].ToString() + "';", true);
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Update success.');window.location ='ContentApproval.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "';", true);
                                    }
                                    //ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Update success.');window.location ='ContentApproval.aspx?merchant=" + Request.QueryString["merchant"].ToString()  + "&user=" + Request.QueryString["user"].ToString() + "';", true);
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Update failed. Please try again later.');", true);
                                    txt_Barcode.Focus();
                                }
                            }

                        }
                    }
                }
            }
        }
    }

    //protected void View_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
    //{
    //    //View.PageIndex = e.NewPageIndex;
    //    LoadImg();
    //}

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
    protected void txt_ItemCode_TextChanged(object sender, EventArgs e)
    {
        txt_Barcode.Text = txt_ItemCode.Text;
        if (ddlType.SelectedValue == "1")
        {
            txt_Subcode.Text = txt_ItemCode.Text;
        }
    }

    protected void ddlDept_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCat();
    }

    protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        txt_Subcode.Text = ddlItem.SelectedValue;
    }

    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlType.SelectedValue == "1")
        {
            txt_Pack.Text = "1.000";
            txt_Pack.Enabled = false;
            ddlItem.Enabled = false;

            txt_Subcode.Text = txt_ItemCode.Text;

            LinkPanel.Visible = false;
        }
        else
        {
            txt_Pack.Enabled = true;
            ddlItem.Enabled = true;

            txt_Subcode.Text = "";
            ddlItem.SelectedValue = "";

            LinkPanel.Visible = true;
        }
    }


    protected void ddl_RdmItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_RdmItem.SelectedValue == "1")
        {
            txt_RdmPoint.Enabled = true;
            txtrewardpoint.Enabled = true;
            txt_RdmPoint.Focus();
        }
        else
        {
            txt_RdmPoint.Enabled = true;
            txtrewardpoint.Enabled = true;
            //txt_RdmPoint.Text = "0";
        }
    }

  

    protected void txt_Filter_TextChanged(object sender, EventArgs e)
    {
        LoadCat();
    }

    protected void txt_Filter2_TextChanged(object sender, EventArgs e)
    {
        LoadLinkItem();
    }
    protected void View_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }

    protected void View_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='#cccccc'");
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c");

        }
    }

    protected void CardItem_DataBound2(object sender, RepeaterItemEventArgs e)
    {

        CheckBox Check = (CheckBox)e.Item.FindControl("CheckBox2");
        CheckBox Check2 = (CheckBox)e.Item.FindControl("CheckBox3");
        Label btn_badge = (Label)e.Item.FindControl("btn_badge");
        Label btn_badge2 = (Label)e.Item.FindControl("btn_badge2");
        Label LabelRecord = (Label)e.Item.FindControl("LabelRecord");
        LinkButton addlistnoitem2 = (LinkButton)e.Item.FindControl("addlistnoitem2");
        ImageButton Image1 = (ImageButton)e.Item.FindControl("Image1");
        ImageButton Image2 = (ImageButton)e.Item.FindControl("Image2");
        ImageButton Image = (ImageButton)e.Item.FindControl("Image3");
        LinkButton UploadOn = (LinkButton)e.Item.FindControl("UploadOn");
        LinkButton UploadOff = (LinkButton)e.Item.FindControl("UploadOff");
        LinkButton DeleteOn = (LinkButton)e.Item.FindControl("DeleteOn");
        LinkButton DeleteOff = (LinkButton)e.Item.FindControl("DeleteOff");
        LinkButton PublishOn = (LinkButton)e.Item.FindControl("PublishOn");
        LinkButton PublishOff = (LinkButton)e.Item.FindControl("PublishOff");

        if (rpt_Item2.Items.Count < 1)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                HtmlGenericControl dvNoRec = e.Item.FindControl("dvNoRecords") as HtmlGenericControl;

                if (dvNoRec != null)
                {
                    dvNoRec.Visible = true;
                }
            }
        }

        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {

            DataRowView drv = (DataRowView)(e.Item.DataItem);

            if (String.IsNullOrEmpty(drv.Row["FilePath"].ToString()) == true)
            {
                Image1.ImageUrl = "Images/NoPic.png";
            }
            else
            {
                
                Image1.ImageUrl = drv.Row["FilePath"].ToString();
            }

            Check.Checked = Convert.ToBoolean(drv.Row["Main_Indicator"].ToString());
            if (drv.Row["Main_Indicator"].ToString() == "True")
            {
                PublishOn.Visible = false;
                PublishOff.Visible = false;


            }
            else
            {
                PublishOff.Visible = false;
                PublishOn.Visible = false;
            }

            if (drv.Row["Default_Indicator"].ToString() == "1")
            {
                UploadOn.Visible = true;
                DeleteOff.Visible = true;
            }
            else if (drv.Row["Default_Indicator"].ToString() == "0")
            {
                UploadOff.Visible = true;
                DeleteOff.Visible = false;
                DeleteOn.Visible = true;
            }



        }

    }

    protected void Item_Details_Command2(object sender, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Delete")
        {
            Int32 tv = 50;
            using (SqlConnection myTxConn = new SqlConnection(DBCon))
            {
                using (SqlCommand myTxcmd = new SqlCommand("Delete_Img", myTxConn))
                {
                    myTxcmd.CommandType = CommandType.StoredProcedure;
                    myTxcmd.Parameters.AddWithValue("@ID", Convert.ToInt32(e.CommandArgument.ToString()));

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
                    ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Delete success.');", true);
                    LoadImgPath(1);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Delete failed. Please try again later.');", true);
                }

            }
            LoadImgPath(1);
        }
        if (e.CommandName == "Update1")
        {
            LinkButton PublishOn = (LinkButton)e.Item.FindControl("PublishOn");
            LinkButton PublishOff = (LinkButton)e.Item.FindControl("PublishOff");

            Int32 tv = 50;
            using (SqlConnection myTxConn = new SqlConnection(DBCon))
            {
                using (SqlCommand myTxcmd = new SqlCommand("Edit_Img_Indicator", myTxConn))
                {
                    myTxcmd.CommandType = CommandType.StoredProcedure;
                    myTxcmd.Parameters.AddWithValue("@ID", Convert.ToInt32(e.CommandArgument.ToString()));
                    myTxcmd.Parameters.AddWithValue("@Item_Code", txt_ItemCode.Text);

                    myTxcmd.Parameters.AddWithValue("@Main_Indicator", "True");

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
                        myTxConn.Dispose();
                        myTxConn.Close();
                    }
                    tv = Convert.ToInt32(TReturnValue.Value.ToString());
                }
                if (tv == 100)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Publish success.');", true);
                    PublishOn.Visible = false;
                    PublishOff.Visible = false;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Publish failed. Please try again later.');", true);
                }

            }



        }

        if (e.CommandName == "Update2")
        {
            LinkButton PublishOn = (LinkButton)e.Item.FindControl("PublishOn");
            LinkButton PublishOff = (LinkButton)e.Item.FindControl("PublishOff");

            Int32 tv = 50;
            using (SqlConnection myTxConn = new SqlConnection(DBCon))
            {
                using (SqlCommand myTxcmd = new SqlCommand("Edit_Img_Indicator", myTxConn))
                {
                    myTxcmd.CommandType = CommandType.StoredProcedure;
                    myTxcmd.Parameters.AddWithValue("@ID", Convert.ToInt32(e.CommandArgument.ToString()));
                    myTxcmd.Parameters.AddWithValue("@Item_Code", txt_ItemCode.Text);

                    myTxcmd.Parameters.AddWithValue("@Main_Indicator", "False");

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
                        myTxConn.Dispose();
                        myTxConn.Close();
                    }
                    tv = Convert.ToInt32(TReturnValue.Value.ToString());
                }
                if (tv == 100)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('UnPublish success.');", true);
                    PublishOn.Visible = false;
                    PublishOff.Visible = false;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('UnPublish failed. Please try again later.');", true);
                }

            }



        }
        if (e.CommandName == "Update")
        {
            CheckBox Check = (CheckBox)e.Item.FindControl("CheckBox2");
            CheckBox Check2 = (CheckBox)e.Item.FindControl("CheckBox3");
            ImageButton Image = (ImageButton)e.Item.FindControl("ImageButton1");
            ImageButton Image2 = (ImageButton)e.Item.FindControl("Image2");
            ImageButton Image3 = (ImageButton)e.Item.FindControl("ImageButton1");
            ImageButton Image4 = (ImageButton)e.Item.FindControl("ImageButton2");
            ImageButton Image5 = (ImageButton)e.Item.FindControl("ImageButton3");
            Label btn_badge = (Label)e.Item.FindControl("btn_badge");
            Label btn_badge2 = (Label)e.Item.FindControl("btn_badge2");

            Int32 tv = 50;
            using (SqlConnection myTxConn = new SqlConnection(DBCon))
            {
                using (SqlCommand myTxcmd = new SqlCommand("Edit_Img_Indicator", myTxConn))
                {
                    myTxcmd.CommandType = CommandType.StoredProcedure;
                    myTxcmd.Parameters.AddWithValue("@ID", Convert.ToInt32(e.CommandArgument.ToString()));
                    myTxcmd.Parameters.AddWithValue("@Item_Code", txt_ItemCode.Text);


                    myTxcmd.Parameters.AddWithValue("@Main_Indicator", "True");




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
                        myTxConn.Dispose();
                        myTxConn.Close();
                    }
                    tv = Convert.ToInt32(TReturnValue.Value.ToString());
                }
                if (tv == 100)
                {
                    
                    ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Insert success.');", true);
                    if (Check.Checked == true)
                    {
                        btn_badge.Visible = true;
                        btn_badge2.Visible = false;
                    }
                    else
                    {
                        btn_badge.Visible = false;
                        btn_badge2.Visible = true;
                    }

                    Image2.Visible = true;
                    Image3.Visible = false;
                    Image4.Visible = true;
                    Image5.Visible = false;
                    Check2.Visible = false;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Insert failed. Please try again later.');", true);
                }
            }

        }
        if (e.CommandName == "Default")
        {

            Int32 tv = 50;
            using (SqlConnection myTxConn = new SqlConnection(DBCon))
            {
                using (SqlCommand myTxcmd = new SqlCommand("Item_Default_Img_Path_miso", myTxConn))
                {
                    myTxcmd.CommandType = CommandType.StoredProcedure;
                    myTxcmd.Parameters.AddWithValue("@ID", Convert.ToInt32(e.CommandArgument.ToString()));
                    myTxcmd.Parameters.AddWithValue("@Item_Code", txt_ItemCode.Text);



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
                        myTxConn.Dispose();
                        myTxConn.Close();
                    }
                    tv = Convert.ToInt32(TReturnValue.Value.ToString());
                }
                if (tv == 100)
                {
                    using (SqlConnection myQRConn = new SqlConnection(DBCon))
                    {
                        using (SqlCommand QRcmd = new SqlCommand("Update MF_Item set Thumbnail_FilePath= (select Thumbnail_FilePath from MF_Multi_Img_miso where ID=@ID and Item_Code=@Item_Code and DeleteInd <> 'X'), FilePath = (select FilePath from MF_Multi_Img_miso where ID=@ID and Item_Code=@Item_Code and DeleteInd <> 'X'),Modified_DT=DATEADD(HOUR, 8, CONVERT(varchar(20),GETUTCDATE(),120)) where DeleteInd<>'X' and Item_Code='" + txt_ItemCode.Text + "'", myQRConn))
                        {
                            QRcmd.Parameters.AddWithValue("@ID", Convert.ToInt32(e.CommandArgument.ToString()));
                            QRcmd.Parameters.AddWithValue("@Item_Code", txt_ItemCode.Text);

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


                    if (Request.QueryString["user"] != null)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Set Image Success!');window.location ='AddItem.aspx?item_code=" + TextBox3.Text + "&merchant=" + Request.QueryString["merchant"].ToString() + "&user=" + Request.QueryString["user"].ToString() + "';", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Set Image Success!');window.location ='AddItem.aspx?item_code=" + TextBox3.Text + "&merchant=" + Request.QueryString["merchant"].ToString() + "';", true);
                    }
                    //ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Set Image Success!');window.location ='AddItem.aspx?item_code=" + TextBox3.Text + "&merchant=" + Request.QueryString["merchant"].ToString() + "&user=" + Request.QueryString["user"].ToString() + "';", true);
                    LoadItem();

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Set Image Failed. Please try again later.');", true);
                }
            }
        }
    }


    protected void CardItem_DataBound(object sender, RepeaterItemEventArgs e)
    {

        CheckBox Check = (CheckBox)e.Item.FindControl("CheckBox2");
        CheckBox Check2 = (CheckBox)e.Item.FindControl("CheckBox3");
        Label btn_badge = (Label)e.Item.FindControl("btn_badge");
        Label btn_badge2 = (Label)e.Item.FindControl("btn_badge2");
        Label LabelRecord = (Label)e.Item.FindControl("LabelRecord");
        LinkButton addlistnoitem2 = (LinkButton)e.Item.FindControl("addlistnoitem2");
        ImageButton WebImage1 = (ImageButton)e.Item.FindControl("WebImage");
        ImageButton Image2 = (ImageButton)e.Item.FindControl("Image2");
        ImageButton Image = (ImageButton)e.Item.FindControl("Image3");
        LinkButton UploadOn = (LinkButton)e.Item.FindControl("UploadOn");
        LinkButton UploadOff = (LinkButton)e.Item.FindControl("UploadOff");
        LinkButton DeleteOn = (LinkButton)e.Item.FindControl("DeleteOn");
        LinkButton DeleteOff = (LinkButton)e.Item.FindControl("DeleteOff");
        LinkButton PublishOn = (LinkButton)e.Item.FindControl("PublishOn");
        LinkButton PublishOff = (LinkButton)e.Item.FindControl("PublishOff");

        //if (rpt_Item.Items.Count < 1)
        //{
        //    if (e.Item.ItemType == ListItemType.Footer)
        //    {
        //        HtmlGenericControl dvNoRec = e.Item.FindControl("dvNoRecords") as HtmlGenericControl;

        //        if (dvNoRec != null)
        //        {
        //            dvNoRec.Visible = true;
        //        }
        //    }
        //}

        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {

            DataRowView drv = (DataRowView)(e.Item.DataItem);

            if (String.IsNullOrEmpty(drv.Row["FilePath"].ToString()) == true)
            {
                WebImage1.ImageUrl = "Images/NoPic.png";
            }
            else
            {
                //@"https://kcgold.azurewebsites.net/EzyShareListing-KCGoldsmith/" +
                WebImage1.ImageUrl = drv.Row["FilePath"].ToString();
            }

            if (drv.Row["Default_Indicator"].ToString() == "1")
            {
                //addlistnoitem2.Visible = true;
                UploadOn.Visible = true;
                DeleteOff.Visible = true;
            }
            else if (drv.Row["Default_Indicator"].ToString() == "0")
            {
                //addlistnoitem2.Visible = false;
                UploadOff.Visible = true;
                DeleteOff.Visible = false;
                DeleteOn.Visible = true;
            }

        }

    }

    #region LoadMultiImg
    protected void LoadImgPath(int pageIndex)
    {
        using (SqlConnection con = new SqlConnection(DBCon))
        {
            using (SqlCommand cmd = new SqlCommand("Img_Listing_Path_miso", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PageIndex", pageIndex);
                cmd.Parameters.AddWithValue("@PageSize", int.Parse("6"));

                cmd.Parameters.AddWithValue("@ItemCode", TextBox3.Text);

                cmd.Parameters.Add("@RecordCount", SqlDbType.Int, 4);
                cmd.Parameters["@RecordCount"].Direction = ParameterDirection.Output;
                con.Open();
                SqlDataReader idr = cmd.ExecuteReader();

                if (idr.HasRows == false)
                {
                    DataTable v = new DataTable();
                    v.Load(idr);

                    //ImageButton1.Visible = true;


                    rpt_Item2.DataSource = v;
                    rpt_Item2.DataBind();
                }
                else
                {
                    DataTable v = new DataTable();
                    v.Load(idr);

                    rpt_Item2.DataSource = v;
                    rpt_Item2.DataBind();
                }
                idr.Close();
                con.Close();

                int recordCount = Convert.ToInt32(cmd.Parameters["@RecordCount"].Value);
                Label1.Text = recordCount.ToString();

                if (recordCount == 0)
                {
                    recordCount = 1;
                }

                this.PopulatePager2(recordCount, pageIndex);
            }

        }
    }
    //protected void LoadImg(int pageIndex)
    //{

    //    using (SqlConnection con = new SqlConnection(DBCon))
    //    {
    //        using (SqlCommand cmd = new SqlCommand("Img_Listing_miso", con))
    //        {
    //            cmd.CommandType = CommandType.StoredProcedure;
    //            cmd.Parameters.AddWithValue("@PageIndex", pageIndex);
    //            cmd.Parameters.AddWithValue("@PageSize", int.Parse("6"));

    //            cmd.Parameters.AddWithValue("@ItemCode", TextBox3.Text);



    //            cmd.Parameters.Add("@RecordCount", SqlDbType.Int, 4);
    //            cmd.Parameters["@RecordCount"].Direction = ParameterDirection.Output;
    //            con.Open();
    //            SqlDataReader idr = cmd.ExecuteReader();

    //            if (idr.HasRows == false)
    //            {
    //                DataTable v = new DataTable();
    //                v.Load(idr);

    //                //ImageButton1.Visible = true;


    //                rpt_Item.DataSource = v;
    //                rpt_Item.DataBind();
    //            }
    //            else
    //            {
    //                DataTable v = new DataTable();
    //                v.Load(idr);

    //                rpt_Item.DataSource = v;
    //                rpt_Item.DataBind();
    //            }

    //            idr.Close();
    //            con.Close();

    //            int recordCount = Convert.ToInt32(cmd.Parameters["@RecordCount"].Value);

    //            lbl_Record2.Text = recordCount.ToString();

    //            if (recordCount == 0)
    //            {
    //                recordCount = 1;
    //            }

    //            this.PopulatePager(recordCount, pageIndex);
    //        }


    //    }

    //}

    //private void PopulatePager(int recordCount, int currentPage)
    //{
    //    ddlPager.Items.Clear();

    //    double dblPageCount = (double)((decimal)recordCount / decimal.Parse("6"));
    //    int pageCount = (int)Math.Ceiling(dblPageCount);

    //    if (pageCount > 0)
    //    {
    //        for (int i = 1; i <= pageCount; i++)
    //        {
    //            ddlPager.Items.Add(new ListItem("Page " + i.ToString(), i.ToString()));
    //        }
    //    }

    //    if (ddlPager.Items.FindByValue(currentPage.ToString()) != null)
    //    {
    //        ddlPager.SelectedValue = currentPage.ToString();
    //    }
    //    else
    //    {
    //        ddlPager.SelectedValue = "1";
    //        LoadImg(1);
    //    }
    //}
    //protected void ddlPager_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    this.LoadImg(Convert.ToInt32(ddlPager.SelectedValue));
    //}

    private void PopulatePager2(int recordCount, int currentPage)
    {
        ddlPager2.Items.Clear();

        double dblPageCount = (double)((decimal)recordCount / decimal.Parse("6"));
        int pageCount = (int)Math.Ceiling(dblPageCount);

        if (pageCount > 0)
        {
            for (int i = 1; i <= pageCount; i++)
            {
                ddlPager2.Items.Add(new ListItem("Page " + i.ToString(), i.ToString()));
            }
        }

        if (ddlPager2.Items.FindByValue(currentPage.ToString()) != null)
        {
            ddlPager2.SelectedValue = currentPage.ToString();
        }
        else
        {
            ddlPager2.SelectedValue = "1";
            LoadImgPath(1);
        }
    }
    protected void ddlPager2_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.LoadImgPath(Convert.ToInt32(ddlPager2.SelectedValue));
    }

 

    #endregion

    protected void LoadSTax()
    {
        using (SqlConnection mySConn = new SqlConnection(DBCon))
        {
            mySConn.Open();

            try
            {          // and CDefault <> 'False'
                using (SqlDataAdapter adp = new SqlDataAdapter("SELECT TaxCode, TaxCode + ' -- ' + TaxDesc + ' -- ' + CAST(Rate as varchar(25)) + '%' as rst FROM MF_SalesTax WHERE DeleteInd <> 'X'  ORDER BY CDefault DESC", mySConn))
                {
                    ddlST.Items.Clear();
                    ddlST.Items.Add(new ListItem("- Select Sales Tax -", ""));
                    DataTable dtST = new DataTable();
                    adp.Fill(dtST);
                    ddlST.DataSource = dtST;
                    ddlST.DataTextField = "rst";
                    ddlST.DataValueField = "TaxCode";
                    ddlST.DataBind();
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

    protected void LoadPTax()
    {

        using (SqlConnection mySConn = new SqlConnection(DBCon))
        {
            mySConn.Open();

            try
            {
                using (SqlDataAdapter adp = new SqlDataAdapter("SELECT *,TaxCode, TaxCode + ' -- ' + TaxDesc + ' -- ' + CAST(Rate as varchar(25)) + '%' as rst FROM MF_PurchaseTax WHERE DeleteInd <> 'X'  ORDER BY CDefault DESC", mySConn))
                {
                    ddlPT.Items.Clear();
                    ddlPT.Items.Add(new ListItem("- Select Purchase Tax -", ""));
                    DataTable dtPT = new DataTable();
                    adp.Fill(dtPT);
                    ddlPT.DataSource = dtPT;
                    ddlPT.DataTextField = "rst";
                    ddlPT.DataValueField = "TaxCode";
                    ddlPT.DataBind();
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

    protected void LoadUOM()
    {
        using (SqlConnection mySConn = new SqlConnection(DBCon))
        {
            mySConn.Open();

            try
            {
                using (SqlDataAdapter adp = new SqlDataAdapter("SELECT * FROM MF_UOM WHERE DeleteInd <> 'X' ORDER BY CDefault DESC", mySConn))
                {
                    ddlUM.Items.Clear();
                    ddlUM.Items.Add(new ListItem("- Select UOM -", ""));
                    DataTable dtUOM = new DataTable();
                    adp.Fill(dtUOM);
                    ddlUM.DataSource = dtUOM;
                    ddlUM.DataTextField = "UOM";
                    ddlUM.DataValueField = "UOM";
                    ddlUM.DataBind();
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

    protected void LoadDept()
    {
        using (SqlConnection mySConn = new SqlConnection(DBCon))
        {
            mySConn.Open();

            try
            {
                using (SqlDataAdapter adp = new SqlDataAdapter("SELECT Department_Code, Department_Code + ' -- ' + Department_Description as rst FROM MF_Department_miso WHERE supplier_code = '"+ Request.QueryString["merchant"].ToString() +"' and DeleteInd <> 'X' ORDER BY Department_Code ASC", mySConn))
                {
                    ddlDept.Items.Clear();
                    ddlDept.Items.Add(new ListItem("- All Department -", ""));
                    DataTable dtDept = new DataTable();
                    adp.Fill(dtDept);
                    if (dtDept.Rows.Count > 0)
                    {
                        ddlDept.DataSource = dtDept;
                        ddlDept.DataTextField = "rst";
                        ddlDept.DataValueField = "Department_Code";
                        ddlDept.DataBind();
                    }
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
    protected void LoadCat()
    {
        using (SqlConnection mySConn = new SqlConnection(DBCon))
        {
            mySConn.Open();

            
                if (ddlDept.SelectedValue == "")
                {
                    using (SqlDataAdapter adp = new SqlDataAdapter("SELECT Category_Code, Category_Code + ' -- ' + Category_Description as rst FROM MF_Category_miso WHERE supplier_code = '"+ Request.QueryString["merchant"] +"' and DeleteInd <> 'X' and (Category_Code like '%" + txt_Filter.Text + "%' or Category_Description like '%" + txt_Filter.Text + "%') ORDER BY Category_Code ASC", mySConn))
                    {
                    ddlCat.Items.Clear();
                    ddlCat.Items.Add(new ListItem("- Select Category -", ""));
                        DataTable dtCat = new DataTable();
                        adp.Fill(dtCat);
                        if (dtCat.Rows.Count > 0)
                        {
                            ddlCat.DataSource = dtCat;
                            ddlCat.DataTextField = "rst";
                            ddlCat.DataValueField = "Category_Code";
                            ddlCat.DataBind();
                        }
                    }
                }
                else
                {
                    using (SqlDataAdapter adp = new SqlDataAdapter("SELECT Category_Code, Category_Code + ' -- ' + Category_Description as rst FROM MF_Category_miso WHERE supplier_code = '" + Request.QueryString["merchant"] + "' and DeleteInd <> 'X' and Department_code='" + ddlDept.SelectedValue + "' and (Category_Code like '%" + txt_Filter.Text + "%' or Category_Description like '%" + txt_Filter.Text + "%') ORDER BY Category_Code ASC", mySConn))
                    {
                        ddlCat.Items.Clear();
                        ddlCat.Items.Add(new ListItem("- Select Category -", ""));
                        DataTable dtCat = new DataTable();
                        adp.Fill(dtCat);
                        if (dtCat.Rows.Count > 0)
                        {
                            ddlCat.DataSource = dtCat;
                        ddlCat.DataTextField = "rst";
                        ddlCat.DataValueField = "Category_Code";
                        ddlCat.DataBind();
                        }
                    }
                }
            
                mySConn.Close();
        }
    }

    protected void LoadBrd()
    {
        using (SqlConnection mySConn = new SqlConnection(DBCon))
        {
            mySConn.Open();

            using (SqlDataAdapter adp = new SqlDataAdapter("SELECT Brand_Code, Brand_Code + ' -- ' + Brand_Desc as Brd FROM MF_Brand WHERE supplier_code = '"+ Request.QueryString["merchant"].ToString() +"' and DeleteInd <> 'X' ORDER BY Brand_Code ASC", mySConn))
            {
                ddlBrd.Items.Clear();
                ddlBrd.Items.Add(new ListItem("- Select Brand -", ""));
                DataTable dtBrd = new DataTable();
                adp.Fill(dtBrd);
                if (dtBrd.Rows.Count > 0)
                {
                    ddlBrd.DataSource = dtBrd;
                    ddlBrd.DataTextField = "Brd";
                    ddlBrd.DataValueField = "Brand_Code";
                    ddlBrd.DataBind();
                }
                else
                {

                }
            }
            mySConn.Close();

        }
    }

    protected void Loadcommgrp()
    {
        using (SqlConnection mySConn = new SqlConnection(DBCon))
        {
            mySConn.Open();

            using (SqlDataAdapter adp = new SqlDataAdapter("SELECT  CommGroup FROM MF_Comm_Group where MerchantID = '"+ Request.QueryString["merchant"] +"' group by CommGroup ORDER BY CommGroup ASC", mySConn))
            {
                ddlcommgroup.Items.Clear();
                ddlcommgroup.Items.Add(new ListItem("- Select Comm Group -", ""));
                DataTable dtcommgrp = new DataTable();
                adp.Fill(dtcommgrp);
                if (dtcommgrp.Rows.Count > 0)
                {
                    ddlcommgroup.DataSource = dtcommgrp;
                    ddlcommgroup.DataTextField = "CommGroup";
                    ddlcommgroup.DataValueField = "CommGroup";
                    ddlcommgroup.DataBind();
                }
            }
            mySConn.Close();

        }
    }

    protected void LoadLinkItem()
    {
       /*sing (SqlConnection mySConn = new SqlConnection(DBCon))
        {
            mySConn.Open();

            try
            {
                using (SqlDataAdapter adp = new SqlDataAdapter("SELECT Item_Code, Item_Code + ' -- ' + LongDesc as rst FROM MF_Item WHERE DeleteInd <> 'X' and Type='1' and (Item_Code like '%" + txt_Filter2.Text.Replace("'", "`") + "%' or Barcode like '%" + txt_Filter2.Text.Replace("'", "`") + "%' or LongDesc like '%" + txt_Filter2.Text.Replace("'", "`") + "%') ORDER BY Item_Code ASC", mySConn))
                {
                    ddlItem.Items.Clear();
                    ddlItem.Items.Add(new ListItem("- Select Item -", ""));
                    DataTable dtItem = new DataTable();
                    adp.Fill(dtItem);
                    ddlItem.DataSource = dtItem;
                    ddlItem.DataTextField = "rst";
                    ddlItem.DataValueField = "Item_Code";
                    ddlItem.DataBind();
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
        }*/
    }


    protected void gvLoad_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }

    protected bool CheckAmount(String amt)
    {
        try
        {
            Double aa = Double.Parse(amt);
            return true;
        }
        catch
        {
            return false;
        }
    }




    //public static System.Drawing.Image ScaleImage(System.Drawing.Image image, int maxHeight)
    //{
    //    var ratio = 1.00;

    //    if (image.Height > image.Width)
    //    {
    //        ratio = (double)maxHeight / image.Height;
    //    }
    //    else
    //    {
    //        ratio = (double)maxHeight / image.Width;

    //    }

    //    var newWidth = (int)(image.Width * ratio);
    //    var newHeight = (int)(image.Height * ratio);
    //    var newImage = new Bitmap(newWidth, newHeight);
    //    using (var g = Graphics.FromImage(newImage))
    //    {
    //        g.DrawImage(image, 0, 0, newWidth, newHeight);
    //        g.SmoothingMode = SmoothingMode.HighQuality;
    //        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
    //        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
    //        g.CompositingQuality = CompositingQuality.HighQuality;


    //    }
    //    return newImage;
    //}

    protected void Description_Changed(object sender, EventArgs e)
    {
        string input = txt_Desc.Text;
        bool isValid = !Regex.IsMatch(input, @"[:;'!_?<>&%]");

        if (isValid)
        {
            // The input does not contain disallowed characters.
            // Perform your desired action here.
        }
        else
        {
            // The input contains disallowed characters.
            // Handle the invalid input accordingly.
        }

        if (String.IsNullOrEmpty(Request.QueryString["Item_Code"]))
        {
            if (txt_Desc.Text.Length <= 20)
            {

                txt_ShortDesc.Text = txt_Desc.Text;
            }
            else
            {
                txt_ShortDesc.Text = txt_Desc.Text.Substring(0, 40);
            }
        }

    }


    protected void ddlCat_SelectedIndexChanged(object sender, EventArgs e)
    {
        txt_Category.Text = ddlCat.SelectedValue;

        if (Request.QueryString["item_code"].ToString() =="")
        {
            using (SqlConnection mySConn = new SqlConnection(DBCon))
            {
                mySConn.Open();
                SqlCommand cmd = new SqlCommand("SELECT itemcode_runno as abc from itemcode_runno where cate_code = '" + txt_Category.Text + "' and Supplier_Code = '" + Request.QueryString["merchant"] + "'", mySConn);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        //if (reader["abc"].ToString() == "" )
                        //{
                        //    txt_ItemCode.Text = txt_Category.Text.Substring(13, 5) + "-1000001";
                        //    txt_Barcode.Text = txt_ItemCode.Text;
                        //    if (ddlType.SelectedValue == "1")
                        //    {
                        //        txt_Subcode.Text = txt_ItemCode.Text;
                        //    }
                        //}
                        //else
                        //{
                        //string idcount = (reader["abc"]).ToString();

                        count = Convert.ToInt32((reader["abc"]).ToString());
                        count += 1;
                        // txt_Category.Text = String.Empty;
                        txt_ItemCode.Text = String.Empty;
                        //txt_Barcode.Text = String.Empty;
                        txt_Subcode.Text = String.Empty;
                        txt_ItemCode.Text += txt_Category.Text + "-" + count.ToString();
                        //txt_Barcode.Text = txt_ItemCode.Text;
                        if (ddlType.SelectedValue == "1")
                        {
                            txt_Subcode.Text = txt_ItemCode.Text;
                        }

                        reader.Close();
                        reader.Dispose();

                    }
                }
                else
                {
                    txt_ItemCode.Text = txt_Category.Text + "-1000001";
                    //txt_Barcode.Text = txt_ItemCode.Text;
                    if (ddlType.SelectedValue == "1")
                    {
                        txt_Subcode.Text = txt_ItemCode.Text;
                    }
                }
                mySConn.Close();
            }
        }

    }


    protected void loadmodifier()
    {
        using (SqlConnection con = new SqlConnection(DBCon))
        {
            using (SqlCommand cmd = new SqlCommand("select modifier_id,modifier_grp_name,case when right(rtrim((select Option_Name + ',' from MF_Modifier_Option where MF_Modifier_Option.Modifier_ID=MF_Modifier_Group.Modifier_ID " +
                "and MF_Modifier_Option.Deleteind <> 'X' FOR XML PATH(''))),1) = ',' then substring(rtrim((select Option_Name +',' from MF_Modifier_Option where MF_Modifier_Option.Modifier_ID=MF_Modifier_Group.Modifier_ID " +
                "	and MF_Modifier_Option.Deleteind <> 'X' FOR XML PATH(''))),1,len(rtrim((select Option_Name +',' from MF_Modifier_Option where MF_Modifier_Option.Modifier_ID=MF_Modifier_Group.Modifier_ID " +
                "and MF_Modifier_Option.Deleteind <> 'X' FOR XML PATH(''))))-1) else (select Option_Name + '(RM'+ Cast(Option_Price as varchar(255)) +')' + ',' from MF_Modifier_Option where MF_Modifier_Option.Modifier_ID=MF_Modifier_Group.Modifier_ID " +
                "	and MF_Modifier_Option.Deleteind <> 'X' FOR XML PATH('')) END AS modifieroption from MF_Modifier_Group where deleteind <> 'X' and merchant_code='"+ Request.QueryString["merchant"] +"'", con))
            {
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter idr = new SqlDataAdapter(cmd);
                DataTable v = new DataTable();
                idr.Fill(v);

                if (v.Rows.Count > 0)
                {
                    grd_viewmodifer.DataSource = v;
                    grd_viewmodifer.DataBind();
                }


            }
        }
    }

    protected void loadprinter()
    {
        using (SqlConnection con = new SqlConnection(DBCon))
        {

            string user = "";
            if (Request.QueryString["user"] != null)
            {
                user = Request.QueryString["user"].ToString();
            }
            else
            {
                user = Request.QueryString["merchant"].ToString();

            };

            using (SqlCommand cmd = new SqlCommand("select Ids,Print_Name,IP_Address from PosSys_Printer where deleteind <> 'X' and merchant_code='"+ Request.QueryString["merchant"] +"' and user_code='"+ user +"'", con))
            {
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter idr = new SqlDataAdapter(cmd);
                DataTable v = new DataTable();
                idr.Fill(v);

                if (v.Rows.Count > 0)
                {
                    grd_viewprinter.DataSource = v;
                    grd_viewprinter.DataBind();
                }
                else
                {
                    grd_viewprinter.DataSource = v;
                    grd_viewprinter.DataBind();

                }

            }
        }
    }

    protected void grd_viewmodifer_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            SqlConnection con = new SqlConnection(DBCon);
            con.Open();

            HiddenField hdmodifierID = (HiddenField)e.Row.FindControl("hdmodifierID");
            CheckBox chkmodifier = (CheckBox)e.Row.FindControl("chkmodifier");

            if (Request.QueryString["Item_code"].ToString() != "")
            {
                SqlCommand loadmodifiercmd = new SqlCommand("select * from mf_item_modifier_assign where item_code='"+ Request.QueryString["Item_code"].ToString() +"' and modifier_id = '" +hdmodifierID.Value+ "' and deleteind <> 'X'", con);
                SqlDataAdapter loadmodifieradp = new SqlDataAdapter(loadmodifiercmd);
                DataTable loadmodifierdt = new DataTable();
                loadmodifieradp.Fill(loadmodifierdt);

                if (loadmodifierdt.Rows.Count > 0)
                {

                    if (loadmodifierdt.Rows[0]["modifier_id"].ToString() == hdmodifierID.Value)
                    {
                        chkmodifier.Checked = true;
                    }
                    else
                    {
                        chkmodifier.Checked = false;
                    }
                }
            }

        }
    }


    protected void ddlweightitem_SelectedIndexChanged(object sender, EventArgs e)
    {
        if(ddlweightitem.SelectedValue == "YES")
        {
            txt_Barcode.Text = "";
            RDweighttype.SelectedValue = "KG";
            if (Request.QueryString["Item_code"].ToString() != "")
            {
                if (RDweighttype.SelectedValue == "KG")
                {
                    ddlUM.Items.Clear();
                    ddlUM.Items.Add(new ListItem("KG", "KG"));
                    ddlUM.Enabled= false;
                    ddlUM.SelectedValue="KG";
                }
                else if (RDweighttype.SelectedValue == "PCS")
                {
                    ddlUM.Items.Clear();
                    ddlUM.Items.Add(new ListItem("PCS", "PCS"));
                    ddlUM.Enabled= false;
                    ddlUM.SelectedValue="PCS";
                }
            }
            else
            {
                if (RDweighttype.SelectedValue == "KG")
                {
                    ddlUM.Items.Clear();
                    ddlUM.Items.Add(new ListItem("KG", "KG"));
                    ddlUM.Enabled= false;
                    ddlUM.SelectedValue="KG";
                }
                else if (RDweighttype.SelectedValue == "PCS")
                {
                    ddlUM.Items.Clear();
                    ddlUM.Items.Add(new ListItem("PCS", "PCS"));
                    ddlUM.Enabled= false;
                    ddlUM.SelectedValue="PCS";
                }
            }

            divweighttype.Visible = true;
            txt_Barcode.Attributes.Add("onkeypress", "return IsNumberWithNoDecimal(this,event);");
            txt_Barcode.Attributes.Add("MaxLength", "5");
        }
        else
        {
            txt_Barcode.Text = "";
            divweighttype.Visible = false;
            LoadUOM();
            ddlUM.Enabled= true;
            txt_Barcode.Attributes.Remove("onkeypress");
            txt_Barcode.Attributes.Remove("MaxLength");
        }
    }

    protected void RDweighttype_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (RDweighttype.SelectedValue == "KG")
        {
            ddlUM.Items.Clear();
            ddlUM.Items.Add(new ListItem("KG", "KG"));
            ddlUM.Enabled= false;
            ddlUM.SelectedValue="KG";
        }
        else if(RDweighttype.SelectedValue == "PCS")
        {
            LoadUOM();
            ddlUM.Enabled= true;
            ddlUM.SelectedValue="PCS";
        }
    }

    protected void grd_viewprinter_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            SqlConnection con = new SqlConnection(DBCon);
            con.Open();

            HiddenField hdprinterID = (HiddenField)e.Row.FindControl("hdprinterID");
            CheckBox chkprinter = (CheckBox)e.Row.FindControl("chkprinter");

            if (Request.QueryString["Item_code"].ToString() != "")
            {
                SqlCommand loadmodifiercmd = new SqlCommand("select * from PosSys_PrinterItem where item_code='"+ Request.QueryString["Item_code"].ToString() +"' and printer_id = '" + hdprinterID.Value + "'", con);
                SqlDataAdapter loadmodifieradp = new SqlDataAdapter(loadmodifiercmd);
                DataTable loadmodifierdt = new DataTable();
                loadmodifieradp.Fill(loadmodifierdt);

                if (loadmodifierdt.Rows.Count > 0)
                {

                    if (loadmodifierdt.Rows[0]["printer_id"].ToString() == hdprinterID.Value)
                    {
                        chkprinter.Checked = true;
                    }
                    else
                    {
                        chkprinter.Checked = false;
                    }
                }
            }

        }
    }


    protected void ddlcommby_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlcommby.SelectedValue == "Group Tier")
        {
            commamt.Visible = false; 
            commgroup.Visible = true;
        }
        else
        {
            commamt.Visible = true;
            commgroup.Visible = false;
        }
    }

    private void BindItemSuppliers()
    {
        string itemCode = (txt_ItemCode.Text == null) ? "" : txt_ItemCode.Text.Trim();

        DataTable dt = new DataTable();
        dt.Columns.Add("Ids", typeof(long));
        dt.Columns.Add("Supplier_Code", typeof(string));
        dt.Columns.Add("Supplier_Name", typeof(string));
        dt.Columns.Add("IsDefault", typeof(string));

        if (itemCode.Length > 0)
        {
            using (SqlConnection con = new SqlConnection(DBCon))
            using (SqlCommand cmd = new SqlCommand(@"
            SELECT i.Ids, i.Supplier_Code, s.Supplier_Name, i.IsDefault
            FROM MF_ItemSupplier i
            INNER JOIN MF_Supplier s ON s.Supplier_Code = i.Supplier_Code
            WHERE i.Item_Code = @Item_Code
            AND i.DeleteInd <> 'X';", con))
            {
                cmd.Parameters.AddWithValue("@Item_Code", itemCode);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
        }

        if (dt.Rows.Count > 0)
        {
            grdSuppliers.DataSource = dt;
            grdSuppliers.DataBind();
        }
        else
        {
            // show "no data" row
            dt.Rows.Add(dt.NewRow());
            grdSuppliers.DataSource = dt;
            grdSuppliers.DataBind();

            int colCount = grdSuppliers.Columns.Count;
            grdSuppliers.Rows[0].Cells.Clear();
            System.Web.UI.WebControls.TableCell cell = new System.Web.UI.WebControls.TableCell();
            cell.ColumnSpan = colCount;
            cell.Text = "No suppliers found for this item.";
            cell.HorizontalAlign = HorizontalAlign.Center;
            cell.ForeColor = System.Drawing.Color.Red;
            cell.Font.Bold = true;
            grdSuppliers.Rows[0].Cells.Add(cell);
        }
    }

    private void BindSupplierDropdown()
    {
        if (grdSuppliers.FooterRow == null) return;

        DropDownList ddl = (DropDownList)grdSuppliers.FooterRow.FindControl("ddlSupplierNameNew");
        if (ddl == null) return;

        using (SqlConnection con = new SqlConnection(DBCon))
        using (SqlCommand cmd = new SqlCommand(@"
        SELECT Supplier_Code, Supplier_Name
        FROM MF_Supplier
        WHERE (DeleteInd IS NULL OR DeleteInd <> 'X')
        ORDER BY Supplier_Name;", con))
        {
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adp.Fill(dt);

            ddl.DataSource = dt;
            ddl.DataTextField = "Supplier_Name";
            ddl.DataValueField = "Supplier_Code";
            ddl.DataBind();

            ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Supplier --", ""));
        }
    }

    protected void grdSuppliers_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Footer)

        {
            DropDownList ddl = (DropDownList)e.Row.FindControl("ddlSupplierNameNew");
            if (ddl != null)
            {
                using (SqlConnection con = new SqlConnection(DBCon))
                using (SqlCommand cmd = new SqlCommand("SELECT Supplier_Code, Supplier_Name FROM MF_Supplier WHERE DeleteInd <> 'X'", con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    ddl.DataSource = dt;
                    ddl.DataTextField = "Supplier_Name";   
                    ddl.DataValueField = "Supplier_Code";  
                    ddl.DataBind();

                    ddl.Items.Insert(0, new ListItem("-- Select Supplier --", ""));
                }
            }
            
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                DropDownList ddlDefault = (DropDownList)e.Row.FindControl("ddlDefault");
                if (ddlDefault != null)
                {
                    string currentValue = DataBinder.Eval(e.Row.DataItem, "IsDefault").ToString();
                    ddlDefault.SelectedValue = currentValue;
                }
            }
        }

        {
            BindSupplierDropdown();
        }
    }

    protected void ddlSupplierNameNew_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = (DropDownList)sender;
        GridViewRow row = (GridViewRow)ddl.NamingContainer;
        TextBox txtCode = (TextBox)row.FindControl("txtSupplierCodeNew");
        if (txtCode != null)
        {
            txtCode.Text = ddl.SelectedValue; // Supplier_Code
        }
    }


    protected void grdSuppliers_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "InsertNew")
        {
            string itemCode = txt_ItemCode.Text.Trim();
            if (string.IsNullOrEmpty(itemCode))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg",
                    "alert('Please save the item first before adding suppliers.');", true);
                return;
            }

            GridViewRow footer = grdSuppliers.FooterRow;
            DropDownList ddlSupplier = (DropDownList)footer.FindControl("ddlSupplierNameNew");
            DropDownList ddlDefault = (DropDownList)footer.FindControl("ddlDefaultNew");

            string supplierCode = ddlSupplier.SelectedValue;
            string isDefault = ddlDefault.SelectedValue;

            if (string.IsNullOrEmpty(supplierCode))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg",
                    "alert('Please select a supplier.');", true);
                return;
            }

            using (SqlConnection con = new SqlConnection(DBCon))
            {
                con.Open();
                using (SqlTransaction tx = con.BeginTransaction())
                {
                    try
                    {
                        // Prevent duplicate supplier
                        using (SqlCommand checkCmd = new SqlCommand(
                            "SELECT COUNT(*) FROM MF_ItemSupplier " +
                            "WHERE Item_Code=@Item_Code AND Supplier_Code=@Supplier_Code AND DeleteInd <> 'X'", con, tx))
                        {
                            checkCmd.Parameters.AddWithValue("@Item_Code", itemCode);
                            checkCmd.Parameters.AddWithValue("@Supplier_Code", supplierCode);
                            int exists = (int)checkCmd.ExecuteScalar();
                            if (exists > 0)
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "msg",
                                    "alert('This supplier is already linked to this item.');", true);
                                tx.Rollback();
                                return;
                            }
                        }

                        if (isDefault == "Yes")
                        {
                            using (SqlCommand resetCmd = new SqlCommand(
                                "UPDATE MF_ItemSupplier SET IsDefault='No' " +
                                "WHERE Item_Code=@Item_Code AND DeleteInd <> 'X'", con, tx))
                            {
                                resetCmd.Parameters.AddWithValue("@Item_Code", itemCode);
                                resetCmd.ExecuteNonQuery();
                            }
                        }

                        using (SqlCommand cmd = new SqlCommand(
                            "INSERT INTO MF_ItemSupplier (Item_Code, Supplier_Code, IsDefault, Created_Dt) " +
                            "VALUES (@Item_Code, @Supplier_Code, @IsDefault, GETDATE())", con, tx))
                        {
                            cmd.Parameters.AddWithValue("@Item_Code", itemCode);
                            cmd.Parameters.AddWithValue("@Supplier_Code", supplierCode);
                            cmd.Parameters.AddWithValue("@IsDefault", isDefault);
                            cmd.ExecuteNonQuery();
                        }

                        tx.Commit();
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }

            BindItemSuppliers();
            BindSupplierDropdown();
        }
    }





    protected void grdSuppliers_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        long ids = Convert.ToInt64(grdSuppliers.DataKeys[e.RowIndex].Value);

        using (SqlConnection con = new SqlConnection(DBCon))
        using (SqlCommand cmd = new SqlCommand(
            "UPDATE MF_ItemSupplier SET DeleteInd='X', Modified_Dt=GETDATE() WHERE Ids=@Ids", con))
        {
            con.Open();
            cmd.Parameters.AddWithValue("@Ids", ids);
            cmd.ExecuteNonQuery();
        }

        BindItemSuppliers();
    }



    protected void grdSuppliers_RowEditing(object sender, GridViewEditEventArgs e)
    {
        grdSuppliers.EditIndex = e.NewEditIndex;
        BindItemSuppliers();
        BindSupplierDropdown();
    }

    protected void grdSuppliers_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        grdSuppliers.EditIndex = -1;
        BindItemSuppliers();
        BindSupplierDropdown();
    }

    private void SetDefaultSupplier(string itemCode, string supplierCode, SqlConnection con, SqlTransaction tx)
    {
        using (SqlCommand reset = new SqlCommand(
         "UPDATE MF_ItemSupplier SET IsDefault='No' WHERE Item_Code=@Item_Code AND Supplier_Code<>@Supplier_Code AND DeleteInd<>'X'", con, tx))
        {
            reset.Parameters.AddWithValue("@Item_Code", itemCode);
            reset.Parameters.AddWithValue("@Supplier_Code", supplierCode);
            reset.ExecuteNonQuery();
        }

        using (SqlCommand set = new SqlCommand(
            "UPDATE MF_ItemSupplier SET IsDefault='Yes' WHERE Item_Code=@Item_Code AND Supplier_Code=@Supplier_Code", con, tx))
        {
            set.Parameters.AddWithValue("@Item_Code", itemCode);
            set.Parameters.AddWithValue("@Supplier_Code", supplierCode);
            set.ExecuteNonQuery();
        }
    }


    protected void grdSuppliers_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        long ids = Convert.ToInt64(grdSuppliers.DataKeys[e.RowIndex].Value);
        GridViewRow row = grdSuppliers.Rows[e.RowIndex];
        DropDownList ddlDefault = row.FindControl("ddlDefault") as DropDownList;

        if (ddlDefault != null)
        {
            using (SqlConnection con = new SqlConnection(DBCon))
            {
                con.Open();

                // if setting default, reset others
                if (ddlDefault.SelectedValue == "Yes")
                {
                    using (SqlCommand resetCmd = new SqlCommand(
                        "UPDATE MF_ItemSupplier SET IsDefault='No' WHERE Item_Code=@itemCode AND DeleteInd <> 'X'", con))
                    {
                        resetCmd.Parameters.AddWithValue("@itemCode", txt_ItemCode.Text);
                        resetCmd.ExecuteNonQuery();
                    }
                }

                // update current row
                using (SqlCommand cmd = new SqlCommand(
                    "UPDATE MF_ItemSupplier SET IsDefault=@isDefault, Modified_Dt=GETDATE() WHERE Ids=@Ids", con))
                {
                    cmd.Parameters.AddWithValue("@isDefault", ddlDefault.SelectedValue);
                    cmd.Parameters.AddWithValue("@Ids", ids);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        grdSuppliers.EditIndex = -1;
        BindItemSuppliers();
    }




    private void BindSuppliersGrid()
    {
        string itemCode = txt_ItemCode.Text.Trim();
        if (string.IsNullOrEmpty(itemCode)) return;

        using (SqlConnection con = new SqlConnection(DBCon))
        using (SqlCommand cmd = new SqlCommand(@"
        SELECT i.Supplier_Code, s.Supplier_Name, i.IsDefault
        FROM MF_ItemSupplier i
        INNER JOIN MF_Supplier s ON i.Supplier_Code = s.Supplier_Code
        WHERE i.Item_Code = @Item_Code AND i.DeleteInd <> 'X'
    ", con))
        {
            cmd.Parameters.AddWithValue("@Item_Code", itemCode);

            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adp.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                grdSuppliers.DataSource = dt;
                grdSuppliers.DataBind();
            }
            else
            {
                dt.Rows.Add(dt.NewRow());
                grdSuppliers.DataSource = dt;
                grdSuppliers.DataBind();

                int columnsCount = grdSuppliers.Columns.Count;
                grdSuppliers.Rows[0].Cells.Clear();
                grdSuppliers.Rows[0].Cells.Add(new TableCell());
                grdSuppliers.Rows[0].Cells[0].ColumnSpan = columnsCount;
                grdSuppliers.Rows[0].Cells[0].Text = "NO SUPPLIER FOUND";
                grdSuppliers.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }
        }
    }


}