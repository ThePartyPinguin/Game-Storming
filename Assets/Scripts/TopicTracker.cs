using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TopicTracker : Singleton<TopicTracker>
{
    public Action<string> OnTopicUpdated;

    public string CurrentTopic { get; private set; }


    public void SetTopic(string newTopic)
    {
        CurrentTopic = newTopic;
        OnTopicUpdated?.Invoke(newTopic);
    }
}

