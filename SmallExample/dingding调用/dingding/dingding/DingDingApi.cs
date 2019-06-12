using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;



namespace dingding
{
    public class DingDingApi
    {

        //https://oapi.dingtalk.com/gettoken?corpid=ding892914b21488a37b35c2f4657eb6378f&corpsecret=XVP6U8FCqj-9trQ8zDDCgZILXGH6Q4FXu40h4_-_MztCWy4w0KVPaP3ZIoLDuSRJ

        /// <summary>
        /// CorpId
        /// </summary>
        private static string CorpId = "CorpId：ding892914b21488a37b35c2f4657eb6378f ";
        /// <summary>
        /// CorpSecret
        /// </summary>
        private static string CorpSecret = "XVP6U8FCqj-9trQ8zDDCgZILXGH6Q4FXu40h4_-_MztCWy4w0KVPaP3ZIoLDuSRJ";

        private static int AgentID = 167524917;



        private static DateTime GetTime { get; set; }

        //public DingTalkApiService()
        //{
        //    if (string.IsNullOrEmpty(Access_Token))
        //    {
        //        GetAccessToken();//重新发送get请求获得获得access_token值
        //    }
        //}
        public DingDingApi()
        {
            if (string.IsNullOrEmpty(Access_Token))
            {
                Access_Token = GetAccessToken();
            }
        }


        private static string Access_Token { get; set; }

        public string GetAccessToken()
        {
            //根据CorpId和CorpSecret发送get请求获得access_token
            string url = "https://oapi.dingtalk.com/gettoken?corpid=" + CorpId + "&corpsecret=" + CorpSecret;
            //创建请求
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            //GET请求
            request.Method = "GET";
            request.ReadWriteTimeout = 5000;
            Encoding encode = Encoding.UTF8;
            //request.ContentType = "text/html;charset=UTF-8";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, encode);

            //返回内容
            string retString = myStreamReader.ReadToEnd();

            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(retString);
            object o = serializer.Deserialize(new JsonTextReader(sr), typeof(AccessToken));
            AccessToken t = o as AccessToken;

            string m = t.Access_Token;
            //AccessToken entity=JsonHelper
            //string s=    myStreamReader.ReadLineAsync();
            string accessToken = retString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[1].Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries)[1].Replace("\"", "");
            //string[] t = retString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            //string s = t[1];
            //string a = s.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries)[1];
            return accessToken;
        }

        //根据传进来的人获得钉钉的userid

        public string GetDepartment(string depName)
        {
            string url = "https://oapi.dingtalk.com/department/list?access_token=" + Access_Token;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //GET请求
            request.Method = "GET";
            request.ReadWriteTimeout = 5000;
            Encoding encode = Encoding.UTF8;
            //request.ContentType = "text/html;charset=UTF-8";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, encode);

            //返回内容
            string retString = myStreamReader.ReadToEnd();
            //{"errcode":0,"department":[{"createDeptGroup":true,"name":"公司Test","id":1,"autoAddUser":true},{"createDeptGroup":false,"name":"运营部","id":61877829,"autoAddUser":false,"parentid":1},{"createDeptGroup":false,"name":"推广部","id":61933824,"autoAddUser":false,"parentid":1},{"createDeptGroup":false,"name":"生产部","id":61939784,"autoAddUser":false,"parentid":1},{"createDeptGroup":false,"name":"产品部","id":61943823,"autoAddUser":false,"parentid":1},{"createDeptGroup":false,"name":"物流部","id":61985767,"autoAddUser":false,"parentid":1},{"createDeptGroup":false,"name":"设计部","id":62021758,"autoAddUser":false,"parentid":1}],"errmsg":"ok"}
            //var department = new { name = string.Empty, id = 0, autoAddUser = false, parentid =1};
            //var tempEntity = new { errcode = string.Empty, List<department>=null, errmsg=string.Empty };
            DepartmentJson dep = new DepartmentJson();
            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(retString);
            //tempEntity = JsonConvert.DeserializeAnonymousType(retString, tempEntity);
            object o = serializer.Deserialize(new JsonTextReader(sr), typeof(DepartmentJson));
            DepartmentJson t = o as DepartmentJson;

            List<department> s = t.department;

            string m = s.Where(p => p.Name == "运营部").Select(w => w.Name).ToList().ToString();
            var id = s.Where(p => p.Name == "运营部").Select(x => x.id).ToList()[0];


            url = "https://oapi.dingtalk.com/user/simplelist?access_token=" + Access_Token + "&department_id=" + id;

            HttpWebRequest requestNew = (HttpWebRequest)WebRequest.Create(url);
            //GET请求
            requestNew.Method = "GET";
            requestNew.ReadWriteTimeout = 5000;
            //Encoding encode = Encoding.UTF8;
            //request.ContentType = "text/html;charset=UTF-8";
            HttpWebResponse responseNew = (HttpWebResponse)requestNew.GetResponse();
            Stream myResponseStreamNew = responseNew.GetResponseStream();
            StreamReader myStreamReaderNew = new StreamReader(myResponseStreamNew, encode);

            //返回内容
            string retStringNew = myStreamReaderNew.ReadToEnd();

            //{"errcode":0,"errmsg":"ok","userlist":[{"name":"张三","userid":"33333333333333333333333333"},{"name":"李四","userid":"3333333333333333"},{"name":"王五","userid":"333333333333"},{"name":"赵六","userid":"4444444444444"}]}
            UserJson user = new UserJson();
            //JsonSerializer serializer = new JsonSerializer();
            StringReader srNew = new StringReader(retStringNew);
            //tempEntity = JsonConvert.DeserializeAnonymousType(retString, tempEntity);
            object oNew = serializer.Deserialize(new JsonTextReader(srNew), typeof(UserJson));
            UserJson v = oNew as UserJson;

            List<Users> z = v.userlist;


            var q = z.Where(p => p.Name == "李七").Select(i => i.UserId).ToList()[0];
            decimal b = Convert.ToDecimal(q);
            return b + "ddddd";
        }

    }
}