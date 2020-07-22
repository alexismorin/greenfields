using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainDerformer : MonoBehaviour {

    [SerializeField]
    float normalAngle = 90;

    public void Generate(Generation world, Color grass, Color dampGrass, Color mud, float maxWorldHeight, float largeTerrainNoiseSize, float largeTerrainNoiseStrength, float smallTerrainNoiseSize, float smallTerrainNoiseStrength, float moistureNoiseSize) {

        MeshFilter meshFilter = GetComponent<MeshFilter>();

        Mesh mesh = meshFilter.mesh;
        Vector3[] vertices = mesh.vertices;
        Color[] colors = mesh.colors;


        for (int i = 0; i < vertices.Length; i++) {

            // World positon of the vertex
            Vector3 worldPositionVertex = transform.TransformPoint(vertices[i]);

            // Noise Calculation
            float largeTerrainNoise = Mathf.PerlinNoise(worldPositionVertex.x * largeTerrainNoiseSize, worldPositionVertex.z * largeTerrainNoiseSize) * largeTerrainNoiseStrength;
            float smallTerrainNoise = Mathf.PerlinNoise(worldPositionVertex.x * smallTerrainNoiseSize, worldPositionVertex.z * smallTerrainNoiseSize) * smallTerrainNoiseStrength;
            float moistureNoise = Mathf.PerlinNoise(worldPositionVertex.x * moistureNoiseSize, worldPositionVertex.z * moistureNoiseSize);

            vertices[i] += new Vector3(0f, (largeTerrainNoise + smallTerrainNoise) * maxWorldHeight, 0f);

            Color groundColor = Color.Lerp(Color.Lerp(grass, dampGrass, largeTerrainNoise + smallTerrainNoise), mud, moistureNoise);
            colors[i] = groundColor;

        }

        // reassignation
        mesh.vertices = vertices;
        mesh.colors = colors;

        mesh.RecalculateBounds();
        //  mesh.RecalculateNormals();
        NormalSolver.RecalculateNormals(mesh, normalAngle);

        meshFilter.mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;

        world.terrainMeshFilters.Add(meshFilter);

    }
}