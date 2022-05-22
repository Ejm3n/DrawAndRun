using UnityEngine;

public class Bonus : MonoBehaviour
{
    [SerializeField] private int points;
    private ParticleSystem particle;
    private MeshRenderer mesh;
    private BoxCollider coll;

    private void Awake()
    {
        coll = GetComponent<BoxCollider>();
        mesh = GetComponent<MeshRenderer>();
        particle = GetComponentInChildren<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        coll.enabled = false;
        Game.instance.AddBonus(points);
        particle.Play();
        mesh.enabled = false;
        Destroy(this,3);
    }
}
