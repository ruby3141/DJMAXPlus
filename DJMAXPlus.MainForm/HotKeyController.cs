using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Input.KeyboardAndMouse;

namespace DJMAXPlus.MainForm
{
    public partial class HotKeyController : UserControl
    {
        public delegate void HotKeyDownEventHandler(HotKeyController sender, HotKey hotKey);
        public event HotKeyDownEventHandler? HotKeyDown;

        public const int WM_HOTKEY = 0x0312;

        public HotKeyController()
        {
            InitializeComponent();
        }

        public bool RegisterHotKey(HotKey hotKey)
        {
            return (bool)Invoke(() =>
                PInvoke.RegisterHotKey(new HWND(Handle), hotKey.GetHashCode(), (HOT_KEY_MODIFIERS)hotKey.Modifier, (uint)hotKey.VKeyCode));
        }

        public bool UnregisterHotKey(HotKey hotKey)
        {
            return (bool)Invoke(() => PInvoke.UnregisterHotKey(new HWND(Handle), hotKey.GetHashCode()));
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            switch (m.Msg)
            {
                case WM_HOTKEY:
                    HotKeyDown?.Invoke(this, HotKey.FromHashCode((int)m.WParam));
                    break;
                default:
                    break;
            }
        }
    }
}
