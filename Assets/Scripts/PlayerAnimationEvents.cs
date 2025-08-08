using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour {
    private Entity player;

    private void Awake() {
        player = GetComponentInParent<Entity>();
    }


    private void DisableMovementAndJump() {
        player.EnableMovementAndJump(false);
    }

    private void EnableMovementAndJump() {
        player.EnableMovementAndJump(true);
    }

    /* public void DamageEnemies() {
        player.DamageEnemies();
    } */



}
