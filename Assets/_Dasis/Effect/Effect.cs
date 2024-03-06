using Dasis.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Dasis.Effect
{
    public class Effect : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem effect;

        [Button]
        public void PlayOneShot()
        {
            effect.gameObject.SetActive(true);
            effect.Play();
            this.WaitForSeconds(effect.main.duration, () =>
            {

            });
        }
    }
}
