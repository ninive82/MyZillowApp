using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Configuration;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace MyZillowApp
{
    public partial class _Default : Page
    {
        static HttpClient client = new HttpClient();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            clearForm();
            try
            {
                string address = AddressInput.Text.Trim();
                string city = CityInput.Text.Trim();
                string state = StateInput.Text.Trim();
                string zip = ZIPInput.Text.Trim();
                bool getZestimate = ShowRentZestimate.Checked;
                string path = ConfigurationManager.AppSettings["ZillowPath"];
                string zID = ConfigurationManager.AppSettings["ZillowKey"];


                address = Server.UrlEncode(address);
                string citystatezip = Server.UrlEncode(city + ", " + state + " " + zip);

                string resultsXML = getZillowXMLResult(path, zID, address, citystatezip, getZestimate);
                if (resultsXML != "")
                {
                    searchresults myResults = parseXML2Object(resultsXML);
                    displayResults(myResults);
                }
            }
            catch (Exception ex)
            {
                LabelError.Text = ex.Message;
            }
        }

        /// <summary>
        /// Calls the zillow API https://www.zillow.com/howto/api/GetSearchResults.htm
        /// </summary>
        /// <param name="address">The string address of the property to search.</param>
        /// <param name="citystatezip">The string city+state combination and/or ZIP code for which to search</param>
        /// <param name="showZestimate">Bool Return Rent Zestimate information if available </param>
        /// <returns>XML representation of this property</returns>
        public static string getZillowXMLResult(string path, string zID, string address, string citystatezip, bool showZestimate)
        {

            HttpResponseMessage resp = client.GetAsync(string.Format("{0}?zws-id={1}&address={2}&citystatezip={3}&rentzestimate={4}",
                                                       path, zID, address, citystatezip, showZestimate)).Result;

            string result = resp.Content.ReadAsStringAsync().Result;
            return result;
        }

        /// <summary>
        /// Converts the parameter xml to object of type searchresults
        /// </summary>
        /// <param name="xml">the string in XML format to convert to object</param>
        /// <returns>searchResults object</returns>
        public static searchresults parseXML2Object(string xml)
        {
            if (xml != "")
            {
                return (searchresults)XmlDeserializeFromString(xml, typeof(searchresults));
            }
            else { return null; }
        }

        /// <summary>
        /// Converts the string objectdata into type object with the format of type 
        /// </summary>
        /// <param name="objectData"> the string in XML format to convert</param>
        /// <param name="type"> the Type of the Object to return</param>
        /// <returns></returns>
        private static object XmlDeserializeFromString(string objectData, Type type)
        {
            var serializer = new XmlSerializer(type);
            object result;
            using (TextReader reader = new StringReader(objectData))
            {
                result = serializer.Deserialize(reader);
            }
            return result;
        }

        /// <summary>
        /// Displays in the web form the infromation saved in the searchresults instance results
        /// </summary>
        /// <param name="results">searchresults instance to display</param>
        protected void displayResults(searchresults results)
        {
            if (results != null)
            {
                if (results.message.code == 0)
                {
                    ResultsSection.Visible = true;
                    responseResultsResult resp = results.response.results.result;
                    LabelPropertyID.Text = resp.zpid.ToString();
                    LinkHomeDetails.NavigateUrl = resp.links.homedetails;
                    LinkChartData.NavigateUrl = resp.links.graphsanddata;
                    LinkMap.NavigateUrl = resp.links.mapthishome;
                    LinkSimilarSales.NavigateUrl = resp.links.comparables;

                    LabelStreet.Text = resp.address.street;
                    LabelCity.Text = resp.address.city;
                    LabelState.Text = resp.address.state;
                    LabelZip.Text = resp.address.zipcode.ToString();
                    LabelLatitude.Text = resp.address.latitude.ToString();
                    LabelLongitude.Text = resp.address.longitude.ToString();

                    LabelZestimate.Text = resp.zestimate.amount.Value.ToString();
                    LabelLastUpdated.Text = resp.zestimate.lastupdated;
                    Label30DayChange.Text = resp.zestimate.valueChange.Value.ToString();//check is in 30 days
                    LabelValuationRangeHigh.Text = resp.zestimate.valuationRange.high.Value.ToString();//check is in dollars
                    LabelValuationRangeLow.Text = resp.zestimate.valuationRange.low.Value.ToString();

                    if (ShowRentZestimate.Checked)
                    {
                        RentSection.Visible = true;
                        LabelRentZestimate.Text = resp.rentzestimate.amount.Value.ToString();
                        LabelRentLastUpdated.Text = resp.rentzestimate.lastupdated.ToString();
                        LabelRent30DayChange.Text = resp.rentzestimate.valueChange.Value.ToString();//check is for 30 days
                        LabelRentValuationRangeHigh.Text = resp.rentzestimate.valuationRange.high.Value.ToString();
                        LabelRentValuationRangeLow.Text = resp.rentzestimate.valuationRange.low.Value.ToString();
                    }
                    else { RentSection.Visible = false; }

                    LabelHomeValueIndex.Text = resp.localRealEstate.region.zindexValue;
                    LinkRegionOverview.NavigateUrl = resp.localRealEstate.region.links.overview;
                    LinkForSaleByOwnerHomes.NavigateUrl = resp.localRealEstate.region.links.forSaleByOwner;
                    LinkForSaleHomes.NavigateUrl = resp.localRealEstate.region.links.forSale;
                }
                else
                {
                    LabelError.Text = results.message.text;
                }
            }
        }
        /// <summary>
        /// Clears all labels and link values in the web form for the next load
        /// it also hides ResultsSection to clear the page
        /// </summary>
        protected void clearForm()
        {
            ResultsSection.Visible = false;
            LabelPropertyID.Text = "";
            LinkHomeDetails.NavigateUrl = "";
            LinkChartData.NavigateUrl = "";
            LinkMap.NavigateUrl = "";
            LinkSimilarSales.NavigateUrl = "";

            LabelStreet.Text = "";
            LabelCity.Text = "";
            LabelState.Text = "";
            LabelZip.Text = "";
            LabelLatitude.Text = "";
            LabelLongitude.Text = "";

            LabelZestimate.Text = "";
            LabelLastUpdated.Text = "";
            Label30DayChange.Text = "";
            LabelValuationRangeHigh.Text = "";
            LabelValuationRangeLow.Text = "";

            RentSection.Visible = false;
            LabelRentZestimate.Text = "";
            LabelRentLastUpdated.Text = "";
            LabelRent30DayChange.Text = "";
            LabelRentValuationRangeHigh.Text = "";
            LabelRentValuationRangeLow.Text = "";

            LabelHomeValueIndex.Text = "";
            LinkRegionOverview.NavigateUrl = "";
            LinkForSaleByOwnerHomes.NavigateUrl = "";
            LinkForSaleHomes.NavigateUrl = "";

            LabelError.Text = "";
        }
    }
}