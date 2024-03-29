using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Image effectImage;
    [SerializeField]
    private float gravityScale;
    private CharacterController m_characterController;
    private CharacterController characterController
    {
        get
        {
            if (m_characterController == null)
            {
                m_characterController = GetComponent<CharacterController>();
            }
            return m_characterController;
        }
    }
    private PlayerInfo m_playerInfo;
    private PlayerInfo playerInfo
    {
        get
        {
            if (m_playerInfo == null)
            {
                m_playerInfo = GetComponent<PlayerInfo>();
            }
            return m_playerInfo;
        }
    }
    [Header("-Camera Setting")]
    [SerializeField]
    private Camera playerCamera;
    [SerializeField]
    private float normalFov;
    [SerializeField]
    private float hookshotFov;
    [SerializeField]
    private float dashFov;
    [SerializeField]
    private float chopdriverFov;
    private CameraFov m_cameraFov;
    private CameraFov cameraFov
    {
        get
        {
            if (m_cameraFov == null)
            {
                m_cameraFov = playerCamera.GetComponent<CameraFov>();
            }
            return m_cameraFov;
        }
    }

    [Header("-HookShot")]
    [SerializeField]
    private bool isUseHookShoot;
    public float hookShotMoveSpeedMin;
    public float hookShotMoveSpeedMax;
    public float hookShotLimitMaxDistance;
    private float hookShotSpeed;
    public float hookShotCoolTime;
    [SerializeField]
    private float hookShotLimitDistance;
    [HideInInspector]
    public State state = State.Normal;
    private Ray mouseRay;
    private RaycastHit hitCollider;
    private bool isHookShotReload = true;
    [SerializeField]
    private Transform hookshotTransform;
    private float hookshotSize;
    public float hookshotThrowSpeed;
    [SerializeField]
    private float minHookshotUpMomentum;

    [Header("-Jump")]
    public float jumpForce;
    public int extraJumpCount;
    public float addExtraJumpCoolTime;
    private bool isAddExtraJumpCoolTime;
    private int m_jumpCount;
    private int jumpCount // jumpCount를 0이상, extraJumpCount + 1 이하로 제한
    {
        get => m_jumpCount;
        set
        {
            if (value < 0)
            {
                m_jumpCount = 0;
            }
            else if (value > extraJumpCount + 1)
            {
                m_jumpCount = extraJumpCount + 1;
            }
            else
            {
                m_jumpCount = value;
            }
        }
    }
    private bool isJump = false;

    [Header("-Move")]
    public float moveSpeed;
    private Vector3 characterVelocity;
    private float characterVelocityY;
    public Vector3 extraCharacterVelocity;
    public float extraCharacterVelocityY = 0;
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

    [Header("-Chop Driver")]
    [SerializeField]
    private float heightForChopDriver;
    private bool isChopDrive = false;
    public float chopDriverCoolTime;
    public float chopDriverForce;
    public float chopDriverDamage;
    public float chopDriverDamageRadius;
    public float chopDriverStunTime;

    //private PlayerParticleManager m_playerParticleManager;
    //private PlayerParticleManager playerParticleManager
    //{
    //    get
    //    {
    //        if (m_playerParticleManager == null)
    //        {
    //            m_playerParticleManager = GetComponent<PlayerParticleManager>();
    //        }
    //        return m_playerParticleManager;
    //    }
    //}

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
        hookshotTransform.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!IsGroggy())
        {
            switch (state)
            {
                case State.Normal:
                    PlayerMovement();
                    HookShot();
                    CheckIpnutDashKeyCode();
                    CheckConditionOfChopDriver();
                    break;

                case State.HookShotThrown:
                    PlayerMovement();
                    HookshotThrow();
                    CheckIpnutDashKeyCode();
                    break;

                case State.HookShotFlyingPlayer:
                    CheckIpnutDashKeyCode();
                    HookShotMovement();
                    break;

                case State.DashingPlayer:
                    Dash();
                    break;

                case State.ChopDriver:
                    ChopDriver();
                    break;
            }
        }
        else
        {
            extraCharacterVelocityY += Physics.gravity.y * gravityScale * Time.deltaTime;
            extraCharacterVelocity -= extraCharacterVelocity * Time.deltaTime * 4f;
            extraCharacterVelocity.y = extraCharacterVelocityY;
            characterController.Move(extraCharacterVelocity * Time.deltaTime);
        }

        HitEffect();
    }


    private void PlayerMovement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        characterVelocity = (transform.right * moveX + transform.forward * moveZ) * moveSpeed;

        PlayerJump();

        characterVelocityY += Physics.gravity.y * gravityScale * Time.deltaTime * 4f;

        characterVelocity.y = characterVelocityY;

        characterVelocity += characterVelocityMomentum;

        characterController.Move(characterVelocity * Time.deltaTime);

        if (characterVelocityMomentum.magnitude >= 0f)
        {
            characterVelocityMomentum -= characterVelocityMomentum * momentumDrag * Time.deltaTime;
            if (characterVelocityMomentum.magnitude < 0f || characterController.isGrounded)
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
            if (jumpCount == 0)
            {
                jumpCount = 1;
            }
            isJump = true;
        }

        if (Input.GetKeyDown(KeyCode.Space) && (!isJump || jumpCount <= extraJumpCount))
        {
            //playerParticleManager.InjectAir(Quaternion.LookRotation(Vector3.down));
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
            if (jumpCount > 0)
            {
                StartCoroutine("AddExtraJump", addExtraJumpCoolTime);
            }
        }
    }

    private IEnumerator AddExtraJump(float coolTime)
    {
        isAddExtraJumpCoolTime = true;
        while (coolTime > 0)
        {
            coolTime -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        Debug.Log("Charged ExtraJump");
        jumpCount--;
        isAddExtraJumpCoolTime = false;
    }


    private void HookShot()
    {
        if (Input.GetMouseButtonDown(1) && isHookShotReload && isUseHookShoot)
        {
            mouseRay = playerCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mouseRay, out hitCollider, hookShotLimitMaxDistance, LayerMask.GetMask("Floor") | LayerMask.GetMask("Enemy")))
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
            cameraFov.SetCameraFov(hookshotFov);
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
            ResetToNormalState();
            float hookshotUpMomentum = hookshotDir.y * hookShotSpeed * momentumExtraSpeed;
            characterVelocityMomentum = Vector3.up * (hookshotUpMomentum > minHookshotUpMomentum ? hookshotUpMomentum : minHookshotUpMomentum);
            cameraFov.SetCameraFov(normalFov);
            hookshotTransform.gameObject.SetActive(false);
        }
    }

    private IEnumerator HookReload(float coolTime)
    {
        yield return new WaitForSeconds(coolTime);
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

        Vector3 dashCharacterVelocity = (transform.right * moveX + transform.forward * moveZ) * dashSpeed;
        if (dashCharacterVelocity == Vector3.zero)
        {
            Debug.Log("Did not dash");
            state = dashBeforeState;
        }
        else
        {
            if (canDash)
            {
                //playerParticleManager.InjectAir(Quaternion.LookRotation(-dashCharacterVelocity));
                cameraFov.SetCameraFov(dashFov);
                StartCoroutine(DashMovement(dashDurationTime, dashCharacterVelocity));
                StartCoroutine(DashReload(dashCoolTime));
            }
        }
    }

    private IEnumerator DashMovement(float durationTime, Vector3 dashCharacterVelocity)
    {
        canDash = false;
        dashCharacterVelocity.y = 0;
        while (durationTime > 0)
        {
            durationTime -= Time.deltaTime;
            hookshotTransform.LookAt(hitCollider.point);
            characterController.Move(dashCharacterVelocity * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
        characterVelocityMomentum = Vector3.zero;
        characterVelocityY = 0;
        cameraFov.SetCameraFov(normalFov);
        state = dashBeforeState;
    }

    private IEnumerator DashReload(float coolTime)
    {
        while (coolTime > 0)
        {
            coolTime -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        canDash = true;
        Debug.Log("Dash Reloaded");
    }


    private void CheckConditionOfChopDriver()
    {
        if (Physics.SphereCast(transform.position, characterController.radius, Vector3.down, out RaycastHit hitFloor, float.MaxValue, LayerMask.GetMask("Floor")))
        {
            if ((transform.position.y - hitFloor.point.y) > heightForChopDriver)
            {
                Debug.Log("Can ChopDriver");
                if (Input.GetKeyDown(KeyCode.Q) && !isChopDrive)
                {
                    characterVelocity = Vector3.zero + Vector3.down * chopDriverForce;
                    characterVelocityMomentum = Vector3.zero;
                    state = State.ChopDriver;
                    characterVelocityY = 5f;
                    cameraFov.SetCameraFov(chopdriverFov);
                }
            }
        }
    }

    private void ChopDriver()
    {
        if (characterVelocityY < 0)
        {
            characterVelocityY = -chopDriverForce;
        }
        else characterVelocityY += Physics.gravity.y * gravityScale * Time.deltaTime;

        characterVelocity.y = characterVelocityY;
        characterController.Move(characterVelocity * Time.deltaTime);

        if (characterController.isGrounded)
        {
            TakeDamageChopDriver();

            StartCoroutine(ReloadChopDriver(chopDriverCoolTime));
            cameraFov.SetCameraFov(normalFov);
            ResetToNormalState();
        }
    }

    private void TakeDamageChopDriver()
    {
        RaycastHit[] hitEnemies = Physics.SphereCastAll(transform.position, chopDriverDamageRadius, Vector3.up, 0f, LayerMask.GetMask("Enemy"));
        Debug.Log(LayerMask.GetMask("Enemy"));
        foreach (RaycastHit hitEnemy in hitEnemies)
        {
            EntityInfo enemyInfo = hitEnemy.transform.GetComponent<EntityInfo>();
            enemyInfo.effect.Stun = chopDriverStunTime;
            enemyInfo.takenDamage = chopDriverDamage;
        }
    }

    private IEnumerator ReloadChopDriver(float coolTime)
    {
        isChopDrive = true;
        while (coolTime > 0)
        {
            coolTime -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        isChopDrive = false;
        Debug.Log("Reload ChopDriver");
    }

    private void ResetToNormalState()
    {
        characterVelocityY = 0;
        state = State.Normal;
    }

    private void HitEffect()
    {
        if (playerInfo.isHit)
        {
            effectImage.color = new Color(effectImage.color.r, effectImage.color.g, effectImage.color.b, 24f / 256f);
            playerInfo.isHit = false;
        }

        effectImage.color = new Color(effectImage.color.r, effectImage.color.g, effectImage.color.b, Mathf.Clamp01(effectImage.color.a - (Time.deltaTime / 10f)));
    }

    private bool IsGroggy()
    {
        playerInfo.effect.Stun -= Time.deltaTime;

        return playerInfo.effect.Stun > 0;
    }
}