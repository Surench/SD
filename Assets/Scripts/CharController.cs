using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour
{
    [SerializeField] Animator animator;

    [SerializeField] SkinnedMeshRenderer meshRenderer;

    public void SetMaterial(Material material)
    {
        meshRenderer.material = material;
    }

    public void Standing()
    {
        animator.SetBool("standing", true);
        animator.SetBool("surfing", false);
    }

    public void Moving()
    {
        animator.SetBool("standing", false);
        animator.SetBool("surfing", true);
    }

    public void Flip()
    {
        animator.SetTrigger("flip");
        animator.SetBool("surfing", false);
    }
}
