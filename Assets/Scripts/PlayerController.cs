using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Renderer rend;
    private AudioSource source;
    public Color currentColor;
    public AudioClip colorChange;
    public AudioClip powerup;
    public AudioClip obstacle;
    public float speed;
    public float forwardSpeed;
    public int score;
    public int totalScore;
    public bool alive;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();
        source = GetComponent<AudioSource>();
        currentColor = Color.red;
        rend.material.color = currentColor;
        alive = true;
        score = 0;
        forwardSpeed = 0.2f;
    }

    private void FixedUpdate()
    {
        if (!(GameController.Instance.isPaused || GameController.Instance.isGameOver))
        {
            Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0.0f, forwardSpeed);
#if UNITY_ANDROID
            movement = new Vector3(Input.acceleration.x, 0.0f, forwardSpeed);
#endif
            rb.AddForce(movement * speed, ForceMode.VelocityChange);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Sphere")
        {
            if (other.gameObject.GetComponent<Renderer>().material.color == currentColor)
            {
                score += 10;
                if (!MusicController.Instance.muted)
                {
                    source.PlayOneShot(powerup);
                }
            }
            else
            {
                score = score / 2;
                if (!MusicController.Instance.muted)
                {
                    source.PlayOneShot(obstacle);
                }
            }
            other.gameObject.GetComponent<MeshRenderer>().enabled = false;

            alive &= score != 0;
        }
        else if (other.tag == "ColorChangingZone")
        {
            Color otherColor = other.gameObject.GetComponent<Light>().color;
            if (currentColor != otherColor)
            {
                currentColor = otherColor;
                rend.material.color = currentColor;
                if (!MusicController.Instance.muted)
                {
                    source.PlayOneShot(colorChange);
                }
            }
        }
    }
}
