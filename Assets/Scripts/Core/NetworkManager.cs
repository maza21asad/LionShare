using System;
using UnityEngine;

/// <summary>
/// Lightweight network abstraction. You can expand this to integrate Photon/NGO later.
/// Two modes supported:
/// - Local (single device testing)
/// - Photon (toggle a compile symbol or expand with Photon PUN calls)
/// 
/// The goal: keep networking code separate from GameManager/TurnManager logic.
/// </summary>
public class NetworkManager : MonoBehaviour
{
    public enum Mode { Local, Photon }
    public Mode mode = Mode.Local;

    // Events you can subscribe to for networked messages
    public static event Action<int, bool> OnRemoteAnswerReceived; // playerId, isCorrect
    public static event Action<int> OnRemoteCashOut; // playerId

    #region Local (single-machine) helpers
    /// <summary>
    /// Local invocation used in single-machine testing: emulate a player answering.
    /// </summary>
    public void LocalPlayerAnswered(int playerId, bool isCorrect)
    {
        // For local mode simply forward to GameManager
        var player = FindPlayerById(playerId);
        if (player != null)
        {
            GameManager.Instance.OnPlayerAnswered(player, isCorrect);
        }
    }

    public void LocalPlayerCashOut(int playerId)
    {
        var p = FindPlayerById(playerId);
        if (p != null) p.CashOut();
    }

    private PlayerController FindPlayerById(int id)
    {
        foreach (var p in GameManager.Instance.players)
            if (p.playerID == id) return p;
        return null;
    }
    #endregion

    #region Photon / Network placeholders
    // TODO: implement Photon RPCs here. Keep GameManager logic network-agnostic:
    // - Broadcast player's answer
    // - Receive other player's answer via event and call GameManager.OnPlayerAnswered locally
    #endregion
}
