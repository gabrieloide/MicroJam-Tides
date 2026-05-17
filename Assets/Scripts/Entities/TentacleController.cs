using UnityEngine;
using DG.Tweening;

public class TentacleController : MonoBehaviour
{
    [Header("Segment References")]
    [SerializeField] private Transform baseSegment;
    [SerializeField] private Transform middleSegment;
    [SerializeField] private Transform tipSegment;

    [Header("Movement Offsets")]
    [SerializeField] private Vector3 windupPositionOffset = new Vector3(0f, 1f, -1.5f);
    [SerializeField] private Vector3 slamPositionOffset = new Vector3(0f, -2.5f, 3.5f);

    [Header("Windup Rotation Angles")]
    [SerializeField] private Vector3 baseWindupRot = new Vector3(-15f, 0f, 0f);
    [SerializeField] private Vector3 middleWindupRot = new Vector3(-25f, 0f, 0f);
    [SerializeField] private Vector3 tipWindupRot = new Vector3(-35f, 0f, 0f);

    [Header("Slam Rotation Angles")]
    [SerializeField] private Vector3 baseSlamRot = new Vector3(35f, 0f, 0f);
    [SerializeField] private Vector3 middleSlamRot = new Vector3(50f, 0f, 0f);
    [SerializeField] private Vector3 tipSlamRot = new Vector3(65f, 0f, 0f);

    [Header("Animation Timings")]
    [SerializeField] private float windupDuration = 0.45f;
    [SerializeField] private float slamDuration = 0.15f;
    [SerializeField] private float recoveryDuration = 0.7f;

    private Vector3 _originalPosition;
    private Quaternion _originalBaseRot;
    private Quaternion _originalMiddleRot;
    private Quaternion _originalTipRot;
    private bool _isAnimating = false;

    private void Start()
    {
        _originalPosition = transform.localPosition;

        if (baseSegment != null) _originalBaseRot = baseSegment.localRotation;
        if (middleSegment != null) _originalMiddleRot = middleSegment.localRotation;
        if (tipSegment != null) _originalTipRot = tipSegment.localRotation;
    }

    public void PlaySlamAnimation(System.Action onImpactCallback = null)
    {
        if (_isAnimating) return;
        _isAnimating = true;

        Sequence slamSequence = DOTween.Sequence();

        slamSequence.Append(transform.DOLocalMove(_originalPosition + windupPositionOffset, windupDuration).SetEase(Ease.OutQuad));
        if (baseSegment != null) slamSequence.Join(baseSegment.DOLocalRotate(baseWindupRot, windupDuration).SetEase(Ease.OutQuad));
        if (middleSegment != null) slamSequence.Join(middleSegment.DOLocalRotate(middleWindupRot, windupDuration).SetEase(Ease.OutQuad));
        if (tipSegment != null) slamSequence.Join(tipSegment.DOLocalRotate(tipWindupRot, windupDuration).SetEase(Ease.OutQuad));

        slamSequence.Append(transform.DOLocalMove(_originalPosition + slamPositionOffset, slamDuration).SetEase(Ease.InCubic));
        if (baseSegment != null) slamSequence.Join(baseSegment.DOLocalRotate(baseSlamRot, slamDuration).SetEase(Ease.InCubic));
        if (middleSegment != null) slamSequence.Join(middleSegment.DOLocalRotate(middleSlamRot, slamDuration).SetEase(Ease.InCubic));
        if (tipSegment != null) slamSequence.Join(tipSegment.DOLocalRotate(tipSlamRot, slamDuration).SetEase(Ease.InCubic));

        slamSequence.AppendCallback(() =>
        {
            onImpactCallback?.Invoke();
        });

        slamSequence.Append(transform.DOLocalMove(_originalPosition, recoveryDuration).SetEase(Ease.OutElastic));
        if (baseSegment != null) slamSequence.Append(baseSegment.DOLocalRotate(_originalBaseRot.eulerAngles, recoveryDuration).SetEase(Ease.OutElastic));
        if (middleSegment != null) slamSequence.Join(middleSegment.DOLocalRotate(_originalMiddleRot.eulerAngles, recoveryDuration).SetEase(Ease.OutElastic));
        if (tipSegment != null) slamSequence.Join(tipSegment.DOLocalRotate(_originalTipRot.eulerAngles, recoveryDuration).SetEase(Ease.OutElastic));

        slamSequence.OnComplete(() =>
        {
            _isAnimating = false;
        });
    }
}
