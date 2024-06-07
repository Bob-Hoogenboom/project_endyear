using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System.Linq;

public class PointCounter : MonoBehaviour
{
    [SerializeField] private List<TargetBehaviour> targetList;
    [SerializeField] private TMP_Text targetScoreTMP;
    [SerializeField] private TMP_Text currentScoreTMP;

    private int _currentScore;

    public UnityEvent onEveryTargetHit;

    private void Start()
    {
        _currentScore = 0;
        targetScoreTMP.text = targetList.Count.ToString();
        currentScoreTMP.text = _currentScore.ToString();
    }

    private void Update()
    {
        foreach (TargetBehaviour target in targetList.ToList())
        {
            if (target.isBroken)
            {
                targetList.Remove(target);

                _currentScore++; // update score index

                currentScoreTMP.text = _currentScore.ToString(); // update score
            }
        }

        if (targetList.Count <= 0) 
        {
            onEveryTargetHit.Invoke();
        }
    }
}
