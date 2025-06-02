using System;
using UnityEngine;
using XLua;


[LuaRegister]
public class Main : MonoBehaviour
{
    [LuaDelegate]
    private static Action testDel;
    [LuaDelegate]
    private static Action<int> intAction;
    [LuaDelegate]
    private static Action<string, double> complex;
    [LuaDelegate]
    private static Func<int, int, int> addFunc;
    [LuaDelegate]
    private static Func<string, string> welcomeFunc;
    
    private static string LuaName => "Main";
    private static void LuaRegister()
    {
        LuaManager.DoString(LuaName);
        LuaManager.GetLuaDelegate(ref testDel, "OnStart");
        LuaManager.GetLuaDelegate(ref intAction, "PrintInt");
        LuaManager.GetLuaDelegate(ref complex, "ReportDamage");
        LuaManager.GetLuaDelegate(ref addFunc, "Add");
        LuaManager.GetLuaDelegate(ref welcomeFunc, "GetWelcome");
        LuaManager.Unload(LuaName);
    }



    


    void Start()
    {
        LuaManager.AutoRegisterAll();
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            testDel();
            intAction(14);
            complex("414",13.2);
           Debug.Log(addFunc(13,1));
           Debug.Log(welcomeFunc("Lll"));
        }
        
    }



}