print("first Lua11!")
function OnStart()
    print("Lua OnStart called!")
end

function PrintInt(v)
    print("Received int:", v)
end

function ReportDamage(name, damage)
    print(name .. " took " .. damage .. " damage.")
end

function Add(a, b)
    return a + b
end

function GetWelcome(name)
    return "Welcome, " .. name
end