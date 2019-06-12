using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Autodesk.AutoCAD;
using Autodesk.AutoCAD.Interop;
using Autodesk.AutoCAD.Interop.Common;
namespace cadPlugIn
{
    public partial class Form1 : Form
    {
        private AcadApplication a;
        public Form1()
        {
            a = new AcadApplicationClass();
            a.Visible = true;//使AutoCAD可见

            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double[] startPoint = new double[3]; //声明直线起点坐标
            double[] endPoint = new double[3];//声明直线终点坐标
            string[] str = textBox1.Text.Split(',');//取出直线起点坐标输入文本框的值，文本框的输入模式为＂x,y,z＂
            for (int i = 0; i < 3; i++)
                startPoint[i] = Convert.ToDouble(str[i]);//将str数组转为double型 
            str = textBox2.Text.Split(',');//取出直线终点坐标输入文本框的值
            for (int i = 0; i < 3; i++)
                endPoint[i] = Convert.ToDouble(str[i]);
            a.ActiveDocument.ModelSpace.AddLine(startPoint, endPoint);//在AutoCAD中画直线
            a.Application.Update();//更新显示

        }
    }
}
/*
（1）Visual Studio .net (2003和2002都可以，我用的是2002）

（2）AutoCAD2000以上版本（我用的是2004）
　　这个例子非常简单，就是通过C#建立的窗体来启动AutoCAD并画一条直线。下面是编程的具体步骤：
（1）通过Visual Studio .net 建立一C#的windows应用程序。
（2）在“解决方案资源管理器”中右击“引用”标签，在弹出的菜单中选择“添加引用”，在“添加引用”对话框中选择“com"选项卡下的下拉列表框中的“AutoCAD 2004 Type Library"项（注意：不同版本的CAD的数字不同），单击右边的“选择”按钮，最后单击下面的“确定”按钮。
（3）在C#窗体中加入两个文本框和一个按钮，分别用于输入直线起点、终点的坐标和在CAD中画直线。下面主要解释一下添加的代码。
（a)在程序的开头加入：using AutoCAD;//导入AutoCAD引用空间
(b)在窗体的变量声明部分加入: private AcadApplication a;//声明AutoCAD对象
(c)在窗体的构造函数部分加入：a=new AcadApplicationClass();//创建AutoCAD对象
a.Visible=true;//使AutoCAD可见
(d)在按钮的消息处理函数中加入：
double[] startPoint=new double[3]; //声明直线起点坐标
double[] endPoint=new double[3];//声明直线终点坐标
string[] str=textBox1.Text.Split(',');//取出直线起点坐标输入文本框的值，文本框的输入模式为＂x,y,z＂
for(int i=0;i<3;i++)
startPoint[i]=Convert.ToDouble(str[i]);//将str数组转为double型 
　　　 str=textBox2.Text.Split(',');//取出直线终点坐标输入文本框的值
for(int i=0;i<3;i++)
endPoint[i]=Convert.ToDouble(str[i]);
a.ActiveDocument.ModelSpace.AddLine(startPoint,endPoint);//在AutoCAD中画直线
　　 a.Application.Update();//更新显示
好了，简单吧，你可以试着编译一下。关于上面一些语句的用法，我会在下一讲中作详细介绍。

*/