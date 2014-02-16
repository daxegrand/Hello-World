using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRockies_one.Models
{
    public class MPlayers: MBase_IE
    {
        private MPlayers() {}
        private static MPlayers instance = null;

        public static MPlayers Instance
        {
            get
            {
                if (instance == null)
                    instance = new MPlayers();
                return instance;
            }
        }
        public List<CPlayer> GetPlayersByTeam(int id,string horp)
        {
            var result = from p in IE_Entities.player_tables
                         orderby p.Lastname
                         where p.Team_ID == id && p.Hitter_or_Pitcher.Equals(horp) && p.Active_ == true
                         select new CPlayer
                         {
                             PlayerId = (long)p.Player_ID,
                             Firstname = p.Firstname,
                             Lastname = p.Lastname,
                             TeamId = (int)p.Team_ID.Value
                         };

            return result.ToList();
        }
    }
}