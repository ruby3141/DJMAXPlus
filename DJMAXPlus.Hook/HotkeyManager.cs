using DJMAXPlus.Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Input.KeyboardAndMouse;

namespace DJMAXPlus.Hook
{
    /// <summary>
    /// Hotkey manager.
    /// </summary>
    public class HotkeyManager
    {
        public ControllerStates ControllerState { get; private set; } = ControllerStates.Uninitialized;

        public delegate void HotkeyDownEventHandler(Hotkey hotkey);
        public event HotkeyDownEventHandler? HotkeyDownEvent;

        // used as ConcurrentHashSet
        private ConcurrentDictionary<int, Hotkey> hotkeyBag;
        private CancellationTokenSource tokenSource;
        private Thread? workerThread;

        public HotkeyManager()
        {
            tokenSource = new CancellationTokenSource();
            hotkeyBag = new ConcurrentDictionary<int, Hotkey>();
            ControllerState = ControllerStates.Ready;
        }

        public void asdfasdf()
        {
            System.Windows.Forms.Application
        }

        public bool RegisterHotKey(Hotkey hotkey)
        {
            if (ControllerState != ControllerStates.Ready)
                return false;

            return hotkeyBag.TryAdd(hotkey.GetHashCode(), hotkey);
        }

        public bool RemoveHotkey(Hotkey hotkey)
        {
            if (ControllerState != ControllerStates.Ready)
                return false;

            return hotkeyBag.TryRemove(hotkey.GetHashCode(), out _);
        }

        public void Run()
        {
            lock (this)
            {
                if (workerThread != null)
                    return;

                workerThread = new Thread((_args) =>
                {
                    dynamic __args = _args!;
                    var args = new { cts = (CancellationToken)__args.cts, bag = (FrozenDictionary<int, Hotkey>)__args.bag };
                    foreach (var kv in args.bag)
                    {
                        PInvoke.RegisterHotKey(HWND.Null, kv.Key, (HOT_KEY_MODIFIERS)kv.Value.Modifier, (uint)kv.Value.VKeyCode);
                    }



                });

                workerThread.Start(new { cts = tokenSource.Token, bag = hotkeyBag.ToFrozenDictionary() });
            }
        }

        public void Stop()
        {
            lock (this)
            {
                if (workerThread == null)
                    return;

                PInvoke.PostThreadMessage(w

                tokenSource.Cancel();
                workerThread.Join();
                workerThread = null;
            }
        }
    }
}
