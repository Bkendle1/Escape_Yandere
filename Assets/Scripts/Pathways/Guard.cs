using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Guard : MonoBehaviour
{
    [Header("Patrol")]
    [SerializeField] private float m_patrolSpeed = 4f;
    [SerializeField] private WaypointPaths m_paths;
    [SerializeField] private float m_waypointDistanceCheck = 0.2f;
    //angle representing agent's field of view 
    [SerializeField] private float m_fieldOfView = 55f;
    //check how close the player needs to be before agent chases
    [SerializeField] private float m_checkDistanceFromPlayer = 10f;
    
    [Header("Chase")]
    [SerializeField] private float m_maxChaseTime = 5f;
    [SerializeField] private float m_chaseSpeed = 7f;
    [SerializeField] private float m_chaseDistance = 2f;
    
    
    [Header("Sounds")]
    [SerializeField] private AudioClip _spottedSFX;
    [SerializeField] private AudioClip[] _voiceLines;
    private AudioSource _audioSource;
    [SerializeField] private float _voiceInterval = 10f;
    private float _ogVoiceInterval;
    
    private NavMeshAgent m_agent;
    private Animator anim;
    private float m_timeSinceLastSeen = 0f;
    private int m_currentWaypoint = -1;
    private int m_lastWaypoint = 0;
    private Transform m_player;
    private NPCState m_state = NPCState.Patrol;
    private Vector3 m_startPos;
    
    void Awake()
    {
        m_agent = GetComponent<NavMeshAgent>();
        m_currentWaypoint = m_lastWaypoint;
        m_agent.SetDestination(m_paths.GetWaypoint(m_currentWaypoint).position);
        m_agent.speed = m_patrolSpeed;
        
        _audioSource = GetComponent<AudioSource>();
        _ogVoiceInterval = _voiceInterval;
        
        //get player's position
        m_player = GameObject.FindWithTag("Player").transform;
        anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        StartCoroutine(VoiceLines());
        m_startPos = transform.position;
    }

    void Update()
    {
        switch (m_state)
        {
            case NPCState.Patrol:
                anim.SetBool("spotted", false);
                Roam();
                break;
            case NPCState.Chase:
                anim.SetBool("spotted", true);
                Chase();
                break;
            case NPCState.Attack:
                //reset positions so player doesn't get spawn camped
                //this doesn't seem to work all the time
                transform.position = m_startPos;
                transform.rotation = Quaternion.identity;
                PutPlayerToJail();
                break;
        }
    }

    //aka patrolling method
    private void Roam()
    {
        /*if the distance between the agent and its
        destination is less than the given distance
        (m_waypointDistanceCheck), set destination to be next waypoint (NextWaypoint()) 
        */
        if (m_agent.remainingDistance < m_waypointDistanceCheck)
        {
            NextWaypoint();
        }

        //chase player if agent can see them
        if (CanISeePlayer())
        {
            _audioSource.PlayOneShot(_spottedSFX);
            m_state = NPCState.Chase;
        }
    }

    IEnumerator VoiceLines()
    {
        int i = 0;
        //play a random voice line
        while(_voiceInterval > 0)
        {
            _voiceInterval--;
            if (_voiceInterval <= 0)
            {
                //_audioSource.PlayOneShot(_voiceLines[0]);
                //_audioSource.PlayOneShot(_voiceLines[UnityEngine.Random.Range(0, _voiceLines.Length)]);
                if (i >= _voiceLines.Length)
                {
                    i = 0;
                }
                _audioSource.PlayOneShot(_voiceLines[i++]);
                _voiceInterval = _ogVoiceInterval;
            }
            yield return new WaitForSeconds(1f);
        }
    }

    private void Chase()
    {
        //increase movement speed
        m_agent.speed = m_chaseSpeed;
        //move towards player
        m_agent.SetDestination(m_player.position);

        
        //if distance between player and agent is further than given chase distance, go back to patrolling
        if(Vector3.Distance(transform.position, m_player.position) > m_chaseDistance)
        {
            //give up chase if agent hasn't seen player for some time
            //go back to patrol state
            //reset timer since guard has last seen player
            m_timeSinceLastSeen += Time.deltaTime;
            if (m_timeSinceLastSeen > m_maxChaseTime)
            {
                m_state = NPCState.Patrol;
                //reset speed and timer
                m_agent.speed = m_patrolSpeed;
                m_timeSinceLastSeen = 0;
            }
        }
        //if agent gets close enough to player, attack
        else if (Vector3.Distance(transform.position, m_player.position) < m_chaseDistance)
        {
            m_state = NPCState.Attack;
        }
        //prepare timer for the next time it sees player
        else
        {
            m_timeSinceLastSeen = 0f;
        }
    }

    private void PutPlayerToJail()
    {
        //GameObject.FindWithTag("Player").GetComponent<FPSControls>().GoToJail();
        if (m_player != null)
        {
            Debug.Log("Caught");
            m_player.GetComponent<FPSControls>().GoToJail();
            GameManager.Instance.UpdateLives(-1);
        }
        
        //reset positions so player doesn't get spawn camped
        //this doesn't seem to work all the time, 
        transform.position = m_startPos;
        transform.rotation = Quaternion.identity;
        
        //go back to patrolling and get next waypoint
        m_agent.speed = m_patrolSpeed;
        m_state = NPCState.Patrol;
        NextWaypoint();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("You collided");
        if (other.CompareTag("Player"))
        {
            PutPlayerToJail();
        }
    }

    private void NextWaypoint() 
    {
        /*set next waypoint to be current but also wrap back around 
        to beginning of the array of waypoints if we exceed the
        maximum number of given waypoints
        */
        m_currentWaypoint = (m_currentWaypoint + 1) % m_paths.GetWaypointCount();
        //set agent's destination to be new current waypoint
        m_agent.SetDestination(m_paths.GetWaypoint(m_currentWaypoint).position);
        Debug.Log("Destination:" + m_currentWaypoint);
    }

    private bool CanISeePlayer()
    {
        
        //create vector to represent where the player is relative to agent's position
        Vector3 DirectionToPlayer = m_player.position - transform.position;
        
        //create angle that's between where the agent is facing (aka their forward vector (transform.forward)
        //and the where the player is relative to the agent (DirectionToPlayer)
        float angle = Vector3.Angle(transform.forward, DirectionToPlayer);
        Debug.DrawRay(transform.position, DirectionToPlayer, Color.red);

        float distanceFromPlayer = Vector3.Distance(transform.position, m_player.position);
        
        //if angle between player's position relative to the agent's
        //and the agent's forward vector is less than the given
        //field of view then the agent should be able to see the player
        if (angle <= m_fieldOfView && (distanceFromPlayer <= m_checkDistanceFromPlayer))
        {
            //once the agent sees the player, shoot a raycast in the direction of the player
            RaycastHit hit;
            if (Physics.Raycast(transform.position, DirectionToPlayer, out hit))
            {
                //if the raycast (hit) lands on the player, then the agent
                //sees the player and moves to Chase state
                if (hit.transform == m_player)
                {
                    Debug.Log("Spotted");
                    return true;
                }
            }
        }
        return false;
    }
    
}
//states that the guard can change to
public enum NPCState {Patrol, Chase, Attack}