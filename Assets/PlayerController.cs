using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    public float gravityDownForce;

    [Header("-HookShot")]
    public float hookShotMoveSpeedMin;
    public float hookShotMoveSpeedMax;
    private float hookShotSpeed;
    [SerializeField]
    private float hookShotLimitDistance;
    public float hookShotCoolTime;
    [HideInInspector]
    public State state;
    private Ray mouseRay;
    private RaycastHit hitCollider;
    private bool isHookShotReload = true;
    [SerializeField]
    private Transform hookshotTransform;
    private float hookshotSize;
    public float hookshotThrowSpeed;

    [Header("-Jump")]
    public float jumpForce;
    public int extraJumpCount;
    public float addExtraJumpCoolTime;
    private bool isAddExtraJumpCoolTime;
    public int jumpCount;
    private bool isJump = false;

    [Header("-Move")]
    public float moveSpeed;
    private Vector3 characterVelocity;
    private float characterVelocityY;
    private Vector3 characterVelocityMomentum;
    [SerializeField]
    private float momentumDrag;
    [SerializeField]
    private float momentumExtraSpeed;

    [Header("-Dash")]
    public float dashSpeed;
    public float dashCoolTime;
    public float dashDurationTime;
    private bool canDash = true;
    private State dashBeforeState;

    [Header("-Dash")]
    [SerializeField]
    private float heightCanChopDriver;
    private bool isChopDrive = false;
    public float chopDriverCoolTime;
    public float chopDriverForce;

    public enum State
    {
        Normal,
        HookShotThrown,
        HookShotFlyingPlayer,
        DashingPlayer,
        ChopDriver
    }

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        state = State.Normal;
        hookshotTransform.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (state == State.Normal)
        {
            PlayerMovement();
            HookShot();
            CheckIpnutDashKeyCode();
            CheckConditionOfChopDriver();
        }
        if(state == State.HookShotThrown)
        {
            PlayerMovement();
            HookshotThrow();
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
            ChopDriver();
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

        characterVelocity += characterVelocityMomentum;

        characterController.Move(characterVelocity * Time.deltaTime);

        if(characterVelocityMomentum.magnitude >= 0f)
        {
            characterVelocityMomentum -= characterVelocityMomentum * momentumDrag * Time.deltaTime;
            if(characterVelocityMomentum.magnitude < 0f || characterController.isGrounded)
            {
                characterVelocityMomentum = Vector3.zero;
            }
        }
    }

    private void PlayerJump()
    {
        if (characterController.isGrounded)
        {
            jumpCount = 0;
            characterVelocityY = 0f;
            isJump = false;
        }
        else
        {
            if (jumpCount == 0) jumpCount = 1;
            isJump = true;
        }

        if (Input.GetKeyDown(KeyCode.Space) && (!isJump || jumpCount <= extraJumpCount))
        {
            characterVelocityY = jumpForce;
            jumpCount++;
        }
        ExtraJump();
    }

    private void ExtraJump()
    {
        if (characterController.isGrounded)
        {
            StopCoroutine("AddExtraJump");
            isAddExtraJumpCoolTime = false;
        }
        if (!isAddExtraJumpCoolTime)
        {
            if (jumpCount == (extraJumpCount + 1))
            {
                StartCoroutine("AddExtraJump", addExtraJumpCoolTime);
            }
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
        Debug.Log("Charged ExtraJump");
        jumpCount--;
        isAddExtraJumpCoolTime = false;
    }


    private void HookShot()
    {
        if (Input.GetMouseButtonDown(1) && isHookShotReload)
        {
            mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mouseRay, out hitCollider, float.MaxValue))
            {
                isHookShotReload = false;
                hookshotSize = 0f;
                hookshotTransform.gameObject.SetActive(true);
                state = State.HookShotThrown;
                StartCoroutine(HookReload(hookShotCoolTime));
            }
        }
    }

    private void HookshotThrow()
    {
        hookshotTransform.LookAt(hitCollider.point);
        hookshotSize += hookshotThrowSpeed * Time.deltaTime;
        hookshotTransform.localScale = new Vector3(1, 1, hookshotSize);

        if (hookshotSize >= Vector3.Distance(hookshotTransform.position, hitCollider.point))
        {
            state = State.HookShotFlyingPlayer;
        }
    }

    private void HookShotMovement()
    {
        hookshotTransform.LookAt(hitCollider.point);
        Vector3 hookshotDir = (hitCollider.point - transform.position).normalized;
        hookShotSpeed = hookShotMoveSpeedMax + hookShotMoveSpeedMin - Mathf.Clamp(Vector3.Distance(hitCollider.point, transform.position), hookShotMoveSpeedMin, hookShotMoveSpeedMax);
        hookshotTransform.localScale = new Vector3(1, 1, Vector3.Distance(transform.position, hitCollider.point));

        characterController.Move(hookshotDir * hookShotSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, hitCollider.point) < hookShotLimitDistance || StopHookshotMovement())
        {
            state = State.Normal;
            characterVelocityY = 0f;
            characterVelocityMomentum = Vector3.up * hookshotDir.y * hookShotSpeed * momentumExtraSpeed;
            hookshotTransform.gameObject.SetActive(false);
        }
    }

    private IEnumerator HookReload(float _coolTime)
    {
        yield return new WaitForSeconds(_coolTime);
        isHookShotReload = true;
        Debug.Log("Hook Reloaded");
    }

    private bool StopHookshotMovement()
    {
        return Input.GetMouseButtonDown(1);
    }

    private void CheckIpnutDashKeyCode()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
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
            hookshotTransform.LookAt(hitCollider.point);
            characterController.Move(_dashCharacterVelocity * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
        characterVelocityY = 0;
        characterVelocityMomentum = Vector3.zero;
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


    private void CheckConditionOfChopDriver()
    {
        if (Physics.SphereCast(transform.position, characterController.radius, Vector3.down, out RaycastHit hitFloor, float.MaxValue, LayerMask.GetMask("Floor")))
        {
            if ((transform.position.y - hitFloor.point.y) > heightCanChopDriver)
            {
                Debug.Log("Can ChopDriver");
                if(Input.GetKeyDown(KeyCode.Q) && !isChopDrive)
                {
                    characterVelocity = Vector3.up * -chopDriverForce;
                    characterVelocityMomentum = Vector3.zero;
                    state = State.ChopDriver;
                }
            }
        }
    }

    private void ChopDriver()
    {
        characterVelocity.y -= gravityDownForce * Time.deltaTime;
        characterController.Move(characterVelocity * Time.deltaTime);
        if (characterController.isGrounded)
        {
            StartCoroutine(ReloadChopDriver(chopDriverCoolTime));
            state = State.Normal;
        }
    }

    private IEnumerator ReloadChopDriver(float _coolTime)
    {
        isChopDrive = true;
        while (_coolTime > 0)
        {
            _coolTime -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        isChopDrive = false;
        Debug.Log("Reload ChopDriver");
    }
}