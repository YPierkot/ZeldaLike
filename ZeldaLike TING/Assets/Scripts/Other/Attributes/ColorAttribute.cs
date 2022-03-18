using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public class ColorAttribute : PropertyAttribute {
    public Color newGUIColor;
    public GuiType guiType;

    public ColorAttribute(float r, float g, float b, GuiType guiType = GuiType.Text) {
        newGUIColor = new Color(r / 256f, g / 256f, b / 256f);
        this.guiType = guiType;
    }
}

public enum GuiType{Text, Background, All}
