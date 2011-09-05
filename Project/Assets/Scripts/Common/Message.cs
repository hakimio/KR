

public class Message
{
    private string name, content;
    public Message(string name, string content)
    {
        this.name = name;
        this.content = content;
    }

    public string Name
    {
        get
        {
            return name;
        }
    }

    public string Content
    {
        get
        {
            return content;
        }
    }
}
