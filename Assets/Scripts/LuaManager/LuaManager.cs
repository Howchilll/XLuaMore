using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using XLua;

public static class LuaManager
{
    
    private static LuaFunction _luaFunc;
    
    private static LuaEnv _le;
    private static LuaEnv LuaEnv => _le ??= InitLuaEnv();
    public static LuaTable Global => LuaEnv.Global;
    public static bool IsInited => _le != null;

    public static void ActionLua(string funcName)
    {
        _luaFunc=Global.Get<LuaFunction>(funcName);
        _luaFunc?.Call();
    }
    public static void ActionLua(string funcName, params object[] args)
    {
        _luaFunc = Global.Get<LuaFunction>(funcName);
        _luaFunc?.Call(args);
    }
    public static T FunctionLua<T>(string funcName)
    {
        _luaFunc = Global.Get<LuaFunction>(funcName);
        var result = _luaFunc?.Call();
        if (result != null && result.Length > 0 && result[0] != null)
        {
            return (T)Convert.ChangeType(result[0], typeof(T));
        }
        return default(T);
    }
    public static T FunctionLua<T>(string funcName, params object[] args)
    {
        _luaFunc = Global.Get<LuaFunction>(funcName);
        var result = _luaFunc?.Call(args);
        if (result != null && result.Length > 0)
        {
            return (T)Convert.ChangeType(result[0], typeof(T));
        }
        return default(T);
    }
    
    
    public static void GetLuaDelegate<T>(ref T del, string funcName) where T : Delegate
    {
        LuaFunction luaFunc = Global.Get<LuaFunction>(funcName);
        if (luaFunc == null) 
        {
            Debug.LogWarning($"Lua function '{funcName}' not found!");
            del = null;
            return;
        }
        del = luaFunc.Cast<T>();
    }
    
    private static LuaEnv InitLuaEnv()
    {
        var env = new LuaEnv();
        env.AddLoader(FindLua);
        return env;
    }
    public static T GetValue<T>(string name)
    {
        return Global.Get<T>(name);
    }

    public static void SetValue<T>(string name, T value)
    {
        Global.Set(name, value);
    }
    
    private static byte[] FindLua(ref string filename)
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Lua", filename + ".lua");
        if (File.Exists(path)) return File.ReadAllBytes(path);
        Debug.Log("MyCustomLoader 重定向失败，文件名为: " + filename);
        return null;
    }

    public static void DoString(string str, bool require = true)
    {
        if (require) LuaEnv.DoString($"require \"{str}\"");
        else LuaEnv.DoString(str);
        
    }

    public static void Unload(string filename)
    {
        _le.DoString($"package.loaded['{filename}'] = nil");
        _le.DoString($"_G['{filename}'] = nil");
        _le.DoString("collectgarbage()");
    }
    
    public static void Tick()
    {
        if (_le != null) LuaEnv.Tick(); 
        
    }

    public static void Dispose()
    {
        if (_le != null)
        {
            _le.Dispose();
            _le = null;
        }
    }
    
    public static void AutoRegisterAll()
    {
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.GetCustomAttributes(typeof(LuaRegister), false).Length > 0);

        foreach (var type in types)
        {
            var method = type.GetMethod("LuaRegister", BindingFlags.NonPublic | BindingFlags.Static);
            if (method != null)
            {
                method.Invoke(null, null);
            }
            else
            {
                UnityEngine.Debug.LogWarning($"[LuaRegister] {type.Name} 没有找到静态 LuaRegister 方法");
            }
        }
    }
}
    
    

