using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public partial class Spawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public List<GameObject> forcedPrefabs;
    public List<GameObject> randomPrefabs;
    public int totalEncounters = 5;

    [Header("Rhythm Settings")]
    public int startWaitingBeats = 4;//forse da raddoppiare 
    public int displayDurationBeats = 4;
    public int intervalBeats = 2;
    public int Scena=1;
    private string expression = "";
    // We now store the actual INSTANCES, not the prefabs
    private List<GameObject> spawnedPool = new List<GameObject>();
    private int currentPoolIndex = 0;

    private int nextActionBeat = 0;
    private bool isWaitingToSpawn = true;
    private bool isFinished = false;

    private int getExpressionOnBeat = 0;

    private bool isFirst = true;

    public LevelLogic levelLogic;
    StressBar stressBar;

    private bool canCheckReaction = true; // Cooldown flag
    [SerializeField] private float reactionCooldownTime = 0.2f;

    void Start()
    {
        stressBar = FindFirstObjectByType<StressBar>();
        PrepareAndPreSpawn();
        nextActionBeat = startWaitingBeats;
    }

    void OnEnable() { BeatManager.OnBeat += HandleBeat; }
    void OnDisable() { BeatManager.OnBeat -= HandleBeat; }

    void PrepareAndPreSpawn()
    {
        // 1. Create the list of blueprints (prefabs)
        List<GameObject> prefabSequence = new List<GameObject>();
        prefabSequence.AddRange(forcedPrefabs);

        while (prefabSequence.Count < totalEncounters)
        {
            int randomIndex = Random.Range(0, randomPrefabs.Count);
            prefabSequence.Add(randomPrefabs[randomIndex]);
        }

        // 2. Shuffle the blueprints
        for (int i = 0; i < prefabSequence.Count; i++)
        {
            GameObject temp = prefabSequence[i];
            int randomIndex = Random.Range(i, prefabSequence.Count);
            prefabSequence[i] = prefabSequence[randomIndex];
            prefabSequence[randomIndex] = temp;
        }

        // 3. PRE-SPAWN everything immediately
        foreach (GameObject prefab in prefabSequence)
        {
            Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y - 1.97f, transform.position.z);
            GameObject obj = Instantiate(prefab, spawnPosition, Quaternion.identity);
            obj.SetActive(false); // Hide it immediately
            spawnedPool.Add(obj);
        }
    }

    void HandleBeat(int beatCount)
    {
        if (isFinished) return;

        if (beatCount >= nextActionBeat)
        {
            if (isWaitingToSpawn)
            {
                ActivateNext();
            }
            else
            {
                DeactivateCurrent();
            }
        }

        getExpressionOnBeat++;

        if (isFirst && getExpressionOnBeat == 8)
        {
            checkReazione();
            
        }
        else if (isFirst && getExpressionOnBeat == 16)
        {
            if (isFirst)
            {
                //Debug.Log("Entratooo");
                //Debug.Log("Entratooo");
                //Debug.Log("Entratooo");
                checkReazione();
            }
            isFirst = false;
        }
        if (getExpressionOnBeat == 8 || getExpressionOnBeat == 16)
        {
            checkReazione();

            if (getExpressionOnBeat == 16)
            {
                isFirst = false;
            }
        }
    }

    void ActivateNext()
    {
        if (currentPoolIndex < spawnedPool.Count)
        {
            // Instead of Instantiate, we just Enable
            spawnedPool[currentPoolIndex].SetActive(true);
            getExpressionOnBeat = 0;

            nextActionBeat += displayDurationBeats;
            isWaitingToSpawn = false;
        }
        else
        {
            isFinished = true;
            Debug.Log("All encounters finished!");
        }
    }

    void DeactivateCurrent()
    {
        if (currentPoolIndex < spawnedPool.Count)
        {
            // Instead of Destroy, we just Disable
            spawnedPool[currentPoolIndex].SetActive(false);
        }

        currentPoolIndex++;

        if (currentPoolIndex < spawnedPool.Count)
        {
            nextActionBeat += intervalBeats;
            isWaitingToSpawn = true;
        }
        else
        {
            isFinished = true;
            Debug.Log("Metti isblinked true");
            // BlinkToNextScene manager = FindFirstObjectByType<BlinkToNextScene>();

            // manager.ReadyToBlink();

            levelLogic.DoFinalScene(Scena);
            
            
        }
    }
    public string getEncounterExpression()
    {
        if (currentPoolIndex <= spawnedPool.Count)
        {
            expression = spawnedPool[currentPoolIndex].GetComponent<MoveOnBeat>().GetCorrectExpression();
        }
        return expression.ToString();
    }
    void checkReazione()
    {
        if (!canCheckReaction) return;
        if (currentPoolIndex < spawnedPool.Count)
        {
            spawnedPool[currentPoolIndex].GetComponent<MoveOnBeat>().ReactionNpc();
            stressBar.CheckStress();
            StartCoroutine(ReactionCooldownRoutine());
        }

    }
    IEnumerator ReactionCooldownRoutine()
    {
        canCheckReaction = false;
        yield return new WaitForSeconds(reactionCooldownTime);
        canCheckReaction = true;
    }
}
