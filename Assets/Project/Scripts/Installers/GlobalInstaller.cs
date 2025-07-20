using System.Collections;
using System.Collections.Generic;
using Project.Scripts;
using Project.Scripts.Players;
using Project.Scripts.SaveSystem;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Installers
{
    public class GlobalInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<PlayerDataSave>().AsSingle().NonLazy();
            Container.Bind<CloudSave>().AsSingle();
            Container.Bind<SaveSelection>().AsSingle();
            Container.Bind<PlayerPrefsSave>().AsSingle().NonLazy();

        }
    }
}
