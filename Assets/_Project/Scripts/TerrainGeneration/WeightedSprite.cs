using System;
using UnityEngine;

[CreateAssetMenu(fileName = "WeightedSprite", menuName = "Scriptable Objects/WeightedSprite")]
public class WeightedSprite : ScriptableObject
{
	[SerializeField] int nullWeight = 0;
	[SerializeField] WSprite[] weightedSprites;

	public int NullWeight => nullWeight;
	public WSprite[] WeightedSprites => weightedSprites;
}

[Serializable]
public struct WSprite
{
	public Sprite sprite;
	public int weight;
}