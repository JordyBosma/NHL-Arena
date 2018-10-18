using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Commands
{
    public class Command
    {
        public string commandType { get; }

        public Command(string commandType)
        {
            this.commandType = commandType;
        }
    }

    /// <summary>
    /// gemaakte hits van een spele op andere spelers
    /// </summary>
    public class HitCommand : Command
    {
        public HitCommand() : base("HitCommand")
        {

        }
    }

    /// <summary>
    /// player death van server naar client
    /// </summary>
    public class DeathCommand : Command
    {
        public DeathCommand() : base("DeathCommand")
        {

        }
    }

    /// <summary>
    /// update de hud van de speler server naar client
    /// </summary>
    public class UpdatePlayerStatsCommand : Command
    {
        public UpdatePlayerStatsCommand() : base("UpdatePlayerStatsCommand")
        {

        }
    }

    /// <summary>
    /// pak ammo op command vanaf server naar client
    /// </summary>
    public class PlayerAmmoPickupCommand : Command
    {
        public PlayerAmmoPickupCommand() : base("PlayerAmmoPickupCommand")
        {

        }
    }

    /// <summary>
    /// Error server naar client
    /// </summary>
    public class ErrorCommand : Command
    {
        public ErrorCommand() : base("ErrorCommand")
        {

        }
    }

    /// <summary>
    /// general object command
    /// </summary>
    public class ObjectCommand : Command
    {
        public string model { get; }

        public ObjectCommand(string objectCommandType, string model) : base(objectCommandType)
        {
            this.model = model;
        }
    }

    public class NewObjectCommand : ObjectCommand
    {
        public NewObjectCommand(string model) : base("NewObjectCommand", model)
        {

        }
    }

    public class UpdateObjectCommand : ObjectCommand
    {
        public UpdateObjectCommand(string model) : base("UpdateObjectCommand", model)
        {

        }
    }

    public class DeleteObjectCommand : ObjectCommand
    {
        public DeleteObjectCommand(string model) : base("DeleteObjectCommand", model)
        {

        }
    }
}
