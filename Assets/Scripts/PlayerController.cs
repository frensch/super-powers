using UnityEngine;
using System.Collections;
using UnityEditor;

public class PlayerController : MonoBehaviour {

    public CharacterDirection charDir;

    private Animator animator;
    private float last_horizontal = 0;
    private float last_vertical = 0;
    private string animation_direction = "Front";
    // Use this for initialization
    public static IEnumerable SceneRoots()
    {
        var prop = new HierarchyProperty(HierarchyType.GameObjects);
        var expanded = new int[0];
        while (prop.Next(expanded))
        {
            yield return prop.pptrValue as GameObject;
        }
    }

    void Start () {
        animator = GetComponent<Animator>();
        //charDir = transform.root.GetComponentInChildren<CharacterDirection>();
        if (!charDir)
            throw new System.Exception();
    }

    void ChangeMoveAnimation(float horizontal, float vertical)
    {
        float MARGIN_UP = 0.2f;
        float MARGIN_DOWN = -MARGIN_UP;
        if (horizontal == 0 && vertical == 0)
        {
            animator.SetTrigger("Idle");
        } else if (horizontal <= MARGIN_UP && horizontal >= MARGIN_DOWN)
        {
            if (vertical > MARGIN_UP)
            {
                animation_direction = "Back";
            }
            else if (vertical < MARGIN_DOWN)
            {
                animation_direction = "Front";
            }
        } else if (vertical <= MARGIN_UP && vertical >= MARGIN_DOWN)
        {
            if (horizontal > MARGIN_UP)
            {
                animation_direction = "Right";
            }
            else if (horizontal < MARGIN_DOWN)
            {
                animation_direction = "Left";
            }
        } else if (vertical > MARGIN_UP)
        {
            if (horizontal > MARGIN_UP)
            {
                animation_direction = "BackRight";
            }
            else if (horizontal < MARGIN_DOWN)
            {
                animation_direction = "BackLeft";
            }
        } else if (vertical < MARGIN_DOWN)
        {
            if (horizontal > MARGIN_UP)
            {
                animation_direction = "FrontRight";
            }
            else if (horizontal < MARGIN_DOWN)
            {
                animation_direction = "FrontLeft";
            }
        }
        if (horizontal != 0 || vertical != 0)
        {
            animator.SetTrigger("Walk");
        }
        animator.SetTrigger(animation_direction);
    }

    static int Sign(float number)
    {
        return number < 0 ? -1 : (number > 0 ? 1 : 0);
    }
    Vector2 CalcMovement()
    {
        Vector2 direction = charDir.CalcDirection(true, false, transform.position);

        //Vector2 movement = new Vector2(Sign(direction.x), Sign(direction.y));
        Vector2 movement = direction.normalized;
        Debug.Log(direction + " " + movement );
        return movement;
    }
    // Update is called once per frame 
    void Update()
    {
        float scale = 0.01f;
        float horizontal = 0;
        float vertical = 0;

        //horizontal = (int)Input.GetAxis("Horizontal");
        //vertical = (int)Input.GetAxis("Vertical");
        Vector2 movement = CalcMovement();
        horizontal = movement.x;
        vertical = movement.y;

        if (Sign(last_vertical) != Sign(vertical) || Sign(last_horizontal) != Sign(horizontal))
            ChangeMoveAnimation(horizontal, vertical);

        last_horizontal = horizontal;
        last_vertical = vertical;
        transform.Translate(new Vector3(horizontal* scale, vertical* scale, 0));
    }
}
