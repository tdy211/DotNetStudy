//
// (C) Copyright 2009 by Autodesk, Inc. All rights reserved.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM 'AS IS' AND WITH ALL ITS FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RobotOM;

namespace RobotSDKSample
{
    public partial class Robot : Form
    {
        // main reference to Robot Application object
        //引用Robot程序类，创建一个实例 iapp
        private IRobotApplication iapp;

        // user numbers for main structure elements
        //定义几个常数
        private int startBar = 1, startNode = 1, beam1 = 2, beam2 = 3;
        // helper flags
        //定义初始状态，几何体创建状态，荷载创建状态
        private bool geometryCreated = false, loadsGenerated = false;
        //构造函数
        public Robot()
        {
            InitializeComponent();
        }
        //窗体加载时
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

            // fill combo-boxes with names of bar sections available in Robot
            //从ROBOT中获取断面库，然后给定义梁，柱的下拉列表赋值
            IRobotNamesArray inames = iapp.Project.Structure.Labels.GetAvailableNames(IRobotLabelType.I_LT_BAR_SECTION);
            for (int i = 1; i < inames.Count; ++i)
            {
                comboColumns.Items.Add(inames.Get(i));
                comboBeams.Items.Add(inames.Get(i));
            }
            comboBeams.SelectedIndex = 0;
            comboColumns.SelectedIndex = 0;
            
            // fill combo-boxes with names of supports available in Robot
            //从Robot中获取支撑种类，填充到支撑下拉列表中
            inames = iapp.Project.Structure.Labels.GetAvailableNames(IRobotLabelType.I_LT_SUPPORT);
            for (int i = 1; i < inames.Count; ++i)
            {
                comboSupportLeft.Items.Add(inames.Get(i));
                comboSupportRight.Items.Add(inames.Get(i));
            }
            comboSupportLeft.SelectedIndex = 0;
            comboSupportRight.SelectedIndex = 0;
        }

        // create the structure geometry
        //建立结构的集合模型
        private void createGeometry(object sender, EventArgs e)
        {
            // switch Interactive flag off to avoid any questions that need user interaction in Robot
            //关闭和ROBOT的交互功能
            iapp.Interactive = 0;

            // create a new project of type Frame 2D
            //创建一个Frame2d类型的项目
            iapp.Project.New(IRobotProjectType.I_PT_FRAME_2D);

            // create nodes
            //创建节点
            double x = 0, y = 0;
            double h = System.Convert.ToDouble(editH.Text);//获取高度
            double l = System.Convert.ToDouble(editL.Text);//获取长度
            double alpha = System.Convert.ToDouble(editA.Text);//获取角度
            IRobotNodeServer inds = iapp.Project.Structure.Nodes;//获取节点集合
            int n1 = startNode;
            inds.Create(n1, x, 0, y);//创建节点，格式为节点号，x坐标，y坐标，z坐标
            inds.Create(n1 + 1, x, 0, y + h);
            inds.Create(n1 + 2, x + (l / 2), 0, y + h + Math.Tan(alpha * (Math.PI / 180)) * l / 2);
            inds.Create(n1 + 3, x + l, 0, y + h);
            inds.Create(n1 + 4, x + l, 0, 0);

            // create bars
            //创建杆单元
            IRobotBarServer ibars = iapp.Project.Structure.Bars;//获取杆单元集合
            int b1 = startBar;
            ibars.Create(b1, n1, n1 + 1);//创建杆单元，格式为单元号,单元起始节点，单元结束节点
            ibars.Create(b1 + 1, n1 + 1, n1 + 2); beam1 = b1 + 1;
            ibars.Create(b1 + 2, n1 + 2, n1 + 3); beam2 = b1 + 2;
            ibars.Create(b1 + 3, n1 + 3, n1 + 4);

            // set selected bar section label to columns
            //给柱子设置截面
            RobotSelection isel = iapp.Project.Structure.Selections.Create(IRobotObjectType.I_OT_BAR);
            isel.AddOne(b1);
            isel.AddOne(b1 + 3);
            ibars.SetLabel(isel, IRobotLabelType.I_LT_BAR_SECTION, comboColumns.Text);
            
            // set selected bar section label to beams
            //给梁设置截面
            isel.Clear();
            isel.AddOne(b1 + 1);
            isel.AddOne(b1 + 2);
            ibars.SetLabel(isel, IRobotLabelType.I_LT_BAR_SECTION, comboBeams.Text);
            
            // set selected support label to nodes
            //给节点设置支持形式
            IRobotNode ind = (IRobotNode)inds.Get(n1);
            ind.SetLabel(IRobotLabelType.I_LT_SUPPORT, comboSupportLeft.Text);
            ind = (IRobotNode)inds.Get(n1 + 4);
            ind.SetLabel(IRobotLabelType.I_LT_SUPPORT, comboSupportRight.Text);

            geometryCreated = true;//结构几何创建结束

            loadsGenerated = false;//结构荷载没有创建

            // switch Interactive flag on to allow user to work with Robot GUI
            //打开robot截面操作
            iapp.Interactive = 1;

            // get the focus back
            this.Activate();
        }

        // helper method that creates a new load record in given load case
        //根据工况设置荷载
        private void create_concentrated_load(IRobotSimpleCase isc, double pos, double val)
        {
            // create a new force concentrated load record
            //添加一个集中荷载记录,类型为集中荷载
            IRobotLoadRecord2 ilr = (IRobotLoadRecord2)isc.Records.Create(IRobotLoadRecordType.I_LRT_BAR_FORCE_CONCENTRATED);
            
            
            // define the values of load record
            //给荷载记录赋值
            ilr.SetValue((short)IRobotBarForceConcentrateRecordValues.I_BFCRV_REL, 1.0);
            ilr.SetValue((short)IRobotBarForceConcentrateRecordValues.I_BFCRV_X, pos);
            ilr.SetValue((short)IRobotBarForceConcentrateRecordValues.I_BFCRV_FZ, val);
            
            // apply load record to both beams of the structure
            ilr.Objects.AddOne(beam1);//将ilr荷载作用到编号为beam1的单元上
            ilr.Objects.AddOne(beam2);//将ilr荷载作用到编号为beam2的单元上
        }

        // generate loads
        private void generateLoads(object sender, EventArgs e)
        {
            if (!geometryCreated)//如果没有创建几何
            {
                // geometry must be created first
                //创建几何
                createGeometry(sender, e);
            }

            // switch Interactive flag off to avoid any questions that need user interaction in Robot
            //关闭robot的交互
            iapp.Interactive = 0;

            if (loadsGenerated)//如果几何已经创建好
            {
                // remove all existing load cases
                //将现有工况全部清除
                RobotSelection iallCases = iapp.Project.Structure.Selections.CreateFull(IRobotObjectType.I_OT_CASE);
                iapp.Project.Structure.Cases.DeleteMany(iallCases);
            }

            // get reference to load cases server
            //获取本项目的工况集合
            IRobotCaseServer icases = (IRobotCaseServer)iapp.Project.Structure.Cases;
            
            // get first available (free) user number for load case
            //获取可用工况编号
            int c1 = icases.FreeNumber;

            if (checkDeadLoad.Checked)//如果选中了自重荷载
            {
                // create dead load case
                //创建自重工况
                IRobotSimpleCase isc1 = (IRobotSimpleCase)icases.CreateSimple(c1, "Dead load", IRobotCaseNature.I_CN_PERMANENT, IRobotCaseAnalizeType.I_CAT_STATIC_LINEAR);
                
                // define dead load record for the entire structure
                //定义自重荷载，添加到自重工况里面取
                IRobotLoadRecord2 ilr1 = (IRobotLoadRecord2)isc1.Records.Create(IRobotLoadRecordType.I_LRT_DEAD);
                ilr1.SetValue((short)IRobotDeadRecordValues.I_DRV_ENTIRE_STRUCTURE, (double)1);
                ++c1;
            }

            // create live load case
            //创建活荷载工况
            IRobotSimpleCase isc = (IRobotSimpleCase)icases.CreateSimple(c1, "Live load", IRobotCaseNature.I_CN_ACCIDENTAL, IRobotCaseAnalizeType.I_CAT_STATIC_LINEAR);
            // convert force value from kN to N and change the direction
            //荷载格式转换
            double val = -1000 * System.Convert.ToDouble(editIntensity1.Text);
            // add force concentrated load records in 5 points on the beams
            //给活荷载工况添加荷载
            create_concentrated_load(isc, 0.0, 0.5*val);
            create_concentrated_load(isc, 0.25, val);
            create_concentrated_load(isc, 0.5, val);
            create_concentrated_load(isc, 0.75, val);
            create_concentrated_load(isc, 1.0, 0.5 * val);

            // create live load case
            //创建另外一个荷载工况
            isc = (IRobotSimpleCase)icases.CreateSimple(c1+1, "Exploitation load", IRobotCaseNature.I_CN_EXPLOATATION, IRobotCaseAnalizeType.I_CAT_STATIC_LINEAR);
            // convert force value from kN to N and change the direction
            //把荷载值从KN转换成N
            val = -1000 * System.Convert.ToDouble(editIntensity2.Text);
            // add force concentrated load records in 5 points on the beams
            //将荷载作用到梁上面
            create_concentrated_load(isc, 0.0, 0.5 * val);
            create_concentrated_load(isc, 0.25, val);
            create_concentrated_load(isc, 0.5, val);
            create_concentrated_load(isc, 0.75, val);
            create_concentrated_load(isc, 1.0, 0.5 * val);

            // create wind load applied to columns
           //添加荷载工况，风荷载
            isc = (IRobotSimpleCase)icases.CreateSimple(c1+2, "Wind load", IRobotCaseNature.I_CN_WIND, IRobotCaseAnalizeType.I_CAT_STATIC_LINEAR);
            IRobotLoadRecord2 ilr = (IRobotLoadRecord2)isc.Records.Create(IRobotLoadRecordType.I_LRT_BAR_UNIFORM);
            ilr.SetValue((short)IRobotBarUniformRecordValues.I_BURV_PZ, 1000 * System.Convert.ToDouble(editIntensity3.Text));
            ilr.SetValue((short)IRobotBarUniformRecordValues.I_BURV_LOCAL, 1.0); 
            ilr.Objects.AddOne(beam1-1);
            ilr.Objects.AddOne(beam2+1);
            
            // create combinations
            //添加荷载组合
            IRobotCaseCombination icc = (IRobotCaseCombination)icases.CreateCombination(c1 + 3, "Comb ULS", IRobotCombinationType.I_CBT_ULS, IRobotCaseNature.I_CN_EXPLOATATION, IRobotCaseAnalizeType.I_CAT_COMB);
            icc.CaseFactors.New(c1, System.Convert.ToDouble(editUls1.Text));
            icc.CaseFactors.New(c1 + 1, System.Convert.ToDouble(editUls2.Text));
            icc.CaseFactors.New(c1 + 2, System.Convert.ToDouble(editUls3.Text));

            icc = (IRobotCaseCombination)icases.CreateCombination(c1 + 4, "Comb SLS", IRobotCombinationType.I_CBT_SLS, IRobotCaseNature.I_CN_EXPLOATATION, IRobotCaseAnalizeType.I_CAT_COMB);
            icc.CaseFactors.New(c1, System.Convert.ToDouble(editSls1.Text));
            icc.CaseFactors.New(c1 + 1, System.Convert.ToDouble(editSls2.Text));
            icc.CaseFactors.New(c1 + 2, System.Convert.ToDouble(editSls3.Text));

            loadsGenerated = true;

            // switch Interactive flag on to allow user to work with Robot GUI
            //允许ROBOT界面交互
            iapp.Interactive = 1;
            
            // get the focus back
            //获取焦点
            this.Activate();
        }
        //查看结果Tab页面的初始化
        private void tabControl2_Selected(object sender, TabControlEventArgs e)
        {
            // fill combo-box with names of all load cases defined in the structure
            comboLoadCase.Items.Clear();
            IRobotCollection icases = iapp.Project.Structure.Cases.GetAll();
            for (int i = 1; i <= icases.Count; ++i)
            {
                IRobotCase ic = (IRobotCase)icases.Get(i);
                int idx = comboLoadCase.Items.Add(ic.Name);
            }
            // select the first item
            if (comboLoadCase.Items.Count > 0)
            {
                comboLoadCase.SelectedIndex = 0;
            }
        }
        //获取计算结果，当选择工况下拉框的值变化时触发
        private void getResults(object sender, EventArgs e)
        {
            if (iapp.Project.Structure.Results.Available == 0)//计算结果不可用
            {
                iapp.Project.CalcEngine.Calculate();//进行结构计算
            }
            //清除表格
            dataGridView1.Rows.Clear();
            //增加12行
            dataGridView1.Rows.Add(12);
            //给表头赋值
            dataGridView1.Rows[0].Cells[0].Value = "FX (N)";
            dataGridView1.Rows[1].Cells[0].Value = "FY (N)";
            dataGridView1.Rows[2].Cells[0].Value = "FZ (N)";
            dataGridView1.Rows[3].Cells[0].Value = "MX (Nm)";
            dataGridView1.Rows[4].Cells[0].Value = "MY (Nm)";
            dataGridView1.Rows[5].Cells[0].Value = "MZ (Nm)";
            dataGridView1.Rows[6].Cells[0].Value = "UX (m)";
            dataGridView1.Rows[7].Cells[0].Value = "UY (m)";
            dataGridView1.Rows[8].Cells[0].Value = "UZ (m)";
            dataGridView1.Rows[9].Cells[0].Value = "RX (Rad)";
            dataGridView1.Rows[10].Cells[0].Value = "RY (Rad)";
            dataGridView1.Rows[11].Cells[0].Value = "RZ (Rad)";

            int idx = comboLoadCase.SelectedIndex + 1;//当前选择的工况索引
            IRobotCase ic = (IRobotCase)iapp.Project.Structure.Cases.GetAll().Get(idx);//根据下拉框选择当前工况
            int case_num = ic.Number;
            for (int n = startNode; n < startNode+5; ++n)
            {
                IRobotReactionData ireact = iapp.Project.Structure.Results.Nodes.Reactions.Value(n, case_num);//获取工况编号为case_num的反力
                IRobotDisplacementData idisp = iapp.Project.Structure.Results.Nodes.Displacements.Value(n, case_num);//获取工况编号为case_num的位移
                //将反力写入到表格中
                dataGridView1.Rows[0].Cells[n].Value = ireact.FX.ToString("####0.00");
                dataGridView1.Rows[1].Cells[n].Value = ireact.FY.ToString("####0.00");
                dataGridView1.Rows[2].Cells[n].Value = ireact.FZ.ToString("####0.00");
                dataGridView1.Rows[3].Cells[n].Value = ireact.MX.ToString("####0.00");
                dataGridView1.Rows[4].Cells[n].Value = ireact.MY.ToString("####0.00");
                dataGridView1.Rows[5].Cells[n].Value = ireact.MZ.ToString("####0.00");
                //将位移写入到表格中
                dataGridView1.Rows[6].Cells[n].Value = idisp.UX.ToString("####0.00");
                dataGridView1.Rows[7].Cells[n].Value = idisp.UY.ToString("####0.00");
                dataGridView1.Rows[8].Cells[n].Value = idisp.UZ.ToString("####0.00");
                dataGridView1.Rows[9].Cells[n].Value = idisp.RX.ToString("####0.00");
                dataGridView1.Rows[10].Cells[n].Value = idisp.RY.ToString("####0.00");
                dataGridView1.Rows[11].Cells[n].Value = idisp.RZ.ToString("####0.00");
            }
        }
    }
}
