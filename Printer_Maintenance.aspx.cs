using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

public partial class Printer_Maintenance : System.Web.UI.Page
{
    protected static string DBCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string user = "";
            if (Request.QueryString["user"] != null)
            {
                 user = Request.QueryString["user"].ToString();
            }
            else
            {
                 user = Request.QueryString["merchant"].ToString();
            }

            MF_Con.Text = DBCon;

            if (Request.QueryString["id"].ToString() != "")
            {
                SqlConnection conn = new SqlConnection(MF_Con.Text);
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from possys_printer where ids='"+ Request.QueryString["id"].ToString() +"' and deleteind <> 'X' and merchant_code='"+ Request.QueryString["merchant"].ToString() +"' and user_code='"+ user +"'",conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                if(dt.Rows.Count > 0)
                {
                    btn_Insert.Visible= false;
                    btn_Update.Visible= true;
                    txt_IPAddress.Text = dt.Rows[0]["ip_address"].ToString();
                    txt_printerName.Text = dt.Rows[0]["print_name"].ToString();
                }

            }
            else
            {
                btn_Insert.Visible= true;
                btn_Update.Visible= false;
            }

        }
    }

    public bool IsAlphanumeric(string input)
    {
        Regex regex = new Regex("^[a-zA-Z0-9\\s]+$");
        return regex.IsMatch(input);
    }

    protected void Back_OnClick(object sender, EventArgs e)
    {
        if (Request.QueryString["user"] != null)
        {
            Response.Redirect("Printer_MaintenanceListing.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "&user=" + Request.QueryString["user"].ToString());
        }
        else
        {
            Response.Redirect("Printer_MaintenanceListing.aspx?merchant=" + Request.QueryString["merchant"].ToString() );
        }
        
    }

    public bool IsIPAddressValid(string input)
    {
        IPAddress ipAddress;
        bool isValid = IPAddress.TryParse(input, out ipAddress);
        return isValid;
    }


    protected void btn_Insert_Click(object sender, EventArgs e)
    {
        string IPAddInput = txt_IPAddress.Text;
        bool isInputValid = IsIPAddressValid(IPAddInput);

        string PrinterInput = txt_printerName.Text;
        bool isPrintInputValid = IsAlphanumeric(PrinterInput);

        string user = "";
        if (Request.QueryString["user"] != null)
        {
            user = Request.QueryString["user"].ToString();
        }
        else
        {
            user = Request.QueryString["merchant"].ToString();

        };


        SqlConnection con = new SqlConnection(MF_Con.Text);
        con.Open();
        SqlCommand checkcmd = new SqlCommand("select * from possys_printer where Print_Name='"+ txt_printerName.Text +"' and merchant_code='"+ Request.QueryString["merchant"].ToString() +"' and user_code='"+ user +"' and deleteind <> 'X' ", con);
        SqlDataAdapter checkadp = new SqlDataAdapter(checkcmd);
        DataTable checkdt = new DataTable();
        checkadp.Fill(checkdt);

        if (checkdt.Rows.Count <= 0)
        {
            if (!isPrintInputValid)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Print Name only allow Alphanumeric');", true);
            }
            else if (!isInputValid)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('IP Address is invalid or IP address cannot more then 255.(Example:***.***.***.***)');", true);
            }
            else
            {
                SqlCommand insertcmd = new SqlCommand("insert into PosSys_Printer(Print_Name,IP_Address,Merchant_Code,User_Code,Created_DT,Modified_DT,deleteind) values(@Print_Name,@IP_Address,@Merchant,@user,DATEADD(HOUR, 8, CONVERT(varchar(20),GETUTCDATE(),120)),DATEADD(HOUR, 8, CONVERT(varchar(20),GETUTCDATE(),120)),'')", con);
                insertcmd.Parameters.AddWithValue("@Print_Name", txt_printerName.Text);
                insertcmd.Parameters.AddWithValue("@IP_Address", txt_IPAddress.Text);
                insertcmd.Parameters.AddWithValue("@Merchant", Request.QueryString["merchant"].ToString());
                if (Request.QueryString["user"] != null)
                {
                    insertcmd.Parameters.AddWithValue("@user", Request.QueryString["user"].ToString());
                }
                else
                {
                    insertcmd.Parameters.AddWithValue("@user", Request.QueryString["merchant"].ToString());
                };

                insertcmd.ExecuteNonQuery();

                if (Request.QueryString["user"] != null)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Print Inserted.');window.location.href='Printer_MaintenanceListing.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "&user="+ Request.QueryString["user"].ToString() + "'", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Print Inserted.');window.location.href='Printer_MaintenanceListing.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "'", true);
                };
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Printer name already existed in merchant.');", true);
        }

        con.Close();
    }

    protected void btn_Update_Click(object sender, EventArgs e)
    {
        string IPAddInput = txt_IPAddress.Text;
        bool isInputValid = IsIPAddressValid(IPAddInput);

        string PrinterInput = txt_printerName.Text;
        bool isPrintInputValid = IsAlphanumeric(PrinterInput);

        string user = "";
        if (Request.QueryString["user"] != null)
        {
            user = Request.QueryString["user"].ToString();
        }
        else
        {
            user = Request.QueryString["merchant"].ToString();
            
        };

        SqlConnection con = new SqlConnection(MF_Con.Text);
        con.Open();
        SqlCommand checkcmd = new SqlCommand("select * from possys_printer where Print_Name='"+ txt_printerName.Text +"' and ids <> '"+ Request.QueryString["id"].ToString() +"' and merchant_code='"+ Request.QueryString["merchant"].ToString() +"' and user_code='"+ user +"' and deleteind <> 'X' ", con);
        SqlDataAdapter checkadp = new SqlDataAdapter(checkcmd);
        DataTable checkdt = new DataTable();
        checkadp.Fill(checkdt);

        if (checkdt.Rows.Count <= 0)
        {
            if (!isPrintInputValid)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Print Name only allow Alphanumeric');", true);
            }
            else if (!isInputValid)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('IP Address is invalid or IP address cannot more then 255.(Example:***.***.***.***)');", true);
            }
            else
            {
                SqlCommand insertcmd = new SqlCommand("update PosSys_Printer set Print_Name=@Print_Name,IP_Address=@IP_Address,Modified_DT=DATEADD(HOUR, 8, CONVERT(varchar(20),GETUTCDATE(),120)) where ids='"+ Request.QueryString["id"].ToString() +"' and merchant_code='"+ Request.QueryString["merchant"].ToString() +"' and user_code='"+ user +"' and deleteind <> 'X'", con);
                insertcmd.Parameters.AddWithValue("@Print_Name", txt_printerName.Text);
                insertcmd.Parameters.AddWithValue("@IP_Address", txt_IPAddress.Text);
                insertcmd.ExecuteNonQuery();

                ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Printer Updated.');", true);

            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Printer name already existed in merchant.');", true);
        }

        con.Close();
    }
}