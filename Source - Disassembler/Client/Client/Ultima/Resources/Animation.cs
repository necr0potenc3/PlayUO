namespace Client
{
    public class Animation
    {
        private int m_Action;
        private Client.OnAnimationEnd m_AnimEnd;
        private int m_Delay;
        private bool m_Forward;
        private bool m_Repeat;
        private int m_RepeatCount;
        private bool m_Running;
        private int m_Start;

        public void Run()
        {
            this.m_Running = true;
            this.m_Start = Renderer.m_Frames;
        }

        public void Stop()
        {
            this.m_Running = false;
        }

        public int Action
        {
            get
            {
                return this.m_Action;
            }
            set
            {
                this.m_Action = value;
            }
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

        public bool Forward
        {
            get
            {
                return this.m_Forward;
            }
            set
            {
                this.m_Forward = value;
            }
        }

        public Client.OnAnimationEnd OnAnimationEnd
        {
            get
            {
                return this.m_AnimEnd;
            }
            set
            {
                this.m_AnimEnd = value;
            }
        }

        public bool Repeat
        {
            get
            {
                return this.m_Repeat;
            }
            set
            {
                this.m_Repeat = value;
            }
        }

        public int RepeatCount
        {
            get
            {
                return this.m_RepeatCount;
            }
            set
            {
                this.m_RepeatCount = value;
            }
        }

        public bool Running
        {
            get
            {
                return this.m_Running;
            }
        }

        public int Start
        {
            get
            {
                return this.m_Start;
            }
            set
            {
                this.m_Start = value;
            }
        }
    }
}