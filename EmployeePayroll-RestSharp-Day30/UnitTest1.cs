// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnitTest1.cs" company="Bridgelabz">
//   Copyright � 2018 Company
// </copyright>
// <creator Name="Akash Kumar Singh"/>
// --------------------------------------------------------------------------------------------------------------------
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;

namespace EmployeePayroll_RestSharp_Day30
{
    [TestClass]
    public class UnitTest1
    {
        RestClient client;

        [TestInitialize]
        public void SetUp()
        {
            //Initialize the base URL to execute requests made by the instance
            client = new RestClient("http://localhost:4000");
        }
        private IRestResponse GetEmployeeList()
        {
            //Arrange
            //Initialize the request object with proper method and URL
            RestRequest request = new RestRequest("/Employees/list", Method.GET);
            //Act
            // Execute the request
            IRestResponse response = client.Execute(request);
            return response;
        }

        /// <summary>
        /// UC 1 : Retrieve all employee details in the json file
        /// </summary>
        [TestMethod]
        public void OnCallingGetAPI_ReturnEmployeeList()
        {
            IRestResponse response = GetEmployeeList();
            // Check if the status code of response equals the default code for the method requested
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            // Convert the response object to list of employees
            List<Employee> employeeList = JsonConvert.DeserializeObject<List<Employee>>(response.Content);
            Assert.AreEqual(8, employeeList.Count);
            foreach (Employee emp in employeeList)
            {
                Console.WriteLine("Id: " + emp.Id + "\t" + "Name: " + emp.Name + "\t" + "Salary: " + emp.Salary);
            }
        }
        /// <summary>
        /// UC 2 : Add new employee to the json file in JSON server and return the same
        /// </summary>
        [TestMethod]
        public void OnCallingPostAPI_ReturnEmployeeObject()
        {
            //Arrange
            //Initialize the request for POST to add new employee
            RestRequest request = new RestRequest("/Employees/list", Method.POST);
            JsonObject jsonObj = new JsonObject();
            jsonObj.Add("name", "Kobe Bryant");
            jsonObj.Add("salary", "100000");
            jsonObj.Add("id", "9");
            //Added parameters to the request object such as the content-type and attaching the jsonObj with the request
            request.AddParameter("application/json", jsonObj, ParameterType.RequestBody);

            //Act
            IRestResponse response = client.Execute(request);

            //Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Employee employee = JsonConvert.DeserializeObject<Employee>(response.Content);
            Assert.AreEqual("Kobe Bryant", employee.Name);
            Assert.AreEqual("100000", employee.Salary);
            Console.WriteLine(response.Content);
        }
        /// <summary>
        /// UC 3 : Adds multiple employees to the json file in JSON server and returns the same
        /// </summary>
        [TestMethod]
        public void OnCallingPostAPIForAEmployeeListWithMultipleEMployees_ReturnEmployeeObject()
        {
            // Arrange
            List<Employee> employeeList = new List<Employee>();
            employeeList.Add(new Employee { Name = "Lebron ", Salary = "85040" });
            employeeList.Add(new Employee { Name = "David ", Salary = "125030" });
            employeeList.Add(new Employee { Name = "Steph ", Salary = "125040" });
            //Iterate the loop for each employee
            foreach (var v in employeeList)
            {
                //Initialize the request for POST to add new employee
                RestRequest request = new RestRequest("/Employees/list", Method.POST);
                JsonObject jsonObj = new JsonObject();
                jsonObj.Add("Name", v.Name);
                jsonObj.Add("Salary", v.Salary);
               // jsonObj.Add("Id", v.Id);
                //Added parameters to the request object such as the content-type and attaching the jsonObj with the request
                request.AddParameter("application/json", jsonObj, ParameterType.RequestBody);

                //Act
                IRestResponse response = client.Execute(request);

                //Assert
                Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
                Employee employee = JsonConvert.DeserializeObject<Employee>(response.Content);
                Assert.AreEqual(v.Name, employee.Name);
                Assert.AreEqual(v.Salary, employee.Salary);
                Console.WriteLine(response.Content);
            }
        }
        /// <summary>
        /// UC 4 : Update the salary into the json file in json server
        /// </summary>
        [TestMethod]
        public void OnCallingPutAPI_ReturnEmployeeObject()
        {
            //Arrange
            //Initialize the request for PUT to add new employee
            RestRequest request = new RestRequest("/Employees/8", Method.PUT);
            JsonObject jsonObj = new JsonObject();
            jsonObj.Add("name", "Paulo Dybala");
            jsonObj.Add("salary", "95000");
            //Added parameters to the request object such as the content-type and attaching the jsonObj with the request
            request.AddParameter("application/json", jsonObj, ParameterType.RequestBody);

            //Act
            IRestResponse response = client.Execute(request);

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Employee employee = JsonConvert.DeserializeObject<Employee>(response.Content);
            Assert.AreEqual("Paulo Dybala", employee.Name);
            Assert.AreEqual("95000", employee.Salary);
            Console.WriteLine(response.Content);
        }
        /// <summary>
        /// UC 5 : Delete the employee details with given id
        /// </summary>
        [TestMethod]
        public void OnCallingDeleteAPI_ReturnSuccessStatus()
        {
            //Arrange
            //Initialize the request for PUT to add new employee
            RestRequest request = new RestRequest("/Employees/7", Method.DELETE);

            //Act
            IRestResponse response = client.Execute(request);

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Console.WriteLine(response.Content);
        }
    }
}
