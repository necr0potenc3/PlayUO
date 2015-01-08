namespace Client
{
    using System;

    public class GDragAmount : GDragable
    {
        private int m_Amount;
        private bool m_First;
        private Client.Item m_Item;
        private GButtonNew m_Okay;
        private GSlider m_Slider;
        private GTextBox m_TextBox;
        private object m_ToDestroy;

        public GDragAmount(Client.Item item) : base(0x85c, 0, 0)
        {
            GTextBox box;
            this.m_First = true;
            this.m_Item = item;
            int amount = (ushort) this.m_Item.Amount;
            this.m_Amount = amount;
            this.m_Okay = new GButtonNew(0x81a, 0x81c, 0x81b, 0x66, 0x25);
            this.m_Okay.CanEnter = true;
            this.m_Okay.Clicked += new EventHandler(this.Okay_Clicked);
            base.m_Children.Add(this.m_Okay);
            GSlider toAdd = new GSlider(0x845, 0x23, 0x10, 0x5f, 15, (double) amount, 0.0, (double) amount, 1.0) {
                OnValueChange = new OnValueChange(this.Slider_OnValueChange)
            };
            base.m_Children.Add(toAdd);
            this.m_Slider = toAdd;
            GHotspot hotspot = new GHotspot(0x1c, 0x10, 0x6d, 15, toAdd);
            base.m_Children.Add(hotspot);
            box = new GTextBox(0, false, 0x1a, 0x2b, 0x42, 15, amount.ToString(), Engine.GetFont(1), Hues.Load(0x455), Hues.Load(0x455), Hues.Load(0x455)) {
                OnTextChange = (OnTextChange) Delegate.Combine(box.OnTextChange, new OnTextChange(this.TextBox_OnTextChange)),
                OnBeforeTextChange = (OnBeforeTextChange) Delegate.Combine(box.OnBeforeTextChange, new OnBeforeTextChange(this.TextBox_OnBeforeTextChange)),
                EnterButton = this.m_Okay
            };
            base.m_Children.Add(box);
            this.m_TextBox = box;
            box.Focus();
            base.m_IsDragging = true;
            base.m_OffsetX = this.Width / 2;
            base.m_OffsetY = this.Height / 2;
            Gumps.LastOver = this;
            Gumps.Drag = this;
            Gumps.Focus = this;
            base.m_X = Engine.m_xMouse - base.m_OffsetX;
            base.m_Y = Engine.m_yMouse - base.m_OffsetY;
        }

        private void Okay_Clicked(object sender, EventArgs e)
        {
            try
            {
                int amount = Convert.ToInt32(this.m_TextBox.String);
                if (amount <= 0)
                {
                    Gumps.Destroy(this);
                }
                else
                {
                    if (amount > this.m_Amount)
                    {
                        amount = this.m_Amount;
                    }
                    base.m_IsDragging = false;
                    Network.Send(new PPickupItem(this.m_Item, (short) ((ushort) amount)));
                    this.m_Item.Amount = (short) ((ushort) amount);
                    Gumps.Desktop.Children.Add(new GDraggedItem(this.m_Item));
                    if (this.m_ToDestroy is Gump)
                    {
                        if (((Gump) this.m_ToDestroy).Parent is GContainer)
                        {
                            ((GContainer) ((Gump) this.m_ToDestroy).Parent).m_Hash[this.m_Item] = null;
                        }
                        Gumps.Destroy((Gump) this.m_ToDestroy);
                    }
                    else if (this.m_ToDestroy is Client.Item)
                    {
                        Client.Item toDestroy = (Client.Item) this.m_ToDestroy;
                        toDestroy.RestoreInfo = new RestoreInfo(toDestroy);
                        World.Remove(toDestroy);
                    }
                    Gumps.Destroy(this);
                }
            }
            catch
            {
            }
        }

        private void Slider_OnValueChange(double v, double old, Gump g)
        {
            this.m_TextBox.String = ((int) v).ToString();
        }

        private void TextBox_OnBeforeTextChange(Gump g)
        {
            if (this.m_First)
            {
                this.m_First = false;
                ((GTextBox) g).String = "";
            }
        }

        private void TextBox_OnTextChange(string text, Gump g)
        {
            try
            {
                int num = Convert.ToInt32(text);
                if (num < 0)
                {
                    this.m_Slider.SetValue(0.0, true);
                }
                else if (num > this.m_Amount)
                {
                    this.m_Slider.SetValue((double) this.m_Amount, true);
                }
                else
                {
                    this.m_Slider.SetValue((double) num, false);
                }
            }
            catch
            {
            }
        }

        public Client.Item Item
        {
            get
            {
                return this.m_Item;
            }
        }

        public object ToDestroy
        {
            get
            {
                return this.m_ToDestroy;
            }
            set
            {
                this.m_ToDestroy = value;
            }
        }
    }
}

