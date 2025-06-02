# XLuaMore

轻量级的 XLua 扩展库，帮你优雅管理 Lua 和 C# 委托绑定，自动生成配置，减少重复劳动。  
专门为了Lua映射为c#方法制作。

！！！要与XLua核心一起使用
---

## 特性

- 通过特性 `[LuaDelegate]` 标记委托，自动收集并生成配置代码  
- 通过特性 `[LuaRegister]` 标记含有Lua转c#委托的类，好在LuaManager统一赋值
  
- 支持 `Action` 和 `Func` 各种泛型委托，包括自定义类型参数  
- 避免了直接在 Lua 里复杂调用和互相依赖，简化调用逻辑，降低出错概率  
- 轻量无侵入，不改变 XLua 原有结构，易于集成和维护
- 提供LuaManager 用来通一管理Lua虚拟机，里面还有很多方法，可以提供更“手动”的lua操作


---

## 使用方法
1.给目标类打上特性`[LuaRegister]` 

    ```csharp
    [LuaRegister]
    public class Main()
    ```
2. 在 C# 代码中用 `[LuaDelegate]` 标记你的静态委托字段，例如：

    ```csharp
    [LuaDelegate]
    private static Action testDel;
    [LuaDelegate]
    private static Action<int> intAction;
    ```

3. 实现一个静态注册方法，调用自动生成的配置注册 Lua 函数：

    ```csharp
    

    public static void Register()
    {
        LuaManager.DoString("Main");
        LuaManager.GetLuaDelegate(ref testDel, "OnStart");
        LuaManager.GetLuaDelegate(ref intAction, "PrintInt");
        // ...
        LuaManager.Unload("Main");
    }
    
    ```
4.完成代码编写后，在编辑器顶部工具栏,
先XLua->Clean Generate Code 清除自动生成的代码，
再 XLua->Generate XLuaDelegate Registration 生成Lua的映射配置到LuaManager文件夹， 
最后再Xlua->Generate Code 生成XLua辅助代码

5. 在启动时调用 `LuaManager.AutoRegisterAll()` 方法，即可完成 Lua 函数与 C# 委托的绑定。

具体使用可以参考 release 的 unitypackage 里的Main.cs脚本
另外 Lua 脚本我放在 StreamingAssets/Lua 里面
---

## 设计理念

> 把 Lua 仅当作“轻量流程控制器”和“委托函数载体”，不要依赖 Lua 内部复杂调用链。  
> 这样能避免顺序错误和激活时机问题，让 C# 主导逻辑，Lua 只是影子和配置。




