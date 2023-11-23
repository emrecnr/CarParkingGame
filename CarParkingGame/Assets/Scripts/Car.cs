using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public Route route;

    public Transform bottomTransform;
    public Transform bodyTransform;
    [SerializeField] MeshRenderer _meshRenderer;
    [SerializeField] Rigidbody _rb;
    [SerializeField] float _danceValue;
    [SerializeField] float _durationMultiplier;
    [SerializeField] private ParticleSystem _smokeFX;

    private void Start()
    {
        bodyTransform.DOLocalMoveY(_danceValue, .1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent(out Car otherCar)) 
        {
            StopDancingAnimation();
            _rb.DOKill(false);

            Vector3 hitPont = collision.contacts[0].point;
            _smokeFX.Play();
            AddExplosionForce(hitPont);

            Game.Instance.OnCarCollision?.Invoke();
        }
    }
    private void AddExplosionForce(Vector3 point)
    {
        _rb.AddExplosionForce(400f, point, 3f);
        _rb.AddForceAtPosition(Vector3.up * 2f, point, ForceMode.Impulse);
        _rb.AddTorque(new Vector3(GetRandomAngle(), GetRandomAngle(), GetRandomAngle()));
    }
    private float GetRandomAngle()
    {
        float angle = 10f;
        float rand = Random.value;
        return rand > .5f ? angle : -angle;
    }
    public void Move(Vector3[] path)
    {
        _rb.DOLocalPath(path , 2f * _durationMultiplier * path.Length).SetLookAt(.01f,false).SetEase(Ease.Linear);
    }
    public void StopDancingAnimation()
    {
        bodyTransform.DOKill(true);
    }

    public void SetColor(Color color)
    {
        _meshRenderer.sharedMaterial.color = color;
    }
}
