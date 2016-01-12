namespace Client.Prompts
{
    public interface IPrompt
    {
        void OnCancel(PromptCancelType type);

        void OnReturn(string message);
    }
}