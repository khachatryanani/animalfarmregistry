using Creatures.Entities;
using Creatures.Types;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;


namespace FarmRegistry
{
    /// <summary>
    /// Main window for AnimalFarm database manipulations
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initialize the main window on start
        /// </summary>
        public MainWindow()
        {
            // Initialize the main components of the window
            InitializeComponent();

            // Set the source of Animal Groups Combobox the Mammual Group Enum object
            AnimalGroupToCreateCombo.ItemsSource = System.Enum.GetValues(typeof(MammualGroup));

            // The the first item in list as selected
            AnimalGroupToCreateCombo.SelectedIndex = 0;

        }

        /// <summary>
        /// Grabds values from specified window controls and generate Http Post request to Registry WebApi
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">Button click event</param>
        private async void AddAnimalButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            #region Creating a new Animal Object
            // Get the animal type as a string from Combo box selected item
            string selectedAnimalType = AnimalTypeToCreateCombo.Text;

            // Parse animal type string to Animal Type enum value
            AnimalType animalType = (AnimalType)Enum.Parse(typeof(AnimalType), selectedAnimalType);


            // Get the animal group as a string from Combo box selected item
            string selectedAnimalGroup = AnimalGroupToCreateCombo.Text;

            // Parse animal group string to Animal Group enum value
            AnimalGroup animalGroup = (AnimalGroup)Enum.Parse(typeof(AnimalGroup), selectedAnimalGroup);

            // Get animal name string from Textbox text
            string animalName = AnimalNameBox.Text;


            // Create an Animal Object to be sent to database
            Animal animalToAdd = new Animal(animalType, animalGroup, animalName);
            #endregion

            #region Sending Http Request with JSON string
            // Convert Animal object to JSON string
            var animalJson = JsonConvert.SerializeObject(animalToAdd);

            // Get WebApi HttpPost Request uri from App.config
            string webApiPost = ConfigurationManager.AppSettings.Get("WebApiPost");

            // Create a Uri object
            Uri httpRequestUri = new Uri(webApiPost);

            // Create an Http Client object to manage the request
            using (HttpClient client = new HttpClient())
            {
                // Try sending request to Web Api
                try
                {
                    // Create a HttpRequest object and add request properties
                    HttpRequestMessage request = new HttpRequestMessage()
                    {
                        Method = HttpMethod.Post,
                        RequestUri = httpRequestUri,
                        Content = new StringContent(animalJson, UnicodeEncoding.UTF8, "application/json")
                    };

                    // Send a [HttpPost] request and get the response
                    HttpResponseMessage response = await client.SendAsync(request);

                    // Check if http response was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Display message to user for successful completion
                        MessageBox.Show($"Succefully completed. Status Code:{response.StatusCode.ToString()}",
                            "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        // Display message to user for invalid input
                        MessageBox.Show($"Unable to complete the request: {response.StatusCode.ToString()}",
                            "Errror Message", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (HttpRequestException exception)
                {
                    // Show exception message in pop up window
                    MessageBox.Show(exception.Message, "Error Message", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            #endregion
        }

        /// <summary>
        /// Grabds values from specified window controls and generate Http Get request to Registry WebApi
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">Button click event</param>
        private async void DisplayButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            // Get animal type and group values from respective Comboboxes
            string animalType = DisplayAnimalsTypeCombo.Text;
            string animalGroup = DisplayAnimalGroupCombo.Text;

            // Get WebApi HttpGet Request uri from App.config
            string webApiGet = ConfigurationManager.AppSettings.Get("WebApiGet");

            // Create a Uri object with full Uri with query parameters
            Uri requestUri = new Uri(String.Concat(webApiGet, "?type=", animalType, "&group=", animalGroup));

            // Create an Http Client object to manage the request
            using (HttpClient client = new HttpClient())
            {
                // Try sending request to Web Api
                try
                {
                    // Submit a [GET] Request and get the response
                    HttpResponseMessage response = await client.GetAsync(requestUri);

                    // Check if http response was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Display message to user for successful completion
                        MessageBox.Show($"Status Code: {response.StatusCode.ToString()}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                        // Read response JSON string from httpReponse object content
                        string responseString = await response.Content.ReadAsStringAsync();

                        // Convert the JSON string to a list of Animal Objects
                        List<Animal> animalsToDisplay = JsonConvert.DeserializeObject<List<Animal>>(responseString);

                        // Bind the Animal objects list to the main ListView control
                        DisplayAllListView.ItemsSource = animalsToDisplay;
                    }
                    else
                    {
                        // Display message to user for invalid input
                        MessageBox.Show($"Unable to complete the request: {response.StatusCode.ToString()}",
                            "Errror Message", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (HttpRequestException exception)
                {
                    // Show exception message in pop up window
                    MessageBox.Show(exception.Message, "Error Message", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Invokes on Type Combobox Selection Changed event and 
        //bind the Group Combobox source to resposnective source
        /// <param name="sender">Combobox for adding new animal</param>
        /// <param name="e">Combobox selection changed event</param>
        private void AnimalTypeToCreateCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // On Combobox selection change grab the newly selected item
            string selectedItem = ((sender as ComboBox).SelectedItem as ComboBoxItem).Content as string;

            switch (selectedItem)
            {
                case "Mammual":
                    // Bind the Group Combobox source to MammualGroup enum object
                    AnimalGroupToCreateCombo.ItemsSource = System.Enum.GetValues(typeof(MammualGroup));

                    // Sets the first item in enum list as Combobox selected item
                    AnimalGroupToCreateCombo.SelectedIndex = 0;
                    break;

                case "Bird":
                    // Bind the Group Combobox source to BirdGroup enum object
                    AnimalGroupToCreateCombo.ItemsSource = System.Enum.GetValues(typeof(BirdGroup));

                    // Sets the first item in enum list as Combobox selected item
                    AnimalGroupToCreateCombo.SelectedIndex = 0;
                    break;
            }
        }

        // Invokes on Type Combobox Selection Changed event and 
        //bind the Group Combobox source to resposnective source
        /// <param name="sender">Combobox for displaying animal list</param>
        /// <param name="e">Combobox selection changed event</param>
        private void DisplayAnimalsTypeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // On Combobox selection change grab the newly selected item
            string selectedItem = ((sender as ComboBox).SelectedItem as ComboBoxItem).Content as string;

            switch (selectedItem)
            {
                case "Mammual":
                    // Enable AnimalGroup Combobox
                    DisplayAnimalGroupCombo.IsEnabled = true;

                    // Bind the Group Combobox source to MammualGroup enum object
                    DisplayAnimalGroupCombo.ItemsSource = System.Enum.GetValues(typeof(MammualGroup));
                    break;

                case "Bird":
                    // Enable AnimalGroup Combobox
                    DisplayAnimalGroupCombo.IsEnabled = true;

                    // Bind the Group Combobox source to BirdGroup enum object
                    DisplayAnimalGroupCombo.ItemsSource = System.Enum.GetValues(typeof(BirdGroup));
                    break;

                default:
                    // if Group Combobox is still null, return
                    if (DisplayAnimalGroupCombo == null)
                    {
                        return;
                    }
                    // Set item source to null and Disable the combobox
                    DisplayAnimalGroupCombo.ItemsSource = null;
                    DisplayAnimalGroupCombo.IsEnabled = false;
                    break;
            }
        }

        /// <summary>
        /// Creates objects from all types in Creatures Entities and Invokes the GetInfo methode
        /// </summary>
        /// <param name="sender">Learn more button</param>
        /// <param name="e">Button click event</param>
        private void Learn_More_Click(object sender, RoutedEventArgs e)
        {

            // Create Assembly object and load all from Creatures project assembly
            Assembly assembly = null;
            try
            {
                assembly = Assembly.Load("Creatures");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            // Get all types from assembly object
            Type[] entities = assembly.GetTypes();

            // Create a stringbuilder object to keep all info from assembly types
            StringBuilder message = new StringBuilder();

            // Run throuw all types from Creatures assembly
            foreach (var item in entities)
            {

                try
                {
                    // Check if the types is from Creatures.Entities
                    if (item.BaseType.ToString().Contains("Creatures.Entities"))
                    {
                        // Contruct the message body
                        // Get the type Name
                        message.Append("Type of creature: ");
                        message.Append(item.Name + System.Environment.NewLine);

                        // Get the main info by invoking the GetInfo methodes on types
                        message.Append(" Main info: ");

                        // Get the methode of GetInfo of the instance
                        MethodInfo methodInfo = item.GetMethod("GetInfo");

                        
                        // Create an instance of the Type
                        object myObject = Activator.CreateInstance(item);

                        // Invoke the GetInfo methode
                        string info = methodInfo.Invoke(myObject, null) as string;

                        // Add info to the message body
                        message.Append(info + System.Environment.NewLine);

                    }

                }
                // Cathing exception of abstract classes initializations
                catch (MemberAccessException)
                {

                }                
            }

            // Display the final information message in Message Box
            MessageBox.Show(message.ToString(), "Message", MessageBoxButton.OK);
        }
    }
}
