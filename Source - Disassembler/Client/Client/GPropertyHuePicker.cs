namespace Client
{
    using System;

    public class GPropertyHuePicker : GAlphaBackground
    {
        private GBrightnessBar m_Bar;
        private GPropertyEntry m_Entry;
        private GHuePicker m_HuePicker;

        public GPropertyHuePicker(GPropertyEntry entry) : base(0, 0, 200, 150)
        {
            int num = (int) entry.Entry.Property.GetValue(entry.Object, null);
            this.m_Entry = entry;
            base.m_CanDrag = false;
            base.FillColor = GumpColors.Control;
            base.BorderColor = GumpColors.ControlDarkDark;
            base.FillAlpha = 1f;
            GHuePicker toAdd = this.m_HuePicker = new GHuePicker(7, 7);
            toAdd.m_CanDrag = false;
            toAdd.OnHueSelect = new OnHueSelect(this.HueSelected);
            base.m_Children.Add(toAdd);
            GBrightnessBar bar = this.m_Bar = new GBrightnessBar((toAdd.X + toAdd.Width) + 1, toAdd.Y, 15, toAdd.Height, toAdd);
            bar.m_CanDrag = false;
            base.m_Children.Add(bar);
            if ((num >= 2) && (num <= 0x3e9))
            {
                num -= 2;
                toAdd.ShadeX = (num / 5) % 20;
                toAdd.ShadeY = (num / 5) / 20;
                toAdd.Brightness = num % 5;
                bar.Refresh();
            }
            GSingleBorder border = new GSingleBorder(bar.X - 1, bar.Y, 1, bar.Height);
            base.m_Children.Add(border);
            this.Width = ((7 + toAdd.Width) + bar.Width) + 7;
            this.Height = (7 + toAdd.Height) + 7;
        }

        protected internal override void Draw(int X, int Y)
        {
            base.Draw(X, Y);
            int num = X + this.m_HuePicker.X;
            int num2 = Y + this.m_HuePicker.Y;
            int num3 = (this.m_HuePicker.Width + 1) + this.m_Bar.Width;
            int height = this.m_HuePicker.Height;
            Renderer.SetTexture(null);
            GumpPaint.DrawSunken3D((X + this.m_HuePicker.X) - 2, (Y + this.m_HuePicker.Y) - 2, ((this.m_HuePicker.Width + 1) + this.m_Bar.Width) + 4, this.m_HuePicker.Height + 4);
        }

        private void HueReleased(int hue, Gump g)
        {
            this.m_Entry.SetValue(hue);
            Gumps.Destroy(this);
        }

        private void HueSelected(int hue, Gump g)
        {
            this.m_Entry.SetValue(hue);
        }
    }
}

