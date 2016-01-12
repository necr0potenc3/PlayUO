namespace Client
{
    public class GItemCounters : GEmpty
    {
        private static bool m_Active;
        private static GItemCounter[] m_List;
        private static bool m_Valid;

        public GItemCounters(params ItemIDValidator[] list)
        {
            this.Y = -1;
            m_List = new GItemCounter[list.Length];
            for (int i = 0; i < m_List.Length; i++)
            {
                GItemCounter counter;
                m_List[i] = counter = new GItemCounter(list[i]);
                base.m_Children.Add(counter);
            }
            new Timer(new OnTick(this.Update_OnTick), 250).Start(true);
        }

        protected internal override void Render(int x, int y)
        {
            if (m_Active && m_Valid)
            {
                base.Render(x, y);
            }
        }

        private void Update_OnTick(Timer t)
        {
            if (Engine.m_Ingame && m_Active)
            {
                Mobile player = World.Player;
                if (player != null)
                {
                    Item backpack = player.Backpack;
                    if (backpack != null)
                    {
                        m_Valid = true;
                        int num = 0;
                        int width = 0;
                        for (int i = 0; i < m_List.Length; i++)
                        {
                            m_List[i].Update(backpack);
                            m_List[i].Y = num;
                            num += m_List[i].Height - 1;
                            if (m_List[i].Width > width)
                            {
                                width = m_List[i].Width;
                            }
                        }
                        for (int j = 0; j < m_List.Length; j++)
                        {
                            m_List[j].Width = width;
                        }
                        this.X = (Engine.ScreenWidth - width) + 1;
                        return;
                    }
                }
            }
            m_Valid = false;
        }

        public static bool Active
        {
            get
            {
                return m_Active;
            }
            set
            {
                m_Active = value;
            }
        }
    }
}