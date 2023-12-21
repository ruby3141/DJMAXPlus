using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DJMAXPlus.MainForm
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            hotKeyController.RegisterHotKey(new HotKey
            {
                Modifier = HotKey.Modifiers.Mod_Ctrl | HotKey.Modifiers.Mod_Shift,
                VKeyCode = HotKey.VKeyCodes.VK_F12,
            });
        }

        private void hotKeyController_HotKeyDown(HotKeyController sender, HotKey hotKey)
        {
            Console.WriteLine($"HotKey Down!: {hotKey.Modifier}, {hotKey.VKeyCode}");
        }
    }
}
