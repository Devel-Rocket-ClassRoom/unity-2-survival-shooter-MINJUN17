using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private LineRenderer bulletLineRenderer;
    public ParticleSystem gunEffect;
    public Transform fireTransform;
    public float fireDistance = 50f;

    private float lastFireTime;
    private float fireInterval = 0.15f;
    private string FireButton = "Fire1";

    private Coroutine coShot;
    private float gunDamage = 20f;

    public bool Fire { get; private set; }

    private void Awake()
    {
        bulletLineRenderer = GetComponent<LineRenderer>();
        bulletLineRenderer.positionCount = 2;
        bulletLineRenderer.enabled = false;
    }


    private void Update()
    {
        Fire = Input.GetButton(FireButton);
    }
    public void Shot()
    {
        if (Time.time > lastFireTime + fireInterval)
        {

            lastFireTime = Time.time;
            Vector3 hitPosition = Vector3.zero;
            Ray ray = new Ray(fireTransform.position, fireTransform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, fireDistance))
            {
                hitPosition = hit.point;

                var target = hit.collider.GetComponent<IDamageable>();
                if (target != null)
                {
                    target.OnDamage(gunDamage, hit.point, hit.normal);
                }
            }
            else
            {
                hitPosition = fireTransform.position + fireTransform.forward * fireDistance;
            }

            if (coShot != null)
            {
                StopCoroutine(coShot);
                coShot = null;
            }
            coShot = StartCoroutine(CoShot(hitPosition));
        }
    }

    private IEnumerator CoShot(Vector3 hitPosition)
    {
        bulletLineRenderer.enabled = true;
        gunEffect.Play();
        bulletLineRenderer.SetPosition(0, fireTransform.position);
        bulletLineRenderer.SetPosition(1, hitPosition);
        bulletLineRenderer.enabled = true;
        yield return new WaitForSeconds(0.03f);
        bulletLineRenderer.enabled = false;
        coShot = null;
    }

}
