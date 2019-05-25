using System;
using System.Drawing;
using System.Windows.Forms;
using GTA;

namespace DeveloperConsole
{
    public class Program : Script
    {
        public const string spaceString = " ";

        public static Console console;
        public static Commands commands;

        public static bool usingConsole = false; // If the user is currently using the console
        public static bool contolsDisabled = true; // If the controls can be used while in the console
        public static bool shiftBeingHeld = false; // If shift is being held (For uppercase)

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
            HintValues.PopulateHints();

            // Queue events
            this.Tick += onTick;
            this.KeyUp += onKeyUp;
            this.KeyDown += onKeyDown;
        }

        /// <summary>
        /// Run on each tick of the game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onTick(object sender, EventArgs e)
        {
            if (UsingConsole)
            {
                if (contolsDisabled) Game.DisableAllControlsThisFrame(1);
                console.Draw();
                console.RunCursor();
            }
        }

        /// <summary>
        /// Runs when a key is pressed down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onKeyDown(object sender, KeyEventArgs e)
        {
            if (usingConsole) console.input.KeyDown(e.KeyCode);
        }

        /// <summary>
        /// Runs when a key is released
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
