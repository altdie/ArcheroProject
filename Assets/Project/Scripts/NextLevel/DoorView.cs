using UnityEngine;

namespace Project.Scripts.NextLevel
{
    public class DoorView : IDoorView
    {
        private readonly Collider _collider;
        private readonly OnTriggerChecker _triggerChecker;

        public DoorView(Collider collider, OnTriggerChecker onTriggerChecker)
        {
            _collider = collider;
            _triggerChecker = onTriggerChecker;
            Disable();
        }

        public void Enable()
        {
            _collider.enabled = true;
            _triggerChecker.Activate();
        }

        public void Disable() => _collider.enabled = false;
    }
}