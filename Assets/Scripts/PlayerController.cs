using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private readonly float speed = 150f;
    private readonly float cameraAnimationSpeed = 2.5f;
    private readonly float moveAnimationSpeed = 5f;
    private readonly float boostMultipler = 2f;
    private readonly float warpCoolTime = 1f;
    private readonly float particleCoolTime = 0.25f;
    private readonly float waterMultipler = 0.25f;
    private readonly float defaultDrag = 2f;
    private readonly float iceDrag = 1f;

    private float speedMultipler = 1f;

    private float currentWarpCoolTime = 0f;

    private float currentParticleCoolTime = 0f;

    private bool isWarpAvailable = true;

    [SerializeField]
    private Transform cameraTransform;
    [SerializeField]
    private Rigidbody2D playerRigidbody2D;
    [SerializeField]
    private Transform playerMesh;
    [SerializeField]
    private Transform shadow;
    [SerializeField]
    private PostProcessVolume processVolume;
    [SerializeField]
    private Transform cameraShakeHolder;
    [SerializeField]
    private Image image;
    [SerializeField]
    private Slider SliderUI_CoolTime;

    [SerializeField]
    private LayerUIController layerUIController;

    [SerializeField]
    private AudioClip warp;

    [SerializeField]
    private GameObject clearEffect;
    [SerializeField]
    private GameObject dieEffect;

    private bool isFloor = false;
    private float outOfFloorTime = 0f;

    private Vector3 lastPos;

    private FloorTypes currentFloorType = FloorTypes.Normal;

    private void Update()
    {
        if (!Public.isDead && !Public.isClear)
        {
            CheckMove();
            CheckControl();

            if (!isFloor)
            {
                outOfFloorTime += Time.deltaTime;
            }
            if(outOfFloorTime > 0.5f)
            {
                Dead();
            }
        }
        CheckMoveAnimation();
        if (currentWarpCoolTime < 0.01f)
        {
            isWarpAvailable = true;
        }
        else
        {
            isWarpAvailable = false;
            currentWarpCoolTime -= Time.deltaTime;
        }
        SliderUI_CoolTime.gameObject.SetActive(!isWarpAvailable);
        SliderUI_CoolTime.value = currentWarpCoolTime / warpCoolTime;
    }

    private void CheckMove()
    {
        float xMovement = Input.GetAxisRaw("Horizontal");
        float yMovement = Input.GetAxisRaw("Vertical");

        if(xMovement != 0 || yMovement != 0)
        {
            Vector3 dir = new Vector3(xMovement, yMovement, 0f);

            speedMultipler = speed;

            if (Input.GetKey(Keys.boost))
            {
                speedMultipler *= boostMultipler;
            }

            if (currentFloorType == FloorTypes.Water)
            {
                speedMultipler *= waterMultipler;
            }

            if (currentParticleCoolTime < 0.01f)
            {
                currentParticleCoolTime = particleCoolTime;
            }
            else
            {
                currentParticleCoolTime -= Time.deltaTime;
            }

            Vector3 dirSpeed = dir * speedMultipler * Time.deltaTime;

            playerRigidbody2D.AddForce(dirSpeed, ForceMode2D.Force);
        }
    }

    private void CheckControl()
    {
        if (isWarpAvailable)
        {
            if (Input.GetKeyDown(Keys.layerBefore))
            {
                if (Public.currentMap.GetLayer().BeforeLayerWarp)
                {
                    Warp(WarpModes.Before);
                }
                else
                {
                    Public.logManager.AddLog("Before Layer Control Is Not Available In This Layer");
                }
            }
            if (Input.GetKeyDown(Keys.layerNext))
            {
                if (Public.currentMap.GetLayer().NextLayerWarp)
                {
                    Warp(WarpModes.Next);
                }
                else
                {
                    Public.logManager.AddLog("Next Layer Control Is Not Available In This Layer");
                }
            }
        }
    }

    private void CheckMoveAnimation()
    {
        cameraTransform.localPosition -= transform.position - lastPos;
        shadow.localPosition -= transform.position - lastPos;
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, new Vector3(0f, 0f, -10f), (moveAnimationSpeed / 5) * Time.deltaTime);
        shadow.localPosition = Vector3.Lerp(shadow.localPosition, Vector3.zero, moveAnimationSpeed * Time.deltaTime);
        lastPos = transform.position;
    }

    private IEnumerator cameraShakeAnimationI;
    private IEnumerator changeLayerAnimationI;
    private IEnumerator imageAnimationI;

    private void StartCameraShakeAnimation()
    {
        try
        {
            StopCoroutine(cameraShakeAnimationI);
        }
        catch
        {

        }
        cameraShakeAnimationI = CameraShakeAnimation(0.5f, 5f);
        StartCoroutine(cameraShakeAnimationI);
    }

    private IEnumerator CameraShakeAnimation(float _duration, float _shakeAmount)
    {
        float duration = _duration;
        while (true)
        {
            duration -= Time.deltaTime;
            cameraShakeHolder.localPosition = new Vector2(Random.Range(-_shakeAmount, _shakeAmount) * Time.deltaTime, Random.Range(-_shakeAmount, _shakeAmount) * Time.deltaTime);
            if(duration < 0.1f)
            {
                yield break;
            }
            yield return null;
        }
    }

    private void StartChangeLayerAnimation()
    {
        try
        {
            StopCoroutine(changeLayerAnimationI);
        }
        catch
        {

        }
        changeLayerAnimationI = ChangeLayerAnimation();
        StartCoroutine(changeLayerAnimationI);
    }

    private IEnumerator ChangeLayerAnimation()
    {
        processVolume.profile.TryGetSettings(out Vignette vignette);
        processVolume.profile.TryGetSettings(out DepthOfField depthOfField);
        float angle = 0;
        vignette.intensity.value = 0f;
        depthOfField.focusDistance.value = Mathf.Abs(cameraTransform.position.z);
        vignette.active = true;
        depthOfField.active = true;
        while (true)
        {
            angle = Mathf.Lerp(angle, 360, cameraAnimationSpeed * Time.deltaTime);
            cameraTransform.localRotation = Quaternion.Euler(cameraTransform.localRotation.eulerAngles.x, cameraTransform.localRotation.eulerAngles.y, angle);
            if(angle <= 180)
            {
                vignette.intensity.value = angle / 180;
                depthOfField.focusDistance.value = (1 - (angle / 180)) * Mathf.Abs(cameraTransform.position.z);
            }
            else
            {
                vignette.intensity.value = (360 - angle) / 180;
                depthOfField.focusDistance.value = ((angle - 180) / 180) * Mathf.Abs(cameraTransform.position.z);
            }
            if (Mathf.Abs(angle-360) < 0.5)
            {
                cameraTransform.localRotation = Quaternion.identity;
                vignette.active = false;
                depthOfField.active = false;
                yield break;
            }
            yield return null;
        }
    }

    private void StartImageAnimation()
    {
        try
        {
            StopCoroutine(imageAnimationI);
        }
        catch
        {

        }
        imageAnimationI = ImageAnimation(0.2f);
        StartCoroutine(imageAnimationI);
    }

    private IEnumerator ImageAnimation(float _duration)
    {
        float currentDuration = _duration;
        image.enabled = true;
        while (true)
        {
            currentDuration -= Time.deltaTime;
            if (currentDuration < 0.05f)
            {
                image.enabled = false;
                yield break;
            }
            yield return null;
        }
    }

    private void Dead()
    {
        if (!Public.isDead && !Public.isClear)
        {
            GameObject.FindGameObjectWithTag(Tags.SCENE_CONTROLLER).GetComponent<GameScene>().Dead();
            StartCameraShakeAnimation();
            StartImageAnimation();
            Instantiate(dieEffect, transform);
            playerMesh.gameObject.SetActive(false);
            shadow.gameObject.SetActive(false);
            Public.isDead = true;
        }
    }

    private void Clear()
    {
        if (!Public.isDead && !Public.isClear)
        {
            Instantiate(clearEffect, transform.position, transform.rotation);
            StartCameraShakeAnimation();
            GameObject.FindGameObjectWithTag(Tags.SCENE_CONTROLLER).GetComponent<GameScene>().Clear();
            Public.isClear = true;
            playerRigidbody2D.drag = 5f;
        }
    }

    private void Warp(WarpModes _warpModes)
    {
        Public.soundManager.Play(warp);
        currentWarpCoolTime = warpCoolTime;
        StartChangeLayerAnimation();
        if (_warpModes == WarpModes.Before)
        {
            Public.currentMap.BeforeLayer();
        }
        else
        {
            Public.currentMap.NextLayer();
        }
        layerUIController.OnWarp();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(Tags.WALL))
        {
            if (collision.collider.GetComponentInParent<Wall>().dieWhenCollide)
            {
                Dead();
            }
        }
        if (collision.collider.CompareTag(Tags.GEAR))
        {
            Dead();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag(Tags.SPAWN_POINT))
        {
            Public.spawnPoint = new SpawnPoint(collider.transform.position, Public.currentMap.currentLayerIndex);
        }
        if (collider.CompareTag(Tags.CLEAR_POINT))
        {
            Clear();
        }
        if (collider.CompareTag(Tags.FLOOR))
        {
            outOfFloorTime = 0f;
            isFloor = true;

            currentFloorType = collider.GetComponent<Floor>().floorTypes;

            if (currentFloorType == FloorTypes.Ice)
            {
                playerRigidbody2D.drag = iceDrag;
            }
            else
            {
                playerRigidbody2D.drag = defaultDrag;
            }
        }
        if (collider.CompareTag(Tags.SWITCH_SLAB))
        {
            collider.GetComponent<SwitchSlab>().SetIsPowered(!collider.GetComponent<SwitchSlab>().isPowered);
        }
        if (isWarpAvailable)
        {
            if (collider.CompareTag(Tags.WARP_SLAB))
            {
                 Warp(collider.GetComponent<WarpSlab>().warpMode);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag(Tags.FLOOR))
        {
            isFloor = false;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag(Tags.FLOOR))
        {
            outOfFloorTime = 0f;
            isFloor = true;
        }
    }
}
