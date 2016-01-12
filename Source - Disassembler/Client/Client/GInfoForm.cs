namespace Client
{
    using System.Drawing;

    public class GInfoForm : GWindowsForm
    {
        private Gump m_ContentGump;
        private GMainMenu m_CPMenu;
        private Gump m_InputGump;
        private InfoProvider m_Provider;

        public GInfoForm() : base(0, 0, 400, 430)
        {
            base.m_NonRestrictivePicking = true;
            base.Client.m_NonRestrictivePicking = true;
            Gumps.Focus = this;
            base.Text = "Information Browser";
            InfoProvider[] providerArray = new InfoProvider[] { new CommandInfoProvider(), new CastingInfoProvider(), new SwingInfoProvider() };
            GMainMenu toAdd = new GMainMenu((this.Width - 130) - 4, 8);
            GMenuItem child = new GMenuItem("Change Provider");
            child.DropDown = true;
            for (int i = 0; i < providerArray.Length; i++)
            {
                child.Add(new ChangeProviderMenu(this, providerArray[i]));
            }
            toAdd.Add(child);
            this.RecurseFormatMenu(child);
            base.Client.Children.Add(toAdd);
            this.m_CPMenu = toAdd;
            this.Provider = providerArray[0];
            base.GUID = "Info Browser";
            this.Center();
        }

        private GMenuItem FormatMenu(GMenuItem mi)
        {
            mi.FillAlpha = 1f;
            mi.DefaultColor = GumpPaint.Blend(Color.WhiteSmoke, SystemColors.Control, (float)0.5f);
            mi.OverColor = GumpPaint.Blend(Color.SteelBlue, SystemColors.Control, (float)0.5f);
            mi.ExpandedColor = GumpPaint.Blend(Color.SteelBlue, SystemColors.Control, (float)0.5f);
            mi.SetHue(Hues.Load(1));
            return mi;
        }

        public static void Open()
        {
            if (Gumps.FindGumpByGUID("Info Browser") == null)
            {
                Gumps.Desktop.Children.Add(new GInfoForm());
            }
        }

        private void RecurseFormatMenu(GMenuItem mi)
        {
            this.FormatMenu(mi);
            for (int i = 0; i < mi.Children.Count; i++)
            {
                if (mi.Children[i] is GMenuItem)
                {
                    this.RecurseFormatMenu((GMenuItem)mi.Children[i]);
                }
            }
        }

        public InfoProvider Provider
        {
            get
            {
                return this.m_Provider;
            }
            set
            {
                if (this.m_Provider != value)
                {
                    if (this.m_InputGump != null)
                    {
                        Gumps.Destroy(this.m_InputGump);
                    }
                    if (this.m_ContentGump != null)
                    {
                        Gumps.Destroy(this.m_ContentGump);
                    }
                    this.m_Provider = value;
                    if (this.m_Provider == null)
                    {
                        this.m_InputGump = null;
                        this.m_ContentGump = null;
                        base.Text = "Information Browser";
                    }
                    else
                    {
                        this.m_InputGump = new GEmpty(6, 8, this.Width - 20, 0x19);
                        this.m_InputGump.m_NonRestrictivePicking = true;
                        int num = 0;
                        for (int i = 0; i < this.m_Provider.Inputs.Length; i++)
                        {
                            Gump toAdd = this.m_Provider.Inputs[i].Gump;
                            toAdd.X = num;
                            num += toAdd.Width + 5;
                            this.m_InputGump.Children.Add(toAdd);
                        }
                        this.m_ContentGump = this.m_Provider.Gump;
                        this.m_ContentGump.X = 6;
                        this.m_ContentGump.Y = 0x2b;
                        base.Client.Children.Add(this.m_ContentGump);
                        base.Client.Children.Add(this.m_InputGump);
                        base.Text = string.Format("Information Browser - {0}", this.m_Provider.Name);
                    }
                    if (this.m_CPMenu != null)
                    {
                        this.m_CPMenu.BringToTop();
                    }
                }
            }
        }
    }
}