using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldObjects;

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
        public Guid shootingPlayer { get; }
        public Guid hitPlayer { get; }
        public int damage { get; }

        public HitCommand(Guid shootingPlayer, Guid hitPlayer, int damage) : base("HitCommand")
        {
            this.shootingPlayer = shootingPlayer;
            this.hitPlayer = hitPlayer;
            this.damage = damage;
        }
    }

    /// <summary>
    /// player death van server naar client
    /// update positie naar deathpos
    /// </summary>
    public class DeathCommand : Command
    {
        public Player deadPlayer { get; }

        public DeathCommand(Player deadPlayer) : base("DeathCommand")
        {
            this.deadPlayer = deadPlayer;
        }
    }

    /// <summary>
    /// update de hud van de speler server naar client
    /// </summary>
    public class UpdatePlayerStatsCommand : Command
    {
        public Player targetPlayer { get; }

        public UpdatePlayerStatsCommand(Player updatePlayer) : base("UpdatePlayerStatsCommand")
        {
            this.targetPlayer = targetPlayer;
        }
    }

    /// <summary>
    /// pak ammo op command vanaf server naar client
    /// </summary>
    public class PlayerAmmoPickupCommand : Command
    {
        public string ammoType { get; }
        public Player targetPlayer;

        public PlayerAmmoPickupCommand(string ammoType, Player targetPlayer) : base("PlayerAmmoPickupCommand")
        {
            this.ammoType = ammoType;
            this.targetPlayer = targetPlayer;
        }
    }

    /// <summary>
    /// Error server naar client
    /// </summary>
    public class ErrorCommand : Command
    {
        public string errorMessage { get; }

        public ErrorCommand(string errorMessage) : base("ErrorCommand")
        {
            this.errorMessage = errorMessage;
        }
    }

    /// <summary>
    /// general object command
    /// </summary>
    public class ObjectCommand : Command
    {
        

        public ObjectCommand(string objectCommandType) : base(objectCommandType)
        {
            
        }
    }

    public class NewObjectCommand : ObjectCommand
    {
        public NewObjectCommand() : base("NewObjectCommand", model)
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
