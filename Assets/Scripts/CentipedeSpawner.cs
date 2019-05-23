using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentipedeSpawner : MonoBehaviour {

    [SerializeField] private Transform _spawnTransform;
    [SerializeField] private Transform _parentTransform;
    [SerializeField] private GameObject _sectionPrefab;

    public void CreateNewCentipede(int sectionCount, float positionUpdateTime)
    {
        List<Transform> sections = new List<Transform>();
        Vector3 size = FieldController.Instance.CellSize * 0.35f;
        for (int i = 0; i < sectionCount - 1; i++)
        {
            GameObject go = Instantiate
                (_sectionPrefab, _spawnTransform.position, Quaternion.identity, _parentTransform);
            sections.Add(go.transform);
            go.transform.localScale = size;
        }

        GameObject head = Instantiate
                (_sectionPrefab, _spawnTransform.position, Quaternion.identity, _parentTransform);
        head.transform.localScale = size;
        CentipedeController cc = head.AddComponent<CentipedeController>();
        cc.SetDirection(Vector2.right);
        cc.SetTail(sections.ToArray());
        cc.SetUpdateTime(positionUpdateTime);
    }
}
