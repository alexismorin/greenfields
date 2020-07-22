using System.Collections;
using System.Collections.Generic;
using Delaunay;
using Delaunay.Geo;
using UnityEngine;

public class FenceGenerator : MonoBehaviour {


    [SerializeField]
    float fenceLength = 2f;

    public void Generate(Voronoi voronoi, Generation world, Zone zone) {

        List<LineSegment> fences = voronoi.VoronoiBoundaryForSite(new Vector2(transform.position.x, transform.position.z));

        for (int i = 0; i < fences.Count; i++) {
            if (world.builtFences.Contains(fences[i])) {
                fences.RemoveAt(i);
            }
        }

        for (int i = 0; i < fences.Count; i++) {

            Vector2 left = (Vector2) fences[i].p0;
            Vector2 right = (Vector2) fences[i].p1;
            Vector3 startPos = new Vector3(left.x, 100f, left.y);
            Vector3 endPos = new Vector3(right.x, 100f, right.y);

            float distance = Vector3.Distance(startPos, endPos);

            int ammountOfFences = Mathf.RoundToInt(distance / fenceLength);
            for (int fenceIndex = 0; fenceIndex < ammountOfFences; fenceIndex++) {
                Vector3 spawnPosition = Vector3.Lerp(startPos, endPos, 1f / ammountOfFences * fenceIndex);
                Quaternion spawnRotation = Quaternion.LookRotation(endPos - spawnPosition);
                GameObject fenceInstance = Instantiate(zone.fence, spawnPosition, spawnRotation);
            }

        }

        foreach (var fence in fences) {
            world.builtFences.Add(fence);
        }


    }


}