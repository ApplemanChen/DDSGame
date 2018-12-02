using UnityEngine;

public class FightController : MonoBehaviour {
    public RopeFactory rope;
    public Rigidbody2D player;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            rope.ConnectBody(player);
        }

        if(Input.GetKeyDown(KeyCode.D))
        {
            rope.AddForce(Vector2.right * 1000);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.AddForce(Vector2.up * 1000);
        }
    }
}
