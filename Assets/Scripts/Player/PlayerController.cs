using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public string playerName;
    public Sprite avatar; 

    [Header("Track")]
    public Transform stepsRoot;
    public Transform pawn;
    public int currentStep = 0;

    [Header("Lion")]
    public LionController lion;

    [Header("Score")]
    public int points = 0;
    public int currentCoins = 0;   // FIXED
    public bool isEliminated = false;

    public void MoveForward(int amount)
    {
        currentStep += amount;
        currentStep = Mathf.Min(currentStep, stepsRoot.childCount - 1);

        pawn.position = stepsRoot.GetChild(currentStep).position;
    }

    public void MoveLionCloser(int amount)
    {
        lion.MoveCloser(amount);

        if (lion.IsCaughtUp(currentStep))
        {
            isEliminated = true;
            RoundManager.instance.PlayerEliminated(this);
        }
    }

    public bool ReachedFinish()
    {
        return currentStep >= stepsRoot.childCount - 1;
    }

    public void InitializeOffline(string name, int coins)
    {
        playerName = name;
        currentCoins = coins;   // FIXED
    }
}
