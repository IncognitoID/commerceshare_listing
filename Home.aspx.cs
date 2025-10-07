using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.Services;
using System.Web;
using System.Configuration;
using System.Drawing.Imaging;
using System.IO;
using System.Drawing.Drawing2D;
using System.Net;
using System.Drawing;

public partial class Home : System.Web.UI.Page
{

    public string myConnectionString;
    public SqlConnection myConnection;

    public static string DBCon;
    public static string Logout;

    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

    public enum MessageType { Success, Error, Info, Warning };

    public string memberid = "";

    protected void Page_Load(object sender, EventArgs e) 
    {
        //slider();
        //string alertmessage = "";
        //alertmessage = "<h4><b>Memo: Delivery Schedule Update</b></h4> Dear valued customers, </br> Please note that any orders placed after January 19th, 2023 will be delivered after January 25th, 2023 due to Chinese New Year holidays. We apologize for any inconvenience this may cause and appreciate your patience and understanding. If you have any further questions, please do not hesitate to contact us, thank you.";
        //ShowMessage_warning2("" + alertmessage + "", MessageType.Warning);
        //return;
    }

    #region message
    protected void ShowMessage(string Message, MessageType type)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "sweetalert_success('" + Message + "','" + type + "');", true);
    }

    protected void ShowMessage_warning(string Message, MessageType type)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "sweetalert_warning('" + Message + "','" + type + "');", true);
    }

    protected void ShowMessage_warning2(string Message, MessageType type)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "sweetalert_warning2('" + Message + "','" + type + "');", true);
    }

    protected void ShowMessage_item_warning(string Message, string id, MessageType type)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage_item_warning('" + Message + "','" + id + "','" + type + "');", true);
    }

    #endregion

    //protected void Page_Load(object sender, EventArgs e)
    //{
    //    if(Page.IsPostBack)
    //    {
    //        promo.Visible = true;
    //        branch.Visible = false;
    //        aboutus.Visible = false;

    //    }
    //}

    //protected void btnpromo_Click(object sender, EventArgs e)
    //{
    //    promo.Visible = true;
    //    branch.Visible = false;
    //    aboutus.Visible = false;
    //}

    //protected void btnbranch_Click(object sender, EventArgs e)
    //{
    //    promo.Visible = false;
    //    branch.Visible = true;
    //    aboutus.Visible = false;
    //}

    //protected void btnaboutus_Click(object sender, EventArgs e)
    //{
    //    promo.Visible = false;
    //    branch.Visible = false;
    //    aboutus.Visible = true;
    //}
    //protected void Page_Load(object sender, EventArgs e)
    //{
    //    if(Page.IsPostBack)
    //    {
    //        promo.Visible = true;
    //        branch.Visible = false;
    //        aboutus.Visible = false;

    //    }
    //}

    //protected void btnpromo_Click(object sender, EventArgs e)
    //{
    //    promo.Visible = true;
    //    branch.Visible = false;
    //    aboutus.Visible = false;
    //}

    //protected void btnbranch_Click(object sender, EventArgs e)
    //{
    //    promo.Visible = false;
    //    branch.Visible = true;
    //    aboutus.Visible = false;
    //}

    //protected void btnaboutus_Click(object sender, EventArgs e)
    //{
    //    promo.Visible = false;
    //    branch.Visible = false;
    //    aboutus.Visible = true;
    //}
    protected void btntc_Click(object sender, EventArgs e)
    {
        Response.Redirect("TermnCondition.aspx");
    }

    protected void slider()
    {

        //string sql = "Select * from OutletDetails order by ids desc";

        //using (SqlCommand cmd = new SqlCommand(sql, con))
        //{
            //cmd.CommandType = CommandType.Text;
            //con.Open();
            //SqlDataAdapter idr = new SqlDataAdapter(cmd);

            //DataTable v = new DataTable();
            //idr.Fill(v);

            //con.Close();
        //}
    }

    protected void outlet_preview_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { });
        string outlet = commandArgs[0];

        System.Web.HttpBrowserCapabilities browser = Request.Browser;

        if (browser.IsMobileDevice == true)
        {
            Response.Redirect("Outlet_Info.aspx?outlet=" + outlet);
        }
        else
        {
            Response.Redirect("Outlet_Info.aspx?outlet=" + outlet);
        }
    }

    protected void outlet_preview_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        HtmlImage outlet_img = (HtmlImage)e.Item.FindControl("outlet_img");
        Label lblOutletB = (Label)e.Item.FindControl("lblOutletBranch");
        Label lblOutletState = (Label)e.Item.FindControl("lblOutletState");

        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DataRowView drv = (DataRowView)(e.Item.DataItem);
            lblOutletB.Text = drv.Row["OutletBranch"].ToString();
            lblOutletState.Text = drv.Row["OutletState"].ToString();
            string imgUrl = drv.Row["OutletImg"].ToString();
            outlet_img.Attributes.Add("src", imgUrl);
        }
    }
}