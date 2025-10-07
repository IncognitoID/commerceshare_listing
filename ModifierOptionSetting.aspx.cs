using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ModifierOptionSetting : System.Web.UI.Page
{
    protected static string DBCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
    public enum MessageType { success, error, info, warning };
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            MF_Con.Text = DBCon;
            BindGrid();

            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[4] { new DataColumn("ids", typeof(string)), new DataColumn("runno", typeof(string)), new DataColumn("option_name", typeof(string)), new DataColumn("option_price", typeof(decimal)) });
            ViewState["optionlist"] = dt;

            if (Request.QueryString["ModifierID"].ToString() != "")
            {
                btnAddC.Visible = false;
                btnUpdateC.Visible = true;
                loadmodifier();
                grdoptionlist.Visible=false;
                grdoptionlist2.Visible=true;

            }
            else
            {
                grdoptionlist.Visible=true;
                grdoptionlist2.Visible=false;
            }


        }
    }

    protected void loadmodifier()
    {
        SqlConnection con = new SqlConnection(MF_Con.Text);
        con.Open();
        SqlCommand getmodifiercmd = new SqlCommand("select * from MF_Modifier_Group where Modifier_ID='"+ Request.QueryString["ModifierID"].ToString() +"' and Merchant_Code='"+ Request.QueryString["merchant"].ToString() +"' and Deleteind <> 'X' ", con);
        SqlDataAdapter getmodifieradp = new SqlDataAdapter(getmodifiercmd);
        DataTable getmodifierdt = new DataTable();
        getmodifieradp.Fill(getmodifierdt);

        if(getmodifierdt.Rows.Count > 0)
        {
            txtModifierName.Text = getmodifierdt.Rows[0]["Modifier_Grp_Name"].ToString();
            txtminselection.Text = getmodifierdt.Rows[0]["Min_Select"].ToString();
            txtmaxselection.Text = getmodifierdt.Rows[0]["Max_Select"].ToString();
            txtremarks.Text  = getmodifierdt.Rows[0]["remarks"].ToString();
        }


        SqlCommand getmodifieroptcmd = new SqlCommand("select '' as runno,ids,Option_Name,Option_Price from MF_Modifier_option where Modifier_ID='"+ Request.QueryString["ModifierID"].ToString() +"' and Deleteind <> 'X' ", con);
        SqlDataAdapter getmodifieroptadp = new SqlDataAdapter(getmodifieroptcmd);
        DataTable getmodifieroptdt = new DataTable();
        getmodifieroptadp.Fill(getmodifieroptdt);

        if (getmodifieroptdt.Rows.Count > 0)
        {
            grdoptionlist2.DataSource=getmodifieroptdt;
            grdoptionlist2.DataBind();

        }

    }

    protected void BindGrid()
    {
        grdoptionlist.DataSource = (DataTable)ViewState["optionlist"];
        grdoptionlist.DataBind();
    }

    protected void btnadd_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["ModifierID"].ToString() == "")
        {
            DataTable checkgriddt = (DataTable)ViewState["optionlist"];
            bool ifExist = false;
            foreach (DataRow dr in checkgriddt.Rows)
            {
                if (dr["option_name"].ToString() == txtoption.Text.Trim())
                {
                    ifExist = true;
                }
            }
            if (!ifExist)
            {
                DataTable optionlistdt = (DataTable)ViewState["optionlist"];

                string runno = (optionlistdt.Rows.Count + 1).ToString();
                decimal price = 0;

                if (txtoptionprice.Text != "")
                {
                    price = Convert.ToDecimal(txtoptionprice.Text);
                }

                optionlistdt.Rows.Add(runno, runno, txtoption.Text, price);
                ViewState["optionlist"] = optionlistdt;
                this.BindGrid();
                txtoption.Text = "";
                txtoptionprice.Text = "0";

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Option name is already added in list.');", true);
            }
        }
        else
        {
            SqlConnection con = new SqlConnection(MF_Con.Text);
            con.Open();
            SqlCommand insertoptcmd = new SqlCommand("insert into MF_Modifier_Option(Modifier_ID,Option_Name,Option_Price,deleteind,Created_DT,Modified_DT) values('"+ Request.QueryString["ModifierID"].ToString() +"',N'"+ txtoption.Text +"',@optionprice ,'',dateadd(hour,(8),CONVERT([varchar](20),getutcdate(),(120))),dateadd(hour,(8),CONVERT([varchar](20),getutcdate(),(120))))", con);
            
            insertoptcmd.Parameters.AddWithValue("@optionprice", txtoptionprice.Text);

            try
            {
                insertoptcmd.ExecuteNonQuery();
                txtoption.Text = "";
                txtoptionprice.Text = "0";
            }
            catch(Exception ec)
            {

            }
            loadmodifier();
        }
    }

    protected void grdoptionlist_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    protected void grdoptionlist_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (Request.QueryString["ModifierID"].ToString() == "")
        {
            DataTable dt = (DataTable)ViewState["optionlist"];
            int index = Convert.ToInt32(e.RowIndex);
            // Delete from ViewState.
            dt.Rows[index].Delete();
            ViewState["Data"] = dt;

            DataTable getcolumdt = (DataTable)ViewState["optionlist"];
            BindGrid();
        }
       
    }

    protected void ShowMessage_warning(string Message, MessageType type)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "sweetalert_warning('" + Message + "','" + type + "');", true);
    }

    protected void ShowMessage_error(string Message, MessageType type)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "sweetalert_error('" + Message + "','" + type + "');", true);
    }


    protected void btnAddC_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(MF_Con.Text);
        con.Open();
        SqlCommand chkmodifiercmd = new SqlCommand("select * from MF_Modifier_Group where Modifier_Grp_Name = '"+ txtModifierName.Text +"' and deleteind <> 'X' and merchant_code='" + Request.QueryString["merchant"].ToString() + "'", con);
        SqlDataAdapter chkmodifieradp = new SqlDataAdapter(chkmodifiercmd);
        DataTable chkmodifierdt = new DataTable();
        chkmodifieradp.Fill(chkmodifierdt);

        DataTable optionlist = (DataTable)ViewState["optionlist"];

        SqlCommand updatemodifierrunnocmd = new SqlCommand("update tb_AllRunno set runno=runno + 1 from tb_AllRunno where ColumnName = 'Modifier_ID'", con);
        updatemodifierrunnocmd.ExecuteNonQuery();

        if (chkmodifierdt.Rows.Count <= 0)
        {
            SqlCommand getmodifieridcmd = new SqlCommand("select runno + 1 as runno from tb_AllRunno where ColumnName = 'Modifier_ID'", con);
            SqlDataAdapter getmodifieridadp = new SqlDataAdapter(getmodifieridcmd);
            DataTable getmodifieriddt = new DataTable();
            getmodifieridadp.Fill(getmodifieriddt);

            if (getmodifieriddt.Rows.Count > 0)
            {
                decimal minselect = 0;
                decimal maxselect = 0; 
                
                if(txtminselection.Text == "")
                {
                    minselect = 0;
                }
                else
                {
                    minselect = Convert.ToDecimal(txtminselection.Text);
                }

                if(txtmaxselection.Text == "")
                {
                    maxselect = 1;
                }
                else
                {
                    maxselect = Convert.ToDecimal(txtmaxselection.Text);
                }

                SqlCommand insertmodifiercmd = new SqlCommand("insert into mf_modifier_group(Modifier_ID,Modifier_Grp_Name,Created_DT,Modified_DT,Min_Select,Max_Select,Deleteind,Merchant_Code,remarks) values('"+ getmodifieriddt.Rows[0]["runno"].ToString() +"','"+ txtModifierName.Text +"',dateadd(hour,(8),CONVERT([varchar](20),getutcdate(),(120))),dateadd(hour,(8),CONVERT([varchar](20),getutcdate(),(120))),'"+ minselect +"','"+ maxselect +"','','"+ Request.QueryString["merchant"].ToString() +"','"+ txtremarks.Text +"')", con);

                try
                {
                    insertmodifiercmd.ExecuteNonQuery();

                    if (optionlist.Rows.Count > 0)
                    {
                        foreach (DataRow dr in optionlist.Rows)
                        {
                            SqlCommand insertcmd = new SqlCommand("insert into MF_Modifier_Option(Modifier_ID,Option_Name,Option_Price,deleteind,Created_DT,Modified_DT) values('"+ getmodifieriddt.Rows[0]["runno"].ToString() +"',N'"+ dr["option_name"].ToString() +"',@optionprice ,'',dateadd(hour,(8),CONVERT([varchar](20),getutcdate(),(120))),dateadd(hour,(8),CONVERT([varchar](20),getutcdate(),(120))))", con);
                            
                            insertcmd.Parameters.AddWithValue("@optionprice", dr["option_price"].ToString());

                            try
                            {
                                insertcmd.ExecuteNonQuery();


                                ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "sweetalert_warning('Item Modifier Updated', 'success');window.location='ModifierSetting.aspx?merchant="+ Request.QueryString["merchant"] +"';", true);

                            }
                            catch (Exception es)
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Failed to insert,please retry later.');", true);

                            }

                        }
                    }

                }
                catch (Exception es)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Failed to insert,please retry later.');", true);
                }



            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "AlertCode", "alert('Modifier already existed.');", true);

        }

    }

    protected void btnUpdateC_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(MF_Con.Text);
        con.Open();

        SqlCommand chkmodifiercmd = new SqlCommand("select * from MF_Modifier_Group where Modifier_id = '"+ Request.QueryString["ModifierID"].ToString() +"' and deleteind <> 'X' and merchant_code='" + Request.QueryString["merchant"].ToString() + "'", con);
        SqlDataAdapter chkmodifieradp = new SqlDataAdapter(chkmodifiercmd);
        DataTable chkmodifierdt = new DataTable();
        chkmodifieradp.Fill(chkmodifierdt);

        if(chkmodifierdt.Rows.Count > 0)
        {
            SqlCommand updatemodifier = new SqlCommand("update MF_Modifier_Group set Modifier_Grp_Name=N'"+ txtModifierName.Text +"',Min_Select='"+ txtminselection.Text +"',Max_Select='"+ txtmaxselection.Text +"',remarks='"+txtremarks.Text+"' where deleteind <> 'X' and merchant_code='"+ Request.QueryString["merchant"].ToString() +"' and modifier_ID='"+ Request.QueryString["ModifierID"].ToString() +"'", con);
            updatemodifier.ExecuteNonQuery();


            ShowMessage_warning("Modifier Updated", MessageType.success);

        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("ModifierSetting.aspx?merchant="+ Request.QueryString["merchant"].ToString() );

    }

    protected void grdoptionlist2_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if(e.CommandName == "optiondelete")
        {
            if (Request.QueryString["ModifierID"].ToString() != "")
            {
                using (SqlConnection con = new SqlConnection(DBCon))
                {
                    con.Open();
                    using (SqlCommand delsubcmd = new SqlCommand("Update mf_modifier_option set DeleteInd=@delete where ids = '" + e.CommandArgument.ToString() +"' and deleteind <> 'X'", con))
                    {
                        delsubcmd.Parameters.AddWithValue("@delete", "X");
                        delsubcmd.ExecuteNonQuery();

                        loadmodifier();
                    }
                }
            }
        }
    }

}