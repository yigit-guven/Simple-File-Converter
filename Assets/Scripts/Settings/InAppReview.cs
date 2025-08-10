using System.Collections;
using UnityEngine;
using Google.Play.Review;

public class RateUsButton : MonoBehaviour
{
    private ReviewManager _reviewManager;

    /// <summary>
    /// Call this from your UI Button OnClick event.
    /// </summary>
    public void OnRateUsClicked()
    {
        StartCoroutine(RequestReview());
    }

    private IEnumerator RequestReview()
    {
        _reviewManager = new ReviewManager();

        // Step 1: Request the review flow
        var requestFlowOperation = _reviewManager.RequestReviewFlow();
        yield return requestFlowOperation;

        if (requestFlowOperation.Error != ReviewErrorCode.NoError)
        {
            Debug.LogWarning("In-app review unavailable: " + requestFlowOperation.Error);
            OpenStorePageFallback();
            yield break;
        }

        var reviewInfo = requestFlowOperation.GetResult();

        // Step 2: Launch the review flow
        var launchFlowOperation = _reviewManager.LaunchReviewFlow(reviewInfo);
        yield return launchFlowOperation;

        // Log completion
        Debug.Log("In-app review flow finished.");
    }

    private void OpenStorePageFallback()
    {
        string storeUrl = "https://play.google.com/store/apps/details?id=com.YigitGuven.SimpleFileConverter" + Application.identifier;
        Application.OpenURL(storeUrl);
    }
}
