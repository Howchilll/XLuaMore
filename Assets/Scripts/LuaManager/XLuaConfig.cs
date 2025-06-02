
using System;
using System.Collections.Generic;
using XLua;

public static class XLuaConfig
{
    [CSharpCallLua]
    public static List<Type> CSharpCallLuaTypes = new List<Type>()
    {
        typeof(System.Action),
        typeof(System.Action<System.Int32>),
        typeof(System.Action<System.String, System.Double>),
        typeof(System.Func<System.Int32, System.Int32, System.Int32>),
        typeof(System.Func<System.String, System.String>),
    };
}