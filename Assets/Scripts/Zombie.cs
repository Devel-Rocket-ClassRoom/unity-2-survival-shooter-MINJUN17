using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Splines;

public class Zombie : LivingEntity
{
    public enum Status
    {
        Idle,
        Trace,
        Attack,
        Die
    }

    private float sinkSpeed;

    private Animator zombieAnimator;
    private NavMeshAgent agent;
    public ParticleSystem hitEffect;

    private Collider zombieCollider;

    public LayerMask targetLayer;
    public Transform target;

    private Status currentStatus;
    private float traceDistance = 50f;
    private float attackDistance = 2f;

    public float damage = 10f;

    private float lastAttackTime;
    private float attackInterval = 1f;

    public Status CurrentStatus
    {
        get { return currentStatus; }
        set
        {
            var prevStatus = currentStatus;
            currentStatus = value;

            Debug.Log(currentStatus);

            switch (currentStatus)
            {
                case Status.Idle:
                    zombieAnimator.SetBool("HasTarget", false);
                    agent.isStopped = true;
                    break;
                case Status.Trace:
                    zombieAnimator.SetBool("HasTarget", true);
                    agent.isStopped = false;
                    break;
                case Status.Attack:
                    zombieAnimator.SetBool("HasTarget", false);
                    agent.isStopped = true;
                    break;
                case Status.Die:
                    zombieAnimator.SetTrigger("Die");
                    agent.isStopped = true;
                    zombieCollider.enabled = false;
                    break;
            }
        }
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        zombieAnimator = GetComponent<Animator>();
        zombieCollider = GetComponent<Collider>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        Debug.Log(Health);
        agent.enabled = true;
        agent.isStopped = true;
        zombieCollider.enabled = true;
        agent.ResetPath();
        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 10f, NavMesh.AllAreas))
        {
            agent.Warp(hit.position);
        }

        CurrentStatus = Status.Idle;
    }


    private void Update()
    {
        switch (currentStatus)
        {
            case Status.Idle:
                UpdateIdle();
                break;
            case Status.Trace:
                UpdateTrace();
                break;
            case Status.Attack:
                UpdateAttack();
                break;
            case Status.Die:
                UpdateDie();
                StartSinking();
                break;
        }
    }

    private void UpdateDie()
    {

    }

    private void UpdateAttack()
    {
        if(target == null || Vector3.Distance(target.position, transform.position) > attackDistance)
        {
            CurrentStatus = Status.Trace;
        }

        var lookAt = target.position;
        lookAt.y = transform.position.y;
        transform.LookAt(lookAt);

        if(Time.time > attackInterval + lastAttackTime)
        {
            lastAttackTime = Time.time;
            
            var livingEntity = target.GetComponent<LivingEntity>();
            if(livingEntity != null)
            {
                livingEntity.OnDamage(damage, transform.position, -transform.forward);
            }
        }
    }

    private void UpdateTrace()
    {
        if (target != null)
        {
            if (Vector3.Distance(target.position, transform.position) < attackDistance)
            {
                CurrentStatus = Status.Attack;
                return;
            }
        }
        if (target == null || Vector3.Distance(target.position, transform.position) > traceDistance)
        {
            target = null;
            CurrentStatus = Status.Idle;
            return;
        }
        agent.SetDestination(target.position);
    }
    private void UpdateIdle()
    {
        if (target != null && Vector3.Distance(target.position, transform.position) < traceDistance)
        {
            CurrentStatus = Status.Trace;
            return;
        }
        target = FindTarget(traceDistance);
    }
    private Transform FindTarget(float radius)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, targetLayer);
        if (colliders.Length == 0)
        {
            return null;
        }
        var target = colliders.OrderBy(x => Vector3.Distance(x.transform.position, transform.position)).First();
        return target.transform;
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (!IsDead)
        {
            hitEffect.transform.position = hitPoint;
            hitEffect.transform.forward = hitNormal;
            hitEffect.Play();
        }
        base.OnDamage(damage, hitPoint, hitNormal);
    }

    public override void Die()
    {
        if (IsDead)
        {
            return;
        }
        base.Die();
        
        CurrentStatus = Status.Die;
    }

    private void StartSinking()
    {
        StartCoroutine(CoSinkAndDestroy());
    }

    private IEnumerator CoSinkAndDestroy()
    {
        Vector3 targetPos = transform.position - new Vector3(0, 2f, 0);

        while (transform.position != targetPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime);
            yield return null;
        }
        Destroy(gameObject);
    }
}
