using UnityEngine;

public class Movement : MonoBehaviour
{
    public Transform cranePivot;    // Empty GameObject at the crane's pivot
    public Transform craneArm;      // Pivots up/down (child of pivot)
    public Transform craneHook;     // Moves up/down (child of arm)

    public float baseRotationSpeed = 50f;
    public float armRotationSpeed = 30f;
    public float hookSpeed = 2f;

    public float minArmAngle = 10f;
    public float maxArmAngle = 75f;

    public float minHookHeight = 0.5f;
    public float maxHookHeight = 10f;

    void Update()
    {
        RotateBase();
        MoveArm();
        MoveHook();
    }

    void RotateBase()
    {
        float horizontal = Input.GetAxis("Horizontal"); // A/D or arrows
        cranePivot.Rotate(0f, horizontal * baseRotationSpeed * Time.deltaTime, 0f);
    }

    void MoveArm()
    {
        float armInput = 0f;

        if (Input.GetKey(KeyCode.W))
            armInput = 1f;
        else if (Input.GetKey(KeyCode.S))
            armInput = -1f;

        float currentX = craneArm.localEulerAngles.x;
        if (currentX > 180f) currentX -= 360f; // Normalize angle

        float newX = Mathf.Clamp(currentX - armInput * armRotationSpeed * Time.deltaTime, -maxArmAngle, -minArmAngle);
        craneArm.localEulerAngles = new Vector3(newX, 0f, 0f);
    }

    void MoveHook()
    {
        float hookInput = 0f;
        if (Input.GetKey(KeyCode.UpArrow)) hookInput = -1f;
        if (Input.GetKey(KeyCode.DownArrow)) hookInput = 1f;

        Vector3 localPos = craneHook.localPosition;
        localPos.y = Mathf.Clamp(localPos.y + hookInput * hookSpeed * Time.deltaTime, -maxHookHeight, -minHookHeight);
        craneHook.localPosition = localPos;
    }
}