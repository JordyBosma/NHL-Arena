using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Commands;
using Newtonsoft.Json;
using WorldObjects;

namespace Clients
{
    public class ClientReceiveManager
    {

        public ClientReceiveManager()
        {

        }

        public List<Command> ReceiveString(string cmdString)
        {
            try
            {
                dynamic jsonobject = JsonConvert.DeserializeObject(cmdString, new JsonSerializerSettings { CheckAdditionalContent = false });
                return ConvertToCommands(jsonobject);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(cmdString);
                System.Diagnostics.Debug.WriteLine(e);
                return null;
            }
        }

        /// <summary>
        /// converts a json object to commands fo the commandmanager
        /// </summary>
        /// <param name="json">json object</param>
        /// <returns>a list of commands from tthe json object</returns>
        private List<Command> ConvertToCommands(dynamic json)
        {
            try
            {
                List<Command> cmdlist = new List<Command>();
                for (int i = 0; i < json.Count; i++)
                {
                    Random rnd = new Random();

                    switch (json[i].commandType.Value)
                    {
                        case "HitCommand":
                            cmdlist.Add(new HitCommand(new Guid(json[i].shootingPlayerGuid.Value), new Guid(json[i].hitPlayerGuid.Value), (int)json[i].damage.Value));
                            break;
                        case "FireCommand":
                            cmdlist.Add(new FireCommand((int)json[i].weaponId.Value, new Guid(json[i].originPlayer.Value), new double[]{ json[i].directionVector[0], json[i].directionVector[1], json[i].directionVector[2] },
                                new double[] { json[i].originPosition[0], json[i].originPosition[1], json[i].originPosition[2] }, json[i].velocity.Value));
                            break;
                        case "UpdatePlayerCommand":
                            cmdlist.Add(new UpdatePlayerCommand(new Guid(json[i].playerGuid.Value), 
                                double.Parse(json[i].x.Value, CultureInfo.InvariantCulture), double.Parse(json[i].y.Value, CultureInfo.InvariantCulture), 
                                double.Parse(json[i].z.Value, CultureInfo.InvariantCulture), double.Parse(json[i].rotationX.Value, CultureInfo.InvariantCulture), 
                                double.Parse(json[i].rotationY.Value, CultureInfo.InvariantCulture), double.Parse(json[i].rotationZ.Value, CultureInfo.InvariantCulture)));
                            break;
                        default:
                            cmdlist.Add(null);
                            break;
                    }
                }
                return cmdlist;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLineIf(e is NullReferenceException, "you called something that doesnt exist.");
                System.Diagnostics.Debug.WriteLine(e);
                return null;
            }
        }
    }
}
