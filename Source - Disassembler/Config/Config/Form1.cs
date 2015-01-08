namespace Config
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Resources;
    using System.Windows.Forms;

    public class Form1 : Form
    {
        private Container components = null;

        public Form1()
        {
            this.InitializeComponent();
            ConfigEntry[] entries = NewConfig.GetEntries();
            for (int i = 0; i < entries.Length; i++)
            {
                ConfigEntry entry = entries[i];
                Label label = new Label {
                    Text = entry.FriendlyName + ":",
                    Width = 120,
                    Location = new Point(5, 5 + (i * 0x16)),
                    Height = 20,
                    TextAlign = ContentAlignment.MiddleRight
                };
                base.Controls.Add(label);
                if (entry.Type == typeof(string))
                {
                    TextBox control = new TextBox {
                        Text = entry.Value.ToString(),
                        Location = new Point(130, 5 + (i * 0x16)),
                        Size = new Size(100, 20)
                    };
                    if (entry.Name == "Password")
                    {
                        control.PasswordChar = '*';
                    }
                    new ToolTip().SetToolTip(control, entry.Comment);
                    base.Controls.Add(control);
                    entry.Control = control;
                }
                else if (entry.Type == typeof(int))
                {
                    NumericUpDown down = new NumericUpDown {
                        Minimum = -2147483648M,
                        Maximum = 2147483647M,
                        Value = (int) entry.Value,
                        Size = new Size(100, 20),
                        Location = new Point(130, 5 + (i * 0x16))
                    };
                    new ToolTip().SetToolTip(down, entry.Comment);
                    base.Controls.Add(down);
                    entry.Control = down;
                }
                else if (entry.Type == typeof(bool))
                {
                    ComboBox box2 = new ComboBox {
                        Size = new Size(100, 20),
                        Location = new Point(130, 5 + (i * 0x16))
                    };
                    box2.Items.Add(true);
                    box2.Items.Add(false);
                    box2.DropDownStyle = ComboBoxStyle.DropDownList;
                    box2.SelectedIndex = ((bool) entry.Value) ? 0 : 1;
                    new ToolTip().SetToolTip(box2, entry.Comment);
                    base.Controls.Add(box2);
                    entry.Control = box2;
                }
            }
            Button button = new Button {
                Text = "&Okay",
                Size = new Size(0x40, 20),
                Location = new Point((base.ClientSize.Width - 5) - 0x40, (((base.ClientSize.Height - 5) - 20) - 2) - 20)
            };
            button.Click += new EventHandler(this.Okay_Click);
            button.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            base.Controls.Add(button);
            Button button2 = new Button {
                Text = "&Cancel",
                Size = new Size(0x40, 20),
                Location = new Point((base.ClientSize.Width - 5) - 0x40, (base.ClientSize.Height - 5) - 20),
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom
            };
            button2.Click += new EventHandler(this.Cancel_Click);
            base.Controls.Add(button2);
            base.CancelButton = button2;
            base.AcceptButton = button;
            base.FormBorderStyle = FormBorderStyle.Fixed3D;
            base.MaximizeBox = false;
            this.Text = "Configuration";
            base.ClientSize = new Size(0xeb, ((((5 + (entries.Length * 0x16)) + 10) + 20) + 2) + 20);
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            ResourceManager manager = new ResourceManager(typeof(Form1));
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x1b0, 0x31d);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "Form1";
            this.Text = "Form1";
        }

        [STAThread]
        private static void Main()
        {
            Application.Run(new Form1());
        }

        private void Okay_Click(object sender, EventArgs args)
        {
            foreach (ConfigEntry entry in NewConfig.GetEntries())
            {
                if (entry.Control is TextBox)
                {
                    entry.Value = ((TextBox) entry.Control).Text;
                }
                else if (entry.Control is NumericUpDown)
                {
                    entry.Value = (int) ((NumericUpDown) entry.Control).Value;
                }
                else if (entry.Control is ComboBox)
                {
                    entry.Value = ((ComboBox) entry.Control).SelectedIndex == 0;
                }
            }
            NewConfig.Save();
            base.Close();
        }
    }
}

