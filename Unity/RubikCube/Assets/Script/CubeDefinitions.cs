using System;

// Custom attribute to hold color data
[AttributeUsage(AttributeTargets.Field)]
public class FaceColorAttribute : Attribute
{
    public CubeColor Color { get; private set; }

    public FaceColorAttribute(CubeColor color)
    {
        Color = color;
    }
}

public enum CubeColor
{
    White,
    Yellow,
    Green,
    Blue,
    Orange,
    Red,
    None
}

public enum CubeFace
{
    [FaceColor(CubeColor.White)]
    Front,

    [FaceColor(CubeColor.Yellow)]
    Back,

    [FaceColor(CubeColor.Green)]
    Left,

    [FaceColor(CubeColor.Blue)]
    Right,

    [FaceColor(CubeColor.Orange)]
    Top,

    [FaceColor(CubeColor.Red)]
    Bottom
}

public static class CubeFaceExtensions
{
    public static CubeColor GetColor(this CubeFace face)
    {
        var memberInfo = typeof(CubeFace).GetMember(face.ToString())[0];
        var attribute = (FaceColorAttribute)memberInfo.GetCustomAttributes(typeof(FaceColorAttribute), false)[0];
        return attribute.Color;
    }
}
