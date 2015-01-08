namespace Client
{
    using System;
    using System.Windows.Forms;

    public class GPaperdoll : GDragable, IRestorableGump
    {
        private int m_Gender;
        private GImage m_Image;
        private Mobile m_Mobile;

        public GPaperdoll(Mobile m, int ID, int X, int Y) : base(ID, X, Y)
        {
            this.m_Mobile = m;
            base.m_CanDrop = true;
        }

        protected internal override void OnDispose()
        {
            this.m_Mobile.Paperdoll = null;
        }

        protected internal override void OnDragDrop(Gump g)
        {
            if ((g != null) && (g.GetType() == typeof(GDraggedItem)))
            {
                GDraggedItem item = (GDraggedItem) g;
                Item toEquip = item.Item;
                Item item3 = null;
                Gump[] gumpArray = base.m_Children.ToArray();
                Point point = base.PointToClient(new Point(Engine.m_xMouse, Engine.m_yMouse));
                for (int i = gumpArray.Length - 1; i >= 0; i--)
                {
                    if ((gumpArray[i] is GPaperdollItem) && gumpArray[i].HitTest(point.X - gumpArray[i].X, point.Y - gumpArray[i].Y))
                    {
                        item3 = ((GPaperdollItem) gumpArray[i]).Item;
                        break;
                    }
                }
                if ((item3 != null) && Map.m_ItemFlags[item3.ID & 0x3fff][TileFlag.Container])
                {
                    Network.Send(new PDropItem(toEquip.Serial, -1, -1, 0, item3.Serial));
                }
                else if (Map.m_ItemFlags[toEquip.ID & 0x3fff][TileFlag.Wearable])
                {
                    Network.Send(new PEquipItem(toEquip, this.m_Mobile));
                }
                else
                {
                    Network.Send(new PDropItem(toEquip.Serial, -1, -1, 0, World.Serial));
                }
                Gumps.Destroy(item);
            }
            if (this.m_Image != null)
            {
                Gumps.Destroy(this.m_Image);
                this.m_Image = null;
            }
        }

        protected internal override void OnDragEnter(Gump g)
        {
            if ((g != null) && (g.GetType() == typeof(GDraggedItem)))
            {
                GDraggedItem item = (GDraggedItem) g;
                Item item2 = item.Item;
                int iD = item2.ID;
                int hue = item2.Hue;
                Engine.ItemArt.Translate(ref iD, ref hue);
                if (Map.m_ItemFlags[iD][TileFlag.Wearable])
                {
                    if (this.m_Image != null)
                    {
                        Gumps.Destroy(this.m_Image);
                    }
                    this.m_Image = new GImage(Gumps.GetEquipGumpID(iD, this.m_Gender, ref hue), Hues.GetItemHue(iD, hue), 8, 0x13);
                    this.m_Image.Alpha = 0.5f;
                    int count = base.m_Children.Count;
                    LayerComparer paperdoll = LayerComparer.Paperdoll;
                    int num4 = paperdoll.GetValue(iD, (Layer) Map.GetQuality(iD));
                    for (int i = 0; i < base.m_Children.Count; i++)
                    {
                        Gump gump = base.m_Children[i];
                        if (gump.GetType() == typeof(GPaperdollItem))
                        {
                            GPaperdollItem item3 = (GPaperdollItem) gump;
                            if (paperdoll.GetValue(item3.Item.ID, (Layer) ((byte) item3.Layer)) < num4)
                            {
                                count = i;
                                break;
                            }
                        }
                    }
                    base.m_Children.Insert(count, this.m_Image);
                }
            }
        }

        protected internal override void OnDragLeave(Gump g)
        {
            if (this.m_Image != null)
            {
                Gumps.Destroy(this.m_Image);
                this.m_Image = null;
            }
        }

        protected internal override void OnMouseDown(int x, int y, MouseButtons mb)
        {
            base.BringToTop();
        }

        public int Extra
        {
            get
            {
                return this.m_Mobile.Serial;
            }
        }

        public int Gender
        {
            get
            {
                return this.m_Gender;
            }
            set
            {
                this.m_Gender = value;
            }
        }

        public int Type
        {
            get
            {
                return 2;
            }
        }
    }
}

