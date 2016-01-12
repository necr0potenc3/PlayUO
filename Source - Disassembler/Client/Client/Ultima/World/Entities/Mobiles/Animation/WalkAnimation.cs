namespace Client
{
    using System.Collections;

    public class WalkAnimation
    {
        private bool m_Advance;
        private float m_Duration;
        private float m_Frames;
        private Mobile m_Mobile;
        private int m_NewDir;
        private int m_NewX;
        private int m_NewY;
        private int m_NewZ;
        private static Queue m_Pool;
        private TimeSync m_Sync;
        private static Queue m_SyncPool = new Queue();
        private int m_X;
        private int m_Y;

        private WalkAnimation(Mobile m, int x, int y, int z, int dir)
        {
            this.Initialize(m, x, y, z, dir);
        }

        public void Dispose()
        {
            m_Pool.Enqueue(this);
            if (this.m_Sync != null)
            {
                m_SyncPool.Enqueue(this.m_Sync);
            }
        }

        private static float GetDuration(bool mounted, int idx)
        {
            if (!mounted)
            {
                return ((idx == 0) ? 0.4f : 0.2f);
            }
            return ((idx == 0) ? 0.2f : 0.1f);
        }

        private static int GetFrames(bool mounted, int idx)
        {
            if (!mounted)
            {
                return ((idx == 0) ? 4 : 2);
            }
            return ((idx == 0) ? 2 : 1);
        }

        private void Initialize(Mobile m, int NewX, int NewY, int NewZ, int NewDir)
        {
            this.m_Mobile = m;
            this.m_NewX = NewX;
            this.m_NewY = NewY;
            this.m_NewZ = NewZ;
            this.m_NewDir = NewDir;
            int x = 0;
            int y = 0;
            int z = 0;
            if (m.Walking.Count == 0)
            {
                x = m.X;
                y = m.Y;
                z = m.Z;
            }
            else
            {
                IEnumerator enumerator = m.Walking.GetEnumerator();
                WalkAnimation current = null;
                while (enumerator.MoveNext())
                {
                    current = (WalkAnimation)enumerator.Current;
                }
                if (current != null)
                {
                    x = current.m_NewX;
                    y = current.m_NewY;
                    z = current.m_NewZ;
                }
            }
            if (!m.Player)
            {
                Engine.EquipSort(m, NewDir & 0x87);
                m.Direction = (byte)NewDir;
            }
            this.m_Advance = false;
            this.m_Sync = null;
            if (((x != NewX) || (y != NewY)) || (z != NewZ))
            {
                int num4 = NewX - x;
                int num5 = NewY - y;
                int num6 = NewZ - z;
                int num7 = (num4 - num5) * 0x16;
                int num8 = ((num4 + num5) * 0x16) - (num6 * 4);
                this.m_X = num7;
                this.m_Y = num8;
                int idx = (NewDir >> 7) & 1;
                ArrayList equip = m.Equip;
                bool mounted = (equip.Count > 0) && (((EquipEntry)equip[0]).m_Layer == Layer.Mount);
                if ((m.Player && Engine.GMPrivs) && ((Engine.m_WalkSpeed != 0.4f) || (Engine.m_RunSpeed != 0.2f)))
                {
                    this.m_Duration = (idx == 0) ? Engine.m_WalkSpeed : Engine.m_RunSpeed;
                }
                else
                {
                    this.m_Duration = GetDuration(mounted, idx);
                }
                if (m.Player && (m.Body == 0x3db))
                {
                    this.m_Duration *= 0.25f;
                }
                this.m_Frames = GetFrames(mounted, idx);
            }
            else
            {
                this.m_X = 0;
                this.m_Y = 0;
                this.m_Duration = 0.1f;
                this.m_Frames = 0f;
            }
        }

        public static WalkAnimation PoolInstance(Mobile m, int x, int y, int z, int dir)
        {
            if (m_Pool == null)
            {
                m_Pool = new Queue();
            }
            if (m_Pool.Count > 0)
            {
                WalkAnimation animation = (WalkAnimation)m_Pool.Dequeue();
                animation.Initialize(m, x, y, z, dir);
                return animation;
            }
            return new WalkAnimation(m, x, y, z, dir);
        }

        public bool Snapshot(ref int xOffset, ref int yOffset, ref int fOffset)
        {
            if (this.m_Sync == null)
            {
                this.Start();
            }
            double normalized = this.m_Sync.Normalized;
            if (!NewConfig.SmoothWalk && (normalized != 1.0))
            {
                switch (((int)this.m_Frames))
                {
                    case 1:
                        normalized = 0.0;
                        break;

                    case 2:
                        normalized = (normalized < 0.5) ? 0.49999 : 0.99999;
                        break;

                    case 4:
                        if (normalized >= 0.25)
                        {
                            if (normalized < 0.5)
                            {
                                normalized = 0.49999;
                            }
                            else if (normalized < 0.75)
                            {
                                normalized = 0.74999;
                            }
                            else
                            {
                                normalized = 0.99999;
                            }
                            break;
                        }
                        normalized = 0.24999;
                        break;
                }
            }
            if (!this.m_Advance)
            {
                xOffset = (int)(this.m_X * normalized);
                yOffset = (int)(this.m_Y * normalized);
            }
            else
            {
                xOffset = -this.m_X + ((int)(this.m_X * normalized));
                yOffset = -this.m_Y + ((int)(this.m_Y * normalized));
            }
            fOffset = (int)(this.m_Frames * normalized);
            return (normalized < 1.0);
        }

        public void Start()
        {
            this.Start(true);
        }

        public void Start(bool update)
        {
            if (this.m_Sync == null)
            {
                if (m_SyncPool.Count > 0)
                {
                    this.m_Sync = (TimeSync)m_SyncPool.Dequeue();
                    this.m_Sync.Initialize((double)this.m_Duration);
                }
                else
                {
                    this.m_Sync = new TimeSync((double)this.m_Duration);
                }
                this.m_Advance = ((this.m_NewDir & 7) >= 1) && ((this.m_NewDir & 7) <= 4);
                if (this.m_Advance)
                {
                    this.m_Mobile.SetLocation((short)this.m_NewX, (short)this.m_NewY, (short)this.m_NewZ);
                    if (update)
                    {
                        this.m_Mobile.Update();
                    }
                    if (this.m_Mobile.Player)
                    {
                        Renderer.eOffsetX += this.m_X;
                        Renderer.eOffsetY += this.m_Y;
                    }
                }
            }
        }

        public bool Advance
        {
            get
            {
                return this.m_Advance;
            }
        }

        public int Frames
        {
            get
            {
                return (int)this.m_Frames;
            }
        }

        public int NewDir
        {
            get
            {
                return this.m_NewDir;
            }
        }

        public int NewX
        {
            get
            {
                return this.m_NewX;
            }
        }

        public int NewY
        {
            get
            {
                return this.m_NewY;
            }
        }

        public int NewZ
        {
            get
            {
                return this.m_NewZ;
            }
        }

        public int xOffset
        {
            get
            {
                return this.m_X;
            }
        }

        public int yOffset
        {
            get
            {
                return this.m_Y;
            }
        }
    }
}