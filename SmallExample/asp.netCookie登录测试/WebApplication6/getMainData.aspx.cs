using System;
using System.Collections.Generic;
using System.Data;
namespace WebApplication6
{
    public partial class getMainData : System.Web.UI.Page
    {
        DBClass dbObj = new DBClass();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string sqlStr;
                string dataClass=Request.QueryString["type"];
                if(dataClass==null)
                {
                    sqlStr = "select * from mainData";
                }else
                {
                    sqlStr = "select * from mainData where type='" + dataClass + "'";
                }
                
                DataTable dsTable = dbObj.GetDataSetStr(sqlStr, "tbClass");
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(dsTable);
                Response.Write(json);
            }
            catch(Exception)
            {
                throw;
            }
            finally
            {
                Response.End();
            }

        }
    }
}