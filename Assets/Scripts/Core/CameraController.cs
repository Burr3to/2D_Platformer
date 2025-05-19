using UnityEditor;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Room camera
    [SerializeField] private float speed;
    private float currentPosX;
    private float currentPosY;
    private Vector3 velocity = Vector3.zero;

    private void Update()
    {
        //Room camera
        transform.position = Vector3.SmoothDamp(transform.position,
            new Vector3(currentPosX, currentPosY, transform.position.z), ref velocity, speed);
    }
    public void MoveToNewRoom(Transform _newRoom)
    {
        currentPosX = _newRoom.position.x;
        currentPosY = _newRoom.position.y;

        PlayerPrefs.SetFloat("CamPosX", _newRoom.position.x);
        PlayerPrefs.SetFloat("CamPosY", _newRoom.position.y);
        PlayerPrefs.SetFloat("CamPosZ", _newRoom.position.z);
    }
    public void MoveToSavedRoom()
    {
        currentPosX = PlayerPrefs.GetFloat("CamPosX");
        currentPosY = PlayerPrefs.GetFloat("CamPosY");

    }
}