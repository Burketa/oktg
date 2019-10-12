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
        InfiniteHopper.IPHColumn column = target.GetComponent<InfiniteHopper.IPHColumn>();

        Vector2 offset = target.position - player.transform.position;
        Debug.Log(offset);

        //TODO: Descobrir como calcular a força nescessária quando a plataforma esta variando a altura
        float Vy = (Mathf.Abs(Physics2D.gravity.y) / 2) * (offset.x / player.moveSpeed);

        Debug.Log(Vy);
        return Vy;
    }
}
