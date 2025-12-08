using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Responsible for high-level round lifecycle:
/// - PrepareRound
/// - StartRound
/// - OnPlayerEaten (immediate elimination & end round)
/// - Handle end-of-round comparisons (cashouts, finishers)
/// - Promote players to next round
/// 
/// This does NOT handle UI details; it raises events other systems can subscribe to.
/// </summary>
public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance { get; private set; }

    public event Action OnRoundStarted;
    public event Action OnRoundFinished;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    /// <summary>
    /// Called at game start to set up/reset data.
    /// </summary>
    public void PrepareRound()
    {
        ResetPlayersForRound();
        GameManager.Instance.ResetGameState();
        StartRound();
    }

    public void StartRound()
    {
        // Reset lion and player states; then start turn manager
        ResetPlayersForRound();
        GameManager.Instance.lion?.ResetLion();

        // Notify listeners (UI)
        OnRoundStarted?.Invoke();

        // Start turn cycle
        GameManager.Instance.turnManager?.StartTurns();
    }

    private void ResetPlayersForRound()
    {
        foreach (var p in GameManager.Instance.players)
            p.ResetForNewRound();
    }

    /// <summary>
    /// Called when a player is eaten by the lion — immediate elimination and round ends.
    /// </summary>
    public void OnPlayerEaten(PlayerController eaten)
    {
        if (eaten == null) return;

        eaten.isEliminated = true;
        Debug.Log($"Player {eaten.playerID} eaten by lion and eliminated.");

        // Stop turn cycle
        GameManager.Instance.turnManager?.StopTurns();

        // Fire round finished event - other systems (UI/Network) will handle showing result
        OnRoundFinished?.Invoke();

        // Next round logic: remove player from active list OR let GameManager handle promotion.
        // Here we assume GameManager.players keeps all players and RoundManager or GameManager
        // removes or marks eliminated players when preparing next round.
    }

    /// <summary>
    /// Called when the round should finish normally (no lion kill),
    /// e.g. all finished / cashed out / or condition met.
    /// This method should be called by GameManager or UI when final checks done.
    /// </summary>
    public void FinishRound()
    {
        // Stop turn cycle
        GameManager.Instance.turnManager?.StopTurns();

        // Determine elimination by points if needed (lowest point out)
        HandleEndOfRoundElimination();

        // Notify listeners & prepare next round (or end game)
        OnRoundFinished?.Invoke();
    }

    private void HandleEndOfRoundElimination()
    {
        // Select players who are not yet eliminated (active or cashed out or finished)
        var alive = GameManager.Instance.players.Where(p => !p.isEliminated).ToList();

        if (alive.Count <= 3)
        {
            // Enough players, nothing to remove
            return;
        }

        // If someone was eaten, they've already been marked eliminated elsewhere
        // If no one was eaten, we eliminate the player with the lowest totalPoints
        PlayerController lowest = alive.OrderBy(p => p.totalPoints).FirstOrDefault();
        if (lowest != null)
        {
            lowest.isEliminated = true;
            Debug.Log($"Player {lowest.playerID} eliminated by lowest points ({lowest.totalPoints}).");
        }
    }

}
