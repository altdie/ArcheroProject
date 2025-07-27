using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.Scripts.ADS;
using Project.Scripts.GameFlowScripts;
using TMPro;
using Unity.Services.Authentication;
using UnityEngine;

public class PanelMenuModel
{
    private readonly SceneLoader _sceneLoader;
    private TextMeshProUGUI _logTxt;

    public PanelMenuModel(SceneLoader sceneLoader, TextMeshProUGUI logTxt)
    {
        _sceneLoader = sceneLoader;
        _logTxt = logTxt;
    }

    public void StartGame()
    {
        _sceneLoader.StartGame();
    }

    public void RemoveADS()
    {
        _logTxt.text = "Player id:" + AuthenticationService.Instance.PlayerId;
    }
}
