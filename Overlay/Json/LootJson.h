#pragma once
#include "pch.h"
class LootJson
{

public:
    LootJson()
    {

    }
    std::string Type = "Loot";
    float X;
    float Y;
    std::string Name;

    void ToJson(json& j) const
    {
        j = json{
            {"Type", Type},
            {"X", X},
            {"Y", Y},
            {"Name",Name}
        };
    }

    // Convert JSON to Entity object
    void FromJson(const json& j)
    {
        Type = j["Type"];
        X = j["X"];
        Y = j["Y"];
        Name = j["Name"];
    }
};