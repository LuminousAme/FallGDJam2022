using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Basic character movement script based on the character controller from Very Very Valet
//https://youtu.be/qdskE8PJy6Q
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;

    private Quaternion targetRotation;

    private Vector3 targetVelocity = Vector3.zero;
    private Vector3 inputDirection;

    private PlayerControls controls;

    private float coyoteTimer;
    private bool grounded;
    private Vector3 currentGravity;

    [SerializeField] private PlayerMovementSettings movementSettings;
    [SerializeField] private Transform lookAtTarget;
    [SerializeField] private Transform horizontalLookRot;
    private Vector2 lookInput;
    private Vector3 eulerAngles;

    private bool active = true;

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Enable();
        //assign the jump function to the started event of the jump action
        controls.Player.Jump.started += ctx => Jump();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        targetRotation = transform.rotation;

        grounded = true;
        coyoteTimer = 0.0f;

        currentGravity = Vector3.down * movementSettings.GetGravityGoingDown();

        eulerAngles = lookAtTarget.localRotation.eulerAngles;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //grab the input direction from the controls and update the current input direction
        Vector2 inputVec2 = controls.Player.Move.ReadValue<Vector2>();
        inputDirection = horizontalLookRot.forward * inputVec2.y + horizontalLookRot.right * inputVec2.x;
        inputDirection.Normalize();

        //update the rotation for the camera
        lookInput = controls.Player.Look.ReadValue<Vector2>();
        //get the scale of the screen
        float width = Screen.width;
        float height = Screen.height;
        lookInput /= new Vector2(width, height);
        //scaled to 1920 x 1080
        lookInput *= new Vector2(1920f, 1080f);

        //scale back down
        lookInput *= new Vector2(0.1f, 0.1f);

        //rotate the player towards their target rotation
        FixedRotatePlayer();
    }

    private void FixedUpdate()
    {
        //calculate gravity
        FixedCalculateGravity();

        //keep the capsule floating, applying any approriate forces related to that, and applying gravity if not needed
        FixedRaiseCapsule();

        //apply the 2 dimensional (forward, and side to side) basic character motion
        FixedCharacterMove();
    }

    private void FixedRotatePlayer()
    {
        //handle rotation from player input
        float vertRot = -lookInput.y * movementSettings.GetVerticalLookSpeed();
        eulerAngles.y += vertRot * Time.deltaTime;
        eulerAngles.y = Mathf.Clamp(eulerAngles.y, movementSettings.GetVertMinAngle(), movementSettings.GetVertMaxAngle());
        lookAtTarget.localPosition = new Vector3(0.0f, 0.0f, 1.0f);
        lookAtTarget.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        lookAtTarget.RotateAround(lookAtTarget.parent.position, lookAtTarget.right, eulerAngles.y);

        //update the rotation for the rigidbody
        float horiRot = lookInput.x * movementSettings.GetHorizontalLookSpeed();
        eulerAngles.x += horiRot * Time.deltaTime;
        horizontalLookRot.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        horizontalLookRot.RotateAround(horizontalLookRot.position, horizontalLookRot.up, eulerAngles.x);
    }

    private void FixedRaiseCapsule()
    {
        //do a raycast down
        RaycastHit rayHit;
        Vector3 rayDir = Vector3.down;

        //if it hit something calculate the force that should be applied as a result
        if (Physics.Raycast(transform.position, rayDir, out rayHit, 1f * movementSettings.GetRideHeight(), movementSettings.GetWalkableMask()))
        {
            float speedAlongRayDir = Vector3.Dot(rayDir, rb.velocity); //the speed that the player is moving along the ray's direction
            float otherVelAlongRayDir = 0.0f; //the speed that the object the ray has collided with is moving along the ray's direction, it is zero if it didn't hit another rigidbody
            if (rayHit.rigidbody != null)
            {
                //but if did hit a rigidbody we need to calculate it's value
                otherVelAlongRayDir = Vector3.Dot(rayDir, rayHit.rigidbody.velocity);
            }

            //calculate how much force needs to be used to keep the player out of the ground
            float relativeSpeed = speedAlongRayDir - otherVelAlongRayDir;
            float x = rayHit.distance - movementSettings.GetRideHeight();
            float springForce = (x * movementSettings.GetSpringStr()) - (relativeSpeed * movementSettings.GetSpringDamp());

            //apply that force to the player
            rb.AddForce(rayDir * springForce);

            //uncomment if we want the player to be able to apply force to object below them
            /*
            //and if it's collided with another rigidbody, apply to it the same force in the opposite direction at the point of collision
            if (rayHit.rigidbody != null)
            {
                rayHit.rigidbody.AddForceAtPosition(rayDir * -springForce, rayHit.point);
            }*/
        }
        else
        {
            rb.AddForce(currentGravity, ForceMode.Acceleration);
            grounded = false;
            coyoteTimer += Time.fixedDeltaTime;
        }

        if (Physics.Raycast(transform.position, rayDir, out rayHit, 2.5f * movementSettings.GetRideHeight(), movementSettings.GetWalkableMask()))
        {
            grounded = true;
            coyoteTimer = 0.0f;
        }
    }

    private void FixedCharacterMove()
    {
        //calculate the ideal velocity for the character this frame
        Vector3 desiredVelocity = inputDirection * movementSettings.GetBaseMaxSpeed();

        //calculate what the velocity should be, adjusted for the fact it needs to be faster if the player is moving away from the direction they were in the last physics update
        float moveDirectionDot = Vector3.Dot(targetVelocity.normalized, desiredVelocity.normalized);
        float ReMappedAccelFromDot = MathUtils.SmoothStepFromValue(1.0f, 2.0f, MathUtils.ReMapClamped(-1.0f, 0.0f, 2.0f, 1.0f, moveDirectionDot, 1.0f, 2.0f));
        float accel = movementSettings.GetBaseAcceleration() * ReMappedAccelFromDot;

        //claculate the acutal new target velocity, based on the acceleration
        targetVelocity = Vector3.MoveTowards(targetVelocity, desiredVelocity, movementSettings.GetBaseAcceleration() * Time.fixedDeltaTime);

        //figure out how much force it would take to get to that velocity
        Vector3 existingVelocity = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z);
        Vector3 forceRequired = (targetVelocity - existingVelocity) / Time.fixedDeltaTime;
        //clamp the magnitude of the force to the maximum
        float maxForce = movementSettings.GetBaseMaxAccelForce() * ReMappedAccelFromDot;
        forceRequired = Vector3.ClampMagnitude(forceRequired, maxForce);

        //apply that force to the rigidbody
        rb.AddForce(forceRequired * rb.mass);
    }

    private void FixedCalculateGravity()
    {
        //set gravity based on player velocity - will see how this works with the floating capsule, we'll have to see
        if (rb.velocity.y >= 0) currentGravity = Vector3.up * movementSettings.GetGravityGoingUp();
        else if (rb.velocity.y < 0) currentGravity = Vector3.up * movementSettings.GetGravityGoingDown();
    }

    private void Jump()
    {
        //if the number of jumps the user has taken is less than the maximum, do a jump
        if (grounded || coyoteTimer <= movementSettings.GetCoyoteTime())
        {
            //add the force
            rb.AddForce(Vector3.up * (movementSettings.GetJumpInitialVerticalVelo()), ForceMode.VelocityChange);
        }
    }

    public Vector3 GetCamEulerAngles() => eulerAngles;

    public void setControlsState(bool on)
    {
        if (on)
        {
            controls.Enable();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            controls.Disable();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
