using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Joystick joystick;
    
    [Header("Movement Variables")] 
    [SerializeField] private float moveSpeed;

    private float _moveSpeed;
    private float rollSpeed;

    [SerializeField] private float crouchDivider;
    
    [Header("Ground Data")] 
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckDistance;

    [Header("Camera")] 
    [SerializeField] private float cameraOffset;
    
    private Rigidbody _rb;
    private Animator _anim;
    private bool _crouching;
    private bool _isGrounded;

    private float mH, mV;
    
    private void Start()
    {
        _rb = this.GetComponent<Rigidbody>();
        _anim = this.GetComponent<Animator>();
        rollSpeed = moveSpeed * 1.5F;
        _moveSpeed = moveSpeed;
        if (_anim != null)
        {
            _anim.SetFloat("InputX", 1);
        }
        
    }

    private void Update()
    {
        // LookTowardsMouse();

        
        Walk();
       // _cam.transform.position = new Vector3(this.gameObject.transform.position.x - 5, _cam.transform.position.y, this.gameObject.transform.position.z + cameraOffset);
    }

    void FixedUpdate ()
    {
        _rb.velocity = new Vector3 (-mH * moveSpeed, _rb.velocity.y, -mV * moveSpeed);
    }
    
    private void Walk()
    {
   //     mH = Input.GetAxis ("Horizontal");
   //     mV = Input.GetAxis ("Vertical");
        //
        
        mH = joystick.Horizontal;
        mV = joystick.Vertical;
        //
        if (mV != 0 || mH != 0)
        {
            _anim.SetBool("Walking", true);
            _anim.SetFloat("InputX", mH);
            _anim.SetFloat("InputY", mV);
        }
        else
            _anim.SetBool("Walking", false);
        
        if (mV != 0 &&  mH != 0) // Check for diagonal movement
        {
            // limit movement speed diagonally  
            mV *= .7F;
            mH *= .7F;
        }

    }

    public void StartRoll()
    {
        if (mV != 0 || mH != 0)
        {
            StartCoroutine(Roll(mH, mV));
        }
    }
    
    private IEnumerator Roll(float mH, float mV)
    {
        moveSpeed = rollSpeed;
        
        _anim.SetTrigger("Roll");
        yield return new WaitForSeconds(1);
        moveSpeed = _moveSpeed * .5F;
        yield return new WaitForSeconds(.3F);
        moveSpeed = _moveSpeed;
    }
    
    private void Jump()
    {
        if (!_isGrounded) return;
    }
    
    

}
