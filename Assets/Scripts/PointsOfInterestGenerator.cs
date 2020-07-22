using System.Collections;
using System.Collections.Generic;
using Delaunay;
using Delaunay.Geo;
using UnityEngine;

public class PointsOfInterestGenerator : MonoBehaviour {

    [SerializeField]
    GameObject pointOfInterestPrefab;

    [SerializeField]
    float spacing = 15f;

    [Range(0f, 1f)]
    [SerializeField]
    float margins = 0.9f;

    public void Generate(Generation world, Vector2 worldSize) {

        Vector3 offset = new Vector3(worldSize.x * (1f - margins), 0f, worldSize.x * (1f - margins));
        PoissonDiscSampler sampler = new PoissonDiscSampler(worldSize.x * margins, worldSize.y * margins, spacing);

        foreach (Vector2 sample in sampler.Samples()) {

            Instantiate(pointOfInterestPrefab, new Vector3(sample.x, 100, sample.y) + (offset / 2f), Quaternion.identity);
        }

        // Using a delaunay library, we find info for fences and paths
        GameObject[] allPointsOfInterest = GameObject.FindGameObjectsWithTag("PointOfInterest");

        List<Vector2> pointPositions = new List<Vector2>();
        List<uint> pointColors = new List<uint>();

        foreach (GameObject point in allPointsOfInterest) {
            pointColors.Add(0);
            pointPositions.Add(new Vector2(point.transform.position.x, point.transform.position.z));
        }

        Voronoi voronoi = new Voronoi(pointPositions, pointColors, new Rect(0, 0, world.worldSize.x, world.worldSize.y));

        foreach (var item in allPointsOfInterest) {
            item.GetComponent<PointOfInterest>().Generate(voronoi, world);
        }



    }

}