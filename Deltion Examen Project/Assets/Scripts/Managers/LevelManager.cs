using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//For this script to work it needs to be put in a level with a player (to avoid EnemyAI errors) and EntitySpawners
//To make it work the way you want you will need to asign all the public values in the inspector

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public GameObject playerPrefab;
    public List<Vector3> playerSpawnPositions = new List<Vector3>();
    private Player playerOne;
    private List<EntitySpawner> allAvailableSpawners = new List<EntitySpawner>();
    private List<EntitySpawner> closestSpawners = new List<EntitySpawner>();

    //objectiveList
    public List<DestroyObjective> Levelobjectives = new List<DestroyObjective>();

    //Wave values
    [Tooltip("Fill from easy to hard for increased difficulty in later waves")]
    public List<GameObject> enemyTypes = new List<GameObject>();
    private List<GameObject> currentWaveEntitys = new List<GameObject>();
    private int curentWave;
    private int curentType;
    private int enemiesToAdd;
    public int minimumSpawnerSpread;
    public float timeBetweenIndividualSpawns;
    public float spawnTickTime;

    //Enemy values
    [HideInInspector]
    public float healthModifier;
    [HideInInspector]
    public float damageModifier;

    private bool waiting;
    private MainCanvas mainCanvas;
    private List<DestroyObjective> advancedObjectives = new List<DestroyObjective>();

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    private void SetupPlayers()
    {
        for (int i = 0; i < GameManager.instance.amountOfPlayers; i++)
        {
            GameObject prePlayer = Instantiate(playerPrefab, playerSpawnPositions[i], Quaternion.identity);
            PlayerController pc = prePlayer.GetComponent<PlayerController>();

            switch (i)
            {
                case 0:
                    GameManager.instance.playerOne = pc;
                    break;
                case 1:
                    GameManager.instance.playerTwo = pc;
                    pc.playerNumber = 1;
                    break;
                case 2:
                    GameManager.instance.playerThree = pc;
                    pc.playerNumber = 2;
                    break;
                case 3:
                    GameManager.instance.playerFour = pc;
                    pc.playerNumber = 3;
                    break;
            }

            if(!pc.inTutorial)
                pc.Initialize();
        }

        FindObjectOfType<CameraMovement>().FindPlayerOne();
        mainCanvas.SetUIPlayers();
        playerOne = GameManager.instance.playerOne.GetComponent<Player>();
    }

    private IEnumerator WaitForLoadoutInit()
    {
        waiting = true;
        yield return new WaitUntil(() => LoudoutManager.instance.loadoutsInit);
        Initialize();
        waiting = false;
    }

    private void Initialize()
    {
        mainCanvas = FindObjectOfType<MainCanvas>();
        SetupPlayers();
        mainCanvas.GenerateDestroyObjective();

        SetdifficultyVariables(GameManager.instance.difficulty);
        if (GameObject.FindObjectOfType<EntitySpawner>())
        {
            foreach (EntitySpawner spawner in GameObject.FindObjectsOfType<EntitySpawner>())
            {
                if (!spawner.objectiveSpawner)
                {
                    allAvailableSpawners.Add(spawner);
                    spawner.timeBetweenSpawns = timeBetweenIndividualSpawns;
                }
            }
            StartCoroutine(SpawnTick(3));
        }
        if (GameObject.FindObjectOfType<DestroyObjective>())
        {
            foreach (DestroyObjective objective in GameObject.FindObjectsOfType<DestroyObjective>())
            {
                Levelobjectives.Add(objective);
            }
        }
    }

    private void Start()
    {
        if(playerSpawnPositions.Count <= 0)
        {
            Debug.LogError("Please assign 4 spawnpositions for players in this levels LevelManager");
            return;
        }

        if (!waiting)
        {
            StartCoroutine(WaitForLoadoutInit());
        }
      
    }

    public void CheckObjectives()
    {
        bool victory = true;

        foreach(DestroyObjective objective in Levelobjectives)
        {
            if (objective.ObjectiveDone && !advancedObjectives.Contains(objective))
            {
                mainCanvas.AdvanceObjective();
                advancedObjectives.Add(objective);
            }
            if (!objective.ObjectiveDone)
            {
                victory = false;
                break;
            }
        }

        if (victory)
            TriggerVictory();
    }

    private void TriggerVictory()
    {
        GameManager.instance.GameOver(true);
    }

    private void SetdifficultyVariables(int difficulty)
    {
        switch(difficulty)
        {
            case 1:
                break;
            case 2:
                healthModifier = 1;
                damageModifier = 1;
                enemiesToAdd = 2;
                break;
            case 3:
                break;
        }
    }

    private void SetupWave()
    {
        curentWave++;

        int amount = enemiesToAdd * Mathf.FloorToInt(curentWave / (curentType + 1));

        for (int i = 0; i < amount; i++)
        {
            currentWaveEntitys.Add(enemyTypes[curentType]);
        }

        if (curentType + 1 == enemyTypes.Count)
            curentType = 0;
        else
            curentType++;

        SpawnWave();
    }

    private void SpawnWave()
    {
        float timeToSpawnAllEnemys;
        timeToSpawnAllEnemys = timeBetweenIndividualSpawns * (currentWaveEntitys.Count - 1) / minimumSpawnerSpread;

        if(spawnTickTime * 0.5f > timeToSpawnAllEnemys || minimumSpawnerSpread == allAvailableSpawners.Count)
        {
            GetNearbySpawners(minimumSpawnerSpread);
        }
        else
        {
            minimumSpawnerSpread++;
            SpawnWave();
            return;
        }

        //These values are used to randomly asign a entity from the wave to a spawner
        int spawnsPerSpawner = Mathf.CeilToInt((float)currentWaveEntitys.Count / closestSpawners.Count);
        List<GameObject> wave = new List<GameObject>();
        wave.AddRange(currentWaveEntitys);

        foreach(EntitySpawner spawner in closestSpawners)
        {
            for(int i = 0; i < spawnsPerSpawner; i++)
            {
                if (wave.Count != 0)
                {
                    int randomEntity = Random.Range(0, wave.Count - 1);
                    spawner.AddToSpawnQue(wave[randomEntity]);
                    wave.Remove(wave[randomEntity]);
                }
            }
        }

        StartCoroutine(SpawnTick(spawnTickTime));
    }

    private void GetNearbySpawners(int spawnerSpread)
    {
        closestSpawners.Clear();

        for (int i = 0; i < spawnerSpread; i++)
        {
            float distance = Mathf.Infinity;
            EntitySpawner closestSpawner = allAvailableSpawners[0];

            foreach(EntitySpawner spawner in allAvailableSpawners)
            {
                float newDistance = Vector3.Distance(spawner.gameObject.transform.position, playerOne.transform.position);
                if (distance > newDistance && newDistance > 2 && !closestSpawners.Contains(spawner))
                {
                    distance = newDistance;
                    closestSpawner = spawner;
                }
            }

            closestSpawners.Add(closestSpawner);
        }
    }

    private IEnumerator SpawnTick(float time)
    {
        yield return new WaitForSeconds(time);
        SetupWave();
    }
}