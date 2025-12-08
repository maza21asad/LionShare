using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Central coordinator for game flow. Keeps references to managers and exposes the main API:
/// - StartRound
/// - OnPlayerAnswered
/// - RequestNextTurn
/// 
/// It does NOT contain UI code. UIManager / TurnManager / RoundManager handle visuals/turns separately.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("References (assign in inspector)")]
    public TurnManager turnManager;
    public RoundManager roundManager;
    public NetworkManager networkManager;

    [HideInInspector] public List<PlayerController> players = new List<PlayerController>();
    [HideInInspector] public LionController lion;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // RoundManager will call StartRound when ready. But provide convenience:
        if (roundManager != null)
            roundManager.PrepareRound();
    }

    /// <summary>
    /// Register players (called when player prefabs are instantiated / assigned).
    /// Order matters: players list order defines turn order.
    /// </summary>
    public void RegisterPlayers(List<PlayerController> playerList)
    {
        players = new List<PlayerController>(playerList);
    }

    public void RegisterLion(LionController lionController)
    {
        lion = lionController;
    }

    /// <summary>
    /// Called by QuestionManager (or UI) when a player answers a question.
    /// </summary>
    /// <param name="player">player who answered</param>
    /// <param name="isCorrect">true if correct</param>
    public void OnPlayerAnswered(PlayerController player, bool isCorrect)
    {
        if (player == null || player.isEliminated || player.isCashedOut)
        {
            // ignore answers from inactive players
            turnManager.RequestNextTurn();
            return;
        }

        if (isCorrect)
        {
            player.HandleCorrectAnswer();
        }
        else
        {
            player.HandleWrongAnswer();

            // Lion reacts only when player.step >= 2 (handled in LionController if needed)
            if (player.step >= 2 && lion != null)
            {
                lion.HandleWrongAnswer();
            }
        }

        // Check collision with lion immediately after the lion potentially moved
        //if (lion != null)
        //{
        //    PlayerController caught = lion.CheckCollision(players);
        //    if (caught != null)
        //    {
        //        // Inform round manager — this will end the round immediately as per rules
        //        roundManager.OnPlayerEaten(caught);
        //        return;
        //    }
        //}

        // Normal progression: ask next player / next turn
        turnManager.RequestNextTurn();
    }

    /// <summary>
    /// Helper to force end of round (used by RoundManager).
    /// </summary>
    public void EndRound()
    {
        // RoundManager will handle elimination logic and start the next round when ready.
        roundManager.FinishRound();
    }

    #region Utility / Debug

    // Quick reset helper (editor/testing)
    [ContextMenu("Reset GameState")]
    public void ResetGameState()
    {
        foreach (var p in players)
        {
            p.ResetForNewRound();
        }

        if (lion != null) lion.ResetLion();
    }

    #endregion
}
