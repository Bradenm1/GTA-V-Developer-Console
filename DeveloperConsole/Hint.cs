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
        public static List<string> vehicles = new List<string>();
        public static List<string> pickups = new List<string>();
        public static List<string> peds = new List<string>();
        public static List<string> commands = new List<string>();
        public static List<string> weapons = new List<string>();
        public static List<string> vehicleMod = new List<string>();
        public static string[] onOff = new string[] { "on", "off" };
        public static List<string> weathers;

        public static string[] FindHinds(string command, string currentParam, int index)
        {
            string[] hints = null;
            if (index > 1)
            {
                Command foundCommand = Program.commands.FindCommand(command.ToLower());
                if (foundCommand != null && foundCommand.autoFill.Count <= (index - 1))
                {
                    switch (foundCommand.autoFill[index - 2])
                    {
                        case HintType.Vehicle:
                            hints = Find(currentParam, vehicles);
                            break;
                        case HintType.Pickup:
                            hints = Find(currentParam, pickups);
                            break;
                        case HintType.Ped:
                            hints = Find(currentParam, peds);
                            break;
                        case HintType.Command:
                            hints = Find(currentParam, commands);
                            break;
                        case HintType.Weather:
                            hints = Find(currentParam, weathers);
                            break;
                        case HintType.OnOff:
                            hints = Find(currentParam, onOff);
                            break;
                        case HintType.Weapon:
                            hints = Find(currentParam, weapons);
                            break;
                        case HintType.VehicleMod:
                            hints = Find(currentParam, vehicleMod);
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                hints = Find(command, commands);
            }
            return hints;
        }

        public static string[] Find(string command, List<string> list)
        {
            string[] hints = Enumerable.Repeat(string.Empty, Program.console.input.textHints.Length).ToArray();
            for(int i = 0, o = 0; i < list.Count; i++)
            {
                if ((o < hints.Length) && (list[i].ToLower().Contains(command.ToLower())))
                {
                    hints[o] = list[i];
                    o++;
                }
            }
            return hints;
        }

        public static string[] Find(string command, string[] list)
        {
            string[] hints = Enumerable.Repeat(string.Empty, Program.console.input.textHints.Length).ToArray();
            for (int i = 0, o = 0; i < list.Length; i++)
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
            AppendVehicleToAutoCorrect(Enum.GetNames(typeof(VehicleHash)));
            AppendPedToAutoCorrect(Enum.GetNames(typeof(PedHash)));
            AppendPickupToAutoCorrect(Enum.GetNames(typeof(PickupType)));
            AppendWeaponToAutoCorrect(Enum.GetNames(typeof(WeaponHash)));
            AppendVehicleModToAutoCorrect(Enum.GetNames(typeof(VehicleMod)));
            weathers = AppendToAutoCorrect(Enum.GetNames(typeof(Weather)));
            for (int i = 0; i < Program.commands.allCommands.Length; i++)
            {
                commands.Add(Program.commands.allCommands[i].commandName);
                commands.Add(Program.commands.allCommands[i].shortName);
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

        private static void AppendWeaponToAutoCorrect(string[] names)
        {
            for (int i = 0; i < names.Length; i++)
            {
                weapons.Add(names[i]);
                weapons.Add(((uint)((WeaponHash)Enum.Parse(typeof(WeaponHash), names[i]))).ToString());
            }
        }

        private static void AppendVehicleModToAutoCorrect(string[] names)
        {
            for (int i = 0; i < names.Length; i++)
            {
                vehicleMod.Add(names[i]);
                vehicleMod.Add(((uint)((VehicleMod)Enum.Parse(typeof(VehicleMod), names[i]))).ToString());
            }
        }
    }
}
