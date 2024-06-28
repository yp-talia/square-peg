using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Tagged Textures")]
public class TextureTagged : ScriptableObject
{
    public enum OptionTags { Food, Sweets, Tools, BreakfastFood, FastFood, Drinks, Furniture, Animal};
    public Sprite[] options;
    public OptionTags[] pairedOptionTag;
    public Sprite selection;
    public string selectionName;
     public string selectionTagName;
}