using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Family MyFamily = new Family();
            foreach(string str in MyFamily)
            {
                richTextBox1.Text += str + "\n";
            }
            InitDatable();

        }
        private DataTable dt = new DataTable();

        private void InitDatable()
        {
            //新建列
            DataColumn col1 = new DataColumn("姓名", typeof(string));
            DataColumn col2 = new DataColumn("性别", typeof(string));
            DataColumn col3 = new DataColumn("出生日期", typeof(string));
            DataColumn col4 = new DataColumn("身份证号码", typeof(string));
            DataColumn col5 = new DataColumn("出生地址", typeof(string));
            DataColumn col6 = new DataColumn("户籍地址", typeof(string));
            DataColumn col7 = new DataColumn("实际居住地址", typeof(string));
            DataColumn col8 = new DataColumn("ID", typeof(string));
            //添加列
            dt.Columns.Add(col1);
            dt.Columns.Add(col2);
            dt.Columns.Add(col3);
            dt.Columns.Add(col4);
            dt.Columns.Add(col5);
            dt.Columns.Add(col6);
            dt.Columns.Add(col7);
            dt.Columns.Add(col8);

            this.dataGridView1.DataSource = dt.DefaultView;
        }
        private void button1_Click(object sender, EventArgs e)//保存
        {
            if(textBox1.Text==string.Empty)
            {
                MessageBox.Show("写入的文件内容不能为空");

            }else
            {
                saveFileDialog1.Filter = "二进制文件(*.dat)|*.dat";
                if(saveFileDialog1.ShowDialog()==DialogResult.OK)
                {
                    System.IO.FileStream myStream = new System.IO.FileStream(saveFileDialog1.FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    BinaryWriter myWriter=new BinaryWriter(myStream);
                    myWriter.Write(textBox1.Text);
                    myWriter.Close();
                    myStream.Close();
                    textBox1.Text=string.Empty;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)//打开
        {
            openFileDialog1.Filter="二进制文件(*.dat)|*.dat";
            if(openFileDialog1.ShowDialog()==DialogResult.OK)
            {
                textBox1.Text=string.Empty;
                FileStream myStream=new FileStream(openFileDialog1.FileName,FileMode.Open,FileAccess.Read);
                BinaryReader myReader=new BinaryReader(myStream);
                if(myReader.PeekChar()!=-1)
                {
                    textBox1.Text=Convert.ToString(myReader.ReadInt32());

                }
                myReader.Close();
                myStream.Close();
            }
        
        }
    }
    public class Family:System.Collections.IEnumerable 
    {
        string[] MyFamily = { "父亲", "母亲", "弟弟", "妹妹" };
        public System.Collections.IEnumerator GetEnumerator()
        {
        for(int i=0;i<MyFamily.Length;i++)
       {  
            yield return MyFamily[i];
       }

       }
    }
}
