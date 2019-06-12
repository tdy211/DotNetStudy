using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RobotOM;
namespace ShuiYunTools
{
    class BeamTools
    {
        //建立结构的几何模型
        private void createGeometry()
        {
        }
        
        //施加集中荷载
        public static void create_concentrated_load(IRobotSimpleCase isc, int elementNO ,double pos, double val)
        {
            // create a new force concentrated load record
            //添加一个集中荷载记录,类型为集中荷载I_LRT_BAR_UNIFORM 
            //IRobotLoadRecord2 ilr = (IRobotLoadRecord2)isc.Records.Create(IRobotLoadRecordType.I_LRT_BAR_FORCE_CONCENTRATED);
            IRobotLoadRecord2 ilr = (IRobotLoadRecord2)isc.Records.Create(IRobotLoadRecordType.I_LRT_BAR_UNIFORM);

            // define the values of load record
            //给荷载记录赋值
            //ilr.SetValue((short)IRobotBarForceConcentrateRecordValues.I_BFCRV_REL, 1.0);
            //ilr.SetValue((short)IRobotBarForceConcentrateRecordValues.I_BFCRV_X, pos);
            //ilr.SetValue((short)IRobotBarForceConcentrateRecordValues.I_BFCRV_FZ, val);
            ilr.SetValue(System.Convert.ToInt16(IRobotUniformRecordValues.I_URV_PZ), val);

            // apply load record to both beams of the structure
            ilr.Objects.AddOne(elementNO);//将ilr荷载作用到编号为beam1的单元上
            //ilr.Objects.AddOne(elementNO);//将ilr荷载作用到编号为beam2的单元上
        }


    }
}
