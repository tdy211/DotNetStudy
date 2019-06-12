using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;
using System.Data;

namespace Ace.App_Code
{
    /// <summary>
    /// 处理JQGRID数据
    /// </summary>
    public class JQGridTest : IHttpHandler
    {
        DBClass dbObj = new DBClass();
        public void ProcessRequest(HttpContext context)
        {
            NameValueCollection forms = context.Request.Form;
            string strOperation = forms.Get("oper");
            string strResponse = string.Empty;
            if (strOperation == null) //oper = null which means its first load.
            {
                string sqlStr = "select * from jqGridData";
                
                DataTable dt = dbObj.GetDataSetStr(sqlStr, "jqGridData");
                string jsonRes = Newtonsoft.Json.JsonConvert.SerializeObject(dt);

                context.Response.Write(jsonRes);
            }
            else if(strOperation == "del")
            {
               //删除；提供id，返回修改成功
               //forms = {oper=del&id=35}
                strResponse = "删除成功";
                context.Response.Write(strResponse);
                
            }
            else if(strOperation == "add")
            {
             //增加,提供除id外的字段，返回增加成功
            //forms = {
            //id=&
            //sdate=2019-01-29&
            //name=%u7ae5%u5927%u4e91&
            //stock=Yes&
            //ship=IN&
            //note=y&
            //oper=add}
                strResponse = "添加成功";
                context.Response.Write(strResponse);
            }
            else if (strOperation == "edit")
            {
                //修改，提供包括id内的所有字段，返回修改成功
                //forms = {
                //id=35&
                //sdate=1999-11-24&
                //name=test2&
                //stock=No&
                //ship=FE&
                //note=444&
                //oper=edit}
                strResponse = "编辑成功";
                context.Response.Write(strResponse);
            }
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }








    }
}