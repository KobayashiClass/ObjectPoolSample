using UnityEngine;

public class SuperBall : Ball
{
    [SerializeField] float m_boundForce = 5f;

    private void OnCollisionEnter(Collision collision)
    {
        m_rb.AddForce(m_boundForce * collision.impulse, ForceMode.Impulse);
    }
}
