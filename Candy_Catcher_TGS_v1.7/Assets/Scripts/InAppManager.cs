using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using OnePF; //namespace OnePF is used to access functionality related to InApp purchase

public class InAppManager : MonoBehaviour {

	public static InAppManager instance;

	string _label = "";
	bool _isInitialized = false;
	public static bool _purchaseDone = false;
	private bool _processingPayment = false;
	Inventory _inventory = null;

	private const string STORE_ONEPF = "org.onepf.store";
	const string SKU = "css";
	//const string SKU_AdRemove = "com.fss.removead";

	private void Start()
	{
		instance = this;
		//OpenIAB.mapSku (SKU, OpenIAB_iOS.STORE, "fbss");
		OpenIAB.mapSku( PluginManager._insta.inAppRemoveAdID, OpenIAB_iOS.STORE, PluginManager._insta.inAppRemoveAdID );
		OpenIAB.mapSku( PluginManager._insta.inAppRemoveAdID, OpenIAB_Android.STORE_GOOGLE, PluginManager._insta.inAppRemoveAdID );

		var options = new Options();
		options.checkInventoryTimeoutMs = Options.INVENTORY_CHECK_TIMEOUT_MS * 2;
		options.discoveryTimeoutMs = Options.DISCOVER_TIMEOUT_MS * 2;
		options.checkInventory = false;
		options.verifyMode = OptionsVerifyMode.VERIFY_SKIP;
		options.storeKeys = new Dictionary<string, string> { {OpenIAB_Android.STORE_GOOGLE, PluginManager._insta.publicKeyAndroid} };
		OpenIAB.init( options );
	}
	public void OnEnable()
	{
		OpenIABEventManager.billingSupportedEvent += billingSupportedEvent;
		OpenIABEventManager.billingNotSupportedEvent += billingNotSupportedEvent;
		OpenIABEventManager.queryInventorySucceededEvent += queryInventorySucceededEvent;
		OpenIABEventManager.queryInventoryFailedEvent += queryInventoryFailedEvent;
		OpenIABEventManager.purchaseSucceededEvent += purchaseSucceededEvent;
		OpenIABEventManager.purchaseSucceededEvent += OnPurchaseSucceded;
		OpenIABEventManager.purchaseFailedEvent += purchaseFailedEvent;
		OpenIABEventManager.consumePurchaseSucceededEvent += consumePurchaseSucceededEvent;
		OpenIABEventManager.consumePurchaseFailedEvent += consumePurchaseFailedEvent;
		OpenIABEventManager.transactionRestoredEvent += transactionRestoredEvent;
	}
	public void OnDisable()
	{
		OpenIABEventManager.billingSupportedEvent -= billingSupportedEvent;
		OpenIABEventManager.billingNotSupportedEvent -= billingNotSupportedEvent;
		OpenIABEventManager.queryInventorySucceededEvent -= queryInventorySucceededEvent;
		OpenIABEventManager.queryInventoryFailedEvent -= queryInventoryFailedEvent;
		OpenIABEventManager.purchaseSucceededEvent -= purchaseSucceededEvent;
		OpenIABEventManager.purchaseFailedEvent -= purchaseFailedEvent;
		OpenIABEventManager.consumePurchaseSucceededEvent -= consumePurchaseSucceededEvent;
		OpenIABEventManager.consumePurchaseFailedEvent -= consumePurchaseFailedEvent;
		OpenIABEventManager.purchaseSucceededEvent -= OnPurchaseSucceded;
		OpenIABEventManager.transactionRestoredEvent -= transactionRestoredEvent;
	}

	private void OnPurchaseSucceded(Purchase purchase)
	{
		Debug.Log ("Purchase Succeeded - "+ purchase.Sku + " and " + PluginManager._insta.inAppRemoveAdID);
		PlayerPrefs.SetInt ("InApp",1);
		PlayerPrefs.Save ();
		_purchaseDone = true;
		Debug.Log ("Working");

		if (purchase.Sku.Equals (PluginManager._insta.inAppRemoveAdID))
		{
			Debug.Log ("Ad Removed");
			PlayerPrefs.SetInt ("removeAd", 1);
			PlayerPrefs.Save ();
			PluginManager._insta.destroyAds ();
			//GameObject.Find ("Canvas").transform.Find ("Panel").transform.Find ("Buttons").transform.Find ("RemoveAdBtn").gameObject.SetActive (false);
			//GameObject.Find ("Canvas").transform.Find ("Panel").transform.Find ("AdRemoved").gameObject.SetActive (true);

			if(Application.loadedLevelName.Equals("Home_scene")){ //msg show only if its home screen
				HomeManager._insta.RemoveAdBtn.SetActive(false);
				HomeManager._insta.myMsg = "Ad Remove Successfully";
				HomeManager._insta.StartCoroutine (HomeManager._insta.ControllerSelection ());
			}

		}
		OpenIAB.consumeProduct(purchase);
		_processingPayment = false;
	}
	public void PurchaseProduct(string good)
	{
		//Debug.Log ("SuccessFull");
		_purchaseDone = false;
		OpenIAB.purchaseProduct(good);
	}
	public void transactionRestoredEvent(string itemName)
	{
		if (itemName.Equals (PluginManager._insta.inAppRemoveAdID))
		{
			Debug.Log ("Ad Removed");
			PlayerPrefs.SetInt ("removeAd", 1);
			PlayerPrefs.Save ();
			PluginManager._insta.destroyAds ();
			//GameObject.Find ("Canvas").transform.Find ("Panel").transform.Find ("Buttons").transform.Find ("RemoveAdBtn").gameObject.SetActive (false);
			if(Application.loadedLevelName.Equals("Home_scene")){ //msg show only if its home screen
				HomeManager._insta.RemoveAdBtn.SetActive(false);
				HomeManager._insta.myMsg = "Ad Remove Successfully";
				HomeManager._insta.StartCoroutine (HomeManager._insta.ControllerSelection ());
			}
			Debug.Log("Ad Removed Restore: " + itemName);
		}
		Debug.Log ("Restore item " + itemName);
	}
	public void restorePurchase()
	{
		Debug.Log ("restore");
		OpenIAB.restoreTransactions ();
	}
	private void OnPurchaseFailed(string error) 
	{
		Debug.Log("Purchase failed: " + error);
		_processingPayment = false;
	}
	private void OnTransactionRestored(string sku) {
		Debug.Log("Transaction restored: " + sku);
	}

	private void OnRestoreSucceeded() {
		Debug.Log("Transactions restored successfully");
	}

	private void OnRestoreFailed(string error) {
		Debug.Log("Transaction restore failed: " + error);
	}
	//default methods
	private void billingSupportedEvent()
	{
		_isInitialized = true;
		Debug.Log("billingSupportedEvent");
	}
	private void billingNotSupportedEvent(string error)
	{
		Debug.Log("billingNotSupportedEvent: " + error);
	}
	private void queryInventorySucceededEvent(Inventory inventory)
	{
		Debug.Log("queryInventorySucceededEvent: " + inventory);
		if (inventory != null)
		{
			_label = inventory.ToString();
			_inventory = inventory;
		}
	}
	private void queryInventoryFailedEvent(string error)
	{
		Debug.Log("queryInventoryFailedEvent: " + error);
		_label = error;
	}
	private void purchaseSucceededEvent(Purchase purchase)
	{
		Debug.Log("purchaseSucceededEvent: " + purchase);
		_label = "PURCHASED:" + purchase.ToString();
	}
	private void purchaseFailedEvent(int errorCode, string errorMessage)
	{
		Debug.Log("purchaseFailedEvent: " + errorMessage);
		_label = "Purchase Failed: " + errorMessage;
	}
	private void consumePurchaseSucceededEvent(Purchase purchase)
	{
		Debug.Log("consumePurchaseSucceededEvent: " + purchase);
		_label = "CONSUMED: " + purchase.ToString();
	}
	private void consumePurchaseFailedEvent(string error)
	{
		Debug.Log("consumePurchaseFailedEvent: " + error);
		_label = "Consume Failed: " + error;
	}
}
