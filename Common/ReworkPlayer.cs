using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MaladyOverhaul.Common
{
    public class ReworkPlayer : ModPlayer
    {
        public class ReworkDebuff : GlobalBuff
        {
            public int amount = 0;
            public override void Update(int type, Player player, ref int buffIndex)
            {
                player.lifeRegenCount = 0;
                var list = player.GetModPlayer<ReworkPlayer>().individualList;
                var debuffs = MaladyOverhaul.Debuffs;

                string n = GetMaladyName(type);

                if (n != "")
                {
                    player.GetModPlayer<ReworkPlayer>().stacks[n] = amount;
                    var time = debuffs[n].Item2 - (player.buffTime[buffIndex] % debuffs[n].Item2);
                    if (time == 0)
                    {
                        player.statLife = Math.Max(player.statLife - amount, 1);
                    }

                    list.Add((player.Center, n, time, player.whoAmI, debuffs[n].Item1));
                }
            }
            public override bool ReApply(int type, Player player, int time, int buffIndex)
            {
                var stacks = player.GetModPlayer<ReworkPlayer>().stacks;

                string n = GetMaladyName(type);
                if (n != "")
                    stacks[n] = Math.Clamp(stacks[n] + 1, 0, MaladyOverhaul.Debuffs[n].Item1);

                return base.ReApply(type, player, time, buffIndex);
            }
            public static string GetMaladyName(int type)
            {
                string n = "Fire";
                switch (type)
                {
                    case BuffID.OnFire:
                        n = "Fire";
                        break;
                    case BuffID.Poisoned:
                        n = "Poison";
                        break;
                    case BuffID.Venom:
                        n = "Venom";
                        break;
                    case BuffID.Frostburn:
                        n = "Frostbite";
                        break;
                    case BuffID.Frostburn2:
                        n = "Frostbite";
                        break;
                    case BuffID.Chilled:
                        n = "Chilled";
                        break;
                    default:
                        n = "";
                        break;
                }
                return n;
            }
        }
        public Dictionary<string, int> stacks = [];
        public List<(Vector2, string, int, int, int)> individualList = [];
        public override void PreUpdateBuffs()
        {
            stacks.Clear();
            foreach (var t in MaladyOverhaul.Debuffs)
            {
                stacks.Add(t.Key, 0);
            }

            ref var list = ref ReworkBase.MaladyUIBase.DrawMaladyUIList;

            list.RemoveAll(t => t.Item4 == Player.whoAmI);

            for (int i = individualList.Count - 1; i > -1; i--)
            {
                Vector2 offset = new(50 * i - (individualList.Count - 1) * 25, -60);
                var t = individualList[i];
                individualList[i] = (t.Item1 + offset, t.Item2, t.Item3, t.Item4, t.Item5);
            }

            list.AddRange(individualList);
            individualList.Clear();
        }
    }
}