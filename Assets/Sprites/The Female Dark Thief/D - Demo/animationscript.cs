using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class animationscript : MonoBehaviour {
    [SerializeField]
    Animator anim = null; //reference to animator

    [SerializeField]
    List<AnimationClip> clips = new List<AnimationClip>(); // list with all aniamtion clip names

    [SerializeField]
    int index = 0; // counter

    [SerializeField]
    Text animText; // update text on canvas
    // Use this for initialization
    void Start () {
       
        anim = GetComponent<Animator>(); // get reference 

        foreach (AnimationClip ac in anim.runtimeAnimatorController.animationClips)
        {
            clips.Add(ac); // add all clips to list
        }

        anim.Play(0); // play first animation

         
    }
	
	// Update is called once per frame
	void Update () {
        // play next clip
        if (Input.GetButtonDown("Fire1"))
        {
            index++;
            if (index > clips.Count-1)
            {
                index = 0;
            }
            anim.Play(clips[index].name); 
        }
        // play previous clip
        if (Input.GetMouseButtonDown(1))
        {
            index--;
            if (index < 0)
            {
                index = clips.Count-1;
            }
            anim.Play(clips[index].name);
        }

        // update text
        animText.text = clips[index].name;
        
    }



}
