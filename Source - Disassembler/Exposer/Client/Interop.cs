namespace Client
{
    using System;

    public class Interop
    {
        private static IInterop m_Interop;

        public static void AddTimer(Timer t)
        {
            m_Interop.AddTimer(t);
        }

        public static object GetMobile(int Serial, string Name)
        {
            return m_Interop.GetMobile(Serial, Name);
        }

        public static int GetTicks()
        {
            return m_Interop.GetTicks();
        }

        public static void OpenStatus(int Serial, bool Drag)
        {
            m_Interop.OpenStatus(Serial, Drag);
        }

        public static void RemoveTimer(Timer t)
        {
            m_Interop.RemoveTimer(t);
        }

        public static void SetMobile(int Serial, string Name, object Value)
        {
            m_Interop.SetMobile(Serial, Name, Value);
        }

        public static IInterop Comm
        {
            get
            {
                return m_Interop;
            }
            set
            {
                m_Interop = value;
            }
        }
    }
}

