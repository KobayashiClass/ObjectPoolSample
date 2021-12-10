using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ホッパーはどうやって生成するかは知らなくていい
// 生成に必要な機能 IFactory<Ball> のみを渡してあげた

public class BallHopper : MonoBehaviour
{
    Transform m_ballRoot;
    IFactory<Ball> m_ballPool;
    Coroutine m_popCoroutine;
    [SerializeField] float m_popSpeed = 1f;

    public void Initialize(IFactory<Ball> ballPool, Transform ballRoot)
    {
        m_ballRoot = ballRoot;
        m_ballPool = ballPool;
        PopStart();
    }

    private void PopStart()
    {
        m_popCoroutine = StartCoroutine(Pop());
    }

    private IEnumerator Pop()
    {
        while (true)
        {
            var ball = m_ballPool.Get();
            ball.SetTransform(m_ballRoot, transform);
            yield return new WaitForSeconds(m_popSpeed);
        }
    }
}
