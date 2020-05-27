using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraMovementOperator : MonoBehaviour {
    private Camera cam;
    [SerializeField] private int speed = 10;
    private float maxCameraSize;
    private float minCameraSize = 4;
    private int CameraBorderOffset = 2;
    private Vector3 CameraMaxPosition;
    private Vector3 CameraMinPosition;
    private Tilemap background;


    // Start is called before the first frame update
    void Awake()
    {
        background = GameObject.Find("Background").GetComponent<Tilemap>();
        cam = Camera.main;
        Bounds camBounds = background.localBounds;
        Vector3 parentOffset = background.gameObject.GetComponentInParent<Transform>().position; //tilemap has only localBounds so we need into consoderation position of parent
        
        CameraMaxPosition = camBounds.max+parentOffset;

        CameraMinPosition = camBounds.min+parentOffset;
        maxCameraSize = background.size.y/2+1;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.IsPaused && !GameManager.IsGameOver) {
            CameraMovement();
            CameraResize();
        }
    }

    void CameraMovement() {
        if (Input.GetKey(KeyCode.UpArrow)) {
            transform.Translate(0, speed * Time.deltaTime, 0);
        }

        if (Input.GetKey(KeyCode.DownArrow)) {
            transform.Translate(0, -speed * Time.deltaTime, 0);
        }

        if (Input.GetKey(KeyCode.RightArrow)) {
            transform.Translate(speed * Time.deltaTime, 0, 0);
        }

        if (Input.GetKey(KeyCode.LeftArrow)) {
            transform.Translate(-speed * Time.deltaTime, 0, 0);
        }

        float cameraHeight = cam.orthographicSize;
        float cameraWidth = cameraHeight * cam.aspect;
        
        Vector3 position = transform.position;
        float x = Mathf.Clamp(position.x, CameraMinPosition.x+cameraWidth-CameraBorderOffset, CameraMaxPosition.x-cameraWidth+CameraBorderOffset);
        float y = Mathf.Clamp(position.y, CameraMinPosition.y+cameraHeight-CameraBorderOffset, CameraMaxPosition.y-cameraHeight+CameraBorderOffset);
        transform.position = new Vector3(x,y,position.z );
    }

    void CameraResize() {
        if (Input.GetKeyDown(KeyCode.KeypadPlus)) {
            cam.orthographicSize -= 0.5f;
            
        } else if (Input.GetKeyDown(KeyCode.KeypadMinus)) {
            cam.orthographicSize += 0.5f;
        }

        cam.orthographicSize -= Input.mouseScrollDelta.y;

        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minCameraSize, maxCameraSize);
    }
}
