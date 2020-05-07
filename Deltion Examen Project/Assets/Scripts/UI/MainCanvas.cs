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

    private List<GameObject> objectives = new List<GameObject>();
    void Start()
    {
        gameOverScreen = GetComponentInChildren<GameOverScreen>(true);
        pauseMenu = GetComponentInChildren<PauseMenu>(true);
        pauseMenu.Initialize();
        PlayerUI[] pUIs = GetComponentsInChildren<PlayerUI>(true);

        foreach (var pui in pUIs)
        {
            playerUis.Add(pui.gameObject);
        }
        Initialize();
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
}