﻿using System.Collections;
using System.Collections.Generic;
using Delaunay;
using Delaunay.Geo;
using UnityEngine;

public class PointOfInterest : MonoBehaviour {

    [SerializeField]
    Zone[] possibleZones;

    [Space (10)]

    public Zone zone;

    [Header ("Generators")]
    [SerializeField]
    GroundColoringGenerator groundColoringGenerator;
    [SerializeField]
    RoadGenerator roadGenerator;
    [SerializeField]
    FenceGenerator fenceGenerator;
    [SerializeField]
    FoliageGenerator foliageGenerator;

    public void Generate (Voronoi sourceVoronoi, Generation world) {

        zone = possibleZones[Random.Range (0, possibleZones.Length)];
        gameObject.name = zone.name;

        groundColoringGenerator.Generate (sourceVoronoi, world, zone);

        if (zone.fence != null) {
            fenceGenerator.Generate (sourceVoronoi, world, zone);
        }

        if (zone.trees.Length > 0 || zone.groundCoverage.Length > 0) {
            foliageGenerator.Generate (sourceVoronoi, world, zone, this);
        }

    }

    public void FoliageGenerationCallback (Voronoi sourceVoronoi, Generation world) {
        roadGenerator.Generate (sourceVoronoi, world, zone);
    }

}