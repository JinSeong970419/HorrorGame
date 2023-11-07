using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Horror
{
    public class TestCallPlayer : MonoBehaviour
    {
        [SerializeField] private GameEventVoid onCallPlayer;

        private float time;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            time += Time.deltaTime;
            if (time > 5f)
            {
                onCallPlayer.Invoke();
                Debug.Log("callPlayer");
                Destroy(this);
            }
        }
    }
}
