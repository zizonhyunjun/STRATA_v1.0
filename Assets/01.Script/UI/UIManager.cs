using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public ClearText clearText;
    public Health health;
    
    private void Awake()
    {
        instance = this;
    }//얜 하는게 뭐임ㅋㅋ
}
