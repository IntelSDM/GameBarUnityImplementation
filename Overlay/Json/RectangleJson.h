#pragma once
#include "pch.h"
class RectangleJson
{

public:
    RectangleJson(float x, float y, float width, float height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }
    std::string Type = "Rectangle";
    float X;
    float Y;
    float Width;
    float Height;

    void ToJson(json& j) const
    {
        j = json{
            {"Type", Type},
            {"X", X},
            {"Y", Y},
            {"Width",Width},
            {"Height",Height}
        };
    }

    // Convert JSON to Entity object
    void FromJson(const json& j)
    {
        Type = j["Type"];
        X = j["X"];
        Y = j["Y"];
        Width = j["Width"];
        Height = j["Height"];
    }
};