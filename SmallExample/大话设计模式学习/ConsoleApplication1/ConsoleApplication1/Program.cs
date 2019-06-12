using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Diagnostics;
namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            TempTest test = new TempTest();
             test.write();
             TempTest test2 = test;
             test.write();
            
        }  

    }
    class TempTest
    {
        public void write()
        {
            Console.WriteLine("hello");
        
        }
    
    }

     interface ILogger
    {
         void Log(string data);
    }
     public class Logger
     {
         public string Name { set; get; }
         public void Log(string data)
         {

             if (data != null) Console.WriteLine("hello,world");
         }
     }







}
