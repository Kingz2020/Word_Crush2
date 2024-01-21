using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayHandler: MonoBehaviour {

    [SerializeField] private TurnManager _turnManager;
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private TextMeshProUGUI currentRound;
    [SerializeField] public TextMeshProUGUI score_0;
    [SerializeField] public TextMeshProUGUI score_1;

    private void Start() {
        _turnManager.OnTurnEnd.AddListener(SetPlayerName);
        _turnManager.OnTurnEnd.AddListener(SetCurrentRound);
        
    }

    public void ResetDisplay() {
        SetPlayerName();
        SetCurrentRound();
    }

    void SetPlayerName() {
        playerName.text = _turnManager.GetActivePlayerName() + "(" + _turnManager.GetActivePlayerPoints() + ")";
        //score_0.text = _turnManager.GetActivePlayerPoints().ToString();
    }

    void SetCurrentRound() {
        currentRound.text = _turnManager.GetRoundNumber().ToString();
    }
}
