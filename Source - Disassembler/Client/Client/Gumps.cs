namespace Client
{
    using System;
    using System.Collections;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public sealed class Gumps
    {
        private byte[] m_Buffer;
        private static Gump m_Capture;
        private static Gump m_Desktop = new Gump(0, 0);
        private static Gump m_Drag;
        private GumpFactory m_Factory;
        private static Gump m_Focus;
        public Entry3D[] m_Index;
        private static bool m_Invalidated;
        private static Gump m_LastDragOver;
        private static Gump m_LastOver;
        private static Gump m_Modal;
        private Hashtable m_Objects;
        private static Gump m_StartDrag;
        private static Point m_StartDragPoint;
        private Stream m_Stream;
        private static GTextBox m_TextFocus;
        private static TimeDelay m_TipDelay;
        private static Hashtable m_ToRestore;
        private Stream m_Verdata;
        private const short Opaque = -32768;

        public unsafe Gumps()
        {
            m_Desktop.GUID = "Desktop";
            m_ToRestore = new Hashtable();
            this.m_Stream = Engine.FileManager.OpenMUL(Files.GumpMul);
            this.m_Verdata = Engine.FileManager.OpenMUL(Files.Verdata);
            string path = "Data/QuickLoad/Gumps.mul";
            string str2 = Engine.FileManager.BasePath(path);
            FileInfo info = new FileInfo(Engine.FileManager.ResolveMUL(Files.GumpMul));
            FileInfo info2 = new FileInfo(Engine.FileManager.ResolveMUL(Files.GumpIdx));
            FileInfo info3 = new FileInfo(Engine.FileManager.ResolveMUL(Files.Verdata));
            if (File.Exists(str2))
            {
                BinaryReader reader = new BinaryReader(Engine.FileManager.OpenBaseMUL(path));
                DateTime time = DateTime.FromFileTime(reader.ReadInt64());
                DateTime time2 = DateTime.FromFileTime(reader.ReadInt64());
                DateTime time3 = DateTime.FromFileTime(reader.ReadInt64());
                if (((info.LastWriteTime == time) && (info2.LastWriteTime == time2)) && (info3.LastWriteTime == time3))
                {
                    int num = reader.ReadInt32();
                    this.m_Index = new Entry3D[num];
                    fixed (Entry3D* entrydRef = this.m_Index)
                    {
                        Engine.NativeRead((FileStream) reader.BaseStream, (void*) entrydRef, num * 12);
                    }
                    reader.Close();
                    return;
                }
                reader.Close();
            }
            FileStream fs = new FileStream(Engine.FileManager.ResolveMUL(Files.GumpIdx), FileMode.Open, FileAccess.Read, FileShare.Read);
            int num2 = (int) (fs.Length / 12L);
            int num3 = num2;
            this.m_Index = new Entry3D[num2];
            fixed (Entry3D* entrydRef2 = this.m_Index)
            {
                Engine.NativeRead(fs, (void*) entrydRef2, num2 * 12);
            }
            fs.Close();
            BinaryReader reader2 = new BinaryReader(Engine.FileManager.OpenMUL(Files.Verdata));
            num2 = reader2.ReadInt32();
            for (int i = 0; i < num2; i++)
            {
                int num5 = reader2.ReadInt32();
                int index = reader2.ReadInt32();
                int num7 = reader2.ReadInt32();
                int num8 = reader2.ReadInt32();
                int num9 = reader2.ReadInt32();
                if (num5 == 12)
                {
                    this.m_Index[index].m_Lookup = num7;
                    this.m_Index[index].m_Length = num8 | -2147483648;
                    this.m_Index[index].m_Extra = num9;
                }
            }
            reader2.Close();
            BinaryWriter writer = new BinaryWriter(Engine.FileManager.CreateBaseMUL(str2));
            writer.Write(info.LastWriteTime.ToFileTime());
            writer.Write(info2.LastWriteTime.ToFileTime());
            writer.Write(info3.LastWriteTime.ToFileTime());
            writer.Write(num3);
            fixed (Entry3D* entrydRef3 = this.m_Index)
            {
                Engine.NativeWrite((FileStream) writer.BaseStream, (void*) entrydRef3, num3 * 12);
            }
            writer.Flush();
            writer.Close();
        }

        public static bool Check(ref int gumpID, ref int hue)
        {
            if (Engine.m_Gumps.m_Index[gumpID].m_Lookup != -1)
            {
                return true;
            }
            for (int i = 0; i < GumpTable.m_Entries.Length; i++)
            {
                GumpTableEntry entry = GumpTable.m_Entries[i];
                if (entry.m_NewID == gumpID)
                {
                    gumpID = entry.m_OldID;
                    if (hue == 0)
                    {
                        hue = entry.m_NewHue;
                    }
                    return true;
                }
            }
            return false;
        }

        public static void Destroy(Gump g)
        {
            if (g != null)
            {
                m_Invalidated = true;
                g.Children.Clear();
                if (g == m_Drag)
                {
                    m_Drag = null;
                }
                if (g == m_Capture)
                {
                    m_Capture = null;
                }
                if (g == m_Focus)
                {
                    m_Focus = null;
                }
                if (g == m_Modal)
                {
                    m_Modal = null;
                }
                if (g == m_LastDragOver)
                {
                    m_LastDragOver = null;
                }
                if (g == m_StartDrag)
                {
                    m_StartDrag = null;
                }
                if (g == m_LastOver)
                {
                    m_LastOver = null;
                }
                if (g == m_TextFocus)
                {
                    m_TextFocus = null;
                }
                if ((g.m_Restore && (g.GUID != null)) && (g.GUID.Length > 0))
                {
                    m_ToRestore[g.GUID] = new Point(g.X, g.Y);
                }
                if (g.HasTag("Dispose"))
                {
                    switch (((string) g.GetTag("Dispose")))
                    {
                        case "Spellbook":
                        {
                            Item tag = (Item) g.GetTag("Container");
                            if (tag != null)
                            {
                                tag.OpenSB = false;
                            }
                            break;
                        }
                        case "Modal":
                            m_Modal = null;
                            break;
                    }
                }
                g.m_Disposed = true;
                g.OnDispose();
                if (g.Parent != null)
                {
                    g.Parent.Children.Remove(g);
                }
            }
        }

        public void DisplayObject(string Name)
        {
            m_Desktop.Children.Add((Gump) this.m_Objects[Name]);
        }

        public void Dispose()
        {
            Stack stack = new Stack();
            stack.Push(m_Desktop);
            while (stack.Count > 0)
            {
                Gump gump = (Gump) stack.Pop();
                if (gump != null)
                {
                    GumpList children = gump.Children;
                    if (children != null)
                    {
                        Gump[] gumpArray = children.ToArray();
                        for (int i = 0; i < gumpArray.Length; i++)
                        {
                            stack.Push(gumpArray[i]);
                        }
                    }
                    try
                    {
                        gump.OnDispose();
                        continue;
                    }
                    catch (Exception exception)
                    {
                        Debug.Trace("Exception in {0}.OnDispose()", gump);
                        Debug.Error(exception);
                        continue;
                    }
                }
            }
            m_Desktop = null;
            this.m_Stream.Close();
            this.m_Stream = null;
            this.m_Verdata.Close();
            this.m_Verdata = null;
            this.m_Buffer = null;
            m_Drag = null;
            m_Capture = null;
            m_Focus = null;
            m_Modal = null;
            m_LastDragOver = null;
            m_StartDrag = null;
            m_LastOver = null;
            m_TextFocus = null;
            m_TipDelay = null;
            this.m_Index = null;
            if (this.m_Objects != null)
            {
                this.m_Objects.Clear();
                this.m_Objects = null;
            }
            if (m_ToRestore != null)
            {
                m_ToRestore.Clear();
                m_ToRestore = null;
            }
        }

        public static bool DoubleClick(int X, int Y)
        {
            if (m_Capture != null)
            {
                Point point = m_Capture.PointToClient(new Point(X, Y));
                m_Capture.OnDoubleClick(point.X, point.Y);
                return true;
            }
            if ((m_Desktop == null) || (m_Desktop.Children.Count == 0))
            {
                return false;
            }
            return (RecurseDoubleClick(0, 0, m_Desktop, X, Y) || (m_Modal != null));
        }

        public static void Draw()
        {
            if (m_Desktop != null)
            {
                m_Desktop.Render(0, 0);
                if ((((m_LastOver != null) && (m_LastOver.Tooltip != null)) && ((m_TipDelay != null) && m_TipDelay.Elapsed)) && Cursor.Visible)
                {
                    Gump gump = m_LastOver.Tooltip.GetGump();
                    if (gump != null)
                    {
                        bool flag = Engine.m_xMouse < (Engine.ScreenWidth / 2);
                        bool flag2 = Engine.m_yMouse < (Engine.ScreenHeight / 2);
                        int x = (Engine.m_xMouse - gump.Width) - 2;
                        int y = (Engine.m_yMouse - gump.Height) - 2;
                        if (flag)
                        {
                            if (flag2)
                            {
                                x = (Engine.m_xMouse + Cursor.Width) + 2;
                            }
                            else
                            {
                                x = Engine.m_xMouse;
                            }
                        }
                        if (flag2)
                        {
                            if (flag)
                            {
                                y = (Engine.m_yMouse + Cursor.Height) + 2;
                            }
                            else
                            {
                                y = Engine.m_yMouse;
                            }
                        }
                        if (x < 2)
                        {
                            x = 2;
                        }
                        else if (((x + gump.Width) + 2) > Engine.ScreenWidth)
                        {
                            x = (Engine.ScreenWidth - gump.Width) - 2;
                        }
                        if (y < 2)
                        {
                            y = 2;
                        }
                        else if (((y + gump.Height) + 2) > Engine.ScreenHeight)
                        {
                            y = (Engine.ScreenHeight - gump.Height) - 2;
                        }
                        gump.Render(x, y);
                    }
                }
            }
        }

        public static Gump FindGumpByGUID(string GUID)
        {
            Stack stack = new Stack();
            stack.Push(m_Desktop);
            while (stack.Count > 0)
            {
                Gump gump = (Gump) stack.Pop();
                if (gump.GUID == GUID)
                {
                    return gump;
                }
                Gump[] gumpArray = gump.Children.ToArray();
                for (int i = 0; i < gumpArray.Length; i++)
                {
                    stack.Push(gumpArray[i]);
                }
            }
            return null;
        }

        public static object[] FindListForSingleClick(int x, int y)
        {
            if (m_Capture != null)
            {
                Point point = m_Capture.PointToClient(new Point(x, y));
                return new object[] { m_Capture, point };
            }
            if ((m_Desktop == null) || (m_Desktop.Children.Count == 0))
            {
                return null;
            }
            return RecurseFindListForSingleClick(0, 0, m_Desktop, x, y);
        }

        public static int GetEquipGumpID(int itemID, int gender, ref int hue)
        {
            int num2;
            int animation = Map.GetAnimation(itemID);
            if (gender == 0)
            {
                num2 = animation + 0xc350;
                if (Check(ref num2, ref hue))
                {
                    return num2;
                }
                num2 += 0x2710;
                if (Check(ref num2, ref hue))
                {
                    return num2;
                }
            }
            else
            {
                num2 = animation + 0xea60;
                if (Check(ref num2, ref hue))
                {
                    return num2;
                }
                num2 -= 0x2710;
                if (Check(ref num2, ref hue))
                {
                    return num2;
                }
            }
            Engine.ItemArt.ForceTranslate(ref itemID);
            animation = Map.GetAnimation(itemID);
            if (gender == 0)
            {
                num2 = animation + 0xc350;
                if (Check(ref num2, ref hue))
                {
                    return num2;
                }
                num2 += 0x2710;
                if (Check(ref num2, ref hue))
                {
                    return num2;
                }
            }
            else
            {
                num2 = animation + 0xea60;
                if (Check(ref num2, ref hue))
                {
                    return num2;
                }
                num2 -= 0x2710;
                if (Check(ref num2, ref hue))
                {
                    return num2;
                }
            }
            return 0;
        }

        public static void Invalidate()
        {
            m_Invalidated = true;
        }

        public static bool IsWorldAt(int X, int Y)
        {
            return ((m_Modal != null) || !RecurseIsWorldAt(0, 0, m_Desktop, X, Y, false));
        }

        public static bool IsWorldAt(int X, int Y, bool CheckDrag)
        {
            return ((m_Modal != null) || !RecurseIsWorldAt(0, 0, m_Desktop, X, Y, CheckDrag));
        }

        public static bool KeyDown(char c)
        {
            if (m_Modal != null)
            {
                if (((m_TextFocus == null) || !m_TextFocus.IsChildOf(m_Modal)) || !m_TextFocus.OnKeyDown(c))
                {
                    if ((m_Focus != null) && m_Focus.IsChildOf(m_Modal))
                    {
                        if (!RecurseKeyDown(m_Focus, c))
                        {
                            RecurseKeyDown(m_Focus.Parent, c);
                        }
                    }
                    else
                    {
                        RecurseKeyDown(m_Modal, c);
                    }
                }
                return true;
            }
            if ((m_TextFocus != null) && m_TextFocus.OnKeyDown(c))
            {
                return true;
            }
            Gump focus = m_Focus;
            while (focus != null)
            {
                if (!RecurseKeyDown(focus, c))
                {
                    focus = focus.Parent;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }

        private static void LinkDocked(GDragable g)
        {
            if (g != null)
            {
                ArrayList dockers = g.Dockers;
                if (dockers.Count != 0)
                {
                    int count = dockers.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Point point = new Point((Point) dockers[i], g.X, g.Y);
                        int num3 = m_Desktop.Children.Count;
                        for (int j = 0; j < num3; j++)
                        {
                            if (((m_Desktop.Children[j] != null) && (m_Desktop.Children[j] != g)) && (m_Desktop.Children[j].GetType() == typeof(GDragable)))
                            {
                                ArrayList list2 = ((GDragable) m_Desktop.Children[j]).Dockers;
                                Point[] pointArray = (Point[]) list2.ToArray(typeof(Point));
                                int num5 = list2.Count;
                                for (int k = 0; k < num5; k++)
                                {
                                    pointArray[k] = new Point(pointArray[k], m_Desktop.Children[j].X, m_Desktop.Children[j].Y);
                                    if (point == pointArray[k])
                                    {
                                        if (((GDragable) m_Desktop.Children[j]).Link(g, k, i))
                                        {
                                            LinkDocked((GDragable) m_Desktop.Children[j]);
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public Size Measure(int gumpID)
        {
            int extra;
            if (gumpID > 0)
            {
                extra = this.m_Index[gumpID].m_Extra;
            }
            else
            {
                extra = 0;
            }
            return new Size((extra >> 0x10) & 0xffff, extra & 0xffff);
        }

        public static void MessageBoxOk(string Prompt, bool Modal, OnClick ClickHandler)
        {
            GBackground background = new GBackground(0xa2c, 0x164, 0xd4, 0x8e, 0x86, true);
            GButton toAdd = new GButton(0x481, 0xa4, 170, new OnClick(Gumps.MessageBoxOk_OnClick));
            toAdd.SetTag("Dialog", background);
            toAdd.SetTag("ClickHandler", ClickHandler);
            GWrappedLabel label = new GWrappedLabel(Prompt, Engine.GetFont(1), Hues.Load(0x76b), background.OffsetX, background.OffsetY, (Engine.ScreenWidth / 2) - (background.OffsetX * 2));
            background.Width = label.Width + (background.OffsetX * 2);
            background.Height = (label.Height + 10) + (background.OffsetY * 2);
            if (background.Width < 150)
            {
                background.Width = 150;
            }
            background.Center();
            toAdd.X = (background.Width - toAdd.Width) / 2;
            toAdd.Y = background.Height - background.OffsetY;
            background.Children.Add(label);
            background.Children.Add(toAdd);
            if (Modal)
            {
                background.Modal = true;
            }
            background.m_CanDrag = true;
            background.m_QuickDrag = true;
            m_Desktop.Children.Add(background);
        }

        public static void MessageBoxOk_OnClick(Gump Sender)
        {
            Gump tag = (Gump) Sender.GetTag("Dialog");
            OnClick click = (OnClick) Sender.GetTag("ClickHandler");
            if (click != null)
            {
                click(Sender);
            }
            if (tag != null)
            {
                Destroy(tag);
            }
        }

        public static bool MouseDown(int X, int Y, MouseButtons mb)
        {
            if (m_Capture != null)
            {
                Point point = m_Capture.PointToClient(new Point(X, Y));
                m_Capture.OnMouseDown(point.X, point.Y, mb);
                Focus = m_Capture;
                return true;
            }
            if ((m_Desktop != null) && (m_Desktop.Children.Count != 0))
            {
                if (RecurseMouseDown(0, 0, m_Desktop, X, Y, mb))
                {
                    return true;
                }
                if (m_Modal != null)
                {
                    Focus = m_Modal;
                    return true;
                }
                if (m_Drag != null)
                {
                    Focus = m_Drag;
                    return !IsWorldAt(X, Y, false);
                }
                Focus = null;
            }
            return false;
        }

        public static bool MouseMove(int X, int Y, MouseButtons mb)
        {
            if (m_Capture != null)
            {
                Point point = m_Capture.PointToClient(new Point(X, Y));
                if (m_LastOver != m_Capture)
                {
                    if (m_LastOver != null)
                    {
                        m_LastOver.OnMouseLeave();
                    }
                    m_Capture.OnMouseEnter(point.X, point.Y, mb);
                    m_LastOver = m_Capture;
                }
                m_Capture.OnMouseMove(point.X, point.Y, mb);
                return true;
            }
            if ((m_Desktop == null) || (m_Desktop.Children.Count == 0))
            {
                return false;
            }
            if ((m_Drag != null) && m_Drag.m_IsDragging)
            {
                int x = X - m_Drag.m_OffsetX;
                int y = Y - m_Drag.m_OffsetY;
                if ((x + m_Drag.Width) < m_Drag.m_DragClipX)
                {
                    x = m_Drag.m_DragClipX - m_Drag.Width;
                }
                else if (x > (Engine.ScreenWidth - m_Drag.m_DragClipX))
                {
                    x = Engine.ScreenWidth - m_Drag.m_DragClipX;
                }
                if ((y + m_Drag.Height) < m_Drag.m_DragClipY)
                {
                    y = m_Drag.m_DragClipY - m_Drag.Height;
                }
                else if (y > (Engine.ScreenHeight - m_Drag.m_DragClipY))
                {
                    y = Engine.ScreenHeight - m_Drag.m_DragClipY;
                }
                Point point2 = m_Drag.Parent.PointToClient(new Point(x, y));
                m_Drag.X = point2.X;
                m_Drag.Y = point2.Y;
                m_Drag.OnDragMove();
                Gump target = null;
                RecurseFindDrop(0, 0, m_Desktop, X, Y, mb, ref target);
                if (target != null)
                {
                    if (m_LastDragOver != target)
                    {
                        if (m_LastDragOver != null)
                        {
                            m_LastDragOver.OnDragLeave(m_Drag);
                        }
                        target.OnDragEnter(m_Drag);
                    }
                }
                else if (m_LastDragOver != null)
                {
                    m_LastDragOver.OnDragLeave(m_Drag);
                }
                m_LastDragOver = target;
                if (m_LastOver != m_Drag)
                {
                    if (m_LastOver != null)
                    {
                        m_LastOver.OnMouseLeave();
                    }
                    m_LastOver = m_Drag;
                    if (m_LastOver != null)
                    {
                        point2 = m_LastOver.PointToClient(new Point(X, Y));
                        m_LastOver.OnMouseEnter(point2.X, point2.Y, mb);
                    }
                }
                return !IsWorldAt(X, Y, false);
            }
            Gump startDrag = m_StartDrag;
            if (!RecurseMouseMove(0, 0, m_Desktop, X, Y, mb))
            {
                if (((startDrag != null) && startDrag.m_CanDrag) && (mb == MouseButtons.Left))
                {
                    m_Drag = startDrag;
                    startDrag.m_IsDragging = true;
                    startDrag.OnDragStart();
                }
                else if (m_LastOver != null)
                {
                    m_LastOver.OnMouseLeave();
                    m_LastOver = null;
                }
                return (m_Modal != null);
            }
            if ((((startDrag != m_LastOver) && (startDrag != null)) && (startDrag.m_CanDrag && !startDrag.m_IsDragging)) && (!startDrag.m_QuickDrag && (mb == MouseButtons.Left)))
            {
                m_Drag = startDrag;
                if (m_LastOver != m_Drag)
                {
                    if (m_LastOver != null)
                    {
                        m_LastOver.OnMouseLeave();
                    }
                    m_LastOver = m_Drag;
                    if (m_LastOver != null)
                    {
                        Point p = new Point(X, Y);
                        p = m_LastOver.PointToClient(p);
                        m_LastOver.OnMouseEnter(p.X, p.Y, mb);
                    }
                }
                startDrag.m_IsDragging = true;
                startDrag.OnDragStart();
                if ((m_Drag != null) && m_Drag.m_IsDragging)
                {
                    MouseMove(X, Y, mb);
                }
                if (m_LastOver != m_Drag)
                {
                    if (m_LastOver != null)
                    {
                        m_LastOver.OnMouseLeave();
                    }
                    m_LastOver = m_Drag;
                    if (m_LastOver != null)
                    {
                        Point point4 = new Point(X, Y);
                        point4 = m_LastOver.PointToClient(point4);
                        m_LastOver.OnMouseEnter(point4.X, point4.Y, mb);
                    }
                }
            }
            else if ((((startDrag == m_LastOver) && (startDrag != null)) && (startDrag.m_CanDrag && !startDrag.m_IsDragging)) && ((!startDrag.m_QuickDrag && (mb == MouseButtons.Left)) && ((m_StartDragPoint ^ new Point(X, Y)) >= 2)))
            {
                m_Drag = startDrag;
                if (m_LastOver != m_Drag)
                {
                    if (m_LastOver != null)
                    {
                        m_LastOver.OnMouseLeave();
                    }
                    m_LastOver = m_Drag;
                    if (m_LastOver != null)
                    {
                        Point point5 = new Point(X, Y);
                        point5 = m_LastOver.PointToClient(point5);
                        m_LastOver.OnMouseEnter(point5.X, point5.Y, mb);
                    }
                }
                startDrag.m_IsDragging = true;
                startDrag.OnDragStart();
                if ((m_Drag != null) && m_Drag.m_IsDragging)
                {
                    MouseMove(X, Y, mb);
                }
                if (m_LastOver != m_Drag)
                {
                    if (m_LastOver != null)
                    {
                        m_LastOver.OnMouseLeave();
                    }
                    m_LastOver = m_Drag;
                    if (m_LastOver != null)
                    {
                        Point point6 = new Point(X, Y);
                        point6 = m_LastOver.PointToClient(point6);
                        m_LastOver.OnMouseEnter(point6.X, point6.Y, mb);
                    }
                }
            }
            return true;
        }

        public static bool MouseUp(int X, int Y, MouseButtons mb)
        {
            m_StartDrag = null;
            if (m_Capture != null)
            {
                Point point = m_Capture.PointToClient(new Point(X, Y));
                m_Capture.OnMouseUp(point.X, point.Y, mb);
                return true;
            }
            if ((m_Desktop == null) || (m_Desktop.Children.Count == 0))
            {
                return false;
            }
            if ((m_Drag != null) && ((mb & MouseButtons.Left) == MouseButtons.Left))
            {
                bool flag = !IsWorldAt(X, Y, false);
                if (m_Drag.m_IsDragging && (m_LastDragOver != null))
                {
                    m_LastDragOver.OnDragDrop(m_Drag);
                    Engine.CancelClick();
                }
                if (m_Drag != null)
                {
                    m_Drag.m_IsDragging = false;
                }
                Drag = null;
                m_LastDragOver = null;
                return flag;
            }
            if (m_Drag != null)
            {
                return !IsWorldAt(X, Y, false);
            }
            return (RecurseMouseUp(0, 0, m_Desktop, X, Y, mb) || (m_Modal != null));
        }

        public static void MouseWheel(int X, int Y, int Delta)
        {
            if (m_Capture != null)
            {
                m_Capture.OnMouseWheel(Delta);
            }
            else if (((m_Desktop != null) && (m_Desktop.Children.Count != 0)) && (!RecurseMouseWheel(0, 0, m_Desktop, X, Y, Delta) && (m_Focus != null)))
            {
                m_Focus.OnMouseWheel(Delta);
            }
        }

        public static void OpenPaperdoll(Mobile m, string Name, bool canDrag)
        {
            if (m != null)
            {
                GPaperdoll paperdoll;
                bool flag = m.Paperdoll != null;
                bool flag2 = flag && (m_LastOver == m.Paperdoll);
                bool flag3 = flag && (m_Drag == m.Paperdoll);
                int num = flag3 ? m_Drag.m_OffsetX : 0;
                int num2 = flag3 ? m_Drag.m_OffsetY : 0;
                int index = flag ? m.Paperdoll.Parent.Children.IndexOf(m.Paperdoll) : -1;
                int x = 0x7fffffff;
                int y = 5;
                if (flag)
                {
                    x = m.Paperdoll.X;
                    y = m.Paperdoll.Y;
                    Destroy(m.Paperdoll);
                }
                else if ((m.PaperdollX < 0x7fffffff) && (m.PaperdollY < 0x7fffffff))
                {
                    x = m.PaperdollX;
                    y = m.PaperdollY;
                    m.PaperdollX = 0x7fffffff;
                    m.PaperdollY = 0x7fffffff;
                }
                OnClick[] clickArray2 = new OnClick[8];
                clickArray2[0] = new OnClick(Engine.Help_OnClick);
                clickArray2[1] = new OnClick(Engine.Options_OnClick);
                clickArray2[2] = new OnClick(Engine.LogOut_OnClick);
                clickArray2[3] = new OnClick(Engine.Journal_OnClick);
                clickArray2[4] = new OnClick(Engine.Skills_OnClick);
                clickArray2[6] = new OnClick(Engine.AttackModeToggle_OnClick);
                clickArray2[7] = new OnClick(Engine.Status_OnClick);
                OnClick[] clickArray = clickArray2;
                int[] numArray = new int[] { 0x2c, 0x47, 0x62, 0x7c, 0x97, 0xb3, 0xcd, 0xe9 };
                int[] numArray3 = new int[] { 0x7ef, 0x7d6, 0x7d9, 0x7dc, 0x7df, 0x7e2, 0, 0x7eb };
                numArray3[6] = World.Player.Flags[MobileFlag.Warmode] ? 0x7e8 : 0x7e5;
                int[] numArray2 = numArray3;
                if (m.Player)
                {
                    paperdoll = new GPaperdoll(m, 0x7d0, x, y);
                    if (!flag && (x >= 0x7fffffff))
                    {
                        paperdoll.X = (Engine.ScreenWidth - paperdoll.Width) - 5;
                    }
                    paperdoll.Children.Add(new GButton(0x7ef, 0x7f1, 0x7f0, 0xb9, 0x2c, null));
                    GButton[] buttonArray = new GButton[7];
                    for (int i = 0; i < 7; i++)
                    {
                        buttonArray[i] = new GButton(numArray2[i], numArray2[i] + 2, numArray2[i] + 1, 0xb9, numArray[i], clickArray[i]);
                        buttonArray[i].Enabled = clickArray[i] != null;
                        paperdoll.Children.Add(buttonArray[i]);
                    }
                }
                else
                {
                    paperdoll = new GPaperdoll(m, 0x7d1, x, y);
                    if (!flag && (x >= 0x7fffffff))
                    {
                        paperdoll.X = (Engine.ScreenWidth - paperdoll.Width) - 5;
                    }
                }
                paperdoll.Children.Add(new GVirtueTrigger(m));
                GButton toAdd = new GButton(numArray2[7], numArray2[7] + 2, numArray2[7] + 1, 0xb9, numArray[7], clickArray[7]);
                toAdd.SetTag("Serial", m.Serial);
                paperdoll.Children.Add(toAdd);
                int hueID = (ushort) m.Hue;
                bool flag4 = false;
                int gender = 0;
                int body = m.Body;
                hueID ^= 0x8000;
                IHue h = Hues.Load(hueID);
                Engine.m_Animations.Translate(ref body, ref h);
                switch (body)
                {
                    case 400:
                        gender = 0;
                        paperdoll.Children.Add(new GImage(12, h, 8, 0x13));
                        break;

                    case 0x191:
                        gender = 1;
                        paperdoll.Children.Add(new GImage(13, h, 8, 0x13));
                        break;

                    case 0x192:
                    case 0x193:
                    {
                        GImage image = new GImage((m.Gender == 0) ? 12 : 13, 8, 0x13) {
                            Alpha = 0.25f
                        };
                        paperdoll.Children.Add(image);
                        flag4 = true;
                        break;
                    }
                    case 0x3db:
                        gender = m.Gender;
                        paperdoll.Children.Add(new GImage((m.Gender == 0) ? 12 : 13, Hues.Load(0x3ea), 8, 0x13));
                        paperdoll.Children.Add(new GImage(0xee3b, h, 8, 0x13));
                        break;

                    default:
                    {
                        int paperdollGump = Config.GetPaperdollGump(body);
                        if (paperdollGump != 0)
                        {
                            paperdoll.Children.Add(new GImage(paperdollGump, h, 8, 0x13));
                        }
                        break;
                    }
                }
                paperdoll.Gender = gender;
                if (flag4)
                {
                    for (int j = 0; j < m.Equip.Count; j++)
                    {
                        EquipEntry entry = (EquipEntry) m.Equip[j];
                        if (entry.m_Layer == Layer.OuterTorso)
                        {
                            int iD = entry.m_Item.ID;
                            int hue = entry.m_Item.Hue;
                            int gumpID = GetEquipGumpID(iD, gender, ref hue);
                            GPaperdollItem item = new GPaperdollItem(8, 0x13, gumpID, entry.m_Item.Serial, Hues.GetItemHue(iD, hue), (int) entry.m_Layer, m, canDrag) {
                                Alpha = 0.5f
                            };
                            paperdoll.Children.Add(item);
                        }
                    }
                }
                else
                {
                    LayerComparer comparer = LayerComparer.Paperdoll;
                    m.Equip.Sort(comparer);
                    for (int k = 0; k < m.Equip.Count; k++)
                    {
                        EquipEntry entry2 = (EquipEntry) m.Equip[k];
                        if (comparer.IsValid(entry2.m_Layer))
                        {
                            int itemID = entry2.m_Item.ID;
                            int num17 = entry2.m_Item.Hue;
                            int num18 = GetEquipGumpID(itemID, gender, ref num17);
                            paperdoll.Children.Add(new GPaperdollItem(8, 0x13, num18, entry2.m_Item.Serial, Hues.GetItemHue(itemID, num17), (int) entry2.m_Layer, m, canDrag));
                        }
                    }
                    m.Equip.Sort(LayerComparer.FromDirection(m.Direction));
                }
                paperdoll.Children.Add(new GProfileScroll(m));
                paperdoll.Children.Add(new GWrappedLabel(Name, Engine.GetFont(1), Hues.Load(0x769), 0x27, 0x108, 0xb8));
                paperdoll.SetTag("Dispose", "Paperdoll");
                paperdoll.SetTag("Serial", m.Serial);
                if (flag2)
                {
                    m_LastOver = paperdoll;
                }
                if (flag3)
                {
                    paperdoll.m_IsDragging = true;
                    paperdoll.OffsetX = num;
                    paperdoll.OffsetY = num2;
                    m_Drag = paperdoll;
                }
                if (((paperdoll.X + paperdoll.Width) - paperdoll.m_DragClipX) < 0)
                {
                    paperdoll.X = paperdoll.m_DragClipX - paperdoll.Width;
                }
                else if ((paperdoll.X + paperdoll.m_DragClipX) >= Engine.ScreenWidth)
                {
                    paperdoll.X = Engine.ScreenWidth - paperdoll.m_DragClipX;
                }
                if (((paperdoll.Y + paperdoll.Height) - paperdoll.m_DragClipY) < 0)
                {
                    paperdoll.Y = paperdoll.m_DragClipY - paperdoll.Height;
                }
                else if ((paperdoll.Y + paperdoll.m_DragClipY) >= Engine.ScreenHeight)
                {
                    paperdoll.Y = Engine.ScreenHeight - paperdoll.m_DragClipY;
                }
                if (index != -1)
                {
                    Desktop.Children.Insert(index, paperdoll);
                }
                else
                {
                    Desktop.Children.Add(paperdoll);
                }
                m.Paperdoll = paperdoll;
            }
        }

        public Texture ReadFromDisk(int GumpID, IHue Hue)
        {
            if (this.m_Factory == null)
            {
                this.m_Factory = new GumpFactory(this);
            }
            return this.m_Factory.Load(GumpID, Hue);
        }

        private static bool RecurseDoubleClick(int X, int Y, Gump g, int mX, int mY)
        {
            if (g.Visible && (g.m_NonRestrictivePicking || (((mX >= X) && (mX < (X + g.Width))) && ((mY >= Y) && (mY < (Y + g.Height))))))
            {
                Gump[] gumpArray = g.Children.ToArray();
                for (int i = gumpArray.Length - 1; i >= 0; i--)
                {
                    Gump gump = gumpArray[i];
                    if (RecurseDoubleClick(X + gump.X, Y + gump.Y, gump, mX, mY))
                    {
                        return true;
                    }
                }
                if (!g.m_NonRestrictivePicking || (((mX >= X) && (mX < (X + g.Width))) && ((mY >= Y) && (mY < (Y + g.Height)))))
                {
                    if ((m_Modal == null) && g.HitTest(mX - X, mY - Y))
                    {
                        if (m_TextFocus != null)
                        {
                            m_TextFocus.Unfocus();
                            m_TextFocus = null;
                        }
                        g.OnDoubleClick(mX - X, mY - Y);
                        return true;
                    }
                    if (((m_Modal != null) && g.IsChildOf(m_Modal)) && g.HitTest(mX - X, mY - Y))
                    {
                        if (m_TextFocus != null)
                        {
                            m_TextFocus.Unfocus();
                            m_TextFocus = null;
                        }
                        g.OnDoubleClick(mX - X, mY - Y);
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool RecurseFindDrop(int X, int Y, Gump g, int mX, int mY, MouseButtons mb, ref Gump target)
        {
            if (g.Visible)
            {
                if (g == m_Drag)
                {
                    return false;
                }
                if (g.m_NonRestrictivePicking || (((mX >= X) && (mX < (X + g.Width))) && ((mY >= Y) && (mY < (Y + g.Height)))))
                {
                    Gump[] gumpArray = g.Children.ToArray();
                    for (int i = gumpArray.Length - 1; i >= 0; i--)
                    {
                        Gump gump = gumpArray[i];
                        if (RecurseFindDrop(X + gump.X, Y + gump.Y, gump, mX, mY, mb, ref target))
                        {
                            return true;
                        }
                    }
                    if ((!g.m_NonRestrictivePicking || (((mX >= X) && (mX < (X + g.Width))) && ((mY >= Y) && (mY < (Y + g.Height))))) && g.m_CanDrop)
                    {
                        if ((m_Modal == null) && g.HitTest(mX - X, mY - Y))
                        {
                            target = g;
                            return true;
                        }
                        if (((m_Modal != null) && g.IsChildOf(m_Modal)) && g.HitTest(mX - X, mY - Y))
                        {
                            target = g;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private static object[] RecurseFindListForSingleClick(int x, int y, Gump g, int mx, int my)
        {
            if (g.Visible && (g.m_NonRestrictivePicking || (((mx >= x) && (mx < (x + g.Width))) && ((my >= y) && (my < (y + g.Height))))))
            {
                Gump[] gumpArray = g.Children.ToArray();
                for (int i = gumpArray.Length - 1; i >= 0; i--)
                {
                    Gump gump = gumpArray[i];
                    object[] objArray = RecurseFindListForSingleClick(x + gump.X, y + gump.Y, gump, mx, my);
                    if (objArray != null)
                    {
                        return objArray;
                    }
                }
                if (!g.m_NonRestrictivePicking || (((mx >= x) && (mx < (x + g.Width))) && ((my >= y) && (my < (y + g.Height)))))
                {
                    if ((m_Modal == null) && g.HitTest(mx - x, my - y))
                    {
                        return new object[] { g, new Point(mx - x, my - y) };
                    }
                    if (((m_Modal != null) && g.IsChildOf(m_Modal)) && g.HitTest(mx - x, my - y))
                    {
                        return new object[] { g, new Point(mx - x, my - y) };
                    }
                }
            }
            return null;
        }

        private static void RecurseFocusChanged(Gump g, Gump focus)
        {
            g.OnFocusChanged(focus);
            Gump[] gumpArray = g.Children.ToArray();
            for (int i = 0; i < gumpArray.Length; i++)
            {
                RecurseFocusChanged(gumpArray[i], focus);
            }
        }

        private static bool RecurseIsWorldAt(int X, int Y, Gump g, int mX, int mY, bool CheckDrag)
        {
            if (g.Visible)
            {
                if (!CheckDrag && (g == m_Drag))
                {
                    return false;
                }
                if (g.m_NonRestrictivePicking || (((mX >= X) && (mX < (X + g.Width))) && ((mY >= Y) && (mY < (Y + g.Height)))))
                {
                    Gump[] gumpArray = g.Children.ToArray();
                    for (int i = gumpArray.Length - 1; i >= 0; i--)
                    {
                        Gump gump = gumpArray[i];
                        if (RecurseIsWorldAt(X + gump.X, Y + gump.Y, gump, mX, mY, CheckDrag))
                        {
                            return true;
                        }
                    }
                    if ((!g.m_NonRestrictivePicking || (((mX >= X) && (mX < (X + g.Width))) && ((mY >= Y) && (mY < (Y + g.Height))))) && g.HitTest(mX - X, mY - Y))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool RecurseKeyDown(Gump g, char c)
        {
            if (!g.Visible)
            {
                return false;
            }
            Gump[] gumpArray = g.Children.ToArray();
            for (int i = gumpArray.Length - 1; i >= 0; i--)
            {
                if (RecurseKeyDown(gumpArray[i], c))
                {
                    return true;
                }
            }
            return ((g.GetType() != typeof(GTextBox)) && g.OnKeyDown(c));
        }

        private static bool RecurseMouseDown(int X, int Y, Gump g, int mX, int mY, MouseButtons mb)
        {
            if (g.Visible && (g.m_NonRestrictivePicking || (((mX >= X) && (mX < (X + g.Width))) && ((mY >= Y) && (mY < (Y + g.Height))))))
            {
                Gump[] gumpArray = g.Children.ToArray();
                for (int i = gumpArray.Length - 1; i >= 0; i--)
                {
                    Gump gump = gumpArray[i];
                    if (RecurseMouseDown(X + gump.X, Y + gump.Y, gump, mX, mY, mb))
                    {
                        return true;
                    }
                }
                if (!g.m_NonRestrictivePicking || (((mX >= X) && (mX < (X + g.Width))) && ((mY >= Y) && (mY < (Y + g.Height)))))
                {
                    if ((m_Modal == null) && g.HitTest(mX - X, mY - Y))
                    {
                        if (m_TextFocus != null)
                        {
                            m_TextFocus.Unfocus();
                            m_TextFocus = null;
                        }
                        if (((m_Drag == null) && g.m_CanDrag) && (mb == MouseButtons.Left))
                        {
                            m_StartDrag = g;
                            m_StartDragPoint = new Point(mX, mY);
                            g.m_OffsetX = mX - X;
                            g.m_OffsetY = mY - Y;
                            if (g.m_QuickDrag)
                            {
                                g.m_IsDragging = true;
                                m_Drag = g;
                                g.OnDragStart();
                            }
                        }
                        g.OnMouseDown(mX - X, mY - Y, mb);
                        Focus = g;
                        if (g == m_Drag)
                        {
                            return !IsWorldAt(mX, mY, false);
                        }
                        return true;
                    }
                    if (((m_Modal != null) && g.IsChildOf(m_Modal)) && g.HitTest(mX - X, mY - Y))
                    {
                        if (m_TextFocus != null)
                        {
                            m_TextFocus.Unfocus();
                            m_TextFocus = null;
                        }
                        if (((m_Drag == null) && g.m_CanDrag) && (mb == MouseButtons.Left))
                        {
                            m_StartDrag = g;
                            m_StartDragPoint = new Point(mX, mY);
                            g.m_OffsetX = mX - X;
                            g.m_OffsetY = mY - Y;
                            if (g.m_QuickDrag)
                            {
                                g.m_IsDragging = true;
                                g.OnDragStart();
                                m_Drag = g;
                            }
                        }
                        g.OnMouseDown(mX - X, mY - Y, mb);
                        Focus = g;
                        if (g == m_Drag)
                        {
                            return !IsWorldAt(mX, mY, false);
                        }
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool RecurseMouseMove(int X, int Y, Gump g, int mX, int mY, MouseButtons mb)
        {
            if (g.Visible && (g.m_NonRestrictivePicking || (((mX >= X) && (mX < (X + g.Width))) && ((mY >= Y) && (mY < (Y + g.Height))))))
            {
                Gump[] gumpArray = g.Children.ToArray();
                for (int i = gumpArray.Length - 1; i >= 0; i--)
                {
                    Gump gump = gumpArray[i];
                    if (RecurseMouseMove(X + gump.X, Y + gump.Y, gump, mX, mY, mb))
                    {
                        return true;
                    }
                }
                if (!g.m_NonRestrictivePicking || (((mX >= X) && (mX < (X + g.Width))) && ((mY >= Y) && (mY < (Y + g.Height)))))
                {
                    if ((m_Modal == null) && g.HitTest(mX - X, mY - Y))
                    {
                        if (m_LastOver == g)
                        {
                            g.OnMouseMove(mX - X, mY - Y, mb);
                        }
                        else
                        {
                            if (m_LastOver != null)
                            {
                                m_LastOver.OnMouseLeave();
                            }
                            g.OnMouseEnter(mX - X, mY - Y, mb);
                            if (g.Tooltip != null)
                            {
                                m_TipDelay = new TimeDelay(g.Tooltip.Delay);
                            }
                            else
                            {
                                m_TipDelay = null;
                            }
                            m_LastOver = g;
                        }
                        return true;
                    }
                    if (((m_Modal != null) && g.IsChildOf(m_Modal)) && g.HitTest(mX - X, mY - Y))
                    {
                        if (m_LastOver == g)
                        {
                            g.OnMouseMove(mX - X, mY - Y, mb);
                        }
                        else
                        {
                            if (m_LastOver != null)
                            {
                                m_LastOver.OnMouseLeave();
                            }
                            g.OnMouseEnter(mX - X, mY - Y, mb);
                            if (g.Tooltip != null)
                            {
                                m_TipDelay = new TimeDelay(g.Tooltip.Delay);
                            }
                            else
                            {
                                m_TipDelay = null;
                            }
                            m_LastOver = g;
                        }
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool RecurseMouseUp(int X, int Y, Gump g, int mX, int mY, MouseButtons mb)
        {
            if (g.Visible && (g.m_NonRestrictivePicking || (((mX >= X) && (mX < (X + g.Width))) && ((mY >= Y) && (mY < (Y + g.Height))))))
            {
                Gump[] gumpArray = g.Children.ToArray();
                for (int i = gumpArray.Length - 1; i >= 0; i--)
                {
                    Gump gump = gumpArray[i];
                    if (RecurseMouseUp(X + gump.X, Y + gump.Y, gump, mX, mY, mb))
                    {
                        return true;
                    }
                }
                if (!g.m_NonRestrictivePicking || (((mX >= X) && (mX < (X + g.Width))) && ((mY >= Y) && (mY < (Y + g.Height)))))
                {
                    if ((m_Modal == null) && g.HitTest(mX - X, mY - Y))
                    {
                        g.OnMouseUp(mX - X, mY - Y, mb);
                        return true;
                    }
                    if (((m_Modal != null) && g.IsChildOf(m_Modal)) && g.HitTest(mX - X, mY - Y))
                    {
                        g.OnMouseUp(mX - X, mY - Y, mb);
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool RecurseMouseWheel(int X, int Y, Gump g, int mX, int mY, int Delta)
        {
            if (g.Visible && (g.m_NonRestrictivePicking || (((mX >= X) && (mX < (X + g.Width))) && ((mY >= Y) && (mY < (Y + g.Height))))))
            {
                Gump[] gumpArray = g.Children.ToArray();
                for (int i = gumpArray.Length - 1; i >= 0; i--)
                {
                    Gump gump = gumpArray[i];
                    if (RecurseMouseWheel(X + gump.X, Y + gump.Y, gump, mX, mY, Delta))
                    {
                        return true;
                    }
                }
                if (!g.m_NonRestrictivePicking || (((mX >= X) && (mX < (X + g.Width))) && ((mY >= Y) && (mY < (Y + g.Height)))))
                {
                    if ((m_Modal == null) && g.HitTest(mX - X, mY - Y))
                    {
                        g.OnMouseWheel(Delta);
                        return true;
                    }
                    if (((m_Modal != null) && g.IsChildOf(m_Modal)) && g.HitTest(mX - X, mY - Y))
                    {
                        g.OnMouseWheel(Delta);
                        return true;
                    }
                }
            }
            return false;
        }

        public static void Restore(Gump ToRestore)
        {
            if (((m_ToRestore != null) && (ToRestore.GUID != null)) && ((ToRestore.GUID.Length > 0) && m_ToRestore.Contains(ToRestore.GUID)))
            {
                Point point = (Point) m_ToRestore[ToRestore.GUID];
                ToRestore.X = point.X;
                ToRestore.Y = point.Y;
            }
        }

        public static void TextBoxTab(Gump Start)
        {
            GumpList children;
            int index;
            GTextBox textBox;
            if (Start.Parent is GWindowsTextBox)
            {
                children = Start.Parent.Parent.Children;
                index = children.IndexOf(Start.Parent);
            }
            else
            {
                children = Start.Parent.Children;
                index = children.IndexOf(Start);
            }
            Gump[] gumpArray = children.ToArray();
            int length = gumpArray.Length;
            if ((Control.ModifierKeys & Keys.Shift) == Keys.None)
            {
                index++;
                for (int i = 0; i < length; i++)
                {
                    Gump gump = gumpArray[(i + index) % length];
                    if (gump is GWindowsTextBox)
                    {
                        textBox = ((GWindowsTextBox) gump).TextBox;
                    }
                    else
                    {
                        textBox = gump as GTextBox;
                    }
                    if (textBox != null)
                    {
                        if (m_TextFocus != null)
                        {
                            m_TextFocus.Unfocus();
                        }
                        textBox.Focus();
                        break;
                    }
                }
            }
            else
            {
                index--;
                for (int j = 0; j < length; j++)
                {
                    Gump gump2 = gumpArray[((length + index) - j) % length];
                    if (gump2 is GWindowsTextBox)
                    {
                        textBox = ((GWindowsTextBox) gump2).TextBox;
                    }
                    else
                    {
                        textBox = gump2 as GTextBox;
                    }
                    if (textBox != null)
                    {
                        if (m_TextFocus != null)
                        {
                            m_TextFocus.Unfocus();
                        }
                        textBox.Focus();
                        break;
                    }
                }
            }
        }

        public static Gump Capture
        {
            get
            {
                return m_Capture;
            }
            set
            {
                m_Capture = value;
            }
        }

        public static Gump Desktop
        {
            get
            {
                return m_Desktop;
            }
        }

        public static Gump Drag
        {
            get
            {
                return m_Drag;
            }
            set
            {
                if (((value == null) && (m_Drag != null)) && (m_Drag.GetType() == typeof(GDragable)))
                {
                    LinkDocked((GDragable) m_Drag);
                }
                if ((value == null) && (m_Drag != null))
                {
                    m_Drag.m_IsDragging = false;
                }
                m_Drag = value;
            }
        }

        public static Gump Focus
        {
            get
            {
                return m_Focus;
            }
            set
            {
                if (m_Focus != value)
                {
                    RecurseFocusChanged(m_Desktop, value);
                }
                m_Focus = value;
            }
        }

        public static bool Invalidated
        {
            get
            {
                return m_Invalidated;
            }
            set
            {
                m_Invalidated = value;
            }
        }

        public static Gump LastOver
        {
            get
            {
                return m_LastOver;
            }
            set
            {
                if (m_LastOver != value)
                {
                    if (m_LastOver != null)
                    {
                        m_LastOver.OnMouseLeave();
                    }
                    m_LastOver = value;
                }
            }
        }

        public static Gump Modal
        {
            get
            {
                return m_Modal;
            }
            set
            {
                m_Modal = value;
                if (((m_Modal != null) && (m_TextFocus != null)) && !m_TextFocus.IsChildOf(m_Modal))
                {
                    m_TextFocus.Unfocus();
                }
            }
        }

        public string Name
        {
            get
            {
                return "Gumps";
            }
        }

        public static Gump StartDrag
        {
            get
            {
                return m_StartDrag;
            }
            set
            {
                m_StartDrag = value;
            }
        }

        public static GTextBox TextFocus
        {
            get
            {
                return m_TextFocus;
            }
            set
            {
                m_TextFocus = value;
            }
        }

        private class GumpFactory : TextureFactory
        {
            private int m_GumpID;
            private Gumps m_Gumps;
            private IHue m_Hue;

            public GumpFactory(Gumps gumps)
            {
                this.m_Gumps = gumps;
            }

            protected override void CoreAssignArgs(Texture tex)
            {
                tex.m_Factory = this;
                tex.m_FactoryArgs = new object[] { this.m_GumpID, this.m_Hue };
            }

            protected override void CoreGetDimensions(out int width, out int height)
            {
                width = height = 0;
                if (this.m_GumpID != 0)
                {
                    Entry3D entryd = this.m_Gumps.m_Index[this.m_GumpID];
                    int lookup = entryd.m_Lookup;
                    int length = entryd.m_Length;
                    if (lookup != -1)
                    {
                        int extra = entryd.m_Extra;
                        width = (extra >> 0x10) & 0xffff;
                        height = extra & 0xffff;
                    }
                }
            }

            protected override bool CoreLookup()
            {
                if (this.m_GumpID == 0)
                {
                    return false;
                }
                Entry3D entryd = this.m_Gumps.m_Index[this.m_GumpID];
                int lookup = entryd.m_Lookup;
                int length = entryd.m_Length;
                if (lookup == -1)
                {
                    return false;
                }
                int extra = entryd.m_Extra;
                int num4 = (extra >> 0x10) & 0xffff;
                int num5 = extra & 0xffff;
                if ((num4 <= 0) || (num5 <= 0))
                {
                    return false;
                }
                return true;
            }

            protected override unsafe void CoreProcessImage(int width, int height, int stride, ushort* pLine, ushort* pLineEnd, ushort* pImageEnd, int lineDelta, int lineEndDelta)
            {
                if (this.m_GumpID != 0)
                {
                    Entry3D entryd = this.m_Gumps.m_Index[this.m_GumpID];
                    int lookup = entryd.m_Lookup;
                    int length = entryd.m_Length;
                    if (lookup != -1)
                    {
                        FileStream verdata;
                        if (length < 0)
                        {
                            length &= 0x7fffffff;
                            if ((this.m_Gumps.m_Buffer == null) || (length > this.m_Gumps.m_Buffer.Length))
                            {
                                this.m_Gumps.m_Buffer = new byte[length];
                            }
                            this.m_Gumps.m_Verdata.Seek((long) lookup, SeekOrigin.Begin);
                            verdata = (FileStream) this.m_Gumps.m_Verdata;
                        }
                        else
                        {
                            if ((this.m_Gumps.m_Buffer == null) || (length > this.m_Gumps.m_Buffer.Length))
                            {
                                this.m_Gumps.m_Buffer = new byte[length];
                            }
                            this.m_Gumps.m_Stream.Seek((long) lookup, SeekOrigin.Begin);
                            verdata = (FileStream) this.m_Gumps.m_Stream;
                        }
                        fixed (byte* numRef = this.m_Gumps.m_Buffer)
                        {
                            Engine.NativeRead(verdata, (void*) numRef, length);
                            int* numPtr = (int*) numRef;
                            for (int* numPtr2 = numPtr; pLine < pImageEnd; numPtr2++)
                            {
                                this.m_Hue.FillLine((ushort*) (numPtr + numPtr2[0]), pLine, pLineEnd);
                                pLine += lineEndDelta;
                                pLineEnd += lineEndDelta;
                            }
                        }
                    }
                }
            }

            public Texture Load(int gumpID, IHue hue)
            {
                if (gumpID == 0xa40)
                {
                    hue = Hues.Load(0x8001);
                }
                this.m_GumpID = gumpID;
                this.m_Hue = hue;
                return base.Construct(false);
            }

            public override Texture Reconstruct(object[] args)
            {
                this.m_GumpID = (int) args[0];
                this.m_Hue = (IHue) args[1];
                return base.Construct(true);
            }
        }
    }
}

