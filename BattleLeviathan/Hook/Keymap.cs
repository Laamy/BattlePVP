#region

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

#endregion

internal class Keymap // now its not as dog balls as the last one
{
    public EventHandler<KeyEvent> OnKeyEvent;

    private readonly Dictionary<char, bool> prevBuff = new Dictionary<char, bool>();

    public static bool GetDown(Keys key)
    {
        return BattlefieldClient.Keymap.prevBuff[(char)key];
    }

    public Keymap()
    {
        for (var c = (char)0; c < 0xFF; c++)
            prevBuff.Add(c, false);
    }

    public void Tick()
    {
        keyTick();
    }

    private void keyTick()
    {
        for (var c = (char)0; c < 0xFF; c++)
        {
            if (OnKeyEvent != null && BattlefieldClient.isFocused)
            {
                bool held = User32.GetAsyncKeyState(c);

                if (held && !prevBuff[c]) OnKeyEvent.Invoke(this, new KeyEvent(c, VKeyCodes.KeyDown));
                else if (!held && prevBuff[c]) OnKeyEvent.Invoke(this, new KeyEvent(c, VKeyCodes.KeyUp));
                else if (held && prevBuff[c]) OnKeyEvent.Invoke(this, new KeyEvent(c, VKeyCodes.KeyHeld));

                prevBuff[c] = held;
            }
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