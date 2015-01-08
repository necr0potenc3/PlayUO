namespace Client
{
    using System;

    public class GMessageBoxYesNo : GDragable
    {
        private MBYesNoCallback m_Callback;

        public GMessageBoxYesNo(string prompt, bool modal, MBYesNoCallback callback) : base(0x816, 0, 0)
        {
            this.m_Callback = callback;
            this.Center();
            base.m_CanClose = false;
            base.m_Children.Add(new GLabel(prompt, Engine.GetFont(1), Hues.Load(0x39f), 0x21, 0x1b));
            base.m_Children.Add(new GMBYNButton(this, 0x817, 0x25, false));
            base.m_Children.Add(new GMBYNButton(this, 0x81a, 100, true));
            Gumps.Modal = this;
        }

        protected virtual void OnSignal(bool response)
        {
        }

        public void Signal(bool response)
        {
            this.OnSignal(response);
            if (this.m_Callback != null)
            {
                this.m_Callback(this, response);
            }
        }

        private class GMBYNButton : GButtonNew
        {
            private GMessageBoxYesNo m_Owner;
            private bool m_Response;

            public GMBYNButton(GMessageBoxYesNo owner, int gumpID, int x, bool response) : base(gumpID, gumpID + 2, gumpID + 1, x, 0x4b)
            {
                this.m_Owner = owner;
                this.m_Response = response;
                base.m_CanEnter = response;
            }

            protected override void OnClicked()
            {
                this.m_Owner.Signal(this.m_Response);
                Gumps.Destroy(this.m_Owner);
            }
        }
    }
}

