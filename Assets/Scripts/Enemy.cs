using UnityEngine;

public class Enemy : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Guy guy;
        if (guy=other.GetComponent<Guy>())
            guy.anim.SetTrigger("Die");        
    }
}
