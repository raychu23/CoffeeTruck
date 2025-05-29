using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

/* How to use:
 * Add this script to a game object
 * Build a DataContainer object by adding key/value pairs.
 * Values may be strings or a list of DataContainers
 * 
			DataContainer dc = new DataContainer();
			dc.addPair("Player Name", "Player One");
			dc.addPair("Group Name", "Team Alpha");

			List<DataContainer> dclist = new List<DataContainer>();
			DataContainer innerdc = new DataContainer();
			innerdc.addPair("Weather", "Rainy");
			innerdc.addPair("Infostuff", "$600");
			innerdc.addPair("Hey", "Listen");
			dclist.Add(innerdc);

			innerdc = new DataContainer();
			innerdc.addPair("Weather", "Moar Rain");
			innerdc.addPair("Infostuff", "$1000");
			innerdc.addPair("Hey", "HEY");
			dclist.Add(innerdc);
			
			dc.addPair("DataPoints", dclist);
 * 
 * Then upload the data to the server:
 * DataUploader du = GetComponent<DataUploader>();
 * du.UploadData(dc);
 */

public class DataUploader : MonoBehaviour
{
	//private WWW servReq;
	//public int gameNum;

 //   public void UploadData(DataContainer data)
 //   {
 //       StartCoroutine("UploadDataRoutine", data);
 //   }

 //   public IEnumerator UploadDataRoutine(DataContainer data)
 //   {
	//	if (data != null)
	//	{
	//		//if (int.Parse(data.dataValues[16].Value.ToString()) == 0) {
			
	//			WWWForm numForm = new WWWForm();
	//			foreach (DataContainer.KeyValue kv in data.dataValues)
	//			{
	//				if (kv.Key == "PlayerID")
	//				{
	//					numForm.AddField("PlayerID", kv.Value.ToString());
	//				}
	//				else if (kv.Key == "GroupID")
	//				{
	//					numForm.AddField("GroupID", kv.Value.ToString());
	//				}
	//				else
	//				{
	//					break;
	//				}
	//			}

	//			//Fetch game number
	//			using (var www = UnityWebRequest.Post("https://stat2games.sites.grinnell.edu/php/getstatisticallygroundednum.php", numForm))
	//			{
	//				//Debug.Log("starting fetching game num");
	//				yield return www.SendWebRequest();
	//				//Debug.Log("fetched");
	//				if (www.isNetworkError || www.isHttpError)
	//				{
	//					Debug.Log("Fetching game number failed.  Error message: ");
	//				}
	//				else
	//				{
	//					gameNum = int.Parse(www.downloadHandler.text);
	//					Debug.Log("gamenum: " + gameNum);
	//				}
	//			}
	//		//} 
	//		WWWForm form = new WWWForm();
	//		foreach (SimArchive.ArchiveEntry entry in SimArchive.This().archiveList)
 //           {
	//			form.AddField("GameNum", gameNum);
	//			form.AddField("PlayerID", data.dataValues[0].Value.ToString());
 //               form.AddField("GroupID", data.dataValues[1].Value.ToString());
	//			if (designTypeAsBusinessPlan)
	//				dc.addPair("DesignType", "Business Plan");
	//			else
	//				dc.addPair("DesignType", DesignTypeStr(designType));
	//			dc.addPair("TotalTests", totalTests);

	//			dc.addPair("Day", entry.day);
	//			dc.addPair("Location", CategorySetup.cats[0].options[entry.options[0]].databaseText);
	//			dc.addPair("Music", CategorySetup.cats[1].options[entry.options[1]].databaseText);
	//			dc.addPair("Price", CategorySetup.cats[2].options[entry.options[2]].databaseText);
	//			dc.addPair("TimeOfDay", CategorySetup.cats[3].options[entry.options[3]].databaseText);
	//			dc.addPair("Weather", CategorySetup.cats[4].options[entry.options[4]].databaseText);
	//			dc.addPair("Temperature", entry.temp);
	//			dc.addPair("Sales", entry.sales);
	//			Debug.Log("dc sales: " + dc.dataValues[11].Value.ToString());
	//			dc.addPair("GrossIncome", entry.grossIncome);
	//			dc.addPair("Costs", entry.costs);
	//			dc.addPair("Profit", entry.profit);
	//			dc.addPair("CurrentMoney", entry.currentMoney);
	//		}
	//			//form.AddField("GameNum", gameNum);
	//			//form.AddField("PlayerID", data.dataValues[0].Value.ToString());
	//			//form.AddField("GroupID", data.dataValues[1].Value.ToString());
	//			//form.AddField("DesignType", data.dataValues[2].Value.ToString());
	//			//form.AddField("TotalTests", int.Parse(data.dataValues[3].Value.ToString()));
	//			//form.AddField("Day", int.Parse(data.dataValues[4].Value.ToString()));
	//			//form.AddField("Location", data.dataValues[5].Value.ToString());
	//			//form.AddField("Music", data.dataValues[6].Value.ToString());
	//			//form.AddField("Price", data.dataValues[7].Value.ToString());
	//			//form.AddField("TimeOfDay", data.dataValues[8].Value.ToString());
	//			//form.AddField("Weather", data.dataValues[9].Value.ToString());
	//			//form.AddField("Temperature", int.Parse(data.dataValues[10].Value.ToString()));
	//			//form.AddField("Sales", int.Parse(data.dataValues[11].Value.ToString()));
	//			//form.AddField("GrossIncome", data.dataValues[12].Value.ToString());
	//			//form.AddField("Costs", data.dataValues[13].Value.ToString());
	//			//form.AddField("Profit", data.dataValues[14].Value.ToString());
	//			//form.AddField("CurrentMoney", data.dataValues[15].Value.ToString());



	//			//foreach (DataContainer.KeyValue kv in data.dataValues) {
	//			//             if (kv.Value is List<DataContainer>) {
	//			//                 List<DataContainer> dclist = (List<DataContainer>)kv.Value;
	//			//                 foreach (DataContainer dc in dclist) {
	//			//                     form.AddField("subcols[]", kv.Key);
	//			//                     form.AddField("subentry[]", dc.ToString());
	//			//                 }
	//			//             }
	//			//             else
	//			//             {
	//			//		form.AddField("subcols[]", kv.Key);
	//			//		form.AddField("subentry[]", kv.Value.ToString());

	//			//	}
	//			//         }
	//			using (var www = UnityWebRequest.Post("https://stat2games.sites.grinnell.edu/php/sendstatisticallygroundedgameinfo.php", form))
	//		{

	//			yield return www.SendWebRequest();

	//			if (www.downloadHandler.text == "0")
	//			{
	//				Debug.Log("Player data created successfully.");
	//			}
	//			else
	//			{
	//				Debug.Log("Player data creation failed. Error # " + www.downloadHandler.text);
	//			}
	//		}
	//		//WWW servReq = new WWW(URLlocations.instance.GetServiceUrl(), form);

	//  //      StartCoroutine(WaitForRequest(servReq));
	//	}
 //   }
    
 //   //IEnumerator WaitForRequest(WWW www) {
 //   //	yield return www;
    	
 //   //	if (www.error == null) {
 //   //		Debug.Log("Submitted!" + www.text);
 //   //	} else {
 //   //		Debug.Log("FAILED: " + www.error);
 //   //	}
 //   //}
}


public class DataContainer {
	public class KeyValue {
		public string Key {get; set;}
		public object Value {get; set;}
		
		public KeyValue(string k, object v) {
			this.Key = k;
			this.Value = v;
		}
	}

	public List<KeyValue> dataValues;
	
	public DataContainer() {
		dataValues = new List<KeyValue>();
	}

    public void addPair(String key, String dataValue)
    {
        dataValues.Add(new KeyValue(key, dataValue));
    }

    public void addPair(String key, float dataValue)
    {
        dataValues.Add(new KeyValue(key, dataValue.ToString()));
    }

    public void addPair(String key, int dataValue)
    {
        dataValues.Add(new KeyValue(key, dataValue.ToString()));
    }

	public void addPair(String key, List<DataContainer> dataValue) {
		dataValues.Add(new KeyValue(key, dataValue));
	}
	
	public override string ToString() {
		string s = "{";
		
		bool first = true;
		foreach (KeyValue kv in dataValues) {
			if (first) {
				first = false;
			} else {
				s += ",";
			}
            if (kv.Value is List<DataContainer>) {
                s += "\""+kv.Key+"\":[\"Container\"]";
            } else {
                s += "\""+kv.Key+"\":\"" + kv.Value.ToString() + "\"";
            }
		}
		
		s += "}";
		
		return s;
	}
}
