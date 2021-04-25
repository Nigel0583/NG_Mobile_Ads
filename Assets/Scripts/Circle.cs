using UnityEngine;

public class Circle : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnMouseDown()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().ScoreKeep();
        Destroy(gameObject);
    }
}