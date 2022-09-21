using Microsoft.Xna.Framework.Graphics;

namespace WPR.XnaCompability.Graphics
{
    public class GraphicsDevice2 : GraphicsDevice
    {
        public GraphicsDevice2(GraphicsAdapter adapter, GraphicsProfile graphicsProfile, PresentationParameters presentationParameters) 
            : base(adapter, graphicsProfile, presentationParameters)
        {
        }

        public new DisplayMode DisplayMode => new DisplayMode(480, 800, base.DisplayMode.Format);
    }
}
