using UnityEngine;

public class StandingGuy : Guy
{
    [SerializeField] private Material standingColor;
    [SerializeField] private Material runColor;
    private Rigidbody rb;

    private void Awake()
    {
       rb= GetComponent<Rigidbody>();
        
        SkinRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        SkinRenderer.material = standingColor;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<Guy>())
        {
            rb.isKinematic = true;
            anim.enabled = true;
            anim.SetTrigger("Run");
            SkinRenderer.material = runColor;
            Game.instance.AddGuy(this);
        }    
    }
}
