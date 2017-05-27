using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyZillowApp;
using System.Xml;

namespace MyZillowApp.Tests
{
    [TestClass]
    public class MyZillowTest
    {

        // Test function getZillowXMLResult with valid encoded arguments
        // should return a string containing the correct address
        [TestMethod]
        public void getZillowXMLResult_WithValidArguments_returnsValidXML()
        {
            // arrange  
            string address = "1592+Ismail+Pl";
            string cityStateZIP = "Placentia%2C+CA+92870";
            string path = "http://www.zillow.com/webservice/GetSearchResults.htm";
            string zID = "X1-ZWz1dyb53fdhjf_6jziz";

            // act  
            string res = MyZillowApp._Default.getZillowXMLResult(path, zID, address, cityStateZIP, false);

            // assert  
            StringAssert.Contains(res, "1592 Ismail Pl");
        }

        // Test function getZillowXMLResult with empty address
        // should return a string containing the error message 'no address specified'
        [TestMethod]
        public void getZillowXMLResult_WithEmptyAddress_returnsErrorMessage()
        {
            // arrange  
            string address = "";
            string cityStateZIP = "Placentia, CA 92870";
            string path = "http://www.zillow.com/webservice/GetSearchResults.htm";
            string zID = "X1-ZWz1dyb53fdhjf_6jziz";

            // act  
            string res = MyZillowApp._Default.getZillowXMLResult(path, zID, address, cityStateZIP, false);

            // assert  
            StringAssert.Contains(res, "Error: no address specified");
        }


        // Test function parseXML2Object with valid XML as argument
        // should return a valid Object representation
        [TestMethod]
        public void parseXML2Object_WithValidArguments_returnsValidObject()
        {
            // arrange  
            string address = "";
            string cityStateZIP = "Placentia, CA 92870";
            string path = "http://www.zillow.com/webservice/GetSearchResults.htm";
            string zID = "X1-ZWz1dyb53fdhjf_6jziz";

            string xml = MyZillowApp._Default.getZillowXMLResult(path, zID, address, cityStateZIP, false);

            // act  
            var res = MyZillowApp._Default.parseXML2Object(xml);

            // assert  
            Assert.IsInstanceOfType(res, typeof(Object));
        }

        // Test function parseXML2Object with invalid XML as argument
        // should return an error exception
        [TestMethod]
        public void parseXML2Object_WithInvalidArguments_throwsException()
        {          
            string xml = "testing";

            // act 
            try
            {
                var res = MyZillowApp._Default.parseXML2Object(xml);
            }
            catch (Exception e)
            {
                // assert  
                StringAssert.Contains(e.Message, "error");
                return;
            }
            Assert.Fail("No exception was thrown.");
        }
    }
}
