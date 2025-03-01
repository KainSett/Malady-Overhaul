using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using MonoMod.Utils;

namespace MaladyOverhaul
{
    public class MaladyOverhaul : Mod
    {
        public static Dictionary<string, (Asset<Texture2D>, Asset<Texture2D>)> Icons = [];
        public override void Load()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                Icons.Add("Fire", (null, null));
                Icons.Add("Poison", (null, null));
                Icons.Add("Venom", (null, null));
                Icons.Add("Frostbite", (null, null));
                Icons.Add("Chilled", (null, null));

                foreach (var t in Icons)
                {
                    Icons[t.Key] = (Assets.Request<Texture2D>($"Assets/{t.Key}"), Assets.Request<Texture2D>($"Assets/{t.Key}White"));
                }
            }
        }
        public override void Unload()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                Icons.Clear();
            }
        }
    }
}
