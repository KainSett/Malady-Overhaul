using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

                foreach (var data in DrawMaladyUIList)
                {
                    var texture = MaladyOverhaul.Icons[data.Item2].Item1.Value;
                    spriteBatch.Draw(texture, data.Item1, null, Color.White, 0, texture.Size() / 2, Main.GameZoomTarget / Main.UIScale, SpriteEffects.None, 0);
                    Utils.DrawBorderString(spriteBatch, data.Item5.ToString(), data.Item1 + texture.Size() * 0.6f, Color.White);

                    if (data.Item3 == 0)
                        continue;

                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);

                    texture = MaladyOverhaul.Icons[data.Item2].Item2.Value;

                    var offset = new Vector2(0, -data.Item3);

                    var color = Color.White;
                    color.A -= (byte)(data.Item3  * 10);

                    spriteBatch.Draw(texture, data.Item1 + offset, null, color, 0, texture.Size() / 2, Main.GameZoomTarget / Main.UIScale, SpriteEffects.None, 0);

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
                int index = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
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
