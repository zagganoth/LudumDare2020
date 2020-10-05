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
    public bool toShoot;

    protected virtual void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.GetComponent<PlayerController>() != null)
        {
            if (toAvoid && !GameManager.instance.playerInvulnerable)
            {
                GameManager.instance.fireRespawnEvent();
            }
            else if(toCollect)
            {
                GameManager.instance.fireCollectEvent(curTag);
                Destroy(this.gameObject);
            }
        }
        else if(collision.gameObject.CompareTag("projectile") && toShoot)
        {
            GameManager.instance.fireCollectEvent(curTag);
            Destroy(this.gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        goalStance = GameManager.instance.currentGoal;
        toShoot = false;
        toAvoid = false;
        toCollect = false;
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
                    if(c.shoot)
                    {
                        toShoot = true;
                    }
                }
            }
        }
        if (toShoot) {
            gameObject.tag = "KeepAlive";
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
