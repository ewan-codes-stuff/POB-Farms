using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    #region Serialised Fields
    [SerializeField] private float volume = 1f;

    [SerializeField] private AudioClip dayAudioClip;

    [SerializeField] private AudioClip nightAudioClip;

    [SerializeField] private AnimationCurve difficultyCurve = AnimationCurve.Linear(0, 0, 1, 1);

    [SerializeField]
    private int baseEnemyBudget = 2;

    [SerializeField] private float baseRoundReward = 10;

    [SerializeField] private GameObject dangerIndicator;
    #endregion

    public static GameManager instance;
    public AIManager aiManager;
    public EnemySpawner enemySpawner;

    private AudioSource source;

    public GameObject ground;
    public LightControl light;

    public Entity house;
    
    private List<GameObject> dangerIndicatorList = new List<GameObject>();

    public Dictionary<Vector2Int, GridTile> tileArray;
    // Start is called before the first frame update
    private void Awake()
    {
        if (instance != null)
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
                return;
            }
        }
        instance = this;
    }

    private void Start()
    {
        GridManager.CreateGridArray();
        //Spawn all enemy danger indicators.
        for(int i = 0; i <= 11; i++)
        {
            GameObject newIndicator = Instantiate(dangerIndicator);
            dangerIndicatorList.Add(newIndicator);
            newIndicator.transform.position = new Vector3(0.0f, -10.0f, 0.0f);
        }
        TurnManager.instance.EndTurnEvent += ManipulateDangerIndicators;
        source = GetComponent<AudioSource>();
        GameManager.instance.SetVolume(PlayerPrefs.GetFloat("masterVolume"));
        PlayDayAudio();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Enemy budget: " + (baseEnemyBudget * (int)GameManager.instance.GetDifficulty()));
            Debug.Log("Current Difficulty: " + ((difficultyCurve.Evaluate((float)TurnManager.instance.GetCurrentRound() / 50) * 10) + 1) * TurnManager.instance.GetCurrentRound());
            Debug.Log("Current Difficulty to int: " + (int)(((difficultyCurve.Evaluate((float)TurnManager.instance.GetCurrentRound() / 50) * 10) + 1) * TurnManager.instance.GetCurrentRound()));
        }
    }

    public GridData GetObjectData()
    {
        return PlacementSystem.instance.objectData;
    }

    public Grid GetGrid()
    {
        return PlacementSystem.instance.grid;
    }

    public List<GameObject> GetPlacedObjects()
    {
        return PlacementSystem.instance.placedGameObject;
    }

    private void ManipulateDangerIndicators()
    {
        int count = 0;
        if(!TurnManager.instance.GetIsNight() && enemySpawner.GetHasRandomisedSpawns())
        {
            //If GetSpawnWall is true, we are using the X dimension walls to spawn from, else is Z Dimension
            if(enemySpawner.GetSpawnWall())
            {
                foreach (GameObject indicator in dangerIndicatorList)
                {
                    indicator.transform.position = new Vector3(enemySpawner.GetSpawnColumn(),1f,count-6);
                    count++;
                    indicator.GetComponentInChildren<SpriteRenderer>().flipX = false;
                }
            }
            else
            {
                foreach (GameObject indicator in dangerIndicatorList)
                {
                    indicator.transform.position = new Vector3(count-6, 1f, enemySpawner.GetSpawnColumn());
                    count++;
                    indicator.GetComponentInChildren<SpriteRenderer>().flipX = true;
                }
            }
            
        }
        else
        {
            foreach (GameObject indicator in dangerIndicatorList)
            {
                indicator.transform.position = new Vector3(0.0f,-10.0f,0.0f);
            }
        }
    }

    #region Audio Control

    public AudioSource GetAudio()
    {
        return source;
    }
    public void PlayDayAudio()
    {
        source.PlayOneShot(dayAudioClip);
    }
    public void PlayNightAudio()
    {
        source.PlayOneShot(nightAudioClip);
    }
    public float GetVolume()
    {
        return volume;
    }

    public void SetVolume(float value)
    {
        volume = value;
        source.volume = volume;
    }
    #endregion

    public int GetBaseEnemyBudget()
    {
        return baseEnemyBudget;
    }
    public float GetDifficulty()
    {
        return ((difficultyCurve.Evaluate((float)TurnManager.instance.GetCurrentRound() / 50) * 10) + 1) * TurnManager.instance.GetCurrentRound();
    }
    public void RoundReward()
    {
        if (Player.instance != null)
        {
            Player.instance.AddCurrency((int)(baseRoundReward * (GetDifficulty())));
        }
    }
}