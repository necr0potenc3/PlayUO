namespace Client
{
    using System;

    public class GIdleWarning : GBackground
    {
        public GIdleWarning() : base(0xa2c, 100, 100, 0, 0, true)
        {
            base.m_CanDrag = true;
            base.m_QuickDrag = true;
            GWrappedLabel toAdd = new GWrappedLabel("You have been idle for too long. If you do not do anything in the next minute, you will be logged out.", Engine.GetFont(2), Hues.Load(0x455), base.OffsetX, base.OffsetY - 12, 0x113);
            base.m_Children.Add(toAdd);
            GButtonNew new2 = new GButtonNew(0x481, 0, (toAdd.Y + toAdd.Height) + 4);
            new2.Clicked += new EventHandler(this.Check_Clicked);
            base.m_Children.Add(new2);
            this.Width = (this.Width - base.UseWidth) + toAdd.Width;
            this.Height = ((this.Height - base.UseHeight) + toAdd.Height) - 12;
            new2.X = base.OffsetX + ((base.UseWidth - new2.Width) / 2);
            this.Center();
        }

        private void Check_Clicked(object sender, EventArgs e)
        {
            Gumps.Destroy(this);
        }
    }
}

