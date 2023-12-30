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
    public static int e = 0;

    public static Keymap handle;
    public static EventHandler<KeyEvent> keyEvent;
    public static EventHandler<KeyEvent> globalKeyEvent;

    private readonly Dictionary<char, uint> _dBuff = new Dictionary<char, uint>();
    private readonly Dictionary<char, bool> _noKey = new Dictionary<char, bool>();

    private readonly Dictionary<char, uint> _rBuff = new Dictionary<char, uint>();
    private readonly Dictionary<char, bool> _yesKey = new Dictionary<char, bool>();

    public static bool IsCursorVisible()
    {
        CURSORINFO cursorInfo = new CURSORINFO();
        cursorInfo.cbSize = Marshal.SizeOf(typeof(CURSORINFO));

        if (User32.GetCursorInfo(out cursorInfo))
            return (cursorInfo.flags & 0x00000001) == 0;

        return true;
    }

    public static bool GetDown(Keys key)
    {
        return handle._dBuff[(char)key] > 0;
    }

    public Keymap()
    {
        handle = this;
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
                if (keyEvent != null)
                    if (BattlefieldClient.isGameFocused())
                        keyEvent.Invoke(this, new KeyEvent(c, VKeyCodes.KeyHeld));

                if (_dBuff[c] > 0)
                    continue;

                _dBuff[c]++;
                _noKey[c] = false;

                if (keyEvent != null)
                    if (BattlefieldClient.isGameFocused())
                        keyEvent.Invoke(this, new KeyEvent(c, VKeyCodes.KeyDown));
            }
            else
            {
                if (_rBuff[c] > 0)
                    continue;

                _rBuff[c]++;
                _yesKey[c] = true;

                if (keyEvent != null)
                    if (BattlefieldClient.isGameFocused())
                        keyEvent.Invoke(this, new KeyEvent(c, VKeyCodes.KeyUp));
            }

            if (_noKey[c])
                _dBuff[c] = 0;

            if (!_yesKey[c])
                _rBuff[c] = 0;
        }
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