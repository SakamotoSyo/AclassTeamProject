using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//ObjectPoolとはObjectを大量にInstantiateまたはDestroyするとパーフォーマンスが落ちることを防ぐためのデザインパターン
//弾幕ゲームやvampireSurvivorsのように大量のInstantiateまたはDestroyするゲームでは必須
//逆にあまり大量のInstantiateまたはDestroyしないゲームではObjectをあらかじめ生成して置いておくのにもメモリを使うためObjectPoolを使う必要はない

public class ObjectPool : SingletonBehaviour<ObjectPool>
{
    [SerializeField] private ObjectPoolData _objectPoolData = default;
    [Header("弾幕をスポーンさせる場所")]
    [SerializeField] private List<Transform> _spawnList = new List<Transform>();

    [Tooltip("プールするList")]
    private List<Pool> _pool = new List<Pool>();

    private int _poolCountIndex = 0;

    protected override void OnAwake()
    {
        _poolCountIndex = 0;
        CreatePool();

    }

    /// <summary> 設定したオブジェクトの種類、数だけプールにオブジェクトを生成して追加する </summary>
    private void CreatePool()
    {
        if (_poolCountIndex >= _objectPoolData.Data.Length)
        {
            //オブジェクトを生成し終わったら再起処理をやめる
            return;
        }

        //poolDataに設定した数だけオブジェクトを生成する
        for (int i = 0; i < _objectPoolData.Data[_poolCountIndex].MaxCount; i++)
        {
            var obj = Instantiate(_objectPoolData.Data[_poolCountIndex].PrefabObj, this.transform);
            obj.SetActive(false);
            _pool.Add(new Pool(obj, _objectPoolData.Data[_poolCountIndex].PoolObjectType));
        }

        _poolCountIndex++;
        CreatePool();
    }

    /// <summary>
    /// オブジェクトを座標で使痛いときに呼び出す関数
    /// </summary>
    /// <param name="position">オブジェクトをスポーンさせる位置を指定する</param>
    /// <param name="objectType">オブジェクトの種類</param>
    /// <returns>生成したオブジェクト</returns>
    public GameObject UseObject(Vector2 position, PoolObjectType objectType)
    {
        foreach (var pool in _pool)
        {
            //オブジェクトが現在プールに入っている状態かつobjectのTypeが一致していたら
            //指定したPositionにObjectを移動させてSetActiveをTrueにする
            //Objectは役目を終えたらSetActiveをfalseにすることでつかいまわすことができる
            if (pool.Object.activeSelf == false && pool.Type == objectType)
            {
                pool.Object.SetActive(true);
                pool.Object.transform.position = position;
                return pool.Object;
            }

        }

        //プールの中に該当するTypeのObjectがなかったら生成する
        var newObj = Instantiate(Array.Find(_objectPoolData.Data, x => x.PoolObjectType == objectType).PrefabObj, this.transform);
        newObj.transform.position = position;
        newObj.SetActive(true);
        _pool.Add(new Pool(newObj, objectType));

        Debug.LogWarning($"{objectType}のプールのオブジェクト数が足りなかったため新たにオブジェクトを生成します" +
       $"\nこのオブジェクトはプールの最大値が少ない可能性があります" +
       $"現在{objectType}の数は{_pool.FindAll(x => x.Type == objectType).Count}です");

        return newObj;

    }

    /// <summary>
    /// オブジェクトを使痛いときに呼び出す関数
    /// </summary>
    /// <param name="position">オブジェクトをスポーンさせる位置を指定する</param>
    /// <param name="objectType">オブジェクトの種類</param>
    /// <returns>生成したオブジェクト</returns>
    public GameObject UseObject(int ListIndex, PoolObjectType objectType)
    {
        foreach (var pool in _pool)
        {
            //オブジェクトが現在プールに入っている状態かつobjectのTypeが一致していたら
            //指定したPositionにObjectを移動させてSetActiveをTrueにする
            //Objectは役目を終えたらSetActiveをfalseにすることでつかいまわすことができる
            if (pool.Object.activeSelf == false && pool.Type == objectType)
            {
                pool.Object.SetActive(true);
                pool.Object.transform.position = _spawnList[ListIndex].position;
                return pool.Object;
            }

        }

        //プールの中に該当するTypeのObjectがなかったら生成する
        var newObj = Instantiate(Array.Find(_objectPoolData.Data, x => x.PoolObjectType == objectType).PrefabObj, this.transform);
        newObj.transform.position = _spawnList[ListIndex].position; ;
        newObj.SetActive(true);
        _pool.Add(new Pool(newObj, objectType));

        Debug.LogWarning($"{objectType}のプールのオブジェクト数が足りなかったため新たにオブジェクトを生成します" +
       $"\nこのオブジェクトはプールの最大値が少ない可能性があります" +
       $"現在{objectType}の数は{_pool.FindAll(x => x.Type == objectType).Count}です");

        return newObj;

    }
}


/// <summary> プールするObjを保存するための構造体 </summary>
public struct Pool
{
    public GameObject Object;
    public PoolObjectType Type;

    public Pool(GameObject g, PoolObjectType t)
    {
        Object = g;
        Type = t;
    }

}

public enum PoolObjectType
{
    /// <summary>円形に広がる弾幕</summary>
    Hamon,
    /// <summary>左から右に流れる弾幕</summary>
    SLefteRight,
    /// <summary>右から左に流れる弾幕</summary>
    SRightELeft,
    /// <summary>上から下に流れる弾幕</summary>
    SUpEDown,
    /// <summary>下から上に流れる弾幕</summary>
    SDownEUp,
    /// <summary>左下から右上に流れる弾幕</summary>
    BottomLeft,
    /// <summary>右下から左下に流れる弾幕</summary>
    BottomRight,
    /// <summary>左上から右下に流れる弾幕</summary>
    TopLeft,
    /// <summary>右上に左下に流れる弾幕</summary>
    TopRight
}