using BepInEx;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using R2API;

namespace TornadoSlamFix
{
    [BepInDependency(ItemAPI.PluginGUID)]
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class TornadoSlamFix : BaseUnityPlugin
    {
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "Laminarium";
        public const string PluginName = "TornadoSlamFix";
        public const string PluginVersion = "1.0.0";

        public void Awake()
        {
            Log.Init(Logger);
            Log.Info("Logger initialized");

            IL.EntityStates.Drifter.TornadoSlam.OnEnter += FixTornadoSlam;
            Log.Info("Tornado Slam fixed");
        }

        private static void FixTornadoSlam(ILContext il)
        {
            ILCursor c = new(il);

            if (c.TryGotoNext(
                    x => x.MatchLdfld("EntityStates.Drifter.TornadoSlam", "baseFireFrequency"),
                    x => x.MatchLdarg(0),
                    x => x.MatchLdfld("EntityStates.BaseState", "attackSpeedStat"),
                    x => x.MatchDiv(),
                    x => x.MatchStfld("EntityStates.Drifter.TornadoSlam", "fireFrequency")
                )) {
                c.Index += 3;
                c.Next.OpCode = OpCodes.Mul;
                Log.Info("IL worked - fireFrequency");
            } else {
                Log.Error("IL failed - fireFrequency");
            }

            if (c.TryGotoNext(
                    x => x.MatchLdfld("EntityStates.Drifter.TornadoSlam", "baseBounceDuration"),
                    x => x.MatchLdarg(0),
                    x => x.MatchLdfld("EntityStates.BaseState", "attackSpeedStat"),
                    x => x.MatchDiv(),
                    x => x.MatchStfld("EntityStates.Drifter.TornadoSlam", "bounceDuration")
                )) {
                c.Index += 3;
                c.Next.OpCode = OpCodes.Mul;
                Log.Info("IL worked - bounceDuration");
            } else {
                Log.Error("IL failed - bounceDuration");
            }
        }
    }
}
