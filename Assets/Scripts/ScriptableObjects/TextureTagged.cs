using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Tagged Textures")]
public class TextureTagged : ScriptableObject
{
    public enum OptionTags { Award, Food, Flowers, Money, Sweets, Tools, BreakfastFood, FastFood, Drinks, Furniture, BathroomFurniture, BedroomFurniture, Animal, Number , Stationery};
    public Sprite[] options;
    public OptionTags[] pairedOptionTag;
    public Sprite selection;
    public string selectionName;
    public string selectionTagName;
}