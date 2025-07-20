using Project.Scripts.GameFlowScripts;
using Project.Scripts.Players;
using UnityEngine;
using Zenject;

namespace Project.Scripts.NextLevel
{
    public class OnTriggerChecker : MonoBehaviour
    {
        private bool _isActive;
        private SceneLoader _sceneLoader;

        [Inject]
        public void Construct(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        public void Activate()
        {
            _isActive = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_isActive)
            {
                return;
            }

            if (other.TryGetComponent(out PlayerMovement player))
            {
                _sceneLoader.ReloadScene();
            }
        }
    }
}
