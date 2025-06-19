using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;
    public float gravity;
    public float walkgravity = -9.81f * 2;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    public bool isGrounded;
    public bool isSwimming;
    public float swimmingGravity = -0.4f;

    // 新增：调试用的变量
    private Color groundCheckColor = Color.red;

    // Update is called once per frame
    void Update()
    {
        if (DialogSystem.Instance.dialogUIActive == false)
        {
            Movement();
        }

        // 绘制地面检测球体的调试线
        Debug.DrawRay(groundCheck.position, Vector3.down * groundDistance, groundCheckColor);
    }

    public void Movement()
    {
        if (isSwimming)
        {
            gravity = swimmingGravity;
        }
        else
        {
            gravity = walkgravity;
        }

        // 改进地面检测逻辑
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // 新增：调试日志
        if (isGrounded)
        {
            groundCheckColor = Color.green;
            Debug.Log("Grounded at: " + Time.time);
        }
        else
        {
            groundCheckColor = Color.red;
            //Debug.Log("Not Grounded at: " + Time.time);
        }

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}