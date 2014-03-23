using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace OSUplayer
{
    class Language
    {
        private Dictionary<string, string> content = new Dictionary<string, string>();
        public string Get(string name)
        {
            return content.ContainsKey(name) ? content[name] : "";
        }
        public Language(Stream rawstream)
        {
            using (var raw = new StreamReader(rawstream))
            {
                var line = "";
                var pos = 0;
                while (!raw.EndOfStream)
                {
                    line = raw.ReadLine();
                    Debug.Assert(line != null, "languageline != null");
                    line = line.Replace(@"\n", "\n");
                    pos = line.IndexOf("=");
                    content.Add(line.Substring(0, pos), line.Substring(pos + 1));
                }
            }
        }
    }
    static class LanguageManager
    {
        private static Dictionary<string, Language> _language;
        private static string _current;
        public static string Current
        {
            private get
            {
                return _current;
            }
            set
            {
                _current = _language.ContainsKey(value) ? value : "English";
            }
        }
        public static IEnumerable<string> LanguageList
        {
            get { return _language.Keys.ToList(); }
        }
        public static string Get(string name)
        {
            var ret = _language[Current].Get(name);
            return ret != "" ? ret : _language["English"].Get(name);
        }
        public static void ApplyLanguage(System.Windows.Forms.Form oriForm)
        {
            foreach (var controlbase in (oriForm.GetType().GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic)))
            {
                var control = controlbase.GetValue(oriForm);
                if (control == null) continue;
                var name = control.GetType().GetProperty("Name");
                if (name == null) continue;
                string nameValue = controlbase.Name;
                //name.GetValue(control, null).ToString();
                var text = control.GetType().GetProperty("Text");
                if (text == null) continue;
                var totext = Get(nameValue + "_Text");
                if (totext == "") continue;
                text.SetValue(control, totext, null);
            }
        }
        public static void InitLanguage(string name)
        {
            _language = new Dictionary<string, Language>();
            var assembly = Assembly.GetExecutingAssembly();
            var rawlanguage = new Language(assembly.GetManifestResourceStream("OSUplayer.Lang.en.txt"));
            _language.Add(rawlanguage.Get("Language_name"), rawlanguage);
            rawlanguage = new Language(assembly.GetManifestResourceStream("OSUplayer.Lang.zh-CN.txt"));
            _language.Add(rawlanguage.Get("Language_name"), rawlanguage);
            rawlanguage = new Language(assembly.GetManifestResourceStream("OSUplayer.Lang.zh-TW.txt"));
            _language.Add(rawlanguage.Get("Language_name"), rawlanguage);
            var alllange = assembly.GetManifestResourceNames();
            if (alllange.Contains(string.Format("OSUplayer.Lang.{0}.txt", name)))
            {
                rawlanguage = new Language(assembly.GetManifestResourceStream(string.Format("OSUplayer.Lang.{0}.txt", name)));
                Current = rawlanguage.Get("Language_name");
            }
            else
            {
                Current = "";
            }
        }
    }
}