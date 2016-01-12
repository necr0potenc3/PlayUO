namespace Client
{
    using System;
    using System.Collections;

    public class MessageManager
    {
        private static ArrayList m_Messages = new ArrayList();
        private static Queue m_ToRemove = new Queue();
        private static int m_yStack;

        public static void AddMessage(IMessage m)
        {
            Gumps.Desktop.Children.Add((Gump)m);
            m_Messages.Insert(0, m);
            Gumps.Invalidated = true;
            if (m is GDynamicMessage)
            {
                int num = 0;
                IMessageOwner owner = ((GDynamicMessage)m).Owner;
                int count = m_Messages.Count;
                for (int i = 0; i < count; i++)
                {
                    if ((m_Messages[i] is GDynamicMessage) && (((GDynamicMessage)m_Messages[i]).Owner == owner))
                    {
                        if ((num >= 3) && !((GDynamicMessage)m_Messages[i]).Unremovable)
                        {
                            Remove((IMessage)m_Messages[i]);
                        }
                        num++;
                    }
                }
            }
            else if (m is GSystemMessage)
            {
                GSystemMessage message = (GSystemMessage)m;
                DateTime time = DateTime.Now - TimeSpan.FromSeconds(1.0);
                int num4 = m_Messages.Count;
                for (int j = 1; j < num4; j++)
                {
                    if (m_Messages[j] is GSystemMessage)
                    {
                        GSystemMessage message2 = (GSystemMessage)m_Messages[j];
                        if ((message2.OrigText == message.Text) && ((j == 1) || (message2.UpdateTime >= time)))
                        {
                            message.DupeCount = message2.DupeCount + 1;
                            message.Text = string.Format("{0} ({1})", message.Text, message.DupeCount);
                            Remove(message2);
                            break;
                        }
                    }
                }
            }
        }

        public static void BeginRender()
        {
            while (m_ToRemove.Count > 0)
            {
                object obj2 = m_ToRemove.Dequeue();
                m_Messages.Remove(obj2);
                Gumps.Destroy((Gump)obj2);
            }
            m_yStack = (Engine.GameY + Engine.GameHeight) - 0x16;
            RecurseProcessItemGumps(Gumps.Desktop, 0, 0, false);
            for (int i = 0; i < m_Messages.Count; i++)
            {
                ((IMessage)m_Messages[i]).OnBeginRender();
            }
            while (m_ToRemove.Count > 0)
            {
                object obj3 = m_ToRemove.Dequeue();
                m_Messages.Remove(obj3);
                Gumps.Destroy((Gump)obj3);
            }
            if (Gumps.Invalidated)
            {
                if (Engine.m_LastMouseArgs != null)
                {
                    Engine.MouseMove(Engine.m_Display, Engine.m_LastMouseArgs);
                    Engine.MouseMoveQueue();
                }
                Gumps.Invalidated = false;
            }
        }

        public static void ClearMessages(IMessageOwner owner)
        {
            int count = m_Messages.Count;
            for (int i = 0; i < count; i++)
            {
                IMessage m = (IMessage)m_Messages[i];
                if ((m is GDynamicMessage) && (((GDynamicMessage)m).Owner == owner))
                {
                    Remove(m);
                }
            }
        }

        private static void RecurseProcessItemGumps(Gump g, int x, int y, bool isItemGump)
        {
            if (isItemGump)
            {
                IItemGump gump = (IItemGump)g;
                Item item = gump.Item;
                item.MessageX = x + gump.xOffset;
                item.MessageY = y + gump.yOffset;
                item.BottomY = y + gump.yBottom;
                item.MessageFrame = Renderer.m_ActFrames;
                Gump desktop = Gumps.Desktop;
                GumpList children = desktop.Children;
                Gump child = g;
                while (child.Parent != desktop)
                {
                    child = child.Parent;
                }
                int index = children.IndexOf(child);
                for (int i = 0; i < m_Messages.Count; i++)
                {
                    if ((m_Messages[i] is GDynamicMessage) && (((GDynamicMessage)m_Messages[i]).Owner == item))
                    {
                        int num3 = children.IndexOf((Gump)m_Messages[i]);
                        if ((num3 < index) && (num3 >= 0))
                        {
                            children.RemoveAt(num3);
                            index = children.IndexOf(child);
                            children.Insert(index + 1, (Gump)m_Messages[i]);
                        }
                    }
                }
            }
            else
            {
                foreach (Gump gump4 in g.Children.ToArray())
                {
                    if (gump4 is IItemGump)
                    {
                        RecurseProcessItemGumps(gump4, x + gump4.X, y + gump4.Y, true);
                    }
                    else if (gump4.Children.Count > 0)
                    {
                        RecurseProcessItemGumps(gump4, x + gump4.X, y + gump4.Y, false);
                    }
                }
            }
        }

        public static void Remove(IMessage m)
        {
            m_ToRemove.Enqueue(m);
            Gumps.Invalidated = true;
        }

        public static int yStack
        {
            get
            {
                return m_yStack;
            }
            set
            {
                m_yStack = value;
            }
        }
    }
}