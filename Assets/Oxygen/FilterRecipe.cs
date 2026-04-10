using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FilterRecipe", menuName = "Oxygen/FilterRecipe")]
public class FilterRecipe : ScriptableObject
{
    public string filterName;
    public int tierProduced;
    public List<RecipeIngredients> ingredients;
}
