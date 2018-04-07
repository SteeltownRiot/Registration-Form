/*
 * John Hopson
 * 
 * A modified version of the registration program that takes input from the user and determines
 * their earliest date and time of registration based on their year in school and the first
 * letter of their last name based on the UofL Fall/Summer 2016 Registration Schedule.
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegistrationForm
{
    public partial class RegistrationForm : Form
    {
        public RegistrationForm()
        {
            InitializeComponent();
        }

        //Precondition: lastLetter must be uppercase letter from A to Z
        //Precondition: letterArray must contain correct char ranges for the UofL Fall/Summer 2016 Registration Schedule
        //Precondition: timeArray must contain correct string values for the UofL Fall/Summer 2016 Registration Schedule
        //Postcondition: Returns time of registration for lowerclassmen based on first letter of last name
        public string SearchArraySequentially(char lastLetter, char[] letterArray, string[] timeArray)
        {
            bool found = false; //Determines if info found, initialized to false ensuring at least one search made
            string time = "ERROR";  //Holds calculated time to return to calling method
            //Search loop iterations based on number of entries in array looking for first letter of student's 
            //last name, while ensuring it hasn't already been found. Once found returns the timeStr.
            for (int index = 0; index < letterArray.Length && !found; index++)
            {
                if (lastLetter <= letterArray[index])
                {
                    found = true;
                    time = timeArray[index];
                }
            }
            return time;
        }

        //Precondition: None
        //Postcondition: Calculates date & time of earliest registration based on credit hours & first letter of last name
        private void findRegTimeBtn_Click(object sender, EventArgs e)
        {
            const float SENIOR_HOURS = 90;    // Min hours for Senior
            const float JUNIOR_HOURS = 60;    // Min hours for Junior
            const float SOPHOMORE_HOURS = 30; // Min hours for Soph.

            const string DAY1 = "30 March";  // 1st day of registration
            const string DAY2 = "31 March";  // 2nd day of registration
            const string DAY3 = "11 April";  // 3rd day of registration
            const string DAY4 = "4 April";   // 4th day of registration
            const string DAY5 = "5 April";   // 5th day of registration
            const string DAY6 = "6 April";   // 6th day of registration

            const string TIME1 = "8:30 AM";  // 1st time block
            const string TIME2 = "10:00 AM"; // 2nd time block
            const string TIME3 = "11:30 AM"; // 3rd time block
            const string TIME4 = "2:00 PM";  // 4th time block
            const string TIME5 = "4:00 PM";  // 5th time block

            string lastNameStr;       // Entered last name
            char lastNameLetterCh;    // First letter of last name, as char
            string dateStr = "Error"; // Holds date of registration
            string timeStr = "ERROR"; // Holds time of registration
            float creditHours;        // Entered credit hours

            if (float.TryParse(creditHrTxt.Text, out creditHours) && creditHours >= 0) // Valid hours
            {
                lastNameStr = lastNameTxt.Text;
                if (lastNameStr.Length > 0) // Empty string?
                {
                    lastNameStr = lastNameStr.ToUpper(); // Ensure upper case
                    lastNameLetterCh = lastNameStr[0];   // First char of last name

                    if (char.IsLetter(lastNameLetterCh)) // Is it a letter?
                    {
                        // Juniors and Seniors share same schedule but different days
                        if (creditHours >= JUNIOR_HOURS)
                        {
                            if (creditHours >= SENIOR_HOURS)
                                dateStr = DAY1;
                            else // Must be juniors
                                dateStr = DAY2;
                            //Creates parallel arrays matching upper limit of letter range for upperclassmen with corresponding time
                            char[] upperClassUpperLetterArray = { 'D', 'I', 'O', 'S', 'Z' };
                            string[] upperTimeArray = { TIME5, TIME1, TIME2, TIME3, TIME4 };

                            ;//Calls method which assigns registration times passing first letter of last name,
                            //upperclassmen letter range array, and upperclassmen time array
                            timeStr = SearchArraySequentially(lastNameLetterCh, upperClassUpperLetterArray, upperTimeArray);
                        }
                        // Sophomores and Freshmen
                        else // Must be soph/fresh
                        {
                            if (creditHours >= SOPHOMORE_HOURS)
                            {
                                // E-Q on one day
                                if ((lastNameLetterCh >= 'E') && // >= E and
                                    (lastNameLetterCh <= 'Q'))   // <= Q
                                    dateStr = DAY3;
                                else // All other letters on next day
                                    dateStr = DAY4;
                            }
                            else // must be freshman
                            {
                                // E-Q on one day
                                if ((lastNameLetterCh >= 'E') && // >= E and
                                    (lastNameLetterCh <= 'Q'))   // <= Q
                                    dateStr = DAY5;
                                else // All other letters on next day
                                    dateStr = DAY6;
                            }
                            //Creates parallel arrays matching upper limit of letter range for lowerclassmen with corresponding time
                            char[] lowerClassUpperLetterArray = { 'B', 'D', 'F', 'I', 'L', 'O', 'Q', 'S', 'V', 'Z' };
                            string[] lowerTimeArray = { TIME4, TIME5, TIME1, TIME2, TIME3, TIME4, TIME5, TIME1, TIME2, TIME3 };
                            
                            //Calls method which assigns registration times passing first letter of last name,
                            //lowerclassmen letter range array, and lowerclassmen time array
                            timeStr = SearchArraySequentially(lastNameLetterCh, lowerClassUpperLetterArray, lowerTimeArray);
                        }

                        // Output results
                        dateTimeLbl.Text = dateStr + " at " + timeStr;
                    }
                    else // First char not a letter
                        MessageBox.Show("The student's last name must begin with a letter. Please try again.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else // Empty textbox
                    MessageBox.Show("The student's last name must be entered. Please try again.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else // Can't parse credit hours
                MessageBox.Show("The completed credit hours you entered are invalid. Credit hours must be a number greater than 0. Please try again.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        //Preconditions: None
        //Postcondition: Clears output label whenever text in either text box is changed
        private void text_Change(object sender, EventArgs e)
        {
            dateTimeLbl.Text = "";
        }
    }
}