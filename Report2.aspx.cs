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

    protected static String strConStringrp = ConfigurationManager.AppSettings["ConnectionString"].ToString();
    SqlConnection con2 = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"].ToString());

    public static string merchant;
    public static string outlet;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["m"] != null)
            {
                merchant = Request.QueryString["m"].ToString();
            }
            else
            {
                merchant = "";
            }

            if (Request.QueryString["b"] != null)
            {
                outlet = Request.QueryString["b"].ToString();
            }
            else
            {
                outlet = "";
            }

            if (merchant == "")
            {
                ddlMerchant.Items.Clear();
                ddlMerchant.Items.Add(new ListItem(("- Select Merchant -"), ""));

                ddlOutlet.Items.Clear();
                ddlOutlet.Items.Add(new ListItem(("- Select Outlet -"), ""));
            }
            else
            {
                SqlDataAdapter adap = new SqlDataAdapter("select * from vms_supplier where supplier_code='" + merchant.ToString() + "' order by supplier_name", con2);
                DataTable dt = new DataTable();
                adap.Fill(dt);
                ddlMerchant.Items.Clear();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow Dr in dt.Rows)
                    {
                        ddlMerchant.Items.Add(new ListItem(Dr["supplier_name"].ToString(), Dr["supplier_code"].ToString()));
                    }
                }
                else
                {
                    ddlMerchant.Items.Add(new ListItem(("- Select Merchant -"), ""));
                }

                if (outlet == "")
                {
                    ddlOutlet.Items.Clear();
                    ddlOutlet.Items.Add(new ListItem(("- Select Outlet -"), ""));
                }
                else if (outlet == "all")
                {
                    SqlDataAdapter adap2 = new SqlDataAdapter("select * from vms_supplier_merchant where supplier_code='" + merchant.ToString() + "' AND DeleteInd != 'X' order by outlet_name", con2);
                    DataTable dt2 = new DataTable();
                    adap2.Fill(dt2);
                    ddlOutlet.Items.Clear();
                    ddlOutlet.Items.Add(new ListItem(("- Select Outlet -"), ""));
                    if (dt2.Rows.Count > 0)
                    {
                        if (dt2.Rows.Count > 1)
                        {
                            ddlOutlet.Items.Add(new ListItem(("All Outlet"), "-all-"));
                        }

                        foreach (DataRow Dr in dt2.Rows)
                        {
                            ddlOutlet.Items.Add(new ListItem(Dr["outlet_name"].ToString(), Dr["outlet_code"].ToString()));
                        }
                    }
                }
                else
                {
                    SqlDataAdapter adap2 = new SqlDataAdapter("select * from vms_supplier_merchant where supplier_code='" + merchant.ToString() + "' and outlet_code='" + outlet.ToString() + "' order by outlet_name", con2);
                    DataTable dt2 = new DataTable();
                    adap2.Fill(dt2);
                    ddlOutlet.Items.Clear();
                    if (dt2.Rows.Count > 0)
                    {
                        foreach (DataRow Dr in dt2.Rows)
                        {
                            ddlOutlet.Items.Add(new ListItem(Dr["outlet_name"].ToString(), Dr["outlet_code"].ToString()));
                        }
                    }
                }
            }
        }
    }

    protected void btn_Excel_Click(object sender, EventArgs e)
    {
        if (ddlMerchant.SelectedValue == "")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Please select merchant.');", true);
        }
        else if (ddlOutlet.SelectedValue == "")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Please select outlet.');", true);
        }
        else if (txt_FDate.Text == "")
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
            SqlConnection con = new SqlConnection(strConStringrp);
            SqlCommand getMerchantType = new SqlCommand("select MerchantType from vms_supplier WHERE supplier_code = '" + ddlMerchant.SelectedValue.ToString() +"'", con);
            con.Open();
            string MerchantType = getMerchantType.ExecuteScalar().ToString();
            con.Close();
            if (MerchantType == "Markup")
            {
                exportExcelTypeMarkup();
            }
            else
            {
                exportExcelTypeOverride();
            }
        }
    }


    protected void exportExcelTypeMarkup()
    {
        using (SqlConnection con = new SqlConnection(strConStringrp))
        {
            using (SqlCommand cmd = new SqlCommand("Report_SO_Details", con)) //Pending changes
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DateFrom", Convert.ToDateTime(txt_FDate.Text).ToString("yyyyMMdd"));
                cmd.Parameters.AddWithValue("@DateTo", Convert.ToDateTime(txt_TDate.Text).AddDays(1).ToString("yyyyMMdd"));
                cmd.Parameters.AddWithValue("@Merchant", ddlMerchant.SelectedValue);
                cmd.Parameters.AddWithValue("@Outlet", ddlOutlet.SelectedValue);
                cmd.Parameters.AddWithValue("@MerchantType", "Markup");

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
                        ds.Tables[3].TableName = "Item Return"; // Added item return as requested. Basically just #report2 but with Return Qty. > 0

                        if (ddlOutlet.SelectedValue == "-all-")
                        {
                            ds.Tables[4].TableName = "Summary";
                        }


                        using (XLWorkbook wb = new XLWorkbook())
                        {
                            if (ddlOutlet.SelectedValue != "-all-")
                            {
                                for (int i = 0; i <= 3; i++) //Modified to not print out auxilary table (Since the if else structure is in the Stored Procedure, I have no idea how to get total without the extra tables sorry)
                                {
                                    //Add DataTable as Worksheet.
                                    wb.Worksheets.Add(ds.Tables[i]);
                                }
                            }
                            else
                            {
                                for (int i = 0; i <= 4; i++) //Modified to not print out auxilary table (Since the if else structure is in the Stored Procedure, I have no idea how to get total without the extra tables sorry)
                                {
                                    //Add DataTable as Worksheet.
                                    wb.Worksheets.Add(ds.Tables[i]);
                                }
                            }

                            //Sum in Last Row - Added by intern guy
                            //Receipt Sales
                            IXLWorksheet receiptSales;
                            wb.Worksheets.TryGetWorksheet("Receipt Sales", out receiptSales);

                            int lastRow = ds.Tables[0].Rows.Count;
                            receiptSales.Cell(lastRow + 2, 1).Value = "Total";
                            receiptSales.Cell(lastRow + 2, 4).FormulaA1 = "=SUM(D2:D" + (lastRow + 1) + ")";
                            receiptSales.Cell(lastRow + 2, 8).FormulaA1 = "=SUM(H2:H" + (lastRow + 1) + ")";
                            receiptSales.Cell(lastRow + 2, 9).FormulaA1 = "=SUM(I2:I" + (lastRow + 1) + ")";

                            //Product Sales
                            IXLWorksheet productSales;
                            wb.Worksheets.TryGetWorksheet("Product Sales", out productSales);

                            lastRow = ds.Tables[1].Rows.Count;
                            int lastColumn = ds.Tables[1].Columns.Count;

                            productSales.Cell(lastRow + 2, 11).FormulaA1 = "SUM(K2:K" + (lastRow + 1) + ")";

                            char columnChar = 'M';
                            //Print total from M column until the last column (case sensitive)
                            //j is for column digit
                            //Pretty sure there is a better way to do this
                            for (int j = 13; j <= lastColumn; j++)
                            {
                                productSales.Cell(lastRow + 2, j).FormulaA1 = "SUM(" + columnChar + "2:" + columnChar + (lastRow + 1) + ")";
                                columnChar++;
                            }

                            

                            //Item Return
                            IXLWorksheet itemReturn;
                            wb.Worksheets.TryGetWorksheet("Item Return", out itemReturn);

                            lastRow = ds.Tables[3].Rows.Count;
                            lastColumn = ds.Tables[3].Columns.Count;
                            columnChar = 'J';

                            itemReturn.Cell(lastRow + 2, 1).Value = "Total";
                            for (int j = 10; j <= lastColumn; j++)
                            {
                                itemReturn.Cell(lastRow + 2, j).FormulaA1 = "SUM(" + columnChar + "2:" + columnChar + (lastRow + 1) + ")";
                                columnChar++;
                            }

                            if (ddlOutlet.SelectedValue != "-all-")
                            {
                                //Summary - Added by intern guy
                                IXLWorksheet summary = wb.Worksheets.Add("Summary");

                                decimal totalSales = 0;
                                decimal totalCost = 0;
                                decimal salesReturn = 0;
                                decimal costReturn = 0;


                                if (ds.Tables[0].Rows.Count > 0) //If no data do not try to get to prevent exception
                                {
                                    totalSales = ds.Tables[4].Rows[0].Field<decimal>("Gross Sales"); //Table 4 is used to get SUM([Net Amt.]) from #report
                                    totalCost = ds.Tables[4].Rows[0].Field<decimal>("Total Cost");

                                    if (ds.Tables[3].Rows.Count > 0)
                                    {
                                        costReturn = ds.Tables[5].Rows[0].Field<decimal>("Total Return Cost");
                                        salesReturn = ds.Tables[5].Rows[0].Field<decimal>("Total Return Sales");

                                    }
                                }
                                // I print the summary cell by cell, not sure if there is a better way for doing this or not
                                summary.Cell(2, 2).Value = "Merchant Sales Summary For (" + Convert.ToDateTime(txt_FDate.Text).ToString("yyyyMMdd") + " - " + Convert.ToDateTime(txt_TDate.Text).ToString("yyyyMMdd") + ")";
                                summary.Cell(4, 2).Value = "Merchant:";
                                summary.Cell(5, 2).Value = "Merchant Outlet:";


                                summary.Cell(4, 3).Value = ddlMerchant.SelectedItem;
                                summary.Cell(5, 3).Value = ddlOutlet.SelectedItem;


                                summary.Cell(8, 2).Value = "Sales";
                                summary.Cell(9, 2).Value = "Gross Sales:";
                                summary.Cell(10, 2).Value = "Total Return";
                                summary.Cell(12, 2).Value = "Net Sales:";

                                summary.Cell(9, 3).Value = totalSales;
                                summary.Cell(10, 3).Value = salesReturn;
                                summary.Cell(12, 3).Value = totalSales - salesReturn;

                                summary.Cell(14, 2).Value = "Payable";
                                summary.Cell(15, 2).Value = "Total Cost:";
                                summary.Cell(16, 2).Value = "Total Return";
                                summary.Cell(18, 2).Value = "Total Payable:";

                                summary.Cell(15, 3).Value = totalCost;
                                summary.Cell(16, 3).Value = costReturn;
                                summary.Cell(18, 3).Value = totalCost - costReturn;



                            }
                            else
                            {
                                IXLWorksheet summary;
                                wb.Worksheets.TryGetWorksheet("Summary", out summary);

                                lastRow = ds.Tables[4].Rows.Count;
                                summary.Cell(lastRow + 2, 1).Value = "Total";
                                summary.Cell(lastRow + 2, 3).FormulaA1 = "=SUM(C2:C" + (lastRow + 1) + ")";
                                summary.Cell(lastRow + 2, 4).FormulaA1 = "=SUM(D2:D" + (lastRow + 1) + ")";
                                summary.Cell(lastRow + 2, 5).FormulaA1 = "=SUM(E2:E" + (lastRow + 1) + ")";
                                summary.Cell(lastRow + 2, 6).FormulaA1 = "=SUM(F2:F" + (lastRow + 1) + ")";
                                summary.Cell(lastRow + 2, 7).FormulaA1 = "=SUM(G2:G" + (lastRow + 1) + ")";
                                summary.Cell(lastRow + 2, 8).FormulaA1 = "=SUM(H2:H" + (lastRow + 1) + ")";
                                summary.Cell(lastRow + 2, 9).FormulaA1 = "=SUM(I2:I" + (lastRow + 1) + ")";
                            }
                            //Summary End - No more modification by intern guy beyond this point
                            //Export the Excel file.
                            string attachment = "attachment; filename=Sales Performance Report (" + ddlMerchant.SelectedItem + ") (" + ddlOutlet.SelectedItem + ") (" + Convert.ToDateTime(txt_FDate.Text).ToString("yyyyMMdd") + " - " + Convert.ToDateTime(txt_TDate.Text).ToString("yyyyMMdd") + ").xls";

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
            }
        }//

    }


    protected void exportExcelTypeOverride()
    {
        using (SqlConnection con = new SqlConnection(strConStringrp))
        {
            using (SqlCommand cmd = new SqlCommand("Report_SO_Details", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DateFrom", Convert.ToDateTime(txt_FDate.Text).ToString("yyyyMMdd"));
                cmd.Parameters.AddWithValue("@DateTo", Convert.ToDateTime(txt_TDate.Text).AddDays(1).ToString("yyyyMMdd"));
                cmd.Parameters.AddWithValue("@Merchant", ddlMerchant.SelectedValue);
                cmd.Parameters.AddWithValue("@Outlet", ddlOutlet.SelectedValue);
                cmd.Parameters.AddWithValue("@MerchantType", "Override");

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
                        ds.Tables[3].TableName = "Item Return"; // Added item return as requested. Basically just #report2 but with Return Qty. > 0

                        if (ddlOutlet.SelectedValue == "-all-")
                        {
                            ds.Tables[4].TableName = "Summary";
                        }


                        using (XLWorkbook wb = new XLWorkbook())
                        {
                            if (ddlOutlet.SelectedValue != "-all-")
                            {
                                for (int i = 0; i <= 3; i++) //Modified to not print out auxilary table (Since the if else structure is in the Stored Procedure, I have no idea how to get total without the extra tables sorry)
                                {
                                    //Add DataTable as Worksheet.
                                    wb.Worksheets.Add(ds.Tables[i]);
                                }
                            }
                            else
                            {
                                for (int i = 0; i <= 4; i++) //Modified to not print out auxilary table (Since the if else structure is in the Stored Procedure, I have no idea how to get total without the extra tables sorry)
                                {
                                    //Add DataTable as Worksheet.
                                    wb.Worksheets.Add(ds.Tables[i]);
                                }
                            }

                            //Sum in Last Row - Added by intern guy
                            //Receipt Sales
                            IXLWorksheet receiptSales;
                            wb.Worksheets.TryGetWorksheet("Receipt Sales", out receiptSales);

                            int lastRow = ds.Tables[0].Rows.Count;
                            receiptSales.Cell(lastRow + 2, 1).Value = "Total";
                            receiptSales.Cell(lastRow + 2, 4).FormulaA1 = "=SUM(D2:D" + (lastRow + 1) + ")";
                            receiptSales.Cell(lastRow + 2, 8).FormulaA1 = "=SUM(H2:H" + (lastRow + 1) + ")";
                            receiptSales.Cell(lastRow + 2, 9).FormulaA1 = "=SUM(I2:I" + (lastRow + 1) + ")";

                            //Product Sales
                            IXLWorksheet productSales;
                            wb.Worksheets.TryGetWorksheet("Product Sales", out productSales);

                            lastRow = ds.Tables[1].Rows.Count;
                            int lastColumn = ds.Tables[1].Columns.Count;

                            productSales.Cell(lastRow + 2, 1).Value = "Total";
                            productSales.Cell(lastRow + 2, 11).FormulaA1 = "=SUM(K2:K" + (lastRow + 1) + ")";

                            char columnChar = 'M';
                            //Print total from M column until the last column (case sensitive)
                            //j is for column digit
                            //Pretty sure there is a better way to do this
                            for (int j = 13; j <= lastColumn; j++)
                            {
                                productSales.Cell(lastRow + 2, j).FormulaA1 = "SUM(" + columnChar + "2:" + columnChar + (lastRow + 1) + ")";
                                columnChar++;
                            }

                            //Item Return
                            IXLWorksheet itemReturn;
                            wb.Worksheets.TryGetWorksheet("Item Return", out itemReturn);

                            lastRow = ds.Tables[3].Rows.Count;
                            lastColumn = ds.Tables[3].Columns.Count;
                            columnChar = 'J';

                            itemReturn.Cell(lastRow + 2, 1).Value = "Total";
                            for (int j = 10; j <= lastColumn; j++)
                            {
                                itemReturn.Cell(lastRow + 2, j).FormulaA1 = "SUM(" + columnChar + "2:" + columnChar + (lastRow + 1) + ")";
                                columnChar++;
                            }

                            if (ddlOutlet.SelectedValue != "-all-")
                            {
                                //Summary - Added by intern guy
                                IXLWorksheet summary = wb.Worksheets.Add("Summary");

                                decimal grossSales = 0;
                                decimal totalReturn = 0;
                                decimal fees = 0;
                                decimal maxR = 0;

                                if (ds.Tables[0].Rows.Count > 0) //If no data do not try to get to prevent exception
                                {
                                    grossSales = ds.Tables[4].Rows[0].Field<decimal>("Gross Sales"); //Table 4 is used to get SUM([Net Amt.]) from #report

                                    if (ds.Tables[3].Rows.Count > 0)
                                    {
                                        totalReturn = ds.Tables[5].Rows[0].Field<decimal>("Total Return"); //Table 5 is used to get SUM([Return QtyAmt.]) from #report2
                                    }

                                    fees = ds.Tables[6].Rows[0].Field<decimal>("Fees"); //Table 6 is to get Fees and the max and min rate
                                    maxR = ds.Tables[6].Rows[0].Field<decimal>("Rate");
                                }
                                // I print the summary cell by cell, not sure if there is a better way for doing this or not
                                summary.Cell(2, 2).Value = "Merchant Sales Summary For (" + Convert.ToDateTime(txt_FDate.Text).ToString("yyyyMMdd") + " - " + Convert.ToDateTime(txt_TDate.Text).ToString("yyyyMMdd") + ")";
                                summary.Cell(4, 2).Value = "Merchant:";
                                summary.Cell(5, 2).Value = "Merchant Outlet:";
                                summary.Cell(6, 2).Value = "Merchant MDR Rate %:";

                                summary.Cell(4, 3).Value = ddlMerchant.SelectedItem;
                                summary.Cell(5, 3).Value = ddlOutlet.SelectedItem;
                                summary.Cell(6, 3).Value = maxR + "%" + "-" + maxR + "%";

                                summary.Cell(8, 2).Value = "Sales";
                                summary.Cell(9, 2).Value = "Gross Sales:";
                                summary.Cell(10, 2).Value = "Item Return:";
                                summary.Cell(12, 2).Value = "MDR Fees:";
                                summary.Cell(14, 2).Value = "Total Payable:";

                                summary.Cell(9, 3).Value = grossSales;
                                summary.Cell(10, 3).Value = totalReturn;
                                summary.Cell(12, 3).Value = Math.Round(fees, 2);
                                summary.Cell(14, 3).Value = grossSales - totalReturn - Math.Round(fees, 2);

                            }
                            else
                            {
                                IXLWorksheet summary;
                                wb.Worksheets.TryGetWorksheet("Summary", out summary);

                                lastRow = ds.Tables[4].Rows.Count;
                                summary.Cell(lastRow + 2, 1).Value = "Total";
                                summary.Cell(lastRow + 2, 4).FormulaA1 = "=SUM(D2:D" + (lastRow + 1) + ")";
                                summary.Cell(lastRow + 2, 5).FormulaA1 = "=SUM(E2:E" + (lastRow + 1) + ")";
                                summary.Cell(lastRow + 2, 6).FormulaA1 = "=SUM(F2:F" + (lastRow + 1) + ")";
                                summary.Cell(lastRow + 2, 7).FormulaA1 = "=SUM(G2:G" + (lastRow + 1) + ")";
                                summary.Cell(lastRow + 2, 8).FormulaA1 = "=SUM(H2:H" + (lastRow + 1) + ")";


                            }
                            //Summary End - No more modification by intern guy beyond this point
                            //Export the Excel file.
                            string attachment = "attachment; filename=Sales Performance Report (" + ddlMerchant.SelectedItem + ") (" + ddlOutlet.SelectedItem + ") (" + Convert.ToDateTime(txt_FDate.Text).ToString("yyyyMMdd") + " - " + Convert.ToDateTime(txt_TDate.Text).ToString("yyyyMMdd") + ").xls";

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
            }
        }
    }

    protected void ddlMerchant_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlMerchant.SelectedValue == "")
        {
            ddlOutlet.Items.Clear();
            ddlOutlet.Items.Add(new ListItem(("- Select Outlet -"), ""));
        }
        else
        {
            SqlDataAdapter adap2 = new SqlDataAdapter("select * from vms_supplier_merchant where supplier_code='" + ddlMerchant.SelectedValue + "' order by outlet_name", con2);
            DataTable dt2 = new DataTable();
            adap2.Fill(dt2);
            ddlOutlet.Items.Clear();
            ddlOutlet.Items.Add(new ListItem(("- Select Outlet -"), ""));
            if (dt2.Rows.Count > 0)
            {
                if (dt2.Rows.Count > 1)
                {
                    ddlOutlet.Items.Add(new ListItem(("All Outlet"), "all"));
                }
                
                foreach (DataRow Dr in dt2.Rows)
                {
                    ddlOutlet.Items.Add(new ListItem(Dr["outlet_name"].ToString(), Dr["outlet_code"].ToString()));
                }
            }
        }
    }
}