using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashCameraZoom : MonoBehaviour
{

  // min & max values for the bounds of the camera based on player positions.
  private float min_x;
  private float max_x;
  private float min_y;
  private float max_y;

  private Vector3 final_camera_center;
  private Vector3 final_look_at;

  private float camera_dist;
  private float camera_size;

  public float camera_speed;

  // extra room for the size of characters since position is based on center of object.
  public float camera_buffer_x = 1.0f;
  public float camera_buffer_y = 1.0f;

  // Start is called before the first frame update
  void Start()
  {
    camera_dist = transform.position.z;
  }

  // Update is called once per frame
  void Update()
  {
    CalculateBounds();
    CalculateCameraPosAndSize();
  }

  void CalculateBounds()
  {
    min_x = Mathf.Infinity;
    max_x = -Mathf.Infinity;
    min_y = Mathf.Infinity;
    max_y = -Mathf.Infinity;

    GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
    foreach (GameObject player in players)
    {
      Vector3 tempPlayer = player.transform.position;
      //X Bounds
      if (tempPlayer.x < min_x)
        min_x = tempPlayer.x;
      if (tempPlayer.x > max_x)
        max_x = tempPlayer.x;
      //Y Bounds
      if (tempPlayer.y < min_y)
        min_y = tempPlayer.y;
      if (tempPlayer.y > max_y)
        max_y = tempPlayer.y;
    }
  }

  void CalculateCameraPosAndSize()
  {
    Vector3 camera_center = Vector3.zero;

    GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
    foreach (GameObject player in players)
    {
      camera_center += player.transform.position;
    }

    final_camera_center = camera_center / players.Length;
    
    //Rotates and Positions camera around a point
    Vector3 pos = transform.rotation * new Vector3(0f, 0f, (camera_dist - final_camera_center.z) ) + final_camera_center;
    transform.position = Vector3.Lerp(transform.position, pos, camera_speed * Time.deltaTime);
    final_look_at = Vector3.Lerp(final_look_at, final_camera_center, camera_speed * Time.deltaTime);
    transform.LookAt(final_look_at);
    //Size
    float sizeX = max_x - min_x + camera_buffer_x;
    float sizeY = max_y - min_y + camera_buffer_y;
    camera_size = (sizeX > sizeY ? sizeX : sizeY);
    GetComponent<Camera>().orthographicSize = camera_size * 0.5f;
  }
}
