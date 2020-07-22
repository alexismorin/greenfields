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

    public void Generate(Voronoi voronoi, Generation world, Zone zone) {

        List<LineSegment> roads = voronoi.SpanningTree(KruskalType.MINIMUM);
        List<Triangle> triangles = ReturnSiteTriangles.SiteTriangles(voronoi, new Vector2(transform.position.x, transform.position.z), world.worldSize);

        foreach (var road in roads) {

            Vector2 a = (Vector2) road.p0;
            Vector2 b = (Vector2) road.p1;
            Vector3 startPos = new Vector3(a.x, 100f, a.y);
            Vector3 endPos = new Vector3(b.x, 100f, b.y);
            //   Debug.DrawLine(startPos, endPos, Color.red, 100f);

            float distance = Vector3.Distance(startPos, endPos);
            int roadSampleCount = Mathf.RoundToInt(distance) * samples;

            for (int i = 0; i < roadSampleCount; i++) {
                float progress = 1f / roadSampleCount * i;
                Vector3 spawnPos = Vector3.Lerp(startPos, endPos, progress);

                RaycastHit hit;
                if (Physics.SphereCast(spawnPos, 3f, Vector3.down, out hit, 200f)) {

                    Debug.DrawLine(hit.point, hit.point + Vector3.up, Color.red, 100f);

                    // Kill fences and trees
                    if (hit.collider.tag != "Terrain") {
                        Destroy(hit.transform.gameObject);
                    }

                    if (hit.collider.tag == "Terrain") {
                        MeshFilter meshFilter = hit.transform.gameObject.GetComponent<MeshFilter>();
                        Mesh mesh = meshFilter.mesh;
                        Vector3[] vertices = mesh.vertices;
                        Color[] colors = mesh.colors;

                        for (int j = 0; j < vertices.Length; j++) {
                            // World positon of the vertex
                            Vector3 worldPositionVertex = meshFilter.transform.TransformPoint(vertices[j]);
                            if (Vector3.Distance(worldPositionVertex, hit.point) < pathDrawDistance) {
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