using System;
using static Store;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using System.Linq;
using System.Xml.Linq;

namespace StoreClassApp
{
	public class StoreClass
	{
        // list of store with available components
        List<Store> storeList = new List<Store>();
        //output box stores
        List<Box> storeBoxList = new List<Box>();
        // default components for each box
        int requiredScrews = 10;
        int requiredWheels = 4;
        int requiredArambars = 2;
        int requirednuts = 30;
        public void initStore()
		{

            
            Store _store1 = new Store
            {
                storeName = "Store01",
                screws = 20,
                wheels = 8,
                arambars = 4,
                nuts = 20,
            };

            Store _store2 = new Store
            {
                storeName = "Store02",
                screws = 100,
                wheels = 100,
                arambars = 100,
                nuts = 20,
            };

            Store _store3 = new Store
            {
                storeName = "Store03",
                screws = 2000,
                wheels = 200,
                arambars = 100,
                nuts = 1000,
            };
            storeList.Add(_store1);
            storeList.Add(_store2);
            storeList.Add(_store3);
            // json = JsonConvert.SerializeObject(storeList);

            
        }

        public int getAvailableProductsFromStore(int itemCount)
        {
            try {
                int status=0;
                for (var box=0; box < itemCount; box++) {
                    Box _box = new Box
                    {
                        boxName = "BoxFern"+(box+1),
                        screws = new List<Components>(),
                        wheels = new List<Components>(),
                        arambars = new List<Components>(),
                        nuts = new List<Components>(),
                    };
                    storeBoxList.Add(_box);
                    foreach (var (store, index) in storeList.Select((v, i) => (v, i)))
                    {
                        // screws
                        Components? _screws =getValues(store, "screws", requiredScrews, index, box);
                        if (_screws != null && _screws.count>0) {
                            storeBoxList[box].screws?.Add(_screws);
                        }

                        // wheels
                        Components? _wheels = getValues(store, "wheels", requiredWheels, index, box);
                        if (_wheels != null && _wheels.count > 0)
                        {
                            storeBoxList[box].wheels?.Add(_wheels);
                        }


                        // arambars
                        Components? _arambars = getValues(store, "arambars", requiredArambars, index, box);
                        if (_arambars != null && _arambars.count > 0)
                        {
                            storeBoxList[box].arambars?.Add(_arambars);
                        }


                        // nuts
                        Components? _nuts = getValues(store, "nuts", requirednuts, index, box);
                        if (_nuts != null && _nuts.count > 0)
                        {
                            storeBoxList[box].nuts?.Add(_nuts);
                        }
                   



                    }

                 
                }


                foreach(var box in storeBoxList){
                    // box name
                    Console.WriteLine(box.boxName);
                    // if any of the stack not available in any store, print out of stack message
                    if(box.screws?.Count==0 || box.wheels?.Count==0 || box.arambars?.Count==0 || box.nuts?.Count == 0){
                        Console.WriteLine("Out of stack");
                        break;
                    }
                    else {
                        // printing screws stores and components
                        foreach (Components item in box.screws)
                        {
                            Console.WriteLine(item.storeName + " | " + "screws=" + item.count);
                        }
                        // printing wheers stores and components
                        foreach (var item in box.wheels)
                        {
                            Console.WriteLine(item.storeName + " | " + "wheels=" + item.count);
                        }
                        // printing arambars stores and components
                        foreach (var item in box.arambars)
                        {
                            Console.WriteLine(item.storeName + " | " + "arambars=" + item.count);
                        }
                        // printing nuts stores and components
                        foreach (var item in box.nuts)
                        {
                            Console.WriteLine(item.storeName + " | " + "nuts=" + item.count);
                        }
                    }

                    
                }

                if (storeBoxList.Count > 0) {
                    status = 1;
                }
                else {
                    status = 2;
                    // Console.WriteLine("Out of stack");
                }


                return status;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return 0;
            }


        }

        public Components? getValues(Store store, string propName,int requiredValue,int i,int boxIndex) {
            try {
            
                int limitValue = 0;
                // get the component property store items
                int storeItems= (int)store.GetType().GetProperty(propName).GetValue(store, null);
                int count = 0;
                int total = 0;

                // each components current boxing itemas
                if (propName== "screws" && storeBoxList[boxIndex].screws != null) {
                    total = storeBoxList[boxIndex].screws.Sum(x => x.count);
                    requiredValue = requiredValue - total;
                }

                if (propName == "wheels" && storeBoxList[boxIndex].wheels != null)
                {
                    total = storeBoxList[boxIndex].wheels.Sum(x => x.count);
                    requiredValue = requiredValue - total;
                }

                if (propName == "arambars" && storeBoxList[boxIndex].arambars != null)
                {
                    total = storeBoxList[boxIndex].arambars.Sum(x => x.count);
                    requiredValue = requiredValue - total;
                }

                if (propName == "nuts" && storeBoxList[boxIndex].nuts != null)
                {
                    total = storeBoxList[boxIndex].nuts.Sum(x => x.count);
                    requiredValue = requiredValue - total;
                }

                if (total == requiredValue) {
                    count = 0;
                 }

               /* if(requiredValue== total) {
                    Console.WriteLine("Required Items not available");
                    return null;
                }*/
               // if store items greate than required box items directly will detect from store
                if (storeItems >= requiredValue) {
                        limitValue = storeItems - requiredValue;
                    count = requiredValue;
                    store.GetType().GetProperty(propName)?.SetValue(store, limitValue); 
                    
                }
                // if store items greate than 0 and less than required value-> detect all store items from store
                else if(storeItems>0 && storeItems<= requiredValue) {
                    count = storeItems;
                    store.GetType().GetProperty(propName)?.SetValue(store, 0);

                }
                // if store items 0 or other directly returing null
                else {
                    return null;
                }
                    // creating store name and components count 
                    Components _counts = new Components
                    {
                    storeName = store.storeName,
                    count = count
                };

                // returning final output
                return _counts;
            }
            catch (Exception e)
            {
              

                throw;

            }


        }
	}
}

