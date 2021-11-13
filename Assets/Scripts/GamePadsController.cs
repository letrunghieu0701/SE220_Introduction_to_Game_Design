using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePadsController : MonoBehaviour
{

    bool _canMoveLeft;
    bool _canMoveRight;

    public bool CanMoveLeft { get => _canMoveLeft; set => _canMoveLeft = value; }
    public bool CanMoveRight { get => _canMoveRight; set => _canMoveRight = value; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PCHandle();
    }

    void PCHandle(){
        _canMoveLeft = Input.GetAxis("Horizontal") < 0;
        _canMoveRight = Input.GetAxis("Horizontal") > 0;
    }
}
