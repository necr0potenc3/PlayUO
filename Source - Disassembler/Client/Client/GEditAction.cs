namespace Client
{
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Windows.Forms;

    public class GEditAction : GWindowsForm
    {
        private Action m_Action;
        private Macro m_Macro;
        private GMacroEditorPanel m_Panel;

        public GEditAction(GMacroEditorPanel p, Macro macro, Action action) : base(0, 0, 0x67, 0x56)
        {
            this.m_Panel = p;
            this.m_Macro = macro;
            this.m_Action = action;
            Gumps.Modal = this;
            Gumps.Focus = this;
            base.Text = "Edit";
            base.m_NonRestrictivePicking = true;
            this.AddButton("↑", 6, 7, 0x18, 0x18, new OnClick(this.Up_OnClick)).Tooltip = new Tooltip("Moves the instruction up", true);
            this.AddButton("↓", 6, 30, 0x18, 0x18, new OnClick(this.Down_OnClick)).Tooltip = new Tooltip("Moves the instruction down", true);
            this.AddButton("Delete", 0x27, 7, 50, 0x18, new OnClick(this.Delete_OnClick)).Tooltip = new Tooltip("Removes the instruction", true);
            this.Center();
        }

        private GSystemButton AddButton(string name, int x, int y, int w, int h, OnClick onClick)
        {
            GSystemButton toAdd = new GSystemButton(x, y, w, h, SystemColors.Control, SystemColors.ControlText, name, Engine.GetUniFont(2)) {
                OnClick = onClick
            };
            base.Client.Children.Add(toAdd);
            return toAdd;
        }

        private GTextBox AddTextBox(string name, int index, string initialText, char pc)
        {
            GLabel label;
            int y = 30 + (index * 0x19);
            label = new GLabel(name, Engine.GetUniFont(2), GumpHues.ControlText, 0, 0) {
                X = 10 - label.Image.xMin,
                Y = (y + ((0x16 - ((label.Image.yMax - label.Image.yMin) + 1)) / 2)) - label.Image.yMin
            };
            base.m_Children.Add(label);
            GAlphaBackground toAdd = new GAlphaBackground(60, y, 200, 0x16) {
                ShouldHitTest = false,
                FillColor = GumpColors.Window,
                FillAlpha = 1f
            };
            base.m_Children.Add(toAdd);
            IHue windowText = GumpHues.WindowText;
            GTextBox box = new GTextBox(0, false, 60, y, 200, 0x16, initialText, Engine.GetUniFont(2), windowText, windowText, windowText, pc);
            base.Client.Children.Add(box);
            return box;
        }

        private void Delete_OnClick(Gump g)
        {
            ArrayList list = new ArrayList(this.m_Macro.Actions);
            list.Remove(this.m_Action);
            this.m_Macro.Actions = (Action[]) list.ToArray(typeof(Action));
            GMacroEditorForm parent = this.m_Panel.Parent.Parent as GMacroEditorForm;
            if (parent != null)
            {
                parent.Current = parent.Current;
            }
            Gumps.Destroy(this);
            Gumps.Focus = parent;
        }

        private void Down_OnClick(Gump g)
        {
            ArrayList list = new ArrayList(this.m_Macro.Actions);
            int index = list.IndexOf(this.m_Action);
            if (index < (list.Count - 1))
            {
                list.RemoveAt(index);
                list.Insert(index + 1, this.m_Action);
            }
            this.m_Macro.Actions = (Action[]) list.ToArray(typeof(Action));
            GMacroEditorForm parent = this.m_Panel.Parent.Parent as GMacroEditorForm;
            if (parent != null)
            {
                parent.Current = parent.Current;
            }
        }

        protected internal override void OnMouseDown(int X, int Y, MouseButtons mb)
        {
            if (mb == MouseButtons.Right)
            {
                Gumps.Destroy(this);
            }
        }

        private void Up_OnClick(Gump g)
        {
            ArrayList list = new ArrayList(this.m_Macro.Actions);
            int index = list.IndexOf(this.m_Action);
            if (index > 0)
            {
                list.RemoveAt(index);
                list.Insert(index - 1, this.m_Action);
            }
            this.m_Macro.Actions = (Action[]) list.ToArray(typeof(Action));
            GMacroEditorForm parent = this.m_Panel.Parent.Parent as GMacroEditorForm;
            if (parent != null)
            {
                parent.Current = parent.Current;
            }
        }
    }
}

