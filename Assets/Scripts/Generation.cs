using System.Collections;
using System.Collections.Generic;
using Delaunay;
using Delaunay.Geo;
using UnityEngine;

public class Generation : MonoBehaviour {

    [SerializeField]
    TerrainGenerator terrainGenerator;
    [SerializeField]
    PointsOfInterestGenerator pointsOfInterestGenerator;


    // Reference Values

    [HideInInspector]
    public Vector2 worldSize = Vector2.zero;
    [HideInInspector]
    public List<MeshFilter> terrainMeshFilters;
    //[HideInInspector]
    public List<LineSegment> builtFences = new List<LineSegment>();

    void Start() {
        terrainGenerator.Generate(this);
        pointsOfInterestGenerator.Generate(this, worldSize);
    }


}