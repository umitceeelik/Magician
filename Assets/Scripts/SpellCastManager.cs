using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpellCastManager : MonoBehaviour
{
    public InputActionProperty spellInput;
    public List<SpellData> spells;
    public MovementRecognizer movementRecognizer;

    [System.Serializable]
    public class SpellData
    {
        public BasicSpell spell;
        public bool useMovement;
        public string movementName;
    }

    public BasicSpell defaultSpell;
    public Transform wandTip;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (spellInput.action.WasPressedThisFrame())
        {
            StartCasting();
        }
        else if(spellInput.action.WasReleasedThisFrame())
        {
            StopCasting();
        }

        
    }

    public void StartCasting()
    {
        movementRecognizer.StartMovement();
    }

    public void StopCasting()
    {
        string movementName = movementRecognizer.EndMovement();
        SpellData spellToCast;

        if (movementName != null)
        {
            int spellToCastIndex = spells.FindIndex(x => x.movementName == movementName && x.useMovement);

            if(spellToCastIndex >= 0)
            {
                spellToCast = spells[spellToCastIndex];
            }
            else
            {
                spellToCast = spells[0];
            }
        }
        else
        {
            spellToCast = spells[0];
        }

        BasicSpell spawnedSpell = Instantiate(spellToCast.spell);
        spawnedSpell.Initialize(wandTip);
    }
}
