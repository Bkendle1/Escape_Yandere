using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPSControls : MonoBehaviour
{
    //Generate input bindings
    private FPSInput m_FPSInput;

    [Header("Movement")] 
    [SerializeField] private float m_movementSpeed = 5f;
    [SerializeField] private float m_runSpeed = 10f;
    
    [SerializeField] private float m_lookSens = 5f;
    [SerializeField] private float m_maxPitch = 90f;
    [SerializeField] private Transform m_camera;

    private InputAction m_move;
    private InputAction m_lookY;
    private InputAction m_lookX;
    
    private InputAction fire;
    [HideInInspector] public bool hasFired;
    
    private float m_pitch;
    private CharacterController m_controller;
    private Vector3 m_spawnPoint;
    
    private bool isSprinting;
    private float og_movementSpeed;
    private Animator anim;
    
   
    private void Awake()
    {
        m_FPSInput = new FPSInput();
        m_controller = GetComponent<CharacterController>();
        m_spawnPoint = transform.position;
        og_movementSpeed = m_movementSpeed;
        anim = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        m_move = m_FPSInput.Player.Move;
        m_lookX = m_FPSInput.Player.MouseX;
        m_lookY = m_FPSInput.Player.MouseY;
        fire = m_FPSInput.Player.Fire;
        
        m_move.Enable();
        m_lookX.Enable();
        m_lookY.Enable();
        fire.Enable();
    }

    private void OnDisable()
    {
        m_move.Disable();
        m_lookX.Disable();
        m_lookY.Disable();
        fire.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Look();
        Run();
        OnFirePressed();
    }

    public void GoToJail()
    {
        Debug.Log("You're in jail");
        transform.position = m_spawnPoint;
        transform.rotation = Quaternion.identity;
    }

    private void Movement()
    {
        //Read the input values from player's keyboard/controller 
        Vector2 InputRead = m_move.ReadValue<Vector2>();

        Vector3 moveDirection = ((transform.right * InputRead.x) + (transform.forward * InputRead.y)).normalized;

        //Time.deltaTime makes movement speed fps independent
        //
        moveDirection *= m_movementSpeed * Time.deltaTime;

        m_controller.Move(moveDirection);
        

        //animations
        //player uses movement keys...
        if (InputRead != Vector2.zero)
        {
            if (Keyboard.current.shiftKey.isPressed)
            {
                anim.SetBool("isSprinting", true);
            }
            else
            {
                anim.SetBool("isSprinting", false);
            }
            
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isSprinting", false);
            anim.SetBool("isWalking", false);
        }
    }

    private void Run()
    {
        if (Keyboard.current.shiftKey.isPressed)
        {
            m_movementSpeed = m_runSpeed;
        }
        else
        {
            m_movementSpeed = og_movementSpeed;
        }
    }

    //check if the player fired/clicked
    private void OnFirePressed()
    {
        Debug.Log("Fire pressed");
        if (fire.triggered)
        {
            hasFired = true;
        }
        else
        {
            hasFired = false;
        }
        
    }
    
    private void Look()
    {
        //+= would be inverted controls
        //Read value of y-input from mouse/controller
        m_pitch -= m_lookY.ReadValue<float>() * m_lookSens * Time.deltaTime;
        
        //sets the boundaries so the camera can only rotate to a certain minimum and maximum degree/angle
        m_pitch = Mathf.Clamp(m_pitch, -m_maxPitch, m_maxPitch);
        
        m_camera.localRotation = Quaternion.Euler(m_pitch, 0, 0);
        
        //Rotate camera about the y-axis
        transform.Rotate(Vector3.up * m_lookX.ReadValue<float>() * m_lookSens * Time.deltaTime);
        
    }
}
