using GTA;
using System.Drawing;

namespace DeveloperConsole
{
    public class Hint
    {
        public UIText[] textHints = new UIText[7]; // The hints

        public Hint(Point position, float textSize = 0.3f)
        {
            CreateHints(position, textSize);
        }

        /// <summary>
        /// Creates the hints
        /// </summary>
        /// <param name="position"></param>
        /// <param name="textSize"></param>
        public void CreateHints(Point position, float textSize = 0.3f)
        {
            for (int i = 0, o = 0; i < textHints.Length; i++, o += 12)
            {
                textHints[i] = new UIText(string.Empty, new Point(position.X, position.Y + o), textSize);
            }
        }

        /// <summary>
        /// Draws the hints
        /// </summary>
        public void DrawHints()
        {
            for (int i = 0; i < textHints.Length; i++)
            {
                textHints[i].Draw();
            }
        }

        /// <summary>
        /// Clear the input of the cursor and run the hint
        /// </summary>
        /// <param name="command"></param>
        public void RunHintString(string command)
        {
            if (command.Contains("|")) command = command.Remove(command.IndexOf('|'), 1);
            RunHint(Utilities.TrimStringArray(command.Split(' ')));
        }

        /// <summary>
        /// Run the hints given the input params
        /// </summary>
        /// <param name="consoleParams"></param>
        public void RunHint(string[] consoleParams)
        {
            string[] hints = HintValues.FindHinds(consoleParams[0], consoleParams[consoleParams.Length - 1], consoleParams.Length);
            if (hints != null && !hints[0].Equals(string.Empty))
            {
                for (int i = 0; i < textHints.Length; i++)
                {
                    if (hints[i] != null)
                    {
                        textHints[i].Caption = hints[i];
                    }
                }
            }
            else
            {
                ClearHints();
            }
        }

        /// <summary>
        /// Clear the hints from the screen
        /// </summary>
        public void ClearHints()
        {
            for (int i = 0; i < textHints.Length; i++)
            {
                textHints[i].Caption = string.Empty;
            }
        }
    }
}
