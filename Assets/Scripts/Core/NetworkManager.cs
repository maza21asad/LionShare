using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Offline version of the NetworkManager.
/// In Phase 1, it simply simulates 4 players locally.
/// In Phase 2, this will be replaced with Photon logic.
/// </summary>
public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance { get; private set; }

    public bool isMultiplayer = false;  // change to true when Photon added
    public int localPlayerIndex = 0;    // who is "me" when testing locally?

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Called when game starts. In offline mode, we spawn 4 fake players.
    /// In multiplayer mode (later), Photon will spawn real players.
    /// </summary>
    public void InitializePlayersOffline(List<PlayerController> players)
    {
        // assign test names, positions etc.
        players[0].InitializeOffline("You", 25000);
        players[1].InitializeOffline("nadir", 25000);
        players[2].InitializeOffline("jon 568", 25000);
        players[3].InitializeOffline("nida", 25000);
    }

    /// <summary>
    /// Called when player answers a question.
    /// Offline: Direct call.
    /// Online (later): Will send an RPC.
    /// </summary>
    public void SendPlayerAnswer(PlayerController player, bool isCorrect)
    {
        // Offline → directly call GameManager
        GameManager.instance.OnPlayerAnswered(player, isCorrect);
    }

    /// <summary>
    /// Offline simulation of stepping movements.
    /// Online version will sync positions later.
    /// </summary>
    public void BroadcastPlayerStep(PlayerController player, int newStep)
    {
        // Nothing required in offline mode.
        // Photon version will sync player position.
    }

    public void BroadcastLionStep(LionController lion, int newStep)
    {
        // Offline: do nothing.
        // Photon version will sync.
    }
}
