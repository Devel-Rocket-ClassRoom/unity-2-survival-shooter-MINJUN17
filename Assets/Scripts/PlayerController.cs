using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 8f;
    private Animator animator;
    private Rigidbody rb;
    public Gun gun;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        bool isMoving = h != 0 || v != 0;
        animator.SetBool("Move", isMoving);

        Vector3 direction = new Vector3(h, 0f, v);
        direction = Vector3.ClampMagnitude(direction, 1f);
        rb.linearVelocity = direction * speed;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 lookDir = hit.point - transform.position;
            lookDir.y = 0f;
            if (lookDir != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(lookDir);
        }
        if (gun.Fire)
        {
            gun.Shot();
        }
    }

    
    
}
