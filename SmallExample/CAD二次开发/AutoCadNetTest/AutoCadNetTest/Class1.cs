using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;

namespace AutoCadNetTest
{
    
    public class Class1
    {
        [CommandMethod("HelloNet")]
        public void HelloNet()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ed.WriteMessage("使用NET开发AutoCAD程序");
        }
        [CommandMethod("AddNewCircleTransaction")] 
        public static void AddNewCircleTransaction()
        { 
            //获取当前文档及数据库 
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database; 
            //启动事务 
            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction()) 
            { 
                //以读模式打开块表以读模式打开BlockTable块表 
                BlockTable acBlkTbl; 
                acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId, OpenMode.ForRead) as BlockTable; //以写的方式打开模型空间记录 
                BlockTableRecord acBlkTblRec; 
                acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord; 
                //以半径3圆心5,5画圆
                using (Circle acCirc = new Circle()) 
                { 
                    acCirc.Center = new Point3d(5, 5, 0); 
                    acCirc.Radius = 3; //将新对象添加到模型空间及事务 
                    acBlkTblRec.AppendEntity(acCirc); 
                    acTrans.AddNewlyCreatedDBObject(acCirc, true); 
                } //提交修改并关闭事务 
                acTrans.Commit(); 
            } 
        }
    }
}
