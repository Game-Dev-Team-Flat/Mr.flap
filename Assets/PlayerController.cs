using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    public float gravityDownForce;

    [Header("-HookShot")]
    public float hookShotCoolTime;
    private bool isHookShotReload = true;
    public float hookShotSpeed;
    [SerializeField]
    private float hookShotLimitDistance;
    [HideInInspector]
    public State state;
    private Ray mouseRay;
    private RaycastHit hitCollider;

    [Header("-Jump")]
    public float jumpForce;
    public int extraJumpCount;
    public float addExtraJumpCoolTime;
    private bool isAddExtraJumpCoolTime = false;
    private int jumpCount;
    private bool isJump = false;

    [Header("-Move")]
    public float moveSpeed;
    private Vector3 characterVelocity;
    private float characterVelocityY;

    [Header("-Dash")]
    public float dashSpeed;
    public float dashCoolTime;
    public float dashDurationTime;
    private bool canDash = true;
    private State dashBeforeState;

    public enum State
    {
        Normal,
        HookShotFlyingPlayer,
        DashingPlayer,
        ChopDriver
    }

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        state = State.Normal;
    }

    private void Update()
    {
        if (state == State.Normal)
        {
            PlayerMovement();
            ShotHook();
            CheckIpnutDashKeyCode();
        }
        if(state == State.HookShotFlyingPlayer)
        {
            CheckIpnutDashKeyCode();
            HookShotMovement();
        }
        if(state == State.DashingPlayer)
        {
            Dash();
        }
        if(state == State.ChopDriver)
        {

        }
    }

    private void PlayerMovement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        characterVelocity = (transform.right * moveX + transform.forward * moveZ) * moveSpeed;

        PlayerJump();

        characterVelocityY -= gravityDownForce * Time.deltaTime;

        characterVelocity.y = characterVelocityY;

        characterController.Move(characterVelocity * Time.deltaTime);
    }

    private void PlayerJump()
    {
        if (characterController.isGrounded)
        {
            jumpCount = 0;
            characterVelocityY = 0f;
            isJump = false;
        }
        else isJump = true;

        if (Input.GetKeyDown(KeySetting.keys[KeyAction.Jump]) && (!isJump || jumpCount <= extraJumpCount))
        {
            characterVelocityY = jumpForce;
            jumpCount++;

            ExtraJump();
        }
    }

    private void ExtraJump()
    {
            if(jumpCount == extraJumpCount && !isAddExtraJumpCoolTime)
            {
                StartCoroutine(AddExtraJump(addExtraJumpCoolTime));
            }
            if(jumpCount != 0)
            {
                //splash white particles
            }
    }

    private IEnumerator AddExtraJump(float _coolTime)
    {
        isAddExtraJumpCoolTime = true;
        while (_coolTime > 0)
        {
            _coolTime -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        jumpCount--;
        isAddExtraJumpCoolTime = false;
    }

    private void ShotHook()
    {
        if (Input.GetKeyDown(KeySetting.keys[KeyAction.ShotHock]) && isHookShotReload)
        {
            mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mouseRay, out hitCollider, float.MaxValue))
            {
                isHookShotReload = false;
                state = State.HookShotFlyingPlayer;
                characterVelocity.y = 0f;
                StartCoroutine(HookReload(hookShotCoolTime));
            }
        }
    }

    private void HookShotMovement()
    {
        Vector3 hookshotDir = (hitCollider.point - transform.position).normalized;

        characterController.Move(hookshotDir * hookShotSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, hitCollider.point) < hookShotLimitDistance)
        {
            state = State.Normal;
            characterVelocityY = 15f;
        }
    }
    private IEnumerator HookReload(float _coolTime)
    {
        while(_coolTime > 0)
        {
            _coolTime -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        isHookShotReload = true;
        Debug.Log("Hook Reloaded");
    }

    private void CheckIpnutDashKeyCode()
    {
        if (Input.GetKeyDown(KeySetting.keys[KeyAction.Dash]) && canDash)
        {
            dashBeforeState = state;
            state = State.DashingPlayer;
        }
    }

    private void Dash()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        Vector3 _dashCharacterVelocity = (transform.right * moveX + transform.forward * moveZ) * dashSpeed;
        if (_dashCharacterVelocity == Vector3.zero)
        {
            Debug.Log("Did not dash");
            state = dashBeforeState;
        }
        else
        {
            if (canDash)
            {
                StartCoroutine(DashMovement(dashDurationTime, _dashCharacterVelocity));
                StartCoroutine(DashReload(dashCoolTime));
            }
        }
    }

    private IEnumerator DashMovement(float _durationTime, Vector3 _dashCharacterVelocity)
    {
        canDash = false;
        _dashCharacterVelocity.y = 0;
        while (_durationTime > 0)
        {
            _durationTime -= Time.deltaTime;
            characterController.Move(_dashCharacterVelocity * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
        state = dashBeforeState;
    }


    private IEnumerator DashReload(float _coolTime)
    {
        while(_coolTime > 0)
        {
            _coolTime -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        canDash = true;
        Debug.Log("Dash Reloaded");
    }
}