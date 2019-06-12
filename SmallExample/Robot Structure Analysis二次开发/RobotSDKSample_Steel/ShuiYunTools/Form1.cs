using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RobotOM;
using System.Linq;

namespace ShuiYunTools
{
    public partial class Form1 : Form
    {
        //引用Robot程序类，创建一个实例 iapp
        private IRobotApplication iapp;
        //定义几个常数
        private int startBar = 1, startNode = 1, beam1 = 2, beam2 = 3;
        //定义初始状态，几何体创建状态，荷载创建状态
        private bool geometryCreated = false, loadsGenerated = false;
        //构造函数
        
        public Form1()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 去除字符串数组中的空值
        /// </summary>
        /// <param name="arrayString"></param>
        /// <returns></returns>
        private string[] RemoveNullElement(string[] arrayString)
        {
            List<string> res = new List<string>();
            foreach(string element in arrayString)
            {
                if(!string.IsNullOrEmpty(element))
                {
                    res.Add(element);
                }
            }
            return res.ToArray();
        }
        /// <summary>
        /// 获取荷载列表
        /// </summary>
        /// <returns></returns>
        private string[] GetCaseInput()
        {
            int rowCounts = dataGridViewInPut.Rows.Count;
            string[] result=new string[rowCounts];
            for (int i = 0; i < rowCounts - 1; i++)
            {
                result[i] = dataGridViewInPut.Rows[i].Cells[0].Value.ToString();
            }
            return result;
        }
        /// <summary>
        /// 获取荷载表的全部内容
        /// </summary>
        /// <returns></returns>
        private List<LoadInfo> GetLoadInfo()
        {
            List<LoadInfo> result = new List<LoadInfo>();
            int rowCounts = dataGridViewInPut.Rows.Count;
            for (int i = 0; i < rowCounts - 1; i++)
            {
                if(!string.IsNullOrEmpty(dataGridViewInPut.Rows[i].Cells[0].Value.ToString()))
                {
                LoadInfo temp = new LoadInfo();
                temp.id = i;
                temp.LoadName=dataGridViewInPut.Rows[i].Cells[0].Value.ToString();
                temp.factor = double.Parse(dataGridViewInPut.Rows[i].Cells[1].Value.ToString());
                temp.LoadType=dataGridViewInPut.Rows[i].Cells[2].Value.ToString();
                temp.ElementNumber =int.Parse( dataGridViewInPut.Rows[i].Cells[3].Value.ToString());
                temp.LoadValue =double.Parse( dataGridViewInPut.Rows[i].Cells[4].Value.ToString());
                result.Add(temp);
                }  
            }
            return result;
        }
        private string[] GetAllLoadCombines()
        {
            int rowCounts = dataGridViewOutPut.Rows.Count;
            string[] result = new string[rowCounts];
            for (int i = 0; i < rowCounts - 1; i++)
            {
                if (!string.IsNullOrEmpty(dataGridViewOutPut.Rows[i].Cells[0].Value.ToString()))
                {
                    result[i] = dataGridViewOutPut.Rows[i].Cells[0].Value.ToString();
                }
            }
            return result;
        
        }
        //根据荷载名称得到荷载对象
        private LoadInfo getLoadFromName(string name)
        {
            List<LoadInfo> loads= GetLoadInfo();
            var selectedItem = from item in loads
                                where item.LoadName == name
                                select item;
            return selectedItem.First();
        }
        //根据工况名称得到工况编号
        private int getCaseNumber(List<caseInfo> caseList,string name)
        {
            var caseNumber = from item in caseList
                             where item.CaseName == name
                             select item;
            return caseNumber.First().CaseNumber;
            
        
        }
        /// <summary>
        /// 添加冲突荷载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            //添加冲突荷载
            string conflickLoad1 = comboBox1.SelectedItem.ToString();
            string conflickLoad2 = comboBox2.SelectedItem.ToString();
            int index=dataGridViewConflick.Rows.Add();
            dataGridViewConflick.Rows[index].Cells[0].Value = conflickLoad1;
            dataGridViewConflick.Rows[index].Cells[1].Value = conflickLoad2;
        }
        /// <summary>
        /// 冲突荷载Tab页面的初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl1_Click(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            string[] loads = GetCaseInput();
            string[] loadsWithoutNull = RemoveNullElement(loads);
            foreach (var load in loadsWithoutNull)
            {
                comboBox1.Items.Add(load.ToString());
                comboBox2.Items.Add(load.ToString());
            }

        }
        /// <summary>
        /// 获取冲突荷载的列表
        /// </summary>
        /// <returns></returns>
        private List<string[]> GetConflickLoads()
        {
            List<string[]> result = new List<string[]>();
            for (int i = 0; i < dataGridViewConflick.Rows.Count-1; i++)
            {
                string[] loadPair = new string[2];
                loadPair[0] = dataGridViewConflick.Rows[i].Cells[0].Value.ToString();
                loadPair[1] = dataGridViewConflick.Rows[i].Cells[1].Value.ToString();
                result.Add(loadPair);
            }
            return result;
        }
        /// <summary>
        /// 进行荷载组合，考虑冲突荷载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click_1(object sender, EventArgs e)
        {
            //第一步，从元素表格里面获取所有元素，组成数组
            string[] elementArr = GetCaseInput(); //数组
            string[] elementArrWithNotNullElement = RemoveNullElement(elementArr);
            for (int i = 0; i < elementArrWithNotNullElement.Length; i++)
            {
                //求元素数组的所有组合
                List<string[]> ListCombination = PermutationAndCombination<string>.GetCombination(elementArrWithNotNullElement, i + 1); //第一个参数是数据源，第二个参数是从中选取的元素数量
                //将结果输出到结果表格中去
                foreach (var arr in ListCombination)
                {
                    if (InvalidLoadCombine(arr))
                    {
                        string result = string.Empty;
                        result = string.Join("+", arr);//数组转成字符串
                        int index = dataGridViewOutPut.Rows.Add();
                        dataGridViewOutPut.Rows[index].Cells[0].Value = result;
                    }
                }
            }
            //获取本项目的荷载容器
            IRobotCaseServer icases = (IRobotCaseServer)iapp.Project.Structure.Cases;
            //获取可用荷载编号
            int c1 = icases.FreeNumber;
            List<LoadInfo> loadList = new List<LoadInfo>();
            loadList = GetLoadInfo();
            List<caseInfo> caseList = new List<caseInfo>();

            for(int i=0;i<loadList.Count;i++)
            {
                //创建一个荷载(I_LRT_BAR_UNIFORM)
                //IRobotSimpleCase isc1 = (IRobotSimpleCase)icases.CreateSimple(c1+i, loadList[i].LoadName, IRobotCaseNature.I_CN_ACCIDENTAL, IRobotCaseAnalizeType.I_CAT_STATIC_LINEAR);
                IRobotSimpleCase isc1 = (IRobotSimpleCase)icases.CreateSimple(c1 + i, loadList[i].LoadName, IRobotCaseNature.I_CN_ACCIDENTAL, IRobotCaseAnalizeType.I_CAT_STATIC_LINEAR);
                //给荷载赋值
                BeamTools.create_concentrated_load(isc1, loadList[i].ElementNumber, 0.5, loadList[i].LoadValue);
                caseInfo temp = new caseInfo();
                temp.CaseName = loadList[i].LoadName;
                temp.CaseNumber = c1 + i;
                caseList.Add(temp);
                
            }

            string[] allCombines = GetAllLoadCombines();
            for (int i = 0; i < allCombines.Length; i++)
            {
                //添加荷载组合
                if (!string.IsNullOrEmpty(allCombines[i]))
                {
                    IRobotCaseCombination icc = (IRobotCaseCombination)icases.CreateCombination(i+c1 + loadList.Count + 1, allCombines[i], IRobotCombinationType.I_CBT_ULS, IRobotCaseNature.I_CN_EXPLOATATION, IRobotCaseAnalizeType.I_CAT_COMB);
                    string[] loadsArray = allCombines[i].Split('+');
                    for (int j = 0; j < loadsArray.Length; j++)
                    {
                        int caseNumber = getCaseNumber(caseList, loadsArray[j]);
                        icc.CaseFactors.New(caseNumber, 1);
                    }
                }
            }
            //
            loadsGenerated = true;
        }
        /// <summary>
        /// 使用冲突荷载对进行荷载组合过滤
        /// </summary>
        /// <param name="inputCombine"></param>
        /// <returns></returns>
        private bool InvalidLoadCombine(string[] inputCombine)
        {
            bool result = true;
            List<string[]> ConflickLoads = GetConflickLoads();//获取冲突荷载对
            foreach (string[] loadPair in ConflickLoads)
            {
                bool b = inputCombine.Contains(loadPair[0]) && inputCombine.Contains(loadPair[1]);
                if (b) result = false;
            }
            return result;
        }
        /// <summary>
        /// 增加datagridView的表头编号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewOutPut_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            for (int i = 0; i < dataGridViewOutPut.Rows.Count; i++)
            {
                dataGridViewOutPut.Rows[i].HeaderCell.Value = (i + 1).ToString();
            }

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }
     
        private void tabPage3_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 创建连续梁模型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
           //截面宽度，高度
            double sectionWidth = double.Parse(txtSectionWdith.Text);
            double sectionHeight = double.Parse(txtSectionHeight.Text);
            //材料
           // string material=comboBoxMaterial.SelectedItem.ToString();
            //几何跨数，单跨长度
            int elementAmount=int.Parse(comboBoxElementCount.SelectedItem.ToString());
            double elementLength=double.Parse(textBoxElementLength.Text);
            //-----------------------------------------------------------------------
            //关闭和ROBOT的交互功能
            iapp.Interactive = 0;
            //创建一个Frame2d类型的项目
            iapp.Project.New(IRobotProjectType.I_PT_FRAME_2D);
            //创建节点
            IRobotNodeServer inds = iapp.Project.Structure.Nodes;//获取节点集合
            int n1 = startNode;
            inds.Create(n1 , 0, 0, 0);
            for (int i = 1; i < elementAmount+1; i++)//创建节点，格式为节点号，x坐标，y坐标，z坐标
            {
                inds.Create(i+1, i * elementLength, 0, 0);
            }
            //创建杆单元
            IRobotBarServer ibars = iapp.Project.Structure.Bars;//获取杆单元集合
            int b1 = startBar;
            for (int i = 1; i < elementAmount + 1; i++)
            {
                ibars.Create(i, i, i+1);//创建杆单元，格式为单元号,单元起始节点，单元结束节点
            }

            //设置材料和截面
            //给量赋值截面
            RobotLabelServer labels;
            labels = iapp.Project.Structure.Labels;
            string ColumnSectionName = "Rect. Column 60*100";
            IRobotLabel label = labels.Create(IRobotLabelType.I_LT_BAR_SECTION, ColumnSectionName);
            RobotBarSectionData section;
            section = (RobotBarSectionData)label.Data;
            section.ShapeType = IRobotBarSectionShapeType.I_BSST_CONCR_COL_R;
            RobotBarSectionConcreteData concrete;
            concrete = (RobotBarSectionConcreteData)section.Concrete;
            concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_B, 0.6);
            concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_H, 1.0);
            section.CalcNonstdGeometry();
            labels.Store(label);
            RobotSelection selectionBars;
            selectionBars = iapp.Project.Structure.Selections.Get(IRobotObjectType.I_OT_BAR);
            //selectionBars.FromText("1 2 3 4 5");
            selectionBars.AddOne(1);
            selectionBars.AddOne(2);
            selectionBars.AddOne(3);
            selectionBars.AddOne(4);
            selectionBars.AddOne(5);
            //给量赋值截面
            string MaterialName = "Concrete 30";
            label = labels.Create(IRobotLabelType.I_LT_MATERIAL, MaterialName);
            RobotMaterialData Material;
            Material = (RobotMaterialData)label.Data;
            Material.Type = IRobotMaterialType.I_MT_CONCRETE;
            Material.E = 30000000000; // Young
            Material.NU = 1 / 6; // Poisson
            Material.RO = 25000; // Unit weight
            Material.Kirchoff = Material.E / (2 * (1 + Material.NU));
            iapp.Project.Structure.Labels.Store(label);
           
            RobotSelection isel = iapp.Project.Structure.Selections.Create(IRobotObjectType.I_OT_BAR);
            isel.AddOne(1);
            isel.AddOne(2);
            isel.AddOne(3);
            isel.AddOne(4);
            isel.AddOne(5);
            ibars.SetLabel(isel, IRobotLabelType.I_LT_BAR_SECTION, ColumnSectionName);
            //ibars.SetLabel(isel, IRobotLabelType.I_LT_MATERIAL, MaterialName);
            //------------------------------------------------
            //支撑
            //给节点设置支持形式
            IRobotNode ind = (IRobotNode)inds.Get(1);
            ind.SetLabel(IRobotLabelType.I_LT_SUPPORT, "固定");
            //ind.SetLabel(IRobotLabelType.I_LT_SUPPORT, comboSupportLeft.Text);
            geometryCreated = true;//结构几何创建结束

            loadsGenerated = false;//结构荷载没有创建

            // switch Interactive flag on to allow user to work with Robot GUI
            //打开robot截面操作
            iapp.Interactive = 1;

            // get the focus back
            this.Activate();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // connect to Robot
            //实例化一个ROBOT程序类
            iapp = new RobotApplicationClass();

            if (iapp.Project.IsActive == 0)
            {
                // we need an opened project to get names of available supports and bar sections
                // create a new project if not connected to existing one
                //创建一个2D框架的工程
                iapp.Project.New(IRobotProjectType.I_PT_FRAME_2D);

            }
            for (int i = 0; i < 5; i++)
            {
                
                int index = dataGridViewInPut.Rows.Add();
                dataGridViewInPut.Rows[index].Cells[0].Value ="第"+(i+1).ToString()+"跨均布荷载";
                dataGridViewInPut.Rows[index].Cells[1].Value = 1;
                dataGridViewInPut.Rows[index].Cells[2].Value ="均布荷载";
                dataGridViewInPut.Rows[index].Cells[3].Value = (i + 1).ToString() ;
                dataGridViewInPut.Rows[index].Cells[4].Value = -10000; 

            }
        }
        /// <summary>
        /// 有限元计算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            if (geometryCreated && loadsGenerated)
            {

                if (iapp.Project.Structure.Results.Available == 0)//计算结果不可用
                {
                    iapp.Project.CalcEngine.Calculate();//进行结构计算
                }
            }
            else
            {
                MessageBox.Show("先建立集合模型和荷载组合");
            }
        }
        //查看计算结果
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (tabControl1.SelectedTab == tabPage5)//进行tabpage位置判断  
            {
                // fill combo-box with names of all load cases defined in the structure
                comboBoxLoadCombine.Items.Clear();
                IRobotCollection icases = iapp.Project.Structure.Cases.GetAll();
                for (int i = 1; i <= icases.Count; ++i)
                {
                    IRobotCase ic = (IRobotCase)icases.Get(i);
                    int idx = comboBoxLoadCombine.Items.Add(ic.Name);
                }
                // select the first item
                if (comboBoxLoadCombine.Items.Count > 0)
                {
                    comboBoxLoadCombine.SelectedIndex = 0;
                }


                if (iapp.Project.Structure.Results.Available == 0)//计算结果不可用
                {
                    iapp.Project.CalcEngine.Calculate();//进行结构计算
                    
                }
                //清除表格
                dataGridView1.Rows.Clear();
                int index = comboBoxLoadCombine.SelectedIndex + 1;//当前选择的工况索引
                IRobotCase icc = (IRobotCase)iapp.Project.Structure.Cases.GetAll().Get(index);//根据下拉框选择当前工况
                int case_num = icc.Number;

                for (int n = 0; n < 5; ++n)
                {
                    int indexData = dataGridView1.Rows.Add();
                    dataGridView1.Rows[indexData].Cells[0].Value = n+1;
                    dataGridView1.Rows[indexData].Cells[1].Value = (iapp.Project.Structure.Results.Bars.Forces.Value(n + 1, case_num, 0).MY / 1000).ToString("####0.00");
                    dataGridView1.Rows[indexData].Cells[2].Value = (iapp.Project.Structure.Results.Bars.Forces.Value(n + 1, case_num, 0.5).MY / 1000).ToString("####0.00");
                    dataGridView1.Rows[indexData].Cells[3].Value = (iapp.Project.Structure.Results.Bars.Forces.Value(n + 1, case_num, 1).MY / 1000).ToString("####0.00");
                    dataGridView1.Rows[indexData].Cells[4].Value = (iapp.Project.Structure.Results.Bars.Forces.Value(n + 1, case_num, 0).FZ / 1000).ToString("####0.00");
                    dataGridView1.Rows[indexData].Cells[5].Value = (iapp.Project.Structure.Results.Bars.Forces.Value(n + 1, case_num, 0.5).FZ / 1000).ToString("####0.00");
                    dataGridView1.Rows[indexData].Cells[6].Value = (iapp.Project.Structure.Results.Bars.Forces.Value(n + 1, case_num, 1).FZ / 1000).ToString("####0.00");
                    

                }
            }  

        }
        private void GetResult()
        {
            if (iapp.Project.Structure.Results.Available == 0)//计算结果不可用
            {
               iapp.Project.CalcEngine.Calculate();//进行结构计算

            }
            //清除表格
            dataGridView1.Rows.Clear();
            int index = comboBoxLoadCombine.SelectedIndex + 1;//当前选择的工况索引
            IRobotCase icc = (IRobotCase)iapp.Project.Structure.Cases.GetAll().Get(index);//根据下拉框选择当前工况
            int case_num = icc.Number;

            for (int n = 0; n < 5; ++n)
            {
                int indexData = dataGridView1.Rows.Add();
                dataGridView1.Rows[indexData].Cells[0].Value = n + 1;
                dataGridView1.Rows[indexData].Cells[1].Value = (iapp.Project.Structure.Results.Bars.Forces.Value(n + 1, case_num, 0).MY / 1000).ToString("####0.00");
                dataGridView1.Rows[indexData].Cells[2].Value = (iapp.Project.Structure.Results.Bars.Forces.Value(n + 1, case_num, 0.5).MY / 1000).ToString("####0.00");
                dataGridView1.Rows[indexData].Cells[3].Value = (iapp.Project.Structure.Results.Bars.Forces.Value(n + 1, case_num, 1).MY / 1000).ToString("####0.00");
                dataGridView1.Rows[indexData].Cells[4].Value = (iapp.Project.Structure.Results.Bars.Forces.Value(n + 1, case_num, 0).FZ / 1000).ToString("####0.00");
                dataGridView1.Rows[indexData].Cells[5].Value = (iapp.Project.Structure.Results.Bars.Forces.Value(n + 1, case_num, 0.5).FZ / 1000).ToString("####0.00");
                dataGridView1.Rows[indexData].Cells[6].Value = (iapp.Project.Structure.Results.Bars.Forces.Value(n + 1, case_num, 1).FZ / 1000).ToString("####0.00");


            }
        }

        private void comboBoxLoadCombine_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetResult();

        }
    }
}
