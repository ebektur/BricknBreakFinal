using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public GameObject BlockPrefab;
    public float myRadius = 2.0f;
    int num = 2;
    public Vector3 randPos;
    //int num = Random.Next(5); 
    // Start is called before the first frame update

    void Awake()
    {
        randPos = Vector3.zero;
        int myCheck = 0;
        for(int i=0; i<num; i++)
        {
            do{
                myCheck = 0;
                randPos = new Vector3(Random.Range(-4.0f,4.0f), 2.0f, Random.Range(-4.0f, 4.0f));
                Collider[] hitCollider = Physics.OverlapBox(randPos, transform.localScale / 2);

                for(int j=0; j<hitCollider.Length;j++){
                    if(hitCollider[j].tag == "Block")
                    {
                        myCheck++;
                    }
                }
            }
                while(myCheck > 0);
                GameObject newObj = (GameObject) Instantiate(BlockPrefab, randPos, Quaternion.identity);
                //Destroy(newObj, 4.0f);
            }

        } //onCollusionEnter


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Player>())
        {
            //other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

    }
}
