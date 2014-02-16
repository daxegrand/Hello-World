using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using CRockies_one.Models;
using System.Web.Security;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Reflection;

namespace CRockies_one.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        DevColoradoRockiesEntities crEntity = new DevColoradoRockiesEntities();
        InsideEdEntities ieEntity = new InsideEdEntities();
        PitcherProfileModels ppMod = new PitcherProfileModels();
        PitcherSplitsModels psMod = new PitcherSplitsModels();
        RunnerChartModels rcMod = new RunnerChartModels();
        OutfielderPitcherModels opMod = new OutfielderPitcherModels();
        HitterProfileModels hpMod = new HitterProfileModels();
        DugoutPitcherModels dpMod = new DugoutPitcherModels();
        NotesEntryForm nefMod = new NotesEntryForm();
        NEFHitterModels nefHMod = new NEFHitterModels();
        NEFPitcherModels nefPMod = new NEFPitcherModels();
        DepthChartModels dcMod = new DepthChartModels();
        [HttpPost]
        public ActionResult AddPlayer(int PlayerID, int TeamID)
        {
            //try
            //{
            PlayerModels p = new PlayerModels();
            
            p.ptable = ieEntity.player_tables.Where(x => x.Player_ID == PlayerID).FirstOrDefault();
            p.ptable.Team_ID = TeamID;
            p.ptable.Active_ = true;

            ieEntity.SaveChanges();
            //}
            //catch (Exception e)
            //{

            //}
            return null;
        }
        public JsonResult SearchPlayers(string term)
        {

            var result1 = (from r in ieEntity.player_tables
                           where r.Lastname.ToLower().StartsWith(term.ToLower()) && r.Active_ == true
                           && (r.Hitter_or_Pitcher != null || !r.Hitter_or_Pitcher.Equals("n",StringComparison.OrdinalIgnoreCase))
                           select new { r.Player_ID, r.Firstname, r.Lastname }).Distinct()
                          .Except(from r in ieEntity.player_tables
                                  where r.Firstname.ToLower().StartsWith(term.ToLower()) && r.Active_ == true
                           && (r.Hitter_or_Pitcher != null || !r.Hitter_or_Pitcher.Equals("n",StringComparison.OrdinalIgnoreCase))
                                  select new { r.Player_ID, r.Firstname, r.Lastname }).OrderBy(x => x.Lastname);

            var result2 = (from r in ieEntity.player_tables
                           where r.Firstname.ToLower().StartsWith(term.ToLower()) && r.Active_ == true
                           && (r.Hitter_or_Pitcher != null || !r.Hitter_or_Pitcher.Equals("n",StringComparison.OrdinalIgnoreCase))
                           select new { r.Player_ID, r.Firstname, r.Lastname }).Distinct()
                          .Except(from r in ieEntity.player_tables
                                  where r.Lastname.ToLower().StartsWith(term.ToLower()) && r.Active_ == true
                           && (r.Hitter_or_Pitcher != null || !r.Hitter_or_Pitcher.Equals("n",StringComparison.OrdinalIgnoreCase))
                                  select new { r.Player_ID, r.Firstname, r.Lastname }).OrderBy(x => x.Lastname);

            var resultall = result1.Concat(result2);

            return Json(resultall, JsonRequestBehavior.AllowGet);
        }
        //public JsonResult SearchPlayers(string term)
        //{

        //    var result1 = (from r in ieEntity.vwPlayers4CR
        //                   where r.Surname.ToLower().StartsWith(term.ToLower())
        //                   select new { r.PlayerID, r.GSS, r.Surname }).Distinct()
        //                  .Except(from r in ieEntity.vwPlayers4CR
        //                          where r.Givenname.ToLower().StartsWith(term.ToLower())
        //                          select new { r.PlayerID, r.GSS, r.Surname }).OrderBy(x => x.Surname);

        //    var result2 = (from r in ieEntity.vwPlayers4CR
        //                   where r.Givenname.ToLower().StartsWith(term.ToLower())
        //                   select new { r.PlayerID, r.GSS, r.Surname }).Distinct()
        //                  .Except(from r in ieEntity.vwPlayers4CR
        //                          where r.Surname.ToLower().StartsWith(term.ToLower())
        //                          select new { r.PlayerID, r.GSS, r.Surname }).OrderBy(x => x.Surname);

        //    var resultall = result1.Concat(result2);

        //    return Json(resultall, JsonRequestBehavior.AllowGet);
        //}
        public JsonResult Players(int id,string horp)
        {
            return Json(new
            {
                p = MPlayers.Instance.GetPlayersByTeam(id,horp)
            });
        }
        public ActionResult NEF_Pitcher(long? _pid, int? _teamid, int? _tag)
        {
            if (_pid != null)
            {

                //var result = (from hue in crEntity.HitterProfile_UserEntered
                //              where hue.PlayerId == _pid
                //              select hue).SingleOrDefault();
                //if (result != null)
                //{

                nefPMod.playerinfo = ieEntity.sproc_playerinfo(_pid).FirstOrDefault();
                nefPMod.UserEntered = crEntity.UserEntereds.Where(x => x.PlayerID == _pid).FirstOrDefault();
                nefPMod.UserEntered2 = crEntity.UserEntered2.Where(x => x.PlayerID == _pid).FirstOrDefault();
                nefPMod.PitcherSplit = crEntity.PitcherSplits.Where(x => x.PlayerId == _pid).FirstOrDefault();
                nefPMod.Pitcher = crEntity.Pitchers.Where(x => x.PlayerID == _pid).FirstOrDefault();
                nefPMod.Outfielder_Pitcher = crEntity.Outfielder_Pitcher.Where(x => x.PlayerID == _pid).FirstOrDefault();
                //}
                //else                //{
                //nefHMod = null;
                //}
            }
            else
            {
                nefPMod = null;
            }
            return View(nefPMod);
        }
        public ActionResult NEF_Hitter(long? _pid, int? _teamid, int? _tag)
        {
            if (_pid != null)
            {
                
                //var result = (from hue in crEntity.HitterProfile_UserEntered
                //              where hue.PlayerId == _pid
                //              select hue).SingleOrDefault();
                //if (result != null)
                //{

                    nefHMod.playerinfo = ieEntity.sproc_playerinfo(_pid).FirstOrDefault();
                    nefHMod.UserEntered = crEntity.HitterProfile_UserEntered.Where(x => x.PlayerId == _pid).FirstOrDefault();
                //}
                //else
                //{
                    //nefHMod = null;
                //}
            }
            else
            {
                nefHMod = null;
            }
            return View(nefHMod);
        }

        #region "NOTES ENTRY FORM"
        public ActionResult NotesEntryForm(int? _teamid)
        {
            if (_teamid != 0)
            {
                nefMod.nefteam = (from l in ieEntity.Team_tables
                                  where l.Team_ID == _teamid
                                  select l).FirstOrDefault();

                nefMod.ptable = crEntity.cr_sproc_NEF_Pitcher(2013, _teamid).OrderBy(x => x.Name).ToList();

                nefMod.htable = crEntity.cr_sproc_NEF_Hitter(2013, _teamid, 1).OrderBy(x => x.Name).ToList();
            }
            else
                nefMod = null;

            return View(nefMod);
        }
        #endregion

        #region "DEPTH CHART"
        public ActionResult DepthChart(int? _teamid)
        {
            if (_teamid != null)
            {
                if (_teamid != 0)
                {
                    dcMod.dcteam = (from l in ieEntity.Team_tables
                                    where l.Team_ID == _teamid
                                    select l).FirstOrDefault();
                }
                else
                    dcMod = null;
            }
            else
            {
                dcMod.dcteam = (from l in ieEntity.Team_tables
                                where l.Team_ID == 30
                                select l).FirstOrDefault();
            }
            return View(dcMod);
        }
        #endregion

        #region "DUGOUT PITCHER"
        public ActionResult DugoutPitcher(long? _pid)
        {
            if (_pid != null)
            {
                var result = (from dp in crEntity.PitcherProfile_CurrentAndSplits
                              where dp.PlayerId == _pid
                              select dp).SingleOrDefault();
                if (result != null)
                {
                    //Ronnel: 2/10/14
                    dpMod.PP_PitchCountLastStart = (from l in crEntity.PitcherProfile_PitchCountLastStart
                                                    where l.PlayerId == _pid
                                                    select l).FirstOrDefault();

                    dpMod.playerinfo = ieEntity.sproc_playerinfo(_pid).FirstOrDefault();
                    dpMod.PitcherProfile = crEntity.PitcherProfile_CurrentAndSplits.Where(x => x.PlayerId == _pid).FirstOrDefault();
                    dpMod.UserEntered = crEntity.UserEntereds.Where(x => x.PlayerID == _pid).FirstOrDefault();
                    dpMod.UserEntered2 = crEntity.UserEntered2.Where(x => x.PlayerID == _pid).FirstOrDefault();
                    dpMod.PitcherNote = crEntity.Pitchers.Where(x => x.PlayerID == _pid).FirstOrDefault();
                    dpMod.DugoutPitcher = crEntity.DugoutPitchers.Where(x => x.PlayerId == _pid).FirstOrDefault();
                }
                else
                {
                    dpMod = null;
                }

            }
            else
            {
                dpMod = null;
            }
            return View(dpMod);
        }
        #endregion

        [HttpPost]
        public ActionResult EditPitcherUserEntered(int PlayerID, string ArmSlot, string Control, string Command,
            string Approach, string Outpitch,
            string careerrhb, string careerlhb,
            string rt1B, string rt2B, string rtSS, string pm1B, string pm2B, string ss1B, string lwr2B,
            string agg,string sum, string det, string bun,
            string FB2SMvel, string FB2SMcom, string FB2SMdes,
            string FB4SMvel, string FB4SMcom, string FB4SMdes,
            string Cuttervel, string Cuttercom, string Cutterdes,
            string Slidervel, string Slidercom, string Sliderdes,
            string Curveballvel, string Curveballcom, string Curveballdes,
            string Changeupvel, string Changeupcom, string Changeupdes,
            string Splitfingervel, string Splitfingercom, string Splitfingerdes,
            string Knuckleballvel, string Knuckleballcom, string Knuckleballdes
            )
        {
            //try
            //{
            NEFPitcherModels p = new NEFPitcherModels();

            var result_UE = (from ue in crEntity.UserEntereds
                          where ue.PlayerID == PlayerID
                          select ue).SingleOrDefault();

            if (result_UE != null)
            {
                p.UserEntered = crEntity.UserEntereds.Where(x => x.PlayerID == PlayerID).FirstOrDefault();
                p.UserEntered.ArmSlot = ArmSlot;
                p.UserEntered.Control = Control;
                p.UserEntered.Command = Command;
                p.UserEntered.Approach = Approach;
                p.UserEntered.Out_Pitch = Outpitch;
                p.UserEntered.ReleaseTimes_1B = rt1B;
                p.UserEntered.ReleaseTimes_2B = rt2B;
                p.UserEntered.ReleaseTimes_SS = rtSS;
                p.UserEntered.PickoffMove_1B = pm1B;
                p.UserEntered.PickoffMove_2B = pm2B;
                p.UserEntered.LooksWRunner_2B = lwr2B;
                p.UserEntered.SlideStepTime_1B = ss1B;
            }
            else
            {
                crEntity.UserEntereds.Add(new UserEntered
                {
                    PlayerID = PlayerID,
                    ArmSlot = ArmSlot,
                    Control = Control,
                    Command = Command,
                    Approach = Approach,
                    Out_Pitch = Outpitch,
                    ReleaseTimes_1B = rt1B,
                    ReleaseTimes_2B = rt2B,
                    ReleaseTimes_SS = rtSS,
                    PickoffMove_1B = pm1B,
                    PickoffMove_2B = pm2B,
                    LooksWRunner_2B = lwr2B,
                    SlideStepTime_1B = ss1B
                });
            }

            var result_OP = (from ue in crEntity.Outfielder_Pitcher
                          where ue.PlayerID == PlayerID
                          select ue).SingleOrDefault();

            if (result_OP != null)
            {
                p.Outfielder_Pitcher = crEntity.Outfielder_Pitcher.Where(x => x.PlayerID == PlayerID).FirstOrDefault();
                p.Outfielder_Pitcher.SPAofHit_Summary = sum;
                p.Outfielder_Pitcher.SPAofHit_Detail = det;
                p.Outfielder_Pitcher.SPAofHit_BuntingAbility = bun;
            }
            else
            {
                crEntity.Outfielder_Pitcher.Add(new Outfielder_Pitcher
                {
                    PlayerID = PlayerID,
                    SPAofHit_Summary = sum,
                    SPAofHit_Detail = det,
                    SPAofHit_BuntingAbility = bun
                });
            }

            var result_PS = (from ue in crEntity.PitcherSplits
                          where ue.PlayerId == PlayerID
                          select ue).SingleOrDefault();

            if (result_PS != null)
            {
                p.PitcherSplit = crEntity.PitcherSplits.Where(x => x.PlayerId == PlayerID).FirstOrDefault();
                p.PitcherSplit.CareerRHB = careerrhb;
                p.PitcherSplit.CareerLHB = careerlhb;
            }
            else
            {
                crEntity.PitcherSplits.Add(new PitcherSplit
                {
                    PlayerId = PlayerID,
                    CareerRHB = careerrhb,
                    CareerLHB = careerlhb
                });
            }

            var result_UE2 = (from ue in crEntity.UserEntered2
                          where ue.PlayerID == PlayerID
                          select ue).SingleOrDefault();

            if (result_UE2 != null)
            {
                p.UserEntered2 = crEntity.UserEntered2.Where(x => x.PlayerID == PlayerID).FirstOrDefault();

                p.UserEntered2.FB2SMVelocity = FB2SMvel;
                p.UserEntered2.FB2SMCommand = FB2SMcom;
                p.UserEntered2.FB2SMDescription = FB2SMdes;

                p.UserEntered2.FB4SMVelocity = FB4SMvel;
                p.UserEntered2.FB4SMCommand = FB4SMcom;
                p.UserEntered2.FB4SMDescription = FB4SMdes;

                p.UserEntered2.CutterCommand = Cuttercom;
                p.UserEntered2.SliderCommand = Slidercom;
                p.UserEntered2.CurveballCommand = Curveballcom;
                p.UserEntered2.ChangeupCommand = Changeupcom;
                p.UserEntered2.SplitfingerCommand = Splitfingercom;
                p.UserEntered2.KnuckleballCommand = Knuckleballcom;

            }
            else
            {
                crEntity.UserEntered2.Add(new UserEntered2
                {
                    PlayerID = PlayerID,
                    FB2SMVelocity = FB2SMvel,
                    FB2SMCommand = FB2SMcom,
                    FB2SMDescription = FB2SMdes,
                    FB4SMVelocity = FB4SMvel,
                    FB4SMCommand = FB4SMcom,
                    FB4SMDescription = FB4SMdes,
                    CutterCommand = Cuttercom,
                    SliderCommand = Slidercom,
                    ChangeupCommand = Changeupcom,
                    CurveballCommand = Curveballcom,
                    SplitfingerCommand = Splitfingercom,
                    KnuckleballCommand = Knuckleballcom
                });
            }

            var result_Pitcher = (from ue in crEntity.Pitchers
                           where ue.PlayerID == PlayerID
                           select ue).SingleOrDefault();

            if (result_Pitcher != null)
            {
                p.Pitcher = crEntity.Pitchers.Where(x => x.PlayerID == PlayerID).FirstOrDefault();

                p.Pitcher.CutterVelocity = Cuttervel;
                p.Pitcher.CutterDescription = Cutterdes;

                p.Pitcher.SliderVelocity = Slidervel;
                p.Pitcher.SliderDescription = Sliderdes;

                p.Pitcher.CurveballVelocity = Curveballvel;
                p.Pitcher.CurveballDescription = Curveballdes;

                p.Pitcher.ChangeupVelocity = Changeupvel;
                p.Pitcher.ChangeupDescription = Changeupdes;

                p.Pitcher.SplitfingerVelocity = Splitfingervel;
                p.Pitcher.SplitfingerDescription = Splitfingerdes;

                p.Pitcher.KnuckleballVelocity = Knuckleballvel;
                p.Pitcher.KnuckleballDescription = Knuckleballdes;
            }
            else
            {
                crEntity.Pitchers.Add(new Pitcher
                {
                    PlayerID = PlayerID,
                    CutterVelocity = Cuttervel,
                    CutterDescription = Cutterdes,
                    SliderVelocity = Slidervel,
                    SliderDescription = Sliderdes,
                    CurveballVelocity = Curveballvel,
                    CurveballDescription = Curveballdes,
                    ChangeupVelocity = Changeupvel,
                    ChangeupDescription = Changeupdes,
                    SplitfingerVelocity = Splitfingervel,
                    SplitfingerDescription = Splitfingerdes,
                    KnuckleballVelocity = Knuckleballvel,
                    KnuckleballDescription = Knuckleballdes
                });
            }

            crEntity.SaveChanges();
            //}
            //catch (Exception e)
            //{

            //}
            return null;
        }


        #region "PITCHER PROFILE"
        public ActionResult PitcherProfile(long? _pid)
        {

            if (_pid != null)
            {
                ppMod.playerinfo = ieEntity.sproc_playerinfo(_pid).FirstOrDefault();

                bool exist = ppMod.exist(_pid);
                if (exist == true)
                {
                    //Ronnel: 2/10/14
                    ppMod.PP_PitchCountLastStart = (from l in crEntity.PitcherProfile_PitchCountLastStart
                                                    where l.PlayerId == _pid
                                                    select l).FirstOrDefault();

                    ppMod.currentoverallstats = crEntity.cr_sproc_pitcherprofile_currentoverallstats(_pid).FirstOrDefault();
                    ppMod.currentoverallstatsvslhh = crEntity.cr_sproc_pitcherprofile_currentstatsvslhh(_pid).FirstOrDefault();
                    ppMod.currentoverallstatsvsrhh = crEntity.cr_sproc_pitcherprofile_currentstatsvsrhh(_pid).FirstOrDefault();

                    ppMod.PP_CurrentAndSplits = (from l in crEntity.PitcherProfile_CurrentAndSplits
                                                 where l.PlayerId == _pid
                                                 select l).FirstOrDefault();
                    ppMod.UserEntered2 = (from l in crEntity.UserEntered2
                                          where l.PlayerID == _pid
                                          select l).FirstOrDefault();
                    ppMod.PitcherNote = (from l in crEntity.Pitchers
                                         where l.PlayerID == _pid
                                         select l).FirstOrDefault();

                    ppMod.UserEntereds = (from l in crEntity.UserEntereds
                                          where l.PlayerID == _pid
                                          select l).FirstOrDefault();
                }
                else
                {
                    ppMod = null;
                }
            }
            else
            {
                ppMod = null;
            }
            return View(ppMod);
        }
        [HttpPost]
        public ActionResult Edit2sm(int PlayerID, string vel, string com, string des)
        {
            try
            {

                PitcherProfileModels p = new PitcherProfileModels();

                var result = (from ue in crEntity.UserEntered2
                              where ue.PlayerID == PlayerID
                              select ue).SingleOrDefault();

                if (result != null)
                {
                    p.UserEntered2 = crEntity.UserEntered2.Where(x => x.PlayerID == PlayerID).FirstOrDefault();
                    p.UserEntered2.FB2SMVelocity = vel;
                    p.UserEntered2.FB2SMCommand = com;
                    p.UserEntered2.FB2SMDescription = des;
                }
                else
                {
                    crEntity.UserEntered2.Add(new UserEntered2
                    {
                        PlayerID = PlayerID,
                        FB2SMVelocity = vel,
                        FB2SMCommand = com,
                        FB2SMDescription = des
                    });
                }

                crEntity.SaveChanges();
            }
            catch (Exception e)
            {

            }
            return null;
        }
        [HttpPost]
        public ActionResult Edit4sm(int PlayerID, string vel, string com, string des)
        {
            try
            {

                PitcherProfileModels p = new PitcherProfileModels();

                var result = (from ue in crEntity.UserEntered2
                              where ue.PlayerID == PlayerID
                              select ue).SingleOrDefault();

                if (result != null)
                {
                    p.UserEntered2 = crEntity.UserEntered2.Where(x => x.PlayerID == PlayerID).FirstOrDefault();
                    p.UserEntered2.FB4SMVelocity = vel;
                    p.UserEntered2.FB4SMCommand = com;
                    p.UserEntered2.FB4SMDescription = des;
                }
                else
                {
                    crEntity.UserEntered2.Add(new UserEntered2
                    {
                        PlayerID = PlayerID,
                        FB4SMVelocity = vel,
                        FB4SMCommand = com,
                        FB4SMDescription = des
                    });
                }

                crEntity.SaveChanges();
            }
            catch (Exception e)
            {

            }
            return null;
        }
        [HttpPost]
        public ActionResult Editcut(int PlayerID, string vel, string com, string des)
        {
            try
            {

                PitcherProfileModels p = new PitcherProfileModels();

                var result2 = (from ue in crEntity.UserEntered2
                              where ue.PlayerID == PlayerID
                              select ue).SingleOrDefault();

                if (result2 != null)
                {
                    p.UserEntered2 = crEntity.UserEntered2.Where(x => x.PlayerID == PlayerID).FirstOrDefault();
                    p.UserEntered2.CutterCommand = com;
                }
                else
                {
                    crEntity.UserEntered2.Add(new UserEntered2
                    {
                        PlayerID = PlayerID,
                        CutterCommand = com
                    });
                }

                var result1 = (from ue in crEntity.Pitchers
                               where ue.PlayerID == PlayerID
                               select ue).SingleOrDefault();

                if (result1 != null)
                {
                    p.PitcherNote = crEntity.Pitchers.Where(x => x.PlayerID == PlayerID).FirstOrDefault();
                    p.PitcherNote.CutterVelocity = vel;
                    p.PitcherNote.CutterDescription = des;
                }
                else
                {
                    crEntity.Pitchers.Add(new Pitcher
                    {
                        PlayerID = PlayerID,
                        CutterVelocity = vel,
                        CutterDescription = des
                    });
                }
                crEntity.SaveChanges();
            }
            catch (Exception e)
            {

            }
            return null;
        }
        [HttpPost]
        public ActionResult Editsl(int PlayerID, string vel, string com, string des)
        {
            try
            {

                PitcherProfileModels p = new PitcherProfileModels();

                var result2 = (from ue in crEntity.UserEntered2
                               where ue.PlayerID == PlayerID
                               select ue).SingleOrDefault();

                if (result2 != null)
                {
                    p.UserEntered2 = crEntity.UserEntered2.Where(x => x.PlayerID == PlayerID).FirstOrDefault();
                    p.UserEntered2.SliderCommand = com;
                }
                else
                {
                    crEntity.UserEntered2.Add(new UserEntered2
                    {
                        PlayerID = PlayerID,
                        SliderCommand = com
                    });
                }

                var result1 = (from ue in crEntity.Pitchers
                               where ue.PlayerID == PlayerID
                               select ue).SingleOrDefault();

                if (result1 != null)
                {
                    p.PitcherNote = crEntity.Pitchers.Where(x => x.PlayerID == PlayerID).FirstOrDefault();
                    p.PitcherNote.SliderVelocity = vel;
                    p.PitcherNote.SliderDescription = des;
                }
                else
                {
                    crEntity.Pitchers.Add(new Pitcher
                    {
                        PlayerID = PlayerID,
                        SliderVelocity = vel,
                        SliderDescription = des
                    });
                }
                crEntity.SaveChanges();
            }
            catch (Exception e)
            {

            }
            return null;
        }
        [HttpPost]
        public ActionResult Editcb(int PlayerID, string vel, string com, string des)
        {
            try
            {

                PitcherProfileModels p = new PitcherProfileModels();

                var result2 = (from ue in crEntity.UserEntered2
                               where ue.PlayerID == PlayerID
                               select ue).SingleOrDefault();

                if (result2 != null)
                {
                    p.UserEntered2 = crEntity.UserEntered2.Where(x => x.PlayerID == PlayerID).FirstOrDefault();
                    p.UserEntered2.CurveballCommand = com;
                }
                else
                {
                    crEntity.UserEntered2.Add(new UserEntered2
                    {
                        PlayerID = PlayerID,
                        CurveballCommand = com
                    });
                }

                var result1 = (from ue in crEntity.Pitchers
                               where ue.PlayerID == PlayerID
                               select ue).SingleOrDefault();

                if (result1 != null)
                {
                    p.PitcherNote = crEntity.Pitchers.Where(x => x.PlayerID == PlayerID).FirstOrDefault();
                    p.PitcherNote.CurveballVelocity = vel;
                    p.PitcherNote.CurveballDescription = des;
                }
                else
                {
                    crEntity.Pitchers.Add(new Pitcher
                    {
                        PlayerID = PlayerID,
                        CurveballVelocity = vel,
                        CurveballDescription = des
                    });
                }
                crEntity.SaveChanges();
            }
            catch (Exception e)
            {

            }
            return null;
        }
        [HttpPost]
        public ActionResult Editch(int PlayerID, string vel, string com, string des)
        {
            try
            {

                PitcherProfileModels p = new PitcherProfileModels();

                var result2 = (from ue in crEntity.UserEntered2
                               where ue.PlayerID == PlayerID
                               select ue).SingleOrDefault();

                if (result2 != null)
                {
                    p.UserEntered2 = crEntity.UserEntered2.Where(x => x.PlayerID == PlayerID).FirstOrDefault();
                    p.UserEntered2.ChangeupCommand = com;
                }
                else
                {
                    crEntity.UserEntered2.Add(new UserEntered2
                    {
                        PlayerID = PlayerID,
                        ChangeupCommand = com
                    });
                }

                var result1 = (from ue in crEntity.Pitchers
                               where ue.PlayerID == PlayerID
                               select ue).SingleOrDefault();

                if (result1 != null)
                {
                    p.PitcherNote = crEntity.Pitchers.Where(x => x.PlayerID == PlayerID).FirstOrDefault();
                    p.PitcherNote.ChangeupVelocity = vel;
                    p.PitcherNote.ChangeupDescription = des;
                }
                else
                {
                    crEntity.Pitchers.Add(new Pitcher
                    {
                        PlayerID = PlayerID,
                        ChangeupVelocity = vel,
                        ChangeupDescription = des
                    });
                }
                crEntity.SaveChanges();
            }
            catch (Exception e)
            {

            }
            return null;
        }
        [HttpPost]
        public ActionResult Editspl(int PlayerID, string vel, string com, string des)
        {
            try
            {

                PitcherProfileModels p = new PitcherProfileModels();

                var result2 = (from ue in crEntity.UserEntered2
                               where ue.PlayerID == PlayerID
                               select ue).SingleOrDefault();

                if (result2 != null)
                {
                    p.UserEntered2 = crEntity.UserEntered2.Where(x => x.PlayerID == PlayerID).FirstOrDefault();
                    p.UserEntered2.SplitfingerCommand = com;
                }
                else
                {
                    crEntity.UserEntered2.Add(new UserEntered2
                    {
                        PlayerID = PlayerID,
                        SplitfingerCommand = com
                    });
                }

                var result1 = (from ue in crEntity.Pitchers
                               where ue.PlayerID == PlayerID
                               select ue).SingleOrDefault();

                if (result1 != null)
                {
                    p.PitcherNote = crEntity.Pitchers.Where(x => x.PlayerID == PlayerID).FirstOrDefault();
                    p.PitcherNote.SplitfingerVelocity = vel;
                    p.PitcherNote.SplitfingerDescription = des;
                }
                else
                {
                    crEntity.Pitchers.Add(new Pitcher
                    {
                        PlayerID = PlayerID,
                        SplitfingerVelocity = vel,
                        SplitfingerDescription = des
                    });
                }
                crEntity.SaveChanges();
            }
            catch (Exception e)
            {

            }
            return null;
        }
        [HttpPost]
        public ActionResult Editknb(int PlayerID, string vel, string com, string des)
        {
            try
            {

                PitcherProfileModels p = new PitcherProfileModels();

                var result2 = (from ue in crEntity.UserEntered2
                               where ue.PlayerID == PlayerID
                               select ue).SingleOrDefault();

                if (result2 != null)
                {
                    p.UserEntered2 = crEntity.UserEntered2.Where(x => x.PlayerID == PlayerID).FirstOrDefault();
                    p.UserEntered2.KnuckleballCommand = com;
                }
                else
                {
                    crEntity.UserEntered2.Add(new UserEntered2
                    {
                        PlayerID = PlayerID,
                        KnuckleballCommand = com
                    });
                }

                var result1 = (from ue in crEntity.Pitchers
                               where ue.PlayerID == PlayerID
                               select ue).SingleOrDefault();

                if (result1 != null)
                {
                    p.PitcherNote = crEntity.Pitchers.Where(x => x.PlayerID == PlayerID).FirstOrDefault();
                    p.PitcherNote.KnuckleballVelocity = vel;
                    p.PitcherNote.KnuckleballDescription = des;
                }
                else
                {
                    crEntity.Pitchers.Add(new Pitcher
                    {
                        PlayerID = PlayerID,
                        KnuckleballVelocity = vel,
                        KnuckleballDescription = des
                    });
                }
                crEntity.SaveChanges();
            }
            catch (Exception e)
            {

            }
            return null;
        }
        [HttpPost]
        public ActionResult EditArmSlot(int PlayerID, string ArmSlot)
        {
            //try
            //{
                PitcherProfileModels p = new PitcherProfileModels();

                var result = (from ue in crEntity.UserEntereds
                where ue.PlayerID == PlayerID
                select ue).SingleOrDefault();

                if (result != null)
                {
                    p.UserEntereds = crEntity.UserEntereds.Where(x => x.PlayerID == PlayerID).FirstOrDefault();
                    p.UserEntereds.ArmSlot = ArmSlot;
                }
                else
                {
                    crEntity.UserEntereds.Add(new UserEntered
                    {
                        PlayerID = PlayerID,
                        ArmSlot = ArmSlot
                    });
                }

                crEntity.SaveChanges();
            //}
            //catch (Exception e)
            //{

            //}
            return null;
        }
        [HttpPost]
        public ActionResult EditControl(int PlayerID, string Control)
        {
            try
            {
                PitcherProfileModels p = new PitcherProfileModels();

                var result = (from ue in crEntity.UserEntereds
                              where ue.PlayerID == PlayerID
                              select ue).SingleOrDefault();

                if (result != null)
                {
                    p.UserEntereds = crEntity.UserEntereds.Where(x => x.PlayerID == PlayerID).FirstOrDefault();
                    p.UserEntereds.Control = Control;
                }
                else
                {
                    crEntity.UserEntereds.Add(new UserEntered
                    {
                        PlayerID = PlayerID,
                        Control = Control
                    });
                }

                crEntity.SaveChanges();
            }
            catch (Exception e)
            {

            }
            return null;
        }
        [HttpPost]
        public ActionResult EditCommand(int PlayerID, string Command)
        {
            try
            {
                
                PitcherProfileModels p = new PitcherProfileModels();

                var result = (from ue in crEntity.UserEntereds
                              where ue.PlayerID == PlayerID
                              select ue).SingleOrDefault();

                if (result != null)
                {
                    p.UserEntereds = crEntity.UserEntereds.Where(x => x.PlayerID == PlayerID).FirstOrDefault();
                    p.UserEntereds.Command = Command;
                }
                else
                {
                    crEntity.UserEntereds.Add(new UserEntered
                    {
                        PlayerID = PlayerID,
                        Command = Command
                    });
                }

                crEntity.SaveChanges();
            }
            catch (Exception e)
            {

            }
            return null;
        }
        [HttpPost]
        public ActionResult EditApproachOutpitch(int PlayerID, string Approach, string Outpitch)
        {
            try
            {
                PitcherProfileModels p = new PitcherProfileModels();

                var result = (from ue in crEntity.UserEntereds
                              where ue.PlayerID == PlayerID
                              select ue).SingleOrDefault();

                if (result != null)
                {
                    p.UserEntereds = crEntity.UserEntereds.Where(x => x.PlayerID == PlayerID).FirstOrDefault();
                    p.UserEntereds.Approach = Approach;
                    p.UserEntereds.Out_Pitch = Outpitch;
                }
                else
                {
                    crEntity.UserEntereds.Add(new UserEntered
                    {
                        PlayerID = PlayerID,
                        Approach = Approach,
                        Out_Pitch = Outpitch
                    });
                }

                crEntity.SaveChanges();
            }
            catch (Exception e)
            {

            }
            return null;
        }
        [HttpPost]
        public ActionResult EditReleaseTimes(int PlayerID, string rt1B, string rt2B, string rtSS)
        {
            try
            {
                
                PitcherProfileModels p = new PitcherProfileModels();

                var result = (from ue in crEntity.UserEntereds
                              where ue.PlayerID == PlayerID
                              select ue).SingleOrDefault();

                if (result != null)
                {
                    p.UserEntereds = crEntity.UserEntereds.Where(x => x.PlayerID == PlayerID).FirstOrDefault();
                    p.UserEntereds.ReleaseTimes_1B = rt1B;
                    p.UserEntereds.ReleaseTimes_2B = rt2B;
                    p.UserEntereds.ReleaseTimes_SS = rtSS;
                }
                else
                {
                    crEntity.UserEntereds.Add(new UserEntered
                    {
                        PlayerID = PlayerID,
                        ReleaseTimes_1B = rt1B,
                        ReleaseTimes_2B = rt2B,
                        ReleaseTimes_SS = rtSS
                    });
                }

                crEntity.SaveChanges();
            }
            catch (Exception e)
            {

            }
            return null;
        }
        [HttpPost]
        public ActionResult EditPickoffMove1B(int PlayerID, string pm1B)
        {
            try
            {
                
                PitcherProfileModels p = new PitcherProfileModels();

                var result = (from ue in crEntity.UserEntereds
                              where ue.PlayerID == PlayerID
                              select ue).SingleOrDefault();

                if (result != null)
                {
                    p.UserEntereds = crEntity.UserEntereds.Where(x => x.PlayerID == PlayerID).FirstOrDefault();
                    p.UserEntereds.PickoffMove_1B = pm1B;
                }
                else
                {
                    crEntity.UserEntereds.Add(new UserEntered
                    {
                        PlayerID = PlayerID,
                        PickoffMove_1B = pm1B
                    });
                }
                
                crEntity.SaveChanges();
            }
            catch (Exception e)
            {

            }
            return null;
        }
        [HttpPost]
        public ActionResult EditPickoffMove2B(int PlayerID, string pm2B)
        {
            try
            {
                
                PitcherProfileModels p = new PitcherProfileModels();

                var result = (from ue in crEntity.UserEntereds
                              where ue.PlayerID == PlayerID
                              select ue).SingleOrDefault();

                if (result != null)
                {
                    p.UserEntereds = crEntity.UserEntereds.Where(x => x.PlayerID == PlayerID).FirstOrDefault();
                    p.UserEntereds.PickoffMove_2B = pm2B;
                }
                else
                {
                    crEntity.UserEntereds.Add(new UserEntered
                    {
                        PlayerID = PlayerID,
                        PickoffMove_2B = pm2B
                    });
                }

                crEntity.SaveChanges();
            }
            catch (Exception e)
            {

            }
            return null;
        }
        #endregion

        #region "RUNNER CHART"
        [HttpPost]
        public ActionResult rcEdit(int PlayerID, string rt1B, string ss1B, string pm1B, string rt2B, string pm2B, string lwr2B)
        {
            try
            {

                RunnerChartModels p = new RunnerChartModels();

                var result = (from ue in crEntity.UserEntereds
                              where ue.PlayerID == PlayerID
                              select ue).SingleOrDefault();

                if (result != null)
                {
                    p.UserEntereds = crEntity.UserEntereds.Where(x => x.PlayerID == PlayerID).FirstOrDefault();
                    p.UserEntereds.ReleaseTimes_1B = rt1B;
                    p.UserEntereds.SlideStepTime_1B=ss1B;
                    p.UserEntereds.PickoffMove_1B = pm1B;
                    p.UserEntereds.ReleaseTimes_2B=rt2B;
                    p.UserEntereds.PickoffMove_2B=pm2B;
                    p.UserEntereds.LooksWRunner_2B=lwr2B;
                }
                else
                {
                    crEntity.UserEntereds.Add(new UserEntered
                    {
                        PlayerID = PlayerID,
                        ReleaseTimes_1B = rt1B,
                        SlideStepTime_1B = ss1B,
                        PickoffMove_1B = pm1B,
                        ReleaseTimes_2B = rt2B,
                        PickoffMove_2B = pm2B,
                        LooksWRunner_2B = lwr2B
                    });
                }

                crEntity.SaveChanges();
            }
            catch (Exception e)
            {

            }
            return null;
        }

        public ActionResult RunnerChart(int? _teamid)
        {
            if (_teamid != 0)
            {
                rcMod.ListRunnerChart = crEntity.cr_sproc_runnerchart(2013, _teamid, 1).ToList();
                rcMod.rcteam = (from l in ieEntity.Team_tables
                                where l.Team_ID == _teamid
                                select l).FirstOrDefault();
            }
            else
                rcMod = null;

            return View(rcMod);
        }
        #endregion

        #region "OUTFIELDER/PITCHER"
        [HttpPost]
        public ActionResult EditSPAofHit(int PlayerID, string sum, string det, string bun)
        {
            try
            {

                OutfielderPitcherModels p = new OutfielderPitcherModels();

                var result = (from ue in crEntity.Outfielder_Pitcher
                              where ue.PlayerID == PlayerID
                              select ue).SingleOrDefault();

                if (result != null)
                {
                    p.OutfielderPitcher = crEntity.Outfielder_Pitcher.Where(x => x.PlayerID == PlayerID).FirstOrDefault();
                    p.OutfielderPitcher.SPAofHit_Summary = sum;
                    p.OutfielderPitcher.SPAofHit_Detail = det;
                    p.OutfielderPitcher.SPAofHit_BuntingAbility = bun;
                }
                else
                {
                    crEntity.Outfielder_Pitcher.Add(new Outfielder_Pitcher
                    {
                        PlayerID = PlayerID,
                        SPAofHit_Summary = sum,
                        SPAofHit_Detail = det,
                        SPAofHit_BuntingAbility = bun
                    });
                }

                crEntity.SaveChanges();
            }
            catch (Exception e)
            {

            }
            return null;
        }
        [HttpPost]
        public ActionResult EditAgressiveness(int PlayerID, string agg)
        {
            //try
            //{

                OutfielderPitcherModels p = new OutfielderPitcherModels();

                var result = (from ue in crEntity.Outfielder_Pitcher
                              where ue.PlayerID == PlayerID
                              select ue).SingleOrDefault();

                if (result != null)
                {
                    p.OutfielderPitcher = crEntity.Outfielder_Pitcher.Where(x => x.PlayerID == PlayerID).FirstOrDefault();
                    p.OutfielderPitcher.Aggressiveness = agg;
                }
                else
                {
                    crEntity.Outfielder_Pitcher.Add(new Outfielder_Pitcher
                    {
                        PlayerID = PlayerID,
                        Aggressiveness = agg
                    });
                }

                crEntity.SaveChanges();
            //}
            //catch (Exception e)
            //{

            //}
            return null;
        }
        public ActionResult OutfielderPitcher(int? _teamid)
        {
            if (_teamid != 0)
            {
                opMod.ListAgressiveness = crEntity.cr_sproc_outfielderpitcher_agg(2013, _teamid, 1).ToList();
                opMod.ListStartingPitchers = crEntity.cr_sproc_outfielderpitcher_sta(2013, _teamid, 1).ToList();
                opMod.opteam = (from l in ieEntity.Team_tables
                                where l.Team_ID == _teamid
                                select l).FirstOrDefault();
            }
            else
                opMod = null;

            return View(opMod);
        }
        #endregion

        #region "HITTER PROFILE"

        [HttpPost]
        public ActionResult EditHitterUserEntered(int PlayerID, string Steal, string Push, string Drag
            , string RHP_HowtoPitch, string LHP_HowtoPitch, string RHP_2Ks_Chase, string LHP_2Ks_Chase, string RHP_Def_INF, string RHP_Def_OF
            , string LHP_Def_INF, string LHP_Def_OF
            , int Arm_Avg, int Accuracy_Avg, int Speed_Avg )//Ronnel added 2/12/14
        {
            HitterProfileModels h = new HitterProfileModels();

            var result = (from ue in crEntity.HitterProfile_UserEntered
                          where ue.PlayerId == PlayerID
                          select ue).SingleOrDefault();

            if (result != null)
            {
                h.UserEntered = crEntity.HitterProfile_UserEntered.Where(x => x.PlayerId == PlayerID).FirstOrDefault();
                //h.UserEntered.Arm = Arm;
                //h.UserEntered.Accuracy = Accuracy;
                //h.UserEntered.Speed = Speed;
                h.UserEntered.Steal = Steal;
                h.UserEntered.Push = Push;
                h.UserEntered.Drag = Drag;
                h.UserEntered.RHP_HowtoPitch = RHP_HowtoPitch;
                h.UserEntered.LHP_HowtoPitch = LHP_HowtoPitch;
                h.UserEntered.RHP_2Ks_Chase = RHP_2Ks_Chase;
                h.UserEntered.LHP_2Ks_Chase = LHP_2Ks_Chase;
                h.UserEntered.RHP_Def_INF = RHP_Def_INF;
                h.UserEntered.RHP_Def_OF = RHP_Def_OF;
                h.UserEntered.LHP_Def_INF = LHP_Def_INF;
                h.UserEntered.LHP_Def_OF = LHP_Def_OF;
                h.UserEntered.Arm_Avg = Arm_Avg;
                h.UserEntered.Accuracy_Avg = Accuracy_Avg;
                h.UserEntered.Speed_Avg = Speed_Avg;

            }
            else
            {
                crEntity.HitterProfile_UserEntered.Add(new HitterProfile_UserEntered
                {
                    PlayerId = PlayerID,
                    //Arm = Arm,
                    //Accuracy = Accuracy,
                    //Speed = Speed,
                    Steal = Steal,
                    Push = Push,
                    Drag = Drag,
                    RHP_HowtoPitch = RHP_HowtoPitch,
                    LHP_HowtoPitch = LHP_HowtoPitch,
                    RHP_2Ks_Chase = RHP_2Ks_Chase,
                    LHP_2Ks_Chase = LHP_2Ks_Chase,
                    RHP_Def_INF = RHP_Def_INF,
                    RHP_Def_OF = RHP_Def_OF,
                    LHP_Def_INF = LHP_Def_INF,
                    LHP_Def_OF = LHP_Def_OF,
                    Arm_Avg = Arm_Avg,
                    Accuracy_Avg = Accuracy_Avg,
                    Speed_Avg = Speed_Avg
                });
            }

            crEntity.SaveChanges();
            
            return null;
        }

        [HttpPost]
        public ActionResult EditArm(int PlayerID, int Arm_Avg)
        {
            HitterProfileModels h = new HitterProfileModels();

            var result = (from ue in crEntity.HitterProfile_UserEntered
                          where ue.PlayerId == PlayerID
                          select ue).SingleOrDefault();

            if (result != null)
            {
                h.UserEntered = crEntity.HitterProfile_UserEntered.Where(x => x.PlayerId == PlayerID).FirstOrDefault();
                //h.UserEntered.Arm = Arm;
                h.UserEntered.Arm_Avg = Arm_Avg;
            }
            else
            {
                crEntity.HitterProfile_UserEntered.Add(new HitterProfile_UserEntered
                {
                    PlayerId = PlayerID,
                    //Arm = Arm,
                    Arm_Avg = Arm_Avg
                });
            }

            crEntity.SaveChanges();
            return null;
        }
        [HttpPost]
        public ActionResult EditAccuracy(int PlayerID, int Accuracy_Avg)
        {
            HitterProfileModels h = new HitterProfileModels();

            var result = (from ue in crEntity.HitterProfile_UserEntered
                          where ue.PlayerId == PlayerID
                          select ue).SingleOrDefault();

            if (result != null)
            {
                h.UserEntered = crEntity.HitterProfile_UserEntered.Where(x => x.PlayerId == PlayerID).FirstOrDefault();
                //h.UserEntered.Accuracy = Accuracy;
                h.UserEntered.Accuracy_Avg = Accuracy_Avg;
            }
            else
            {
                crEntity.HitterProfile_UserEntered.Add(new HitterProfile_UserEntered
                {
                    PlayerId = PlayerID,
                    //Accuracy = Accuracy,
                    Accuracy_Avg = Accuracy_Avg
                });
            }

            crEntity.SaveChanges();
            return null;
        }
        [HttpPost]
        public ActionResult EditSpeed(int PlayerID, int Speed_Avg)
        {
            HitterProfileModels h = new HitterProfileModels();

            var result = (from ue in crEntity.HitterProfile_UserEntered
                          where ue.PlayerId == PlayerID
                          select ue).SingleOrDefault();

            if (result != null)
            {
                h.UserEntered = crEntity.HitterProfile_UserEntered.Where(x => x.PlayerId == PlayerID).FirstOrDefault();
                //h.UserEntered.Speed = Speed;
                h.UserEntered.Speed_Avg = Speed_Avg;
            }
            else
            {
                crEntity.HitterProfile_UserEntered.Add(new HitterProfile_UserEntered
                {
                    PlayerId = PlayerID,
                    //Speed = Speed,
                    Speed_Avg = Speed_Avg
                });
            }

            crEntity.SaveChanges();
            return null;
        }
        [HttpPost]
        public ActionResult EditSteal(int PlayerID, string Steal)
        {
            HitterProfileModels h = new HitterProfileModels();

            var result = (from ue in crEntity.HitterProfile_UserEntered
                          where ue.PlayerId == PlayerID
                          select ue).SingleOrDefault();

            if (result != null)
            {
                h.UserEntered = crEntity.HitterProfile_UserEntered.Where(x => x.PlayerId == PlayerID).FirstOrDefault();
                h.UserEntered.Steal = Steal;
            }
            else
            {
                crEntity.HitterProfile_UserEntered.Add(new HitterProfile_UserEntered
                {
                    PlayerId = PlayerID,
                    Steal = Steal
                });
            }

            crEntity.SaveChanges();
            return null;
        }
        [HttpPost]
        public ActionResult EditPush(int PlayerID, string Push)
        {
            HitterProfileModels h = new HitterProfileModels();

            var result = (from ue in crEntity.HitterProfile_UserEntered
                          where ue.PlayerId == PlayerID
                          select ue).SingleOrDefault();

            if (result != null)
            {
                h.UserEntered = crEntity.HitterProfile_UserEntered.Where(x => x.PlayerId == PlayerID).FirstOrDefault();
                h.UserEntered.Push = Push;
            }
            else
            {
                crEntity.HitterProfile_UserEntered.Add(new HitterProfile_UserEntered
                {
                    PlayerId = PlayerID,
                    Push = Push
                });
            }

            crEntity.SaveChanges();
            return null;
        }
        [HttpPost]
        public ActionResult EditDrag(int PlayerID, string Drag)
        {
            HitterProfileModels h = new HitterProfileModels();

            var result = (from ue in crEntity.HitterProfile_UserEntered
                          where ue.PlayerId == PlayerID
                          select ue).SingleOrDefault();

            if (result != null)
            {
                h.UserEntered = crEntity.HitterProfile_UserEntered.Where(x => x.PlayerId == PlayerID).FirstOrDefault();
                h.UserEntered.Drag = Drag;
            }
            else
            {
                crEntity.HitterProfile_UserEntered.Add(new HitterProfile_UserEntered
                {
                    PlayerId = PlayerID,
                    Drag = Drag
                });
            }

            crEntity.SaveChanges();
            return null;
        }
        [HttpPost]
        public ActionResult EditRHP_HowtoPitch(int PlayerID, string RHP_HowtoPitch)
        {
            HitterProfileModels h = new HitterProfileModels();

            var result = (from ue in crEntity.HitterProfile_UserEntered
                          where ue.PlayerId == PlayerID
                          select ue).SingleOrDefault();

            if (result != null)
            {
                h.UserEntered = crEntity.HitterProfile_UserEntered.Where(x => x.PlayerId == PlayerID).FirstOrDefault();
                h.UserEntered.RHP_HowtoPitch = RHP_HowtoPitch;
            }
            else
            {
                crEntity.HitterProfile_UserEntered.Add(new HitterProfile_UserEntered
                {
                    PlayerId = PlayerID,
                    RHP_HowtoPitch = RHP_HowtoPitch
                });
            }

            crEntity.SaveChanges();
            return null;
        }
        [HttpPost]
        public ActionResult EditLHP_HowtoPitch(int PlayerID, string LHP_HowtoPitch)
        {
            HitterProfileModels h = new HitterProfileModels();

            var result = (from ue in crEntity.HitterProfile_UserEntered
                          where ue.PlayerId == PlayerID
                          select ue).SingleOrDefault();

            if (result != null)
            {
                h.UserEntered = crEntity.HitterProfile_UserEntered.Where(x => x.PlayerId == PlayerID).FirstOrDefault();
                h.UserEntered.LHP_HowtoPitch = LHP_HowtoPitch;
            }
            else
            {
                crEntity.HitterProfile_UserEntered.Add(new HitterProfile_UserEntered
                {
                    PlayerId = PlayerID,
                    LHP_HowtoPitch = LHP_HowtoPitch
                });
            }

            crEntity.SaveChanges();
            return null;
        }
        [HttpPost]
        public ActionResult EditRHP_2Ks_Chase(int PlayerID, string RHP_2Ks_Chase)
        {
            HitterProfileModels h = new HitterProfileModels();

            var result = (from ue in crEntity.HitterProfile_UserEntered
                          where ue.PlayerId == PlayerID
                          select ue).SingleOrDefault();

            if (result != null)
            {
                h.UserEntered = crEntity.HitterProfile_UserEntered.Where(x => x.PlayerId == PlayerID).FirstOrDefault();
                h.UserEntered.RHP_2Ks_Chase = RHP_2Ks_Chase;
            }
            else
            {
                crEntity.HitterProfile_UserEntered.Add(new HitterProfile_UserEntered
                {
                    PlayerId = PlayerID,
                    RHP_2Ks_Chase = RHP_2Ks_Chase
                });
            }

            crEntity.SaveChanges();
            return null;
        }
        [HttpPost]
        public ActionResult EditLHP_2Ks_Chase(int PlayerID, string LHP_2Ks_Chase)
        {
            HitterProfileModels h = new HitterProfileModels();

            var result = (from ue in crEntity.HitterProfile_UserEntered
                          where ue.PlayerId == PlayerID
                          select ue).SingleOrDefault();

            if (result != null)
            {
                h.UserEntered = crEntity.HitterProfile_UserEntered.Where(x => x.PlayerId == PlayerID).FirstOrDefault();
                h.UserEntered.LHP_2Ks_Chase = LHP_2Ks_Chase;
            }
            else
            {
                crEntity.HitterProfile_UserEntered.Add(new HitterProfile_UserEntered
                {
                    PlayerId = PlayerID,
                    LHP_2Ks_Chase = LHP_2Ks_Chase
                });
            }

            crEntity.SaveChanges();
            return null;
        }
        [HttpPost]
        public ActionResult EditRHP_Def(int PlayerID, string RHP_Def_INF, string RHP_Def_OF)
        {
            HitterProfileModels h = new HitterProfileModels();

            var result = (from ue in crEntity.HitterProfile_UserEntered
                          where ue.PlayerId == PlayerID
                          select ue).SingleOrDefault();

            if (result != null)
            {
                h.UserEntered = crEntity.HitterProfile_UserEntered.Where(x => x.PlayerId == PlayerID).FirstOrDefault();
                h.UserEntered.RHP_Def_INF = RHP_Def_INF;
                h.UserEntered.RHP_Def_OF = RHP_Def_OF;
            }
            else
            {
                crEntity.HitterProfile_UserEntered.Add(new HitterProfile_UserEntered
                {
                    PlayerId = PlayerID,
                    RHP_Def_INF = RHP_Def_INF,
                    RHP_Def_OF = RHP_Def_OF
                });
            }

            crEntity.SaveChanges();
            return null;
        }
        [HttpPost]
        public ActionResult EditLHP_Def(int PlayerID, string LHP_Def_INF, string LHP_Def_OF)
        {
            HitterProfileModels h = new HitterProfileModels();

            var result = (from ue in crEntity.HitterProfile_UserEntered
                          where ue.PlayerId == PlayerID
                          select ue).SingleOrDefault();

            if (result != null)
            {
                h.UserEntered = crEntity.HitterProfile_UserEntered.Where(x => x.PlayerId == PlayerID).FirstOrDefault();
                h.UserEntered.LHP_Def_INF = LHP_Def_INF;
                h.UserEntered.LHP_Def_OF = LHP_Def_OF;
            }
            else
            {
                crEntity.HitterProfile_UserEntered.Add(new HitterProfile_UserEntered
                {
                    PlayerId = PlayerID,
                    LHP_Def_INF = LHP_Def_INF,
                    LHP_Def_OF = LHP_Def_OF
                });
            }

            crEntity.SaveChanges();
            return null;
        }
        public ActionResult HitterProfile(long? _pid)
        {

            if (_pid != null)
            {
                var result = (from hp in crEntity.HitterProfiles
                              where hp.PlayerId == _pid
                              select hp).SingleOrDefault();
                if (result != null)
                {
                    hpMod.playerinfo = ieEntity.sproc_playerinfo(_pid).FirstOrDefault();
                    hpMod.HitterProfile = crEntity.HitterProfiles.Where(x => x.PlayerId == _pid).FirstOrDefault();
                    hpMod.UserEntered = crEntity.HitterProfile_UserEntered.Where(x => x.PlayerId == _pid).FirstOrDefault();
                }
                else
                {
                    hpMod = null;
                }

            }
            else
            {
                hpMod = null;
            }
            return View(hpMod);
        }
        #endregion

        #region "PITCHER SPLITS"
        [HttpPost]
        public ActionResult EditCareer(int PlayerID, string rhb, string lhb)
        {
            //try
            //{
            PitcherSplitsModels p = new PitcherSplitsModels();

            var result = (from ue in crEntity.PitcherSplits
                          where ue.PlayerId == PlayerID
                          select ue).SingleOrDefault();

            if (result != null)
            {
                p.PitcherSplits = crEntity.PitcherSplits.Where(x => x.PlayerId == PlayerID).FirstOrDefault();
                p.PitcherSplits.CareerRHB= rhb;
                p.PitcherSplits.CareerLHB = lhb;
            }
            else
            {
                crEntity.PitcherSplits.Add(new PitcherSplit
                {
                    PlayerId = PlayerID,
                    CareerRHB = rhb,
                    CareerLHB = lhb
                });
            }

            crEntity.SaveChanges();
            //}
            //catch (Exception e)
            //{

            //}
            return null;
        }

        public ActionResult PitcherSplits(int? _teamid)
        {
            if (_teamid != 0)
            {
                psMod.ListPitcherSplits = crEntity.cr_sproc_pitchersplits(2013, _teamid, 1).ToList();
                psMod.team = (from l in ieEntity.Team_tables
                              where l.Team_ID == _teamid
                              select l).FirstOrDefault();
            }
            else
                psMod = null;

            return View(psMod);
        }
        #endregion

        #region "EXPORT PITCHER PROFILE"
        public ActionResult ExportPitcherProfile(long? _pid)
        {
            try
            {
                /*@Model.playerinfo.Firstname*/
                ppMod.playerinfo = ieEntity.sproc_playerinfo(_pid).FirstOrDefault();
                ppMod.currentoverallstats = crEntity.cr_sproc_pitcherprofile_currentoverallstats(_pid).FirstOrDefault();
                ppMod.currentoverallstatsvslhh = crEntity.cr_sproc_pitcherprofile_currentstatsvslhh(_pid).FirstOrDefault();
                ppMod.currentoverallstatsvsrhh = crEntity.cr_sproc_pitcherprofile_currentstatsvsrhh(_pid).FirstOrDefault();

                ppMod.PP_CurrentAndSplits = (from l in crEntity.PitcherProfile_CurrentAndSplits
                                             where l.PlayerId == _pid
                                             select l).FirstOrDefault();
                ppMod.UserEntered2 = (from l in crEntity.UserEntered2
                                      where l.PlayerID == _pid
                                      select l).FirstOrDefault();
                ppMod.PitcherNote = (from l in crEntity.Pitchers
                                     where l.PlayerID == _pid
                                     select l).FirstOrDefault();

                ppMod.UserEntereds = (from l in crEntity.UserEntereds
                                      where l.PlayerID == _pid
                                      select l).FirstOrDefault();

                string path = Server.MapPath("~/App_Data/PitcherProfile.xlsx");
                FileStream MyFileStream = new FileStream(path, FileMode.Open);
                
                
                ExcelPackage pck = new ExcelPackage(new MemoryStream(), MyFileStream);


                var ws = pck.Workbook.Worksheets[1];
                //int row = new Random().Next(10) + 10;   //Pick a random row to print the text

                /*Pitcher Name*/
                ws.Cells[3, 2].Value = "PITCHER PROFILE - # " + ppMod.playerinfo.Uniform + " " + ppMod.playerinfo.Firstname + " " + ppMod.playerinfo.Lastname + " " + ppMod.playerinfo.Bats + "HP";
                
                /*Current Overall Stats*/
                ws.Cells[8, 2].Value = ppMod.currentoverallstats.Record;
                ws.Cells[8, 3].Value = ppMod.currentoverallstats.G;
                ws.Cells[8, 4].Value = ppMod.currentoverallstats.ERA;
                ws.Cells[8, 5].Value = ppMod.currentoverallstats.IP;
                ws.Cells[8, 6].Value = ppMod.currentoverallstats.H;
                ws.Cells[8, 7].Value = ppMod.currentoverallstats.HR;
                ws.Cells[8, 8].Value = ppMod.currentoverallstats.BB;
                ws.Cells[8, 9].Value = ppMod.currentoverallstats.SO;
                ws.Cells[8, 10].Value = ppMod.currentoverallstats.SBA;
                ws.Cells[8, 11].Value = ppMod.currentoverallstats.PperPA;
                ws.Cells[8, 12].Value = ppMod.currentoverallstats.PperIP;
                ws.Cells[8, 13].Value = ppMod.currentoverallstats.PperGS;                                 
             
                if (!(ppMod.currentoverallstats.GB__Overall == string.Empty || ppMod.currentoverallstats.GB__Overall.Equals("", StringComparison.OrdinalIgnoreCase)))
                    ws.Cells[8, 14].Value = Math.Round(Convert.ToDecimal(ppMod.currentoverallstats.GB__Overall) * 100, 1) + "%";

                if (!(ppMod.currentoverallstats.FB__Overall == string.Empty || ppMod.currentoverallstats.FB__Overall.Equals("", StringComparison.OrdinalIgnoreCase)))
                    ws.Cells[8, 15].Value = Math.Round(Convert.ToDecimal(ppMod.currentoverallstats.FB__Overall) * 100, 1) + "%";


                /*Current Stats VS LHH/RHH*/

                
                if (ppMod.playerinfo.Bats.Equals("L", StringComparison.OrdinalIgnoreCase))
                {
                    ws.Cells[10, 2].Value = "Current Stats VS RHH";

                    if (!(ppMod.currentoverallstatsvsrhh.AVG_vsRHH == string.Empty || ppMod.currentoverallstatsvsrhh.AVG_vsRHH.Equals("", StringComparison.OrdinalIgnoreCase)))
                        ws.Cells[12, 2].Value = Convert.ToDecimal(ppMod.currentoverallstatsvsrhh.AVG_vsRHH).ToString("#.000");

                    ws.Cells[12, 4].Value = ppMod.currentoverallstatsvsrhh.H_vsRHH;
                    ws.Cells[12, 5].Value = ppMod.currentoverallstatsvsrhh.HR_vsRHH;
                    ws.Cells[12, 6].Value = ppMod.currentoverallstatsvsrhh.BB_vsRHH;
                    ws.Cells[12, 7].Value = ppMod.currentoverallstatsvsrhh.SO_vsRHH;
                    ws.Cells[12, 8].Value = ppMod.currentoverallstatsvsrhh.PperPA_vsRHH;

                    
                    if (!(ppMod.currentoverallstatsvsrhh.GB__vsRHH == string.Empty || ppMod.currentoverallstatsvsrhh.GB__vsRHH.Equals("", StringComparison.OrdinalIgnoreCase)))
                        ws.Cells[12, 10].Value = Math.Round(Convert.ToDecimal(ppMod.currentoverallstatsvsrhh.GB__vsRHH) * 100, 1) + "%";
                    
                    if (!(ppMod.currentoverallstatsvsrhh.FB__vsRHH == string.Empty || ppMod.currentoverallstatsvsrhh.FB__vsRHH.Equals("", StringComparison.OrdinalIgnoreCase)))
                        ws.Cells[12, 12].Value = Math.Round(Convert.ToDecimal(ppMod.currentoverallstatsvsrhh.FB__vsRHH) * 100, 1) + "%";
                    
                    if (!(ppMod.currentoverallstatsvsrhh.SLG_vsRHH == string.Empty || ppMod.currentoverallstatsvsrhh.SLG_vsRHH.Equals("", StringComparison.OrdinalIgnoreCase)))
                        ws.Cells[12, 14].Value = Convert.ToDecimal(ppMod.currentoverallstatsvsrhh.SLG_vsRHH).ToString("#.000");

                    if (!(ppMod.currentoverallstatsvsrhh.OBP_vsRHH == string.Empty || ppMod.currentoverallstatsvsrhh.OBP_vsRHH.Equals("", StringComparison.OrdinalIgnoreCase)))
                        ws.Cells[12, 15].Value = Convert.ToDecimal(ppMod.currentoverallstatsvsrhh.OBP_vsRHH).ToString("#.000");
                }
                else
                {
                    if (!(ppMod.currentoverallstatsvslhh.AVG_vsLHH == string.Empty || ppMod.currentoverallstatsvslhh.AVG_vsLHH.Equals("", StringComparison.OrdinalIgnoreCase)))
                        ws.Cells[12, 2].Value = Convert.ToDecimal(ppMod.currentoverallstatsvslhh.AVG_vsLHH).ToString("#.000");

                    ws.Cells[12, 4].Value = ppMod.currentoverallstatsvslhh.H_vsLHH;
                    ws.Cells[12, 5].Value = ppMod.currentoverallstatsvslhh.HR_vsLHH;
                    ws.Cells[12, 6].Value = ppMod.currentoverallstatsvslhh.BB_vsLHH;
                    ws.Cells[12, 7].Value = ppMod.currentoverallstatsvslhh.SO_vsLHH;
                    ws.Cells[12, 8].Value = ppMod.currentoverallstatsvslhh.PperPA_vsLHH;

                    if (!(ppMod.currentoverallstatsvslhh.GB__vsLHH == string.Empty || ppMod.currentoverallstatsvslhh.GB__vsLHH.Equals("", StringComparison.OrdinalIgnoreCase)))
                        ws.Cells[12, 10].Value = Math.Round(Convert.ToDecimal(ppMod.currentoverallstatsvslhh.GB__vsLHH) * 100, 1) + "%";

                    if (!(ppMod.currentoverallstatsvslhh.FB__vsLHH == string.Empty || ppMod.currentoverallstatsvslhh.FB__vsLHH.Equals("", StringComparison.OrdinalIgnoreCase)))
                        ws.Cells[12, 12].Value = Math.Round(Convert.ToDecimal(ppMod.currentoverallstatsvslhh.FB__vsLHH) * 100, 1) + "%";

                    if (!(ppMod.currentoverallstatsvslhh.SLG_vsLHH == string.Empty || ppMod.currentoverallstatsvslhh.SLG_vsLHH.Equals("", StringComparison.OrdinalIgnoreCase)))
                        ws.Cells[12, 14].Value = Convert.ToDecimal(ppMod.currentoverallstatsvslhh.SLG_vsLHH).ToString("#.000");

                    if (!(ppMod.currentoverallstatsvslhh.OBP_vsLHH == string.Empty || ppMod.currentoverallstatsvslhh.OBP_vsLHH.Equals("", StringComparison.OrdinalIgnoreCase)))
                        ws.Cells[12, 15].Value = Convert.ToDecimal(ppMod.currentoverallstatsvslhh.OBP_vsLHH).ToString("#.000");
                }
                
                /*Approach / Outpitch*/

                try { ws.Cells[15, 2].Value = "Approach - " + ppMod.UserEntereds.Approach + "(OUT-PITCH - " + ppMod.UserEntereds.Out_Pitch + ")"; }
                catch { }


                try { ws.Cells[19, 2].Value = ppMod.UserEntereds.ReleaseTimes_1B; }
                catch { }
                try { ws.Cells[19, 3].Value = ppMod.UserEntereds.ReleaseTimes_2B; }
                catch { }
                try { ws.Cells[19, 4].Value = ppMod.UserEntereds.ReleaseTimes_SS; }
                catch { }

                try { ws.Cells[18, 5].Value = "Pickoff Move 1B:" + ppMod.UserEntereds.PickoffMove_1B; }
                catch { }
                try { ws.Cells[18, 10].Value = "Pickoff Move 2B:" + ppMod.UserEntereds.PickoffMove_2B; }
                catch { }


                try { ws.Cells[22, 4].Value = ppMod.UserEntered2.FB2SMVelocity; }
                catch { }
                try { ws.Cells[22, 6].Value = ppMod.UserEntered2.FB2SMCommand; }
                catch { }
                try { ws.Cells[22, 8].Value = ppMod.UserEntered2.FB2SMDescription; }
                catch { }

                try { ws.Cells[23, 4].Value = ppMod.UserEntered2.FB4SMVelocity; }
                catch { }
                try { ws.Cells[23, 6].Value = ppMod.UserEntered2.FB4SMCommand; }
                catch { }
                try { ws.Cells[23, 8].Value = ppMod.UserEntered2.FB4SMDescription; }
                catch { }

                try { ws.Cells[24, 4].Value = ppMod.PitcherNote.CutterVelocity; }
                catch { }
                try { ws.Cells[24, 6].Value = ppMod.UserEntered2.CutterCommand; }
                catch { }
                try { ws.Cells[24, 8].Value = ppMod.PitcherNote.CutterDescription; }
                catch { }

                try { ws.Cells[25, 4].Value = ppMod.PitcherNote.SliderVelocity; }
                catch { }
                try { ws.Cells[25, 6].Value = ppMod.UserEntered2.SliderCommand; }
                catch { }
                try { ws.Cells[25, 8].Value = ppMod.PitcherNote.SliderDescription; }
                catch { }

                try { ws.Cells[26, 4].Value = ppMod.PitcherNote.CurveballVelocity; }
                catch { }
                try { ws.Cells[26, 6].Value = ppMod.UserEntered2.CurveballCommand; }
                catch { }
                try { ws.Cells[26, 8].Value = ppMod.PitcherNote.CurveballDescription; }
                catch { }

                try { ws.Cells[27, 4].Value = ppMod.PitcherNote.ChangeupVelocity; }
                catch { }
                try { ws.Cells[27, 6].Value = ppMod.UserEntered2.ChangeupCommand; }
                catch { }
                try { ws.Cells[27, 8].Value = ppMod.PitcherNote.ChangeupDescription; }
                catch { }

                try { ws.Cells[28, 4].Value = ppMod.PitcherNote.SplitfingerVelocity; }
                catch { }
                try { ws.Cells[28, 6].Value = ppMod.UserEntered2.SplitfingerCommand; }
                catch { }
                try { ws.Cells[28, 8].Value = ppMod.PitcherNote.SplitfingerDescription; }
                catch { }

                try { ws.Cells[28, 4].Value = ppMod.PitcherNote.KnuckleballVelocity; }
                catch { }
                try { ws.Cells[28, 6].Value = ppMod.UserEntered2.KnuckleballCommand; }
                catch { }
                try { ws.Cells[28, 8].Value = ppMod.PitcherNote.KnuckleballDescription; }
                catch { }
                



                /*
                row = 18;
                ws.Cells[row, 1].Value = ppMod.playerinfo.Firstname + " " + ppMod.playerinfo.Lastname;
                ws.Cells[row, 1, row, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[row, 1, row, 5].Style.Fill.BackgroundColor.SetColor(Color.LightGoldenrodYellow);
                */
                /*asdasd*/
                Response.BinaryWrite(pck.GetAsByteArray());
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                //Response.ContentType = "application/ms-excel";
                Response.AddHeader("content-disposition", "attachment;  filename=PitcherProfile_" + _pid + ".xlsx");
                MyFileStream.Close();
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Error: " + e);
            }

           

            return null;
        }
        #endregion

        #region "EXPORT RUNNER CHART"
        public ActionResult ExportRunnerChart(int? _teamid)
        {
            try
            {
                rcMod.ListRunnerChart = crEntity.cr_sproc_runnerchart(2013, _teamid, 1).ToList();
                rcMod.rcteam = (from l in ieEntity.Team_tables
                                where l.Team_ID == _teamid
                                select l).FirstOrDefault();

                string path = Server.MapPath("~/App_Data/RunnerChart.xlsx");
                FileStream MyFileStream = new FileStream(path, FileMode.Open);


                ExcelPackage pck = new ExcelPackage(new MemoryStream(), MyFileStream);


                var ws = pck.Workbook.Worksheets[1];

                ws.Cells[5, 3].Value = rcMod.rcteam.City_or_State + " " + rcMod.rcteam.Nickname;
                if (rcMod.ListRunnerChart != null && rcMod.ListRunnerChart != null)
                {
                    int row = 10;
                    foreach (var item in rcMod.ListRunnerChart)
                    {
                        ws.Cells[row, 2].Value = item.Name;
                        ws.Cells[row, 5].Value = item.ReleaseTimes_1B;
                        ws.Cells[row, 6].Value = item.SlideStepTime_1B;
                        ws.Cells[row, 7].Value = item.PickoffMove_1B;
                        ws.Cells[row, 9].Value = item.ReleaseTimes_2B;
                        ws.Cells[row, 10].Value = item.PickoffMove_2B;
                        ws.Cells[row, 12].Value = item.LooksWRunner_2B;
                        ++row;
                    }
                }


                Response.BinaryWrite(pck.GetAsByteArray());
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                //Response.ContentType = "application/ms-excel";
                Response.AddHeader("content-disposition", "attachment;  filename=RunnerChart_" + _teamid + ".xlsx");
                MyFileStream.Close();

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }

            return null;
        }
        #endregion

        #region "EXPORT PITCHER SPLITS"
        public ActionResult ExportPitcherSplits(int? _teamid)
        {
            try
            {
                if (_teamid != 0)
                {
                    psMod.ListPitcherSplits = crEntity.cr_sproc_pitchersplits(2013, _teamid, 1).ToList();
                    psMod.team = (from l in ieEntity.Team_tables
                                  where l.Team_ID == _teamid
                                  select l).FirstOrDefault();
                }
                else
                    psMod = null;

                string path = Server.MapPath("~/App_Data/PitcherSplits.xlsx");
                FileStream MyFileStream = new FileStream(path, FileMode.Open);


                ExcelPackage pck = new ExcelPackage(new MemoryStream(), MyFileStream);


                var ws = pck.Workbook.Worksheets[1];

                ws.Cells[5, 1].Value = psMod.team.City_or_State.ToUpper() + " " + psMod.team.Nickname.ToUpper();

                
                if (psMod.ListPitcherSplits != null && psMod.ListPitcherSplits != null)
                {

                    int row = 10;
                    foreach (var item in psMod.ListPitcherSplits)
                    {
                        ++row;
                        ws.InsertRow(row, 1, 10);
                        ws.Cells["A" + row + ":C" + row].Merge = true;
                        ws.Cells["D" + row + ":E" + row].Merge = true;
                        ws.Cells["F" + row + ":G" + row].Merge = true;
                        ws.Cells["H" + row + ":I" + row].Merge = true;
                        ws.Cells["J" + row + ":K" + row].Merge = true;
                    }

                    
                    row = 10;
                    foreach (var item in psMod.ListPitcherSplits)
                    {
                        ws.Cells[row, 1].Value = item.Name;

                        ws.Cells[row, 4].Value = Convert.ToDecimal(item.RHH);

                        try 
                        {
                            string value = item.Career_RHB;

                            if (value == null)
                                ws.Cells[row, 6].Value = "";
                            else
                                ws.Cells[row, 6].Value = Convert.ToDecimal(value);
                        }
                        catch { };

                        ws.Cells[row, 8].Value = Convert.ToDecimal(item.LHH);

                        try
                        {
                            string value = item.Career_LHB;

                            if (value == null)
                                ws.Cells[row, 10].Value = "";
                            else
                                ws.Cells[row, 10].Value = Convert.ToDecimal(value);

                        }
                        catch { };

                       
                        
                        if (Convert.ToDecimal(item.RHH) > Convert.ToDecimal(item.LHH))
                        {
                            ws.Cells[row, 4, row, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[row, 4, row, 5].Style.Fill.BackgroundColor.SetColor(Color.Red);

                            ws.Cells[row, 6, row, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[row, 6, row, 7].Style.Fill.BackgroundColor.SetColor(Color.Red);

                            ws.Cells[row, 8, row, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[row, 8, row, 9].Style.Fill.BackgroundColor.SetColor(Color.Blue);

                            ws.Cells[row, 10, row, 11].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[row, 10, row, 11].Style.Fill.BackgroundColor.SetColor(Color.Blue);
                        }
                        
                        else if (Convert.ToDecimal(item.RHH) < Convert.ToDecimal(item.LHH))
                        {
                            ws.Cells[row, 4, row, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[row, 4, row, 5].Style.Fill.BackgroundColor.SetColor(Color.Blue);

                            ws.Cells[row, 6, row, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[row, 6, row, 7].Style.Fill.BackgroundColor.SetColor(Color.Blue);

                            ws.Cells[row, 8, row, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[row, 8, row, 9].Style.Fill.BackgroundColor.SetColor(Color.Red);

                            ws.Cells[row, 10, row, 11].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[row, 10, row, 11].Style.Fill.BackgroundColor.SetColor(Color.Red);
                        }
                        ++row;
                    }
                    ws.Cells[row, 1].Style.Font.Bold = true;
                    ws.Cells[row, 1].Style.Font.Size = 14;
                    ws.Cells[row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[row, 1].Value = "Team Totals";


                    ws.Cells[row, 4].Style.Font.Bold = true;
                    ws.Cells[row, 4].Style.Font.Size = 14;
                    ws.Cells[row, 4].Formula = "AVERAGE(D10:E" + (row - 1) + ")";

                    ws.Cells[row, 6].Style.Font.Bold = true;
                    ws.Cells[row, 6].Style.Font.Size = 14;
                    ws.Cells[row, 6].Formula = "AVERAGE(F10:G" + (row - 1) + ")";

                    ws.Cells[row, 8].Style.Font.Bold = true;
                    ws.Cells[row, 8].Style.Font.Size = 14;
                    ws.Cells[row, 8].Formula = "AVERAGE(H10:I" + (row - 1) + ")";

                    ws.Cells[row, 10].Style.Font.Bold = true;
                    ws.Cells[row, 10].Style.Font.Size = 14;
                    ws.Cells[row, 10].Formula = "AVERAGE(J10:K" + (row - 1) + ")";
                }
                

                

                Response.BinaryWrite(pck.GetAsByteArray());
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                //Response.ContentType = "application/ms-excel";
                Response.AddHeader("content-disposition", "attachment;  filename=PitcherSplits_" + _teamid + ".xlsx");
                MyFileStream.Close();

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }

            return null;
        }
        #endregion
    }
}
