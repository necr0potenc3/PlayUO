namespace Client
{
    using System;
    using System.Windows.Forms;

    public class GSlider : Gump
    {
        private bool m_Draw;
        private double m_End;
        private Texture m_Gump;
        private int m_HalfWidth;
        private int m_Height;
        private double m_Increase;
        private Client.OnValueChange m_OnValueChange;
        private int m_Position;
        private double m_Start;
        private VertexCache m_vCache;
        private int m_Width;
        private int m_yOffset;

        public GSlider(int SliderID, int X, int Y, int Width, int Height, double Value, double Start, double End, double Increase) : this(SliderID, Hues.Default, X, Y, Width, Height, Value, Start, End, Increase)
        {
        }

        public GSlider(int SliderID, IHue Hue, int X, int Y, int Width, int Height, double Value, double Start, double End, double Increase) : base(X, Y)
        {
            this.m_vCache = new VertexCache();
            this.m_Width = Width;
            this.m_Height = Height;
            this.m_Start = Start;
            this.m_End = End;
            this.m_Increase = Increase;
            this.m_Gump = Hue.GetGump(SliderID);
            if ((this.m_Gump != null) && !this.m_Gump.IsEmpty())
            {
                this.m_HalfWidth = this.m_Gump.Width / 2;
                this.m_yOffset = (this.m_Height - this.m_Gump.Height) / 2;
                this.m_Draw = true;
            }
            this.SetValue(Value, false);
        }

        protected internal override void Draw(int X, int Y)
        {
            if (this.m_Draw)
            {
                this.m_vCache.Draw(this.m_Gump, (X + this.m_Position) - this.m_HalfWidth, Y + this.m_yOffset);
            }
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
            else if (Position >= this.m_Width)
            {
                Position = this.m_Width - 1;
            }
            double num = (this.m_End - this.m_Start) + 1.0;
            num -= 1E-13;
            double num2 = ((double) Position) / ((double) (this.m_Width - 1));
            double num3 = num2 * num;
            num3 += this.m_Start;
            return (double) ((int) num3);
        }

        protected internal override bool HitTest(int X, int Y)
        {
            return true;
        }

        protected internal override void OnMouseDown(int X, int Y, MouseButtons mb)
        {
            if (mb != MouseButtons.None)
            {
                this.Slide(X);
            }
        }

        protected internal override void OnMouseEnter(int X, int Y, MouseButtons mb)
        {
            if (mb != MouseButtons.None)
            {
                this.Slide(X);
            }
        }

        protected internal override void OnMouseMove(int X, int Y, MouseButtons mb)
        {
            if (mb != MouseButtons.None)
            {
                this.Slide(X);
            }
        }

        protected internal override void OnMouseUp(int X, int Y, MouseButtons mb)
        {
            if (mb != MouseButtons.None)
            {
                this.Slide(X);
            }
            Gumps.Capture = null;
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
            this.m_Position = (int) (num2 * (this.m_Width - 1));
            if (CallOnChange && (this.m_OnValueChange != null))
            {
                this.m_OnValueChange(Value, old, this);
            }
        }

        public void Slide(int X)
        {
            Gumps.Capture = this;
            int position = this.m_Position;
            double old = this.GetValue();
            int num3 = X;
            this.m_Position = X;
            if (this.m_Position < 0)
            {
                this.m_Position = 0;
            }
            else if (this.m_Position >= this.m_Width)
            {
                this.m_Position = this.m_Width - 1;
            }
            double num4 = (this.m_End - this.m_Start) + 1.0;
            num4 -= 1E-13;
            double num5 = ((double) this.m_Position) / ((double) (this.m_Width - 1));
            if (this.m_Position == 0)
            {
                num5 = 0.0;
            }
            else if (this.m_Position == (this.m_Width - 1))
            {
                num5 = 1.0;
            }
            double num6 = num5 * num4;
            num6 += this.m_Start;
            num6 = (int) num6;
            if (((old != num6) || (position != this.m_Position)) || (num3 != position))
            {
                if (this.m_OnValueChange != null)
                {
                    this.m_OnValueChange(num6, old, this);
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

        public override int Height
        {
            get
            {
                return this.m_Height;
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
        }
    }
}

