using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プールされるコンポーネントは IPooledを実装すること
/// </summary>
public interface IPoolable
{
    /// <summary>
    /// 役目を終えたタイミングで呼ぶこと
    /// </summary>
    event System.Action OnRestore;

    /// <summary>
    /// 生成時に呼ばれます
    /// </summary>
    void Initialize();

    /// <summary>
    /// アクティブ状態になると呼ばれます
    /// </summary>
    void Setup();
}

/// <summary>
/// オブジェクトを生成するできるインターフェース
/// </summary>
/// <typeparam name="T">生成する対象のコンポーネント</typeparam>
public interface IFactory<out T>
{
    /// <summary>
    /// オブジェクトを受け取ります
    /// </summary>
    /// <returns></returns>
    T Create();
}

/// <summary>
/// プレハブか、指定コンポーネントがアタッチされたオブジェクトを新規作成するクラス
/// </summary>
/// <typeparam name="T">指定コンポーネント</typeparam>
public class Factory<T> : IFactory<T> where T : MonoBehaviour
{
    protected GameObject m_src;
    
    public Factory() : this(null) { }
    public Factory(GameObject src)
    {
        m_src = src;
    }

    public virtual T Create()
    {
        return Create();
    }

    protected T Instantiate()
    {
        T t;
        if (m_src)
        {
            var obj = GameObject.Instantiate(m_src);
            t = obj.GetComponent<T>();
            if (t == null) throw new Exception($"指定したプレハブに{typeof(T).Name}がアタッチされていません");
        }
        else
        {
            var obj = new GameObject();
            t = obj.AddComponent<T>();
        }
        return t;
    }
}

/// <summary>
/// オブジェクトプールクラス プール領域外は基底の[Factory]で生成する
/// </summary>
/// <typeparam name="T">IPoolableが継承されたコンポーネント</typeparam>
public class ObjectPool<T> : Factory<T> where T : MonoBehaviour, IPoolable
{
    private List<(T instance, bool isActive)> m_instances;

    public ObjectPool() : this(null, 0) { }
    public ObjectPool(GameObject src) : this(src, 0) { }
    public ObjectPool(int poolSize) : this(null, poolSize) { }
    public ObjectPool(GameObject src, int poolSize) : base(src)
    {
        m_instances = new List<(T, bool)>();
        for (var i = 0; i < poolSize; i++) Push();
    }

    /// <summary>
    /// プールから取得するか新しく生成する
    /// </summary>
    /// <returns>利用可能なオブジェクト</returns>
    public override T Create()
    {
        var index = FindFirstDisableIndex();
        T t;
        if (index == -1)
        {
            t = Push();
            index = m_instances.Count - 1;
        }else
        {
            t = m_instances[index].instance;
        }

        System.Action restore = null; 
        restore = () => {
            var i = FindIndex(t);
            if (i == -1) throw new Exception("instance not found");

            m_instances[i] = (t, false);
            t.OnRestore -= restore;
        };
        t.OnRestore += restore;

        m_instances[index] = (t, true);

        t.Setup();

        return t;
    }

    private int FindIndex(T instance)
    {
        return m_instances.FindIndex(tuple => tuple.instance == instance);
    }

    private int FindFirstDisableIndex()
    {
        return m_instances.FindIndex(tuple => tuple.isActive == false);
    }

    private T Push(bool isActive = false)
    {
        var t = Instantiate();
        m_instances.Add((t, isActive));
        t.Initialize();
        return t;
    }
}