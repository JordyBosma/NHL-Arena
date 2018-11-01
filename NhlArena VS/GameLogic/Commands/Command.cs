﻿using System;
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
    /// This is only a trigger for the client.cs, DO NOT SEND THROUGH SOCKET!!!!
    /// DO NOT SEND THROUGH SOCKET!!!!
    /// DO NOT SEND THROUGH SOCKET!!!!
    /// DO NOT SEND THROUGH SOCKET!!!!
    /// </summary>
    public class SendCommand : Command
    {
        public SendCommand() : base("")
        {

        }
    }

    /// <summary>
    /// This is only a trigger for the client.cs, DO NOT SEND THROUGH SOCKET!!!!
    /// DO NOT SEND THROUGH SOCKET!!!!
    /// DO NOT SEND THROUGH SOCKET!!!!
    /// DO NOT SEND THROUGH SOCKET!!!!
    /// </summary>
    public class DisconnectCommand : Command
    {
        public DisconnectCommand() : base("")
        {

        }
    }

    /// <summary>
    /// gemaakte hits van een spele op andere spelers
    /// </summary>
    public class HitCommand : Command
    {
        public Guid shootingPlayerGuid { get; }
        public Guid hitPlayerGuid { get; }
        public int damage { get; }

        public HitCommand(Guid shootingPlayerGuid, Guid hitPlayerGuid, int damage) : base("HitCommand")
        {
            this.shootingPlayerGuid = shootingPlayerGuid;
            this.hitPlayerGuid = hitPlayerGuid;
            this.damage = damage;
        }
    }

    public class FireCommand : Command
    {
        public Guid originPlayer { get; }
        public double[] directionVector { get; }
        public double[] originPosition { get; }
        public double velocity { get; }

        public FireCommand(Guid originPlayer, double[] directionVector, double[] originPosition, double velocity) : base("FireCommand")
        {
            this.originPlayer = originPlayer;
            this.directionVector = directionVector;
            this.originPosition = originPosition;
            this.velocity = velocity;
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
        public Guid guid { get; }
        public int health { get; }
        public int armour { get; }
        public int kills { get; }
        public int deaths { get; }

        public UpdatePlayerStatsCommand(Player updatePlayer) : base("UpdatePlayerStatsCommand")
        {
            this.guid = updatePlayer.guid;
            this.health = updatePlayer.health;
            this.armour = updatePlayer.armour;
            this.kills = updatePlayer.kills;
            this.deaths = updatePlayer.deaths;
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

    public class InitializePlayerCommand : Command
    {
        public Guid playerGuid { get; }
        public Guid gameGuid { get; }

        public InitializePlayerCommand(Guid playerGuid, Guid gameGuid) : base("InitializePlayerCommand")
        {
            this.playerGuid = playerGuid;
            this.gameGuid = gameGuid;
        }
    }

    public class UpdatePlayerCommand : Command
    {
        public Guid playerGuid { get; }
        public double x { get; }
        public double y { get; }
        public double z { get; }
        public double rotationX { get; }
        public double rotationY { get; }
        public double rotationZ { get; }

        public UpdatePlayerCommand(Guid playerGuid, double x, double y, double z, double rotationX, double rotationY, double rotationZ): base("UpdatePlayerCommand")
        {
            this.playerGuid = playerGuid;
            this.x = x;
            this.y = y;
            this.z = z;
            this.rotationX = rotationX;
            this.rotationY = rotationY;
            this.rotationZ = rotationZ;
        }
    }

    /// <summary>
    /// general object command
    /// </summary>
    public class ObjectCommand : Command
    {
        public Object3D obj { get; }

        public ObjectCommand(string objectCommandType, Object3D obj) : base(objectCommandType)
        {
            this.obj = obj;
        }
    }

    public class NewObjectCommand : ObjectCommand
    {
        public NewObjectCommand(Object3D obj) : base("NewObjectCommand", obj)
        {

        }
    }

    public class UpdateObjectCommand : ObjectCommand
    {
        public UpdateObjectCommand(Object3D obj) : base("UpdateObjectCommand", obj)
        {

        }
    }

    public class DeleteObjectCommand : ObjectCommand
    {
        public DeleteObjectCommand(Object3D obj) : base("DeleteObjectCommand", obj)
        {

        }
    }
}
