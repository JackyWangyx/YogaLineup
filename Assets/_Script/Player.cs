using Aya.Extension;
using Aya.Physical;
using Sirenix.OdinInspector;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator Animator { get; set; }
    public Rigidbody Rigidbody { get; set; }
    public Renderer Renderer { get; set; }

    public void Awake()
    {
        CacheComponent();
    }

    public void CacheComponent()
    {
        Animator = GetComponentInChildren<Animator>();
        Rigidbody = GetComponent<Rigidbody>();
        Renderer = GetComponentInChildren<Renderer>();
    }

    public void Play(string animationName)
    {
        Animator?.Play(animationName);
    }

    public void Init()
    {
        Play("Idle");
    }

    public void Start()
    {
        
    }

    public void Update()
    {
        
    }


    public void FixedUpdate()
    {


    }

    public void LateUpdate()
    {

    }

    public void Win()
    {
        Play("Win");
    }

    public void Die()
    {
        Play("Lose");
    }
}
