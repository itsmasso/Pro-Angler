using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.U2D;

public class WaterSpringMovement : MonoBehaviour
{
    public float velocity = 0;
    public float force = 0;
    public float height = 0;
    private float targetHeight = 0;
    [SerializeField]
    private SpriteShapeController spriteShapeController = null;
    private int waveIndex = 0;

    private float resistance = 40f;
    [SerializeField] private LayerMask targetLayer;
    private float timer;

    private bool onContactCooldown = false;
    public void Init(SpriteShapeController ssc)
    {
        var index = transform.GetSiblingIndex();
        waveIndex = index + 1;
        spriteShapeController = ssc;
        velocity = 0;
        height = transform.localPosition.y;
        targetHeight = transform.localPosition.y;

    }

    public void WavePointUpdate()
    {
        if (spriteShapeController != null)
        {
            Spline waterSpline = spriteShapeController.spline;
            Vector3 wavePosition = waterSpline.GetPosition(waveIndex);
            waterSpline.SetPosition(waveIndex, new Vector3(wavePosition.x, transform.localPosition.y, wavePosition.z));
        }
    }

    public void WaveSpringUpdate(float springStiffness, float dampening)
    {
        onObjectContact();
        height = transform.localPosition.y;
        var x = height - targetHeight;
        var loss = -dampening * velocity;

        force = - springStiffness * x + loss;
        velocity += force;
        var y = transform.localPosition.y;
        transform.localPosition = new Vector3(transform.localPosition.x, y + velocity, transform.localPosition.z);
    }

    

    private void onObjectContact()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 2f, transform.position.normalized, 0f, targetLayer);
        if(hit.collider != null)
        {
            Rigidbody2D rb = hit.collider.gameObject.GetComponent<Rigidbody2D>();
            Debug.Log("hit");
            if(rb != null)
            {
                Debug.Log("rb yes");
                timer += Time.deltaTime;
                if(timer >= 1f)
                {
                    onContactCooldown = false;
                }
                if (!onContactCooldown)
                {
                    var speed = rb.velocity;

                    velocity += speed.y / resistance;
                    onContactCooldown = true;
                }
                
            }
        }
    }
}
