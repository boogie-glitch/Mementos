using UnityEngine;
using UnityEngine.Playables;

// A behaviour that is attached to a playable
public class PlayerAttacking : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerController.Instance.anim.SetBool(AnimationStrings.isAttacking, false);
    }
}
