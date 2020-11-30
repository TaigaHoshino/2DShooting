using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.SceneManagement;
public class AdmobGamePlay : MonoBehaviour
{
    public string ios_Banner1;
    public string ios_Banner2;
    public string ios_Interstitial;
    public string android_Banner1;
    public string android_Banner2;
    public string android_Interstitial; 
    private static InterstitialAd interstitial;
    private AdRequest request;
    bool is_close_interstitial = false;
    private int reShowCount;
    private static int adCounter;
    void Awake()
    {
        //android_Interstitial = "ca-app-pub-3940256099942544/1033173712";
        android_Interstitial = "ca-app-pub-5584040938629320/5573343738";
        RequestInterstitial();
        reShowCount = 0;
//        RequestBanner();
    }

    void Start()
    {
        //SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    //void OnSceneUnloaded(Scene scene)
    //{
    //    Debug.Log(adRandom);
    //    if(adRandom == 3)
    //    {
    //        InterstitialShow();
    //    }
            
    //}

    public void StartAd()
    {
        adCounter++;
        if (adCounter >= 4)
        {
            InterstitialShow();
        }
    } 

    //    public void RequestBanner()
    //    {
    //#if UNITY_ANDROID
    //        string adUnitId1 = android_Banner1;
    //        string adUnitId2 = android_Banner2;
    //#elif UNITY_IPHONE
    //string adUnitId1 = ios_Banner1;
    //string adUnitId2 = ios_Banner2;
    //#else
    //string adUnitId = "unexpected_platform";
    //#endif
    //        BannerView bannerView1 = new BannerView(adUnitId1, AdSize.Banner, AdPosition.Top);
    //        BannerView bannerView2 = new BannerView(adUnitId2, AdSize.Banner, AdPosition.Bottom);
    //        request = new AdRequest.Builder()
    //        .AddTestDevice(AdRequest.TestDeviceSimulator)
    //        .AddTestDevice("自分のiPhoneのUDID")
    //        .Build();
    //        bannerView1.LoadAd(request);
    //        bannerView2.LoadAd(request);
    //    }
public void RequestInterstitial()
{
#if UNITY_ANDROID
        string adUnitId = android_Interstitial;
#elif UNITY_IPHONE
string adUnitId = ios_Interstitial;
#else
string adUnitId = "unexpected_platform";
#endif
        if (is_close_interstitial)
        {
            interstitial.Destroy();
        }
        interstitial = new InterstitialAd(adUnitId);
        //request = new AdRequest.Builder()
        //.AddTestDevice(AdRequest.TestDeviceSimulator)
        //.AddTestDevice("366D9D32D74F5288")
        //.Build();
        request = new AdRequest.Builder().Build();
        interstitial.LoadAd(request);
        interstitial.OnAdClosed += HandleAdClosed;
        interstitial.OnAdFailedToLoad += HandleAdReLoad;
        is_close_interstitial = false;
    }
    // インタースティシャル広告を閉じたとき
    void HandleAdClosed(object sender, System.EventArgs e)
    {
        is_close_interstitial = true;
        RequestInterstitial();
    }
    // 広告のロードに失敗したとき
    void HandleAdReLoad(object sender, System.EventArgs e)
    {
        is_close_interstitial = true;
        StartCoroutine(_waitConnect());
    }
    // 次のロードまで30秒待つ
    IEnumerator _waitConnect()
    {
        while (true)
        {
            yield return new WaitForSeconds(30.0f);
            // ネットに接続できるときだけリロード
            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                RequestInterstitial();
                break;
            }
        }
    }
    //インタースティシャル広告を表示したいときに呼び出す
    public void InterstitialShow()
    {
        //準備できてたら表示
        if (interstitial.IsLoaded())
        {
            interstitial.Show();
            adCounter = 0;
        }
    }
}