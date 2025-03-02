using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace MaladyOverhaul.Common.ReworkBase
{
    public class MaladyUIBase
    {
        public static List<(Vector2, string, int, int, int)> DrawMaladyUIList = [];
        
        public class MaladyUIElement : UIElement
        {
            public override void Draw(SpriteBatch spriteBatch)
            {
                base.Draw(spriteBatch);
                if (Main.dedServ)
                    return;

                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                foreach (var data in DrawMaladyUIList)
                {
                    var texture = MaladyOverhaul.Icons[data.Item2].Item1.Value;
                    var origin = texture.Size() / 2;
                    spriteBatch.Draw(texture, data.Item1 - Main.screenPosition, null, Color.White, 0, origin, Main.GameZoomTarget / Main.UIScale, SpriteEffects.None, 0);
                    Utils.DrawBorderString(spriteBatch, data.Item5.ToString(), data.Item1 - Main.screenPosition + texture.Size() * 0.1f, Color.White);

                    if (data.Item3 == 0)
                        continue;

                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);

                    texture = MaladyOverhaul.Icons[data.Item2].Item2.Value;

                    var offset = new Vector2(0, data.Item3);

                    var color = Color.White;
                    color.A -= (byte)Math.Min(255, data.Item3 * 19);
                    origin = texture.Size() / 2;

                    spriteBatch.Draw(texture, data.Item1 - Main.screenPosition - offset, null, color, 0, origin, Main.GameZoomTarget / Main.UIScale, SpriteEffects.None, 0);

                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                }
            }
        }
        public class MaladyUIState : UIState
        {
            public MaladyUIElement element;
            public override void OnInitialize()
            {
                base.OnInitialize();
                element = new MaladyUIElement();
                element.IgnoresMouseInteraction = true;
                //element.Height.Set(26, 0);
                //element.Width.Set(22, 0);
                Append(element);
            }
        }
        [Autoload(Side = ModSide.Client)]
        public class MaladyUISystem : ModSystem
        {
            internal MaladyUIState state;
            private UserInterface Interface;
            public void Show(UserInterface Interface)
            {
                Interface?.SetState(state);
            }
            public void Hide(UserInterface Interface)
            {
                Interface?.SetState(null);
            }
            public override void Load()
            {
                base.Load();
                state = new MaladyUIState();
                Interface = new UserInterface();
                state.Activate();
                Show(Interface);
            }
            public override void UpdateUI(GameTime gameTime)
            {
                base.UpdateUI(gameTime);
                Interface?.Update(gameTime);
            }
            public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
            {
                base.ModifyInterfaceLayers(layers);
                int index = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Entity Health Bars"));
                if (index == -1)
                    return;

                layers.Insert(index, new LegacyGameInterfaceLayer("MaladyOverhaul: Malady UI", delegate
                {
                    Interface.Draw(Main.spriteBatch, new GameTime());
                    return true;
                }, InterfaceScaleType.UI));
            }
        }
    }
}