using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TopicLabelUpdater : MonoBehaviour
{
    private TMP_Text _topicLabel;

    private TopicTracker _topicTracker;
    // Start is called before the first frame update
    void Start()
    {
        _topicLabel = GetComponent<TMP_Text>();
        _topicTracker = TopicTracker.Instance;
        _topicTracker.OnTopicUpdated += UpdateTopic;
        UpdateTopic(_topicTracker.CurrentTopic);
    }

    public void UpdateTopic(string topic)
    {
        _topicLabel.text = "Topic: "+ topic;
    }
}
