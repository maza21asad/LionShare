using UnityEngine;

/// <summary>
/// Holds individual player data + handles point logic and progression.
/// This is the ONLY class that modifies points/steps.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Player Info")]
    public int playerID;
    public string playerName;

    [Header("Status Flags")]
    public bool isEliminated = false;
    public bool isCashedOut = false;
    public bool isFinished = false;

    [Header("Game State")]
    public int step = 1;              // step 1 → 9
    public float stepRemainingPoints; // points left after deduction
    public int totalPoints;           // accumulated correct points

    // INTERNAL TABLE – points per step (1–9)
    private readonly int[] stepBasePoints = new int[]
    {
        0,      // UNUSED (index 0)
        100,    // step 1
        200,    // step 2
        400,    // step 3
        800,    // step 4
        1600,   // step 5
        3200,   // step 6
        6400,   // step 7
        12800,  // step 8
        25600   // step 9
    };

    // Called when new round begins
    public void ResetForNewRound()
    {
        isEliminated = false;
        isCashedOut = false;
        isFinished = false;

        step = 1;
        totalPoints = 0;
        stepRemainingPoints = stepBasePoints[step];
    }

    // Called on wrong answer
    public void HandleWrongAnswer()
    {
        if (isEliminated || isCashedOut || isFinished) return;

        // Deduct 80%
        stepRemainingPoints *= 0.2f; // Keep only 20%

        // This step does NOT advance
    }

    // Called on correct answer
    public void HandleCorrectAnswer()
    {
        if (isEliminated || isCashedOut || isFinished) return;

        // Add remaining step points
        totalPoints += Mathf.RoundToInt(stepRemainingPoints);

        // Move to next step
        step++;

        if (step > 9)
        {
            // Step 9 completed — stop moving
            isFinished = true;
            step = 9;
            return;
        }

        // Reset next step's full points
        stepRemainingPoints = stepBasePoints[step];
    }

    // Called when entering safe zone (step >= 6)
    public void CashOut()
    {
        if (step < 6) return; // not allowed
        if (isEliminated || isCashedOut || isFinished) return;

        isCashedOut = true;

        // Player keeps their totalPoints and stops moving
    }
}
