namespace Client
{
    using System;

    public abstract class GSliderBase : Gump
    {
        private int m_Cur;
        private int m_Height;
        private int m_LargeOffset;
        private int m_Max;
        private int m_Min;
        private OnSliderChanged m_OnChanged;
        private OnSliderChanging m_OnChanging;
        private int m_SmallOffset;
        private int m_WheelOffset;
        private int m_Width;

        public GSliderBase(int x, int y) : base(x, y)
        {
            this.m_WheelOffset = 1;
            this.m_LargeOffset = 1;
            this.m_SmallOffset = 1;
        }

        protected virtual void ChangeTo(int v)
        {
            int cur = this.m_Cur;
            if (this.OnChanging(v))
            {
                this.m_Cur = v;
                this.OnChanged(cur);
            }
        }

        public int GetPosition(int size)
        {
            if (this.m_Cur < this.m_Min)
            {
                this.m_Cur = this.m_Min;
            }
            if (this.m_Cur > this.m_Max)
            {
                this.m_Cur = this.m_Max;
            }
            int num = this.m_Max - this.m_Min;
            int num2 = this.m_Cur - this.m_Min;
            if (num == 0)
            {
                return 0;
            }
            return ((size * num2) / num);
        }

        public int GetValue(int pos, int size)
        {
            if (pos < 0)
            {
                pos = 0;
            }
            if (pos > (size - 1))
            {
                pos = size - 1;
            }
            int num = size - 1;
            int num2 = pos;
            if (num == 0)
            {
                return 0;
            }
            return (this.m_Min + (((this.m_Max - this.m_Min) * num2) / num));
        }

        protected virtual void OnChanged(int oldValue)
        {
            if (this.m_OnChanged != null)
            {
                this.m_OnChanged(oldValue, this);
            }
        }

        protected virtual bool OnChanging(int newValue)
        {
            if (this.m_OnChanging != null)
            {
                return this.m_OnChanging(newValue, this);
            }
            return true;
        }

        protected internal override void OnMouseWheel(int delta)
        {
            this.Value -= Math.Sign(delta) * this.WheelOffset;
        }

        protected virtual void SlideTo(int pos, int size)
        {
            int num = this.GetValue(pos, size);
            if (this.Value < num)
            {
                if ((this.Value + this.m_SmallOffset) < num)
                {
                    this.Value += this.m_SmallOffset;
                }
                else
                {
                    this.Value = num;
                }
            }
            else if (this.Value > num)
            {
                if ((this.Value - this.m_SmallOffset) > num)
                {
                    this.Value -= this.m_SmallOffset;
                }
                else
                {
                    this.Value = num;
                }
            }
        }

        public OnSliderChanged Changed
        {
            get
            {
                return this.m_OnChanged;
            }
            set
            {
                this.m_OnChanged = value;
            }
        }

        public OnSliderChanging Changing
        {
            get
            {
                return this.m_OnChanging;
            }
            set
            {
                this.m_OnChanging = value;
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

        public int LargeOffset
        {
            get
            {
                return this.m_LargeOffset;
            }
            set
            {
                this.m_LargeOffset = value;
            }
        }

        public int Maximum
        {
            get
            {
                return this.m_Max;
            }
            set
            {
                this.m_Max = value;
                if (this.m_Cur > this.m_Max)
                {
                    this.ChangeTo(this.m_Max);
                }
            }
        }

        public int Minimum
        {
            get
            {
                return this.m_Min;
            }
            set
            {
                this.m_Min = value;
                if (this.m_Cur < this.m_Min)
                {
                    this.ChangeTo(this.m_Min);
                }
            }
        }

        public int SmallOffset
        {
            get
            {
                return this.m_SmallOffset;
            }
            set
            {
                this.m_SmallOffset = value;
            }
        }

        public int Value
        {
            get
            {
                return this.m_Cur;
            }
            set
            {
                if (value < this.m_Min)
                {
                    value = this.m_Min;
                }
                else if (value > this.m_Max)
                {
                    value = this.m_Max;
                }
                if (this.m_Cur != value)
                {
                    this.ChangeTo(value);
                }
            }
        }

        public int WheelOffset
        {
            get
            {
                return this.m_WheelOffset;
            }
            set
            {
                this.m_WheelOffset = value;
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

