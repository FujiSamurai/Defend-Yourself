using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform player;
    [SerializeField] private Transform playerVisual;
    [SerializeField] private Transform combatLookAt;
    [SerializeField] private GameObject thirdPersonCam;
    [SerializeField] private GameObject combatCam;
    [SerializeField] private GameObject topdownCam;
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private GameInput _gameInput;

    [Header("Attributes")]
    [SerializeField] private float _rotationSpeed;

    private Vector2 _inputAxisDirection;

    private bool _isTopdownCam;
    private bool _isCombatCam;
    private bool _isExploreCam;

    public CameraStyle currentStyle;

    public enum CameraStyle
    {
        Explore,
        Combat,
        Topdown
    }

    private void Start()
    {
        _gameInput.MovementEvent += GetAxisDirection;
        _gameInput.ExploreCamStyleEvent += HandleExploreCamStyle;
        _gameInput.CombatCamStyleEvent += HandleCombatCamStyle;
        _gameInput.TopdownCamStyleEvent += HandleTopdownCamStyle;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _isExploreCam = true;
    }

    private void HandleTopdownCamStyle()
    {
        _isTopdownCam = true;
        SwitchCameraStyle(CameraStyle.Topdown);
    }

    private void HandleCombatCamStyle()
    {
        _isCombatCam = true;
        SwitchCameraStyle(CameraStyle.Combat);
    }

    private void HandleExploreCamStyle()
    {
        _isExploreCam = true;
        SwitchCameraStyle(CameraStyle.Explore);
    }

    private void GetAxisDirection(Vector2 axisDir)
    {
        _inputAxisDirection = axisDir;
    }

    private void Update()
    {
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        if (currentStyle == CameraStyle.Explore && _isExploreCam || currentStyle == CameraStyle.Topdown && _isTopdownCam)
        {
            float horizontalInput = _inputAxisDirection.x;
            float verticalInput = _inputAxisDirection.y;
            Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

            if (inputDir != Vector3.zero)
            {
                playerVisual.forward = Vector3.Slerp(playerVisual.forward, inputDir.normalized, _rotationSpeed * Time.deltaTime);
            }
        }
        else if (currentStyle == CameraStyle.Combat && _isCombatCam)
        {
            Vector3 combatViewDir = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
            orientation.forward = combatViewDir.normalized;

            playerVisual.forward = combatViewDir.normalized;
        }
    }

    private void SwitchCameraStyle(CameraStyle newStyle)
    {
        thirdPersonCam.SetActive(false);
        combatCam.SetActive(false);
        topdownCam.SetActive(false);

        if (newStyle == CameraStyle.Explore) thirdPersonCam.SetActive(true);
        if (newStyle == CameraStyle.Combat) combatCam.SetActive(true);
        if (newStyle == CameraStyle.Topdown) topdownCam.SetActive(true);

        currentStyle = newStyle;
    }
}
