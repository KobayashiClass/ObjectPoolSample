﻿using UnityEngine;

public class BallManager : MonoBehaviour
{
    [SerializeField] Transform m_ballRoot;
    [SerializeField] GameObject m_ballPrefab;
    [SerializeField] GameObject m_superBallPrefab;
    [SerializeField] BallHopper[] m_ballHoppers;

    public ObjectPool<Ball> BallPool { get; private set; }
    public ObjectPool<SuperBall> SuperBallPool { get; private set; }

    private void Start()
    {
        BallPool = new ObjectPool<Ball>(m_ballPrefab);
        SuperBallPool = new ObjectPool<SuperBall>(m_superBallPrefab);

        foreach (var hopper in m_ballHoppers)
        {
            if (GenerateBooleanRate(0.6f))
            {
                // IFactory<Ball>　でホッパーを初期化する
                hopper.Initialize(BallPool, m_ballRoot);
            }
            else
            {
                // IFactory<SuperBall> でホッパーを初期化する
                hopper.Initialize(SuperBallPool, m_ballRoot);
            }
        }
    }

    private bool GenerateBooleanRate(float rate)
    {
        return Random.Range(0f, 1f) < rate;
    }
}