using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WPR.MonoGameCompability.Graphics
{
    public class SpriteBatch2 : SpriteBatch
    {
        public SpriteBatch2(GraphicsDevice device)
            : base(device)
        {

        }

        public SpriteBatch2(GraphicsDevice device, int capacity) 
            : base(device, capacity)
        {

        }
        public void Begin(SpriteSortMode sortMode, BlendState blendState)
        {
            base.Begin(sortMode: sortMode, blendState: blendState, transformMatrix: Matrix.Identity);
        }
		public void Begin()
        {
            base.Begin(sortMode: SpriteSortMode.Deferred, transformMatrix: Matrix.Identity);
        }
    }
}