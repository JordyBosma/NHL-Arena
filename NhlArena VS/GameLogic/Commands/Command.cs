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

    public class HitCommand : Command
    {
        public HitCommand() : base("HitCommand")
        {

        }
    }

    public class UpdatePlayerStatsCommand : Command
    {
        public UpdatePlayerStatsCommand() : base("UpdatePlayerStatsCommand")
        {

        }
    }

    public class PlayerAmmoPickupCommand : Command
    {
        public PlayerAmmoPickupCommand() : base("PlayerAmmoPickupCommand")
        {

        }
    }

    public class ErrorCommand : Command
    {
        public ErrorCommand() : base("ErrorCommand")
        {

        }
    }

    public class ObjectCommand : Command
    {
        public ObjectCommand(string objectCommandType) : base(objectCommandType)
        {

        }
    }

    public class NewObjectCommand : ObjectCommand
    {
        public NewObjectCommand() : base("NewObjectCommand")
        {

        }
    }
    
    public class UpdateObjectCommand : ObjectCommand
    {
        public UpdateObjectCommand() : base("UpdateObjectCommand")
        {

        }
    }

    public class DeleteObjectCommand : ObjectCommand
    {
        public DeleteObjectCommand() : base("DeleteObjectCommand")
        {

        }
    }
}
