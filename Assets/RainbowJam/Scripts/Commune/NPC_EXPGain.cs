using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.RainbowJam.Scripts.Commune
{
    class NPC_EXPGain : MonoBehaviour
    {
        //This is going to accumulate EXP whilst the NPCs are doing things in the commune, somehow

        void Start()
        {

        }

        void Update()
        {
            var npcScript = gameObject.GetComponent<NPC>();

            if (npcScript == null)
                return;

            if (npcScript.GetPersonalStory() == null)
                return;

            npcScript.GetPersonalStory().AddEXP(1.0f);

            //Debug.Log("GAINING EXP");
        }

    }
}
