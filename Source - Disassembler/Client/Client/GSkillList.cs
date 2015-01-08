namespace Client
{
    using System;

    public class GSkillList : GSingleBorder
    {
        private GHotspot m_Hotspot;
        private GSkills m_Owner;
        private GSkillGump[] m_SkillGumps;
        private GAlphaVSlider m_Slider;
        private GSingleBorder m_SliderBorder;
        private int m_xLast;
        private int m_xWidthLast;
        private int m_yHeightLast;
        private int m_yLast;
        private static Type tGLabel = typeof(GLabel);
        private static Type tGSkillGump = typeof(GSkillGump);

        public GSkillList(GSkills owner) : base(4, 4, 250, 50)
        {
            this.m_Owner = owner;
            base.m_CanDrag = false;
            Skills skills = Engine.Skills;
            this.m_SkillGumps = new GSkillGump[0x100];
            int y = 4;
            for (int i = 0; i < skills.Groups.Length; i++)
            {
                GLabel label;
                SkillGroup group = skills.Groups[i];
                label = new GLabel(group.Name, Engine.GetUniFont(1), Hues.Bright, 4, y) {
                    X = label.X - label.Image.xMin,
                    Y = label.Y - label.Image.yMin
                };
                label.SetTag("yBase", label.Y);
                base.m_Children.Add(label);
                y += 4 + (label.Image.yMax - label.Image.yMin);
                for (int j = 0; j < group.Skills.Count; j++)
                {
                    Skill skill = (Skill) group.Skills[j];
                    GSkillGump toAdd = new GSkillGump(skill, y, base.m_Width - 20, this.m_Owner.ShowReal);
                    this.m_SkillGumps[skill.ID] = toAdd;
                    base.m_Children.Add(toAdd);
                    y += 4 + toAdd.Height;
                }
            }
            this.m_SliderBorder = new GSingleBorder(0, 0, 0x10, 100);
            base.m_Children.Add(this.m_SliderBorder);
            this.m_Slider = new GAlphaVSlider(0, 6, 0x10, 100, 0.0, 0.0, (double) (y + 1), 1.0);
            this.m_Slider.SetTag("Max", y + 1);
            this.m_Slider.OnValueChange = (OnValueChange) Delegate.Combine(this.m_Slider.OnValueChange, new OnValueChange(this.Slider_OnValueChange));
            base.m_Children.Add(this.m_Slider);
            this.m_Hotspot = new GHotspot(0, 0, 0x10, 100, this.m_Slider);
            base.m_Children.Add(this.m_Hotspot);
        }

        protected internal override void Draw(int x, int y)
        {
            if (((this.m_xLast != x) || (this.m_yLast != y)) || ((this.m_xWidthLast != base.m_Width) || (this.m_yHeightLast != base.m_Height)))
            {
                Clipper clipper = new Clipper(x + 1, y + 1, base.m_Width - 0x11, base.m_Height - 2);
                foreach (Gump gump in base.m_Children.ToArray())
                {
                    Type type = gump.GetType();
                    if (type == tGLabel)
                    {
                        ((GLabel) gump).Scissor(clipper);
                    }
                    else if (type == tGSkillGump)
                    {
                        ((GSkillGump) gump).Scissor(clipper);
                    }
                }
                this.m_xLast = x;
                this.m_yLast = y;
                this.m_xWidthLast = base.m_Width;
                this.m_yHeightLast = base.m_Height;
            }
            base.Draw(x, y);
        }

        public void OnSkillChange(Skill skill, bool showReal)
        {
            GSkillGump gump = this.m_SkillGumps[skill.ID];
            if (gump != null)
            {
                gump.OnSkillChange(showReal ? ((double) skill.Real) : ((double) skill.Value), skill.Lock);
            }
        }

        private void Slider_OnValueChange(double vNew, double vOld, Gump sender)
        {
            int num = (int) vNew;
            double tag = (int) sender.GetTag("Max");
            tag = ((double) num) / tag;
            num = (int) -((((int) sender.GetTag("Max")) - base.m_Height) * tag);
            foreach (Gump gump in base.m_Children.ToArray())
            {
                Type type = gump.GetType();
                if (type == tGLabel)
                {
                    gump.Y = num + ((int) gump.GetTag("yBase"));
                }
                else if (type == tGSkillGump)
                {
                    gump.Y = num + ((GSkillGump) gump).yBase;
                }
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
                double num = this.m_Slider.GetValue();
                this.m_Slider.Height = base.m_Height - 11;
                this.m_Slider.SetValue(num, true);
                this.m_SliderBorder.Height = base.m_Height;
                this.m_Hotspot.Height = base.m_Height;
            }
        }

        public bool ShowReal
        {
            set
            {
                Skills skills = Engine.Skills;
                if (!value)
                {
                    for (int i = 0; i < this.m_SkillGumps.Length; i++)
                    {
                        if (this.m_SkillGumps[i] == null)
                        {
                            break;
                        }
                        Skill skill = skills[i];
                        this.m_SkillGumps[i].OnSkillChange((double) skill.Value, skill.Lock);
                    }
                }
                else
                {
                    for (int j = 0; j < this.m_SkillGumps.Length; j++)
                    {
                        if (this.m_SkillGumps[j] == null)
                        {
                            break;
                        }
                        Skill skill2 = skills[j];
                        this.m_SkillGumps[j].OnSkillChange((double) skill2.Real, skill2.Lock);
                    }
                }
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
                this.m_Slider.X = base.m_Width - 15;
                this.m_SliderBorder.X = base.m_Width - 0x10;
                this.m_Hotspot.X = base.m_Width - 0x10;
                for (int i = 0; i < this.m_SkillGumps.Length; i++)
                {
                    if (this.m_SkillGumps[i] == null)
                    {
                        break;
                    }
                    this.m_SkillGumps[i].Width = base.m_Width - 20;
                }
            }
        }
    }
}

