using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
namespace Ace.App_Code
{ 
/// <summary>
/// DBClass 的摘要说明
/// </summary>
public class DBClass
{
	public DBClass()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}
    /// <summary>
    /// 连接数据库
    /// </summary>
    /// <returns>返回数据库连接对象</returns>
    public SqlConnection GetConnection()
    {
        //string myStr=ConfigurationManager.AppSettings["ConnectionString"].ToString();
        string myStr = "server=(local);database=db_Student;UId=sa2;password=ctesi1234";
        SqlConnection myConn=new SqlConnection(myStr);
        return myConn;
    }
    /// <summary>
    /// 执行SQL语句，返回受影响的行数（用于添加，修改，删除）
    /// </summary>
    /// <param name="myCmd">执行SQL语句的命令对象</param>
    public void ExecNonQuery(SqlCommand myCmd)
    {
        try
        {
            if(myCmd.Connection.State!=ConnectionState.Open)
            {
                myCmd.Connection.Open();
            }
            myCmd.ExecuteNonQuery();
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
        finally
        {
            if(myCmd.Connection.State==ConnectionState.Open)
            {
                myCmd.Connection.Close();
            }
        }
    }
    /// <summary>
    /// 执行查询，并返回查询所返回的结果集中第一行第一列
    /// </summary>
    /// <param name="myCmd">执行sql的命令对象</param>
    /// <returns></returns>
    public string  ExecScalar(SqlCommand myCmd)
    {
        string strSql;
        try
        {
            if (myCmd.Connection.State != ConnectionState.Open)
            {
                myCmd.Connection.Open();
            }
            //执行查询，并返回查询所返回的结果集中第一行的第一列。忽略其他行或列，返回值为object类型
            strSql = Convert.ToString(myCmd.ExecuteScalar());
            return strSql;
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
        finally
        {
            if (myCmd.Connection.State == ConnectionState.Open)
            {
                myCmd.Connection.Close();
            }
        }
    }
    /// <summary>
    /// 从数据库中检索数据，并返回数据集的表集合
    /// </summary>
    /// <param name="myCmd">sql命令</param>
    /// <param name="TableName">返回的数据表名称</param>
    /// <returns></returns>
    public DataTable GetDataSet(SqlCommand myCmd,string TableName)
    {
        SqlDataAdapter adapt;
        DataSet ds = new DataSet();
        try
        {
            if (myCmd.Connection.State != ConnectionState.Open)
            {
                myCmd.Connection.Open();
            }
            adapt = new SqlDataAdapter(myCmd);
            adapt.Fill(ds, TableName);
            return ds.Tables[TableName];

        }
        catch( Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
        finally
        {
           if (myCmd.Connection.State == ConnectionState.Open)
            {
                myCmd.Connection.Close();
            }
        }
        }
    /// <summary>
    /// 执行存储过程语句，返回 SqlCommand 对象
    /// </summary>
    /// <param name="strProcName">存储过程名称</param>
    /// <returns>返回command</returns>
    public SqlCommand GetCommandProc(string strProcName)
    {
        SqlConnection myConn=GetConnection();
        SqlCommand myCmd=new SqlCommand();
        myCmd.Connection=myConn;
        myCmd.CommandText = strProcName;
        myCmd.CommandType = CommandType.StoredProcedure;
        return myCmd;
    }
    /// <summary>
    /// 根据字符串得到sqlCommand 类对象
    /// </summary>
    /// <param name="strSql"></param>
    /// <returns></returns>
    public SqlCommand GetCommandStr(string strSql)
    {
        SqlConnection myConn = GetConnection();
        SqlCommand myCmd = new SqlCommand();
        myCmd.Connection = myConn;
        myCmd.CommandText = strSql;
        myCmd.CommandType = CommandType.Text;
        return myCmd;

    }

    public DataTable GetDataSetStr(string sqlStr,string TableName)
    {
        SqlConnection myConn = GetConnection();
        myConn.Open();
        DataSet ds = new DataSet();
        SqlDataAdapter adapt = new SqlDataAdapter(sqlStr, myConn);
        adapt.Fill(ds, TableName);
        myConn.Close();
        return ds.Tables[TableName];
    }


}
}