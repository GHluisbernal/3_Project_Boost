using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] private float mainThrust = 100f;
    [SerializeField] private float rcsThrust = 100f;
    [SerializeField] private float levelLoadDelay = 1f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip dead;

    [SerializeField] ParticleSystem mainEngineParticle;
    [SerializeField] ParticleSystem successParticle;
    [SerializeField] ParticleSystem deadParticle;
    private Rigidbody Rigidbody { get; set; }
    private AudioSource AudioSource { get; set; }

    enum State { Flying, Dying, Ending }
    private State state = State.Flying;
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Flying)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Flying)
        {
            return;
        }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                state = State.Flying;
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        AudioSource.Stop();
        AudioSource.PlayOneShot(dead);
        deadParticle.Play();
        Invoke(nameof(LoadFirstLevel), levelLoadDelay);
    }

    private void StartSuccessSequence()
    {
        state = State.Ending;
        AudioSource.Stop();
        AudioSource.PlayOneShot(success);
        successParticle.Play();
        Invoke(nameof(LoadNextLevel), levelLoadDelay);
    }

    private void LoadFirstLevel()
    {

        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1);
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Rigidbody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
            if (!AudioSource.isPlaying)
            {
                AudioSource.PlayOneShot(mainEngine);
            }
            if (!mainEngineParticle.isPlaying)
            {
                mainEngineParticle.Play();
            }
        }
        else
        {
            mainEngineParticle.Stop();
            AudioSource.Stop();
        }
    }

    private void RespondToRotateInput()
    {
        Rigidbody.freezeRotation = true;

        var rotationThisFrame = rcsThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {

            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
        Rigidbody.freezeRotation = false;
    }
}
