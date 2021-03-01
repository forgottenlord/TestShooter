using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime;
    public CharacterController Owner;
    [SerializeField]
    private CharacterSettings settings;
    public void Update()
    {
        transform.position += transform.forward * Time.deltaTime * settings.BulletSpeed;
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0)
        {
            gameObject.SetActive(false);
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        CharacterController character = other.GetComponent<CharacterController>();
        if (character != null && character != Owner)
        {
            character.DealDamage();
            gameObject.SetActive(false);
            if (character.ragDoll)
            {
                var bodies = character.ragDoll.BuildBodies();
                for (int n = 0; n < bodies.Count; n++)
                {
                    bodies[n].AddForce((bodies[n].transform.position - transform.position) * settings.ShotImpactStrength);
                }
            }
        }
    }
}
