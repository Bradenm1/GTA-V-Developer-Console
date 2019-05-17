using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using GTA;
using GTA.Native;

namespace DeveloperConsole
{
    public class Console
    {
        private UIContainer consoleContainer;
        private UIText consoleHeaderText;
        public Input input;
        public List<string> fullLog = new List<string>();
        public UIText[] consoleLog;
        public Entity selectedEntity;
        private UIRectangle[] edges = new UIRectangle[4];

        public Console(Point C_position, Size C_size, Color C_color, float textSize = 0.3f, int edgeWdith = 4)
        {
            this.consoleContainer = new UIContainer(C_position, C_size, C_color);
            this.consoleHeaderText = new UIText("Developer Console", new Point(C_position.X + edgeWdith, C_position.Y), textSize, Color.Black);
            this.input = new Input(new Point(C_position.X + edgeWdith, C_size.Height - 10), textSize);
            CreateEdges(C_position, C_size, C_color, edgeWdith);
            CreateLogs(new Point(C_position.X + edgeWdith, C_size.Height - 10), textSize);
        }

        public void Draw()
        {
            consoleHeaderText.Draw();
            input.Draw();
            DrawLogs();
            DrawEdges();
            consoleContainer.Draw();
        }

        private void CreateEdges(Point position, Size size, Color color, int width)
        {
            color = Color.FromArgb(color.A + 80, color.R + 80, color.G + 80, color.B + 80);
            edges[0] = new UIRectangle(position, new Size(size.Width, width), color);
            edges[1] = new UIRectangle(position, new Size(width, size.Height), color);
            edges[2] = new UIRectangle(new Point(position.X + size.Width - width, position.Y), new Size(width, size.Height), color);
            edges[3] = new UIRectangle(new Point(position.X, position.Y + size.Height - width), new Size(size.Width, width), color);
        }

        private void DrawEdges()
        {
            for (int i = 0; i < edges.Length; i++)
            {
                edges[i].Draw();
            }
        }

        public void SetConsolePosition(Point _point)
        {
            consoleContainer.Position = _point;
        }

        public void SetConsoleSize(Size _size)
        {
            consoleContainer.Size = _size;
        }

        public void AppendLog(string log)
        {
            for (int i = consoleLog.Length - 1; i > 0; i--)
            {
                consoleLog[i].Caption = consoleLog[i - 1].Caption;
            }
            consoleLog[0].Caption = log;
        }

        private const string helpName = "help";
        private const string commandNotExist = "Command does not exist!";
        private const string commandFailed = "Command Failed!";
        public void RunCommand(string[] inputParams)
        {
            if (inputParams[0].ToLower().Equals(helpName))
            {
                Command command = Program.commands.FindCommand(inputParams[1].ToLower());
                if (command == null)
                    AppendLog(commandNotExist);
                else
                    AppendLog(inputParams[1] + Program.spaceString + command.GetHelp());
            }
            else
            {
                Command command = Program.commands.FindCommand(inputParams[0].ToLower());
                if (command == null)
                {
                    AppendLog(inputParams[0] + Program.spaceString + commandNotExist);
                }
                else
                {
                    AppendLog(string.Join(Program.spaceString, inputParams));
                    if (!command.RunCommnad(inputParams)) AppendLog(commandFailed);
                }
            }
        }

        private void CreateLogs(Point position, float textSize)
        {
            consoleLog = new UIText[((position.Y - 12) / 12)];
            consoleLog[0] = new UIText(Program.emptyString, new Point(position.X, position.Y - 12), textSize);
            for (int i = 1, o = 24; i < consoleLog.Length; i++, o += 12)
            {
                consoleLog[i] = new UIText(Program.emptyString, new Point(position.X, position.Y - o), textSize);
            }
        }

        private void DrawLogs()
        {
            for (int i = 0; i < consoleLog.Length; i++)
            {
                consoleLog[i].Draw();
            }
        }

        public void ClearLogs()
        {
            for (int i = 0; i < consoleLog.Length; i++)
            {
                consoleLog[i].Caption = Program.emptyString;
            }
        }
    }
}
