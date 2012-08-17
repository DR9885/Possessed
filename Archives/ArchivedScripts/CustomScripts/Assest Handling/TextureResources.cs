using System.Linq;
using UnityEngine;

public class TextureResources : ResourceLoader<Texture2D>
{
    public Texture2D this[KeyCode argValue]
    {
        get
        {
            switch(argValue)
            {
                case KeyCode.Space: return this["ICON_space_small"];
                case KeyCode.Return: return this["ICON_enter_1_small"];
                case KeyCode.UpArrow: return this["quicktime_up_v2"];
                case KeyCode.DownArrow: return this["quicktime_down_v2"];
                case KeyCode.LeftArrow: return this["quicktime_left_v2"];
                case KeyCode.RightArrow: return this["quicktime_right_v2"];
                case KeyCode.E: return this["ICON_e"];
            }
            return null;
        }
    }
}