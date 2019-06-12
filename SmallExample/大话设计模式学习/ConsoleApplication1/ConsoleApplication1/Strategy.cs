///策略模式
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ConsoleApplication1
{
    /// <summary>
    /// 上下文类
    /// </summary>
    class CashContext
    {
        private CashSuper cs;
        public CashContext( string type)
        {
            switch (type)
            { 
                case "无折扣":
                    cs = new CashNormal();
                    break;
                case"满200减20":
                    cs = new CashReturn("200","20");
                    break;
                case"打8折":
                    cs = new CashRebate(0.8);
                    break;
            }

        }
        public double GetResult(double money)
        {
            return cs.AcceptCash(money);
        }
    }
    /// <summary>
    /// 打折策略的父类
    /// </summary>
     abstract class CashSuper
    {
        public abstract double AcceptCash(double money);
    }
    /// <summary>
    /// 子类，正常，不打折
    /// </summary>
    class CashNormal : CashSuper
    {
        public override double AcceptCash(double money)
        {
            return money;
        }
    }
    /// <summary>
    /// 子类，打某个折扣
    /// </summary>
    class CashRebate:CashSuper
    {
        private double moneyRebate = 1d;//d表示double
        public CashRebate(double rebate)
        {
            this.moneyRebate = rebate;
        }
        public override double AcceptCash(double money)
        {
            return money * moneyRebate;
        }
    
    }
    /// <summary>
    /// 子类，满多少返多少
    /// </summary>
    class CashReturn : CashSuper
    {
        private double moneyCondition=0.0d;
        private double moneyReturn = 0.0d;
        public  CashReturn(string moneyCondition, string moneyReturn)
        {
            this.moneyCondition = double.Parse(moneyCondition);
            this.moneyCondition = double.Parse(moneyReturn);

        }
        public override double AcceptCash(double money)
        {
            double result = money;
            if (money >= moneyCondition)
                result = money - Math.Floor(money/moneyCondition)*moneyReturn;
            return result;
        }
    }

    class Strategy
    {/*
      //使用打折模式给客户端
        public double SumMoney = 0;
        private void btnOK_Click(object sender,EventArgs e)
        {
            string strage = "打8折";
            double price = 1.5;
            int counts = 53;
            CashContext context = new CashContext(strage);
           double result= context.GetResult(price*counts);
           SumMoney=SumMoney+result;
        }
      */

    }
}
