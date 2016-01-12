namespace Client
{
    using Client.Targeting;

    public class AutoTargetSession
    {
        public TargetAction m_Action;
        public object m_Entity;
        public Timer m_Timer;

        public AutoTargetSession(object entity, TargetAction action)
        {
            this.m_Entity = entity;
            this.m_Action = action;
            this.m_Timer = new Timer(new OnTick(Engine.AutoTarget_Expire), 0x9c4, 1);
            this.m_Timer.SetTag("Session", this);
            this.m_Timer.Start(false);
        }
    }
}