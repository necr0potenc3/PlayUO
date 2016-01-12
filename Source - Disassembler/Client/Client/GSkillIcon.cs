namespace Client
{
    using System.Windows.Forms;

    public class GSkillIcon : GAlphaBackground, IRestorableGump
    {
        private Skill m_Skill;

        public GSkillIcon(Skill skill) : base(0, 0, 100, 20)
        {
            this.m_Skill = skill;
            GLabel toAdd = new GSkillIconName(skill);
            base.m_Width = (toAdd.Image.xMax - toAdd.Image.xMin) + 9;
            base.m_Height = (toAdd.Image.yMax - toAdd.Image.yMin) + 9;
            base.m_Children.Add(toAdd);
            base.m_DragClipX = base.m_Width - 1;
            base.m_DragClipY = base.m_Height - 1;
        }

        protected internal override void OnMouseDown(int x, int y, MouseButtons mb)
        {
            base.BringToTop();
        }

        protected internal override void OnMouseUp(int x, int y, MouseButtons mb)
        {
            if ((mb & MouseButtons.Right) != MouseButtons.None)
            {
                Gumps.Destroy(this);
            }
        }

        int IRestorableGump.Extra
        {
            get
            {
                return this.m_Skill.ID;
            }
        }

        int IRestorableGump.Type
        {
            get
            {
                return 3;
            }
        }

        private class GSkillIconName : GTextButton
        {
            private Skill m_Skill;

            public GSkillIconName(Skill skill) : base(skill.Name, Engine.GetUniFont(0), Hues.Bright, Hues.Load(0x35), 4, 4, null)
            {
                this.m_Skill = skill;
                base.m_CanDrag = true;
                base.m_QuickDrag = false;
                this.X -= base.Image.xMin;
                this.Y -= base.Image.yMin;
                base.m_Tooltip = new SkillTooltip(skill);
            }

            protected internal override bool HitTest(int x, int y)
            {
                return ((((!Engine.amMoving && (Engine.TargetHandler == null)) && ((x >= base.m_Image.xMin) && (x <= base.m_Image.xMax))) && (y >= base.m_Image.yMin)) && (y <= base.m_Image.yMax));
            }

            protected internal override void OnDragStart()
            {
                base.m_IsDragging = false;
                Gumps.Drag = null;
                base.m_Parent.m_IsDragging = true;
                base.m_Parent.m_OffsetX = base.m_X + base.m_OffsetX;
                base.m_Parent.m_OffsetY = base.m_Y + base.m_OffsetY;
                base.m_Parent.X = Engine.m_xMouse - base.m_Parent.m_OffsetX;
                base.m_Parent.Y = Engine.m_yMouse - base.m_Parent.m_OffsetY;
                Gumps.Drag = base.m_Parent;
            }

            protected internal override void OnMouseDown(int x, int y, MouseButtons mb)
            {
                base.BringToTop();
            }

            protected internal override void OnMouseUp(int x, int y, MouseButtons mb)
            {
                if ((mb & MouseButtons.Right) != MouseButtons.None)
                {
                    Gumps.Destroy(base.m_Parent);
                }
                else if ((mb & MouseButtons.Left) != MouseButtons.None)
                {
                    this.m_Skill.Use();
                }
            }
        }
    }
}