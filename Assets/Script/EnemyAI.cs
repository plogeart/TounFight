using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    [Header("Cibles")]
    public string targetTag = "Player";
    private Transform target;

    [Header("Combat")]
    public int damagePoints = 10; 
    public float damageTiming = 0.5f; // Délai avant l'impact
    public float detectionRadius = 10f; 
    public float attackRange = 1.5f;    
    public float timeBetweenAttacks = 2f; 

    [Header("Zone de Garde")]
    private Vector3 guardPosition;
    private float guardRange = 50f;
    private bool isReturningHome = false;

    [Header("Composants")]
    public NavMeshAgent agent;
    public Animator anim;

    private bool alreadyAttacked;

    void Start()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        
        // On cherche l'animator sur l'objet ou ses enfants (important pour ton setup)
        if (anim == null) anim = GetComponentInChildren<Animator>();

        GameObject playerObj = GameObject.FindGameObjectWithTag(targetTag);
        if (playerObj != null) target = playerObj.transform;

        guardPosition = transform.position;
    }

    public void SetupGuard(Vector3 center, float range)
    {
        guardPosition = center;
        guardRange = range;
    }

    void Update()
    {
        if (target == null) return;

        float distanceFromHome = Vector3.Distance(transform.position, guardPosition);
        if (distanceFromHome > guardRange) isReturningHome = true;

        if (isReturningHome)
        {
            ReturnHome();
            if (distanceFromHome < 2f) isReturningHome = false;
            return;
        }

        // --- Logique Combat ---
        float distanceToPlayer = Vector3.Distance(transform.position, target.position);

        if (distanceToPlayer <= detectionRadius && distanceToPlayer > attackRange)
        {
            Chasing();
        }
        else if (distanceToPlayer <= attackRange)
        {
            Attacking();
        }
        else
        {
            Idling();
        }
    }

    void ReturnHome()
    {
        agent.isStopped = false;
        agent.SetDestination(guardPosition);
        if(anim) anim.SetBool("Running", true);
    }

    void Chasing()
    {
        agent.isStopped = false;
        agent.SetDestination(target.position);
        if(anim) anim.SetBool("Running", true);
    }

    void Attacking()
    {
        agent.isStopped = true;
        RotateTowards(target.position);
        if(anim) anim.SetBool("Running", false);

        if (!alreadyAttacked)
        {
            // LOG 1 : L'attaque commence
            Debug.Log("⚔️ [Goblin] : Je lance mon attaque ! (Trigger Animation)");

            // On lance l'animation (même si c'est Idle pour l'instant, le code continue)
            if(anim) anim.SetTrigger("Attack");
            
            alreadyAttacked = true;
            StartCoroutine(DealDamageWithDelay());
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    void Idling()
    {
        agent.isStopped = true;
        if(anim) anim.SetBool("Running", false);
    }

    IEnumerator DealDamageWithDelay()
    {
        // On attend le moment de l'impact
        yield return new WaitForSeconds(damageTiming);

        // C'est ici que l'attaque se joue (Calcul de distance)
        if (target != null)
        {
            float currentDistance = Vector3.Distance(transform.position, target.position);
            
            // On vérifie si le joueur est toujours à portée (avec une petite marge de 0.5m)
            if (currentDistance <= attackRange + 0.5f)
            {
                // LOG 2 : L'attaque touche
                Debug.Log("👊 [Goblin] : BOUM ! Joueur touché à " + currentDistance + "m");

                PlayerHealth pHealth = target.GetComponent<PlayerHealth>();
                if (pHealth != null)
                {
                    pHealth.TakeDamage(damagePoints);
                }
                else
                {
                    Debug.LogWarning("⚠️ Le joueur n'a pas de script 'PlayerHealth' !");
                }
            }
            else
            {
                // LOG 3 : L'attaque rate (joueur parti)
                Debug.Log("💨 [Goblin] : Raté ! Le joueur a fui (Distance: " + currentDistance + "m)");
            }
        }
    }

    void ResetAttack()
    {
        alreadyAttacked = false;
        Debug.Log("🔄 [Goblin] : Prêt pour la prochaine attaque.");
    }

    void RotateTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow; Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.red; Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}