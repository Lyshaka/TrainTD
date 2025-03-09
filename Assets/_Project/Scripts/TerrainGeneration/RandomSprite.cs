using System;
using UnityEngine;

public class RandomSprite : MonoBehaviour
{
	[SerializeField] WeightedSprite weightedSprite;

	private int[] _prefixSums;
	private int _totalWeight = -1;

	public void Init()
	{
		//Debug.Log("Initialisation !");

		_totalWeight = weightedSprite.NullWeight;
		_prefixSums = new int[weightedSprite.WeightedSprites.Length];

		for (int i = 0; i < weightedSprite.WeightedSprites.Length; i++)
		{
			_totalWeight += weightedSprite.WeightedSprites[i].weight;
			_prefixSums[i] = _totalWeight;
			//Debug.Log("_prefixSums[i] : " + _prefixSums[i]);
		}
	}


	public Sprite GetRandomSprite()
	{
		if (_totalWeight == -1)
		{
			Debug.LogError("Initialize RandomSprite using Init() !");
			return null;
		}

		int rand = UnityEngine.Random.Range(0, _totalWeight);

		if (rand < weightedSprite.NullWeight)
			return null;

		int index = Array.BinarySearch(_prefixSums, rand);

		if (index < 0)
			index = ~index;  // If not found, get the insertion index

		return weightedSprite.WeightedSprites[index].sprite;
	}
}
