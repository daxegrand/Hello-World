using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace CRockies_one.Models
{
    public class PlayerModels
    {
        public player_table ptable { get; set; }
    }
    public class NEFPitcherModels
    {
        public sproc_playerinfo_Result playerinfo { get; set; }
        public UserEntered UserEntered { get; set; }
        public UserEntered2 UserEntered2 { get; set; }
        public PitcherSplit PitcherSplit { get; set; }
        public Pitcher Pitcher { get; set; }
        public Outfielder_Pitcher Outfielder_Pitcher { get; set; }   
    }
    public class NEFHitterModels
    {
        public sproc_playerinfo_Result playerinfo { get; set; }
        public HitterProfile_UserEntered UserEntered { get; set; }
    }
    #region "DEPTH CHART"
    public class DepthChartModels
    {
        public Team_table dcteam { get; set; }
    }
    #endregion

    #region "NOTES ENTRY FORM"
    public class NotesEntryForm
    {
        public Team_table nefteam { get; set; }
        public List<cr_sproc_NEF_Pitcher_Result> ptable { get; set; }
        public List<cr_sproc_NEF_Hitter_Result> htable { get; set; }
    }
    #endregion

    #region "DUGOUT PITCHER"
    public class DugoutPitcherModels
    {
        public PitcherProfile_PitchCountLastStart PP_PitchCountLastStart { get; set; } //Ronnel: 2/10/14
        public sproc_playerinfo_Result playerinfo { get; set; }
        public PitcherProfile_CurrentAndSplits PitcherProfile { get; set; }
        public UserEntered UserEntered { get; set; }
        public UserEntered2 UserEntered2 { get; set; }
        public Pitcher PitcherNote { get; set; }
        public DugoutPitcher DugoutPitcher { get; set; }
    }
    #endregion

    #region "HITTER PROFILE"
    public class HitterProfileModels
    {
        public sproc_playerinfo_Result playerinfo { get; set; }
        public HitterProfile HitterProfile { get; set; }
        public HitterProfile_UserEntered UserEntered { get; set; }
    }
    #endregion

    #region "OUTFIELDER/PITCHER"
    public class OutfielderPitcherModels
    {
        public List<cr_sproc_outfielderpitcher_agg_Result> ListAgressiveness { get; set; }
        public List<cr_sproc_outfielderpitcher_sta_Result> ListStartingPitchers { get; set; }
        public Outfielder_Pitcher OutfielderPitcher { get; set; }
        public Team_table opteam { get; set; }
    }
    #endregion

    #region "RUNNER CHART"
    public class RunnerChartModels
    {
        public List<cr_sproc_runnerchart_Result> ListRunnerChart { get; set; }
        public UserEntered UserEntereds { get; set; }
        public Team_table rcteam { get; set; }
    }
    #endregion

    #region "PITCHER SPLITS"
    public class PitcherSplitsModels
    {
        public List<cr_sproc_pitchersplits_Result> ListPitcherSplits { get; set; }
        public PitcherSplit PitcherSplits { get; set; }
        public Team_table team { get; set; }
    }
    #endregion

    #region "PITCHER PROFILE"
    public class PitcherProfileModels
    {
        DevColoradoRockiesEntities crEntity = new DevColoradoRockiesEntities();
        InsideEdEntities ieEntity = new InsideEdEntities();

        public PitcherProfile_PitchCountLastStart PP_PitchCountLastStart { get; set; } //Ronnel: 2/10/14
        public sproc_playerinfo_Result playerinfo { get; set; }
        public cr_sproc_pitcherprofile_currentoverallstats_Result currentoverallstats { get; set; }
        public cr_sproc_pitcherprofile_currentstatsvslhh_Result currentoverallstatsvslhh { get; set; }
        public cr_sproc_pitcherprofile_currentstatsvsrhh_Result currentoverallstatsvsrhh { get; set; }
        public UserEntered2 UserEntered2 { get; set; }
        public UserEntered UserEntereds { get; set; }
        public Pitcher PitcherNote { get; set; }
        public PitcherProfile_CurrentAndSplits PP_CurrentAndSplits { get; set; }
        

        public bool exist(long? _pid)
        {
            var result = (from cr in crEntity.cr_sproc_pitcherprofile_currentoverallstats(_pid)
                         select cr.Record);
            var count = result.Count();
            if (count > 0)
                return true;
            else
                return false;
        }


    }
    
    #endregion
    
}