using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;

namespace AddressBookSystem_RESTSharp
{
    [TestClass]
    public class UnitTest1
    {
        RestClient client;

        [TestInitialize]
        public void SetUp()
        {
            //Initialize the base URL to execute requests made by the instance
            client = new RestClient("http://localhost:3000");
        }
        private IRestResponse GetContactList()
        {
            //Arrange
            //Initialize the request object with proper method and URL
            RestRequest request = new RestRequest("/Contacts", Method.GET);
            //Act
            // Execute the request
            RestSharp.IRestResponse response = client.Execute(request);
            return response;
        }
       
         /* UC22:- Ability to Read Entries of Address Book from JSONServer.
                   - Use RESTSharp for REST Api Calls from MSTest Test Code.
                   - Update Address Book Memory of the Data retrieved from JSON Server.
         */
       
        [TestMethod]
        public void ReadEntriesFromJsonServer()
        {
            IRestResponse response = GetContactList();
            // Check if the status code of response equals the default code for the method requested
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            // Convert the response object to list of employees
            List<Contact> employeeList = JsonConvert.DeserializeObject<List<Contact>>(response.Content);
            Assert.AreEqual(2, employeeList.Count);
            foreach (Contact c in employeeList)
            {
                Console.WriteLine($"Id:- {c.Id}\tFullName:- {c.FirstName} {c.LastName}\tPhoneNumber:- {c.PhoneNumber}\tAddress:- {c.Address}\tCity:- {c.City}\tState:- {c.State}\tZip:- {c.Zip}\tEmail:- {c.Email}");
            }
        }
        /*UC23:- Ability to Add Multiple Entries to Address Book JSONServer and sync with Address Book Application Memory.
                 - Use RESTSharp for REST Api Calls from MSTest Test Code
         */

        [TestMethod]
        public void OnCallingPostAPIForAContactListWithMultipleContacts_ReturnContactObject()
        {
            // Arrange
            List<Contact> contactList = new List<Contact>();
            contactList.Add(new Contact { FirstName = "Priyanshu", LastName = "PataNahi", PhoneNumber = "789456123", Address = "UPWale", City = "PataNahi", State = "UP", Zip = "789554", Email = "ps@gmail.com" });
            contactList.Add(new Contact { FirstName = "Vishal", LastName = "MalumNahi", PhoneNumber = "321654987", Address = "Mumbai", City = "Mumbai", State = "Maharashtra", Zip = "442206", Email = "vs@gmail.com" });
            contactList.Add(new Contact { FirstName = "Ekta", LastName = "Kumbhare", PhoneNumber = "123456987", Address = "Bajaj Nagar", City = "Pune", State = "Maharashtra", Zip = "442203", Email = "ek@gmail.com" });

            //Iterate the loop for each contact
            foreach (var v in contactList)
            {
                //Initialize the request for POST to add new contact
                RestRequest request = new RestRequest("/Contacts", Method.POST);
                JsonObject jsonObj = new JsonObject();
                jsonObj.Add("firstname", v.FirstName);
                jsonObj.Add("lastname", v.LastName);
                jsonObj.Add("PhoneNumber", v.PhoneNumber);
                jsonObj.Add("address", v.Address);
                jsonObj.Add("city", v.City);
                jsonObj.Add("state", v.State);
                jsonObj.Add("zip", v.Zip);
                jsonObj.Add("email", v.Email);

                //Added parameters to the request object such as the content-type and attaching the jsonObj with the request
                request.AddParameter("application/json", jsonObj, ParameterType.RequestBody);

                //Act
                IRestResponse response = client.Execute(request);

                //Assert
                Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
                Contact contact = JsonConvert.DeserializeObject<Contact>(response.Content);
                Assert.AreEqual(v.FirstName, contact.FirstName);
                Assert.AreEqual(v.LastName, contact.LastName);
                Assert.AreEqual(v.PhoneNumber, contact.PhoneNumber);
                Console.WriteLine(response.Content);
            }
        }


        /* UC24:- Ability to Update Entry in Address Book JSONServer and sync with Address Book Application Memory. 
                  - Use RESTSharp for REST Api Calls from MSTest Test Code.
        */
        
        [TestMethod]
        public void OnCallingPutAPI_ReturnContactObjects()
        {
            //Arrange
            //Initialize the request for PUT to add new employee
            RestRequest request = new RestRequest("/Contacts/4", Method.PUT);
            JsonObject jsonObj = new JsonObject();
            jsonObj.Add("firstname", "TereNam");
            jsonObj.Add("lastname", "Gela");
            jsonObj.Add("PhoneNumber", "4455669988");
            jsonObj.Add("address", "Mela");
            jsonObj.Add("city", "Mumbai");
            jsonObj.Add("state", "Maharashtra");
            jsonObj.Add("zip", "442205");
            jsonObj.Add("email", "terenam@gmail.com");
            //Added parameters to the request object such as the content-type and attaching the jsonObj with the request
            request.AddParameter("application/json", jsonObj, ParameterType.RequestBody);

            //Act
            IRestResponse response = client.Execute(request);

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Contact contact = JsonConvert.DeserializeObject<Contact>(response.Content);
            Assert.AreEqual("TereNam", contact.FirstName);
            Assert.AreEqual("Gela", contact.LastName);
            Assert.AreEqual("442205", contact.Zip);
            Console.WriteLine(response.Content);
        }

        /*UC25:- Ability to Delete Entry in Address Book JSONServer and sync with Address Book Application Memory.
                 - Use RESTSharp for REST Api Calls from MSTest Test Code.
         */
        [TestMethod]
        public void OnCallingDeleteAPI_ReturnSuccessStatus()
        {
            //Arrange
            //Initialize the request for PUT to add new employee
            RestRequest request = new RestRequest("/Contacts/4", Method.DELETE);

            //Act
            IRestResponse response = client.Execute(request);

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Console.WriteLine(response.Content);
        }
    }
}
