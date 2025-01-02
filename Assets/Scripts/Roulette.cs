using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using Utility;
using Debug = Utility.DebugExtensions;

public class Roulette : GameItem, IButtonHandler, IHighlightHandler
{
    private List<GameObject> triangles = new();
    private GameObject pinObj;

    [ShowInInspector] private bool _isClicked;
    [ShowInInspector] private bool _isSpinning;
    [ShowInInspector] private bool _isStopped;
    
    [Header("Settings")]
    public Rigidbody2D rigidBody;
    [Range(0.5f, 8)] public float radius;
    [Range(50, 500)] public int stopVelocity;
    [Range(1500, 3500)] public int torque;

    [Header("Other Things")]
    public GameObject pin;
    public GameObject spinBtn;
    public Material highlightMat;
    public Material rouletteMat;
    
    private void Update()
    {
        CheckSpin();
        Highlight();
    }

    #region Methods
    
        public override async void Init()
        {
            if (!GameManager.Instance.ValidatePolygonSides())
            {
                Debug.Log("CHANGE NUMBER OF POLYGON SIDES!", LogType.Error);
                EditorApplication.isPlaying = false;
                return;
            }
            
            _isStopped = false;
            GameManager.Instance.SetLandIndex(-1);
            
            triangles = MeshExtensions.DrawCircle(GameManager.Instance.polygonSides, radius, rouletteMat);
            triangles.ForEach(x =>
            {
                x.transform.SetParent(itemRenderer.transform);
                PolygonColliderExtensions.CreatePolygon2DColliderPoints(x.GetComponent<MeshFilter>(), x.GetComponent<PolygonCollider2D>());
            });
            
            CreateButton();
            pinObj = Instantiate(pin, Vector3.up, Quaternion.identity, transform);
            base.Init();
            await Hide();
        }
    
        private void CheckSpin()
        {
            if (rigidBody.angularVelocity <= stopVelocity && _isSpinning && !_isClicked)
            {
                var angle = (int)(itemRenderer.transform.localRotation.eulerAngles.z);
                if (angle % (360 / GameManager.Instance.polygonSides) == 0)
                {
                    rigidBody.velocity = Vector2.zero;
                    rigidBody.angularVelocity = 0;
                    itemRenderer.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));
                    
                    _isSpinning = false;
                    _isStopped = true;

                    GameManager.Instance.gameData.CheckWinningNumbers();
                }
            }
            else if (rigidBody.angularVelocity > stopVelocity && _isSpinning && _isClicked)
            {
                _isClicked = false;
            }
        }

        public void CreateButton()
        {
            var trigger = Instantiate(spinBtn, Vector3.zero, Quaternion.identity, transform).GetComponent<EventTrigger>();
            var trigEvent = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerUp
            };
            
            trigEvent.callback.AddListener(OnAction);
            trigger.triggers.Add(trigEvent);
        }

        [PropertySpace(10), Button("Spin Roulette")]
        public void OnAction(BaseEventData eventData)
        {
            if (_isSpinning) return;
            if (GameManager.Instance.GetLandIndex() > 0) triangles[GameManager.Instance.GetLandIndex()-1].GetComponent<PolygonCollider2D>().RemoveHighlightAroundCollider();
            _isSpinning = _isClicked = true;
            _isStopped = false;
            GameManager.Instance.SetLandIndex(-1);
            rigidBody.AddTorque(Random.Range(torque, torque * 2.5f));
        }
    
        public void Highlight()
        {
            if (!_isStopped || _isClicked || _isSpinning) return;
            if (GameManager.Instance.GetLandIndex() > 0) return;
            var contact = Physics2D.OverlapPoint(pinObj.transform.position);
            if (contact)
            {
                GameManager.Instance.SetLandIndex(int.Parse(contact.name));
                (contact as PolygonCollider2D).HighlightAroundCollider(Color.yellow, highlightMat);
            }
        }
        
        // TODO: REDESIGN ROULETTE WHEEL
        // ENLARGE SLICES SO BETTER ABLE TO SEE NUMBER
        // MAYBE USE UI INSTEAD OF MESH RENDERER?

        #endregion
}