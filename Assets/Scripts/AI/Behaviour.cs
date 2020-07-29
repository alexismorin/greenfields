using UnityEngine;

[CreateAssetMenu (fileName = "AI Behaviour", menuName = "AI Behaviour", order = 1)]
public class SpawnManagerScriptableObject : ScriptableObject {

    [Header ("Loop Behaviour")]

    [SerializeField]
    float behaviourTime = 5f;

    [Tooltip ("Searches the nearest object of this tag and walks toward it. If set to wander the character will do as such and use the satisfation distance as a range")]
    [SerializeField]
    string seekNearest = null;

    [Tooltip ("How close to the target does one have to be to acheive the current goal. ")]
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