using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Oculus.Voice;
using Meta.WitAi.Json;

public class SpellCastManager : MonoBehaviour
{
    public InputActionProperty spellInput;
    public List<SpellData> spells;
    public MovementRecognizer movementRecognizer;
    public AppVoiceExperience voiceRecognizer;

    private string saidSpell;

    [System.Serializable]
    public class SpellData
    {
        public BasicSpell spell;
        public bool useMovement;
        public string movementName;
        public bool useVoice;
        public string voiceName;
    }

    public BasicSpell defaultSpell;
    public Transform wandTip;
    void Start()
    {
        voiceRecognizer.VoiceEvents.OnPartialResponse.AddListener(SetSaidSpeell);
    }

    private void SetSaidSpeell(WitResponseNode response)
    {
        string intentName = response["intents"][0]["name"].Value.ToString();
        saidSpell = intentName;
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
            StartCoroutine(StopCasting());
        }

        
    }

    public void StartCasting()
    {
        movementRecognizer.StartMovement();
        voiceRecognizer.Activate();
        saidSpell = "";
    }

    public IEnumerator StopCasting()
    {
        string movementName = movementRecognizer.EndMovement();
        voiceRecognizer.Deactivate();

        yield return new WaitForSeconds(0.35f);

        SpellData spellToCast;

        if (movementName != null)
        {
            Debug.Log(movementName);
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
        else if(saidSpell != null && saidSpell != "")
        {
            Debug.Log(saidSpell);
            int spellToCastIndex = spells.FindIndex(x => x.voiceName == saidSpell && x.useVoice);

            if (spellToCastIndex >= 0)
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
