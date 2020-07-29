using System.Collections;
using System.Collections.Generic;
using Delaunay;
using Delaunay.Geo;
using UnityEngine;

public class FoliageGenerator : MonoBehaviour {

    [SerializeField]
    int baseTreeDensity = 50;
    [SerializeField]
    int baseGroundCoverageDensity = 100;

    public void Generate (Voronoi voronoi, Generation world, Zone zone, PointOfInterest pointOfInterest) {

        // We set up data about the region

        List<Vector2> boundaryPoints = voronoi.Region (new Vector2 (transform.position.x, transform.position.z));
        Bounds boundaryBounds = new Bounds ();

        foreach (Vector2 point in boundaryPoints) {
            boundaryBounds.Encapsulate (new Vector3 (point.x, 100f, point.y));
        }

        List<Triangle> triangles = ReturnSiteTriangles.SiteTriangles (voronoi, new Vector2 (transform.position.x, transform.position.z), world.worldSize);

        // We pick how much foliage we'll actually have
        int treeSpawnAmmount = Mathf.RoundToInt ((boundaryBounds.size.magnitude * 0.01f) * baseTreeDensity * zone.treeDensity);
        int treeGroundCoverageAmmount = Mathf.RoundToInt ((boundaryBounds.size.magnitude * 0.01f) * baseGroundCoverageDensity * zone.groundCoverageDensity);

        for (int i = 0; i < treeSpawnAmmount; i++) {
            Triangle triangle = triangles[Random.Range (0, triangles.Count)];
            Vector2 spawnPos = RandomWithinTriangle (triangle);
            GameObject tree = Instantiate (zone.trees[Random.Range (0, zone.trees.Length)], new Vector3 (spawnPos.x, 100f, spawnPos.y), Quaternion.identity);
            tree.transform.localScale = Vector3.one * Random.Range (zone.treeSizes.x, zone.treeSizes.y);
        }

        for (int i = 0; i < treeGroundCoverageAmmount; i++) {
            Triangle triangle = triangles[Random.Range (0, triangles.Count)];
            Vector2 spawnPos = RandomWithinTriangle (triangle);
            GameObject tree = Instantiate (zone.groundCoverage[Random.Range (0, zone.groundCoverage.Length)], new Vector3 (spawnPos.x, 100f, spawnPos.y), Quaternion.identity);
        }

        StartCoroutine (EndGeneration (voronoi, world, pointOfInterest));

    }

    IEnumerator EndGeneration (Voronoi voronoi, Generation world, PointOfInterest pointOfInterest) {
        yield return new WaitForSeconds (1f);
        pointOfInterest.FoliageGenerationCallback (voronoi, world);
    }

    private Vector2 RandomWithinTriangle (Triangle t) {
        var r1 = Mathf.Sqrt (Random.Range (0f, 1f));
        var r2 = Random.Range (0f, 1f);
        var m1 = 1 - r1;
        var m2 = r1 * (1 - r2);
        var m3 = r2 * r1;

        var p1 = t.sites[0].Coord;
        var p2 = t.sites[1].Coord;
        var p3 = t.sites[2].Coord;
        return (m1 * p1) + (m2 * p2) + (m3 * p3);
    }

}