using System;
using StoreClassApp;
using System.Threading.Channels;
using Microsoft.VisualBasic;

namespace StoreController
{
        
    public class StoreInit
    {

        static void Main(string[] args)
        {

            StoreClass _store = new StoreClass();
            // intialize store values as list
            _store.initStore();
            getStoreResults(_store);
        }

        static void getStoreResults(StoreClass _store) {
            try {
                Console.WriteLine("please enter how many no of fern chair required");
                int itemCount=0;
                string? inputValue = Console.ReadLine();
                if (!int.TryParse(inputValue, out itemCount) || (int.TryParse(inputValue, out itemCount) && itemCount<=0))
                {
                    Console.WriteLine("Please enter no of boxes in number");
                    getStoreResults(_store);
                }
           
                int result=_store.getAvailableProductsFromStore(itemCount);
                // if out of stack retrying with another option
                if (result == 2) {
                    getStoreResults(_store);
                }
                Console.ReadLine();
                // if required to re-enter box count, we can enable this function
                // getStoreResults(_store);
            }
            catch (Exception e)
            {
                Console.WriteLine("Something went wrong, Please try again");
                getStoreResults(_store);
            }

        }
       
    }

}