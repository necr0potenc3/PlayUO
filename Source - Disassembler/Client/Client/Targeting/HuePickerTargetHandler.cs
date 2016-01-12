namespace Client.Targeting
{
    using Client;

    public class HuePickerTargetHandler : ITargetHandler
    {
        private GBrightnessBar m_Bar;
        private GHuePicker m_Picker;

        public HuePickerTargetHandler(GHuePicker picker, GBrightnessBar bar)
        {
            this.m_Picker = picker;
            this.m_Bar = bar;
        }

        public void OnCancel(TargetCancelType type)
        {
            Engine.AddTextMessage("Hue picker request canceled.");
        }

        public void OnTarget(object o)
        {
            if ((o is Item) || (o is StaticTarget))
            {
                int num = 0;
                if (o is Item)
                {
                    num = ((Item)o).Hue & 0x3fff;
                }
                else
                {
                    num = ((StaticTarget)o).Hue.HueID() & 0x3fff;
                }
                if ((num >= 2) && (num < 0x3ea))
                {
                    num -= 2;
                    int num2 = num % 5;
                    num /= 5;
                    int num3 = num % 20;
                    num /= 20;
                    int num4 = num;
                    this.m_Picker.Brightness = num2;
                    this.m_Picker.ShadeX = num3;
                    this.m_Picker.ShadeY = num4;
                    this.m_Bar.Refresh();
                }
                else if (num >= 2)
                {
                    Engine.AddTextMessage("You cannot figure out the proper dye mixture for that color.");
                }
                else
                {
                    Engine.AddTextMessage("Do you think that is colorful?");
                }
            }
            else
            {
                Engine.TargetHandler = this;
            }
        }
    }
}