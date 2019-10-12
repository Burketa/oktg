using System.Collections.Generic;
using UnityEngine;

public class Bot : MonoBehaviour
{
    private static int jumpsThisSession = 0;

    public static float CalculateJumpForce(InfiniteHopper.IPHPlayer player)
    {
        Transform columns = GameObject.Find("Columns").transform;

        List<Transform> positions = new List<Transform>();
        foreach (Transform column in columns)
        {
            positions.Add(column);
        }

        Transform nextColumn = positions[jumpsThisSession + 1].transform;

        float jumpForce = CalculateLaunchSpeed(player, nextColumn);
        jumpForce = Mathf.Clamp(jumpForce, 0, player.jumpChargeMax);

        Debug.Log("Jump Force: " + jumpForce);

        jumpsThisSession++;
        return jumpForce;
    }
    private static float CalculateLaunchSpeed(InfiniteHopper.IPHPlayer player, Transform target)
    {
        Vector2 offset = target.position - player.transform.position;
        Debug.Log(offset);

        float distance = offset.x;
        float yOffset = offset.y;
        Debug.Log("distance: " + distance);
        Debug.Log("yoffset: " + yOffset);

        float angle = Mathf.Acos(player.moveSpeed * Mathf.Deg2Rad);
        Debug.Log("angle: " + angle);

        //TODO: Descobrir como calcular a força nescessária...
        float jumpForce = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * -Physics2D.gravity.y * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));

        Debug.Log(jumpForce);
        return jumpForce;
    }
}
