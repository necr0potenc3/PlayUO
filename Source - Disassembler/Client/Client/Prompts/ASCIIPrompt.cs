namespace Client.Prompts
{
    using Client;

    public class ASCIIPrompt : IPrompt
    {
        private int m_Prompt;
        private int m_Serial;
        private string m_Text;

        public ASCIIPrompt(int serial, int prompt, string text)
        {
            this.m_Serial = serial;
            this.m_Prompt = prompt;
            if (((this.m_Text = this.Text) != null) && ((this.m_Text = this.m_Text.Trim()).Length > 0))
            {
                Engine.AddTextMessage(this.m_Text, (float)12.5f);
            }
            else
            {
                this.m_Text = "";
            }
        }

        public void OnCancel(PromptCancelType type)
        {
            if (type == PromptCancelType.UserCancel)
            {
                Network.Send(new PPrompt_Cancel_ASCII(this.m_Serial, this.m_Prompt));
            }
        }

        public void OnReturn(string message)
        {
            Network.Send(new PPrompt_Reply_ASCII(this.m_Serial, this.m_Prompt, message));
        }

        public int Prompt
        {
            get
            {
                return this.m_Prompt;
            }
        }

        public int Serial
        {
            get
            {
                return this.m_Serial;
            }
        }

        public string Text
        {
            get
            {
                return this.m_Text;
            }
        }
    }
}