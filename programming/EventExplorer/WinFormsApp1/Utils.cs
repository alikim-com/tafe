
namespace utils;

using System.Collections;
using System.Text.Json;
using System.Text.RegularExpressions;

public class Utils
{
    static public void Msg(object obj)
    {
        string outp = "";

        var type = obj.GetType();
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
        {
            Type[] genArgs = type.GetGenericArguments();
            Type keyType = genArgs[0];
            Type valueType = genArgs[1];
            if (
            keyType.GetMethod("ToString")?.DeclaringType != typeof(object) &&
            valueType.GetMethod("ToString")?.DeclaringType != typeof(object)
            ) {
                dynamic genDict = (dynamic)obj;
                foreach (var rec in genDict)
                    outp += $"{rec.Key}: {rec.Value}";
            }

        } else {
            outp = obj.ToString() ?? "";
        }
        if(outp != "") MessageBox.Show(outp);
    }

    static public void Msg(object[] obj)
    {
        foreach (var o in obj)
            MessageBox.Show(o.ToString());
    }

    static public string StripComments(string inp)
    {
        var blockComments = @"/\*(.*?)\*/";
        var lineComments = @"//(.*?)\r?\n";
        var strings = @"""((\\[^\n]|[^""\n])*)""";
        var verbatimStrings = @"@(""[^""]*"")+";

        return
        Regex.Replace
        (
            inp,
            blockComments + "|" + lineComments + "|" + strings + "|" + verbatimStrings,
            me =>
            {
                if (me.Value.StartsWith("/*") || me.Value.StartsWith("//"))
                    return me.Value.StartsWith("//") ? Environment.NewLine : "";

                return me.Value;
            },
            RegexOptions.Singleline
        );
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


