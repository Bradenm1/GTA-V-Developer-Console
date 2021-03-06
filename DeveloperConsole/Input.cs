﻿using GTA;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeveloperConsole
{
    public class Input
    {
        public Hint hint;
        public UIText inputText;
        public int selectedIndex = 0; // Selected position for the cursor within the input
        public int selectPastCommand = 0; // The select command in the history
        public int blinkingTick = 25; // The blinking rate for the cursor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="position"></param>
        /// <param name="textSize"></param>
        public Input(Point position, float textSize = 0.3f)
        {
            this.inputText = new UIText(string.Empty, position, textSize);
            this.hint = new Hint(new Point(position.X + 10, position.Y + 16), textSize);
        }

        /// <summary>
        /// Draw the input
        /// </summary>
        public void Draw()
        {
            hint.DrawHints();
            inputText.Draw();
        }

        /// <summary>
        /// Append a new character to the input string
        /// </summary>
        /// <param name="input"></param>
        public void AppendInput(string input)
        {
            inputText.Caption = inputText.Caption.Insert(SelectedIndex, input);
            SelectedIndex += 1;
        }

        /// <summary>
        /// Delete last character within the input
        /// </summary>
        public void DeleteLastCharacter()
        {
            if (inputText.Caption.Contains('|')) inputText.Caption = inputText.Caption.Remove(inputText.Caption.IndexOf('|'), 1);
            inputText.Caption = inputText.Caption.Remove(SelectedIndex > 0 ? SelectedIndex - 1 : 0, 1);
            SelectedIndex -= 1;
        }


        /// <summary>
        /// Clear the input string
        /// </summary>
        public void ClearInput()
        {
            inputText.Caption = string.Empty;
            SelectedIndex = 0;
            SelectPastCommand = 0;
            blinkingTick = 35;
            hint.ClearHints();
        }

        /// <summary>
        /// Key down call
        /// </summary>
        /// <param name="key"></param>
        public void KeyDown(Keys key)
        {
            switch (key)
            {
                default:
                    break;
            }
        }

        /// <summary>
        /// Run the cursor for the input
        /// </summary>
        public void RunSelection()
        {
            if (blinkingTick == 35) inputText.Caption = inputText.Caption.Insert(selectedIndex, "|");
            else
            {
                if (blinkingTick > 35)
                {
                    inputText.Caption = inputText.Caption.Remove(inputText.Caption.IndexOf('|'), 1);
                    blinkingTick = 0;
                }
            }
            blinkingTick++;
        }

        /// <summary>
        /// Key up call
        /// </summary>
        /// <param name="key"></param>
        public void KeyUp(Keys key)
        {
            switch (key)
            {
                case Keys.D0:
                case Keys.D1:
                case Keys.D2:
                case Keys.D3:
                case Keys.D4:
                case Keys.D5:
                case Keys.D6:
                case Keys.D7:
                case Keys.D8:
                case Keys.D9:
                    string number = Enum.GetName(typeof(Keys), key);
                    number = number.Remove(0, 1);
                    AppendInput(number);
                    hint.RunHintString(inputText.Caption);
                    break;
                case Keys.A:
                case Keys.B:
                case Keys.C:
                case Keys.D:
                case Keys.E:
                case Keys.F:
                case Keys.G:
                case Keys.H:
                case Keys.I:
                case Keys.J:
                case Keys.K:
                case Keys.L:
                case Keys.M:
                case Keys.N:
                case Keys.O:
                case Keys.P:
                case Keys.Q:
                case Keys.R:
                case Keys.S:
                case Keys.T:
                case Keys.U:
                case Keys.V:
                case Keys.W:
                case Keys.X:
                case Keys.Y:
                case Keys.Z:
                    string character = Enum.GetName(typeof(Keys), key);
                    character = Program.shiftBeingHeld ? character : character.ToLower();
                    AppendInput(character);
                    hint.RunHintString(inputText.Caption);
                    break;
                case Keys.Back:
                case Keys.Delete:
                    DeleteLastCharacter();
                    hint.RunHintString(inputText.Caption);
                    break;
                case Keys.Space:
                    AppendInput(" ");
                    hint.RunHintString(inputText.Caption);
                    break;
                case Keys.Return:
                    if (inputText.Caption.Contains('|')) inputText.Caption = inputText.Caption.Remove(inputText.Caption.IndexOf('|'), 1);
                    Program.console.fullLog.Add(inputText.Caption);
                    Program.console.RunCommand(Utilities.TrimStringArray(inputText.Caption.Split(' ')));
                    ClearInput();
                    break;
                case Keys.Tab:
                    if (inputText.Caption.Contains('|')) inputText.Caption = inputText.Caption.Remove(inputText.Caption.IndexOf('|'), 1);
                    string[] consoleParams = Utilities.TrimStringArray(inputText.Caption.Split(' '));
                    consoleParams[consoleParams.Length - 1] = hint.textHints[0].Caption;
                    inputText.Caption = Utilities.ConvertStringArrayToString(consoleParams).Trim();
                    SelectedIndex = inputText.Caption.Length;
                    hint.RunHintString(inputText.Caption);
                    break;
                case Keys.Right:
                    SelectedIndex += 1;
                    break;
                case Keys.Left:
                    SelectedIndex -= 1;
                    break;
                case Keys.Up:
                    SelectPastCommand += 1;
                    inputText.Caption = Program.console.fullLog[Program.console.fullLog.Count - SelectPastCommand];
                    SelectedIndex = inputText.Caption.Length;
                    //inputText.Caption = ((uint)((VehicleHash)Enum.Parse(typeof(VehicleHash), "Adder"))).ToString();
                    break;
                case Keys.Down:
                    inputText.Caption = Program.console.fullLog[Program.console.fullLog.Count - SelectPastCommand];
                    SelectedIndex = inputText.Caption.Length;
                    SelectPastCommand -= 1;
                    break;
                case Keys.LShiftKey:
                case Keys.RShiftKey:
                case Keys.KeyCode:
                case Keys.Modifiers:
                case Keys.None:
                case Keys.LButton:
                case Keys.RButton:
                case Keys.Cancel:
                case Keys.MButton:
                case Keys.XButton1:
                case Keys.XButton2:
                case Keys.LineFeed:
                case Keys.Clear:
                case Keys.ShiftKey:
                case Keys.ControlKey:
                case Keys.Menu:
                case Keys.Pause:
                case Keys.Capital:
                case Keys.KanaMode:
                case Keys.JunjaMode:
                case Keys.FinalMode:
                case Keys.HanjaMode:
                case Keys.Escape:
                case Keys.IMEConvert:
                case Keys.IMENonconvert:
                case Keys.IMEAccept:
                case Keys.IMEModeChange:
                case Keys.Prior:
                case Keys.Next:
                case Keys.End:
                case Keys.Home:
                case Keys.Select:
                case Keys.Print:
                case Keys.Execute:
                case Keys.Snapshot:
                case Keys.Insert:
                case Keys.Help:
                case Keys.LWin:
                case Keys.RWin:
                case Keys.Apps:
                case Keys.Sleep:
                case Keys.NumPad0:
                case Keys.NumPad1:
                case Keys.NumPad2:
                case Keys.NumPad3:
                case Keys.NumPad4:
                case Keys.NumPad5:
                case Keys.NumPad6:
                case Keys.NumPad7:
                case Keys.NumPad8:
                case Keys.NumPad9:
                case Keys.Multiply:
                case Keys.Add:
                case Keys.Separator:
                case Keys.Subtract:
                case Keys.Decimal:
                case Keys.Divide:
                case Keys.F1:
                case Keys.F2:
                case Keys.F3:
                case Keys.F4:
                case Keys.F5:
                case Keys.F6:
                case Keys.F7:
                case Keys.F8:
                case Keys.F9:
                case Keys.F10:
                case Keys.F11:
                case Keys.F12:
                case Keys.F13:
                case Keys.F14:
                case Keys.F15:
                case Keys.F16:
                case Keys.F17:
                case Keys.F18:
                case Keys.F19:
                case Keys.F20:
                case Keys.F21:
                case Keys.F22:
                case Keys.F23:
                case Keys.F24:
                case Keys.NumLock:
                case Keys.Scroll:
                case Keys.LControlKey:
                case Keys.RControlKey:
                case Keys.LMenu:
                case Keys.RMenu:
                case Keys.BrowserBack:
                case Keys.BrowserForward:
                case Keys.BrowserRefresh:
                case Keys.BrowserStop:
                case Keys.BrowserSearch:
                case Keys.BrowserFavorites:
                case Keys.BrowserHome:
                case Keys.VolumeMute:
                case Keys.VolumeDown:
                case Keys.VolumeUp:
                case Keys.MediaNextTrack:
                case Keys.MediaPreviousTrack:
                case Keys.MediaStop:
                case Keys.MediaPlayPause:
                case Keys.LaunchMail:
                case Keys.SelectMedia:
                case Keys.LaunchApplication1:
                case Keys.LaunchApplication2:
                case Keys.OemSemicolon:
                case Keys.Oemplus:
                case Keys.Oemcomma:
                case Keys.OemMinus:
                case Keys.OemPeriod:
                case Keys.OemQuestion:
                case Keys.Oemtilde:
                case Keys.OemOpenBrackets:
                case Keys.OemPipe:
                case Keys.OemCloseBrackets:
                case Keys.OemQuotes:
                case Keys.Oem8:
                case Keys.OemBackslash:
                case Keys.ProcessKey:
                case Keys.Packet:
                case Keys.Attn:
                case Keys.Crsel:
                case Keys.Exsel:
                case Keys.EraseEof:
                case Keys.Play:
                case Keys.Zoom:
                case Keys.NoName:
                case Keys.Pa1:
                case Keys.OemClear:
                case Keys.Shift:
                case Keys.Control:
                case Keys.Alt:
                    break;
                default:
                    break;

            }
        }

        public int SelectedIndex
        {
            get
            {
                return selectedIndex;
            }

            set
            {
                if (selectedIndex > inputText.Caption.Length) selectedIndex = inputText.Caption.Length;
                else
                {
                    if (value >= 0) selectedIndex = value;
                    else selectedIndex = 0;
                }
            }
        }

        public int SelectPastCommand
        {
            get
            {
                return selectPastCommand;
            }

            set
            {
                if (selectPastCommand > Program.console.fullLog.Count) selectPastCommand = Program.console.fullLog.Count;
                else
                {
                    if (value >= 0) selectPastCommand = value;
                    else selectPastCommand = 0;
                }
            }
        }
    }
}
