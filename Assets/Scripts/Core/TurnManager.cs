using System;
using UnityEngine;

/// <summary>
/// Handles turn sequencing and asking the current player for a question.
/// This component exposes events so UI/Network layers can subscribe.
/// </summary>
public class TurnManager : MonoBehaviour
{
    public static event Action<PlayerController> OnTurnChanged;
    public int currentTurnIndex = 0;

    private bool isRunning = false;

    public void StartTurns()
    {
        currentTurnIndex = 0;
        isRunning = true;
        NotifyCurrentTurn();
    }

    public void StopTurns()
    {
        isRunning = false;
    }

    /// <summary>
    /// Called by GameManager after resolving the previous answer.
    /// This method advances the index to the next active player and notifies listeners.
    /// </summary>
    public void RequestNextTurn()
    {
        if (!isRunning) return;

        int attempts = 0;
        int playerCount = GameManager.Instance.players.Count;

        // find next active player (skip eliminated/cashed out)
        do
        {
            currentTurnIndex = (currentTurnIndex + 1) % playerCount;
            attempts++;

            // safety: if all players except one are eliminated/cashed out, stop
            if (attempts > playerCount)
            {
                // nothing to do — round manager should detect end conditions
                return;
            }
        } while (IsPlayerInactive(GameManager.Instance.players[currentTurnIndex]));

        NotifyCurrentTurn();
    }

    private bool IsPlayerInactive(PlayerController p)
    {
        return p == null || p.isEliminated || p.isCashedOut;
    }

    private void NotifyCurrentTurn()
    {
        var currentPlayer = GameManager.Instance.players[currentTurnIndex];
        OnTurnChanged?.Invoke(currentPlayer);

        // Optionally, if using local flow you can automatically call QuestionManager here:
        // QuestionManager.Instance.ShowQuestionFor(currentPlayer);
    }

    /// <summary>
    /// Force set the turn to a specific player index (useful for reconnection/networking).
    /// </summary>
    public void SetTurnIndex(int index)
    {
        currentTurnIndex = index;
        NotifyCurrentTurn();
    }
}
