using GTA;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperConsole
{
    public enum HintType { Vehicle = 0, Pickup, Ped, Command, Weather, Weapon, OnOff, VehicleMod };
    public class Hint
    {
        public static Dictionary<HintType, Dictionary<string, string>> allHints = new Dictionary<HintType, Dictionary<string, string>>();
        public static List<string> commands = new List<string>();
        public static string[] onOff = new string[] { "on", "off" };

        public static string[] FindHinds(string command, string currentParam, int index)
        {
            if (index > 1)
            {
                Command foundCommand = Program.commands.FindCommand(command.ToLower());
                if (foundCommand != null && foundCommand.autoFill.Count <= (index - 1))
                    return Find(currentParam, allHints[foundCommand.autoFill[index - 2]]);
            }
            else
                return Find(command, commands);
            return null;
        }

        public static string[] Find(string command, Dictionary<string, string> dict)
        {
            string[] hints = Enumerable.Repeat(string.Empty, Program.console.input.textHints.Length).ToArray();
            int o = 0;
            foreach (KeyValuePair<string, string> entry in dict)
            {
                if (o >= hints.Length) break;
                if (checkEntry(command, entry.Value.ToString()))
                {
                    hints[o] = entry.Value.ToString();
                    o++;
                }
                if (checkEntry(command, entry.Key.ToString()))
                {
                    hints[o] = entry.Key.ToString();
                    o++;
                }
            }
            return hints;
        }

        public static bool checkEntry(string command, string value)
        {
            if (value.ToLower().Contains(command.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static string[] Find(string command, List<string> list)
        {
            string[] hints = Enumerable.Repeat(string.Empty, Program.console.input.textHints.Length).ToArray();
            for (int i = 0, o = 0; i < list.Count; i++)
            {
                if ((o < hints.Length) && (list[i].ToLower().Contains(command.ToLower())))
                {
                    hints[o] = list[i];
                    o++;
                }
            }
            return hints;
        }

        public static void PopulateHints()
        {
            allHints.Add(HintType.Vehicle, EnumNamedValues<VehicleHash>());
            allHints.Add(HintType.Ped, EnumNamedValues<PedHash>());
            allHints.Add(HintType.Pickup, EnumNamedValues<PickupType>());
            allHints.Add(HintType.Weapon, EnumNamedValues<WeaponHash>());
            allHints.Add(HintType.VehicleMod, EnumNamedValues<VehicleMod>());
            allHints.Add(HintType.Weather, EnumNamedValues<Weather>());
            for (int i = 0; i < Commands.allCommands.Length; i++)
            {
                commands.Add(Commands.allCommands[i].commandName);
                commands.Add(Commands.allCommands[i].shortName);
            }
            UI.Notify("DONE HINTS");
        }

        public static Dictionary<string, string> EnumNamedValues<T>() where T : System.Enum
        {
            var result = new Dictionary<string, string>();
            T[] values = (T[])Enum.GetValues(typeof(T));
            string[] names = Enum.GetNames(typeof(T));

            for ( int i = 0; i < values.Length; i++)
            {
                if (!result.ContainsKey(values[i].ToString("D")))
                    result.Add(values[i].ToString("D"), names[i]);
            }
            return result;
        }
    }
}
