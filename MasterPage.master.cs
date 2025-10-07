using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Globalization;

public partial class MasterPage : System.Web.UI.MasterPage
{
    /// <summary>
    /// Last Modified : Hann
    /// Date : 04/07/2016
    /// Table Used : -
    /// Store Proc Used : -
    /// </summary>

    Business BL = new Business();
    DBClass DL = new DBClass();
    protected static string DBCon;
    public static string Logout;
    public int count;

    //protected static String DBCon = ConfigurationManager.AppSettings["ConnectionString"].ToString();

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.MaintainScrollPositionOnPostBack = true;
        if (HttpContext.Current.Request.Url.AbsolutePath == "https://ezyshare.online/ezysharelisting/ContentApproval.aspx")
        {
            product.Attributes.Add("class", "btn11 active");
            //product.Attributes.Add("class","btn11");
        }
        if (HttpContext.Current.Request.Url.AbsolutePath == "https://ezyshare.online/ezysharelisting/DepartmentSetupList.aspx")
        {
            department.Attributes.Add("class", "btn11 active");
            product.Attributes.Add("class", "btn11");
        }
        if (HttpContext.Current.Request.Url.AbsolutePath == "https://ezyshare.online/ezysharelisting//CategorySetupList.aspx")
        {
            category.Attributes.Add("class", "btn11 active");
            product.Attributes.Add("class", "btn11");
        }
        if (HttpContext.Current.Request.Url.AbsolutePath == "https://ezyshare.online/ezysharelisting//BrandSetupList.aspx")
        {
            Brand.Attributes.Add("class", "btn11 active");
            product.Attributes.Add("class", "btn11");
        }
        if (HttpContext.Current.Request.Url.AbsolutePath == "https://ezyshare.online/ezysharelisting//ItemGroupingListing.aspx")
        {
            grouping.Attributes.Add("class", "btn11 active");
            product.Attributes.Add("class", "btn11");
        }
        if (HttpContext.Current.Request.Url.AbsolutePath == "https://ezyshare.online/ezysharelisting//PromoCampaign.aspx")
        {
            promotion.Attributes.Add("class", "btn11 active");
            product.Attributes.Add("class", "btn11");
        }
        //IframeId.Attributes["src"] = "ContentApproval.aspx?merchant=" + Request.QueryString["merchant"].ToString();
    }

    public string GetActive()
{
    return System.IO.Path.GetFileNameWithoutExtension(System.Web.HttpContext.Current.Request.Url.AbsolutePath).ToLower();
}

    protected void advance_click(object sender, EventArgs e)
    {
        Session["PName"] = "Advance Item Listing";
        Response.Redirect("AdvanceListing.aspx");
    }

    protected void modifier_click(object sender, EventArgs e)
    {
        Session["PName"] = "Modifier Listing";
        Response.Redirect("ModifierListing.aspx");
    }

    protected void printer_click(object sender, EventArgs e)
    {
        Session["PName"] = "Printer Listing";
        Response.Redirect("PrinterListing.aspx");
    }

    //protected void template_click(object sender, EventArgs e)
    //{
    //    Session["PName"] = "Template Setup";
    //    Response.Redirect("TemplateSetup.aspx?edit=1");
    //}

    protected void terminal_click(object sender, EventArgs e)
    {
        Session["PName"] = "POS Terminal Listing";
        Response.Redirect("TerminalListing.aspx");
    }

    protected void country_click(object sender, EventArgs e)
    {
        Session["PName"] = "Country Listing";
        Response.Redirect("Country.aspx");
    }

    protected void state_click(object sender, EventArgs e)
    {
        Session["PName"] = "State Listing";
        Response.Redirect("StateList.aspx");
    }

    protected void city_click(object sender, EventArgs e)
    {
        Session["PName"] = "City Listing";
        Response.Redirect("CityListing.aspx");
    }

    protected void ShipRateSetup_click(object sender, EventArgs e)
    {
        Session["PName"] = "Shipping Rate Setup Listing";
        Response.Redirect("ShippingRateSetupListing.aspx");
    }

    protected void user_click(object sender, EventArgs e)
    {
        Session["PName"] = "User Listing";
        Response.Redirect("UserListing.aspx");
    }

    protected void st_click(object sender, EventArgs e)
    {
        Session["PName"] = "Sales Tax Listing";
        Response.Redirect("STListing.aspx");
    }

    protected void pt_click(object sender, EventArgs e)
    {
        Session["PName"] = "Purchase Tax Listing";
        Response.Redirect("PTListing.aspx");
    }

    protected void uom_click(object sender, EventArgs e)
    {
        Session["PName"] = "UOM Listing";
        Response.Redirect("UOMListing.aspx");
    }

    protected void brand_click(object sender, EventArgs e)
    {
        Session["PName"] = "Item Brand Listing";
        Response.Redirect("BrandListing.aspx");
    }

    protected void payment_click(object sender, EventArgs e)
    {
        Session["PName"] = "Non-Cash Listing";
        Response.Redirect("PaymentListing.aspx");
    }

    protected void store_click(object sender, EventArgs e)
    {
        Session["PName"] = "Store Listing";
        Response.Redirect("BranchListing.aspx");
    }
    protected void table_click(object sender, EventArgs e)
    {
        Session["PName"] = "F&B Table Listing";
        Response.Redirect("TableListing.aspx");
    }

    protected void transaction_click(object sender, EventArgs e)
    {
        Session["PName"] = "Transaction Type Setup";
        Response.Redirect("TransactionListing.aspx");
    }
    //protected void promo_click(object sender, EventArgs e)
    //{
    //    Session["PName"] = "Promotion Item Listing";
    //    Response.Redirect("PromotionListing.aspx");
    //}

    //protected void price_click(object sender, EventArgs e)
    //{
    //    Session["PName"] = "Price Item Listing";
    //    Response.Redirect("PriceListing.aspx");
    //}

    protected void item_click(object sender, EventArgs e)
    {
        Session["PName"] = "Item Listing";
        Response.Redirect("BItemListing.aspx");
        
    }

    protected void cat_click(object sender, EventArgs e)
    {
        Session["PName"] = "Category Listing";
        Response.Redirect("CategoryListing.aspx");
    }

    protected void dept_click(object sender, EventArgs e)
    {
        Session["PName"] = "Department Listing";
        Response.Redirect("DepartmentListing.aspx");
    }

    protected void db_click(object sender, EventArgs e)
    {
        Session["PName"] = "Dashboard";
        Response.Redirect("LandingPage.aspx");
    }

    protected void logout_Click(object sender, EventArgs e)
    {
        //WS_License.Service1SoapClient ws1 = new WS_License.Service1SoapClient();
        //Logout = ws1.GetLogout();

        //Session.Clear();
        //Session.Abandon();
        //Session["SID"] = null;
        //Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //Response.Cache.SetAllowResponseInBrowserHistory(false);
        //Response.Write("<script>window.parent.location.href='" + Logout + "'</script>");
    }

    protected void btn_item_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["user"] != null)
        {

            Response.Redirect("ContentApproval.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "&user=" + Request.QueryString["user"].ToString());
            product.Attributes.Add("class", "active");
        }
        else
        {

            Response.Redirect("ContentApproval.aspx?merchant=" + Request.QueryString["merchant"].ToString());
            product.Attributes.Add("class", "active");

        };
    }

    protected void btn_department_Click(object sender, EventArgs e)
    {
        Response.Redirect("DepartmentSetupList.aspx?merchant=" + Request.QueryString["merchant"].ToString());
        department.Attributes.Add("class","active");
    }

    protected void btn_category_Click(object sender, EventArgs e)
    {
        Response.Redirect("CategorySetupList.aspx?merchant=" + Request.QueryString["merchant"].ToString());
        category.Attributes.Add("class", "active");
    }

    protected void Brand_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("BrandSetupList.aspx?merchant=" + Request.QueryString["merchant"].ToString());
        category.Attributes.Add("class", "active");
    }

    protected void grouping_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("ItemGroupingListing.aspx?merchant=" + Request.QueryString["merchant"].ToString());
        grouping.Attributes.Add("class", "active");
    }

    protected void promotion_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("PromoCampaign.aspx?merchant=" + Request.QueryString["merchant"].ToString());
        promotion.Attributes.Add("class", "active");
    }
    protected void modifier_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("ModifierSetting.aspx?merchant=" + Request.QueryString["merchant"].ToString());
        promotion.Attributes.Add("class", "active");
    }

    protected void printer_ServerClick(object sender, EventArgs e)
    {
        if (Request.QueryString["user"] != null)
        {
            Response.Redirect("Printer_MaintenanceListing.aspx?merchant=" + Request.QueryString["merchant"].ToString() + "&user=" + Request.QueryString["user"].ToString());
            promotion.Attributes.Add("class", "active");
        }
        else
        {
            Response.Redirect("Printer_MaintenanceListing.aspx?merchant=" + Request.QueryString["merchant"].ToString() );
            promotion.Attributes.Add("class", "active");
        };
    }

    protected void wizard_ServerClick(object sender, EventArgs e)
    {
        if (Request.QueryString["user"] != null)
        {
            Response.Redirect("../simplifiedmerchant/item.aspx?mid=" + Request.QueryString["merchant"].ToString());
            promotion.Attributes.Add("class", "active");
        }
        else
        {
            Response.Redirect("../simplifiedmerchant/item.aspx?mid=" + Request.QueryString["merchant"].ToString());
            promotion.Attributes.Add("class", "active");
        };
    }
}
