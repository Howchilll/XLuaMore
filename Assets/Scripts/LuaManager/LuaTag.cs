using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)] 
public class LuaDelegate :Attribute { }

[AttributeUsage(AttributeTargets.Class)]
public class LuaRegister : Attribute {}