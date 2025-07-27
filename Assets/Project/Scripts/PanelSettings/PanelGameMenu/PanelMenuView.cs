using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelMenuView : MonoBehaviour
{
    [SerializeField] private Button _startGameButton;
    [SerializeField] private Button _removeAdsButton;
    [SerializeField] private TextMeshProUGUI _logText;

    public event Action StartGameClicked;
    public event Action RemoveADSClicked;

    public void Awake()
    {
        _startGameButton.onClick.AddListener(OnStartGameClicked);
        _removeAdsButton.onClick.AddListener(OnRemoveADSClicked);
    }

    private void OnDestroy()
    {
        _startGameButton.onClick.RemoveListener(OnStartGameClicked);
        _removeAdsButton.onClick.RemoveListener(OnRemoveADSClicked);
    }

    private void OnStartGameClicked()
    {
        StartGameClicked?.Invoke();
    }

    private void OnRemoveADSClicked()
    {
        RemoveADSClicked?.Invoke();
    }
}
