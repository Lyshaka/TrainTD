using TMPro;
using UnityEngine;

public class GroundGeneration : MonoBehaviour
{
	[Header("Ground Generation Properties")]
	[SerializeField] Vector2Int groundSize;
	[SerializeField] float tileSize = 1f;
	[SerializeField] Sprite groundSprite;

	[Header("Ground Generation Properties")]
	[SerializeField] RandomSprite randomSprite;

	[Header("Rails Generation Properties")]
	[SerializeField] int railNumber;
	[SerializeField] float railSize = 1f;
	[SerializeField] Sprite railSprite;

	[Header("Debug")]
	[SerializeField] GameObject debugTextPrefab;
	[SerializeField] bool showNumbers;
	[SerializeField] float decimalProgress = 0f;
	[SerializeField] int currentProgress = 0;
	[SerializeField] TextMeshProUGUI scrollValueUI;

	private Transform[] _tilesColumns;
	private int[] _scrollOffsets;
	private SpriteRenderer[,] _tilesGroundSpriteRenderer;
	private SpriteRenderer[,] _tilesDecorSpriteRenderer;
	private Transform[] _railsTransform;

	private TextMeshPro[,] _debugText;

	private void Start()
	{
		GenerateGround();
	}

	void GenerateGround()
	{
		GameObject groundParent = new("GroundTiles");
		groundParent.transform.parent = transform;

		_tilesColumns = new Transform[groundSize.x];
		_scrollOffsets = new int[groundSize.x];
		_tilesGroundSpriteRenderer = new SpriteRenderer[groundSize.x, groundSize.y];
		_tilesDecorSpriteRenderer = new SpriteRenderer[groundSize.x, groundSize.y];
		_debugText = new TextMeshPro[groundSize.x, groundSize.y];

		randomSprite.Init();

		for (int i = 0; i < groundSize.x; i++)
		{
			// Create and store column parent
			GameObject column = new($"Column_{i}");
			_tilesColumns[i] = column.transform;
			_tilesColumns[i].parent = groundParent.transform;

			for (int j = 0; j < groundSize.y; j++)
			{
				// Create and store the object parent
				GameObject obj = new($"Tile_{i}_{j}");
				obj.transform.parent = column.transform;

				// Add the ground sprite renderer
				GameObject groundSR = new("GroundSprite");
				groundSR.transform.parent = obj.transform;
				_tilesGroundSpriteRenderer[i, j] = groundSR.AddComponent<SpriteRenderer>();
				_tilesGroundSpriteRenderer[i, j].sprite = groundSprite;

				// Add the decor sprite renderer
				GameObject decorSR = new("DecorSprite");
				decorSR.transform.parent = obj.transform;
				_tilesDecorSpriteRenderer[i, j] = decorSR.AddComponent<SpriteRenderer>();
				_tilesDecorSpriteRenderer[i, j].sortingOrder = 10;

				if (showNumbers)
				{
					GameObject t = Instantiate(debugTextPrefab, Vector3.zero, Quaternion.identity, obj.transform);
					_debugText[i, j] = t.GetComponentInChildren<TextMeshPro>();
					_debugText[i, j].text = $"({Random.Range(0, 100)})";
				}

				// Move the object parent to the correct position and set its size according to the tile size
				obj.transform.position = new(0f, (j - groundSize.y / 2f + 0.5f) * tileSize);
				obj.transform.localScale = new(tileSize, tileSize, tileSize);
			}

			_tilesColumns[i].position = new((i - groundSize.x / 2f + 0.5f) * tileSize, 0f);
			_scrollOffsets[i] = i;
		}

		GameObject railParent = new("Rails");
		railParent.transform.parent = transform;

		_railsTransform = new Transform[railNumber];

		for (int i = 0; i < railNumber; i++)
		{
			GameObject obj = new($"Rail_{i}");
			_railsTransform[i] = obj.transform;
			SpriteRenderer sr = obj.AddComponent<SpriteRenderer>();
			sr.sprite = railSprite;
			sr.sortingOrder = 20;
			_railsTransform[i].position = new((i - railNumber / 2f + 0.5f) * railSize, 0f);
			_railsTransform[i].localScale = new(railSize, railSize, railSize);
			_railsTransform[i].parent = railParent.transform;
		}

		for (int j = 0; j < groundSize.x; j++)
			UpdateNewTiles(j);
	}

	public void Scroll(float speed)
	{
		float delta = speed * Time.deltaTime;

		// Scroll rails
		for (int i = 0; i < railNumber; i++)
		{
			_railsTransform[i].position += new Vector3(delta, 0f);

			if (_railsTransform[i].position.x < -(railNumber * railSize) / 2f)
			{
				Vector3 newPos = _railsTransform[i].position;
				newPos.x += (railNumber * railSize);
				_railsTransform[i].position = newPos;
			}

			if (_railsTransform[i].position.x > (railNumber * railSize) / 2f)
			{
				Vector3 newPos = _railsTransform[i].position;
				newPos.x -= (railNumber * railSize);
				_railsTransform[i].position = newPos;
			}
		}

		// Scroll ground
		for (int i = 0; i < groundSize.x; i++)
		{
			_tilesColumns[i].position += new Vector3(delta, 0f);

			if (_tilesColumns[i].position.x < -(groundSize.x * tileSize) / 2f)
			{
				Vector3 newPos = _tilesColumns[i].position;
				newPos.x += (groundSize.x * tileSize);
				_tilesColumns[i].position = newPos;

				_scrollOffsets[i] += groundSize.x;
				scrollValueUI.text = "Scroll Value : " + currentProgress;

				UpdateNewTiles(i);
			}

			if (_tilesColumns[i].position.x > (groundSize.x * tileSize) / 2f)
			{
				Vector3 newPos = _tilesColumns[i].position;
				newPos.x -= (groundSize.x * tileSize);
				_tilesColumns[i].position = newPos;

				_scrollOffsets[i] -= groundSize.x;
				scrollValueUI.text = "Scroll Value : " + currentProgress;

				UpdateNewTiles(i);
			}
		}

		decimalProgress -= speed * Time.deltaTime;
		if (decimalProgress > 1f)
		{
			decimalProgress -= 1f;
			currentProgress++;
		}

		if (decimalProgress < 0f)
		{
			decimalProgress += 1f;
			currentProgress--;
		}
	}

	void UpdateNewTiles(int index)
	{
		for (int j = 0; j < groundSize.y; j++)
		{
			int seed = (_scrollOffsets[index] * 73856093) ^ (j * 19349663);
			Random.InitState(seed);
			//Debug.Log($"Seed : {seed}, x : ({i}), y: ({j})");

			if (j != groundSize.y / 2)
				_tilesDecorSpriteRenderer[index, j].sprite = randomSprite.GetRandomSprite();

			if (showNumbers)
				_debugText[index, j].text = $"({Random.Range(0, 100)})";
		}
	}

}
