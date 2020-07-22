using System.Collections;
using System.Collections.Generic;
using Delaunay;
using Delaunay.Geo;
using UnityEngine;

public class GroundColoringGenerator : MonoBehaviour {

    public void Generate(Voronoi voronoi, Generation world, Zone zone) {

        if (zone.tintGround) {

            List<Triangle> triangles = ReturnSiteTriangles.SiteTriangles(voronoi, new Vector2(transform.position.x, transform.position.z), world.worldSize);

            foreach (var triangle in triangles) {



                foreach (MeshFilter meshFilter in world.terrainMeshFilters) {



                    Mesh mesh = meshFilter.mesh;
                    Vector3[] vertices = mesh.vertices;
                    Color[] colors = mesh.colors;

                    for (int i = 0; i < vertices.Length; i++) {

                        // World positon of the vertex
                        Vector3 worldPositionVertex = meshFilter.transform.TransformPoint(vertices[i]);



                        if (InTriangle(new Vector2(worldPositionVertex.x, worldPositionVertex.z), triangle.sites[0].Coord, triangle.sites[1].Coord, triangle.sites[2].Coord)) {
                            colors[i] = zone.groundColour;
                        }
                    }
                    mesh.colors = colors;
                    meshFilter.mesh = mesh;
                }
            }
        }
    }

    public bool InTriangle(Vector2 p, Vector2 p0, Vector2 p1, Vector2 p2) {
        var a = .5f * (-p1.y * p2.x + p0.y * (-p1.x + p2.x) + p0.x * (p1.y - p2.y) + p1.x * p2.y);
        var sign = a < 0 ? -1 : 1;
        var s = (p0.y * p2.x - p0.x * p2.y + (p2.y - p0.y) * p.x + (p0.x - p2.x) * p.y) * sign;
        var t = (p0.x * p1.y - p0.y * p1.x + (p0.y - p1.y) * p.x + (p1.x - p0.x) * p.y) * sign;

        return s > 0 && t > 0 && (s + t) < 2 * a * sign;
    }

}