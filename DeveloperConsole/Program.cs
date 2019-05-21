using System;
using System.Drawing;
using System.Windows.Forms;
using GTA;
using GTA.Native;

namespace DeveloperConsole
{
    public class Program : Script
    {
        public const string spaceString = " ";

        public static Console console;
        public static Commands commands;

        public static bool usingConsole = false;
        public static bool contolsDisabled = true;
        public static bool shiftBeingHeld = false;

        public static Random random = new Random();

        public bool UsingConsole
        {
            get
            {
                return usingConsole;
            }

            set
            {
                usingConsole = value;
                console.input.ClearInput();
            }
        }

        public Program()
        {
            commands = new Commands();
            console = new Console(new Point(10, 10), new Size(500, 128), Color.FromArgb(150, 120, 120, 120));
            Hint.PopulateHints();
            this.Tick += onTick;
            this.KeyUp += onKeyUp;
            this.KeyDown += onKeyDown;
        }

        private void onTick(object sender, EventArgs e)
        {
            if (UsingConsole)
            {
                if (contolsDisabled) Game.DisableAllControlsThisFrame(1);
                console.Draw();
                console.RunCursor();
            }
        }

        private void onKeyDown(object sender, KeyEventArgs e)
        {
            if (usingConsole) console.input.KeyDown(e.KeyCode);
        }

        private void onKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F10)
               UsingConsole = !UsingConsole;
            else
            {
                if (e.KeyCode == Keys.Escape) UsingConsole = false;
                else
                {
                    shiftBeingHeld = e.Shift;
                    if (usingConsole) console.input.KeyUp(e.KeyCode);
                }
            }
        }
    }
}
