using System.Diagnostics;

using Intersect.Client.Framework.Gwen.Control;
using Intersect.Client.Framework.Gwen.Input;
using Intersect.Client.Framework.Input;
using Intersect.Client.General;
using Intersect.Client.MonoGame.Graphics;
using Intersect.Client.ThirdParty;
using Intersect.Logging;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Keys = Intersect.Client.Framework.GenericClasses.Keys;
using MonoKeys = Microsoft.Xna.Framework.Input.Keys;
using Rectangle = Intersect.Client.Framework.GenericClasses.Rectangle;

namespace Intersect.Client.MonoGame.Input;

public partial class MonoInput : GameInput
{
    private Game _myGame;
    
    private readonly Dictionary<Keys, MonoKeys> _intersectKeys;

    private KeyboardState _lastKeyboardState;

    private MouseState _lastMouseState;

    public Point PointerPosition = new();

    public Point PointerScroll = new();

    private readonly GamePadCapabilities[] _gamePadCapabilities = new GamePadCapabilities[
        GamePad.MaximumGamePadCount
    ];

    private bool _keyboardOpened;

    public MonoInput(Game myGame)
    {
        _myGame = myGame;
        _intersectKeys = [];
        
        foreach (Keys key in Enum.GetValues(typeof(Keys)))
        {
            if (!_intersectKeys.ContainsKey(key))
            {
                foreach (MonoKeys monoKey in Enum.GetValues(typeof(MonoKeys)))
                {
                    if (key == Keys.Shift)
                    {
                        _intersectKeys.TryAdd(key, MonoKeys.LeftShift);
                        break;
                    }

                    if (key is Keys.Control or Keys.LControlKey)
                    {
                        _intersectKeys.TryAdd(key, MonoKeys.LeftControl);
                        break;
                    }

                    if (key == Keys.RControlKey)
                    {
                        _intersectKeys.TryAdd(key, MonoKeys.RightControl);
                        break;
                    }

                    if (key == Keys.Return)
                    {
                        _intersectKeys.TryAdd(key, MonoKeys.Enter);
                        break;
                    }

                    if (key.ToString() != monoKey.ToString())
                    {
                        continue;
                    }

                    _intersectKeys.TryAdd(key, monoKey);
                    break;
                }
            }

            if (!_intersectKeys.ContainsKey(key))
            {
                Debug.WriteLine("Mono does not have a key to match: " + key);
            }
        }

        InputHandler.FocusChanged += InputHandlerOnFocusChanged;
    }

    private void InputHandlerOnFocusChanged(Base? control, FocusSource focusSource)
    {
        if (control == default)
        {
            return;
        }

        if (focusSource == FocusSource.Mouse)
        {
            return;
        }

        Vector2 center = new(
            (control.BoundsGlobal.Left + control.BoundsGlobal.Right) / 2f,
            (control.BoundsGlobal.Bottom + control.BoundsGlobal.Top) / 2f
        );

        Mouse.SetPosition((int)center.X, (int)center.Y);
    }

    public override bool IsPointerDown(MouseButtons mb) => mb switch
    {
        MouseButtons.Left => _lastMouseState.LeftButton == ButtonState.Pressed,
        MouseButtons.Right => _lastMouseState.RightButton == ButtonState.Pressed,
        MouseButtons.Middle => _lastMouseState.MiddleButton == ButtonState.Pressed,
        MouseButtons.X1 => _lastMouseState.XButton1 == ButtonState.Pressed,
        MouseButtons.X2 => _lastMouseState.XButton2 == ButtonState.Pressed,
        _ => throw new ArgumentOutOfRangeException(nameof(mb), mb, null)
    };

    public override bool IsKeyDown(Keys key) => _intersectKeys.ContainsKey(key) && _lastKeyboardState.IsKeyDown(_intersectKeys[key]);

    private void CheckMouseButton(Keys modifier, ButtonState bs, MouseButtons mb)
    {
        if (Globals.GameState == GameStates.Intro)
        {
            //No mouse input allowed while showing intro slides
            return;
        }

        if (bs == ButtonState.Pressed && !IsPointerDown(mb))
        {
            Core.Input.OnMouseDown(modifier, mb);
        }
        else if (bs == ButtonState.Released && IsPointerDown(mb))
        {
            Core.Input.OnMouseUp(modifier, mb);
        }
    }

    private void CheckMouseScrollWheel(int scrlVValue, int scrlHValue)
    {
        if (scrlVValue != PointerScroll.Y || scrlHValue != PointerScroll.X)
        {
            PointerScroll.Y = scrlVValue;
            PointerScroll.X = scrlHValue;
        }
    }

    public override void Update(TimeSpan elapsed)
    {
        if (_myGame.IsActive)
        {
            if (_keyboardOpened)
            {
                Steam.PumpEvents();
            }

            var gamePadCapabilities = Enumerable
                .Range(0, GamePad.MaximumGamePadCount)
                .Select(GamePad.GetCapabilities)
                .ToArray();

            Array.Copy(
                gamePadCapabilities,
                _gamePadCapabilities,
                Math.Min(gamePadCapabilities.Length, _gamePadCapabilities.Length)
            );

            var gamePadState = Enumerable
                .Range(0, GamePad.MaximumGamePadCount)
                .Select(GamePad.GetState)
                .FirstOrDefault(gamePad => gamePad.IsConnected);

            if (gamePadState.IsConnected)
            {
                var deltaX = (int)(gamePadState.ThumbSticks.Right.X * elapsed.TotalSeconds * 1000);
                var deltaY = (int)(-gamePadState.ThumbSticks.Right.Y * elapsed.TotalSeconds * 1000);

                var temporaryMouseState = Mouse.GetState();
                Mouse.SetPosition(temporaryMouseState.X + deltaX, temporaryMouseState.Y + deltaY);
            }

            var keyboardState = Keyboard.GetState();
            var mouseState = Mouse.GetState();

            if (mouseState.X != PointerPosition.X || mouseState.Y != PointerPosition.Y)
            {
                PointerPosition = new(
                    (int)(mouseState.X * ((MonoRenderer)Core.Graphics.Renderer!).GetMouseOffset().X),
                    (int)(mouseState.Y * ((MonoRenderer)Core.Graphics.Renderer).GetMouseOffset().Y)
                );
            }

            if (gamePadState.IsConnected)
            {
                var leftMouseButton = gamePadState.Buttons.A == ButtonState.Released
                    ? mouseState.LeftButton
                    : ButtonState.Pressed;

                var rightMouseButton = gamePadState.Buttons.B == ButtonState.Released
                    ? mouseState.RightButton
                    : ButtonState.Pressed;

                mouseState = new MouseState(
                    mouseState.X,
                    mouseState.Y,
                    mouseState.ScrollWheelValue,
                    leftMouseButton,
                    mouseState.MiddleButton,
                    rightMouseButton,
                    mouseState.XButton1,
                    mouseState.XButton2,
                    mouseState.HorizontalScrollWheelValue
                );

                var gamePadKeys = Enum.GetValues<Buttons>()
                    .Where(gamePadState.IsButtonDown)
                    .Select(
                        button => button switch
                        {
                            Buttons.None => MonoKeys.None,
                            Buttons.DPadUp => MonoKeys.Up,
                            Buttons.DPadDown => MonoKeys.Down,
                            Buttons.DPadLeft => MonoKeys.Left,
                            Buttons.DPadRight => MonoKeys.Right,
                            Buttons.Start => MonoKeys.Escape,
                            Buttons.Back => MonoKeys.None,
                            Buttons.LeftStick => MonoKeys.None,
                            Buttons.RightStick => MonoKeys.None,
                            Buttons.LeftShoulder => MonoKeys.Back,
                            Buttons.RightShoulder => MonoKeys.Tab,
                            Buttons.BigButton => MonoKeys.None,
                            Buttons.A => MonoKeys.Enter,
                            Buttons.B => MonoKeys.Back,
                            Buttons.X => MonoKeys.E,
                            Buttons.Y => MonoKeys.None,
                            Buttons.LeftThumbstickLeft => MonoKeys.None,
                            Buttons.RightTrigger => MonoKeys.None,
                            Buttons.LeftTrigger => MonoKeys.None,
                            Buttons.RightThumbstickUp => MonoKeys.None,
                            Buttons.RightThumbstickDown => MonoKeys.None,
                            Buttons.RightThumbstickRight => MonoKeys.None,
                            Buttons.RightThumbstickLeft => MonoKeys.None,
                            Buttons.LeftThumbstickUp => MonoKeys.None,
                            Buttons.LeftThumbstickDown => MonoKeys.None,
                            Buttons.LeftThumbstickRight => MonoKeys.None,
                            _ => throw new ArgumentOutOfRangeException(nameof(button), button, null)
                        }
                    );

                keyboardState = new KeyboardState(
                    [.. keyboardState.GetPressedKeys(), .. gamePadKeys],
                    keyboardState.CapsLock,
                    keyboardState.NumLock
                );
            }

            // Get what modifier key we're currently pressing.
            var modifier = GetPressedModifier(keyboardState);

            //Check for state changes in the left mouse button
            CheckMouseButton(modifier, mouseState.LeftButton, MouseButtons.Left);
            CheckMouseButton(modifier, mouseState.RightButton, MouseButtons.Right);
            CheckMouseButton(modifier, mouseState.MiddleButton, MouseButtons.Middle);
            CheckMouseButton(modifier, mouseState.XButton1, MouseButtons.X1);
            CheckMouseButton(modifier, mouseState.XButton2, MouseButtons.X2);

            CheckMouseScrollWheel(mouseState.ScrollWheelValue, mouseState.HorizontalScrollWheelValue);

            foreach (var key in _intersectKeys)
            {
                if (keyboardState.IsKeyDown(key.Value) && !_lastKeyboardState.IsKeyDown(key.Value))
                {
                    Log.Diagnostic("{0} -> {1}", key.Key, key.Value);
                    Core.Input.OnKeyPressed(modifier, key.Key);
                }
                else if (
                    !keyboardState.IsKeyDown(key.Value) && _lastKeyboardState.IsKeyDown(key.Value)
                )
                {
                    Core.Input.OnKeyReleased(modifier, key.Key);
                }
            }

            _lastKeyboardState = keyboardState;
            _lastMouseState = mouseState;
        }
        else
        {
            var modifier = GetPressedModifier(_lastKeyboardState);

            foreach (var key in _intersectKeys)
            {
                if (_lastKeyboardState.IsKeyDown(key.Value))
                {
                    Core.Input.OnKeyReleased(modifier, key.Key);
                }
            }

            CheckMouseButton(modifier, ButtonState.Released, MouseButtons.Left);
            CheckMouseButton(modifier, ButtonState.Released, MouseButtons.Right);
            CheckMouseButton(modifier, ButtonState.Released, MouseButtons.Middle);
            CheckMouseButton(modifier, ButtonState.Released, MouseButtons.X1);
            CheckMouseButton(modifier, ButtonState.Released, MouseButtons.X2);

            _lastKeyboardState = new KeyboardState();
            _lastMouseState = new MouseState();
        }
    }

    public Keys GetPressedModifier(KeyboardState state)
    {
        var modifier = Keys.None;
        if (state.IsKeyDown(MonoKeys.LeftControl) || state.IsKeyDown(MonoKeys.RightControl))
        {
            modifier = Keys.Control;
        }

        if (state.IsKeyDown(MonoKeys.LeftShift) || state.IsKeyDown(MonoKeys.RightShift))
        {
            modifier = Keys.Shift;
        }

        if (state.IsKeyDown(MonoKeys.LeftAlt) || state.IsKeyDown(MonoKeys.RightAlt))
        {
            modifier = Keys.Alt;
        }

        return modifier;
    }

    public override void OpenKeyboard(
        KeyboardType type,
        string text,
        bool autoCorrection,
        bool multiLine,
        bool secure
    )
    {
        //no on screen keyboard for pc clients
    }

    public override void OpenKeyboard(
        KeyboardType keyboardType,
        Action<string?> inputHandler,
        string description,
        string text,
        bool multiline = false,
        uint maxLength = 1024,
        Rectangle? inputBounds = default
    )
    {
        if (!Steam.SteamDeck)
        {
            return;
        }

        _keyboardOpened = Steam.ShowKeyboard(
            inputHandler,
            description,
            existingInput: text,
            keyboardType == KeyboardType.Password,
            maxLength,
            inputBounds
        );
    }
}