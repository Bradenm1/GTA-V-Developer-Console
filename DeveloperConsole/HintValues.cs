using GTA;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeveloperConsole
{
    public enum HintType { Vehicle = 0, Pickup, Ped, Command, Weather, Weapon, OnOff, VehicleMod };
    public class HintValues
    {
        // Contains hints for command being entered
        public static Dictionary<HintType, Dictionary<string, string>> allHints = new Dictionary<HintType, Dictionary<string, string>>();
        // Contains command hints
        public static List<string> commands = new List<string>();
        public static string[] onOff = new string[] { "on", "off" };

        /// <summary>
        /// Populate the allHints dictonary with the hints
        /// </summary>
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
        }

        /// <summary>
        /// Finds hints for a given command givens the current currentparam
        /// </summary>
        /// <param name="command"></param>
        /// <param name="currentParam"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string[] FindHinds(string command, string currentParam, int index)
        {
            Command foundCommand = Program.commands.FindCommand(command.ToLower());
            if (foundCommand != null)
            {
                if (index < 1) // Incase it's still on the first param but it's complete
                    index++;
                if (foundCommand != null && foundCommand.autoFill.Count <= (index - 1))
                    return Find(currentParam, allHints[foundCommand.autoFill[index - 2]]); // Return the correct hint given the index of the param
            }
            else
                return Find(command, commands); // Command is null so return some command hints if any
            return null;
        }

        /// <summary>
        /// Searches through a dictionary to find a value containing the given command
        /// </summary>
        /// <param name="command"></param>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static string[] Find(string command, Dictionary<string, string> dict)
        {
            string[] hints = Enumerable.Repeat(string.Empty, Program.console.input.hint.textHints.Length).ToArray();
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

        /// <summary>
        /// Checks if the given command contains within a value
        /// </summary>
        /// <param name="command"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool checkEntry(string command, string value)
        {
            if (value.ToLower().Contains(command.ToLower()))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Searches through a list to find a value containing the given command
        /// </summary>
        /// <param name="command"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string[] Find(string command, List<string> list)
        {
            string[] hints = Enumerable.Repeat(string.Empty, Program.console.input.hint.textHints.Length).ToArray();
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

        /// <summary>
        /// Gets the values and names of enums
        /// </summary>
        /// <typeparam name="T">An enum</typeparam>
        /// <returns></returns>
        private static Dictionary<string, string> EnumNamedValues<T>() where T : System.Enum
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
