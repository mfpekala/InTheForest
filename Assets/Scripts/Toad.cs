using UnityEngine;

public enum ToadActions
{
    Idle,
    Wander,
    Attack,
}

public class ToadController : MonoBehaviour
{
    public Vector2 idleTimeRange; // Two numbers, how long to stay in idle when entering idle
    public Vector2 wanderDistanceRange; // How far away can it target when wandering
    public float moveSpeed = 4.0f;
    public Animator animator;
    public Room room;

    private ToadActions currentAction = ToadActions.Idle;
    private float timer;
    private Vector2 wanderGoal;
    private bool hasAntTarget = false;
    private GameObject antTarget;

    private void Update()
    {
        ActionStateMachine();
        PerformAction();
    }

    private void ActionStateMachine()
    {
        timer -= Time.deltaTime;
        bool pickNewState = false;
        float rand = Random.value;
        switch (currentAction)
        {
            case ToadActions.Idle:
                if (timer < 0.0f)
                {
                    pickNewState = true;

                }
                break;
            case ToadActions.Wander:
                float distanceToGoal = Vector2.Distance(transform.position, wanderGoal);
                if (distanceToGoal < 0.2f)
                {
                    pickNewState = true;
                }
                break;
            case ToadActions.Attack:
                pickNewState = !hasAntTarget;
                break;
            default:
                break;
        }

        if (pickNewState)
        {
            ToadLaw currentLaw = Laws.Instance.toadLaws[room.currentRoom];
            if (currentLaw == ToadLaw.None)
            {
                animator.SetBool("isIdle", true);
                // Debug.Log("Picking new state, old state");
                // if (rand < 0.4f) // 40% chance to idle
                if (rand < 0.0f) // 0% chance to idle
                {
                    // Debug.Log("Starting Idle");
                    currentAction = ToadActions.Idle;
                    timer = Random.Range(idleTimeRange.x, idleTimeRange.y);
                }
                else
                {
                    // Debug.Log("Starting Wander");
                    currentAction = ToadActions.Wander;
                    StartWander();
                }
            }
            else
            {
                // The toad be following laws
                switch (currentLaw)
                {
                    case ToadLaw.GoUp:
                        if (room.currentRoom < 5)
                        {
                            room.currentRoom += 1;
                            currentAction = ToadActions.Wander;
                            StartWander();
                        }
                        break;
                    case ToadLaw.GoDown:
                        if (room.currentRoom > 0)
                        {
                            room.currentRoom -= 1;
                            currentAction = ToadActions.Wander;
                            StartWander();
                        }
                        break;
                    case ToadLaw.Attack:
                        currentAction = ToadActions.Attack;
                        if (Random.value < 0.2f)
                        {
                            currentAction = ToadActions.Wander;
                            StartWander();
                        }
                        break;
                }
            }
        }
    }

    private void StartWander()
    {
        // Set "isIdle" parameter to false
        animator.SetBool("isIdle", false);

        Vector2 targetPosition = room.RandomPoint();

        if (targetPosition.x < transform.position.x)
        {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }
        else
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }

        wanderGoal = targetPosition;
    }

    private void PerformAction()
    {
        switch (currentAction)
        {
            case ToadActions.Idle:
                // Logic for idling
                break;
            case ToadActions.Wander:
                // Logic for wandering
                Vector2 moveDirection = (wanderGoal - (Vector2)transform.position).normalized;
                transform.position += new Vector3(moveDirection.x * Time.deltaTime * moveSpeed, moveDirection.y * Time.deltaTime * moveSpeed, 0.0f);
                break;
            case ToadActions.Attack:
                // Logic for attacking
                if (!hasAntTarget)
                {
                    GameObject[] gos;
                    gos = GameObject.FindGameObjectsWithTag("Ant");
                    GameObject closest = null;
                    float distance = Mathf.Infinity;
                    Vector3 position = transform.position;
                    foreach (GameObject go in gos)
                    {
                        if (!room.ContainsPoint((Vector2)go.transform.position))
                        {
                            continue;
                        }
                        Vector3 diff = go.transform.position - position;
                        float curDistance = diff.sqrMagnitude;
                        if (curDistance < distance)
                        {
                            closest = go;
                            distance = curDistance;
                        }
                    }
                    if (closest != null)
                    {
                        hasAntTarget = true;
                        antTarget = closest;
                    }
                }
                else
                {
                    if (antTarget != null)
                    {
                        if (!room.ContainsPoint((Vector2)antTarget.transform.position))
                        {
                            antTarget = null;
                            hasAntTarget = false;
                        }
                        // Double check that antTarget exists
                        if (Vector2.Distance(transform.position, antTarget.transform.position) > 0.2f)
                        {
                            // Move towards the ant target
                            Vector2 moveDirection2 = (antTarget.transform.position - transform.position).normalized;
                            transform.position += new Vector3(moveDirection2.x * Time.deltaTime * moveSpeed, moveDirection2.y * Time.deltaTime * moveSpeed, 0.0f);
                            // Set the animation target accordingly
                            animator.SetBool("isIdle", false);
                            if (moveDirection2.x < 0.0f)
                            {
                                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                            }
                            else
                            {
                                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                            }
                        }
                        else
                        {
                            // You've reached the ant target, perform attack logic here
                            // For example, destroy the ant target and reset state
                            Destroy(antTarget);
                            hasAntTarget = false;
                            // Set the animation back to idle
                            animator.SetBool("isIdle", true);
                        }
                    }
                    else
                    {
                        // If antTarget somehow becomes null, set hasAntTarget to false and do nothing
                        hasAntTarget = false;
                    }
                }
                break;
        }
    }
}
