using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdmobManager : MonoBehaviour
{
    private BannerView bannerView;

    [SerializeField] private string appID = "ca-app-pub-6800953054676725~1470843669";

    [SerializeField] private string bannerID = "ca-app-pub-6800953054676725/7046603973";
    [SerializeField] private string regularAD = "ca-app-pub-6800953054676725/6323166350";

    private void Awake()
    {
        MobileAds.Initialize(appID);
    }

    public void OnClickShowBanner()
    {
        this.RequestBanner();
    }

    public void OnClickShowAd()
    {
        this.RequestRegularAD();
    }

    private void RequestRegularAD()
    {
        InterstitialAd AD = new InterstitialAd(regularAD);

        AdRequest request = new AdRequest.Builder().Build();

        AD.LoadAd(request);
    }

    private void RequestBanner()
    {
        bannerView = new BannerView(bannerID, AdSize.Banner, AdPosition.Bottom);

        AdRequest request = new AdRequest.Builder().Build();



        bannerView.LoadAd(request);
        bannerView.Show();
    }
}
