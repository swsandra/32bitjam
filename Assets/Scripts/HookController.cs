using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookController : MonoBehaviour
{
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
    // [SerializeField] float moveUpSeconds = 1f;
    [SerializeField] float blinkRate = .1f;
    Camera cam;
    Vector3 screenBounds;
    float leftLimit, rightLimit, topLimit, bottomLimit, hookAboveCamLimit;
    float hookWidth, hookHeight;
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
        cam = Camera.main;
        camDirection = -1;
        // Calculate screen limits
        screenBounds = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, cam.transform.position.z));
        float camXOffset = cam.transform.position.x;
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        hookWidth = renderer.bounds.size.x;
        hookHeight = renderer.bounds.size.y;
        leftLimit = screenBounds.x+hookWidth;
        rightLimit = (screenBounds.x*-1)+(camXOffset*2)-hookWidth;
        topLimit = seaTop.position.y-hookHeight;
        bottomLimit = treasure.transform.position.y+(hookHeight*2);
        initialVerticalSpeed = verticalSpeed;
        // Start camera on sea top
        cam.transform.position = new Vector3(cam.transform.position.x, topLimit, cam.transform.position.z);
        // Start hook above camara
        hookAboveCamLimit = topLimit+(hookHeight*2);
        transform.position = new Vector3(transform.position.x, hookAboveCamLimit, transform.position.z);
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
            screenBounds = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, cam.transform.position.z));
            float camYOffset = cam.transform.position.y;
            if (treasure.transform.position.y >= hookAboveCamLimit || transform.position.y > (screenBounds.y*-1)+(camYOffset*2)) {
                // endAnimation = false;
                Debug.Log("salio de la pantalla");
                // TODO: salir
            }
            return;
        }

        bottomLimit = treasure.transform.position.y+(hookHeight*2); // Treasure can change position

        hookTranslation = Input.GetAxisRaw("Horizontal");
        // camTranslation = Input.GetAxisRaw("Vertical");

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

        if (cam.transform.position.y == topLimit && hasTreasure){
            endAnimation = true;
            // Debug.Log("Gano");
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (endAnimation) return;
        if (other.CompareTag("Obstacle") && !invulnerable){
            if (hasTreasure){
                DropTreasure();
            }else{
                StartCoroutine(MoveUpCoroutine());
                StartCoroutine(MakeInvulnerable());
                StartCoroutine(Blink());
            }
        }else if (other.CompareTag("Treasure") && !hasTreasure){ // Grab treasure
            // StartCoroutine(MakeInvulnerable());
            hasTreasure = true;
            camDirection = 1;
            other.transform.SetParent(transform);
            other.transform.localPosition = new Vector3(0, other.transform.localPosition.y, other.transform.localPosition.z);
            Treasure treasureComponent = other.GetComponent<Treasure>();
            treasureComponent.isGrounded = false;
            treasureComponent.isHooked = true;
        }else if (other.CompareTag("Junk")){ // Treasure was junk
            Debug.Log("toco el tesoro basura");
            endAnimation = true;
        }
    }

    IEnumerator MakeInvulnerable() {
        invulnerable = true;
        // Debug.Log("invulnerable");
        yield return new WaitForSeconds(invulnerableSeconds);
        invulnerable = false;
        // Debug.Log("ya no es invulnerable");
    }

    public void DropTreasure(){
        if (invulnerable || endAnimation) return; // This function can be called from treasure OnTriggerEnter

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
        // Debug.Log("boto el tesoro");
        yield return new WaitForSeconds(dropTreasureSeconds);
        treasureDropped = false;
        // Debug.Log("ya lo puede recoger");
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
        verticalSpeed *= 2;
        camDirection = 1;
        // yield return new WaitForSeconds(moveUpSeconds);
        yield return new WaitForSeconds(invulnerableSeconds);
        verticalSpeed = initialVerticalSpeed;
        camDirection = -1;
    }
}
