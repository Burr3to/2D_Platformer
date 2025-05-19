using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private AudioClip checkpoint;
    private Transform currentCheckpoint;
    private Health playerHealth;
    [SerializeField] private InGameMenu inGameMenumanager;
    public bool load { get; set; }
   
    private void Awake()
    {
        playerHealth = GetComponent<Health>();
    }
    public void Respawn()
    {
        inGameMenumanager.pauseUI.SetActive(true);
        inGameMenumanager.SwitchPause();
        playerHealth.Respawn(); //Restore player health and reset animation
        if (currentCheckpoint != null)
        {
            transform.position = currentCheckpoint.position; //Move player to checkpoint location

            //Move the camera to the checkpoint's room
            Camera.main.GetComponent<CameraController>().MoveToNewRoom(currentCheckpoint.parent);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Checkpoint")
        {
            currentCheckpoint = collision.transform;
            SoundManager.instance.PlaySound(checkpoint);
            collision.GetComponent<Collider2D>().enabled = false; //deactivate checkpoint collider
            collision.GetComponent<Animator>().SetTrigger("appear");

            inGameMenumanager.CheckpointActivated = true;
        }
    }
}