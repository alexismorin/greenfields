using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Perlin Noise", menuName = "Perlin Noise")]
public class PerlinNoise : ScriptableObject {

    public float size = 0.01f;
    public float strength = 1f;
    public Vector2 offset = Vector2.zero;
    public AnimationCurve falloffRemapCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    public float ReturnNoise(Vector3 samplePos) {
        return falloffRemapCurve.Evaluate(Mathf.PerlinNoise((samplePos.x + offset.x) * size, (samplePos.z + offset.y) * size)) * strength;
    }

}