using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseMoveComponent_Character : MonoBehaviour,IMove,IDash
{
    private Stat moveSpeed;
    private Stat dashSpeed;
    private Stat dashDuration;
    private Stat dashCooldown;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isDashing;
    private float lastDashTime;
    private Animator animator;

    public void Init(CharacterConfiguration config)
    {
        moveSpeed = new(config.moveSpeed, 0.01f);
        dashSpeed = new(config.dashSpeed, 0.01f);
        dashCooldown = new(config.dashCooldown, 0.01f);
        dashDuration = new(config.dashDuration, 0.01f);

        rb = GetComponent<Rigidbody2D>();
        moveInput = Vector2.zero;
        isDashing = false;
        lastDashTime = -999f;
        animator = GetComponent<Animator>();
    }

    public Stat MoveSpeed => moveSpeed;
    public Stat DashSpeed => dashSpeed;
    public Stat DashCooldown => dashCooldown;
    public Stat DashDuration => dashDuration;

    public void UpdateInfo(CharacterMoveStats moveStats)
    {

    }

    private void Update()
    {
        moveInput.x = Input.GetKey(KeyCode.D) ? 1 : Input.GetKey(KeyCode.A) ? -1 : 0;
        moveInput.y = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0;
        moveInput = moveInput.normalized;

        HandleDashInput();
    }

    void FixedUpdate()
    {
        if (isDashing && Time.time >= lastDashTime + dashDuration.FinalValue)
        {
            EndDash();
        }

        MoveAndDash();
    }

    void HandleDashInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && CanDash())
        {
            StartDash();
        }
    }

    bool CanDash()
    {
        return !isDashing && Time.time >= lastDashTime + dashCooldown.FinalValue && moveInput != Vector2.zero;
    }

    protected virtual void StartDash()
    {
        isDashing = true;
        lastDashTime = Time.time;
    }

    protected virtual void MoveAndDash()
    {
        float targetSpeed = isDashing ? dashSpeed.FinalValue : moveSpeed.FinalValue;
        rb.velocity = moveInput * targetSpeed;
    }

    protected virtual void EndDash()
    {
        isDashing = false;
    }
}

public struct CharacterMoveStats
{
    public float moveSpeed;
    public float dashSpeed;
    public float dashCooldown;
}
