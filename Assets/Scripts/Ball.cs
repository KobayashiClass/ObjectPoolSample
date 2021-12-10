using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プールされる前提のボールクラス
/// </summary>
public class Ball : MonoBehaviour, IPoolable
{
    public event Action OnRestore;
    protected Rigidbody m_rb;

    public void Initialize()
    {
        gameObject.SetActive(false);
        m_rb = GetComponent<Rigidbody>();
    }

    public void Setup()
    {
        gameObject.SetActive(true);
        m_rb.velocity = Vector3.zero;
        m_rb.angularVelocity = Vector3.zero;
    }

    /// <summary>
    /// 位置と回転と親をリセットします。
    /// </summary>
    /// <param name="parent">親</param>
    /// <param name="locate">ロケーター</param>
    public void SetTransform(Transform parent, Transform locate)
    {
        transform.parent = parent;
        transform.position = locate.position;
        transform.rotation = locate.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "GetArea"){
            if (OnRestore != null) OnRestore();
            gameObject.SetActive(false);
        }
    }
}
