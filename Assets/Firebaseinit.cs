using Firebase;
using Firebase.Analytics;
using UnityEngine;

public class Firebaseinit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(continuationAction: task =>
       {
           FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
       });
    }
}
