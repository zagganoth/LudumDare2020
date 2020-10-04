using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField]
    public string curTag;
    [SerializeField]
    public int minLevel;
    Goal goalStance;
    public bool toCollect;
    public bool toAvoid;

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.GetComponent<PlayerController>() != null)
        {
            if (toAvoid)
            {
                GameManager.instance.fireRespawnEvent();
            }
            else if(toCollect)
            {
                GameManager.instance.fireCollectEvent(curTag);
                Destroy(this.gameObject);
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        goalStance = GameManager.instance.currentGoal;
        rb = GetComponent<Rigidbody>();
        determineBehaviour();
        childStart();
    }

    private void determineBehaviour()
    {
        goalStance = GameManager.instance.currentGoal;
        foreach(var condition in goalStance.winConditions)
        {
            if(condition.cond.GetType() == typeof(ObstacleCondition))
            {
                ObstacleCondition c = condition.cond as ObstacleCondition;
                if (curTag.Equals(c.desiredTag))
                {
                    if (c.avoid)
                    {
                        toAvoid = true;
                        toCollect = false;
                    }
                    else if (c.collect)
                    {
                        toAvoid = false;
                        toCollect = true;
                    }
                }
            }
        }
    }

    protected virtual void childStart()
    {

    }
    // Update is called once per frame
    protected virtual void Update()
    {
        
    }
}
