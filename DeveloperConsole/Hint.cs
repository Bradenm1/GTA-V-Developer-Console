using GTA;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperConsole
{
    public class Hint
    {
        public static List<string> vehicles = new List<string>();
        public static List<string> pickups = new List<string>();
        public static List<string> peds = new List<string>();
        public static List<string> commands = new List<string>();
        public static List<string> weathers;

        public static string FindHinds(string command)
        {
            string hint;
            hint = Find(command, vehicles);
            if (!hint.Equals(Program.emptyString)) return hint;
            hint = Find(command, pickups);
            if (!hint.Equals(Program.emptyString)) return hint;
            hint = Find(command, peds);
            if (!hint.Equals(Program.emptyString)) return hint;
            hint = Find(command, commands);
            if (!hint.Equals(Program.emptyString)) return hint;
            hint = Find(command, weathers);
            return hint;
        }

        public static string Find(string command, List<string> list)
        {
            string hint = Program.emptyString;
            foreach (string value in list)
            {
                if (!value.Equals(Program.emptyString) && value.IndexOf(command, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    hint = value;
                }
            }
            return hint;
        }

        public static void PopulateHints()
        {
            AppendVehicleToAutoCorrect(Enum.GetNames(typeof(VehicleHash)));
            AppendPedToAutoCorrect(Enum.GetNames(typeof(PedHash)));
            AppendPickupToAutoCorrect(Enum.GetNames(typeof(PickupType)));
            weathers = AppendToAutoCorrect(Enum.GetNames(typeof(Weather)));
            for (int i = 0; i < Program.commands.allCommands.Length; i++)
            {
                commands.Add(Program.commands.allCommands[i].commandName);
            }
        }

        private static List<string> AppendToAutoCorrect(string[] names)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < names.Length; i++)
            {
                list.Add(names[i]);
            }
            return list;
        }

        private static void AppendVehicleToAutoCorrect(string[] names)
        {
            for (int i = 0; i < names.Length; i++)
            {
                vehicles.Add(names[i]);
                vehicles.Add(((uint)((VehicleHash)Enum.Parse(typeof(VehicleHash), names[i]))).ToString());
            }
        }

        private static void AppendPedToAutoCorrect(string[] names)
        {
            for (int i = 0; i < names.Length; i++)
            {
                peds.Add(names[i]);
                peds.Add(((uint)((PedHash)Enum.Parse(typeof(PedHash), names[i]))).ToString());
            }
        }

        private static void AppendPickupToAutoCorrect(string[] names)
        {
            for (int i = 0; i < names.Length; i++)
            {
                pickups.Add(names[i]);
                pickups.Add(((uint)((PickupType)Enum.Parse(typeof(PickupType), names[i]))).ToString());
            }
        }
    }
}
