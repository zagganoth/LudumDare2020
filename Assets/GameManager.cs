using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public event EventHandler onRespawn;
    public event EventHandler onLevelIncrease;
    public event EventHandler onDie;
    public event EventHandler<OnCollectEventArgs> onCollect;
    public List<ObstacleSpawner> spawners;
    public int currentLevel;
    public float scoreMultiplier;
    public float scoreBuffer;
    public int score;
    [SerializeField]
    public int currentLives;
    public class OnCollectEventArgs : EventArgs
    {
        public string tag;
    }

    [SerializeField]
    public PlayerController player;
    [SerializeField]
    public Goal currentGoal;
    [SerializeField]
    public List<GoalCondition> conditionsPool;
    [SerializeField]
    public List<int> maxConditionsPerLevel;
    public int coinsCollected;
    private int baseLives;
    public bool playerInvulnerable;
    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this);
        }
        currentLevel = 1;
        GenerateNewGoal();
        scoreMultiplier = 1;
        score = 0;
        scoreBuffer = 0;
        baseLives = currentLives;
    }
    private void Start()
    {
        spawners = new List<ObstacleSpawner>(GameObject.FindObjectsOfType<ObstacleSpawner>());

        //onCollect += increaseGoalAmount;
        //onLevelIncrease += increaseLevel;
    }

    private void increaseLevel()
    {
        currentLevel++;
        score += Mathf.RoundToInt(scoreBuffer);
        scoreBuffer = 0;
        currentLives = baseLives;
        GenerateNewGoal();
    }

    private void GenerateNewGoal()
    {
        currentGoal = new Goal();
        HashSet<GoalCondition> incompatibleConditions = new HashSet<GoalCondition>();
        HashSet<GoalCondition> activeConditions = new HashSet<GoalCondition>();
        int conditionsActive;
        int maxLevelIndex = Math.Min(maxConditionsPerLevel.Count - 1, currentLevel);
        conditionsActive = UnityEngine.Random.Range(1, Math.Min(maxConditionsPerLevel[maxLevelIndex], conditionsPool.Count));
        ActiveGoalCondition curCondition;
        int maxTries = 5;
        int curTries;
        for(int i = 0; i < conditionsActive; i++)
        {
            curTries = 0;

            var randCond = conditionsPool[UnityEngine.Random.Range(0, conditionsPool.Count)];
            //Try to select a random condition, but if the condition is incompatible with the currently selected conditions, try 
            //max_try times, then give up
            while((incompatibleConditions.Contains(randCond) || activeConditions.Contains(randCond) || randCond.minLevel > currentLevel) && curTries < maxTries)
            {
                randCond = conditionsPool[UnityEngine.Random.Range(0, Math.Min(maxConditionsPerLevel[maxLevelIndex], conditionsPool.Count))];
                curTries++;
            }
            if (curTries < maxTries)
            {
                curCondition = new ActiveGoalCondition();
                curCondition.Init(randCond, currentLevel);
                foreach (var condition in curCondition.cond.incompatibleConditions)
                {
                    incompatibleConditions.Add(condition);

                }
                foreach(var condition in curCondition.cond.requiredConditions)
                {
                    if(!activeConditions.Contains(condition))
                    {
                        activeConditions.Add(condition);
                        ActiveGoalCondition addCond = new ActiveGoalCondition();
                        addCond.cond = condition;
                        currentGoal.winConditions.Add(addCond);
                    }
                }

                currentGoal.winConditions.Add(curCondition);
                activeConditions.Add(curCondition.cond);
            }
        }
    }
    private void ResetGoal() { 
        foreach(var cond in currentGoal.winConditions)
        {
            cond.Reset();
        }
        coinsCollected = 0;
        scoreMultiplier = 1;
        scoreBuffer = 0;
    }
    private void increaseGoalAmount(string tag)
    {
        foreach(var curCondition in currentGoal.winConditions)
        {
            if(curCondition.cond.GetType() == typeof(ObstacleCondition))
            {
                ObstacleCondition c = curCondition.cond as ObstacleCondition;
                if (tag.Equals(c.desiredTag)) curCondition.currentAmount++;
                if(c.exactly && curCondition.currentAmount > curCondition.requiredAmount)
                {
                    fireRespawnEvent();
                    break;
                }
            }
        }
    }

    public void fireCollectEvent(string tag)
    {
        if(tag.Equals("Coin"))
        {
            coinsCollected++;
            scoreMultiplier *= 1.1f;
            scoreBuffer += scoreMultiplier;
        }
        if(tag.Equals("speedUp"))
        {
            player.startingFlySpeed += 0.1f;
            player.flySpeed = player.startingFlySpeed;
        }
        else if(tag.Equals("speedDown"))
        {
            player.startingFlySpeed -= 0.1f;
        }
        else if(tag.Equals("invulnerable"))
        {
            playerInvulnerable = true;
            player.SetInvulnerable();
            StartCoroutine(endInvulnerable());
        }
        increaseGoalAmount(tag);
        onCollect?.Invoke(this, new OnCollectEventArgs { tag = tag});
    }
    public IEnumerator endInvulnerable()
    {
        yield return new WaitForSeconds(5f);
        playerInvulnerable = false;
        player.SetVulnerable();
    }
    public void fireRespawnEvent(bool stillAlive = false)
    {

        if (stillAlive && currentGoal.complete())
        {
            increaseLevel();
            onLevelIncrease?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            if (!stillAlive)
            {
                onDie?.Invoke(this, EventArgs.Empty);
                currentLives--;
            }
            if (currentLives == 0)
            {

                currentLevel = 1;
                //score = 0;
                scoreBuffer = 0;
                scoreMultiplier = 1;
                coinsCollected = 0;
                currentLives = baseLives;
                GenerateNewGoal();
                SceneManager.LoadScene(2);
            }
            else
            {
                ResetGoal();
            }
        }
        onRespawn?.Invoke(this, EventArgs.Empty);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /*
#if UNITY_EDITOR
    [CustomEditor(typeof(GameManager),true)]
    public class GameManagerEditor : Editor
    {
        GameManager targetObject;
        SerializedObject so;
        private void OnEnable()
        {
            //DrawDefaultInspector();
        }
        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
        }
    }
#endif*/
}
