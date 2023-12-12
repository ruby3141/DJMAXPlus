using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Win32;
using Windows.Win32.Foundation;

namespace DJMAXPlus.Hook
{
    public class OverlayProjector
    {
        public static IntPtr GetWindowHandle(string name)
        {
            return PInvoke.FindWindow(null, name);
        }
    }
}
