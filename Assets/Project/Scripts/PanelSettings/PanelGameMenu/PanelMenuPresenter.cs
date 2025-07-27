using System.Collections;
using System.Collections.Generic;
using Project.Scripts.PanelSettings;
using UnityEngine;
using Zenject;

public class PanelMenuPresenter : IInitializable
{
    private readonly PanelMenuView _view;
    private readonly PanelMenuModel _model;

    public PanelMenuPresenter(PanelMenuView view, PanelMenuModel model)
    {
        _view = view;
        _model = model;
    }

    public void Initialize()
    {
        SubscribeOnClick();
    }

    public void SubscribeOnClick()
    {
        _view.RemoveADSClicked += OnRemoveADSClicked;
        _view.StartGameClicked += OnStartGameClicked;
    }

    private void OnRemoveADSClicked()
    {
       _model.RemoveADS();
    }

    private void OnStartGameClicked()
    {
        _model.StartGame();
        Dispose();
    }

    public void Dispose()
    {
        _view.RemoveADSClicked -= OnRemoveADSClicked;
        _view.StartGameClicked -= OnStartGameClicked;
    }
}
