using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainMeshApplicator : MonoBehaviour {

    [Tooltip("Do we deform the mesh to fit the procedurally generated terrain. Good for fences.")]
    [SerializeField]
    bool deformMesh = false;

    [SerializeField]
    string collisonTag = "Terrain";

    [SerializeField]
    LayerMask deformLayerMask;

    [Header("Misc Placement Settings")]

    [SerializeField]
    bool randomYRotation = false;

    [SerializeField]
    bool orientToSurfaceNormal = false;

    [Header("Colour Settings")]

    [SerializeField]
    bool useGroundColor = false;


    void Start() {

        RaycastHit hit;
        if (Physics.Raycast(GetComponent<MeshRenderer>().bounds.center, Vector3.down, out hit, 200f)) {

            if (hit.transform.tag == collisonTag) {

                if (deformMesh) {
                    // For each vertex, we raycast down and add an offset
                    MeshFilter meshFilter = GetComponent<MeshFilter>();
                    Mesh mesh = meshFilter.mesh;
                    Vector3[] vertices = mesh.vertices;

                    for (int i = 0; i < vertices.Length; i++) {

                        // World positon of the vertex
                        Vector3 worldPositionVertex = transform.TransformPoint(vertices[i]);

                        RaycastHit vertexHit;

                        if (Physics.Raycast(worldPositionVertex, Vector3.down, out vertexHit, 300f, deformLayerMask)) {
                            float baseHeight = vertices[i].y;
                            vertices[i] -= new Vector3(0f, vertexHit.distance - baseHeight, 0f);
                        } else {
                            float baseHeight = vertices[i].y;
                            vertices[i] -= new Vector3(0f, 100f - baseHeight, 0f);
                        }
                    }
                    mesh.vertices = vertices;
                    mesh.RecalculateBounds();
                    mesh.RecalculateNormals();
                    NormalSolver.RecalculateNormals(mesh, 90f);
                    meshFilter.mesh = mesh;

                    gameObject.AddComponent<BoxCollider>();

                } else {
                    transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
                }

                // Misc Additonal

                if (useGroundColor) {

                    // we fetch color data from the terrain

                    MeshFilter filter = hit.transform.gameObject.GetComponent<MeshFilter>();
                    Mesh mesh = filter.mesh;
                    Color[] colors = mesh.colors;
                    Vector3[] vertices = mesh.vertices;

                    Color finalGrassColor = Color.white;
                    float closestDistance = 1000f;


                    for (int i = 0; i < vertices.Length; i++) {

                        Vector3 worldPositionVertex = transform.TransformPoint(vertices[i]);
                        float distancce = Vector3.Distance(transform.position, worldPositionVertex);

                        if (distancce < closestDistance) {
                            closestDistance = distancce;
                            finalGrassColor = colors[i];
                        }
                    }

                    // now we color our actual mesh

                    filter = gameObject.GetComponent<MeshFilter>();
                    mesh = filter.mesh;
                    colors = mesh.colors;

                    for (int i = 0; i < colors.Length; i++) {
                        colors[i] = new Color(finalGrassColor.r, finalGrassColor.g, finalGrassColor.b, colors[i].a);
                    }

                    mesh.colors = colors;
                    filter.mesh = mesh;

                }

                if (orientToSurfaceNormal) {
                    Vector3 lookAt = Vector3.Cross(-hit.normal, transform.right);
                    lookAt = lookAt.y < 0 ? -lookAt : lookAt;
                    transform.rotation = Quaternion.LookRotation(hit.point + lookAt, hit.normal);
                }

                if (randomYRotation) {
                    transform.Rotate(new Vector3(0f, Random.value * 360, 0f), Space.Self);
                }

            } else {
                Destroy(this.gameObject);
            }
        }
    }
}