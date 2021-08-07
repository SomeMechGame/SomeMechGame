using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour{
    float playerHeight = 2f;

    [SerializeField] Transform orientation;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float airMultiplier = 0.4f;
    float movementMultiplier = 10f;

    [Header("Sprinting")]
    [SerializeField] float crouchSpeed = 2f;
    [SerializeField] float walkSpeed = 4f;
    [SerializeField] float sprintSpeed = 6f;
    [SerializeField] float acceleration = 10f;
    [SerializeField] float slideBoost = 10f;

    [Header("Jumping")]
    public float jumpForce = 5f;

    [Header("Keybinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Drag")]
    [SerializeField] float groundDrag = 6f;
    [SerializeField] float slideDrag = 3f;
    [SerializeField] float airDrag = 2f;

    float horizontalMovement;
    float verticalMovement;

    [Header("Ground Detection")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float groundDistance = 0.2f;
    public bool isGrounded { get; private set;}
    public bool isCrouching {get; private set;}
    public bool isSliding {get; private set;}
    public bool hasJumped {get; set;}
    private bool canSlide = true;
    public float slideCooldown = 2f;

    public AudioSource audioSource;
    Vector3 moveDirection;
    Vector3 slopeMoveDirection;

    Rigidbody rb;

    RaycastHit slopeHit;

    private bool OnSlope(){
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f)){
            if (slopeHit.normal != Vector3.up){
                return true;
            }else{
                return false;
            }
        }
        return false;
    }

    private void Start(){
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update(){
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded)
            hasJumped = false;

        MyInput();
        ApplyDrag();
        ApplySpeed();

        if(Input.GetKeyDown(jumpKey)){
            Jump();
        }
        if(Input.GetKeyDown(crouchKey)){
            if(canSlide){
                if(rb.velocity.magnitude > 8){
                    isSliding = true;
                    transform.localScale = new Vector3(1, .5f, 1);
                    playerHeight = 1f;
                    rb.AddForce(moveDirection.normalized * slideBoost, ForceMode.Impulse);
                    canSlide = false;
                    StartCoroutine(cooldownController());
                }else{
                    isCrouching = true;
                    transform.localScale = new Vector3(1, .5f, 1);
                    playerHeight = 1f;
                }
            }
        }

        if(isSliding){
            if(rb.velocity.magnitude < 2){
                isSliding = false;
                isCrouching = true;
            }
        }

        if(Input.GetKeyUp(crouchKey)){
            isCrouching = false;
            isSliding = false;
            transform.localScale = new Vector3(1, 1, 1);
            playerHeight = 2f;
        }

        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
    }

    void MyInput(){
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");
        float forwardMultiplier = 1f;

        if(isSliding){
            forwardMultiplier = 0f;
        }else{
            forwardMultiplier = 1f;
        }

        moveDirection = (orientation.forward * verticalMovement * forwardMultiplier) + 
        (orientation.right * horizontalMovement * forwardMultiplier);
    }

    void Jump(){
        if(isGrounded){
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            audioSource.Play();
        }else if (!hasJumped){
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            hasJumped = true;
            audioSource.Play();
        }
    }

    void ApplySpeed(){
        if (Input.GetKey(sprintKey) && isGrounded && !isCrouching){
            moveSpeed = Mathf.Lerp(moveSpeed, sprintSpeed, acceleration * Time.deltaTime);
        }else if(!isCrouching){
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, acceleration * Time.deltaTime);
        }else{
            moveSpeed = Mathf.Lerp(moveSpeed, crouchSpeed, acceleration * Time.deltaTime);
        }
    }

    void ApplyDrag(){
        if (isGrounded){
            if(isSliding){
                rb.drag = slideDrag;
            }else{
                rb.drag = groundDrag;
            }
        }else{
            rb.drag = airDrag;
        }
    }

    private void FixedUpdate(){
        MovePlayer();
    }

    void MovePlayer(){
        if (isGrounded && !OnSlope()){
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }else if (isGrounded && OnSlope()){
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }else if (!isGrounded){
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
        }
    }

    IEnumerator cooldownController(){
        yield return new WaitForSeconds(slideCooldown);
        canSlide = true;
    }

}