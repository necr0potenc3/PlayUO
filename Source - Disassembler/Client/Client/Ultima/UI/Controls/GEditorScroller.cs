namespace Client
{
    using System.Windows.Forms;

    public class GEditorScroller : GSliderBase
    {
        private int m_Offset;
        private GEditorPanel m_Panel;
        private Texture m_ScrollTexture;
        private State m_State;

        public GEditorScroller(GEditorPanel panel) : base(0, 0)
        {
            this.m_State = State.Inactive;
            this.m_Panel = panel;
            base.LargeOffset = 0x15;
            base.WheelOffset = 0x15;
            base.SmallOffset = 7;
        }

        protected internal override unsafe void Draw(int X, int Y)
        {
            if (this.m_ScrollTexture == null)
            {
                this.m_ScrollTexture = new Texture(0x10, 0x10, true);
                LockData data = this.m_ScrollTexture.Lock(LockFlags.WriteOnly);
                ushort num = Engine.C32216(GumpColors.ControlLightLight);
                ushort num2 = Engine.C32216(GumpColors.ScrollBar);
                for (int i = 0; i < 0x10; i++)
                {
                    ushort* numPtr = (ushort*)((int)data.pvSrc + (i * data.Pitch));
                    for (int j = 0; j < 0x10; j++)
                    {
                        if ((((i & 1) + j) & 1) == 0)
                        {
                            numPtr++;
                            numPtr[0] = num;
                        }
                        else
                        {
                            numPtr++;
                            numPtr[0] = num2;
                        }
                    }
                }
                this.m_ScrollTexture.Unlock();
            }
            this.m_ScrollTexture.Draw(X, Y, this.Width, this.Height);
            int barHeight = this.GetBarHeight();
            int num6 = Y + 0x10;
            int num7 = this.Height - 0x20;
            int position = base.GetPosition(num7 - barHeight);
            Renderer.SetTexture(null);
            if (this.m_State == State.LargeScrollUp)
            {
                if (position > 0)
                {
                    Renderer.SetAlphaEnable(true);
                    Renderer.SetAlpha(0.9f);
                    Renderer.SolidRect(GumpColors.ControlDarkDark, X, Y + this.Width, this.Width, position);
                    Renderer.SetAlphaEnable(false);
                    int num9 = base.PointToClient(new Point(Engine.m_xMouse, Engine.m_yMouse)).Y - 0x10;
                    if (position > num9)
                    {
                        base.Value -= base.LargeOffset;
                    }
                    else
                    {
                        this.m_State = State.Inactive;
                    }
                }
            }
            else if ((this.m_State == State.LargeScrollDown) && (((num7 - position) - barHeight) > 0))
            {
                Renderer.SetAlphaEnable(true);
                Renderer.SetAlpha(0.9f);
                Renderer.SolidRect(GumpColors.ControlDarkDark, X, (num6 + position) + barHeight, this.Width, (num7 - position) - barHeight);
                Renderer.SetAlphaEnable(false);
                int num10 = base.PointToClient(new Point(Engine.m_xMouse, Engine.m_yMouse)).Y - 0x10;
                if ((position + barHeight) < num10)
                {
                    base.Value += base.LargeOffset;
                }
                else
                {
                    this.m_State = State.Inactive;
                }
            }
            GumpPaint.DrawRaised3D(X, num6 + position, 0x10, barHeight);
            if (this.m_State == State.SmallScrollUp)
            {
                GumpPaint.DrawFlat(X, Y, this.Width, this.Width);
                Renderer.SetAlphaEnable(true);
                Renderer.SetAlpha(1f);
                Engine.m_WinScrolls[0].Draw(X + 5, Y + 7, GumpColors.ControlText);
                Renderer.SetAlphaEnable(false);
                base.Value -= base.SmallOffset;
            }
            else
            {
                GumpPaint.DrawRaised3D(X, Y, this.Width, this.Width);
                Renderer.SetAlphaEnable(true);
                Renderer.SetAlpha(1f);
                Engine.m_WinScrolls[0].Draw(X + 4, Y + 6, GumpColors.ControlText);
                Renderer.SetAlphaEnable(false);
            }
            Renderer.SetTexture(null);
            if (this.m_State == State.SmallScrollDown)
            {
                GumpPaint.DrawFlat(X, (Y + this.Height) - this.Width, this.Width, this.Width);
                Renderer.SetAlphaEnable(true);
                Renderer.SetAlpha(1f);
                Engine.m_WinScrolls[1].Draw(X + 5, ((Y + this.Height) - this.Width) + 7, GumpColors.ControlText);
                Renderer.SetAlphaEnable(false);
                base.Value += base.SmallOffset;
            }
            else
            {
                GumpPaint.DrawRaised3D(X, (Y + this.Height) - this.Width, this.Width, this.Width);
                Renderer.SetAlphaEnable(true);
                Renderer.SetAlpha(1f);
                Engine.m_WinScrolls[1].Draw(X + 4, ((Y + this.Height) - this.Width) + 6, GumpColors.ControlText);
                Renderer.SetAlphaEnable(false);
            }
        }

        private int GetBarHeight()
        {
            int num = this.Height - 0x20;
            int num2 = (num * base.LargeOffset) / ((base.Maximum - base.Minimum) + 1);
            if (num2 > num)
            {
                num2 = num;
            }
            if (num2 < 8)
            {
                num2 = 8;
            }
            return num2;
        }

        protected internal override bool HitTest(int X, int Y)
        {
            return true;
        }

        protected override void OnChanged(int oldValue)
        {
            this.m_Panel.Layout();
        }

        protected internal override void OnDispose()
        {
            if (this.m_ScrollTexture != null)
            {
                this.m_ScrollTexture.Dispose();
            }
            this.m_ScrollTexture = null;
            base.OnDispose();
        }

        protected internal override void OnMouseDown(int X, int Y, MouseButtons mb)
        {
            int barHeight = this.GetBarHeight();
            int num2 = 0x10;
            int num3 = this.Height - 0x20;
            if (Y < num2)
            {
                this.m_State = State.SmallScrollUp;
                Gumps.Capture = this;
            }
            else if (Y >= (num2 + num3))
            {
                this.m_State = State.SmallScrollDown;
                Gumps.Capture = this;
            }
            else
            {
                int position = base.GetPosition(num3 - barHeight);
                int num5 = (Y - num2) - position;
                if (num5 < 0)
                {
                    this.m_State = State.LargeScrollUp;
                    Gumps.Capture = this;
                }
                else if (num5 >= barHeight)
                {
                    this.m_State = State.LargeScrollDown;
                    Gumps.Capture = this;
                }
                else
                {
                    this.m_State = State.Normal;
                    this.m_Offset = num5;
                    base.Value = base.GetValue((num5 - this.m_Offset) + position, (this.Height - 0x20) - barHeight);
                    Gumps.Capture = this;
                }
            }
        }

        protected internal override void OnMouseMove(int X, int Y, MouseButtons mb)
        {
            if ((Gumps.Capture == this) && (this.m_State == State.Normal))
            {
                int barHeight = this.GetBarHeight();
                base.Value = base.GetValue((Y - 0x10) - this.m_Offset, (this.Height - 0x20) - barHeight);
            }
        }

        protected internal override void OnMouseUp(int X, int Y, MouseButtons mb)
        {
            if ((Gumps.Capture == this) && (this.m_State == State.Normal))
            {
                int barHeight = this.GetBarHeight();
                base.Value = base.GetValue((Y - 0x10) - this.m_Offset, (this.Height - 0x20) - barHeight);
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