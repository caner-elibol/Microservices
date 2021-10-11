using CommandService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommandService.Data
{
    public interface ICommandRepo
    {
        //Platforms
        bool SaveChanges();
        IEnumerable<Platform> GetAllPlatforms();    
        void CreatePlatform(Platform plat);
        bool PlatformExists(int platformId);

        //Commands
        IEnumerable<Command> GetCommandsForPlatform(int platformId);
        Command GetCommand(int platformId, int commandId);
        void CreateCommand(int platformId, Command command);



    }
}
