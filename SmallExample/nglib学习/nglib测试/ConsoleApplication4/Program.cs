using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace ConsoleApplication4
{
    class Program
    {
        static void Main(string[] args)
        {
            List<PontOfVol> points = new List<PontOfVol>();
            PontOfVol p1 = new PontOfVol();
            p1.x = 0;
            p1.y = 0;
            p1.z = 0;
            PontOfVol p2 = new PontOfVol();
            p2.x = 1;
            p2.y = 0;
            p2.z = 0;
            PontOfVol p3= new PontOfVol();
            p3.x = 1;
            p3.y = 1;
            p3.z = 1;
            PontOfVol p4 = new PontOfVol();
            p4.x = 1;
            p4.y = 0;
            p4.z = 1;
            PontOfVol p5 = new PontOfVol();
            p5.x = 0;
            p5.y = 1;
            p5.z = 1;
            PontOfVol p6 = new PontOfVol();
            p6.x = 0;
            p6.y = 0;
            p6.z = 1;
            PontOfVol p7 = new PontOfVol();
            p7.x = 0;
            p7.y = 1;
            p7.z = 0;
            PontOfVol p8= new PontOfVol();
            p8.x = 1;
            p8.y = 1;
            p8.z = 0;
            points.Add(p1);
            points.Add(p2);
            points.Add(p3);
            points.Add(p4);
            points.Add(p5);
            points.Add(p6);
            points.Add(p7);
            points.Add(p8);

            List<SurfaceOfVol> surfaces = new List<SurfaceOfVol>();
            SurfaceOfVol surface1 = new SurfaceOfVol();
            surface1.first = 2;
            surface1.second = 1;
            surface1.third = 7;
            SurfaceOfVol surface2 = new SurfaceOfVol();
            surface2.first = 8;
            surface2.second = 2;
            surface2.third = 7;
            SurfaceOfVol surface3 = new SurfaceOfVol();
            surface3.first = 6;
            surface3.second = 1;
            surface3.third = 2;
            SurfaceOfVol surface4 = new SurfaceOfVol();
            surface4.first = 4;
            surface4.second = 6;
            surface4.third = 2;
            SurfaceOfVol surface5 = new SurfaceOfVol();
            surface5.first = 4;
            surface5.second = 3;
            surface5.third = 5;
            SurfaceOfVol surface6 = new SurfaceOfVol();
            surface6.first = 5;
            surface6.second = 6;
            surface6.third = 4;
            SurfaceOfVol surface7 = new SurfaceOfVol();
            surface7.first = 8;
            surface7.second = 3;
            surface7.third = 4;
            SurfaceOfVol surface8 = new SurfaceOfVol();
            surface8.first = 8;
            surface8.second = 4;
            surface8.third = 2;
            SurfaceOfVol surface9 = new SurfaceOfVol();
            surface9.first = 5;
            surface9.second = 3;
            surface9.third = 8;
            SurfaceOfVol surface10 = new SurfaceOfVol();
            surface10.first = 7;
            surface10.second = 5;
            surface10.third = 8;
            SurfaceOfVol surface11 = new SurfaceOfVol();
            surface11.first = 1;
            surface11.second = 6;
            surface11.third = 5;
            SurfaceOfVol surface12 = new SurfaceOfVol();
            surface12.first = 7;
            surface12.second = 1;
            surface12.third = 5;
            surfaces.Add(surface1);
            surfaces.Add(surface2);
            surfaces.Add(surface3);
            surfaces.Add(surface4);
            surfaces.Add(surface5);
            surfaces.Add(surface6);
            surfaces.Add(surface7);
            surfaces.Add(surface8);
            surfaces.Add(surface9);
            surfaces.Add(surface10);
            surfaces.Add(surface11);
            surfaces.Add(surface12);
            int i = 0;
            int np = 8;
            int nse = 12;
            int ne = 0;
            int[] trig = new int[3];
            int[] tet = new int[4];
            double[] point = new double[3];
            string strMeshFile = "cube.surf";

            // initialize the Netgen library.
            NgLib.Ng_Init();

            // Generate new mesh structure.
            IntPtr mesh = NgLib.Ng_NewMesh();

            // Read surface mesh from file.
            // feed points to the mesh.
            for (int i = 0; i < np; ++i)
            {
                point[0]=points[i-1].x;
                point[1] = points[i - 1].y;
                point[2] = points[i - 1].z;
                NgLib.Ng_AddPoint(mesh, point);
            }

            // feed surface elements to the mesh.
            for (int i = 0; i < nse; ++i)
            {
                trig[0] = surfaces[i - 1].first;
                trig[1] = surfaces[i - 1].second;
                trig[2] = surfaces[i - 1].third;
                NgLib.Ng_AddSurfaceElement(mesh, Ng_Surface_Element_Type.NG_TRIG, trig);
            }

            // generate volume mesh.
            Ng_Meshing_Parameters mp = new Ng_Meshing_Parameters();
            mp.maxh = 1e6;
            mp.fineness = 1;
            mp.second_order = 0;

            NgLib.Ng_GenerateVolumeMesh(mesh, mp);

            // volume mesh output.
            np = NgLib.Ng_GetNP(mesh);
            for (int i = 1; i <= np; ++i)
            {
                NgLib.Ng_GetPoint(mesh, i, point);

            }

            ne = NgLib.Ng_GetNE(mesh);
            for (int i = 1; i <= ne; ++i)
            {
                NgLib.Ng_GetVolumeElement(mesh, i, tet);

            }

            // Save mesh.
            NgLib.Ng_SaveMesh(mesh, "test.vol");

            // deconstruct Netgen library.
            NgLib.Ng_Exit();



        }



    }
    public class PontOfVol
    {
        public double x;
        public double y;
        public double z;
    }
    public class SurfaceOfVol
    {
        public int first;
        public int second;
        public int third;
    }


}



