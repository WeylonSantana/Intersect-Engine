//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18408
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Threading;
using System.Collections.Generic;
namespace IntersectServer
{
	public static class GlobalVariables
	{

        //Options
        public static int ServerPort = 4501;
        public static string MySQLHost = "localhost";
        public static int MySQLPort = 3306;
        public static string MySQLUser = "root";
        public static string MySQLPass = "pass";
        public static string MySQLDB = "IntersectDB";

        //Game Maps
		public static Map[] GameMaps;
		public static int mapCount;

        //Game Npcs
        public static NPCBase[] GameNpcs;
        public static int npcCount;

		public static List<Client> clients = new List<Client>();
		public static List<Thread> clientThread = new List<Thread>();
		public static List<Entity> entities = new List<Entity>();
		public static string[] tilesets;

        //Game helping stuff
        public static Random rand = new Random();

        public static int GameTime = 0;

        public static int findOpenEntity()
        {
            for (int i = 0; i < GlobalVariables.entities.Count; i++)
            {
                if (GlobalVariables.entities[i] == null)
                {
                    return i;
                }
                else if (i == GlobalVariables.entities.Count - 1)
                {
                    GlobalVariables.entities.Add(null);
                    return GlobalVariables.entities.Count - 1;
                }
            }
            GlobalVariables.entities.Add(null);
            return GlobalVariables.entities.Count - 1;
        }
	}
}

