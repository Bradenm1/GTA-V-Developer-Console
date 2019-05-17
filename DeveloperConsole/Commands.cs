using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperConsole
{
    public class Commands
    {
        public Command[] allCommands = new Command[9];

        public Commands()
        {
            RegisterCommands();
        }

        private void RegisterCommands()
        {
            allCommands[0] = new SpawnCar();
            allCommands[1] = new Clear();
            allCommands[2] = new GetHash();
            allCommands[3] = new SpawnPickup();
            allCommands[4] = new SpawnPed();
            allCommands[5] = new ClearWantedLevel();
            allCommands[6] = new SetWantedLevel();
            allCommands[7] = new Explode();
            allCommands[8] = new ChangeWeather();
        }

        public Command FindCommand(string commandName)
        {
            for (int i = 0; i < allCommands.Length; i++)
            {
                if (allCommands[i].commandName == commandName || allCommands[i].shortName == commandName)
                    return allCommands[i];
            }
            return null;
        }
    }

    /// <summary>
    /// Base command class
    /// </summary>
    public abstract class Command
    {
        public string commandName;
        public string shortName;

        public bool RunCommnad(string[] inputParams)
        {
            try
            {
                return Run(inputParams);
            }
            catch
            {
                return false;
            }
        }

        public abstract bool Run(string[] inputParams);
        public abstract string GetHelp(); // Help method, used to check what a command does
    }

    public class SpawnCar : Command
    {
        public SpawnCar()
        {
            commandName = "spawncar";
            shortName = "sc";
        }

        public override string GetHelp()
        {
            return "Spawn anytype of vehicle. Params -> [String - CarName, String - PlateName (Optional)]";
        }

        // inputParams[1] - Vehicle name
        // inputParams[2] - Number Plate
        // Input Params[3] - Rotation
        public override bool Run(string[] inputParams)
        {
            Vehicle vehicle = World.CreateVehicle(inputParams[1], GTA.Game.Player.Character.Position + Game.Player.Character.ForwardVector * 3.0f, Game.Player.Character.Heading + 90);
            if (vehicle == null)
                World.CreateVehicle(int.Parse(inputParams[1]), GTA.Game.Player.Character.Position + Game.Player.Character.ForwardVector * 3.0f, Game.Player.Character.Heading + 90);
            if (vehicle == null) return false;
            vehicle.CanTiresBurst = false;
            vehicle.CustomPrimaryColor = Color.FromArgb(Program.random.Next(0, 255), Program.random.Next(0, 255), Program.random.Next(0, 255));
            vehicle.CustomSecondaryColor = Color.FromArgb(Program.random.Next(0, 255), Program.random.Next(0, 255), Program.random.Next(0, 255));
            vehicle.PlaceOnGround();
            vehicle.NumberPlate = inputParams.Length > 2 ? inputParams[2] : "3DAZC8";
            return true;
        }
    }

    public class SpawnPickup : Command
    {
        public SpawnPickup()
        {
            commandName = "spawnpickup";
            shortName = "sp";
        }

        public override string GetHelp()
        {
            return "Spawn anytype of vehicle. Params -> [String - CarName, String - PlateName (Optional)]";
        }

        // inputParams[1] - Vehicle name
        // inputParams[2] - Number Plate
        // Input Params[3] - Rotation
        public override bool Run(string[] inputParams)
        {
            //Pickup pickup = World.CreatePickup(PickupType.WeaponSmokeGrenade, GTA.Game.Player.Character.Position + Game.Player.Character.ForwardVector * 3.0f, , inputParams.Length > 1 ? int.Parse(inputParams[2]) : 1);
            return true;
        }
    }

    public class SpawnPed : Command
    {
        public SpawnPed()
        {
            commandName = "spawnactor";
            shortName = "sa";
        }

        public override string GetHelp()
        {
            return "Spawn anytype of vehicle. Params -> [String - CarName, String - PlateName (Optional)]";
        }

        // inputParams[1] - Ped Hash
        public override bool Run(string[] inputParams)
        {
            PedHash pedHash = PedHash.Abigail;
            Ped ped;
            if (Enum.TryParse<PedHash>(inputParams[1], out pedHash))
                ped = World.CreatePed(pedHash, Game.Player.Character.Position + Game.Player.Character.ForwardVector * 3.0f + Game.Player.Character.UpVector * 2.0f);
            else return false;
            return true;
        }
    }

    /// <summary>
    /// Prints a hash to the console
    /// </summary>
    public class GetHash : Command
    {
        public GetHash()
        {
            commandName = "gethash";
            shortName = "gh";
        }

        public override string GetHelp()
        {
            return "Gets the hash for a Vehicle/Ped/Weapon. Params -> [String - Hash]";
        }

        public override bool Run(string[] inputParams)
        {
            VehicleHash vehicleHash = VehicleHash.Adder;
            if (Enum.TryParse<VehicleHash>(inputParams[1], out vehicleHash)) Program.console.AppendLog(inputParams[1] + " Hash: " + ((uint)vehicleHash).ToString());
            else
            {
                PedHash pedHash = PedHash.Abigail;
                if (Enum.TryParse<PedHash>(inputParams[1], out pedHash)) Program.console.AppendLog(inputParams[1] + " Hash: " + ((uint)pedHash).ToString());
                else
                {
                    WeaponHash weaponHash = WeaponHash.AdvancedRifle;
                    if (Enum.TryParse<WeaponHash>(inputParams[1], out weaponHash)) Program.console.AppendLog(inputParams[1] + " Hash: " + ((uint)pedHash).ToString());
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }

    /// <summary>
    /// Command to clear the console logs
    /// </summary>
    public class Clear : Command
    {
        public Clear()
        {
            commandName = "clear";
            shortName = "c";
        }

        public override string GetHelp()
        {
            return "Clears console logs";
        }

        public override bool Run(string[] inputParams)
        {
            Program.console.ClearLogs();
            return true;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ClearWantedLevel : Command
    {
        public ClearWantedLevel()
        {
            commandName = "clearwantedlevel";
            shortName = "cwl";
        }

        public override string GetHelp()
        {
            return "Clears the players wanted level";
        }

        public override bool Run(string[] inputParams)
        {
            Game.Player.WantedLevel = 0;
            return true;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SetWantedLevel : Command
    {
        public SetWantedLevel()
        {
            commandName = "setwantedlevel";
            shortName = "swl";
        }

        public override string GetHelp()
        {
            return "Clears the players wanted level";
        }

        public override bool Run(string[] inputParams)
        {
            int wantedLevel = 0;
            if (int.TryParse(inputParams[1], out wantedLevel) && wantedLevel < 6 && wantedLevel >= 0) Game.Player.WantedLevel = wantedLevel;
            return true;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GetRayCast : Command
    {
        public GetRayCast()
        {
            commandName = "get";
            shortName = "g";
        }

        public override string GetHelp()
        {
            return "Gets object player is looking at";
        }

        public override bool Run(string[] inputParams)
        {
            RaycastResult ray = World.GetCrosshairCoordinates();

            if(ray.DitHitAnything)
            {
                Program.console.selectedEntity = ray.HitEntity;
                return true;
            }
            return false;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Explode : Command
    {
        RaycastResult ray;

        public Explode()
        {
            commandName = "explode";
            shortName = "ex";
        }

        public override string GetHelp()
        {
            return "Gets object player is looking at";
        }

        public override bool Run(string[] inputParams)
        {
            ray = World.GetCrosshairCoordinates();

            if (ray.DitHitAnything)
            {
                World.AddExplosion(ray.HitCoords, ExplosionType.Rocket, int.Parse(inputParams[1]), 0);
                return true;
            }
            return false;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ChangeWeather : Command
    {
        Weather weather = Weather.Clear;
        float duration = 500;

        public ChangeWeather()
        {
            commandName = "changeweather";
            shortName = "cw";
        }

        public override string GetHelp()
        {
            return "Gets object player is looking at";
        }

        public override bool Run(string[] inputParams)
        {
            float.TryParse(inputParams[2], out duration); // Get duration
            if (Enum.TryParse<Weather>(inputParams[1], out weather))
            {
                World.TransitionToWeather(weather, 300);
                return true;
            }
            else
            {
                World.TransitionToWeather((Weather)int.Parse(inputParams[1]), 300);
                return true;
            }
        }
    }
}
