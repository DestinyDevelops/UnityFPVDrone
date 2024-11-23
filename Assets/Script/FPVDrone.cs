using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class FPVDrone : MonoBehaviour
{
    private FPVDroneAudio droneAudio;
    private Collider col;
    private Rigidbody rb;

    [Header("FPV Drone Settings")]
    public float throttleForce;
    public float pitchForce;
    public float yawForce;
    public float rollForce;

    [Header("Status")]
    public bool autoStart;
    public bool started;
    public float mps;

    [Header("Debug")]
    public TextMeshProUGUI debugText;

    void Start()
    {
        droneAudio = GetComponent<FPVDroneAudio>();
        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();

        if (autoStart)
            ToggleStart();

        debugText.enabled = false;
    }

    void Update()
    {
        debugText.text = "Throttle: " + Input.GetAxisRaw("Throttle") + "\n" +
                         "Pitch: " + Input.GetAxisRaw("Pitch") + "\n" +
                         "Yaw: " + Input.GetAxisRaw("Yaw") + "\n" +
                         "Roll: " + Input.GetAxisRaw("Roll") + "\n" +
                         "Speed: " + rb.linearVelocity.magnitude + "\n" +
                         "Velocity: " + rb.linearVelocity.ToString() + "\n" +
                         "Position: " + transform.position.ToString();

        if (Input.GetKeyDown(KeyCode.F1))
            debugText.enabled = !debugText.enabled;

        mps = rb.linearVelocity.magnitude;

        float throttle = Input.GetAxisRaw("Throttle");
        float pitch = Input.GetAxisRaw("Pitch");
        float yaw = Input.GetAxisRaw("Yaw");
        float roll = Input.GetAxisRaw("Roll");

        if(started)
        {
            rb.AddForce(transform.up * throttleForce * throttle * Time.deltaTime);
            rb.AddTorque(transform.right * pitch * pitchForce * Time.deltaTime);
            rb.AddTorque(transform.up * yaw * yawForce * Time.deltaTime);
            rb.AddTorque(transform.forward * roll * rollForce * Time.deltaTime);
        }

        if(Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            ToggleStart();
        }
    }

    public void ToggleStart()
    {
        started = !started;

        if (started)
            droneAudio.StartDrone();
        else
            droneAudio.StopDrone();
    }

    private void OnValidate()
    {
        started = false;
    }
}
