using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentipedeSpawner : MonoBehaviour {

    [SerializeField] private Transform _spawnTransform;
    [SerializeField] private Transform _parentTransform;
    [SerializeField] private GameObject _sectionPrefab;

    private void Awake()
    {
        Messenger<int, float>.AddListener(EventStrings.CREATE_NEW_CENTIPEDE, CreateNewCentipede);
    }

    private void OnDestroy()
    {
        Messenger<int, float>.RemoveListener(EventStrings.CREATE_NEW_CENTIPEDE, CreateNewCentipede);
    }

    // Method for creating new centipede with defined body length and speed
    public void CreateNewCentipede(int sectionCount, float positionUpdateTime)
    {
        List<Transform> sections = new List<Transform>();
        Vector3 size = GlobalVariables.CELL_SIZE * 0.35f;

        // Creating tail of the new centipede
        for (int i = 0; i < sectionCount - 1; i++)
        {
            GameObject go = Instantiate
                (_sectionPrefab, _spawnTransform.position, Quaternion.identity, _parentTransform);
            sections.Add(go.transform);
            go.transform.localScale = size;
        }

        // Creating the head of centipede
        // Setting direction, tail and speed (time btw changing position) 
        GameObject head = Instantiate
                (_sectionPrefab, _spawnTransform.position, Quaternion.identity, _parentTransform);
        head.transform.localScale = size;
        CentipedeController cc = head.AddComponent<CentipedeController>();
        cc.SetDirection(Vector2.right);
        cc.SetTail(sections.ToArray());
        cc.SetUpdateTime(positionUpdateTime);
    }
}
