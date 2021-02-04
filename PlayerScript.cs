using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
	//gives my Player the ability to interact with other game elements using physics
	private Rigidbody2D myRigidbody;
	//this is set to public so I can interact with it in the inspector.  It will give the Player variable speed.
	private Animator myAnimator;
	//
	public float movementSpeed;
	//can be set to true or false to change the Players facing direction
	private bool facingRight;
	[SerializeField]
	private Transform[] groundpoints; // serializefeild allows the variable to show up in the unity spectre
	[SerializeField]
	private float groundradius; //give some size to the ground points
	[SerializeField]
	private LayerMask whatisground; //allows us to set other objects as ground points
	private bool isGrounded; // this whil allow us t only activate jump when this is true
	private bool jump; //this is how we get our character to jump
	[SerializeField]
	private float jumpforce; // control magnitude of the jump
	public bool imAlive;
	public GameObject reset;
	//health slider vars
	private Slider healthBar;
	public float health = 10f;
	private float healthBurn = 3f;



	// Use this for initialization
	void Start()
	{   //initial value to set the Player facing right
		facingRight = true;
		//associates the Rigidbody component of the Player with a variable we can use later on
		myRigidbody = GetComponent<Rigidbody2D>();
		myAnimator = GetComponent<Animator>();
		imAlive = true;
		//health slider variables
		healthBar = GameObject.Find("health slider").GetComponent<Slider>();
		healthBar.minValue = 0f;
		healthBar.maxValue = health;
		healthBar.value = healthBar.maxValue;
		reset.SetActive(false);


	}
	void Update()
	{
		HandleInput();
	}
	/* Update is called once per frame.  
Fixed Update locks in speed and performance regardless of console performance and quality*/
	void FixedUpdate()
	{
		//access the keyboard controls and move left and right
		float horizontal = Input.GetAxis("Horizontal");
		//just to see what is being reported by the keyboard on the console
		//Debug.Log (horizontal);
		isGrounded = IsGrounded();
		//calling the function in the game 
		//controls Player on the x and y axis

		if (imAlive)
		{
			HandleMovement(horizontal);
			//controls player facing direction
			Flip(horizontal);
		}
		
	}




	private void HandleInput()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			jump = true; //
			Debug.Log("I'm Jumping!!!"); //test the code in the console
			myAnimator.SetBool("Jumping", true); //set the jumping bool in the animator 
		}
	}




	private void HandleMovement(float horizontal)
	{
		if (jump && isGrounded)
		{
			myRigidbody.AddForce(new Vector2(0, jumpforce));
			jump = false;
			isGrounded = false;
		}
		//moves the Player on x axis and y axis
		myRigidbody.velocity = new Vector2(horizontal * movementSpeed, myRigidbody.velocity.y);
		myAnimator.SetFloat("Speed", Mathf.Abs(horizontal));
	}



	private void Flip(float horizontal)
	{
		//logical test to make sure that we are changing his facing direction
		if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
		{
			facingRight = !facingRight; //this sets the value of facingRight to its opposite
			Vector3 theScale = transform.localScale; //this accesses the local player scale component
			theScale.x *= -1;  //multiplying the x value of scale by -1 allows us to get either 1 or -1 as the result
			transform.localScale = theScale; //this reports the new value to the player's scale component
		}
	}


	/* be sure to create 3 child gameobjects to the player object. name them groundpoint_lt, groundpoint_rt, and groundpoint_center
	align them so that they are at the feet of the character.*/




	private bool IsGrounded()
	{
		if (myRigidbody.velocity.y <= 0)
		{
			//if player is not moving vertically test each of Player’s groundpoints for contact with the Ground
			foreach (Transform point in groundpoints)
			{
				Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundradius, whatisground);
				for (int i = 0; i < colliders.Length; i++)
				{
					if (colliders[i].gameObject != gameObject)
					//if any of the groundpoints is in contact(collides) with anything other than the Player, return true
					{
						return true;
					}
				}
			}
		}
		return false;
	}




	//this function will be called when player collides with an enemy
	public void UpdateHealth()
	{
		if (health > 1)
		{
			//if the health bar has life left..
			health -= healthBurn; //current value of health minus 2f
			healthBar.value = health;  //update the interface slider
		}
		else
		{
			ImDead(); //if no life left, run this function which kills
		}
	}





	public void ImDead(){ 
		myAnimator.SetBool("dead",true);
		imAlive = false;
		reset.gameObject.SetActive(true);

	}






    private void OnCollisionEnter2D(Collision2D target)
    {
		if(target.gameObject.tag == "Ground")
        {
			myAnimator.SetBool("Jumping", false);
        }
		if (target.gameObject.tag == "deadly"){
			ImDead();
        }

		if (target.gameObject.tag == "damage"){
			UpdateHealth();
		}

	}

   
}