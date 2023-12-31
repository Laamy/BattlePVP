using System;
using System.Collections.Generic;

using static User32;

public delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

class WinGraphics
{
    public static Dictionary<IntPtr, WinEventDelegate> overlayDelegates = new Dictionary<IntPtr, WinEventDelegate>(); // window hooks

    /// <summary>
    /// Creates then adds a new window event delegate ready for hooking
    /// </summary>
    public static void AddAdjust(IntPtr wnd, Action<IntPtr, uint, IntPtr, int, int, uint, uint> action)
    {
        overlayDelegates.Add(wnd, new WinEventDelegate(action));
    }

    /// <summary>
    /// Get the window event delegate assigned with a window(handle) if it exists
    /// </summary>
    public static WinEventDelegate Get(IntPtr wnd)
    {
        return overlayDelegates[wnd];
    }

    /// <summary>
    /// Set the window(handle) with a click through state (WARNING: excludes titlebar)
    /// </summary>
    public static void SetClickthrough(IntPtr wnd)
    {
        int initStyle = GetWindowLong(wnd, -20);
        SetWindowLong(wnd, -20, initStyle | 0x80000 | 0x20);
    }

    /// <summary>
    /// Initializes a window delegate based on Handle if AddAdjust has been called..
    /// </summary>
    public static void InitDelegate(IntPtr wnd)
    {
        if (!overlayDelegates.ContainsKey(wnd))
            throw new Exception("Please call AddAdjust before trying to initialize a window event delegate");

        // hook OnAdjust window event delegare (cross thread event stuff for the window)
        // hooks : WindowMoved, WindowClicked
        SetWinEventHook((uint)SWEH_Events.EVENT_OBJECT_LOCATIONCHANGE, (uint)SWEH_Events.EVENT_OBJECT_LOCATIONCHANGE, IntPtr.Zero, overlayDelegates[wnd], BattlefieldClient.GameId, GetWindowThreadProcessId(BattlefieldClient.WinHandle, IntPtr.Zero), (uint)SWEH_dwFlags.WINEVENT_OUTOFCONTEXT | (uint)SWEH_dwFlags.WINEVENT_SKIPOWNPROCESS | (uint)SWEH_dwFlags.WINEVENT_SKIPOWNTHREAD);
        SetWinEventHook((uint)SWEH_Events.EVENT_SYSTEM_FOREGROUND, (uint)SWEH_Events.EVENT_SYSTEM_FOREGROUND, IntPtr.Zero, overlayDelegates[wnd], 0, 0, (uint)SWEH_dwFlags.WINEVENT_OUTOFCONTEXT | (uint)SWEH_dwFlags.WINEVENT_SKIPOWNPROCESS | (uint)SWEH_dwFlags.WINEVENT_SKIPOWNTHREAD);
    }
}