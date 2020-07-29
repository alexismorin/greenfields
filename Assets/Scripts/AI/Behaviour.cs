using UnityEngine;

[CreateAssetMenu (fileName = "AI Behaviour", menuName = "AI Behaviour", order = 1)]
public class SpawnManagerScriptableObject : ScriptableObject {

    [SerializeField]
    bool wander = false;

    [Header ("Loop Behaviour")]

    [SerializeField]
    string seekNearest = null;

    [SerializeField]
    float satisfactionDistance = 2f;

    [Header ("On Goal Acheivement")]

    Behaviour nextState;

    [Header ("Effects")]
    [Range (0f, 1f)]
    float healthCost = 0f;
    [Range (0f, 1f)]
    float energyCost = 0.0025f;
    [Range (0f, 1f)]
    float hungerCost = 0.005f;
    [Range (0f, 1f)]
    float thirstCost = 0.01f;

    public void Behave (AI ai) {

    }
}