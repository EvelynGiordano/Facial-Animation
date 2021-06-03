using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CharacterBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    public UnityEngine.UI.Button op1;
    public UnityEngine.UI.Button op2;
    public UnityEngine.UI.Button restart;
    public UnityEngine.UI.Button quit;
    public UnityEngine.UI.Button replay;
    public Text statement;
    public Animator anim;
    public GameObject character;

    private DialogueContainer _containerCache;
    private bool start = false;
    private string choiceText;
    private string option1Text;
    private string option2Text;
    private string dialogueText;
    private string currentNodeGuid;
    private string op1Target;
    private string op2Target;
    private bool found1 = false;
    private bool found2 = false;
    private bool playing = false;

    private void Start()
    {
        statement.gameObject.SetActive(true);
        character.gameObject.SetActive(true);
        op1.gameObject.SetActive(false);
        op2.gameObject.SetActive(false);
        statement.text = "Click anywhere to start!";

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !start)
            start = true;

        if (start & !playing)
        {
            playing = true;
            Play();
        }
    }

    private void Play()
    {
        _containerCache = Resources.Load<DialogueContainer>("FacialAnimationTree");
        if (_containerCache == null)
        {
            Debug.Log("File Not Found");
            return;
        }


        dialogueText = _containerCache.DialogueNodeData[0].DialogueText;
        currentNodeGuid = _containerCache.DialogueNodeData[0].Guid;

        foreach (var node in _containerCache.NodeLinks)
        {
            if ((currentNodeGuid == node.BaseNodeGuid))
            {
                if (!found1)
                {
                    option1Text = node.PortName;
                    op1Target = node.TargetNodeGuid;
                    found1 = true;
                    continue;
                }

                if (!found2)
                {
                    option2Text = node.PortName;
                    op2Target = node.TargetNodeGuid;
                    found2 = true;
                    continue;
                }

            }
        }

        SetInteraction();
    }


    void OnEnable()
    {
        op1.onClick.AddListener(Option1);//adds a listener for when you click the button
        op2.onClick.AddListener(Option2);

    }
    void Option1()// your listener calls this function
    {
        op1.gameObject.SetActive(false);
        op2.gameObject.SetActive(false);
        choiceText = op1.GetComponentInChildren<Text>().text;
        _containerCache = Resources.Load<DialogueContainer>("FacialAnimationTree");
        
        foreach (var node in _containerCache.NodeLinks)
        {
            if(node.PortName == choiceText)
            {
                op1Target = node.TargetNodeGuid;
            }
        }

        foreach(var node in _containerCache.DialogueNodeData)
        {
            if(node.Guid == op1Target)
            {
                dialogueText = node.DialogueText;
                currentNodeGuid = node.Guid;
            }
        }

        foreach (var node in _containerCache.NodeLinks)
        {
            if ((currentNodeGuid == node.BaseNodeGuid))
            {
                if (!found1)
                {
                    option1Text = node.PortName;
                    op1Target = node.TargetNodeGuid;
                    found1 = true;
                    continue;
                }

                if (!found2)
                {
                    option2Text = node.PortName;
                    op2Target = node.TargetNodeGuid;
                    found2 = true;
                    continue;
                }

            }
        }
        if(!found1 && !found2)  //no options found
        {
            anim.SetTrigger(dialogueText);
            statement.text = dialogueText;
            op1.gameObject.SetActive(false);
            op2.gameObject.SetActive(false);
            StartCoroutine(Restart(9));
            return;
        }else
        {
            SetInteraction();
        }
       
    }

   

    void Option2()// your listener calls this function
    {
        op1.gameObject.SetActive(false);
        op2.gameObject.SetActive(false);
        choiceText = op2.GetComponentInChildren<Text>().text;
        _containerCache = Resources.Load<DialogueContainer>("FacialAnimationTree");

        foreach (var node in _containerCache.NodeLinks)
        {
            if (node.PortName == choiceText)
            {
                op2Target = node.TargetNodeGuid;
            }
        }

        foreach (var node in _containerCache.DialogueNodeData)
        {
            if (node.Guid == op2Target)
            {
                dialogueText = node.DialogueText;
                currentNodeGuid = node.Guid;
            }
        }

        foreach (var node in _containerCache.NodeLinks)
        {
            if ((currentNodeGuid == node.BaseNodeGuid))
            {
                if (!found1)
                {
                    option1Text = node.PortName;
                    op1Target = node.TargetNodeGuid;
                    found1 = true;
                    continue;
                }

                if (!found2)
                {
                    option2Text = node.PortName;
                    op2Target = node.TargetNodeGuid;
                    found2 = true;
                    continue;
                }

            }
        }
        if (!found1 && !found2)  //no options found
        {
            anim.SetTrigger(dialogueText);
            statement.text = dialogueText;
            op1.gameObject.SetActive(false);
            op2.gameObject.SetActive(false);
            StartCoroutine(Restart(9));
            return;
        }
        else
        {
            SetInteraction();
        }
    }

    private IEnumerator Delay(float duration, string s1, string s2)
    {
        yield return new WaitForSeconds(duration);
        this.op1.gameObject.SetActive(true);
        this.op2.gameObject.SetActive(true);
        this.op1.GetComponentInChildren<Text>().text = s1;
        this.op2.GetComponentInChildren<Text>().text = s2;

    }

    public void SetInteraction()
    {
        anim.SetTrigger(dialogueText);
        statement.text = dialogueText;
        StartCoroutine(Delay(3, option1Text, option2Text));
        found1 = false;
        found2 = false;


    }

    private IEnumerator Restart(float duration)
    {
        yield return new WaitForSeconds(duration);
        SceneManager.LoadScene("facialAnimation");

    }
}