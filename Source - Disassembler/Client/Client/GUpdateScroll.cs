namespace Client
{
    using System;

    public class GUpdateScroll : GBackground
    {
        public GUpdateScroll(string text) : base(0x13c2, 100, 100, 40, 30, true)
        {
            GLabel toAdd = new GLabel("Updates", Engine.DefaultFont, Hues.Load(0x1f0), base.OffsetX, base.OffsetY);
            GBackground background = new GBackground(0xbbc, 100, 100, base.OffsetX, (toAdd.Y + toAdd.Height) + 4, true);
            GWrappedLabel label2 = new GWrappedLabel(text, Engine.GetFont(1), Hues.Load(0x455), background.OffsetX + 2, background.OffsetY + 2, 250);
            background.Width = ((background.Width - background.UseWidth) + label2.Width) + 6;
            background.Height = ((background.Height - background.UseHeight) + label2.Height) + 2;
            background.Children.Add(label2);
            this.Width = (this.Width - base.UseWidth) + background.Width;
            this.Height = (((this.Height - base.UseHeight) + toAdd.Height) + 4) + background.Height;
            toAdd.X += (base.UseWidth - toAdd.Width) / 2;
            base.m_CanDrag = true;
            base.m_QuickDrag = true;
            base.CanClose = true;
            background.SetMouseOverride(this);
            base.m_Children.Add(toAdd);
            base.m_Children.Add(background);
        }
    }
}

