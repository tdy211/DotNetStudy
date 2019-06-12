using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
namespace ConsoleApplication4
{
    class NgLib
    {
        [DllImport("nglib.dll", EntryPoint = "?Ng_Init@nglib@@YAXXZ")]
        public static extern void Ng_Init();
        [DllImport("nglib.dll", EntryPoint = "?Ng_Exit@nglib@@YAXXZ")]
        public static extern void Ng_Exit();
        [DllImport("nglib.dll", EntryPoint = "?Ng_NewMesh@nglib@@YAPEAPEAXXZ")]
        public static extern IntPtr Ng_NewMesh();
        [DllImport("nglib.dll", EntryPoint = "?Ng_AddPoint@nglib@@YAXPEAPEAXPEAN@Z")]
        public static extern void Ng_AddPoint(IntPtr mesh, double[] point);
        [DllImport("nglib.dll", EntryPoint = "?Ng_AddSurfaceElement@nglib@@YAXPEAPEAXW4Ng_Surface_Element_Type@1@PEAH@Z")]
        public static extern void Ng_AddSurfaceElement(IntPtr mesh, Ng_Surface_Element_Type NG_TRIG, int[] trig);
        [DllImport("nglib.dll", EntryPoint = "?Ng_GenerateVolumeMesh@nglib@@YA?AW4Ng_Result@1@PEAPEAXPEAVNg_Meshing_Parameters@1@@Z")]
        public static extern void Ng_GenerateVolumeMesh(IntPtr mesh, Ng_Meshing_Parameters mp);
        [DllImport("nglib.dll", EntryPoint = "?Ng_GetNP@nglib@@YAHPEAPEAX@Z")]
        public static extern int Ng_GetNP(IntPtr mesh);
        [DllImport("nglib.dll", EntryPoint = "?Ng_GetPoint@nglib@@YAXPEAPEAXHPEAN@Z")]
        public static extern void Ng_GetPoint(IntPtr mesh, int i, double[] point);
        [DllImport("nglib.dll", EntryPoint = "?Ng_GetNE@nglib@@YAHPEAPEAX@Z")]
        public static extern int Ng_GetNE(IntPtr mesh);
        [DllImport("nglib.dll", EntryPoint = "?Ng_GetVolumeElement@nglib@@YA?AW4Ng_Volume_Element_Type@1@PEAPEAXHPEAH@Z")]
        public static extern void Ng_GetVolumeElement(IntPtr mesh, int i, int[] tet);
        [DllImport("nglib.dll", EntryPoint = "?Ng_SaveMesh@nglib@@YAXPEAPEAXPEBD@Z")]
        public static extern void Ng_SaveMesh(IntPtr mesh, string fileName);





    }
    enum Ng_Surface_Element_Type
    { NG_TRIG = 1, NG_QUAD = 2, NG_TRIG6 = 3, NG_QUAD6 = 4, NG_QUAD8 = 5 };

    /// Currently implemented volume element types
    enum Ng_Volume_Element_Type
    { NG_TET = 1, NG_PYRAMID = 2, NG_PRISM = 3, NG_TET10 = 4 };

    /// Values returned by Netgen functions
    enum Ng_Result
    {
        NG_ERROR = -1,
        NG_OK = 0,
        NG_SURFACE_INPUT_ERROR = 1,
        NG_VOLUME_FAILURE = 2,
        NG_STL_INPUT_ERROR = 3,
        NG_SURFACE_FAILURE = 4,
        NG_FILE_NOT_FOUND = 5
    };
    class Ng_Meshing_Parameters
    {

        public int uselocalh;                      //!< Switch to enable / disable usage of local mesh size modifiers

        public double maxh;                        //!< Maximum global mesh size allowed
        public double minh;                        //!< Minimum global mesh size allowed

        public double fineness;                    //!< Mesh density: 0...1 (0 => coarse; 1 => fine)
        public double grading;                     //!< Mesh grading: 0...1 (0 => uniform mesh; 1 => aggressive local grading)

        public double elementsperedge;             //!< Number of elements to generate per edge of the geometry
        public double elementspercurve;            //!< Elements to generate per curvature radius

        public int closeedgeenable;                //!< Enable / Disable mesh refinement at close edges
        public double closeedgefact;               //!< Factor to use for refinement at close edges (larger => finer)

        public int second_order;                   //!< Generate second-order surface and volume elements
        public int quad_dominated;                 //!< Creates a Quad-dominated mesh 

        public string meshsize_filename;           //!< Optional external mesh size file 

        public int optsurfmeshenable;              //!< Enable / Disable automatic surface mesh optimization
        public int optvolmeshenable;               //!< Enable / Disable automatic volume mesh optimization

        public int optsteps_3d;                     //!< Number of optimize steps to use for 3-D mesh optimization
        public int optsteps_2d;                     //!< Number of optimize steps to use for 2-D mesh optimization

        // Philippose - 13/09/2010
        // Added a couple more parameters into the meshing parameters list 
        // from Netgen into Nglib
        public int invert_tets;                    //!< Invert all the volume elements
        public int invert_trigs;                   //!< Invert all the surface triangle elements

        public int check_overlap;                  //!< Check for overlapping surfaces during Surface meshing
        public int check_overlapping_boundary;     //!< Check for overlapping surface elements before volume meshing


        /*!
           Default constructor for the Mesh Parameters class

           Note: This constructor initialises the variables in the 
           class with the following default values
           - #uselocalh: 1
           - #maxh: 1000.0
           - #fineness: 0.5
           - #grading: 0.3
           - #elementsperedge: 2.0
           - #elementspercurve: 2.0
           - #closeedgeenable: 0
           - #closeedgefact: 2.0
           - #secondorder: 0
           - #meshsize_filename: null
           - #quad_dominated: 0
           - #optsurfmeshenable: 1
           - #optvolmeshenable: 1
           - #optsteps_2d: 3
           - #optsteps_3d: 3
           - #invert_tets: 0
           - #invert_trigs:0 
           - #check_overlap: 1
           - #check_overlapping_boundary: 1
        */
        [DllImport("nglib.dll", EntryPoint = "??0Ng_Meshing_Parameters@nglib@@QEAA@XZ")]
         static extern Ng_Meshing_Parameters();





        /*!
            Reset the meshing parameters to their defaults

            This member function resets all the meshing parameters 
            of the object to the default values
        */
        [DllImport("nglib.dll", EntryPoint = "?Reset_Parameters@Ng_Meshing_Parameters@nglib@@QEAAXXZ")]
        public static extern void Reset_Parameters();





        /*!
            Transfer local meshing parameters to internal meshing parameters

            This member function transfers all the meshing parameters 
            defined in the local meshing parameters structure of nglib into 
            the internal meshing parameters structure used by the Netgen core
        */
        [DllImport("nglib.dll", EntryPoint = "?Transfer_Parameters@Ng_Meshing_Parameters@nglib@@QEAAXXZ")]
        public static extern void Transfer_Parameters();


    }

}
