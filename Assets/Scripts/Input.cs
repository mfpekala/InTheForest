using UnityEngine;

public class InputController : MonoBehaviour
{
    public Animator animator;
    public bool hidden = false;

    void Update()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.enabled = !hidden && !Laws.Instance.inMenu;
        }

        int newAnimState = 0;
        switch (Laws.Instance.state)
        {
            case InputState.Empty:
                newAnimState = 0;
                break;
            case InputState.Q:
                newAnimState = 1;
                break;
            case InputState.W:
                newAnimState = 2;
                break;
            case InputState.E:
                newAnimState = 3;
                break;
        }
        animator.SetInteger("stateInt", newAnimState);
    }
}
