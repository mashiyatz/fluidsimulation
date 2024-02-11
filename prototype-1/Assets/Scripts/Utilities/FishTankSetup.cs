using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishTankSetup : MonoBehaviour
{
    [SerializeField]
    private Transform tankBase;
    [SerializeField]
    private Transform wallLeft;
    [SerializeField]
    private Transform wallRight;
    [SerializeField]
    private Transform wallFront;
    [SerializeField]
    private Transform wallBack;

    [Min(5f)]
    public float height = 7f;
    [Min(5f)]
    public float length = 7f;
    [Min(5f)]
    public float width = 7f;

    private float _height = 7f;
    private float _length = 7f;
    private float _width = 7f;

    public static float boundaryWidth;

    private List<Transform> walls = new();

    public float Height
    {
        get { return _height; }
        set
        {
            if (_height == value) return;
            _height = value;
            if (OnHeightChange != null) OnHeightChange(_height);
        }
    }

    public float Length
    {
        get { return _length; }
        set
        {
            if (_length == value) return;
            _length = value;
            if (OnLengthChange != null) OnLengthChange(_length);
        }
    }

    public float Width
    {
        get { return _width; }
        set
        {
            if (_width == value) return;
            _width = value;
            if (OnWidthChange != null) OnWidthChange(_width);
        }
    }


    public delegate void OnHeightChangeDelegate(float newVal);
    public event OnHeightChangeDelegate OnHeightChange;

    public delegate void OnLengthChangeDelegate(float newVal);
    public event OnLengthChangeDelegate OnLengthChange;

    public delegate void OnWidthChangeDelegate(float newVal);
    public event OnWidthChangeDelegate OnWidthChange;


    void Start()
    {
        walls.Add(wallLeft);
        walls.Add(wallRight);
        walls.Add(wallFront);
        walls.Add(wallBack);
        OnHeightChange += HeightChangeHandler;        
        OnLengthChange += LengthChangeHandler;        
        OnWidthChange += WidthChangeHandler;        
    }

    private void HeightChangeHandler(float newVal)
    {
        foreach (Transform wall in walls) {
            wall.localScale = new Vector3(wall.localScale.x, newVal, wall.localScale.z);
            wall.position = new Vector3(wall.position.x, newVal / 2, wall.position.z);
        }
    }

    private void LengthChangeHandler(float newVal)
    {
        tankBase.localScale = new Vector3(newVal, tankBase.localScale.y, tankBase.localScale.z);
        wallFront.localScale = new Vector3(newVal, wallFront.localScale.y, wallFront.localScale.z);
        wallBack.localScale = new Vector3(newVal, wallBack.localScale.y, wallBack.localScale.z);

        wallLeft.position = new Vector3(newVal / 2, wallLeft.position.y, wallLeft.position.z);
        wallRight.position = new Vector3(-newVal / 2, wallRight.position.y, wallRight.position.z);
    }

    private void WidthChangeHandler(float newVal)
    {
        tankBase.localScale = new Vector3(tankBase.localScale.x, newVal, tankBase.localScale.z);
        wallLeft.localScale = new Vector3(newVal, wallLeft.localScale.y, wallLeft.localScale.z);
        wallRight.localScale = new Vector3(newVal, wallRight.localScale.y, wallRight.localScale.z);
        
        wallFront.position = new Vector3(wallFront.position.x, wallFront.position.y, newVal / 2);
        wallBack.position = new Vector3(wallBack.position.x, wallBack.position.y, -newVal / 2);
    }

    void Update()
    {
        Height = height;
        Length = length;
        Width = width;

        boundaryWidth = Width;
    }
}
