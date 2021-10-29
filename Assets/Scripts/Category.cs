using UnityEngine;

public class Category : MonoBehaviour
{
    public Map[] maps;

    public string categoryName
    {
        get
        {
            return name.Replace("(Clone)", "");
        }
    }
}
