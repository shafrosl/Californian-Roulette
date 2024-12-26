using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using Utility;
using Debug = Utility.DebugExtensions;

public class Roulette : MonoBehaviour, IButtonHandler, IHighlightHandler
{
    private List<GameObject> triangles = new();
    private GameObject pinObj;

    [ShowInInspector] private bool _justClicked;
    [ShowInInspector] private bool _isSpinning;
    [ShowInInspector] private bool _isStopped;
    
    [Header("Settings")]
    public GameObject roulette;
    public Rigidbody2D rigidBody;
    [Range(3, 72), ValidateInput("ValidatePolygonSides", "Choose another number!")] public int polygonSides;
    [Range(0.5f, 8)] public float radius;
    [Range(50, 500)] public int stopVelocity;
    [Range(1500, 3500)] public int torque;

    [Header("Other Roulette Stuff")]
    public GameObject pin;
    public GameObject spinBtn;
    public Material highlightMat;
    public Material rouletteMat;
    
    // remove later and put in separate data script
    [ShowInInspector] private int landIndex;
    private int[] excludeNumbers = { 4, 5, 8, 10, 20, 40 };
    
    private void Start() => Init();

    private void Update()
    {
        CheckSpin();
        Highlight();
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
                x.transform.SetParent(roulette.transform);
                PolygonColliderExtensions.CreatePolygon2DColliderPoints(x.GetComponent<MeshFilter>(), x.GetComponent<PolygonCollider2D>());
            });
            
            CreateButton();
            pinObj = Instantiate(pin, Vector3.up, Quaternion.identity, transform);
        }
    
        private void CheckSpin()
        {
            if (rigidBody.angularVelocity <= stopVelocity && _isSpinning && !_justClicked)
            {
                var angle = (int)(roulette.transform.localRotation.eulerAngles.z);
                if (angle % (360 / polygonSides) == 0)
                {
                    rigidBody.velocity = Vector2.zero;
                    rigidBody.angularVelocity = 0;
                    roulette.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));
                    
                    _isSpinning = false;
                    _isStopped = true;
                }
            }
            else if (rigidBody.angularVelocity > stopVelocity && _isSpinning && _justClicked)
            {
                _justClicked = false;
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
            if (landIndex > 0) triangles[landIndex-1].GetComponent<PolygonCollider2D>().RemoveHighlightAroundCollider();
            _isSpinning = _justClicked = true;
            _isStopped = false;
            landIndex = -1;
            rigidBody.AddTorque(Random.Range(torque, torque * 2.5f));
        }
    
        public void Highlight()
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

        private bool ValidatePolygonSides()
        {
            if (excludeNumbers.Any(number => polygonSides == number)) return false;
            return (360 % polygonSides == 0);
        }

        #endregion
        
}
