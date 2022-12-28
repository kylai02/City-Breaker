using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exploder : MonoBehaviour
{
    public GameObject chips;
    public float radius = 1f;
    public float power = 1f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("test_object"))
        {
            //SpawnEffect();
            
        }
    }


    void SpawnEffect(GameObject m_chips)
    {
        if (m_chips != null)
        {
            GameObject myChips = Instantiate(m_chips, transform.position, Quaternion.identity);
            Destroy(myChips, 1f);
            Vector3 explosionPos = myChips.transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);

            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(power, explosionPos, radius, -3f);
                }
            }
        }
    }
}
