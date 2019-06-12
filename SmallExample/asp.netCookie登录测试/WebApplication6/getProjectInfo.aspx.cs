using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Security.Cryptography;
namespace WebApplication6
{
    public partial class getProjectInfo : System.Web.UI.Page
    {
        DBClass dbObj = new DBClass();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {    /*
                string sqlStr = "select * from projectBasicData";
                DataTable dsTable = dbObj.GetDataSetStr(sqlStr, "tbClass");
                List<ProjectData> root = DataTableToList(dsTable);
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(root);
                //Response.Write(json);
                */
                /*
                1.获取用户名
                2.用户名生成sha256
                3. 用户名和对应的sha256,进行签名
                4.发送cookie
               */
                string UserName = "abc";//获取用户名字符串
                string UidEncode = Auth.SetAuth(UserName);
                HttpCookie Cook1 = new HttpCookie("Ukey", UidEncode+","+UserName);//加密字符串和用户名装入Cookie
                Cook1.Expires = DateTime.Now.AddMinutes(10);
                Response.Cookies.Add(Cook1);


               
                //故意修改数据（将源数据的5修改成4）
                //signedData[(hmac.HashSize >> 3) + 4] = 4;
                //输出数据
                //PrintData(signedData, hmac);
                //认证
                //Console.WriteLine(VerityData(key, signedData, hmac) ? "数据正确" : "数据已被修改");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Response.End();
            }

        }
        public List<ProjectData> DataTableToList(DataTable dt)
        {

            ArrayList Arraywatershad = new ArrayList();//流域列表
            ArrayList Arrayregion = new ArrayList();//上中下游列表
            ArrayList Arrayproject = new ArrayList();//项目名称列表
            var dtCamera = dt.AsEnumerable();
            //通过linq获取流域列表
            var queryShed = from p in dtCamera
                            select p.Field<string>("waterShad");
            foreach (var shed in queryShed)
            {
                if (!Arraywatershad.Contains(shed))
                {

                    Arraywatershad.Add(shed);
                }

            }

            //通过linq获取上下游列表
            var queryregion = from p in dtCamera
                              select p.Field<string>("region");
            foreach (var regin in queryregion)
            {
                if (!Arrayregion.Contains(regin))
                {
                    Arrayregion.Add(regin);
                }

            }
            //通过linq获取//项目列表
            var queryproject = from p in dtCamera
                               select p.Field<string>("projectName");
            foreach (var project in queryproject)
            {
                if (!Arrayproject.Contains(project))
                {
                    Arrayproject.Add(project);
                }

            }

            List<ProjectData> _projects = new List<ProjectData>();
            //遍历摄像头信息，按照项目分类汇总
            foreach (string project in Arrayproject)//项目列表
            {
                ProjectData _prj = new ProjectData();
                _prj.label = project.ToString();
                _prj.children = new List<ProjectData>();

                var qurey =
                    from p in dtCamera
                    where p.Field<string>("projectName") == project.ToString()
                    select p;
                foreach (var row in qurey)
                {
                    ProjectData tempDetail = new ProjectData();
                    tempDetail.label = row.Field<string>("projectName");
                    tempDetail.attPath = row.Field<string>("AttUrl");
                    tempDetail.projectPart = row.Field<string>("projectPart");
                    tempDetail.introduction = row.Field<string>("projectInstr");
                    tempDetail.isDetail = true;
                    _prj.children.Add(tempDetail);
                }
                _projects.Add(_prj);
            }
            //按上下游分类汇总
            List<ProjectData> _reginons = new List<ProjectData>();
            foreach (string region in Arrayregion)//上下游列表
            {
                ProjectData _rgCamera = new ProjectData();
                _rgCamera.label = region.ToString();
                _rgCamera.children = new List<ProjectData>();

                var qurey =
                    from p in dtCamera
                    where p.Field<string>("region") == region.ToString()
                    select p.Field<string>("projectName");//获得区域下的项目
                ArrayList pjlist = new ArrayList();
                foreach (var project in qurey)
                {
                    pjlist.Add(project);
                }

                foreach (ProjectData prj in _projects)
                {
                    if (pjlist.Contains(prj.label))
                    {
                        _rgCamera.children.Add(prj);
                    }
                }

                _reginons.Add(_rgCamera);
                //reginons.Add(rgCamera);
            }

            //按流域分类汇总
            List<ProjectData> _watershads = new List<ProjectData>();
            foreach (string shad in Arraywatershad)//流域列表
            {
                ProjectData _chCamera = new ProjectData();
                _chCamera.label = shad.ToString();
                _chCamera.children = new List<ProjectData>();

                //获取流域下的区域
                var qurey =
                   from p in dtCamera
                   where p.Field<string>("waterShad") == shad.ToString()
                   select p.Field<string>("region");
                ArrayList rglist = new ArrayList();
                foreach (var rg in qurey)
                {
                    rglist.Add(rg);
                }
                //便利区域list,配对

                foreach (ProjectData reg in _reginons)
                {
                    if (rglist.Contains(reg.label))
                    {
                        _chCamera.children.Add(reg);
                    }

                }
                _watershads.Add(_chCamera);
            }
            return _watershads;

        }
    }
    public class ProjectData
    {
        public string label;//名称
        public string attPath;
        public string projectPart;
        public string introduction;
        public bool isDetail = false;
        public List<ProjectData> children;
    }
}