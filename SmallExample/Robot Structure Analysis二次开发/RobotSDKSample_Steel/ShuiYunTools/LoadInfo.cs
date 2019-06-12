using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShuiYunTools
{
   public class LoadInfo
    {
        public int id { get; set; }//序号
        public string LoadName { get; set; }//荷载名称
        public double factor { get; set; }
        public string LoadType { get; set; }//荷载类型
        public double LoadValue { get; set; }//荷载大小
        public int ElementNumber { get; set; }//作用在哪个单元号上面
        
    }
   public class caseInfo
   {
       public string CaseName { get; set; }
       public int CaseNumber { get; set; }
   }

}
