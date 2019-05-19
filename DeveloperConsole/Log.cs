using GTA;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperConsole
{
    public class Log
    {
        public const int MARGIN = 12;

        public UIText[] logs;

        public Log(Point position, Size size, float textSize, int edgeWidth)
        {
            CreateLogs(new Point(position.X + edgeWidth, size.Height), textSize);
        }

        private void CreateLogs(Point position, float textSize)
        {
            logs = new UIText[((position.Y - MARGIN) / MARGIN)];
            logs[0] = new UIText(Program.emptyString, new Point(position.X, position.Y - MARGIN), textSize);
            for (int i = 1, o = 24; i < logs.Length; i++, o += MARGIN)
            {
                logs[i] = new UIText(Program.emptyString, new Point(position.X, position.Y - o), textSize);
            }
        }

        public void DrawLogs()
        {
            for (int i = 0; i < logs.Length; i++)
            {
                logs[i].Draw();
            }
        }

        public void AppendLog(string log)
        {
            for (int i = logs.Length - 1; i > 0; i--)
            {
                logs[i].Caption = logs[i - 1].Caption;
            }
            logs[0].Caption = log;
        }

        public void ClearLogs()
        {
            for (int i = 0; i < logs.Length; i++)
            {
                logs[i].Caption = Program.emptyString;
            }
        }
    }
}
