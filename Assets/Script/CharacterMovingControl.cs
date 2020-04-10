using UnityEngine;
using MPJamPack;

[RequireComponent(typeof(SmartBoxCollider))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AbstractCharacterInput))]
public class CharacterMovingControl : MonoBehaviour {
    [SerializeField]
    private float movingSpeed;

    private AbstractCharacterInput input;

    private SmartBoxCollider boxCollider;
    private new Rigidbody2D rigidbody2D;

    private CharacterAnimator animator;

    private bool FaceRight {
        get {
            return transform.localScale.x > 0;
        }
    }

    private void Awake()
    {
        boxCollider = GetComponent<SmartBoxCollider>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<CharacterAnimator>();

        enabled = false;
    }

    public void SetInput(AbstractCharacterInput _input) {
        input = _input;
        enabled = true;
    }

    private void Turn(bool faceRight) {
        if (FaceRight == faceRight) return;

        transform.localScale = new Vector3(
            (faceRight? 1: -1) * Mathf.Abs(transform.localScale.x),
            transform.localScale.y,
            transform.localScale.z);
    }

    private void FixedUpdate() {
        Vector2 velocity = rigidbody2D.velocity;

        velocity.x = 0;
        if (input.Left) velocity.x -= movingSpeed;
        if (input.Right) velocity.x += movingSpeed;

        if (velocity.x < 0 && boxCollider.LeftTouched) velocity.x = 0;
        if (velocity.x > 0 && boxCollider.RightTouched) velocity.x = 0;

        if (velocity.x != 0) Turn(velocity.x > 0);

        velocity.y = 0;
        if (input.Down) velocity.y -= movingSpeed;
        if (input.Up) velocity.y += movingSpeed;

        if (velocity.y < 0 && boxCollider.DownTouched) velocity.y = 0;
        if (velocity.y > 0 && boxCollider.UpTouched) velocity.y = 0;

        if (velocity.x != 0 || velocity.y != 0) {
            animator.SetAnimation(CharacterAnimator.State.Walk);
        }
        else if (animator.enabled)
            animator.SetAnimation(CharacterAnimator.State.Idle);

        rigidbody2D.velocity = velocity;
    }

    public void Stop() {
        rigidbody2D.velocity = Vector3.zero;
    }
}