using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainDerformer : MonoBehaviour {

    [SerializeField]
    float normalAngle = 90;

    public void Generate (Generation world, Color grass, Color dampGrass, Color mud, PerlinNoise[] terrainNoises, PerlinNoise moistureNoise) {

        MeshFilter meshFilter = GetComponent<MeshFilter> ();

        Mesh mesh = meshFilter.mesh;
        Vector3[] vertices = mesh.vertices;
        Color[] colors = mesh.colors;

        for (int i = 0; i < vertices.Length; i++) {

            // World positon of the vertex
            Vector3 worldPositionVertex = transform.TransformPoint (vertices[i]);

            float finalDeform = 0f;
            float maxWorldHeight = 0f;

            foreach (PerlinNoise noise in terrainNoises) {
                finalDeform += noise.ReturnNoise (worldPositionVertex);
                maxWorldHeight += noise.strength;
            }

            float moisture = moistureNoise.ReturnNoise (worldPositionVertex);

            vertices[i] += new Vector3 (0f, finalDeform, 0f);

            Color groundColor = Color.Lerp (Color.Lerp (grass, dampGrass, 1f / maxWorldHeight * vertices[i].y), mud, moisture);
            colors[i] = groundColor;

        }

        // reassignation
        mesh.vertices = vertices;
        mesh.colors = colors;

        mesh.RecalculateBounds ();
        mesh.RecalculateNormals ();
        NormalSolver.RecalculateNormals (mesh, normalAngle);

        meshFilter.mesh = mesh;
        GetComponent<MeshCollider> ().sharedMesh = mesh;

        world.terrainMeshFilters.Add (meshFilter);

    }
}