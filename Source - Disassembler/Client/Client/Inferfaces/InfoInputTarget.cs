namespace Client
{
    using Client.Targeting;
    using System.Drawing;

    public class InfoInputTarget : InfoInput
    {
        private bool m_Items;
        private bool m_Land;
        private bool m_Mobiles;
        private bool m_Statics;

        public InfoInputTarget(string name, bool items, bool mobiles, bool land, bool statics) : base(name)
        {
            this.m_Items = items;
            this.m_Mobiles = mobiles;
            this.m_Land = land;
            this.m_Statics = statics;
        }

        public override Gump CreateGump()
        {
            GSystemButton button = new GSystemButton(0, 0, 100, 0x18, SystemColors.Control, SystemColors.ControlText, base.Name, Engine.GetUniFont(1));
            button.FillAlpha = 1f;
            button.InactiveColor = GumpPaint.Blend(Color.WhiteSmoke, SystemColors.Control, (float)0.5f);
            button.ActiveColor = GumpPaint.Blend(Color.SteelBlue, SystemColors.Control, (float)0.5f);
            button.PressedColor = GumpPaint.Blend(Color.White, GumpPaint.Blend(Color.SteelBlue, SystemColors.Control, (float)0.5f), (float)0.5f);
            button.OnClick = new OnClick(this.OnClicked);
            return button;
        }

        private void OnClicked(Gump g)
        {
            Engine.TargetHandler = new InternalTarget(this);
        }

        public override void UpdateGump(Gump g)
        {
        }

        public bool Items
        {
            get
            {
                return this.m_Items;
            }
            set
            {
                this.m_Items = value;
            }
        }

        public bool Land
        {
            get
            {
                return this.m_Land;
            }
            set
            {
                this.m_Land = value;
            }
        }

        public bool Mobiles
        {
            get
            {
                return this.m_Mobiles;
            }
            set
            {
                this.m_Mobiles = value;
            }
        }

        public bool Statics
        {
            get
            {
                return this.m_Statics;
            }
            set
            {
                this.m_Statics = value;
            }
        }

        private class InternalTarget : ITargetHandler
        {
            private InfoInputTarget m_Input;

            public InternalTarget(InfoInputTarget input)
            {
                this.m_Input = input;
            }

            public void OnCancel(TargetCancelType why)
            {
            }

            public void OnTarget(object targeted)
            {
                if (this.m_Input.Items && (targeted is Item))
                {
                    this.m_Input.Active = targeted;
                }
                else if (this.m_Input.Mobiles && (targeted is Mobile))
                {
                    this.m_Input.Active = targeted;
                }
                else if (this.m_Input.Land && (targeted is LandTarget))
                {
                    this.m_Input.Active = targeted;
                }
                else if (this.m_Input.Statics && (targeted is StaticTarget))
                {
                    this.m_Input.Active = targeted;
                }
                else
                {
                    Engine.TargetHandler = this;
                }
            }
        }
    }
}