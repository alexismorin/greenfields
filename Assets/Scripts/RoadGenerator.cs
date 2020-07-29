using System.Collections;
using System.Collections.Generic;
using Delaunay;
using Delaunay.Geo;
using UnityEngine;

public class RoadGenerator : MonoBehaviour {

    [SerializeField]
    float pathDrawDistance = 1.5f;

    [SerializeField]
    int samples = 10;

    [SerializeField]
    LayerMask layerMask;

    public void Generate (Voronoi voronoi, Generation world, Zone zone) {

        List<LineSegment> roads = voronoi.SpanningTree (KruskalType.MINIMUM);
        List<Triangle> triangles = ReturnSiteTriangles.SiteTriangles (voronoi, new Vector2 (transform.position.x, transform.position.z), world.worldSize);

        foreach (Triangle triangle in triangles) {

            foreach (var road in roads) {

                if (InTriangle ((Vector2) road.p0, triangle.sites[0].Coord, triangle.sites[1].Coord, triangle.sites[2].Coord)) {

                    Vector2 a = (Vector2) road.p0;
                    Vector2 b = (Vector2) road.p1;
                    Vector3 startPos = new Vector3 (a.x, 100f, a.y);
                    Vector3 endPos = new Vector3 (b.x, 100f, b.y);

                    float distance = Vector3.Distance (startPos, endPos);
                    int roadSampleCount = Mathf.RoundToInt (distance) * samples;

                    for (int i = 0; i < roadSampleCount; i++) {
                        float progress = 1f / roadSampleCount * i;
                        Vector3 spawnPos = Vector3.Lerp (startPos, endPos, progress);

                        RaycastHit hit;

                        if (Physics.SphereCast (spawnPos, 1f, Vector3.down, out hit, 200f, layerMask)) {

                            // Kill fences and trees
                            if (hit.collider.tag != "Terrain") {
                                hit.transform.GetComponent<Collider> ().enabled = false;
                                Destroy (hit.transform.gameObject);

                            }

                            if (hit.collider.tag == "Terrain") {
                                MeshFilter meshFilter = hit.transform.gameObject.GetComponent<MeshFilter> ();
                                Mesh mesh = meshFilter.mesh;
                                Vector3[] vertices = mesh.vertices;
                                Color[] colors = mesh.colors;

                                for (int j = 0; j < vertices.Length; j++) {
                                    // World positon of the vertex
                                    Vector3 worldPositionVertex = meshFilter.transform.TransformPoint (vertices[j]);
                                    if (Vector3.Distance (worldPositionVertex, hit.point) < pathDrawDistance) {
                                        colors[j] = zone.roadColour;
                                    }
                                }
                                mesh.colors = colors;
                                meshFilter.mesh = mesh;

                            }

                        }
                    }
                }
            }
        }

    }

    public bool InTriangle (Vector2 p, Vector2 p0, Vector2 p1, Vector2 p2) {
        var a = .5f * (-p1.y * p2.x + p0.y * (-p1.x + p2.x) + p0.x * (p1.y - p2.y) + p1.x * p2.y);
        var sign = a < 0 ? -1 : 1;
        var s = (p0.y * p2.x - p0.x * p2.y + (p2.y - p0.y) * p.x + (p0.x - p2.x) * p.y) * sign;
        var t = (p0.x * p1.y - p0.y * p1.x + (p0.y - p1.y) * p.x + (p1.x - p0.x) * p.y) * sign;

        return s > 0 && t > 0 && (s + t) < 2 * a * sign;
    }

}