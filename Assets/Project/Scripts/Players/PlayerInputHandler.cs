using UnityEngine;

namespace Project.Scripts.Players
{
    public class PlayerInputHandler
    {
        private readonly Joystick _joystick;

        public PlayerInputHandler(Joystick joystick)
        {
            _joystick = joystick;
        }

        public Vector3 GetInputDirection()
        {
            return new Vector3(_joystick.Horizontal, 0f, _joystick.Vertical).normalized;
        }
    }
}