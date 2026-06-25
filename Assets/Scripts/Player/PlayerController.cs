using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float groundDrag = 5f;
    [SerializeField] private float airDrag = 2f;
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private Camera playerCamera;
    
    private CharacterController characterController;
    private Vector3 moveDirection;
    private Vector3 velocity;
    private float xRotation = 0f;
    private bool isGrounded;
    private Team playerTeam;
    private string tempPlayerName;
    private Weapon currentWeapon;
    private float health = 100f;
    private bool isDead = false;
    
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private InputAction fireAction;
    private InputAction reloadAction;
    
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        SetupInput();
    }
    
    private void SetupInput()
    {
        var inputMap = InputSystem.actions;
        moveAction = inputMap.FindAction("Move");
        lookAction = inputMap.FindAction("Look");
        jumpAction = inputMap.FindAction("Jump");
        fireAction = inputMap.FindAction("Fire");
        reloadAction = inputMap.FindAction("Reload");
    }
    
    private void Update()
    {
        if (isDead) return;
        
        HandleMovement();
        HandleCamera();
        HandleWeaponInput();
        ApplyGravity();
    }
    
    private void HandleMovement()
    {
        isGrounded = characterController.isGrounded;
        
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        moveDirection = transform.forward * moveInput.y + transform.right * moveInput.x;
        moveDirection = moveDirection.normalized * moveSpeed;
        
        if (jumpAction.triggered && isGrounded)
        {
            Jump();
        }
        
        characterController.Move(moveDirection * Time.deltaTime + velocity * Time.deltaTime);
    }
    
    private void HandleCamera()
    {
        Vector2 lookInput = lookAction.ReadValue<Vector2>();
        float mouseX = lookInput.x * mouseSensitivity;
        float mouseY = lookInput.y * mouseSensitivity;
        
        transform.Rotate(Vector3.up * mouseX);
        
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
    
    private void HandleWeaponInput()
    {
        if (fireAction.triggered && currentWeapon != null)
        {
            currentWeapon.Fire();
        }
        
        if (reloadAction.triggered && currentWeapon != null)
        {
            currentWeapon.Reload();
        }
    }
    
    private void ApplyGravity()
    {
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        else
        {
            velocity.y += Physics.gravity.y * Time.deltaTime;
        }
    }
    
    private void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y);
    }
    
    public void SetupPlayer(string name, Team team)
    {
        tempPlayerName = name;
        playerTeam = team;
        gameObject.name = $"Player_{name}_{team.TeamName}";
    }
    
    public void EquipWeapon(Weapon weapon)
    {
        if (currentWeapon != null)
        {
            Destroy(currentWeapon.gameObject);
        }
        currentWeapon = weapon;
    }
    
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }
    
    private void Die()
    {
        isDead = true;
        characterController.enabled = false;
        gameObject.SetActive(false);
    }
    
    public float GetHealth() => health;
    public Team GetTeam() => playerTeam;
    public string GetPlayerName() => tempPlayerName;
}
