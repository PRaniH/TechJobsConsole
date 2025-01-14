﻿using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace TechJobsConsole
{
    class JobData
    {
        static List<Dictionary<string, string>> AllJobs = new List<Dictionary<string, string>>();
        static bool IsDataLoaded = false;

        public static List<Dictionary<string, string>> FindAll()
        {
            LoadData();
            return AllJobs;
        }

        /*
         * Returns a list of all values contained in a given column,
         * without duplicates. 
         */
        public static List<string> FindAll(string column)
        {
            LoadData();

            List<string> values = new List<string>();

            foreach (Dictionary<string, string> job in AllJobs)
            {
                string aValue = job[column];

                if (!values.Contains(aValue))
                {
                    values.Add(aValue);
                }
            }
            return values;
        }

        public static List<Dictionary<string, string>> FindByColumnAndValue(string column, string value)
        {
            // load data, if not already loaded
            LoadData();

            List<Dictionary<string, string>> jobs = new List<Dictionary<string, string>>();

            foreach (Dictionary<string, string> row in AllJobs)
            {
                var valueLower = value.ToLower(); //Make the search term value case insensitive

                string aValue = row[column].ToLower();

                if (aValue.Contains(valueLower))
                {
                    jobs.Add(row);
                }
            }

            return jobs;
        }

        /*
         * Load and parse data from job_data.csv
         */
        private static void LoadData()
        {

            if (IsDataLoaded)
            {
                return;
            }

            List<string[]> rows = new List<string[]>();

            using (StreamReader reader = File.OpenText("job_data.csv"))
            {
                while (reader.Peek() >= 0)
                {
                    string line = reader.ReadLine();
                    string[] rowArrray = CSVRowToStringArray(line);
                    if (rowArrray.Length > 0)
                    {
                        rows.Add(rowArrray);
                    }
                }
            }

            string[] headers = rows[0];
            rows.Remove(headers);

            // Parse each row array into a more friendly Dictionary
            foreach (string[] row in rows)
            {
                Dictionary<string, string> rowDict = new Dictionary<string, string>();

                for (int i = 0; i < headers.Length; i++)
                {
                    rowDict.Add(headers[i], row[i]);
                }
                AllJobs.Add(rowDict);
            }

            IsDataLoaded = true;
        }

        /*
         * Parse a single line of a CSV file into a string array
         */
        private static string[] CSVRowToStringArray(string row, char fieldSeparator = ',', char stringSeparator = '\"')
        {
            bool isBetweenQuotes = false;
            StringBuilder valueBuilder = new StringBuilder();
            List<string> rowValues = new List<string>();

            // Loop through the row string one char at a time
            foreach (char c in row.ToCharArray())
            {
                if ((c == fieldSeparator && !isBetweenQuotes))
                {
                    rowValues.Add(valueBuilder.ToString());
                    valueBuilder.Clear();
                }
                else
                {
                    if (c == stringSeparator)
                    {
                        isBetweenQuotes = !isBetweenQuotes;
                    }
                    else
                    {
                        valueBuilder.Append(c);
                    }
                }
            }

            // Add the final value
            rowValues.Add(valueBuilder.ToString());
            valueBuilder.Clear();

            return rowValues.ToArray();
        }

        /* Implement FindByValue
At this stage, the application will allow users to search a given column of the data for a given string. Your next task is to enable a search to go across all of the columns.

In the JobData class, create a new(public static) method called FindByValue that will search for a string within each of the columns.Here are a few observations:


The method that you write should not contain duplicate jobs. So, for example, if a listing has position type "Web - Front End" and name "Front end web dev" then searching for "web" should not include the listing twice.
As with PrintJobs, you should write your code in a way that if a new column is added to the data, your code will automatically search the new column as well.
You should not write code that calls FindByColumnAndValue once for each column.Rather, utilize loops and collection methods as you did above.
You should, on the other hand, read and understand FindByColumnAndValue, since your code will look similar in some ways.
You'll need to call FindByValue from somewhere in Main. We'll leave it up to you to find where.You might have noticed that when you try to search all columns using the app, a message is printed, so that is a good clue to help you find where to place this new method call. */


        public static List<Dictionary<string,string>> FindByValue (string value) //Need search term value and probably all data
        {
            LoadData(); //Load all of the data from the CSV file

            List<Dictionary<string, string>> jobs = new List<Dictionary<string, string>>(); //Create a new list of dictionaries that will contain our results

            foreach (Dictionary<string, string> row in AllJobs) //For each row (Dictionary) in the the file loaded by LoadData()
            {

                foreach (var column in row) //For each column in each row (Dictionary)
                {
                    var columnLower = column.Value.ToLower(); //Make this case insensitive

                    var valueLower = value.ToLower(); //Make the search term value case insensitive
                        
                    if (columnLower.Contains(valueLower)) //If the value of the item contains our search term value

                    {
                        jobs.Add(row); //Add the Dictionary row to our new list of Dictionaries
                        break; //Don't check this column/row anymore (don't repeat this one in the results)

                        }

                }
            }

            return jobs; //After going through everything, return the new list of Dictionaries which are our search results

        }


    }
}
