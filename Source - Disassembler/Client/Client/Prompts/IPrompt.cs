namespace Client.Prompts
{
    using System;

    public interface IPrompt
    {
        void OnCancel(PromptCancelType type);
        void OnReturn(string message);
    }
}

