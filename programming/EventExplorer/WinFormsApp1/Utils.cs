
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
            Msg($"Utils.ReadFile : An error occurred: {ex.Message}");
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
            Msg($"Utils.WriteFile : An error occurred: {ex.Message}");
        }
    }

    static public string[] ReadFolder(string path)
    {
        try
        {
            return Directory.GetFiles(path).Select(e => Path.GetFileName(e) ?? "").ToArray();
        }
        catch (Exception ex)
        {
            Msg($"Utils.ReadFolder : An error occurred: {ex.Message}");
            return Array.Empty<string>();
        }
    }

    static public void SaveProfile<P>(string profPath, string fName, P prof)
    {
        string outp = JsonSerializer.Serialize(prof);

        OpenSaveFileDialog(profPath, fName, outp);
    }

    static public void OpenSaveFileDialog(string profPath, string fName, string outp)
    {
        using SaveFileDialog saveFileDialog = new();
        
        saveFileDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
        saveFileDialog.Title = "Save As";
        saveFileDialog.InitialDirectory = profPath;
        saveFileDialog.FileName = fName;

        DialogResult result = saveFileDialog.ShowDialog();

        if (result == DialogResult.OK)
        {
            string selectedFilePath = saveFileDialog.FileName;
            WriteFile(selectedFilePath, "", outp);
        }
    }

    static public P? LoadProfile<P>(string pfile, string profPath)
    {
        try
        {
            var input = ReadFile(profPath, pfile);

            var obj = JsonSerializer.Deserialize<P>(input);

            return obj;
        }
        catch (Exception ex)
        {
            Msg($"Utils.LoadProfile : There was an error <{ex.Message}> reading '{pfile}'");
            return default;
        }
    }
}


