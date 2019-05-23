using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static DeveloperConsole.Hint;

namespace DeveloperConsole
{
    public class Commands
    {
        public static Command[] allCommands = new Command[30];

        public Commands()
        {
            RegisterCommands();
        }

        private void RegisterCommands()
        {
            try
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
                allCommands[9] = new SetTimeScale();
                allCommands[10] = new SetNightVision();
                allCommands[11] = new SetThermalVision();
                allCommands[12] = new SetMoney();
                allCommands[13] = new SetTime();
                allCommands[14] = new ChangePlayerSkin();
                allCommands[15] = new Help();
                allCommands[16] = new SetWeather();
                allCommands[17] = new SetControlInConsole();
                allCommands[18] = new KillAllPeps();
                allCommands[19] = new KillAllVehicles();
                allCommands[20] = new Frenzy();
                allCommands[21] = new SetGodeMode();
                allCommands[22] = new AllWeapons();
                allCommands[23] = new AddWeapon();
                allCommands[24] = new SetPosition();
                allCommands[25] = new Fix();
                allCommands[26] = new SetVehicleGodeMode();
                allCommands[27] = new SetVehiclePrimaryColour();
                allCommands[28] = new SetVehicleSecondaryColour();
                allCommands[29] = new SetVehicleMod();
            }
            catch
            {
                Program.console.log.AppendLog("There was an error registering the commands!");
            }
        }

        public Command FindCommand(string commandName)
        {
            for (int i = 0; i < allCommands.Length; i++)
            {
                if (allCommands[i].commandName.Equals(commandName) || allCommands[i].shortName.Equals(commandName))
                    return allCommands[i];
            }
            return null;
        }

        public Entity GetEntity(string param)
        {
            switch (param)
            {
                case "player":
                    return (Entity)Game.Player.Character;
                case "crosshair":
                        RaycastResult ray = World.GetCrosshairCoordinates();

                        if (ray.DitHitAnything)
                        {
                            return ray.HitEntity;
                        }
                    break;
                default:
                    return null;
            }
            return null;
        }
    }

    /// <summary>
    /// Base command class
    /// </summary>
    public abstract class Command
    {
        protected const string on = "on";
        protected const string off = "off";

        public string commandName;
        public string shortName;
        public string help = "No help for this command";
        public List<HintType> autoFill = new List<HintType>();

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
    }

    /// <summary>
    /// 
    /// </summary>
    public class Help : Command
    {
        public Help()
        {
            commandName = "help";
            shortName = "h";
            help = "Help command. Params -> [CommandName]";
            autoFill.Add(HintType.Command);
        }

        public override bool Run(string[] inputParams)
        {
            
            if (inputParams.Length == 2)
            {
                Command command = Program.commands.FindCommand(inputParams[1].ToLower());
                if (command != null)
                    Program.console.log.AppendLog(inputParams[1] + Program.spaceString + command.help);
            }
            else // Help command without params
            {
                Program.console.log.AppendLog("Example 'help cwl' or 'help 1'.");
            }
            return true;
        }
    }

    /// <summary>
    /// Clears the players wanted level
    /// </summary>
    public class Fix : Command
    {
        public Fix()
        {
            commandName = "fix";
            shortName = "f";
            help = "Fix players vehicle.";
        }

        public override bool Run(string[] inputParams)
        {
            Game.Player.LastVehicle.Repair();
            return true;
        }
    }

    /// <summary>
    /// Set a mod for a vehicle
    /// </summary>
    public class SetVehicleMod : Command
    {
        private int modIndex = 0;
        private bool variations = false;

        public SetVehicleMod()
        {
            commandName = "setvehiclemod";
            shortName = "svm";
            help = "Set Vehicles Mod. Params -> [VehicleMod, ModIndex, Variations(Bool)]";
            autoFill.Add(HintType.VehicleMod);
        }

        public override bool Run(string[] inputParams)
        {
            VehicleMod vehicleMod = VehicleMod.Aerials;
            if (inputParams.Length > 2) int.TryParse(inputParams[2], out modIndex);
            if (inputParams.Length > 3) bool.TryParse(inputParams[3], out variations);
            if (Enum.TryParse<VehicleMod>(inputParams[1], out vehicleMod))
            {
                Game.Player.LastVehicle.SetMod(vehicleMod, modIndex, variations);
                return true;
            }
            else
            {
                Game.Player.LastVehicle.SetMod((VehicleMod)int.Parse(inputParams[1]), modIndex, variations);
                return true;
            }
        }
    }

    /// <summary>
    /// Spawn a car
    /// </summary>
    public class SpawnCar : Command
    {
        private const string defaultPlate = "3DAZC8";

        public SpawnCar()
        {
            commandName = "spawncar";
            shortName = "sc";
            help = "Spawn anytype of vehicle. Params -> [CarName, PlateName (Optional)]";
            autoFill.Add(HintType.Vehicle);
        }

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
            vehicle.NumberPlate = inputParams.Length > 2 ? inputParams[2] : defaultPlate;

            return true;
        }
    }

    /// <summary>
    /// Spawn a pickup
    /// </summary>
    public class SpawnPickup : Command
    {
        public SpawnPickup()
        {
            commandName = "spawnpickup";
            shortName = "sp";
            help = "Spawn anytype of vehicle. Params -> [CarName, String - PlateName (Optional)]";
            autoFill.Add(HintType.Pickup);
        }

        public override bool Run(string[] inputParams)
        {
            return true;
        }
    }

    /// <summary>
    /// Spawn a ped
    /// </summary>
    public class SpawnPed : Command
    {
        public SpawnPed()
        {
            commandName = "spawnactor";
            shortName = "sa";
            help = "Spawn anytype of ped. Params -> [PedHash/PedName]";
            autoFill.Add(HintType.Ped);
        }

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
        private const string hashAppend = " Hash: ";

        public GetHash()
        {
            commandName = "gethash";
            shortName = "gh";
            help = "Gets the hash for a Vehicle/Ped/Weapon. Params -> [Hash]";
        }

        public override bool Run(string[] inputParams)
        {
            VehicleHash vehicleHash = VehicleHash.Adder;
            if (Enum.TryParse<VehicleHash>(inputParams[1], out vehicleHash)) Program.console.log.AppendLog(inputParams[1] + hashAppend + ((uint)vehicleHash).ToString());
            else
            {
                PedHash pedHash = PedHash.Abigail;
                if (Enum.TryParse<PedHash>(inputParams[1], out pedHash)) Program.console.log.AppendLog(inputParams[1] + hashAppend + ((uint)pedHash).ToString());
                else
                {
                    WeaponHash weaponHash = WeaponHash.AdvancedRifle;
                    if (Enum.TryParse<WeaponHash>(inputParams[1], out weaponHash)) Program.console.log.AppendLog(inputParams[1] + hashAppend + ((uint)pedHash).ToString());
                    else return false;
                }
            }
            return true;
        }
    }

    /// <summary>
    /// Command to clear the console logs
    /// </summary>
    public class ChangePlayerSkin : Command
    {
        public ChangePlayerSkin()
        {
            commandName = "changeplayermodel";
            shortName = "cps";
            help = "Change the players skin. Params -> [PedHash/Name]";
            autoFill.Add(HintType.Ped);
        }

        public override bool Run(string[] inputParams)
        {
            PedHash pedHash = PedHash.Abigail;
            if (!Enum.TryParse<PedHash>(inputParams[1], out pedHash)) return false;

            Model characterModel = new Model(pedHash);
            characterModel.Request(500);

            if (characterModel.IsInCdImage && characterModel.IsValid)
            {
                while (!characterModel.IsLoaded) Script.Wait(100);

                Function.Call(Hash.SET_PLAYER_MODEL, Game.Player, characterModel.Hash);
                Function.Call(Hash.SET_PED_DEFAULT_COMPONENT_VARIATION, Game.Player.Character.Handle);
            }

            characterModel.MarkAsNoLongerNeeded();
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
            help = "Clears console logs";
        }

        public override bool Run(string[] inputParams)
        {
            Program.console.log.ClearLogs();
            return true;
        }
    }

    /// <summary>
    /// Clears the players wanted level
    /// </summary>
    public class ClearWantedLevel : Command
    {
        public ClearWantedLevel()
        {
            commandName = "clearwantedlevel";
            shortName = "cwl";
            help = "Clears the players wanted level";
        }

        public override bool Run(string[] inputParams)
        {
            Game.Player.WantedLevel = 0;
            return true;
        }
    }

    /// <summary>
    /// Sets the players wanted level
    /// </summary>
    public class SetWantedLevel : Command
    {
        public SetWantedLevel()
        {
            commandName = "setwantedlevel";
            shortName = "swl";
            help = "Clears the players wanted level.";
        }

        public override bool Run(string[] inputParams)
        {
            int wantedLevel = 0;
            if (int.TryParse(inputParams[1], out wantedLevel) && wantedLevel < 6 && wantedLevel >= 0) Game.Player.WantedLevel = wantedLevel;
            return true;
        }
    }

    /// <summary>
    /// Sets the time scale
    /// </summary>
    public class SetTimeScale : Command
    {
        public SetTimeScale()
        {
            commandName = "settimescale";
            shortName = "st";
            help = "Sets the timescale. Params -> [TimeScale]";
        }

        public override bool Run(string[] inputParams)
        {
            int timeScale = 0;
            if (int.TryParse(inputParams[1], out timeScale) && timeScale >= 0) Game.TimeScale = timeScale;
            return true;
        }
    }

    /// <summary>
    /// Sets the time scale
    /// </summary>
    public class SetMoney : Command
    {
        public SetMoney()
        {
            commandName = "setmoney";
            shortName = "sm";
            help = "Sets the money for the player. Params -> [Money]";
        }

        public override bool Run(string[] inputParams)
        {
            int money = 0;
            if (int.TryParse(inputParams[1], out money) && money >= 0) Game.Player.Money = money;
            return true;
        }
    }

    /// <summary>
    /// Sets the time scale
    /// </summary>
    public class SetGmodeMode : Command
    {
        public SetGmodeMode()
        {
            commandName = "setmoney";
            shortName = "sm";
            help = "Sets the money for the player. Params -> [On/Off]";
        }

        public override bool Run(string[] inputParams)
        {
            Game.Player.Character.ApplyForce(Game.Player.Character.ForwardVector, Game.Player.Character.Rotation, ForceType.MaxForceRot);
            int money = 0;
            if (int.TryParse(inputParams[1], out money) && money >= 0) Game.Player.Money = money;
            return true;
        }
    }

    /// <summary>
    /// Sets the time scale
    /// </summary>
    public class SetTime : Command
    {
        public SetTime()
        {
            commandName = "settime";
            shortName = "st";
            help = "The the current time. Params -> [Hours (Optional), Minutes (Optional), Seconds (Optional), Days (Optional)]";
        }

        public override bool Run(string[] inputParams)
        {
            int hours = World.CurrentDayTime.Hours;
            int minutes = World.CurrentDayTime.Minutes;
            int seconds = World.CurrentDayTime.Seconds;
            int days = World.CurrentDayTime.Days;
            if (inputParams.Length > 1) int.TryParse(inputParams[1], out hours);
            if (inputParams.Length > 2) int.TryParse(inputParams[2], out minutes);
            if (inputParams.Length > 3) int.TryParse(inputParams[3], out seconds);
            if (inputParams.Length > 4) int.TryParse(inputParams[4], out days);

            TimeSpan timeSpan = new TimeSpan(days, hours, minutes, seconds);
            World.CurrentDayTime = timeSpan;
            return true;
        }
    }

    /// <summary>
    /// Set Player Position
    /// </summary>
    public class SetPosition : Command
    {
        public SetPosition()
        {
            commandName = "setposition";
            shortName = "st";
            help = "The the current time. Params -> [PosX, PosY, PosZ]";
        }

        public override bool Run(string[] inputParams)
        {
            float x = Game.Player.Character.Position.X;
            float y = Game.Player.Character.Position.Y;
            float z = Game.Player.Character.Position.Z;
            if (inputParams.Length > 1) float.TryParse(inputParams[1], out x);
            if (inputParams.Length > 2) float.TryParse(inputParams[2], out y);
            if (inputParams.Length > 3) float.TryParse(inputParams[3], out z);
            Game.Player.Character.Position = new Vector3(x, y, z);
            return true;
        }
    }

    /// <summary>
    /// Base class for setting car colour
    /// </summary>
    public abstract class SetVehicleColourBase : Command
    {
        public SetVehicleColourBase()
        {
            help = "Change the vehicles colour. Params -> [Alpha, R, G, B]";
        }

        public override bool Run(string[] inputParams)
        {
            int a = Game.Player.LastVehicle.CustomPrimaryColor.A;
            int r = Game.Player.LastVehicle.CustomPrimaryColor.R;
            int g = Game.Player.LastVehicle.CustomPrimaryColor.G;
            int b = Game.Player.LastVehicle.CustomPrimaryColor.B;

            if (inputParams.Length > 1) int.TryParse(inputParams[1], out a);
            if (inputParams.Length > 2) int.TryParse(inputParams[2], out r);
            if (inputParams.Length > 3) int.TryParse(inputParams[3], out g);
            if (inputParams.Length > 4) int.TryParse(inputParams[4], out b);
            return Run(Color.FromArgb(a, r, g, b));
        }

        public abstract bool Run(Color color);
    }

    /// <summary>
    /// Set Vehicle Primary Colour
    /// </summary>
    public class SetVehiclePrimaryColour : SetVehicleColourBase
    {
        public SetVehiclePrimaryColour()
        {
            commandName = "setvehicleprimarycolour";
            shortName = "svpc";
        }

        public override bool Run(Color color)
        {
            Game.Player.LastVehicle.CustomPrimaryColor = color;
            return true;
        }
    }

    /// <summary>
    /// Set Vehicle Primary Colour
    /// </summary>
    public class SetVehicleSecondaryColour : SetVehicleColourBase
    {
        public SetVehicleSecondaryColour()
        {
            commandName = "setvehiclesecondarycolour";
            shortName = "svsc";
        }

        public override bool Run(Color color)
        {
            Game.Player.LastVehicle.CustomSecondaryColor = color;
            return true;
        }
    }

    /// <summary>
    /// Set Night Visison
    /// </summary>
    public class SetNightVision : Command
    {

        public SetNightVision()
        {
            commandName = "setnightvision";
            shortName = "snv";
            help = "Sets the Night Vision. Params -> [On/Off)]";
        }

        public override bool Run(string[] inputParams)
        {
            if (inputParams[1].Equals(on)) Game.Nightvision = true;
            else
            {
                if (inputParams[1].Equals(off)) Game.Nightvision = false;
            }
            return true;
        }
    }

    /// <summary>
    /// Set Thermal Visison
    /// </summary>
    public class SetThermalVision : Command
    {

        public SetThermalVision()
        {
            commandName = "setthermalvision";
            shortName = "stv";
            help = "Sets the Thermal Vision. Params -> [On/Off]";
        }

        public override bool Run(string[] inputParams)
        {
            if (inputParams[1].Equals(on)) Game.ThermalVision = true;
            else
            {
                if (inputParams[1].Equals(off)) Game.ThermalVision = false;
            }
            return true;
        }
    }

    /// <summary>
    /// Get an entity given a raycast from crosshair
    /// </summary>
    public class GetRayCast : Command
    {
        public GetRayCast()
        {
            commandName = "get";
            shortName = "g";
            help = "Gets object player is looking at";
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
    /// Explode where the player is looking
    /// </summary>
    public class Explode : Command
    {
        RaycastResult ray;
        int radius = 15;

        public Explode()
        {
            commandName = "explode";
            shortName = "ex";
            help = "Cause an explode where the player is looking. Params -> [Radius]";
        }

        public override bool Run(string[] inputParams)
        {
            ray = World.GetCrosshairCoordinates();

            if (ray.DitHitAnything)
            {
                if (inputParams[1].Length > 1) int.TryParse(inputParams[1], out radius);
                World.AddExplosion(ray.HitCoords, ExplosionType.Rocket, radius, 0);
                return true;
            }
            return false;
        }
    }

    /// <summary>
    /// Change the weather
    /// </summary>
    public class ChangeWeather : Command
    {
        Weather weather = Weather.Clear;
        float duration = 500;

        public ChangeWeather()
        {
            commandName = "changeweather";
            shortName = "cw";
            help = "Gets object player is looking at";
            autoFill.Add(HintType.Weather);

        }

        public override bool Run(string[] inputParams)
        {
            if (inputParams.Length > 2)
                float.TryParse(inputParams[2], out duration); // Get duration
            if (Enum.TryParse<Weather>(inputParams[1], out weather))
            {
                World.TransitionToWeather(weather, duration);
                return true;
            }
            else
            {
                World.TransitionToWeather((Weather)int.Parse(inputParams[1]), duration);
                return true;
            }
        }
    }

    /// <summary>
    /// Add Weapon
    /// </summary>
    public class AddWeapon : Command
    {
        WeaponHash weaponHash = WeaponHash.AdvancedRifle;
        int ammo = 9999;

        public AddWeapon()
        {
            commandName = "addweapon";
            shortName = "aw";
            help = "Add weapon to the player. Params -> [Weapon,Amount]";
            autoFill.Add(HintType.Weapon);

        }

        public override bool Run(string[] inputParams)
        {
            if (inputParams.Length > 2)
                int.TryParse(inputParams[2], out ammo);
            if (Enum.TryParse<WeaponHash>(inputParams[1], out weaponHash))
            {
                Game.Player.Character.Weapons.Give(weaponHash, ammo, true, true);
                return true;
            }
            else
            {
                Game.Player.Character.Weapons.Give((WeaponHash)int.Parse(inputParams[1]), ammo, true, true);
                return true;
            }
        }
    }

    /// <summary>
    /// Set the weather
    /// </summary>
    public class SetWeather : Command
    {
        Weather weather = Weather.Clear;

        public SetWeather()
        {
            commandName = "setweather";
            shortName = "sw";
            help = "Set the weather";
            autoFill.Add(HintType.Weather);

        }

        public override bool Run(string[] inputParams)
        {
            if (Enum.TryParse<Weather>(inputParams[1], out weather))
            {
                World.Weather = weather;
                return true;
            }
            return false;
        }
    }

    /// <summary>
    /// Set the weather
    /// </summary>
    public class SetControlInConsole : Command
    {
        public SetControlInConsole()
        {
            commandName = "setconsolecontrols";
            shortName = "scc";
            help = "Sets if the player use inputs while in the console. Params -> [On/Off]";
        }

        public override bool Run(string[] inputParams)
        {
            if (inputParams[1].Equals(on)) Program.contolsDisabled = true;
            else
            {
                if (inputParams[1].Equals(off)) Program.contolsDisabled = false;
            }
            return true;
        }
    }

    /// <summary>
    /// Give all weapons to the player
    /// </summary>
    public class AllWeapons : Command
    {
        public AllWeapons()
        {
            commandName = "allweapons";
            shortName = "aw";
            help = "Give all weapons to the player";
        }

        public override bool Run(string[] inputParams)
        {
            WeaponHash[] values = (WeaponHash[])Enum.GetValues(typeof(WeaponHash));
            foreach (WeaponHash weapon in values)
            {
                Game.Player.Character.Weapons.Give(weapon, 9999, false, true);
            }
            return true;
        }
    }

    /// <summary>
    /// Set the weather
    /// </summary>
    public class KillAllVehicles : Command
    {
        public KillAllVehicles()
        {
            commandName = "killallvehicles";
            shortName = "dav";
            help = "Kill all vehicles";
        }

        public override bool Run(string[] inputParams)
        {
            foreach(Vehicle vehicle in World.GetNearbyVehicles(Game.Player.Character.Position, 1000))
            {
                if (vehicle != Game.Player.LastVehicle) vehicle.Explode();
            }
            return true;
        }
    }

    /// <summary>
    /// Set the weather
    /// </summary>
    public class KillAllPeps : Command
    {
        public KillAllPeps()
        {
            commandName = "killallpeps";
            shortName = "kap";
            help = "Destroy all peps";
        }

        public override bool Run(string[] inputParams)
        {
            foreach (Ped pep in World.GetNearbyPeds(Game.Player.Character.Position, 1000))
            {
                if (pep != Game.Player.Character) pep.Health = -1;
            }
            return true;
        }
    }

    /// <summary>
    /// Set player godemode
    /// </summary>
    public class SetGodeMode : Command
    {
        public SetGodeMode()
        {
            commandName = "setgodmode";
            shortName = "sgm";
            help = "Sets player godmode. Params -> [On/Off]";
        }

        public override bool Run(string[] inputParams)
        {
            if (inputParams[1].Equals(on)) Game.Player.IsInvincible = true;
            else
            {
                if (inputParams[1].Equals(off)) Game.Player.IsInvincible = false;
            }
            return true;
        }
    }

    /// <summary>
    /// Set player godemode
    /// </summary>
    public class SetVehicleGodeMode : Command
    {
        public SetVehicleGodeMode()
        {
            commandName = "setvehiclegodmode";
            shortName = "svgm";
            help = "Sets godmode on the players vehicle. Params -> [On/Off]";
        }

        public override bool Run(string[] inputParams)
        {
            if (inputParams[1].Equals(on)) Game.Player.LastVehicle.IsInvincible = true;
            else
            {
                if (inputParams[1].Equals(off)) Game.Player.LastVehicle.IsInvincible = false;
            }
            return true;
        }
    }

    /// <summary>
    /// Set the weather
    /// </summary>
    public class Frenzy : Command
    {
        public Frenzy()
        {
            commandName = "frenzy";
            shortName = "frenzy";
            help = "Pepes go on a freenzy";
        }

        public override bool Run(string[] inputParams)
        {
            Ped[] peds = World.GetNearbyPeds(Game.Player.Character.Position, 1000);
            Vehicle[] vehicles = World.GetNearbyVehicles(Game.Player.Character.Position, 1000);
             
            foreach (Ped pep in peds)
            {
                if (!pep.IsPlayer)
                {
                    pep.Task.FightAgainst(peds[Program.random.Next(0, peds.Length)], 120);//World.GetClosestPed(pep.Position, 1000));
                }
            }
            foreach (Vehicle vehicle in vehicles)
            {
                if (Game.Player.LastVehicle != vehicle)
                {
                    vehicle.EnginePowerMultiplier = Program.random.Next(0, 1000);
                    vehicle.EngineTorqueMultiplier = Program.random.Next(0, 1000);
                    vehicle.MaxSpeed = Program.random.Next(0, 1000);
                    vehicle.Speed = Program.random.Next(-1000, 1000);
                    vehicle.HasGravity = Program.random.Next(0, 2) == 1 ? true : false;
                    vehicle.HighBeamsOn = Program.random.Next(0, 2) == 1 ? true : false;
                    vehicle.LightsOn = Program.random.Next(0, 2) == 1 ? true : false;
                }
            }
            return true;
        }
    }
}