using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using XLua;

public static class LuaDelegateAutoRegister
{
    [MenuItem("XLua/Generate LuaDelegate Registration")]
    public static void GenerateRegistration()
    {
        HashSet<Type> delegateTypes = new HashSet<Type>();


        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            // 过滤编辑器等无关程序集，简化扫描
            if (assembly.FullName.StartsWith("UnityEditor")) continue;

            foreach (Type type in assembly.GetTypes())
            {
                
                var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
                foreach (var field in fields)
                {
                    if (field.GetCustomAttribute<LuaDelegate>() != null)
                    {
                        delegateTypes.Add(field.FieldType);
                    }
                }
            }
        }


        string code = @"
using System;
using System.Collections.Generic;
using XLua;

public static class XLuaConfig
{
    [CSharpCallLua]
    public static List<Type> CSharpCallLuaTypes = new List<Type>()
    {";

        foreach (var t in delegateTypes)
        {
            code += "\n        typeof(" + GetFriendlyName(t) + "),";
        }

        code += @"
    };
}";

        // 写文件到项目 Assets/XLuaConfig.cs
        string path = Application.dataPath + "/Scripts/LuaManager/XLuaConfig.cs";
        string directory = System.IO.Path.GetDirectoryName(path);
        if (!System.IO.Directory.Exists(directory))
        {
            System.IO.Directory.CreateDirectory(directory);
            Debug.Log($"Created directory: {directory}");
        }
        System.IO.File.WriteAllText(path, code);
        Debug.Log("XLuaConfig.cs generated with " + delegateTypes.Count + " delegate types.");
        AssetDatabase.Refresh();
    }

    // 辅助函数，把泛型类型名字写成正常C#形式，比如 Action<int> 而不是 Action`1
    private static string GetFriendlyName(Type type)
    {
        if (!type.IsGenericType)
            return type.FullName.Replace("+", ".");

        string mainName = type.GetGenericTypeDefinition().FullName;
        mainName = mainName.Substring(0, mainName.IndexOf('`'));
        Type[] args = type.GetGenericArguments();

        string argsString = string.Join(", ", args.Select(t => GetFriendlyName(t)));
        return mainName + "<" + argsString + ">";
    }
}
