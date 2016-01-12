namespace Client
{
    using Microsoft.DirectX.Direct3D;

    public class GContextMenu : Gump
    {
        private int m_Height;
        private static GContextMenu m_Instance;
        private object m_Owner;
        private CustomVertex.TransformedColoredTextured[] m_VertexPool;
        private int m_Width;

        private GContextMenu(object owner, PopupEntry[] list) : base(100, 100)
        {
            this.m_VertexPool = VertexConstructor.Create();
            this.m_Owner = owner;
            base.m_GUID = "MobilePopup";
            int num = 0;
            int num2 = 0;
            int length = list.Length;
            IFont uniFont = Engine.GetUniFont(3);
            IHue bright = Hues.Bright;
            IHue focusHue = Hues.Load(0x35);
            IHue hue = Hues.Default;
            OnClick onClick = new OnClick(this.Entry_OnClick);
            for (int i = 0; i < length; i++)
            {
                PopupEntry entry = list[i];
                GLabel toAdd = null;
                if (entry.Flags == 1)
                {
                    toAdd = new GLabel(entry.Text, uniFont, hue, 7, 7 + num2);
                }
                else
                {
                    toAdd = new GTextButton(entry.Text, uniFont, bright, focusHue, 7, 7 + num2, onClick);
                    toAdd.SetTag("EntryID", entry.EntryID);
                }
                toAdd.X -= toAdd.Image.xMin;
                toAdd.Y -= toAdd.Image.yMin;
                num2 += (toAdd.Image.yMax - toAdd.Image.yMin) + 4;
                if (((toAdd.Image.xMax - toAdd.Image.xMin) + 1) > num)
                {
                    num = (toAdd.Image.xMax - toAdd.Image.xMin) + 1;
                }
                base.m_Children.Add(toAdd);
            }
            num2 -= 3;
            this.m_Width = num + 14;
            this.m_Height = num2 + 14;
        }

        public static void Close()
        {
            Gumps.Destroy(m_Instance);
        }

        public static void Display(object owner, PopupEntry[] list)
        {
            Gumps.Destroy(m_Instance);
            m_Instance = new GContextMenu(owner, list);
            Gumps.Desktop.Children.Add(m_Instance);
        }

        protected internal override void Draw(int X, int Y)
        {
            int xWidth = this.m_Width - 0x18;
            int yHeight = this.m_Height - 0x18;
            Renderer.AlphaTestEnable = false;
            Renderer.SetAlphaEnable(true);
            Renderer.SetAlpha(1f);
            Engine.m_Edge[0].Draw(X, Y, 0, this.m_VertexPool);
            Engine.m_Edge[1].Draw(X + 12, Y, xWidth, 12, 0);
            Engine.m_Edge[2].Draw((X + 12) + xWidth, Y, 0, this.m_VertexPool);
            Engine.m_Edge[3].Draw(X, Y + 12, 12, yHeight, 0);
            Engine.m_Edge[4].Draw((X + 12) + xWidth, Y + 12, 12, yHeight, 0);
            Engine.m_Edge[5].Draw(X, (Y + 12) + yHeight, 0, this.m_VertexPool);
            Engine.m_Edge[6].Draw(X + 12, (Y + 12) + yHeight, xWidth, 12, 0);
            Engine.m_Edge[7].Draw((X + 12) + xWidth, (Y + 12) + yHeight, 0, this.m_VertexPool);
            Renderer.SetAlpha(0.4f);
            Renderer.SetTexture(null);
            Renderer.SolidRect(0, X + 12, Y + 12, xWidth, yHeight);
            Renderer.SetAlphaEnable(false);
            Renderer.AlphaTestEnable = true;
        }

        private void Entry_OnClick(Gump Sender)
        {
            int tag = (int)Sender.GetTag("EntryID");
            Network.Send(new PPopupResponse(this.m_Owner, tag));
            Gumps.Destroy(this);
        }

        protected internal override bool HitTest(int X, int Y)
        {
            return false;
        }

        public override int Height
        {
            get
            {
                return this.m_Height;
            }
        }

        public override int Width
        {
            get
            {
                return this.m_Width;
            }
        }

        public override int X
        {
            get
            {
                int num;
                if (this.m_Owner is Mobile)
                {
                    num = ((Mobile)this.m_Owner).ScreenX - (this.m_Width / 2);
                }
                else
                {
                    num = ((Item)this.m_Owner).MessageX - (this.m_Width / 2);
                }
                if (num < 0)
                {
                    return 0;
                }
                if ((num + this.m_Width) > Engine.ScreenWidth)
                {
                    num = Engine.ScreenWidth - this.m_Width;
                }
                return num;
            }
        }

        public override int Y
        {
            get
            {
                int screenY;
                if (this.m_Owner is Mobile)
                {
                    Mobile owner = (Mobile)this.m_Owner;
                    screenY = owner.ScreenY;
                    if (((Renderer.MiniHealth && owner.OpenedStatus) && ((owner.StatusBar == null) && (owner.HPMax > 0))) && (owner.HPCur > 0))
                    {
                        screenY += 11;
                    }
                    screenY += 8;
                }
                else
                {
                    Item item = (Item)this.m_Owner;
                    screenY = item.BottomY + 4;
                }
                if (screenY < 0)
                {
                    return 0;
                }
                if ((screenY + this.m_Height) > Engine.ScreenHeight)
                {
                    screenY = Engine.ScreenHeight - this.m_Height;
                }
                return screenY;
            }
        }
    }
}