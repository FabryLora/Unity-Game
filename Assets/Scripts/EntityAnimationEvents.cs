using UnityEngine;

public class EntityAnimationEvents : MonoBehaviour {
    private Entity entity;

    private void Awake() {
        entity = GetComponentInParent<Entity>();
    }

    private void DisableMovementAndJump() => entity.EnableMovementAndJump(false);

    private void EnableMovementAndJump() => entity.EnableMovementAndJump(true);

    public void DamageTargets() => entity.DamageTargets();




}
