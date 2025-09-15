using UnityEngine;

// taken from Game Programming class, slightly modified
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    public float jumpHeight = 0.4f;
    public float gravity = 9.81f;
    public float airControl = 10f;
    public GameObject jumpEffect;

    Vector3 input;
    Vector3 moveDirection;
    CharacterController controller;
    bool hasLanded = false;


    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // get input
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        // input vector
        input = transform.right * moveHorizontal + transform.forward * moveVertical;
        input.Normalize();

        if (controller.isGrounded)
        {
            // landing particle effects
            if (!hasLanded)
            {
                PlayJumpEffect();
                hasLanded = true;
            }

            moveDirection = input;
            // jump

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = Mathf.Sqrt(2 * jumpHeight * gravity);
                PlayJumpEffect();
                hasLanded = false;
            }
            else
            {
                moveDirection.y = 0.0f;
            }
        }
        else
        {
            // midair
            input.y = moveDirection.y;
            moveDirection = Vector3.Lerp(moveDirection, input, airControl * Time.deltaTime);
        }

        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * speed * Time.deltaTime);
    }

    void PlayJumpEffect()
    {
        Vector3 offset = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
        Destroy(Instantiate(jumpEffect, offset, transform.rotation), 5f);
    }
}