﻿
namespace utils;

using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;

public class Utils
{
    static public TEnum SafeEnumFromStr<TEnum>(string enm, string caller = "") where TEnum : struct
    {
        if (!Enum.TryParse(enm, out TEnum outp))
            throw new Exception($"{caller} : couldn't find enum '{enm}' of type '{typeof(TEnum)}'");

        return outp;
    }

    static public TValue SafeDictValue<TKey, TValue>(Dictionary<TKey, TValue> dict, TKey key, string caller = "") where TKey : notnull
    {
        if (!dict.TryGetValue(key, out TValue? outp))
            throw new Exception($"{caller}.SafeDictValue : key '{key}' not found");
        if (outp == null)
            throw new Exception($"{caller}.SafeDictValue : null value for the key '{key}'");
        return outp;
    }

    static public void Msg(object? obj)
    {
        if (obj == null)
        {
            MessageBox.Show("null\n");
            return;
        }

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
            )
            {
                dynamic dynObj = (dynamic)obj;
                foreach (var rec in dynObj)
                    outp += $"{rec.Key}: {rec.Value}\n";
            }

        } else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
        {

            Type[] genArgs = type.GetGenericArguments();
            Type itmType = genArgs[0];
            if (itmType.GetMethod("ToString")?.DeclaringType != typeof(object))
            {
                dynamic dynObj = (dynamic)obj;
                foreach (var rec in dynObj)
                    outp += rec.ToString() + '\n';
            }

        } else
        {

            outp = obj.ToString() ?? "";
        }

        if (outp != "") MessageBox.Show(outp + '\n');
    }

    static public void Msg(object[] obj)
    {
        foreach (var o in obj) Msg(o);
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

    static public List<string> GetProfileNames<P>(string profPath) where P : INamedProfile
    {
        List<string> names = new();

        string[] files = ReadFolder(profPath);

        foreach (string fname in files)
        {
            var input = ReadFile(profPath, fname);
            var obj = JsonSerializer.Deserialize<P>(input);
            if (obj == null) continue;

            var name = obj.Name;
            if (string.IsNullOrEmpty(name)) continue;
            names.Add(name);
        }

        return names;
    }

    static public string OpenLoadFileDialog(string profPath)
    {
        using OpenFileDialog openFileDialog = new();

        openFileDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
        openFileDialog.Title = "Load game";
        openFileDialog.InitialDirectory = profPath;

        DialogResult result = openFileDialog.ShowDialog();

        if (result == DialogResult.OK)
        {
            string selectedFilePath = openFileDialog.FileName;
            string outp = ReadFile(profPath, selectedFilePath);
            return outp;
        }

        return "";
    }

    static public P? LoadProfileFromString<P>(string input)
    {
        var obj = JsonSerializer.Deserialize<P>(input);
        if (obj == null)
        {
            Msg($"Utils.LoadProfileFromString : input string '{input}' produces null object");
            return default;
        }
        return obj;
    }

    static public P? LoadProfileByName<P>(string pname, string profPath) where P : INamedProfile
    {
        string[] files = ReadFolder(profPath);

        foreach (string pfile in files)
        {
            var input = ReadFile(profPath, pfile);
            var obj = JsonSerializer.Deserialize<P>(input);
            if (obj == null) continue;

            if (obj.Name == pname) return obj;
        }

        Msg($"Utils.LoadProfileByName : No profile with the name '{pname}' found in '{profPath}'");
        return default;
    }

    public interface INamedProfile
    {
        string Name { get; set; }
    }
}


