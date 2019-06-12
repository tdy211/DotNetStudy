using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication6
{
    public partial class Authorization : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                HttpCookie cookie = HttpContext.Current.Request.Cookies["Ukey"];
                string LoginCode="5000";
                if (cookie != null)
                {
                    string AuthString = HttpContext.Current.Request.Cookies["Ukey"].Value;
                    DateTime ExpireTime = HttpContext.Current.Request.Cookies["Ukey"].Expires;

                    if (AuthString != null && AuthString != "")
                    {
                        if (ExpireTime >= DateTime.Now)
                        {
                            bool result = Auth.VerityAuth(AuthString);
                            if (result)
                            {
                                LoginCode = "0000";//登陆正常
                            }
                            else
                            {
                                LoginCode = "1000";//登陆异常（用户信息非法），跳转到登录页面
                            }
                        }
                        else
                        {
                            LoginCode = "2000"; //登录过期，跳转到登录页面
                        }
                    }
                }

                else
                {
                    LoginCode = "4000";//未发现登录信息，跳转到登录页面
                }
                HttpCookie loginCook = new HttpCookie("LogState", LoginCode);
                loginCook.Expires = DateTime.Now.AddMinutes(30);
                Response.Cookies.Add(loginCook);
                

            }
            catch (Exception)
            {
                throw ;
            }
            finally
            {
                Response.End();
            }

        }
    }
}