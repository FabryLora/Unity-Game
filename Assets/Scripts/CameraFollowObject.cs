/* using System.Collections;
using UnityEngine;

public class CameraFollowObject : MonoBehaviour {
    [Header("References")]

    [SerializeField] private Transform _playerTransform;

    [Header("Flip rotation stats")]
    [SerializeField] private float _flipYRotationTime = 0.5f;

    private Coroutine _turnCoroutine;

    private Entity _player;

    private bool _isFacingRight;

    void Awake() {
        _player = _playerTransform.GetComponent<Entity>();

        _isFacingRight = _player.IsFacingRight;
    }

    void Update() {
        transform.position = _playerTransform.position;
    }

    public void CallTurn() {
        LeanTween.rotateY(gameObject, DetermineEndRRotation(), _flipYRotationTime).setEaseInOutSine();
    }



    private float DetermineEndRRotation() {
        _isFacingRight = !_isFacingRight;

        if (_isFacingRight) {
            return 180f;
        }
        else {
            return 0f;
        }
    }


}
 */