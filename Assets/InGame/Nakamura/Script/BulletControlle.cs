using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BulletControlle : MonoBehaviour
{
    [SerializeField,Header("bulletの移動方向")] BulletDir _bulletDir;
    [SerializeField, Header("bulletの速度")] int _bulletSpeed = 5;
    Rigidbody2D _rb2d;

    [Tooltip("どの方向に飛ばすか")]
    bool[] _dirBool;
    [Tooltip("力をかける方向")]
    Vector2[] _dirPower = new Vector2[] {new Vector2(-1, 0), new Vector2(1, 0), new Vector2(0, 1), new Vector2(0, -1) };
    [Tooltip("力をかける方向を保存しておくための変数")]
    Vector2 dir;
    
    //float _upVector = 1;
    //float _downVector = -1;
    //float _rightVector = 1;
    //float _leftVector = -1;
    // Start is called before the first frame update
    void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Start()
    {
        //if (_bulletDir.GetUp)
        //{
        //    if (_bulletDir.GetLeft)
        //    {
        //        _rb2d.velocity = new Vector2(_leftVector, _upVector).normalized * _bulletSpeed;
        //    }
        //    else if (_bulletDir.GetRight)
        //    {
        //        _rb2d.velocity = new Vector2(_rightVector, _upVector).normalized * _bulletSpeed;
        //    }
        //    else
        //    {
        //        _rb2d.velocity = new Vector2(0, _upVector).normalized * _bulletSpeed;
        //    }
        //}
        //else if (_bulletDir.GetDown)
        //{
        //    if (_bulletDir.GetLeft)
        //    {
        //        _rb2d.velocity = new Vector2(_leftVector, _downVector).normalized * _bulletSpeed;
        //    }
        //    else if (_bulletDir.GetRight)
        //    {
        //        _rb2d.velocity = new Vector2(_rightVector, _downVector).normalized * _bulletSpeed;
        //    }
        //    else
        //    {
        //        _rb2d.velocity = new Vector2(0, _downVector).normalized * _bulletSpeed;
        //    }
        //}
        //else if (_bulletDir.GetRight)
        //{
        //    _rb2d.velocity = new Vector2(_rightVector, 0).normalized * _bulletSpeed;
        //}
        //else if (_bulletDir.GetLeft)
        //{
        //    _rb2d.velocity = new Vector2(_leftVector, 0).normalized * _bulletSpeed;
        //}
        //else
        //{
        //    _rb2d.velocity = new Vector2(0, 0) * _bulletSpeed;
        //}
        _dirBool = new bool[] { _bulletDir.GetLeft, _bulletDir.GetRight, _bulletDir.GetUp, _bulletDir.GetDown };
        dir = Vector2.zero;
        for (int i = 0; i < _dirBool.Length; i++) 
        {
            if (_dirBool[i]) 
            {
                dir += _dirPower[i];
            }
        }

        _rb2d.velocity = dir.normalized * _bulletSpeed;
    }
}

[System.Serializable]
class BulletDir
{
    [SerializeField, Header("上方向")] private bool _up = false;
    public bool GetUp
    {
        get
        {
            return _up;
        }
    }
    [SerializeField, Header("下方向")] private bool _down = false;
    public bool GetDown
    {
        get
        {
            return _down;
        }
    }
    [SerializeField, Header("右方向")] private bool _right = false;
    public bool GetRight
    {
        get
        {
            return _right;
        }
    }
    [SerializeField, Header("左方向")] private bool _lleft = false;
    public bool GetLeft
    {
        get
        {
            return _lleft;
        }
    }
}
