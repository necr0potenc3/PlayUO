namespace Client
{
    using System;
    using System.Windows.Forms;

    public class Macro : IComparable
    {
        private Action[] m_Actions;
        private static Macro m_Current;
        private DateTime m_DelayEnd;
        private int m_FileIndex;
        private int m_Index;
        private Keys m_Key;
        private Keys m_Mods;
        public const Keys WheelDown = 0x11001;
        public const Keys WheelPress = 0x11002;
        public const Keys WheelUp = 0x11000;

        public Macro(Keys key, Keys mods, Action[] actions)
        {
            this.m_Key = key;
            this.m_Mods = mods;
            this.m_Actions = actions;
            this.m_Index = -1;
        }

        public void AddAction(Action a)
        {
            Action[] actions = this.m_Actions;
            this.m_Actions = new Action[actions.Length + 1];
            for (int i = 0; i < actions.Length; i++)
            {
                this.m_Actions[i] = actions[i];
            }
            this.m_Actions[actions.Length] = a;
        }

        public bool CheckKey(Keys key)
        {
            Keys modifierKeys = System.Windows.Forms.Control.ModifierKeys;
            return ((key == this.m_Key) && (modifierKeys == this.m_Mods));
        }

        public int CompareTo(object obj)
        {
            Macro macro = (Macro) obj;
            if (this.IsWheelMacro() && !macro.IsWheelMacro())
            {
                return -1;
            }
            if (!this.IsWheelMacro() && macro.IsWheelMacro())
            {
                return 1;
            }
            return (this.m_FileIndex - macro.m_FileIndex);
        }

        public static bool Delay(int ms)
        {
            if (m_Current != null)
            {
                if (m_Current.m_DelayEnd == DateTime.MinValue)
                {
                    m_Current.m_DelayEnd = DateTime.Now + TimeSpan.FromMilliseconds((double) ms);
                    return false;
                }
                if (DateTime.Now >= m_Current.m_DelayEnd)
                {
                    m_Current.m_DelayEnd = DateTime.MinValue;
                    return true;
                }
                return false;
            }
            return true;
        }

        private bool GetMod(Keys key)
        {
            return ((this.m_Mods & key) != Keys.None);
        }

        public bool IsWheelMacro()
        {
            return (((this.m_Key == 0x11000) || (this.m_Key == 0x11001)) || (this.m_Key == 0x11002));
        }

        public static void Repeat()
        {
            if (m_Current != null)
            {
                m_Current.m_Index = -1;
            }
        }

        private void SetMod(Keys key, bool value)
        {
            if (value)
            {
                this.m_Mods |= key;
            }
            else
            {
                this.m_Mods &= ~key;
            }
        }

        public bool Slice()
        {
            m_Current = this;
            if (this.m_Index < this.m_Actions.Length)
            {
                Action action = this.m_Actions[this.m_Index];
                ActionHandler handler = action.Handler;
                if ((handler == null) || handler.Plugin.OnMacroAction(action.Param))
                {
                    this.m_Index++;
                }
            }
            if (this.m_Index >= this.m_Actions.Length)
            {
                this.m_Index = -1;
            }
            m_Current = null;
            return this.Running;
        }

        public void Start()
        {
            this.m_Index = 0;
            if (this.Slice())
            {
                Macros.Running.Add(this);
            }
        }

        public void Stop()
        {
            Macros.Running.Remove(this);
        }

        public Action[] Actions
        {
            get
            {
                return this.m_Actions;
            }
            set
            {
                this.m_Actions = value;
            }
        }

        public bool Alt
        {
            get
            {
                return this.GetMod(Keys.Alt);
            }
            set
            {
                this.SetMod(Keys.Alt, value);
            }
        }

        public bool Control
        {
            get
            {
                return this.GetMod(Keys.Control);
            }
            set
            {
                this.SetMod(Keys.Control, value);
            }
        }

        public int FileIndex
        {
            get
            {
                return this.m_FileIndex;
            }
            set
            {
                this.m_FileIndex = value;
            }
        }

        public Keys Key
        {
            get
            {
                return this.m_Key;
            }
            set
            {
                this.m_Key = value;
            }
        }

        public Keys Mods
        {
            get
            {
                return this.m_Mods;
            }
            set
            {
                this.m_Mods = value;
            }
        }

        public bool Running
        {
            get
            {
                return (this.m_Index >= 0);
            }
        }

        public bool Shift
        {
            get
            {
                return this.GetMod(Keys.Shift);
            }
            set
            {
                this.SetMod(Keys.Shift, value);
            }
        }
    }
}

