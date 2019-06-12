using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using System.Data.SqlClient;

namespace EasyUI.ashx
{
    /// <summary>
    /// easyuiTest 的摘要说明
    /// </summary>
    public class easyuiTest : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //数据库连接字符串：
            //string connString = "data source=119.180.261.117,1433;initial catalog=anxiuyun;user id=sa;pwd=sa";
            //这里data source 是指数据库地址和端口号，需要注意的是地址和端口号之间是用","进行分隔的；initial catalog 是数据库名字 ；user id和pwd分别指用户名和密码。
            SqlConnection conn = new SqlConnection("Data Source=(local);uid=sa2;pwd=ctesi1234;Initial Catalog=db_Student;");
            string command = context.Request.QueryString["test"];//前台传的标示值   
            if (command == "add")
            {
                try
                {
                    


                    string StuNum = context.Request["StuNum"];
                    string StuName = context.Request["StuName"];
                    string Phone = context.Request["Phone"];
                    string Email = context.Request["Email"];
                    string sql = "INSERT INTO StuInfo VALUES('" + StuNum + "','" + StuName + "','" + Phone + "','" + Email + "')";
                    conn.Open();
                    SqlCommand com = new SqlCommand(sql, conn);
                    com.ExecuteNonQuery();
                    conn.Close();
                    context.Response.Write("T");
                    context.Response.End();

                }
                catch (Exception ex)
                {
                    context.Response.Write("F");
                }
            }
            else if (command == "modify")
            {

                try
                {
                    string StuID = context.Request["StuId"];
                    string StuNum = context.Request["StuNum"];
                    string StuName = context.Request["StuName"];
                    string Phone = context.Request["Phone"];
                    string Email = context.Request["Email"];
                    string sql = "UPDATE StuInfo SET StuNum='" + StuNum + "',StuName='" + StuName + "',Phone='" + Phone + "',Email='" + Email + "' WHERE StuId=" + StuID + "";
                    conn.Open();
                    SqlCommand com = new SqlCommand(sql, conn);
                    com.ExecuteNonQuery();
                    conn.Close();
                    context.Response.Write("T");
                    context.Response.End();

                }
                catch (Exception ex)
                {
                    context.Response.Write("F");
                }
            }
            else if (command == "delete")
            {


                try
                {
                    string StuID = context.Request["StuId"];
                    string sql = "DELETE FROM StuInfo WHERE StuId=" + StuID + "";
                    conn.Open();
                    SqlCommand com = new SqlCommand(sql, conn);
                    com.ExecuteNonQuery();
                    conn.Close();
                    context.Response.Write("T");
                    context.Response.End();


                }
                catch (Exception ex)
                {
                    context.Response.Write("F");
                }
            }
            else
            {//调用查询方法
                DataSet ds = new DataSet();
                string sql = "select * from StuInfo";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                da.Fill(ds);
                string strJson = Dataset2Json(ds, -1);//DataSet数据转化为Json数据
                context.Response.Write(strJson);//返回给前台页面
                context.Response.End();
            }
        }
        #region DataSet转换成Json格式
        /// <summary>
        /// DataSet转换成Json格式  
        /// </summary>  
        /// <param name="ds">DataSet</param> 
        /// <returns></returns>  
        public static string Dataset2Json(DataSet ds, int total = -1)
        {
            StringBuilder json = new StringBuilder();


            foreach (DataTable dt in ds.Tables)
            {
                //{"total":5,"rows":[
                json.Append("{\"total\":");
                if (total == -1)
                {
                    json.Append(dt.Rows.Count);
                }
                else
                {
                    json.Append(total);
                }
                json.Append(",\"rows\":[");
                json.Append(DataTable2Json(dt));
                json.Append("]}");
            } return json.ToString();
        }
        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        #region dataTable转换成Json格式
        public static string DataTable2Json(DataTable dt)
        {
            StringBuilder jsonBuilder = new StringBuilder();


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonBuilder.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(dt.Columns[j].ColumnName);
                    jsonBuilder.Append("\":\"");
                    jsonBuilder.Append(dt.Rows[i][j].ToString());
                    jsonBuilder.Append("\",");
                }
                if (dt.Columns.Count > 0)
                {
                    jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                }
                jsonBuilder.Append("},");
            }
            if (dt.Rows.Count > 0)
            {
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            }


            return jsonBuilder.ToString();
        }
        #endregion dataTable转换成Json格式


    }
}