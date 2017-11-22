using UnityEngine;
using UnityEngine.Networking;

public class PlayerMovement : NetworkBehaviour
{
    public float speed = 6f;            // The speed that the player will move at.
    Animator anim;                      // Reference to the animator component.
    int floorMask;                      // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
    float camRayLength = 100f;          // The length of the ray from the camera into the scene.

    public float timeBetweenBullets = 0.15f;        // The time between each shot.
    float timer;                                    // A timer to determine when to fire.
    public GameObject bulletPrefab;

    [SyncVar]
    public Transform bulletSpawnPoint;

    //public ParticleSystem particleSystem;

    void Awake()
    {
        // Create a layer mask for the floor layer.
        floorMask = LayerMask.GetMask("Floor");
        // Set up references.
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (PauseMenu.isOn)
            return;

        if (!isLocalPlayer)
            return;

        // Store the input axes.
        float h = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        float v = Input.GetAxis("Vertical") * Time.deltaTime * speed;
        Vector3 playerToMouse = Vector3.zero;

        // Turn the player to face the mouse cursor.
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;
        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        {
            playerToMouse = floorHit.point;
            playerToMouse.y = 0f;
        }

        CmdMoveAndAnimate(h, v, playerToMouse);

        timer += Time.deltaTime;
        if (Input.GetButton("Fire1") && timer >= timeBetweenBullets)
        {
            CmdShoot();
            timer = 0;
        }
    }

    [Command]
    void CmdMoveAndAnimate(float h, float v, Vector3 playerToMouse)
    {
        RpcMoveAndAnimate(h, v, playerToMouse);
    }

    [Command]
    void CmdShoot()
    {
        RpcFire();
    }

    [ClientRpc]
    void RpcMoveAndAnimate(float h, float v, Vector3 playerToMouse)
    {
        // Move the player around the scene.
        transform.position += new Vector3(h, 0, v);
        transform.LookAt(playerToMouse);

        // Animate the player.
        bool walking = h != 0f || v != 0f;
        anim.SetBool("IsWalking", walking);
    }  

    [ClientRpc]
    void RpcFire()
    {
        GameObject bullet = (GameObject)Instantiate(bulletPrefab,
           bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
        bullet.GetComponent<Bullet>().setOwner(this.gameObject);

        NetworkServer.Spawn(bullet);

        bullet.SetActive(true);                
        Physics.IgnoreCollision(bullet.GetComponent<Collider>(), GetComponent<Collider>());
    }

    private void OnCollisionExit(Collision collision)
    {
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().isKinematic = false;
    }
}