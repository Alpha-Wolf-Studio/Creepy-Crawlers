using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UiButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Action onButtonEnter;
    public Action onButtonExit;

    [Header("RayCast Collision:")]
    [Tooltip("Chequea Alphas en el raycast. Modificar el Read/Write Enabled en la imagen si éste es true.")]
    [SerializeField] private bool modifyHitBox;

    [SerializeField] private float alphaRayCast = 0.1f;

    [Header("Effect Scale:")]
    [SerializeField] private float scaleSpeed = 3;

    [SerializeField] private float scaleLimit = 1.2f;
    private bool increment = false;
    private Vector3 initialScale;
    private Vector3 scale;

    [Header("Effect Image:")]
    [SerializeField] private bool modifyImage;

    [SerializeField] private Sprite imageDefault;
    [SerializeField] private Sprite imageHighlighted;
    private Image currentImage;

    [Header("Effect Color Text:")]
    [SerializeField] private bool textHighlight;

    [SerializeField] private TextMeshProUGUI textToHighlight;
    [SerializeField] private Color colorHighlight;
    private Color colorNormal;

    [Header("Other:")]
    [SerializeField] private bool enableObject;
    [SerializeField] private GameObject objectToEnable;
    [SerializeField] private Transform objectToRotate;
    [SerializeField] private float rotateSpeed;

    private void Awake ()
    {
        increment = false;
        initialScale = transform.localScale;

        if (modifyHitBox)
            GetComponent<Image>().alphaHitTestMinimumThreshold = alphaRayCast;

        if (modifyImage)
            currentImage = GetComponent<Image>();

        if (enableObject)
        {
            if (!objectToEnable)
            {
                Debug.LogWarning("No tiene un objeto asignado.", gameObject);
                enableObject = false;
            }
        }

        if (textHighlight)
        {
            if (!textToHighlight)
            {
                textHighlight = false;
                Debug.Log("No tiene asignado un Text.", gameObject);
            }
            else
                colorNormal = textToHighlight.color;
        }
    }

    private void OnEnable ()
    {
        transform.localScale = initialScale;
        increment = false;
    }

    private void Update ()
    {
        ChangeScale();
        RotateObjectToRotate();
    }

    private void OnDisable ()
    {
        if (modifyImage)
            currentImage.sprite = imageDefault;

        if (textHighlight)
            textToHighlight.color = colorNormal;

        if (enableObject)
            objectToEnable.SetActive(false);
    }

    public void OnMouseEnterButton ()
    {
        onButtonEnter?.Invoke();
        increment = true;

        if (modifyImage)
            currentImage.sprite = imageHighlighted;

        if (enableObject)
            objectToEnable.SetActive(true);

        if (textHighlight)
            textToHighlight.color = colorHighlight;
    }

    public void OnMouseExitButton ()
    {
        onButtonExit?.Invoke();
        increment = false;

        if (modifyImage)
            currentImage.sprite = imageDefault;

        if (enableObject)
            objectToEnable.SetActive(false);

        if (textHighlight)
            textToHighlight.color = colorNormal;
    }

    private void ChangeScale ()
    {
        float timeStep = scaleSpeed * Time.unscaledDeltaTime;
        scale = transform.localScale;
        if (increment)
        {
            if (transform.localScale.x < scaleLimit)
            {
                scale = new Vector3(scale.x + timeStep, scale.y + timeStep, scale.z + timeStep);
                transform.localScale = scale;
            }
            else
            {
                transform.localScale = new Vector3(scaleLimit, scaleLimit, scaleLimit);
            }
        }
        else
        {
            if (transform.localScale.x > initialScale.x)
            {
                scale = new Vector3(scale.x - timeStep, scale.y - timeStep, scale.z - timeStep);
                transform.localScale = scale;
            }
            else
            {
                transform.localScale = new Vector3(initialScale.x, initialScale.y, initialScale.z);
            }
        }
    }

    public void RotateObjectToRotate ()
    {
        if (objectToRotate)
            objectToRotate.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);
    }

    public void OnPointerEnter (PointerEventData eventData)
    {
        OnMouseEnterButton();
    }

    public void OnPointerExit (PointerEventData eventData)
    {
        OnMouseExitButton();
    }

    public void OnPointerClick (PointerEventData eventData)
    {
        if (objectToRotate)
            objectToRotate.rotation = Quaternion.identity;
    }
}