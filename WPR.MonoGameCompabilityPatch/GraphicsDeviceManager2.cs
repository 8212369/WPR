using Microsoft.Xna.Framework;

namespace WPR.XnaCompability
{
    public class GraphicsDeviceManager2 : GraphicsDeviceManager
    {
        public GraphicsDeviceManager2(Game game)
            : base(game)
        {

        }

        public new bool IsFullScreen {
            get => false;
            set => base.IsFullScreen = false;
        }
    }
}
