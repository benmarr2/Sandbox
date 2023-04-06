using System;
using Godot;

public partial class TerrainGenerator : MeshInstance3D 
{
    TerrainGenerator()
    {
        GenerateTerrain();
    }

    private FastNoiseLite _fastNoiseLite = new();

    private int _chunkSize = 64;
    private float _scale = 10.0f; // Increase the scale for flatter terrain

    public void GenerateTerrain()
    {
        var surfaceTool = new SurfaceTool();
        var arrayMesh = new ArrayMesh();

        _fastNoiseLite.Seed = new Random().Next();
        _fastNoiseLite.FractalOctaves = 2; // Reduce the FractalOctaves for less noise complexity
        _fastNoiseLite.Frequency = 5.0f; // Lower the frequency for smoother transitions
        _fastNoiseLite.NoiseType = FastNoiseLite.NoiseTypeEnum.Simplex;

        surfaceTool.Begin(Mesh.PrimitiveType.Triangles);

        for (int x = 0; x < _chunkSize; x++)
        {
            for (int z = 0; z < _chunkSize; z++)
            {
                float height = _fastNoiseLite.GetNoise2D(x, z);

                Vector3 vertex = new Vector3(x * _scale, height * 64, z * _scale);
                surfaceTool.AddVertex(vertex);
            }
        }

        for (int x = 0; x < _chunkSize - 1; x++)
        {
            for (int z = 0; z < _chunkSize - 1; z++)
            {
                int i = x + z * _chunkSize;

                surfaceTool.AddIndex(i);
                surfaceTool.AddIndex(i + 1);
                surfaceTool.AddIndex(i + _chunkSize);

                surfaceTool.AddIndex(i + 1);
                surfaceTool.AddIndex(i + _chunkSize + 1);
                surfaceTool.AddIndex(i + _chunkSize);
            }
        }

        surfaceTool.GenerateNormals();
        surfaceTool.Commit(arrayMesh);

        // Create a StaticBody3D for physics
        var terrainBody = new StaticBody3D();
        AddChild(terrainBody);

        // Create a MeshInstance3D and set the mesh
        var terrainMesh = new MeshInstance3D
        {
            Mesh = arrayMesh
        };
        terrainBody.AddChild(terrainMesh);


        

        // Load and apply the material
        Material terrainMaterial = (Material)ResourceLoader.Load("res://simple_material.tres");
        terrainMesh.SetSurfaceOverrideMaterial(0, terrainMaterial);

        GD.Print("Generated");
    }
}
