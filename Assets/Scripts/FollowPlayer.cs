using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private Vector3 offset;
    public GameObject player;

    private void Start()
    {
        offset = transform.position - player.transform.position;
    }

    private void LateUpdate()
    {

        transform.position = new Vector3(transform.position.x, transform.position.y, player.transform.position.z - offset.z);
    }
}