using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Prime31;


public class InAppPurchases : MonoBehaviour {
	#if UNITY_IPHONE
	Dictionary<string, bool> IAPsOwned =  new Dictionary<string, bool>();
	private bool HasCheckedForPurchases = false;

	// Use this for initialization
	private List<StoreKitProduct> _products;	
	
	void Start()
	{
		// you cannot make any purchases until you have retrieved the products from the server with the requestProductData method
		// we will store the products locally so that we will know what is purchaseable and when we can purchase the products
		StoreKitManager.productListReceivedEvent += allProducts =>
		{
			Debug.Log( "received total products: " + allProducts.Count );
			_products = allProducts;
		};
		
		StoreKitManager.purchaseSuccessful += purchase_sucessful;
		
		IAPsOwned.Add("Story_1", PlayerPrefs.GetBool("Story_1", false));
		IAPsOwned.Add("Story_2", PlayerPrefs.GetBool("Story_2", false));
		IAPsOwned.Add("Story_3", PlayerPrefs.GetBool("Story_3", false));
		
	}
	
	// Update is called once per frame
	void Update () {
		if(HasCheckedForPurchases == false)
		{
			CheckPreviousPurchases();
		}
	}
	
	public void purchase_sucessful(StoreKitTransaction transaction)
    {   
		// we've recieved a transaction so we know our store connection is active
		HasCheckedForPurchases = true;		
		
		// See whether Dictionary contains this string.
		if (IAPsOwned.ContainsKey(transaction.productIdentifier))
		{
	   	 	IAPsOwned[transaction.productIdentifier] = true;
			PlayerPrefs.SetBool("transaction.productIdentifier", true);
	   			
		}else{
			Debug.Log("In App purchase not found: "  + transaction.productIdentifier);   
		}
		
        Debug.Log("Sucessful Purchase: " + transaction.productIdentifier);   
    }
	
	void CheckPreviousPurchases()
	{
		if(Application.internetReachability == NetworkReachability.NotReachable)
		{
			StoreKitBinding.restoreCompletedTransactions();				
		}
	}
	
	void OnGUI() {
		beginColumn();
		if (GUI.Button("Output IAP Status"))
		{
			foreach(KeyValuePair<string, bool> entry in IAPsOwned)
			{
				Debug.Log(entry.Key + ": " + entry.Value);
			}
			
		}
		
		if( GUILayout.Button( "Get Can Make Payments" ) )
		{
			bool canMakePayments = StoreKitBinding.canMakePayments();
			Debug.Log( "StoreKit canMakePayments: " + canMakePayments );
		}
		
		
		if( GUILayout.Button( "Get Product Data" ) )
		{
			// array of product ID's from iTunesConnect.  MUST match exactly what you have there!
			var productIdentifiers = new string[] { _products[0].productIdentifier };
			StoreKitBinding.requestProductData( productIdentifiers );
		}
		
		
		if( GUILayout.Button( "Restore Completed Transactions" ) )
		{
			StoreKitBinding.restoreCompletedTransactions();
		}

	
		endColumn( true );
	}
	#endif
}
