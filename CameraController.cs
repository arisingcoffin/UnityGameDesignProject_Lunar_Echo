using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform[] _OriginalRotation;             
    [SerializeField] private Slider _SliderValue;

    [SerializeField] private GameObject[] _Cameras;
    [SerializeField] private GameObject _SizedImagedUI;
    [SerializeField] private GameObject _Text;

    /*Cahnging the Camera texture using the names*/
    [SerializeField] private RenderTexture[] _CameraViews;/*Texture Names: 
                                                           * 0 = nothing
                                                           *1 =DinningRoom1
                                                           *2 =DinningRoom2
                                                           *3 =Enterance
                                                           *4 =Showstage1
                                                           *5 =Showstage2
                                                           */
    private RawImage _RawImage;
    //Camera Zoom Index//
    public float[] _zoomValue = new float[5];
    /*Camera Zoom Indexes:
                  *Showstage.1 = 0;
                  *DiningRoom.1 = 1;
                  *Entrance = 2;
                  *Showstage.2 = 3;
                  *DiningRoom.2 = 4;
     */
    //Rotation Indexes//
    public float[] yRotation = new float[5];
    public float[] xRotation = new float[5];
    /*Camera RotationIndex:
                *Showstage.1 = 0;
                *DiningRoom.1 = 1;
                *Enterance = 2;
                *Showstage.2 = 3;
                *DiningRoom.2 = 4;
     */
    // Camera Active//
    /* IMPORTANT: Each Index number correlates to what camera is active so that the camera can be rotated */
    [Range(0, 5)]
    public int cameraIndex; /* Camera Index:
                      * 0 = Static;
                      * 1 = showstage.1;
                      * 2 = DiningRoom.1;
                      * 3 = Enterance;
                      * 4 = Showstage.2;
                      * 5 = DiningRoom.2;     
                      */
    //Camera Controls//

    /* IMPORTANT: Each Index number correlates to what button is being used. EX) controlIndex = 1 , Camera is not doing anything; controlIndex = 3 (Left Button), Camera going left */
    int controlIndex;/* Control Index:
                      * 0 = ResettingRotations
                      * 1 = Nothing; 
                      * 2 = Up; 
                      * 3 = UpRight; 
                      * 4 = UpLeft; 
                      * 5 = Down; 
                      * 6 = DownRight; 
                      * 7 = DownLeft; 
                      * 8 = Left; 
                      * 9 = Right;
                      * 10 = IncreaseZoom;
                      * 11 = DecreaseZoom
                      */
    private void Start()
    {
        _RawImage = _SizedImagedUI.GetComponent<RawImage>();
        cameraIndex = 1;
        controlIndex = 1;
    }
    private void FixedUpdate()
    {
        //Camera Changer//

        //Rotations//
        RotationUp();
        RotationDown();
        RotationLeft();
        RotationDownLeft();
        RotationUpLeft();
        RotationRight();
        RotationDownRight();
        RotationUpRight();
        ReturnPosition();
        //Zoom//
        CameraZoomIn();
        CameraZoomOut();
    }
    private void Update()
    {
        CameraChanging();
        Limiter();
    }


    /*Buttons*/
    //Camera Changer//
    public void NextCamera()
    {
        cameraIndex++;
    }
    public void ReturnCamera()
    {
        cameraIndex--;
    }
    public void Showstage_1()
    {
        cameraIndex = 1;
    }
    public void DiningRoom_1()
    {
        cameraIndex = 2;
    }
    public void Entrance()
    {
        cameraIndex = 3;
    }
    public void Showstage_2()
    {
        cameraIndex = 4;
    }
    public void DiningRoom_2()
    {
        cameraIndex = 5;
    }
    //Contol Rotation//
    //Up 2//
    public void HoldUp()
    {
        controlIndex = 2;
    }
    //UpRight 3//
    public void HoldUpRight()
    {
        controlIndex = 3;
    }
    //UpLeft 4//
    public void HoldUpLeft()
    {
        controlIndex = 4;
    }
    //Down 5//
    public void HoldDown()
    {
        controlIndex = 5;
    }
    //DownRight 6//
    public void HoldDownRight()
    {
        controlIndex = 6;
    }
    //DownLeft 7//
    public void HoldDownLeft()
    {
        controlIndex = 7;
    }
    //Left 8//
    public void HoldLeft()
    {
        controlIndex = 8;
    }
    //Right 9//
    public void HoldRight()
    {
        controlIndex = 9;
    }
    //Releasing Button 1//
    public void ReleaseButton()
    {
        controlIndex = 1;
    }
    //Resetting 0 //
    public void ReturnCameraButton()
    {
        controlIndex = 0;
    }
    public void IncreaseZoom()
    {
        controlIndex = 10;
    }
    public void DecreaseZoom()
    {
        controlIndex = 11;
    }

    /*Actions*/
    // Camera Changer//

    private void CameraChanging()
    {
        foreach (RenderTexture _Render in _CameraViews)
        {
            if (cameraIndex == 0 && _Render.name == "Static")
            {
                _RawImage.texture = _Render;
                return;
            }
            else if (cameraIndex == 1 && _Render.name == "Showstage1")
            {
                _RawImage.texture = _Render;
                return;
            }
            else if (cameraIndex == 2 && _Render.name == "DinningRoom1")
            {
                _RawImage.texture = _Render;
                return;
            }
            else if (cameraIndex == 3 && _Render.name == "Enterance")
            {
                _RawImage.texture = _Render;
                return;
            }
            else if (cameraIndex == 4 && _Render.name == "Showstage2")
            {
                _RawImage.texture = _Render;
                return;
            }
            else if (cameraIndex == 5 && _Render.name == "DinningRoom2")
            {
                _RawImage.texture = _Render;
                return;
            }
        }
        Debug.Log("Couldn't find any renders");
    }
    //Zoom for the Camera//
    private void CameraZoomIn()
    {
        if (controlIndex == 10)
        {
            foreach (GameObject _camera in _Cameras)
            {
                if (cameraIndex == 1 && _camera.name == "Showstage.1")
                {
                    float _FieldOfView = Time.fixedDeltaTime * 50f;
                    Camera _CameraFOV = _camera.GetComponent<Camera>();

                    _zoomValue[0] -= _FieldOfView;

                    _CameraFOV.fieldOfView = _zoomValue[0];
                    return;
                }
                else if (cameraIndex == 2 && _camera.name == "DiningRoom.1")
                {
                    float _FieldOfView = Time.fixedDeltaTime * 50f;
                    Camera _CameraFOV = _camera.GetComponent<Camera>();

                    _zoomValue[1] -= _FieldOfView;

                    _CameraFOV.fieldOfView = _zoomValue[1];
                    return;
                }
                else if (cameraIndex == 3 && _camera.name == "Enterance")
                {
                    float _FieldOfView = Time.fixedDeltaTime * 50f;
                    Camera _CameraFOV = _camera.GetComponent<Camera>();

                    _zoomValue[2] -= _FieldOfView;

                    _CameraFOV.fieldOfView = _zoomValue[2];
                    return;
                }
                else if (cameraIndex == 4 && _camera.name == "Showstage.2")
                {
                    float _FieldOfView = Time.fixedDeltaTime * 50f;
                    Camera _CameraFOV = _camera.GetComponent<Camera>();

                    _zoomValue[3] -= _FieldOfView;

                    _CameraFOV.fieldOfView = _zoomValue[3];
                    return;
                }
                else if (cameraIndex == 5 && _camera.name == "DiningRoom.2")
                {
                    float _FieldOfView = Time.fixedDeltaTime * 50f;
                    Camera _CameraFOV = _camera.GetComponent<Camera>();

                    _zoomValue[4] -= _FieldOfView;

                    _CameraFOV.fieldOfView = _zoomValue[4];
                    return;
                }
            }
        }
    }
    private void CameraZoomOut()
    {
        if (controlIndex == 11)
        {
            foreach (GameObject _camera in _Cameras)
            {
                if (cameraIndex == 1 && _camera.name == "Showstage.1")
                {
                    float _FieldOfView = Time.fixedDeltaTime * 50f;
                    Camera _CameraFOV = _camera.GetComponent<Camera>();

                    _zoomValue[0] += _FieldOfView;

                    _CameraFOV.fieldOfView = _zoomValue[0];
                    return;
                }
                else if (cameraIndex == 2 && _camera.name == "DiningRoom.1")
                {
                    float _FieldOfView = Time.fixedDeltaTime * 50f;
                    Camera _CameraFOV = _camera.GetComponent<Camera>();

                    _zoomValue[1] += _FieldOfView;

                    _CameraFOV.fieldOfView = _zoomValue[1];
                    return;
                }
                else if (cameraIndex == 3 && _camera.name == "Enterance")
                {
                    float _FieldOfView = Time.fixedDeltaTime * 50f;
                    Camera _CameraFOV = _camera.GetComponent<Camera>();

                    _zoomValue[2] += _FieldOfView;

                    _CameraFOV.fieldOfView = _zoomValue[2];
                    return;
                }
                else if (cameraIndex == 4 && _camera.name == "Showstage.2")
                {
                    float _FieldOfView = Time.fixedDeltaTime * 50f;
                    Camera _CameraFOV = _camera.GetComponent<Camera>();

                    _zoomValue[3] += _FieldOfView;

                    _CameraFOV.fieldOfView = _zoomValue[3];
                    return;
                }
                else if (cameraIndex == 5 && _camera.name == "DiningRoom.2")
                {
                    float _FieldOfView = Time.fixedDeltaTime * 50f;
                    Camera _CameraFOV = _camera.GetComponent<Camera>();

                    _zoomValue[4] += _FieldOfView;

                    _CameraFOV.fieldOfView = _zoomValue[4];
                    return;
                }
            }
        }
    }
    //Rotation for the Camera
    private void ReturnPosition()
    {
       //TEST AND RESULT//
    }
    private void RotationUp()
    {
        if (controlIndex == 2)
        {
            foreach (GameObject _camera in _Cameras)
            {
                if(cameraIndex == 1 && _camera.name == "Showstage.1")
                {
                    float valueX = Time.fixedDeltaTime * _SliderValue.value;

                    xRotation[0] -= valueX;
                    _camera.transform.rotation = Quaternion.Euler(xRotation[0], yRotation[0], 0f);
                    return;
                }
                else if(cameraIndex == 2 && _camera.name == "DiningRoom.1") 
                {
                    float valueX = Time.fixedDeltaTime * _SliderValue.value;

                    xRotation[1] -= valueX;
                    _camera.transform.rotation = Quaternion.Euler(xRotation[1], yRotation[1], 0f);
                    return;
                }
                else if(cameraIndex == 3 && _camera.name == "Enterance")
                {
                    float valueX = Time.fixedDeltaTime * _SliderValue.value;

                    xRotation[2] -= valueX;
                    _camera.transform.rotation = Quaternion.Euler(xRotation[2], yRotation[2], 0f);
                    return;
                }
                else if (cameraIndex == 4 && _camera.name == "Showstage.2")
                {
                    float valueX = Time.fixedDeltaTime * _SliderValue.value;

                    xRotation[3] -= valueX;
                    _camera.transform.rotation = Quaternion.Euler(xRotation[3], yRotation[3], 0f);
                    return;
                }
                else if (cameraIndex == 5 && _camera.name == "DiningRoom.2")
                {
                    float valueX = Time.fixedDeltaTime * _SliderValue.value;

                    xRotation[4] -= valueX;
                    _camera.transform.rotation = Quaternion.Euler(xRotation[4], yRotation[4], 0f);
                    return;
                }
            }
        }
    }
    private void RotationUpRight()
    {
        if (controlIndex == 3)
        {
            foreach (GameObject _camera in _Cameras)
            {
                if (cameraIndex == 1 && _camera.name == "Showstage.1")
                {
                    float valueY = Time.fixedDeltaTime * _SliderValue.value;
                    float valueX = Time.fixedDeltaTime * _SliderValue.value;

                    yRotation[0] += valueY;
                    xRotation[0] -= valueX;
                    _camera.transform.rotation = Quaternion.Euler(xRotation[0], yRotation[0], 0f);
                    return;
                }
                else if (cameraIndex == 2 && _camera.name == "DiningRoom.1")
                {
                    float valueY = Time.fixedDeltaTime * _SliderValue.value;
                    float valueX = Time.fixedDeltaTime * _SliderValue.value;

                    yRotation[1] += valueY;
                    xRotation[1] -= valueX;
                    _camera.transform.rotation = Quaternion.Euler(xRotation[1], yRotation[1], 0f);
                    return;
                }
                else if (cameraIndex == 3 && _camera.name == "Enterance")
                {
                    float valueY = Time.fixedDeltaTime * _SliderValue.value;
                    float valueX = Time.fixedDeltaTime * _SliderValue.value;

                    yRotation[2] += valueY;
                    xRotation[2] -= valueX;
                    _camera.transform.rotation = Quaternion.Euler(xRotation[2], yRotation[2], 0f);
                    return;
                }
                else if (cameraIndex == 4 && _camera.name == "Showstage.2")
                {
                    float valueY = Time.fixedDeltaTime * _SliderValue.value;
                    float valueX = Time.fixedDeltaTime * _SliderValue.value;

                    yRotation[3] += valueY;
                    xRotation[3] -= valueX;
                    _camera.transform.rotation = Quaternion.Euler(xRotation[3], yRotation[3], 0f);
                    return;
                }
                else if (cameraIndex == 5 && _camera.name == "DiningRoom.2")
                {
                    float valueY = Time.fixedDeltaTime * _SliderValue.value;
                    float valueX = Time.fixedDeltaTime * _SliderValue.value;

                    yRotation[4] += valueY;
                    xRotation[4] -= valueX;
                    _camera.transform.rotation = Quaternion.Euler(xRotation[4], yRotation[4], 0f);
                    return;
                }
            }
        }
    }
    private void RotationUpLeft()
    {
        if (controlIndex == 4)
        {
            foreach (GameObject _camera in _Cameras)
            {
                if (cameraIndex == 1 && _camera.name == "Showstage.1")
                {
                    float valueY = Time.fixedDeltaTime * _SliderValue.value;
                    float valueX = Time.fixedDeltaTime * _SliderValue.value;

                    yRotation[0] -= valueY;
                    xRotation[0] -= valueX;
                    _camera.transform.rotation = Quaternion.Euler(xRotation[0], yRotation[0], 0f);
                    return;
                }
                else if (cameraIndex == 2 && _camera.name == "DiningRoom.1")
                {
                    float valueY = Time.fixedDeltaTime * _SliderValue.value;
                    float valueX = Time.fixedDeltaTime * _SliderValue.value;

                    yRotation[1] -= valueY;
                    xRotation[1] -= valueX;
                    _camera.transform.rotation = Quaternion.Euler(xRotation[1], yRotation[1], 0f);
                    return;
                }
                else if (cameraIndex == 3 && _camera.name == "Enterance")
                {
                    float valueY = Time.fixedDeltaTime * _SliderValue.value;
                    float valueX = Time.fixedDeltaTime * _SliderValue.value;

                    yRotation[2] -= valueY;
                    xRotation[2] -= valueX;
                    _camera.transform.rotation = Quaternion.Euler(xRotation[2], yRotation[2], 0f);
                    return;
                }
                else if (cameraIndex == 4 && _camera.name == "Showstage.2")
                {
                    float valueY = Time.fixedDeltaTime * _SliderValue.value;
                    float valueX = Time.fixedDeltaTime * _SliderValue.value;

                    yRotation[3] -= valueY;
                    xRotation[3] -= valueX;
                    _camera.transform.rotation = Quaternion.Euler(xRotation[3], yRotation[3], 0f);
                    return;
                }
                else if (cameraIndex == 5 && _camera.name == "DiningRoom.2")
                {
                    float valueY = Time.fixedDeltaTime * _SliderValue.value;
                    float valueX = Time.fixedDeltaTime * _SliderValue.value;

                    yRotation[4] -= valueY;
                    xRotation[4] -= valueX;
                    _camera.transform.rotation = Quaternion.Euler(xRotation[4], yRotation[4], 0f);
                    return;
                }
            }
        }
    }
    private void RotationDown()
    {
        if (controlIndex == 5)
        {
            foreach (GameObject _camera in _Cameras)
            {
                if (cameraIndex == 1 && _camera.name == "Showstage.1")
                {
                    float valueX = Time.fixedDeltaTime * _SliderValue.value;

                    xRotation[0] += valueX;
                    _camera.transform.rotation = Quaternion.Euler(xRotation[0], yRotation[0], 0f);
                    return;
                }
                else if (cameraIndex == 2 && _camera.name == "DiningRoom.1")
                {
                    float valueX = Time.fixedDeltaTime * _SliderValue.value;

                    xRotation[1] += valueX;
                    _camera.transform.rotation = Quaternion.Euler(xRotation[1], yRotation[1], 0f);
                    return;
                }
                else if (cameraIndex == 3 && _camera.name == "Enterance")
                {
                    float valueX = Time.fixedDeltaTime * _SliderValue.value;

                    xRotation[2] += valueX;
                    _camera.transform.rotation = Quaternion.Euler(xRotation[2], yRotation[2], 0f);
                    return;
                }
                else if (cameraIndex == 4 && _camera.name == "Showstage.2")
                {
                    float valueX = Time.fixedDeltaTime * _SliderValue.value;

                    xRotation[3] += valueX;
                    _camera.transform.rotation = Quaternion.Euler(xRotation[3], yRotation[3], 0f);
                    return;
                }
                else if (cameraIndex == 5 && _camera.name == "DiningRoom.2")
                {
                    float valueX = Time.fixedDeltaTime * _SliderValue.value;

                    xRotation[4] += valueX;
                    _camera.transform.rotation = Quaternion.Euler(xRotation[4], yRotation[4], 0f);
                    return;
                }
            }
        }
    }
    private void RotationDownRight()
    {
        if (controlIndex == 6)
        {
            foreach (GameObject _camera in _Cameras)
            {
                if (cameraIndex == 1 && _camera.name == "Showstage.1")
                {
                    float valueY = Time.fixedDeltaTime * _SliderValue.value;
                    float valueX = Time.fixedDeltaTime * _SliderValue.value;

                    yRotation[0] += valueY;
                    xRotation[0] += valueX;
                    _camera.transform.rotation = Quaternion.Euler(xRotation[0], yRotation[0], 0f);
                    return;
                }
                else if (cameraIndex == 2 && _camera.name == "DiningRoom.1")
                {
                    float valueY = Time.fixedDeltaTime * _SliderValue.value;
                    float valueX = Time.fixedDeltaTime * _SliderValue.value;

                    yRotation[1] += valueY;
                    xRotation[1] += valueX;
                    _camera.transform.rotation = Quaternion.Euler(xRotation[1], yRotation[1], 0f);
                    return;
                }
                else if (cameraIndex == 3 && _camera.name == "Enterance")
                {
                    float valueY = Time.fixedDeltaTime * _SliderValue.value;
                    float valueX = Time.fixedDeltaTime * _SliderValue.value;

                    yRotation[2] += valueY;
                    xRotation[2] += valueX;
                    _camera.transform.rotation = Quaternion.Euler(xRotation[2], yRotation[2], 0f);
                    return;
                }
                else if (cameraIndex == 4 && _camera.name == "Showstage.2")
                {
                    float valueY = Time.fixedDeltaTime * _SliderValue.value;
                    float valueX = Time.fixedDeltaTime * _SliderValue.value;

                    yRotation[3] += valueY;
                    xRotation[3] += valueX;
                    _camera.transform.rotation = Quaternion.Euler(xRotation[3], yRotation[3], 0f);
                    return;
                }
                else if (cameraIndex == 5 && _camera.name == "DiningRoom.2")
                {
                    float valueY = Time.fixedDeltaTime * _SliderValue.value;
                    float valueX = Time.fixedDeltaTime * _SliderValue.value;

                    yRotation[4] += valueY;
                    xRotation[4] += valueX;
                    _camera.transform.rotation = Quaternion.Euler(xRotation[4], yRotation[4], 0f);
                    return;
                }
            }
        }
    }
    private void RotationDownLeft()
    {
        if (controlIndex == 7)
        {
            foreach (GameObject _camera in _Cameras)
            {
                if (cameraIndex == 1 && _camera.name == "Showstage.1")
                {
                    float valueY = Time.fixedDeltaTime * _SliderValue.value;
                    float valueX = Time.fixedDeltaTime * _SliderValue.value;

                    yRotation[0] -= valueY;
                    xRotation[0] += valueX;
                    _camera.transform.rotation = Quaternion.Euler(xRotation[0], yRotation[0], 0f);
                    return;
                }
                else if (cameraIndex == 2 && _camera.name == "DiningRoom.1")
                {
                    float valueY = Time.fixedDeltaTime * _SliderValue.value;
                    float valueX = Time.fixedDeltaTime * _SliderValue.value;

                    yRotation[1] -= valueY;
                    xRotation[1] += valueX;
                    _camera.transform.rotation = Quaternion.Euler(xRotation[1], yRotation[1], 0f);
                    return;
                }
                else if (cameraIndex == 3 && _camera.name == "Enterance")
                {
                    float valueY = Time.fixedDeltaTime * _SliderValue.value;
                    float valueX = Time.fixedDeltaTime * _SliderValue.value;

                    yRotation[2] -= valueY;
                    xRotation[2] += valueX;
                    _camera.transform.rotation = Quaternion.Euler(xRotation[2], yRotation[2], 0f);
                    return;
                }
                else if (cameraIndex == 4 && _camera.name == "Showstage.2")
                {
                    float valueY = Time.fixedDeltaTime * _SliderValue.value;
                    float valueX = Time.fixedDeltaTime * _SliderValue.value;

                    yRotation[3] -= valueY;
                    xRotation[3] += valueX;
                    _camera.transform.rotation = Quaternion.Euler(xRotation[3], yRotation[3], 0f);
                    return;
                }
                else if (cameraIndex == 5 && _camera.name == "DiningRoom.2")
                {
                    float valueY = Time.fixedDeltaTime * _SliderValue.value;
                    float valueX = Time.fixedDeltaTime * _SliderValue.value;

                    yRotation[4] -= valueY;
                    xRotation[4] += valueX;
                    _camera.transform.rotation = Quaternion.Euler(xRotation[4], yRotation[4], 0f);
                    return;
                }
            }
        }
    }
    private void RotationLeft()
    {
        if (controlIndex == 8)
        {
            foreach (GameObject _camera in _Cameras)
            {
                if (cameraIndex == 1 && _camera.name == "Showstage.1")
                {
                    float valueY = Time.fixedDeltaTime * _SliderValue.value;

                    yRotation[0] -= valueY;
                    _camera.transform.rotation = Quaternion.Euler(xRotation[0], yRotation[0], 0f);
                    return;
                }
                else if (cameraIndex == 2 && _camera.name == "DiningRoom.1")
                {
                    float valueY = Time.fixedDeltaTime * _SliderValue.value;

                    yRotation[1] -= valueY;
                    _camera.transform.rotation = Quaternion.Euler(xRotation[1], yRotation[1], 0f);
                    return;
                }
                else if (cameraIndex == 3 && _camera.name == "Enterance")
                {
                    float valueY = Time.fixedDeltaTime * _SliderValue.value;

                    yRotation[2] -= valueY;
                    _camera.transform.rotation = Quaternion.Euler(xRotation[2], yRotation[2], 0f);
                    return;
                }
                else if (cameraIndex == 4 && _camera.name == "Showstage.2")
                {
                    float valueY = Time.fixedDeltaTime * _SliderValue.value;

                    yRotation[3] -= valueY;
                    _camera.transform.rotation = Quaternion.Euler(xRotation[3], yRotation[3], 0f);
                    return;
                }
                else if (cameraIndex == 5 && _camera.name == "DiningRoom.2")
                {
                    float valueY = Time.fixedDeltaTime * _SliderValue.value;

                    yRotation[4] -= valueY;
                    _camera.transform.rotation = Quaternion.Euler(xRotation[4], yRotation[4], 0f);
                    return;
                }
            }
        }
    }
    private void RotationRight()
    {
        if (controlIndex == 9)
        {
            foreach (GameObject _camera in _Cameras)
            {
                if (cameraIndex == 1 && _camera.name == "Showstage.1")
                {
                    float valueY = Time.fixedDeltaTime * _SliderValue.value;

                    yRotation[0] += valueY;
                    _camera.transform.rotation = Quaternion.Euler(xRotation[0], yRotation[0], 0f);
                    return;
                }
                else if (cameraIndex == 2 && _camera.name == "DiningRoom.1")
                {
                    float valueY = Time.fixedDeltaTime * _SliderValue.value;

                    yRotation[1] += valueY;
                    _camera.transform.rotation = Quaternion.Euler(xRotation[1], yRotation[1], 0f);
                    return;
                }
                else if (cameraIndex == 3 && _camera.name == "Enterance")
                {
                    float valueY = Time.fixedDeltaTime * _SliderValue.value;

                    yRotation[2] += valueY;
                    _camera.transform.rotation = Quaternion.Euler(xRotation[2], yRotation[2], 0f);
                    return;
                }
                else if (cameraIndex == 4 && _camera.name == "Showstage.2")
                {
                    float valueY = Time.fixedDeltaTime * _SliderValue.value;

                    yRotation[3] += valueY;
                    _camera.transform.rotation = Quaternion.Euler(xRotation[3], yRotation[3], 0f);
                    return;
                }
                else if (cameraIndex == 5 && _camera.name == "DiningRoom.2")
                {
                    float valueY = Time.fixedDeltaTime * _SliderValue.value;

                    yRotation[4]+= valueY;
                    _camera.transform.rotation = Quaternion.Euler(xRotation[4], yRotation[4], 0f);
                    return;
                }
            }
        }
    }
    private void Limiter()
    {
        //cameraIndex
        //ControlIndex
        //zoom value
        cameraIndex = Mathf.Clamp(cameraIndex, 0, 5);
        controlIndex = Mathf.Clamp(controlIndex, 0, 11);
        _zoomValue[0] = Mathf.Clamp(_zoomValue[0], 30, 70);
        _zoomValue[1] = Mathf.Clamp(_zoomValue[1], 30, 70);
        _zoomValue[2] = Mathf.Clamp(_zoomValue[2], 30, 70);
        _zoomValue[3] = Mathf.Clamp(_zoomValue[3], 30, 70);
        _zoomValue[4] = Mathf.Clamp(_zoomValue[4], 30, 70);
    }
}
