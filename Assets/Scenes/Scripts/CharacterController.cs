using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private bool targetIsSet = false;
    private Vector3 target;
    private Rigidbody RgB;
    private Collider collider;

    [SerializeField]
    private CharacterSettings settings;

    [SerializeField]
    public RagDollBuilder ragDoll;
    
    [SerializeField]
    private FastIKFabric rightHand;

    private bool attackMode = false;
    [SerializeField]
    public bool AttackMode
    {
        get
        {
            return attackMode;
        }
        set
        {
            attackMode = value;
            rightHand.enabled = isAlive && attackMode;
        }
    }
    [SerializeField]
    public bool IsAlive
    {
        get
        {
            return isAlive;
        }
        set
        {
            isAlive = value;
            animator.enabled = isAlive;
            RgB.useGravity = isAlive;
            RgB.constraints = isAlive ?
                RigidbodyConstraints.FreezeRotation :
                RigidbodyConstraints.FreezeAll;
            collider.enabled = isAlive;
            rightHand.enabled = isAlive;
            if (isAlive)
            {
                ragDoll?.RemoveRagdoll();
            }
            else
            {
                targetIsSet = false;
                ragDoll?.SetRagdoll();
            }
        }
    }

    [SerializeField]
    private bool isAlive = false;
    private Animator animator;
    public Bullet bulletPrototype;
    [SerializeField]
    private bool autoRevive = false;
    void Start()
    {
        animator = Common.SetComponent<Animator>(gameObject);
        RgB = transform.GetComponent<Rigidbody>();
        collider = transform.GetComponent<Collider>();
        IsAlive = true;
        AttackMode = false;
    }

    private IEnumerator AutoRevive()
    {
        yield return new WaitForSeconds(10f);
        IsAlive = true;
    }

    void Update()
    {
        reloadTime += Time.deltaTime;
        MovingToPosition();
    }
    public void DealDamage()
    {
        IsAlive = false;
    }
    private float reloadTime = 0;
    public void SetTarget(Vector3 _target)
    {
        if (isAlive && attackMode)
        {
            if (reloadTime > 1 / settings.PlayerFireRate)
            {
                Bullet bullet = Instantiate<Bullet>(bulletPrototype);
                bullet.transform.position = rightHand.Target.position;
                bullet.Owner = this;
                bullet.lifeTime = 5f;
                bullet.transform.LookAt(_target);
                reloadTime = 0;
                return;
            }
        }
        else
        {
            targetIsSet = true;
            target = _target;
        }
    }
    public void SetMode(bool _attackMode)
    {
        attackMode = _attackMode;
    }
    public void SetAlive(bool _isAlive)
    {
        isAlive = _isAlive;
        if (autoRevive == false)
        {
            StartCoroutine(AutoRevive());
        }
    }
    public void MovingToPosition()
    {
        if (targetIsSet)
        {
            Vector3 diff = target - transform.position;
            transform.eulerAngles = new Vector3(0,
                diff.z > 0 ?
                Mathf.Atan(diff.x / diff.z) * Mathf.Rad2Deg :
                Mathf.Atan(diff.x / diff.z) * Mathf.Rad2Deg + 180,
                0);
            transform.position += transform.forward * Time.deltaTime * settings.PlayerSpeed;
            if ((diff).magnitude < 1.1f)
            {
                targetIsSet = false;
            }
            animator.SetFloat("MoveSpeed", settings.PlayerSpeed);
        }
        else
        {
            animator.SetFloat("MoveSpeed", 0);
        }
    }
}
