using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;
using UnityEngine.UI;

/// <summary>
/// 課金管理：Unity IAP（アプリ内課金システム）
/// とりあえずgoogleのみ
/// 
/// 1.ユーザーが課金
/// 2．課金コールバックを受け取る
/// 3.レシート検証で成功
/// 4.アイテム追加
/// 
/// </summary>
public class PurchaseManager : MonoBehaviour, IStoreListener
{
    public static PurchaseManager InstanceGooglePlay;

    #region private static
    // Purchasing システムの参照
    private static IStoreController storeController;

    // 拡張した場合のPurchasing サブシステムの参照
    private static IExtensionProvider storeExtensionProvider;

    //これが商品のID：あとで設定：広告解除ならIDを”RemoveAds”とかにする
    private static string productIDNonConsumable = "nonconsumable";

    // Google Play Console ストア識別子：あとで登録？
    private　static string productNameGooglePlayNonConsumable = "purchasing.nonconsumable";
#endregion

    /// <summary>
    /// 購入状態
    /// </summary>
    public enum PURCHASE_STATE
    {
        NOT_PURCHASED = 0,//未購入
        PURCHASED = 1,//購入済み
        PENDING = 2,//保留中
    }

    private PURCHASE_STATE purchaseState = 0;

    //<summary> 初期化判定 </summary>
    private bool isInitialized = false;

    #region ストア初期化
    //<summary> ストア初期化 </summary>
    void Awake() { InitializeThis(); }
    void InitializeThis()
    {
        InstanceGooglePlay ??= this;

        if(storeController == null)
        {
            InitializePurchasing();
        }
    }

    /// <summary>
    /// ストア初期化
    /// Consumable = 0,消耗品
    //　NonConsumable = 1,１回のみ購入可能
    //  Subscription = 2　繰り返し購入して復元出来る：サブすく
    /// </summary>
    public void InitializePurchasing()
    {
        //購入処理が完了していれば
        if (IsInitialized())
            return;

        //購入機能：Apple/google
        var module = StandardPurchasingModule.Instance();

        //ダミーストアUIをだせる
        //module.useFakeStoreUIMode = FakeStoreUIMode.StandardUser;

        //値を取得しやすくする
        var builder = ConfigurationBuilder.Instance(module);

        //ID、タイプ、およびストア固有の ID のオプションのセットを使用して製品を追加：複数の場合はどうする？
        //IAP Catalog (Window > Unity IAP > IAP Catalog) からでも登録できる
        builder.AddProduct(productIDNonConsumable, ProductType.NonConsumable,
                           new IDs()
                           {   /* ストアごとにIDが異なる場合*/
                               //{"iOSProductID", AppleAppStore.Name },
                               //{"AndroidProductID,  GooglePlay.Name }

                               //とりあえずアンドロイドのみ
                               { productNameGooglePlayNonConsumable,  GooglePlay.Name }
                           });
#if false
        //コメントアウトを折りたたむ機能 #if false
        /*Androidの消耗品をクリアするため
        builder.AddProduct(productIDNonConsumable, ProductType.Consumable, new IDs()
			{
				{ productNameGooglePlayNonConsumable,  GooglePlay.Name }

			});
#endif
        //ストアの初期化リクエスト
        UnityPurchasing.Initialize(this, builder);
    }

    /// <summary>
    /// 初期化判定
    /// </summary>
    /// <returns></returns>
    private bool IsInitialized()
    {
        return storeController != null && storeExtensionProvider != null;
    }

    /// <summary>
    /// ストアの初期化失敗
    /// </summary>
    /// <param name="error"></param>
    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log(" 初期化失敗の理由:" + error);
    }

    /// <summary>
    /// ストア初期化成功時のコールバック
    /// </summary>
    /// <param name="controller">ストアの商品情報</param>
    /// <param name="extensions"></param>
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        // ストアの初期化に成功しました。
        Debug.Log("OnInitialized: PASS");

        // 購買システム全体。
        storeController = controller;

        // デバイス固有のストア機能にアクセスするための、ストア固有のサブシステム。
        storeExtensionProvider = extensions;

        // レシートの検証：指定方法は幾つか。今回はひとつだけなので、allの0番目。
        if (storeController.products.all[0].hasReceipt)
        {
            //レシートあり
            purchaseState = checkGoogleReceipt(storeController.products.all[0].receipt);
        }
        else
        {
            // レシートなし
            purchaseState = PURCHASE_STATE.NOT_PURCHASED;
        }
        isInitialized = true;
    }
    #endregion


    #region 購入
    /// <summary>
    /// 購入処理：productIDNonConsumable
    /// </summary>
    public void BuyNonConsumable() { BuyProductID(productIDNonConsumable); }
    public void BuyProductID(string productId)///これを外部から呼ぶこと
    {
        try
        {
            if (IsInitialized())
            {
                //IDから製品取得
                Product product = storeController.products.WithID(productId);
                                      //購入可能な商品か
                if(product != null && product.availableToPurchase)
                {
                    //Unity IAP 製品 ID,ストア用ID
                    Debug.Log(string.Format("Purchasing product asychronously: '{0}' - '{1}'", 
                                             product.definition.id, product.definition.storeSpecificId));

                    //購入開始！！
                    storeController.InitiatePurchase(product);
                }
                else
                {   
                    Debug.Log("BuyProductID失敗。製品を購入していません。見つからないか、購入できません");
                }

            }
            else
            {   
                Debug.Log("BuyProductID 初期化失敗.");
            }
            
        }catch(Exception e)
        {
            Debug.Log("購入時の失敗：例外の種類は＝" + e);
        }
    }


    /// <summary>
    /// 購入完了後に呼ばれる
    /// </summary>
    /// <param name="args">購入した製品とその購入レシートを含む</param>
    /// <returns>購入状態　完了or処理中</returns>
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        //文字列比較                                                                     //比較方法
        if (String.Equals(args.purchasedProduct.definition.id, productIDNonConsumable, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("購入: PASS.製品のID: '{0}'", args.purchasedProduct.definition.id));
            checkGoogleReceipt(args.purchasedProduct.receipt);

        }
        else
        {
            Debug.Log(string.Format("失敗。認識されていない製品: '{0}'", args.purchasedProduct.definition.id));
        }
        return PurchaseProcessingResult.Complete;
    }


    /// <summary>
    /// /購入失敗コールバック
    /// </summary>
    /// <param name="product"></param>
    /// <param name="failureReason"></param>
    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log(string.Format("購入失敗した商品: '{0}', 理由: {1}", product.definition.storeSpecificId, failureReason));
    }
    #endregion

    #region レシート検証
    /// <summary>
    /// レシート検証：ユーザーが購入していないコンテンツにアクセスすることを防ぐ
    /// Androidはコンビニ払いで支払いが保留中になっている時、
    /// その状態もレシートで確認することができます。
    /// lastGoogleResultに直値で4という値が入っている時、保留中の状態と判断しています。
    /// </summary>
    /// <param name="receipt"></param>
    /// <returns></returns>
    PURCHASE_STATE checkGoogleReceipt(string receipt)
    {

#if UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_OSX

        PURCHASE_STATE resultstate;
        // バリデーターを準備します。Google Play と Apple ストア両方の検証に使用することができます。
        //　エラー回避には、Google Play 公開キーか Apple の root 証明書が必要です
        var validator = new CrossPlatformValidator(GooglePlayTangle.Data(),
                                                   AppleTangle.Data(), Application.identifier);

        try
        {
            // Google Play で、result は 1 つの product ID を取得します
            // Apple stores で、receipts には複数のプロダクトが含まれます
            var result = validator.Validate(receipt);
            // 情報提供の目的で、ここにレシートをリストします
            Debug.Log("領収書は有効です");

            GooglePurchaseState lastGoogleResult = GooglePurchaseState.Cancelled;
            DateTime lastData = DateTime.Today;

            bool setdata = false;
            foreach (IPurchaseReceipt productReceipt in result)
            {
                Debug.Log(productReceipt.productID);
                Debug.Log(productReceipt.purchaseDate);
                Debug.Log(productReceipt.transactionID);

                //GooglePlay の購入レシート
                GooglePlayReceipt google = productReceipt as GooglePlayReceipt;
                if (null != google)
                {
                    Debug.Log(google.purchaseState);
                    if (!setdata)
                    {
                        setdata = true;
                        lastData = productReceipt.purchaseDate;
                        lastGoogleResult = google.purchaseState;
                    }
                    else
                    {
                        if (lastData < productReceipt.purchaseDate)
                        {
                            lastData = productReceipt.purchaseDate;
                            lastGoogleResult = google.purchaseState;
                        }
                    }
                }
            }


            if (lastGoogleResult == GooglePurchaseState.Purchased)
            {
                resultstate = PURCHASE_STATE.PURCHASED;
            }
            else if ((int)lastGoogleResult == 4)
            {
                resultstate = PURCHASE_STATE.PENDING;
            }
            else
            {
                resultstate = PURCHASE_STATE.NOT_PURCHASED;
            }
        }
        catch (IAPSecurityException)
        {
            Debug.Log("Invalid receipt, not unlocking content");
            resultstate = PURCHASE_STATE.NOT_PURCHASED;
        }

        return resultstate;
#endif

        return PURCHASE_STATE.PURCHASED;
    }
    #endregion

    /// <summary>
    /// 価格の取得
    /// </summary>
    /// <returns></returns>
    public string GetlocalizedPriceString()
    {
        string retstr = "";
        if (storeController != null)
        {
            byte[] bytesData = System.Text.Encoding.UTF8.GetBytes(storeController.products.all[0].metadata.localizedPriceString);
            if (bytesData[0] == 0xC2 && bytesData[1] == 0xA5)
            {
                retstr = "\\";
                retstr += storeController.products.all[0].metadata.localizedPriceString.Substring(1, storeController.products.all[0].metadata.localizedPriceString.Length - 1);
                retstr += "(" + storeController.products.all[0].metadata.isoCurrencyCode + ")";
            }
            else
            {
                retstr = storeController.products.all[0].metadata.localizedPriceString;
                retstr += "(" + storeController.products.all[0].metadata.isoCurrencyCode + ")";
            }
        }
        else
        {
            retstr = null;

        }
        return retstr;
    }

    //参考
    /*複数登録
     * var products = new [] {
	new ProductDefinition ("jp.nyanta.tetr4lab.unityiaptest.item1", ストアID, ProductType.Consumable),
	new ProductDefinition ("jp.nyanta.tetr4lab.unityiaptest.item2", ProductType.NonConsumable),
	new ProductDefinition ("jp.nyanta.tetr4lab.unityiaptest.item3", ProductType.NonConsumable),
};
     */

    /*■この後の対応
     1．アプリ内アイテムの追加
     Google Play Console のアプリ管理画面からアプリ内アイテムを選択し、課金アイテムを追加します。
     商品名やアイテムIDは本番で使用するものと同じものを使用します。

    ここにで設定するアイテムIDは実装したコードに埋め込んでおかなければなりません。
    こちらで紹介しているproductNameGooglePlayNonConsumableにアイテムIDを設定しておきます。

    2.テスト：手順チェック
    https://hirokuma.blog/?p=4513

    3．ユーザーと権限にアカウントを追加
     テスターのアカウント（gmailアドレス）を追加します。これを入力し忘れたアカウントでテストすると実際に課金されてしまうので、注意が必要です。
     */


}
