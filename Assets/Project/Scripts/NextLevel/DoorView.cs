using UnityEngine;

namespace Project.Scripts.NextLevel
{
    public class DoorView : IDoorView
    {
        private readonly Collider _collider;

        public DoorView(Collider collider)
        {
            _collider = collider;
        }

        public void Enable() => _collider.enabled = true;
        public void Disable() => _collider.enabled = false;
    }
}