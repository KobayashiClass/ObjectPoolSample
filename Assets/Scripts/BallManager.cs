using UnityEngine;

public class BallManager : MonoBehaviour
{
    [SerializeField] Transform m_ballRoot;
    [SerializeField] GameObject m_ballPrefab;
    [SerializeField] BallHopper[] m_ballHoppers;

    public ObjectPool<Ball> BallPool { get; private set; }

    private void Start()
    {
        BallPool = new ObjectPool<Ball>(m_ballPrefab);

        foreach (var hopper in m_ballHoppers)
        {
            // IFactory<Ball>　でホッパーを初期化する
            hopper.Initialize(BallPool, m_ballRoot);
        }
    }
}