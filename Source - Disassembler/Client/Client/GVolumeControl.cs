namespace Client
{
    using System.Drawing;
    using System.Windows.Forms;

    public class GVolumeControl : Gump
    {
        private Texture m_Background;
        private Texture m_Slider;

        public GVolumeControl() : base(0, 0)
        {
            this.m_Background = new Texture(new Bitmap("Data/Images/volume.png"));
            this.m_Slider = new Texture(new Bitmap("Data/Images/volume_slider.png"));
            base.m_Children.Add(new GVolumeSlider(true, this.m_Slider, 13, 0x30));
            base.m_Children.Add(new GVolumeSlider(false, this.m_Slider, 13, 0x5e));
            base.GUID = "Volume";
        }

        protected internal override void Draw(int X, int Y)
        {
            Renderer.AlphaTestEnable = false;
            Renderer.SetAlphaEnable(true);
            Renderer.SetAlpha(1f);
            this.m_Background.Draw(X, Y);
            Renderer.SetAlphaEnable(false);
            Renderer.AlphaTestEnable = true;
        }

        protected internal override bool HitTest(int X, int Y)
        {
            return true;
        }

        protected internal override void OnDispose()
        {
            base.OnDispose();
            this.m_Background.Dispose();
            this.m_Slider.Dispose();
        }

        protected internal override void OnMouseUp(int X, int Y, MouseButtons mb)
        {
            if (mb == MouseButtons.Right)
            {
                Gumps.Destroy(this);
            }
        }

        protected internal override void Render(int X, int Y)
        {
            this.X = (Engine.GameX + Engine.GameWidth) - this.Width;
            this.Y = Engine.GameY;
            base.Render(X, Y);
        }

        public override int Height
        {
            get
            {
                return this.m_Background.Height;
            }
            set
            {
            }
        }

        public override int Width
        {
            get
            {
                return this.m_Background.Width;
            }
            set
            {
            }
        }
    }
}