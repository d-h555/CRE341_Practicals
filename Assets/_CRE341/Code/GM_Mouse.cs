using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine.SceneManagement;
public class GM_Mouse : MonoBehaviour
{
   public static GM_Mouse instance{ get; private set; }

    [Header("Chase Parameters")]
    public AnimationCurve distanceCurve;
    public AnimationCurve angleCurve;
    public AnimationCurve chaseContinuityCurve;

    [Header("Flee Parameters")]
    public AnimationCurve fleeDistanceCurve;

    public List<GameObject> miceList = new List<GameObject>();
    public List<GameObject> catList = new List<GameObject>();

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
