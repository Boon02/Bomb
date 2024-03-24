using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IInteract, ICollect{
    
    [Range(0, 20)] [SerializeField] private float moveSpeed = 7f;
    private LayerMask colliderLayer;

    public Action<int, BombID> OnSetBomb;
    public Action OnDead;

    private bool isWalking;
    private InputPlayer inputPlayerActions;
    
    private BombID bombID;
    private int health;
    private int radiusBomb;
    
    public bool IsWalking => isWalking;
    public float MoveSpeed => moveSpeed;
    public int RadiusBomb => radiusBomb;

    public void Init() {
        
    }
    
    void Awake() {
        inputPlayerActions = new InputPlayer();
        inputPlayerActions.Player.Enable();
        inputPlayerActions.Player.Interact.performed += Interact_performed;
    }

    private void OnDestroy() {
        inputPlayerActions.Player.Interact.performed -= Interact_performed;
        
        inputPlayerActions.Player.Disable();
    }

    private void Interact_performed(InputAction.CallbackContext obj) {
        OnSetBomb?.Invoke(radiusBomb, bombID);
    }

    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        HandleMovement();
    }

    private Vector2 GetMovementVectorNormalized() {
        return inputPlayerActions.Player.Moving.ReadValue<Vector2>().normalized;
    }
    void HandleMovement() {
        var inputVector = GetMovementVectorNormalized();
        var moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerHeight = 2f;
        
        bool canMove = !Physics.BoxCast(transform.position, Vector3.one * GameManager.HEIGHT_PLAYER / 2, moveDir, Quaternion.identity, moveDistance, colliderLayer);
        
        if(!canMove){
            // try move x
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = (moveDir.x < -0.5 || moveDir.x > 0.5) &&  !Physics.BoxCast(transform.position, Vector3.one * GameManager.HEIGHT_PLAYER / 2, moveDirX, Quaternion.identity, moveDistance, colliderLayer);
            if (canMove) {
                moveDir = moveDirX;
            }
            else {
                // try move y
                Vector3 moveDirY = new Vector3(0, 0, moveDir.y).normalized;
                canMove = (moveDir.y < -0.5 || moveDir.y > 0.5) &&  !Physics.BoxCast(transform.position, Vector3.one * GameManager.HEIGHT_PLAYER / 2, moveDirY, Quaternion.identity, moveDistance, colliderLayer);
                if (canMove) {
                    moveDir = moveDirY;
                }
            }
        }

        if (canMove) {
            transform.position += moveDir * moveDistance;
        }

        isWalking = moveDir != Vector3.zero;
        transform.forward = Vector3.Lerp(transform.forward, moveDir, Time.deltaTime * 10f);
    }


    public bool IsDead => health <= 0;
    public void Interact() {
        if (IsDead) return;

        health -= 1;
        if (IsDead) {
            OnDead?.Invoke();
        }
    }

    public void Collect(ItemID itemID, int value, ValueTypeBooster type) {
        GameManager.Instance.Collect(this, itemID, value, type);
    }

    public void SetSpeed(float moveSpeed) {
        this.moveSpeed = moveSpeed;
    }

    public void SetHealth(int health) {
        this.health = health;
    }

    public void SetBombID(BombID bombID) {
        this.bombID = bombID;
    }

    public void SetColliderLayer(LayerMask colliderLayer) {
        this.colliderLayer = colliderLayer;
    }

    public void SetRadiusBomb(int radiusBomb) {
        this.radiusBomb = radiusBomb;
    }
}