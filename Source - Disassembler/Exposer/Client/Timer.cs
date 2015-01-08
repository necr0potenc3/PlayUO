namespace Client
{
    using System;
    using System.Collections;

    public class Timer
    {
        private int m_CurExecute;
        private int m_Delay;
        private int m_LastExecute;
        private int m_MaxExecute;
        private OnTick m_OnTick;
        private bool m_State;
        private Hashtable m_Tags;

        public Timer(OnTick OnTick, int Delay)
        {
            this.m_OnTick = OnTick;
            this.m_Delay = Delay;
            this.m_MaxExecute = -1;
            this.m_CurExecute = -1;
            this.m_LastExecute = 0;
            this.m_Tags = new Hashtable();
        }

        public Timer(OnTick OnTick, int Delay, int MaxExecute)
        {
            this.m_OnTick = OnTick;
            this.m_Delay = Delay;
            this.m_MaxExecute = MaxExecute;
            this.m_CurExecute = 0;
            this.m_LastExecute = 0;
            this.m_Tags = new Hashtable();
        }

        public void Delete()
        {
            this.m_State = false;
        }

        public object GetTag(string Name)
        {
            return this.m_Tags[Name];
        }

        public bool HasTag(string Name)
        {
            return this.m_Tags.Contains(Name);
        }

        public void RemoveTag(string Name)
        {
            this.m_Tags.Remove(Name);
        }

        public void SetTag(string Name, object Value)
        {
            this.m_Tags[Name] = Value;
        }

        public void Start(bool Now)
        {
            this.m_State = true;
            if (Now)
            {
                this.m_LastExecute = 0;
                this.Tick();
            }
            else
            {
                this.m_LastExecute = Interop.GetTicks();
            }
            Interop.AddTimer(this);
        }

        public void Stop()
        {
            this.m_State = false;
        }

        public bool Tick()
        {
            if (!this.m_State)
            {
                return false;
            }
            int ticks = Interop.GetTicks();
            if (ticks >= (this.m_LastExecute + this.m_Delay))
            {
                if ((this.m_MaxExecute != -1) && (this.m_CurExecute++ >= this.m_MaxExecute))
                {
                    this.m_State = false;
                    return false;
                }
                if (this.m_OnTick != null)
                {
                    this.m_OnTick(this);
                }
                this.m_LastExecute = ticks;
            }
            return true;
        }

        public override string ToString()
        {
            return string.Format("Delay: {0}ms MaxExecute: {1} Running: {2} Target: {3}", new object[] { this.m_Delay, this.m_MaxExecute, this.m_State, this.m_OnTick.Method.Name });
        }

        public int Delay
        {
            get
            {
                return this.m_Delay;
            }
            set
            {
                this.m_Delay = value;
            }
        }

        public bool Enabled
        {
            get
            {
                return this.m_State;
            }
        }
    }
}

