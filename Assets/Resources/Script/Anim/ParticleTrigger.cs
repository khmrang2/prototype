using UnityEngine;

public class ParticleTrigger : MonoBehaviour
{
    public ParticleSystem[] impactParticles; // 여러 파티클 시스템

    public void PlayImpactParticle(int index)
    {
        if (impactParticles != null && impactParticles.Length > index)
        {
            var selectedParticle = impactParticles[index];
            selectedParticle.Play();  // 선택한 파티클 실행

        }
    }
}