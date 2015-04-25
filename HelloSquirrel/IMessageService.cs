namespace HelloSquirrel
{
    public class InputMessageResult
    {
        public InputMessageResult(string input, MessageResult messageResult)
        {
            Input = input;
            MessageResult = messageResult;
        }

        public string Input { get; private set; }

        public MessageResult MessageResult { get; private set; }
    }

    public enum MessageResult
    {
        None = 0,
        Ok = 1,
        Cancel = 2,
        Yes = 6,
        No = 7
    }

    public enum MessageButton
    {
        Ok = 0,
        OkCancel = 1,
        YesNoCancel = 3,
        YesNo = 4,
    }

    public enum MessageImage
    {
        None = 0,
        Error = 16,
        Question = 32,
        Warning = 48,
        Information = 64,
    }

    public interface IMessageService
    {
        MessageResult Show(string message, string caption = "", MessageButton button = MessageButton.Ok,
            MessageImage icon = MessageImage.None);
    }
}
