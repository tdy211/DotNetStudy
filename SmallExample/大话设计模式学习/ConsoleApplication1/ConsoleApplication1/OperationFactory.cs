///大话设计模式
///工厂模式
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    /// <summary>
    /// 父类
    /// </summary>
     class Operation
    {
        public double NumberA { get; set; }
        public double NumberB { get; set; }
        public virtual double GetResult()
        {
            double result = 0;
            return result;
        }
    }

    /// <summary>
    /// 子类
    /// </summary>
     class OperationAdd : Operation
     {
         public override double GetResult()
         {
             double result = NumberA + NumberB;
             return result;
         }
     }
    /// <summary>
    /// 子类
    /// </summary>
     class OperationSub : Operation
     {
         public override double GetResult()
         {
             double result = NumberA - NumberB;
             return result;
         }
     }
     class OperationMul : Operation
     {
         public override double GetResult()
         {
             double result = NumberA * NumberB;
             return result;
         }
     }
    /// <summary>
    /// 子类
    /// </summary>
     class OperationDiv : Operation
     {
         public override double GetResult()
         {
             if (NumberB==0)
             {
                 throw new Exception("除数不能为0");
             }
             double result = NumberA / NumberB;
             return result;
         }
     }
    /// <summary>
    /// 工厂类
    /// </summary>
     class OperationFactory
    {
        public static Operation CreateOperate(string operate)
        {
            Operation oper = null;
            switch (operate)
            {
                case "+":
                    oper = new OperationAdd();
                    break;
                case "-":
                    oper = new OperationSub();
                    break;
                case "*":
                    oper = new OperationMul();
                    break;
                case "/":
                    oper = new OperationDiv();
                    break;
            }
            return oper;

        }
    }
    //客户端代码（使用工厂模式）
    public class UseFactory
    {/*
        public static void Test()
        {
            Operation oper;
            oper = OperationFactory.CreateOperate("+");
            oper.NumberA = 1;
            oper.NumberB = 2;
            double result = oper.GetResult();
        
        }
      */
    }

    
}
