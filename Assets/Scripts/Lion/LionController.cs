using UnityEngine;

public class LionController : MonoBehaviour
{
    public Transform lionObject;
    public Transform stepsRoot;

    public int lionStep = -1;        // behind the player

    public void MoveCloser(int amount)
    {
        lionStep += amount;
        lionStep = Mathf.Min(lionStep, stepsRoot.childCount - 1);

        lionObject.position = stepsRoot.GetChild(lionStep).position;
    }

    public bool IsCaughtUp(int playerStep)
    {
        return lionStep >= playerStep;
    }
}
