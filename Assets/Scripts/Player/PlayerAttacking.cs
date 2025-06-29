using UnityEngine;
using UnityEngine.Playables;

// A behaviour that is attached to a playable
public class PlayerAttacking : StateMachineBehaviour
{
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (PlayerController.Instance.IsMoveing)
        {
            PlayerController.Instance.isMoveAttack = true;
        }
        PlayerController.Instance.canTurn = false; // Disable turning while attacking
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerController.Instance.anim.SetBool(AnimationStrings.isAttacking, false);
        //PlayerController.Instance.isAttacking = false;
        PlayerController.Instance.canTurn = true; // Re-enable turning after attack
        PlayerController.Instance.SetFacingDirection(PlayerController.Instance.moveInput);
    }
}
