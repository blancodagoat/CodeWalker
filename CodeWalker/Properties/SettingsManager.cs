using System;
using System.Collections.Specialized;
using System.IO;
using System.Xml.Linq;

namespace CodeWalker.Properties;

public static class SettingsManager
{
    private static readonly string SettingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Settings.xml");
    private static XDocument? _doc;
    private static bool _loaded = false;

    private static void EnsureLoaded()
    {
        if (_loaded) return;
        
        if (File.Exists(SettingsPath))
        {
            try
            {
                _doc = XDocument.Load(SettingsPath);
            }
            catch
            {
                _doc = CreateDefaultDocument();
            }
        }
        else
        {
            _doc = CreateDefaultDocument();
        }
        
        _loaded = true;
    }

    private static XDocument CreateDefaultDocument()
    {
        return new XDocument(
            new XElement("Settings")
        );
    }

    public static string GetString(string key, string defaultValue = "")
    {
        EnsureLoaded();
        var element = _doc?.Root?.Element(key);
        return element?.Value ?? defaultValue;
    }

    public static bool GetBool(string key, bool defaultValue = false)
    {
        EnsureLoaded();
        var element = _doc?.Root?.Element(key);
        if (element == null) return defaultValue;
        return bool.TryParse(element.Value, out var result) ? result : defaultValue;
    }

    public static int GetInt(string key, int defaultValue = 0)
    {
        EnsureLoaded();
        var element = _doc?.Root?.Element(key);
        if (element == null) return defaultValue;
        return int.TryParse(element.Value, out var result) ? result : defaultValue;
    }

    public static long GetLong(string key, long defaultValue = 0)
    {
        EnsureLoaded();
        var element = _doc?.Root?.Element(key);
        if (element == null) return defaultValue;
        return long.TryParse(element.Value, out var result) ? result : defaultValue;
    }

    public static float GetFloat(string key, float defaultValue = 0f)
    {
        EnsureLoaded();
        var element = _doc?.Root?.Element(key);
        if (element == null) return defaultValue;
        return float.TryParse(element.Value, out var result) ? result : defaultValue;
    }

    public static double GetDouble(string key, double defaultValue = 0.0)
    {
        EnsureLoaded();
        var element = _doc?.Root?.Element(key);
        if (element == null) return defaultValue;
        return double.TryParse(element.Value, out var result) ? result : defaultValue;
    }

    public static StringCollection GetStringCollection(string key)
    {
        EnsureLoaded();
        var collection = new StringCollection();
        var element = _doc?.Root?.Element(key);
        if (element != null)
        {
            foreach (var item in element.Elements("Item"))
            {
                collection.Add(item.Value);
            }
        }
        return collection;
    }

    public static void SetString(string key, string value)
    {
        EnsureLoaded();
        if (_doc?.Root == null) return;
        
        var element = _doc.Root.Element(key);
        if (element == null)
        {
            element = new XElement(key);
            _doc.Root.Add(element);
        }
        element.Value = value ?? string.Empty;
    }

    public static void SetBool(string key, bool value)
    {
        SetString(key, value.ToString());
    }

    public static void SetInt(string key, int value)
    {
        SetString(key, value.ToString());
    }

    public static void SetLong(string key, long value)
    {
        SetString(key, value.ToString());
    }

    public static void SetFloat(string key, float value)
    {
        SetString(key, value.ToString());
    }

    public static void SetDouble(string key, double value)
    {
        SetString(key, value.ToString());
    }

    public static void SetStringCollection(string key, StringCollection value)
    {
        EnsureLoaded();
        if (_doc?.Root == null) return;
        
        var element = _doc.Root.Element(key);
        if (element == null)
        {
            element = new XElement(key);
            _doc.Root.Add(element);
        }
        else
        {
            element.RemoveAll();
        }

        foreach (string? item in value)
        {
            if (item != null)
            {
                element.Add(new XElement("Item", item));
            }
        }
    }

    public static void Save()
    {
        try
        {
            EnsureLoaded();
            _doc?.Save(SettingsPath);
        }
        catch
        {
            // Silently fail if we can't save
        }
    }

    public static void Reset()
    {
        try
        {
            if (File.Exists(SettingsPath))
            {
                File.Delete(SettingsPath);
            }
            _doc = CreateDefaultDocument();
            _loaded = true;
        }
        catch
        {
            // Silently fail if we can't reset
        }
    }
}
