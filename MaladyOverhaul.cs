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
        public static Dictionary<string, (int, int)> Debuffs = [];
        public static string[] names = ["Fire", "Poison", "Venom", "Frostbite", "Chilled"];
        public override void Load()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                foreach (var n in names)
                {
                    Icons[n] = (Assets.Request<Texture2D>($"Assets/{n}Icon"), Assets.Request<Texture2D>($"Assets/{n}IconWhite"));
                }

                Debuffs["Fire"] = (4, 25);
                Debuffs["Venom"] = (2, 10);
                Debuffs["Poison"] = (5, 30);
                Debuffs["Frostbite"] = (5, 50);
                Debuffs["Chilled"] = (3, 40);

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
