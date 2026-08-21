using System;

public readonly struct PlayerColor
{
    public byte Red {get;}
    public byte Green {get;}
    public byte Blue {get;}

    public PlayerColor(Byte red, Byte green, byte blue)
    {
        Red = red;
        Green = green;
        Blue = blue;
    }
}