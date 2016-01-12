namespace Client
{
    using System;
    using System.Collections;

    public class GObjectProperties : GAlphaBackground
    {
        private int m_CompactHeight;
        private double m_HeightDuration;
        private static GObjectProperties m_Instance;
        private ObjectPropertyList m_List;
        private object m_Object;
        private TimeSync m_Sync;
        private Timer m_Timer;
        private int m_TotalHeight;
        private int m_TotalWidth;
        private double m_WidthDuration;
        public bool m_WorldTooltip;

        public GObjectProperties(int number, object o, ObjectPropertyList propList) : base(0, 0, 2, 20)
        {
            this.m_Object = o;
            base.m_CanDrag = false;
            this.m_Timer = new Timer(new OnTick(this.Roll_OnTick), 0);
            this.m_Timer.Start(false);
            int length = propList.Properties.Length;
            if ((length == 0) && (number != -1))
            {
                length = 1;
            }
            this.SetList(number, propList);
            this.m_WidthDuration = this.m_TotalWidth * 0.000625;
            this.m_HeightDuration = length * 0.0125;
            this.m_Sync = new TimeSync(this.m_WidthDuration + this.m_HeightDuration);
        }

        public static void Display(object o)
        {
            Hide();
            if (o is Item)
            {
                Item item = (Item)o;
                ObjectPropertyList propertyList = item.PropertyList;
                if (propertyList != null)
                {
                    m_Instance = new GObjectProperties(0xf9060 + item.ID, o, propertyList);
                }
            }
            else if (o is Mobile)
            {
                Mobile mobile = (Mobile)o;
                ObjectPropertyList propList = mobile.PropertyList;
                if (propList != null)
                {
                    m_Instance = new GObjectProperties(-1, mobile, propList);
                }
            }
            if (m_Instance != null)
            {
                Gumps.Desktop.Children.Add(m_Instance);
                m_Instance.m_WorldTooltip = true;
            }
        }

        private IHue GetDefaultHue()
        {
            if (this.m_Object is Mobile)
            {
                return Hues.GetNotoriety(((Mobile)this.m_Object).Notoriety);
            }
            return Hues.Load(0x34);
        }

        public static void Hide()
        {
            if (m_Instance != null)
            {
                Gumps.Destroy(m_Instance);
            }
            m_Instance = null;
        }

        protected internal override bool HitTest(int X, int Y)
        {
            return false;
        }

        protected internal override void Render(int X, int Y)
        {
            if (this.m_Object is Item)
            {
                Item item = (Item)this.m_Object;
                if (item.PropertyList == null)
                {
                    item.QueryProperties();
                }
                else if (item.PropertyList != this.m_List)
                {
                    this.SetList(0xf9060 + (item.ID & 0x3fff), item.PropertyList);
                }
            }
            if (this.m_WorldTooltip)
            {
                bool flag = Engine.m_xMouse < (Engine.ScreenWidth / 2);
                bool flag2 = Engine.m_yMouse < (Engine.ScreenHeight / 2);
                int xMouse = (Engine.m_xMouse - this.Width) - 2;
                int yMouse = (Engine.m_yMouse - this.Height) - 2;
                if (flag)
                {
                    if (flag2)
                    {
                        xMouse = (Engine.m_xMouse + Cursor.Width) + 2;
                    }
                    else
                    {
                        xMouse = Engine.m_xMouse;
                    }
                }
                if (flag2)
                {
                    if (flag)
                    {
                        yMouse = (Engine.m_yMouse + Cursor.Height) + 2;
                    }
                    else
                    {
                        yMouse = Engine.m_yMouse;
                    }
                }
                if (xMouse < 2)
                {
                    xMouse = 2;
                }
                else if (((xMouse + this.Width) + 2) > Engine.ScreenWidth)
                {
                    xMouse = (Engine.ScreenWidth - this.Width) - 2;
                }
                if (yMouse < 2)
                {
                    yMouse = 2;
                }
                else if (((yMouse + this.Height) + 2) > Engine.ScreenHeight)
                {
                    yMouse = (Engine.ScreenHeight - this.Height) - 2;
                }
                this.X = xMouse;
                this.Y = yMouse;
            }
            base.Render(X, Y);
        }

        protected void Roll_OnTick(Timer t)
        {
            double num;
            double elapsed = this.m_Sync.Elapsed;
            if (elapsed < this.m_WidthDuration)
            {
                num = elapsed / this.m_WidthDuration;
                this.Width = 2 + ((int)(num * (this.m_TotalWidth - 2)));
                this.Height = this.m_CompactHeight;
            }
            else
            {
                num = (elapsed - this.m_WidthDuration) / this.m_HeightDuration;
                if (num >= 1.0)
                {
                    if (this.m_Timer != null)
                    {
                        this.m_Timer.Stop();
                    }
                    this.m_Timer = null;
                    this.Width = this.m_TotalWidth;
                    this.Height = this.m_TotalHeight;
                }
                else
                {
                    this.Width = this.m_TotalWidth;
                    this.Height = this.m_CompactHeight + ((int)(num * (this.m_TotalHeight - this.m_CompactHeight)));
                }
            }
            Engine.Redraw();
        }

        public void SetList(int number, ObjectPropertyList propList)
        {
            this.m_List = propList;
            base.m_Children.Clear();
            int y = 5;
            int num2 = 10;
            ObjectProperty[] properties = propList.Properties;
            if ((properties.Length == 0) && (number != -1))
            {
                properties = new ObjectProperty[] { new ObjectProperty(number, "") };
            }
            int num3 = 0;
            int index = -1;
            int num5 = 0;
            int num6 = 0;
            int num7 = 0;
            int num8 = 0;
            int num9 = 0;
            int num10 = 0;
            int num11 = 0;
            for (int i = 0; i < properties.Length; i++)
            {
                if ((properties[i].Number >= 0x102e5d) && (properties[i].Number <= 0x102e61))
                {
                    int num13 = 0;
                    try
                    {
                        num13 = int.Parse(properties[i].Arguments.Trim());
                    }
                    catch
                    {
                    }
                    switch (properties[i].Number)
                    {
                        case 0x102e5d:
                            num7 += num13;
                            break;

                        case 0x102e5e:
                            num9 += num13;
                            break;

                        case 0x102e5f:
                            num6 += num13;
                            break;

                        case 0x102e60:
                            num5 += num13;
                            break;

                        case 0x102e61:
                            num8 += num13;
                            break;
                    }
                    if (index == -1)
                    {
                        index = i;
                    }
                    num3 += num13;
                    continue;
                }
                if (properties[i].Number == 0x102e86)
                {
                    try
                    {
                        num11 = int.Parse(properties[i].Arguments.Trim());
                    }
                    catch
                    {
                    }
                }
                else if (properties[i].Number == 0x10312f)
                {
                    try
                    {
                        num10 = int.Parse(properties[i].Arguments.Trim());
                    }
                    catch
                    {
                    }
                }
            }
            if (num10 > 0)
            {
                num10 *= 100 + num11;
                num10 /= 100;
            }
            ResistInfo info = null;
            ResistInfo info2 = null;
            if (index != -1)
            {
                ArrayList list = new ArrayList(properties);
                list.Insert(index, new ObjectProperty(0xfea1b, string.Format("total resist {0}%", num3)));
                properties = (ObjectProperty[])list.ToArray(typeof(ObjectProperty));
                info = ResistInfo.Find(properties[0].Text, ResistInfo.m_Armor);
                info2 = ResistInfo.Find(properties[0].Text, ResistInfo.m_Materials);
            }
            int num14 = -1;
            if (this.m_Object is Mobile)
            {
                Mobile mobile = (Mobile)this.m_Object;
                ArrayList equip = mobile.Equip;
                int num15 = 0;
                int num16 = 0;
                int num17 = 0;
                int num18 = 0;
                int num19 = 0;
                int num20 = 0;
                int num21 = 0;
                for (int k = 0; k < equip.Count; k++)
                {
                    EquipEntry entry = (EquipEntry)equip[k];
                    if (entry.m_Item != null)
                    {
                        Item item = entry.m_Item;
                        ObjectPropertyList propertyList = item.PropertyList;
                        if (propertyList == null)
                        {
                            item.QueryProperties();
                        }
                        else
                        {
                            foreach (ObjectProperty property in propertyList.Properties)
                            {
                                if ((property.Number >= 0x102e5d) && (property.Number <= 0x102e61))
                                {
                                    int num24 = 0;
                                    try
                                    {
                                        num24 = int.Parse(property.Arguments.Trim());
                                    }
                                    catch
                                    {
                                    }
                                    switch (property.Number)
                                    {
                                        case 0x102e5d:
                                            num17 += num24;
                                            break;

                                        case 0x102e5e:
                                            num19 += num24;
                                            break;

                                        case 0x102e5f:
                                            num16 += num24;
                                            break;

                                        case 0x102e60:
                                            num15 += num24;
                                            break;

                                        case 0x102e61:
                                            num18 += num24;
                                            break;
                                    }
                                }
                                else if (property.Number == 0x102e3d)
                                {
                                    try
                                    {
                                        num20 += int.Parse(property.Arguments.Trim());
                                    }
                                    catch
                                    {
                                    }
                                }
                                else if (property.Number == 0x102e3c)
                                {
                                    try
                                    {
                                        num21 += int.Parse(property.Arguments.Trim());
                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                        }
                    }
                }
                num14 = num15;
                if (num16 < num14)
                {
                    num14 = num16;
                }
                if (num17 < num14)
                {
                    num14 = num17;
                }
                if (num18 < num14)
                {
                    num14 = num18;
                }
                if (num19 < num14)
                {
                    num14 = num19;
                }
                if ((((num15 != 0) || (num16 != 0)) || ((num17 != 0) || (num18 != 0))) || (((num19 != 0) || (num20 != 0)) || (num21 != 0)))
                {
                    ArrayList list4 = new ArrayList(properties);
                    if ((num20 != 0) || (num21 != 0))
                    {
                        list4.Add(new ObjectProperty(0xfea1b, string.Format("FC {0}, FCR {1}", num20, num21)));
                    }
                    list4.Add(new ObjectProperty(0x102e60, num15.ToString()));
                    list4.Add(new ObjectProperty(0x102e5f, num16.ToString()));
                    list4.Add(new ObjectProperty(0x102e5d, num17.ToString()));
                    list4.Add(new ObjectProperty(0x102e61, num18.ToString()));
                    list4.Add(new ObjectProperty(0x102e5e, num19.ToString()));
                    properties = (ObjectProperty[])list4.ToArray(typeof(ObjectProperty));
                }
            }
            if (((this.m_Object is Item) && (((Item)this.m_Object).Container is GContainer)) && ((GContainer)((Item)this.m_Object).Container).m_TradeContainer)
            {
                ArrayList list5 = new ArrayList(properties);
                if (list5.Count > 0)
                {
                    list5[0] = new ObjectProperty(0xfea1b, "Their Offer");
                }
                Item item2 = (Item)this.m_Object;
                Item[] itemArray = item2.FindItems(new ItemIDValidator(new int[] { 0xeed, 0xeec, 0xeee }));
                int num25 = 0;
                for (int m = 0; m < itemArray.Length; m++)
                {
                    num25 += itemArray[m].Amount;
                }
                Item[] itemArray2 = item2.FindItems(new ItemIDValidator(new int[] { 0x14ef, 0x14f0 }));
                for (int n = 0; n < itemArray2.Length; n++)
                {
                    ObjectPropertyList list6 = itemArray2[n].PropertyList;
                    if (list6 == null)
                    {
                        itemArray2[n].QueryProperties();
                    }
                    else
                    {
                        bool flag = false;
                        for (int num28 = 0; num28 < list6.Properties.Length; num28++)
                        {
                            if (list6.Properties[num28].Number == 0xfe3d1)
                            {
                                flag = true;
                            }
                            else if ((list6.Properties[num28].Number == 0x102f82) && flag)
                            {
                                try
                                {
                                    num25 += int.Parse(list6.Properties[num28].Arguments.Trim());
                                }
                                catch
                                {
                                }
                                break;
                            }
                        }
                    }
                }
                list5.Add(new ObjectProperty(0xfea1b, string.Format("Total Gold: {0:N0}", num25)));
                properties = (ObjectProperty[])list5.ToArray(typeof(ObjectProperty));
            }
            for (int j = 0; j < properties.Length; j++)
            {
                ObjectProperty property2 = properties[j];
                GLabel toAdd = new GLabel(Engine.MakeProperCase(property2.Text), Engine.DefaultFont, (j == 0) ? this.GetDefaultHue() : Hues.Bright, 5, y);
                if (property2.Number == 0x103132)
                {
                    int num30 = 0;
                    try
                    {
                        num30 = int.Parse(property2.Arguments.Trim());
                    }
                    catch
                    {
                    }
                    Mobile player = World.Player;
                    if ((player != null) && (num30 > player.Str))
                    {
                        toAdd.Hue = Hues.Load(0x22);
                    }
                }
                else if (property2.Number == 0x103332)
                {
                    toAdd.Hue = Hues.Load(0x59);
                    toAdd.Text = "Insured";
                }
                else if (((num14 >= 0) && (property2.Number >= 0x102e5d)) && (property2.Number <= 0x102e61))
                {
                    int num31 = 0;
                    try
                    {
                        num31 = int.Parse(property2.Arguments.Trim());
                    }
                    catch
                    {
                    }
                    if (num31 == num14)
                    {
                        if (property2.Number == 0x102e5d)
                        {
                            toAdd.Hue = Hues.Load(0x5f);
                        }
                        else if (property2.Number == 0x102e5e)
                        {
                            toAdd.Hue = Hues.Load(0x1a);
                        }
                        else if (property2.Number == 0x102e5f)
                        {
                            toAdd.Hue = Hues.Load(0x2d);
                        }
                        else if (property2.Number == 0x102e60)
                        {
                            toAdd.Hue = Hues.Load(0x482);
                        }
                        else if (property2.Number == 0x102e61)
                        {
                            toAdd.Hue = Hues.Load(70);
                        }
                    }
                }
                else if (((info != null) && (property2.Number >= 0x102e5d)) && (property2.Number <= 0x102e61))
                {
                    int num32 = 0;
                    try
                    {
                        num32 = int.Parse(property2.Arguments.Trim());
                    }
                    catch
                    {
                    }
                    int num33 = 0;
                    switch (property2.Number)
                    {
                        case 0x102e5d:
                            num33 = info.m_Cold + ((info2 == null) ? 0 : info2.m_Cold);
                            break;

                        case 0x102e5e:
                            num33 = info.m_Energy + ((info2 == null) ? 0 : info2.m_Energy);
                            break;

                        case 0x102e5f:
                            num33 = info.m_Fire + ((info2 == null) ? 0 : info2.m_Fire);
                            break;

                        case 0x102e60:
                            num33 = info.m_Physical + ((info2 == null) ? 0 : info2.m_Physical);
                            break;

                        case 0x102e61:
                            num33 = info.m_Poison + ((info2 == null) ? 0 : info2.m_Poison);
                            break;
                    }
                    int num34 = num32 - num33;
                    if (num34 > 0)
                    {
                        toAdd.Text = toAdd.Text + string.Format(" (+{0}%)", num34);
                    }
                    else if (num34 < 0)
                    {
                        toAdd.Text = toAdd.Text + string.Format(" (-{0}%)", Math.Abs(num34));
                    }
                }
                else if (property2.Number == 0x10312f)
                {
                    Mobile mobile3 = World.Player;
                    if (mobile3 != null)
                    {
                        double num35 = Math.Floor((double)(40000.0 / ((double)((mobile3.StamCur + 100) * num10)))) / 2.0;
                        toAdd.Text = toAdd.Text + string.Format(" ({0:F1}s)", num35);
                    }
                }
                toAdd.Y -= toAdd.Image.yMin;
                toAdd.X -= toAdd.Image.xMin;
                y = (toAdd.Y + toAdd.Image.yMax) + 5;
                if ((10 + ((toAdd.Image.xMax - toAdd.Image.xMin) + 1)) > num2)
                {
                    num2 = 10 + ((toAdd.Image.xMax - toAdd.Image.xMin) + 1);
                }
                base.m_Children.Add(toAdd);
                if (j == 0)
                {
                    this.m_CompactHeight = y;
                }
            }
            this.m_TotalWidth = num2;
            this.m_TotalHeight = y;
            if (this.m_Timer == null)
            {
                this.Width = num2;
                this.Height = y;
            }
            foreach (GLabel label2 in base.m_Children.ToArray())
            {
                label2.X = ((num2 - ((label2.Image.xMax - label2.Image.xMin) + 1)) / 2) - label2.Image.xMin;
                label2.Scissor(1 - label2.X, 1 - label2.Y, this.Width - 2, this.Height - 2);
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
                foreach (GLabel label in base.m_Children.ToArray())
                {
                    label.Scissor(1 - label.X, 1 - label.Y, this.Width - 2, this.Height - 2);
                }
            }
        }

        public static GObjectProperties Instance
        {
            get
            {
                return m_Instance;
            }
        }

        public object Object
        {
            get
            {
                return this.m_Object;
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
                foreach (GLabel label in base.m_Children.ToArray())
                {
                    label.Scissor(1 - label.X, 1 - label.Y, this.Width - 2, this.Height - 2);
                }
            }
        }
    }
}