using UnityEngine;
public class Move : MonoBehaviour
{
    public float speed = 3;
    void Update()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Vertical"), 0, 0);
        transform.Translate(movement * speed * Time.deltaTime); // This line is missing
    }
}