using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class hud : MonoBehaviour
{
    [Header("carry, reload, current")]
    [SerializeField]
    private Text[] text;
    [SerializeField]
    private Gun theGun;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
       text[0].text = theGun.GetcarrryBulletcount().ToString();
       text[1].text = theGun.GetreloadBulletCount().ToString();
       text[2].text = theGun.GetcurrentBulletcount().ToString();

    }
}
