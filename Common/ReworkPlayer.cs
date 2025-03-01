using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MaladyOverhaul.Common
{
    public class ReworkPlayer : ModPlayer
    {
        public class ReworkDebuff : GlobalBuff
        {
            public override void Update(int type, Player player, ref int buffIndex)
            {
                ref var list = ref player.GetModPlayer<ReworkPlayer>().individualList;

                switch(type)
                {
                    case BuffID.OnFire:
                        list.Add((player.Center, "Fire", player.buffTime[player.FindBuffIndex(type)] % 25, player.whoAmI, 4));
                        break;
                    case BuffID.Poisoned:
                        list.Add((player.Center, "Poison", player.buffTime[player.FindBuffIndex(type)] % 30, player.whoAmI, 5));
                        break;
                    case BuffID.Venom:
                        list.Add((player.Center, "Venom", player.buffTime[player.FindBuffIndex(type)] % 15, player.whoAmI, 2));
                        break;
                    case BuffID.Frostburn2:
                        list.Add((player.Center, "Frostbite", player.buffTime[player.FindBuffIndex(type)] % 50, player.whoAmI, 5));
                        break;
                    case BuffID.Chilled:
                        list.Add((player.Center, "Chilled", player.buffTime[player.FindBuffIndex(type)] % 40, player.whoAmI, 3));
                        break;
                }
            }
        }
        public List<(Vector2, string, int, int, int)> individualList = [];
        public override void PreUpdateBuffs()
        {
            ref var list = ref ReworkBase.MaladyUIBase.DrawMaladyUIList;

            list.RemoveAll(t => t.Item4 == Player.whoAmI);

            for (int i = individualList.Count - 1; i > 0; i--)
            {
                Vector2 offset = new(300 / (individualList.Count + 1) - 150, 0);
                var t = individualList[i];
                individualList[i] = (t.Item1 + offset, t.Item2, t.Item3, t.Item4, t.Item5);
            }

            list.AddRange(individualList);
            individualList.Clear();
        }
    }
}