namespace Client
{
    using System;
    using System.Windows.Forms;

    public class Plugin
    {
        public virtual bool OnCommandEntered(string s)
        {
            return false;
        }

        public virtual bool OnKeyDown(KeyEventArgs e)
        {
            return false;
        }

        public virtual bool OnMacroAction(string parms)
        {
            return true;
        }

        public virtual bool OnPacketRecv(byte[] data)
        {
            return false;
        }

        public virtual bool OnPacketSend(byte[] data)
        {
            return false;
        }

        public virtual void Run(IExposed e)
        {
        }

        public virtual string Description
        {
            get
            {
                return "<none>";
            }
        }

        public virtual string Name
        {
            get
            {
                return "<no name>";
            }
        }
    }
}

