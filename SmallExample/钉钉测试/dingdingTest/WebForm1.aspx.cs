using DingTalk.Api;
using DingTalk.Api.Request;
using DingTalk.Api.Response;
using Newtonsoft.Json;
using HttpSender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace dingdingTest
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //1,获取ucml端发送人信息（姓名，对应钉钉userID,部门ID）

            //2,获取钉钉的AccessToken
            string CorpId = "ding6756aeeb5450e16935c2f4657eb6378f";
            string CorpSecret = "ZiSJ4vH63JBbOYmv-qoyrwRftZydIiIqV4dXdCjfFUh5UFpNrt17pVHisHcXVIHL";
            string AccessToken = "";
            string AccessUrl = string.Format("https://oapi.dingtalk.com/gettoken?corpid={0}&corpsecret={1}", CorpId, CorpSecret);
            Newtonsoft.Json.Linq.JToken json = Newtonsoft.Json.Linq.JToken.Parse(HttpSender.Sender.Get(AccessUrl));
            AccessToken = json["access_token"].ToString();
            /*
            //3,发送信息
            DingTalk.Api.IDingTalkClient client = new DefaultDingTalkClient("https://eco.taobao.com/router/rest");
            DingTalk.Api.Request.CorpMessageCorpconversationAsyncsendRequest req = new DingTalk.Api.Request.CorpMessageCorpconversationAsyncsendRequest();
            req.Msgtype = "oa";//发送消息是以oa的形式发送的,其他的还有text,image等形式
            req.AgentId = 194755687;//微应用ID
            req.UseridList = "0753662464750225";//收信息的userId,这个是by公司来区分，在该公司内这是一个唯一标识符
            req.DeptIdList = "82249101";//部门ID
            req.ToAllUser = false;//是否发给所有人
            req.Msgcontent = "{\"message_url\": \"http://dingtalk.com\",\"head\": {\"bgcolor\": \"FFBBBBBB\",\"text\": \"八堡船闸PC版流程提示\"},\"body\": {\"title\": \"测试文本\",\"form\": [{\"key\": \"姓名:\",\"value\": \"张三\"},{\"key\": \"爱好:\",\"value\": \"打球、听音乐\"}],\"rich\": {\"num\": \"15.6\",\"unit\": \"元\"},\"content\": \"大段文本大段文本大段文本大段文本大段文本大段文本大段文本大段文本大段文本大段文本大段文本大段文本\"}}";
            DingTalk.Api.Response.CorpMessageCorpconversationAsyncsendResponse rsp = client.Execute(req, AccessToken);//发送消息
            */
            //查看信息发送情况
            DingTalk.Api.IDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/topapi/message/corpconversation/getsendresult");
            OapiMessageCorpconversationGetsendresultRequest request = new OapiMessageCorpconversationGetsendresultRequest();
            request.AgentId = 194755687;
            //request.TaskId = "";
            OapiMessageCorpconversationGetsendresultResponse response = client.Execute(request, AccessToken);
            Label1.Text = response.ToString();
            

        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Label1.Text = System.DateTime.Now.ToString();
        }
    }
}