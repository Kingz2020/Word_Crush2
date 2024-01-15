using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayHandler: MonoBehaviour {

    [SerializeField] private TurnManager _turnManager;
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private TextMeshProUGUI currentRound;

    void Start() {
        SetPlayerName();
        SetCurrentRound();
    }

    private void Awake() {
        _turnManager.OnTurnEnd.AddListener(SetPlayerName);
        _turnManager.OnTurnEnd.AddListener(SetCurrentRound);
        
    }

    void SetPlayerName() {
        playerName.text = _turnManager.GetActivePlayerName();
    }

    void SetCurrentRound() {
        currentRound.text = _turnManager.GetRoundNumber().ToString();
    }
}