namespace Client
{
    using System;
    using System.Windows.Forms;

    public class GVolumeSlider : GSliderBase
    {
        private int m_Offset;
        private bool m_Sound;
        private State m_State;
        private Texture m_Texture;

        public GVolumeSlider(bool sound, Texture texture, int x, int y) : base(x, y)
        {
            this.m_State = State.Inactive;
            this.m_Sound = sound;
            this.m_Texture = texture;
            base.LargeOffset = 5;
            base.WheelOffset = 5;
            base.SmallOffset = 1;
            base.Minimum = 0;
            base.Maximum = 100;
            base.Value = sound ? VolumeControl.Sound : VolumeControl.Music;
            this.Width = 100;
            this.Height = 15;
        }

        protected internal override void Draw(int X, int Y)
        {
            int position = base.GetPosition(this.Width);
            if (this.m_State == State.LargeScrollUp)
            {
                if (position > 0)
                {
                    int x = base.PointToClient(new Point(Engine.m_xMouse, Engine.m_yMouse)).X;
                    if (position > x)
                    {
                        base.Value -= base.LargeOffset;
                    }
                    else
                    {
                        this.m_State = State.Inactive;
                    }
                }
            }
            else if ((this.m_State == State.LargeScrollDown) && (((this.Width - position) - this.m_Texture.Width) > 0))
            {
                base.Value += base.LargeOffset;
                int num3 = base.PointToClient(new Point(Engine.m_xMouse, Engine.m_yMouse)).X;
                if ((position + this.m_Texture.Width) < num3)
                {
                    base.Value += base.LargeOffset;
                }
                else
                {
                    this.m_State = State.Inactive;
                }
            }
            Renderer.AlphaTestEnable = false;
            Renderer.SetAlphaEnable(true);
            Renderer.SetAlpha(1f);
            this.m_Texture.Draw((X + position) - (this.m_Texture.Width / 2), Y + ((this.Height - this.m_Texture.Height) / 2));
            Renderer.SetAlphaEnable(false);
            Renderer.AlphaTestEnable = true;
        }

        protected internal override bool HitTest(int X, int Y)
        {
            return true;
        }

        protected override void OnChanged(int oldValue)
        {
            if (this.m_Sound)
            {
                VolumeControl.Sound = base.Value;
            }
            else
            {
                VolumeControl.Music = base.Value;
            }
        }

        protected internal override void OnMouseDown(int X, int Y, MouseButtons mb)
        {
            if (mb == MouseButtons.Left)
            {
                int width = this.m_Texture.Width;
                int position = base.GetPosition(this.Width);
                this.m_State = State.Normal;
                this.m_Offset = X - position;
                base.Value = base.GetValue(X, this.Width);
                Gumps.Capture = this;
            }
        }

        protected internal override void OnMouseMove(int X, int Y, MouseButtons mb)
        {
            if ((Gumps.Capture == this) && (this.m_State == State.Normal))
            {
                base.Value = base.GetValue(X, this.Width);
            }
        }

        protected internal override void OnMouseUp(int X, int Y, MouseButtons mb)
        {
            if ((Gumps.Capture == this) && (this.m_State == State.Normal))
            {
                base.Value = base.GetValue(X, this.Width);
            }
            else if (mb == MouseButtons.Right)
            {
                Gumps.Destroy(base.Parent);
            }
            this.m_State = State.Inactive;
            Gumps.Capture = null;
        }

        private enum State
        {
            Normal,
            SmallScrollUp,
            SmallScrollDown,
            LargeScrollUp,
            LargeScrollDown,
            Inactive
        }
    }
}

