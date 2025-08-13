using UnityEngine;

public class MiniMapCameraScript : MonoBehaviour
{
    public Transform player;
    public float rotationSmoothSpeed = 5f; // Tốc độ quay mượt

    void LateUpdate()
    {
        // Cập nhật vị trí theo player (trừ trục Y giữ nguyên)
        Vector3 newPosition = player.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;

        // Lấy góc Y mục tiêu (player)
        float targetYRotation = player.eulerAngles.y;

        // Quay camera mượt hơn bằng LerpAngle
        float smoothYRotation = Mathf.LerpAngle(transform.eulerAngles.y, -targetYRotation, Time.deltaTime * rotationSmoothSpeed);

        // Áp dụng góc quay
        transform.rotation = Quaternion.Euler(90f, smoothYRotation, 0f);
    }
}
