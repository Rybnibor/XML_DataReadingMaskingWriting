using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;


namespace xmlbooks
{
    /// <summary>
    /// The program, which reads data from an XML file (books.xml) and save it as a separate file (new.xml) masking credit card 
    /// information (number and CVC) according to the following scheme: Number: 412 827 ****** 9009 - Visible may be only the last 
    /// four and the first six digits. CVC: ***. The algorithm includes situations where the card number is the wrong length, empty 
    /// or will contain invalid characters.
    /// </summary>
    class Program
    {
        static bool errorYes = false;
        static void Main(string[] args)
        {
            try
            {
                XmlTextReader reader = new XmlTextReader("books.xml");
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;

                XmlWriter writer = XmlWriter.Create("new.xml", settings);
                writer.WriteStartDocument();
                writer.WriteComment("Modified file.");
                string active = "";

                while (reader.Read())
                {

                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element: // The node is an element.
                            writer.WriteStartElement(reader.Name);
                            active = reader.Name;
                            break;
                        case XmlNodeType.Text: //Display the text in each element.
                            string reading = reader.Value;
                            if (active == "cvc")
                            {
                                reading = "***";
                            }
                            else if (active == "number")
                            {
                                reading = zmien(reading);
                            }
                            writer.WriteString(reading);
                            break;
                        case XmlNodeType.EndElement: //Display the end of the element.
                            writer.WriteEndElement();
                            break;
                    }
                }

                writer.WriteEndDocument();
                writer.Flush();
                writer.Close();
                if (errorYes)
                {
                    Console.WriteLine("Conversion ended with errors.");
                }
                else
                {
                    Console.WriteLine("Conversion ended successfully. File is ready to use.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e);
            }   
            //Console.ReadLine();  
        }

        /// <summary>
        /// Changes the specified card number into masked one. Visible may be only the last 
        /// four and the first six digits.
        /// </summary>
        /// <param name="CardNumber">The card number.</param>
        /// <returns></returns>
        static string zmien(string CardNumber)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder(CardNumber);
            System.Text.StringBuilder sbReady = new System.Text.StringBuilder("");
            
            for (int j = 0; j < sb.Length; j++)
            {
                if (System.Char.IsNumber(sb[j]) == true)
                {
                    sbReady[sbReady.Length++] = sb[j];                    
                }                                      
            }

            if (sbReady.Length==16)
            {
                for (int i = 6; i < 12; i++)
                {
                    sbReady[i] = '*';
                }
            }
            else
            {
                /*
                for (int j = 0; j < sbReady.Length; j++)
                {
                        sbReady[j] = 'x';
                }
                */
                errorYes = true;
                Console.WriteLine("\nInvalid card number !!! \n");
            }
            // Store the new string. 
            CardNumber = sbReady.ToString();
            return CardNumber;
        }
    }
}
