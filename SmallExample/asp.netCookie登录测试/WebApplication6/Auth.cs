using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;

namespace WebApplication6
{
    public class Auth
    {
       

        public static string SetAuth(string userName)
        {
            byte[] uid = System.Text.Encoding.Default.GetBytes(userName);//将用户名字符串转成byte[]
            SHA256 mySHA256 = SHA256.Create();
            byte[] key = mySHA256.ComputeHash(uid);//将用户名进行加密SHA256
            string uidEncode = BitConverter.ToString(key);//得到加密后的字符串
            return uidEncode;
        }
        public static bool VerityAuth(string token)
        {
            string[] tokenArray = token.Split(',');
            string tokenKey = tokenArray[0];
            string tokenUname = tokenArray[1];
            SHA256 VerifySHA256 = SHA256.Create();
            byte[] Verifykey = VerifySHA256.ComputeHash(System.Text.Encoding.Default.GetBytes(tokenUname));//将用户名进行加密SHA256
            string VerifyName = BitConverter.ToString(Verifykey);//得到加密后的字符串
            bool VerifyResult = (VerifyName == tokenKey);
            return VerifyResult;
            
            
        }
        
    }
}