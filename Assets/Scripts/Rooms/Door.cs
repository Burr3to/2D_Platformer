using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform previousRoom;
    [SerializeField] private Transform nextRoom;
    [SerializeField] private CameraController cam;
    [SerializeField] private float customOrthographicSize = 0f;
    private void Awake()
    {
        cam = Camera.main.GetComponent<CameraController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (customOrthographicSize > 0)
            {
                Camera.main.orthographicSize = customOrthographicSize;
            }

            if (Mathf.Abs(collision.transform.position.x) < Mathf.Abs(transform.position.x)) //from left to right
            {
                cam.MoveToNewRoom(nextRoom);
            }
            else if (Mathf.Abs(collision.transform.position.x) > Mathf.Abs(transform.position.x)) //from right to left
            {
                cam.MoveToNewRoom(previousRoom);
            }
            else if (collision.transform.position.y < transform.position.y) //zdole nahor
            {
                cam.MoveToNewRoom(nextRoom);
            }
            else if (collision.transform.position.y > transform.position.y) //zhora nadol
            {
                cam.MoveToNewRoom(previousRoom);
            }
        }
    }
}