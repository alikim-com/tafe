
namespace utils;

using System.Text.Json;

public class Utils
{
    static public void Msg(object obj) => MessageBox.Show(obj.ToString());

    static public void Msg(object[] obj)
    {
        foreach (var o in obj)
            MessageBox.Show(o.ToString());
    }

    static public string ReadFile(string path, string name)
    {
        try
        {
            return File.ReadAllText(Path.Combine(path, name));
        }
        catch (Exception ex)
        {
            Msg($"An error occurred: {ex.Message}");
            return "";
        }
    }

    static public void WriteFile(string path, string name, string outp)
    {
        try
        {
            File.WriteAllText(Path.Combine(path, name), outp);
        }
        catch (Exception ex)
        {
            Msg($"An error occurred: {ex.Message}");
        }
    }

    static public string[] ReadFolder(string fpath)
    {
        try
        {
            return Directory.GetFiles(fpath).Select(e => Path.GetFileName(e) ?? "").ToArray();
        }
        catch (Exception ex)
        {
            Msg($"An error occurred: {ex.Message}");
            return Array.Empty<string>();
        }
    }

    static void SaveProfile(Profile prof, string path, string name)
    {
        string outp = JsonSerializer.Serialize(prof); 
        WriteFile(path, name, outp);
    }
}

public class BoxInfo
{
    string name;
    Point pos;
    Size size;

    public BoxInfo(string _name, Point _pos, Size _size)
    {
        name = _name;
        pos = _pos;
        size = _size;
    }
}

public class Profile
{
    readonly string name = "Default";
    readonly List<BoxInfo> boxes = new();

    public Profile(string _name, List<WinformsUMLEvents.ClassBox> _boxes)
    {
        name = _name;
        foreach(var _box in _boxes)
            boxes.Add(new BoxInfo(_box.name, _box.pos, _box.size));
    }
}
