using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuyController : MonoBehaviour {



	[Header("Movement Settings")]
	public float speed;
	public float jumpStrength;
	public float fallMult;
	public float upMult;
	public float dashSpeed;
	public float dashLength;
	public float dashWaitTime;
    public int maxJumpNum;


    SpriteRenderer sP;
    Rigidbody2D rB;

    int jumpNum;
    bool facingLeft, isDashing, isDashable;
	float dashStartTime;


    Vector3 newDashPos, transPos, transScale;

	// Use this for initialization
	void Start () {
        sP = GetComponent<SpriteRenderer>();
        rB = GetComponent<Rigidbody2D>();
        newDashPos = transform.position;
		transScale = transform.localScale;


		isDashable = true;
        
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        float h = Input.GetAxis("Horizontal");
        transPos = transform.position;
        Dashing();
        

        transform.position += new Vector3(h * speed * Time.deltaTime, 0, 0);
        //transPos = Vector3.Lerp(transPos, newDashPos, dashSpeed);
        

        if (h < 0)
        {
			//sP.flipX = true;
			transScale.x = -1;
			transform.localScale = transScale;
            facingLeft = true;
        }
        else if (h > 0) {
			transScale.x = 1;
			transform.localScale = transScale;
            //sP.flipX = false;
            facingLeft = false;
        }

        if (rB.velocity.y < 0) {
            rB.velocity += Physics2D.gravity.y * Time.deltaTime * (fallMult - 1) * Vector2.up;
        } else if (rB.velocity.y > 0) {
            rB.velocity += Physics2D.gravity.y * Time.deltaTime * (upMult - 1) * Vector2.up;
        }


		if (Input.GetButtonDown("Jump")) {
            if (jumpNum < maxJumpNum)
            {
                rB.AddForce(Vector2.up * jumpStrength);
                jumpNum++;
            }

        }


		if (Input.GetKeyDown(KeyCode.X))
		{
			if (isDashable)
			{
				dashStartTime = Time.time;
				if (facingLeft == true)
				{
					newDashPos = new Vector3(transPos.x - dashLength, transPos.y, 0);

				}
				else if (facingLeft == false)
				{
					newDashPos = new Vector3(transPos.x + dashLength, transPos.y, 0);

				}
				//transform.position = transPos;
				isDashing = true;
				isDashable = false;
			}
		}
	}



    private void OnCollisionEnter2D(Collision2D collision)
    {
        jumpNum = 0;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {

    }


    void Dashing() {
        if (isDashing)
        {
            print("Dashing");
            transPos = Vector3.Lerp(transPos, newDashPos, dashSpeed * Time.deltaTime);
            transform.position = transPos;
        }
        if (Mathf.Abs(transform.position.x - newDashPos.x) < .5f) {
            isDashing = false;
        }

		if(Time.time > dashStartTime + dashWaitTime){
			isDashable = true;
		}


    }
}
