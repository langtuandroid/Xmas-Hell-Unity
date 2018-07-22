using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class VelocityStretch : MonoBehaviour
{
    public Rigidbody2D RigidBody;
    public Vector2 TargetPosition;
    public float Speed;

    private Vector2 _previousPosition;
    private bool _reachedPosition;

    void Start()
    {
        _previousPosition = RigidBody.position;
        _reachedPosition = false;
    }
	
	void Update()
    {
        var newPosition = RigidBody.position;

        if (!_reachedPosition)
        {
            float step = Speed * Time.deltaTime;
            newPosition = Vector2.Lerp(newPosition, TargetPosition, step);
            //RigidBody.position = newPosition;
        }

        //if (!_reachedPosition && (newPosition - TargetPosition).magnitude < 0.1f)
        //{
        //    _reachedPosition = true;
        //    RigidBody.position = TargetPosition;
        //    transform.localScale = Vector2.one;
        //}

        var direction = newPosition - _previousPosition;
        var maxSqueeze = 2f;

        var newScale = Vector2.one;

        if (direction.magnitude > 0.1f)
        {
            //get raw ball deformation
            var ballDeformation = direction;

            //translate deformation to 0% - 100% range
            ballDeformation = ballDeformation.normalized;
            ballDeformation *= maxSqueeze;

            //deformation is the same for directions (1f,0f) and (-1f,0f), since we are adding to sprite base scale you just need positive values
            ballDeformation.x = Mathf.Abs(ballDeformation.x);
            ballDeformation.y = Mathf.Abs(ballDeformation.y);

            newScale += ballDeformation;
        }

        transform.localScale = Vector2.Lerp(transform.localScale, newScale, Time.deltaTime * 100f);

        //var velocity = new Vector2(Mathf.Abs(direction.normalized.x), Mathf.Abs(direction.normalized.y));
        //var magnitude = direction.magnitude;

        //if (magnitude > 0)
        //{
        //    Debug.Log("Magnitude: " + magnitude);
        //}

        //var newScale = transform.localScale;
        //var factor = 1f;
        //newScale.x = Mathf.Clamp(1f - magnitude * factor, 0.1f, 1f);
        //newScale.y = Mathf.Clamp(1f + magnitude * factor, 1f, 1.9f);

        //transform.localScale = Vector2.Lerp(transform.localScale, newScale, Time.deltaTime * 100f);


        _previousPosition = RigidBody.position;
    }
}
