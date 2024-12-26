using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using Utility;
using Debug = Utility.DebugExtensions;

public class Roulette : MonoBehaviour, ISpinButtonHandler, IPinHandler
{
    private List<GameObject> triangles = new();
    private GameObject pinObj;

    [Header("Other Roulette Stuff")]
    public GameObject pin;
    public GameObject spinBtn;
    public Material highlightMat;
    public Material rouletteMat;
    
    [Header("Settings")]
    public Rigidbody2D rigidBody;
    [Range(3, 72), ValidateInput("ValidatePolygonSides", "Polygon Sides are not divisible by 360!")] public int polygonSides;
    [Range(0.5f, 8)] public float radius;
    [Range(50, 500)] public int stopVelocity;
    [Range(1500, 3500)] public int torque;

    [ShowInInspector] private bool _justClicked;
    [ShowInInspector] private bool _isSpinning;
    [ShowInInspector] private bool _isStopped;
    
    // remove later and put in separate data script
    [ShowInInspector] private int landIndex;
    
    private void Start() => Init();

    private void Update()
    {
        CheckSpin();
        HighlightCard();
    }

    #region Methods

        private void Init()
        {
            if (!ValidatePolygonSides())
            {
                Debug.Log("CHANGE NUMBER OF POLYGON SIDES!", LogType.Error);
                EditorApplication.isPlaying = false;
                return;
            }
            
            _isStopped = false;
            landIndex = -1;
            
            triangles = MeshExtensions.DrawCircle(polygonSides, radius, rouletteMat);
            triangles.ForEach(x =>
            {
                x.transform.SetParent(transform);
                PolygonColliderExtensions.CreatePolygon2DColliderPoints(x.GetComponent<MeshFilter>(), x.GetComponent<PolygonCollider2D>());
            });
            
            var trigger = Instantiate(spinBtn, Vector3.zero, Quaternion.identity, transform.parent).GetComponent<EventTrigger>();
            var trigEvent = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerUp
            };
            
            trigEvent.callback.AddListener(Spin);
            trigger.triggers.Add(trigEvent);
            
            pinObj = Instantiate(pin, Vector3.up, Quaternion.identity, transform.parent);
        }
    
        private void CheckSpin()
        {
            if (rigidBody.angularVelocity <= stopVelocity && _isSpinning && !_justClicked)
            {
                var angle = (int)(transform.localRotation.eulerAngles.z);
                if (angle % (360 / polygonSides) == 0)
                {
                    rigidBody.velocity = Vector2.zero;
                    rigidBody.angularVelocity = 0;
                    transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));
                    
                    _isSpinning = false;
                    _isStopped = true;
                }
            }
            else if (rigidBody.angularVelocity > stopVelocity && _isSpinning && _justClicked)
            {
                _justClicked = false;
            }
        }
        
        [PropertySpace(10), Button("Spin Roulette")]
        public void Spin(BaseEventData eventData)
        {
            if (_isSpinning) return;
            if (landIndex > 0) triangles[landIndex-1].GetComponent<PolygonCollider2D>().RemoveHighlightAroundCollider();
            _isSpinning = _justClicked = true;
            _isStopped = false;
            landIndex = -1;
            rigidBody.AddTorque(Random.Range(torque, torque * 2.5f));
        }
    
        public void HighlightCard()
        {
            if (!_isStopped || _justClicked || _isSpinning) return;
            if (landIndex > 0) return;
            var contact = Physics2D.OverlapPoint(pinObj.transform.position);
            if (contact)
            {
                landIndex = int.Parse(contact.name);
                (contact as PolygonCollider2D).HighlightAroundCollider(Color.yellow, highlightMat);
            }
        }

        private bool ValidatePolygonSides() => 360 % polygonSides == 0;

    #endregion
        
}
