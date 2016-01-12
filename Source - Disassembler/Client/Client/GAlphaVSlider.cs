namespace Client
{
    using System;
    using System.Windows.Forms;

    public class GAlphaVSlider : Gump
    {
        protected double m_End;
        protected int m_HalfHeight;
        protected int m_Height;
        protected double m_Increase;
        protected Client.OnValueChange m_OnValueChange;
        protected int m_Position;
        protected double m_ScrollOffset;
        protected double m_Start;
        protected int m_Width;
        protected int m_xOffset;

        public GAlphaVSlider(int X, int Y, int Width, int Height, double Value, double Start, double End, double Increase) : base(X, Y)
        {
            this.m_ScrollOffset = 5.0;
            this.m_Width = Width;
            this.m_Height = Height;
            this.m_Start = Start;
            this.m_End = End;
            this.m_Increase = Increase;
            this.m_HalfHeight = 6;
            this.SetValue(Value, false);
        }

        protected internal override void Draw(int X, int Y)
        {
            Renderer.SetTexture(null);
            Renderer.AlphaTestEnable = false;
            Renderer.SetAlphaEnable(true);
            Renderer.SetAlpha(0.5f);
            Renderer.SolidRect(0x808080, X, (Y - this.m_HalfHeight) + 1, this.m_Width - 2, (this.m_Height + (this.m_HalfHeight * 2)) - 3);
            X += this.m_xOffset;
            Y += this.m_Position - this.m_HalfHeight;
            X--;
            Renderer.SetAlpha(0.8f);
            Renderer.TransparentRect(0, X, Y, 0x10, 12);
            Renderer.SetAlpha(0.5f);
            if (Gumps.Capture == this)
            {
                Renderer.GradientRect(0xfafafa, 0x969696, X + 1, Y + 1, 14, 10);
            }
            else
            {
                Renderer.GradientRect(0xc8c8c8, 0x646464, X + 1, Y + 1, 14, 10);
            }
            Renderer.SetAlphaEnable(false);
            Renderer.AlphaTestEnable = true;
        }

        public double GetValue()
        {
            return this.GetValue(this.m_Position);
        }

        public double GetValue(int Position)
        {
            if (Position < 0)
            {
                Position = 0;
            }
            else if (Position >= this.m_Height)
            {
                Position = this.m_Height - 1;
            }
            double num = (this.m_End - this.m_Start) + 1.0;
            num -= 1E-13;
            double num2 = ((double)Position) / ((double)(this.m_Height - 1));
            double num3 = num2 * num;
            num3 += this.m_Start;
            return (double)((int)num3);
        }

        protected internal override bool HitTest(int X, int Y)
        {
            return (!Engine.amMoving && (Engine.TargetHandler == null));
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
            this.SetValue(this.GetValue() + ((-Math.Sign(Delta) * this.m_ScrollOffset) * this.m_Increase), true);
        }

        public void SetValue(double Value, bool CallOnChange)
        {
            if (Value > this.m_End)
            {
                Value = this.m_End;
            }
            else if (Value < this.m_Start)
            {
                Value = this.m_Start;
            }
            double old = this.GetValue();
            double num2 = Value - this.m_Start;
            num2 /= this.m_End - this.m_Start;
            if (num2 < 0.0)
            {
                num2 = 0.0;
            }
            else if (num2 > 1.0)
            {
                num2 = 1.0;
            }
            this.m_Position = (int)(num2 * (this.m_Height - 1));
            if (CallOnChange && (this.m_OnValueChange != null))
            {
                this.m_OnValueChange(Value, old, this);
            }
        }

        public void Slide(int Y)
        {
            Gumps.Capture = this;
            int position = this.m_Position;
            double old = this.GetValue();
            this.m_Position = Y;
            if (this.m_Position < 0)
            {
                this.m_Position = 0;
            }
            else if (this.m_Position >= this.m_Height)
            {
                this.m_Position = this.m_Height - 1;
            }
            double num3 = (this.m_End - this.m_Start) + 1.0;
            num3 -= 1E-13;
            double num4 = ((double)this.m_Position) / ((double)(this.m_Height - 1));
            double num5 = num4 * num3;
            num5 += this.m_Start;
            num5 = (int)num5;
            if (position != this.m_Position)
            {
                if (this.m_OnValueChange != null)
                {
                    this.m_OnValueChange(num5, old, this);
                }
                Engine.Redraw();
            }
        }

        public double End
        {
            get
            {
                return this.m_End;
            }
            set
            {
                this.m_End = value;
            }
        }

        public int HalfHeight
        {
            get
            {
                return this.m_HalfHeight;
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
            }
        }

        public double Increase
        {
            get
            {
                return this.m_Increase;
            }
            set
            {
                this.m_Increase = value;
            }
        }

        public Client.OnValueChange OnValueChange
        {
            get
            {
                return this.m_OnValueChange;
            }
            set
            {
                this.m_OnValueChange = value;
            }
        }

        public int Position
        {
            get
            {
                return this.m_Position;
            }
            set
            {
                this.m_Position = value;
            }
        }

        public double ScrollOffset
        {
            get
            {
                return this.m_ScrollOffset;
            }
            set
            {
                this.m_ScrollOffset = value;
            }
        }

        public double Start
        {
            get
            {
                return this.m_Start;
            }
            set
            {
                this.m_Start = value;
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
                this.m_xOffset = (this.m_Width - 0x10) / 2;
            }
        }
    }
}