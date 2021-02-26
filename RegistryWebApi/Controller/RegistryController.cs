using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace RegistryWebApi.Controller
{
    /// <summary>
    /// Registry controlls web pi calls to POST and GET data from AnimalFarm Database
    /// </summary>
    public class RegistryController : ApiController
    {
        // Get the Database connection string from Web.config file
        private string connectionString = ConfigurationManager.ConnectionStrings["AnimalFarmDatabase"].ConnectionString;

        /// <summary>
        /// Gets the data from Animals table from AnimalFarm database
        /// </summary>
        /// <param name="type">Parameter for Type column in database</param>
        /// <param name="group">Parameter for Group column in database</param>
        /// <returns>HttpResponse object</returns>
        [HttpGet]
        public HttpResponseMessage GetAnimals([FromUri] string type, string group)
        {
            #region Getting Parammeters from Request Uri
            // Check if request was sent for all types of Animals and pass the correct string value for SQL query
            string animalType = (type.Equals( "Animals") ? "%" : type);

            // Check if request was sent without a specified group of Animals and pass the correct string value for SQL query
            string animalGroup = (group ?? "%");
            #endregion

            #region Connecting to Database
            // Create connection to AnimalFarm database
            SqlConnection connection = new SqlConnection(connectionString);

            // Create SQL command object
            SqlCommand command = new SqlCommand();

            // Add quesry string to command with values of request uri parameters
            command.CommandText = "SELECT [Type], [Group], [Name] FROM Animals WHERE [Type] like @Type AND [Group] like @Group ORDER BY [Type], [Group] FOR JSON AUTO";
            command.Parameters.AddWithValue("@Type", animalType);
            command.Parameters.AddWithValue("@Group", animalGroup);
           
            // Connect command to the AnimalFarm database
            command.Connection = connection;
            #endregion

            #region Querying Data from Database
            // Open the connection to AnimalFarm database
            connection.Open();

            // Execute the command and read the response from database in JSON format
            SqlDataReader reader = command.ExecuteReader();

            // Create Stringbuilder object to add the databse response into
            StringBuilder jsonString = new StringBuilder();
            while (reader.Read())
            {
                // Read database JSON response into Stringbuilder object
                jsonString.Append(reader.GetValue(0).ToString());
            }

            // Close the dataReader object
            reader.Close();

            // Close the connection to AnimalFarm database
            connection.Close();
            #endregion

            #region Sending back the HttpResponse with JSON
            // Create HttpResponse object with Status: OK
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

            // Add JSON to the HttpResponse Content
            response.Content = new StringContent(jsonString.ToString(), UnicodeEncoding.UTF8, "application/json");
      
            /// Return the HttpResponse
            return response;
            #endregion
        }


        /// <summary>
        /// Post a new Animal data into the Animals table from AnimalFarm database
        /// </summary>
        [HttpPost]
        public async Task<OkResult> PostNewAnimal()
        {
            // Get the Content of HTTP request into a string 
            string requestContent = await Request.Content.ReadAsStringAsync();

            // Parse json string to JObject
            JObject jsonObject = JObject.Parse(requestContent);

            // Read values from JObject into strings
            string animalType = jsonObject["Type"].ToString();
            string animalGroup = jsonObject["Group"].ToString();
            string animalName = jsonObject["Name"].ToString();

            // Create connection to AnimalFarm Database
            SqlConnection connection = new SqlConnection(connectionString);
           
            // Create SQL command object
            SqlCommand command = new SqlCommand();

            // Add quesry string to command with values from http request body
            command.CommandText = "INSERT INTO Animals ([Type], [Group], [Name]) Values (@Type,@Group,@Name)";
            command.Parameters.AddWithValue("@Type", animalType);
            command.Parameters.AddWithValue("@Group", animalGroup);
            command.Parameters.AddWithValue("@Name", animalName);

            // Connect command to the AnimalFarm database
            command.Connection = connection;

            // Open connection to AnimalFarm databse
            connection.Open();

            // Execute command
            command.ExecuteNonQuery();

            // Close connection to AnimalFarm database
            connection.Close();
           
            // Resturn Ok result
            return Ok();
        }
    }
}