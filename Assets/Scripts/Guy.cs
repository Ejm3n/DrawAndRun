using System.Collections;
using UnityEngine;

public class Guy : MonoBehaviour
{
    public bool Alive = true;
    [Range(0, 1)]
    public float Speed;
    [Header("Components")]
    [SerializeField]
    protected Animator Anim;
    [SerializeField]
    protected SkinnedMeshRenderer SkinRenderer;
    [SerializeField]
    protected ParticleSystem deathEffect;
    //Basic
    public Vector3 Position { get { return transform.position; } set { transform.position = value; } }
    public Vector3 LocalPosition { get { return transform.localPosition; } set { transform.localPosition = value; } }

    public Animator anim => Anim;

    //Coroutine
    protected Coroutine MoveToCoroutine;

    private BoxCollider coll;

    public void Die()
    {
        StopAllCoroutines();
        if (coll == null)
            coll = GetComponent<BoxCollider>();
        coll.enabled = false;
        Speed = 0;
        Anim.SetTrigger("Die");
        deathEffect.Play();
        SkinRenderer.enabled =false;
        Alive = false;
        Game.instance.RemoveGuy(this); 
    }

    public void MoveTo(Vector3 Point)
    {
        if(MoveToCoroutine != null)
        {
            StopCoroutine(MoveToCoroutine);
        }
        MoveToCoroutine = StartCoroutine(MoveToCour(Point));
    }

    protected IEnumerator MoveToCour(Vector3 Point)
    {
        while(Vector3.Distance(transform.position, Point) > 1)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, Point, Speed / 10);
            yield return new WaitForFixedUpdate();
        }
        MoveToCoroutine = null;
        yield break;
    }

    public void SetColor(Color color)
    {
        SkinRenderer.materials[0].SetColor("_Color", color);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            Die();
        }       
    }
}



