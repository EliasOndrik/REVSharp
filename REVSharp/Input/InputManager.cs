
using Silk.NET.Input;
using Silk.NET.Maths;

namespace REVSharp.Input
{
    public interface IInputManager
    {
        public bool IsKeyPressed(Key key);
        public bool IsKeyReleased(Key key);
        public bool IsKeyJustPressed(Key key);
        public bool IsKeyJustReleased(Key key);

        public bool IsMouseButtonPressed(MouseButton button);

        public bool IsMouseButtonReleased(MouseButton button);

        public Vector2D<float> GetMousePosition();

        public float GetMouseScroll();
    }
    public class InputManager : IInputManager
    {
        private readonly IInputContext _inputContext;
        public IKeyboard? _keyboard;
        public IMouse? _mouse;
        private readonly List<Key> _pressedKeys;
        private readonly List<MouseButton> _pressedMouseButtons;
        public InputManager(IInputContext inputContext)
        {
           
            _inputContext = inputContext;
            _keyboard = inputContext.Keyboards[0];
            _mouse = inputContext.Mice[0];
            _pressedKeys = [];
            _pressedMouseButtons = [];
        }

        public bool IsKeyPressed(Key key)
        {
            
            if (_keyboard == null)
            {
                return false;
            }
            bool isPressed = _keyboard.IsKeyPressed(key);
            if (isPressed)
            {
                if (!_pressedKeys.Contains(key))
                {
                    _pressedKeys.Add(key);
                }
            }
            else
            {
                _pressedKeys.Remove(key);
            }
            return isPressed;
        }
        public bool IsKeyReleased(Key key)
        {
            if (_keyboard == null)
            {
                return false;
            }
            return !IsKeyPressed(key);
        }
        public bool IsKeyJustPressed(Key key)
        {
            if (_keyboard == null)
            {
                return false;
            }
            return !_pressedKeys.Contains(key) && IsKeyPressed(key);
        }
        public bool IsKeyJustReleased(Key key)
        {
            if (_keyboard == null)
            {
                return false;
            }
            return _pressedKeys.Contains(key) && !IsKeyPressed(key);
        }
        
        public bool IsMouseButtonPressed(MouseButton button)
        {
            
            if (_mouse == null)
            {
                return false;
            }
            bool isPressed = _mouse.IsButtonPressed(button);
            if (isPressed)
            {
                if (!_pressedMouseButtons.Contains(button))
                {
                    _pressedMouseButtons.Add(button);
                }
            }
            else
            {
                _pressedMouseButtons.Remove(button);
            }
            return isPressed;
        }

        public bool IsMouseButtonReleased(MouseButton button)
        {
            if (_mouse == null)
            {
                return false;
            }
            return !IsMouseButtonPressed(button);
        }

        public Vector2D<float> GetMousePosition()
        {
            Vector2D<float> position = new(0, 0);
            if (_mouse == null)
            {
                return position;
            }

            position.X = _mouse.Position.X;
            position.Y = _mouse.Position.Y;
            return position;
        }

        public float GetMouseScroll()
        {
            if (_mouse == null)
            {
                return 0;
            }
            return _mouse.ScrollWheels[0].Y;
        }


    }
}
