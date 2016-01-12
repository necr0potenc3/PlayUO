namespace Client
{
    using System;
    using System.Windows.Forms;

    public class GQuestionMenu : GBackground
    {
        private GQuestionMenuEntry[] m_Entries;
        private int m_MenuID;
        private int m_Serial;
        private GVSlider m_Slider;

        public GQuestionMenu(int serial, int menuID, string question, AnswerEntry[] answers) : base(0x23f4, Engine.ScreenWidth / 2, 100, 50, 50, true)
        {
            base.m_CanDrag = true;
            base.m_QuickDrag = true;
            this.m_Serial = serial;
            this.m_MenuID = menuID;
            GWrappedLabel toAdd = new GWrappedLabel(question, Engine.GetFont(1), Hues.Load(0x455), base.OffsetX + 4, base.OffsetY + 4, base.UseWidth - 8);
            base.m_Children.Add(toAdd);
            this.m_Entries = new GQuestionMenuEntry[answers.Length];
            GBackground background = new GQuestionBackground(this.m_Entries, base.UseWidth - 8, ((base.UseHeight - 8) - toAdd.Height) - 4, base.OffsetX + 4, (toAdd.Y + toAdd.Height) + 4);
            background.SetMouseOverride(this);
            int offsetX = background.OffsetX;
            int offsetY = background.OffsetY;
            int useWidth = background.UseWidth;
            for (int i = 0; i < answers.Length; i++)
            {
                GQuestionMenuEntry entry = new GQuestionMenuEntry(offsetX, offsetY, useWidth, answers[i]);
                background.Children.Add(entry);
                entry.Radio.ParentOverride = background;
                this.m_Entries[i] = entry;
                offsetY += entry.Height + 4;
            }
            background.Height = ((offsetY - 4) - background.OffsetY) + (background.Height - background.UseHeight);
            this.Height = (((((this.Height - base.UseHeight) + 4) + toAdd.Height) + 4) + background.Height) + 4;
            int num5 = (int)(Engine.ScreenHeight * 0.75);
            if (this.Height > num5)
            {
                this.Height = num5;
                background.Height = ((base.UseHeight - 8) - toAdd.Height) - 4;
            }
            offsetY -= 4;
            offsetY -= background.OffsetY;
            if (offsetY > background.UseHeight)
            {
                int num6 = offsetY;
                background.Width += 0x13;
                this.Width += 0x13;
                offsetX = (background.OffsetX + background.UseWidth) - 15;
                offsetY = background.OffsetY;
                background.Children.Add(new GImage(0x101, offsetX, offsetY));
                background.Children.Add(new GImage(0xff, offsetX, (offsetY + background.UseHeight) - 0x20));
                for (int j = offsetY + 30; (j + 0x20) < background.UseHeight; j += 30)
                {
                    background.Children.Add(new GImage(0x100, offsetX, j));
                }
                this.m_Slider = new GVSlider(0xfe, offsetX + 1, (offsetY + 1) + 12, 13, (background.UseHeight - 2) - 0x18, 0.0, 0.0, (double)(num6 - background.UseHeight), 1.0);
                this.m_Slider.OnValueChange = new OnValueChange(this.OnScroll);
                this.m_Slider.ScrollOffset = 20.0;
                background.Children.Add(this.m_Slider);
                background.Children.Add(new GHotspot(offsetX, offsetY, 15, background.UseHeight, this.m_Slider));
            }
            GButtonNew new2 = new GButtonNew(0xf3, 0xf2, 0xf1, 0, (background.Y + background.Height) + 4);
            GButtonNew new3 = new GButtonNew(0xf9, 0xf7, 0xf8, 0, new2.Y);
            new2.Clicked += new EventHandler(this.Cancel_Clicked);
            new3.Clicked += new EventHandler(this.Okay_Clicked);
            new2.X = ((base.OffsetX + base.UseWidth) - 4) - new2.Width;
            new3.X = (new2.X - 4) - new3.Width;
            base.m_Children.Add(new2);
            base.m_Children.Add(new3);
            this.Height += 4 + new2.Height;
            base.m_Children.Add(background);
            this.Center();
        }

        private void Cancel()
        {
            Network.Send(new PQuestionMenuCancel(this.m_Serial, this.m_MenuID));
            Gumps.Destroy(this);
        }

        private void Cancel_Clicked(object sender, EventArgs e)
        {
            this.Cancel();
        }

        private void Okay_Clicked(object sender, EventArgs e)
        {
            for (int i = 0; i < this.m_Entries.Length; i++)
            {
                if (this.m_Entries[i].Radio.State)
                {
                    AnswerEntry answer = this.m_Entries[i].Answer;
                    Network.Send(new PQuestionMenuResponse(this.m_Serial, this.m_MenuID, answer.Index, answer.ItemID, 0));
                    Gumps.Destroy(this);
                    break;
                }
            }
        }

        protected internal override void OnMouseDown(int x, int y, MouseButtons mb)
        {
            base.BringToTop();
        }

        protected internal override void OnMouseUp(int x, int y, MouseButtons mb)
        {
            if ((mb & MouseButtons.Right) != MouseButtons.None)
            {
                this.Cancel();
            }
        }

        protected internal override void OnMouseWheel(int delta)
        {
            if (this.m_Slider != null)
            {
                this.m_Slider.OnMouseWheel(delta);
            }
        }

        private void OnScroll(double vNew, double vOld, Gump g)
        {
            int num = (int)vNew;
            for (int i = 0; i < this.m_Entries.Length; i++)
            {
                this.m_Entries[i].Y = this.m_Entries[i].yBase - num;
            }
        }

        public int MenuID
        {
            get
            {
                return this.m_MenuID;
            }
        }

        public int Serial
        {
            get
            {
                return this.m_Serial;
            }
        }
    }
}