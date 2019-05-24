using GTA;
using System.Drawing;

namespace DeveloperConsole
{
    public class Log
    {
        public const int MARGIN = 12; // Margin between each line

        public UIText[] logs;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="position"></param>
        /// <param name="size"></param>
        /// <param name="textSize"></param>
        /// <param name="edgeWidth"></param>
        public Log(Point position, Size size, float textSize, int edgeWidth)
        {
            CreateLogs(new Point(position.X + edgeWidth, size.Height), textSize);
        }

        /// <summary>
        /// Creates the logs for the console
        /// </summary>
        /// <param name="position"></param>
        /// <param name="textSize"></param>
        private void CreateLogs(Point position, float textSize)
        {
            logs = new UIText[((position.Y - MARGIN) / MARGIN)];
            logs[0] = new UIText(string.Empty, new Point(position.X, position.Y - MARGIN), textSize);
            for (int i = 1, o = 24; i < logs.Length; i++, o += MARGIN)
            {
                logs[i] = new UIText(string.Empty, new Point(position.X, position.Y - o), textSize);
            }
        }

        /// <summary>
        /// Draw the logs
        /// </summary>
        public void DrawLogs()
        {
            for (int i = 0; i < logs.Length; i++)
            {
                logs[i].Draw();
            }
        }

        /// <summary>
        /// Append a new string to the log
        /// </summary>
        /// <param name="log"></param>
        public void AppendLog(string log)
        {
            for (int i = logs.Length - 1; i > 0; i--)
            {
                logs[i].Caption = logs[i - 1].Caption;
            }
            logs[0].Caption = log;
        }

        /// <summary>
        /// Clear the logs
        /// </summary>
        public void ClearLogs()
        {
            for (int i = 0; i < logs.Length; i++)
            {
                logs[i].Caption = string.Empty;
            }
        }
    }
}
