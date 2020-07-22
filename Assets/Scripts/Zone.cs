using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Zone", menuName = "Greenfields/Zone")]
public class Zone : ScriptableObject {

    [Header("Colours")]
    public bool tintGround = false;
    public Color groundColour = Color.white;
    public Color wetGroundColour = Color.white;
    public Color roadColour = Color.white;

    [Header("Fencing")]

    public GameObject fence;
    public GameObject brokenFence;

    [Header("Buildings")]

    public GameObject[] buildings;

    [Header("Foliage")]

    [Range(0f, 1f)]
    public float treeDensity = 1f;
    public GameObject[] trees;
    public Vector2 treeSizes = Vector2.one;

    [Space(10f)]

    [Range(0f, 1f)]
    public float groundCoverageDensity = 1f;
    public GameObject[] groundCoverage;

    [Header("Misc")]
    public float decorationDensity = 0.5f;
    public GameObject[] decoration;
}