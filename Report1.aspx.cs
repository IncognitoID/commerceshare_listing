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
using ClosedXML.Excel;

public partial class GroupListing : System.Web.UI.Page
{
    Business BL = new Business();
    DBClass DL = new DBClass();
    protected static string DBCon;
    public static string Logout;
    public int count;

    protected static String strConStringrp = ConfigurationManager.AppSettings["ConnectionString"].ToString();

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btn_Excel_Click(object sender, EventArgs e)
    {
        if (txt_FDate.Text == "")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Please select date from.');", true);
        }
        else if (txt_TDate.Text == "")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Please select date to.');", true);
        }
        else if (Convert.ToDateTime(txt_FDate.Text) > Convert.ToDateTime(txt_TDate.Text))
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Invalid date range.');", true);
        }
        else
        {
            using (SqlConnection con = new SqlConnection(strConStringrp))
            {
                using (SqlCommand cmd = new SqlCommand("Report_SO_Details", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DateFrom", Convert.ToDateTime(txt_FDate.Text).ToString("yyyyMMdd"));
                    cmd.Parameters.AddWithValue("@DateTo", Convert.ToDateTime(txt_TDate.Text).AddDays(1).ToString("yyyyMMdd"));

                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        sda.SelectCommand = cmd;

                        using (DataSet ds = new DataSet())
                        {
                            sda.Fill(ds);

                            //Set Name of DataTables.
                            ds.Tables[0].TableName = "Receipt Sales";
                            ds.Tables[1].TableName = "Product Sales";
                            ds.Tables[2].TableName = "Category Sales";

                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                foreach (DataTable dt in ds.Tables)
                                {
                                    //Add DataTable as Worksheet.
                                    wb.Worksheets.Add(dt);
                                }

                                //Export the Excel file.
                                string attachment = "attachment; filename=Sales Performance Report (" + Convert.ToDateTime(txt_FDate.Text).ToString("yyyyMMdd") + " - " + Convert.ToDateTime(txt_TDate.Text).ToString("yyyyMMdd") + ").xls";

                                Response.Clear();
                                Response.Buffer = true;
                                Response.Charset = "";
                                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                Response.AddHeader("content-disposition", attachment);

                                using (MemoryStream MyMemoryStream = new MemoryStream())
                                {
                                    wb.SaveAs(MyMemoryStream);
                                    MyMemoryStream.WriteTo(Response.OutputStream);
                                    Response.Flush();
                                    Response.End();
                                }
                            }
                        }
                    }


                    //con.Open();

                    //SqlDataReader idr = cmd.ExecuteReader();

                    //if (idr.HasRows == true)
                    //{
                    //    DataTable v = new DataTable();

                    //    v.Load(idr);

                    //    //DataRow _ravi = v.NewRow();

                    //    //v.Rows.Add(_ravi);

                    //    string attachment = "attachment; filename=Sales Performance Report (" + Convert.ToDateTime(txt_FDate.Text).ToString("yyyyMMdd") + " - " + Convert.ToDateTime(txt_TDate.Text).ToString("yyyyMMdd") + ").xls";
                    //    Response.ClearContent();
                    //    Response.AddHeader("content-disposition", attachment);
                    //    Response.ContentType = "application/vnd.ms-excel";

                    //    string tab = "";

                    //    foreach (DataColumn dc in v.Columns)
                    //    {
                    //        Response.Write(tab + dc.ColumnName);
                    //        tab = "\t";
                    //    }

                    //    Response.Write("\n");

                    //    int i;

                    //    foreach (DataRow dr in v.Rows)
                    //    {
                    //        tab = "";

                    //        for (i = 0; i < v.Columns.Count; i++)
                    //        {
                    //            Response.Write(tab + dr[i].ToString());
                    //            tab = "\t";
                    //        }

                    //        Response.Write("\n");
                    //    }
                    //    Response.End();
                    //}
                    //else
                    //{
                    //    ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('No record found.');", true);
                    //}

                    //idr.Close();
                    //con.Close();
                }
            }
        }
    }
}