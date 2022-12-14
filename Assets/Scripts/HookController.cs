using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookController : MonoBehaviour
{
    [SerializeField] Camera cam;
    [Header("Movement")]
    [SerializeField] float horizontalSpeed = 10f;
    [SerializeField] float verticalSpeed = 10f;

    [Header("Vertical Bounds")]
    [SerializeField] Transform seaTop;
    [SerializeField] Transform seaBottom;
    [SerializeField] GameObject treasure;

    [Header("Time intervals")]
    [SerializeField] float invulnerableSeconds = 1.5f;
    [SerializeField] float dropTreasureSeconds = 1f;
    [SerializeField] float blinkRate = .1f;
    [Header("Audio")]
    [SerializeField] AudioSource chainSound;
    [SerializeField] AudioClip hookSound;
    [SerializeField] AudioSource song;

    [Header("Objectives")]
    [SerializeField] GameObject trueTreasure;
    [SerializeField] GameObject junkTreasure;
    Vector3 camBottomLeft, camTopRight;
    float leftLimit, rightLimit, topLimit, bottomLimit, hookAboveCamLimit;
    float hookWidth, hookHeight, treasureHeight;
    float hookTranslation;
    float camDirection;
    float initialVerticalSpeed;
    bool startAnimation, endAnimation;
    bool invulnerable;
    bool hasTreasure;
    bool treasureDropped;

    private void Start() {
        startAnimation = true;
        hasTreasure = false;
        camDirection = -1;
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        song.time = GameManager.instance.musicTimer;
        hookWidth = renderer.bounds.size.x;
        hookHeight = renderer.bounds.size.y;
        topLimit = seaTop.position.y-hookHeight;

        // Select correct objective (junk or treasure)
        treasure = GameManager.instance.treasureType.Equals("Junk") ? junkTreasure : trueTreasure;
        treasure.SetActive(true);
        treasure.tag = GameManager.instance.treasureType;

        // Start camera on sea top
        cam.transform.position = new Vector3(cam.transform.position.x, topLimit, cam.transform.position.z);
        // Start hook above camara
        hookAboveCamLimit = topLimit+(hookHeight*2);
        transform.position = new Vector3(transform.position.x, hookAboveCamLimit, transform.position.z);

        // Calculate screen limits
        camTopRight = CalculateTopRight();
        camBottomLeft = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.transform.position.z));
        leftLimit = camBottomLeft.x+hookWidth;
        rightLimit = camTopRight.x-hookWidth;

        treasureHeight = treasure.GetComponent<Renderer>().bounds.size.y;
        initialVerticalSpeed = verticalSpeed;
    }

    void Update()
    {
        if (treasureDropped) return;

        Vector3 hookMovement;
        if (startAnimation){ // Start animation
            hookMovement = Vector3.down * (verticalSpeed/2) * Time.deltaTime;
            transform.position += hookMovement;
            if (transform.localPosition.y <= 0) startAnimation = false; // Referent to camera
            return;
        }

        if (endAnimation){ // Start animation
            hookMovement = Vector3.up * (verticalSpeed/2) * Time.deltaTime;
            transform.position += hookMovement;
            camTopRight = CalculateTopRight();
            if (treasure.transform.position.y >= hookAboveCamLimit || transform.position.y > camTopRight.y) {
                GameManager.instance.musicTimer = song.time;
                GameManager.instance.LoadLevelSceneFromHook();
            }
            return;
        }

        bottomLimit = CalculateBottomLimit(); // Treasure can change position

        hookTranslation = Input.GetAxisRaw("Horizontal");

        hookMovement = new Vector3(hookTranslation, 0, 0).normalized * horizontalSpeed * Time.deltaTime;
        Vector3 camMovement = new Vector3(0, camDirection, 0).normalized * verticalSpeed * Time.deltaTime;

        transform.position += hookMovement;
        cam.transform.position += camMovement;

        // Hook movement bounds
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, leftLimit, rightLimit),
            transform.position.y,
            transform.position.z);

        // Camera movement bounds
        cam.transform.position = new Vector3(
            cam.transform.position.x,
            Mathf.Clamp(cam.transform.position.y, bottomLimit, topLimit),
            cam.transform.position.z);

        if (cam.transform.position.y >= topLimit && hasTreasure){
            endAnimation = true;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (startAnimation || endAnimation) return;
        if (other.CompareTag("Obstacle") && !invulnerable){
            if (hasTreasure){
                DropTreasure();
            }else{
                StartCoroutine(MoveUpCoroutine());
                StartCoroutine(MakeInvulnerable());
                StartCoroutine(Blink());
            }
        }else if (other.CompareTag("Treasure") && !hasTreasure){ // Grab treasure
            AudioSource.PlayClipAtPoint(hookSound,transform.position);
            hasTreasure = true;
            camDirection = 1;
            other.transform.position = new Vector3(transform.position.x, transform.position.y-(treasureHeight/2), other.transform.position.z);
            other.transform.SetParent(transform);
            Treasure treasureComponent = other.GetComponent<Treasure>();
            treasureComponent.isGrounded = false;
            treasureComponent.isHooked = true;
        }else if (other.CompareTag("Junk")){ // Treasure was junk
            endAnimation = true;
        }
    }

    float CalculateBottomLimit(){
        return treasure.transform.position.y+treasureHeight;
    }

    Vector3 CalculateTopRight(){
        return cam.ViewportToWorldPoint(new Vector3(0, 0, cam.transform.position.z));
    }

    IEnumerator MakeInvulnerable() {
        invulnerable = true;
        yield return new WaitForSeconds(invulnerableSeconds);
        invulnerable = false;
    }

    public void DropTreasure(){
        if (invulnerable || endAnimation) return; // This function can be called from treasure OnTriggerEnter
        AudioSource.PlayClipAtPoint(hookSound,transform.position);
        treasure.transform.SetParent(null);
        hasTreasure = false;
        treasure.GetComponent<Treasure>().isHooked = false;
        StartCoroutine(DropTreasureCoroutine());
        camDirection = -1;
        StartCoroutine(MakeInvulnerable());
        StartCoroutine(Blink());
    }

    IEnumerator DropTreasureCoroutine() {
        treasureDropped = true;
        chainSound.Stop();
        yield return new WaitForSeconds(dropTreasureSeconds);
        chainSound.Play();
        treasureDropped = false;
    }

    IEnumerator Blink(){
        var endTime = Time.time + invulnerableSeconds;
        MeshRenderer[] childRenderers = GetComponentsInChildren<MeshRenderer>();
        while(Time.time<endTime){
            disableAllRenderers(childRenderers);
            yield return new WaitForSeconds(blinkRate);
            enableAllRenderers(childRenderers);
            yield return new WaitForSeconds(blinkRate);
        }
    }

    void disableAllRenderers(MeshRenderer[] renderers){
        foreach (MeshRenderer meshRenderer in renderers)
            meshRenderer.enabled = false;
    }

    void enableAllRenderers(MeshRenderer[] renderers){
        foreach (MeshRenderer meshRenderer in renderers)
            meshRenderer.enabled = true;
    }

    IEnumerator MoveUpCoroutine(){
        chainSound.Stop();
        verticalSpeed *= 2;
        camDirection = 1;
        yield return new WaitForSeconds(invulnerableSeconds);
        verticalSpeed = initialVerticalSpeed;
        camDirection = -1;
        chainSound.Play();
    }
}
