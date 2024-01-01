#region

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

#endregion

// my old trero keymap https://github.com/Laamy/Trero/blob/master/Trero/ClientBase/KeyBase/Keymap.cs
internal class Keymap
{
    public EventHandler<KeyEvent> OnKeyEvent;

    private readonly Dictionary<char, uint> _dBuff = new Dictionary<char, uint>();
    private readonly Dictionary<char, bool> _noKey = new Dictionary<char, bool>();

    private readonly Dictionary<char, uint> _rBuff = new Dictionary<char, uint>();
    private readonly Dictionary<char, bool> _yesKey = new Dictionary<char, bool>(); // this is over complicated for smth u only need 1 dictionary for but its old so

    public static bool GetDown(Keys key)
    {
        return BattlefieldClient.Keymap._dBuff[(char)key] > 0;
    }

    public Keymap()
    {
        for (var c = (char)0; c < 0xFF; c++)
        {
            _rBuff.Add(c, 0);
            _dBuff.Add(c, 0);
            _noKey.Add(c, true);
            _yesKey.Add(c, true);
        }

        Program.BackgroundTick += keyTick;
    }

    private void keyTick(object sender, EventArgs e)
    {
        for (var c = (char)0; c < 0xFF; c++)
        {
            _noKey[c] = true;
            _yesKey[c] = false;

            if (User32.GetAsyncKeyState(c))
            {
                if (OnKeyEvent != null)
                    if (BattlefieldClient.isFocused)
                        OnKeyEvent.Invoke(this, new KeyEvent(c, VKeyCodes.KeyHeld));

                if (_dBuff[c] > 0)
                    continue;

                _dBuff[c]++;
                _noKey[c] = false;

                if (OnKeyEvent != null)
                    if (BattlefieldClient.isFocused)
                        OnKeyEvent.Invoke(this, new KeyEvent(c, VKeyCodes.KeyDown));
            }
            else
            {
                if (_rBuff[c] > 0)
                    continue;

                _rBuff[c]++;
                _yesKey[c] = true;

                if (OnKeyEvent != null)
                    if (BattlefieldClient.isFocused)
                        OnKeyEvent.Invoke(this, new KeyEvent(c, VKeyCodes.KeyUp));
            }

            if (_noKey[c])
                _dBuff[c] = 0;

            if (!_yesKey[c])
                _rBuff[c] = 0;
        }
    }

    public void SimulateRelease(ushort keyCode)
    {
        INPUT[] inputs = new INPUT[1];
        inputs[0].type = 1;
        inputs[0].u.ki.wVk = keyCode;
        inputs[0].u.ki.dwFlags = 2;

        User32.SendInput(1, inputs, Marshal.SizeOf(typeof(INPUT)));
    }

    public void SimulatePress(ushort keyCode)
    {
        INPUT[] inputs = new INPUT[1];
        inputs[0].type = 1;
        inputs[0].u.ki.wVk = keyCode;
        inputs[0].u.ki.dwFlags = 0;

        User32.SendInput(1, inputs, Marshal.SizeOf(typeof(INPUT)));
    }
}

public class KeyEvent : EventArgs // flare's key events
{
    public Keys key;
    public VKeyCodes vkey;

    public KeyEvent(char v, VKeyCodes c)
    {
        key = (Keys)v;
        vkey = c;
    }

    public KeyEvent(Keys v, VKeyCodes c)
    {
        key = v;
        vkey = c;
    }
}

public enum VKeyCodes
{
    KeyDown = 0,
    KeyHeld = 1,
    KeyUp = 2
}