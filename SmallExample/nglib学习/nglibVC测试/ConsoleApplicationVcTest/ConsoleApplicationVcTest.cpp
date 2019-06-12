// ConsoleApplicationVcTest.cpp : 定义控制台应用程序的入口点。
//

#include "stdafx.h"
#include <iostream>
#include <fstream>

namespace nglib {
#include <nglib.h>
}

#pragma comment(lib, "nglib.lib")


int main(int argc, char* argv[])
{
	std::cout << "Netgen Testing..." << std::endl;

	int i = 0;
	int np = 0;
	int nse = 0;
	int ne = 0;
	int trig[3] = { 0 };
	int tet[4] = { 0 };

	double point[3] = { 0.0 };

	std::string strMeshFile = (argc > 1) ? argv[1] : "cube.surf";
	std::ifstream meshFile(strMeshFile.c_str());

	// initialize the Netgen library.
	nglib::Ng_Init();

	// Generate new mesh structure.
	nglib::Ng_Mesh* mesh = nglib::Ng_NewMesh();

	// Read surface mesh from file.
	// feed points to the mesh.
	meshFile >> np;
	std::cout << "Reading " << np << " points..." << std::endl;
	for (int i = 0; i < np; ++i)
	{
		meshFile >> point[0] >> point[1] >> point[2];
		nglib::Ng_AddPoint(mesh, point);
	}
	std::cout << "done." << std::endl;

	// feed surface elements to the mesh.
	meshFile >> nse;
	std::cout << "Reading " << nse << " faces..." << std::endl;
	for (int i = 0; i < nse; ++i)
	{
		meshFile >> trig[0] >> trig[1] >> trig[2];
		nglib::Ng_AddSurfaceElement(mesh, nglib::NG_TRIG, trig);
	}
	std::cout << "done." << std::endl;

	// generate volume mesh.
	nglib::Ng_Meshing_Parameters mp;
	mp.maxh = 1e6;
	mp.fineness = 1;
	mp.second_order = 0;

	std::cout << "start meshing..." << std::endl;
	nglib::Ng_GenerateVolumeMesh(mesh, &mp);
	std::cout << "meshing done." << std::endl;

	// volume mesh output.
	np = nglib::Ng_GetNP(mesh);
	std::cout << "Points: " << np << std::endl;
	for (int i = 1; i <= np; ++i)
	{
		nglib::Ng_GetPoint(mesh, i, point);
		std::cout << i << ": " << point[0] << ", " << point[1] << ", " << point[2] << std::endl;
	}

	ne = nglib::Ng_GetNE(mesh);
	std::cout << "Elements: " << ne << std::endl;
	for (int i = 1; i <= ne; ++i)
	{
		nglib::Ng_GetVolumeElement(mesh, i, tet);
		std::cout << i << ": " << tet[0] << ", " << tet[1] << ", " << tet[2] << ", " << tet[3] << std::endl;
	}

	// Save mesh.
	nglib::Ng_SaveMesh(mesh, "test.vol");

	// deconstruct Netgen library.
	nglib::Ng_Exit();

	return 0;

}

