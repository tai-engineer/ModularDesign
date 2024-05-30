using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using UnityEngine;
using TMPro;

namespace UI
{
    public class CharacterDisplayInfo : MonoBehaviour
    {
        [SerializeField]
        CharacterManager _character;

        [SerializeField]
        TextMeshProUGUI _moveVectorUI;

        void Update()
        {
            var moveVector = _character.MoveVector;
            _moveVectorUI.text = moveVector.ToString();
        }
    }
}
