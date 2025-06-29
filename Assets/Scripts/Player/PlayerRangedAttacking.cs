using UnityEngine;
using UnityEngine.Playables;

public class PlayerRangedAttacking : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerController.Instance.anim.SetBool(AnimationStrings.isRangeAttack, false);
    }
}
