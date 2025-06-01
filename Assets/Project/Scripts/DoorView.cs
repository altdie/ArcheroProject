using UnityEngine;

namespace Project.Scripts
{
    public class DoorView : IDoorView
    {
        private Collider _collider;

        public DoorView(Collider collider)
        {
            _collider = collider;
        }

        public void Enable() => _collider.enabled = true;
        public void Disable() => _collider.enabled = false;
    }
}