using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class KnifeTarget : MonoBehaviour
{
    public Transform Player;
    public float UpdateRate = 0.1f;
    private NavMeshAgent Agent;

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        StartCoroutine(FollowTarget());
    }

    private IEnumerator FollowTarget()
    {
        WaitForSeconds Wait = new WaitForSeconds(UpdateRate);

        while (enabled)
        {
            NavMeshPath path = new NavMeshPath();
            Agent.CalculatePath(Player.transform.position, path);

            if (path.status == NavMeshPathStatus.PathComplete)
            {
                Agent.SetDestination(Player.transform.position);
            }
            else
            {
                Agent.ResetPath();
            }
            
            yield return Wait;
        }
    }
}

/*
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class KnifeTarget : MonoBehaviour
{
    public Transform target;
    public float AttackDistance;

    private NavMeshAgent m_Agent;
    private Animator m_Animator;
    private float m_Distance;
    private Vector3 m_StartingPoint;
    private bool m_PathCalculate = true;

    void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        m_Animator = GetComponent<Animator>();
        m_StartingPoint = transform.position;
    }

    void Update()
    {
        m_Distance = Vector3.Distance(m_Agent.transform.position, target.position);
        if (m_Distance < AttackDistance)
        {
            m_Agent.isStopped = true;
            m_Animator.SetBool("Attack", true);
        }
        else
        {
            m_Agent.isStopped = false;
            if (!m_Agent.hasPath && m_PathCalculate)
            {
                m_Agent.destination = m_StartingPoint;
                m_PathCalculate = false;
            }
            else
            {
                m_Animator.SetBool("Attack", false);
                m_Agent.destination = target.position;
                m_PathCalculate = true;
            }
        }
    }

    void OnAnimationMove()
    {
        if(m_Animator.GetBool("Attack") == false)
        {
            m_Agent.speed = (m_Animator.deltaPosition / Time.deltaTime).magnitude;
        }
    }
}
*/
