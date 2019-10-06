using UnityEngine.UI;
using UnityEngine;

public class Block : MonoBehaviour
{
    public static int maxHeight = 25;
    public Transform spriteTransform;
    public Text heightText;
    public SpriteRenderer blockSprite;

    public float _positionX;
    public float positionX //use this to change the position while sorting the array
    {
        set
        {
            _positionX = value;
            transform.position = new Vector3(value, transform.position.y, transform.position.z);
        }
        get
        {
            return _positionX;
        }
    }


    public int height   //this is the value of block to be used for sorting
    {
        set
        {
            if(Mathf.Abs(value) < Block.maxHeight)
            {
                _height = value;
                spriteTransform.localScale = new Vector3(spriteTransform.localScale.x, _height, spriteTransform.localScale.z);
                heightText.text = _height.ToString();
                //set the color of the block
                SetColor();
            }
        }
        get
        {
            return _height;
        }
    }

    int _height;

    void SetColor()
    {
        blockSprite.color = Color.black;    //set the color to 0,0,0 at begining
        float _red = ((float)_height / (float)Block.maxHeight);
        float _green = ((float)Block.maxHeight - (float)_height) / (float)Block.maxHeight;

        blockSprite.color = new Color(_red, _green, 0, 1);
    }

}
