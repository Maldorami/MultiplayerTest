using UnityEngine;
using UnityEngine.Networking;

public class PlayerMovement : NetworkBehaviour
{
    public float speed = 6f;            // The speed that the player will move at.

    Vector3 movement;                   // The vector to store the direction of the player's movement.
    Animator anim;                      // Reference to the animator component.
    int floorMask;                      // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
    float camRayLength = 100f;          // The length of the ray from the camera into the scene.

    public float timeBetweenBullets = 0.15f;        // The time between each shot.
    float timer;                                    // A timer to determine when to fire.
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;

    //public ParticleSystem particleSystem;
    
    void Awake()
    {
        // Create a layer mask for the floor layer.
        floorMask = LayerMask.GetMask("Floor");
        // Set up references.
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (PauseMenu.isOn)
            return;

        if (!isLocalPlayer)
            return;

        // Store the input axes.
        float h = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        float v = Input.GetAxis("Vertical") * Time.deltaTime * speed;

        // Move the player around the scene.
        transform.position += new Vector3(h, 0, v);

        // Turn the player to face the mouse cursor.
        // Create a ray from the mouse cursor on screen in the direction of the camera.
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        // Create a RaycastHit variable to store information about what was hit by the ray.
        RaycastHit floorHit;
        // Perform the raycast and if it hits something on the floor layer...
        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask)){
            // Create a vector from the player to the point on the floor the raycast from the mouse hit.
            Vector3 playerToMouse = floorHit.point;
            playerToMouse.y = 0f;

            // Set the player's rotation to this new rotation.
            transform.LookAt(playerToMouse);
        }

        // Animate the player.
        // Create a boolean that is true if either of the input axes is non-zero.
        bool walking = h != 0f || v != 0f;

        // Tell the animator whether or not the player is walking.
        anim.SetBool("IsWalking", walking);


        // Add the time since Update was last called to the timer.
        timer += Time.deltaTime;

        // If the Fire1 button is being press and it's time to fire...
        if (Input.GetButton("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0)
        {
            // ... shoot the gun.
            CmdShoot();

            // Reset the timer.
            timer = 0f;
        }
    }

    [Command]
    void CmdShoot()
    {
        //particleSystem.Stop();
        //particleSystem.Play();

        GameObject bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 10.0f;
        bullet.GetComponent<Bullet>().setOwner(this.gameObject);
        NetworkServer.Spawn(bullet);
        Destroy(bullet, 2.0f);

    }
}