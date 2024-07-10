using UnityEngine;
using UnityEngine.InputSystem;

namespace Infrastructure
{
    public class PlayerConfiguration
    {
        public PlayerConfiguration(PlayerInput input)
        {
            Input = input;
            Devices = input.devices.ToArray();
            ControlScheme = Input.currentControlScheme;
        }

        public SkinElement Skin { get; set; }
        public EyesElement Eyes { get; set; }
        public MouthElement Mouth { get; set; }

        public Color Color { get; set; }
        public int Number { get; set; }
        public int Position { get; set; } = -1;
        public int Score { get; set; } = 0;
        public bool IsReady { get; set; }

        public PlayerInput Input { get; private set; }
        public InputDevice[] Devices { get; private set; }
        public string ControlScheme { get; private set; }
    }
}