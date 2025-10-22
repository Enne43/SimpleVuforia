using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class ChangeModel : MonoBehaviour
{
    #region VARS
    [Header("UI")]
    [SerializeField] Button leftButton;
    [SerializeField] Button rightButton;

    [Header("Target")]
    [SerializeField] ObserverBehaviour imageTarget; 

    private GameObject currentModel;
    private int index = 0;
    private bool targetVisible = false;
    #endregion

    private void OnEnable()
    {
        leftButton.onClick.AddListener(OnLeftButtonClick);
        rightButton.onClick.AddListener(OnRightButtonClick);

        if (imageTarget != null)
            imageTarget.OnTargetStatusChanged += OnTargetStatusChanged;

        OnTargetLost();
    }

    private void OnDisable()
    {
        leftButton.onClick.RemoveListener(OnLeftButtonClick);
        rightButton.onClick.RemoveListener(OnRightButtonClick);

        if (imageTarget != null)
            imageTarget.OnTargetStatusChanged -= OnTargetStatusChanged;
    }

    /// <summary>
    /// Método para cuando el estado del tracker ha cambiado.
    /// Es como usar lo que tiene en el inspector.
    /// </summary>
    /// <param name="behaviour"></param>
    /// <param name="status"></param>
    private void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        bool isVisible = status.Status == Status.TRACKED;

        if (isVisible != targetVisible)
        {
            targetVisible = isVisible;

            if (targetVisible)
                OnTargetFound();
            else
                OnTargetLost();
        }
    }

    private void OnLeftButtonClick() => ChangeActualModel(false);
    private void OnRightButtonClick() => ChangeActualModel(true);

    private void ChangeActualModel(bool next)
    {
        if (!targetVisible) return;

        var models = AddModels.instance.models;
        int count = models.Count;
        if (count == 0) return;

        if (currentModel != null)
            currentModel.SetActive(false);

        index = next ? (index + 1) % count : (index - 1 + count) % count;

        currentModel = models[index];
        currentModel.SetActive(true);
    }

    private void OnTargetFound()
    {
        leftButton.gameObject.SetActive(true);
        rightButton.gameObject.SetActive(true);

        if (currentModel != null)
            currentModel.SetActive(true);
    }

    private void OnTargetLost()
    {
        leftButton.gameObject.SetActive(false);
        rightButton.gameObject.SetActive(false);

        if (currentModel != null)
            currentModel.SetActive(false);
    }
}
