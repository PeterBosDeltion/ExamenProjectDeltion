using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvas : MonoBehaviour
{
    private PauseMenu pauseMenu;
    public List<GameObject> playerUis = new List<GameObject>();
    [HideInInspector]
    public GameOverScreen gameOverScreen;

    public GameObject objectivesParent;
    public GameObject objectivePrefab;

    private bool waiting;
    private List<GameObject> objectives = new List<GameObject>();

    public GameObject easterEggPrefab;
    void Start()
    {
        gameOverScreen = GetComponentInChildren<GameOverScreen>(true);
        pauseMenu = GetComponentInChildren<PauseMenu>(true);
        if (!waiting)
            StartCoroutine(WaitForPlayerInit());
        PlayerUI[] pUIs = GetComponentsInChildren<PlayerUI>(true);

        foreach (var pui in pUIs)
        {
            playerUis.Add(pui.gameObject);
        }
        Initialize();
    }

    private IEnumerator WaitForPlayerInit()
    {
        waiting = true;
        yield return new WaitUntil(() => GameManager.instance.playersSpawned);
        pauseMenu.Initialize();
        waiting = false;
    }

    private void Initialize()
    {
        for (int i = 0; i < GameManager.instance.amountOfPlayers; i++)
        {
            playerUis[i].SetActive(true);
        }
    }

    public void SetUIPlayers()
    {
        foreach (var ui in playerUis)
        {
            ui.GetComponent<PlayerUI>().GetMyPlayer();
        }
    }

    public void GenerateDestroyObjective()
    {
        DestroyObjective[] destroyObjectives = FindObjectsOfType<DestroyObjective>();

        GameObject objective = Instantiate(objectivePrefab, objectivesParent.transform);
        objective.GetComponent<ObjectiveBar>().SetObjectiveDestroy("Destroy bug nests", destroyObjectives.Length);

        objectives.Add(objective);
    }

    public void AdvanceObjective()
    {
        foreach (GameObject objective in objectives)
        {
            ObjectiveBar bar = objective.GetComponent<ObjectiveBar>();
            //if() In case of multiple objectives, implement some way of checking which one to advance. Not supported at the moment
            bar.AdvanceDestroyObjective();
        }
    }

    public void EasterEgg()
    {
        GameObject g = Instantiate(easterEggPrefab, transform);
        Destroy(g, 5.0F);
    }
}