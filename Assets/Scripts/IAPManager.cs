using System;
using System.Threading;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : Singleton<IAPManager>, IStoreListener
{
    private static IStoreController _mStoreController; // The Unity Purchasing system.
    private static IExtensionProvider _mStoreExtensionProvider; // The store-specific Purchasing subsystems.


    public string NoAds = "1no_ads";
    public string ColorMod = "com.guerinnigel.shapeselector.colormod";
    public Camera cam;
    
    private void Start()
    {
        // If we haven't set up the Unity Purchasing reference
        if (_mStoreController == null)
        {
            // Begin to configure our connection to Purchasing
            InitializePurchasing();
        }
    }

    private void InitializePurchasing()
    {
        // If we have already connected to Purchasing ...
        if (IsInitialized())
        {
            // ... we are done here.
            return;
        }

        // Create a builder, first passing in a suite of Unity provided stores.
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        // Continue adding the non-consumable product.
        builder.AddProduct(NoAds, ProductType.NonConsumable);
        builder.AddProduct(ColorMod, ProductType.NonConsumable);


        // Kick off the remainder of the set-up with an asynchronous call, passing the configuration 
        // and this class' instance. Expect a response either in OnInitialized or OnInitializeFailed.
        UnityPurchasing.Initialize(this, builder);
    }


    public static bool IsInitialized()
    {
        // Only say we are initialized if both the Purchasing references are set.
        return _mStoreController != null && _mStoreExtensionProvider != null;
    }

    public void BuyNoAds()
    {
        // Buy the non-consumable product using its general identifier. Expect a response either 
        // through ProcessPurchase or OnPurchaseFailed asynchronously.
        BuyProductID(NoAds);
    }

    public string GetProductPriceFromStore(string id)
    {
        return _mStoreController is {products: { }}
            ? _mStoreController.products.WithID(id).metadata.localizedPriceString
            : "";
    }

    private static void BuyProductID(string productId)
    {
        // If Purchasing has been initialized ...
        if (IsInitialized())
        {
            // ... look up the Product reference with the general product identifier and the Purchasing 
            // system's products collection.
            var product = _mStoreController.products.WithID(productId);

            // If the look up found a product for this device's store and that product is ready to be sold ... 
            if (product is {availableToPurchase: true})
            {
                Debug.Log($"Purchasing product asynchronously: '{product.definition.id}'");
                // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
                // asynchronously.
                _mStoreController.InitiatePurchase(product);
            }
            // Otherwise ...
            else
            {
                // ... report the product look-up failure situation  
                Debug.Log(
                    "BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        // Otherwise ...
        else
        {
            // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
            // retrying initialization.
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }
    
    //  
    // --- IStoreListener
    //

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        // Purchasing has succeeded initializing. Collect our Purchasing references.
        Debug.Log("OnInitialized: PASS");

        // Overall Purchasing system, configured with products for this application.
        _mStoreController = controller;
        // Store specific subsystem, for accessing device-specific store features.
        _mStoreExtensionProvider = extensions;
    }


    public void OnInitializeFailed(InitializationFailureReason error)
    {
        // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }


    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        //  a non-consumable product has been purchased by this user.
        if (string.Equals(args.purchasedProduct.definition.id, NoAds, StringComparison.Ordinal))
        {
            Debug.Log($"ProcessPurchase: PASS. Product: '{args.purchasedProduct.definition.id}'");
            GameManager.Instance.RemoveAds();
        }
        // Or ... an unknown product has been purchased by this user. Fill in additional products here....
        else
        {
            Debug.Log($"ProcessPurchase: FAIL. Unrecognized product: '{args.purchasedProduct.definition.id}'");
        }

        return PurchaseProcessingResult.Complete;
    }


    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
        // this reason with the user to guide their troubleshooting actions.
        Debug.Log(
            $"OnPurchaseFailed: FAIL. Product: '{product.definition.storeSpecificId}', PurchaseFailureReason: {failureReason}");
    }
    
    public void OnPurchaseComplete(Product product)
    {
        if (product.definition.id != ColorMod) return;
        Debug.Log("OnPurchaseComplete Testing");
        ColorChange();
    }

    private void ColorChange()
    {
        cam.backgroundColor = Color.red;
    }
    
}