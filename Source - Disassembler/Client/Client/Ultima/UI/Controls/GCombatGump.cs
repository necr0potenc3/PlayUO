namespace Client
{
    public class GCombatGump : GDragable
    {
        private static IHue m_AbleHue = new Hues.HFill(0x15155a);
        private static IHue m_ActiveHue = new Hues.HFill(0x5a1515);
        private static IHue m_DefaultHue = new Hues.HFill(0x5a4a31);
        private static GCombatGump m_Instance;
        private GAbilityIcon m_PrimaryIcon;
        private GAbilityIcon m_SecondaryIcon;

        public GCombatGump() : base(0x2b02, 50, 50)
        {
            AbilityInfo[] abilities = AbilityInfo.Abilities;
            AbilityInfo active = AbilityInfo.Active;
            AbilityInfo primary = AbilityInfo.Primary;
            AbilityInfo secondary = AbilityInfo.Secondary;
            IFont uniFont = Engine.GetUniFont(1);
            OnClick onClick = new OnClick(this.Name_OnClick);
            GLabel toAdd = new GLabel("INDEX", Engine.GetFont(6), Hues.Default, 100, 4);
            base.m_Children.Add(toAdd);
            toAdd = new GLabel("INDEX", Engine.GetFont(6), Hues.Default, 0x106, 4);
            base.m_Children.Add(toAdd);
            for (int i = 0; i < abilities.Length; i++)
            {
                AbilityInfo a = abilities[i];
                IHue hueFor = GetHueFor(a);
                toAdd = new GTextButton(Localization.GetString(a.Name), uniFont, hueFor, hueFor, 0x38 + ((i / 9) * 0xa2), 0x26 + ((i % 9) * 15), onClick);
                a.NameLabel = (GTextButton)toAdd;
                toAdd.SetTag("Ability", a);
                toAdd.Tooltip = new Tooltip(Localization.GetString(a.Tooltip), true, 240);
                toAdd.Tooltip.Delay = 0.25f;
                base.m_Children.Add(toAdd);
            }
            this.m_PrimaryIcon = new GAbilityIcon(true, true, primary.Icon, 0xda, 0x69);
            this.m_PrimaryIcon.Tooltip = new Tooltip(Localization.GetString(primary.Name), true);
            this.m_PrimaryIcon.Tooltip.Delay = 0.25f;
            this.m_PrimaryIcon.Hue = (primary == AbilityInfo.Active) ? Hues.Load(0x8026) : Hues.Default;
            base.m_Children.Add(this.m_PrimaryIcon);
            toAdd = new GLabel("Primary", Engine.GetFont(6), Hues.Default, 0x10c, 0x69);
            base.m_Children.Add(toAdd);
            toAdd = new GLabel("Ability Icon", Engine.GetFont(6), Hues.Default, 0x10c, 0x77);
            base.m_Children.Add(toAdd);
            this.m_SecondaryIcon = new GAbilityIcon(true, false, secondary.Icon, 0xda, 150);
            this.m_SecondaryIcon.Tooltip = new Tooltip(Localization.GetString(secondary.Name), true);
            this.m_SecondaryIcon.Tooltip.Delay = 0.25f;
            this.m_SecondaryIcon.Hue = (secondary == AbilityInfo.Active) ? Hues.Load(0x8026) : Hues.Default;
            base.m_Children.Add(this.m_SecondaryIcon);
            toAdd = new GLabel("Secondary", Engine.GetFont(6), Hues.Default, 0x10c, 150);
            base.m_Children.Add(toAdd);
            toAdd = new GLabel("Ability Icon", Engine.GetFont(6), Hues.Default, 0x10c, 0xa4);
            base.m_Children.Add(toAdd);
        }

        public static IHue GetHueFor(AbilityInfo a)
        {
            if (a == AbilityInfo.Active)
            {
                return m_ActiveHue;
            }
            if ((a == AbilityInfo.Primary) || (a == AbilityInfo.Secondary))
            {
                return m_AbleHue;
            }
            return m_DefaultHue;
        }

        private void InternalUpdate()
        {
            Gump[] gumpArray = base.m_Children.ToArray();
            for (int i = 0; i < gumpArray.Length; i++)
            {
                GTextButton button = gumpArray[i] as GTextButton;
                if (button != null)
                {
                    AbilityInfo tag = (AbilityInfo)button.GetTag("Ability");
                    if (tag != null)
                    {
                        tag.NameLabel.FocusHue = tag.NameLabel.DefaultHue = GetHueFor(tag);
                    }
                }
            }
        }

        private void Name_OnClick(Gump sender)
        {
            AbilityInfo tag = (AbilityInfo)sender.GetTag("Ability");
            if (AbilityInfo.Active == tag)
            {
                AbilityInfo.Active = null;
            }
            else
            {
                AbilityInfo.Active = tag;
            }
        }

        protected internal override void OnDispose()
        {
            base.OnDispose();
            m_Instance = null;
        }

        public static void Open()
        {
            if (m_Instance == null)
            {
                m_Instance = new GCombatGump();
                Gumps.Desktop.Children.Add(m_Instance);
            }
        }

        public static void Update()
        {
            if (m_Instance != null)
            {
                m_Instance.InternalUpdate();
            }
            GAbilityIcon.Update();
        }
    }
}