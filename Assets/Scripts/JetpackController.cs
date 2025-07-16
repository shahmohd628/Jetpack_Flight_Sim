using StarterAssets;
using UnityEngine;

public class JetpackController : MonoBehaviour
{
    [Header("Movement & Jetpack")]
    public CharacterController controller;
    public Transform groundCheck;
    public LayerMask groundMask;
    public float groundDistance = 0.4f;

    [Header("Jetpack Settings")]
    public float jetpackForce = 12f;
    public float fuel = 100f;
    public float maxFuel = 100f;
    public float fuelBurnRate = 15f;
    public float fuelRegenRate = 10f;

    [Header("Gravity Settings")]
    public float noramalGravity = -9.81f;
    public float fallGravity = -25f;

    [Header("Particles & Sound")]
    public ParticleSystem[] jetpackParticles; // Left and Right flames
    public AudioSource jetpackSound;

    [Header("Animator Control")]
    public Animator playerAnimator;

    private Vector3 velocity;
    private bool isGrounded;
    private bool wasGrounded;
    private bool isFlying;

    void Update()
    {
        // Ground Check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Toggle Animator when transitioning between grounded and airborne
        if (isGrounded && !wasGrounded)
        {
            if (playerAnimator != null)
                playerAnimator.enabled = true;
        }
        else if (!isGrounded && wasGrounded)
        {
            if (playerAnimator != null)
                playerAnimator.enabled = false;
        }

        wasGrounded = isGrounded;

        // Reset downward velocity when grounded
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Jetpack Activation
        if (Input.GetButton("Jump") && fuel > 0)
        {
            velocity.y = jetpackForce;
            fuel -= fuelBurnRate * Time.deltaTime;

            if (!isFlying)
            {
                isFlying = true;
                PlayJetpackFX(true);
            }
        }
        else
        {
            if (isFlying)
            {
                isFlying = false;
                PlayJetpackFX(false);
            }

            if (isGrounded && fuel < maxFuel)
            {
                fuel += fuelRegenRate * Time.deltaTime;
            }
        }

        // Apply appropriate gravity
        if (isFlying)
        {
            velocity.y += noramalGravity * Time.deltaTime;
        }
        else
        {
            velocity.y += fallGravity * Time.deltaTime;
        }

        // Apply movement
        controller.Move(velocity * Time.deltaTime);
    }

    void PlayJetpackFX(bool play)
    {
        foreach (var ps in jetpackParticles)
        {
            if (ps == null)
                continue;

            if (play && !ps.isPlaying)
                ps.Play();
            else if (!play && ps.isPlaying)
                ps.Stop();
        }

        if (jetpackSound != null)
        {
            if (play && !jetpackSound.isPlaying)
                jetpackSound.Play();
            else if (!play && jetpackSound.isPlaying)
                jetpackSound.Stop();
        }
    }

    // Public access for UI or other scripts
    public float CurrentFuel => fuel;
    public float MaxFuel => maxFuel;
    public float FuelNormalized => fuel / maxFuel;
}
