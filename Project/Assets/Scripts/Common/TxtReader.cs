using System.IO;

public class TxtReader
{
    public static string read(string file)
    {
        StreamReader reader = new StreamReader(file);
        string text = "";
        string line;

        while (!reader.EndOfStream)
        {
            line = reader.ReadLine();
            while (line.Equals("") || line[0] == '#' || line[0] == '\n')
                if (!reader.EndOfStream)
                    line = reader.ReadLine();
                else
                    return text;
            if (!reader.EndOfStream)
                text += line + "\n";
            else
                text += line;
        }

        return text;
    }
}
