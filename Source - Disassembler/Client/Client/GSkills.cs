namespace Client
{
    using System;
    using System.Windows.Forms;

    public class GSkills : GAlphaBackground, IResizable, IRestorableGump
    {
        private bool m_ShouldClose;
        private bool m_ShowReal;
        private GSkillList m_SkillList;
        private GLabel m_Total;
        private GTextButton m_ValueType;

        public GSkills() : base(50, 50, 250, 0x7d)
        {
            base.m_Children.Add(new GVResizer(this));
            base.m_Children.Add(new GHResizer(this));
            base.m_Children.Add(new GLResizer(this));
            base.m_Children.Add(new GTResizer(this));
            base.m_Children.Add(new GHVResizer(this));
            base.m_Children.Add(new GLTResizer(this));
            base.m_Children.Add(new GHTResizer(this));
            base.m_Children.Add(new GLVResizer(this));
            this.m_ShowReal = false;
            this.m_Total = new GLabel(string.Format("Total: {0:F1}", this.GetTotalSkillCount()), Engine.GetUniFont(1), Hues.Bright, 0, 0);
            base.m_Children.Add(this.m_Total);
            this.m_ValueType = new GTextButton("Used Values", Engine.GetUniFont(1), Hues.Bright, Hues.Load(0x35), 0, 0, new OnClick(this.ValueType_OnClick));
            this.m_ValueType.X = 4 - this.m_ValueType.Image.xMin;
            base.m_Children.Add(this.m_ValueType);
            this.m_SkillList = new GSkillList(this);
            base.m_Children.Add(this.m_SkillList);
            this.Width = 250;
            this.Height = 0x7d;
        }

        private float GetTotalSkillCount()
        {
            float num = 0f;
            Skills skills = Engine.Skills;
            for (int i = 0; i < 0x100; i++)
            {
                Skill skill = skills[i];
                if (skill == null)
                {
                    return num;
                }
                num += skill.Real;
            }
            return num;
        }

        protected internal override void OnDispose()
        {
            Engine.m_SkillsOpen = false;
            Engine.m_SkillsGump = null;
        }

        protected internal override void OnMouseDown(int x, int y, MouseButtons mb)
        {
            base.BringToTop();
            if ((mb & MouseButtons.Right) != MouseButtons.None)
            {
                this.m_ShouldClose = true;
            }
        }

        protected internal override void OnMouseEnter(int x, int y, MouseButtons mb)
        {
            if (this.m_ShouldClose && ((mb & MouseButtons.Right) == MouseButtons.None))
            {
                this.m_ShouldClose = false;
            }
        }

        protected internal override void OnMouseUp(int x, int y, MouseButtons mb)
        {
            if (this.m_ShouldClose && ((mb & MouseButtons.Right) != MouseButtons.None))
            {
                Gumps.Destroy(this);
            }
            else
            {
                this.m_ShouldClose = false;
            }
        }

        public void OnSkillChange(Skill skill)
        {
            this.m_SkillList.OnSkillChange(skill, this.m_ShowReal);
            this.UpdateTotal();
        }

        protected internal override void Render(int x, int y)
        {
            base.Render(x, y);
        }

        private void UpdateTotal()
        {
            this.m_Total.Text = string.Format("Total: {0:F1}", this.GetTotalSkillCount());
            this.m_Total.X = (base.m_Width - 5) - this.m_Total.Image.xMax;
            this.m_Total.Y = (base.m_Height - 5) - this.m_Total.Image.yMax;
            int num = this.m_Total.Y + this.m_Total.Image.yMin;
            if ((this.m_ValueType.Y + this.m_ValueType.Image.yMin) < num)
            {
                num = this.m_ValueType.Y + this.m_ValueType.Image.yMin;
            }
            this.m_SkillList.Height = num - 7;
        }

        private void ValueType_OnClick(Gump g)
        {
            this.m_ShowReal = !this.m_ShowReal;
            this.m_ValueType.Text = this.m_ShowReal ? "Real Values" : "Used Values";
            this.m_ValueType.X = 4 - this.m_ValueType.Image.xMin;
            this.m_ValueType.Y = (base.m_Height - 5) - this.m_ValueType.Image.yMax;
            this.m_SkillList.ShowReal = this.m_ShowReal;
        }

        public int Extra
        {
            get
            {
                return 0;
            }
        }

        public override int Height
        {
            get
            {
                return base.m_Height;
            }
            set
            {
                base.m_Height = value;
                this.m_Total.Y = (base.m_Height - 5) - this.m_Total.Image.yMax;
                this.m_ValueType.Y = (base.m_Height - 5) - this.m_ValueType.Image.yMax;
                int num = this.m_Total.Y + this.m_Total.Image.yMin;
                if ((this.m_ValueType.Y + this.m_ValueType.Image.yMin) < num)
                {
                    num = this.m_ValueType.Y + this.m_ValueType.Image.yMin;
                }
                this.m_SkillList.Height = num - 7;
            }
        }

        public int MaxHeight
        {
            get
            {
                return Engine.ScreenHeight;
            }
        }

        public int MaxWidth
        {
            get
            {
                return Engine.ScreenWidth;
            }
        }

        public int MinHeight
        {
            get
            {
                return 0x2b;
            }
        }

        public int MinWidth
        {
            get
            {
                return 0xe1;
            }
        }

        public bool ShowReal
        {
            get
            {
                return this.m_ShowReal;
            }
        }

        public int Type
        {
            get
            {
                return 1;
            }
        }

        public override int Width
        {
            get
            {
                return base.m_Width;
            }
            set
            {
                base.m_Width = value;
                this.m_Total.X = (base.m_Width - 5) - this.m_Total.Image.xMax;
                this.m_SkillList.Width = base.m_Width - 8;
            }
        }
    }
}

