using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraMovementOperator : MonoBehaviour {
    private Camera cam;
    [SerializeField] private int speed = 10;
    [SerializeField] private float maxCameraSize;
    private float minCameraSize = 4;
    private int cameraBorderOffset = 2;
    [SerializeField] private float cameraPixelBorderOffset;
    private Vector3 cameraMaxPosition;
    private Vector3 cameraMinPosition;
    private Tilemap background;


    // Start is called before the first frame update
    void Awake()
    {
        background = GameObject.Find("Background").GetComponent<Tilemap>();
        cam = Camera.main;
        Bounds camBounds = background.localBounds;
        Vector3 parentOffset = background.gameObject.GetComponentInParent<Transform>().position; //tilemap has only localBounds so we need into consoderation position of parent
        
        cameraMaxPosition = camBounds.max+parentOffset;

        cameraMinPosition = camBounds.min+parentOffset;
        maxCameraSize = (float)background.size.y/2+0.5f;
        cameraPixelBorderOffset = 25;
        
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
        //movement with arrow keys or WASD
        if (Input.GetKey(KeyCode.UpArrow) ||Input.GetKey(KeyCode.W)) {
            transform.Translate(0, speed * Time.deltaTime, 0);
        }

        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
            transform.Translate(0, -speed * Time.deltaTime, 0);
        }

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
            transform.Translate(speed * Time.deltaTime, 0, 0);
        }

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
            transform.Translate(-speed * Time.deltaTime, 0, 0);
        }
        int screenWidth = Screen.width;
        int screenHeight = Screen.height;
        
        //movement with mouse
        if (Input.mousePosition.x > screenWidth - cameraPixelBorderOffset)
        {
            transform.Translate(speed * Time.deltaTime,0,0);
        }
        if (Input.mousePosition.x < 0 + cameraPixelBorderOffset)
        {
            transform.Translate(-speed * Time.deltaTime,0,0);
        }
        if (Input.mousePosition.y > screenHeight - cameraPixelBorderOffset)
        {
            transform.Translate(0,speed * Time.deltaTime,0);
        }
        if (Input.mousePosition.y < 0 + cameraPixelBorderOffset)
        {
            transform.Translate(0,-speed * Time.deltaTime,0);
        }
        

        float cameraHeight = cam.orthographicSize;
        float cameraWidth = cameraHeight * cam.aspect;
        
        Vector3 position = transform.position;
        float x = Mathf.Clamp(position.x, cameraMinPosition.x+cameraWidth-cameraBorderOffset, cameraMaxPosition.x-cameraWidth+cameraBorderOffset);
        float y = Mathf.Clamp(position.y, cameraMinPosition.y+cameraHeight-cameraBorderOffset, cameraMaxPosition.y-cameraHeight+cameraBorderOffset);
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
