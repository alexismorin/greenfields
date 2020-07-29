using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour {

    [SerializeField]
    GameObject terrainTilePrefab;

    [Header ("World Size")]

    [SerializeField]
    int xTiles = 5;
    [SerializeField]
    int zTiles = 5;
    [SerializeField]
    float tileSize = 10f;

    [Header ("Terrain Deformation")]

    [SerializeField]
    PerlinNoise[] terrainNoises;

    [SerializeField]
    PerlinNoise moistureNoise;

    [Header ("Colours")]
    [SerializeField]
    Color grassColor = Color.green;
    [SerializeField]
    Color dampGrassColor = Color.yellow;
    [SerializeField]
    Color mudColor = Color.magenta;

    public void Generate (Generation world) {

        // We give the world generator the max world size as a future lookup for placement
        world.worldSize = new Vector2 (xTiles * tileSize, zTiles * tileSize);

        for (int x = 0; x < xTiles; x++) {
            for (int z = 0; z < zTiles; z++) {
                GameObject tileInstance = Instantiate (terrainTilePrefab, new Vector3 (x * tileSize, 0f, z * tileSize), Quaternion.identity);
                tileInstance.GetComponent<TerrainDerformer> ().Generate (world, grassColor, dampGrassColor, mudColor, terrainNoises, moistureNoise);
            }
        }
    }

}