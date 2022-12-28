using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildingExploder : MonoBehaviour
{
    public GameObject originalObject;
    public GameObject fractureObject;
    //public GameObject explosionVFX;

    public float explosionMinForce = 5f;
    public float explosionMaxForce = 100f;
    public float explosionForceRadius = 10f;
    public float fragScaleFactor = 1f;

    private GameObject fracObj;

    void Start()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Explode();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reset();
        }
    }



    void Explode()
    {
        if (originalObject != null)
        {
            originalObject.SetActive(false);

            if (fractureObject != null)
            {
                fracObj = Instantiate(fractureObject) as GameObject;

                foreach (Transform t in fracObj.transform)
                {
                    var rb = t.GetComponent<Rigidbody>();

                    if (rb != null)
                        rb.AddExplosionForce(Random.Range(explosionMinForce, explosionMaxForce), originalObject.transform.position, explosionForceRadius);

                    //yield return StartCoroutine(Shrink(t, 2));
                }

                Destroy(fracObj, 5);

                //if (explosionVFX != null)
                //{
                //    GameObject exploVFX = Instantiate(explosionVFX) as GameObject;
                //    Destroy(exploVFX, 7);
                //}
            }
        }
    }


    void Reset()
    {
        Destroy(fracObj);
        originalObject.SetActive(true);
    }

    IEnumerable Shrink(Transform t, float delay)
    {
        yield return new WaitForSeconds(delay);

        Vector3 newScale = t.localScale;

        while(newScale.x >= 0)
        {
            newScale -= new Vector3(fragScaleFactor, fragScaleFactor, fragScaleFactor);

            t.localScale = newScale;
            yield return new WaitForSeconds(0.05f);
        }
    }



}
