namespace Client
{
    using System;
    using System.Windows.Forms;

    public class GVSlider : Gump
    {
        private bool m_Draw;
        private double m_End;
        private Texture m_Gump;
        private int m_HalfHeight;
        private int m_Height;
        private double m_Increase;
        private Client.OnValueChange m_OnValueChange;
        private int m_Position;
        protected double m_ScrollOffset;
        private double m_Start;
        private VertexCache m_vCache;
        private int m_Width;
        private int m_xOffset;

        public GVSlider(int SliderID, int X, int Y, int Width, int Height, double Value, double Start, double End, double Increase) : this(SliderID, Hues.Default, X, Y, Width, Height, Value, Start, End, Increase)
        {
        }

        public GVSlider(int SliderID, IHue Hue, int X, int Y, int Width, int Height, double Value, double Start, double End, double Increase) : base(X, Y)
        {
            this.m_ScrollOffset = 5.0;
            this.m_vCache = new VertexCache();
            this.m_Width = Width;
            this.m_Height = Height;
            this.m_Start = Start;
            this.m_End = End;
            this.m_Increase = Increase;
            this.m_Gump = Hue.GetGump(SliderID);
            if ((this.m_Gump != null) && !this.m_Gump.IsEmpty())
            {
                this.m_HalfHeight = this.m_Gump.Height / 2;
                this.m_xOffset = (this.m_Width - this.m_Gump.Width) / 2;
                this.m_Draw = true;
            }
            this.SetValue(Value, false);
        }

        protected internal override void Draw(int X, int Y)
        {
            if (this.m_Draw)
            {
                this.m_vCache.Draw(this.m_Gump, X + this.m_xOffset, (Y + this.m_Position) - this.m_HalfHeight);
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
                return this.m_Start;
            }
            if (Position >= (this.m_Height - 1))
            {
                return this.m_End;
            }
            double num = ((double) Position) / ((double) this.m_Height);
            double num2 = ((this.m_End - this.m_Start) + 1.0) / this.m_Increase;
            double num3 = (num * num2) + 0.5;
            int num4 = (int) num3;
            double end = num4 * this.m_Increase;
            end += this.m_Start;
            if (end > this.m_End)
            {
                end = this.m_End;
            }
            else if (end < this.m_Start)
            {
                end = this.m_Start;
            }
            return end;
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
            int position = this.m_Position;
            double old = this.GetValue();
            double num3 = Value - this.m_Start;
            num3 /= ((this.m_End - this.m_Start) + 1.0) / this.m_Increase;
            if (num3 < 0.0)
            {
                num3 = 0.0;
            }
            else if (num3 > 1.0)
            {
                num3 = 1.0;
            }
            this.m_Position = (int) ((num3 * this.m_Height) + 0.5);
            if ((Value == this.m_End) && (Value != this.m_Start))
            {
                this.m_Position = this.m_Height - 1;
            }
            if (this.m_Position < 0)
            {
                this.m_Position = 0;
            }
            else if (this.m_Position >= this.m_Height)
            {
                this.m_Position = this.m_Height - 1;
            }
            if (CallOnChange && (this.m_OnValueChange != null))
            {
                this.m_OnValueChange(Value, old, this);
            }
            if (this.GetValue() != Value)
            {
            }
        }

        private void Slide(int Y)
        {
            Gumps.Capture = this;
            if (Y < 0)
            {
                Y = 0;
            }
            if (Y >= this.m_Height)
            {
                Y = this.m_Height - 1;
            }
            int position = this.m_Position;
            double old = this.GetValue();
            this.m_Position = Y;
            if (position != Y)
            {
                if (this.m_OnValueChange != null)
                {
                    this.m_OnValueChange(this.GetValue(), old, this);
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
                this.m_xOffset = (this.m_Width - this.m_Gump.Width) / 2;
            }
        }
    }
}

