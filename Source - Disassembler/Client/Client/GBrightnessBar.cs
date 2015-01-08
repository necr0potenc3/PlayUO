namespace Client
{
    using System;
    using System.Windows.Forms;

    public class GBrightnessBar : Gump
    {
        private int[] m_ChunkHeights;
        private int m_Height;
        private int m_Position;
        private GHuePicker m_Target;
        private int m_Width;

        public GBrightnessBar(int X, int Y, int Width, int Height, GHuePicker Target) : base(X, Y)
        {
            this.m_Width = Width;
            this.m_Height = Height;
            this.m_Target = Target;
            this.FillHeights();
        }

        protected internal override void Draw(int X, int Y)
        {
            Renderer.SetTexture(null);
            int num = 0;
            for (int i = 0; i < 4; i++)
            {
                Renderer.GradientRect(this.m_Target.Color(i), this.m_Target.Color(i + 1), X, Y + num, this.m_Width, this.m_ChunkHeights[i]);
                num += this.m_ChunkHeights[i];
            }
            Renderer.AlphaTestEnable = false;
            Renderer.SetAlphaEnable(true);
            Renderer.SetAlpha(1f);
            Renderer.ColorAlphaEnable = true;
            Engine.m_Slider.Draw(X, (Y + this.m_Position) - 1, 0);
            Renderer.ColorAlphaEnable = false;
            Renderer.SetAlphaEnable(false);
            Renderer.AlphaTestEnable = true;
        }

        private void FillHeights()
        {
            this.m_ChunkHeights = new int[4];
            for (int i = 0; i < 4; i++)
            {
                this.m_ChunkHeights[i] = this.m_Height / 4;
            }
            for (int j = 0; j < (this.m_Height % 4); j++)
            {
                this.m_ChunkHeights[j]++;
            }
        }

        protected internal override bool HitTest(int X, int Y)
        {
            return true;
        }

        protected internal override void OnMouseDown(int X, int Y, MouseButtons mb)
        {
            if (mb != MouseButtons.None)
            {
                this.Slide(Y);
            }
        }

        protected internal override void OnMouseEnter(int X, int Y, MouseButtons mb)
        {
            if (mb != MouseButtons.None)
            {
                this.Slide(Y);
            }
        }

        protected internal override void OnMouseMove(int X, int Y, MouseButtons mb)
        {
            if (mb != MouseButtons.None)
            {
                this.Slide(Y);
            }
        }

        protected internal override void OnMouseUp(int X, int Y, MouseButtons mb)
        {
            if (mb != MouseButtons.None)
            {
                this.Slide(Y);
            }
            Gumps.Capture = null;
        }

        protected internal override void OnMouseWheel(int Delta)
        {
            if ((Delta > 0) && (this.m_Target.Brightness > 0))
            {
                this.m_Target.Brightness--;
            }
            else if ((Delta < 0) && (this.m_Target.Brightness < 4))
            {
                this.m_Target.Brightness++;
            }
            this.m_Position = (int) ((((double) this.m_Target.Brightness) / 4.0) * (this.m_Height - 1));
        }

        public void Refresh()
        {
            this.m_Position = (int) ((((double) this.m_Target.Brightness) / 4.0) * (this.m_Height - 1));
        }

        public void Slide(int Y)
        {
            Gumps.Capture = this;
            this.m_Position = Y;
            if (this.m_Position < 0)
            {
                this.m_Position = 0;
            }
            else if (this.m_Position >= this.m_Height)
            {
                this.m_Position = this.m_Height - 1;
            }
            int num = (int) (((((double) this.m_Position) / ((double) (this.m_Height - 1))) - 1E-13) * 5.0);
            if (this.m_Target.Brightness != num)
            {
                this.m_Target.Brightness = num;
            }
        }

        public override int Height
        {
            get
            {
                return this.m_Height;
            }
            set
            {
                this.m_Height = value;
                this.FillHeights();
            }
        }

        public override int Width
        {
            get
            {
                return this.m_Width;
            }
            set
            {
                this.m_Width = value;
            }
        }
    }
}

