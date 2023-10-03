using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraFocus : MonoBehaviour
{
    [SerializeField] BoxCollider2D bounds;
    [SerializeField] Transform target;
    [SerializeField] Vector2 offset;
    [SerializeField] bool isSideScroller;
    [SerializeField] bool isVerticalScroller;
    [SerializeField] float distance;
    
    [SerializeField] bool hasParralax;
    [SerializeField] float followSpeed;
    [SerializeField] float lookaheadTime;
    
    float playerLookAhead;
    const float ORTHO_CAMERA_DISTANCE = 10;
    float lookaheadVelocity;
    CloneManager cloneManager;

    // Start is called before the first frame update
    void Start()
    {
        cloneManager = FindFirstObjectByType<CloneManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Camera _camera = GetComponent<Camera>();

        _camera.orthographic = !hasParralax;

        _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, distance, Time.deltaTime * followSpeed);
        

        if(target.gameObject == cloneManager.currentlyControlledPlayer.gameObject)
        {
            playerLookAhead = Mathf.SmoothDamp(playerLookAhead, cloneManager.currentlyControlledPlayer.direction ? 3 : -3, ref lookaheadVelocity, lookaheadTime);
        }
        else
        {
            playerLookAhead = Mathf.SmoothDamp(playerLookAhead, 0, ref lookaheadVelocity, followSpeed);
        }


        transform.position = Vector3.Lerp(transform.position, target.position + (Vector3)offset + Vector3.right * playerLookAhead, Time.deltaTime * followSpeed);

        transform.position = new Vector3(isVerticalScroller ? 0 : transform.position.x, isSideScroller ? 0 : transform.position.y, hasParralax ? distance * -1.75f : -ORTHO_CAMERA_DISTANCE);

        if(!bounds.bounds.Contains(transform.position))
        {
            Vector2 _closest = bounds.ClosestPoint(transform.position);
            transform.position = new Vector3(_closest.x, _closest.y, transform.position.z);
        }
        
        

        
        
    }
    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position*Vector2.one, 0.25f);
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}
