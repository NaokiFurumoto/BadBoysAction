using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;
using UnityEngine.UI;

/// <summary>
/// �ۋ��Ǘ��FUnity IAP�i�A�v�����ۋ��V�X�e���j
/// �Ƃ肠����google�̂�
/// 
/// 1.���[�U�[���ۋ�
/// 2�D�ۋ��R�[���o�b�N���󂯎��
/// 3.���V�[�g���؂Ő���
/// 4.�A�C�e���ǉ�
/// 
/// </summary>
public class PurchaseManager : MonoBehaviour, IStoreListener
{
    public static PurchaseManager InstanceGooglePlay;

    #region private static
    // Purchasing �V�X�e���̎Q��
    private static IStoreController storeController;

    // �g�������ꍇ��Purchasing �T�u�V�X�e���̎Q��
    private static IExtensionProvider storeExtensionProvider;

    //���ꂪ���i��ID�F���ƂŐݒ�F�L�������Ȃ�ID���hRemoveAds�h�Ƃ��ɂ���
    private static string productIDNonConsumable = "nonconsumable";

    // Google Play Console �X�g�A���ʎq�F���Ƃœo�^�H
    private�@static string productNameGooglePlayNonConsumable = "purchasing.nonconsumable";
#endregion

    /// <summary>
    /// �w�����
    /// </summary>
    public enum PURCHASE_STATE
    {
        NOT_PURCHASED = 0,//���w��
        PURCHASED = 1,//�w���ς�
        PENDING = 2,//�ۗ���
    }

    private PURCHASE_STATE purchaseState = 0;

    //<summary> ���������� </summary>
    private bool isInitialized = false;

    #region �X�g�A������
    //<summary> �X�g�A������ </summary>
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
    /// �X�g�A������
    /// Consumable = 0,���Օi
    //�@NonConsumable = 1,�P��̂ݍw���\
    //  Subscription = 2�@�J��Ԃ��w�����ĕ����o����F�T�u����
    /// </summary>
    public void InitializePurchasing()
    {
        //�w���������������Ă����
        if (IsInitialized())
            return;

        //�w���@�\�FApple/google
        var module = StandardPurchasingModule.Instance();

        //�_�~�[�X�g�AUI��������
        //module.useFakeStoreUIMode = FakeStoreUIMode.StandardUser;

        //�l���擾���₷������
        var builder = ConfigurationBuilder.Instance(module);

        //ID�A�^�C�v�A����уX�g�A�ŗL�� ID �̃I�v�V�����̃Z�b�g���g�p���Đ��i��ǉ��F�����̏ꍇ�͂ǂ�����H
        //IAP Catalog (Window > Unity IAP > IAP Catalog) ����ł��o�^�ł���
        builder.AddProduct(productIDNonConsumable, ProductType.NonConsumable,
                           new IDs()
                           {   /* �X�g�A���Ƃ�ID���قȂ�ꍇ*/
                               //{"iOSProductID", AppleAppStore.Name },
                               //{"AndroidProductID,  GooglePlay.Name }

                               //�Ƃ肠�����A���h���C�h�̂�
                               { productNameGooglePlayNonConsumable,  GooglePlay.Name }
                           });
#if false
        //�R�����g�A�E�g��܂肽���ދ@�\ #if false
        /*Android�̏��Օi���N���A���邽��
        builder.AddProduct(productIDNonConsumable, ProductType.Consumable, new IDs()
			{
				{ productNameGooglePlayNonConsumable,  GooglePlay.Name }

			});
#endif
        //�X�g�A�̏��������N�G�X�g
        UnityPurchasing.Initialize(this, builder);
    }

    /// <summary>
    /// ����������
    /// </summary>
    /// <returns></returns>
    private bool IsInitialized()
    {
        return storeController != null && storeExtensionProvider != null;
    }

    /// <summary>
    /// �X�g�A�̏��������s
    /// </summary>
    /// <param name="error"></param>
    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log(" ���������s�̗��R:" + error);
    }

    /// <summary>
    /// �X�g�A�������������̃R�[���o�b�N
    /// </summary>
    /// <param name="controller">�X�g�A�̏��i���</param>
    /// <param name="extensions"></param>
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        // �X�g�A�̏������ɐ������܂����B
        Debug.Log("OnInitialized: PASS");

        // �w���V�X�e���S�́B
        storeController = controller;

        // �f�o�C�X�ŗL�̃X�g�A�@�\�ɃA�N�Z�X���邽�߂́A�X�g�A�ŗL�̃T�u�V�X�e���B
        storeExtensionProvider = extensions;

        // ���V�[�g�̌��؁F�w����@�͊���B����͂ЂƂ����Ȃ̂ŁAall��0�ԖځB
        if (storeController.products.all[0].hasReceipt)
        {
            //���V�[�g����
            purchaseState = checkGoogleReceipt(storeController.products.all[0].receipt);
        }
        else
        {
            // ���V�[�g�Ȃ�
            purchaseState = PURCHASE_STATE.NOT_PURCHASED;
        }
        isInitialized = true;
    }
    #endregion


    #region �w��
    /// <summary>
    /// �w�������FproductIDNonConsumable
    /// </summary>
    public void BuyNonConsumable() { BuyProductID(productIDNonConsumable); }
    public void BuyProductID(string productId)///������O������ĂԂ���
    {
        try
        {
            if (IsInitialized())
            {
                //ID���琻�i�擾
                Product product = storeController.products.WithID(productId);
                                      //�w���\�ȏ��i��
                if(product != null && product.availableToPurchase)
                {
                    //Unity IAP ���i ID,�X�g�A�pID
                    Debug.Log(string.Format("Purchasing product asychronously: '{0}' - '{1}'", 
                                             product.definition.id, product.definition.storeSpecificId));

                    //�w���J�n�I�I
                    storeController.InitiatePurchase(product);
                }
                else
                {   
                    Debug.Log("BuyProductID���s�B���i���w�����Ă��܂���B������Ȃ����A�w���ł��܂���");
                }

            }
            else
            {   
                Debug.Log("BuyProductID ���������s.");
            }
            
        }catch(Exception e)
        {
            Debug.Log("�w�����̎��s�F��O�̎�ނ́�" + e);
        }
    }


    /// <summary>
    /// �w��������ɌĂ΂��
    /// </summary>
    /// <param name="args">�w���������i�Ƃ��̍w�����V�[�g���܂�</param>
    /// <returns>�w����ԁ@����or������</returns>
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        //�������r                                                                     //��r���@
        if (String.Equals(args.purchasedProduct.definition.id, productIDNonConsumable, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("�w��: PASS.���i��ID: '{0}'", args.purchasedProduct.definition.id));
            checkGoogleReceipt(args.purchasedProduct.receipt);

        }
        else
        {
            Debug.Log(string.Format("���s�B�F������Ă��Ȃ����i: '{0}'", args.purchasedProduct.definition.id));
        }
        return PurchaseProcessingResult.Complete;
    }


    /// <summary>
    /// /�w�����s�R�[���o�b�N
    /// </summary>
    /// <param name="product"></param>
    /// <param name="failureReason"></param>
    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log(string.Format("�w�����s�������i: '{0}', ���R: {1}", product.definition.storeSpecificId, failureReason));
    }
    #endregion

    #region ���V�[�g����
    /// <summary>
    /// ���V�[�g���؁F���[�U�[���w�����Ă��Ȃ��R���e���c�ɃA�N�Z�X���邱�Ƃ�h��
    /// Android�̓R���r�j�����Ŏx�������ۗ����ɂȂ��Ă��鎞�A
    /// ���̏�Ԃ����V�[�g�Ŋm�F���邱�Ƃ��ł��܂��B
    /// lastGoogleResult�ɒ��l��4�Ƃ����l�������Ă��鎞�A�ۗ����̏�ԂƔ��f���Ă��܂��B
    /// </summary>
    /// <param name="receipt"></param>
    /// <returns></returns>
    PURCHASE_STATE checkGoogleReceipt(string receipt)
    {

#if UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_OSX

        PURCHASE_STATE resultstate;
        // �o���f�[�^�[���������܂��BGoogle Play �� Apple �X�g�A�����̌��؂Ɏg�p���邱�Ƃ��ł��܂��B
        //�@�G���[����ɂ́AGoogle Play ���J�L�[�� Apple �� root �ؖ������K�v�ł�
        var validator = new CrossPlatformValidator(GooglePlayTangle.Data(),
                                                   AppleTangle.Data(), Application.identifier);

        try
        {
            // Google Play �ŁAresult �� 1 �� product ID ���擾���܂�
            // Apple stores �ŁAreceipts �ɂ͕����̃v���_�N�g���܂܂�܂�
            var result = validator.Validate(receipt);
            // ���񋟂̖ړI�ŁA�����Ƀ��V�[�g�����X�g���܂�
            Debug.Log("�̎����͗L���ł�");

            GooglePurchaseState lastGoogleResult = GooglePurchaseState.Cancelled;
            DateTime lastData = DateTime.Today;

            bool setdata = false;
            foreach (IPurchaseReceipt productReceipt in result)
            {
                Debug.Log(productReceipt.productID);
                Debug.Log(productReceipt.purchaseDate);
                Debug.Log(productReceipt.transactionID);

                //GooglePlay �̍w�����V�[�g
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
    /// ���i�̎擾
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

    //�Q�l
    /*�����o�^
     * var products = new [] {
	new ProductDefinition ("jp.nyanta.tetr4lab.unityiaptest.item1", �X�g�AID, ProductType.Consumable),
	new ProductDefinition ("jp.nyanta.tetr4lab.unityiaptest.item2", ProductType.NonConsumable),
	new ProductDefinition ("jp.nyanta.tetr4lab.unityiaptest.item3", ProductType.NonConsumable),
};
     */

    /*�����̌�̑Ή�
     1�D�A�v�����A�C�e���̒ǉ�
     Google Play Console �̃A�v���Ǘ���ʂ���A�v�����A�C�e����I�����A�ۋ��A�C�e����ǉ����܂��B
     ���i����A�C�e��ID�͖{�ԂŎg�p������̂Ɠ������̂��g�p���܂��B

    �����ɂŐݒ肷��A�C�e��ID�͎��������R�[�h�ɖ��ߍ���ł����Ȃ���΂Ȃ�܂���B
    ������ŏЉ�Ă���productNameGooglePlayNonConsumable�ɃA�C�e��ID��ݒ肵�Ă����܂��B

    2.�e�X�g�F�菇�`�F�b�N
    https://hirokuma.blog/?p=4513

    3�D���[�U�[�ƌ����ɃA�J�E���g��ǉ�
     �e�X�^�[�̃A�J�E���g�igmail�A�h���X�j��ǉ����܂��B�������͂��Y�ꂽ�A�J�E���g�Ńe�X�g����Ǝ��ۂɉۋ�����Ă��܂��̂ŁA���ӂ��K�v�ł��B
     */


}
