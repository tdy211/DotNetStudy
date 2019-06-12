using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace ConsoleApplicationdll
{
    class Program
    {
        static void Main(string[] args)
        {
            usedll.Ng_Init();
            usedll.Ng_Exit();
            Console.Write("ok");
        }
    }
    public class usedll
    {
        [DllImport("nglib.dll", CharSet = CharSet.Unicode, EntryPoint = "?xNg_Exit@nglib@@YAXXZ")]
        public static extern void Ng_Exit();
        //[DllImport("nglib.dll", CharSet = CharSet.Unicode, EntryPoint = "?xNg_Init@nglib@@YAXXZ")]
        [DllImport("nglib.dll", CharSet = CharSet.Unicode, EntryPoint = "getfun")]
        public static extern void Ng_Init();
        
    }
}
