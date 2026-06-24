using Celeste.Mod.Aqua.Miscellaneous;
using Microsoft.Xna.Framework;

namespace Celeste.Mod.Aqua.Core
{
    public static class FlyFeatherExtensions
    {
        public static void Initialize()
        {
            On.Celeste.FlyFeather.Respawn += FlyFeather_Respawn;
            On.Celeste.FlyFeather.Update += FlyFeather_Update;
        }

        public static void Uninitialize()
        {
            On.Celeste.FlyFeather.Respawn -= FlyFeather_Respawn;
            On.Celeste.FlyFeather.Update -= FlyFeather_Update;
        }

        private static void FlyFeather_Respawn(On.Celeste.FlyFeather.orig_Respawn orig, FlyFeather self)
        {
            if (DataContainer.For(self).Has("respawn_position"))
            {
                self.Position = DataContainer.For(self).Get<Vector2>("respawn_position");
                DataContainer.For(self).Remove("respawn_position");
            }
            orig(self);
        }

        private static void FlyFeather_Update(On.Celeste.FlyFeather.orig_Update orig, FlyFeather self)
        {
            orig(self);
            if (self.IsHookable() && DataContainer.For(self).Has("respawn_position"))
            {
                self.outline.Position = DataContainer.For(self).Get<Vector2>("respawn_position") - self.Position;
            }
        }
    }
}
