using System.Collections;
using System.Collections.Generic;
using Delaunay;
using Delaunay.Geo;
using UnityEngine;

public static class ReturnSiteTriangles {

    public static List<Triangle> SiteTriangles(Voronoi voronoi, Vector2 sitePosition, Vector2 worldSize) {

        // We set up data about the region

        List<Vector2> boundaryPoints = voronoi.Region(sitePosition);
        List<uint> boundaryPointColors = new List<uint>();

        foreach (Vector2 point in boundaryPoints) {
            boundaryPointColors.Add(0);
        }

        // Using Delaunay Triangulation, we split the site into smaller subsections
        Voronoi siteVoronoi = new Voronoi(boundaryPoints, boundaryPointColors, new Rect(0, 0, worldSize.x, worldSize.y));
        return siteVoronoi._triangles;


    }


}