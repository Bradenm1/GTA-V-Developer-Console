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
        private const string consoleName = "Developer Console:";

        // Design
        private UIText consoleHeaderText;
        private UIText consoleInputText;
        private UIContainer consoleContainer;
        private UIContainer consoleInputContainer;
        private UIRectangle[] edges = new UIRectangle[4];

        public Input input;
        public Log log;
        public List<string> fullLog = new List<string>();
        public Entity selectedEntity;

        public Console(Point position, Size size, Color color, float textSize = 0.3f, int edgeWdith = 4)
        {
            this.consoleContainer = new UIContainer(position, size, color);
            this.consoleHeaderText = new UIText(consoleName, new Point(position.X + edgeWdith, position.Y), textSize);
            this.consoleInputContainer = new UIContainer(new Point(position.X, position.Y + size.Height + edgeWdith), new Size(size.Width, 16), color);
            this.consoleInputText = new UIText("--", new Point(position.X, position.Y + size.Height + 4), textSize);
            this.input = new Input(new Point(position.X + 12, position.Y + size.Height + 4), textSize);
            this.log = new Log(position, size, textSize, edgeWdith);
            CreateFrame(position, size, color, edgeWdith);
        }

        private void CreateFrame(Point position, Size size, Color color, int width)
        {
            color = Color.FromArgb(color.A + 80, color.R + 80, color.G + 80, color.B + 80);
            edges[0] = new UIRectangle(position, new Size(size.Width, width), color);
            edges[1] = new UIRectangle(position, new Size(width, size.Height), color);
            edges[2] = new UIRectangle(new Point(position.X + size.Width - width, position.Y), new Size(width, size.Height), color);
            edges[3] = new UIRectangle(new Point(position.X, position.Y + size.Height - width), new Size(size.Width, width), color);
        }

        public void RunCursor()
        {
            input.RunSelection();
        }

        public void Draw()
        {
            consoleHeaderText.Draw();
            input.Draw();
            consoleInputText.Draw();
            log.DrawLogs();
            consoleInputContainer.Draw();
            DrawFrame();
            consoleContainer.Draw();
        }

        private void DrawFrame()
        {
            for (int i = 0; i < edges.Length; i++)
            {
                edges[i].Draw();
            }
        }

        private const string commandNotExist = "command does not exist!";
        private const string commandFailed = "Command Failed!";
        public void RunCommand(string[] inputParams)
        {
            Command command = Program.commands.FindCommand(inputParams[0].ToLower());
            if (command == null)
            {
                log.AppendLog(inputParams[0] + Program.spaceString + commandNotExist);
            }
            else
            {
                log.AppendLog(string.Join(Program.spaceString, inputParams));
                if (!command.RunCommnad(inputParams)) log.AppendLog(commandFailed);
            }
        }
    }
}
