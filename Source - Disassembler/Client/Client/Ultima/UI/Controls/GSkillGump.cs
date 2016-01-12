namespace Client
{
    using System.Windows.Forms;

    public class GSkillGump : Gump
    {
        private int m_Height;
        private GThreeToggle m_Lock;
        private GLabel m_Name;
        private Skill m_Skill;
        private GLabel m_Value;
        private int m_Width;
        private int m_yBase;

        public GSkillGump(Skill skill, int y, int width, bool showReal) : base(4, y)
        {
            this.m_yBase = y;
            this.m_Skill = skill;
            if (skill.Action)
            {
                this.m_Name = new GUsableSkillName(skill);
            }
            else
            {
                this.m_Name = new GLabel(skill.Name, Engine.GetUniFont(1), Hues.Bright, 20, 0);
            }
            this.m_Name.X -= this.m_Name.Image.xMin;
            this.m_Name.Y -= this.m_Name.Image.yMin;
            this.m_Width = width;
            this.m_Height = this.m_Name.Image.yMax - this.m_Name.Image.yMin;
            base.m_Children.Add(this.m_Name);
            float num = showReal ? this.m_Skill.Real : this.m_Skill.Value;
            this.m_Value = new GLabel(num.ToString("F1"), Engine.GetUniFont(1), Hues.Bright, 0, 0);
            this.m_Value.X = (this.m_Width - 0x18) - this.m_Value.Image.xMax;
            this.m_Value.Y -= this.m_Value.Image.yMin;
            if ((this.m_Value.Image.yMax - this.m_Value.Image.yMin) > this.m_Height)
            {
                this.m_Height = this.m_Value.Image.yMax - this.m_Value.Image.yMin;
            }
            base.m_Children.Add(this.m_Value);
            this.m_Lock = new GThreeToggle(Engine.m_SkillUp, Engine.m_SkillDown, Engine.m_SkillLocked, (int)this.m_Skill.Lock, this.m_Width - 4, 0);
            this.m_Lock.OnStateChange = new OnStateChange(this.Lock_OnStateChange);
            this.UpdateLock();
            base.m_Children.Add(this.m_Lock);
        }

        private void Lock_OnStateChange(int state, Gump g)
        {
            SkillLock @lock = (SkillLock)state;
            if (this.m_Skill.Lock != @lock)
            {
                this.m_Skill.Lock = @lock;
                Network.Send(new PUpdateSkillLock(this.m_Skill));
            }
        }

        public void OnSkillChange(double newValue, SkillLock newLock)
        {
            this.m_Value.Text = newValue.ToString("F1");
            this.m_Value.X = (this.m_Width - 0x18) - this.m_Value.Image.xMax;
            this.m_Value.Y = -this.m_Value.Image.yMin;
            this.m_Lock.State = (int)newLock;
            this.UpdateLock();
        }

        public void Scissor(Clipper c)
        {
            this.m_Name.Scissor(c);
            this.m_Value.Scissor(c);
            this.m_Lock.Scissor(c);
        }

        private void UpdateLock()
        {
            this.m_Lock.X = ((this.m_Width - 4) - 8) - (this.m_Lock.Width / 2);
            this.m_Lock.Y = (this.m_Height - this.m_Lock.Height) / 2;
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

        public override int Width
        {
            get
            {
                return this.m_Width;
            }
            set
            {
                this.m_Width = value;
                this.m_Value.X = (this.m_Width - 0x18) - this.m_Value.Image.xMax;
                this.m_Lock.X = ((this.m_Width - 4) - 8) - (this.m_Lock.Width / 2);
            }
        }

        public int yBase
        {
            get
            {
                return this.m_yBase;
            }
        }

        private class GUsableSkillName : GTextButton
        {
            private Skill m_Skill;

            public GUsableSkillName(Skill skill) : base(skill.Name, Engine.GetUniFont(1), Hues.Bright, Hues.Load(0x35), 20, 0, null)
            {
                this.m_Skill = skill;
                base.m_CanDrag = true;
                base.m_QuickDrag = false;
            }

            protected internal override void OnDragStart()
            {
                base.m_IsDragging = false;
                Gumps.Drag = null;
                GSkillIcon toAdd = new GSkillIcon(this.m_Skill);
                toAdd.m_IsDragging = true;
                toAdd.m_OffsetX = toAdd.Width / 2;
                toAdd.m_OffsetY = toAdd.Height / 2;
                toAdd.X = Engine.m_xMouse - toAdd.m_OffsetX;
                toAdd.Y = Engine.m_yMouse - toAdd.m_OffsetY;
                Gumps.Desktop.Children.Add(toAdd);
                Gumps.Drag = toAdd;
            }

            protected internal override void OnMouseDown(int x, int y, MouseButtons mb)
            {
                base.m_Parent.BringToTop();
            }

            protected internal override void OnMouseUp(int x, int y, MouseButtons mb)
            {
                base.m_Parent.BringToTop();
                if ((mb & MouseButtons.Left) != MouseButtons.None)
                {
                    this.m_Skill.Use();
                }
            }
        }
    }
}